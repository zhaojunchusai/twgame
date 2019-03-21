using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ActivitySingleAwardItem : GameActivityContentItemBase
{

    [HideInInspector]public UIButton Btn_GetAwardButton;
    [HideInInspector]public UILabel Lbl_BtnLabel;
    [HideInInspector]public UISprite Spt_BtnGetAwardButtonBGSprite;
    [HideInInspector]public UISprite Spt_AwardsBGSprite;
    [HideInInspector]public UILabel Lbl_DescLabel;
    [HideInInspector]public MailAttachmentItem Item_0;

    public override void Initialize()
    {
        if (mIsInit)
            return;
        mIsInit = true;

        Btn_GetAwardButton = transform.FindChild("GetAwardButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnLabel = transform.FindChild("GetAwardButton/FGLabel").gameObject.GetComponent<UILabel>();
        Spt_BtnGetAwardButtonBGSprite = transform.FindChild("GetAwardButton/BGSprite").gameObject.GetComponent<UISprite>();
        Spt_AwardsBGSprite = transform.FindChild("AwardsBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_DescLabel = transform.FindChild("DescLabel").gameObject.GetComponent<UILabel>();
        Item_0 = transform.FindChild("Item_0").gameObject.AddComponent<MailAttachmentItem>();
        Item_0.mHideNameLabel = true;

        SetLabelValues();
        UIEventListener.Get(Btn_GetAwardButton.gameObject).onClick = ButtonEvent_GetAwardButton;
        UIEventListener.Get(Item_0.gameObject).onPress = PressItem;
        UpdateState(GameAcitvityAwardStateEnum.CanNotReceive);
    }

    void Awake()
    {
        Initialize(); 
    }

    public override void SetLabelValues()
    {
        Lbl_DescLabel.text = "";
        Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_GETAWARD;
    }

    public override void UpdateDesc(string str)
    {
        if (string.IsNullOrEmpty(str))
            Lbl_DescLabel.text = "";
        Lbl_DescLabel.text = str;
    }

    public override void UnInitialize()
    {
        mPossibleAwardList.Clear();
    }

    public override void UpdateState(GameAcitvityAwardStateEnum newState)
    {
        base.UpdateState(newState);
        switch (_state)
        {
            case GameAcitvityAwardStateEnum.CanNotReceive:
                Spt_BtnGetAwardButtonBGSprite.color = Color.black;
                Btn_GetAwardButton.gameObject.collider.enabled = false;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_GETAWARD;
                Lbl_BtnLabel.color = mLabelDisableColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
                break;
            case GameAcitvityAwardStateEnum.CanReceive:
                Spt_BtnGetAwardButtonBGSprite.color = Color.white;
                Btn_GetAwardButton.gameObject.collider.enabled = true;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_GETAWARD;
                Lbl_BtnLabel.color = mLabelNormalColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.Outline;
                Lbl_BtnLabel.effectColor = mOutlineColor;
                break;
            case GameAcitvityAwardStateEnum.Recieved:
                Spt_BtnGetAwardButtonBGSprite.color = Color.black;
                Btn_GetAwardButton.gameObject.collider.enabled = false;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                Lbl_BtnLabel.color = mLabelDisableColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
                break;
        }
    }

    public void UpdateItemInfo(uint activityID, GameActivityReqData data)
    {
        Initialize();

        _id = activityID;
        _signID = (uint)data.condition;
        List<CommonItemData> list = new List<CommonItemData>();
        if (data.mDrops != null && data.mDrops.Count > 0)
            foreach (var tmp in data.mDrops)
            {
                CommonItemData item = new CommonItemData(tmp.drop_id, tmp.drop_number);
                list.Add(item);
            }
        mPossibleAwardList.Clear();
        mPossibleAwardList.AddRange(list);
        if (list.Count < 1)
        {
            Debug.LogError("Can find info for SingleAwardItem  dropID =" + data.rewards_id);
            return;
        }
        Item_0.UpdateItemInfo(list[0].ID, list[0].Num);
    }

}
