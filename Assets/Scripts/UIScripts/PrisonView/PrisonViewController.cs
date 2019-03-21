using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PrisonViewController : UIBase
{
    public PrisonView view;
    public int MaxCount = 4;
    private fogs.proto.msg.EnslaveInfo tmpInfo;
    private GameObject player;
    private TdSpine.MainSpine mainSpine;
    private int totalMoney = 0;
    public int TotalMoney
    {
        set
        {
            this.totalMoney += value;
            this.view.Lbl_Label_get_had.text = CommonFunction.GetTenThousandUnit(this.totalMoney);
        }
        get
        {
            return this.totalMoney;
        }
    }
    private List<fogs.proto.msg.PrisonInfo> PrisonList;
    private List<PrisonComonent> _prisonItemList = new List<PrisonComonent>();

    public override void Initialize()
    {

        if (view == null)
            view = new PrisonView();
        view.Initialize();
        BtnEventBinding();
        InitEffectPos();
        PlayerData.Instance._Prison.SendGetEnslaveInfoReq();
       // this.SetInfo(PlayerData.Instance._Prison.GetPrisonInfo());
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSlaveView);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));

    }
    #region 奴役系统服务器操作回馈
    void _Prison_PrisonControlEvent(PrisonType type, int errorCode)
    {
        switch (type)
        {
            //case PrisonType.GetOtherPlayerInfoResp: if (errorCode == 0) this.OnGetOtherPlayerInfoResp(); break;
            case PrisonType.MatchStrangerResp: if (errorCode == 0) this.OnGetStronger(); break;
            case PrisonType.GetEnslaveRecordResp: if (errorCode == 0) this.OnGetEnslaveRecordResp(); break;
            case PrisonType.GetEnslaveInfoResp: if (errorCode == 0) this.OnGetEnslaveInfoResp(); break;
            case PrisonType.CollectMoneyReq: if (errorCode == 0) this.OnGetEnslaveInfoResp(); break;
            case PrisonType.CollectAllMoneyReq: if (errorCode == 0) this.OnGetEnslaveInfoResp(); break;
            case PrisonType.ReleaseEnslaveReq: if (errorCode == 0) this.OnGetEnslaveInfoResp(); break;
            case PrisonType.IsNotify: this.OnNotify(errorCode); break;
        }
        return;
    }
    void OnNotify(int errorcode)
    {
        if (errorcode == 0)
            this.view.MarkNotify.SetActive(true);
        else
            this.view.MarkNotify.SetActive(false);
    }
    void OnGetOtherPlayerInfoResp()
    {
        if(PlayerData.Instance._Prison._atenaPlayer != null)
        {

        }
    }
    void OnGetEnslaveRecordResp()
    {
        if(PlayerData.Instance._Prison.GetEnslaveRecord() != null)
        {
            this.view.MarkNotify.SetActive(false);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PRISONMARKVIEW);
            UISystem.Instance.PrisonMarkView.InitPrisonMarkItem();
        }
    }
    void OnGetStronger()
    {
        if (!UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_CHOOSEPRISONVIEW))
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CHOOSEPRISONVIEW);
        else
            UISystem.Instance.ChoosePrisonView.InitStrangerItem();
    }
    void OnGetEnslaveInfoResp()
    {
        this.SetInfo(PlayerData.Instance._Prison.GetPrisonInfo());
    }
    #endregion
    /// <summary>
    /// 界面数据初始化
    /// </summary>
    /// <param name="Info">囚牢信息</param>
    public void SetInfo(fogs.proto.msg.EnslaveInfo Info)
    {
        if (Info == null)
            return;
        this.tmpInfo = Info;
        this.SetLabel(Info);
        this.SetType(Info);
        this._SkeleAnimation();
        this.InitPrisonItem();
    }
    
    #region 私有函数

    private void SetLabel(fogs.proto.msg.EnslaveInfo Info)
    {
        if (Info == null)
            return;

        if (Info.prison_info == null)
            this.view.Lbl_Label_slave.text = string.Format("0/{0}",this.MaxCount);
        else
        {
            this.view.Lbl_Label_slave.text = string.Format("{0}/{1}", PlayerData.Instance._Prison.GetPrisonCount(), PlayerData.Instance._Prison.GetPrisonInfoCount());
        }

        Color col = new Color();
        if (Info.status != 1)
            col = Color.red;
        else
            col = Color.white;

        this.view.Lbl_Label_get.text = string.Format("{0}%",Info.status == 1? 100:60);
        this.view.Lbl_Label_get.color = col;

        int Totle = 0;
        this.PrisonList = PlayerData.Instance._Prison.GetPrisonList();
        for (int i = 0; i < this.PrisonList.Count; ++i )
        {
            fogs.proto.msg.PrisonInfo myInfo = this.PrisonList[i];
            if (myInfo == null)
                continue;
            SlaveIncomeInfo resultInfo = ConfigManager.Instance.mSlaveIncomeConfig.FindAllByLv(myInfo.level);
            if (resultInfo != null)
                Totle += resultInfo.money * 6;
        }
        if (Info.status == 2)
            Totle = (int)(Totle * 0.6);

        this.view.Lbl_Label_get_h.text = CommonFunction.GetTenThousandUnit(Totle);
        this.view.Lbl_Label_get_h.color = col;

        this.view.Lbl_Label_get_had.text = CommonFunction.GetTenThousandUnit((int)Info.total_money);
        this.totalMoney = Info.total_money;

        this.view.Lbl_Label_Level.text = string.Format("Lv.{0}",PlayerData.Instance._Level);
        this.view.Lbl_Label_Name.text = PlayerData.Instance._NickName;
    }

    private void SetType(fogs.proto.msg.EnslaveInfo Info)
    {
        if (Info == null)
            return;

        if(Info.status == 1)
        {
            this.SetFreeType();
        }
        else
        {
            this.SetSlaveryType();
        }

        this.view.MarkNotify.SetActive(PlayerData.Instance._Prison.GetNotify());
    }

    private void SetFreeType()
    {
        this.view.UIPanel_Free_Type.gameObject.SetActive(true);
        this.view.UIPanel_Slavery_Type.gameObject.SetActive(false);
    }
    private void SetSlaveryType()
    {
        this.view.UIPanel_Free_Type.gameObject.SetActive(false);
        this.view.UIPanel_Slavery_Type.gameObject.SetActive(true);
        if (this.tmpInfo != null && this.tmpInfo.slave_holder != null)
            this.view.Lbl_Label_Slavery.text = string.Format(ConstString.PRISON_BE_SLAVER, this.tmpInfo.slave_holder.name);
    }

    private void _SkeleAnimation()
    {
        if (this.view.UIPanel_Player == null) return;
        if (this.player != null)
        {
            this.mainSpine = null;
            GameObject.Destroy(this.player);
        }
        ResourceLoadManager.Instance.LoadCharacter(CommonFunction.GetHeroResourceNameByGender((EHeroGender)PlayerData.Instance._Gender), ResourceLoadType.AssetBundle, (obj) =>
        {
            if (obj != null)
            {
                GameObject go = CommonFunction.InstantiateObject(obj, this.view.UIPanel_Player.transform);
                this.player = go;
                TdSpine.MainSpine mainSpine = go.GetComponent<TdSpine.MainSpine>();
                if (mainSpine == null)
                    mainSpine = go.AddComponent<TdSpine.MainSpine>();
                this.mainSpine = mainSpine;
                this.mainSpine.InitSkeletonAnimation();
                go.transform.localScale *= RoleManager.Instance.Get_UIHero_Scale;
                this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);

                Main.Instance.StartCoroutine(SpineEquipInit());

            }
        });
        UIEventListener.Get(this.view.UIPanel_Player.gameObject).onClick = (go) =>
        {
            if (this.mainSpine == null) return;

            List<string> tempList = new List<string>();

            tempList.Add(GlobalConst.ANIMATION_NAME_MOVE);
            tempList.Add(GlobalConst.ANIMATION_NAME_VICTORY);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_BASIC);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE6);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE8);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE10);

            int index = UnityEngine.Random.Range(0, tempList.Count - 1);

            this.mainSpine.pushAnimation(tempList[index], true, 1);
            this.mainSpine.EndEvent += (string animationName) =>
            {
                this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
            };
        };
    }
    private IEnumerator SpineEquipInit()
    {
        yield return 0;
        if (this.mainSpine != null)
            this.mainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot);
        this.mainSpine.ResetAlph(1.0f);
        this.mainSpine.setSortingOrder(this.view.UIPanel_Player.sortingOrder + 1);
    }
    #endregion

    #region 囚牢滑动层
    public void InitPrisonItem()
    {
        PrisonList = PlayerData.Instance._Prison.GetPrisonList();
        Main.Instance.StartCoroutine(CreatPrisonItem(PrisonList));
    }
    private IEnumerator CreatPrisonItem(List<fogs.proto.msg.PrisonInfo> _data)
    {
        this.view.ScrView_PrisonScroll.ResetPosition();
        yield return 0.5;
        int count = _data.Count;
        int itemCount = _prisonItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.UIWrapContent_Grid.highCount) - 1;
        if (index == 0)
            index = 1;
        this.view.UIWrapContent_Grid.minIndex = 0;
        this.view.UIWrapContent_Grid.maxIndex = index;
        if (count % 2 != 0)
        {
            this.view.UIWrapContent_Grid.cullContent = false;
        }
        else
        {
            this.view.UIWrapContent_Grid.cullContent = true;
        }
        if (count > 6)
        {
            this.view.UIWrapContent_Grid.enabled = true;
            if (count % 2 != 0)
            {
                this.view.UIWrapContent_Grid.cullContent = false;
            }
            else
            {
                this.view.UIWrapContent_Grid.cullContent = true;
            }
            count = 14;
        }
        else
        {
            //Grd_Grid.enabled = true;
            //this.view.UIWrapContent_Grid.enabled = true;
            this.view.UIWrapContent_Grid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _prisonItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {

            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.item, this.view.Grd_Grid.transform);
                PrisonComonent item = vGo.GetComponent<PrisonComonent>();
                if (item == null)
                {
                    item = vGo.AddComponent<PrisonComonent>();
                    item.MyStart(vGo);
                }
                _prisonItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _prisonItemList[i].PrisonEvent += OnPrison;
                _prisonItemList[i].CloseEvent += OnClose;
            }
            else
            {
                _prisonItemList[i].gameObject.SetActive(true);
            }
            _prisonItemList[i].SetInfo(_data[i],i + 1);
        }

        if (this.view.UIWrapContent_Grid.enabled)
            this.view.UIWrapContent_Grid.ReGetChild();
        yield return 0;
        this.view.Grd_Grid.Reposition();
        yield return 0;
        this.view.ScrView_PrisonScroll.ResetPosition();
        this.view.Grd_Grid.repositionNow = true;
        this.view.Grd_Grid.gameObject.SetActive(false);
        this.view.Grd_Grid.gameObject.SetActive(true);
    }
    public void OnPrison(PrisonComonent comp)
    {
        if(comp.tmpInfo.status == 1)
        {
            PlayerData.Instance._Prison.StartGetStronger(fogs.proto.msg.EnslaveMatchType.EMT_LEVEL,(uint)comp.tmpIndex);
        }
        else
        {
            if (comp.tmpInfo.locked == 1)
                PlayerData.Instance._Prison.StartCollectMoneyReq(comp.tmpInfo.prison_id);
        }
        return;
    }
    public void OnClose(PrisonComonent comp)
    {
        if(comp.tmpInfo != null && comp.tmpInfo.locked == 1)
        {
            PrisonComonent tmpComp = comp;
            UISystem.Instance.HintView.ShowMessageBox
                (
                MessageBoxType.mb_YesNo,
                string.Format(ConstString.PRISON_IS_RELEASE,tmpComp.tmpInfo.name),
                () => { return; },
                () => 
                {
                    PlayerData.Instance._Prison.StartReleaseEnslaveReq(tmpComp.tmpInfo.prison_id);
                    return;
                },
                ConstString.PRISON_Close,
                ConstString.PRISON_RELEASE
                );
        }
        return;
    }
    public void SetPrisonInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= PrisonList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        PrisonComonent item = _prisonItemList[wrapIndex];
        item.SetInfo(PrisonList[realIndex], realIndex);
    }
    #endregion
    public void ButtonEvent_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.Close(null,null);
    }
    public void ButtonEvent_Button_OneKeyGet(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        PlayerData.Instance._Prison.StartCollectAllMoneyReq();
    }
    public void ButtonEvent_Button_PrisonRule(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PRISONRULEVIEW);
        UISystem.Instance.PrisonRuleView.SetInfo(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_PRISON_RULE), ConstString.PRISON_RULE);
    }
    public void ButtonEvent_Button_PrisonMark(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        PlayerData.Instance._Prison.StartGetEnslaveRecordReq();
    }
    public void ButtonEvent_Spt_Slavery_Back(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        fogs.proto.msg.EnslaveInfo tmpInfo = PlayerData.Instance._Prison.GetPrisonInfo();
        if (tmpInfo != null)
        {
            PlayerData.Instance._Prison.StartGetOtherPlayerInfo(PlayerData.Instance._AccountName, PlayerData.Instance._AccountID);
        }
    }
    public override void Destroy()
    {
        base.Destroy();

        PlayerData.Instance._Prison.PrisonControlEvent -= _Prison_PrisonControlEvent;
        if(this._prisonItemList != null)
        {
            for (int i = 0; i < this._prisonItemList.Count;++i )
            {
                PrisonComonent tmp = this._prisonItemList[i];
                if (tmp == null)
                    continue;
                tmp.EndTime();
            }
            this._prisonItemList.Clear();
        }
    }
    public override void Uninitialize()
    {
        base.Uninitialize();
        PlayerData.Instance._Prison.PrisonControlEvent -= _Prison_PrisonControlEvent;
        if (this._prisonItemList != null)
        {
            for (int i = 0; i < this._prisonItemList.Count; ++i)
            {
                PrisonComonent tmp = this._prisonItemList[i];
                if (tmp == null)
                    continue;
                tmp.EndTime();
            }
        }
    }
    public void BtnEventBinding()
    {
        PlayerData.Instance._Prison.PrisonControlEvent += _Prison_PrisonControlEvent;

        this.view.UIWrapContent_Grid.onInitializeItem = SetPrisonInfo;

        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
        UIEventListener.Get(view.Btn_Button_OneKeyGet.gameObject).onClick = ButtonEvent_Button_OneKeyGet;
        UIEventListener.Get(view.Btn_Button_PrisonRule.gameObject).onClick = ButtonEvent_Button_PrisonRule;
        UIEventListener.Get(view.Btn_Button_PrisonMark.gameObject).onClick = ButtonEvent_Button_PrisonMark;
        UIEventListener.Get(view.Spt_Slavery_Back.gameObject).onClick = ButtonEvent_Spt_Slavery_Back;
    }

    //==================================================================================
    public GameObject Go_FlyEffect,Go_BoomEffect;
    public List<GameObject> List_EffectPos=new List<GameObject>();
    public void InitEffectPos()
    {
        List_EffectPos.Clear();
        List_EffectPos.Add(view.Go_EffectPos_1);
        List_EffectPos.Add(view.Go_EffectPos_2);
        List_EffectPos.Add(view.Go_EffectPos_3);
        List_EffectPos.Add(view.Go_EffectPos_4);
    }
    public void PlayOnesFlyEffect(uint id)
    {
        if (Go_FlyEffect ==null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_PRISONFLYEFFECT, (GameObject gb) => { Go_FlyEffect = gb; });
        } 
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_FlyEffect, List_EffectPos[(int)id-1].transform);
        //iTween.MoveTo(go, view.Lbl_Label_get_had.gameObject.transform.position,0.5F);
        iTween.MoveTo(go, iTween.Hash("time", 0.5F, "position", view.Spt_Player_Back.transform.position, "islocal", false, "easetype", iTween.EaseType.linear));
        Main.Instance.StartCoroutine(PlayBoomEffect(0.5F));
    }
    public void PlayALLFlyEffect()
    {
        if (Go_FlyEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_PRISONFLYEFFECT, (GameObject gb) => { Go_FlyEffect = gb; });
        }
        for (int i = 0; i < PrisonList.Count;i++ )
        {
            if (PrisonList[i].accname!=null&&PrisonList[i].current_money!=0)
            {
                GameObject go = ShowEffectManager.Instance.ShowEffect(Go_FlyEffect, List_EffectPos[(int)(PrisonList[i].prison_id-1)].transform);
                iTween.MoveTo(go, iTween.Hash("time", 0.5F, "position", view.Spt_Player_Back.transform.position, "islocal", false, "easetype", iTween.EaseType.linear));
            }

        }

        Main.Instance.StartCoroutine(PlayBoomEffect(0.5F));

    }

    public IEnumerator PlayBoomEffect(float time)
    {
        if (Go_BoomEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_PRISONBOOMEFFECT, (GameObject gb) => { Go_BoomEffect = gb; });
        }
        view.EffectMask.SetActive(true);
        view.EffectPanel.depth = 30;
        yield return new WaitForSeconds(time);
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_BoomEffect, view.Spt_Player_Back.transform);
        yield return new WaitForSeconds(0.5F);
        view.EffectMask.SetActive(false);
    }

}
