using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using Assets.Script.Common;
using fogs.proto.msg;
public class GateInfoViewController : UIBase
{
    private class ReadyEquipData
    {
        public Weapon weapon;
        public int index;
        public ReadyEquipData(Weapon data, int slot)
        {
            weapon = data;
            index = slot;
        }
    }

    public GateInfoView view;
    /// <summary>
    /// 关卡数据  (服务器)
    /// </summary>
    private fogs.proto.msg.PassDungeon gateinfo;
    /// <summary>
    /// 关卡数据   仅包含配置表数据
    /// </summary>
    private StageInfo stageinfo;
    public EFightType battletype;

    private bool isDirectBattle = false;
    /// <summary>
    /// 敌方阵容
    /// </summary>
    private List<GateEnemyInfoComponent> enemy_dic;
    /// <summary>
    /// 可能获得奖励
    /// </summary>
    private List<GateAwardsComponent> awards_dic;
    /// <summary>
    /// 当前拥有的可出战士兵
    /// </summary>
    private List<OwnSoldierLineupComponent> ownsoldier_dic;
    /// <summary>
    /// 已经准备出战队的士兵
    /// </summary>
    private List<LineupItemComponent> ready_dic;

    /// <summary>
    /// 锁定的
    /// </summary>
    private List<GateInfoReadyLockComponent> readyLock_dic;

    /// <summary>
    /// 当前拥有的神器
    /// </summary>
    private List<GateInfoOwnEquipComponent> ownEquip_dic;
    /// <summary>
    /// 当前准备携带的装备
    /// </summary>
    private List<GateInfoEquipComponent> readyEquip_dic;
    /// <summary>
    /// 当前拥有的装备数据
    /// </summary>
    private List<Weapon> ownEquipList = new List<Weapon>();

    /// <summary>
    /// 准备出战士兵
    /// </summary>
    public List<Soldier> readysoldierlist;
    private ReadyEquipData[] readyEquipArray = new ReadyEquipData[6];
    ///// <summary>
    ///// 准备穿戴的装备
    ///// </summary>
    //private Weapon[] readyEquipArray = new Weapon[6];
    /// <summary>
    /// 统御力
    /// </summary>
    private int leadership = 0;
    private List<GameObject> starlist = new List<GameObject>();

    private List<CommonItemData> dropItemList;

    private int sweepCount;
    private int readyLockCount;
    private long singleSweepCD = -1;
    private ActivityDungeonInfo activityInfo;
    private EndlessDungeonInfo endlessInfo;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new GateInfoView();
            view.Initialize();
            BtnEventBinding();
        }
        FightRelatedModule.Instance.isSweep = false;
        FightRelatedModule.Instance.isFightState = false;
        if (enemy_dic == null)
            enemy_dic = new List<GateEnemyInfoComponent>();
        if (awards_dic == null)
            awards_dic = new List<GateAwardsComponent>();
        if (ready_dic == null)
            ready_dic = new List<LineupItemComponent>();
        if (ownEquip_dic == null)
            ownEquip_dic = new List<GateInfoOwnEquipComponent>();
        if (readyEquip_dic == null)
            readyEquip_dic = new List<GateInfoEquipComponent>();
        if (starlist == null)
            starlist = new List<GameObject>();
        if (ownsoldier_dic == null)
            ownsoldier_dic = new List<OwnSoldierLineupComponent>();
        if (readyLock_dic == null)
            readyLock_dic = new List<GateInfoReadyLockComponent>();
        readysoldierlist = new List<Soldier>();
        isDirectBattle = false;
        InitView();
        view.Wrap_OwnSoldier.onInitializeItem = UpdateOwnSoldierInfo;
        view.UIWrapContent_OwnEquip.onInitializeItem = UpdateOwnEquipInfo;
        PlayerData.Instance.NotifyResetEvent += UpdateResetData;
        PlayerData.Instance.UpdateLevelEvent += UpdateLevelData;
        PlayerData.Instance.UpdateVipEvent += UpdateVIPData;
        PetSystemModule.Instance.PetUpdateEvent += UpdatePetInfo;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        // PlayOpenStageInfoGroupAnim();
    }

    public bool GuideCheckID(int id)
    {
        return id == stageinfo.ID;
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(GateInfoView.UIName);
    }

    private void InitReadyEquipArray()
    {
        readyEquipArray = new ReadyEquipData[6];
        readyEquipArray[0] = new ReadyEquipData(null, 0);
        readyEquipArray[1] = new ReadyEquipData(null, 1);
        readyEquipArray[2] = new ReadyEquipData(null, 2);
        readyEquipArray[3] = new ReadyEquipData(null, 4);
        readyEquipArray[4] = new ReadyEquipData(null, 5);
        readyEquipArray[5] = new ReadyEquipData(null, 6);
    }

    private void InitView()
    {
        view.Gobj_OwnSoldierComp.SetActive(false);
        view.Gobj_EnemyInfoComp.gameObject.SetActive(false);
        view.Gobj_AwardsInfoComp.gameObject.SetActive(false);
        view.Spt_Star.gameObject.SetActive(false);
        view.Gobj_LineupSelect.gameObject.SetActive(false);
        view.Gobj_StageInfoGroup.SetActive(true);
        view.Gobj_ReadySoldierComp.SetActive(false);
        view.Gobj_LockSoldierComp.SetActive(false);
        view.Gobj_OwnSoldierComp.SetActive(false);
        view.Gobj_OwnEquipComp.SetActive(false);
        view.Gobj_EquipSelect.SetActive(false);
        view.Gobj_ReadyEquipComp.SetActive(false);
    }

    #region Update Event

    public void UpdateResetData(NotifyReset data)
    {
        if (data == null) return;
        UpdateSweepInfo();
    }

    public void UpdateVIPData()
    {
        UpdateSweepInfo();
    }

    public void UpdateLevelData()
    {
        UpdateSweepInfo();
    }

    #region 装备
    /// <summary>
    /// 检测是否超过装备上限 true:已超限
    /// </summary>
    private bool CheckEquipLimit(Weapon weapon)
    {
        switch ((EquiptType)weapon.Att.type)
        {
            case EquiptType._Weapon:
                {
                    if (readyEquipArray[0].weapon == null || readyEquipArray[1].weapon == null || readyEquipArray[2].weapon == null)
                    {
                        return false;
                    }
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_WEAPONOVERFLOW);
                }
                break;
            case EquiptType._ring:
                {
                    if (readyEquipArray[4].weapon == null || readyEquipArray[5].weapon == null || readyEquipArray[3].weapon == null)
                    {
                        return false;
                    }
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_RINGOVERLOW);
                }
                break;
            //case EquiptType._necklace:
            //    {
            //        if (readyEquipArray[3].weapon == null)
            //        {
            //            return false;
            //        }
            //        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_NECKLACEOVERFLOW);
            //    }
            //    break;
        }
        return true;
    }

    /// <summary>
    /// 更新当前准备携带的装备数据
    /// </summary>
    /// <param name="weapon"></param>
    private void UpdateReadyEquipData(Weapon weapon, bool isAdd)
    {
        if (isAdd)
        {
            switch ((EquiptType)weapon.Att.type)
            {
                case EquiptType._Weapon:
                    {
                        if (readyEquipArray[0].weapon == null)
                        {
                            readyEquipArray[0].weapon = weapon;
                        }
                        else if (readyEquipArray[1].weapon == null)
                        {
                            readyEquipArray[1].weapon = weapon;
                        }
                        else if (readyEquipArray[2].weapon == null)
                        {
                            readyEquipArray[2].weapon = weapon;
                        }
                    }
                    break;
                case EquiptType._ring:
                    {
                        if (readyEquipArray[3].weapon == null)
                        {
                            readyEquipArray[3].weapon = weapon;
                        }
                        else if (readyEquipArray[4].weapon == null)
                        {
                            readyEquipArray[4].weapon = weapon;
                        }
                        else if (readyEquipArray[5].weapon == null)
                        {
                            readyEquipArray[5].weapon = weapon;
                        }
                    }
                    break;
                //case EquiptType._necklace:
                //    {
                //        if (readyEquipArray[3].weapon == null)
                //        {
                //            readyEquipArray[3].weapon = weapon;
                //        }
                //    }
                //    break;
            }
        }
        else
        {
            for (int i = 0; i < readyEquipArray.Length; i++)
            {
                ReadyEquipData readyEquip = readyEquipArray[i];
                if (readyEquip != null && readyEquip.weapon != null)
                {
                    if (readyEquip.weapon.uId.Equals(weapon.uId))
                    {
                        readyEquip.weapon = null;
                        break;
                    }
                }

            }
        }
    }

    private void DelOwnEquipsData(Weapon weapon)
    {
        for (int i = 0; i < ownEquip_dic.Count; i++)
        {
            GateInfoOwnEquipComponent comp = ownEquip_dic[i];
            if (comp.EquipInfo.uId.Equals(weapon.uId))
            {
                comp.IsSelect = false;
                break;
            }
        }
    }

    private int GetPlayerCombatPower()
    {
        Dictionary<Soldier, int> soldierList = new Dictionary<Soldier, int>();
        List<CalBaseData> equips = new List<CalBaseData>();
        List<CalBaseData> skills = new List<CalBaseData>();
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            Weapon equip = readyEquipArray[i].weapon;
            if (equip != null)
            {
                CalBaseData equipData = new CalBaseData(equip.Att.id, equip.Level);
                if (equip._Skill != null)
                {
                    CalBaseData skillData = new CalBaseData(equip._Skill.Att.nId, equip._Skill.Level);
                    skills.Add(skillData);
                }
                equips.Add(equipData);
            }
        }
        if (PlayerData.Instance._SkillsDepot != null && PlayerData.Instance._SkillsDepot._skillsList != null)
        {
            for (int i = 0; i < PlayerData.Instance._SkillsDepot._skillsList.Count; i++)
            {
                Skill skill = PlayerData.Instance._SkillsDepot._skillsList[i];
                if (skill == null)
                    continue;
                CalBaseData skillData = new CalBaseData(skill.Att.nId, skill.Lv);
                skills.Add(skillData);
            }
        }
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            Soldier soldier = readysoldierlist[i];
            soldierList.Add(soldier, 1);  //需求PVE 数量为1
        }
        PetData petData = PlayerData.Instance._PetDepot.GetEquipedPet();
        if (petData != null)
        {
            CalBaseData skillData = new CalBaseData(petData.Skill.Att.nId, petData.Skill.Lv);
            skills.Add(skillData);
        }
        int PlayerCombatPower = Calculation_Attribute.Instance.Calculation_PlayerCombatPower(PlayerData.Instance._Level, soldierList, equips, skills);
        return PlayerCombatPower;
    }

    public void OpenEquipPanel()
    {
        view.Gobj_EquipSelect.SetActive(true);
        InitReadyEquipArray();
        List<Weapon> equipedWeapons = new List<Weapon>(PlayerData.Instance._ArtifactedDepot._EquiptList);
        for (int i = 0; i < equipedWeapons.Count; i++)
        {
            Weapon weapon = equipedWeapons[i];
            //if (weapon == null) continue;
            //if (weapon.Att.type != (int)EquiptType._mount && weapon.Att.type != (int)EquiptType._clothing)
            //{
            for (int j = 0; j < readyEquipArray.Length; j++)  //为啥位置不保存为一个变量?
            {
                ReadyEquipData readyEquip = readyEquipArray[j];
                if (i.Equals(readyEquip.index))
                {
                    readyEquip.weapon = weapon;
                }
            }
            //}
        }
        UpdateEquipPanel();
        Main.Instance.StartCoroutine(CreateReadyEquipComps());
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenGodWeaponChoose);
        UpdatePetInfo();
    }

    private void CloseEquipPanel()
    {
        view.Gobj_EquipSelect.SetActive(false);
    }

    private void UpdateEquipPanel()
    {
        UpdateOwnEquipsData();
        Main.Instance.StartCoroutine(CreateOwnEquips());
        UpdateCombatPower();
    }
    /// <summary>
    /// 更新战斗力
    /// </summary>
    private void UpdateCombatPower()
    {
        view.Lbl_EquipCombatPower.text = GetPlayerCombatPower().ToString();
    }

    private void UpdateOwnEquipsData()
    {
        if (ownEquipList == null)
            ownEquipList = new List<Weapon>();
        ownEquipList.Clear();
        List<Weapon> list = PlayerData.Instance._WeaponDepot.GetLockAndUnlockList();
        if (list == null)
        {
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            Weapon weapon = list[i];
            if (weapon.Att.type != (int)EquiptType._mount && weapon.Att.type != (int)EquiptType._clothing && weapon.Att.type != (int)EquiptType._necklace)
            {
                ownEquipList.Add(weapon);
            }
        }
    }

    private void UpdateOwnEquipInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_OwnEquip.enabled == false) return;
        if (realIndex >= ownEquipList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            GateInfoOwnEquipComponent comp = ownEquip_dic[wrapIndex];
            Weapon _info = ownEquipList[realIndex];
            comp.UpdateCompInfo(_info);
            comp.IsSelect = IsSelectedOwnEquip(_info);
        }
    }

    /// <summary>
    /// 当前拥有的装备
    /// </summary>
    private IEnumerator CreateOwnEquips()
    {
        view.ScrView_OwnEquip.ResetPosition();
        int MAXCOUNT = 28;
        int count = ownEquipList.Count;
        int itemCount = ownEquip_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_OwnEquip.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_OwnEquip.minIndex = -index;
        view.UIWrapContent_OwnEquip.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_OwnEquip.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_OwnEquip.enabled = false;
        }
        yield return null;
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                ownEquip_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            GateInfoOwnEquipComponent comp = null;
            if (i < itemCount)
            {
                comp = ownEquip_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_OwnEquipComp, view.Grd_OwnEquip.transform);
                comp = new GateInfoOwnEquipComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_OwnEquip);
                ownEquip_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(ownEquipList[i]);
            comp.callBack = ButtonEvent_ArtifactDetail;
            comp.IsSelect = IsSelectedOwnEquip(ownEquipList[i]);
            comp.mRootObject.SetActive(true);
        }
        yield return null;
        view.UIWrapContent_OwnEquip.ReGetChild();
        yield return null;
        view.Grd_OwnEquip.repositionNow = true;
        yield return null;
        view.ScrView_OwnEquip.ResetPosition();
    }

    private IEnumerator CreateReadyEquipComps()
    {
        if (readyEquip_dic.Count < 6)
        {
            int index = readyEquip_dic.Count;
            for (int i = index; i < 6; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_ReadyEquipComp.gameObject, view.Grd_RedayEquipGrid.transform);
                GateInfoEquipComponent comp = new GateInfoEquipComponent();
                go.name = "index_" + index;
                comp.MyStart(go);
                comp.AddEventListener(ButtonEvent_ReadyEquip);
                go.name = i.ToString();
                go.SetActive(false);
                readyEquip_dic.Add(comp);
            }
            yield return null;
            view.Grd_RedayEquipGrid.repositionNow = true;   //因为生成后位置就已固定  不在需要设定位置 add by taiwei
        }
        yield return null;
        UpdateReadyEquips();
    }

    /// <summary>
    /// 当前准备携带的装备
    /// </summary>
    public void UpdateReadyEquips()
    {
        UpdateCombatPower();
        for (int index = 0; index < readyEquip_dic.Count; index++)
        {
            GateInfoEquipComponent comp = readyEquip_dic[index];
            if (index < readyEquipArray.Length)
            {
                Weapon weapon = readyEquipArray[index].weapon;
                if (weapon == null)
                {
                    comp.mRootObject.SetActive(false);
                }
                else
                {
                    comp.UpdateInfo(weapon);
                    comp.mRootObject.SetActive(true);
                }
            }
            else
            {
                comp.mRootObject.SetActive(false);
            }
        }
    }

    private void UpdatePetInfo()
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Pet, false))
        {
            view.Gobj_EquipPet.SetActive(false);
            return;
        }
        List<PetData> petList = PlayerData.Instance._PetDepot.GetOwnedPets();
        if (petList == null || petList.Count == 0)
        {
            view.Gobj_EquipPet.SetActive(false);
        }
        else
        {
            PetData petData = PlayerData.Instance._PetDepot.GetEquipedPet();
            if (petData == null)
            {
                view.Lbl_EquipedPetName.text = string.Format(ConstString.PET_PREPAREBATTLE_EQUIPEDPET, ConstString.HINT_NO);
            }
            else
            {
                view.Lbl_EquipedPetName.text = string.Format(ConstString.PET_PREPAREBATTLE_EQUIPEDPET, petData.PetInfo.name);
            }
            view.Gobj_EquipPet.SetActive(true);
        }
    }


    #endregion

    public void UpdateViewInfo(ActivityDungeonInfo info, StageInfo stage, fogs.proto.msg.PassDungeon gate, int remainCount, EFightType vFightType)
    {
        activityInfo = info;
        UpdateViewInfo(stage, gate, remainCount, vFightType);
    }

    public void UpdateViewInfo(EndlessDungeonInfo info, StageInfo stage, EFightType type)
    {
        sweepCount = 0;
        stageinfo = stage;
        gateinfo = null;
        battletype = type;
        endlessInfo = info;
        if (stageinfo == null)
        {
            return;
        }
        view.Lbl_SPLabel.text = stageinfo.Physical.ToString();
        view.Lbl_StageDesc.text = stageinfo.Describe;
        view.Lbl_TitleLabel.text = stageinfo.Name;
        UpdateEnemyCast();
        dropItemList = CommonFunction.GetCommonItemDataList(stageinfo.DropID);
        Main.Instance.StartCoroutine(UpdateAwards());
        switch (battletype)
        {
            //case EFightType.eftActivity:
            //    {
            //        view.Gobj_SweepGroup.SetActive(false);
            //        view.Spt_Elite.enabled = false;
            //        view.Gobj_StarsGroup.SetActive(true);
            //        view.Gobj_GateCountGroup.SetActive(false);
            //        view.Gobj_PurchaesGroup.SetActive(false);
            //        UpdateGrades();
            //    } break;
            case EFightType.eftEndless:
                {
                    view.Gobj_SweepGroup.SetActive(true);
                    view.Spt_Elite.enabled = false;
                    view.Gobj_StarsGroup.SetActive(false);
                    view.Gobj_GateCountGroup.SetActive(true);
                    view.Gobj_PurchaesGroup.SetActive(false);
                    view.Lbl_GateCountLabel.text = string.Format(ConstString.ENDLESS_TOTALGATECOUNT, GlobalCoefficient.ExpeditionGateCount);
                }
                break;
        }
        UpdateButtomInfo();
        UpdateSweepInfo();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenLevelInfoView);
    }

    public void UpdateViewInfo(StageInfo stage, fogs.proto.msg.PassDungeon gate, int remainCount, EFightType vFightType)
    {

        stageinfo = stage;
        gateinfo = gate;
        battletype = vFightType;
        if (stageinfo == null)
        {
            return;
        }
        view.Lbl_SPLabel.text = stageinfo.Physical.ToString();
        view.Lbl_StageDesc.text = stageinfo.Describe;
        //if (stage.IsElite == (int)MainBattleType.Crusade)
        //{
        view.Lbl_TitleLabel.text = stageinfo.Name;
        //}
        //else
        //{
        //    view.Lbl_TitleLabel.text = stageinfo.Name;
        //}
        UpdateButtomInfo();
        UpdateEnemyCast();
        dropItemList = CommonFunction.GetCommonItemDataList(stageinfo.DropID);
        Main.Instance.StartCoroutine(UpdateAwards());
        switch (battletype)
        {
            case EFightType.eftMain:
                {
                    if (stageinfo.IsElite == (byte)(MainBattleType.Crusade))
                    {
                        //view.Gobj_PurchaesGroup.SetActive(false);
                        view.Spt_Elite.enabled = false;
                    }
                    else if (stageinfo.IsElite == (byte)(MainBattleType.Escort))
                    {
                        //view.Gobj_PurchaesGroup.SetActive(true);
                        view.Spt_Elite.enabled = true;
                    }
                    view.Gobj_StarsGroup.SetActive(true);
                    view.Gobj_GateCountGroup.SetActive(false);
                    UpdateGrades();
                }
                break;
            case EFightType.eftActivity:
                {
                    //view.Gobj_SweepGroup.SetActive(false);
                    view.Spt_Elite.enabled = false;
                    view.Gobj_StarsGroup.SetActive(true);
                    view.Gobj_GateCountGroup.SetActive(false);
                    view.Gobj_PurchaesGroup.SetActive(false);
                    UpdateGrades();
                } break;
            case EFightType.eftEndless:
                {
                    view.Gobj_SweepGroup.SetActive(true);
                    view.Spt_Elite.enabled = false;
                    view.Gobj_StarsGroup.SetActive(false);
                    view.Gobj_GateCountGroup.SetActive(true);
                    view.Gobj_PurchaesGroup.SetActive(false);
                    view.Lbl_GateCountLabel.text = string.Format(ConstString.ENDLESS_TOTALGATECOUNT, GlobalCoefficient.ExpeditionGateCount);
                }
                break;
        }
        UpdateSweepInfo();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenLevelInfoView);
    }

    private void UpdateButtomInfo()
    {
        switch (battletype)
        {
            case EFightType.eftMain:
            case EFightType.eftActivity:
                {
                    view.Gobj_PurchaesGroup.transform.localPosition = new Vector3(-355, -230, 0);
                    view.Gobj_SweepGroup.transform.localPosition = new Vector3(-92, -230, 0);
                    view.Gobj_SPGroup.transform.localPosition = new Vector3(-335, -65);
                    view.Btn_DirectBattle.gameObject.SetActive(true);
                } break;
            case EFightType.eftEndless:
                {
                    view.Gobj_PurchaesGroup.transform.localPosition = new Vector3(-320, -230, 0);
                    view.Gobj_SweepGroup.transform.localPosition = new Vector3(0, -230, 0);
                    view.Gobj_SPGroup.transform.localPosition = new Vector3(-195, -65);
                    view.Btn_DirectBattle.gameObject.SetActive(false);
                } break;
        }
    }

    private void UpdateSweepInfo()
    {
        switch (battletype)
        {
            case EFightType.eftMain:
                {
                    view.Gobj_SurplusSweepCard.transform.localPosition = new Vector3(-140, 50, 0);
                    sweepCount = GetSweepCount();
                    if (stageinfo.ChallengeCount <= 0)
                    {
                        view.Gobj_PurchaesGroup.SetActive(false);
                    }
                    else
                    {
                        view.Gobj_PurchaesGroup.SetActive(true);
                        view.Lbl_SurplusCount.text = string.Format(ConstString.GATE_TODAYSWEEPCOUNT, sweepCount.ToString());
                        if (sweepCount <= 0)
                        {
                            view.Btn_Purchase.gameObject.SetActive(true);
                        }
                        else
                        {
                            view.Btn_Purchase.gameObject.SetActive(false);
                        }
                    }
                    if (gateinfo == null)
                    {
                        view.Gobj_SweepGroup.SetActive(false);
                        return;
                    }
                } break;
            case EFightType.eftActivity:
                {
                    ActivityDungeonInfo info = GetActivityInfo();
                    sweepCount = info.max_times - info.today_times;
                    if (sweepCount <= 0)
                        sweepCount = 0;
                    view.Gobj_PurchaesGroup.SetActive(true);
                    view.Lbl_SurplusCount.text = string.Format(ConstString.GATE_TODAYSWEEPCOUNT, sweepCount.ToString());
                    if (sweepCount > 0)
                    {
                        view.Btn_Purchase.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.Btn_Purchase.gameObject.SetActive(true);
                    }
                    if (gateinfo == null)
                    {
                        view.Gobj_SweepGroup.SetActive(false);
                        return;
                    }
                } break;
            case EFightType.eftEndless:
                {
                    EndlessDungeonInfo info = GetEndlessInfo();
                    sweepCount = info.max_times - info.today_times;
                    //表示功能不开放
                    if (ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Endless) < 0)
                    {
                        if (sweepCount <= 0)
                            sweepCount = 0;
                        view.Gobj_PurchaesGroup.SetActive(true);
                        view.Lbl_SurplusCount.text = string.Format(ConstString.GATE_TODAYSWEEPCOUNT, sweepCount.ToString());
                        view.Btn_Purchase.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (sweepCount <= 0)
                        {
                            sweepCount = 0;
                            view.Btn_Purchase.gameObject.SetActive(true);
                            view.Lbl_StarUnlockTip.gameObject.SetActive(false);
                        }
                        else
                        {
                            view.Btn_Purchase.gameObject.SetActive(false);
                            view.Lbl_StarUnlockTip.gameObject.SetActive(true);
                        }
                        view.Gobj_PurchaesGroup.SetActive(true);
                        view.Lbl_SurplusCount.text = string.Format(ConstString.GATE_TODAYSWEEPCOUNT, sweepCount.ToString());
                    }
                } break;
        }
        //if (gateinfo.star_level < 3)
        //{
        //    view.Gobj_SweepGroup.SetActive(false);
        //}
        //else
        //{
        if (battletype == EFightType.eftMain)
        {
            if (stageinfo.IsBoss == false)
            {
                view.Gobj_SweepGroup.SetActive(false);
                return;
            }
        }
        view.Gobj_SweepGroup.SetActive(true);
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Clear, false))   //首先判定单次扫荡是否开启
        {
            view.Gobj_SingleSweepGroup.gameObject.SetActive(true);
            view.Gobj_MulitSweepGroup.gameObject.SetActive(false);
            view.Gobj_SingleSweepTip.SetActive(true);
            view.Gobj_SingleSweepTip.transform.localPosition = new Vector3(0, 40, 0);
            view.Gobj_SingleSweepDiamondTip.SetActive(false);
            view.Gobj_MulitSweepDiamondTip.SetActive(false);
            OpenLevelData data = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Clear);
            if (data != null)
            {
                if ((data.vipLevel > 0) && (data.openLevel <= 0))
                {
                    view.Spt_SingleSweepVIP.enabled = true;
                    view.Lbl_SingleSweepTipLevel.enabled = true;
                    view.Lbl_SingleSweepTipLevel.text = data.vipLevel.ToString();
                    view.Lbl_SingleSweepTip.text = ConstString.GATE_ESCORT_LOCKTIP_UNLOCK;
                }
                else if ((data.openLevel > 0) && (data.vipLevel <= 0))
                {
                    view.Spt_SingleSweepVIP.enabled = false;
                    view.Lbl_SingleSweepTipLevel.enabled = false;
                    view.Lbl_SingleSweepTip.text = string.Format(ConstString.BACKPACK_LEVELLOCKTIP, data.openLevel);
                }
                else if ((data.openLevel > 0) && (data.vipLevel > 0))
                {
                    view.Spt_SingleSweepVIP.enabled = true;
                    view.Lbl_SingleSweepTipLevel.enabled = true;
                    view.Lbl_SingleSweepTipLevel.text = data.vipLevel.ToString();
                    view.Lbl_SingleSweepTip.text = ConstString.GATE_SWEEPTIP_EITHER + string.Format(ConstString.BACKPACK_LEVELLOCKTIP, data.openLevel);
                }
                else
                {
                    view.Spt_SingleSweepVIP.enabled = false;
                    view.Lbl_SingleSweepTipLevel.enabled = false;
                    view.Lbl_SingleSweepTip.text = string.Empty;
                }
            }
            view.Gobj_MulitSweepTip.SetActive(false);
            view.Gobj_SurplusSweepCard.SetActive(false);
            //view.Btn_SingleSweep.isEnabled = false;
            view.Lbl_StarUnlockTip.enabled = false;
            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, true);
            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, false);
            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
        }
        else
        {
            switch (battletype)
            {
                case EFightType.eftActivity:
                case EFightType.eftMain:
                    {
                        view.Gobj_SurplusSweepCard.transform.localPosition = new Vector3(-140, 50, 0);
                        view.Gobj_SingleSweepGroup.gameObject.SetActive(true);
                        view.Gobj_MulitSweepGroup.gameObject.SetActive(true);
                        //view.Grd_SweepBtnGrid.repositionNow = true;
                        view.Gobj_SingleSweepTip.SetActive(false);
                        view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.BatchClear, false))
                        {
                            OpenLevelData mutileData = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.BatchClear);
                            if (mutileData != null)
                            {
                                if ((mutileData.vipLevel > 0) && (mutileData.openLevel <= 0))
                                {
                                    view.Spt_MulitSweepVIP.enabled = true;
                                    view.Lbl_MulitSweepTipLevel.enabled = true;
                                    view.Lbl_MulitSweepTipLevel.text = mutileData.vipLevel.ToString();
                                    view.Lbl_MulitSweepTip.text = ConstString.GATE_ESCORT_LOCKTIP_UNLOCK;
                                    view.Gobj_MulitSweepTip.transform.localPosition = new Vector3(175, 40, 0);
                                }
                                else if ((mutileData.openLevel > 0) && (mutileData.vipLevel <= 0))
                                {
                                    view.Spt_MulitSweepVIP.enabled = false;
                                    view.Lbl_MulitSweepTipLevel.enabled = false;
                                    view.Lbl_MulitSweepTip.text = string.Format(ConstString.BACKPACK_LEVELLOCKTIP, mutileData.openLevel);
                                    view.Gobj_MulitSweepTip.transform.localPosition = new Vector3(135, 40, 0);
                                }
                                else if ((mutileData.openLevel > 0) || (mutileData.vipLevel > 0))
                                {
                                    view.Spt_MulitSweepVIP.enabled = true;
                                    view.Lbl_MulitSweepTipLevel.enabled = true;
                                    view.Lbl_MulitSweepTipLevel.text = mutileData.vipLevel.ToString();
                                    view.Lbl_MulitSweepTip.text = ConstString.GATE_SWEEPTIP_EITHER + string.Format(ConstString.BACKPACK_LEVELLOCKTIP, mutileData.openLevel);
                                    view.Gobj_MulitSweepTip.transform.localPosition = new Vector3(150, 40, 0);
                                }
                                else
                                {
                                    view.Spt_MulitSweepVIP.enabled = false;
                                    view.Lbl_MulitSweepTipLevel.enabled = false;
                                    view.Lbl_MulitSweepTip.text = string.Empty;
                                }
                                view.Lbl_MulitSweepTipLevel.text = mutileData.vipLevel.ToString();
                                view.Gobj_MulitSweepDiamondTip.SetActive(false);
                            }
                            switch (battletype)
                            {
                                case EFightType.eftActivity:
                                case EFightType.eftMain:
                                    {
                                        if (gateinfo.star_level < 3)  //小于三星不能扫荡
                                        {
                                            view.Lbl_StarUnlockTip.text = ConstString.GATE_SWEEPTIP_STARLIMIT;//TODO：计算位置
                                            view.Lbl_StarUnlockTip.transform.localPosition = new Vector3(-160, 40, 0);
                                            //view.Btn_SingleSweep.isEnabled = false;
                                            view.Lbl_StarUnlockTip.enabled = true;
                                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, true);
                                            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, false);
                                            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                                            view.Gobj_MulitSweepDiamondTip.SetActive(false);
                                            view.Gobj_SingleSweepDiamondTip.SetActive(false);
                                            view.Gobj_SurplusSweepCard.SetActive(false);
                                        }
                                        else
                                        {
                                            //view.Btn_SingleSweep.isEnabled = true;
                                            view.Lbl_StarUnlockTip.enabled = false;
                                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, false);
                                            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, true);
                                            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                                            UpdateSweepCard();
                                        }
                                    } break;
                            }
                            //view.Btn_MulitSweep.isEnabled = false;
                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnMulitSweepBG, true);
                            //CommonFunction.SetUILabelColor(view.Lbl_BtnMulitSweep, false);
                            view.Gobj_MulitSweepTip.SetActive(true);
                            view.Gobj_MulitSweepDiamondTip.SetActive(false);
                        }
                        else
                        {   //判定多次扫荡已开启 则一定说明单次扫荡已经开启
                            view.Gobj_MulitSweepTip.SetActive(false);
                            switch (battletype)
                            {
                                case EFightType.eftMain:
                                    {
                                        if (gateinfo.star_level < 3)  //小于三星不能扫荡
                                        {
                                            view.Lbl_StarUnlockTip.text = ConstString.GATE_SWEEPTIP_STARLIMIT;//TODO：计算位置
                                            view.Lbl_StarUnlockTip.transform.localPosition = new Vector3(-160, 40, 0);
                                            //view.Btn_SingleSweep.isEnabled = false;
                                            view.Lbl_StarUnlockTip.enabled = true;
                                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, true);
                                            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, false);
                                            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                                            //view.Btn_MulitSweep.isEnabled = false;
                                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnMulitSweepBG, true);
                                            //CommonFunction.SetUILabelColor(view.Lbl_BtnMulitSweep, false);
                                            view.Gobj_MulitSweepDiamondTip.SetActive(false);
                                            view.Gobj_SingleSweepDiamondTip.SetActive(false);
                                            view.Gobj_SurplusSweepCard.SetActive(false);
                                        }
                                        else
                                        {
                                            //view.Btn_SingleSweep.isEnabled = true;
                                            view.Lbl_StarUnlockTip.enabled = false;
                                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, false);
                                            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, true);
                                            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                                            if (stageinfo.ChallengeCount <= 0)
                                            {
                                                //view.Btn_MulitSweep.isEnabled = true;
                                                //CommonFunction.UpdateWidgetGray(view.Spt_BtnMulitSweepBG, false);
                                                //CommonFunction.SetUILabelColor(view.Lbl_BtnMulitSweep, true);
                                            }
                                            else
                                            {
                                                if (sweepCount <= 0)
                                                {
                                                    //view.Btn_MulitSweep.isEnabled = false;
                                                    //CommonFunction.UpdateWidgetGray(view.Spt_BtnMulitSweepBG, true);
                                                    //CommonFunction.SetUILabelColor(view.Lbl_BtnMulitSweep, false);
                                                }
                                                else
                                                {
                                                    //view.Btn_MulitSweep.isEnabled = true;
                                                    //CommonFunction.SetUILabelColor(view.Lbl_BtnMulitSweep, true);
                                                    //CommonFunction.UpdateWidgetGray(view.Spt_BtnMulitSweepBG, false);
                                                }
                                            }
                                            UpdateSweepCard();
                                        }
                                    }
                                    break;
                                case EFightType.eftActivity:
                                    {
                                        if (gateinfo.star_level < 3)  //小于三星不能扫荡
                                        {
                                            view.Lbl_StarUnlockTip.text = ConstString.GATE_SWEEPTIP_STARLIMIT;//TODO：计算位置
                                            view.Lbl_StarUnlockTip.transform.localPosition = new Vector3(-160, 40, 0);
                                            //view.Btn_SingleSweep.isEnabled = false;
                                            view.Lbl_StarUnlockTip.enabled = true;
                                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, true);
                                            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, false);
                                            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                                            //view.Btn_MulitSweep.isEnabled = false;
                                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnMulitSweepBG, true);
                                            //CommonFunction.SetUILabelColor(view.Lbl_BtnMulitSweep, false);
                                            view.Gobj_MulitSweepDiamondTip.SetActive(false);
                                            view.Gobj_SingleSweepDiamondTip.SetActive(false);
                                            view.Gobj_SurplusSweepCard.SetActive(false);
                                        }
                                        else
                                        {
                                            //view.Btn_SingleSweep.isEnabled = true;
                                            view.Lbl_StarUnlockTip.enabled = false;
                                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, false);
                                            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, true);
                                            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                                            if (sweepCount <= 0)
                                            {
                                                //view.Btn_MulitSweep.isEnabled = false;
                                                //CommonFunction.UpdateWidgetGray(view.Spt_BtnMulitSweepBG, true);
                                                //CommonFunction.SetUILabelColor(view.Lbl_BtnMulitSweep, false);
                                            }
                                            else
                                            {
                                                //view.Btn_MulitSweep.isEnabled = true;
                                                //CommonFunction.SetUILabelColor(view.Lbl_BtnMulitSweep, true);
                                                //CommonFunction.UpdateWidgetGray(view.Spt_BtnMulitSweepBG, false);
                                            }
                                            UpdateSweepCard();
                                        }
                                    } break;
                            }
                        }
                    } break;
                case EFightType.eftEndless:
                    {
                        view.Gobj_MulitSweepGroup.SetActive(false);
                        view.Gobj_MulitSweepTip.SetActive(false);
                        view.Gobj_SurplusSweepCard.transform.localPosition = new Vector3(-60, 50, 0);
                        view.Gobj_SingleSweepGroup.SetActive(true);
                        view.Gobj_SingleSweepTip.SetActive(false);
                        //view.Grd_SweepBtnGrid.repositionNow = true;
                        if (endlessInfo.passed == 0)  //[0=false, !0=true]
                        {
                            //view.Lbl_StarUnlockTip.text = ConstString.GATE_SWEEPTIP_ENDLESS;
                            //view.Lbl_StarUnlockTip.transform.localPosition = new Vector3(-15, 40, 0);
                            //view.Btn_SingleSweep.isEnabled = false;
                            view.Lbl_StarUnlockTip.enabled = true;
                            view.Lbl_StarUnlockTip.transform.localPosition = new Vector3(-415, -5, 0);
                            view.Lbl_StarUnlockTip.text = ConstString.GATE_SWEEPTIP_ENDLESSCLOSE;
                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, true);
                            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, false);
                            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                            view.Gobj_MulitSweepDiamondTip.SetActive(false);
                            view.Gobj_SingleSweepDiamondTip.SetActive(false);
                            //view.Gobj_SurplusSweepCard.SetActive(false);
                        }
                        else
                        {

                            view.Lbl_StarUnlockTip.transform.localPosition = new Vector3(-415, -5, 0);
                            //view.Btn_SingleSweep.isEnabled = true;
                            view.Lbl_StarUnlockTip.enabled = true;
                            //CommonFunction.UpdateWidgetGray(view.Spt_BtnSingleSweepBG, false);
                            //CommonFunction.SetUILabelColor(view.Lbl_BtnSingleSweep, true);
                            view.Lbl_BtnSingleSweep.text = ConstString.GATE_INFOPANEL_SWEEP;
                            view.Lbl_StarUnlockTip.text = string.Format(ConstString.GATE_SWEEPTIP_ENDLESS, endlessInfo.high_victory);
                        }
                        UpdateSweepCard();
                    } break;
                //}
            }
        }

        UpdateSweepCount();
        view.Grd_SweepBtnGrid.repositionNow = true;
    }

    private void UpdateSweepCard()
    {
        int sweepCard = PlayerData.Instance.GetSweepCard();  //扫荡卡
        if (sweepCard <= 0)
        {
            view.Gobj_SurplusSweepCard.SetActive(false);
            view.Gobj_SingleSweepDiamondTip.SetActive(true);
            view.Gobj_MulitSweepDiamondTip.SetActive(true);
            string diamondstr = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_SWEEP_CONSUMEDIAMOND);
            int diamond = 0;
            int.TryParse(diamondstr, out diamond);
            view.Lbl_SingleSweepDiamondTip.text = "x" + diamond.ToString();
        }
        else
        {
            view.Gobj_SurplusSweepCard.SetActive(true);
            view.Lbl_SweepCardCount.text = "x" + sweepCard.ToString();
            view.Gobj_SingleSweepDiamondTip.SetActive(false);
            view.Gobj_MulitSweepDiamondTip.SetActive(false);
        }
    }

    private void UpdateLockInfo()
    {
        OpenLevelData opendata3 = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.GateSoldierLockLevel3);
        OpenLevelData opendata4 = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.GateSoldierLockLevel4);
        OpenLevelData opendata5 = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.GateSoldierLockLevel5);
        OpenLevelData opendata6 = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.GateSoldierLockLevel6);
        if (opendata3 == null || opendata4 == null || opendata5 == null || opendata6 == null) return;
        List<OpenLevelData> list = new List<OpenLevelData>();
        if (!ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.GateSoldierLockLevel3, false))
        {
            readyLockCount = 2;
            list.Add(opendata3);
            list.Add(opendata4);
            list.Add(opendata5);
            list.Add(opendata6);
        }
        else if (!ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.GateSoldierLockLevel4, false))
        {
            readyLockCount = 3;
            list.Add(opendata4);
            list.Add(opendata5);
            list.Add(opendata6);
        }
        else if (!ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.GateSoldierLockLevel5, false))
        {
            readyLockCount = 4;
            list.Add(opendata5);
            list.Add(opendata6);
        }
        else if (!ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.GateSoldierLockLevel6, false))
        {
            readyLockCount = 5;
            list.Add(opendata6);
        }
        else
        {
            readyLockCount = 6;
        }
        UpdateLock(list);
    }

    private void UpdateLock(List<OpenLevelData> list)
    {
        view.Grd_LockSoldierGrid.gameObject.SetActive(true);
        if (list.Count <= readyLock_dic.Count)
        {
            for (int i = 0; i < readyLock_dic.Count; i++)
            {
                GateInfoReadyLockComponent comp = readyLock_dic[i];
                if (i < list.Count)
                {
                    comp.mRootObject.SetActive(true);
                    OpenLevelData data = list[i];
                    if (data != null)
                    {
                        StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID((uint)data.gateId);
                        if (info != null)
                        {
                            comp.UpdateInfo(info.GateSequence);
                        }
                    }
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int index = readyLock_dic.Count;
            for (int i = 0; i < list.Count; i++)
            {
                GateInfoReadyLockComponent comp = null;
                if (i < index)
                {
                    comp = readyLock_dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_LockSoldierComp.gameObject, view.Grd_LockSoldierGrid.transform);
                    comp = new GateInfoReadyLockComponent();
                    comp.MyStart(go);
                    readyLock_dic.Add(comp);
                }
                if (comp == null) return;
                comp.mRootObject.SetActive(true);
                OpenLevelData data = list[i];
                if (data != null)
                {
                    StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID((uint)data.gateId);
                    if (info != null)
                    {
                        comp.UpdateInfo(info.GateSequence);
                    }
                }
            }
        }
        view.Grd_LockSoldierGrid.repositionNow = true;
    }

    private void UpdateOwnSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.Wrap_OwnSoldier.enabled == false) return;
        List<Soldier> ownSoldierList = new List<Soldier>(PlayerData.Instance._SoldierDepot._soldierList);
        if (realIndex >= ownSoldierList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            OwnSoldierLineupComponent comp = ownsoldier_dic[wrapIndex];
            comp.UpdateCompInfo(ownSoldierList[realIndex]);
            comp.IsShowEnergy = true;
            comp.IsSelect = IsSelectedOwnSoldier(ownSoldierList[realIndex]);
            if (comp.IsSelect)
            {
                comp.IsMask = false;
            }
            if (!comp.IsSelect)
            {
                comp.IsEnable = !IsSameSoldier(comp.soldierAtt);
            }
        }
    }

    private bool IsSelectedOwnSoldier(Soldier soldier)
    {
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            Soldier readySoldier = readysoldierlist[i];
            if (readySoldier.uId == soldier.uId)
            {
                return true;
            }
        }
        return false;
    }


    private void UpdateEnemyCast()
    {
        if (stageinfo.EnemySquad.Count <= enemy_dic.Count)
        {
            for (int index = 0; index < enemy_dic.Count; index++)
            {
                GateEnemyInfoComponent comp = enemy_dic[index];
                if (index < stageinfo.EnemySquad.Count)
                {
                    SingleEnemyInfo _singleinfo = stageinfo.EnemySquad[index];
                    if (_singleinfo == null) continue;
                    MonsterAttributeInfo _monster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(_singleinfo.MonsterID);
                    if (_monster == null) continue;
                    if (_singleinfo.IsBoss == 1)
                    {
                        comp.UpdateInfo(_monster, true);
                    }
                    else
                    {
                        comp.UpdateInfo(_monster, false);
                    }
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.Clear();
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int enemyObj_count = enemy_dic.Count;
            for (int index = 0; index < stageinfo.EnemySquad.Count; index++)
            {
                SingleEnemyInfo _singleinfo = stageinfo.EnemySquad[index];
                if (_singleinfo == null) continue;
                MonsterAttributeInfo _monster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(_singleinfo.MonsterID);
                if (_monster == null) continue;
                GateEnemyInfoComponent comp = null;
                if (index < enemyObj_count)
                {
                    comp = enemy_dic[index];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_EnemyInfoComp, view.Grd_EnemyGrid.transform);
                    comp = new GateEnemyInfoComponent(go);
                    go.name = _monster.ID.ToString();
                    enemy_dic.Add(comp);
                }
                if (_singleinfo.IsBoss == 1)
                {
                    comp.UpdateInfo(_monster, true);
                }
                else
                {
                    comp.UpdateInfo(_monster, false);
                }
                comp.AddPressLisetener(ButtonEvent_EnemyComp);
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_EnemyGrid.repositionNow = true;
    }



    /// <summary>
    /// 更新掉落包数据
    /// </summary>
    private IEnumerator UpdateAwards()
    {

        if (dropItemList != null)
        {
            if (dropItemList.Count <= awards_dic.Count)
            {
                for (int i = 0; i < awards_dic.Count; i++)
                {
                    GateAwardsComponent comp = awards_dic[i];
                    if (comp == null) continue;
                    if (i < dropItemList.Count)
                    {
                        CommonItemData award = dropItemList[i];
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
                        if (!comp.mRootObject.activeSelf)
                            comp.mRootObject.SetActive(true);
                    }
                    else
                    {
                        comp.Clear();
                        if (comp.mRootObject.activeSelf)
                            comp.mRootObject.SetActive(false);
                    }
                }
            }
            else
            {
                int count = GlobalCoefficient.GateAwardsCountLimit;
                if (dropItemList.Count <= count)
                {
                    count = dropItemList.Count;
                }
                for (int i = 0; i < count; i++)
                {
                    CommonItemData award = dropItemList[i];
                    GateAwardsComponent comp = null;
                    if (i < awards_dic.Count)
                    {
                        comp = awards_dic[i];
                    }
                    else
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Gobj_AwardsInfoComp, view.Grd_AwardsGrid.transform);
                        comp = new GateAwardsComponent(go);
                        awards_dic.Add(comp);
                    }
                    if (comp == null) continue;
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
                    }
                    if (isSpecialID)
                    {
                        comp.UpdateCompInfo(award.ID, award.SubType, CommonFunction.GetIconNameByID(award.ID), (int)ItemQualityEnum.White);
                    }
                    else
                    {
                        comp.UpdateCompInfo(award.ID, award.SubType, award.Icon, (int)award.Quality);
                    }
                    comp.AddPressLisetener(ButtonEvent_AwardComp);
                    comp.mRootObject.SetActive(true);
                }
            }
        }
        yield return null;
        view.Grd_AwardsGrid.repositionNow = true;
    }

    /// <summary>
    /// 更新星级评价
    /// </summary>
    private void UpdateGrades()
    {
        if (gateinfo == null)  //为NULL 则说明为无尽模式  没有星级等评定条件 
        {
            view.Grd_StarsGrid.gameObject.SetActive(false);
            return;
        }
        view.Grd_StarsGrid.gameObject.SetActive(true);
        if (gateinfo.star_level <= starlist.Count)
        {
            for (int i = 0; i < starlist.Count; i++)
            {
                if (i < gateinfo.star_level)
                {
                    starlist[i].gameObject.SetActive(true);
                }
                else
                {
                    starlist[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            int index = starlist.Count;
            for (int i = 0; i < starlist.Count; i++)
            {
                starlist[i].gameObject.SetActive(true);
            }
            for (int i = index; i < gateinfo.star_level; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Spt_Star.gameObject, view.Grd_StarsGrid.transform);
                starlist.Add(go);
                go.SetActive(true);
            }
        }
        view.Grd_StarsGrid.repositionNow = true;
    }

    private void UpdateLineupPanelInfo()
    {
        readysoldierlist.Clear();
        switch (battletype)
        {
            case EFightType.eftActivity:
                {
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.ActivityReadySoldier))
                    {
                        List<ulong> list = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.ActivityReadySoldier);
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(list[i]);
                                if (soldier == null) continue;
                                readysoldierlist.Add(soldier);
                            }
                        }
                    }
                } break;
            case EFightType.eftEndless:
                {
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.EndlessReadySoldier))
                    {
                        List<ulong> list = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.EndlessReadySoldier);
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(list[i]);
                                if (soldier == null) continue;
                                readysoldierlist.Add(soldier);
                            }
                        }
                    }
                }
                break;
            case EFightType.eftMain:
                {
                    List<ulong> list = PlayerData.Instance._MajorDungeonSoldierList;
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(list[i]);
                            if (soldier == null) continue;
                            readysoldierlist.Add(soldier);
                        }
                    }
                    //if (PlayerPrefsTool.HasKey(AppPrefEnum.GateReadySoldier))
                    //{
                    //    List<ulong> list = PlayerPrefsTool.ReadString<List<ulong>>(AppPrefEnum.GateReadySoldier);
                    //    if (list != null)
                    //    {
                    //        for (int i = 0; i < list.Count; i++)
                    //        {
                    //            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(list[i]);
                    //            if (soldier == null) continue;
                    //            readysoldierlist.Add(soldier);
                    //        }
                    //    }
                    //}
                } break;
        }

        UpdateLeadership();
        Main.Instance.StartCoroutine(CreateOwnSoldier());
        if (battletype == EFightType.eftMain)
        {
            readyLockCount = 6;
            UpdateLockInfo();
        }
        else
        {
            view.Grd_LockSoldierGrid.gameObject.SetActive(false);
        }
        UpdateReadySoldier();
    }

    private IEnumerator CreateOwnSoldier()
    {
        List<Soldier> ownSoldierList = PlayerData.Instance._SoldierDepot._soldierList;
        int MAXCOUNT = 28;
        int count = ownSoldierList.Count;
        int itemCount = ownsoldier_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.Wrap_OwnSoldier.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.Wrap_OwnSoldier.minIndex = -index;
        view.Wrap_OwnSoldier.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.Wrap_OwnSoldier.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.Wrap_OwnSoldier.enabled = false;
        }
        yield return null;
        if (count < itemCount)
        {
            for (int i = count; i < itemCount; i++)
            {
                OwnSoldierLineupComponent comp = ownsoldier_dic[i];
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
            OwnSoldierLineupComponent comp = null;
            if (i < itemCount)
            {
                comp = ownsoldier_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_OwnSoldierComp, view.Grd_OwnSoldier.transform);
                comp = new OwnSoldierLineupComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_OwnSoldier);
                ownsoldier_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(ownSoldierList[i]);
            comp.IsShowEnergy = true;
            comp.IsShowLeader = true;
            comp.IsEnable = true;
            comp.IsSelect = IsSelectedOwnSoldier(ownSoldierList[i]);
            if (!comp.mRootObject.activeSelf)
                comp.mRootObject.SetActive(true);
        }
        yield return null;
        view.Wrap_OwnSoldier.ReGetChild();
        yield return null;
        view.Grd_OwnSoldier.Reposition();
        yield return null;
        view.ScrView_OwnSoldier.ResetPosition();
        UpdateSameSoldierStatus();
    }

    private void UpdateReadySoldier()
    {
        readysoldierlist = SortReadySoldiers(readysoldierlist);
        UpdateLeadership();
        if (readysoldierlist.Count <= ready_dic.Count)
        {
            for (int i = 0; i < ready_dic.Count; i++)
            {
                LineupItemComponent comp = ready_dic[i];
                if (i < readysoldierlist.Count)
                {
                    comp.UpdateInfo(readysoldierlist[i]);
                    comp.IsSelect = false;
                    comp.IsEnable = true;
                    comp.IsShowEnergy = true;
                    comp.IsShowLeader = true;
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.Clear();
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int ob_count = ready_dic.Count;   //已经生成的物件数量
            LineupItemComponent comp = null;
            for (int i = 0; i < readysoldierlist.Count; i++)
            {
                Soldier tReadySoldier = readysoldierlist[i];
                if (i < ob_count)
                {
                    comp = ready_dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_ReadySoldierComp.gameObject, view.Grd_RedaySoldierGrid.transform);
                    comp = new LineupItemComponent();
                    comp.MyStart(go);
                    comp.AddEventListener(ButtonEvent_ReadySoldier);
                    ready_dic.Add(comp);
                }
                if (comp == null) return;
                comp.UpdateInfo(tReadySoldier);
                comp.IsSelect = false;
                comp.IsShowEnergy = true;
                comp.IsShowLeader = true;
                comp.IsEnable = true;
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_RedaySoldierGrid.repositionNow = true;
    }

    private void DelOwnSoldierData(Soldier soldier)
    {
        foreach (OwnSoldierLineupComponent tmp in ownsoldier_dic)
        {
            OwnSoldierLineupComponent comp = tmp;
            if (comp.soldierAtt.Equals(soldier))
            {
                comp.IsSelect = false;
                break;
            }
        }
    }

    /// <summary>
    /// 更新准备出战的士兵数据 默认删除已出战士兵
    /// 返回false 则说明已达出战士兵上线
    /// </summary>
    private bool UpdateReadySoldierData(Soldier data, bool isAdd = false)
    {
        if (isAdd)
        {
            if (battletype == EFightType.eftMain)
            {
                if (readysoldierlist.Count >= readyLockCount && readyLockCount < GlobalCoefficient.LineupSoldierLimit)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_UNLOCKSOLDIERLIMIT);
                    return false;
                }
                else if (readysoldierlist.Count >= GlobalCoefficient.LineupSoldierLimit)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_SOLDIERLIMIT);
                    return false;
                }
                else
                {
                    readysoldierlist.Add(data);
                    return true;
                }
            }
            else
            {
                if (readysoldierlist.Count >= GlobalCoefficient.LineupSoldierLimit)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_SOLDIERLIMIT);
                    return false;
                }
                else
                {
                    readysoldierlist.Add(data);
                    return true;
                }
            }

        }
        else
        {
            List<Soldier> _list = new List<Soldier>();
            for (int i = 0; i < readysoldierlist.Count; i++)
            {
                if (readysoldierlist[i].uId != data.uId)
                {
                    _list.Add(readysoldierlist[i]);
                }
            }
            readysoldierlist.Clear();
            readysoldierlist.AddRange(_list);
            return true;
        }
    }

    private List<Soldier> SortReadySoldiers(List<Soldier> needSortList)
    {
        List<Soldier> OwnSoldierList = new List<Soldier>(needSortList);
        OwnSoldierList.Sort
               (
               (left, right) =>
               {
                   if (left.Att.call_energy != right.Att.call_energy)
                   {
                       if (left.Att.call_energy > right.Att.call_energy)
                       {
                           return 1;
                       }
                       else
                       {
                           return -1;
                       }
                   }

                   if (left.Level != right.Level)
                   {
                       if (left.Level > right.Level)
                           return -1;
                       else
                           return 1;
                   }
                   if (left.Att.Star != right.Att.Star)
                   {
                       if (left.Att.Star > right.Att.Star)
                           return -1;
                       else
                           return 1;
                   }
                   if (left.Att.quality != right.Att.quality)
                   {
                       if (left.Att.quality > right.Att.quality)
                           return -1;
                       else
                           return 1;
                   }
                   if (left.Att.id != right.Att.id)
                   {
                       if (left.Att.id < right.Att.id)
                           return -1;
                       else
                           return 1;

                   }
                   return 0;
               }
               );
        return OwnSoldierList;
    }

    private void UpdateLeadership()
    {
        leadership = 0;
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            leadership += readysoldierlist[i].Att.leaderShip;
        }
        view.Lbl_Leadership.text = string.Format(ConstString.GATE_LINEUPANEL_LEADERSHIP, leadership, ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(PlayerData.Instance._Level).Leadership);
    }

    public void ReceiveBuyDungeonTimes()
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_BUYDUNGEONCOUNTSUCCESS);
        UpdateSweepInfo();
    }

    public void ReceiveBuyOtherDungeonTimes()
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_BUYDUNGEONCOUNTSUCCESS);
        UpdateSweepInfo();
    }

    private void UpdateSweepCount()
    {
        int count = 1;
        if (sweepCount / (int)GlobalCoefficient.RepeatSweepCount > 0)
        {
            count = 10;
        }
        else
        {
            count = sweepCount;
        }
        string name = string.Empty;
        if (count == 0)
        {
            switch (battletype)
            {
                case EFightType.eftMain:
                    {
                        if (stageinfo.ChallengeCount > GlobalCoefficient.RepeatSweepCount || stageinfo.ChallengeCount <= 0)
                        {
                            count = (int)GlobalCoefficient.RepeatSweepCount;
                        }
                        else
                        {
                            count = stageinfo.ChallengeCount;
                        }
                    } break;
                case EFightType.eftActivity:
                    {
                        int restCount = activityInfo.max_times - activityInfo.today_times;
                        if (restCount > GlobalCoefficient.RepeatSweepCount)
                        {
                            count = (int)GlobalCoefficient.RepeatSweepCount;
                        }
                        else
                        {
                            if (activityInfo.max_times <= GlobalCoefficient.RepeatSweepCount)
                            {
                                count = activityInfo.max_times;
                            }
                            else 
                            {
                                count = restCount;
                            }
                        }
                    } break;
                case EFightType.eftEndless:
                    {
                        int restCount = endlessInfo.max_times - endlessInfo.today_times;
                        if (restCount > GlobalCoefficient.RepeatSweepCount || restCount <= 0)
                        {
                            count = (int)GlobalCoefficient.RepeatSweepCount;
                        }
                        else
                        {
                            count = restCount;
                        }
                    }
                    break;
            }
        }
        view.Lbl_MutilSweepCount.text = string.Format(ConstString.GATE_INFOPANEL_REPEATSWEEP, ConstString.GATE_MUTILSWEEP_COUNT[count]);
        string diamondstr = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_SWEEP_CONSUMEDIAMOND);
        int diamond = 0;
        int.TryParse(diamondstr, out diamond);
        view.Lbl_MulitSweepDiamondTip.text = "x" + diamond * count;
    }

    #endregion

    /// <summary>
    /// 当前装备是否被选中
    /// </summary>
    /// <param name="equip"></param>
    /// <returns></returns>
    private bool IsSelectedOwnEquip(Weapon equip)
    {
        if (equip == null) return false;
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            Weapon readyEquip = readyEquipArray[i].weapon;
            if (readyEquip != null && readyEquip.uId.Equals(equip.uId))
            {
                return true;
            }
        }
        return false;
    }
    private bool IsSameSoldier(Soldier soldier)
    {
        Soldier same = readysoldierlist.Find((tmp) => { return tmp.Att.type == soldier.Att.type; });
        if (same != null)
            return true;
        return false;
    }
    #region Button Event

    private void ButtonEvent_ArtifactDetail(BaseComponent baseComp, bool state)
    {
        GateInfoOwnEquipComponent comp = baseComp as GateInfoOwnEquipComponent;
        if (comp == null) return;
        if (state)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ARTIFACTDETAILINFO);
            UISystem.Instance.ArtifactDetailView.UpdateViewInfo(comp.mRootObject, comp.EquipInfo);
        }
        else
        {
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ARTIFACTDETAILINFO);
        }
    }

    /// <summary>
    /// 当前选择的装备
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_ReadyEquip(BaseComponent baseComp)
    {
        if (FightRelatedModule.Instance.isFightState)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        GateInfoEquipComponent comp = baseComp as GateInfoEquipComponent;
        if (comp == null) return;
        if (battletype == EFightType.eftMain)
        {
            OpenLevelData opendata = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.ArtifactControl);
            if (opendata != null)
            {
                if (!PlayerData.Instance.IsPassedGate((uint)opendata.gateId))
                {
                    StageInfo stageData = ConfigManager.Instance.mStageData.GetInfoByID((uint)opendata.gateId);
                    if (stageData != null)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_EQUIP_UNLOCKUNLOAD, stageData.GateSequence));
                        return;
                    }
                }
            }
        }
        UpdateReadyEquipData(comp.EquipInfo, false);
        DelOwnEquipsData(comp.EquipInfo);
        UpdateReadyEquips();
    }

    /// <summary>
    /// 当前拥有的装备
    /// </summary>
    /// <param name="go"></param>
    private void ButtonEvent_OwnEquip(BaseComponent baseComp)
    {
        if (FightRelatedModule.Instance.isFightState)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        GateInfoOwnEquipComponent comp = baseComp as GateInfoOwnEquipComponent;
        if (comp == null) return;
        if (comp.IsSelect) return;
        if (comp.EquipInfo.IsLock) return;
        //if (comp.EquipInfo.Att.levelLimit >= PlayerData.Instance._Level)
        //{
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_EQUIP_LEVELLIMIT);
        //    return;
        //}
        if (CheckEquipLimit(comp.EquipInfo))
        {
            // UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_EXCEED_ARTIFACTLIMIT);
            return;
        }
        comp.IsSelect = true;
        UpdateReadyEquipData(comp.EquipInfo, comp.IsSelect);
        UpdateReadyEquips();
        GuideManager.Instance.CheckTrigger(GuideTrigger.ChoosedGodWeapon);

    }

    private void ButtonEvent_OwnSoldier(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        LineupItemComponent comp = baseComp as LineupItemComponent;
        if (comp == null)
        {
            Debug.LogError("can not get LineupItemComponent by:");
            return;
        }
        if (comp.IsSelect)
        {
            if (UpdateReadySoldierData(comp.soldierAtt, !comp.IsSelect))
            {
                DelOwnSoldierData(comp.soldierAtt);
                UpdateReadySoldier();
                UpdateSameSoldierStatus();
            }
            return;
        }
        if (IsSameSoldier(comp.soldierAtt))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_SOLDIER_SAME);
            return;
        }
        int tmpLeadership = leadership + comp.soldierAtt.Att.leaderShip;
        if (tmpLeadership > ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(PlayerData.Instance._Level).Leadership)
        {
            PlayLearderShipAnim();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_OVERLEADERSHIP);
            return;
        }
        if (UpdateReadySoldierData(comp.soldierAtt, !comp.IsSelect))
        {
            comp.IsSelect = !comp.IsSelect;
            UpdateReadySoldier();
            GuideManager.Instance.CheckTrigger(GuideTrigger.ChoosedSoldier);

        }
        UpdateSameSoldierStatus();
    }

    private void ButtonEvent_ReadySoldier(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        LineupItemComponent comp = baseComp as LineupItemComponent;
        if (comp == null)
        {
            Debug.LogError("can not get LineupItemComponent :");
            return;
        }
        if (UpdateReadySoldierData(comp.soldierAtt, false))
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.SoldierCancel);
            DelOwnSoldierData(comp.soldierAtt);
            UpdateReadySoldier();
        }
        UpdateSameSoldierStatus();
    }

    /// <summary>
    /// 更新相同类型的士兵状态
    /// </summary>
    private void UpdateSameSoldierStatus()
    {
        for (int i = 0; i < ownsoldier_dic.Count; i++)
        {
            OwnSoldierLineupComponent comp = ownsoldier_dic[i];
            comp.IsEnable = !IsSameSoldier(comp.soldierAtt);
        }
    }

    private void ButtonEvent_Next(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        HeroAttributeInfo info = ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(PlayerData.Instance._Level);
        if (info != null)
        {
            if (leadership > info.Leadership)
            {
                PlayLearderShipAnim();
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_OVERLEADERSHIP);
                return;
            }
        }
        if (!PlayerData.Instance.IsEnoughSP(stageinfo.Physical, true))
        {
            return;
        }
        //List<UInt64> list = new List<UInt64>();
        //for (int i = 0; i < readysoldierlist.Count; i++)
        //{
        //    list.Add(readysoldierlist[i].uId);
        //}
        if (readysoldierlist.Count <= 0)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.OpenNoSoldierToFight);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GATE_SOLDIERTIP_NOSOLDIER,
                () =>
                {
                    OpenEquipPanel();
                    //FightRelatedModule.Instance.SendDungeonStart(stageinfo.ID, list);
                },
                null,
                ConstString.HINT_LEFTBUTTON_GOON,
                ConstString.HINT_RIGHTBUTTON_CANCEL
               );
            return;
        }
        OpenEquipPanel();
        //FightRelatedModule.Instance.SendDungeonStart(stageinfo.ID, list);
    }

    private void ButtonEvent_ColseEquipSelect(GameObject btn)
    {
        if (FightRelatedModule.Instance.isFightState)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        CloseEquipPanel();
    }

    private void ButtonEvent_StartBattle(GameObject btn)
    {
        if (FightRelatedModule.Instance.isFightState)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        FightRelatedModule.Instance.isFightState = true;
        int weaponCount = 0;
        int ringCount = 0;
        for (int l = 0; l < readyEquipArray.Length; l++)
        {
            //TODO:应该判断readyEquipArray[l].weapon == null  此处处理错误
            //待下个版本 修改  add by taiwei
            if (readyEquipArray[l] != null)
            {
                ReadyEquipData data = readyEquipArray[l];
                if (data.weapon == null)
                {
                    continue;
                }
                if (data.weapon.Att.type == (int)EquiptType._Weapon)
                {
                    weaponCount++;
                }
                if (data.weapon.Att.type == (int)EquiptType._ring)
                {
                    ringCount++;
                }
            }
        }
        bool weaponEnough = false;
        bool ringEnough = false;
        IsWeaponEnough(weaponCount, ringCount, out weaponEnough, out ringEnough);
        if (weaponEnough || ringEnough)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GATE_EQUIPHADFREE, () =>
            {
                GotoOneKeyEquip();
            }, () =>
            {
                FightRelatedModule.Instance.isFightState = false;
            });
        }
        else
        {
            GotoOneKeyEquip();
        }
        //if (isEquipFree == false)
        //{
        //    GotoOneKeyEquip();
        //}
    }

    private void ButtonEvent_ChangePet(GameObject go)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);
        UISystem.Instance.PetChooseView.UpdateViewInfo();
    }

    private void IsWeaponEnough(int weaponNum, int ringNum, out bool weaponEnough, out bool ringEnough)
    {
        weaponEnough = false;
        ringEnough = false;
        List<Weapon> list = PlayerData.Instance._WeaponDepot.GetLockAndUnlockList();
        if (list == null)
        {
            return;
        }
        int weaponCount = 0;
        int ringCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            Weapon weapon = list[i];
            if (weapon.IsLock == true)
                continue;
            if (weapon.Att.type == (int)EquiptType._Weapon)
            {
                weaponCount++;
            }
            if (weapon.Att.type == (int)EquiptType._ring)
            {
                ringCount++;
            }
        }
        if (weaponCount >= 3 && weaponNum < 3)
        {
            weaponEnough = true;
        }
        else if (weaponNum < weaponCount && weaponCount < 3)
        {
            weaponEnough = true;
        }
        else
        {
            weaponEnough = false;
        }
        if (ringCount >= 3 && ringNum < 3)
        {
            ringEnough = true;
        }
        else if (ringNum < ringCount && ringCount < 3)
        {
            ringEnough = true;
        }
        else
        {
            ringEnough = false;
        }
    }

    private void GotoOneKeyEquip()
    {
        List<Weapon> equiped = PlayerData.Instance._ArtifactedDepot._EquiptList;
        List<fogs.proto.msg.ReplaceEquip> upEquips = new List<fogs.proto.msg.ReplaceEquip>();
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            ReadyEquipData readyWeapon = readyEquipArray[i];
            if (readyWeapon != null && readyWeapon.weapon != null)
            {
                ReplaceEquip replace = new ReplaceEquip();
                replace.uid = readyWeapon.weapon.uId;
                Weapon equipedWeapon = equiped.Find((tmp) =>
                {
                    if (tmp == null) return false;
                    return tmp.uId == readyWeapon.weapon.uId;
                });
                if (equipedWeapon == null)  //说明之前未装备
                {
                    replace.position = readyWeapon.index + 1;
                    upEquips.Add(replace);
                }
                else
                {
                    for (int j = 0; j < equiped.Count; j++)
                    {
                        if (readyWeapon.weapon.uId.Equals(equipedWeapon.uId))  //首先找到相同ID的数据
                        {
                            if (!readyWeapon.index.Equals(j))   //是否在相同的位置
                            {
                                replace.position = readyWeapon.index + 1;
                                upEquips.Add(replace);
                                break;
                            }
                        }
                    }
                }
            }
        }
        List<ulong> offEquips = new List<ulong>();
        for (int i = 0; i < equiped.Count; i++)
        {
            Weapon equipedWeapon = equiped[i];
            if (equipedWeapon == null) continue;
            if (equipedWeapon.Att.type == (int)EquiptType._necklace || equipedWeapon.Att.type == (int)EquiptType._clothing || equipedWeapon.Att.type == (int)EquiptType._mount)
                continue;
            ReplaceEquip replace = upEquips.Find((tmp) =>
            {
                if (tmp == null) return false;
                return tmp.uid == equipedWeapon.uId;
            });
            if (replace != null)  //对比位置变动
            {
                if (replace.position != i + 1)  //位置有变动的
                {
                    offEquips.Add(equipedWeapon.uId);
                }
            }
            else
            {
                offEquips.Add(equipedWeapon.uId);
            }
        }
        if (offEquips.Count > 0 || upEquips.Count > 0)
        {
            FightRelatedModule.Instance.SendOneKeyReplaceEquip(EFightType.eftMain, offEquips, upEquips);
        }
        else
        {
            FightRelatedModule.Instance.isFightState = false;
            OneKeyEquipSuccess();
        }
    }

    private void ButtonEvent_EnemyComp(GateEnemyInfoComponent baseComp, bool isPress)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (baseComp == null) return;
        HintManager.Instance.SeeDetail(baseComp.mRootObject, isPress, baseComp.MonsterInfo);
    }

    private void ButtonEvent_AwardComp(GateAwardsComponent baseComp, bool isPress)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (baseComp == null) return;
        HintManager.Instance.SeeDetail(baseComp.mRootObject, isPress, baseComp.ItemID);
    }

    private void ButtonEvent_CloseButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        bool isOpen = FightRelatedModule.Instance.isOpenNext;
        if (isOpen)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATE);
            UISystem.Instance.GateView.UpdateViewInfo(stageinfo.ID);
        }
        else
        {
            switch (battletype)
            {
                case EFightType.eftMain:
                    {
                        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_GATE))
                        {
                            UISystem.Instance.GateView.UpdateViewInfo(stageinfo.ID);
                        }
                    } break;
                case EFightType.eftActivity:
                    {
                        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_ACTIVITIES))
                        {
                            UISystem.Instance.ActivitiesView.UpdateDifficultyPanelStatus(sweepCount);
                        }
                    }
                    break;
            }
        }
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
    }

    private void ButtonEvent_Back(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.Gobj_LineupSelect.SetActive(false);
        view.StageInfoGroup_TScale.gameObject.SetActive(true);
        //PlayOpenStageInfoGroupAnim();
        ClearLineupPanel();
    }

    private void ButtonEvent_SingleSweep(GameObject btn)
    {
        if (FightRelatedModule.Instance.isSweep)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (!ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Clear, false))
        {
            OpenLevelData data = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Clear);
            if (data != null)
            {
                if ((data.vipLevel > 0) && (data.openLevel <= 0))
                {
                    string tip = string.Format(ConstString.UNLOCK_VIP_0, data.vipLevel) + ConstString.GATE_SWEEPTIP_SWEEPUNLOCK;
                    CommonFunction.ShowVipLvNotEnoughTip(tip);
                }
                else if ((data.openLevel > 0) && (data.vipLevel <= 0))
                {
                    string tip = string.Format(ConstString.UNLOCK_LEVEL_1, data.openLevel) + ConstString.GATE_SWEEPTIP_SWEEPUNLOCK;
                    CommonFunction.ShowVipLvNotEnoughTip(tip);
                }
                else if ((data.openLevel > 0) && (data.vipLevel > 0))
                {
                    string tip = string.Format(ConstString.UNLOCK_LEVEL_1, data.openLevel) + ConstString.GATE_SWEEPTIP_EITHER;
                    tip += string.Format(ConstString.UNLOCK_VIP_0, data.vipLevel) + ConstString.GATE_SWEEPTIP_SWEEPUNLOCK;
                    CommonFunction.ShowVipLvNotEnoughTip(tip);
                }
            }
            return;
        }

        int sweepCard = PlayerData.Instance.GetSweepCard();
        switch (battletype)
        {
            case EFightType.eftMain:
                {
                    if (gateinfo == null || stageinfo == null)
                        return;
                    if (gateinfo.star_level < 3)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPTIP_STARLIMIT);
                        return;
                    }
                    if (stageinfo.ChallengeCount > 0)
                    {
                        if (sweepCount <= 0)
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_CHALLENGETIMESNOTENOUGH);
                            return;
                        }
                    }
                } break;
            case EFightType.eftActivity:
                {
                    if (gateinfo == null || stageinfo == null)
                        return;
                    if (gateinfo.star_level < 3)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPTIP_STARLIMIT);
                        return;
                    }
                    if (sweepCount <= 0)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_CHALLENGETIMESNOTENOUGH);
                        return;
                    }
                }
                break;
            case EFightType.eftEndless:
                {
                    if (endlessInfo.passed == 0)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPTIP_ENDLESSCLOSE);
                        return;
                    }
                    if (sweepCount <= 0)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_CHALLENGETIMESNOTENOUGH);
                        return;
                    }
                } break;
        }
        if (PlayerData.Instance.IsEnoughSP(stageinfo.Physical, true))
        {
            if (dropItemList == null)
                dropItemList = new List<CommonItemData>();
            if (sweepCard <= 0)
            {
                string diamondstr = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_SWEEP_CONSUMEDIAMOND);
                int diamond = 0;
                int.TryParse(diamondstr, out diamond);
                if (PlayerData.Instance.GoldIsEnough(2, diamond))
                {
                    if (PlayerData.Instance.IsSoldierFull())
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_SWEEPTIP_SOLIDERMAX, SoldierDepot.MAXCOUNT));
                    }
                    else
                    {
                        switch (battletype)
                        {
                            case EFightType.eftMain:
                                FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_MAJOR, gateinfo.dgn_id, 1);
                                break;
                            case EFightType.eftActivity:
                                FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_ACTIVITY, gateinfo.dgn_id, 1);
                                break;
                            case EFightType.eftEndless:
                                FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_ENDLESS, stageinfo.ChapterID, 1);
                                break;
                        }
                    }
                }
                else
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_TOP_DIAMONDLACK);
                }
            }
            else
            {
                if (PlayerData.Instance.IsSoldierFull())
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_SWEEPTIP_SOLIDERMAX, SoldierDepot.MAXCOUNT));
                }
                else
                {
                    switch (battletype)
                    {
                        case EFightType.eftMain:
                            FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_MAJOR, gateinfo.dgn_id, 1);
                            break;
                        case EFightType.eftActivity:
                            FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_ACTIVITY, gateinfo.dgn_id, 1);
                            break;
                        case EFightType.eftEndless:
                            FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_ENDLESS, stageinfo.ChapterID, 1);
                            break;
                    }
                }
            }
        }
    }

    private void ButtonEvent_MulitSweep(GameObject btn)
    {
        if (FightRelatedModule.Instance.isSweep)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.BatchClear, false))
        {
            OpenLevelData data = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.BatchClear);
            if (data != null)
            {
                if ((data.vipLevel > 0) && (data.openLevel <= 0))
                {
                    string tip = string.Format(ConstString.UNLOCK_VIP_0, data.vipLevel) + ConstString.GATE_SWEEPTIP_MUTILSWEEPUNLOCK;
                    CommonFunction.ShowVipLvNotEnoughTip(tip);
                }
                else if ((data.openLevel > 0) && (data.vipLevel <= 0))
                {
                    string tip = string.Format(ConstString.UNLOCK_LEVEL_1, data.openLevel) + ConstString.GATE_SWEEPTIP_MUTILSWEEPUNLOCK;
                    CommonFunction.ShowVipLvNotEnoughTip(tip);
                }
                else if ((data.openLevel > 0) && (data.vipLevel > 0))
                {
                    string tip = string.Format(ConstString.UNLOCK_LEVEL_1, data.openLevel) + ConstString.GATE_SWEEPTIP_EITHER;
                    tip += string.Format(ConstString.UNLOCK_VIP_0, data.vipLevel) + ConstString.GATE_SWEEPTIP_MUTILSWEEPUNLOCK;
                    CommonFunction.ShowVipLvNotEnoughTip(tip);
                }
            }
            return;
        }
        int count = 1;
        int sweepCard = PlayerData.Instance.GetSweepCard();
        switch (battletype)
        {
            case EFightType.eftMain:
                {

                    if (gateinfo == null || stageinfo == null)
                        return;
                    if (stageinfo.ChallengeCount > 0)
                    {
                        if (sweepCount <= 0)
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_CHALLENGETIMESNOTENOUGH);
                            return;
                        }
                    }
                    if (gateinfo.star_level < 3)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPTIP_STARLIMIT);
                        return;
                    }
                    if (stageinfo.ChallengeCount <= 0)
                    {
                        count = 10;
                    }
                    else
                    {
                        if (sweepCount / GlobalCoefficient.RepeatSweepCount > 0)
                        {
                            count = 10;
                        }
                        else
                        {
                            count = sweepCount;
                        }
                        if (sweepCount < count)
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEP_NOSWEEPCOUNT);
                            return;
                        }
                    }
                } break;
            case EFightType.eftActivity:
                {
                    if (gateinfo == null || stageinfo == null)
                        return;
                    //if (stageinfo.ChallengeCount > 0)
                    //{
                    if (sweepCount <= 0)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_CHALLENGETIMESNOTENOUGH);
                        return;
                    }
                    //}
                    if (gateinfo.star_level < 3)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPTIP_STARLIMIT);
                        return;
                    }
                    if (sweepCount / GlobalCoefficient.RepeatSweepCount > 0)
                    {
                        count = 10;
                    }
                    else
                    {
                        count = sweepCount;
                    }
                    if (sweepCount < count)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEP_NOSWEEPCOUNT);
                        return;
                    }
                } break;
            case EFightType.eftEndless:
                {
                    if (sweepCount <= 0)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_CHALLENGETIMESNOTENOUGH);
                        return;
                    }
                } break;
        }
        if (PlayerData.Instance.IsEnoughSP(stageinfo.Physical * count, true))
        {
            if (dropItemList == null)
                dropItemList = new List<CommonItemData>();
            if (sweepCard < count)
            {
                string diamondstr = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_SWEEP_CONSUMEDIAMOND);
                int diamond = 0;
                int.TryParse(diamondstr, out diamond);
                if (PlayerData.Instance.GoldIsEnough(2, diamond * count))
                {
                    //int soldierCount = 0;
                    //for (int i = 0; i < dropItemList.Count; i++)
                    //{
                    //    CommonItemData data = dropItemList[i];
                    //    if (data.Type == IDType.Soldier)
                    //    {
                    //        soldierCount++;
                    //    }
                    //}
                    if (PlayerData.Instance.IsSoldierFull())
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_SWEEPTIP_SOLIDERMAX, SoldierDepot.MAXCOUNT));
                    }
                    else
                    {
                        switch (battletype)
                        {
                            case EFightType.eftMain:
                                FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_MAJOR, gateinfo.dgn_id, count);
                                break;
                            case EFightType.eftActivity:
                                FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_ACTIVITY, gateinfo.dgn_id, count);
                                break;
                            case EFightType.eftEndless:
                                FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_ENDLESS, stageinfo.ChapterID, count);
                                break;
                        }
                    }
                }
                else
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_TOP_DIAMONDLACK);
                }
            }
            else
            {
                //int soldierCount = 0;
                //for (int i = 0; i < dropItemList.Count; i++)
                //{
                //    CommonItemData data = dropItemList[i];
                //    if (data.Type == IDType.Soldier)
                //    {
                //        soldierCount++;
                //    }
                //}
                if (PlayerData.Instance.IsSoldierFull())
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_SWEEPTIP_SOLIDERMAX, SoldierDepot.MAXCOUNT));
                }
                else
                {
                    switch (battletype)
                    {
                        case EFightType.eftMain:
                            FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_MAJOR, gateinfo.dgn_id, count);
                            break;
                        case EFightType.eftActivity:
                            FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_ACTIVITY, gateinfo.dgn_id, count);
                            break;
                        case EFightType.eftEndless:
                            FightRelatedModule.Instance.SendMopupDungeon(DungeonType.DGT_ENDLESS, stageinfo.ChapterID, count);
                            break;
                    }
                }
            }
        }
    }

    private void ButtonEvent_DirectBattle(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (!PlayerData.Instance.IsOutstripLevel(stageinfo.UnlockLV))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_LINEUPPANEL_LEVELLIMIT, stageinfo.UnlockLV));
            return;
        }
        if (stageinfo.IsElite != (int)MainBattleType.Crusade)
        {
            if (sweepCount <= 0)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_CHALLENGETIMESNOTENOUGH);
                return;
            }
        }
        if (!PlayerData.Instance.IsEnoughSP(stageinfo.Physical, true))
        {
            return;
        }
        if (PlayerData.Instance.IsSoldierFull())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_BATTLETIP_SOLIDERMAX, SoldierDepot.MAXCOUNT));
        }
        else
        {
            switch (battletype)
            {
                case EFightType.eftMain:
                    {
                        if (PlayerData.Instance._MajorDungeonSoldierList == null || PlayerData.Instance._MajorDungeonSoldierList.Count <= 0)
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_DIRECTBATTLE_NOSOLDIER);
                            return;
                        }
                        if (!IsHadEquip())
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_DIRECTBATTLE_NOEQUIP);
                            return;
                        }
                        isDirectBattle = true;
                        List<ulong> soldiers = new List<ulong>();
                        for (int i = 0; i < PlayerData.Instance._MajorDungeonSoldierList.Count; i++)
                        {
                            ulong uid = PlayerData.Instance._MajorDungeonSoldierList[i];
                            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(uid);
                            if (soldier != null)
                                soldiers.Add(uid);

                        }
                        FightRelatedModule.Instance.SendDungeonStart(stageinfo.ID, soldiers);
                    }
                    break;
                case EFightType.eftActivity:
                    {
                        List<ulong> list = new List<ulong>();
                        if (PlayerPrefsTool.HasKey(AppPrefEnum.ActivityReadySoldier))
                        {
                            list = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.ActivityReadySoldier);
                        }
                        if (list == null || list.Count <= 0)
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_DIRECTBATTLE_NOSOLDIER);
                            return;
                        }
                        if (!IsHadEquip())
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_DIRECTBATTLE_NOEQUIP);
                            return;
                        }
                        isDirectBattle = true;
                        FightRelatedModule.Instance.SendDungeonStart(stageinfo.ID, list);
                    }
                    break;
                case EFightType.eftEndless:
                    break;
            }
        }

    }
    /// 装备部位 0=武器；1=戒指；2=项链；3=衣服；4=坐骑
    private bool IsHadEquip()
    {
        if (PlayerData.Instance._ArtifactedDepot == null || PlayerData.Instance._ArtifactedDepot._EquiptList == null)
            return false;
        return PlayerData.Instance._ArtifactedDepot._EquiptList.Find((equip) =>
        {
            if (equip == null)
                return false;

            return (equip.Att.type != 2 && equip.Att.type != 4);
        }) != null;
    }
    private void ButtonEvent_ReadyBattle(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!PlayerData.Instance.IsOutstripLevel(stageinfo.UnlockLV))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_LINEUPPANEL_LEVELLIMIT, stageinfo.UnlockLV));
            return;
        }
        if (stageinfo.IsElite != (int)MainBattleType.Crusade)
        {
            if (sweepCount <= 0)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_CHALLENGETIMESNOTENOUGH);
                return;
            }
        }
        if (!PlayerData.Instance.IsEnoughSP(stageinfo.Physical, true))
        {
            //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_LINEUPPANEL_SPLIMIT);
            return;
        }
        //int soldierCount = 0;
        //if (dropItemList == null)
        //    dropItemList = new List<CommonItemData>();
        //for (int i = 0; i < dropItemList.Count; i++)
        //{
        //    CommonItemData data = dropItemList[i];
        //    if (data.Type == IDType.Soldier)
        //    {
        //        soldierCount++;
        //    }
        //}
        if (PlayerData.Instance.IsSoldierFull())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_BATTLETIP_SOLIDERMAX, SoldierDepot.MAXCOUNT));
        }
        else
        {
            ReadyBattle();
        }
    }
    /// <summary>
    /// 购买今日扫荡次数
    /// </summary>
    private void ButtonEvent_PurchaseSweep(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        switch (battletype)
        {
            case EFightType.eftMain:
                {
                    int count = 1;
                    if (PlayerData.Instance._TodayPlayDungeons != null && PlayerData.Instance._TodayPlayDungeons.dungeons != null)
                    {
                        fogs.proto.msg.TodayPlayDungeons.DungeonTimes dungeons = PlayerData.Instance._TodayPlayDungeons.dungeons.Find((tmp) => { return tmp.id == stageinfo.ID; });
                        if (dungeons != null)
                        {
                            count = dungeons.buy_times;
                        }
                    }
                    if (count <= 0)
                    {
                        count = 0;
                    }
                    VipData vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
                    if (vipData != null)
                    {
                        if (count >= vipData.ElteCount)
                        {
                            if (PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Elite))
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                            }
                            else
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASELIMIT, count, vipData.ElteCount), () =>
                                {
                                    UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                                    UISystem.Instance.VipRechargeView.ShowRecharge();
                                }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                            }
                        }
                        else
                        {
                            TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)count + 1);
                            if (data == null || data.AdvanceLevelChallengeTimes == null)// data为NULL 则可默认为已经达到购买上限
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                            }
                            else if (data.AdvanceLevelChallengeTimes.Type == ECurrencyType.None && data.AdvanceLevelChallengeTimes.Number == 0)  // 消耗金币类型为0则说明无购买次数
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                            }
                            else
                            {
                                if (count >= vipData.ElteCount)
                                {
                                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(string.Format(ConstString.PVP_PURCHASELIMIT, count, vipData.ElteCount), count, vipData.ElteCount), () =>
                                    {
                                        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                                        UISystem.Instance.VipRechargeView.ShowRecharge();
                                    }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                                }
                                else
                                {
                                    if (CommonFunction.CheckMoneyEnough(data.AdvanceLevelChallengeTimes.Type, data.AdvanceLevelChallengeTimes.Number, true))
                                    {
                                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.GATE_PURCHASESWEEPCOUNT, data.AdvanceLevelChallengeTimes.Number, count, vipData.ElteCount), () =>
                                        {
                                            FightRelatedModule.Instance.SendBuyDungeonTimes(stageinfo.ID);
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERRORDATA);
                    }
                }
                break;
            case EFightType.eftActivity:
                {
                    ActivityDungeonInfo info = GetActivityInfo();
                    int count = info.today_buy_times;
                    if (count <= 0)
                    {
                        count = 0;
                    }
                    VipData vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
                    if (vipData != null)
                    {
                        if (count >= vipData.ActivityCount)
                        {
                            if (PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Activity))
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                            }
                            else
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASELIMIT, count, vipData.ActivityCount), () =>
                                {
                                    UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                                    UISystem.Instance.VipRechargeView.ShowRecharge();
                                }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                            }
                        }
                        else
                        {
                            TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)count + 1);
                            if (data == null || data.ActivityDgnChallengeTimes == null)// data为NULL 则可默认为已经达到购买上限
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                            }
                            else if (data.ActivityDgnChallengeTimes.Type == ECurrencyType.None && data.ActivityDgnChallengeTimes.Number == 0)  // 消耗金币类型为0则说明无购买次数
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                            }
                            else
                            {
                                if (count >= vipData.ActivityCount)
                                {
                                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(string.Format(ConstString.PVP_PURCHASELIMIT, count, vipData.ActivityCount), count, vipData.ActivityCount), () =>
                                    {
                                        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                                        UISystem.Instance.VipRechargeView.ShowRecharge();
                                    }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                                }
                                else
                                {
                                    if (CommonFunction.CheckMoneyEnough(data.ActivityDgnChallengeTimes.Type, data.ActivityDgnChallengeTimes.Number, true))
                                    {
                                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.GATE_PURCHASESWEEPCOUNT, data.ActivityDgnChallengeTimes.Number, count, vipData.ActivityCount), () =>
                                        {
                                            FightRelatedModule.Instance.SendBuyOtherDungeonTimes(OtherDungeonType.ACTIVITY_DGN, info.chapter_id);
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERRORDATA);
                    }
                } break;
            case EFightType.eftEndless:
                {

                    VipData vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
                    if (vipData != null)
                    {
                        if (endlessInfo.today_buy_times < vipData.EndlessLimit)
                        {
                            TimesExpendData timesExpendData = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)endlessInfo.today_buy_times + 1);
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASECHANGLLECOUNT, timesExpendData.EndlessConsume.Number, ConfigManager.Instance.mTimesBuyConfig.GetTimesBuyData().EndlessTimes, endlessInfo.today_buy_times, vipData.EndlessLimit), () =>
                            {
                                FightRelatedModule.Instance.SendBuyOtherDungeonTimes(OtherDungeonType.ENDLESS_DGN, endlessInfo.chapter_id);
                            });
                        }
                        else
                        {
                            if (PlayerData.Instance._VipLv < ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Endless))
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.NO_BUY_SP_TIMES_TO_VIP, () =>
                                {
                                    UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                                    UISystem.Instance.VipRechargeView.ShowRecharge();
                                }, null, ConstString.FORMAT_RECHARGE);
                            }
                            else
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                            }
                        }
                    }
                }
                break;
        }

    }
    #endregion
    public void ReadyBattle()
    {
        isDirectBattle = false;
        //view.Gobj_LineupSelect.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
        view.Gobj_LineupSelect.SetActive(true);
        UpdateLineupPanelInfo();
        //Main.Instance.StartCoroutine(PlayOpenLineupSelectAnim());
        //PlayOpenLineupSelectAnim();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierChoose);
    }

    public void OneKeyEquipSuccess()
    {
        HeroAttributeInfo info = ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(PlayerData.Instance._Level);
        if (info != null)
        {
            if (leadership > info.Leadership)
            {
                PlayLearderShipAnim();
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_OVERLEADERSHIP);
                return;
            }
        }
        if (stageinfo == null) return;
        if (!PlayerData.Instance.IsEnoughSP(stageinfo.Physical, true))
        {
            return;
        }
        List<UInt64> list = new List<UInt64>();
        if (readysoldierlist == null)
        {
            readysoldierlist = new List<Soldier>();
        }
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            Soldier soldier = readysoldierlist[i];
            if (soldier == null) continue;
            list.Add(readysoldierlist[i].uId);
        }
        if (readysoldierlist.Count == 0)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.OpenNoSoldierToFight);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GATE_SOLDIERTIP_NOSOLDIER,
                () =>
                {
                    FightRelatedModule.Instance.SendDungeonStart(stageinfo.ID, list);
                },

                () => { FightRelatedModule.Instance.isFightState = false; },
                ConstString.HINT_LEFTBUTTON_GOON,
                ConstString.HINT_RIGHTBUTTON_CANCEL
               );
            return;
        }
        FightRelatedModule.Instance.SendDungeonStart(stageinfo.ID, list);
    }

    public void EnterTheBattle()
    {
        UISystem.Instance.CloseAllUI();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FIGHT);
        UISystem.Instance.HintView.ShowFightLoading(UISystem.Instance.FightView.ShowFightTitleInfo);
        FightRelatedModule.Instance.isFightState = false;
        Scheduler.Instance.AddTimer(0.5f, false, DelayEnterBattle);
    }

    private void DelayEnterBattle()
    {
        List<ulong> readysoldierUIDS = new List<ulong>();
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            readysoldierUIDS.Add(readysoldierlist[i].uId);
        }
        List<Soldier> battleSoldiers = new List<Soldier>();
        switch (battletype)
        {
            case EFightType.eftEndless:
                {
                    PlayerPrefsTool.WriteObject<List<ulong>>(AppPrefEnum.EndlessReadySoldier, readysoldierUIDS);
                    ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.EndlessReadySoldier, readysoldierUIDS);
                    battleSoldiers = readysoldierlist;
                }
                break;
            case EFightType.eftMain:
                {
                    if (isDirectBattle)
                    {
                        for (int i = 0; i < PlayerData.Instance._MajorDungeonSoldierList.Count; i++)
                        {
                            ulong soldierUID = PlayerData.Instance._MajorDungeonSoldierList[i];
                            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(soldierUID);
                            if (soldier == null)
                                continue;
                            battleSoldiers.Add(soldier);
                        }
                    }
                    else
                    {
                        PlayerData.Instance._MajorDungeonSoldierList = readysoldierUIDS;
                        battleSoldiers = readysoldierlist;
                    }
                    //PlayerPrefsTool.WriteString<List<ulong>>(AppPrefEnum.GateReadySoldier, readysoldierUIDS);
                }
                break;
            case EFightType.eftActivity:
                {
                    if (isDirectBattle)
                    {
                        List<ulong> list = new List<ulong>();
                        if (PlayerPrefsTool.HasKey(AppPrefEnum.ActivityReadySoldier))
                        {
                            list = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.ActivityReadySoldier);
                        }
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                ulong soldierUID = list[i];
                                Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(soldierUID);
                                if (soldier == null)
                                    continue;
                                battleSoldiers.Add(soldier);
                            }
                        }
                    }
                    else
                    {
                        PlayerPrefsTool.WriteObject<List<ulong>>(AppPrefEnum.ActivityReadySoldier, readysoldierUIDS);
                        ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.ActivityReadySoldier, readysoldierUIDS);
                        battleSoldiers = readysoldierlist;
                    }
                }
                break;
        }
        switch (battletype)
        {
            case EFightType.eftMain:
                UISystem.Instance.FightView.SetViewInfo(EFightType.eftMain, stageinfo, battleSoldiers, false, PlayerData.Instance._PetDepot.GetEquipedPet());
                break;
            case EFightType.eftActivity:
                UISystem.Instance.FightView.SetViewInfo(EFightType.eftActivity, stageinfo, battleSoldiers, false, PlayerData.Instance._PetDepot.GetEquipedPet());
                break;
            case EFightType.eftEndless:
                ChapterInfo info = ConfigManager.Instance.mChaptersData.GetChapterByID(stageinfo.ChapterID);
                if (info != null)
                {
                    List<StageInfo> list = CommonFunction.SortStages(info.gates);
                    if (list != null && list.Count > 0)
                    {
                        UISystem.Instance.FightView.SetViewInfo(EFightType.eftEndless, list[0], battleSoldiers, false, PlayerData.Instance._PetDepot.GetEquipedPet());
                    }
                }
                break;
        }
    }


    //private long singleSweepTime;
    //private bool isSingleSweep = true;
    public void SweepSuccess(fogs.proto.msg.MopupDungeonResp data)
    {
        //if (isSingleSweep)
        //{
        //    singleSweepTime = Main.mTime;
        //    //Scheduler.Instance.AddUpdator(UpdateSweepStatus);
        //    isSingleSweep = false;
        //}
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SWEEPRESULT);
        UISystem.Instance.SweepResultView.RefreshUIStatus(data);
        UpdateSweepInfo();
        FightRelatedModule.Instance.isSweep = false;
    }

    private int GetSweepCount()
    {
        int count = stageinfo.ChallengeCount;
        if (count > 0)
        {
            if (PlayerData.Instance._TodayPlayDungeons != null && PlayerData.Instance._TodayPlayDungeons.dungeons != null)
            {
                fogs.proto.msg.TodayPlayDungeons.DungeonTimes dungeons = PlayerData.Instance._TodayPlayDungeons.dungeons.Find((tmp) => { return tmp.id == stageinfo.ID; });
                if (dungeons != null)
                {
                    count = stageinfo.ChallengeCount * (1 + dungeons.buy_times) - dungeons.times;
                }
            }
        }
        else
        {
            count = 0;
        }
        if (count <= 0)
            count = 0;
        return count;
    }

    private ActivityDungeonInfo GetActivityInfo()
    {
        if (PlayerData.Instance._ActivityDungeonList != null && activityInfo != null)
        {
            for (int i = 0; i < PlayerData.Instance._ActivityDungeonList.Count; i++)
            {
                ActivityDungeonInfo info = PlayerData.Instance._ActivityDungeonList[i];
                if (info == null) continue;
                if (info.chapter_id == activityInfo.chapter_id)
                {
                    activityInfo = info;
                }
            }
        }
        return activityInfo;
    }

    private EndlessDungeonInfo GetEndlessInfo()
    {
        if (PlayerData.Instance._EndlessDungeonList != null && endlessInfo != null)
        {
            for (int i = 0; i < PlayerData.Instance._EndlessDungeonList.Count; i++)
            {
                EndlessDungeonInfo info = PlayerData.Instance._EndlessDungeonList[i];
                if (info == null) continue;
                if (info.chapter_id == endlessInfo.chapter_id)
                {
                    endlessInfo = info;
                }
            }
        }
        return endlessInfo;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_CloseButton;
        UIEventListener.Get(view.Btn_Back.gameObject).onClick = ButtonEvent_Back;
        UIEventListener.Get(view.Btn_SingleSweep.gameObject).onClick = ButtonEvent_SingleSweep;
        UIEventListener.Get(view.Btn_MulitSweep.gameObject).onClick = ButtonEvent_MulitSweep;
        UIEventListener.Get(view.Btn_ReadyBattle.gameObject).onClick = ButtonEvent_ReadyBattle;
        UIEventListener.Get(view.Btn_DirectBattle.gameObject).onClick = ButtonEvent_DirectBattle;
        UIEventListener.Get(view.Btn_Purchase.gameObject).onClick = ButtonEvent_PurchaseSweep;
        UIEventListener.Get(view.Btn_Next.gameObject).onClick = ButtonEvent_Next;
        UIEventListener.Get(view.Btn_StartBattle.gameObject).onClick = ButtonEvent_StartBattle;
        UIEventListener.Get(view.Btn_CloseEquipSelect.gameObject).onClick = ButtonEvent_ColseEquipSelect;
        UIEventListener.Get(view.Gobj_EquipPet).onClick = ButtonEvent_ChangePet;
    }

    private void ClearLineupPanel()
    {

    }
    //界面动画
    ////public void PlayOpenLineupSelectAnim()
    ////{
    ////    //view.LineupSelect_TScale.delay = 10;
    ////    //yield return null;
    ////    //view.Gobj_LineupSelect.SetActive(true);
    ////    CommonFunction.PlayOpenAnimation(view.LineupSelect_TScale.gameObject, false);
    ////    //view.LineupSelect_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    ////    //view.LineupSelect_TScale.ResetToBeginning();
    ////    //view.LineupSelect_TScale.PlayForward();
    ////    //view.LineupSelect_TScale.SetOnFinished(ReadyBattle);

    ////}
    ////public void PlayOpenStageInfoGroupAnim()
    ////{
    ////    view.StageInfoGroup_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    ////    view.StageInfoGroup_TScale.Restart();
    ////    view.StageInfoGroup_TScale.PlayForward();

    ////}

    public void PlayLearderShipAnim()
    {
        TweenScale tweenScale = view.Gobj_LeadershipLimitGroup.GetComponent<TweenScale>();
        if (tweenScale == null)
        {
            tweenScale = view.Gobj_LeadershipLimitGroup.AddComponent<TweenScale>();
            tweenScale.from = Vector3.one * 1.2f;
            tweenScale.to = Vector3.one;
            tweenScale.duration = 0.2f;
            tweenScale.ignoreTimeScale = true;
            tweenScale.delay = 0f;
            tweenScale.style = UITweener.Style.Once;
        }
        tweenScale.ResetToBeginning();
        tweenScale.PlayForward();
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
        FightRelatedModule.Instance.isSweep = false;
        FightRelatedModule.Instance.isOpenNext = false;
        FightRelatedModule.Instance.isFightState = false;
        PlayerData.Instance.NotifyResetEvent -= UpdateResetData;
        PlayerData.Instance.UpdateVipEvent -= UpdateVIPData;
        PlayerData.Instance.UpdateLevelEvent -= UpdateLevelData;
        PetSystemModule.Instance.PetUpdateEvent -= UpdatePetInfo;
        //Scheduler.Instance.RemoveUpdator(UpdateSweepStatus);
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        awards_dic.Clear();
        enemy_dic.Clear();
        ownsoldier_dic.Clear();
        ready_dic.Clear();
        starlist.Clear();
        ownEquip_dic.Clear();
        readyEquip_dic.Clear();
        readyLock_dic.Clear();
        Scheduler.Instance.RemoveTimer(DelayEnterBattle);
    }
}
