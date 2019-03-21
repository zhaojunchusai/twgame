using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ActivityBuyFundCountItem : GameActivityContentItemBase
{
    [HideInInspector]public UISprite Spt_AwardsBGSprite;
    [HideInInspector]public UILabel Lbl_DescLabel;
    [HideInInspector]public UILabel Lbl_NumLabel;

    [HideInInspector]public UIButton Btn_BuyButton;
    [HideInInspector]public UISprite Spt_BtnBuyButtonBGSprite;
    [HideInInspector]public UILabel Lbl_BtnLabel;

    public override void Initialize()
    {
        if (mIsInit)
            return;
        mIsInit = true;

        Spt_AwardsBGSprite = transform.FindChild("AwardsBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_DescLabel = transform.FindChild("DescLabel").gameObject.GetComponent<UILabel>();
        Lbl_NumLabel = transform.FindChild("Label").gameObject.GetComponent<UILabel>();
        Btn_BuyButton = transform.FindChild("BuyButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnLabel = transform.FindChild("BuyButton/FGLabel").gameObject.GetComponent<UILabel>();
        Spt_BtnBuyButtonBGSprite = transform.FindChild("BuyButton/BGSprite").gameObject.GetComponent<UISprite>();
        SetLabelValues();
        UIEventListener.Get(Btn_BuyButton.gameObject).onClick = ButtonEvent_GotoBuyFundButton;
        UpdateState();
    }

    void Awake()
    {
        Initialize(); 
    }

    public override void SetLabelValues()
    {
        Lbl_DescLabel.text = ConstString.GAMEACTIVTIY_FOUND_CURRENTNUM;
        Lbl_NumLabel.text = "0";
        Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_WELFARE_IWANTTOBUY;
    }

    public override void UpdateState()
    {
        Initialize();
        if (PlayerData.Instance.BuyFundNum > 0)
        {
            Spt_BtnBuyButtonBGSprite.color = Color.black;
            Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYBUY;
            Btn_BuyButton.collider.enabled = false;
            Lbl_BtnLabel.color = mLabelDisableColor;
            Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
           
        }
        else
        {
            Spt_BtnBuyButtonBGSprite.color = Color.white;
            Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_WELFARE_IWANTTOBUY;
            Btn_BuyButton.collider.enabled = true;
            Lbl_BtnLabel.color = mLabelNormalColor;
            Lbl_BtnLabel.effectStyle = UILabel.Effect.Outline;
            Lbl_BtnLabel.effectColor = mOutlineColor;
        }
    }

    public override void UnInitialize()
    {

    }

    public void UpdateCountNum(int num)
    {
        Initialize();
        //num = Mathf.Max(0, num);
        Lbl_NumLabel.text =CommonFunction.GetTenThousandUnit(num);
    }

}
