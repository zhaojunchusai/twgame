using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ActivityBuyFundItem :GameActivityContentItemBase
{
    
    [HideInInspector]public UIButton Btn_GetAwardButton;
    [HideInInspector]public UILabel Lbl_BtnLabel;
    [HideInInspector]public UILabel Lbl_BuyFundCountLabel;
    [HideInInspector]public UISprite Spt_BtnGetAwardButtonBGSprite;
    [HideInInspector]public UILabel Lbl_DescLabel;
    [HideInInspector]public UISprite Spt_ArrowSprite;
    [HideInInspector]public UISprite Spt_AwardsBGSprite;
    [HideInInspector]public MailAttachmentItem Item_0;
    [HideInInspector]public MailAttachmentItem Item_1;

    public override void Initialize()
    {
        if (mIsInit)
            return;
        mIsInit = true;

        Btn_GetAwardButton = transform.FindChild("GetAwardButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnLabel = transform.FindChild("GetAwardButton/FGLabel").gameObject.GetComponent<UILabel>();
        Spt_BtnGetAwardButtonBGSprite = transform.FindChild("GetAwardButton/BGSprite").gameObject.GetComponent<UISprite>();
        Lbl_DescLabel = transform.FindChild("DescLabel").gameObject.GetComponent<UILabel>();
        Lbl_BuyFundCountLabel = transform.FindChild("BuyCountLabel").gameObject.GetComponent<UILabel>();
        Spt_AwardsBGSprite = transform.FindChild("AwardsBGSprite").gameObject.GetComponent<UISprite>();
        Spt_ArrowSprite = transform.FindChild("ArrowSprite").gameObject.GetComponent<UISprite>();
        Item_0 = transform.FindChild("Item_0").gameObject.AddComponent<MailAttachmentItem>();
        Item_1 = transform.FindChild("Item_1").gameObject.AddComponent<MailAttachmentItem>();
        Item_0.mHideNameLabel = true;
        Item_1.mHideNameLabel = true;
        SetLabelValues();
        UIEventListener.Get(Item_0.gameObject).onPress = PressItem;
        UIEventListener.Get(Item_1.gameObject).onPress = PressItem;
        UIEventListener.Get(Btn_GetAwardButton.gameObject).onClick = ButtonEvent_BuyFundButton;
        UpdateState();
    }

    void Awake()
    {
        Initialize(); 
    }

    public override void SetLabelValues()
    {
        Lbl_DescLabel.text = string.Format(ConstString.GAMEACTIVTIY_FOUND_REQVIP, ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SERVER_FOUNDATION_VIP));
        Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_BUY;
        Lbl_BuyFundCountLabel.text = string.Format(ConstString.GAMEACTIVTIY_FOUND_BUYNUM, 0);
    }

    public override void UpdateState()
    {
        Initialize();
        if (PlayerData.Instance.BuyFundNum > 0)
        {
            Spt_BtnGetAwardButtonBGSprite.color = Color.black;
            Btn_GetAwardButton.collider.enabled = false;
            Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYBUY;
            Lbl_BtnLabel.color = mLabelDisableColor;
            Lbl_BtnLabel.effectStyle = UILabel.Effect.None;

        }
        else
        {
            Spt_BtnGetAwardButtonBGSprite.color = Color.white;
            Btn_GetAwardButton.collider.enabled = true;
            Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_BUY;
            Lbl_BtnLabel.color = mLabelNormalColor;
            Lbl_BtnLabel.effectStyle = UILabel.Effect.Outline;
            Lbl_BtnLabel.effectColor = mOutlineColor;
        }
    }

    public override void UnInitialize()
    {

    }

    public void UpdateItemInfo(uint activityID, int diamondNum)
    {
        Initialize();
        _id = activityID;
        Item_0.UpdateItemInfo(GlobalCoefficient.GemID, diamondNum);
        Item_1.UpdateItemInfo(GlobalCoefficient.FundID, 0);
    }

    public void UpdateCountNum(int number)
    {
        Initialize();
        Lbl_BuyFundCountLabel.text = string.Format(ConstString.GAMEACTIVTIY_FOUND_BUYNUM,CommonFunction.GetTenThousandUnit(number,10000));
    }
}
