using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class FriendInviteViewController : UIBase
{
    public FriendInviteView view;
    public List<FriendInviteItem> FBInviteItemList = new List<FriendInviteItem>();
    public List<FBFriend> tmpFBInviteList = new List<FBFriend>();

    public override void Initialize()
    {
        if (view == null)
            view = new FriendInviteView();
        view.Initialize();
        BtnEventBinding();
        //FriendModule.Instance.SendGetFacebookFriends();
        tmpFBInviteList = PlayerData.Instance.FbfriendDepot.GetFriendList(Filte);
        Main.Instance.StartCoroutine(CreatFBInviteItem(tmpFBInviteList));
    }
    bool Filte(FBFriend vFriend)
    {
        if (vFriend == null)
            return false;

        return vFriend.LastLoadTime <= 0;
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        this.FBInviteItemList.Clear();
        this.tmpFBInviteList.Clear();
    }
    private void ClickMask(GameObject go)
    {
        this.Close(null,null);
    }
    private IEnumerator CreatFBInviteItem(List<FBFriend> _data)
    {
        yield return 0.5;
        int count = _data.Count;
        int itemCount = FBInviteItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.wrapContent.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.wrapContent.minIndex = -index;
        this.view.wrapContent.maxIndex = 0;

        if (count > 5)
        {
            this.view.wrapContent.enabled = true;
            count = 5;
        }
        else
        {
            this.view.wrapContent.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                FBInviteItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {

                GameObject vGo = CommonFunction.InstantiateObject(this.view.Gobj_UnionApplyItem, this.view.Grd_Content.transform);
                FriendInviteItem item = vGo.GetComponent<FriendInviteItem>();
                if (item == null)
                {
                    item = vGo.AddComponent<FriendInviteItem>();
                    item.MyStart(vGo);
                }
                FBInviteItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
            }
            else
            {
                FBInviteItemList[i].gameObject.SetActive(true);
            }
            FBInviteItemList[i].SetInfo(_data[i]);
        }

        this.view.wrapContent.ReGetChild();
        this.view.Grd_Content.repositionNow = true;
        yield return 0;
        this.view.ScrView_Content.ResetPosition();
        yield return 0.5;
        this.view.Grd_Content.repositionNow = true;
        this.view.Grd_Content.gameObject.SetActive(false);
        this.view.Grd_Content.gameObject.SetActive(true);
    }
    public void SetFBInviteInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpFBInviteList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        if (realIndex > PlayerData.Instance.FbfriendDepot.GetStarGetThirdFriendsInfoReqIndex())
            PlayerData.Instance.FbfriendDepot.StartGetThirdFriendsInfoReq();

        FriendInviteItem item = FBInviteItemList[wrapIndex];
        item.SetInfo(tmpFBInviteList[realIndex]);
    }

    public override void Uninitialize()
    {
        view.ScrView_Content.onDragFinishedDown = null;
    }
    public void BtnEventBinding()
    {
        this.view.wrapContent.onInitializeItem = SetFBInviteInfo;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ClickMask;
    }


}
