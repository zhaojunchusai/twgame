using UnityEngine;
using System;
using System.Collections;
public class StrangerComponent : MonoBehaviour
{
    public UISprite Spt_QualitySprite;
    public UISprite Spt_IconTexture;
    public UISprite Spt_QualityBack;
    public UILabel Lbl_Level;
    public UILabel Lbl_Label_Name;
    public UILabel Lbl_Label_Master;
    public UILabel Lbl_Label_Master_Title;
    public UILabel Lbl_Label_Power;
    public UILabel Lbl_Label_Prompt;
    public UILabel Lbl_Label_State;
    public UILabel Lbl_label_State_Title;

    public GameObject _uiRoot;
    public GameObject Back;
    public fogs.proto.msg.OtherPlayer tmpInfo;

    public delegate void _StrangerDeleget(StrangerComponent comp);
    public _StrangerDeleget StrangerEvent;

    public void MyStart(GameObject root)
    {
        this._uiRoot = root;

        Spt_QualitySprite = _uiRoot.transform.FindChild("ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = _uiRoot.transform.FindChild("ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_QualityBack = _uiRoot.transform.FindChild("ArtifactComp/back").gameObject.GetComponent<UISprite>();

        Lbl_Level = _uiRoot.transform.FindChild("ArtifactComp/LevelBG/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Name = _uiRoot.transform.FindChild("LabelGroup/Label_Name").gameObject.GetComponent<UILabel>();
        Lbl_Label_Master_Title = _uiRoot.transform.FindChild("LabelGroup/Label_Master").gameObject.GetComponent<UILabel>();
        Lbl_Label_Master = _uiRoot.transform.FindChild("LabelGroup/Label_Master/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Power = _uiRoot.transform.FindChild("LabelGroup/Label_Power/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Prompt = _uiRoot.transform.FindChild("LabelGroup/Label_Prompt").gameObject.GetComponent<UILabel>();
        Back = _uiRoot.transform.FindChild("BackGroup/Back").gameObject;
        Lbl_Label_State = _uiRoot.transform.FindChild("LabelGroup/Label_State/Label").gameObject.GetComponent<UILabel>();
        Lbl_label_State_Title = _uiRoot.transform.FindChild("LabelGroup/Label_State").gameObject.GetComponent<UILabel>();

        UIEventListener.Get(_uiRoot).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (StrangerEvent != null)
            StrangerEvent(this);
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

        if(this.Lbl_Label_Master != null && !this.tmpInfo.slave_holder.accname.Equals(""))
        {
            this.Lbl_Label_Master_Title.gameObject.SetActive(true);
            this.Lbl_Label_Master.text = this.tmpInfo.slave_holder.name;
            Lbl_label_State_Title.gameObject.SetActive(false);
        }
        else
        {
            this.Lbl_Label_Master_Title.gameObject.SetActive(false);
            this.Lbl_Label_Master.text = "";
            Lbl_label_State_Title.gameObject.SetActive(true);

        }

        if(this.Lbl_Label_Power != null)
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
        if (this.Lbl_Label_State != null)
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

    }
}