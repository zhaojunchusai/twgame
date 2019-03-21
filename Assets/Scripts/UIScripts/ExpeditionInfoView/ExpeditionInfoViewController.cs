using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
public class ExpeditionInfoViewController : UIBase
{
    public ExpeditionInfoView view;
    private ExpeEnemyPlayerComponent enemyComp;
    /// <summary>
    /// 敌方阵容
    /// </summary>
    private List<ExpeEnemyLineupComponent> enemy_dic;
    /// <summary>
    /// 当前拥有的可出战士兵
    /// </summary>
    private List<ExpeditionSoldierComponent> ownsoldier_dic;
    /// <summary>
    /// 已经准备出战队的士兵
    /// </summary>
    private List<LineupItemComponent> ready_dic;
    public List<fogs.proto.msg.SoldierList> readysoldierlist;

    private List<SoldierList> ownSoldierList;

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
    private ReadyEquipData[] readyEquipArray = new ReadyEquipData[6];
    public override void Initialize()
    {
        if (view == null)
        {
            view = new ExpeditionInfoView();
            view.Initialize();
        }
        if (enemy_dic == null)
            enemy_dic = new List<ExpeEnemyLineupComponent>();
        if (ownsoldier_dic == null)
            ownsoldier_dic = new List<ExpeditionSoldierComponent>();
        if (ready_dic == null)
            ready_dic = new List<LineupItemComponent>();
        if (enemyComp == null)
        {
            enemyComp = new ExpeEnemyPlayerComponent(view.Gobj_PlayerInfoComp);
        }
        if (ownSoldierList == null)
            ownSoldierList = new List<SoldierList>();
        if (ownEquip_dic == null)
            ownEquip_dic = new List<GateInfoOwnEquipComponent>();
        if (readyEquip_dic == null)
            readyEquip_dic = new List<GateInfoEquipComponent>();
        if (ownEquipList == null)
            ownEquipList = new List<Weapon>();
        ownEquipList.Clear();
        BtnEventBinding();
        view.Gobj_LineupSelect.SetActive(false);
        view.Gobj_EnemyInfoComp.SetActive(false);
        view.Gobj_OwnSoldierComp.SetActive(false);
        view.Gobj_ReadySoldierComp.SetActive(false);
        view.Gobj_StageInfoGroup.SetActive(true);
        view.Gobj_OwnEquipComp.SetActive(false);
        view.Gobj_ReadyEquipComp.SetActive(false);
        view.Gobj_EquipSelect.SetActive(false);
        FightRelatedModule.Instance.isFightState = false;
        // PlayOpenAnim();
        view.Wrap_OwnSoldier.onInitializeItem = UpdateOwnSoldierInfo;
        view.UIWrapContent_OwnEquip.onInitializeItem = UpdateOwnEquipInfo;
        PetSystemModule.Instance.PetUpdateEvent += OnUpdatePetInfo;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
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



    #region Update Method

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
                    if (readyEquipArray[3].weapon == null || readyEquipArray[4].weapon == null || readyEquipArray[5].weapon == null)
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
                break;
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
            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(readysoldierlist[i].uid);
            if (soldier == null) continue;
            soldierList.Add(soldier, readysoldierlist[i].num);
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
    }

    private void CloseEquipPanel()
    {
        view.Gobj_EquipSelect.SetActive(false);
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

    private void UpdateEquipPanel()
    {
        UpdateOwnEquipsData();
        Main.Instance.StartCoroutine(CreateOwnEquips());
        UpdateCombatPower();
        UpdatePetInfo();
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
        view.Grd_OwnEquip.Reposition();
        yield return null;
        view.ScrView_OwnEquip.ResetPosition();
    }

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

    private IEnumerator CreateReadyEquipComps()
    {
        if (readyEquip_dic.Count < 6)
        {
            int index = readyEquip_dic.Count;
            for (int i = index; i < 6; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_ReadyEquipComp.gameObject, view.Grd_RedayEquipGrid.transform);
                GateInfoEquipComponent comp = new GateInfoEquipComponent();
                comp.MyStart(go);
                comp.AddEventListener(ButtonEvent_ReadyEquip);
                go.name = i.ToString();
                go.SetActive(false);
                readyEquip_dic.Add(comp);
            }
            yield return null;
            view.Grd_RedayEquipGrid.repositionNow = true;
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
            if (comp == null) continue;
            if (index < readyEquipArray.Length)
            {
                if (readyEquipArray[index] == null) continue;
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

    private void OnUpdatePetInfo()
    {
        UpdatePetInfo();
        UpdateCombatPower();
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
    public void UpdateInfo()
    {
        UpdateView();
        UpdateEnemyCast();
    }
    private void UpdateView()
    {
        ExpeditionInfo expeditionInfo = PlayerData.Instance._ExpeditionInfo;
        if (expeditionInfo.enemies != null && expeditionInfo.enemies.hero != null)
        {
            enemyComp.UpdateInfo(expeditionInfo.enemies.hero.icon, expeditionInfo.enemies.hero.icon_frame, PlayerData.Instance._ExpeditionInfo.enemies.hero.level);
            // enemyComp.UpdateInfo(CommonFunction.GetHeroIconNameByGender((EHeroGender)PlayerData.Instance._ExpeditionInfo.enemies.hero.gender), PlayerData.Instance._ExpeditionInfo.enemies.hero.level);
            ExpeditionData _cData = ConfigManager.Instance.mExpeditionConfig.GetExpeditionDataByID(expeditionInfo.cur_gate_id);
            if (_cData != null)
            {
                view.Lbl_PlayerName.text = expeditionInfo.enemies.hero.charname + "(" + _cData.progress + "/" + GlobalCoefficient.ExpeditionGateCount + ")";
                if (string.IsNullOrEmpty(expeditionInfo.enemies.hero.unionname))
                {
                    view.Lbl_UnionName.text = ConstString.HINT_NO;
                }
                else
                {
                    view.Lbl_UnionName.text = expeditionInfo.enemies.hero.unionname;
                }
            }
        }
        else
        {
            view.Lbl_PlayerName.text = string.Empty;
            view.Lbl_UnionName.text = string.Empty;
        }
        view.Gobj_LineupSelect.SetActive(false);
    }

    private void UpdateEnemyCast()
    {
        if (enemy_dic.Count < GlobalCoefficient.LineupSoldierLimit)
        {
            for (int index = enemy_dic.Count; index < GlobalCoefficient.LineupSoldierLimit; index++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_EnemyInfoComp, view.Grd_EnemyGrid.transform);
                ExpeEnemyLineupComponent comp = new ExpeEnemyLineupComponent();
                comp.MyStart(go);
                go.name = "enemy_" + index;
                enemy_dic.Add(comp);
                go.SetActive(true);
            }
        }
        List<ArenaSoldier> soldiers = new List<ArenaSoldier>();
        List<ArenaSoldier> left = new List<ArenaSoldier>();
        List<ArenaSoldier> right = new List<ArenaSoldier>();
        for (int i = 0; i < PlayerData.Instance._ExpeditionInfo.enemies.soldiers.Count; i++)
        {
            ArenaSoldier soldier = PlayerData.Instance._ExpeditionInfo.enemies.soldiers[i];
            if (soldier.num <= 0)
            {
                right.Add(soldier);
            }
            else
            {
                left.Add(soldier);
            }
        }
        soldiers.AddRange(left);
        soldiers.AddRange(right);
        for (int i = 0; i < enemy_dic.Count; i++)
        {
            ExpeEnemyLineupComponent comp = enemy_dic[i];
            if (i < soldiers.Count)
            {
                fogs.proto.msg.ArenaSoldier enemySoldier = soldiers[i];
                Soldier tmpEnemySoldier = Soldier.createByID(enemySoldier.soldier.id);
                tmpEnemySoldier.Serialize(enemySoldier.soldier);
                tmpEnemySoldier.SerializeShowInfo(enemySoldier.soldier.attr);
                comp.UpdateInfo(tmpEnemySoldier);
                comp.UpdateNum(enemySoldier.num);
                comp.IsShowEnergy = true;
                comp.IsShowLeader = false;
                comp.IsSelect = false;
                comp.IsEnable = true;
                if (enemySoldier.num <= 0)
                {
                    comp.IsDead = true;
                }
                else
                {
                    comp.IsDead = false;
                }
                comp.mRootObject.SetActive(true);
            }
            else
            {
                comp.mRootObject.SetActive(false);
            }
        }
        view.Grd_EnemyGrid.repositionNow = true;
    }

    private void UpdateLineupInfo()
    {
        ownSoldierList = GetCastSoldiers();
        readysoldierlist = new List<fogs.proto.msg.SoldierList>();
        if (PlayerPrefsTool.HasKey(AppPrefEnum.ExpeditionSoldier))
        {
            List<ulong> saveUIDs = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.ExpeditionSoldier);
            if (saveUIDs != null)
            {
                for (int i = 0; i < saveUIDs.Count; i++)
                {
                    for (int j = 0; j < ownSoldierList.Count; j++)
                    {
                        SoldierList soldierList = ownSoldierList[j];
                        if (soldierList.uid == saveUIDs[i] && soldierList.num != 0) //已经阵亡的 不添加
                        {
                            readysoldierlist.Add(soldierList);
                        }
                    }
                }
            }
        }
        Main.Instance.StartCoroutine(CreateOwnSoldier());
        Main.Instance.StartCoroutine(UpdateReadySoldier());
    }

    /// <summary>
    /// 更新相同类型的士兵状态
    /// </summary>
    public void UpdateSameSoldierStatus()
    {
        for (int i = 0; i < ownsoldier_dic.Count; i++)
        {
            ExpeditionSoldierComponent comp = ownsoldier_dic[i];
            if (!comp.IsDead && !comp.IsLowLevel)
            {
                comp.IsEnable = !CheckSameSoldier(comp.soldierAtt);
            }
        }
    }

    private void UpdateCastSoldierCount()
    {
        int count = 0;
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            SoldierList _info = readysoldierlist[i];
            count += _info.num;
        }
        view.Lbl_GeneralNum.text = count.ToString();
    }

    private IEnumerator CreateOwnSoldier()
    {
        view.ScrView_OwnSoldier.ResetPosition();
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
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                ownsoldier_dic[i].mRootObject.SetActive(false);
            }
        }
        yield return null;
        for (int i = 0; i < count; i++)
        {
            ExpeditionSoldierComponent comp = null;
            SoldierList _info = ownSoldierList[i];
            Soldier _soldier = PlayerData.Instance._SoldierDepot.FindByUid(_info.uid);
            if (_soldier == null) continue;
            if (i < itemCount)
            {
                comp = ownsoldier_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_OwnSoldierComp, view.Grd_OwnSoldier.transform);
                comp = new ExpeditionSoldierComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_OwnSoldier);

                ownsoldier_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(_soldier, _info.num);
            comp.IsDead = false;
            if (_info.num == 0)
            {
                comp.IsDead = true;
            }
            comp.IsSelect = IsSelectReadySoldier(_soldier);
            if (!comp.IsSelect && !comp.IsDead)
            {
                comp.IsEnable = true;
            }
            else
            {
                comp.IsEnable = false;
            }
            if (comp.soldierAtt.Level < GlobalCoefficient.ExpeditonSoldierLevel)
            {
                comp.IsLowLevel = true;
                comp.IsDead = false;
                comp.IsEnable = false;
            }
            else
            {
                comp.IsLowLevel = false;
            }
            comp.IsShowEnergy = true;
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

    private void UpdateOwnSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.Wrap_OwnSoldier.enabled == false) return;
        if (realIndex >= ownSoldierList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            ExpeditionSoldierComponent comp = ownsoldier_dic[wrapIndex];
            SoldierList _info = ownSoldierList[realIndex];
            Soldier _soldier = PlayerData.Instance._SoldierDepot.FindByUid(_info.uid);
            comp.UpdateCompInfo(_soldier, _info.num);
            comp.IsShowEnergy = true;
            if (comp.soldierAtt.Level < GlobalCoefficient.ExpeditonSoldierLevel)  //首先判定等级是否满足 
            {
                comp.IsSelect = false;
                comp.IsLowLevel = true;
                comp.IsDead = false;
                comp.IsEnable = false;
            }
            else
            {
                comp.IsLowLevel = false;
                if (_info.num <= 0)
                {
                    comp.IsDead = true;
                }
                else
                {
                    comp.IsDead = false;
                    comp.IsSelect = IsSelectReadySoldier(_soldier);
                    if (comp.IsSelect)
                    {
                        comp.IsMask = false;
                    }
                    if (!comp.IsSelect)
                    {
                        comp.IsEnable = !CheckSameSoldier(_soldier);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取可用的上阵士兵数据
    /// </summary>
    /// <returns></returns>
    private List<SoldierList> GetCastSoldiers()
    {
        List<SoldierList> _soldierList = new List<SoldierList>();  //满足条件的士兵
        List<SoldierList> _ableSoldierList = new List<SoldierList>();   //可用士兵
        List<SoldierList> _deadSoldierList = new List<SoldierList>();   //已经死亡的士兵
        List<Soldier> _playerSoldiers = new List<Soldier>(PlayerData.Instance._SoldierDepot._soldierList.ToArray());
        List<SoldierList> _remainSoldiers = new List<SoldierList>(PlayerData.Instance._ExpeditionInfo.remain_army);  //目前可用的士兵
        for (int i = 0; i < _playerSoldiers.Count; i++)
        {
            Soldier _soldier = _playerSoldiers[i];
            //  if (_soldier.Lv < GlobalCoefficient.ExpeditonSoldierLevel) continue;   //不满足远征等级限制的士兵不添加到列表
            SoldierList _info = new SoldierList();
            _info.uid = _soldier.uId;
            _info.num = 0;
            for (int j = 0; j < _remainSoldiers.Count; j++)
            {
                SoldierList _remain = _remainSoldiers[j];
                if (_soldier.uId == _remain.uid)
                {
                    _info.num = _remain.num;
                }
            }
            if (_info.num != 0)
            {
                _ableSoldierList.Add(_info);
            }
            else
            {
                _deadSoldierList.Add(_info);
            }
        }
        _soldierList.AddRange(_ableSoldierList);
        _soldierList.AddRange(_deadSoldierList);   //已经死亡的士兵排在最后边 且不可点击
        return _soldierList;
    }

    private List<fogs.proto.msg.SoldierList> SortReadySoldiers(List<fogs.proto.msg.SoldierList> needSortList)
    {
        List<fogs.proto.msg.SoldierList> list = new List<SoldierList>();
        List<Soldier> OwnSoldierList = new List<Soldier>();
        for (int i = 0; i < needSortList.Count; i++)
        {
            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(needSortList[i].uid);
            if (soldier != null)
                OwnSoldierList.Add(soldier);
        }
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

        for (int i = 0; i < OwnSoldierList.Count; i++)
        {
            SoldierList soldierList = new SoldierList();
            soldierList.uid = OwnSoldierList[i].uId;
            for (int j = 0; j < needSortList.Count; j++)
            {
                if (soldierList.uid == needSortList[j].uid)
                {
                    soldierList.num = needSortList[j].num;
                    continue;
                }
            }
            list.Add(soldierList);
        }
        return list;
    }

    private IEnumerator UpdateReadySoldier()
    {
        readysoldierlist = SortReadySoldiers(readysoldierlist);
        if (readysoldierlist.Count <= ready_dic.Count)
        {
            for (int i = 0; i < ready_dic.Count; i++)
            {
                LineupItemComponent comp = ready_dic[i];
                if (i < readysoldierlist.Count)
                {
                    SoldierList _info = readysoldierlist[i];
                    Soldier _soldier = PlayerData.Instance._SoldierDepot.FindByUid(_info.uid);
                    comp.UpdateInfo(_soldier);
                    comp.UpdateNum(_info.num);
                    comp.IsSelect = false;
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int ob_count = ready_dic.Count;   //已经生成的物件数量
            for (int i = 0; i < readysoldierlist.Count; i++)
            {
                LineupItemComponent comp = null;
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
                if (comp == null) continue;
                SoldierList _info = readysoldierlist[i];
                if (_info == null) continue;
                Soldier _soldier = PlayerData.Instance._SoldierDepot.FindByUid(_info.uid);
                comp.UpdateInfo(_soldier);
                comp.UpdateNum(_info.num);
                comp.IsSelect = false;
                comp.IsShowEnergy = false;
                comp.IsShowLeader = false;
                comp.IsEnable = true;
                comp.mRootObject.SetActive(true);
            }
        }
        yield return null;
        view.Grd_RedaySoldierGrid.Reposition();
        UpdateCastSoldierCount();
    }

    /// <summary>
    /// 检测同名武将 true存在同名武将
    /// </summary>
    /// <param name="soldierID"></param>
    /// <returns></returns>
    private bool CheckSameSoldier(Soldier ownSoldier)
    {
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(readysoldierlist[i].uid);
            if (soldier == null) continue;
            if (ownSoldier.Att.type.Equals(soldier.Att.type))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 更新准备出战的士兵数据 默认删除已出战士兵
    /// 返回false 则说明已达出战士兵上线
    /// </summary>
    private bool UpdateReadySoldierData(Soldier data, int num, bool isAdd = false)
    {
        if (isAdd)
        {
            if (readysoldierlist.Count >= GlobalCoefficient.LineupSoldierLimit)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_LINEUPPANEL_SOLDIERLIMIT);
                return false;
            }
            else
            {
                SoldierList _info = new SoldierList();
                _info.uid = data.uId;
                _info.num = num;
                readysoldierlist.Add(_info);
                return true;
            }
        }
        else
        {
            List<SoldierList> _list = new List<SoldierList>();
            for (int i = 0; i < readysoldierlist.Count; i++)
            {
                if (readysoldierlist[i].uid != data.uId)
                {
                    _list.Add(readysoldierlist[i]);
                }
            }
            readysoldierlist.Clear();
            readysoldierlist.AddRange(_list);
            return true;
        }
    }


    private void DelOwnSoldierData(Soldier soldier)
    {
        for (int i = 0; i < ownsoldier_dic.Count; i++)
        {
            ExpeditionSoldierComponent comp = ownsoldier_dic[i];
            if (comp.soldierAtt.Equals(soldier))
            {
                comp.IsSelect = false;
                break;
            }
        }
    }

    #endregion

    #region Button Event
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
    }
    private void ButtonEvent_OwnSoldier(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ExpeditionSoldierComponent comp = baseComp as ExpeditionSoldierComponent;
        if (comp == null)
        {
            Debug.LogError("can not get LineupItemComponent");
            return;
        }
        if (!comp.IsEnable || comp.IsSelect) return;
        if (CheckSameSoldier(comp.soldierAtt))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_SOLDIER_SAME);
            return;
        }
        if (UpdateReadySoldierData(comp.soldierAtt, comp.soldierCount, !comp.IsSelect))
        {
            Main.Instance.StartCoroutine(UpdateReadySoldier());
            comp.IsSelect = IsSelectReadySoldier(comp.soldierAtt);
            comp.IsEnable = !comp.IsSelect;
        }
        UpdateSameSoldierStatus();
    }
    /// <summary>
    /// 已准备出站的士兵
    /// </summary>
    public void ButtonEvent_ReadySoldier(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        LineupItemComponent comp = baseComp as LineupItemComponent;
        if (comp == null)
        {
            Debug.LogError("can not get LineupItemComponent ");
            return;
        }
        if (UpdateReadySoldierData(comp.soldierAtt, comp.soldierCount, false))
        {
            DelOwnSoldierData(comp.soldierAtt);
            Main.Instance.StartCoroutine(UpdateReadySoldier());
        }
        UpdateSameSoldierStatus();
    }

    public bool IsSelectReadySoldier(Soldier soldier)
    {
        if (readysoldierlist == null)
            readysoldierlist = new List<SoldierList>();
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            if (readysoldierlist[i].uid.Equals(soldier.uId))
            {
                return true;
            }
        }
        return false;
    }

    public void ButtonEvent_Exit(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EXPEDITIONINFO);
        //UISystem.Instance.ExpeditionView.PlayOpenAnim();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

    }

    public void ButtonEvent_ReadyBattle(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        if (FightRelatedModule.Instance.mCastleInfo.mAttribute.HP <= 0)
        {
            ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.ExpeditionCastleDestroy);
            return;
        }
        ReadyBattle();
        // PlayOpenLineupSelect();
    }

    public void ButtonEvent_Colse(GameObject btn)
    {
        view.Gobj_LineupSelect.SetActive(false);
        //  PlayOpenAnim();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

    }

    public void ButtonEvent_ColseEquipSelect(GameObject btn)
    {
        if (FightRelatedModule.Instance.isFightState)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        CloseEquipPanel();
    }
    private void ButtonEvent_ChangePet(GameObject go)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);
        UISystem.Instance.PetChooseView.UpdateViewInfo();
    }
    public void ButtonEvent_StartBattle(GameObject btn)
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
            FightRelatedModule.Instance.SendOneKeyReplaceEquip(EFightType.eftExpedition, offEquips, upEquips);
        }
        else
        {
            OneKeyEquipSuccess();
        }
    }

    public void ButtonEvent_Next(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));

        List<SoldierList> list = new List<SoldierList>();
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            list.Add(readysoldierlist[i]);
        }
        if (readysoldierlist.Count == 0)
        {
            //策划在禅道中提出设计修改(140) add by taiwei
            //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GATE_LINEUPPANEL_NOSOLDIER,
            //    () =>
            //    {
            //        //EnterTheBattle();
            //        FightRelatedModule.Instance.SendStartExpedition(list);
            //    },
            //    null,
            //    ConstString.HINT_LEFTBUTTON_GOON,
            //    ConstString.HINT_RIGHTBUTTON_CANCEL
            //   );
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_NOSOLDIER);
            return;
        }
        OpenEquipPanel();
        //FightRelatedModule.Instance.SendDungeonStart(stageinfo.ID, list);
    }

    #endregion

    public override void Uninitialize()
    {
        FightRelatedModule.Instance.isFightState = false;

        PetSystemModule.Instance.PetUpdateEvent -= OnUpdatePetInfo;
        Main.Instance.StopCoroutine(CreateOwnSoldier());
        Clear();
    }
    /// <summary>
    /// 准备战斗
    /// </summary>
    public void ReadyBattle()
    {
        view.Gobj_LineupSelect.SetActive(true);
        UpdateLineupInfo();
    }

    public void OneKeyEquipSuccess()
    {
        List<SoldierList> list = new List<SoldierList>();
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            list.Add(readysoldierlist[i]);
        }
        FightRelatedModule.Instance.SendStartExpedition(list);
    }

    /// <summary>
    /// 进入战斗
    /// </summary>
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
        List<FightSoldierInfo> list = new List<FightSoldierInfo>();
        List<ulong> soldierUIDs = new List<ulong>();
        for (int i = 0; i < readysoldierlist.Count; i++)
        {
            SoldierList readySoldier = readysoldierlist[i];
            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(readySoldier.uid);
            if (soldier == null)  //如果找不到该武将  则不添加数据 add by taiwei 
            {
                continue;
            }
            soldierUIDs.Add(soldier.uId);
            FightSoldierInfo info = new FightSoldierInfo(soldier, readySoldier.num);
            list.Add(info);
        }
        PlayerPrefsTool.WriteObject<List<ulong>>(AppPrefEnum.ExpeditionSoldier, soldierUIDs);
        ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.ExpeditionSoldier, soldierUIDs);
        UISystem.Instance.FightView.SetViewInfo(EFightType.eftExpedition, readysoldierlist, list, PlayerData.Instance._PetDepot.GetEquipedPet());
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_ViewMask.gameObject).onClick = ButtonEvent_Exit;
        UIEventListener.Get(view.Btn_ReadyBattle.gameObject).onClick = ButtonEvent_ReadyBattle;
        UIEventListener.Get(view.Btn_Colse.gameObject).onClick = ButtonEvent_Colse;
        UIEventListener.Get(view.Btn_Next.gameObject).onClick = ButtonEvent_Next;
        UIEventListener.Get(view.Btn_StartBattle.gameObject).onClick = ButtonEvent_StartBattle;
        UIEventListener.Get(view.Btn_CloseEquipSelect.gameObject).onClick = ButtonEvent_ColseEquipSelect;
        UIEventListener.Get(view.Gobj_EquipPet).onClick = ButtonEvent_ChangePet;
    }

    public void Clear()
    {
        view.Lbl_UnionName.text = string.Empty;
        view.Lbl_PlayerName.text = string.Empty;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        enemy_dic.Clear();
        ownsoldier_dic.Clear();
        ready_dic.Clear();
        enemyComp = null;
        readyEquip_dic.Clear();
        ownEquip_dic.Clear();
        Scheduler.Instance.RemoveTimer(DelayEnterBattle);
    }

    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();

    //}
    //public void PlayOpenLineupSelect()
    //{
    //    view.LineupSelect_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.LineupSelect_TScale.Restart();
    //    view.LineupSelect_TScale.PlayForward();
    //}
}

