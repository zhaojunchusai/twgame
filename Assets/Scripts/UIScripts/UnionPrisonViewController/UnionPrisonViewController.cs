using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
public class UnionPrisonViewController : UIBase
{
    public UnionPrisonView view;
    private UnionAltarInfo tmpUnionAltarInfo;
    private long leftTime;
    private long rightTime;
    private int maxTime = 86400;
    private bool isLeft = true;
    public override void Initialize()
    {
        if (view == null)
            view = new UnionPrisonView();
        view.Initialize();
        BtnEventBinding();
        UnionPrisonModule.Instance.UnionPrisonControlEvent += Instance_UnionPrisonControlEvent;
        PlayerData.Instance.NotifyResetEvent += Instance_NotifyResetEvent;
        UnionPrisonModule.Instance.SendQueryAltarReq();
    }

    void Instance_NotifyResetEvent(NotifyReset data)
    {
        this.InitUI();
    }

    void Instance_UnionPrisonControlEvent(UnionPrisonType type, int errorCode)
    {
        switch(type)
        {
            case UnionPrisonType.AltarFightResp: this.InitUI(); break;
            case UnionPrisonType.LockEnemyRebelUnionResp: break;
            case UnionPrisonType.QueryAltarResp: this.InitUI(); break;
            case UnionPrisonType.MatchAltarUnionReq: break;
            case UnionPrisonType.AltarHandleDependReq: this.SetDepend(); this.SetMark(); break;
            case UnionPrisonType.QueryAltarRecordResp: break;
            case UnionPrisonType.AltarHandleDependReq_Comfor: 
                UnionPrisonView.DependView tmp = isLeft ? this.view.leftView : this.view.rightView;
                tmp.effect_anwei.gameObject.SetActive(false); 
                tmp.effect_anwei.gameObject.SetActive(true);
                break;
            case UnionPrisonType.AltarHandleDependReq_Molest: 
                UnionPrisonView.DependView tmp1 = isLeft ? this.view.leftView : this.view.rightView;
                tmp1.effect_tiaoxi.gameObject.SetActive(true);
                tmp1.effect_tiaoxi.loop = false;
                tmp1.effect_tiaoxi.Play();
                break;
        }
    }
    public void InitUI()
    {
        tmpUnionAltarInfo = UnionPrisonModule.Instance.AltarInfo;
        this.SetDepend();
        this.SetMiddle();
        this.SetMark();
    }
    #region 设置中间圣火坛

    public void SetMiddle()
    {
        if (this.tmpUnionAltarInfo == null || this.tmpUnionAltarInfo.flame_position == null)
            return;

        if(this.tmpUnionAltarInfo.flame_position.status == AltarFlameStatus.NORMAL_STATUS)
        {
            this.view.middleView.EmptyGroup.SetActive(true);
            this.view.middleView.HadGroup.SetActive(false);

            this.view.middleView.EmptyGroup_Lbl_Buff.text = string.Format("{0:0%}", (float)this.tmpUnionAltarInfo.flame_position.up_probability / 10000.0f);
            this.view.middleView.EmptyGroup_Lbl_Level.text = this.tmpUnionAltarInfo.flame_position.union_level.ToString();
            int allUp = this.tmpUnionAltarInfo.flame_position.up_probability;
            foreach (DependPosition tmp in tmpUnionAltarInfo.depend_position)
            {
                if (tmp == null)
                    continue;
                allUp += tmp.up_probability;
            }
            this.view.middleView.EmptyGroup_Lbl_Count.text = string.Format("{0:0%}", (float)allUp / 10000.0f);

        }
        else
        {
            this.view.middleView.EmptyGroup.SetActive(false);
            this.view.middleView.HadGroup.SetActive(true);
            this.view.middleView.Btn_Revolt.gameObject.SetActive(UnionPrisonModule.Instance.had_privilege_fight);
            int allUp = 0;
            foreach (DependPosition tmp in tmpUnionAltarInfo.depend_position)
            {
                if (tmp == null)
                    continue;
                allUp += tmp.up_probability;
            }
            this.view.middleView.HadGroup_Lbl_Count.text = string.Format("{0:0%}", (float)allUp / 10000.0f);

            CommonFunction.SetHeadAndFrameSprite(
                this.view.middleView.Spt_Sprite_Icon,
                this.view.middleView.Spt_Sprite_Quality,
                (uint)this.tmpUnionAltarInfo.flame_position.host_icon,
                (uint)this.tmpUnionAltarInfo.flame_position.host_frame, true
                );
            this.view.middleView.Lbl_UnionName.text = this.tmpUnionAltarInfo.flame_position.host_union_name;
            this.view.middleView.Lbl_UnionLeader.text = this.tmpUnionAltarInfo.flame_position.host_chairman;
        }
    }
    public void SetMark()
    {
        if (UnionPrisonModule.Instance.altar_records == null)
            return;
        PrisonMarkComponent tmp = null;
        if (UnionPrisonModule.Instance.altar_records.Count > 0 && this.view.markView.Item1 != null)
        {
            tmp = null;
            tmp = this.view.markView.Item1.GetComponent<PrisonMarkComponent>();
            if(tmp == null)
                tmp = this.view.markView.Item1.AddComponent<PrisonMarkComponent>();
            if(tmp != null)
            {
                tmp.MyStart(this.view.markView.Item1);
                tmp.SetInfo(UnionPrisonModule.Instance.altar_records[0]);
            }
        }
        if (UnionPrisonModule.Instance.altar_records.Count > 1 && this.view.markView.Item2 != null)
        {
            tmp = null;
            tmp = this.view.markView.Item2.GetComponent<PrisonMarkComponent>();
            if (tmp == null)
                tmp = this.view.markView.Item2.AddComponent<PrisonMarkComponent>();
            if (tmp != null)
            {
                tmp.MyStart(this.view.markView.Item2);
                tmp.SetInfo(UnionPrisonModule.Instance.altar_records[1]);

            }
        }
        if (UnionPrisonModule.Instance.altar_records.Count > 2 && this.view.markView.Item3 != null)
        {
            tmp = null;
            tmp = this.view.markView.Item3.GetComponent<PrisonMarkComponent>();
            if (tmp == null)
                tmp = this.view.markView.Item3.AddComponent<PrisonMarkComponent>();
            if (tmp != null)
            {
                tmp.MyStart(this.view.markView.Item3);
                tmp.SetInfo(UnionPrisonModule.Instance.altar_records[2]);
            }
        }
        this.view.markView.UITable_Table.Reposition();
    }
    #endregion
    #region 设置依附军团
    public void SetDepend()
    {
        if (tmpUnionAltarInfo == null || tmpUnionAltarInfo.depend_position == null)
            return;
        foreach(DependPosition tmp in tmpUnionAltarInfo.depend_position)
        {
            if (tmp == null)
                continue;
            if(tmp.position_id == 1)
            {
                leftTime = 0;
                this.view.leftView.Lbl_Time.text = "";
                if (tmp.status == DependStatus.HAD_DEPEND_UNION)
                {
                    leftTime = tmp.be_free_time;
                    long time = leftTime - Main.mTime;
                    if (time > maxTime)
                        leftTime = Main.mTime + maxTime;
                    Scheduler.Instance.AddTimer(0.5f,true,this.SetLeftTime);
                }
                this.SetDependView(this.view.leftView,tmp);
            }
            if(tmp.position_id == 2)
            {
                rightTime = 0;
                this.view.rightView.Lbl_Time.text = "";
                if (tmp.status == DependStatus.HAD_DEPEND_UNION)
                {
                    rightTime = tmp.be_free_time;
                    long time = rightTime - Main.mTime;
                    if (time > maxTime)
                        rightTime = Main.mTime + maxTime;
                    Scheduler.Instance.AddTimer(0.5f, true, this.SetRightTime);
                }
                this.SetDependView(this.view.rightView, tmp);
            }
        }
    }
    private void SetDependView(UnionPrisonView.DependView vView, DependPosition vInfo)
    {
        if (vView == null || vInfo == null)
            return;

        if (vInfo.status == DependStatus.NO_DEPEND_UNION)
        {
            this.SetEmptyDependView(vView,vInfo);
        }
        else
        {
            this.SetHadDependView(vView, vInfo);
        }
    }
    private void SetEmptyDependView(UnionPrisonView.DependView vView, DependPosition vInfo)
    {
        vView.EmptyObject.SetActive(true);
        vView.HadObject.SetActive(false);

        vView.Lbl_UnionName.text = "--------";
        vView.Lbl_UnionLeader.text = "--------";
        vView.IconObject.SetActive(false);
        vView.EmptyGroup_Lbl_Count.text = string.Format("{0}/{1}", vInfo.did_times, UnionPrisonModule.Instance.max_handle_times);
        vView.Btn_Battle.gameObject.SetActive(UnionPrisonModule.Instance.had_privilege_fight);

        vView.Btn_Comfort.gameObject.SetActive(false);
        vView.Btn_Molest.gameObject.SetActive(false);
    }
    private void SetHadDependView(UnionPrisonView.DependView vView, DependPosition vInfo)
    {
        vView.EmptyObject.SetActive(false);
        if (!vView.HadObject.activeSelf)
            vView.HadObject.SetActive(true);

        vView.Lbl_UnionName.text = vInfo.union_name;
        vView.Lbl_UnionLeader.text = vInfo.chairman;
        vView.IconObject.SetActive(true);
        vView.Btn_Battle.gameObject.SetActive(false);
        vView.Btn_Comfort.gameObject.SetActive(true);
        vView.Btn_Molest.gameObject.SetActive(true);
        vView.HadGroup_Lbl_Level.text = vInfo.union_level.ToString();
        vView.HadGroup_Lbl_Count.text = string.Format("{0}/{1}", vInfo.did_times, UnionPrisonModule.Instance.max_handle_times);
        ///需要等读入配置表后替换文本
        vView.HadGroup_Lbl_Buff.text = string.Format("{0:0%}", (float)vInfo.up_probability / 10000.0f);
        vView.Btn_Close.gameObject.SetActive(UnionPrisonModule.Instance.had_privilege_fight);
        CommonFunction.SetHeadAndFrameSprite(vView.Spt_Sprite_Icon, vView.Spt_Sprite_Quality, (uint)vInfo.icon, (uint)vInfo.frame, true);
    }
    public void SetLeftTime()
    {
        if(this.leftTime == 0)
        {
            Scheduler.Instance.RemoveTimer(this.SetLeftTime);
            return;
        }
        long tmp = leftTime - Main.mTime;
        if(tmp < 0)
        {
            Scheduler.Instance.RemoveTimer(this.SetLeftTime);
            return;
        }
        long hour = tmp / 3600;
        long min = (tmp % 3600) / 60;
        long sec = tmp % 60;
        this.view.leftView.Lbl_Time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, min, sec);
    }
    public void SetRightTime()
    {
        if (this.rightTime == 0)
        {
            Scheduler.Instance.RemoveTimer(this.SetRightTime);
            return;
        }
        long tmp = rightTime - Main.mTime;
        if (tmp < 0)
        {
            Scheduler.Instance.RemoveTimer(this.SetLeftTime);
            return;
        }
        long hour = tmp / 3600;
        long min = (tmp % 3600) / 60;
        long sec = tmp % 60;
        this.view.rightView.Lbl_Time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, min, sec);
    }
    #endregion
    public void ButtonEvent_RuleBtn(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PRISONRULEVIEW);
        UISystem.Instance.PrisonRuleView.SetInfo(ConfigManager.Instance.mUnionConfig.GetUnionBaseData().altar_rule, ConstString.TITLE_RULE);
    }
    public void ButtonEvent_rightView_Battle(GameObject btn)
    {
        UnionPrisonModule.Instance.pos = 2;
        UnionPrisonModule.Instance.SendMatchAltarUnionReq(MatchAltarUnionType.NORMAL);
    }
    public void ButtonEvent_rightView_Molest(GameObject btn)
    {
        UnionPrisonModule.Instance.SendAltarHandleDependReq(HandleDependType.MOLEST, 2);
        this.isLeft = false;
    }
    public void ButtonEvent_rightView_Comfort(GameObject btn)
    {
        UnionPrisonModule.Instance.SendAltarHandleDependReq(HandleDependType.COMFORT, 2);
        this.isLeft = false;

    }
    public void ButtonEvent_rightView_Close(GameObject btn)
    {
        UISystem.Instance.HintView.ShowMessageBox(
            MessageBoxType.mb_YesNo,
            ConstString.UINONTPRISON_ISDELETE,
            () => 
            {
                UnionPrisonModule.Instance.SendDeleteDependUnionReq(2);
            },
            () => { }
            );
    }
    public void ButtonEvent_leftView_Battle(GameObject btn)
    {
        UnionPrisonModule.Instance.pos = 1;
        UnionPrisonModule.Instance.SendMatchAltarUnionReq(MatchAltarUnionType.NORMAL);
    }
    public void ButtonEvent_leftView_Molest(GameObject btn)
    {
        UnionPrisonModule.Instance.SendAltarHandleDependReq(HandleDependType.MOLEST, 1);
        this.isLeft = true;
    }
    public void ButtonEvent_leftView_Comfort(GameObject btn)
    {
        UnionPrisonModule.Instance.SendAltarHandleDependReq(HandleDependType.COMFORT, 1);
        this.isLeft = true;
    }
    public void ButtonEvent_leftView_Close(GameObject btn)
    {
        UISystem.Instance.HintView.ShowMessageBox(
            MessageBoxType.mb_YesNo,
            ConstString.UINONTPRISON_ISDELETE,
            () =>
            {
                UnionPrisonModule.Instance.SendDeleteDependUnionReq(1);
            },
            () => { }
            );
    }
    public void ButtonEvent_Revolt(GameObject btn)
    {
        if (UnionPrisonModule.Instance.AltarInfo.flame_position == null)
            return;
        FlamePosition tmpPos = UnionPrisonModule.Instance.AltarInfo.flame_position;
        UnionPrisonModule.Instance.pos = 0;
        MatchAltarUnionInfo tmp = new MatchAltarUnionInfo();
        tmp.union_id = tmpPos.host_unionid;
        tmp.union_lv = tmpPos.host_union_lv;
        tmp.union_name = tmpPos.host_union_name;
        tmp.status = tmpPos.host_union_status;
        tmp.member_num = tmpPos.host_union_member_num;
        tmp.max_meber_num = tmpPos.host_max_meber_num;
        tmp.icon = tmpPos.host_union_icon.ToString();
        tmp.chairman = tmpPos.host_chairman;
        tmp.depend_host = tmpPos.host_host_uname;
        tmp.up_probability = tmpPos.host_up_probability;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONPRISONINFOVIEW);
        UISystem.Instance.UnionPrisonInfoView.SetInfo(tmp);
    }
    public void ButtonEvent_Rescue(GameObject btn)
    {

    }
    public void ButtonEvent_Mark(GameObject btn)
    {
        UnionPrisonModule.Instance.SendQueryAltarRecordReq();
    }
    public void ButtonEvent_Close(GameObject btn)
    {
        this.Close(null,null);
        UnionModule.Instance.OpenUnion();
    }
    public override void Uninitialize()
    {
        UnionPrisonModule.Instance.UnionPrisonControlEvent -= Instance_UnionPrisonControlEvent;
        PlayerData.Instance.NotifyResetEvent -= Instance_NotifyResetEvent;

        Scheduler.Instance.RemoveTimer(this.SetLeftTime);
        Scheduler.Instance.RemoveTimer(this.SetRightTime);

    }
    public override void Destroy()
    {
        base.Destroy();
        UnionPrisonModule.Instance.UnionPrisonControlEvent -= Instance_UnionPrisonControlEvent;
        PlayerData.Instance.NotifyResetEvent -= Instance_NotifyResetEvent;
        Scheduler.Instance.RemoveTimer(this.SetLeftTime);
        Scheduler.Instance.RemoveTimer(this.SetRightTime);

    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_RuleBtn.gameObject).onClick = ButtonEvent_RuleBtn;
        UIEventListener.Get(view.rightView.Btn_Battle.gameObject).onClick = ButtonEvent_rightView_Battle;
        UIEventListener.Get(view.rightView.Btn_Molest.gameObject).onClick = ButtonEvent_rightView_Molest;
        UIEventListener.Get(view.rightView.Btn_Comfort.gameObject).onClick = ButtonEvent_rightView_Comfort;
        UIEventListener.Get(view.rightView.Btn_Close.gameObject).onClick = ButtonEvent_rightView_Close;

        UIEventListener.Get(view.leftView.Btn_Battle.gameObject).onClick = ButtonEvent_leftView_Battle;
        UIEventListener.Get(view.leftView.Btn_Molest.gameObject).onClick = ButtonEvent_leftView_Molest;
        UIEventListener.Get(view.leftView.Btn_Comfort.gameObject).onClick = ButtonEvent_leftView_Comfort;
        UIEventListener.Get(view.leftView.Btn_Close.gameObject).onClick = ButtonEvent_leftView_Close;

        UIEventListener.Get(view.middleView.Btn_Revolt.gameObject).onClick = ButtonEvent_Revolt;
        UIEventListener.Get(view.middleView.Btn_Rescue.gameObject).onClick = ButtonEvent_Rescue;

        UIEventListener.Get(view.markView.Btn_Back).onClick = ButtonEvent_Mark;
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
    }
}
