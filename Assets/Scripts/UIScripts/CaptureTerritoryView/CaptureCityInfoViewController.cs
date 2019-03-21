using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class CaptureCityInfoViewController : UIBase 
{
    public CaptureCityInfoView view;
    private CaptureCityData _cityData;
    private CommonItemData[] _awardList = new CommonItemData[6];
    private CaptureScoreItem _myScoreItem = null;
    private CampaignRankInfo _myScoreInfo = null;
    private CampaignRankInfo _myUnionScoreInfo = null;
    private List<CaptureScoreItem> _scoreItemList = new List<CaptureScoreItem>();
    private List<CampaignRankInfo> _rankInfoList;
    private const int COUNT_PER_PAGE = 15;    
    private int _page = 0;
    private bool _fightCDAutoBuy = false;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new CaptureCityInfoView();
            view.Initialize();
            BtnEventBinding();            
        }
        _myScoreInfo = CaptureTerritoryModule.Instance.MyPersonRankInfo;
        _myUnionScoreInfo = CaptureTerritoryModule.Instance.MyUnionRankInfo;
        view.Scl_ScoreItems.onDragFinishedDown = ReqNewPage;
        if (_myScoreItem == null)
            _myScoreItem = view.Gobj_MyScoreItem.AddComponent<CaptureScoreItem>();
        _cityData = CaptureTerritoryModule.Instance.CurrentCity;

        CommonFunction.SetSpriteName(view.Spt_Selected, GlobalConst.SpriteName.CaptureRankSelectSpt);
        CaptureTerritoryModule.Instance.FightCDTimerUpdate += SetFightTimer;
        SetTitle();
        SetCityAwardDesc();
        SetBoxs();
        SetFightTimer();
        ButtonEvent_UnionRank(null);
    }
        
    private void SetTitle()
    {
        if (_cityData.mIsCapital)
        {
            view.Gobj_TitleHigh.SetActive(true);
            view.Gobj_TitleLow.SetActive(false);
        }
        else
        {
            view.Gobj_TitleHigh.SetActive(false);
            view.Gobj_TitleLow.SetActive(true);

        }
        view.Lbl_TitleLb.text = _cityData.mName;
        view.Lbl_BoxAwardTitle.text = _cityData.mBoxAwardTitle;
    }

    private void SetCityAwardDesc() 
    {
        view.Lbl_AwardDesc.text = _cityData.mCityAwardDesc;
    }

    private void SetBoxs()
    {
        //根据配置表给_awardList复制  没有赋值则为空   
        List<CommonItemData> itemList = CommonFunction.GetCommonItemDataList(_cityData.mAwardDropID);

        for (int i = 0; i < 3; i++)
        {
            if (i < itemList.Count)
            {
                _awardList[i] = itemList[i];
            }
            else
            {
                _awardList[i] = null;
            }
            SetSingleBox(i, _awardList[i]);
        }
            
        itemList = _cityData.mBoxDrops;
        for (int i = 0; i < 3; i++)
        {
            if (i < itemList.Count)
            {
                _awardList[i + 3] = itemList[i];
            }
            else
            {
                _awardList[i + 3] = null;
            }
            SetSingleBox(i + 3, _awardList[i + 3]);
        }
    }

    private void SetSingleBox(int index,CommonItemData item)
    {
        if (index >= 6)
            return;

        if (item == null)
        {
            view.Gobj_Props[index].SetActive(false);
            return;
        }

        view.Gobj_Props[index].SetActive(true);
        CommonFunction.SetSpriteName(view.Spt_Icons[index], item.Icon);
        CommonFunction.SetQualitySprite(view.Spt_Qualitys[index], item.Quality);
        view.Lbl_Nums[index].text = string.Format(ConstString.FORMAT_NUM_X, item.Num);
        if (item.Type == IDType.Prop && item.SubType == ItemTypeEnum.SoldierChip)
        {
            CommonFunction.SetSpriteName(view.Spt_Marks[index], GlobalConst.SpriteName.MarkSoldierChip);
            view.Spt_Marks[index].gameObject.SetActive(true);
            view.Spt_Marks[index].width = 32;
            view.Spt_Marks[index].height = 28;
            view.Spt_Marks[index].transform.localPosition = new Vector3(-27, 29, 0);
        }
        else if (item.Type == IDType.Prop && item.SubType == ItemTypeEnum.EquipChip)
        {
            CommonFunction.SetSpriteName(view.Spt_Marks[index], GlobalConst.SpriteName.MarkEquipChip);
            view.Spt_Marks[index].gameObject.SetActive(true);
            view.Spt_Marks[index].width = 30;
            view.Spt_Marks[index].height = 36;
            view.Spt_Marks[index].transform.localPosition = new Vector3(-28, 25, 0);
        }
        else
        {
            view.Spt_Marks[index].gameObject.SetActive(false);
        }
    }

    private void InitRankContent(List<CampaignRankInfo> list, int page)
    {
        if (list == null )
            return;

        _page = page;

        if (page == 1)
            view.Scl_ScoreItems.ResetPosition();

        int startIndex = COUNT_PER_PAGE * (page - 1);
        int endIndex = Mathf.Min(list.Count, COUNT_PER_PAGE * page);

        for (int i = startIndex; i < endIndex; i++)
        {
            if (i >= _scoreItemList.Count)
            {
                CaptureScoreItem unionItem = CommonFunction.InstantiateItem<CaptureScoreItem>(view.Gobj_CaptureScoreItem, view.Grd_Grid.transform);
                _scoreItemList.Add(unionItem);
            }
            _scoreItemList[i].gameObject.SetActive(true);
            _scoreItemList[i].gameObject.name = string.Format("item{0:D3}", i);
            _scoreItemList[i].Init(list[i], i +1);
        }

        for (int i = endIndex; i < _scoreItemList.Count; i++)
        {
            _scoreItemList[i].gameObject.SetActive(false);
        }

        view.Grd_Grid.Reposition();
    }

    private void GetMyScoreInfo()
    {
        view.Lbl_TitleName.text = ConstString.SUB_TITLE_MEMBER_NAME;
        _rankInfoList = CaptureTerritoryModule.Instance.PersonalRank;
        _myScoreItem.Init(_myScoreInfo, _myScoreInfo.rank);
    }

    private void GetMyUnionScoreInfo()
    {
        view.Lbl_TitleName.text = ConstString.SUB_TITLE_UNION_NAME;
        _rankInfoList = CaptureTerritoryModule.Instance.UnionRank;
        _myScoreItem.Init(_myUnionScoreInfo, _myUnionScoreInfo.rank);
    }
    private void ReqNewPage()
    {
        InitRankContent(_rankInfoList, _page + 1);
    }
    private void SetFightTimer()
    {
        if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting
            && CaptureTerritoryModule.Instance.FightCDTimer > 0)
        {
            view.Lbl_FightCDTip.text = string.Format(ConstString.CAPTURETERRITORY_FIGHT_TIMER,
                CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.FightCDTimer));

            view.Lbl_BtnFightLabel.text = ConstString.CAPTURETERRITORY_FIGHT_BTN_NOW;
        }
        else
        {
            view.Lbl_BtnFightLabel.text = ConstString.CAPTURETERRITORY_FIGHT_BTN;
            view.Lbl_FightCDTip.text = "";
        }
    }
    private void SetSelectedSpt(bool isPersonRank)
    {
        view.Spt_Selected.transform.localPosition = isPersonRank ? new Vector3(-84, -200, 0) : new Vector3(-249, -200, 0);
    }

    private void ShowBuyFightCD()
    {
        if (_fightCDAutoBuy)
        {
            CaptureTerritoryModule.Instance.SendClearCampaignCD();
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo_Mark, 
                string.Format(ConstString.CAPTURETERRITORY_FIGHT_BUY_TIP,ConfigManager.Instance.mCaptureTerritoryConfig.FightCDCost),
           CaptureTerritoryModule.Instance.SendClearCampaignCD,
           null, ConstString.CAPTURETERRITORY_FIGHT_BTN, "", BuyFightCDMarkCallback, false);
        }
    }
    public void ShowCityFightInfoView()
    {
        if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.NotFighting)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TIP_NOT_FIGHTING);
            return;
        }

        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CAPTURE_TERRITORY_INFO);
        StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID(_cityData.mStageID);
        bool hasUnionRank = CaptureTerritoryModule.Instance.UnionRank.Count >0 ;
        string bestUnionName = hasUnionRank ? CaptureTerritoryModule.Instance.UnionRank[0].name : ConstString.NOTHING;
        int unionScore = hasUnionRank ? CaptureTerritoryModule.Instance.UnionRank[0].score : 0;
        UISystem.Instance.CaptureTerritoryInfoView.UpdateViewInfo(info, _cityData.mBoxDrops, bestUnionName, unionScore,_myUnionScoreInfo.score);
    }

    private void PressGiftProp(GameObject go, bool press)
    {
        string name = go.transform.parent.gameObject.name;
        //预制名为 prop1
        int index = int.Parse(name.Substring(4, 1));
        if (GiftIsNull(index - 1))
        {
            view.Gobj_Props[index - 1].SetActive(false);
            return;
        }

        HintManager.Instance.SeeDetail(go, press, _awardList[index - 1].ID);
    }
    private bool GiftIsNull(int index)
    {
        return _awardList[index] == null || _awardList[index].ID == 0;
    }

    private void BuyFightCDMarkCallback(bool selected)
    {
        _fightCDAutoBuy = selected;
    }

    public void ButtonEvent_UnionRank(GameObject btn)
    {
        GetMyUnionScoreInfo();
        InitRankContent(_rankInfoList, 1);
        SetSelectedSpt(false);
    }

    public void ButtonEvent_PersonRank(GameObject btn)
    {
        GetMyScoreInfo();
        InitRankContent(_rankInfoList, 1);
        SetSelectedSpt(true);
    }

    public void ButtonEvent_Fight(GameObject btn)
    {
        if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.NotFighting)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TIP_NOT_FIGHTING);
            return;
        }
        if (CaptureTerritoryModule.Instance.FightCDTimer > 0)
        {
            ShowBuyFightCD();
            return;
        }

        ShowCityFightInfoView();
    }
    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(CaptureCityInfoView.UIName);
    }
    
    public override void Uninitialize()
    {
        CaptureTerritoryModule.Instance.FightCDTimerUpdate -= SetFightTimer;
    }

    public override void Destroy()
    {
        view = null;
        _cityData = null;
        _myScoreItem = null;
        _myScoreInfo = null;
        _myUnionScoreInfo = null;
        _scoreItemList.Clear();
        _rankInfoList = null;
        _page = 0;
        _fightCDAutoBuy = false;
        for (int i = 0; i < _awardList.Length; i++)
        {
            _awardList[i] = null;
        }
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_UnionRank.gameObject).onClick = ButtonEvent_UnionRank;
        UIEventListener.Get(view.Btn_PersonRank.gameObject).onClick = ButtonEvent_PersonRank;
        UIEventListener.Get(view.Btn_Fight.gameObject).onClick = ButtonEvent_Fight;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ButtonEvent_Close;
        for (int i = 0; i < view.Spt_PropBGs.Length; i++)
        {
            UIEventListener.Get(view.Spt_PropBGs[i].gameObject).onPress = PressGiftProp;
        }
    }


}
