using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
public class PropsPackageCommonData
{
    public int quality;
    public int level = 0;
    public string icon;
    public int star = 0;
    public string name;
    public int num;
    public uint id;
    public bool isEquipChip = false;
    public bool isSoldierChip = false;
    public bool isSoldier = false;
    public static PropsPackageCommonData SoldierDataToThis(SoldierAttributeInfo vData)
    {
        PropsPackageCommonData tmpData = new PropsPackageCommonData();
        tmpData.quality = vData.quality;
        tmpData.level = vData.baseLevel;
        tmpData.icon = vData.Icon;
        tmpData.star = vData.Star;
        tmpData.name = vData.Name;
        tmpData.id = vData.id;
        tmpData.isSoldier = true;
        return tmpData;
    }
    public static PropsPackageCommonData EquipDataToThis(EquipAttributeInfo vData)
    {
        PropsPackageCommonData tmpData = new PropsPackageCommonData();
        tmpData.quality = vData.quality;
        tmpData.level = vData.baseLevel;
        tmpData.icon = vData.icon;
        tmpData.star = vData.star;
        tmpData.name = vData.name;
        tmpData.id = vData.id;
        return tmpData;
    }
    public static PropsPackageCommonData ItemDataToThis(ItemInfo vData)
    {
        PropsPackageCommonData tmpData = new PropsPackageCommonData();
        tmpData.quality = vData.quality;
        tmpData.level = 0;
        tmpData.icon = vData.icon;
        tmpData.star = 0;
        tmpData.name = vData.name;
        tmpData.id = vData.id;
        tmpData.isEquipChip = (vData.type == (int)ItemTypeEnum.EquipChip);
        tmpData.isSoldierChip = (vData.type == (int)ItemTypeEnum.SoldierChip);
        return tmpData;
    }
    public static PropsPackageCommonData DataToThisById(uint vId)
    {
        PropsPackageCommonData tmpData = new PropsPackageCommonData();
        tmpData.id = vId;
        IDType type = CommonFunction.GetTypeOfID(vId.ToString());
        switch (type)
        {
            case IDType.Gold:
                {
                    tmpData.name = ConstString.NAME_GOLD;
                    tmpData.icon = GlobalConst.SpriteName.Gold;
                    tmpData.quality = (int)ItemQualityEnum.White;
                    break;
                }
            case IDType.Diamond:
                {
                    tmpData.name = ConstString.NAME_DIAMOND;
                    tmpData.icon = GlobalConst.SpriteName.Diamond;
                    tmpData.quality = (int)ItemQualityEnum.White;
                    break;
                }
            case IDType.SP:
                {
                    tmpData.name = ConstString.NAME_SP;
                    tmpData.icon = GlobalConst.SpriteName.SP;
                    tmpData.quality = (int)ItemQualityEnum.White;
                    break;
                }
            case IDType.Honor:
                {
                    tmpData.name = ConstString.NAME_HONOR;
                    tmpData.icon = GlobalConst.SpriteName.Honor;
                    tmpData.quality = (int)ItemQualityEnum.White;
                    break;
                }
            case IDType.Medal:
                {
                    tmpData.name = ConstString.NAME_MEDAL;
                    tmpData.icon = GlobalConst.SpriteName.Medal;
                    tmpData.quality = (int)ItemQualityEnum.White;
                    break;
                }
            case IDType.Prop:
                {
                    tmpData = ItemDataToThis(ConfigManager.Instance.mItemData.GetItemInfoByID(vId));
                    break;
                }
            case IDType.Soldier:
                {
                    tmpData = SoldierDataToThis(ConfigManager.Instance.mSoldierData.FindById(vId));
                    break;
                }
            case IDType.EQ:
                {
                    tmpData = EquipDataToThis(ConfigManager.Instance.mEquipData.FindById(vId));
                    break;
                }
            case IDType.UnionToken:
                {
                    tmpData.name = ConstString.NAME_UNIONTOKEN;
                    tmpData.icon = GlobalConst.SpriteName.UnionToken;
                    tmpData.quality = (int)ItemQualityEnum.White;
                    break;
                }
            case IDType.RecycleCoin:
                {
                    tmpData.name = ConstString.NAME_RECYCLECOIN;
                    tmpData.icon = GlobalConst.SpriteName.RecycleCoin;
                    tmpData.quality = (int)ItemQualityEnum.White;
                    break;
                }
            case IDType.Exp:
                {
                    tmpData.name = ConstString.NAME_EXP;
                    tmpData.icon = GlobalConst.SpriteName.HeroExp;
                    tmpData.quality = (int)ItemQualityEnum.White;
                    break;
                }
            default:
                {
                    tmpData = null;
                    break;
                }
        }
        return tmpData;
    }
}
public class SoldierData
{
    public SoldierAttributeInfo info;
    public int count;
}
public class SoldierPropsPackageViewController : UIBase
{
    public SoldierPropsPackageView view;
    private List<SoldierPropsPackageComponent> soldier_dic;

    private ItemInfo itemInfo;
    private List<PropsPackageCommonData> dataList;
    private List<ID2Num> selectedList = null;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new SoldierPropsPackageView();
            view.Initialize();
        }
        if (soldier_dic == null)
            soldier_dic = new List<SoldierPropsPackageComponent>();
        if (selectedList == null)
            selectedList = new List<ID2Num>();
        if (dataList == null)
        {
            dataList = new List<PropsPackageCommonData>();
        }
        BtnEventBinding();
        
        view.UIWrapContent_ItemGrid.onInitializeItem = UpdateWrapSoldierInfo;
    }

    #region Update Event

    private void UpdateWrapSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_ItemGrid.enabled == false) return;
        if (realIndex >= dataList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
            go.name = realIndex.ToString();
            SoldierPropsPackageComponent comp = soldier_dic[wrapIndex];
            PropsPackageCommonData _info = dataList[realIndex];
            comp.UpdateCompInfo(_info);
            comp.IsSelect = IsSelectReadySoldier(_info.id);
        }
    }

    public void UpdateViewInfo(ItemInfo info)
    {
        itemInfo = info;
        if (info == null)
            return;
        List<Prop_value> list = itemInfo.proplist;
        if (list == null)
            return;
        ProplistToDataList(list,itemInfo.use_type);
        Main.Instance.StartCoroutine(UpdateSoldierNum());
    }
    private void ProplistToDataList(List<Prop_value> list, int use_type)
    {
        if (list == null)
            return;
        this.view.Lbl_Title.text = ConstString.SOLDIERSELECT_PROP;
        for (int i = 0; i < list.Count; i++)
        {
            Prop_value prop = list[i];
            if (prop == null)
                continue;
            PropsPackageCommonData tmpData = PropsPackageCommonData.DataToThisById(prop.id);
            if (tmpData != null)
            {
                tmpData.num = prop.count;
                dataList.Add(tmpData);
            }
        }
        return;
    }
    private IEnumerator UpdateSoldierNum()
    {
        int MAXCOUNT = 12;
        int count = dataList.Count;
        int itemCount = soldier_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_ItemGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_ItemGrid.minIndex = -index;
        view.UIWrapContent_ItemGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_ItemGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_ItemGrid.enabled = false;
        }
        yield return null;
        if (count < itemCount)
        {
            for (int i = count; i < itemCount; i++)
            {
                SoldierPropsPackageComponent comp = soldier_dic[i];
                if (comp != null && comp.mRootObject != null)
                {
                    if (comp.mRootObject.activeSelf)
                    {
                        comp.mRootObject.SetActive(false);
                    }
                }
            }
        }
        for (int i = 0; i < count; i++)
        {
            PropsPackageCommonData data = dataList[i];
            if (data == null)
                continue;
            SoldierPropsPackageComponent comp = null;
            if (i < itemCount)
            {
                comp = soldier_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_OwnSoldierComp, view.Grd_ItemGrid.transform);
                comp = new SoldierPropsPackageComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_OwnSoldier);
                soldier_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.IsSelect = false;
            comp.UpdateCompInfo(data);
        }
        yield return null;
        view.UIWrapContent_ItemGrid.ReGetChild();
        yield return null;
        view.Grd_ItemGrid.Reposition();
        yield return null;
        view.ScrView_ScorllView.ResetPosition();
    }

    private void UpdateSelectedSoldiersData(ID2Num info, bool status)
    {
        if (info == null)
            return;
        if (selectedList == null)
            selectedList = new List<ID2Num>();
        selectedList.Clear();  //目前仅允许选择单个
        ID2Num data = selectedList.Find((tmp) =>
          {
              if (tmp == null) return false;
              return tmp.id == info.id;
          });
        if (status)
        {
            if (data != null)
                return;
            selectedList.Add(info);
        }
        else
        {
            if (data != null)
            {
                selectedList.Remove(data);
            }
        }
    }

    private void UpdateSelectedSoldierStatus()
    {
        for (int i = 0; i < soldier_dic.Count; i++)
        {
            SoldierPropsPackageComponent comp = soldier_dic[i];
            comp.IsSelect = IsSelectReadySoldier(comp._PropsPackageInfo.id);
        }
    }
    #endregion
    #region Button Event
    public void ButtonEvent_SummonButton(GameObject btn)
    {
        if (selectedList == null || selectedList.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_TIP_SELECTSOLDIER);
            return;
        }
        BackPackModule.Instance.SendUseItem(itemInfo.id, 1, selectedList);
        CloseView();
    }

    public void ButtonEvent_CloseButton(GameObject go)
    {
        CloseView();
    }

    public void ButtonEvent_OwnSoldier(BaseComponent baseComp)
    {
        SoldierPropsPackageComponent comp = baseComp as SoldierPropsPackageComponent;
        if (comp == null)
            return;
        if (comp.IsSelect)
            return;
        ID2Num data = new ID2Num();
        data.id = (int)comp._PropsPackageInfo.id;
        data.num = comp._PropsPackageInfo.num;
        UpdateSelectedSoldiersData(data, true);
        UpdateSelectedSoldierStatus();
    }

    #endregion

    private bool IsSelectReadySoldier(uint id)
    {
        for (int i = 0; i < selectedList.Count; i++)
        {
            ID2Num data = selectedList[i];
            if (data == null)
                continue;
            if (id == data.id)
                return true;
        }
        return false;
    }

    private void CloseView()
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SOLDIERPROPSPACKAGEVIEW);
    }

    public override void Uninitialize()
    {
        selectedList.Clear();
        dataList.Clear();
        itemInfo = null;
        Main.Instance.StopCoroutine(UpdateSoldierNum());
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        soldier_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_SummonButton.gameObject).onClick = ButtonEvent_SummonButton;
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_CloseButton;
    }
}
