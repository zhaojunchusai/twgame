using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
public class FriendMissionComonent : MonoBehaviour
{
    public GameObject _uiRoot;

    public UILabel Lbl_Descript;
    public UIButton Btn_GiveBtn;
    public UILabel Lbl_Btn_Give;
    public UILabel MissionNum;
    public UIGrid grid;
    public GameObject Item;

    public fogs.proto.msg.TaskInfo tmpInfo;
    public delegate void OnGiveDele(FriendMissionComonent com);
    public event OnGiveDele OnGiveEvent;
    public void MyStart(GameObject root)
    {
        this._uiRoot = root;
        if (_uiRoot == null)
            return;

        Lbl_Descript = _uiRoot.transform.FindChild("Descript").gameObject.GetComponent<UILabel>();
        Btn_GiveBtn = _uiRoot.transform.FindChild("GiveBtn").gameObject.GetComponent<UIButton>();
        Lbl_Btn_Give = _uiRoot.transform.FindChild("GiveBtn/Lb").gameObject.GetComponent<UILabel>();
        MissionNum = _uiRoot.transform.FindChild("MissionNum").gameObject.GetComponent<UILabel>();
        grid = _uiRoot.transform.FindChild("Grid").gameObject.GetComponent<UIGrid>();
        Item = _uiRoot.transform.FindChild("Grid/Material").gameObject;
        Item.SetActive(false);
        UIEventListener.Get(this.Btn_GiveBtn.gameObject).onClick = OnGive;
    }
    public void OnGive(GameObject btn)
    {
        if (this.tmpInfo.status == fogs.proto.msg.TaskStatus.FINISHED)
        {
            if (this.OnGiveEvent != null)
                this.OnGiveEvent(this);
        }
    }
    public void SetInfo(fogs.proto.msg.TaskInfo Info,bool isShowNum = false)
    {
        this.tmpInfo = Info;
        if (this.tmpInfo == null)
            return;
        FriendMissionInfo missionInfo = ConfigManager.Instance.mFriendMissionConfig.FindById(this.tmpInfo.id);
        if (missionInfo == null)
            return;

        this.Lbl_Descript.text = string.Format(missionInfo.Descript, missionInfo.PlayerLevel, missionInfo.PlayerCount);

        switch (this.tmpInfo.status)
        {
            case fogs.proto.msg.TaskStatus.FINISHED:
                this.Btn_GiveBtn.gameObject.SetActive(true);
                this.MissionNum.gameObject.SetActive(false);
                CommonFunction.SetGameObjectGray(this.Btn_GiveBtn.gameObject, false);
                this.Lbl_Btn_Give.text = ConstString.EMAIL_RECEIVE;
                break;
            case fogs.proto.msg.TaskStatus.AWARDED:
                this.Btn_GiveBtn.gameObject.SetActive(true);
                CommonFunction.SetGameObjectGray(this.Btn_GiveBtn.gameObject, true);
                this.Lbl_Btn_Give.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                this.MissionNum.gameObject.SetActive(false);
                break;
            case fogs.proto.msg.TaskStatus.UNFINISHED:
                this.Btn_GiveBtn.gameObject.SetActive(false);
                this.MissionNum.gameObject.SetActive(true);
                this.MissionNum.text = string.Format("({0}/{1})", this.tmpInfo.con_value, missionInfo.PlayerCount);
                break;
        }

        List<CommonItemData> tmpData = CommonFunction.GetCommonItemDataList(missionInfo.RewardBag);
        List<Transform> tmpReward = this.grid.GetChildList();

        for (int i = 0; i < tmpData.Count; ++i)
        {
            GameObject tmpItem = null;
            if (i < tmpReward.Count && tmpReward[i] != null)
            {
                tmpItem = tmpReward[i].gameObject;
            }
            else
            {
                tmpItem = CommonFunction.InstantiateObject(this.Item, this.grid.transform);
                this.grid.AddChild(tmpItem.transform);
            }
            if (tmpItem == null)
                continue;
            tmpItem.SetActive(true);
            UISprite icon = tmpItem.transform.Find("Head/Icon").gameObject.GetComponent<UISprite>();
            UISprite quality = tmpItem.transform.Find("Head/IconFrame").gameObject.GetComponent<UISprite>();
            UISprite quality_bg = tmpItem.transform.Find("Head/IconBG").gameObject.GetComponent<UISprite>();
            UILabel num = tmpItem.transform.Find("Num").gameObject.GetComponent<UILabel>();
            CommonFunction.SetSpriteName(icon, tmpData[i].Icon);
            CommonFunction.SetQualitySprite(quality, tmpData[i].Quality, quality_bg);
            CommonItemData tmpCData = tmpData[i];
            UIEventListener.Get(quality.gameObject).onPress = (GameObject go, bool state) => 
            {
                HintManager.Instance.SeeDetail(go, state, tmpCData);
            };
            num.text = string.Format("x{0}", CommonFunction.GetTenThousandUnit(tmpData[i].Num,10000));
        }
        if (tmpReward.Count > tmpData.Count)
        {
            for (int i = tmpData.Count; i < tmpReward.Count; ++i)
            {
                if (tmpReward[i] == null)
                    continue;
                tmpReward[i].gameObject.SetActive(false);
            }
        }
        this.grid.repositionNow = true;
    }
}
