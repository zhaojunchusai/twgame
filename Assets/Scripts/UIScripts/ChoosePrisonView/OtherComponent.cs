using UnityEngine;
using System;
using System.Collections;
public class OtherComponent : MonoBehaviour
{
    public UISprite Spt_QualitySprite;
    public UISprite Spt_IconTexture;
    public UISprite QualitySprite_back;

    public UILabel Lbl_Level;
    public UISprite Spt_Back;
    public UISprite Spt_Back_Power;
    public UILabel Lbl_Label_Name;
    public UILabel Lbl_Label_Master;
    public UILabel Lbl_label_Master_Title;
    public UILabel Lbl_Label_Power;
    public UILabel Lbl_Label_Prompt;
    public UILabel Lbl_Label_State;
    public UILabel Lbl_label_State_Title;
    public UIButton Btn_Button_UnLock;
    public UIButton Btn_Button_Lock;

    public GameObject _uiRoot;
    public GameObject Back;

    public delegate void LockDeleget(OtherComponent comp);
    public LockDeleget LockEvent;

    public delegate void UnLockDeleget(OtherComponent comp);
    public UnLockDeleget UnLockEvent;

    public delegate void TouchDeleget(OtherComponent comp);
    public TouchDeleget TouchEvent;

    public fogs.proto.msg.OtherPlayer tmpInfo;
    public void MyStart(GameObject root)
    {
        this._uiRoot = root;

        Spt_QualitySprite = _uiRoot.transform.FindChild("ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = _uiRoot.transform.FindChild("ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        QualitySprite_back = _uiRoot.transform.FindChild("ArtifactComp/back").gameObject.GetComponent<UISprite>();

        Lbl_Level = _uiRoot.transform.FindChild("ArtifactComp/LevelBG/Label").gameObject.GetComponent<UILabel>();
        
        Lbl_Label_Name = _uiRoot.transform.FindChild("LabelGroup/Label_Name").gameObject.GetComponent<UILabel>();
        Lbl_label_Master_Title = _uiRoot.transform.FindChild("LabelGroup/Label_Master").gameObject.GetComponent<UILabel>();
        Lbl_Label_Master = _uiRoot.transform.FindChild("LabelGroup/Label_Master/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Power = _uiRoot.transform.FindChild("LabelGroup/Label_Power/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Prompt = _uiRoot.transform.FindChild("LabelGroup/Label_Prompt").gameObject.GetComponent<UILabel>();
        Lbl_Label_State = _uiRoot.transform.FindChild("LabelGroup/Label_State/Label").gameObject.GetComponent<UILabel>();
        Lbl_label_State_Title = _uiRoot.transform.FindChild("LabelGroup/Label_State").gameObject.GetComponent<UILabel>();
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
    public void SetInfo(fogs.proto.msg.OtherPlayer info)
    {
        this.tmpInfo = info;
        if (this.tmpInfo == null)
            return;

        if (Spt_QualitySprite != null && Spt_IconTexture != null)
            CommonFunction.SetHeadAndFrameSprite(Spt_IconTexture, Spt_QualitySprite, this.tmpInfo.icon, this.tmpInfo.icon_frame, true);

        if (this.Lbl_Label_Name != null)
            this.Lbl_Label_Name.text = this.tmpInfo.name;

        if (this.Lbl_Label_Master.text != null)
            this.Lbl_Label_Master.text = this.tmpInfo.slave_holder.name;

        if (this.tmpInfo.slave_holder.name.Equals(""))
        {
            this.Lbl_label_Master_Title.gameObject.SetActive(false);
            Lbl_label_State_Title.gameObject.SetActive(true);
        }
        else
        {
            this.Lbl_label_Master_Title.gameObject.SetActive(true);
            Lbl_label_State_Title.gameObject.SetActive(false);
        }

        if (this.Lbl_Label_Power != null)
            this.Lbl_Label_Power.text = this.tmpInfo.combat_power.ToString();

        if (this.Lbl_Level != null)
            this.Lbl_Level.text = this.tmpInfo.level.ToString();

        if(this.Lbl_Label_Prompt != null)
        {
            if (this.tmpInfo.slave_holder.accname.Equals(""))
            {
                this.Lbl_Label_Prompt.color = new Color(0.99f, 0.82f, 0);
                this.Lbl_Label_Prompt.text = ConstString.PRISON_TOUCH_SLAVE;
            }
            else
            {
                this.Lbl_Label_Prompt.color = new Color(0.96f, 0.22f, 0.21f);
                this.Lbl_Label_Prompt.text = ConstString.PRISON_TOUCH_SLAVER;
            }
        }

        if(this.Lbl_Label_State != null)
        {
            if (this.tmpInfo.slave_holder.accname.Equals(""))
            {
                this.Lbl_Label_State.color = new Color(0.76f, 1, 0.16f);
                this.Lbl_Label_State.text = ConstString.PRISON_STATE_FREE;
            }
            else
            {
                this.Lbl_Label_State.color = new Color(1, 0.23f, 0.23f);
                this.Lbl_Label_State.text = ConstString.PRISON_STATE_SLAVER;
            }
        }

        if(this.Btn_Button_Lock != null && this.Btn_Button_UnLock != null)
        {
            this.Btn_Button_Lock.gameObject.SetActive(this.tmpInfo.locked == 1);
            this.Btn_Button_UnLock.gameObject.SetActive(this.tmpInfo.locked == 0);
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