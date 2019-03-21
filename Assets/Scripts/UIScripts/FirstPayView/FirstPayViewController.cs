using UnityEngine;
using System;
using System.Collections.Generic;

public class FirstPayViewController : UIBase
{
    public FirstPayView view;
    private List<CommonItemData> _rewards;
    private bool _isFirstPay = true;
    private Vector3 _btnPos1 = new Vector3(-240, -170, 0);
    private Vector3 _btnPos2 = new Vector3(-150, -170, 0);
    private Vector3 _btnPos3 = new Vector3(-30, -170, 0);
    public override void Initialize()
    {
        if (view == null)
            view = new FirstPayView();
        view.Initialize();
        BtnEventBinding();
    }
    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(FirstPayView.UIName);
    }

    public void ShowGift(bool isFirstPay)
    {
        _isFirstPay = isFirstPay;
        if (isFirstPay)
        {
            CommonFunction.SetSpriteName(view.Spt_FG, GlobalConst.SpriteName.FIRST_PAY_BG);
            view.Spt_FG.transform.localPosition = new Vector3(0, -15, 0);
            view.Spt_FG.width = 1000;
            view.Spt_FG.height = 444;

            view.Btn_Recharge.gameObject.SetActive(true);
            view.Btn_GetReward.gameObject.SetActive(true);
            view.Btn_Recharge.transform.localPosition = _btnPos1;
            view.Btn_GetReward.transform.localPosition = _btnPos3;

            view.Lbl_Title.text = ConstString.TITLE_FIRST_PAY;
            _rewards = CommonFunction.GetCommonItemDataList(GlobalCoefficient.FirstPayGiftDroppackID);
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_FG, GlobalConst.SpriteName.FIRST_LOGIN_REWARD_BG);
            view.Spt_FG.transform.localPosition = new Vector3(3, 0, 0);
            view.Spt_FG.width = 992;
            view.Spt_FG.height = 556;

            view.Btn_Recharge.gameObject.SetActive(false);
            view.Btn_GetReward.gameObject.SetActive(true);
            view.Btn_GetReward.transform.localPosition = _btnPos2;

            view.Lbl_Title.text = ConstString.TITLE_FIRST_LOGIN_REWARD;
            _rewards = CommonFunction.GetCommonItemDataList(
                uint.Parse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.FIRST_LOGIN_REWARD_DROP_ID)));
        }
        ShowGift();
        for (int i = 0; i < view.Spt_IconFrames.Length; i++)
        {
            UIEventListener.Get(view.Spt_IconFrames[i].gameObject).onPress = PressProp;
        }
    }

    private void ShowGift()
    {
        if (_rewards.Count != 3)
        {
            Debug.LogError("first pay gift droppack xml config error");
            return;
        }
        for (int i = 0; i < _rewards.Count; i++)
        {
            CommonFunction.SetSpriteName(view.Spt_Icons[i], _rewards[i].Icon);
            CommonFunction.SetQualitySprite(view.Spt_IconFrames[i], _rewards[i].Quality);
            view.Lbl_Names[i].text = _rewards[i].Name;
        }
    }

    private void PressProp(GameObject go, bool press)
    {
        int index = -1;
        switch (go.transform.parent.name)
        {
            case "Prop1":
                {
                    index = 0;
                    break;
                }
            case "Prop2":
                {
                    index = 1;
                    break;
                }
            case "Prop3":
                {
                    index = 2;
                    break;
                }
        }

        if (index == -1)
        {
            Debug.LogError("PressProp Error");
            return;
        }
        HintManager.Instance.SeeDetail(go, press, _rewards[index].ID);
    }

    public void ButtonEvent_BackBtn(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(FirstPayView.UIName);
    }

    public void ButtonEvent_Recharge(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
        UISystem.Instance.VipRechargeView.ShowRecharge();
        //UISystem.Instance.CloseGameUI(FirstPayView.UIName);
    }

    public void ButtonEvent_GetReward(GameObject btn)
    {
        if (_isFirstPay)
            MainCityModule.Instance.SendFirstRechargeAward();
        else
        {
            MainCityModule.Instance.SendGetFirstLoginReward();
        }
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_BackBtn.gameObject).onClick = ButtonEvent_BackBtn;
        UIEventListener.Get(view.Btn_Recharge.gameObject).onClick = ButtonEvent_Recharge;
        UIEventListener.Get(view.Btn_GetReward.gameObject).onClick = ButtonEvent_GetReward;
    }
}
