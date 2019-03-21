using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class FriendInviteItem : MonoBehaviour
{
    public GameObject _uiRoot;

    public UILabel Lbl_Descript;
    public UILabel Lbl_Name;
    public UILabel MissionNum;
    public UITexture Spt_Icon;
    public GameObject Bg;
    public FBFriend tmpInfo;
    public void MyStart(GameObject root)
    {
        this._uiRoot = root;
        if (_uiRoot == null)
            return;

        Lbl_Descript = _uiRoot.transform.FindChild("Descript").gameObject.GetComponent<UILabel>();
        Lbl_Name = _uiRoot.transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Spt_Icon = _uiRoot.transform.FindChild("Head/Icon").gameObject.GetComponent<UITexture>();
        Bg = _uiRoot.transform.FindChild("BG").gameObject;
        UIEventListener.Get(this.Bg).onClick = OnGive;
    }
    public void OnGive(GameObject btn)
    {
        if (this.tmpInfo == null)
            return;
        FriendModule.Instance.SendInviteThirdFriendsReq(fogs.proto.msg.ThirdFriendType.FACEBOOK,fogs.proto.msg.ThirdFriendsOpResultType.FAKE_SUCCESS,this.tmpInfo.Name);
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_FRIENDINVITEVIEW);
    }
    public void SetInfo(FBFriend vFriend)
    {
        this.tmpInfo = vFriend;
        if (this.tmpInfo == null)
            return;
        this.Lbl_Name.text = this.tmpInfo.Name;
        this.Spt_Icon.mainTexture = this.tmpInfo.uiTexture;
        this.tmpInfo.DownLoadEvent += tmpInfo_DownLoadEvent;
    }

    void tmpInfo_DownLoadEvent(Texture2D texture)
    {
        this.Spt_Icon.mainTexture = texture;
    }
    
}
