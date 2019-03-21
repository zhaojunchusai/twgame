using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
public class FBFriendComonent : MonoBehaviour
{
    public GameObject _uiRoot;

    public UILabel Lbl_Name;
    public UILabel Lbl_FightNum;
    public UIButton Btn_GiveBtn;
    public UILabel Lbl_Btn_Give;
    public UISprite Spt_IconBG;
    public UISprite Spt_IconFrame;
    public UITexture Spt_Icon;

    public FBFriend tmpInfo;
    int day = 0;
    public delegate void _GiveDeleget(FBFriendComonent comp);
    public _GiveDeleget GiveEvent;

    public delegate void _GetDeleget(FBFriendComonent comp);
    public _GetDeleget GetEvent;

    public void MyStart(GameObject root)
    {
        this._uiRoot = root;
        if (_uiRoot == null)
            return;

        Lbl_Name = _uiRoot.transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_FightNum = _uiRoot.transform.FindChild("Liveness").gameObject.GetComponent<UILabel>();
        Btn_GiveBtn = _uiRoot.transform.FindChild("GiveBtn").gameObject.GetComponent<UIButton>();
        Lbl_Btn_Give = _uiRoot.transform.FindChild("GiveBtn/Lb").gameObject.GetComponent<UILabel>();
        Spt_IconBG = _uiRoot.transform.FindChild("Head/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = _uiRoot.transform.FindChild("Head/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = _uiRoot.transform.FindChild("Head/Icon").gameObject.GetComponent<UITexture>();

        UIEventListener.Get(this.Btn_GiveBtn.gameObject).onClick = OnGive;

    }
    public void OnGive(GameObject btn)
    {
        if (this.tmpInfo == null)
            return;
        if (tmpInfo.LastLoadTime <= 0)
        {
            FriendModule.Instance.SendInviteThirdFriendsReq(fogs.proto.msg.ThirdFriendType.FACEBOOK, fogs.proto.msg.ThirdFriendsOpResultType.FAKE_SUCCESS, this.tmpInfo.Name);
        }
        else
        {
            string message = ConstString.FRIEND_WAKE_AWARK;
            FriendMissionInfo info = ConfigManager.Instance.mFriendMissionConfig.FindByType(4);
            string cut = "";
            if(info != null)
            {
                List<CommonItemData> tmpData = CommonFunction.GetCommonItemDataList(info.RewardBag);
                if(tmpData != null)
                {
                    for (int i = 0; i < tmpData.Count;++i )
                    {
                        if (cut == "")
                            cut += tmpData[i].Name;
                        else
                            cut += ("," + tmpData[i].Name);
                    }
                }
            }
            message = string.Format(message, this.tmpInfo.Name, cut, FriendModule.Instance.AwakeSurplurs, FriendModule.Instance.MAXFRIENDAWAKENUM);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, message, () =>
            {
                FriendModule.Instance.SendWakeThirdFriendsReq(fogs.proto.msg.ThirdFriendType.FACEBOOK, fogs.proto.msg.ThirdFriendsOpResultType.FAKE_SUCCESS, this.tmpInfo.Name);
            });
        }
    }
    public void SetInfo(FBFriend Info)
    {
        this.tmpInfo = Info;
        if (this.tmpInfo == null)
            return;
        this.tmpInfo.getTexture(OnDownLoad);
        this.Lbl_Name.text = this.tmpInfo.Name;
        if(this.tmpInfo.LastLoadTime <= 0)
        {
            this.Lbl_FightNum.text = ConstString.FIREND_NOT_INGAME;
            this.Lbl_FightNum.color = Color.red;
        }
        else
        {
            long timeCompare = Main.mTime - this.tmpInfo.LastLoadTime;
            day = (int)(timeCompare / 86400);
            this.Lbl_FightNum.color = Color.white;
            if (day > 0)
                this.Lbl_FightNum.text = CommonFunction.ReplaceEscapeChar(string.Format(ConstString.FIREND_LAST_LOADTIEM, day));
            else
            {
                int hour = (int)(timeCompare / 3600);
                if(hour > 0)
                    this.Lbl_FightNum.text = CommonFunction.ReplaceEscapeChar(string.Format(ConstString.FIREND_LAST_LOADTIEMHOURE, hour));
                else
                    this.Lbl_FightNum.text = CommonFunction.ReplaceEscapeChar(string.Format(ConstString.FIREND_LAST_LOADTIEMONEHOUR));
            }
        }
        if (this.tmpInfo.LastLoadTime <= 0)
            this.Lbl_Btn_Give.text = ConstString.FIREND_Invit;
        else
            this.Lbl_Btn_Give.text = ConstString.FIREND_Wake;
        if (tmpInfo.uiTexture != null)
            this.Spt_Icon.mainTexture = tmpInfo.uiTexture;
        else
            this.Spt_Icon.mainTexture = null;
    }
    void OnDownLoad(Texture2D texture)
    {
        if (this != null && this.Spt_Icon != null)
            this.Spt_Icon.mainTexture = texture;
    }
}
