using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum ChooseCheck
{

    OK = 0,
    /// <summary>
    /// 有已经上阵武将
    /// </summary>
    HadIntoBattle = 5,
    /// <summary>
    /// 有高品质武将
    /// </summary>
    HadHightQuality = 4,
    /// <summary>
    /// 星级过高
    /// </summary>
    HadHighStar = 3,
    /// <summary>
    /// 已装备
    /// </summary>
    HadEquip = 2,
    /// <summary>
    /// 高品质但是等级为1
    /// </summary>
    HightQualityButOneLevel = 1
}
public class SDIntensifyPanel
{
    public GameObject root;
    public Soldier infoSoldier;
    public GameObject Btn_Button_close;
    public UIButton Btn_Button_intensify;
    public UIButton Btn_Button_fastSelect;

    public UISlider Slider_ProgressBar;

    public UIPanel scrollPanel;
    public UISprite Icon;
    public UISprite quality;
    public UISprite quanlityBack;
    public UILabel Name;
    public GameObject AttGroup;
    public UILabel Lbl_Label_Lv_Before;
    public UILabel Lbl_Label_Lv_After;
    public GameObject AfterGroup;
    public GameObject MaxLevel;
    public GameObject Item_IntensifyEffect;
    public UILabel Progress_LB;
    public UILabel PromptLabel;
    public UILabel lbl_Label_Step;
    public UIGrid Grd_Grid;
    public GameObject Item;

    private UIWrapContent WrapContent;
    public UIScrollView ScrollView;
    public List<Soldier> materialList;
    public List<UInt64> chooseUid;
    public Vector3 startPos;

    private List<SDMateialtemComponent> _soldierItemList = new List<SDMateialtemComponent>();
    private bool isMax = false;
    private Color red = new Color(1,0.21f,0);

    public GameObject Go_SoldierEffect;

    public void init(GameObject _uiRoot)
    {
        root = _uiRoot.transform.FindChild("SoldierIntensifyPanel").gameObject;
        root.SetActive(false);
        Item_IntensifyEffect = _uiRoot.transform.FindChild("SoldierIntensifyPanel/IntensifyLabelItem").gameObject;
        Btn_Button_close = _uiRoot.transform.FindChild("SoldierIntensifyPanel/MaskSprite").gameObject;
        Btn_Button_intensify = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/IntensifyButton").gameObject.GetComponent<UIButton>();
        Btn_Button_fastSelect = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/FastSelectButton").gameObject.GetComponent<UIButton>();

        Icon = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        quality = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        quanlityBack = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/ArtifactComp/back").gameObject.GetComponent<UISprite>();

        Name = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/NameGroup/NameLabel").gameObject.GetComponent<UILabel>();
        AttGroup = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/AttributeComparisonGroup/AttGroup").gameObject;

        Lbl_Label_Lv_Before = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/LevelGroup/LevelLabelLeft").gameObject.GetComponent<UILabel>();
        Lbl_Label_Lv_After = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/LevelGroup/AfterGroup/LevelLabelRight").gameObject.GetComponent<UILabel>();
        AfterGroup = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/LevelGroup/AfterGroup").gameObject;
        MaxLevel = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/LevelGroup/MaxLevel").gameObject;
        lbl_Label_Step = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/ArtifactComp/Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        Slider_ProgressBar = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/ProgressBar").gameObject.GetComponent<UISlider>();
        Progress_LB = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/ProgressBar/Label").gameObject.GetComponent<UILabel>();
        PromptLabel = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/PromptLabel").gameObject.GetComponent<UILabel>();
        WrapContent = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/MaterialScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollView = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/MaterialScroll/Grid").gameObject.GetComponent<UIGrid>();
        Item = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/MaterialScroll/item").gameObject;
        scrollPanel = _uiRoot.transform.FindChild("SoldierIntensifyPanel/Anim/MaterialScroll").gameObject.GetComponent<UIPanel>();
        WrapContent.onInitializeItem = SetSoldierInfo;
        Item.gameObject.SetActive(false);
    }
    public void setInfo(Soldier sd)
    {
        if (!root.activeSelf)
            //UISystem.Instance.SoldierAttView.PlayOpenSoldierIntensifyAnim();
        chooseUid = new List<UInt64>();
        startPos = scrollPanel.transform.localPosition;

        chooseUid.Clear();
        root.SetActive(true);
        isMax = false;
        infoSoldier = sd;
        InitSoldierItem();
        _setInfo();
    }
    public void OnClose()
    {
        root.SetActive(false);
        infoSoldier = null;
        isMax = false;
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseSoldierLevelUp);
    }
    public void OnIntensify()
    {

    }
    private void _setInfo()
    {
        if (infoSoldier == null) return;
        if (Icon != null)
        {
            CommonFunction.SetSpriteName(Icon, infoSoldier.Att.Icon);
        }
        if (quality)
        {
            CommonFunction.SetQualitySprite(quality, infoSoldier.Att.quality, quanlityBack);
        }
        if (Name != null)
        {
            Name.text = infoSoldier.Att.Name;
        }
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(this.lbl_Label_Step,infoSoldier.StepNum);
        CommonFunction.SetAttributeGroup(AttGroup,infoSoldier.showInfoSoldier);
        if (Lbl_Label_Lv_Before != null && Lbl_Label_Lv_After != null)
        {
            if(infoSoldier.isMaxLevel())
            {
                this.MaxLevel.SetActive(true);
                this.AfterGroup.SetActive(false);
                Lbl_Label_Lv_Before.text = string.Format("Lv.{0}", infoSoldier.Lv);
                GuideManager.Instance.CheckTrigger(GuideTrigger.SoldierLevelMax);
            }
            else
            {
                this.MaxLevel.SetActive(false);
                this.AfterGroup.SetActive(false);
                Lbl_Label_Lv_Before.text = string.Format("Lv.{0}", infoSoldier.Lv);
                Lbl_Label_Lv_After.text = "";
            }
        }
        if (Slider_ProgressBar)
        {
            Slider_ProgressBar.value = (float)infoSoldier._CurrentExp / infoSoldier._NextLvExp;
        }
        if(Progress_LB)
        {
            Progress_LB.text = string.Format("{0}/{1}",infoSoldier._CurrentExp,infoSoldier._NextLvExp);
            if (infoSoldier._CurrentExp > infoSoldier._NextLvExp)
                Progress_LB.color = Color.red;
            else
                Progress_LB.color = Color.white;
        }
        if(PromptLabel)
        {
            if(infoSoldier.isMaxLevel())
            {
                if(infoSoldier.Att.evolveId == 0)
                {
                    this.PromptLabel.text = ConstString.PROMPT_HAD_MAXSTARANDLV;
                }
                else
                {
                    this.PromptLabel.text = ConstString.PROMPT_HAD_MAXLEVEL;
                }
            }
            else
            {
                this.PromptLabel.text = "";
            }
        }
    }

    public void InitSoldierItem()
    {
        materialList = PlayerData.Instance._SoldierDepot.getSoldierList(filter);
        materialList.Reverse();
        Main.Instance.StartCoroutine(CreatSoldierItem(materialList));
    }
    private IEnumerator CreatSoldierItem(List<Soldier> _data)
    {
        Grd_Grid.Reposition();
        yield return 0;
        ScrollView.ResetPosition();
        yield return 0;
        //if (_data.Count == 1 || _data.Count > 14)
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
            //Grd_Grid.enabled = true;
            //WrapContent.enabled = true;
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
                SDMateialtemComponent item = vGo.GetComponent<SDMateialtemComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<SDMateialtemComponent>();
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
            _soldierItemList[i].SetInfo(_data[i]);
            if (this.chooseUid.Contains(materialList[i].uId))
                _soldierItemList[i].mark.SetActive(true);
            else
                _soldierItemList[i].mark.SetActive(false);
        }
        if (count > 12)
            WrapContent.ReGetChild();
        yield return 0;
        Grd_Grid.Reposition();
        yield return 0;
        ScrollView.ResetPosition();
        Grd_Grid.repositionNow = true;
        Grd_Grid.gameObject.SetActive(false);
        Grd_Grid.gameObject.SetActive(true);
        startPos = scrollPanel.transform.localPosition;
    }
    public void SetSoldierInfo(GameObject go, int wrapIndex, int realIndex)
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
        SDMateialtemComponent item = _soldierItemList[wrapIndex];
        item.SetInfo(materialList[realIndex]);
        if (this.chooseUid.Contains(materialList[realIndex].uId))
            item.mark.SetActive(true);
        else
            item.mark.SetActive(false);
    }
    public void OnFastChoose()
    {
        if (isMax || this.infoSoldier.isMaxLevel())
        {
            ErrorCode.ShowErrorTip(319);
            return;
        }

        Vector3 afterPos = scrollPanel.transform.localPosition;
        float increment = Math.Abs(afterPos.x - startPos.x);
        int num = (int)increment / 100;
        int k = 12;
        int j = 0;
        if ((increment % 100) <= 88 && (increment % 100) >= 12)
            k += 2;
        if (increment % 100 >= 90 || increment % 100 == 0)
        {
            if (increment != 0)
            {
                j += 2;
                k += 2;
            }
        }

        var temp = Grd_Grid.GetChildList();
        List<Transform> list = new List<Transform>(temp.Count + 1);
        list.AddRange(temp);
        list.Sort((left, right) => 
        {
            if(left.localPosition.x != right.localPosition.x)
            {
                if (left.localPosition.x < right.localPosition.x)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });

        k = k < list.Count ? k : list.Count;

        for (int i = j; i < k; ++i)
        {
            GameObject mark = list[i].FindChild("mark").gameObject;
            SDMateialtemComponent comp = list[i].GetComponent<SDMateialtemComponent>();
            if (!chooseUid.Contains(comp.uid))
            {
                Soldier sd = PlayerData.Instance._SoldierDepot.TextStrong(infoSoldier.uId, chooseUid);
                if (sd.isMaxLevel())
                {
                    OnSelect();
                    return;
                }
                Soldier choose = PlayerData.Instance._SoldierDepot.FindByUid(comp.uid);
                if (choose == null)
                    continue;
                ChooseCheck result =  ChooseFilter(choose);
                if (result != ChooseCheck.OK && result != ChooseCheck.HightQualityButOneLevel)
                    continue;
                mark.SetActive(true);
                chooseUid.Add(comp.uid);
            }
        }
        OnSelect();
    }
    public void OnItemTouch(SDMateialtemComponent com)
    {
        GameObject mark = com.transform.FindChild("mark").gameObject;
        SDMateialtemComponent comp = com;
        if(mark.activeSelf)
        {
            mark.SetActive(false);
            chooseUid.Remove(comp.uid);
        }
        else
        {
            if (isMax || this.infoSoldier.isMaxLevel())
            {
                ErrorCode.ShowErrorTip(319);
                return;
            }
            mark.SetActive(true);
            GuideManager.Instance.CheckTrigger(GuideTrigger.ChooseLvUpMatSoldierSucceed);
            chooseUid.Add(comp.uid);
        }
        OnSelect();
    }

    private bool filter(Soldier sd)
    {
        if (infoSoldier == null) return false;

        if (infoSoldier.uId == sd.uId) return false;

        return true;
    }
    private void OnSelect()
    {
        Soldier sd = PlayerData.Instance._SoldierDepot.TextStrong(infoSoldier.uId,chooseUid);

        CommonFunction.SetAttributeGroup(AttGroup, infoSoldier.showInfoSoldier, sd.showInfoSoldier);
        if(Slider_ProgressBar)
        {
            Slider_ProgressBar.value = (float)sd._CurrentExp / sd._NextLvExp;
        }
        if (Progress_LB)
        {
            Progress_LB.text = string.Format("{0}/{1}", sd._CurrentExp, sd._NextLvExp);
            if(sd._CurrentExp > sd._NextLvExp)
            {
                Progress_LB.color = red;
            }
            else
            {
                Progress_LB.color = Color.white;
            }
        }
        if (Lbl_Label_Lv_Before != null && Lbl_Label_Lv_After != null)
        {
            if (infoSoldier.isMaxLevel())
            {
                this.MaxLevel.SetActive(true);
                this.AfterGroup.SetActive(false);
                Lbl_Label_Lv_Before.text = string.Format("Lv.{0}", infoSoldier.Lv);
            }
            else
            {
                this.MaxLevel.SetActive(false);
                this.AfterGroup.SetActive(true);
                Lbl_Label_Lv_Before.text = string.Format("Lv.{0}", infoSoldier.Lv);
                Lbl_Label_Lv_After.text = string.Format("Lv.{0}", sd.Level);
                if (this.chooseUid.Count == 0)
                    this.AfterGroup.SetActive(false);
            }
        }
        if (sd.isMaxLevel())
            isMax = true;
        else
            isMax = false;
    }
    public ChooseCheck ChooseFilter(Soldier sd)
    {
        if (CommonFunction.IsAlreadyBattle(sd.uId))
            return ChooseCheck.HadIntoBattle;
        if (sd.Att.quality >= 4)
        {
            if (sd.Level == 1 && sd.Att.quality == 4)
                return ChooseCheck.HightQualityButOneLevel;
            else
                return ChooseCheck.HadHightQuality;
        }
        if (sd.Att.Star >= 4)
            return ChooseCheck.HadHighStar;
        return ChooseCheck.OK;
    }
    //========================================================================================
    SetParticleSortingLayer Pre;
    public void PlaySoldierUpLevelEffect()//武将升级
    {
        if(Go_SoldierEffect==null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_SOLDIERLEVELUP, (GameObject gb) => { Go_SoldierEffect = gb; });
        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_SoldierEffect, Icon.transform);
        Pre = go.GetComponent <SetParticleSortingLayer >();
        Main.Instance.StartCoroutine(CreateLevelUPLabel(0.250F));
    }

    public IEnumerator CreateLevelUPLabel(float time)
    {
        //Debug.LogError(" infoSoldier.showInfoSoldier.Attack   " + UISystem .Instance .SoldierAttView .SoldierStrong_Att );//旧属性
        if(infoSoldier != null)
        {
            Soldier sd = PlayerData.Instance._SoldierDepot.TextStrong(infoSoldier.uId, chooseUid);
            int UPHP = sd.showInfoSoldier.HP - UISystem.Instance.SoldierAttView.SoldierStorng_HP;
            int UPATT = sd.showInfoSoldier.Attack - UISystem.Instance.SoldierAttView.SoldierStrong_Att;
            CommonFunction.ResetParticlePanelOrder(Item_IntensifyEffect, root, Pre);

            //Debug.LogError("sd.showInfoSoldier.Attack   " + sd.showInfoSoldier.Attack);//新属性
            if (UPATT != 0)
            {
                GameObject LabelAttObj = CommonFunction.InstantiateObject(Item_IntensifyEffect, Icon.transform);
                IntensifyLabelItem LabelAttItem = LabelAttObj.AddComponent<IntensifyLabelItem>();
                LabelAttItem.UpdateItem(ConstString.phy_atk + "+" + UPATT.ToString());
                LabelAttObj.SetActive(true);
            }
            yield return new WaitForSeconds(time);
            if (UPHP != 0)
            {
                GameObject LabelHpObj = CommonFunction.InstantiateObject(Item_IntensifyEffect, Icon.transform);
                IntensifyLabelItem LabelHpItem = LabelHpObj.AddComponent<IntensifyLabelItem>();
                LabelHpItem.UpdateItem(ConstString.hp_max + "+" + UPHP.ToString());
                LabelHpObj.SetActive(true);
                //Debug .LogError("sd.showInfoSoldier.HP "+sd.showInfoSoldier.HP);
                //Debug.LogError("infoSoldier.showInfoSoldier.HP " + infoSoldier.showInfoSoldier.HP);
            }
        }
    }
}
