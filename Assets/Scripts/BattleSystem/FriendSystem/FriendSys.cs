using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public class FriendSys
{

}
public class FBFriendSys
{
    public List<FBFriend> FBFriendList = new List<FBFriend>();
    public int starGetIndex = 0;
    public int hadGetIndex = 0;
    public const int OneGetCount = 20;
    public bool IsApplyNotice = false;
    private bool isInit = false;
    public delegate bool Filte(FBFriend vFriend);
    public void Clear()
    {
        this.FBFriendList.Clear();
        this.starGetIndex = 0;
        this.hadGetIndex = 0;
        this.isInit = false;
    }
    public void Init()
    {
        if (!this.isInit)
        {
            FriendModule.Instance.SendGetFacebookFriends();
            //this.TEXT();
        }
    }
    public void TEXT()
    {
        for (int i = 20; i < 41; ++i)
        {
            string Id = (i+1).ToString();
            string Name = "xiaoxing" + (i + 1).ToString();
            int num = UnityEngine.Random.Range(0,9);

            string Url = string.Format("http://192.168.0.252:81/TDTest/Icon_Equip_2100000{0}.png", num);
            FBFriend tmpFBFriend = new FBFriend();
            tmpFBFriend.Instance(Id, Name, Url);
            this.FBFriendList.Add(tmpFBFriend);
        }
        this.starGetIndex = 0;
        this.isInit = true;
        this.StartGetThirdFriendsInfoReq();
    }
    public void Searlize(List<object> vFBFriendList)
    {
        this.isInit = true;
        this.FBFriendList.Clear();
        this.starGetIndex = 0;
        this.hadGetIndex = 0;
        if ((vFBFriendList != null) && (vFBFriendList.Count > 0))
        {
            foreach (object tmpListObj in vFBFriendList)
            {
                if (tmpListObj != null)
                {
                    Dictionary<string, object> tmpDic = tmpListObj as Dictionary<string, object>;
                    if (tmpDic != null)
                    {
                        string Id = "";
                        string Name = "";
                        string Url = "";

                        if (tmpDic.ContainsKey("id"))
                            Id = tmpDic["id"] as string;
                        if (tmpDic.ContainsKey("name"))
                            Name = tmpDic["name"] as string;
                        if (tmpDic.ContainsKey("picture"))
                            Url = tmpDic["picture"] as string;
                        FBFriend tmpFBFriend = new FBFriend();
                        tmpFBFriend.Instance(Id,Name,Url);
                        this.FBFriendList.Add(tmpFBFriend);
                    }
                }
            }
        }
    }
    public void MutilAdd(List<object> vFBFriendList)
    {
        if ((vFBFriendList != null) && (vFBFriendList.Count > 0))
        {
            foreach (object tmpListObj in vFBFriendList)
            {
                if (tmpListObj != null)
                {
                    Dictionary<string, object> tmpDic = tmpListObj as Dictionary<string, object>;
                    if (tmpDic != null)
                    {
                        string Id = "";
                        string Name = "";
                        string Url = "";

                        if (tmpDic.ContainsKey("id"))
                            Id = tmpDic["id"] as string;
                        if (tmpDic.ContainsKey("name"))
                            Name = tmpDic["name"] as string;
                        if (tmpDic.ContainsKey("picture"))
                            Url = tmpDic["picture"] as string;
                        FBFriend tmpFBFriend = new FBFriend();
                        tmpFBFriend.Instance(Id, Name, Url);
                        this.FBFriendList.Add(tmpFBFriend);
                    }
                }
            }
        }
        this.starGetIndex = 0;
        this.StartGetThirdFriendsInfoReq();
    }
    public FBFriend FindByID(string id)
    {
        if (this.FBFriendList == null || this.FBFriendList.Count <= 0)
            return null;
        return this.FBFriendList.Find((tmpInfo) => { if (tmpInfo == null)return false; return tmpInfo.Id.Equals(id); });
    }
    public FBFriend FindByName(string Name)
    {
        if (this.FBFriendList == null || this.FBFriendList.Count <= 0)
            return null;
        return this.FBFriendList.Find((tmpInfo) => { if (tmpInfo == null)return false; return tmpInfo.Name.Equals(Name); });
    }
    public List<FBFriend> GetFriendList(Filte vFilte)
    {
        List<FBFriend> tmpList = new List<FBFriend>();
        if (this.FBFriendList == null || vFilte == null)
            return tmpList;

        tmpList.Capacity = this.FBFriendList.Count + 1;
        tmpList = this.FBFriendList.FindAll((tmpFriend) => { return vFilte(tmpFriend); });
        return tmpList;
    }
    public int GetFriendCount()
    {
        if (this.FBFriendList == null || this.FBFriendList.Count <= 0)
            return 0;
        return this.FBFriendList.Count;
    }
    public void StartGetThirdFriendsInfoReq()
    {
        if (this.FBFriendList == null || this.FBFriendList.Count <= 0)
            return;
        if (this.starGetIndex >= this.FBFriendList.Count)
            return;
        if (this.hadGetIndex > this.starGetIndex)
            return;
        List<string> UserId = new List<string>();
        int num = this.starGetIndex + OneGetCount;
        num = (num) < this.FBFriendList.Count ? num : this.FBFriendList.Count;
        this.hadGetIndex += OneGetCount;

        for (int i = starGetIndex; i < num; ++i)
        {
            FBFriend tmpFriend = this.FBFriendList[i];
            if (tmpFriend == null)
                continue;
            tmpFriend.StartDownLoadPicture();
            UserId.Add(tmpFriend.Name);
        }
        FriendModule.Instance.SendGetThirdFriendsInfoReq(ThirdFriendType.FACEBOOK, UserId);
    }
    public void ReceiveGetThirdFriendsInfoResp(List<ThirdFriends> vFriendList)
    {
        if (this.FBFriendList == null || this.FBFriendList.Count <= 0 || vFriendList == null || vFriendList.Count <= 0)
            return;
        for (int i = 0; i < vFriendList.Count; ++i)
        {
            ThirdFriends tmpFriend = vFriendList[i];
            if (tmpFriend == null)
                continue;
            int num = this.starGetIndex + OneGetCount;
            num = (num) < this.FBFriendList.Count ? num : this.FBFriendList.Count;
            bool isfind = false;
            for (int j = starGetIndex; j < num; ++j)
            {
                FBFriend tmpFb = this.FBFriendList[j];
                if (tmpFb == null)
                    continue;
                if (tmpFb.Name.Equals(tmpFriend.name))
                {
                    tmpFb.LastLoadTime = tmpFriend.login_tick;
                    tmpFb.next_tick = tmpFriend.next_tick;
                    isfind = true;
                    break;
                }
            }
            if(!isfind)
            {
                FBFriend tmpFb = this.FindByName(tmpFriend.name);
                if (tmpFb == null)
                    continue;
                tmpFb.LastLoadTime = tmpFriend.login_tick;
                tmpFb.next_tick = tmpFriend.next_tick;
            }
        }
        if(this.hadGetIndex == OneGetCount)
        {
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitFBFriendTable();
        }
        this.starGetIndex += OneGetCount;
        if(this.hadGetIndex > this.starGetIndex)
        {
            this.StartGetThirdFriendsInfoReq();
            return;
        }
    }
    public int GetStarGetThirdFriendsInfoReqIndex()
    {
        int num = this.starGetIndex - OneGetCount / 2;
        return num < 0 ? 0 : num;
    }
}
public class FBFriend
{
    public string Id;
    public string Name;
    public string Url;
    public Texture2D uiTexture;
    public bool IsHadDownLoad = false;
    public UInt32 LastLoadTime;
    public UInt32 next_tick = 0;
    public delegate void DownLoadCallBack(Texture2D texture);
    public event DownLoadCallBack DownLoadEvent;
    public void Instance(string vId,string vName,string vUrl)
    {
        this.Id = vId;
        this.Name = vName;
        this.Url = vUrl;
    }
    public void StartDownLoadPicture()
    {
        if (this.uiTexture != null)
            return;
        HttpDownLoadTexture.Instance.StartDownLoad(new KeyValuePair<string, HttpDownLoadTexture.DownLoadCallBack>(Url, OnDownLoad));
    }
    public void getTexture(DownLoadCallBack callBack)
    {
        if (this.uiTexture != null && callBack != null)
            callBack(this.uiTexture);
        else
        {
            DownLoadEvent += callBack;
        }
    }
    void OnDownLoad(Texture2D texture)
    {
        this.uiTexture = texture;
        if (DownLoadEvent != null)
        {
            DownLoadEvent(this.uiTexture);
            Delegate[] delegateList = DownLoadEvent.GetInvocationList();
            if (delegateList != null)
            {
                foreach (Delegate tmp in delegateList)
                {
                    if (tmp != null)
                        DownLoadEvent -= tmp as DownLoadCallBack;
                }
            }
        }
    }
}
public class HttpDownLoadTexture : Singleton<HttpDownLoadTexture>
{
    public delegate void DownLoadCallBack(Texture2D texture);
    List<KeyValuePair<string, DownLoadCallBack>> DownUrlList = new List<KeyValuePair<string, DownLoadCallBack>>();

    public void StartDownLoad(KeyValuePair<string, DownLoadCallBack> vValue)
    {
        if (this.DownUrlList.Count <= 0)
        {
            this.DownUrlList.Add(vValue);
            Main.Instance.StartCoroutine(DownLoad());
        }
        else
            this.DownUrlList.Add(vValue);
    }
    private IEnumerator DownLoad()
    {
        if(DownUrlList == null || DownUrlList.Count <= 0)
            yield break;

        while (DownUrlList.Count > 0)
        {
            if (string.IsNullOrEmpty(DownUrlList[0].Key))
            {
                DownUrlList.RemoveAt(0);
                continue;
            }
            WWW www = new WWW(DownUrlList[0].Key);
            yield return www;

            if (DownUrlList[0].Value != null && string.IsNullOrEmpty(www.error))
                DownUrlList[0].Value(www.texture);
            www.Dispose();
            DownUrlList.RemoveAt(0);
        }
    }
}