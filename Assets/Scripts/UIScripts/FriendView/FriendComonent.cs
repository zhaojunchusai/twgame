using UnityEngine;
using System;
using System.Collections;
using Assets.Script.Common;
public class FriendComonent : MonoBehaviour
{
    public GameObject _uiRoot;

    public UISprite ItmeBg;
    public UILabel Lbl_Name;
    public UILabel Lbl_Union;
    public UILabel Lbl_FightNum;
    public UIButton Btn_GetBtn;
    public UIButton Btn_GiveBtn;
    public UILabel Lbl_Btn_Give;
    public UISprite Spt_IconBG;
    public UISprite Spt_IconFrame;
    public UISprite Spt_Icon;
    public UILabel Lbl_CharLv;

    public fogs.proto.msg.BasePlayerInfo tmpInfo;

    public delegate void _GiveDeleget(FriendComonent comp);
    public _GiveDeleget GiveEvent;

    public delegate void _GetDeleget(FriendComonent comp);
    public _GetDeleget GetEvent;

    public void MyStart(GameObject root)
    {
        this._uiRoot = root;
        if (_uiRoot == null)
            return;

        ItmeBg = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Lbl_Name = _uiRoot.transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_Union = _uiRoot.transform.FindChild("Union").gameObject.GetComponent<UILabel>();
        Lbl_FightNum = _uiRoot.transform.FindChild("FightNum").gameObject.GetComponent<UILabel>();
        Btn_GetBtn = _uiRoot.transform.FindChild("GetBtn").gameObject.GetComponent<UIButton>();
        Btn_GiveBtn = _uiRoot.transform.FindChild("GiveBtn").gameObject.GetComponent<UIButton>();
        Lbl_Btn_Give = _uiRoot.transform.FindChild("GiveBtn/Lb").gameObject.GetComponent<UILabel>();
        Spt_IconBG = _uiRoot.transform.FindChild("Head/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = _uiRoot.transform.FindChild("Head/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = _uiRoot.transform.FindChild("Head/Icon").gameObject.GetComponent<UISprite>();
        Lbl_CharLv = _uiRoot.transform.FindChild("CharLv").gameObject.GetComponent<UILabel>();

        UIEventListener.Get(this.Btn_GiveBtn.gameObject).onClick = OnGive;
        UIEventListener.Get(this.Btn_GetBtn.gameObject).onClick = OnGet;
        UIEventListener.Get(this.ItmeBg.gameObject).onClick = OnTouch;

    }
    public void OnGive(GameObject btn)
    {
        if (GiveEvent != null)
            GiveEvent(this);
    }
    public void OnGet(GameObject btn)
    {
        if (GetEvent != null)
            GetEvent(this);
    }
    public void OnTouch(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FUNCTIONMENUBVIEW);
        fogs.proto.msg.ChatInfo tmpInfo = new fogs.proto.msg.ChatInfo();
        tmpInfo.area_id = (uint)this.tmpInfo.area_id;
        tmpInfo.other_accname = this.tmpInfo.accname;
        tmpInfo.send_type = 1;
        tmpInfo.char_name = this.tmpInfo.charname;
        tmpInfo.player_level = (uint)this.tmpInfo.level;
        tmpInfo.accid = (uint)this.tmpInfo.accid;
        tmpInfo.chat_type = fogs.proto.msg.ChatType.CT_NORMAL;

        tmpInfo.vip_level = 1;
        tmpInfo.enslave = (uint)this.tmpInfo.enslave_status;

        UISystem.Instance.FunctionMenuView.UpdateViewInfo(tmpInfo, 2);
    }
    public void SetInfo(fogs.proto.msg.BasePlayerInfo Info)
    {
        this.tmpInfo = Info;
        if (this.tmpInfo == null)
            return;

        if (Spt_IconFrame != null && Spt_Icon != null)
            CommonFunction.SetHeadAndFrameSprite(Spt_Icon, Spt_IconFrame, (uint)this.tmpInfo.icon, (uint)this.tmpInfo.icon_frame, true);
        this.Lbl_Name.text = this.tmpInfo.charname;
        this.Lbl_Union.text = string.Format(ConstString.REMOVEBLOCKLEGION, string.IsNullOrEmpty(this.tmpInfo.union_name) ? ConstString.PRISON_DO_EMPTY : this.tmpInfo.union_name);
        this.Lbl_FightNum.text = this.tmpInfo.combat_power.ToString();
        this.Lbl_CharLv.text = this.tmpInfo.level.ToString();

        if(FriendModule.Instance.FindHadGiveListByAccid(this.tmpInfo.accid) != null)
        {
            this.Lbl_Btn_Give.text = ConstString.FIREND_HAD_GIVE;
            CommonFunction.SetGameObjectGray(this.Btn_GiveBtn.gameObject,true);
        }
        else
        {
            this.Lbl_Btn_Give.text = ConstString.FIREND_BTN_GIVE;
            CommonFunction.SetGameObjectGray(this.Btn_GiveBtn.gameObject, false);
        }

        if (FriendModule.Instance.FindHadGetListByAccid(this.tmpInfo.accid) != null)
        {
            this.Btn_GetBtn.gameObject.SetActive(true);
        }
        else
            this.Btn_GetBtn.gameObject.SetActive(false);
    }
}
