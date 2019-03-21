using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
/// <summary>
/// 主线副本关卡选择按钮组件
/// </summary>
public class CrusadeStageButtonComponent : BaseComponent
{
    /// <summary>
    /// 普通小关卡
    /// </summary>
    private Transform ui_NormalGroup;
    /// <summary>
    /// BOSS关卡
    /// </summary>
    private Transform ui_BossGroup;
    /// <summary>
    /// BOSS关卡ICON
    /// </summary>
    private UISprite ui_BossIcon;
    private UISprite ui_BgTexture;
    private UISprite ui_BgSprite;
    ///// <summary>
    ///// 当前关卡标识
    ///// </summary>
    //private Transform ui_IndicateGroup;
    /// <summary>
    /// 小关卡Sprite
    /// </summary>
    private UISprite ui_NormalSprite;
    /// <summary>
    /// BOSS关卡星星组
    /// </summary>
    private UIGrid ui_GateStarGroup;
    /// <summary>
    /// BOSS关卡评级模板
    /// </summary>
    private UISprite ui_GateStarSprite;

    private List<GameObject> starlist;

    private bool isEnable = false;
    public bool IsEnable
    {
        get
        {
            return isEnable;
        }
    }


    private bool isActive = false;
    /// <summary>
    /// 当前关卡是否激活
    /// </summary>
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
            if (!isActive)
                isEnable = false;
            if (ui_NormalGroup == null)
                FindChildProperty();
            if (stageData.stageinfo == null)
            {
                Clear();
                return;
            }
            if (ui_NormalGroup.gameObject.activeSelf == stageData.stageinfo.IsBoss)
                ui_NormalGroup.gameObject.SetActive(!stageData.stageinfo.IsBoss);
            if (ui_BossGroup.gameObject.activeSelf == !stageData.stageinfo.IsBoss)
                ui_BossGroup.gameObject.SetActive(stageData.stageinfo.IsBoss);
            if (stageData.stageinfo.IsBoss)
            {
                if (value)
                {
                    isEnable = true;
                    ui_BgSprite.color = Color.white;
                    //CommonFunction.UpdateTextureGray(ui_BossIcon, false);
                    CommonFunction.UpdateWidgetGray(ui_BgTexture, false);
                    CommonFunction.UpdateWidgetGray(ui_BossIcon, false);
                }
                else
                {
                    isEnable = false;
                    ui_BgSprite.color = Color.black;
                    //CommonFunction.UpdateTextureGray(ui_BossIcon, true);
                    CommonFunction.UpdateWidgetGray(ui_BgTexture, true);
                    CommonFunction.UpdateWidgetGray(ui_BossIcon, true);
                }
                //ui_IndicateGroup.localPosition = new Vector3(0, 70, 0);
            }
            else
            {
                //ui_IndicateGroup.localPosition = new Vector3(0, 40, 0);
                if (stageData.gateinfo != null && stageData.gateinfo.star_level == 0) //说明没有被打过且当前需要被打
                {
                    isEnable = true;
                    if (value)
                    {
                        ui_NormalSprite.color = Color.white;
                    }
                    else
                    {
                        ui_NormalSprite.color = Color.black;
                    }
                }
                else
                {
                    isEnable = false;
                    if (value)
                    {
                        ui_NormalSprite.color = Color.white;
                    }
                    else
                    {
                        ui_NormalSprite.color = Color.black;
                    }
                }
            }
        }
    }
    private bool _isCurrent = false;
    /// <summary>
    /// 是否是当前任务关卡
    /// </summary>
    public bool IsCurrent
    {
        set
        {
            _isCurrent = value;
            if (_isCurrent)
            {
                isEnable = true;
            }
        }
        get
        {
            return _isCurrent;
        }
    }

    private StageData _stagedata;
    public StageData stageData
    {
        get
        {
            return _stagedata;
        }
    }

    public override void MyStart(GameObject root)
    {
        //base.MyStart(root);
        mRootObject = root;
        InitData();
        FindChildProperty();
        IsCurrent = false;
        if (ui_GateStarSprite.gameObject.activeSelf)
            ui_GateStarSprite.gameObject.SetActive(false);
    }

    private void InitData()
    {
        starlist = new List<GameObject>();
    }

    private void FindChildProperty()
    {
        if (mRootObject == null)
        {
            Debug.LogError("mRootObject = null");
            return;
        }

        ui_NormalGroup = mRootObject.transform.FindChild("NormalGroup").gameObject.GetComponent<Transform>();
        ui_NormalSprite = mRootObject.transform.FindChild("NormalGroup/NormalSprite").gameObject.GetComponent<UISprite>();
        ui_BossGroup = mRootObject.transform.FindChild("BossGroup").gameObject.GetComponent<Transform>();
        //ui_IndicateGroup = mRootObject.transform.FindChild("IndicateGroup").gameObject.GetComponent<Transform>();
        ui_BgTexture = mRootObject.transform.FindChild("BossGroup/BgTexture").gameObject.GetComponent<UISprite>();
        ui_BossIcon = mRootObject.transform.FindChild("BossGroup/IconGroup/BossIcon").gameObject.GetComponent<UISprite>();
        ui_GateStarGroup = mRootObject.transform.FindChild("BossGroup/GateStarGroup").gameObject.GetComponent<UIGrid>();
        ui_GateStarSprite = mRootObject.transform.FindChild("BossGroup/GateStarGroup/GateStarSprite").gameObject.GetComponent<UISprite>();
        ui_BgSprite = mRootObject.transform.FindChild("BossGroup/IconGroup/BGSprite").gameObject.GetComponent<UISprite>();
    }

    public void UpdateInfo(StageData data)
    {
        _stagedata = data;
        if (_stagedata == null)
        {
            Clear();
            return;
        }
        if (_stagedata.stageinfo.IsBoss)
        {
            UpdateIcon(_stagedata.stageinfo.BossIcon, _stagedata.stageinfo.BgTexture);
        }
        IsCurrent = false;
        if (_stagedata.gateinfo == null)
        {
            IsActive = false;
            UpdateGrade(0);
        }
        else
        {
            IsActive = true;
            if (_stagedata.gateinfo.star_level == 0)
            {
                IsCurrent = true;
            }
            UpdateGrade(_stagedata.gateinfo.star_level);
        }
    }
    /// <summary>
    /// 更新评级
    /// </summary>
    private void UpdateGrade(uint grade)
    {
        if (grade <= 0)
        {
            ui_GateStarGroup.gameObject.SetActive(false);
        }
        else
        {
            ui_GateStarGroup.gameObject.SetActive(true);
            Main.Instance.StartCoroutine(CreateStars(grade));
        }
    }
    private System.Collections.IEnumerator CreateStars(uint grade)
    {
        if (grade <= starlist.Count)   //如果评级小于已经生成的Gameobject 则不在生成
        {
            for (int i = 0; i < starlist.Count; i++)
            {
                GameObject go = starlist[i];
                if (i < grade)  //评级已约定为123
                {
                    starlist[i].SetActive(true);
                }
                else
                {
                    starlist[i].SetActive(false);
                }
            }
        }
        else
        {
            int index = starlist.Count;
            for (int i = 0; i < starlist.Count; i++)
            {
                starlist[i].SetActive(true);
            }
            for (int i = index; i < grade; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(ui_GateStarSprite.gameObject, ui_GateStarGroup.transform) as GameObject;
                go.name = "index_" + i;
                go.SetActive(true);
                starlist.Add(go);
            }
        }
        yield return null;
        ui_GateStarGroup.repositionNow = true;
    }

    private void UpdateIcon(string icon, string bg)
    {
        CommonFunction.SetSpriteName(ui_BgTexture, bg);
        ui_BgTexture.MakePixelPerfect();
        if (ui_BgSprite != null)
            ui_BgSprite.MakePixelPerfect();
        CommonFunction.SetSpriteName(ui_BossIcon, icon);
    }

    public override void Clear()
    {
        base.Clear();
        ui_BossIcon.spriteName = string.Empty;
        ui_BgTexture.spriteName = string.Empty;
        isEnable = false;
    }
}

public class EliteCrusadeStageBtnComponent : BaseComponent
{
    private UIGrid Grd_GateStarGroup;
    private UISprite Spt_GateStar1;
    private UISprite Spt_GateStar2;
    private UISprite Spt_GateStar3;
    private UISprite Spt_Icon;
    private UILabel Lbl_Name;
    private List<GameObject> starlist;

    private StageData stagedata;
    public StageData StageData
    {
        get
        {
            return stagedata;
        }
    }

    private bool isEnable = false;
    public bool IsEnable
    {
        get
        {
            return isEnable;
        }
    }

    private bool isActive = false;
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
            if (!isActive)
                isEnable = false;
            if (stagedata == null || stagedata.stageinfo == null)
                return;
            if (stagedata.stageinfo.IsBoss == false)  //说明不是boss关卡 且没有被打过且当前需要被打
            {
                if (stagedata.gateinfo.star_level == 0)
                {
                    isEnable = true;
                    CommonFunction.UpdateWidgetGray(Spt_Icon, true);
                }
            }
            else
            {
                isEnable = true;
                CommonFunction.UpdateWidgetGray(Spt_Icon, !isActive);
            }
        }
    }

    private bool isCurrent = false;
    public bool IsCurrent
    {
        get
        {
            return isCurrent;
        }
        set
        {
            isCurrent = value;

        }
    }


    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Grd_GateStarGroup = mRootObject.transform.FindChild("GateStarGroup").GetComponent<UIGrid>();
        Spt_GateStar1 = mRootObject.transform.FindChild("GateStarGroup/GateStarSprite_1").GetComponent<UISprite>();
        Spt_GateStar2 = mRootObject.transform.FindChild("GateStarGroup/GateStarSprite_2").GetComponent<UISprite>();
        Spt_GateStar3 = mRootObject.transform.FindChild("GateStarGroup/GateStarSprite_3").GetComponent<UISprite>();
        Lbl_Name = mRootObject.transform.FindChild("Label").GetComponent<UILabel>();
        Spt_Icon = mRootObject.transform.FindChild("IconSprite").GetComponent<UISprite>();
        if (starlist == null)
        {
            starlist = new List<GameObject>();
            starlist.Add(Spt_GateStar1.gameObject);
            starlist.Add(Spt_GateStar2.gameObject);
            starlist.Add(Spt_GateStar3.gameObject);
        }
        IsCurrent = false;
    }

    public void UpdateInfo(StageData data)
    {
        stagedata = data;
        if (stagedata == null)
        {
            return;
        }
        if (stagedata.stageinfo.IsBoss)
        {
            UpdateIcon(stagedata.stageinfo.BgTexture);
        }
        IsCurrent = false;
        if (stagedata.gateinfo == null)
        {
            IsActive = false;
            UpdateStar(0);
        }
        else
        {
            IsActive = true;
            if (stagedata.gateinfo.star_level == 0)
            {
                IsCurrent = true;
            }
            UpdateStar(stagedata.gateinfo.star_level);
        }
        Lbl_Name.enabled = false;
        Lbl_Name.text = stagedata.stageinfo.Name;
    }

    private void UpdateStar(uint grade)
    {
        for (int i = 0; i < starlist.Count; i++)
        {
            GameObject go = starlist[i];
            if (i < grade)  //评级已约定为123
            {
                starlist[i].SetActive(true);
            }
            else
            {
                starlist[i].SetActive(false);
            }
        }
        Grd_GateStarGroup.repositionNow = true;
    }

    private void UpdateIcon(string icon)
    {
        CommonFunction.SetSpriteName(Spt_Icon, icon);
        Spt_Icon.MakePixelPerfect();
    }
}


public class EscortStageComponent : BaseComponent
{
    private UISprite Spt_ItemBg;
    private UILabel Lbl_TitleLb;
    private UILabel Lbl_AwardLb;
    private UISprite Spt_Icon;
    private UISprite Spt_IconBG;
    private UISprite Spt_IconFrame;
    private UIButton Btn_Go;
    private UISprite Spt_GoBg;
    private UILabel Lbl_GoLb;
    private UILabel Lbl_unlockTip;
    private UIGrid Grd_AwardItems;
    private GameObject Gobj_AwardItem1;
    private GameObject Gobj_AwardItem2;
    private GameObject Gobj_AwardItem3;
    private GameObject Gobj_AwardItem4;
    private UIGrid Grd_StarItems;
    private UISprite Spt_Star1;
    private UISprite Spt_Star2;
    private UISprite Spt_Star3;

    public delegate void OnChallengeHandle(BaseComponent comp);
    public OnChallengeHandle onChallengeHandle;
    public delegate void OnAwardItemPress(EscortItemComponent comp, bool isPress);
    public OnAwardItemPress onAwradItemHandle;
    private List<GameObject> starList;
    private List<EscortItemComponent> itemList;

    private Color currentColor = new Color(255 / 255.0F, 255 / 255.0F, 196 / 255.0F);
    private Color normalTitleColor = new Color(227 / 255.0F, 169 / 255.0F, 75 / 255.0F);
    private Color normalAwardColor = new Color(160 / 255.0F, 139 / 255.0F, 113 / 255.0F);
    private Color lockBtnColor = Color.grey;
    private Color activeBtnColor = new Color(125 / 255.0F, 81 / 255.0F, 35 / 255.0F);
    private StageData stageData;
    public StageData StageData
    {
        get
        {
            return stageData;
        }
    }

    private bool isEnable = false;
    public bool IsEnable
    {
        get
        {
            return isEnable;
        }
    }

    private bool isActive = false;
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
            if (Lbl_unlockTip == null)
            {
                FindChildProperty();
            }
            Spt_GoBg.color = isActive ? Color.white : Color.black;
            Lbl_GoLb.color = isActive ? activeBtnColor : lockBtnColor;
            if (isActive)
            {
                if (stageData.gateinfo.star_level != 0 && stageData.stageinfo.IsBoss == false)
                {
                    isEnable = false;
                }
                else
                {
                    isEnable = true;
                }
                Lbl_unlockTip.enabled = false;
                Grd_StarItems.gameObject.SetActive(true);
                UpdateStar(stageData.gateinfo.star_level);
            }
            else
            {
                isEnable = false;
                Grd_StarItems.gameObject.SetActive(false);
                Lbl_unlockTip.enabled = true;
                UpdateLockTip();

            }
        }
    }

    private bool isCurrent = false;
    public bool IsCurrent
    {
        get
        {
            return isCurrent;
        }
        set
        {
            isCurrent = value;
            if (Lbl_TitleLb == null)
            {
                FindChildProperty();
            }
            if (isCurrent)
            {
                IsActive = true;
                CommonFunction.SetSpriteName(Spt_ItemBg, GlobalConst.SpriteName.TASK_FINISHBG);
                Lbl_TitleLb.color = currentColor;
                Lbl_AwardLb.color = currentColor;
            }
            else
            {
                CommonFunction.SetSpriteName(Spt_ItemBg, GlobalConst.SpriteName.TASK_UNFINISHBG);
                Lbl_TitleLb.color = normalTitleColor;
                Lbl_AwardLb.color = normalAwardColor;
            }
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        FindChildProperty();
    }

    private void FindChildProperty()
    {
        Spt_ItemBg = mRootObject.transform.FindChild("ItemBG").GetComponent<UISprite>();
        Spt_IconFrame = mRootObject.transform.FindChild("IconFrame").GetComponent<UISprite>();
        Spt_IconBG = mRootObject.transform.FindChild("IconFrame/IconBG").GetComponent<UISprite>();
        Spt_Icon = mRootObject.transform.FindChild("IconFrame/Icon").GetComponent<UISprite>();
        Lbl_TitleLb = mRootObject.transform.FindChild("TitleLb").GetComponent<UILabel>();
        Lbl_AwardLb = mRootObject.transform.FindChild("AwardLb").GetComponent<UILabel>();
        Btn_Go = mRootObject.transform.FindChild("GoBtn").GetComponent<UIButton>();
        Spt_GoBg = mRootObject.transform.FindChild("GoBtn/BG").GetComponent<UISprite>();
        Lbl_GoLb = mRootObject.transform.FindChild("GoBtn/Lb").GetComponent<UILabel>();
        Lbl_unlockTip = mRootObject.transform.FindChild("unlockTip").GetComponent<UILabel>();
        Grd_AwardItems = mRootObject.transform.FindChild("AwardItems").GetComponent<UIGrid>();
        Gobj_AwardItem1 = mRootObject.transform.FindChild("AwardItems/Item_1").gameObject;
        Gobj_AwardItem2 = mRootObject.transform.FindChild("AwardItems/Item_2").gameObject;
        Gobj_AwardItem3 = mRootObject.transform.FindChild("AwardItems/Item_3").gameObject;
        Gobj_AwardItem4 = mRootObject.transform.FindChild("AwardItems/Item_4").gameObject;
        Grd_StarItems = mRootObject.transform.FindChild("StarItems").GetComponent<UIGrid>();
        Spt_Star1 = mRootObject.transform.FindChild("StarItems/Star_1").GetComponent<UISprite>();
        Spt_Star2 = mRootObject.transform.FindChild("StarItems/Star_2").GetComponent<UISprite>();
        Spt_Star3 = mRootObject.transform.FindChild("StarItems/Star_3").GetComponent<UISprite>();
        if (starList == null)
        {
            starList = new List<GameObject>();
            starList.Add(Spt_Star1.gameObject);
            starList.Add(Spt_Star2.gameObject);
            starList.Add(Spt_Star3.gameObject);
        }
        if (itemList == null)
        {
            itemList = new List<EscortItemComponent>();
            EscortItemComponent comp1 = new EscortItemComponent();
            comp1.MyStart(Gobj_AwardItem1);
            comp1.AddPressLisetener(AwardItemPress);
            itemList.Add(comp1);
            EscortItemComponent comp2 = new EscortItemComponent();
            comp2.MyStart(Gobj_AwardItem2);
            comp2.AddPressLisetener(AwardItemPress);
            itemList.Add(comp2);
            EscortItemComponent comp3 = new EscortItemComponent();
            comp3.MyStart(Gobj_AwardItem3);
            comp3.AddPressLisetener(AwardItemPress);
            itemList.Add(comp3);
            EscortItemComponent comp4 = new EscortItemComponent();
            comp4.MyStart(Gobj_AwardItem4);
            comp4.AddPressLisetener(AwardItemPress);
            itemList.Add(comp4);
        }
        UIEventListener.Get(Btn_Go.gameObject).onClick = ChallengeHandle;
    }

    public void UpdateCompInfo(StageData data)
    {
        stageData = data;
        if (stageData == null) return;
        if (stageData.gateinfo == null)
        {
            IsActive = false;
            IsCurrent = false;
        }
        else
        {
            if (stageData.gateinfo.star_level == 0)
            {
                IsCurrent = true;
            }
            else
            {
                IsCurrent = false;
            }
            IsActive = true;
        }
        CommonFunction.SetSpriteName(Spt_Icon, stageData.stageinfo.BossIcon);
        Lbl_TitleLb.text = string.Format(ConstString.GATE_ESCORT_TITLE, stageData.stageinfo.Name, stageData.remainRaidTimes, stageData.stageinfo.ChallengeCount);
        Lbl_AwardLb.text = ConstString.GATE_ESCORT_AWARDTIP;
        UpdateAwardItems();
    }

    private void UpdateLockTip()
    {
        List<uint> ids = new List<uint>();
        for (int i = 0; i < stageData.stageinfo.PreStage.Count; i++)
        {
            uint id = stageData.stageinfo.PreStage[i].StageID;
            if (!PlayerData.Instance.IsPassedGate(id))
            {
                ids.Add(id);
            }
        }
        List<StageInfo> unlockStages = ConfigManager.Instance.mStageData.GetStageListByIDs(ids);
        if (unlockStages == null || unlockStages.Count == 0)
        {
            Lbl_unlockTip.text = string.Empty;
            return;
        }
        string lockTip = string.Empty;
        System.Text.StringBuilder sub = new System.Text.StringBuilder();
        sub.Append(ConstString.GATE_ESCORT_LOCKTIP_FRONT);
        for (int i = 0; i < unlockStages.Count; i++)
        {
            StageInfo unlockStage = unlockStages[i];
            sub.Append(unlockStage.GateSequence);
            if (i < unlockStages.Count - 1)
                sub.Append(ConstString.GATE_ESCORT_LOCKTIP_AND);
        }
        sub.Append(ConstString.GATE_ESCORT_LOCKTIP_UNLOCK);
        Lbl_unlockTip.text = sub.ToString();
    }

    private void UpdateAwardItems()
    {
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(stageData.stageinfo.DropID);
        if (list == null) list = new List<CommonItemData>();
        for (int i = 0; i < itemList.Count; i++)
        {
            EscortItemComponent comp = itemList[i];
            if (comp == null) continue;
            if (i < list.Count)
            {
                CommonItemData award = list[i];
                bool isSpecialID = false;
                IDType idType = CommonFunction.GetTypeOfID(award.ID.ToString());
                switch (idType)
                {
                    case IDType.SP:
                    case IDType.Gold:
                    case IDType.Diamond:
                    case IDType.Medal:
                    case IDType.Exp:
                    case IDType.SoldierExp:
                    case IDType.Honor:
                        isSpecialID = true;
                        break;
                } if (isSpecialID)
                {
                    comp.UpdateCompInfo(award.ID, award.SubType, CommonFunction.GetIconNameByID(award.ID), 1);
                }
                else
                {
                    comp.UpdateCompInfo(award.ID, award.SubType, award.Icon, (int)award.Quality);
                }
                comp.mRootObject.SetActive(true);

            }
            else
            {
                comp.mRootObject.SetActive(false);
            }
        }
        Grd_AwardItems.Reposition();
    }

    private void UpdateStar(uint grade)
    {
        for (int i = 0; i < starList.Count; i++)
        {
            GameObject go = starList[i];
            if (i < grade)
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
        Grd_StarItems.Reposition();
    }

    private void AwardItemPress(BaseComponent basecomp, bool isPress)
    {
        EscortItemComponent comp = basecomp as EscortItemComponent;
        if (comp == null) return;
        if (onAwradItemHandle != null)
        {
            onAwradItemHandle(comp, isPress);
        }
    }

    private void ChallengeHandle(GameObject go)
    {
        if (onChallengeHandle != null)
            onChallengeHandle(this);
    }
}

public class EscortItemComponent : ItemBaseComponent
{
    private UISprite Spt_AwardType;

    private uint itemID;
    public uint ItemID
    {
        get
        {
            return itemID;
        }
    }

    public delegate void PressDelegate(EscortItemComponent comp, bool isPress);
    public PressDelegate pressDelegate;
    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_IconTexture = mRootObject.transform.FindChild("IconSprite").GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("IconBGSprite").GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("FreamSprite").GetComponent<UISprite>();
        Spt_AwardType = mRootObject.transform.FindChild("Mark").GetComponent<UISprite>();
    }

    public void AddPressLisetener(PressDelegate callBack)
    {
        pressDelegate = callBack;
        UIEventListener.Get(mRootObject).onPress = PressHandle;
    }

    private void PressHandle(GameObject go, bool IsPress)
    {
        if (pressDelegate != null)
        {
            pressDelegate(this, IsPress);
        }
    }
    public void UpdateCompInfo(uint id, ItemTypeEnum subType, string icon, int quality)
    {
        itemID = id;
        base.UpdateInfo(icon, quality);
        if (subType == ItemTypeEnum.EquipChip)
        {
            Spt_AwardType.enabled = true;
            CommonFunction.SetSpriteName(Spt_AwardType, GlobalConst.SpriteName.MarkEquipChip);
            Spt_AwardType.MakePixelPerfect();
        }
        else if (subType == ItemTypeEnum.SoldierChip)
        {
            Spt_AwardType.enabled = true;
            CommonFunction.SetSpriteName(Spt_AwardType, GlobalConst.SpriteName.MarkSoldierChip);
            Spt_AwardType.MakePixelPerfect();
        }
        else
        {
            Spt_AwardType.enabled = false;
        }
    }
}