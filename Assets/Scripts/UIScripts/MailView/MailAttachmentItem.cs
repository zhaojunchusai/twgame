using UnityEngine;
using System.Collections;

public class MailAttachmentItem : MonoBehaviour
{
    private ItemQualityEnum _quality = ItemQualityEnum.White;
    public bool mHideNameLabel;
    [HideInInspector]public UISprite Spt_BGSprite;
    [HideInInspector]public UISprite Spt_QualitySprite;
    [HideInInspector]public UISprite Spt_IconBGSprite;
    [HideInInspector]public UISprite Spt_IconSprite;
    [HideInInspector]public UISprite Spt_ChipMark;
    [HideInInspector]public UILabel Lbl_NumberLabel;
    [HideInInspector]public UILabel Lbl_NameLabel;
    private bool _initialized = false;
    private uint _id;
    public uint ID
    {
        get { return _id; }
    }
    private int _num;
    public int Num
    {
        get { return _num; }
    }

    private CommonItemData _info;

    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;
        Spt_BGSprite = transform.FindChild("BGSprite").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = transform.FindChild("FreamSprite").gameObject.GetComponent<UISprite>();
        Spt_IconBGSprite = transform.FindChild("IconBGSprite").gameObject.GetComponent<UISprite>();
        Spt_IconSprite = transform.FindChild("IconSprite").gameObject.GetComponent<UISprite>();
        Spt_ChipMark = transform.FindChild("FreamSprite/Mark").gameObject.GetComponent<UISprite>();
        Lbl_NumberLabel = transform.FindChild("NumberLabel").gameObject.GetComponent<UILabel>();
       
        Lbl_NameLabel = transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        SetLabelValues();

    }

    void Awake()
    {
        Initialize();
        if (mHideNameLabel)
        {
            Lbl_NameLabel.text = "";
            Lbl_NameLabel.gameObject.SetActive(false);
        }
    }

    public void SetLabelValues()
    {
        Lbl_NumberLabel.text = "";
        Lbl_NameLabel.text = "";
    }

    public void Uninitialize()
    {

    }
    /// <summary>
    /// 更新组件信息 图标 名字 品质 数量，目前支持的类型：金币 钻石 体力 士兵 道具 装备
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    public void UpdateItemInfo(uint id, int num,bool isDisName = true)
    {
        Initialize();
        _id = id;
        _num = num;
        IDType type = CommonFunction.GetTypeOfID(id.ToString());
        Spt_ChipMark.gameObject.SetActive(false);
        CommonItemData data = new CommonItemData(id, num, true, false);
        CommonFunction.SetSpriteName(Spt_IconSprite, data.Icon);
        UpdateItemQuality((ItemQualityEnum)data.Quality);
        UpdateItemName(data.Name);
        UpdateItemNum(num);
        if (data.Type == IDType.Prop)
        {
            CommonFunction.SetChipMark(Spt_ChipMark, data.SubType, new Vector3(-25, 25, 0), new Vector3(-25, 27, 0));
        }
        else if (data.Type == IDType.LifeSoul)
        {
            CommonFunction.SetLifeSoulMark(Spt_ChipMark, data.LifeSoulGodEquip, new Vector3(26, 26, 0));
        }
        #region old
        /*
        switch (type)
        {
            case IDType.Gold:
                UpdateItemNum(num);
                UpdateItemName(ConstString.NAME_GOLD);
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.Gold_L);
                UpdateItemQuality(_quality);
                break;
            case IDType.Diamond:
                UpdateItemNum(num);
                UpdateItemName(ConstString.NAME_DIAMOND);
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.Diamond_L);
                UpdateItemQuality(_quality);
                break;
            case IDType.Medal:
                UpdateItemNum(num);
                UpdateItemName(ConstString.NAME_MEDAL);
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.Medal);
                UpdateItemQuality(_quality);
                break;
            case IDType.Honor:
                UpdateItemNum(num);
                UpdateItemName(ConstString.NAME_HONOR);
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.Honor);
                UpdateItemQuality(_quality);
                break;
            case IDType.Exp:
                UpdateItemNum(num);
                UpdateItemName(ConstString.NAME_EXP);
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.HeroExp);
                UpdateItemQuality(_quality);
                break;
            case IDType.SoldierExp:
                UpdateItemNum(num);
                UpdateItemName("");
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.SoldierExp);
                UpdateItemQuality(_quality);
                break;
            case IDType.SP:
                UpdateItemNum(num);
                UpdateItemName(ConstString.NAME_SP);
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.SP_L);
                UpdateItemQuality(_quality);
                break;
            case IDType.UnionToken:
                UpdateItemNum(num);
                UpdateItemName(ConstString.NAME_UNIONTOKEN);
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.UnionToken);
                UpdateItemQuality(_quality);
                break;
            case IDType.Soldier:
                SoldierAttributeInfo soldier = ConfigManager.Instance.mSoldierData.FindById(id);
                if (soldier == null)
                {
                    DebugUtil.LogError("Cannt find soldier !!! that id = " + id);
                    ClearItemInfo();
                    UpdateItemName("Soldier " + id);
                    return;
                }
                CommonFunction.SetSpriteName(Spt_IconSprite, soldier.Icon);
                UpdateItemQuality((ItemQualityEnum)soldier.quality);
                UpdateItemName(soldier.Name);
                UpdateItemNum(num);
                break;
            case IDType.Prop:
                ItemInfo item = ConfigManager.Instance.mItemData.GetItemInfoByID(id);
                if (item == null)
                {
                    DebugUtil.LogError("Cannt find item info !!! that id = " + id);
                    ClearItemInfo();
                    UpdateItemName("Prop "+ id);
                    return;
                }
                CommonFunction.SetSpriteName(Spt_IconSprite, item.icon);
                UpdateItemQuality((ItemQualityEnum)item.quality);
                UpdateItemName(item.name);
                UpdateItemNum(num);
                CommonFunction.SetChipMark(Spt_ChipMark, (ItemTypeEnum)item.type, new Vector3(-25, 25, 0), new Vector3(-25, 27, 0));
                break;
            case IDType.EQ:
                EquipAttributeInfo equip = ConfigManager.Instance.mEquipData.FindById(id);
                if (equip == null)
                {
                    DebugUtil.LogError("Cannt find equip info !!! that id = " + id);
                    ClearItemInfo();
                    UpdateItemName("Equip " + id);
                    return;
                }
                CommonFunction.SetSpriteName(Spt_IconSprite, equip.icon);
                UpdateItemQuality((ItemQualityEnum)equip.quality);
                UpdateItemName(equip.name);
                UpdateItemNum(num);
                break;

            default:
                DebugUtil.LogError("Cannt find any info !!! that id = " + id);
                ClearItemInfo();
                UpdateItemName("? " + id);
                break;
        }
        */
        #endregion
        if (!isDisName)
            UpdateItemName("");
    }

    public void UpdateItemNum(int number)
    {
        if (number <= 0)
            Lbl_NumberLabel.text = "";
        else
            Lbl_NumberLabel.text = string.Format(ConstString.FORMAT_NUM_X, CommonFunction.GetTenThousandUnit(number, 10000));
    }

    public void UpdateIconSprite(string spName)
    {
        CommonFunction.SetSpriteName(Spt_IconSprite, spName);
    }

    public void UpdateItemQuality(ItemQualityEnum quality)
    {
        CommonFunction.SetQualitySprite(Spt_QualitySprite, quality,Spt_IconBGSprite);
    }

    public void UpdateItemName(string str)
    {
        if (mHideNameLabel)
            Lbl_NameLabel.text = "";
        else
            Lbl_NameLabel.text = str;
    }

    public void ClearItemInfo()
    {
        _id = 0;
        UpdateItemQuality(_quality);
        UpdateItemNum(0);
        UpdateItemName("");
        CommonFunction.SetSpriteName(Spt_IconSprite,GlobalConst.SpriteName.QUESTION);
        Spt_ChipMark.gameObject.SetActive(false);
    }
}
