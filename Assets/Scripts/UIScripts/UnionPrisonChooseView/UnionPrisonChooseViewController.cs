using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class UnionPrisonChooseViewController : UIBase 
{
    public UnionPrisonChooseView view;
    public GameObject Go_StrangerEffect;
    public GameObject Go_EnemyEffect;
    public List<GameObject> List_OtherEffectPos = new List<GameObject>();
    public List<GameObject> List_StrangerEffectPos=new List<GameObject>();
    private List<fogs.proto.msg.MatchAltarUnionInfo> tmpStranger;
    private List<UStrangerComponent> StangerItemList = new List<UStrangerComponent>();

    private List<fogs.proto.msg.MatchAltarUnionInfo> tmpOther;
    private List<UOtherComponent> OtherItemList = new List<UOtherComponent>();

    private int ToggleValue = 1;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new UnionPrisonChooseView();
            view.Initialize();
        }
        InitEffectPos();
        BtnEventBinding();
        this.view.Tog_TabStranger.Set(true);
        //UnionPrisonModule.Instance.SendMatchAltarUnionReq(fogs.proto.msg.MatchAltarUnionType.NORMAL);
    }
    #region 陌生人滑动层
    public void InitStrangerItem()
    {
        tmpStranger = UnionPrisonModule.Instance.StrangeList;
        tmpStranger.Sort((left, right) =>
        {
            if (left == null || right == null)
            {
                if (left == null)
                    return 1;
                else
                    return -1;
            }
            if (left.union_lv != right.union_lv)
            {
                if (left.union_lv > right.union_lv)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        Main.Instance.StartCoroutine(CreatStrangerItem(tmpStranger));
        ShowStrangerEffect(tmpStranger);
    }
    private IEnumerator CreatStrangerItem(List<fogs.proto.msg.MatchAltarUnionInfo> _data)
    {
        this.view.ScrView_StrangerGroup.ResetPosition();
        yield return 0.5;
        int count = _data.Count;
        int itemCount = StangerItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.UIWrapContent_StrangerGroup.highCount) - 1;
        if (index == 0)
            index = 1;
        this.view.UIWrapContent_StrangerGroup.minIndex = 0;
        this.view.UIWrapContent_StrangerGroup.maxIndex = index;
        if (count % 2 != 0)
        {
            this.view.UIWrapContent_StrangerGroup.cullContent = false;
        }
        else
        {
            this.view.UIWrapContent_StrangerGroup.cullContent = true;
        }
        if (count > 3)
        {
            this.view.UIWrapContent_StrangerGroup.enabled = true;
            if (count % 2 != 0)
            {
                this.view.UIWrapContent_StrangerGroup.cullContent = false;
            }
            else
            {
                this.view.UIWrapContent_StrangerGroup.cullContent = true;
            }
        }
        else
        {
            //Grd_Grid.enabled = true;
            //this.view.UIWrapContent_StrangerGroup.enabled = true;
            this.view.UIWrapContent_StrangerGroup.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                StangerItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.Item_StrangerGroup, this.view.Grd_Grid_StrangerGroup.transform);
                UStrangerComponent item = vGo.GetComponent<UStrangerComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<UStrangerComponent>();
                    item.MyStart(vGo);
                }
                StangerItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                StangerItemList[i].StrangerEvent += OnItemTouch;
            }
            else
            {
                StangerItemList[i].gameObject.SetActive(true);
            }
            StangerItemList[i].SetInfo(_data[i]);
        }

        if (this.view.UIWrapContent_StrangerGroup.enabled)
            this.view.UIWrapContent_StrangerGroup.ReGetChild();
        yield return 0;
        this.view.Grd_Grid_StrangerGroup.Reposition();
        yield return 0;
        this.view.ScrView_StrangerGroup.ResetPosition();
        yield return 0;
        this.view.Grd_Grid_StrangerGroup.repositionNow = true;
        this.view.Grd_Grid_StrangerGroup.gameObject.SetActive(false);
        this.view.Grd_Grid_StrangerGroup.gameObject.SetActive(true);
    }
    public void OnItemTouch(UStrangerComponent comp)
    {
        if (comp.tmpInfo != null)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONPRISONINFOVIEW);
            UISystem.Instance.UnionPrisonInfoView.SetInfo(comp.tmpInfo);
        }
        return;
    }
    public void SetStrangerInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpStranger.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        UStrangerComponent item = StangerItemList[wrapIndex];
        item.SetInfo(tmpStranger[realIndex]);
    }
    #endregion
    #region 其他人滑动层
    public void InitOtherItem(List<fogs.proto.msg.MatchAltarUnionInfo> _data)
    {
        tmpOther = _data;
        tmpOther.Sort((left, right) => 
        {
            if(left == null || right  == null)
            {
                if (left == null)
                    return 1;
                else
                    return -1;
            }
            if(left.lock_status != right.lock_status)
            {
                if (left.lock_status == fogs.proto.msg.AltarLockStatus.LOCK_STATUS)
                    return -1;
                else
                    return 1;
            }
            if(left.union_lv != right.union_lv)
            {
                if (left.union_lv > right.union_lv)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        if (_data == null)
            tmpOther = new List<fogs.proto.msg.MatchAltarUnionInfo>();
        ShowEnemyEffect(tmpOther);
        Main.Instance.StartCoroutine(CreatOtherItem(tmpOther));
    }
    private IEnumerator CreatOtherItem(List<fogs.proto.msg.MatchAltarUnionInfo> _data)
    {
        this.view.ScrView_OtherGroup.ResetPosition();
        yield return 0.5;
        int count = _data.Count;
        int itemCount = OtherItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.UIWrapContent_OtherGroup.highCount) - 1;
        if (index == 0)
            index = 1;
        this.view.UIWrapContent_OtherGroup.minIndex = 0;
        this.view.UIWrapContent_OtherGroup.maxIndex = index;
        if (count % 2 != 0)
        {
            this.view.UIWrapContent_OtherGroup.cullContent = false;
        }
        else
        {
            this.view.UIWrapContent_OtherGroup.cullContent = true;
        }
        if (count > 3)
        {
            this.view.UIWrapContent_OtherGroup.enabled = true;
            if (count % 2 != 0)
            {
                this.view.UIWrapContent_OtherGroup.cullContent = false;
            }
            else
            {
                this.view.UIWrapContent_OtherGroup.cullContent = true;
            }
        }
        else
        {
            //Grd_Grid.enabled = true;
            //this.view.UIWrapContent_OtherGroup.enabled = true;
            this.view.UIWrapContent_OtherGroup.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                OtherItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                OtherItemList.Capacity = i + 1;
                GameObject vGo = CommonFunction.InstantiateObject(this.view.Item_OtherGroup, this.view.Grd_Grid_OtherGroup.transform);
                UOtherComponent item = vGo.GetComponent<UOtherComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<UOtherComponent>();
                    item.MyStart(vGo);
                }
                OtherItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                OtherItemList[i].LockEvent += OnLockTouch;
                OtherItemList[i].UnLockEvent += OnUnLockTouch;
                OtherItemList[i].TouchEvent += OnOtherTouch;
            }
            else
            {
                OtherItemList[i].gameObject.SetActive(true);
            }
            OtherItemList[i].SetInfo(_data[i]);
        }

        if (this.view.UIWrapContent_OtherGroup.enabled)
            this.view.UIWrapContent_OtherGroup.ReGetChild();
        yield return 0;
        this.view.Grd_Grid_OtherGroup.Reposition();
        yield return 0;
        this.view.ScrView_OtherGroup.ResetPosition();
        this.view.Grd_Grid_OtherGroup.repositionNow = true;
        this.view.Grd_Grid_OtherGroup.gameObject.SetActive(false);
        this.view.Grd_Grid_OtherGroup.gameObject.SetActive(true);
    }
    public void OnLockTouch(UOtherComponent comp)
    {
        fogs.proto.msg.LockUnionType type = fogs.proto.msg.LockUnionType.LOCK_ENEMY_UNION;
        if (this.ToggleValue != 2)
            type = fogs.proto.msg.LockUnionType.LOCK_REBEL_UNION;
        UnionPrisonModule.Instance.SendLockEnemyRebelUnionReq(comp.tmpInfo.union_id, type, fogs.proto.msg.AltarLockStatus.LOCK_STATUS);
        return;
    }
    public void OnUnLockTouch(UOtherComponent comp)
    {
        fogs.proto.msg.LockUnionType type = fogs.proto.msg.LockUnionType.LOCK_ENEMY_UNION;
        if (this.ToggleValue != 2)
            type = fogs.proto.msg.LockUnionType.LOCK_REBEL_UNION;
        UnionPrisonModule.Instance.SendLockEnemyRebelUnionReq(comp.tmpInfo.union_id, type, fogs.proto.msg.AltarLockStatus.UN_LOCK_STATUS);

        return;
    }
    public void OnOtherTouch(UOtherComponent comp)
    {
        if (comp.tmpInfo != null)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONPRISONINFOVIEW);
            UISystem.Instance.UnionPrisonInfoView.SetInfo(comp.tmpInfo);
        }
        return;
    }

    public void SetOtherInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpOther.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        UOtherComponent item = OtherItemList[wrapIndex];
        item.SetInfo(tmpOther[realIndex]);
    }
    #endregion

    public void ButtonEvent_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.Close(null,null);
    }

    public void ButtonEvent_Button_Level(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        UnionPrisonModule.Instance.SendMatchAltarUnionReq(fogs.proto.msg.MatchAltarUnionType.NORMAL);
    }

    public void ButtonEvent_Button_Power(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

         string name = this.view.inPut.value;
         UnionPrisonModule.Instance.SendAltarSerachUnionReq(name);
    }
    public void ButtonEvent_Tog_TabStranger(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.ToggleValue = 1;
        UnionPrisonModule.Instance.SendMatchAltarUnionReq(fogs.proto.msg.MatchAltarUnionType.NORMAL);
    }
    public void ButtonEvent_Tog_TabEnemy(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.ToggleValue = 2;
        UnionPrisonModule.Instance.SendMatchAltarUnionReq(fogs.proto.msg.MatchAltarUnionType.ENEMY);
    }
    public void ButtonEvent_Tog_TabEscaped(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.ToggleValue = 3;
        UnionPrisonModule.Instance.SendMatchAltarUnionReq(fogs.proto.msg.MatchAltarUnionType.REBEL);
    }

    public override void Uninitialize()
    {
        //PlayerData.Instance._Prison.PrisonControlEvent -= _Prison_PrisonControlEvent;
    }

    public void BtnEventBinding()
    {
        //PlayerData.Instance._Prison.PrisonControlEvent += _Prison_PrisonControlEvent;
        this.view.UIWrapContent_StrangerGroup.onInitializeItem = SetStrangerInfo;
        this.view.UIWrapContent_OtherGroup.onInitializeItem = SetOtherInfo;
        UIEventListener.Get(view.Mask.gameObject).onClick = ButtonEvent_Button_close;
        UIEventListener.Get(view.Btn_Button_Level.gameObject).onClick = ButtonEvent_Button_Level;
        UIEventListener.Get(view.Btn_Button_Power.gameObject).onClick = ButtonEvent_Button_Power;
        UIEventListener.Get(view.Tog_TabStranger.gameObject).onClick = ButtonEvent_Tog_TabStranger;
        UIEventListener.Get(view.Tog_TabEnemy.gameObject).onClick = ButtonEvent_Tog_TabEnemy;
        UIEventListener.Get(view.Tog_TabEscaped.gameObject).onClick = ButtonEvent_Tog_TabEscaped;
    }
    #region 奴役系统服务器操作回馈
    //void _Prison_PrisonControlEvent(PrisonType type, int errorCode)
    //{
    //    if(errorCode == 0)
    //    {
    //        switch(type)
    //        {
    //            case PrisonType.GetOtherPlayerInfoResp: this.OnGetOtherPlayerInfoResp(); break;
    //            case PrisonType.StartEnslaveFightResp: this.OnStartEnslaveFightResp(); break;
    //            case PrisonType.GetEnemyPlayerResp: this.OnGetEnemyPlayerResp(); break;
    //            case PrisonType.GetRunAwayPlayerResp: this.OnGetRunAwayPlayerResp(); break;
    //            case PrisonType.EnslaveLockedResp: OnLockUnLock(); break;
    //            case PrisonType.EnslaveUnLockedResp: OnLockUnLock(); break;
    //        }
    //    }
    //    else
    //    {
    //        switch(type)
    //        {
    //            case PrisonType.EnslaveFightBeforeResp: this.OnEnslaveFightBeforeResp(); break;
    //            case PrisonType.GetEnemyPlayerResp: this.OnGetEnemyPlayerResp(); break;
    //            case PrisonType.GetRunAwayPlayerResp: this.OnGetRunAwayPlayerResp(); break;
    //        }
    //    }
    //}
    //void OnLockUnLock()
    //{
    //    if (this.ToggleValue == 2)
    //        this.OnGetEnemyPlayerResp();
    //    else
    //        this.OnGetRunAwayPlayerResp();
    //}
    //void OnGetEnemyPlayerResp()
    //{
    //    this.InitOtherItem(PlayerData.Instance._Prison.EnemyList);
    //}
    //void OnGetRunAwayPlayerResp()
    //{
    //    this.InitOtherItem(PlayerData.Instance._Prison.RunAwayList);
    //}
    //void OnGetOtherPlayerInfoResp()
    //{
    //    if(PlayerData.Instance._Prison._atenaPlayer != null)
    //    {
    //    }
    //}
    //void OnStartEnslaveFightResp()
    //{
    //    UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
    //    this.Close(null,null);
    //}
    //void OnEnslaveFightBeforeResp()
    //{
    //   // PlayerData.Instance._Prison.StartGetStronger();
    //}
    #endregion
    public override void Destroy()
    {
        base.Destroy();

        this.view = null;
        if (this.OtherItemList != null)
            this.OtherItemList.Clear();
        if (this.StangerItemList != null)
            this.StangerItemList.Clear();
        if (this.tmpOther != null)
            this.tmpOther.Clear();
        if (this.tmpStranger != null)
            this.tmpStranger.Clear();
        DestroyEffectPos();
       // PlayerData.Instance._Prison.PrisonControlEvent -= _Prison_PrisonControlEvent;
    }

    //===============================================================
    
    public void InitEffectPos()
    {
        view.Go_EffectMask.SetActive(false);
        List_StrangerEffectPos.Clear();
        List_StrangerEffectPos.Add(view.Go_StrangerPos_1);
        List_StrangerEffectPos.Add(view.Go_StrangerPos_2);
        List_StrangerEffectPos.Add(view.Go_StrangerPos_3);
        List_StrangerEffectPos.Add(view.Go_StrangerPos_4);
        List_OtherEffectPos.Clear();
        List_OtherEffectPos.Add(view.Go_OtherEffectPos_1);
        List_OtherEffectPos.Add(view.Go_OtherEffectPos_2);
        List_OtherEffectPos.Add(view.Go_OtherEffectPos_3);
        List_OtherEffectPos.Add(view.Go_OtherEffectPos_4);
    }
    public void DestroyEffectPos()
    {
        List_StrangerEffectPos.Clear();
        List_OtherEffectPos.Clear();
    }
    public void ShowStrangerEffect(List<fogs.proto.msg.MatchAltarUnionInfo> _data)//陌生人
    {
        if (Go_StrangerEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_PRISONSTRANGER, (GameObject gb) => { Go_StrangerEffect = gb; });
        }
        int Num = 0;
        if(_data.Count==null){ return ; }
        if (_data.Count > 3) { Num = 4; }
        else { Num = _data.Count; }

        OpenEffectMask();
        view.ScrView_StrangerGroup.ResetPosition();
        Main.Instance.StartCoroutine(CloseEffectMask(1.0F));
        for (int i = 0; i < Num; i++)
        {
            //Debug.LogError("Go_StrangerEffect=" + Go_StrangerEffect + "          List_StrangerEffectPos=" + List_StrangerEffectPos[i].transform);
            GameObject itemEffect = ShowEffectManager.Instance.ShowEffect(Go_StrangerEffect, List_StrangerEffectPos[i].transform);
        }
    }

    public void ShowEnemyEffect(List<fogs.proto.msg.MatchAltarUnionInfo> _data)//仇人逃奴
    {
        if(Go_EnemyEffect==null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_PRISONENEMY, (GameObject gb) => { Go_EnemyEffect = gb; });
        }
        int Num = 0;
        if (_data.Count == 0) { return; }
        if (_data.Count > 3) { Num = 4; }
        else { Num = _data.Count; }

        OpenEffectMask();
        view.ScrView_OtherGroup.ResetPosition();
        Main.Instance.StartCoroutine(CloseEffectMask(1.0F));
        for (int i = 0; i < Num; i++) 
        {
            GameObject itemEffect = ShowEffectManager.Instance.ShowEffect(Go_StrangerEffect, List_OtherEffectPos[i].transform);
        }
    }
    public void OpenEffectMask()
    {
        view.Go_EffectMask.SetActive(true);
        view.Panel_Effect.depth = 30;

    }
    public IEnumerator CloseEffectMask(float time)
    {
        yield return new WaitForSeconds(time);
        view.Go_EffectMask.SetActive(false);
    }
}
