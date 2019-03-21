using UnityEngine;
using System;
using System.Collections;

public class RechargeItem : MonoBehaviour
{
    [HideInInspector]
    public UIButton Btn_RechargeItem;
    [HideInInspector]
    public UISprite Spt_ActIcon;
    [HideInInspector]
    public UILabel Lbl_Title;
    [HideInInspector]
    public UILabel Lbl_Desc;
    [HideInInspector]
    public UILabel Lbl_Price;
    [HideInInspector]
    public UISprite Spt_BG;
    [HideInInspector]
    public UISprite Spt_DescBG;
    [HideInInspector]
    public UISprite Spt_IconBG1;
    [HideInInspector]
    public UISprite Spt_IconBG2;
    [HideInInspector]
    public UISprite Spt_FirstPay;
    [HideInInspector]
    public UISprite Spt_HasBuy;
    private bool _initialized = false;
    private RechargeShowData _data = new RechargeShowData();
    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;
        Btn_RechargeItem = GetComponent<UIButton>();
        Spt_ActIcon = transform.FindChild("RechargeInfo/ActIcon").gameObject.GetComponent<UISprite>();
        Lbl_Title = transform.FindChild("RechargeInfo/Title").gameObject.GetComponent<UILabel>();
        Lbl_Desc = transform.FindChild("RechargeInfo/Desc").gameObject.GetComponent<UILabel>();
        Lbl_Price = transform.FindChild("RechargeInfo/Price").gameObject.GetComponent<UILabel>();
        //Spt_BG = transform.FindChild("BGS/BG").gameObject.GetComponent<UISprite>();
        //Spt_DescBG = transform.FindChild("BGS/DescBG").gameObject.GetComponent<UISprite>();
        //Spt_IconBG1 = transform.FindChild("BGS/IconBG1").gameObject.GetComponent<UISprite>();
        //Spt_IconBG2 = transform.FindChild("BGS/IconBG2").gameObject.GetComponent<UISprite>();
        Spt_FirstPay = transform.FindChild("FirstPay").gameObject.GetComponent<UISprite>();
        Spt_HasBuy = transform.FindChild("HasBuy").gameObject.GetComponent<UISprite>();
        //SetLabelValues();

        UIEventListener.Get(Btn_RechargeItem.gameObject).onClick = ButtonEvent_RechargeItem;
    }

    public void InitItem(RechargeShowData data)
    {
        Initialize();
        if (data != null)
        {
            _data = data;
        }

        Lbl_Price.text = string.Format(ConstString.FORMAT_RECHARGE_PRICE, _data.Price);
        Spt_HasBuy.gameObject.SetActive(false);
        if (_data.IsMonthCard)
        {
            Spt_FirstPay.gameObject.SetActive(false);
            Lbl_Title.text = ConstString.MONTH_CARD;
            if (PlayerData.Instance.HasMonthCard == 1)
            {
                Spt_HasBuy.gameObject.SetActive(true);
                Lbl_Desc.text = string.Format(ConstString.FORMAT_MONTH_CARD_DESC_BOUGHT, CommonFunction.GetLfetTime((long)PlayerData.Instance.MonthCardDeadline) / 86400);
            }
            else
            {
                Lbl_Desc.text = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.MONTH_CARD_DESC);
            }
        }
        else
        {
            Lbl_Title.text = string.Format(ConstString.FORMAT_RECHARGE_ITEM_TITLE, _data.GetDiamond);
            if (!PlayerData.Instance.NoFirstPayList.Contains((int)_data.ID))
            {
                Spt_FirstPay.gameObject.SetActive(true);

                Lbl_Desc.text = _data.FirstGift > 0 ? string.Format(ConstString.FORMAT_RECHARGE_GIFT, _data.FirstGift) : ConstString.RECHARGE_NO_GIFT;
            }
            else
            {
                Spt_FirstPay.gameObject.SetActive(false);
                Lbl_Desc.text = _data.NormalGift > 0 ? string.Format(ConstString.FORMAT_RECHARGE_GIFT, _data.NormalGift) : ConstString.RECHARGE_NO_GIFT;
            }

        }
        CommonFunction.SetSpriteName(Spt_ActIcon, _data.Icon);
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = "";
        Lbl_Desc.text = "";
        Lbl_Price.text = "";
    }

    public void ButtonEvent_RechargeItem(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Get_Reward, gameObject.transform));

        if (_data.IsMonthCard && PlayerData.Instance.HasMonthCard == 1)
        {
            ErrorCode.ShowErrorTip((int)ErrorCodeEnum.HadMonthCard);
            return;
        }
        if (!GlobalConst.ISOPENSDKOPERATE)
        {
            StoreModule.Instance.SendGetCommodityOrderNum(_data.ID);
        }
        else
        {
            if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
            {
                int tmpState = 0;
#if UNITY_ANDROID
                bool state = true;
                if (GlobalConst.IS_GOOGLEAPK)
                {
                    state = ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.P7725ThirdPartyPayment, false);
                }
                SDKManager.Instance.EnableThirdPartyPayment(state);
                if (state)
                {
                    tmpState = 1;
                }
#endif
                if (_data.IsMonthCard)
                {
                    SDKManager.Instance.ShowRecharge(((uint)GlobalConst.PLATFORM).ToString(), tmpState.ToString(), 0, true);
                }
                else
                {
                    SDKManager.Instance.ShowRecharge(((uint)GlobalConst.PLATFORM).ToString(), tmpState.ToString(), 0, false);
                }
            }
        }
    }

    public void Uninitialize()
    {

    }


}
