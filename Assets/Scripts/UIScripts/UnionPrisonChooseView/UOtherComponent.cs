using UnityEngine;
using System;
using System.Collections;
public class UOtherComponent : MonoBehaviour
{
    public UISprite Spt_QualitySprite;
    public UISprite Spt_IconTexture;
    public UISprite QualitySprite_back;

    public UILabel Lbl_Level;
    public UISprite Spt_Back;
    public UISprite Spt_Back_Power;
    public GameObject State;
    public UILabel Lbl_Label_Name;
    public UILabel Lbl_Label_Master;
    public UILabel Lbl_label_Master_Title;
    public UILabel Lbl_Label_Power;
    public UILabel Lbl_Label_Prompt;
    public UILabel Lbl_Label_State;
    public UILabel Lbl_label_State_Title;
    public UILabel Lbl_label_Buff;

    public UIButton Btn_Button_UnLock;
    public UIButton Btn_Button_Lock;

    public GameObject _uiRoot;
    public GameObject Back;

    public delegate void LockDeleget(UOtherComponent comp);
    public LockDeleget LockEvent;

    public delegate void UnLockDeleget(UOtherComponent comp);
    public UnLockDeleget UnLockEvent;

    public delegate void TouchDeleget(UOtherComponent comp);
    public TouchDeleget TouchEvent;

    public fogs.proto.msg.MatchAltarUnionInfo tmpInfo;
    public void MyStart(GameObject root)
    {
        this._uiRoot = root;

        Spt_QualitySprite = _uiRoot.transform.FindChild("ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = _uiRoot.transform.FindChild("ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        QualitySprite_back = _uiRoot.transform.FindChild("ArtifactComp/back").gameObject.GetComponent<UISprite>();
        State = _uiRoot.transform.FindChild("ArtifactComp/state").gameObject;

        Lbl_Level = _uiRoot.transform.FindChild("ArtifactComp/LevelBG/Label").gameObject.GetComponent<UILabel>();
        
        Lbl_Label_Name = _uiRoot.transform.FindChild("LabelGroup/Label_Name").gameObject.GetComponent<UILabel>();
        Lbl_label_Master_Title = _uiRoot.transform.FindChild("LabelGroup/Label_Master").gameObject.GetComponent<UILabel>();
        Lbl_Label_Master = _uiRoot.transform.FindChild("LabelGroup/Label_Master/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Power = _uiRoot.transform.FindChild("LabelGroup/Label_Power/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Prompt = _uiRoot.transform.FindChild("LabelGroup/Label_Prompt").gameObject.GetComponent<UILabel>();
        Lbl_Label_State = _uiRoot.transform.FindChild("LabelGroup/Label_State/Label").gameObject.GetComponent<UILabel>();
        Lbl_label_State_Title = _uiRoot.transform.FindChild("LabelGroup/Label_State").gameObject.GetComponent<UILabel>();
        Lbl_label_Buff = _uiRoot.transform.FindChild("LabelGroup/Label_Buff/Label").gameObject.GetComponent<UILabel>();
        Btn_Button_UnLock = _uiRoot.transform.FindChild("ButtonGroup/Button_UnLock").gameObject.GetComponent<UIButton>();
        Btn_Button_Lock = _uiRoot.transform.FindChild("ButtonGroup/Button_Lock").gameObject.GetComponent<UIButton>();
        Back = _uiRoot.transform.FindChild("BackGroup/Back").gameObject;
        UIEventListener.Get(this.Btn_Button_Lock.gameObject).onClick = OnLock;
        UIEventListener.Get(this.Btn_Button_UnLock.gameObject).onClick = OnUnLock;
        UIEventListener.Get(_uiRoot).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }
    public void SetInfo(fogs.proto.msg.MatchAltarUnionInfo info)
    {
        this.tmpInfo = info;
        if (this.tmpInfo == null)
            return;

        if (Spt_QualitySprite != null && Spt_IconTexture != null)
            CommonFunction.SetSpriteName(Spt_IconTexture, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(info.icon));

        if (this.Lbl_Label_Name != null)
            this.Lbl_Label_Name.text = this.tmpInfo.union_name;

        if (this.Lbl_Label_Master != null)
            this.Lbl_Label_Master.text = string.Format("{0}/{1}",this.tmpInfo.member_num,this.tmpInfo.max_meber_num);


        if (this.Lbl_Label_Power != null)
            this.Lbl_Label_Power.text = this.tmpInfo.chairman;

        if (this.Lbl_Level != null)
            this.Lbl_Level.text = this.tmpInfo.union_lv.ToString();

        if (this.tmpInfo.status == fogs.proto.msg.AltarFlameStatus.DEPEND_STATUS)
            this.State.SetActive(true);
        else
            this.State.SetActive(false);
        ///5月27日中午姜总要求百分数不显示小数点后面的位数
        this.Lbl_label_Buff.text = string.Format(ConstString.UNIONPRISONINFO, (float)this.tmpInfo.up_probability / 10000);


        if (this.tmpInfo.status == fogs.proto.msg.AltarFlameStatus.NORMAL_STATUS)
        {
            this.Lbl_Label_State.color = new Color(0.35f, 0.89f, 0.42f);
            this.Lbl_label_State_Title.text = ConstString.UNION_PRISON_STATETITLE;
            this.Lbl_Label_State.text = ConstString.UNION_PRISON_FREESTATE;
            this.Lbl_Label_State.spacingX = 2;
        }
        else
        {
            this.Lbl_Label_State.color = new Color(1, 0.46f, 0.04f);
            this.Lbl_label_State_Title.text = ConstString.UNION_PRISION_STATETITLEFREE;
            this.Lbl_Label_State.spacingX = 0;
            this.Lbl_Label_State.text = this.tmpInfo.depend_host;
        }

        if (this.Btn_Button_Lock != null && this.Btn_Button_UnLock != null)
        {
            this.Btn_Button_Lock.gameObject.SetActive(this.tmpInfo.lock_status == fogs.proto.msg.AltarLockStatus.UN_LOCK_STATUS);
            this.Btn_Button_UnLock.gameObject.SetActive(this.tmpInfo.lock_status == fogs.proto.msg.AltarLockStatus.LOCK_STATUS);
        }
    }
    public void OnLock(GameObject btn)
    {
        if (LockEvent != null)
            LockEvent(this);
    }
    public void OnUnLock(GameObject btn)
    {
        if (UnLockEvent != null)
            UnLockEvent(this);
    }
}