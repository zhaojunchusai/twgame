using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
public class UnionPrisonInfoViewController : UIBase 
{
    public UnionPrisonInfoView view;
    public MatchAltarUnionInfo tmpInfo;
    public delegate void ButtonEvent(int btnPos);
    public ButtonEvent TouchListen;
    public override void Initialize()
    {
        if (view == null)
            view = new UnionPrisonInfoView();
        view.Initialize();
        BtnEventBinding();
    }
    /// <summary>
    /// 军团信息界面
    /// </summary>
    /// <param name="icon">军团的图标</param>
    /// <param name="level">军团等级</param>
    /// <param name="LabelPos">需要显示的Label 1.名称2.状态4.成员8.团长16.招募的buff</param>
    /// <param name="vLabelValue">设置显示属性的Label，对于状态Label可以直接传入fogs.proto.msg.AltarFlameStatus结构体</param>
    /// <param name="ButtonPos">需要显示的Button 1.第一个Button 现在也只做了一个button</param>
    /// <param name="vButtonValue">设置下面按钮的文字和点击回馈</param>
    public void SetInfo(string icon, int level, fogs.proto.msg.AltarFlameStatus vState, int LabelPos = 0, List<string> vLabelValue = null, int ButtonPos = 0,ButtonEvent vEvent = null, List<string> vButtonValue = null)
    {
        this.Clear();
        string tmpIcon = ConfigManager.Instance.mUnionConfig.GetUnionIconByID(icon);
        if(string.IsNullOrEmpty(tmpIcon))
        {
            CommonFunction.SetSpriteName(this.view.Spt_IconTexture, GlobalConst.SpriteName.QUESTION);
        }
        else
            CommonFunction.SetSpriteName(this.view.Spt_IconTexture, tmpIcon);

        if(level == 0)
        {
            this.view.Obj_Level.SetActive(false);
        }
        else
        {
            this.view.Obj_Level.SetActive(true);
            this.view.Lbl_Label_Level.text = level.ToString();
        }
        if (vState == fogs.proto.msg.AltarFlameStatus.DEPEND_STATUS)
            this.view.Spt_state.gameObject.SetActive(true);
        else
            this.view.Spt_state.gameObject.SetActive(false);

        if((LabelPos & 1) != 0)
        {
            this.view.Obj_Name.SetActive(true);
            if(vLabelValue != null && vLabelValue.Count >= 1)
            {
                this.view.Lbl_Label_Name.text = vLabelValue[0];
                vLabelValue.RemoveAt(0);
            }
        }
        if ((LabelPos & 2) != 0)
        {
            this.view.Obj_State.SetActive(true);
            if (vLabelValue != null && vLabelValue.Count >= 1)
            {
                if (vState == fogs.proto.msg.AltarFlameStatus.DEPEND_STATUS)
                {
                    this.view.Lbl_Label_State.color = new Color(1, 0.46f, 0.04f);
                    this.view.Lbl_Label_State_Title.text = ConstString.UNION_PRISION_STATETITLEFREE;
                    this.view.Lbl_Label_State.text = string.Empty;
                }
                else
                {
                    this.view.Lbl_Label_State.color = new Color(0.35f, 0.89f, 0.42f);
                    this.view.Lbl_Label_State_Title.text = ConstString.UNION_PRISON_STATETITLE;
                    this.view.Lbl_Label_State.text = ConstString.UNION_PRISON_FREESTATE;
                    this.view.Spt_state.gameObject.SetActive(false);
                }
                if (!string.IsNullOrEmpty(vLabelValue[0]))
                {
                    this.view.Lbl_Label_State.text = vLabelValue[0];
                }
                vLabelValue.RemoveAt(0);
            }
        }
        if ((LabelPos & 4) != 0)
        {
            this.view.Obj_Menber.SetActive(true);
            if (vLabelValue != null && vLabelValue.Count >= 1)
            {
                this.view.Lbl_Label_Member.text = vLabelValue[0];
                vLabelValue.RemoveAt(0);
            }

        }
        if ((LabelPos & 8) != 0)
        {
            this.view.Obj_Leader.SetActive(true);
            if (vLabelValue != null && vLabelValue.Count >= 1)
            {
                this.view.Lbl_Label_Leader.text = vLabelValue[0];
                vLabelValue.RemoveAt(0);
            }

        }
        if ((LabelPos & 16) != 0)
        {
            this.view.Obj_Buff.SetActive(true);
            if (vLabelValue != null && vLabelValue.Count >= 1)
            {
                this.view.Lbl_Label_Buff.text = vLabelValue[0];
                vLabelValue.RemoveAt(0);
            }
        }
        if((ButtonPos & 1) != 0)
        {
            this.view.Btn_RuleBtn.gameObject.SetActive(true);
            if (vButtonValue != null && vButtonValue.Count >= 1)
            {
                this.view.Btn_RuleLbl.text = vButtonValue[0];
                vButtonValue.RemoveAt(0);
            }
            else
                this.view.Btn_RuleLbl.text = ConstString.UNIONMEMBERINFO;
        }
        this.view.Lbl_Grid.Reposition();
        this.TouchListen = vEvent;
    }
    public void SetInfo(MatchAltarUnionInfo info)
    {
        this.tmpInfo = info;
        if (this.tmpInfo == null)
            return;
        List<string> LabelValue = new List<string>();
        LabelValue.Add(this.tmpInfo.union_name);
        if (this.tmpInfo.status == fogs.proto.msg.AltarFlameStatus.NORMAL_STATUS)
        {
            LabelValue.Add(ConstString.UNION_PRISON_FREESTATE);
        }
        else
        {
            LabelValue.Add(this.tmpInfo.depend_host);
        }
        LabelValue.Add(string.Format("{0}/{1}", this.tmpInfo.member_num, this.tmpInfo.max_meber_num));
        LabelValue.Add(this.tmpInfo.chairman);
        LabelValue.Add(string.Format(ConstString.UNIONPRISONINFO, (float)this.tmpInfo.up_probability / 10000));
        List<string> ButtonValue = new List<string>();
        ButtonValue.Add(ConstString.UNIONPRISONFIGHT);
        this.SetInfo(info.icon, info.union_lv, info.status, 31, LabelValue, 1, OnTouch, ButtonValue);
    }
    public void OnTouch(int pos)
    {
        if (this.tmpInfo != null && pos == 1)
        {
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_UNIONPRISONCHOOSEVIEW);
            this.Close(null, null);
            UnionPrisonModule.Instance.SendAltarFightReq(this.tmpInfo.union_id);
        }
    }
    public void ButtonEvent_RuleBtn(GameObject btn)
    {
        if (this.TouchListen != null)
            this.TouchListen(1);
    }
    public void ButtonEvent_Close(GameObject btn)
    {
        this.Close(null,null);
    }
    public void Clear()
    {
        this.view.Obj_Name.SetActive(false);
        this.view.Obj_Menber.SetActive(false);
        this.view.Obj_Leader.SetActive(false);
        this.view.Obj_State.SetActive(false);
        this.view.Obj_Buff.SetActive(false);
        this.view.Btn_RuleBtn.gameObject.SetActive(false);
        this.view.Spt_state.gameObject.SetActive(false);
    }
    public override void Uninitialize()
    {
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_RuleBtn.gameObject).onClick = ButtonEvent_RuleBtn;
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_Close;
    }


}
