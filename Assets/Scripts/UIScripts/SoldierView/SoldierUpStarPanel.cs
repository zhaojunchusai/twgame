using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
public enum STEPORSTAR
{
    STEP,
    STAR
}
public class SDUpStarPanel
{
    public GameObject root;
    public Soldier infoSoldier;
    public Soldier afterSoldier;
    public Soldier materialSoldier;
    public GameObject Btn_Button_close;
    public UIButton Btn_Button_intensify;
    public UILabel Intensify_Upstar_Label;
    public UILabel Intensify_Exp_Label;
    public GameObject Soldier1;
    public GameObject Soldier2;
    public GameObject Soldier3;

    public GameObject AttGroup;

    private UIWrapContent WrapContent;
    public UIScrollView ScrollView;
    public UIGrid Grd_Grid;
    public GameObject Item;
    public UILabel PromptLabel;
    public UILabel CostLabel;
    public UILabel TitleLabel;
    public GameObject Go_Effect;
    public GameObject GO_EffectFram;
    //public UIPanel  Go_EffectMask;
   // GameObject LastMark;
    GameObject LessLv;
    public List<Soldier> materialList;
    public List<UInt64> chooseUid = new List<ulong>();
    public int Power = 0;
    public int MaxPower = 0;
    public int needMoney = 0;
    public int ExpNeed = 0;
    public bool isMaxStar = false;
    private List<SDUpStartemComponent> _soldierItemList = new List<SDUpStartemComponent>();

    public List<Soldier> IntensifyMaterial;
    private List<SDMateialtemComponent> _sDMateialtemList = new List<SDMateialtemComponent>();
    public List<UInt64> chooseList = new List<ulong>();
    private UIWrapContent WrapContent_Instance;
    public UIScrollView ScrollView_Instance;
    public UIGrid Grd_Grid_Instance;
    public GameObject Item_Instance;
    public UISlider Slider_ProgressBar;
    public UILabel Slider_Label;
    public UIButton intensify;
    public UIButton back;
    private bool isMax = false;
    public int EXP = 0;
    public GameObject UpStarGroup;
    public GameObject EXPAddGroup;
    public GameObject GoldGroup;
    public UISprite GoldIcon;
    public UILabel GoldNum;
    public UILabel GoldTitle;
    public UILabel Prompt_Instance;
    public UILabel CostLabel_Instance;
    public UILabel StepAdd;

    public GameObject Gbj_StepProgress;
    public UIProgressBar StepProgress;
    public UILabel Step_Label;
    public UISprite Step_Spt;

    public int numGold = 0;
    public int StarOrStepMoney = 0;
    public int StarOrStepMoneyType = 1;
    public bool isCanNext = false;
    public int LastStep = -1;
    public STEPORSTAR type;
    private Vector3 Up_Step = new Vector3(196,-182,0);
    private Vector3 Up_Star = new Vector3(15,-182,0);
    public void init(GameObject _uiRoot)
    {
        root = _uiRoot.transform.FindChild("SoldierUpStarPanel").gameObject;
        root.SetActive(false);
        TitleLabel = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/TitleGroup/TTSprite").gameObject.GetComponent<UILabel>();
        Btn_Button_close = _uiRoot.transform.FindChild("SoldierUpStarPanel/MaskSprite").gameObject;
        Btn_Button_intensify = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/IntensifyButton").gameObject.GetComponent<UIButton>();
        Intensify_Upstar_Label = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/IntensifyButton/Label").gameObject.GetComponent<UILabel>();
        Intensify_Exp_Label = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/IntensifyButton/Label").gameObject.GetComponent<UILabel>();
        //Go_EffectMask = _uiRoot.transform.FindChild("Anim/EffectMask").gameObject.GetComponent <UIPanel >();
        //Go_EffectMask.gameObject.SetActive(false);
        Soldier1 = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/SoldierComp1").gameObject;
        Soldier2 = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/SoldierComp2").gameObject;
        Soldier3 = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/SoldierComp3").gameObject;

        GO_EffectFram =_uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/SoldierComp3/QualitySprite").gameObject;

        AttGroup = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/AttributeComparisonGroup/AttGroup").gameObject;

        WrapContent = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/MaterialScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollView = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/MaterialScroll/Grid").gameObject.GetComponent<UIGrid>();
        PromptLabel = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/PromptLabel").gameObject.GetComponent<UILabel>();
        CostLabel = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/IntensifyButton/CostGroup/CostLabel").gameObject.GetComponent<UILabel>();

        Gbj_StepProgress = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/StepGroup").gameObject;
        StepProgress = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/StepGroup/ProgressBar").gameObject.GetComponent<UIProgressBar>();
        Step_Label = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/StepGroup/ProgressBar/Label").gameObject.GetComponent<UILabel>();
        Step_Spt = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/StepGroup/CanStar").gameObject.GetComponent<UISprite>();

        Item = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup/MaterialScroll/item").gameObject;
        LessLv = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/SoldierComp1/LessLv").gameObject;

        WrapContent_Instance = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/MaterialScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollView_Instance = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Grid_Instance = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/MaterialScroll/Grid").gameObject.GetComponent<UIGrid>();
        Item_Instance = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/MaterialScroll/item").gameObject;
        Slider_ProgressBar = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/ProgressBar").gameObject.GetComponent<UISlider>();
        Slider_Label = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/ProgressBar/Label").gameObject.GetComponent<UILabel>();
        intensify = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/IntensifyButton").gameObject.GetComponent<UIButton>();
        back = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/ReturnButton").gameObject.GetComponent<UIButton>();
        UpStarGroup = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/UpStarGroup").gameObject;
        EXPAddGroup = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup").gameObject;
        GoldGroup = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/GoldAdd").gameObject;
        GoldIcon = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/GoldAdd/BGSprite").gameObject.GetComponent<UISprite>();
        GoldNum = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/GoldAdd/Label").gameObject.GetComponent<UILabel>();
        GoldTitle = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/GoldAdd/LabeT").gameObject.GetComponent<UILabel>();
        Prompt_Instance = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/PromptLabel").gameObject.GetComponent<UILabel>();
        CostLabel_Instance = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim/EXPAddGroup/IntensifyButton/CostGroup/CostLabel").gameObject.GetComponent<UILabel>();
        StepAdd = Soldier3.transform.FindChild("Step").gameObject.GetComponent<UILabel>();


        GoldGroup.SetActive(false);
        Item.gameObject.SetActive(false);
        Item_Instance.SetActive(false);
        LessLv.SetActive(false);
        WrapContent.onInitializeItem = SetUpStarInfo;
        WrapContent_Instance.onInitializeItem = SetSoldierInfo;
    }

    public void setInfo(Soldier sd)
    {
        if(!this.root.activeSelf)
            //UISystem.Instance.SoldierAttView.PlayOpenSoliderUpStarAnim();
        {
        }
        
        root.SetActive(true);
        GoldGroup.SetActive(false);
        infoSoldier = sd;
        needMoney = 0;
        ExpNeed = 0;
        materialList = null;
        chooseUid = new List<ulong>();
        _setInfo();
        PlayerData.Instance.UpdatePlayerGoldEvent += Instance_UpdatePlayerGoldEvent;
    }

    void Instance_UpdatePlayerGoldEvent()
    {
        if (CostLabel)
        {
            if (PlayerData.Instance.GoldIsEnough(this.StarOrStepMoneyType, this.StarOrStepMoney))
            {
                CommonFunction.SetLabelColor(CostLabel, Color.white, Color.black);
                CostLabel.effectStyle = UILabel.Effect.Outline;
            }
            else
            {
                Color effect = new Color(0.149f, 0, 0);
                CommonFunction.SetLabelColor(CostLabel, Color.red, effect);
            }
            CostLabel.text = CommonFunction.GetTenThousandUnit(this.StarOrStepMoney);
        }
    }

    public void OnClose()
    {
        //ShowEffect();
        root.SetActive(false);
        GoldGroup.SetActive(false);
        this.chooseList.Clear();
        chooseUid.Clear();
        this.Power = 0;
        MaxPower = 0;
        needMoney = 0;
        ExpNeed = 0;
        this.EXP = 0;
        this.isMaxStar = false;
        this.isMax = false;
        infoSoldier = null;
        this.materialSoldier = null;
        this.afterSoldier = null;
        this.isCanNext = false;
        this.LastStep = -1;
        PlayerData.Instance.UpdatePlayerGoldEvent -= Instance_UpdatePlayerGoldEvent;
    }

    public void OnIntensify()
    {

    }

    private void _setInfo()
    {
        if (infoSoldier == null) return;
        this.type = this.infoSoldier.IsMaxStep() ? STEPORSTAR.STAR : STEPORSTAR.STEP;
        Soldier tmpS = Soldier.createByID(infoSoldier.Att.id);
        CommonFunction.SetGameObjectGray(this.Step_Spt.gameObject, true);
        if (this.type == STEPORSTAR.STAR)
        {
            this.afterSoldier = Soldier.createByID(this.infoSoldier.Att.evolveId);
            this.StarOrStepMoney = this.infoSoldier.Att.sellNum;
            this.StarOrStepMoneyType = this.infoSoldier.Att.sellType;
            this.Gbj_StepProgress.SetActive(false);
            this.Btn_Button_intensify.transform.localPosition = Up_Star;
        }
        else
        {
            this.afterSoldier = Soldier.createByID(this.infoSoldier.Att.id);
            this.afterSoldier.StepNum = this.infoSoldier.StepNum;
            this.afterSoldier.ShowInfoAdd(this.infoSoldier.Level - this.afterSoldier.Level);
            this.afterSoldier.Level = this.infoSoldier.Level;
            this.StarOrStepMoney = 0;
            this.StarOrStepMoneyType = this.infoSoldier.Att.stepMoneyType;
            //this.LastStep = this.infoSoldier.StepNum;
            this.Btn_Button_intensify.transform.localPosition = Up_Step;
            this.Gbj_StepProgress.SetActive(true);
        }
        if (tmpS != null)
        {
            tmpS.ShowInfoAdd(infoSoldier.Level - tmpS.Level);
            afterSoldier.showInfoSoldier.HP = infoSoldier.showInfoSoldier.HP + afterSoldier.showInfoSoldier.HP - tmpS.showInfoSoldier.HP;
            afterSoldier.showInfoSoldier.Attack = infoSoldier.showInfoSoldier.Attack + afterSoldier.showInfoSoldier.Attack - tmpS.showInfoSoldier.Attack;
        }
        materialSoldier = Soldier.createByID(this.type == STEPORSTAR.STAR ? infoSoldier.Att.evolveMateria.Key : infoSoldier.Att.stepMaterialId);
        if (materialSoldier != null)
        {
            if (this.type == STEPORSTAR.STAR)
            {
                Power = materialSoldier.StarWorth();
                materialSoldier.ShowInfoAdd(infoSoldier.Att.evolveMateria.Value - materialSoldier.Level);
                materialSoldier.Level = infoSoldier.Att.evolveMateria.Value;
                materialSoldier.AddStep(infoSoldier.Att.evolveMaterialStep);
            }
            else
            {
                materialSoldier.ShowInfoAdd(infoSoldier.Att.stepMaterialLv - materialSoldier.Level);
                materialSoldier.Level = infoSoldier.Att.stepMaterialLv;
                materialSoldier.AddStep(infoSoldier.Att.stepMaterialStep);
                Power = materialSoldier.StarWorth() * (infoSoldier.Att.maxStep - infoSoldier.StepNum);
                this.StepProgress.value = (float)afterSoldier.StepNum / afterSoldier.Att.maxStep;
                this.Step_Label.text = string.Format("{0}/{1}", afterSoldier.StepNum, afterSoldier.Att.maxStep);
            }
        }
        this.MaxPower = this.Power;
        this.TitleLabel.text = type == STEPORSTAR.STAR ? ConstString.SOLDIERUPSTEP1 : ConstString.SOLDIERUPSTEP2;
        this.Intensify_Upstar_Label.text = type == STEPORSTAR.STAR ? ConstString.SOLDIERUPSTAR12 : ConstString.SOLDIERUPSTAR13;
        this.Intensify_Exp_Label.text = type == STEPORSTAR.STAR ? ConstString.SOLDIERUPSTAR12 : ConstString.SOLDIERUPSTAR13;
        if (afterSoldier != null)
        {
            CommonFunction.SetAttributeGroup(AttGroup, infoSoldier.showInfoSoldier, this.afterSoldier.GetShowInfoAddByStep(0));
        }
        this.UpStarGroup.SetActive(true);
        this.EXPAddGroup.SetActive(false);
        if (Soldier1)
        {
            UISprite tx = Soldier1.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
            UISprite qt = Soldier1.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
            UISprite bc = Soldier1.transform.FindChild("back").gameObject.GetComponent<UISprite>();
            UIGrid Grid_star = Soldier1.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
            UILabel lbl_Label_Lv = Soldier1.transform.FindChild("Lv/Label").gameObject.GetComponent<UILabel>();
            UILabel lbl_Label_Step = Soldier1.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
            lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
            if(lbl_Label_Step)
            {
                lbl_Label_Step.text = CommonFunction.GetStepShow(lbl_Label_Step,infoSoldier.StepNum);
            }
            if (tx)
            {
                if (infoSoldier != null)
                {
                    CommonFunction.SetSpriteName(tx, infoSoldier.Att.Icon);
                    tx.gameObject.SetActive(true);
                }
                else
                    tx.gameObject.SetActive(false);

            }
            if (qt)
            {
                if (infoSoldier != null)
                {
                    CommonFunction.SetQualitySprite(qt, infoSoldier.Att.quality, bc);
                    qt.gameObject.SetActive(true);
                }
                else
                    qt.gameObject.SetActive(false);
            }
            if (lbl_Label_Lv)
            {
                lbl_Label_Lv.text = infoSoldier.Level.ToString();
            }
            if (Grid_star)
            {
                var tempList = Grid_star.GetChildList();
                for (int i = 0; i < tempList.Count; ++i)
                {
                    GameObject star = tempList[i].FindChild("SelectSprite").gameObject;
                    if (i < infoSoldier.Att.Star)
                    {
                        star.SetActive(true);
                    }
                    else
                    {
                        star.SetActive(false);
                    }
                }
            }
            //if(!infoSoldier.isMaxLevel())
            //{
            //    this.LessLv.SetActive(true);
            //}
            //else
            //{
            //    this.LessLv.SetActive(false);
            //}
        }
        if (Soldier2)
        {
            UISprite tx = Soldier2.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
            UISprite qt = Soldier2.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
            UISprite bc = Soldier2.transform.FindChild("back").gameObject.GetComponent<UISprite>();

            //UIGrid Grid_star = Soldier2.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
            //UILabel lbl_Label_Lv = Soldier2.transform.FindChild("Lv/Label").gameObject.GetComponent<UILabel>();
            UILabel lbl_Label_Num = Soldier2.transform.FindChild("Num").gameObject.GetComponent<UILabel>();
            //GameObject mask = Soldier2.transform.FindChild("Mask").gameObject;
            //GameObject lvless = Soldier2.transform.FindChild("LessLv").gameObject;
            //lvless.SetActive(false);
            //mask.SetActive(false);
            if (materialSoldier == null)
                Soldier2.SetActive(false);
            else
            {
                Soldier2.SetActive(true);
                if (tx)
                {
                    CommonFunction.SetSpriteName(tx, materialSoldier.Att.Icon);
                    tx.gameObject.SetActive(true);
                }
                if (qt)
                {
                    CommonFunction.SetQualitySprite(qt, materialSoldier.Att.quality, bc);
                    qt.gameObject.SetActive(true);
                }
                if(lbl_Label_Num)
                {
                    lbl_Label_Num.text = string.Format("0/{0}", materialSoldier.StarWorth());
                }
                //if (lbl_Label_Lv)
                //{
                //    lbl_Label_Lv.text = infoSoldier.Att.evolveMateria.Value.ToString();
                //}
                //if (Grid_star)
                //{
                //    var tempList = Grid_star.GetChildList();
                //    for (int i = 0; i < tempList.Count; ++i)
                //    {
                //        GameObject star = tempList[i].FindChild("SelectSprite").gameObject;
                //        if (temp != null)
                //        {
                //            if (i < material.Att.Star)
                //            {
                //                star.SetActive(true);
                //            }
                //            else
                //            {
                //                star.SetActive(false);
                //            }
                //        }
                //        else
                //        {
                //            star.SetActive(false);
                //        }
                //    }
                //}
                CommonFunction.SetGameObjectGray(Soldier2, true);
            }
        }
        if (Soldier3)
        {
            UISprite tx = Soldier3.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
            UISprite qt = Soldier3.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
            UISprite bc = Soldier3.transform.FindChild("back").gameObject.GetComponent<UISprite>();

            UIGrid Grid_star = Soldier3.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
            UILabel lbl_Label_Lv = Soldier3.transform.FindChild("Lv/Label").gameObject.GetComponent<UILabel>();
            UILabel lbl_Label_Step = Soldier3.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
            lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
            if (tx)
            {
                if (afterSoldier != null)
                {
                    CommonFunction.SetSpriteName(tx, afterSoldier.Att.Icon);
                    tx.gameObject.SetActive(true);
                }
                else
                    tx.gameObject.SetActive(false);
            }
            if (qt)
            {
                if (afterSoldier != null)
                {
                    CommonFunction.SetQualitySprite(qt, afterSoldier.Att.quality, bc);
                    qt.gameObject.SetActive(true);
                }
                else
                    qt.gameObject.SetActive(false);
            }
            if (lbl_Label_Lv)
            {
                lbl_Label_Lv.text = afterSoldier.Level.ToString();
            }
            if(lbl_Label_Step)
            {
                lbl_Label_Step.text = CommonFunction.GetStepShow(lbl_Label_Step,afterSoldier.StepNum);
            }
            if (Grid_star)
            {
                var tempList = Grid_star.GetChildList();
                for (int i = 0; i < tempList.Count; ++i)
                {
                    GameObject star = tempList[i].FindChild("SelectSprite").gameObject;
                    if (afterSoldier != null)
                    {
                        if (i < afterSoldier.Att.Star)
                        {
                            star.SetActive(true);
                        }
                        else
                        {
                            star.SetActive(false);
                        }
                    }
                    else
                    {
                        star.SetActive(false);
                    }
                }
            }
        }
        if (PromptLabel)
        {
            if(this.materialSoldier != null)
            {
                string message = this.type == STEPORSTAR.STAR?ConstString.PROMPT_UP_STAR_SOLDIER:ConstString.SOLDIERUPSTEP3;
                this.PromptLabel.text = string.Format(message, this.materialSoldier.Att.Star, this.materialSoldier.Att.Name);
            }
        }
        //if(this.Btn_Button_intensify)
        //{
        //    UISprite Button_Back = this.Btn_Button_intensify.transform.FindChild("Background").GetComponent<UISprite>();
        //    UILabel Button_Label = this.Btn_Button_intensify.transform.FindChild("Label").GetComponent<UILabel>();
        //    if (Button_Back != null)
        //    {
        //        if(this.infoSoldier.isMaxLevel())
        //        {
        //            CommonFunction.UpdateWidgetGray(Button_Back, false);
        //            CommonFunction.SetUILabelColor(Button_Label, true);
        //        }
        //        else
        //        {
        //            CommonFunction.UpdateWidgetGray(Button_Back, true);
        //            CommonFunction.SetUILabelColor(Button_Label, false);
        //        }
        //    }
        //}
        if (CostLabel)
        {
            if (PlayerData.Instance.GoldIsEnough(this.StarOrStepMoneyType, this.StarOrStepMoney))
            {
                CommonFunction.SetLabelColor(CostLabel, Color.white,Color.black);
                CostLabel.effectStyle = UILabel.Effect.Outline;
            }
            else
            {
                Color effect = new Color(0.149f,0,0);
                CommonFunction.SetLabelColor(CostLabel, Color.red, effect);
            }
            CostLabel.text = CommonFunction.GetTenThousandUnit(this.StarOrStepMoney);
        }
        InitUpStarItem();
    }

    public void InitUpStarItem()
    {
        materialList = PlayerData.Instance._SoldierDepot.getSoldierList(filter);

        Main.Instance.StartCoroutine(CreatUpStarItem(materialList));
    }

    private IEnumerator CreatUpStarItem(List<Soldier> _data)
    {
        Grd_Grid.Reposition();
        yield return 0;
        ScrollView.ResetPosition();
        yield return 0;
        //if (_data.Count == 1)
        //{
        //    this.ScrollView.ResetPosition();
        //    yield return 0;
        //}
        WrapContent.CleanChild();
        int count = _data.Count;
        int itemCount = _soldierItemList.Count;

        int index = Mathf.CeilToInt((float)count / WrapContent.highCount) - 1;
        if (index == 0)
            index = 1;
        WrapContent.minIndex = 0;
        WrapContent.maxIndex = index;
        if (count % 2 != 0)
        {
            this.WrapContent.cullContent = false;
        }
        else
        {
            this.WrapContent.cullContent = true;
        }

        if (count > 14)
        {
            WrapContent.enabled = true;
            if (count % 2 != 0)
            {
                this.WrapContent.cullContent = false;
            }
            else
            {
                this.WrapContent.cullContent = true;
            }
            count = 14;
        }
        else
        {
            WrapContent.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _soldierItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(Item, Grd_Grid.transform);
                SDUpStartemComponent item = vGo.GetComponent<SDUpStartemComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<SDUpStartemComponent>();
                    item.MyStart(vGo);
                }
                _soldierItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _soldierItemList[i].TouchEvent += OnItemTouch;
            }
            else
            {
                _soldierItemList[i].gameObject.SetActive(true);
            }
            _soldierItemList[i].SetInfo(_data[i], false);
            if (this.chooseUid.Contains(materialList[i].uId) )
                _soldierItemList[i].mark.SetActive(true);
            else
                _soldierItemList[i].mark.SetActive(false);
            //GameObject LessLv = _soldierItemList[i].transform.FindChild("LessLv").gameObject;
            //if (LessLv != null)
            //{
            //    if (_data[i].Level < infoSoldier.Att.evolveMateria.Value)
            //        LessLv.gameObject.SetActive(true);
            //    else
            //        LessLv.gameObject.SetActive(false);
            //}
        }
        if (count > 10)
            WrapContent.ReGetChild();
        yield return 0;
        Grd_Grid.Reposition();
        yield return 0;
        ScrollView.ResetPosition();
        Grd_Grid.repositionNow = true;
        Grd_Grid.gameObject.SetActive(false);
        Grd_Grid.gameObject.SetActive(true);
    }

    public void SetUpStarInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= materialList.Count)
        {
            go.SetActive(false);

            return;
        }
        else
        {
            go.SetActive(true);
        }
        SDUpStartemComponent item = _soldierItemList[wrapIndex];
        Soldier tmpSlodier = materialList[realIndex];
        if (this.chooseUid.Contains(tmpSlodier.uId))
            item.mark.SetActive(true);
        else
            item.mark.SetActive(false);
        item.SetInfo(tmpSlodier, (!item.mark.activeInHierarchy) && (Power <= 0));
        //GameObject LessLv = go.transform.FindChild("LessLv").gameObject;
        //if (LessLv != null)
        //{
        //    if (tmpSlodier.Level < infoSoldier.Att.evolveMateria.Value)
        //        LessLv.gameObject.SetActive(true);
        //    else
        //        LessLv.gameObject.SetActive(false);
        //}
    }

    public void OnItemTouch(SDUpStartemComponent go)
    {
        GameObject mark = go.mark;
        SDUpStartemComponent comp = go;
        //if (!this.infoSoldier.isMaxLevel())
        //{
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.SHOULD_MAX_LEVEL,this.infoSoldier.Att.Name,this.infoSoldier.Att.evolveMateria.Value));
        //    return;
        //}
        //GameObject LessLv = go.transform.FindChild("LessLv").gameObject;
        //if (LessLv.activeSelf)
        //{
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.MATERIAL_LEVEL_NOENOUGH, this.infoSoldier.Att.evolveMateria.Value));
        //    return;
        //}
        Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(go.uID);
        if(mark != null)
        {
            if (mark.activeSelf)
            {
                mark.SetActive(false);
                chooseUid.Remove(go.uID);
                if (tmpSoldier != null)
                    Power += tmpSoldier.StarWorth();
            }
            else
            {
                if (Power <= 0)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,this.type == STEPORSTAR.STAR? ConstString.SOLDIEREQUP_EXPWILLFULL:ConstString.SOLDIEREQUP_EXPWILLFULL_Step);
                    return;
                }
                mark.SetActive(true);
                chooseUid.Add(go.uID);

                if (tmpSoldier != null)
                    Power -= tmpSoldier.StarWorth();
            }
        }
        if (Power <= 0)
            SetAllGray(true);
        else
            SetAllGray(false);
        if (this.type == STEPORSTAR.STAR)
            OnSelect();
        else
            OnSelect_UpStep();
    }

    private bool filter(Soldier sd)
    {
        if (infoSoldier == null) return false;
        if (sd == null) return false;
        if (materialSoldier == null) return false;
        if (infoSoldier.uId == sd.uId) return false;
        if (infoSoldier.Att.Star < sd.Att.Star) return false;
        if (!materialSoldier.IsTheSameTree(sd.Att.id)) return false;
        return true;
    }
    private void SetAllGray(bool isGray)
    {
        if (this._soldierItemList == null || this._soldierItemList.Count <= 0)
            return;

        for (int i = 0; i < this._soldierItemList.Count; ++i)
        {
            SDUpStartemComponent tmp = this._soldierItemList[i];
            if (tmp == null)
                continue;
            Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(tmp.uID);
            if (tmpSoldier == null)
                continue;

            tmp.SetGray(!tmp.mark.gameObject.activeSelf && isGray);
        }
    }
    /// <summary>
    /// 每次选择材料后界面刷新
    /// </summary>
    private void OnSelect()
    {
        UISprite tx = Soldier2.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
        UILabel lbl_Label_Num = Soldier2.transform.FindChild("Num").gameObject.GetComponent<UILabel>();
        UISprite qt = Soldier2.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
        UISprite bc = Soldier2.transform.FindChild("back").gameObject.GetComponent<UISprite>();

        SoldierAttributeInfo temp = new SoldierAttributeInfo();
        temp = Soldier.createByID(infoSoldier.Att.evolveMateria.Key).Att;
        if (Power > 0)
        {
            CommonFunction.SetGameObjectGray(Soldier2, true);
        }
        else
        {
            CommonFunction.SetGameObjectGray(Soldier2, false);
        }
        if (tx)
        {
            if (temp != null)
            {
                CommonFunction.SetSpriteName(tx, temp.Icon);
                tx.gameObject.SetActive(true);
            }
            else
                tx.gameObject.SetActive(false);
        }
        if (qt)
        {
            if (temp != null)
            {
                CommonFunction.SetQualitySprite(qt, temp.quality, bc);
                qt.gameObject.SetActive(true);
            }
            else
                qt.gameObject.SetActive(false);
        }
        int num = materialSoldier.StarWorth();
        if (lbl_Label_Num && temp != null)
        {
            lbl_Label_Num.text = string.Format("{0}/{1}",num - Power,num );
        }
        if (Power <= 0)
        {
            this.isCanNext = true;
            this.ExpAndMoney();
            int AddNum = this.needMoney + this.ExpNeed;
            if (AddNum <= 0)
                this.PromptLabel.text = ConstString.SOLDIERUPSTAR_HANEXP;
            else
            {
                //GoldGroup.SetActive(true);
                this.PromptLabel.text = ConstString.SOLDIERUPSTAR_HANEXP_NEXTSTEP;
                //if (CommonFunction.CheckMoneyEnough((ECurrencyType)infoSoldier.Att.sellType, infoSoldier.Att.sellNum,false))
                //    this.GoldNum.text = string.Format(ConstString.SOLDIERUUPSTAR_SHOLDADDGOLD, AddNum);
                //else
                //    this.GoldNum.text = string.Format(ConstString.SOLDIERUUPSTAR_SHOLDADDGOLDGOLD, AddNum);

                //CommonFunction.SetMoneyIcon(this.GoldIcon, (ECurrencyType)this.infoSoldier.Att.sellType);
            }
        }
        else
        {
            this.isCanNext = false;
            GoldGroup.SetActive(false);
        }
        if(Power > 0)
        {
            this.PromptLabel.text = ConstString.SOLDIERUPSTAR_SHOLDEXP;
        }
        if (this.chooseUid == null || this.chooseUid.Count <= 0)
        {
            if (this.materialSoldier != null)
            {
                string message = this.type == STEPORSTAR.STAR ? ConstString.PROMPT_UP_STAR_SOLDIER : ConstString.SOLDIERUPSTEP3;
                this.PromptLabel.text = string.Format(ConstString.PROMPT_UP_STAR_SOLDIER, this.materialSoldier.Att.Star, this.materialSoldier.Att.Name);
            }
        }
    }
    private void OnSelect_UpStep()
    {
        UISprite tx = Soldier2.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
        UILabel lbl_Label_Num = Soldier2.transform.FindChild("Num").gameObject.GetComponent<UILabel>();
        UISprite qt = Soldier2.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
        UISprite bc = Soldier2.transform.FindChild("back").gameObject.GetComponent<UISprite>();
        UILabel lbl_Label_Step = Soldier3.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        if ((MaxPower - Power) < materialSoldier.StarWorth())
        {
            CommonFunction.SetGameObjectGray(Soldier2, true);
        }
        else
        {
            CommonFunction.SetGameObjectGray(Soldier2, false);
        }
        if (tx)
        {
            if (materialSoldier != null)
            {
                CommonFunction.SetSpriteName(tx, materialSoldier.Att.Icon);
                tx.gameObject.SetActive(true);
            }
            else
                tx.gameObject.SetActive(false);
        }
        if (qt)
        {
            if (materialSoldier != null)
            {
                CommonFunction.SetQualitySprite(qt, materialSoldier.Att.quality, bc);
                qt.gameObject.SetActive(true);
            }
            else
                qt.gameObject.SetActive(false);
        }
        int num = materialSoldier.StarWorth();
        int step = (MaxPower - Power) / num;
        if (lbl_Label_Num && materialSoldier != null)
        {
            int tmpResult = (MaxPower - Power);
            if (tmpResult == 0 && (MaxPower - Power) > 0)
                tmpResult = num;
            if (step < 0)
                step = 0;
            if (step + afterSoldier.StepNum >= afterSoldier.Att.maxStep)
            {
                step = afterSoldier.Att.maxStep - afterSoldier.StepNum;
                CommonFunction.SetGameObjectGray(this.Step_Spt.gameObject,false);
            }
            else
            {
                CommonFunction.SetGameObjectGray(this.Step_Spt.gameObject, true);
            }
            this.StepProgress.value = (float)(step + afterSoldier.StepNum) / afterSoldier.Att.maxStep;
            this.Step_Label.text = string.Format("{0}/{1}", step + afterSoldier.StepNum, afterSoldier.Att.maxStep);
            //if (step + afterSoldier.StepNum >= afterSoldier.Att.maxStep)
            //{
            //    tmpResult = (MaxPower - Power) - (step - 1) * (num);
            //}
            lbl_Label_Num.text = string.Format("{0}/{1}", tmpResult, num * (step + 1 + afterSoldier.StepNum > afterSoldier.Att.maxStep?step:step + 1));
            this.StarOrStepMoney = this.infoSoldier.Att.stepMoneyNum * step;
            if (afterSoldier != null)
                CommonFunction.SetAttributeGroup(AttGroup, infoSoldier.showInfoSoldier, this.afterSoldier.GetShowInfoAddByStep(step));
            if (lbl_Label_Step)
            {
                lbl_Label_Step.text = CommonFunction.GetStepShow(lbl_Label_Step,afterSoldier.StepNum + step);
            }
        }
        if (step > 0)
        {
            this.isCanNext = true;
            if (step + afterSoldier.StepNum < afterSoldier.Att.maxStep)
            {
                this.PromptLabel.text = string.Format(ConstString.SOLDIERUPSTEP5,step);
            }
            else
            {
                this.ExpAndMoney();
                int AddNum = this.ExpNeed + this.needMoney;
                if (AddNum <= 0)
                    this.PromptLabel.text = ConstString.SOLDIERUPSTEP6;
                else
                {
                    this.PromptLabel.text = ConstString.SOLDIERUPSTEP7;
                }
            }
        }
        else
        {
            this.isCanNext = false;
            GoldGroup.SetActive(false);
        }
        if (step <= 0)
        {
            this.PromptLabel.text = ConstString.SOLDIERUPSTEP4;
        }
        if (this.chooseUid == null || this.chooseUid.Count <= 0)
        {
            if (this.materialSoldier != null)
            {
                string message = this.type == STEPORSTAR.STAR ? ConstString.PROMPT_UP_STAR_SOLDIER : ConstString.SOLDIERUPSTEP3;
                this.PromptLabel.text = string.Format(message, this.materialSoldier.Att.Star, this.materialSoldier.Att.Name);
            }
        }
        if (CostLabel)
        {
            if (PlayerData.Instance.GoldIsEnough(this.StarOrStepMoneyType, this.StarOrStepMoney))
            {
                CommonFunction.SetLabelColor(CostLabel, Color.white, Color.black);
                CostLabel.effectStyle = UILabel.Effect.Outline;
            }
            else
            {
                Color effect = new Color(0.149f, 0, 0);
                CommonFunction.SetLabelColor(CostLabel, Color.red, effect);
            }
            CostLabel.text = CommonFunction.GetTenThousandUnit(this.StarOrStepMoney);
        }
        if (this.LastStep != -1 && this.LastStep != step)
        {
            SetStepAdd();
            Scheduler.Instance.AddTimer(0.04f, true, ScalAdd);
        }
        this.LastStep = step;
    }
    public void RefreshCost()
    {
        //if (CostLabel)
        //{
        //    if (PlayerData.Instance.GoldIsEnough(this.infoSoldier.Att.sellType, this.infoSoldier.Att.sellNum))
        //    {
        //        CommonFunction.SetLabelColor(CostLabel, Color.white, Color.black);
        //        CostLabel.effectStyle = UILabel.Effect.Outline;
        //    }
        //    else
        //    {
        //        Color effect = new Color(0.149f, 0, 0);
        //        CommonFunction.SetLabelColor(CostLabel, Color.red, effect);
        //    }
        //    CostLabel.text = this.infoSoldier.Att.sellNum.ToString();
        //}
        if (CostLabel_Instance)
        {
            if (PlayerData.Instance.GoldIsEnough(this.StarOrStepMoneyType, this.StarOrStepMoney))
            {
                CommonFunction.SetLabelColor(CostLabel_Instance, Color.white, Color.black);
                CostLabel_Instance.effectStyle = UILabel.Effect.Outline;
            }
            else
            {
                Color effect = new Color(0.149f, 0, 0);
                CommonFunction.SetLabelColor(CostLabel_Instance, Color.red, effect);
            }
            if (PlayerData.Instance.GoldIsEnough(this.StarOrStepMoneyType, this.numGold + this.StarOrStepMoney))
            {
                this.GoldNum.color = Color.white;
            }
            else
            {
                this.GoldNum.color = Color.red;
            }
            CostLabel_Instance.text = CommonFunction.GetTenThousandUnit(this.StarOrStepMoney);
        }
    }
    /// <summary>
    /// 计算是否需要补足经验或者金钱
    /// </summary>
    public void ExpAndMoney()
    {
        int hadMoney = 0;
        int hadExp = 0;
        int allMoney = 0;
        int allExp = 0;
        if (this.type == STEPORSTAR.STAR)
            allExp += this.infoSoldier.GetMaxLevelEXP();
        int num = 1;
        if (this.type == STEPORSTAR.STEP)
        {
            int step = (MaxPower - Power) / materialSoldier.StarWorth();
            if (step < 0)
                step = 0;
            if (step + afterSoldier.StepNum > afterSoldier.Att.maxStep)
            {
                step = afterSoldier.Att.maxStep - afterSoldier.StepNum;
            }
            num = step;
        }
        Vector2 tmp = this.materialSoldier.GetExpAndMoneyWorth() * num;
        allExp += (int)tmp.x;
        allMoney += (int)tmp.y;
        for (int i = 0; i < this.chooseUid.Count;++i )
        {
            Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(this.chooseUid[i]);
            if (tmpSoldier == null)
                continue;
            Vector2 tmpWorth = tmpSoldier.GetExpAndMoneyWorth();
            hadExp += (int)tmpWorth.x;
            hadMoney += (int)tmpWorth.y;
        }
        this.needMoney = allMoney - hadMoney;
        this.ExpNeed = allExp - hadExp;
    }

    public void ShowEffect()
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETSPECIALITEM);
        UISystem.Instance.GetSpecialItemView.SetInfoSoldierUPStar(UISystem .Instance.SoldierAttView.centerPanel.soldier);
    }

    public void SetStepAdd()
    {
        this.StepAdd.fontSize = 18;
        Scheduler.Instance.RemoveTimer(ScalAdd);
        Scheduler.Instance.RemoveTimer(ScalDele);
    }
    public void ScalAdd()
    {
        if(this.StepAdd == null)
        {
            Scheduler.Instance.RemoveTimer(ScalAdd);
            return;
        }
        this.StepAdd.fontSize += 20;
        if (this.StepAdd.fontSize >= 56)
        {
            Scheduler.Instance.RemoveTimer(ScalAdd);
            Scheduler.Instance.AddTimer(0.04f, true, ScalDele);
        }
    }
    public void ScalDele()
    {
        if (this.StepAdd == null)
        {
            Scheduler.Instance.RemoveTimer(ScalAdd);
            return;
        }
        this.StepAdd.fontSize -= 15;
        if (this.StepAdd.fontSize <= 18)
        {
            SetStepAdd();
        }
    }
    #region 经验补充
    //武将升星优化，经验补充用金币补充
    public void SetInfo_Instance()
    {
        this.UpStarGroup.SetActive(false);
        this.EXPAddGroup.SetActive(true);
        this.InitSoldierItem();
    }
    public void InitSoldierItem()
    {
        IntensifyMaterial = PlayerData.Instance._SoldierDepot.getSoldierList(filter_instance);
        IntensifyMaterial.Reverse();
        this.ChooseMaterial();
        this.OnSelect_Instance();
        Main.Instance.StartCoroutine(CreatSoldierItem(IntensifyMaterial));
    }
    private void ChooseMaterial()
    {
        if (this.IntensifyMaterial == null)
            return;
        int needEXP = this.ExpNeed;
        for (int i = 0; i < this.IntensifyMaterial.Count; ++i)
        {
            Soldier tmpSoldier = this.IntensifyMaterial[i];
            if (tmpSoldier == null)
                continue;

            if (this.ChooseFilter(tmpSoldier) != ChooseCheck.OK)
                continue;

            if (EXP >= needEXP)
                break;

            EXP += tmpSoldier.getBeExp();
            this.chooseList.Add(tmpSoldier.uId);
        }
    }
    private IEnumerator CreatSoldierItem(List<Soldier> _data)
    {
        Grd_Grid_Instance.Reposition();
        yield return 0;
        ScrollView_Instance.ResetPosition();
        yield return 0;
        WrapContent_Instance.CleanChild();

        int count = _data.Count;
        int itemCount = _sDMateialtemList.Count;

        int index = Mathf.CeilToInt((float)count / WrapContent_Instance.highCount) - 1;
        if (index == 0)
            index = 1;
        WrapContent_Instance.minIndex = 0;
        WrapContent_Instance.maxIndex = index;
        if (count % 2 != 0)
        {
            this.WrapContent_Instance.cullContent = false;
        }
        else
        {
            this.WrapContent_Instance.cullContent = true;
        }
        if (count > 14)
        {
            WrapContent_Instance.enabled = true;
            if (count % 2 != 0)
            {
                this.WrapContent_Instance.cullContent = false;
            }
            else
            {
                this.WrapContent_Instance.cullContent = true;
            }
            count = 14;
        }
        else
        {
            //Grd_Grid.enabled = true;
            //WrapContent_Instance.enabled = true;
            WrapContent_Instance.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _sDMateialtemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(Item_Instance, Grd_Grid_Instance.transform);
                SDMateialtemComponent item = vGo.GetComponent<SDMateialtemComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<SDMateialtemComponent>();
                    item.MyStart(vGo);
                }
                _sDMateialtemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _sDMateialtemList[i].TouchEvent += OnItemTouch;
            }
            else
            {
                _sDMateialtemList[i].gameObject.SetActive(true);
            }
            _sDMateialtemList[i].SetInfo(_data[i]);
            if (this.chooseList.Contains(IntensifyMaterial[i].uId))
                _sDMateialtemList[i].mark.SetActive(true);
            else
                _sDMateialtemList[i].mark.SetActive(false);
        }
        if (count > 12)
            WrapContent_Instance.ReGetChild();
        yield return 0;
        Grd_Grid_Instance.Reposition();
        yield return 0;
        ScrollView_Instance.ResetPosition();
    }
    public void SetSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= IntensifyMaterial.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        SDMateialtemComponent item = _sDMateialtemList[wrapIndex];
        item.SetInfo(IntensifyMaterial[realIndex]);
        if (this.chooseList.Contains(IntensifyMaterial[realIndex].uId))
            item.mark.SetActive(true);
        else
            item.mark.SetActive(false);
    }
    public void OnItemTouch(SDMateialtemComponent com)
    {
        GameObject mark = com.transform.FindChild("mark").gameObject;
        SDMateialtemComponent comp = com;
        Soldier tmp = PlayerData.Instance._SoldierDepot.FindByUid(com.uid);
        if (tmp == null)
            return;

        if (mark.activeSelf)
        {
            mark.SetActive(false);
            chooseList.Remove(comp.uid);
            this.EXP -= tmp.getBeExp();
        }
        else
        {
            if (isMax)
            {
                ErrorCode.ShowErrorTip(319);
                return;
            }
            mark.SetActive(true);
            chooseList.Add(comp.uid);
            this.EXP += tmp.getBeExp();
        }
        OnSelect_Instance();
    }
    private void OnSelect_Instance()
    {
        if (this.chooseList == null)
            return;
        int needEXP = this.ExpNeed;
        this.Slider_ProgressBar.value = (float)this.EXP / (float)needEXP;
        this.Slider_Label.text = string.Format(ConstString.FQWRQEREQ, this.EXP, needEXP);
        if (this.EXP >= needEXP)
            this.isMax = true;
        else
            this.isMax = false;
        this.GoldGroup.SetActive(false);
        this.Prompt_Instance.gameObject.SetActive(false);
        int expMlp = (needEXP - EXP) > 0 ? needEXP - EXP : 0;
        int num = this.needMoney + expMlp * int.Parse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SOLDIERUPSTAR_EXPADD));
        this.GoldNum.text = num.ToString();
        CommonFunction.SetMoneyIcon(this.GoldIcon, (ECurrencyType)this.StarOrStepMoneyType);
        this.numGold = num;
        if(this.needMoney == 0)
        {
            if(this.EXP >= needEXP)
            {
                this.Prompt_Instance.gameObject.SetActive(true);
                this.Prompt_Instance.text = this.type == STEPORSTAR.STAR ? ConstString.SOLDUERUPSTAR18 : ConstString.SOLDUERUPSTAR19;
            }
            else
            {
                this.GoldGroup.SetActive(true);
                this.GoldTitle.text = ConstString.SOLDUERUPSTAR_1;
            }
        }
        else
        {
            if (this.EXP >= needEXP)
            {
                this.GoldGroup.SetActive(true);
                this.GoldTitle.text =  ConstString.SOLDUERUPSTAR17;
            }
            else
            {
                this.GoldGroup.SetActive(true);
                this.GoldTitle.text = ConstString.SOLDUERUPSTAR16;
            }
        }
        if (CostLabel_Instance)
        {
            if (PlayerData.Instance.GoldIsEnough(this.StarOrStepMoneyType, StarOrStepMoney))
            {
                CommonFunction.SetLabelColor(CostLabel_Instance, Color.white, Color.black);
                CostLabel_Instance.effectStyle = UILabel.Effect.Outline;
            }
            else
            {
                Color effect = new Color(0.149f, 0, 0);
                CommonFunction.SetLabelColor(CostLabel_Instance, Color.red, effect);
            }
            if (PlayerData.Instance.GoldIsEnough(this.StarOrStepMoneyType, num + this.StarOrStepMoney))
            {
                this.GoldNum.color = Color.white;
            }
            else
            {
                this.GoldNum.color = Color.red;
            }
            CostLabel_Instance.text = CommonFunction.GetTenThousandUnit(this.StarOrStepMoney);
        }
    }
    public void OnReturn()
    {
        this.UpStarGroup.SetActive(true);
        this.EXPAddGroup.SetActive(false);

        this.chooseList.Clear();
        this.EXP = 0;
    }
    private bool filter_instance(Soldier sd)
    {
        if (infoSoldier == null) return false;

        if (infoSoldier.uId == sd.uId) return false;

        if (chooseUid.Contains(sd.uId)) return false;

        return true;
    }
    public ChooseCheck ChooseFilter(Soldier sd)
    {
        if (CommonFunction.IsAlreadyBattle(sd.uId))
            return ChooseCheck.HadIntoBattle;
        if (sd.Att.quality >= 4)
        {
            if (sd.Level == 1 && sd.Att.quality == 4)
                return ChooseCheck.OK;
            else
                return ChooseCheck.HadHightQuality;
        }
        return ChooseCheck.OK;
    }
    #endregion
}