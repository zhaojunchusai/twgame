using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;

public class GameActivityContentItemBase : MonoBehaviour
{
    protected Color mLabelNormalColor = new Color(111 / 255f, 52 / 255f, 14 / 255f);
    protected Color mOutlineColor = new Color(234 / 255f, 201 / 255f, 93 / 255f);
    protected Color mLabelDisableColor = new Color(53f / 255f, 53f / 255f, 53f / 255f);

    protected uint _id = 0;
    public uint ID
    {
        get { return _id; }
    }
    /// <summary>
    /// 这个signID用于更新活动具体兑换/奖励情况的,购买物品是dropID,其他都是number
    /// </summary>
    protected uint _signID;
    public uint SignID
    {
        get { return _signID; }
    }

    protected GameAcitvityAwardStateEnum _state = GameAcitvityAwardStateEnum.CanNotReceive;
    public GameAcitvityAwardStateEnum State
    {
        get { return _state; }

    }

    protected List<CommonItemData> mPossibleAwardList = new List<CommonItemData>();

    protected bool mIsInit = false;

    public virtual void UpdateDesc(string str)
    {

    }

    public virtual void SetLabelValues()
    {
       
    }
   
    public virtual void Initialize()
    {
 
    }

    public virtual void UnInitialize()
    { 
    
    }

    protected void ButtonEvent_BuyFundButton(GameObject go)
    {
        GameActivityModule.Instance.OnSendBuyFoundRequest();
    }

    protected void ButtonEvent_GotoBuyFundButton(GameObject go)
    {
        UISystem.Instance.GameActivityView.TryGotoBuyFound();
    }

    protected void ButtonEvent_GetAwardButton(GameObject go)
    {
        if (CommonFunction.GetItemOverflowTip(mPossibleAwardList))
            return;
        GameActivityModule.Instance.OnSendActivityRewardRequest(_id, _signID);
    }

    protected void PressItem(GameObject go, bool press)
    {
        MailAttachmentItem item = go.GetComponent<MailAttachmentItem>();
        HintManager.Instance.SeeDetail(go, press, item.ID);
    }

    public virtual void UpdateState(GameAcitvityAwardStateEnum newState)
    {
        _state = newState;
    }

    public virtual void UpdateState()
    {

    }

    public virtual void UpdateProgress(uint current)
    {
        
    }

    public virtual void UpdateProgress(ActivityReward data)
    {

    }

    public virtual void UpdateRankInfo(ActivityRankInfo info)
    {

    }
}
