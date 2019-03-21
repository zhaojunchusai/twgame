using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
public class LifeSpiritViewController : UIBase
{
    public class AttDesc
    {
        public string title;

        public string desc;
    }

    public class LifeSoulChangeData
    {
        public bool isSimilarEquip;
        public LifeSoulData data;
    }

    public LifeSpiritView view;

    private List<LifeSpiritAttDescComponent> mainAttDesc_dic;

    private LifeSpiritPlayerInfoComponent playerInfoComp;
    private List<LifeSpiritSoldierComponent> ownsoldier_dic;
    private List<LifeSpiritEquipedComponent> equipedSoul_dic;

    private List<LifeSpiritExchangeComponent> exchangeSoul_dic;
    private List<LifeSpiritAttComponent> detailAttDesc_dic;

    private List<UISprite> mainStar_dic;




    private int playerLifeSpiritHole = 8;
    private int soldierLifeSpiritHole = 6;
    private Soldier currentSoldier;
    private LifeSpiritEquipedComponent currentSpiritComp;

    private GameObject PlayerObj;
    private TdSpine.MainSpine playerMainSpine;


    private string soldierAnimation = "";
    private GameObject SoldierObj;
    private TdSpine.MainSpine soldierMainSpine;

    private List<LifeSoulChangeData> chooseSoulList;

    private List<string> cacheNameList;


    public override void Initialize()
    {
        if (view == null)
        {
            view = new LifeSpiritView();
            view.Initialize();
            BtnEventBinding();
        }
        if (cacheNameList == null)
            cacheNameList = new List<string>();
        cacheNameList.Clear();
        PlayerData.Instance._LifeSoulDepot.LifeSoulChangeEvent += LifeSoulChange;
        Init();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenLifeSoulView);
    }

    private void Init()
    {
        if (playerInfoComp == null)
        {
            playerInfoComp = new LifeSpiritPlayerInfoComponent();
            playerInfoComp.MyStart(view.Gobj_PlayerInfo);
        }
        view.Gobj_SoldierComp.SetActive(false);
        view.Gobj_LifeSpriteChoose.SetActive(false);
        view.Gobj_LifeSpriteComp.SetActive(false);
        view.Gobj_LifeSpiritDetailPanel.SetActive(false);
        view.Gobj_CenterAttComp.SetActive(false);
        playerInfoComp.IsSelect = true;
        if (ownsoldier_dic == null)
            ownsoldier_dic = new List<LifeSpiritSoldierComponent>();
        if (equipedSoul_dic == null)
            equipedSoul_dic = new List<LifeSpiritEquipedComponent>();
        if (exchangeSoul_dic == null)
            exchangeSoul_dic = new List<LifeSpiritExchangeComponent>();
        if (detailAttDesc_dic == null)
            detailAttDesc_dic = new List<LifeSpiritAttComponent>();
        if (mainAttDesc_dic == null)
            mainAttDesc_dic = new List<LifeSpiritAttDescComponent>();
        if (mainStar_dic == null)
            mainStar_dic = new List<UISprite>();
        mainStar_dic.Clear();
        mainStar_dic.Add(view.Gobj_EquipStarComp1);
        mainStar_dic.Add(view.Gobj_EquipStarComp2);
        mainStar_dic.Add(view.Gobj_EquipStarComp3);
        mainStar_dic.Add(view.Gobj_EquipStarComp4);
        mainStar_dic.Add(view.Gobj_EquipStarComp5);
        mainStar_dic.Add(view.Gobj_EquipStarComp6);

        view.UIWrapContent_SoldierScrollGrid.onInitializeItem = UpdateWrapOwnSoldierInfo;
        view.UIWrapContent_LifeSpiritChooseGrid.onInitializeItem = UpdateWrapLifeSpiritInfo;
        if (this.currentSoldier != null)
            this.currentSoldier.UpdateAttributeEvent += soldier_UpdateAttributeEvent;
        InitMainLifeSpirit();
        UpdateViewInfo();

    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITVIW);
    }

    private void InitMainLifeSpirit()
    {
        if (equipedSoul_dic != null && equipedSoul_dic.Count >= playerLifeSpiritHole)
            return;
        if (equipedSoul_dic == null)
            equipedSoul_dic = new List<LifeSpiritEquipedComponent>();
        if (equipedSoul_dic.Count <= playerLifeSpiritHole)
            equipedSoul_dic.Clear();
        LifeSpiritEquipedComponent comp1 = new LifeSpiritEquipedComponent();
        comp1.MyStart(view.Gobj_MainRightLifeSpriteComp1);
        comp1.Index = 1;
        comp1.AddEventListener(ButtonEvent_LifeSpirit);
        equipedSoul_dic.Add(comp1);
        LifeSpiritEquipedComponent comp2 = new LifeSpiritEquipedComponent();
        comp2.MyStart(view.Gobj_MainRightLifeSpriteComp2);
        comp2.Index = 2;
        comp2.AddEventListener(ButtonEvent_LifeSpirit);
        equipedSoul_dic.Add(comp2);

        LifeSpiritEquipedComponent comp3 = new LifeSpiritEquipedComponent();
        comp3.MyStart(view.Gobj_MainRightLifeSpriteComp3);
        comp3.Index = 3;
        comp3.AddEventListener(ButtonEvent_LifeSpirit);
        equipedSoul_dic.Add(comp3);

        LifeSpiritEquipedComponent comp4 = new LifeSpiritEquipedComponent();
        comp4.MyStart(view.Gobj_MainRightLifeSpriteComp4);
        comp4.Index = 4;
        comp4.AddEventListener(ButtonEvent_LifeSpirit);
        equipedSoul_dic.Add(comp4);

        LifeSpiritEquipedComponent comp5 = new LifeSpiritEquipedComponent();
        comp5.MyStart(view.Gobj_MainRightLifeSpriteComp5);
        comp5.Index = 5;
        comp5.AddEventListener(ButtonEvent_LifeSpirit);
        equipedSoul_dic.Add(comp5);

        LifeSpiritEquipedComponent comp6 = new LifeSpiritEquipedComponent();
        comp6.MyStart(view.Gobj_MainRightLifeSpriteComp6);
        comp6.Index = 6;
        comp6.AddEventListener(ButtonEvent_LifeSpirit);
        equipedSoul_dic.Add(comp6);

        LifeSpiritEquipedComponent comp7 = new LifeSpiritEquipedComponent();
        comp7.MyStart(view.Gobj_MainRightLifeSpriteComp7);
        comp7.Index = 7;
        comp7.AddEventListener(ButtonEvent_LifeSpirit);
        equipedSoul_dic.Add(comp7);

        LifeSpiritEquipedComponent comp8 = new LifeSpiritEquipedComponent();
        comp8.MyStart(view.Gobj_MainRightLifeSpriteComp8);
        comp8.Index = 8;
        comp8.AddEventListener(ButtonEvent_LifeSpirit);
        equipedSoul_dic.Add(comp8);
    }

    private void LifeSoulChange()
    {
        if (playerInfoComp.IsSelect)
        {
            UpdateSelectPlayerInfo();
        }
        else
        {
            UpdateSelectSoldierInfo();
        }
        if (view.Gobj_LifeSpiritDetailPanel.activeSelf)
        {
            UpdateDetailPanelItemInfo();
        }
    }

    #region Update Event

    public void UpdateViewInfo()
    {
        UpdatePlayerInfo();
        Main.Instance.StartCoroutine(UpdateSoliders());
        if (playerInfoComp.IsSelect)
        {
            UpdateSelectPlayerInfo();
        }
        else
        {
            UpdateSelectSoldierInfo();
        }
        int num = PlayerData.Instance.GetSoldierListCount();
        if (num >= SoldierDepot.MAXCOUNT)
        {
            view.Lbl_SoldierCount.color = new Color(1, 0.21f, 0.21f);
        }
        else
        {
            view.Lbl_SoldierCount.color = new Color(0.46f, 0.98f, 0.61f);
        }
        view.Lbl_SoldierCount.text = string.Format("{0}/{1}", num, SoldierDepot.MAXCOUNT);
    }

    private void UpdateSelectPlayerInfo()
    {
        Main.Instance.StartCoroutine(UpdatePlayerLifeSpirits());
        if (SoldierObj != null)
            SoldierObj.SetActive(false);
        view.Obj_Step.SetActive(false);
        if (IsFastEquip())
        {
            view.Lbl_FastEquipButton.text = ConstString.FAST_EQUIP;
        }
        else
        {
            view.Lbl_FastEquipButton.text = ConstString.FAST_GETOFF;
        }
        CreatePlayerObj();

        int NextLvExp = ConfigManager.Instance.mHeroData.FindByLevel((int)PlayerData.Instance._Level).EXP;
        view.Lbl_MainCenterLevel.text = "LV." + PlayerData.Instance._Level.ToString();
        if (NextLvExp != 0)
            view.Slider_MainProgressBar.value = (float)PlayerData.Instance._CurrentExp / NextLvExp;
        else
            view.Slider_MainProgressBar.value = 1;
        view.Lbl_MainSliderProgressBar.text = string.Format("{0}/{1}", PlayerData.Instance._CurrentExp.ToString(), NextLvExp.ToString());
        view.Lbl_CenterCombatPower.text = PlayerData.Instance._Attribute.CombatPower.ToString();
        view.Gobj_CenterStarLevelGroup.gameObject.SetActive(false);
        view.Lbl_CenterTalent.gameObject.SetActive(false);
        view.Lbl_CenterPosition.gameObject.SetActive(false);
        view.Gobj_CenterObjName.SetActive(false);

        view.Grd_CenterAttGrid.cellHeight = 25;
        List<AttDesc> list = new List<AttDesc>();
        list.Add(new AttDesc() { title = ConstString.hp_max, desc = PlayerData.Instance._Attribute.HP.ToString() });
        list.Add(new AttDesc() { title = ConstString.phy_atk, desc = PlayerData.Instance._Attribute.Attack.ToString() });
        list.Add(new AttDesc() { title = ConstString.tnc_rate, desc = PlayerData.Instance._Attribute.Tenacity.ToString() });
        list.Add(new AttDesc() { title = ConstString.crt_rate, desc = PlayerData.Instance._Attribute.Crit.ToString() });
        list.Add(new AttDesc() { title = ConstString.ddg_rate, desc = PlayerData.Instance._Attribute.Dodge.ToString() });
        list.Add(new AttDesc() { title = ConstString.acc_rate, desc = PlayerData.Instance._Attribute.Accuracy.ToString() });
        list.Add(new AttDesc() { title = ConstString.energy_max, desc = PlayerData.Instance._Attribute.Energy.ToString() });
        list.Add(new AttDesc() { title = ConstString.mp_max, desc = PlayerData.Instance._Attribute.MP.ToString() });
        UpdateMainAttDescValue(list);
    }

    private void UpdateSelectSoldierInfo()
    {
        if (PlayerObj != null)
            PlayerObj.SetActive(false);
        Main.Instance.StartCoroutine(UpdateSoldierLifeSpirits());
        view.Obj_Step.SetActive(GlobalConst.IsOpenStep);

        if (IsFastEquip())
        {
            view.Lbl_FastEquipButton.text = ConstString.FAST_EQUIP;
        }
        else
        {
            view.Lbl_FastEquipButton.text = ConstString.FAST_GETOFF;
        }
        CreateSoldierObj();
        UpdateSoldierAttInfo();
    }

    private void UpdateSoldierAttInfo()
    {
        if (currentSoldier == null) return;
        ShowInfoSoldiers soldierInfAtt = currentSoldier.showInfoSoldier;
        SoldierAttributeInfo soldierAtt = currentSoldier.Att;
        if (soldierAtt == null || soldierInfAtt == null) return;
        view.Lbl_CenterObjName.text = currentSoldier.Att.Name;

        view.Lbl_MainCenterLevel.text = string.Format("Lv.{0}", currentSoldier.Level.ToString());
        if (currentSoldier._NextLvExp != 0)
            view.Slider_MainProgressBar.value = (float)currentSoldier._CurrentExp / currentSoldier._NextLvExp;
        else
            view.Slider_MainProgressBar.value = 1;

        view.Lbl_MainSliderProgressBar.text = string.Format("{0}/{1}", currentSoldier._CurrentExp.ToString(), currentSoldier._NextLvExp.ToString());

        view.Lbl_CenterCombatPower.text = soldierInfAtt.CombatPower.ToString();
        view.Lbl_CenterTalent.text = soldierAtt.talent.ToString();
        if (view.Gobj_CenterStarLevelGroup != null)
        {
            this.view.Gobj_CenterStarLevelGroup.gameObject.SetActive(true);
            UpdateStarLevel();
        }
        view.lbl_Label_Step.text = CommonFunction.GetStepShow(view.lbl_Label_Step, currentSoldier.StepNum);
        view.Lbl_CenterPosition.gameObject.SetActive(true);
        view.Lbl_CenterTalent.gameObject.SetActive(true);
        view.Gobj_CenterObjName.SetActive(true);
        int[] SeatPos = { 3, 6, 5 };
        int[] Carrer = { 2, 0, 1, 4 };
        if (this.currentSoldier.Att.Stance <= SeatPos.Length && this.currentSoldier.Att.Career <= Carrer.Length && this.currentSoldier.Att.Stance > 0 && this.currentSoldier.Att.Career > 0)
        {
            view.Lbl_CenterPosition.text = (SeatPos[this.currentSoldier.Att.Stance - 1] * 10 + Carrer[this.currentSoldier.Att.Career - 1]).ToString();
        }
        view.Grd_CenterAttGrid.cellHeight = 35;
        List<AttDesc> list = new List<AttDesc>();
        list.Add(new AttDesc() { title = ConstString.hp_max, desc = currentSoldier.showInfoSoldier.HP.ToString() });
        list.Add(new AttDesc() { title = ConstString.phy_atk, desc = currentSoldier.showInfoSoldier.Attack.ToString() });
        list.Add(new AttDesc() { title = ConstString.tnc_rate, desc = currentSoldier.showInfoSoldier.Tenacity.ToString() });
        list.Add(new AttDesc() { title = ConstString.crt_rate, desc = currentSoldier.showInfoSoldier.Crit.ToString() });
        list.Add(new AttDesc() { title = ConstString.ddg_rate, desc = currentSoldier.showInfoSoldier.Dodge.ToString() });
        list.Add(new AttDesc() { title = ConstString.acc_rate, desc = currentSoldier.showInfoSoldier.Accuracy.ToString() });
        UpdateMainAttDescValue(list);
    }

    private void CreatePlayerObj()
    {
        if (this.PlayerObj != null)
        {
            PlayerObj.SetActive(true);
            return;
        }
        else
        {
            string assetName = CommonFunction.GetHeroResourceNameByGender((EHeroGender)PlayerData.Instance._Gender);
            ResourceLoadManager.Instance.LoadCharacter(assetName, ResourceLoadType.AssetBundle, (obj) =>
            {
                if (obj != null)
                {
                    string cacheName = ResPath.ReplaceFileName(assetName, false);
                    cacheNameList.Add(cacheName);
                    GameObject go = CommonFunction.InstantiateObject(obj, view.Gobj_PlayerModel.transform);
                    go.SetActive(true);
                    this.PlayerObj = go;
                    TdSpine.MainSpine mainSpine = go.GetComponent<TdSpine.MainSpine>();
                    if (mainSpine != null)
                        UnityEngine.Object.Destroy(mainSpine);
                    mainSpine = go.AddComponent<TdSpine.MainSpine>();
                    this.playerMainSpine = mainSpine;
                    this.playerMainSpine.InitSkeletonAnimation();
                    go.transform.localScale *= RoleManager.Instance.Get_UIHero_Scale;
                    this.playerMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);

                    Main.Instance.StartCoroutine(PlayerSpineEquipInit());

                }
            });
        }
    }

    private IEnumerator PlayerSpineEquipInit()
    {
        yield return 0;
        if (this.playerMainSpine != null)
            this.playerMainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot);
        this.playerMainSpine.ResetAlph(1.0f);
        UIPanel panel = view.Gobj_MainPanel.GetComponent<UIPanel>();
        this.playerMainSpine.setSortingOrder(panel.sortingOrder + 1);
        this.playerMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
        //UISystem.Instance.ResortViewOrder();
    }

    private void UpdateStarLevel()
    {
        if (currentSoldier == null)
            return;
        for (int i = 0; i < mainStar_dic.Count; i++)
        {
            if (i < currentSoldier.Att.Star)
            {
                mainStar_dic[i].enabled = true;
            }
            else
            {
                mainStar_dic[i].enabled = false;
            }
        }
    }

    private void UpdateMainAttDescValue(List<AttDesc> list)
    {
        int count = mainAttDesc_dic.Count;
        if (list.Count < count)
        {
            for (int i = list.Count; i < count; i++)
            {
                LifeSpiritAttDescComponent comp = mainAttDesc_dic[i];
                if (comp != null)
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            AttDesc tmpInfo = list[i];
            LifeSpiritAttDescComponent comp = null;
            if (i < count)
            {
                comp = mainAttDesc_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_CenterAttComp, view.Grd_CenterAttGrid.transform);
                comp = new LifeSpiritAttDescComponent();
                comp.MyStart(go);
                mainAttDesc_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(tmpInfo.title, tmpInfo.desc);
            comp.mRootObject.SetActive(true);
        }
        view.Grd_CenterAttGrid.Reposition();
    }

    private void CreateSoldierObj()
    {
        if (currentSoldier == null)
            return;
        if (this.soldierAnimation.Equals(currentSoldier.Att.Animation) && this.SoldierObj != null)
        {
            SoldierObj.SetActive(true);
            return;
        }
        if (this.SoldierObj != null)
        {
            GameObject.Destroy(this.SoldierObj);
        }
        ResourceLoadManager.Instance.LoadCharacter(currentSoldier.Att.Animation, ResourceLoadType.AssetBundle, (obj) =>
        {
            if (obj != null)
            {
                string cacheName = ResPath.ReplaceFileName(currentSoldier.Att.Animation, false);
                cacheNameList.Add(cacheName);
                this.soldierAnimation = currentSoldier.Att.Animation;
                GameObject go = CommonFunction.InstantiateObject(obj, view.Gobj_PlayerModel.transform);
                go.SetActive(true);
                this.SoldierObj = go;
                TdSpine.MainSpine mainSpine = go.GetComponent<TdSpine.MainSpine>();
                if (mainSpine == null)
                    mainSpine = go.AddComponent<TdSpine.MainSpine>();
                this.soldierMainSpine = mainSpine;
                this.soldierMainSpine.InitSkeletonAnimation();
                this.soldierMainSpine.StartEvent += mainSpine_StartEvent;
                this.soldierMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
                go.transform.localScale *= this.currentSoldier.Att.Scale;
                //this.mainSpine.SetColor(new Color(0,0.14f,1));
                Main.Instance.StartCoroutine(SpineSorting());
            }
        });
    }

    private void UpdatePlayerInfo()
    {
        playerInfoComp.UpdateCompInfo();
    }


    private IEnumerator UpdateSoliders()
    {
        List<Soldier> ownSoldierList = PlayerData.Instance._SoldierDepot._soldierList;
        int MAXCOUNT = 10;
        int count = ownSoldierList.Count;
        int itemCount = ownsoldier_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_SoldierScrollGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_SoldierScrollGrid.minIndex = -index;
        view.UIWrapContent_SoldierScrollGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_SoldierScrollGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_SoldierScrollGrid.enabled = false;
        }
        yield return null;
        if (count < itemCount)
        {
            for (int i = count; i < itemCount; i++)
            {
                LifeSpiritSoldierComponent comp = ownsoldier_dic[i];
                if (comp != null && comp.mRootObject != null)
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < count; i++)
        {
            LifeSpiritSoldierComponent comp = null;
            if (i < itemCount)
            {
                comp = ownsoldier_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_SoldierComp, view.Grd_SoldierScrollGrid.transform);
                comp = new LifeSpiritSoldierComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_OwnSoldier);
                ownsoldier_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(ownSoldierList[i]);
            comp.IsSelect = false;
            comp.mRootObject.SetActive(true);
        }
        yield return null;
        view.UIWrapContent_SoldierScrollGrid.ReGetChild();
        yield return null;
        view.Grd_SoldierScrollGrid.Reposition();
        yield return null;
        view.ScrView_SoldierScrollView.ResetPosition();
    }

    private void UpdateWrapOwnSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_SoldierScrollGrid.enabled == false) return;
        List<Soldier> ownSoldierList = PlayerData.Instance._SoldierDepot._soldierList;
        if (realIndex >= ownSoldierList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            LifeSpiritSoldierComponent comp = ownsoldier_dic[wrapIndex];
            comp.UpdateCompInfo(ownSoldierList[realIndex]);
            comp.IsSelect = false;
            if (currentSoldier != null)
            {
                if (currentSoldier.uId == comp.Soldier.uId)
                    comp.IsSelect = true;
            }
        }
    }

    private void UpdateWrapLifeSpiritInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_LifeSpiritChooseGrid.enabled == false) return;
        if (chooseSoulList == null)
        {
            return;
        }
        if (realIndex >= chooseSoulList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            LifeSpiritExchangeComponent comp = exchangeSoul_dic[wrapIndex];
            comp.UpdateCompInfo(chooseSoulList[realIndex].data);
            comp.IsSimilarEquiped = chooseSoulList[realIndex].isSimilarEquip;
        };
    }

    private IEnumerator UpdatePlayerLifeSpirits()
    {
        view.Gobj_HeroBGGroup.SetActive(true);
        view.Gobj_SoldierBGGroup.SetActive(false);
        view.Grd_MainRightEquip.transform.localPosition = new Vector3(-55, 160, 0);
        view.Grd_MainRightEquip.cellWidth = 112;
        view.Grd_MainRightEquip.cellHeight = 95;
        List<LifeSoulData> list = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
        for (int i = 0; i < equipedSoul_dic.Count; i++)
        {
            LifeSpiritEquipedComponent comp = equipedSoul_dic[i];
            LifeSoulData data = list.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.SoulPOD.position == comp.Index;
            });
            comp.mRootObject.SetActive(true);
            comp.UpdateEquipCompInfo(data, true, (int)PlayerData.Instance._Level);
            if (!comp.IsLock)
            {
                if (data == null)
                {
                    comp.IsEquipable = IsAbleEquip(true);
                    comp.IsReplaceable = false;
                }
                else
                {
                    comp.IsEquipable = false;
                    comp.IsReplaceable = IsAbleChange(true, data);
                }
            }
        }
        yield return null;
        view.Grd_MainRightEquip.repositionNow = true;
    }

    private IEnumerator UpdateSoldierLifeSpirits()
    {
        if (currentSoldier != null)
        {
            view.Gobj_HeroBGGroup.SetActive(false);
            view.Gobj_SoldierBGGroup.SetActive(true);
            view.Grd_MainRightEquip.transform.localPosition = new Vector3(-55, 98, 0);
            view.Grd_MainRightEquip.cellWidth = 112;
            view.Grd_MainRightEquip.cellHeight = 105;
            List<LifeSoulData> list = PlayerData.Instance._LifeSoulDepot.GetSoldierLifeSoulByUID(currentSoldier.uId);
            for (int i = 0; i < equipedSoul_dic.Count; i++)
            {
                LifeSpiritEquipedComponent comp = equipedSoul_dic[i];
                LifeSoulData data = list.Find((tmp) =>
                {
                    if (tmp == null)
                        return false;
                    return tmp.SoulPOD.position == comp.Index;
                });
                if (i < soldierLifeSpiritHole)
                {
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
                comp.UpdateEquipCompInfo(data, false, currentSoldier.Att.Star);
                if (!comp.IsLock)
                {
                    if (data == null)
                    {
                        comp.IsEquipable = IsAbleEquip(false);
                        comp.IsReplaceable = false;
                    }
                    else
                    {
                        comp.IsEquipable = false;
                        comp.IsReplaceable = IsAbleChange(false, data);
                    }
                }
            }
        }
        yield return null;
        view.Grd_MainRightEquip.repositionNow = true;
    }

    public IEnumerator UpdateChangeLifeSpirit(List<LifeSoulChangeData> soulList)
    {
        int MAXCOUNT = 10;
        int count = soulList.Count;
        int itemCount = exchangeSoul_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_LifeSpiritChooseGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_LifeSpiritChooseGrid.minIndex = -index;
        view.UIWrapContent_LifeSpiritChooseGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_LifeSpiritChooseGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_LifeSpiritChooseGrid.enabled = false;
        }
        yield return null;
        if (count < itemCount)
        {
            for (int i = count; i < itemCount; i++)
            {
                LifeSpiritExchangeComponent comp = exchangeSoul_dic[i];
                if (comp != null && comp.mRootObject != null)
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < count; i++)
        {
            LifeSoulChangeData data = soulList[i];
            if (data == null)
                continue;
            LifeSpiritExchangeComponent comp = null;
            if (i < itemCount)
            {
                comp = exchangeSoul_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_LifeSpriteComp, view.Grd_LifeSpiritChooseGrid.transform);
                comp = new LifeSpiritExchangeComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_ExchangeLifeSpirit);
                exchangeSoul_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(data.data);
            comp.IsSimilarEquiped = data.isSimilarEquip;
        }
        yield return null;
        if (count > 2)
            view.UIWrapContent_LifeSpiritChooseGrid.ReGetChild();
        yield return null;
        view.Grd_LifeSpiritChooseGrid.Reposition();
        yield return null;
        view.ScrView_LifeSpriteChooseScrollView.ResetPosition();
    }


    private void UpdateSelectSoldier()
    {
        for (int i = 0; i < ownsoldier_dic.Count; i++)
        {
            LifeSpiritSoldierComponent comp = ownsoldier_dic[i];
            if (comp == null)
                continue;
            comp.IsSelect = false;
            if (currentSoldier != null)
            {
                if (comp.Soldier.uId == currentSoldier.uId)
                    comp.IsSelect = true;
            }
        }
    }

    private void OpenLifeSpiritDetailPanel()
    {
        view.Gobj_LifeSpiritDetailPanel.SetActive(true);
        UpdateDetailPanelItemInfo();
    }

    private void UpdateDetailPanelItemInfo()
    {
        if (currentSpiritComp == null || currentSpiritComp.LifeSoulData == null)
            return;
        CommonFunction.SetSpriteName(view.Spt_LifeSpiritInfoIcon, currentSpiritComp.LifeSoulData.SoulInfo.icon);
        CommonFunction.SetLifeSpiritTypeMark(view.Spt_LifeSpiritInfoTypeMark, currentSpiritComp.LifeSoulData.SoulInfo.godEquip);
        CommonFunction.SetQualitySprite(view.Spt_LifeSpiritInfoQuality, currentSpiritComp.LifeSoulData.SoulInfo.quality, view.Spt_LifeSpiritInfoBg);
        view.Lbl_LifeSpiritDescLabel.text = currentSpiritComp.LifeSoulData.SoulInfo.desc;
        view.Lbl_LifeSpiritInfoName.text = currentSpiritComp.LifeSoulData.SoulInfo.name;
        view.Lbl_LifeSpiritLevel.text = string.Format(ConstString.LIFESPIRIT_LIFESOUL_LEVEL, currentSpiritComp.LifeSoulData.SoulPOD.level.ToString());
        if (currentSpiritComp.LifeSoulData.SoulInfo.skillID != 0)
        {
            view.Grd_DetailAttGroup.gameObject.SetActive(false);
            view.Lbl_LifeSpiritDetailSkillDesc.enabled = true;
            view.Lbl_LifeSpiritDetailSkillDesc.text = currentSpiritComp.LifeSoulData.Skill.GetDescript(currentSpiritComp.LifeSoulData.Skill.Level);
        }
        else
        {
            view.Grd_DetailAttGroup.gameObject.SetActive(true);
            view.Lbl_LifeSpiritDetailSkillDesc.enabled = false;
            UpdateLifeSpiritDetailAttValue();
        }
    }

    private void UpdateLifeSpiritDetailAttValue()
    {
        ShowInfoWeapon data = new ShowInfoWeapon();
        data.HP = currentSpiritComp.LifeSoulData.SoulInfo.hp_initial + currentSpiritComp.LifeSoulData.SoulInfo.hp_up * (currentSpiritComp.LifeSoulData.SoulPOD.level - 1);
        data.Attack = currentSpiritComp.LifeSoulData.SoulInfo.attack_initial + currentSpiritComp.LifeSoulData.SoulInfo.attack_up * (currentSpiritComp.LifeSoulData.SoulPOD.level - 1);
        data.Accuracy = currentSpiritComp.LifeSoulData.SoulInfo.accrate_initial + currentSpiritComp.LifeSoulData.SoulInfo.accrate_up * (currentSpiritComp.LifeSoulData.SoulPOD.level - 1);
        data.Dodge = currentSpiritComp.LifeSoulData.SoulInfo.ddgrate_initial + currentSpiritComp.LifeSoulData.SoulInfo.ddgrate_up * (currentSpiritComp.LifeSoulData.SoulPOD.level - 1);
        data.Crit = currentSpiritComp.LifeSoulData.SoulInfo.crt_initial + currentSpiritComp.LifeSoulData.SoulInfo.crt_up * (currentSpiritComp.LifeSoulData.SoulPOD.level - 1);
        data.Tenacity = currentSpiritComp.LifeSoulData.SoulInfo.tenacity_initial + currentSpiritComp.LifeSoulData.SoulInfo.tenacity_up * (currentSpiritComp.LifeSoulData.SoulPOD.level - 1);
        data.HP = Mathf.CeilToInt(data.HP / 10000);
        data.Attack = Mathf.CeilToInt(data.Attack / 10000);
        data.Accuracy = Mathf.CeilToInt(data.Accuracy / 10000);
        data.Dodge = Mathf.CeilToInt(data.Dodge / 10000);
        data.Crit = Mathf.CeilToInt(data.Crit / 10000);
        data.Tenacity = Mathf.CeilToInt(data.Tenacity / 10000);
        List<KeyValuePair<string, string>> attributeList = CommonFunction.GetWeaponAttributeDesc(data);
        int count = detailAttDesc_dic.Count;
        if (attributeList.Count < count)
        {
            for (int i = attributeList.Count; i < count; i++)
            {
                LifeSpiritAttComponent comp = detailAttDesc_dic[i];
                if (comp != null)
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < attributeList.Count; i++)
        {
            KeyValuePair<string, string> tmpInfo = attributeList[i];
            LifeSpiritAttComponent comp = null;
            if (i < count)
            {
                comp = detailAttDesc_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_LifeSpiritDetailAttComp, view.Grd_DetailAttGroup.transform);
                comp = new LifeSpiritAttComponent();
                comp.MyStart(go);
                detailAttDesc_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateInfo(tmpInfo.Key, tmpInfo.Value);
            comp.mRootObject.SetActive(true);
        }
        view.Grd_DetailAttGroup.Reposition();
    }

    private void OpenLifeSpiritChoosePanel()
    {
        List<LifeSoulData> list = null;
        if (playerInfoComp.IsSelect)
        {
            if (currentSpiritComp == null || currentSpiritComp.LifeSoulData == null)
            {
                list = PlayerData.Instance._LifeSoulDepot.GetAllPlayersLifeSoul();
            }
            else
            {
                list = PlayerData.Instance._LifeSoulDepot.GetAllPlayersLifeSoulExcept(currentSpiritComp.LifeSoulData.SoulPOD.uid);
            }
        }
        else
        {
            if (currentSpiritComp == null || currentSpiritComp.LifeSoulData == null)
            {
                list = PlayerData.Instance._LifeSoulDepot.GetAllSoldiersLifeSoul();
            }
            else
            {
                list = PlayerData.Instance._LifeSoulDepot.GetAllSoldiersLifeSoulExcept(currentSpiritComp.LifeSoulData.SoulPOD.uid);
            }
        }
        SortChooseLifeSoul(list);
        view.Gobj_LifeSpriteChoose.SetActive(true);
        Main.Instance.StartCoroutine(UpdateChangeLifeSpirit(chooseSoulList));
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenChangeSoul);
    }

    private bool IsSameEquiped(LifeSoulData soulData)
    {
        List<LifeSoulData> list = null;
        if (playerInfoComp.IsSelect)
        {
            list = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
        }
        else
        {
            list = PlayerData.Instance._LifeSoulDepot.GetSoldierLifeSoulByUID(currentSoldier.uId);
        }

        if (list == null)
            return false;
        for (int i = 0; i < list.Count; i++)
        {
            LifeSoulData data = list[i];
            if (data == null)
                continue;
            if (currentSpiritComp != null && currentSpiritComp.LifeSoulData != null)
            {
                if (data.SoulInfo.type == currentSpiritComp.LifeSoulData.SoulInfo.type)
                    continue;
            }
            if (data.SoulInfo.type == soulData.SoulInfo.type)
                return true;
        }
        return false;
    }

    private void SortChooseLifeSoul(List<LifeSoulData> list)
    {
        List<LifeSoulChangeData> typeList = new List<LifeSoulChangeData>();
        List<LifeSoulChangeData> conflictList = new List<LifeSoulChangeData>();
        for (int i = 0; i < list.Count; i++)
        {
            LifeSoulData data = list[i];
            if (data == null)
                continue;
            if (currentSpiritComp.LifeSoulData == null)
            {
                if (IsSameEquiped(data))
                {
                    LifeSoulChangeData tmp = new LifeSoulChangeData();
                    tmp.isSimilarEquip = true;
                    tmp.data = data;
                    conflictList.Add(tmp);
                }
                else
                {
                    LifeSoulChangeData tmp = new LifeSoulChangeData();
                    tmp.isSimilarEquip = false;
                    tmp.data = data;
                    typeList.Add(tmp);
                }
            }
            else
            {
                if (currentSpiritComp.LifeSoulData.SoulInfo.type == data.SoulInfo.type)
                {
                    LifeSoulChangeData tmp = new LifeSoulChangeData();
                    tmp.isSimilarEquip = false;
                    tmp.data = data;
                    typeList.Add(tmp);
                }
                else
                {
                    if (IsSameEquiped(data))
                    {
                        LifeSoulChangeData tmp = new LifeSoulChangeData();
                        tmp.isSimilarEquip = true;
                        tmp.data = data;
                        conflictList.Add(tmp);
                    }
                    else
                    {
                        LifeSoulChangeData tmp = new LifeSoulChangeData();
                        tmp.isSimilarEquip = false;
                        tmp.data = data;
                        typeList.Add(tmp);
                    }
                }
            }
        }
        typeList.Sort((left, right) =>
        {
            if (left == null || right == null)
                return 0;
            if ((!left.data.IsEquipedSoldier && !left.data.IsEquipedPlayer) != (!right.data.IsEquipedSoldier && !right.data.IsEquipedPlayer))
            {
                if (!left.data.IsEquipedSoldier && !left.data.IsEquipedPlayer) return -1;
                else return 1;
            }
            if (left.data.SoulInfo.quality != right.data.SoulInfo.quality)
            {
                if (left.data.SoulInfo.quality < right.data.SoulInfo.quality)
                    return 1;
                else
                    return -1;
            }
            if (left.data.SoulPOD.level != right.data.SoulPOD.level)
            {
                if (left.data.SoulPOD.level < right.data.SoulPOD.level)
                    return 1;
                else
                    return -1;
            }
            if (left.data.SoulInfo.id != right.data.SoulInfo.id)
            {
                if (left.data.SoulInfo.id < right.data.SoulInfo.id)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        conflictList.Sort((left, right) =>
        {
            if (left == null || right == null)
                return 0;
            if ((!left.data.IsEquipedSoldier && !left.data.IsEquipedPlayer) != (!right.data.IsEquipedSoldier && !right.data.IsEquipedPlayer))
            {
                if (!left.data.IsEquipedSoldier && !left.data.IsEquipedPlayer) return -1;
                else return 1;
            }
            if (left.data.SoulInfo.quality != right.data.SoulInfo.quality)
            {
                if (left.data.SoulInfo.quality < right.data.SoulInfo.quality)
                    return 1;
                else
                    return -1;
            }
            if (left.data.SoulPOD.level != right.data.SoulPOD.level)
            {
                if (left.data.SoulPOD.level < right.data.SoulPOD.level)
                    return 1;
                else
                    return -1;
            }
            if (left.data.SoulInfo.id != right.data.SoulInfo.id)
            {
                if (left.data.SoulInfo.id < right.data.SoulInfo.id)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });

        chooseSoulList = new List<LifeSoulChangeData>();
        chooseSoulList.AddRange(typeList);
        chooseSoulList.AddRange(conflictList);
    }

    private bool IsAbleEquip(bool isPlayer)
    {
        List<LifeSoulData> changeList = PlayerData.Instance._LifeSoulDepot.GetAllExchangeLifeSouls(isPlayer);
        if (changeList == null || changeList.Count <= 0)
            return false;
        List<LifeSoulData> equipedList = null;
        if (isPlayer)
        {
            equipedList = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
        }
        else
        {
            equipedList = PlayerData.Instance._LifeSoulDepot.GetSoldierLifeSoulByUID(currentSoldier.uId);
        }
        if (equipedList == null || equipedList.Count == null)
            return true;
        for (int i = 0; i < changeList.Count; i++)
        {
            LifeSoulData data = changeList[i];
            if (data == null)
                continue;
            LifeSoulData tmpData = equipedList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.SoulInfo.type == data.SoulInfo.type;
            });
            if (tmpData == null)   //如果找不到同类型的命魂则说明可装备
                return true;
        }
        return false;
    }

    private bool IsAbleChange(bool isPlayer, LifeSoulData currentLifeSoul)
    {
        List<LifeSoulData> changeList = PlayerData.Instance._LifeSoulDepot.GetAllExchangeLifeSoulsExcept(isPlayer, currentLifeSoul.SoulPOD.uid);
        if (changeList == null || changeList.Count <= 0)
            return false;
        List<LifeSoulData> equipedList = null;
        if (isPlayer)
        {
            equipedList = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
        }
        else
        {
            equipedList = PlayerData.Instance._LifeSoulDepot.GetSoldierLifeSoulByUID(currentSoldier.uId);
        }
        if (equipedList == null || equipedList.Count <= 0)
            return false;
        for (int i = 0; i < changeList.Count; i++)
        {
            LifeSoulData changeData = changeList[i];
            if (changeData == null)
                continue;
            if (changeData.SoulInfo.quality > currentLifeSoul.SoulInfo.quality) //品质高 更换  
            {
                if (changeData.SoulInfo.type == currentLifeSoul.SoulInfo.type)   //和当前类型相同
                {
                    //因为装备时已经排除了相同类型的  所以不在遍历判断
                    return true;
                }
                else
                {
                    LifeSoulData equipedData = equipedList.Find((tmp) =>
                    {
                        if (tmp == null)
                            return false;
                        return tmp.SoulInfo.type == changeData.SoulInfo.type;
                    });
                    if (equipedData == null)   //上面已经排除了当前孔和该命魂类型相同的情况 因而这里不会找到当前命魂孔类型相同的数据
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    #endregion

    private bool IsFastEquip()
    {
        int count = playerLifeSpiritHole;
        if (!playerInfoComp.IsSelect)
        {
            count = soldierLifeSpiritHole;
        }
        for (int i = 0; i < count; i++)
        {
            LifeSpiritEquipedComponent comp = equipedSoul_dic[i];
            if (comp == null)
                continue;
            if (comp.IsLock)
                continue;
            if (comp.LifeSoulData == null)
                return true;
        }
        return false;
    }

    #region Button Event
    void mainSpine_StartEvent(string animationName)
    {
        if (animationName.Equals(GlobalConst.ANIMATION_NAME_IDLE))
        {
            this.soldierMainSpine.gameObject.SetActive(true);
            this.soldierMainSpine.StartEvent -= mainSpine_StartEvent;
        }
    }

    private IEnumerator SpineSorting()
    {
        yield return 0;
        UIPanel panel = view.Gobj_MainPanel.GetComponent<UIPanel>();
        this.soldierMainSpine.setSortingOrder(panel.sortingOrder + 1);
        this.soldierMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
    }

    private void ButtonEvent_LifeSpirit(BaseComponent basecomp)
    {
        LifeSpiritEquipedComponent comp = basecomp as LifeSpiritEquipedComponent;
        if (comp == null)
            return;
        if (comp.IsLock)
            return;
        currentSpiritComp = comp;
        if (comp.LifeSoulData == null)
        {
            OpenLifeSpiritChoosePanel();
        }
        else
        {
            OpenLifeSpiritDetailPanel();
        }
    }

    private void ButtonEvent_ExchangeLifeSpirit(BaseComponent basecomp)
    {
        LifeSpiritExchangeComponent comp = basecomp as LifeSpiritExchangeComponent;
        if (comp == null)
            return;
        if (comp.IsSimilarEquiped)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRIT_EQUIPED_SAMELIFESOUL);
            return;
        }
        List<EquipedSoul> list = new List<EquipedSoul>();
        EquipedSoul soul = new EquipedSoul();
        soul.uid = comp.LifeSoulData.SoulPOD.uid;
        soul.id = comp.LifeSoulData.SoulPOD.id;
        soul.position = currentSpiritComp.Index;
        list.Add(soul);
        if (playerInfoComp.IsSelect)
        {
            LifeSpiritModule.Instance.SendPutOnSoul(LifeSoulOpType.ONCE, list);
        }
        else
        {
            LifeSpiritModule.Instance.SendPutOnSoul(LifeSoulOpType.ONCE, list, currentSoldier.uId);
        }
    }

    private void ButtonEvent_PlayerInfo(GameObject go)
    {
        playerInfoComp.IsSelect = true;
        currentSoldier = null;
        UpdateSelectSoldier();
        UpdateSelectPlayerInfo();
    }

    private void ButtonEvent_PlayerModel(GameObject go)
    {
        if (playerInfoComp.IsSelect)
        {
            if (this.playerMainSpine == null) return;

            List<string> tempList = new List<string>();
            tempList.Add(GlobalConst.ANIMATION_NAME_MOVE);
            tempList.Add(GlobalConst.ANIMATION_NAME_VICTORY);
            //tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_BASIC);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE6);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE8);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE10);
            int index = UnityEngine.Random.Range(0, tempList.Count - 1);

            this.playerMainSpine.pushAnimation(tempList[index], true, 1);
            this.playerMainSpine.EndEvent += (string animationName) =>
            {
                this.playerMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
            };
        }
        else
        {
            if (this.soldierMainSpine == null) return;
            List<string> tempList = new List<string>();
            tempList.Add(GlobalConst.ANIMATION_NAME_MOVE);
            tempList.Add(GlobalConst.ANIMATION_NAME_VICTORY);
            //tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_BASIC);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE);
            int index = UnityEngine.Random.Range(0, tempList.Count - 1);
            this.soldierMainSpine.pushAnimation(tempList[index], true, 1);
            this.soldierMainSpine.EndEvent += (string animationName) =>
            {
                this.soldierMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
            };
        }
    }

    private void ButtonEvent_OwnSoldier(BaseComponent baseComp)
    {
        LifeSpiritSoldierComponent comp = baseComp as LifeSpiritSoldierComponent;
        if (comp == null)
            return;
        if (comp.IsSelect)
            return;
        if (this.currentSoldier != null)
            this.currentSoldier.UpdateAttributeEvent -= soldier_UpdateAttributeEvent;
        currentSoldier = comp.Soldier;
        if (this.currentSoldier != null)
            this.currentSoldier.UpdateAttributeEvent += soldier_UpdateAttributeEvent;


        playerInfoComp.IsSelect = false;
        UpdateSelectSoldier();
        UpdateSelectSoldierInfo();
    }

    public void ButtonEvent_IntensifyButton(GameObject btn)
    {
        if (currentSpiritComp == null || currentSpiritComp.LifeSoulData == null)
            return;
        if (currentSpiritComp.LifeSoulData.IsAbleToUpgrade())
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITINTENSIFY);
            UISystem.Instance.LifeSpiritIntensifyView.UpdateViewInfo(currentSpiritComp.LifeSoulData);
        }
    }

    public void ButtonEvent_ChangeButton(GameObject btn)
    {
        OpenLifeSpiritChoosePanel();
    }

    public void ButtonEvent_UnloadButton(GameObject btn)
    {
        if (PlayerData.Instance._LifeSoulDepot.IsPackNotFull())
        {
            List<ulong> i_uid = new List<ulong>();
            i_uid.Add(currentSpiritComp.LifeSoulData.SoulPOD.uid);
            if (playerInfoComp.IsSelect)
            {
                LifeSpiritModule.Instance.SendTakeOffSoul(LifeSoulOpType.ONCE, i_uid);
            }
            else
            {
                LifeSpiritModule.Instance.SendTakeOffSoul(LifeSoulOpType.ONCE, i_uid, currentSoldier.uId);
            }
        }
    }

    public void ButtonEvent_CloseLifeSoulChoose(GameObject btn)
    {
        view.Gobj_LifeSpriteChoose.SetActive(false);
    }

    public void ButtonEvent_CloseView(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITVIW);
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseLifeSoulView);
    }

    public void ButtonEvent_CloseDetailView(GameObject btn)
    {
        view.Gobj_LifeSpiritDetailPanel.SetActive(false);
    }

    public void ButtonEvent_LifeSpiritBackPack(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITPACKVIEW);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_TOPFUNC);
    }

    public void ButtonEvent_HuntingLifeSpirit(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREYLIFESPIRITVIEW);
    }

    public void ButtonEvent_FastEquipButton(GameObject btn)
    {
        if (IsFastEquip())
        {
            if (playerInfoComp.IsSelect)
            {
                LifeSpiritModule.Instance.SendPutOnSoul(LifeSoulOpType.ONE_KEY, new List<EquipedSoul>());
            }
            else
            {
                LifeSpiritModule.Instance.SendPutOnSoul(LifeSoulOpType.ONE_KEY, new List<EquipedSoul>(), currentSoldier.uId);
            }
        }
        else
        {
            if (playerInfoComp.IsSelect)
            {
                LifeSpiritModule.Instance.SendTakeOffSoul(LifeSoulOpType.ONE_KEY, new List<ulong>());
            }
            else
            {
                LifeSpiritModule.Instance.SendTakeOffSoul(LifeSoulOpType.ONE_KEY, new List<ulong>(), currentSoldier.uId);
            }
        }
    }

    void soldier_UpdateAttributeEvent()
    {
        UpdateSoldierAttInfo();
    }

    #endregion

    public void OnTakeoffSoulSuccess()
    {
        view.Gobj_LifeSpriteChoose.SetActive(false);
        view.Gobj_LifeSpiritDetailPanel.SetActive(false);
        //if (playerInfoComp.IsSelect)
        //{
        //    UpdateSelectPlayerInfo();
        //}
        //else
        //{
        //    UpdateSelectSoldierInfo();
        //}
    }

    public void OnPutonLifeSoulSuccess()
    {
        GuideManager.Instance.CheckTrigger(GuideTrigger.EquipSoulSuccess);
        view.Gobj_LifeSpriteChoose.SetActive(false);
        view.Gobj_LifeSpiritDetailPanel.SetActive(false);
        //if (playerInfoComp.IsSelect)
        //{
        //    UpdateSelectPlayerInfo();
        //}
        //else
        //{
        //    UpdateSelectSoldierInfo();
        //}
    }

    public override void Uninitialize()
    {
        PlayerData.Instance._LifeSoulDepot.LifeSoulChangeEvent -= LifeSoulChange;
        if (this.SoldierObj != null)
        {
            GameObject.Destroy(this.SoldierObj);
            if (this.currentSoldier != null)
                this.currentSoldier.UpdateAttributeEvent -= soldier_UpdateAttributeEvent;
        }
        Main.Instance.StopCoroutine(UpdatePlayerLifeSpirits());
        Main.Instance.StopCoroutine(UpdateSoldierLifeSpirits());
        currentSoldier = null;
        this.soldierAnimation = "";
        Main.Instance.StopCoroutine(UpdateSoliders());
        ResourceLoadManager.Instance.ReleaseBundleForName(cacheNameList);
        if (cacheNameList != null)
            cacheNameList.Clear();
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (PlayerObj != null)
        {
            GameObject.Destroy(PlayerObj);
        }
        playerInfoComp = null;
        if (ownsoldier_dic != null)
            ownsoldier_dic.Clear();
        if (equipedSoul_dic != null)
            equipedSoul_dic.Clear();
        if (detailAttDesc_dic != null)
            detailAttDesc_dic.Clear();
        if (exchangeSoul_dic != null)
            exchangeSoul_dic.Clear();
        if (mainAttDesc_dic != null)
            mainAttDesc_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_LifeSpiritIntensify.gameObject).onClick = ButtonEvent_IntensifyButton;
        UIEventListener.Get(view.Btn_LifeSpiritChange.gameObject).onClick = ButtonEvent_ChangeButton;
        UIEventListener.Get(view.Btn_LifeSpiritUnLoad.gameObject).onClick = ButtonEvent_UnloadButton;
        UIEventListener.Get(view.Spt_LifeSpiritDetailMaskSprite.gameObject).onClick = ButtonEvent_CloseDetailView;
        UIEventListener.Get(view.Btn_CloseView.gameObject).onClick = ButtonEvent_CloseView;
        UIEventListener.Get(view.Btn_LifeSpiritBackPack.gameObject).onClick = ButtonEvent_LifeSpiritBackPack;
        UIEventListener.Get(view.Btn_HuntingLifeSpirit.gameObject).onClick = ButtonEvent_HuntingLifeSpirit;
        UIEventListener.Get(view.Btn_FastEquipButton.gameObject).onClick = ButtonEvent_FastEquipButton;
        UIEventListener.Get(view.Spt_CloseChooseMark.gameObject).onClick = ButtonEvent_CloseLifeSoulChoose;
        UIEventListener.Get(view.Gobj_PlayerInfo).onClick = ButtonEvent_PlayerInfo;
        UIEventListener.Get(view.Gobj_PlayerModel).onClick = ButtonEvent_PlayerModel;
    }


}
