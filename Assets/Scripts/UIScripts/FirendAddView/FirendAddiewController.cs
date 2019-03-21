using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;
public class FirendAddiewController : UIBase
{
    public FirendAddView view;
    private int _page;
    private const int COUNT_PER_PAGE = 25;
    private List<BasePlayerInfo> _pendingfriendMembers;
    private List<BasePlayerInfo> _matchMembers;
    private List<BasePlayerInfo> _findMembers;
    private List<FirendAddItem> _friendApplyItems;
    private EventDelegate inputEvent;
    private string InputStr = "";
    private int MaxCDTime = 60;
    private int CDTime = 0;
    private int state = 0;
    public override void Initialize()
    {
        if (view == null)
            view = new FirendAddView();
        view.Initialize();
        BtnEventBinding();
        view.ScrView_Content.onDragFinishedDown = ReqNewPage;
        inputEvent = new EventDelegate(OnInput);
        this.view.input.onSubmit.Add(inputEvent);
        Scheduler.Instance.RemoveTimer(this.TimeDown);
        if (this.view != null && this.view.Lbl_Btn_RefreshLabel != null)
            this.view.Lbl_Btn_RefreshLabel.text = ConstString.TimeCd;
        this.StartTimeDown();
        if ((this._matchMembers == null || this._matchMembers.Count <= 0) && this.CDTime <= 0)
            FriendModule.Instance.SendRefreshRecommendFriendsReq();
    }
    public override void Destroy()
    {
        base.Destroy();
        this.view.input.onSubmit.Remove(inputEvent);
        view = null;
        if (_pendingfriendMembers != null) _pendingfriendMembers.Clear();
        if (_friendApplyItems != null) _friendApplyItems.Clear();

    }
    public void InitMatch(List<BasePlayerInfo> data)
    {
        if (this.view == null)
            return;

        this.state = 0;
        this._matchMembers = data;
        this.StartTimeDown();
        this.view.Lbl_Btn_RefreshLabel.text = ConstString.TimeCd;
        InitUI(this._matchMembers);
    }
    public void InitFind(List<BasePlayerInfo> data)
    {
        this._findMembers = data;
        this.state = 1;
        this.view.Lbl_Btn_RefreshLabel.text = ConstString.FRIEND_EWLSDA;
        CommonFunction.SetGameObjectGray(this.view.Btn_Refresh.gameObject, false);
        InitUI(this._findMembers);
    }

    public void InitUI(List<BasePlayerInfo> data)
    {
        _page = 1;
        //if (_friendApplyItems != null)
        //    _friendApplyItems.Clear();
        ShowApplyMembers(data);
    }

    public void DelOne(int index)
    {
        int maxIndex = Mathf.Min(_page * COUNT_PER_PAGE, _pendingfriendMembers.Count + 1);
        for (int i = index; i < maxIndex - 1; i++)
        {
            _friendApplyItems[i].InitItem(_friendApplyItems[i + 1].MemberInfo);
        }
        _friendApplyItems[maxIndex - 1].gameObject.SetActive(false);
        view.ScrView_Content.ResetPosition();
    }
    private void ShowApplyMembers(List<BasePlayerInfo> data)
    {
        this._pendingfriendMembers = data;
        this.ShowApplyMembers();
    }
    private void ShowApplyMembers()
    {
        if (_friendApplyItems == null)
            _friendApplyItems = new List<FirendAddItem>();

        int minIndex = _page * COUNT_PER_PAGE - COUNT_PER_PAGE;
        int maxIndex = Mathf.Min(_page * COUNT_PER_PAGE, _pendingfriendMembers.Count);
        //Debug.Log(string.Format("page ={0},min ={1},max ={2},{3},{4}", _page, minIndex, maxIndex, 
        //    _pendingfriendMembers.Count, _friendApplyItems.Count));
        for (int i = minIndex; i < maxIndex; i++)
        {
            if (i >= _friendApplyItems.Count)
            {
                _friendApplyItems.Add(InstantiateUnionApplyItem());
            }
            _friendApplyItems[i].gameObject.SetActive(true);
            _friendApplyItems[i].InitItem(_pendingfriendMembers[i]);
        }

        for (int i = maxIndex; i < _friendApplyItems.Count; i++)
        {
            _friendApplyItems[i].gameObject.SetActive(false);
        }
        view.ScrView_Content.ResetPosition();
        view.Grd_Content.Reposition();
    }

    private void ReqNewPage()
    {
        if (_pendingfriendMembers.Count <= _page * COUNT_PER_PAGE)
        {
            return;
        }
        ++_page;
        ShowApplyMembers();
    }
    private void OnInput()
    {
        InputStr = this.view.input.value;
    }
    private FirendAddItem InstantiateUnionApplyItem()
    {
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_UnionApplyItem, view.Grd_Content.transform);
        return go.AddComponent<FirendAddItem>();
    }

    public void ButtonEvent_Btn_Refresh(GameObject btn)
    {
        if (this.state == 1)
        {
            this.state = 0;
            this.view.Lbl_Btn_RefreshLabel.text = ConstString.TimeCd;
            if (this.CDTime > 0)
            {
                CommonFunction.SetGameObjectGray(this.view.Btn_Refresh.gameObject, true);
            }
            this.InitMatch(this._matchMembers);
            return;
        }
        if (this.state == 0)
        {
            if (this.CDTime <= 0)
                FriendModule.Instance.SendRefreshRecommendFriendsReq();
        }
    }
    public void StartTimeDown()
    {
        if (this.view == null)
            return;

        if (PlayerData.Instance.refresh_tick <= 0)
        {
            this.CDTime = 0;
            CommonFunction.SetGameObjectGray(this.view.Btn_Refresh.gameObject, false);
            return;
        }

        Scheduler.Instance.RemoveTimer(this.TimeDown);
        this.CDTime = MaxCDTime - (int)(Main.mTime - PlayerData.Instance.refresh_tick);
        long freshcd = 60;
        if (!long.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_FRIENDS_REFRESHCD), out freshcd))
        {
            freshcd = 60;
        }
        if (CDTime > freshcd)
        {
            this.CDTime = (int)freshcd;
        }
        this.CDTime = MaxCDTime - (int)(Main.mTime - PlayerData.Instance.refresh_tick);
        if (this.CDTime < 0)
        {
            this.view.Lbl_Btn_RefreshLabel.text = ConstString.TimeCd;
            CommonFunction.SetGameObjectGray(this.view.Btn_Refresh.gameObject, false);
            return;
        }
        CommonFunction.SetGameObjectGray(this.view.Btn_Refresh.gameObject, true);
        Scheduler.Instance.AddTimer(1.0f, true, this.TimeDown);
    }
    void TimeDown()
    {
        if (this.CDTime <= 0)
        {
            Scheduler.Instance.RemoveTimer(this.TimeDown);
            if (this.view != null && this.view.Lbl_Btn_RefreshLabel != null)
            {
                CommonFunction.SetGameObjectGray(this.view.Btn_Refresh.gameObject, false);
                if (this.state == 0)
                    this.view.Lbl_Btn_RefreshLabel.text = ConstString.TimeCd;
                return;
            }
        }
        if (this.view != null && this.view.Lbl_Btn_RefreshLabel != null && state == 0)
            this.view.Lbl_Btn_RefreshLabel.text = this.CDTime.ToString();
        --this.CDTime;
    }
    public void ButtonEvent_Btn_Find(GameObject btn)
    {
        string message = this.view.input.value;
        if (message.Equals(PlayerData.Instance._NickName))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FRIEND_CANNOTADD_SELF);
            return;
        }
        FriendModule.Instance.SendSearchFriendsReq(message);
    }

    private void ClickMask(GameObject go)
    {
        UISystem.Instance.CloseGameUI(FirendAddView.UIName);
    }

    public override void Uninitialize()
    {
        this.view.input.onSubmit.Remove(inputEvent);
        view.ScrView_Content.onDragFinishedDown = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Refresh.gameObject).onClick = ButtonEvent_Btn_Refresh;
        UIEventListener.Get(view.Btn_Find.gameObject).onClick = ButtonEvent_Btn_Find;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ClickMask;
    }
    public void PlayEffect(Transform tr)
    {
        GameObject Go_Effect = null;

        if (Go_Effect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_JOINUNION, (GameObject gb) => { Go_Effect = gb; });
        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_Effect, tr.transform);

    }

}
