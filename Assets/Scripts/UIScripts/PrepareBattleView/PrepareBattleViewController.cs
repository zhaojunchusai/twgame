using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;

public class PrepareBattleViewController : UIBase
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

    private class SoldierData
    {
        public Soldier soldier;
        public int count;

        public SoldierData()
        {

        }

        public SoldierData(Soldier data, int num)
        {
            soldier = data;
            count = num;
        }
    }

    private PrepareBattleView view;
    private PrepareCommonData mCommonData;
    /// <summary>
    /// 锁定的
    /// </summary>
    private List<GateInfoReadyLockComponent> readyLock_dic;
    private List<ExpeditionSoldierComponent> ownSoldier_dic;
    private List<LineupItemComponent> readySoldier_dic;
    /// <summary>
    /// 当前拥有的神器
    /// </summary>
    private List<PVPOwnEquipTypeComponent> ownEquip_dic;
    /// <summary>
    /// 当前准备携带的装备
    /// </summary>
    private List<PVPEquipComponent> readyEquip_dic;
    private ReadyEquipData[] readyEquipArray = new ReadyEquipData[6];
    private List<SoldierData> readySoldierList = new List<SoldierData>();     // 准备出战的士兵
    private List<Weapon> ownEquipList = new List<Weapon>();                   // 当前拥有的装备数据
    private List<SoldierData> ownSoldierList = new List<SoldierData>();       // 当前拥有的士兵数据
    private int battleGenerals = 0; // 上阵武将数
    private int dominanceForce = 0; // 统御力
    private int currentEnergy = 0;         //能量 
    private float battleReadyTime = 0;
    private PetData currentPetData;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new PrepareBattleView();
            view.Initialize();
            BtnEventBinding();
        }
        FightRelatedModule.Instance.isFightState = false;
        UnionModule.Instance.isFightState = false;
        if (ownSoldier_dic == null)
            ownSoldier_dic = new List<ExpeditionSoldierComponent>();
        if (readySoldier_dic == null)
            readySoldier_dic = new List<LineupItemComponent>();
        if (ownEquip_dic == null)
            ownEquip_dic = new List<PVPOwnEquipTypeComponent>();
        if (readyEquip_dic == null)
            readyEquip_dic = new List<PVPEquipComponent>();
        if (readyLock_dic == null)
            readyLock_dic = new List<GateInfoReadyLockComponent>();
        InitView();
        currentPetData = null;
        PetSystemModule.Instance.PetChooseEvent += OnPetChoose;
    }

    private void InitView()
    {
        view.Gobj_EquipSelect.SetActive(false);
        view.Gobj_Countdown.SetActive(false);
        view.Gobj_LockSoldierComp.SetActive(false);
        view.Gobj_OwnEquipComp.SetActive(false);
        view.Gobj_OwnSoldierComp.SetActive(false);
        view.Gobj_ReadyEquipComp.SetActive(false);
        view.Gobj_ReadySoldierComp.SetActive(false);
        view.Gobj_GeneralNum.SetActive(false);
        view.Gobj_EnergyGroup.SetActive(false);
        view.Gobj_LineupSelect.SetActive(true);
        view.UIWrapContent_OwnSoldier.onInitializeItem = UpdateWrapOwnSoldierInfo;
        view.UIWrapContent_OwnEquip.onInitializeItem = UpdateWrapOwnEquipInfo;
    }

    private void InitPetInfo()
    {
        currentPetData = null;
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
            uint petID = 0;
            switch (mCommonData.FightType)
            {
                case EFightType.eftActivity:
                case EFightType.eftEndless:
                case EFightType.eftMain:
                case EFightType.eftExpedition:
                    {
                        currentPetData = PlayerData.Instance._PetDepot.GetEquipedPet();
                    }
                    break;
                case EFightType.eftHegemony:
                    {
                        if (PlayerPrefsTool.HasKey(AppPrefEnum.HegemonyPet))
                        {
                            petID = (uint)PlayerPrefsTool.ReadInt(AppPrefEnum.HegemonyPet);
                        }
                    } break;
                case EFightType.eftQualifying:
                    {
                        petID = (uint)QualifyingModule.Instance.poleLobbyData.pettypeid;
                    } break;
                case EFightType.eftPVP:
                    {
                        petID = (uint)PVPModule.Instance.PvpData.pettypeid;
                    } break;
                case EFightType.eftSlave:
                    {
                        if (PlayerPrefsTool.HasKey(AppPrefEnum.SlavePet))
                        {
                            petID = (uint)PlayerPrefsTool.ReadInt(AppPrefEnum.SlavePet);
                        }
                    } break;
                case EFightType.eftUnion:
                    {
                        if (PlayerPrefsTool.HasKey(AppPrefEnum.ExoticAdvanturePet))
                        {
                            petID = (uint)PlayerPrefsTool.ReadInt(AppPrefEnum.ExoticAdvanturePet);
                        }
                    }
                    break;
                case EFightType.eftCaptureTerritory:
                    {
                        if (PlayerPrefsTool.HasKey(AppPrefEnum.CaptureTerritoryPet))
                        {
                            petID = (uint)PlayerPrefsTool.ReadInt(AppPrefEnum.CaptureTerritoryPet);
                        }
                    }
                    break;
                case EFightType.eftServerHegemony:
                    {
                        petID = SupermacyModule.Instance.PetTypeID;
                    }
                    break;
                case EFightType.eftCrossServerWar:
                    {
                        if (PlayerPrefsTool.HasKey(AppPrefEnum.CrossServerWarPet))
                        {
                            petID = (uint)PlayerPrefsTool.ReadInt(AppPrefEnum.CrossServerWarPet);
                        }
                    }
                    break;
            }
            if (petID != 0)
            {
                currentPetData = PlayerData.Instance._PetDepot.GetPetDataByTypeID(petID);
            }
            else
            {
                currentPetData = null;
            }
            if (currentPetData == null)
            {
                view.Lbl_EquipedPetName.text = string.Format(ConstString.PET_PREPAREBATTLE_EQUIPEDPET, ConstString.HINT_NO);
            }
            else
            {
                view.Lbl_EquipedPetName.text = string.Format(ConstString.PET_PREPAREBATTLE_EQUIPEDPET, currentPetData.PetInfo.name);
            }
            view.Gobj_EquipPet.SetActive(true);
        }
    }

    public void UpdateViewInfo(PrepareCommonData data)
    {
        mCommonData = data;
        if (data == null)
            return;
        OpenSoldierSelect();
    }

    public void UpdateViewInfo(EFightType type)
    {
        mCommonData = new PrepareCommonData();
        mCommonData.FightType = type;
        OpenSoldierSelect();
    }
    public void UpdateViewInfo(EFightType type, ArenaPlayer info)
    {
        mCommonData = new PrepareCommonData();
        mCommonData.FightType = type;
        mCommonData.ArenaPlayer = info;
        OpenSoldierSelect();
    }

    public void UpdateViewInfo(EFightType type, int city)
    {
        mCommonData = new PrepareCommonData();
        mCommonData.FightType = type;
        mCommonData.Other = city;
        OpenSoldierSelect();
    }

    public void UpdateViewInfo(EFightType type, StageInfo info)
    {
        mCommonData = new PrepareCommonData();
        mCommonData.FightType = type;
        mCommonData.StageInfo = info;
        OpenSoldierSelect();
    }

    public void UpdateViewInfo(EFightType type, UnionPveDgnInfo dgnInfo, StageInfo info)
    {
        mCommonData = new PrepareCommonData();
        mCommonData.FightType = type;
        mCommonData.StageInfo = info;
        mCommonData.UnionDgnInfo = dgnInfo;
        OpenSoldierSelect();
    }

    private void UpdateWrapOwnSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_OwnSoldier.enabled == false) return;
        if (realIndex >= ownSoldierList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            ExpeditionSoldierComponent comp = ownSoldier_dic[wrapIndex];
            SoldierData _info = ownSoldierList[realIndex];
            comp.UpdateCompInfo(_info.soldier, _info.count);
            comp.IsSelect = IsSelectReadySoldier(_info.soldier.uId);
            comp.IsShowEnergy = true;
            switch (mCommonData.FightType)
            {
                case EFightType.eftQualifying:
                case EFightType.eftSlave:
                    {
                        if (comp.IsSelect)
                        {
                            comp.IsMask = false;
                        }
                        if (!comp.IsSelect)
                        {
                            comp.IsEnable = (!CheckSameSoldier(_info.soldier)) && (!CheckEnergyLimit(_info.soldier));
                        }
                    }
                    break;
                case EFightType.eftHegemony:
                case EFightType.eftUnion:
                case EFightType.eftServerHegemony:
                case EFightType.eftCaptureTerritory:
                case EFightType.eftCrossServerWar:
                    {
                        if (comp.IsSelect)
                        {
                            comp.IsMask = false;
                        }
                        if (!comp.IsSelect)
                        {
                            comp.IsEnable = !CheckSameSoldier(comp.soldierAtt);
                        }
                    }
                    break;
            }
        }
    }
    private void UpdateWrapOwnEquipInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_OwnEquip.enabled == false) return;
        if (realIndex >= ownEquipList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            PVPOwnEquipTypeComponent comp = ownEquip_dic[wrapIndex];
            Weapon _info = ownEquipList[realIndex];
            comp.UpdateCompInfo(_info);
            comp.IsSelect = IsSelectedOwnEquip(_info);
        }
    }

    #region 武将选择

    private void OpenSoldierSelect()
    {
        view.Gobj_LineupSelect.SetActive(true);
        if (ownSoldierList == null)
        {
            ownSoldierList = new List<SoldierData>();
        }
        ownSoldierList.Clear();
        if (readySoldierList == null)
        {
            readySoldierList = new List<SoldierData>();
        }
        readySoldierList.Clear();
        switch (mCommonData.FightType)
        {
            case EFightType.eftSlave:
                {
                    for (int i = 0; i < PlayerData.Instance._SoldierDepot._soldierList.Count; i++)
                    {
                        Soldier soldier = PlayerData.Instance._SoldierDepot._soldierList[i];
                        if (soldier == null)
                            continue;
                        SoldierData soldierData = new SoldierData(soldier, 0);
                        ownSoldierList.Add(soldierData);
                    }
                    view.Gobj_LeadershipGroup.SetActive(false);
                    view.Gobj_GeneralNum.SetActive(false);
                    view.Btn_Save.gameObject.SetActive(false);
                    view.Btn_StartBattle.gameObject.SetActive(true);
                    view.Gobj_EnergyGroup.SetActive(true);
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.SlaveReadySoldier))
                    {
                        List<SoldierList> prefSoldiers = PlayerPrefsTool.ReadObject<List<SoldierList>>(AppPrefEnum.SlaveReadySoldier);
                        if (prefSoldiers != null)
                        {
                            for (int i = 0; i < prefSoldiers.Count; i++)
                            {
                                SoldierList mSoldier = prefSoldiers[i];
                                for (int j = 0; j < ownSoldierList.Count; j++)
                                {
                                    SoldierData soldierData = ownSoldierList[j];
                                    if (soldierData == null || soldierData.soldier == null) continue;
                                    if (soldierData.soldier.uId == mSoldier.uid && mSoldier.num != 0)
                                    {
                                        soldierData.count = mSoldier.num;
                                        readySoldierList.Add(soldierData);
                                    }
                                }
                            }
                        }
                    }
                    battleReadyTime = GlobalCoefficient.PVPReadyTime;
                    Scheduler.Instance.AddUpdator(UpdateBattleReadyTime);
                } break;
            case EFightType.eftHegemony:
                {
                    for (int i = 0; i < PlayerData.Instance._SoldierDepot._soldierList.Count; i++)
                    {
                        Soldier soldier = PlayerData.Instance._SoldierDepot._soldierList[i];
                        if (soldier == null)
                            continue;
                        SoldierData soldierData = new SoldierData(soldier, GlobalCoefficient.PVPSoldierCountLimit);
                        ownSoldierList.Add(soldierData);
                    }

                    if (SupermacyModule.Instance.soldiers != null)
                    {
                        if (PlayerPrefsTool.HasKey(AppPrefEnum.HegemonySoldier))
                        {
                            List<SoldierList> soldiers = PlayerPrefsTool.ReadObject<List<SoldierList>>(AppPrefEnum.HegemonySoldier);
                            for (int i = 0; i < soldiers.Count; i++)
                            {
                                SoldierList mSoldier = soldiers[i];
                                for (int j = 0; j < ownSoldierList.Count; j++)
                                {
                                    SoldierData soldierData = ownSoldierList[j];
                                    if (soldierData == null || soldierData.soldier == null) continue;
                                    if (soldierData.soldier.uId == mSoldier.uid && mSoldier.num != 0)
                                    {
                                        soldierData.count = mSoldier.num;
                                        readySoldierList.Add(soldierData);
                                    }
                                }
                            }
                        }
                    }
                    view.Gobj_EnergyGroup.SetActive(false);
                    view.Gobj_LeadershipGroup.SetActive(false);
                    view.Gobj_GeneralNum.SetActive(true);
                    view.Btn_StartBattle.gameObject.SetActive(true);
                    view.Gobj_Countdown.SetActive(false);
                    view.Btn_Save.gameObject.SetActive(false);
                } break;
            case EFightType.eftUnion:
                {
                    for (int i = 0; i < PlayerData.Instance._SoldierDepot._soldierList.Count; i++)
                    {
                        Soldier soldier = PlayerData.Instance._SoldierDepot._soldierList[i];
                        if (soldier == null)
                            continue;
                        SoldierData soldierData = new SoldierData(soldier, GlobalCoefficient.PVPSoldierCountLimit);
                        ownSoldierList.Add(soldierData);
                    }
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.ExoticAdvantureSoldier))
                    {
                        List<ulong> saveUIDs = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.ExoticAdvantureSoldier);
                        if (saveUIDs != null)
                        {
                            for (int i = 0; i < saveUIDs.Count; i++)
                            {
                                for (int j = 0; j < ownSoldierList.Count; j++)
                                {
                                    SoldierData soldierData = ownSoldierList[j];
                                    if (soldierData == null || soldierData.soldier == null) continue;
                                    if (soldierData.soldier.uId == saveUIDs[i])
                                    {
                                        soldierData.count = GlobalCoefficient.PVPSoldierCountLimit;
                                        readySoldierList.Add(soldierData);
                                    }
                                }
                            }
                        }
                    }
                    view.Gobj_EnergyGroup.SetActive(false);
                    view.Gobj_LeadershipGroup.SetActive(false);
                    view.Gobj_GeneralNum.SetActive(true);
                    view.Btn_StartBattle.gameObject.SetActive(true);
                    view.Gobj_Countdown.SetActive(true);
                    view.Btn_Save.gameObject.SetActive(false);
                    battleReadyTime = GlobalCoefficient.PVPReadyTime;
                    Scheduler.Instance.AddUpdator(UpdateBattleReadyTime);
                } break;
            case EFightType.eftCaptureTerritory:
                {
                    for (int i = 0; i < PlayerData.Instance._SoldierDepot._soldierList.Count; i++)
                    {
                        Soldier soldier = PlayerData.Instance._SoldierDepot._soldierList[i];
                        if (soldier == null)
                            continue;
                        SoldierData soldierData = new SoldierData(soldier, GlobalCoefficient.PVPSoldierCountLimit);
                        ownSoldierList.Add(soldierData);
                    }
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.CaptureTerritorySoldier))
                    {
                        List<SoldierList> saveUIDs = PlayerPrefsTool.ReadObject<List<SoldierList>>(AppPrefEnum.CaptureTerritorySoldier);
                        if (saveUIDs != null)
                        {
                            for (int i = 0; i < saveUIDs.Count; i++)
                            {
                                for (int j = 0; j < ownSoldierList.Count; j++)
                                {
                                    SoldierData soldierData = ownSoldierList[j];
                                    if (soldierData == null || soldierData.soldier == null) continue;
                                    if (soldierData.soldier.uId == saveUIDs[i].uid)
                                    {
                                        soldierData.count = saveUIDs[i].num;
                                        readySoldierList.Add(soldierData);
                                    }
                                }
                            }
                        }
                    }
                    view.Gobj_EnergyGroup.SetActive(false);
                    view.Gobj_LeadershipGroup.SetActive(false);
                    view.Gobj_GeneralNum.SetActive(true);
                    view.Btn_StartBattle.gameObject.SetActive(true);
                    view.Btn_Save.gameObject.SetActive(false);
                    view.Gobj_Countdown.SetActive(false);
                } break;
            case EFightType.eftCrossServerWar:
                {
                    for (int i = 0; i < PlayerData.Instance._SoldierDepot._soldierList.Count; i++)
                    {
                        Soldier soldier = PlayerData.Instance._SoldierDepot._soldierList[i];
                        if (soldier == null)
                            continue;
                        SoldierData soldierData = new SoldierData(soldier, GlobalCoefficient.PVPSoldierCountLimit);
                        ownSoldierList.Add(soldierData);
                    }
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.CrossServerWarSoldier))
                    {
                        List<SoldierList> saveUIDs = PlayerPrefsTool.ReadObject<List<SoldierList>>(AppPrefEnum.CrossServerWarSoldier);
                        if (saveUIDs != null)
                        {
                            for (int i = 0; i < saveUIDs.Count; i++)
                            {
                                for (int j = 0; j < ownSoldierList.Count; j++)
                                {
                                    SoldierData soldierData = ownSoldierList[j];
                                    if (soldierData == null || soldierData.soldier == null) continue;
                                    if (soldierData.soldier.uId == saveUIDs[i].uid)
                                    {
                                        soldierData.count = saveUIDs[i].num;
                                        readySoldierList.Add(soldierData);
                                    }
                                }
                            }
                        }
                    }
                    view.Gobj_EnergyGroup.SetActive(false);
                    view.Gobj_LeadershipGroup.SetActive(false);
                    view.Gobj_GeneralNum.SetActive(true);
                    view.Btn_StartBattle.gameObject.SetActive(true);
                    view.Btn_Save.gameObject.SetActive(false);
                    view.Gobj_Countdown.SetActive(false);
                }
                break;
            case EFightType.eftServerHegemony:
                {
                    for (int i = 0; i < PlayerData.Instance._SoldierDepot._soldierList.Count; i++)
                    {
                        Soldier soldier = PlayerData.Instance._SoldierDepot._soldierList[i];
                        if (soldier == null)
                            continue;
                        SoldierData soldierData = new SoldierData(soldier, GlobalCoefficient.PVPSoldierCountLimit);
                        ownSoldierList.Add(soldierData);
                    }
                    if (SupermacyModule.Instance.soldiers != null)
                    {
                        List<SoldierList> saveUIDs = SupermacyModule.Instance.soldiers;
                        if (saveUIDs != null)
                        {
                            for (int i = 0; i < saveUIDs.Count; i++)
                            {
                                for (int j = 0; j < ownSoldierList.Count; j++)
                                {
                                    SoldierData soldierData = ownSoldierList[j];
                                    if (soldierData == null || soldierData.soldier == null) continue;
                                    if (soldierData.soldier.uId == saveUIDs[i].uid)
                                    {
                                        soldierData.count = saveUIDs[i].num;
                                        readySoldierList.Add(soldierData);
                                    }
                                }
                            }
                        }
                    }
                    view.Gobj_EnergyGroup.SetActive(false);
                    view.Gobj_LeadershipGroup.SetActive(false);
                    view.Gobj_GeneralNum.SetActive(true);
                    view.Btn_StartBattle.gameObject.SetActive(true);
                    view.Btn_Save.gameObject.SetActive(false);
                    view.Gobj_Countdown.SetActive(false);
                } break;
            case EFightType.eftQualifying:
                {
                    for (int i = 0; i < PlayerData.Instance._SoldierDepot._soldierList.Count; i++)
                    {
                        Soldier soldier = PlayerData.Instance._SoldierDepot._soldierList[i];
                        if (soldier == null)
                            continue;
                        SoldierData soldierData = new SoldierData(soldier, 0);
                        ownSoldierList.Add(soldierData);
                    }
                    List<SoldierList> prefSoldiers = QualifyingModule.Instance.poleLobbyData.defence_soldiers;
                    if (prefSoldiers != null)
                    {
                        for (int i = 0; i < prefSoldiers.Count; i++)
                        {
                            SoldierList mSoldier = prefSoldiers[i];
                            for (int j = 0; j < ownSoldierList.Count; j++)
                            {
                                SoldierData soldierData = ownSoldierList[j];
                                if (soldierData == null || soldierData.soldier == null) continue;
                                if (soldierData.soldier.uId == mSoldier.uid && mSoldier.num != 0)
                                {
                                    soldierData.count = mSoldier.num;
                                    readySoldierList.Add(soldierData);
                                }
                            }
                        }
                    }
                    view.Gobj_LeadershipGroup.SetActive(false);
                    view.Gobj_GeneralNum.SetActive(false);
                    view.Btn_Save.gameObject.SetActive(mCommonData.IsAdjust);
                    view.Btn_StartBattle.gameObject.SetActive(!mCommonData.IsAdjust);
                    view.Gobj_EnergyGroup.SetActive(true);
                    view.Gobj_Countdown.SetActive(false);
                    //battleReadyTime = GlobalCoefficient.PVPReadyTime;
                    //Scheduler.Instance.AddUpdator(UpdateBattleReadyTime);
                } break;
        }
        Main.Instance.StartCoroutine(CreateOwnSoldier());
        Main.Instance.StartCoroutine(UpdateReadySoldier());
    }

    private IEnumerator CreateOwnSoldier()
    {
        view.ScrView_OwnSoldier.ResetPosition();
        yield return null;
        int MAXCOUNT = 28;
        int count = ownSoldierList.Count;
        int itemCount = ownSoldier_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_OwnSoldier.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_OwnSoldier.minIndex = -index;
        view.UIWrapContent_OwnSoldier.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_OwnSoldier.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_OwnSoldier.enabled = false;
        }
        if (count < itemCount)
        {
            for (int i = count; i < itemCount; i++)
            {
                ownSoldier_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            ExpeditionSoldierComponent comp = null;
            SoldierData data = ownSoldierList[i];
            if (data == null) continue;
            if (i < itemCount)
            {
                comp = ownSoldier_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_OwnSoldierComp, view.Grd_OwnSoldier.transform);
                comp = new ExpeditionSoldierComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_OwnSoldier);
                ownSoldier_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(data.soldier, data.count);
            switch (mCommonData.FightType)
            {
                case EFightType.eftSlave:
                case EFightType.eftHegemony:
                case EFightType.eftUnion:
                case EFightType.eftCaptureTerritory:
                case EFightType.eftServerHegemony:
                case EFightType.eftQualifying:
                case EFightType.eftCrossServerWar:
                    comp.IsDead = false;
                    comp.IsLowLevel = false;
                    break;
            }
            comp.IsShowEnergy = true;
            comp.IsSelect = IsSelectReadySoldier(comp.soldierAtt.uId);
        }
        yield return null;
        view.UIWrapContent_OwnSoldier.ReGetChild();
        yield return null;
        view.Grd_OwnSoldier.Reposition();
        yield return null;
        view.ScrView_OwnSoldier.ResetPosition();
        UpdateSameSoldierStatus();
        //if (mCommonData.FightType == EFightType.eftSlave)
        //{
        //    UpdateSoldierEnergyStatus();
        //}
    }

    private IEnumerator UpdateReadySoldier()
    {
        UpdateEnergy();
        switch (mCommonData.FightType)
        {
            case EFightType.eftQualifying:
            case EFightType.eftSlave:
                {
                    UpdateBattleGeneralsCount();
                } break;
            case EFightType.eftHegemony:
            case EFightType.eftCaptureTerritory:
            case EFightType.eftServerHegemony:
            case EFightType.eftUnion:
            case EFightType.eftCrossServerWar:
                {
                    readySoldierList = SortReadySoldiers(readySoldierList);
                    UpdateBattleGeneralsCount();
                }
                break;
        }
        int itemCount = readySoldier_dic.Count;
        if (readySoldierList.Count < itemCount)
        {
            for (int i = readySoldierList.Count; i < itemCount; i++)
            {
                LineupItemComponent comp = readySoldier_dic[i];
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            LineupItemComponent comp = null;
            if (i < itemCount)
            {
                comp = readySoldier_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_ReadySoldierComp.gameObject, view.Grd_RedaySoldierGrid.transform);
                comp = new LineupItemComponent();
                comp.MyStart(go);
                comp.AddEventListener(ButtonEvent_ReadySoldier);
                readySoldier_dic.Add(comp);
            }
            if (comp == null) continue;
            SoldierData _info = readySoldierList[i];
            if (_info == null) continue;
            comp.UpdateInfo(_info.soldier);
            if (mCommonData.FightType == EFightType.eftSlave || mCommonData.FightType == EFightType.eftQualifying)
            {
                comp.UpdateNum(_info.count, GlobalCoefficient.PVPSoldierCountLimit);
            }
            else
            {
                comp.UpdateNum(_info.count);
            }
            comp.IsSelect = false;
            comp.IsShowEnergy = true;
            comp.IsShowLeader = false;
            comp.IsEnable = true;
            comp.mRootObject.SetActive(true);
        }
        yield return null;
        view.Grd_RedaySoldierGrid.Reposition();
    }

    private List<SoldierData> SortReadySoldiers(List<SoldierData> needSortSoldiers)
    {
        List<SoldierData> list = new List<SoldierData>(needSortSoldiers);
        list.Sort
               (
               (left, right) =>
               {
                   if (left == null || left.soldier == null || right == null || right.soldier == null)
                       return 0;
                   if (left.soldier.Att.call_energy != right.soldier.Att.call_energy)
                   {
                       if (left.soldier.Att.call_energy > right.soldier.Att.call_energy)
                       {
                           return 1;
                       }
                       else
                       {
                           return -1;
                       }
                   }
                   if (left.soldier.Level != right.soldier.Level)
                   {
                       if (left.soldier.Level > right.soldier.Level)
                           return -1;
                       else
                           return 1;
                   }
                   if (left.soldier.Att.Star != right.soldier.Att.Star)
                   {
                       if (left.soldier.Att.Star > right.soldier.Att.Star)
                           return -1;
                       else
                           return 1;
                   }
                   if (left.soldier.Att.quality != right.soldier.Att.quality)
                   {
                       if (left.soldier.Att.quality > right.soldier.Att.quality)
                           return -1;
                       else
                           return 1;
                   }
                   if (left.soldier.Att.id != right.soldier.Att.id)
                   {
                       if (left.soldier.Att.id < right.soldier.Att.id)
                           return -1;
                       else
                           return 1;
                   }
                   return 0;
               }
               );
        return list;
    }

    /// <summary>
    /// 更新战斗准备倒计时  true：打开 
    /// </summary>
    private void UpdateBattleReadyTime()
    {
        view.Gobj_Countdown.SetActive(true);
        battleReadyTime -= Time.deltaTime;
        view.Lbl_ReadyCountDown.text = CommonFunction.GetTimeString(Mathf.CeilToInt(battleReadyTime));
        if (battleReadyTime <= 0)
        {
            Scheduler.Instance.RemoveUpdator(UpdateBattleReadyTime);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_CHALLENGE_TIMEOUT,
                () =>
                {
                    view.Gobj_Countdown.SetActive(false);
                    UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
                });
        }
    }

    /// <summary>
    /// 更新准备出战的士兵数据 默认删除已出战士兵
    /// 返回false 则说明已达出战士兵上线
    /// </summary>
    private bool UpdateReadySoldierData(Soldier data, int num, bool isAdd = false)
    {
        if (isAdd)
        {
            if (mCommonData.FightType == EFightType.eftSlave || mCommonData.FightType == EFightType.eftQualifying)
            {
                SoldierData mSoldierData = readySoldierList.Find((tmp) => { return tmp.soldier.uId == data.uId; });
                if (mSoldierData == null)
                {
                    if (readySoldierList.Count >= GlobalCoefficient.LineupSoldierLimit)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_SOLDIERLIMIT);
                        return false;
                    }
                    else
                    {
                        SoldierData info = new SoldierData();
                        info.soldier = data;
                        info.count++;
                        currentEnergy += info.soldier.Att.call_energy;
                        readySoldierList.Add(info);
                    }
                }
                else
                {
                    if (mSoldierData.count >= GlobalCoefficient.PVPSoldierCountLimit)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.PVPSOLDIERCOUNTLIMIT, GlobalCoefficient.PVPSoldierCountLimit));
                        return false;
                    }
                    else
                    {
                        mSoldierData.count++;
                        currentEnergy += mSoldierData.soldier.Att.call_energy;
                    }
                }
            }
            else
            {
                if (readySoldierList.Count >= GlobalCoefficient.LineupSoldierLimit)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_SOLDIERLIMIT);
                    return false;
                }
                SoldierData _info = new SoldierData();
                _info.soldier = data;
                _info.count = num;
                readySoldierList.Add(_info);
            }
        }
        else
        {
            SoldierData mSoldierData = readySoldierList.Find((tmp) => { return tmp.soldier.uId == data.uId; });
            if (mSoldierData != null)
            {
                mSoldierData.count -= num;
                currentEnergy -= mSoldierData.soldier.Att.call_energy;
                if (mSoldierData.count <= 0)   //如果该武将已经为0  则删除该数据
                {
                    readySoldierList.Remove(mSoldierData);
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public bool IsSelectReadySoldier(ulong soldierUID)
    {
        if (readySoldierList == null)
            readySoldierList = new List<SoldierData>();
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            SoldierData soldierData = readySoldierList[i];
            if (soldierData == null || soldierData.soldier == null) continue;
            if (soldierData.soldier.uId.Equals(soldierUID))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 更新相同类型的士兵状态
    /// </summary>
    public void UpdateSameSoldierStatus()
    {
        for (int i = 0; i < ownSoldier_dic.Count; i++)
        {
            ExpeditionSoldierComponent comp = ownSoldier_dic[i];
            if (comp == null) continue;
            switch (mCommonData.FightType)
            {
                case EFightType.eftQualifying:
                case EFightType.eftSlave:
                    {
                        if (!comp.IsSelect)
                        {
                            bool sameSoldier = CheckSameSoldier(comp.soldierAtt);
                            bool energyOverflow = CheckEnergyLimit(comp.soldierAtt);
                            comp.IsEnable = (!sameSoldier) && (!energyOverflow);
                        }
                    } break;
                default:
                    comp.IsEnable = !CheckSameSoldier(comp.soldierAtt);
                    break;
            }
        }
    }

    /// <summary>
    /// 检测同类型武将 true存在同类型武将
    /// </summary>
    /// <param name="soldierID"></param>
    /// <returns></returns>
    private bool CheckSameSoldier(Soldier ownSoldier)
    {
        if (ownSoldier == null) return false;
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            SoldierData soldierData = readySoldierList[i];
            if (soldierData == null || soldierData.soldier == null) continue;
            if (mCommonData.FightType == EFightType.eftSlave || mCommonData.FightType == EFightType.eftQualifying)
            {
                if (soldierData.soldier.uId == ownSoldier.uId)
                {
                    return false;
                }
            }
            if (ownSoldier.Att.type.Equals(soldierData.soldier.Att.type))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获得当前战斗力
    /// </summary>
    private ShowInfoHero GetShowInfoHero()
    {
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
        ShowInfoHero tmpHeroInfo = Calculation_Attribute.Instance.Calculation_Attribute_Hero(PlayerData.Instance._Level, equips, skills);
        return tmpHeroInfo;
    }
    private int GetCurrentEnergy()
    {
        int energy = 0;
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            SoldierData readySoldier = readySoldierList[i];
            if (readySoldier == null || readySoldier.soldier == null) continue;
            energy += readySoldier.soldier.Att.call_energy * readySoldier.count;
        }
        return energy;
    }

    private void UpdateSoldierEnergyStatus()
    {
        for (int i = 0; i < ownSoldier_dic.Count; i++)
        {
            ExpeditionSoldierComponent comp = ownSoldier_dic[i];
            comp.IsMask = CheckEnergyLimit(comp.soldierAtt);
        }
    }

    private void UpdateEnergy()
    {
        currentEnergy = GetCurrentEnergy();
        view.Lbl_Energy.text = string.Format(ConstString.PVP_BATTLE_ENERGY, currentEnergy.ToString(), GlobalCoefficient.PVPEnergyLimit);
    }

    private bool CheckEnergyLimit(Soldier ownSoldier)
    {
        int energy = currentEnergy + ownSoldier.Att.call_energy;
        if (energy > GlobalCoefficient.PVPEnergyLimit)
        {
            return true;
        }
        return false;
    }

    private bool CheckEnergyLimit()
    {
        int energy = GetCurrentEnergy();
        if (energy > GlobalCoefficient.PVPEnergyLimit)
        {
            return true;
        }
        return false;
    }

    private void UpdateBattleGeneralsCount()
    {
        battleGenerals = 0;
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            SoldierData _info = readySoldierList[i];
            if (_info == null) continue;
            battleGenerals += _info.count;
        }
        view.Lbl_GeneralNum.text = battleGenerals.ToString();
    }
    private void UpdateDominanceForce()
    {
        dominanceForce = 0;
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            SoldierData soldierData = readySoldierList[i];
            if (soldierData == null || soldierData.soldier == null) continue;
            dominanceForce += soldierData.soldier.Att.leaderShip;
        }
        view.Lbl_GeneralNum.text = string.Format(ConstString.GATE_LINEUPANEL_LEADERSHIP, dominanceForce, ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(PlayerData.Instance._Level).Leadership);
    }

    private void DelOwnSoldierData(Soldier soldier)
    {
        if (mCommonData.FightType == EFightType.eftSlave || mCommonData.FightType == EFightType.eftQualifying)
        {
            SoldierData ready = readySoldierList.Find((tmp) => { return tmp.soldier.uId == soldier.uId; });
            if (ready != null && ready.count > 0) return;//如果该武将数量大于0 则不处理
        }
        for (int i = 0; i < ownSoldier_dic.Count; i++)
        {
            ExpeditionSoldierComponent comp = ownSoldier_dic[i];
            if (comp == null || comp.soldierAtt == null) continue;
            if (comp.soldierAtt.uId.Equals(soldier.uId))
            {
                comp.IsSelect = false;
                break;
            }
        }
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
        int count = 1;
        if (mCommonData.FightType != EFightType.eftSlave && mCommonData.FightType != EFightType.eftQualifying)
        {
            count = comp.soldierCount;
        }
        if (UpdateReadySoldierData(comp.soldierAtt, count, false))
        {
            DelOwnSoldierData(comp.soldierAtt);
            Main.Instance.StartCoroutine(UpdateReadySoldier());
            UpdateSameSoldierStatus();
        }
    }
    #endregion

    #region 神器选择

    private void InitReadyEquipArray()
    {
        readyEquipArray = new ReadyEquipData[6];
        if (mCommonData.FightType == EFightType.eftPVP || mCommonData.FightType == EFightType.eftQualifying || mCommonData.FightType == EFightType.eftSlave || mCommonData.FightType == EFightType.eftServerHegemony) //需要发送给服务器的装备数据位置均为1
        {
            readyEquipArray[0] = new ReadyEquipData(null, 1);
            readyEquipArray[1] = new ReadyEquipData(null, 2);
            readyEquipArray[2] = new ReadyEquipData(null, 3);
            readyEquipArray[3] = new ReadyEquipData(null, 5);
            readyEquipArray[4] = new ReadyEquipData(null, 6);
            readyEquipArray[5] = new ReadyEquipData(null, 7);
        }
        else
        {
            readyEquipArray[0] = new ReadyEquipData(null, 0);
            readyEquipArray[1] = new ReadyEquipData(null, 1);
            readyEquipArray[2] = new ReadyEquipData(null, 2);
            readyEquipArray[3] = new ReadyEquipData(null, 4);
            readyEquipArray[4] = new ReadyEquipData(null, 5);
            readyEquipArray[5] = new ReadyEquipData(null, 6);
        }
        List<EquipList> prefEquips = null;
        switch (mCommonData.FightType)
        {
            case EFightType.eftHegemony:
                {
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.HegemonyEquip))
                    {
                        prefEquips = PlayerPrefsTool.ReadObject<List<EquipList>>(AppPrefEnum.HegemonyEquip);
                    }
                } break;
            case EFightType.eftQualifying:
                {
                    prefEquips = QualifyingModule.Instance.poleLobbyData.defence_equips;
                } break;
            case EFightType.eftSlave:
                {
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.SlaveReadyEquip))
                    {
                        prefEquips = PlayerPrefsTool.ReadObject<List<EquipList>>(AppPrefEnum.SlaveReadyEquip);
                    }
                } break;
            case EFightType.eftUnion:
                {
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.ExoticAdvantureEquip))
                    {
                        prefEquips = PlayerPrefsTool.ReadObject<List<EquipList>>(AppPrefEnum.ExoticAdvantureEquip);
                    }
                }
                break;
            case EFightType.eftCaptureTerritory:
                {
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.CaptureTerritoryEquip))
                    {
                        prefEquips = PlayerPrefsTool.ReadObject<List<EquipList>>(AppPrefEnum.CaptureTerritoryEquip);
                    }
                }
                break;
            case EFightType.eftCrossServerWar:
                {
                    if (PlayerPrefsTool.HasKey(AppPrefEnum.CrossServerWarEquip))
                    {
                        prefEquips = PlayerPrefsTool.ReadObject<List<EquipList>>(AppPrefEnum.CrossServerWarEquip);
                    }
                }
                break;
            case EFightType.eftServerHegemony:
                {
                    if (SupermacyModule.Instance.equips != null)
                    {
                        prefEquips = SupermacyModule.Instance.equips;
                    }
                }
                break;
        }
        if (prefEquips != null)
        {
            for (int i = 0; i < prefEquips.Count; i++)
            {
                EquipList mEquip = prefEquips[i];
                if (mEquip == null) continue;
                for (int j = 0; j < readyEquipArray.Length; j++)  //为啥位置不保存为一个变量?
                {
                    ReadyEquipData readyEquip = readyEquipArray[j];
                    if (mEquip.pos.Equals(readyEquip.index))
                    {
                        Weapon mWeapon = PlayerData.Instance._WeaponDepot._weaponList.Find((tmp) =>
                        {
                            if (tmp == null || tmp.IsLock) return false;
                            return tmp.uId == mEquip.uid;
                        });
                        readyEquip.weapon = mWeapon;
                    }
                }
            }
            return;
        }
        List<Weapon> equipedWeapons = new List<Weapon>(PlayerData.Instance._ArtifactedDepot._EquiptList);
        for (int i = 0; i < equipedWeapons.Count; i++)
        {
            Weapon weapon = equipedWeapons[i];
            for (int j = 0; j < readyEquipArray.Length; j++)  //为啥位置不保存为一个变量?
            {
                ReadyEquipData readyEquip = readyEquipArray[j];
                int index = i;
                if (mCommonData.FightType == EFightType.eftPVP || mCommonData.FightType == EFightType.eftQualifying || mCommonData.FightType == EFightType.eftSlave || mCommonData.FightType == EFightType.eftServerHegemony)
                {
                    index = i + 1;
                    if (index.Equals(readyEquip.index))
                    {
                        readyEquip.weapon = weapon;
                    }
                }
                else
                {
                    if (index.Equals(readyEquip.index))
                    {
                        readyEquip.weapon = weapon;
                    }
                }
            }
        }
    }

    private void OpenEquipSelect()
    {
        view.Gobj_EquipSelect.SetActive(true);
        InitReadyEquipArray();
        UpdateEquipPanel();
        Main.Instance.StartCoroutine(CreateReadyEquipComps());
        InitPetInfo();
    }
    private void UpdateEquipPanel()
    {
        UpdateOwnEquipsData();
        Main.Instance.StartCoroutine(CreateOwnEquips());
    }

    private IEnumerator CreateReadyEquipComps()
    {
        if (readyEquip_dic.Count < 6)
        {
            int index = readyEquip_dic.Count;
            for (int i = index; i < 6; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_ReadyEquipComp.gameObject, view.Grd_RedayEquipGrid.transform);
                PVPEquipComponent comp = new PVPEquipComponent();
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
        if (list == null) return;
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
            //break;
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
            PVPEquipComponent comp = ownEquip_dic[i];
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
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            SoldierData soldierData = readySoldierList[i];
            if (soldierData == null || soldierData.soldier == null) continue;
            soldierList.Add(soldierData.soldier, soldierData.count);
        }
        if (currentPetData != null)
        {
            CalBaseData skillData = new CalBaseData(currentPetData.Skill.Att.nId, currentPetData.Skill.Lv);
            skills.Add(skillData);
        }
        int PlayerCombatPower = Calculation_Attribute.Instance.Calculation_PlayerCombatPower(PlayerData.Instance._Level, soldierList, equips, skills);
        return PlayerCombatPower;
    }
    private void CloseEquipPanel()
    {
        view.Gobj_EquipSelect.SetActive(false);
    }
    /// <summary>
    /// 当前拥有的装备
    /// </summary>
    private IEnumerator CreateOwnEquips()
    {
        view.ScrView_OwnEquip.ResetPosition();
        yield return null;
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
            PVPOwnEquipTypeComponent comp = null;
            if (i < itemCount)
            {
                comp = ownEquip_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_OwnEquipComp, view.Grd_OwnEquip.transform);
                comp = new PVPOwnEquipTypeComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_OwnEquip);
                ownEquip_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(ownEquipList[i]);
            comp.callBack = ButtonEvent_ArtifactDetail;
            comp.IsSelect = IsSelectedOwnEquip(ownEquipList[i]);
        }
        yield return null;
        view.UIWrapContent_OwnEquip.ReGetChild();
        yield return null;
        view.Grd_OwnEquip.Reposition();
        yield return null;
        view.ScrView_OwnEquip.ResetPosition();
    }

    /// <summary>
    /// 当前准备携带的装备
    /// </summary>
    public void UpdateReadyEquips()
    {
        for (int index = 0; index < readyEquip_dic.Count; index++)
        {
            PVPEquipComponent comp = readyEquip_dic[index];
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
        UpdateCombatPower();
    }

    /// <summary>
    /// 当前装备是否被选中
    /// </summary>
    /// <param name="equip"></param>
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

    private void OnPetChoose(uint PetID)
    {
        UpdatePetInfo(PetID);
        UpdateCombatPower();
    }

    private void UpdatePetInfo(uint PetID)
    {
        List<PetData> petList = PlayerData.Instance._PetDepot.GetOwnedPets();
        if (petList == null || petList.Count == 0)
        {
            view.Gobj_EquipPet.SetActive(false);
        }
        else
        {
            Debug.Log("---" + PetID);
            if (PetID == 0)
            {
                currentPetData = null;
            }
            else
            {
                currentPetData = petList.Find((tmp) =>
                {
                    if (tmp == null)
                        return false;
                    return tmp.PetInfo.id == PetID;
                });
            }
            if (currentPetData == null)
            {
                view.Lbl_EquipedPetName.text = string.Format(ConstString.PET_PREPAREBATTLE_EQUIPEDPET, ConstString.HINT_NO);
            }
            else
            {
                view.Lbl_EquipedPetName.text = string.Format(ConstString.PET_PREPAREBATTLE_EQUIPEDPET, currentPetData.PetInfo.name);
            }
            view.Gobj_EquipPet.SetActive(true);
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
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        PVPEquipComponent comp = baseComp as PVPEquipComponent;
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
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        PVPOwnEquipTypeComponent comp = baseComp as PVPOwnEquipTypeComponent;
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
    private void ButtonEvent_ArtifactDetail(BaseComponent baseComp, bool state)
    {
        PVPOwnEquipTypeComponent comp = baseComp as PVPOwnEquipTypeComponent;
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

    private void ButtonEvent_OwnSoldier(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ExpeditionSoldierComponent comp = baseComp as ExpeditionSoldierComponent;
        if (comp == null)
        {
            Debug.LogError("can not get LineupItemComponent");
            return;
        }
        switch (mCommonData.FightType)
        {
            case EFightType.eftQualifying:
            case EFightType.eftSlave:
                break;
            case EFightType.eftHegemony:
            case EFightType.eftUnion:
            case EFightType.eftCaptureTerritory:
            case EFightType.eftServerHegemony:
            case EFightType.eftCrossServerWar:
            default:
                if (!comp.IsEnable)
                {
                    return;
                }
                break;
        }
        if (CheckSameSoldier(comp.soldierAtt))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_SOLDIER_SAME);
            return;
        }
        if (mCommonData.FightType == EFightType.eftSlave || mCommonData.FightType == EFightType.eftQualifying)
        {
            if (CheckEnergyLimit(comp.soldierAtt))
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_EXCEED_ENERGYLIMIT);
                return;
            }
        }
        bool status = true;
        int count = 1;
        if (mCommonData.FightType != EFightType.eftSlave && mCommonData.FightType != EFightType.eftQualifying)
        {
            status = !comp.IsSelect;
            count = comp.soldierCount;
        }
        if (UpdateReadySoldierData(comp.soldierAtt, count, status))
        {
            Main.Instance.StartCoroutine(UpdateReadySoldier());
            comp.IsSelect = IsSelectReadySoldier(comp.soldierAtt.uId);
            UpdateSameSoldierStatus();
        }
    }

    private void ButtonEvent_CloseEquipSelect(GameObject go)
    {
        CloseEquipPanel();
    }

    private void ButtonEvent_CloseSoldierSelect(GameObject go)
    {
        switch (mCommonData.FightType)
        {
            case EFightType.eftCaptureTerritory:
            case EFightType.eftServerHegemony:
            case EFightType.eftHegemony:
            case EFightType.eftQualifying:
            case EFightType.eftCrossServerWar:
                CloseView();
                break;
            case EFightType.eftUnion:
                UnionModule.Instance.OnSendCancelReadyUnionPve();
                break;
            case EFightType.eftSlave:
                FightRelatedModule.Instance.SendEnslaveFightBeforeBack();
                break;
        }
    }

    private void ButtonEvent_Next(GameObject go)
    {
        switch (mCommonData.FightType)
        {
            case EFightType.eftServerHegemony:
            case EFightType.eftCaptureTerritory:
            case EFightType.eftHegemony:
            case EFightType.eftUnion:
            case EFightType.eftCrossServerWar:
                {
                    if (readySoldierList.Count <= 0)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_NOSOLDIER);
                        return;
                    }
                    OpenEquipSelect();
                } break;
            case EFightType.eftSlave:
                {
                    if (readySoldierList.Count <= 0)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GATE_SOLDIERTIP_NOSOLDIER, () =>
                        {
                            if (CheckEnergyLimit())
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_EXCEED_ENERGYNOTENGOUGH);
                            }
                            else
                            {
                                //if (readySoldierList.Count < GlobalCoefficient.CastSoldierTypeLimit)
                                //{
                                //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_GENERALTYPE_LIMIT, () =>
                                //    {
                                //        OpenEquipSelect();
                                //    });
                                //}
                                //else
                                //{
                                OpenEquipSelect();
                                //}
                            }
                        });
                    }
                    else
                    {
                        if (CheckEnergyLimit())
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_EXCEED_ENERGYNOTENGOUGH);
                        }
                        else
                        {
                            if (readySoldierList.Count < GlobalCoefficient.CastSoldierTypeLimit)
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_GENERALTYPE_LIMIT, () =>
                                {
                                    OpenEquipSelect();
                                });
                            }
                            else
                            {
                                OpenEquipSelect();
                            }
                        }

                    }
                }
                break;
            case EFightType.eftQualifying:
                {
                    if (readySoldierList.Count <= 0)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GATE_SOLDIERTIP_NOSOLDIER, () =>
                        {
                            if (CheckEnergyLimit())
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_EXCEED_ENERGYNOTENGOUGH);
                            }
                            else
                            {
                                OpenEquipSelect();
                            }
                        });
                    }
                    else
                    {
                        if (CheckEnergyLimit())
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_EXCEED_ENERGYNOTENGOUGH);
                        }
                        else
                        {
                            if (readySoldierList.Count < GlobalCoefficient.CastSoldierTypeLimit)
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_GENERALTYPE_LIMIT, () =>
                                {
                                    OpenEquipSelect();
                                });
                            }
                            else
                            {
                                OpenEquipSelect();
                            }
                        }

                    }
                }
                break;
        }
    }
    private void ButtonEvent_Save(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        List<SoldierList> soldiers = GetReadySoldierList();
        List<EquipList> equips = GetReadyEquipArray();
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
                switch (mCommonData.FightType)
                {
                    case EFightType.eftPVP:
                        {
                            uint PetID = 0;
                            if (currentPetData != null)
                            {
                                PetID = currentPetData.PetInfo.PetID;
                            }
                            PVPModule.Instance.SendSaveDefenceArray(equips, soldiers, GetPlayerCombatPower(), currentPetData.PetInfo.PetID);
                        }
                        break;
                    case EFightType.eftQualifying:
                        {
                            uint PetID = 0;
                            if (currentPetData != null)
                            {
                                PetID = currentPetData.PetInfo.PetID;
                            }
                            QualifyingModule.Instance.SendSavePoleArray(soldiers, equips, GetPlayerCombatPower(), PetID);
                        }
                        break;
                }
            });
        }
        else
        {
            switch (mCommonData.FightType)
            {
                case EFightType.eftPVP:
                    {
                        uint PetID = 0;
                        if (currentPetData != null)
                        {
                            PetID = currentPetData.PetInfo.PetID;
                        }
                        PVPModule.Instance.SendSaveDefenceArray(equips, soldiers, GetPlayerCombatPower(), currentPetData.PetInfo.PetID);
                    }
                    break;
                case EFightType.eftQualifying:
                    {
                        uint PetID = 0;
                        if (currentPetData != null)
                        {
                            PetID = currentPetData.PetInfo.PetID;
                        }
                        QualifyingModule.Instance.SendSavePoleArray(soldiers, equips, GetPlayerCombatPower(), PetID);
                    }
                    break;
            }
        }

    }
    private void ButtonEvent_StartBattle(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        List<SoldierList> soldiers = GetReadySoldierList();
        List<EquipList> equips = GetReadyEquipArray();
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
        uint PetID = 0;
        if (currentPetData != null)
        {
            PetID = currentPetData.PetInfo.PetID;
        }
        if (weaponEnough || ringEnough)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GATE_EQUIPHADFREE, () =>
            {

                switch (mCommonData.FightType)
                {
                    case EFightType.eftHegemony:
                        {
                            PlayerPrefsTool.WriteObject<List<SoldierList>>(AppPrefEnum.HegemonySoldier, soldiers);
                            ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.HegemonySoldier, soldiers);
                            PlayerPrefsTool.WriteObject<List<EquipList>>(AppPrefEnum.HegemonyEquip, equips);
                            PlayerPrefsTool.WriteInt(AppPrefEnum.HegemonyPet, (int)PetID);
                            UnionModule.Instance.OnSendJoinUnionPVP(mCommonData.Other, soldiers, equips);
                        }
                        break;
                    case EFightType.eftCaptureTerritory:
                        {
                            CaptureTerritoryModule.Instance.SendStartCampaignPvp(mCommonData.StageInfo.ID, soldiers, equips);
                        }
                        break;
                    case EFightType.eftCrossServerWar:
                        {
                            CrossServerWarModule.Instance.OnSendGotoBattle(soldiers, equips);
                        }
                        break;
                    case EFightType.eftServerHegemony:
                        {
                            SupermacyModule.Instance.SendStationReady(soldiers, equips, PetID);
                        }
                        break;
                    case EFightType.eftUnion:
                        UnionModule.Instance.OnSendStartUnionPveDgn(mCommonData.StageInfo.ID, soldiers, equips);
                        break;
                    case EFightType.eftSlave:
                        {
                            FightRelatedModule.Instance.SendStartEslaveFightReq(PlayerData.Instance._Prison.SaveFightBeforeReq.type, soldiers, equips);
                        }
                        break;
                    case EFightType.eftQualifying:
                        {
                            QualifyingModule.Instance.SendStartPole(mCommonData.Other, mCommonData.AccountID, mCommonData.UID, soldiers, equips);
                        } break;
                }
            });
        }
        else
        {
            switch (mCommonData.FightType)
            {
                case EFightType.eftCaptureTerritory:
                    {
                        CaptureTerritoryModule.Instance.SendStartCampaignPvp(mCommonData.StageInfo.ID, soldiers, equips);
                    }
                    break;
                case EFightType.eftCrossServerWar:
                    {
                        CrossServerWarModule.Instance.OnSendGotoBattle(soldiers, equips);
                    }
                    break;
                case EFightType.eftHegemony:
                    {
                        PlayerPrefsTool.WriteObject<List<SoldierList>>(AppPrefEnum.HegemonySoldier, soldiers);
                        ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.HegemonySoldier, soldiers);
                        PlayerPrefsTool.WriteObject<List<EquipList>>(AppPrefEnum.HegemonyEquip, equips);
                        PlayerPrefsTool.WriteInt(AppPrefEnum.HegemonyPet, (int)PetID);
                        UnionModule.Instance.OnSendJoinUnionPVP(mCommonData.Other, soldiers, equips);
                    }
                    break;
                case EFightType.eftUnion:
                    UnionModule.Instance.OnSendStartUnionPveDgn(mCommonData.StageInfo.ID, soldiers, equips);
                    break;
                case EFightType.eftSlave:
                    {
                        FightRelatedModule.Instance.SendStartEslaveFightReq(PlayerData.Instance._Prison.SaveFightBeforeReq.type, soldiers, equips);
                    }
                    break;
                case EFightType.eftServerHegemony:
                    {
                        SupermacyModule.Instance.SendStationReady(soldiers, equips, PetID);
                    } break;
                case EFightType.eftQualifying:
                    {
                        QualifyingModule.Instance.SendStartPole(mCommonData.Other, mCommonData.AccountID, mCommonData.UID, soldiers, equips);
                    } break;
            }
        }
    }

    private void ButtonEvent_ChangePet(GameObject go)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);
        uint petID = 0;
        if (currentPetData != null)
        {
            petID = currentPetData.PetInfo.id;
        }
        UISystem.Instance.PetChooseView.UpdateViewInfo(mCommonData.FightType, petID);
    }
    #endregion

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

    /// <summary>
    /// 取得当前准备携带的装备
    /// </summary>
    private List<EquipList> GetReadyEquipArray()
    {
        List<EquipList> equips = new List<EquipList>();
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            ReadyEquipData readyData = readyEquipArray[i];
            if (readyData == null || readyData.weapon == null) continue;
            Weapon weapon = readyData.weapon;
            if (weapon != null)
            {
                EquipList equip = new EquipList();
                equip.uid = weapon.uId;
                equip.pos = readyData.index;
                equips.Add(equip);
            }
        }
        return equips;
    }
    private List<SoldierList> GetReadySoldierList()
    {
        List<SoldierList> list = new List<SoldierList>();
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            SoldierData readySoldier = readySoldierList[i];
            if (readySoldier == null || readySoldier.soldier == null) continue;
            SoldierList soldier = new SoldierList();
            soldier.uid = readySoldier.soldier.uId;
            soldier.num = readySoldier.count;
            list.Add(soldier);
        }
        return list;
    }

    public void OnStartCaptureTerritoryResp(Attribute att)
    {
        mCommonData.PlayerAtt = att;
        EnterTheBattle();
    }

    public void OnStartEnslaveFightResp(Attribute att)
    {
        mCommonData.PlayerAtt = att;
        EnterTheBattle();
    }

    public void OnStartCrossServerWarResp(Attribute att)
    {
        mCommonData.PlayerAtt = att;
        EnterTheBattle();
    }
    public void OnStartSeverHegemony(Attribute att)
    {
        mCommonData.PlayerAtt = att;
        EnterTheBattle();
    }

    public void OnStartUnionPvpDgnResp(Attribute att)
    {
        mCommonData.PlayerAtt = att;
        EnterTheBattle();
    }

    public void OnSavePoleArrayResp(int combat_power)
    {
        List<SoldierList> soldiers = GetReadySoldierList();
        List<EquipList> equips = GetReadyEquipArray();
        PlayerData.Instance._QualifyingInfo.defence_soldiers.Clear();
        PlayerData.Instance._QualifyingInfo.defence_soldiers.AddRange(soldiers);
        PlayerData.Instance._QualifyingInfo.defence_equips.Clear();
        PlayerData.Instance._QualifyingInfo.defence_equips.AddRange(equips);
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_QUALIFYING))
        {
            UISystem.Instance.QualifyingView.UpdateDefenseInfo(soldiers, equips, combat_power);
        }
        CloseView();
    }

    public void OnCancelReadyUnionPve()
    {
        CloseView();
    }
    public void OnEnslaveFightBeforeBack()
    {
        CloseView();
    }

    public void OnReadyQualifyingBattle(StartPoleResp data)
    {
        if (data.fight_type == 1)
        {
            QualifyingModule.Instance.SetIsRevenge(false);
        }
        else
        {
            QualifyingModule.Instance.SetIsRevenge(true);
        }
        mCommonData.PlayerAtt = data.hero_attr;
        EnterTheBattle();
    }

    private void CloseView()
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
    }

    /// <summary>
    /// 进入战斗
    /// </summary>
    public void EnterTheBattle()
    {
        UISystem.Instance.CloseAllUI();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FIGHT);
        UISystem.Instance.HintView.ShowFightLoading(UISystem.Instance.FightView.ShowFightTitleInfo);
        UnionModule.Instance.isFightState = false;
        Scheduler.Instance.AddTimer(0.5f, false, DelayEnterBattle);
    }

    private void DelayEnterBattle()
    {
        List<FightSoldierInfo> mFightSoldierList = new List<FightSoldierInfo>();
        List<SoldierList> mPrefSoldiers = new List<SoldierList>();
        List<ulong> mSaveSoldierUIDList = new List<ulong>();
        Dictionary<ulong, int> mSoldiers_dic = new Dictionary<ulong, int>();
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            SoldierData readySoldier = readySoldierList[i];
            if (readySoldier == null || readySoldier.soldier == null)  //如果找不到该武将  则不添加数据 add by taiwei 
            {
                continue;
            }
            mSaveSoldierUIDList.Add(readySoldier.soldier.uId);
            FightSoldierInfo info = new FightSoldierInfo(readySoldier.soldier, readySoldier.count);
            mFightSoldierList.Add(info);
            SoldierList mSoldier = new SoldierList();
            mSoldier.uid = readySoldier.soldier.uId;
            mSoldier.num = readySoldier.count;
            mPrefSoldiers.Add(mSoldier);
            mSoldiers_dic.Add(mSoldier.uid, mSoldier.num);
        }
        Dictionary<ulong, int> mReadyEquip_dic = new Dictionary<ulong, int>();
        List<EquipList> mPrefReadyEquips = GetReadyEquipArray();
        for (int i = 0; i < mPrefReadyEquips.Count; i++)
        {
            EquipList equipData = mPrefReadyEquips[i];
            mReadyEquip_dic.Add(equipData.uid, equipData.pos);
        }
        ShowInfoHero heroinfo = GetShowInfoHero(mCommonData.PlayerAtt);
        int PetID = 0;
        if (currentPetData != null)
        {
            PetID = (int)currentPetData.PetInfo.PetID;
        }

        switch (mCommonData.FightType)
        {
            case EFightType.eftCaptureTerritory:
                {
                    PlayerPrefsTool.WriteObject<List<SoldierList>>(AppPrefEnum.CaptureTerritorySoldier, mPrefSoldiers);
                    ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.CaptureTerritorySoldier, mPrefSoldiers);
                    PlayerPrefsTool.WriteObject<List<EquipList>>(AppPrefEnum.CaptureTerritoryEquip, mPrefReadyEquips);
                    PlayerPrefsTool.WriteInt(AppPrefEnum.CaptureTerritoryPet, PetID);
                    UISystem.Instance.FightView.SetViewInfo(EFightType.eftCaptureTerritory, heroinfo, mCommonData.StageInfo, mReadyEquip_dic, mFightSoldierList, currentPetData);
                }
                break;
            case EFightType.eftCrossServerWar:
                {
                    PlayerPrefsTool.WriteObject<List<SoldierList>>(AppPrefEnum.CrossServerWarSoldier, mPrefSoldiers);
                    ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.CrossServerWarSoldier, mPrefSoldiers);
                    PlayerPrefsTool.WriteObject<List<EquipList>>(AppPrefEnum.CrossServerWarEquip, mPrefReadyEquips);
                    PlayerPrefsTool.WriteInt(AppPrefEnum.CrossServerWarPet, PetID);
                    UISystem.Instance.FightView.SetViewInfo(EFightType.eftCrossServerWar, heroinfo, mCommonData.StageInfo, mReadyEquip_dic, mFightSoldierList,currentPetData);
                }
                break;
            case EFightType.eftServerHegemony:
                {
                    SupermacyModule.Instance.soldiers = mPrefSoldiers;
                    SupermacyModule.Instance.equips = mPrefReadyEquips;
                    UISystem.Instance.FightView.SetViewInfo(EFightType.eftServerHegemony, heroinfo, mReadyEquip_dic, mSoldiers_dic, mCommonData.ArenaPlayer, currentPetData);
                }
                break;
            case EFightType.eftUnion:
                {
                    PlayerPrefsTool.WriteObject<List<ulong>>(AppPrefEnum.ExoticAdvantureSoldier, mSaveSoldierUIDList);
                    ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.ExpeditionSoldier, mSaveSoldierUIDList);
                    PlayerPrefsTool.WriteObject<List<EquipList>>(AppPrefEnum.ExoticAdvantureEquip, mPrefReadyEquips);
                    PlayerPrefsTool.WriteInt(AppPrefEnum.ExoticAdvanturePet, PetID);
                    UISystem.Instance.FightView.SetViewInfo(EFightType.eftUnion, heroinfo, mCommonData.StageInfo, mFightSoldierList, mReadyEquip_dic, mCommonData.UnionDgnInfo.total_hurt, currentPetData);
                }
                break;
            case EFightType.eftSlave:
                {
                    PlayerPrefsTool.WriteObject<List<SoldierList>>(AppPrefEnum.SlaveReadySoldier, mPrefSoldiers);
                    ReadyBattleSoldierManager.Instance.UpdateReadySoldier(AppPrefEnum.SlaveReadySoldier, mPrefSoldiers);
                    PlayerPrefsTool.WriteObject<List<EquipList>>(AppPrefEnum.SlaveReadyEquip, mPrefReadyEquips);
                    PlayerPrefsTool.WriteInt(AppPrefEnum.SlavePet, PetID);
                    UISystem.Instance.FightView.SetViewInfo(EFightType.eftSlave, heroinfo, mReadyEquip_dic, mSoldiers_dic, PlayerData.Instance._Prison._atenaPlayer, currentPetData);
                }
                break;
            case EFightType.eftQualifying:
                {
                    UISystem.Instance.FightView.SetViewInfo(EFightType.eftQualifying, heroinfo, mReadyEquip_dic, mSoldiers_dic, mCommonData.ArenaPlayer, currentPetData);
                } break;
        }
    }

    private ShowInfoHero GetShowInfoHero(Attribute att)
    {
        ShowInfoHero attribute = new ShowInfoHero();
        if (att == null)
            return attribute;
        attribute.KeyData = PlayerData.Instance._Level;
        attribute.Attack = att.phy_atk;
        attribute.Crit = att.crt_rate;
        attribute.Dodge = att.ddg_rate;
        attribute.Accuracy = att.acc_rate;
        attribute.AttDistance = att.atk_space;
        attribute.AttRate = CommonFunction.GetSecondTimeByMilliSecond(att.atk_interval);
        attribute.Energy = att.energy_max;
        attribute.EnergyRecovery = att.energy_revert;
        attribute.HP = att.hp_max;
        attribute.HPRecovery = att.hp_revert;
        attribute.Leadership = (ushort)att.leader;
        attribute.MoveSpeed = att.speed;
        attribute.MP = att.mp_max;
        attribute.MPRecovery = att.mp_revert;
        attribute.Tenacity = att.tnc_rate;
        attribute.CombatPower = att.combat_power;
        return attribute;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_CloseEquipSelect.gameObject).onClick = ButtonEvent_CloseEquipSelect;
        UIEventListener.Get(view.Btn_CloseLineupSelect.gameObject).onClick = ButtonEvent_CloseSoldierSelect;
        UIEventListener.Get(view.Btn_Next.gameObject).onClick = ButtonEvent_Next;
        UIEventListener.Get(view.Btn_Save.gameObject).onClick = ButtonEvent_Save;
        UIEventListener.Get(view.Btn_StartBattle.gameObject).onClick = ButtonEvent_StartBattle;
        UIEventListener.Get(view.Gobj_EquipPet).onClick = ButtonEvent_ChangePet;
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
        //ownEquipList.Clear();
        //ownSoldierList.Clear();
        //readySoldierList.Clear();
        Scheduler.Instance.RemoveUpdator(UpdateBattleReadyTime);
        FightRelatedModule.Instance.isFightState = false;
        UnionModule.Instance.isFightState = false;
        PetSystemModule.Instance.PetChooseEvent -= OnPetChoose;
    }

    public override void Destroy()
    {
        base.Destroy();
        Scheduler.Instance.RemoveTimer(DelayEnterBattle);
        view = null;
        readyEquip_dic.Clear();
        readyLock_dic.Clear();
        readySoldier_dic.Clear();
        ownEquip_dic.Clear();
        ownSoldier_dic.Clear();
    }
}