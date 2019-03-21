using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;
public class CaptureTerritoryViewController : UIBase 
{
    public CaptureTerritoryView view;
    private List<CompaignCityItem> _cities;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new CaptureTerritoryView();
            view.Initialize();
            view.Trans_CityParent.localScale = Vector3.one / 0.79f;
            view.Trans_Map.localScale = Vector3.one * 0.79f;
        }
        BtnEventBinding();
        InitAllocateBoxNotice();
        InitToken();
        InitCity();
        SetTitle();
        SetFightCD();
        CaptureTerritoryModule.Instance.CampaignTimerUpdate += SetTitle;
        Assets.Script.Common.Scheduler.Instance.AddLateUpdator(LateUpdateFixPanel);
        CaptureTerritoryModule.Instance.FightCDTimerUpdate += SetFightCD;
    }

    public void InitUI()
    {
        InitAllocateBoxNotice();
        InitToken();
        InitCity();
        SetTitle();
        SetFightCD();
    }

    public override void Destroy()
    {
        view = null;
        _cities = null;
    }

    private void SetMapScale(bool big)
    {
        view.ScrView_Map.ResetPosition();
        view.ScrView_Map.transform.localPosition = Vector3.zero;
        view.UIPanel_Map.transform.localPosition = Vector3.zero;
        view.Trans_Map.localPosition = Vector3.zero;

        view.Trans_Map.localScale = big ? Vector3.one : Vector3.one * 0.79f;

        view.Spt_BtnAddScaleBackground.color = big ? Color.black : Color.white;
        view.Spt_BtnReduceScaleBackground.color = big ? Color.white : Color.black;
    }

    private void LateUpdateFixPanel()
    {

    }

    public void SetTitle()
    {
        //if (CaptureTerritoryModule.Instance.CampaignTimer > 0)
        //{
        //    view.Gobj_Title.SetActive(false);
        //    view.Lbl_Timer.gameObject.SetActive(true);
        //    view.Lbl_Timer.text = CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.CampaignTimer);
        //}
        //else
        //{
        //    view.Gobj_Title.SetActive(true);
        //    view.Lbl_Timer.gameObject.SetActive(false);
        //}
        if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting)
        {            
            view.Lbl_Timer.text = string.Format(ConstString.TIP_CAMPAIGN_TIMER_END, 
                CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.CampaignTimer));            
        }
        else
        {
            view.Lbl_Timer.text = string.Format(ConstString.TIP_CAMPAIGN_TIMER_START,
                CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.CampaignTimer));
        }
    }

    public void SetFightCD()
    {
        if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting 
            && CaptureTerritoryModule.Instance.FightCDTimer > 0)
        {
            view.Lbl_FightCD.gameObject.SetActive(true);
            view.Lbl_FightCD.text = string.Format(ConstString.FIGHT_CD_TIP,
                CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.FightCDTimer));
        }
        else
        {
            view.Lbl_FightCD.gameObject.SetActive(false);
        }               
    }

    private void InitCity()
    {
        if (_cities != null)
        {
            RefreshCityState();            
            return;
        }
        _cities = new List<CompaignCityItem>();
        List<CaptureCityData> list = ConfigManager.Instance.mCaptureTerritoryConfig.GetCityList();
        for (int i = 0; i < list.Count; i++)
        {
            _cities.Add(CommonFunction.InstantiateItem<CompaignCityItem>(view.Gobj_CityItem, view.Trans_CityParent));
            _cities[i].transform.localPosition = list[i].mPos;
            _cities[i].Init(list[i]);
        }
    }

    public void RefreshCityState()
    {
        for (int i = 0; i < _cities.Count; i++)
        {
            _cities[i].SetFightState();
        }
    }

    public void InitAllocateBoxNotice()
    {
        if(UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_CHAIRMAN)
        {
            view.Btn_Allocation.gameObject.SetActive(true);
            view.Spt_BtnAllocationNotice.gameObject.SetActive(CaptureTerritoryModule.Instance.ChestInfo.Count > 0);
        }
        else
        {
            view.Btn_Allocation.gameObject.SetActive(false);
        }
    }

    public void InitToken()
    {
        if (UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_CHAIRMAN)
        {
            if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.NotFighting)
            {
                SetTokenIncrease(false, false, 0, 0, 0);
                CommonFunction.SetSpriteName(view.Spt_NotFighting, GlobalConst.SpriteName.CaptureTokenCanNotClick);
                view.Spt_NotFighting.width = 154;
                view.Spt_NotFighting.height = 18;
            }
            else
            {
                bool hasBuy = false;
                foreach (TokenLevel item in CaptureTerritoryModule.Instance.TokenInfo)
                {
                    if (item.level > 0)
                    {
                        hasBuy = true;
                        break;
                    }
                }
                SetTokenIncrease(hasBuy, true, 0, 0, 0);
                CommonFunction.SetSpriteName(view.Spt_NotFighting, GlobalConst.SpriteName.CaptureTokenCanClick);
                view.Spt_NotFighting.width = 162;
                view.Spt_NotFighting.height = 26;
            }
        }
        else
        {
            if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.NotFighting)
            {
                SetTokenIncrease(true, false, 0, 0, 0);
            }
            else
            {                
                SetTokenIncrease(true,true,0,0,0);
            }
        }
    }

    /// <summary>
    /// 设置攻城令界面信息
    /// </summary>
    /// <param name="showIncNum">是否显示攻城令伤害提升</param>
    /// <param name="useModuleData">是否使用module的实际数据</param>
    /// <param name="wei">魏国伤害提升(不使用module时)</param>
    /// <param name="shu">蜀国伤害提升(不使用module时)</param>
    /// <param name="wu">吴国伤害提升(不使用module时)</param>
    private void SetTokenIncrease(bool showIncNum,bool useModuleData,float wei ,float shu,float wu)
    {
        if (showIncNum)
        {
            view.Gobj_TokenIncrease.SetActive(true);
            view.Spt_NotFighting.gameObject.SetActive(false);

            if (useModuleData)
            {
                SetIncreaseLabel(
                    CaptureTerritoryModule.Instance.GetTokenIncrease(NationType.WEI)
                    , CaptureTerritoryModule.Instance.GetTokenIncrease(NationType.SHU)
                    , CaptureTerritoryModule.Instance.GetTokenIncrease(NationType.WU)
                    );
            }
            else
            {
                SetIncreaseLabel(wei, shu, wu);
            }
        }
        else
        {
            view.Gobj_TokenIncrease.SetActive(false);
            view.Spt_NotFighting.gameObject.SetActive(true);
            SetIncreaseLabel(wei, shu, wu);
        }
    }

    private void SetIncreaseLabel(float wei, float shu, float wu)
    {
        view.Lbl_CountryInc1.text = string.Format(ConstString.PLAYEREXP, Mathf.FloorToInt(wei * 100));
        view.Lbl_CountryInc2.text = string.Format(ConstString.PLAYEREXP, Mathf.FloorToInt(shu * 100));
        view.Lbl_CountryInc3.text = string.Format(ConstString.PLAYEREXP, Mathf.FloorToInt(wu * 100));

        view.Spt_CountryIncFG1.fillAmount = wei;
        view.Spt_CountryIncFG2.fillAmount = shu;
        view.Spt_CountryIncFG3.fillAmount = wu;

        view.Spt_Token1.color = wei > 0 ? Color.white : Color.black;
        view.Spt_Token2.color = shu > 0 ? Color.white : Color.black;
        view.Spt_Token3.color = wu > 0 ? Color.white : Color.black;
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(CaptureTerritoryView.UIName);
    }

    public void ButtonEvent_Rank(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(RankView.UIName);
        UISystem.Instance.RankView.ShowRank(RankType.CAMPAIGN_UNION);
    }

    public void ButtonEvent_Rule(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(CaptureTerritoryRule.UIName);
    }

    public void ButtonEvent_Allocation(GameObject btn)
    {
        if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TIP_FIGHTING_TO_ALLOCATE_BOX);
            return;
        }
        CaptureTerritoryModule.Instance.SendGetScoreInfo();
    }

    public void ButtonEvent_AddScale(GameObject btn)
    {
    }

    public void ButtonEvent_ReduceScale(GameObject btn)
    {
    }

    public void ButtonEvent_Token(GameObject btn)
    {
        if (UnionModule.Instance.MyUnionMemberInfo.position != UnionPosition.UP_CHAIRMAN)
        {
            return;
        }

        if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.NotFighting)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TOKEN_TIP_NOT_FIGHTING);
        }
        else
        {
            UISystem.Instance.ShowGameUI(CaptureTokenView.UIName);
        }
    }

    public override void Uninitialize()
    {
        CaptureTerritoryModule.Instance.CampaignTimerUpdate -= SetTitle;
        CaptureTerritoryModule.Instance.FightCDTimerUpdate -= SetFightCD;
        Assets.Script.Common.Scheduler.Instance.RemoveLateUpdator(LateUpdateFixPanel);
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_Rank.gameObject).onClick = ButtonEvent_Rank;
        UIEventListener.Get(view.Btn_Rule.gameObject).onClick = ButtonEvent_Rule;
        UIEventListener.Get(view.Btn_Allocation.gameObject).onClick = ButtonEvent_Allocation;
        UIEventListener.Get(view.Btn_AddScale.gameObject).onClick = ButtonEvent_AddScale;
        UIEventListener.Get(view.Btn_ReduceScale.gameObject).onClick = ButtonEvent_ReduceScale;
        UIEventListener.Get(view.Spt_TokenBG.gameObject).onClick = ButtonEvent_Token;
    }
}
