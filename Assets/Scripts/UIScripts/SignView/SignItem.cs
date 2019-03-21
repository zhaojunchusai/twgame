using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class SignItem : MonoBehaviour
{
    [HideInInspector]
    public UISprite Spt_BG;
    [HideInInspector]
    public UISprite Spt_BG1;
    [HideInInspector]
    public UILabel Lbl_Num;
    [HideInInspector]
    public UISprite Spt_PropBG;
    [HideInInspector]
    public UISprite Spt_Quality;
    [HideInInspector]
    public UISprite Spt_Icon;
    [HideInInspector]
    public UISprite Spt_ChipMark;
    [HideInInspector]
    public UISprite Spt_GotTip;
    [HideInInspector]
    public GameObject Spt_CanGet;
    [HideInInspector]
    public UILabel Lbl_Day;
    public LoginAwardData _data;
    private CommonItemData _itemData = new CommonItemData();
    private bool _initialized = false;
    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;

        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        //Spt_BG1 = transform.FindChild("BG1").gameObject.GetComponent<UISprite>();
        Lbl_Num = transform.FindChild("Num").gameObject.GetComponent<UILabel>();
        Spt_PropBG = transform.FindChild("Prop/PropBG").gameObject.GetComponent<UISprite>();
        Spt_Quality = transform.FindChild("Prop/Quality").gameObject.GetComponent<UISprite>();
        Spt_ChipMark = transform.FindChild("Prop/Mark").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Prop/Icon").gameObject.GetComponent<UISprite>();
        Spt_GotTip = transform.FindChild("GotTip").gameObject.GetComponent<UISprite>();
        Spt_CanGet = transform.FindChild("CanGet").gameObject;
        Lbl_Day = transform.FindChild("Day").gameObject.GetComponent<UILabel>();
        //SetLabelValues();

    }

    public void InitItem(LoginAwardData data)
    {
        Initialize();
        _data = data;
        Lbl_Num.text = string.Format(ConstString.FORMAT_NUM_X, CommonFunction.GetTenThousandUnit((int)data.AwardNum, 10000));
        if (data.Type == LoginAwardConfig.ELoginAwardType.Continuous)
        {
            if (data.DayCount == 7)
            {
                Lbl_Day.text = ConstString.FORMAT_CONTINUOUS_LOGIN_DAY_7;
            }
            else
            {
                Lbl_Day.text = string.Format(ConstString.FORMAT_CONTINUOUS_LOGIN_DAY, data.DayCount);
            }
        }
        else
        {
            Lbl_Day.text = string.Format(ConstString.FORMAT_TOTAL_LOGIN_DAY, data.DayCount);
        }
        Spt_ChipMark.gameObject.SetActive(false);
        _itemData = new CommonItemData(data.AwardItemID, 0, true);
        if (_itemData.Type == IDType.Prop)
        {
            CommonFunction.SetChipMark(Spt_ChipMark, _itemData.SubType, new Vector3(-28, 25, 0), new Vector3(-27, 29, 0));
        }
        else if (_itemData.Type == IDType.LifeSoul)
        {
            
            CommonFunction.SetLifeSoulMark(Spt_ChipMark, _itemData.LifeSoulGodEquip, new Vector3(28, 29, 0));
        }
        #region old
        /*
        switch (data.AwardType)
        {
            case LoginAwardConfig.EAwardType.SpecialItem:
                {
                    IDType type = CommonFunction.GetTypeOfID(data.AwardItemID.ToString());
                    switch (type)
                    {
                        case IDType.Gold:
                            {
                                _itemData = new CommonItemData(GlobalCoefficient.CoinID,
                                    ConstString.NAME_GOLD,
                                    ItemQualityEnum.White,
                                    (int)data.AwardNum,
                                    GlobalConst.SpriteName.Gold_L,
                                    type,
                                    ConstString.DESC_GOLD);
                                break;
                            }
                        case IDType.Diamond:
                            {
                                _itemData = new CommonItemData(GlobalCoefficient.GemID,
                                    ConstString.NAME_DIAMOND,
                                    ItemQualityEnum.White,
                                    (int)data.AwardNum,
                                    GlobalConst.SpriteName.Diamond_L,
                                    type,
                                    ConstString.DESC_DIAMOND);
                                break;
                            }
                        case IDType.SP:
                            {
                                _itemData = new CommonItemData(GlobalCoefficient.SpID,
                                    ConstString.NAME_SP,
                                    ItemQualityEnum.White,
                                    (int)data.AwardNum,
                                    GlobalConst.SpriteName.SP_L,
                                    type,
                                    ConstString.DESC_SP);
                                break;
                            }
                        case IDType.Honor:
                            {
                                _itemData = new CommonItemData(GlobalCoefficient.HonorID,
                                    ConstString.NAME_HONOR,
                                    ItemQualityEnum.White,
                                    (int)data.AwardNum,
                                    GlobalConst.SpriteName.Honor,
                                    type,
                                    ConstString.DESC_HONOR);
                                break;
                            }
                        case IDType.Medal:
                            {
                                _itemData = new CommonItemData(GlobalCoefficient.MedalID,
                                    ConstString.NAME_MEDAL,
                                    ItemQualityEnum.White,
                                    (int)data.AwardNum,
                                    GlobalConst.SpriteName.Medal,
                                    type,
                                    ConstString.DESC_MEDAL);
                                break;
                            }
                    }
                    break;
                }
            case LoginAwardConfig.EAwardType.Equip:
                {
                    EquipAttributeInfo tmpEquip = ConfigManager.Instance.mEquipData.FindById(data.AwardItemID);
                    _itemData = new CommonItemData(tmpEquip);
                    break;
                }
            case LoginAwardConfig.EAwardType.Prop:
                {
                    ItemInfo tmpItem = ConfigManager.Instance.mItemData.GetItemInfoByID(data.AwardItemID);
                    _itemData = new CommonItemData(tmpItem);
                    CommonFunction.SetChipMark(Spt_ChipMark, (ItemTypeEnum)tmpItem.type, new Vector3(-28, 25, 0), new Vector3(-27, 29, 0));
                    break;
                }
            case LoginAwardConfig.EAwardType.Soldier:
                {
                    SoldierAttributeInfo info = ConfigManager.Instance.mSoldierData.FindById(data.AwardItemID);
                    _itemData = new CommonItemData(info);
                    break;
                }
        }*/
        #endregion
        _itemData.Num = (int)data.AwardNum;
        CommonFunction.SetSpriteName(Spt_Icon, _itemData.Icon);
        CommonFunction.SetQualitySprite(Spt_Quality, _itemData.Quality, Spt_PropBG);
        RefreshSignState();
    }

    public void RefreshSignState()
    {
        if (_data.Type == LoginAwardConfig.ELoginAwardType.Continuous)
        {
            int daycount = Math.Min(PlayerData.Instance.LoginChangeInfo.continu_num, 7);

            if (_data.DayCount == daycount)
            {
                if (PlayerData.Instance.LoginChangeInfo.get_continu_award == (int)ELoginAwardGetContinous.Got)
                {
                    Spt_GotTip.gameObject.SetActive(true);
                    Spt_CanGet.gameObject.SetActive(false);
                }
                else
                {
                    Spt_GotTip.gameObject.SetActive(false);
                    Spt_CanGet.gameObject.SetActive(true);
                }
            }
            else
            {
                Spt_GotTip.gameObject.SetActive(false);
                Spt_CanGet.gameObject.SetActive(false);
                if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
                {
                    if (PlayerData.Instance.LoginChangeInfo.has_got_continue_award != null)
                    {
                        if (PlayerData.Instance.LoginChangeInfo.has_got_continue_award.Contains((int)_data.DayCount))
                        {
                            Spt_GotTip.gameObject.SetActive(true);
                            Spt_CanGet.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            if (_data.DayCount <= PlayerData.Instance.LoginChangeInfo.cumulative_num)
            {
                if (PlayerData.Instance.LoginChangeInfo.get_cumulative_award.Contains((int)_data.DayCount))
                {
                    Spt_GotTip.gameObject.SetActive(true);
                    Spt_CanGet.gameObject.SetActive(false);
                }
                else
                {
                    Spt_GotTip.gameObject.SetActive(false);
                    Spt_CanGet.gameObject.SetActive(true);
                }
            }
            else
            {
                Spt_GotTip.gameObject.SetActive(false);
                Spt_CanGet.gameObject.SetActive(false);
            }
        }

        if (Spt_CanGet.gameObject.activeSelf)
        {
            UIEventListener.Get(Spt_BG.gameObject).onClick = ClickItem;
            UIEventListener.Get(Spt_BG.gameObject).onPress = null;
        }
        else
        {
            UIEventListener.Get(Spt_BG.gameObject).onPress = PressItem;
            UIEventListener.Get(Spt_BG.gameObject).onClick = null;
        }
    }

    private void ClickItem(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Get_Reward, gameObject.transform));
        List<CommonItemData> data = new List<CommonItemData>();
        data.Add(_itemData);
        if (CommonFunction.GetItemOverflowTip(data))
        {
            return;
        }
        if (_data.Type == LoginAwardConfig.ELoginAwardType.Continuous)
        {
            TaskModule.Instance.ContinuAwardReq();
        }
        else
        {
            TaskModule.Instance.CumulativeAwardReq((int)_data.DayCount);
        }
    }

    private void PressItem(GameObject go, bool press)
    {
        HintManager.Instance.SeeDetail(go, press, _data.AwardItemID);
    }

    private void SetLabelValues()
    {
        Lbl_Num.text = "";
        Lbl_Day.text = "";
    }

    public void Uninitialize()
    {

    }


}
