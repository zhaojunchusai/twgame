using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class FirendApplyViewController : UIBase
{
    public FirendApplyView view;
    private int _page;
    private const int COUNT_PER_PAGE = 30;
    private List<AppendFriends> _pendingfriendMembers;
    private List<FirendApplyItem> _friendApplyItems;
    public override void Initialize()
    {
        if (view == null)
            view = new FirendApplyView();
        view.Initialize();
        BtnEventBinding();
        view.ScrView_Content.onDragFinishedDown = ReqNewPage;
        FriendModule.Instance.SendGetAppendFriendsListReq();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (_pendingfriendMembers != null) _pendingfriendMembers.Clear();
        if (_friendApplyItems != null) _friendApplyItems.Clear();
    }
    public void InitUI()
    {
        int count = FriendModule.Instance.MyAppendFriends == null?0:FriendModule.Instance.MyAppendFriends.Count;
        string message = string.Format("{0}/{1}", count,FriendModule.Instance.MAXAPPLYCOUNT);
        view.Lbl_ApplyCount.text = string.Format(ConstString.FORMAT_UNION_APPLY_MEMBER_COUNT,
                                                 FriendModule.Instance.MyAppendFriends.Count);
        _page = 1;
        ShowApplyMembers();
    }

    public void DelOne(int index)
    {
        int maxIndex = Mathf.Min(_page * COUNT_PER_PAGE , _pendingfriendMembers.Count + 1);
        for (int i = index; i < maxIndex - 1; i++)
        {
            _friendApplyItems[i].InitItem(_friendApplyItems[i + 1].MemberInfo);
        }
        _friendApplyItems[maxIndex - 1].gameObject.SetActive(false);
        view.ScrView_Content.ResetPosition();
    }

    private void ShowApplyMembers()
    {
        if (_friendApplyItems == null)
            _friendApplyItems = new List<FirendApplyItem>();

        _pendingfriendMembers = FriendModule.Instance.MyAppendFriends;
        int minIndex = _page * COUNT_PER_PAGE - COUNT_PER_PAGE;
        int maxIndex = Mathf.Min(_page * COUNT_PER_PAGE , _pendingfriendMembers.Count);
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

    private FirendApplyItem InstantiateUnionApplyItem()
    {
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_UnionApplyItem, view.Grd_Content.transform);
        return go.AddComponent<FirendApplyItem>();
    }

    public void ButtonEvent_OneKeyRefuse(GameObject btn)
    {
        if (FriendModule.Instance.MyAppendFriends.Count <= 0)
            return;
        FriendModule.Instance.SendAuditAppendFriendsReq(AuditFriendsOpType.AFO_REFUSE,FriendsOpType.FO_ALL,null);
    }

    public void ButtonEvent_OneKeyAgree(GameObject btn)
    {
        if(FriendModule.Instance.MyAppendFriends.Count <= 0)
            return;
        FriendModule.Instance.SendAuditAppendFriendsReq(AuditFriendsOpType.AFO_ACCPET, FriendsOpType.FO_ALL, null);
    }

    private void ClickMask(GameObject go)
    {
        UISystem.Instance.CloseGameUI(FirendApplyView.UIName);
    }

    public override void Uninitialize()
    {
        view.ScrView_Content.onDragFinishedDown = null;
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_OneKeyRefuse.gameObject).onClick = ButtonEvent_OneKeyRefuse;
        UIEventListener.Get(view.Btn_OneKeyAgree.gameObject).onClick = ButtonEvent_OneKeyAgree;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ClickMask;
    }


}
