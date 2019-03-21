using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;
using Assets.Script.Common;

/// <summary>
/// PVP敌人信息组件
/// </summary>
public class PVPEnemyComponent : ItemBaseComponent
{
    /// <summary>
    /// 玩家名称
    /// </summary>
    private UILabel Lbl_NameLabel;
    /// <summary>
    /// 玩家等级
    /// </summary>
    private UILabel Lbl_LevelLabel;
    /// <summary>
    /// 排名
    /// </summary>
    private UILabel Lbl_RankLabel;
    /// <summary>
    /// 战斗力
    /// </summary>
    private UILabel Lbl_CombatPowerLabel;
    private GameObject Gobj_ItemBaseComp;
    /// <summary>
    /// 挑战
    /// </summary>
    private UIButton Btn_ChallengeButton;
    public GameObject Gobj_EffectItemPoint;

    private ArenaPlayer enemyInfo = null;
    public ArenaPlayer EnemyInfo
    {
        get
        {
            return enemyInfo;
        }
    }

    public delegate void ChallengeDelegate(GameObject go, ArenaPlayer enemy);
    public ChallengeDelegate challengeHandle;
    public delegate void PlayerInfoDelegate(GameObject go, ArenaPlayer enemy);
    public PlayerInfoDelegate playerInfoHandle;
    public PVPEnemyComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Gobj_ItemBaseComp = mRootObject.transform.FindChild("ItemBaseComp").gameObject;
        Lbl_LevelLabel = mRootObject.transform.FindChild("LevelBG/LevelLabel").gameObject.GetComponent<UILabel>();
        Lbl_NameLabel = mRootObject.transform.FindChild("NameGroup/NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_RankLabel = mRootObject.transform.FindChild("InfoGroup/RankGroup/RankLabel").gameObject.GetComponent<UILabel>();
        Lbl_CombatPowerLabel = mRootObject.transform.FindChild("InfoGroup/CombatPowerGroup/CombatPowerLabel").gameObject.GetComponent<UILabel>();
        Btn_ChallengeButton = mRootObject.transform.FindChild("ChallengeButton").gameObject.GetComponent<UIButton>();
        Gobj_EffectItemPoint = mRootObject.transform.FindChild("EffectItemPoint").gameObject;
        UIEventListener.Get(Btn_ChallengeButton.gameObject).onClick = ButtonEvent_Challenge;
        UIEventListener.Get(Gobj_ItemBaseComp).onClick = ButtonEvent_PlayerInfo;
    }

    /// <summary>
    /// 更新信息
    /// </summary>
    /// <param name="enemy"></param>
    public void UpdateInfo(ArenaPlayer enemy)
    {
        if (enemy == null)
        {
            Clear();
            return;
        }
        enemyInfo = enemy;
        CommonFunction.SetHeadAndFrameSprite(Spt_IconTexture, Spt_QualitySprite, enemyInfo.hero.icon, enemyInfo.hero.icon_frame, true);
        //base.UpdateInfo(CommonFunction.GetHeroIconNameByGender((EHeroGender)enemyInfo.hero.gender), (int)ItemQualityEnum.White);
        Lbl_LevelLabel.text = enemyInfo.hero.level.ToString();
        Lbl_NameLabel.text = enemyInfo.hero.charname;
        Lbl_RankLabel.text = enemyInfo.rank.ToString();
        Lbl_CombatPowerLabel.text = enemyInfo.combat_power.ToString();

        if (PlayerData.Instance._AccountID.Equals(enemy.hero.accid))
        {
            Btn_ChallengeButton.gameObject.SetActive(false);
        }
        else
        {
            Btn_ChallengeButton.gameObject.SetActive(true);
        }
    }
    private void ButtonEvent_PlayerInfo(GameObject go)
    {
        if (playerInfoHandle != null)
        {
            playerInfoHandle(go, enemyInfo);
        }
    }
    private void ButtonEvent_Challenge(GameObject go)
    {
        if (challengeHandle != null)
        {
            challengeHandle(go, enemyInfo);
        }
    }

    public override void Clear()
    {
        base.Clear();
        enemyInfo = null;
    }
}

/// <summary>
/// PVP 神器组件
/// </summary>
public class PVPEquipComponent : ItemBaseComponent
{
    /// <summary>
    /// 名称
    /// </summary>
    private UILabel Lbl_NameLabel;
    /// <summary>
    /// 武器携带技能消耗魔法值
    /// </summary>
    private UILabel Lbl_EnergyLabel;
    private GameObject Gobj_LevelGroup;
    private UILabel Lbl_LevelLabel;

    /// <summary>
    /// 选中
    /// </summary>
    private UISprite Spt_SelectSprite;

    private GameObject Gobj_EnergyGroup;

    public bool IsShowEnergy
    {
        set
        {
            Gobj_EnergyGroup.SetActive(value);
        }
    }
    private bool isSelect = false;
    public bool IsSelect
    {
        get
        {
            return isSelect;
        }
        set
        {
            isSelect = value;
            Spt_SelectSprite.enabled = isSelect;
        }
    }

    public bool IsShowLevel
    {
        set
        {
            Gobj_LevelGroup.SetActive(value);
        }
    }

    private Weapon equipInfo;
    public Weapon EquipInfo
    {
        get
        {
            return equipInfo;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_NameLabel = mRootObject.transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Lbl_EnergyLabel = mRootObject.transform.FindChild("EnergyGroup/EnergyLabel").gameObject.GetComponent<UILabel>();
        Lbl_LevelLabel = mRootObject.transform.FindChild("LevelGroup/LevelLabel").gameObject.GetComponent<UILabel>();
        Gobj_EnergyGroup = mRootObject.transform.FindChild("EnergyGroup").gameObject;
        Gobj_LevelGroup = mRootObject.transform.FindChild("LevelGroup").gameObject;
        IsSelect = false;
    }

    public void UpdateInfo(Weapon equip)
    {
        if (equip == null)
        {
            Clear();
            return;
        }
        equipInfo = equip;
        Lbl_NameLabel.text = equipInfo.Att.name;
        if (equipInfo._Skill == null)
        {
            Lbl_EnergyLabel.text = (0).ToString();
        }
        else
        {
            Lbl_EnergyLabel.text = equipInfo._Skill.Att.expendMagic.ToString();
        }
        if (equipInfo.IsLock)
        {
            IsShowLevel = false;
        }
        else
        {
            IsShowLevel = true;
            Lbl_LevelLabel.text = equipInfo.Level.ToString();
        }
        base.UpdateInfo(equipInfo.Att.icon, equipInfo.Att.quality);
        IsShowEnergy = !equipInfo.IsLock;
    }

    public override void Clear()
    {
        base.Clear();
    }
}

/// <summary>
/// PVP 士兵组件
/// </summary>
public class PVPSoldierComponent : ItemBaseComponent
{
    public UIGrid Grd_StarsGrid;     //星级父物体
    public UISprite Spt_StarSprite;  //星级模板
    public UISprite Spt_SelectSprite;
    public Transform Gobj_EnergyGroup;
    public UILabel Lbl_EnergyLabel;    //召唤能量
    public UILabel Lbl_NumLabel;
    public UILabel Lbl_LeadershipLabel;
    public UISprite Spt_MaskSprite;
    public UILabel Lbl_LevelLabel;
    private List<GameObject> starlist;
    public UILabel lbl_Label_Step;
    private Soldier _soldierAtt;
    public Soldier soldierAtt
    {
        get
        {
            return _soldierAtt;
        }
    }

    /// <summary>
    /// 是否显示召唤能量
    /// </summary>
    public bool IsShowEnergy
    {
        set
        {
            Gobj_EnergyGroup.gameObject.SetActive(value);
        }
    }

    public PVPSoldierComponent(GameObject root)
    {
        base.MyStart(root);
        Grd_StarsGrid = mRootObject.transform.FindChild("StarsGrid").gameObject.GetComponent<UIGrid>();
        Spt_StarSprite = mRootObject.transform.FindChild("StarsGrid/StarSprite").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabel = mRootObject.transform.FindChild("LevelBG/LevelLabel").gameObject.GetComponent<UILabel>();
        Gobj_EnergyGroup = mRootObject.transform.FindChild("EnergyGroup").gameObject.transform;
        Lbl_EnergyLabel = mRootObject.transform.FindChild("EnergyGroup/EnergyLabel").gameObject.GetComponent<UILabel>();
        Lbl_NumLabel = mRootObject.transform.FindChild("NumLabel").gameObject.GetComponent<UILabel>();
        lbl_Label_Step = mRootObject.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        IsShowEnergy = false;
        Spt_StarSprite.gameObject.SetActive(false);
        starlist = new List<GameObject>();
        Clear();
    }

    public void UpdateInfo(Soldier att, int num = 0)
    {
        _soldierAtt = att;
        if (_soldierAtt == null)
        {
            Debug.LogError("solider data is null!!!");
            Clear();
            return;
        }
        SoldierAttributeInfo info = soldierAtt.Att;
        if (info == null)
            return;
        base.UpdateInfo(info.Icon, info.quality);
        UpdateStars(_soldierAtt.Att.Star);
        Lbl_LevelLabel.text = _soldierAtt.Level.ToString();
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(lbl_Label_Step, att.StepNum);
        UpdateNum(num);
    }

    public void UpdateNum(int num)
    {
        if (num == 0 || num == 1)
        {
            Lbl_NumLabel.text = string.Empty;
        }
        else
        {
            Lbl_NumLabel.text = "x" + num.ToString();
        }
    }

    private void UpdateStars(int level)
    {
        if (level < starlist.Count)
        {
            for (int i = 0; i < starlist.Count; i++)
            {
                GameObject go = starlist[i];
                if (i < level)
                {
                    go.SetActive(true);
                }
                else
                {
                    go.SetActive(false);
                }
            }
        }
        else
        {
            int index = starlist.Count;
            for (int i = 0; i < starlist.Count; i++)
            {
                GameObject go = starlist[i];
                go.SetActive(true);
            }
            for (int i = index; i < level; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(Spt_StarSprite.gameObject, Grd_StarsGrid.transform);
                go.name = "star_" + i.ToString();
                go.SetActive(true);
                starlist.Add(go);
            }
        }
        Grd_StarsGrid.repositionNow = true;
    }

    public override void Clear()
    {
        base.Clear();
        IsShowEnergy = false;
        Lbl_EnergyLabel.text = string.Empty;
        //IsSelect = false;
        UpdateStars(0);
    }
}

/// <summary>
/// 战斗日志
/// </summary>
public class PVPBattleLogComponent : BaseComponent
{
    /// <summary>
    /// 战斗结果
    /// </summary>
    private UISprite Spt_BattleResult;
    /// <summary>
    /// 排名变化
    /// </summary>
    private UILabel Lbl_RankChangeLabel;
    /// <summary>
    /// 玩家ICON
    /// </summary>
    private UISprite Spt_IconTexture;
    private UISprite Spt_ItemBgSprite;
    private UISprite Spt_QualitySprite;
    private GameObject Gobj_ItemBaseComp;
    /// <summary>
    /// 攻守标识
    /// </summary>
    private UISprite Spt_CampSprite;
    /// <summary>
    /// 玩家名称
    /// </summary>
    private UILabel Lbl_NameLabel;
    /// <summary>
    /// 玩家等级
    /// </summary>
    private UILabel Lbl_LevelLabel;
    /// <summary>
    /// 复仇按钮背景
    /// </summary>
    private UISprite Spt_RevengeButtonBG;
    /// <summary>
    /// 复仇按钮前置图
    /// </summary>
    private UILabel Lbl_RevengeButtonDesc;
    /// <summary>
    /// 战斗时间戳
    /// </summary>
    private UILabel Lbl_BattleTimeLabel;
    /// <summary>
    /// 复仇按钮
    /// </summary>
    private UIButton Btn_RevengeButton;
    private ArenaRecord recordData;

    public delegate void RevengeDelegate(GameObject go, ArenaRecord enemy);
    public RevengeDelegate revengedHandle;
    public delegate void PlayerInfoDelegate(GameObject go, ArenaPlayer info);
    public PlayerInfoDelegate playerInfoHandle;

    public PVPBattleLogComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_BattleResult = mRootObject.transform.FindChild("BattleResultGroup/BattleResult").gameObject.GetComponent<UISprite>();
        Lbl_RankChangeLabel = mRootObject.transform.FindChild("BattleResultGroup/RankChangeLabel").gameObject.GetComponent<UILabel>();
        Gobj_ItemBaseComp = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp").gameObject;
        Spt_QualitySprite = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Spt_CampSprite = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/CampSprite").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabel = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/LvGroup/LevelLabel").gameObject.GetComponent<UILabel>();
        Lbl_NameLabel = mRootObject.transform.FindChild("EnemyInfoGroup/InfoGroup/NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_BattleTimeLabel = mRootObject.transform.FindChild("EnemyInfoGroup/InfoGroup/BattleTimeLabel").gameObject.GetComponent<UILabel>();
        Btn_RevengeButton = mRootObject.transform.FindChild("RevengeButton").gameObject.GetComponent<UIButton>();
        Spt_RevengeButtonBG = mRootObject.transform.FindChild("RevengeButton/RevengeButtonBG").gameObject.GetComponent<UISprite>();
        Lbl_RevengeButtonDesc = mRootObject.transform.FindChild("RevengeButton/RevengeButtonDesc").gameObject.GetComponent<UILabel>();
        UIEventListener.Get(Btn_RevengeButton.gameObject).onClick = ButtonEvent_Revenge;
        UIEventListener.Get(Gobj_ItemBaseComp.gameObject).onClick = ButtonEvent_PlayerInfo;
    }

    public void UpdateInfo(ArenaRecord record)
    {
        if (record == null)
        {
            Clear();
            return;
        }
        recordData = record;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (recordData == null) return;
        UpdateBattleResult();
        Lbl_NameLabel.text = recordData.opponent.hero.charname;
        UpdateRevengeButton();
        UpdateEnemyInfo();
        UpdateCampSprite();
        UpdateBattleTime();
    }

    private void UpdateEnemyInfo()
    {
        CommonFunction.SetSpriteName(Spt_IconTexture, CommonFunction.GetHeroIconNameByGender((EHeroGender)recordData.opponent.hero.gender));
        Lbl_LevelLabel.text = recordData.opponent.hero.level.ToString();
    }

    /// <summary>
    /// 更新战斗结果
    /// </summary>
    /// <param name="result"></param>
    private void UpdateBattleResult()
    {
		switch (recordData.type) //约定敌方信息1为攻击方  2为防御方（此处胜负是自己的显示 因而应该取反）
		{
		case 2:  //自己是攻击方
		{
			if (recordData.result == 1)  //约定1为胜利  2为失败
			{
				CommonFunction.SetSpriteName(Spt_BattleResult, GlobalConst.SpriteName.PVP_BattleLogVictory);
				Lbl_RankChangeLabel.text = string.Format(ConstString.PVP_PLAYERRANKUP, Mathf.Abs(recordData.rank_change));
			}
			else
			{
				CommonFunction.SetSpriteName(Spt_BattleResult, GlobalConst.SpriteName.PVP_BattleLogDefaultDeuce);
				Lbl_RankChangeLabel.text = ConstString.PVP_PLAYERRANKKEEP;
			}
		} break;
		case 1:  //自己是防守方
		{
			if (recordData.result == 1)  //约定1为胜利  2为失败
			{
				CommonFunction.SetSpriteName(Spt_BattleResult, GlobalConst.SpriteName.PVP_BattleLogVictoryDeuce);
				Lbl_RankChangeLabel.text = ConstString.PVP_PLAYERRANKKEEP;
			}
			else
			{
				CommonFunction.SetSpriteName(Spt_BattleResult, GlobalConst.SpriteName.PVP_BattleLogDefault);
				Lbl_RankChangeLabel.text = string.Format(ConstString.PVP_PLAYERRANKDOWN, Mathf.Abs(recordData.rank_change));
			}
		} break;
		default:
			Spt_BattleResult.spriteName = string.Empty;
			Lbl_RankChangeLabel.text = string.Empty;
			break;
		}
    }

    private void UpdateCampSprite()
    {
        if (recordData.type == 1)
        {
            CommonFunction.SetSpriteName(Spt_CampSprite, GlobalConst.SpriteName.PVP_BattleLogCampAttack);
        }
        else if (recordData.type == 2)
        {
            CommonFunction.SetSpriteName(Spt_CampSprite, GlobalConst.SpriteName.PVP_BattleLogCampDefense);
        }
    }

    private void UpdateBattleTime()
    {
        long battleTime = (long)Mathf.Abs(Main.mTime - recordData.time);
        if (battleTime <= 60)
            battleTime = 60;
        Lbl_BattleTimeLabel.text = string.Format(ConstString.PVP_BATTLETIME, CommonFunction.GetIntegerTimeString(battleTime));
    }

    /// <summary>
    /// 复仇按钮是否可用
    /// </summary>
    /// <param name="status"></param>
    public void UpdateRevengeButton()
    {
        if (recordData.can_revenge == 1)
        {
            Btn_RevengeButton.gameObject.SetActive(true);
            if (PVPModule.Instance.CDTime > 0)
            {
                Btn_RevengeButton.collider.enabled = false;
                Spt_RevengeButtonBG.color = Color.gray;
            }
            else
            {
                Btn_RevengeButton.collider.enabled = true;
                Spt_RevengeButtonBG.color = Color.white;
                CommonFunction.UpdateWidgetGray(Spt_RevengeButtonBG, false);
            }
        }
        else
        {
            Btn_RevengeButton.gameObject.SetActive(false);
        }
    }

    private void ButtonEvent_PlayerInfo(GameObject go)
    {
        if (playerInfoHandle != null)
        {
            playerInfoHandle(go, recordData.opponent);
        }
    }

    private void ButtonEvent_Revenge(GameObject go)
    {
        if (revengedHandle != null)
        {
            revengedHandle(go, recordData);
        }
    }

    public override void Clear()
    {
        base.Clear();
    }

}

public class PVPRankGetAwardsComponent : BaseComponent
{
    private UILabel Lbl_PVPRank;
    private UIGrid Grd_Awards;
    private GameObject Gobj_AwardComp;
    private List<PVPRankAwardsComponent> awardsList;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_PVPRank = mRootObject.transform.FindChild("PVPRank").gameObject.GetComponent<UILabel>();
        Grd_Awards = mRootObject.transform.FindChild("Awards").gameObject.GetComponent<UIGrid>();
        Gobj_AwardComp = mRootObject.transform.FindChild("Awards/gobj_AwardComp").gameObject;
        if (awardsList == null) awardsList = new List<PVPRankAwardsComponent>();
    }

    public void UpdateInfo(ArenaAwardInfo info)
    {
        if (info == null) return;
        if (info.min_rank == info.max_rank)
        {
            Lbl_PVPRank.text = string.Format(ConstString.PVP_RULEDESC_RANK, info.min_rank);
        }
        else if (info.max_rank == -1)
        {
            Lbl_PVPRank.text = string.Format(ConstString.FORMAT_RANK_BACK, info.min_rank);
        }
        else
        {
            string rank = info.min_rank.ToString() + "-" + info.max_rank.ToString();
            Lbl_PVPRank.text = string.Format(ConstString.PVP_RULEDESC_RANK, rank);
        }
        Main.Instance.StartCoroutine(UpdateRankComps(info));
    }
    private System.Collections.IEnumerator UpdateRankComps(ArenaAwardInfo info)
    {
        if (Gobj_AwardComp.activeSelf)
            Gobj_AwardComp.SetActive(false);
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList((uint)info.drop_id);
        if (list.Count <= awardsList.Count)
        {
            for (int i = 0; i < awardsList.Count; i++)
            {
                PVPRankAwardsComponent comp = awardsList[i];
                if (i < list.Count)
                {
                    CommonItemData data = list[i];
                    comp.mRootObject.SetActive(true);
                    bool isSpecialID = false;
                    IDType idType = CommonFunction.GetTypeOfID(data.ID.ToString());
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
                        comp.UpdateInfo(CommonFunction.GetIconNameByID(data.ID), (int)ItemQualityEnum.White, data.Num);
                    }
                    else
                    {
                        comp.UpdateInfo(data.Icon, (int)data.Quality, data.Num);
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
            for (int i = 0; i < list.Count; i++)
            {
                CommonItemData data = list[i];
                PVPRankAwardsComponent comp = null;
                int index = awardsList.Count;
                if (i < index)
                {
                    comp = awardsList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(Gobj_AwardComp, Grd_Awards.transform);
                    comp = new PVPRankAwardsComponent();
                    comp.MyStart(go);
                    awardsList.Add(comp);
                }
                if (comp == null) continue;
                bool isSpecialID = false;
                IDType idType = CommonFunction.GetTypeOfID(data.ID.ToString());
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
                    comp.UpdateInfo(CommonFunction.GetIconNameByID(data.ID), (int)ItemQualityEnum.White, data.Num);
                }
                else
                {
                    comp.UpdateInfo(data.Icon, (int)data.Quality, data.Num);
                }
                // comp.UpdateInfo(data.Icon, (int)data.Quality, data.Num);
                comp.mRootObject.SetActive(true);
            }
        }
        yield return null;
        Grd_Awards.Reposition();
    }

    public override void Clear()
    {
        base.Clear();
        awardsList.Clear();
    }

}

public class PVPRankAwardsComponent : ItemBaseComponent
{
    private UILabel Lbl_CountLabel;

    public override void MyStart(GameObject go)
    {
        base.MyStart(go);
        Spt_QualitySprite = mRootObject.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBgSprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("IconSprite").gameObject.GetComponent<UISprite>();
        Lbl_CountLabel = mRootObject.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();
    }

    public void UpdateInfo(string icon, int quality, int count)
    {
        base.UpdateInfo(icon, quality);
        if (count <= 1)
        {
            Lbl_CountLabel.text = string.Empty;
        }
        else
        {
            if (count / 10000 > 0)
            {
                Lbl_CountLabel.text = "x" + string.Format(ConstString.TASK_TENTHOUSAND, (count / 10000).ToString());
            }
            else
            {
                Lbl_CountLabel.text = "x" + count.ToString();
            }
        }
    }
}

public class PVPRankAwardDescGroupComponent : BaseComponent
{
    private List<PVPRankGetAwardsComponent> awardsList;

    private GameObject Gobj_AwardGroup;
    private UITable Table_GetAwards;
    private UIGrid Grd_GetAwards;
    private UISprite Spt_RuleDescSprite;
    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Gobj_AwardGroup = mRootObject.transform.FindChild("GetAwards/gobj_AwardGroup").gameObject;
        Table_GetAwards = mRootObject.transform.FindChild("GetAwards").gameObject.GetComponent<UITable>();
        Grd_GetAwards = mRootObject.transform.FindChild("GetAwards").gameObject.GetComponent<UIGrid>();
        Spt_RuleDescSprite = mRootObject.transform.FindChild("AwardDescSprite").gameObject.GetComponent<UISprite>();
    }

    public void UpdateInfo(List<ArenaAwardInfo> list)
    {
        Spt_RuleDescSprite.height = 200 + Mathf.Max(list.Count-1, 0) * 100;
        Main.Instance.StartCoroutine(UpdateAwards(list));
    }

    public void UpdateInfo()
    {
        List<ArenaAwardInfo> list = new List<ArenaAwardInfo>(ConfigManager.Instance.mArenaAwardConfig.GetArenaAwardList());
        list.Sort((ArenaAwardInfo left, ArenaAwardInfo right) =>
        {
            if (left == null || right == null)
            {
                return 0;
            }
            else if (left.id < right.id)
            {
                return -1;
            }
            else if (left.id == right.id)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        });
        List<ArenaAwardInfo> awards = new List<ArenaAwardInfo>();
        int count = 10;
        if (list.Count <= count)
        {
            count = list.Count;
        }
        for (int i = 0; i < count; i++)
        {
            awards.Add(list[i]);
        }
        Main.Instance.StartCoroutine(UpdateAwards(awards));
        Spt_RuleDescSprite.height = 1100;  //未经过计算
    }

    private System.Collections.IEnumerator UpdateAwards(List<ArenaAwardInfo> list)
    {
        if (Gobj_AwardGroup.activeSelf)
            Gobj_AwardGroup.SetActive(false);
        if (awardsList == null)
            awardsList = new List<PVPRankGetAwardsComponent>();

        if (list.Count <= awardsList.Count)
        {
            for (int i = 0; i < awardsList.Count; i++)
            {
                PVPRankGetAwardsComponent comp = awardsList[i];
                if (i < list.Count)
                {
                    ArenaAwardInfo data = list[i];
                    comp.mRootObject.SetActive(true);
                    comp.UpdateInfo(data);
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                ArenaAwardInfo data = list[i];
                PVPRankGetAwardsComponent comp = null;
                int index = awardsList.Count;
                if (i < index)
                {
                    comp = awardsList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(Gobj_AwardGroup,
                        Table_GetAwards == null ? Grd_GetAwards.transform : Table_GetAwards.transform);
                    comp = new PVPRankGetAwardsComponent();
                    comp.MyStart(go);
                    awardsList.Add(comp);
                }
                comp.UpdateInfo(data);
                comp.mRootObject.SetActive(true);
            }
        }
        yield return null;
        if (Table_GetAwards != null)
            Table_GetAwards.Reposition();
        else
        {
            Grd_GetAwards.Reposition();
        }
    }

    public override void Clear()
    {
        base.Clear();
        for (int i = 0; i < awardsList.Count; i++)
        {
            awardsList[i].Clear();
        }
        awardsList.Clear();
    }
}

public class PVPGetAwradsGroupComponet : ItemBaseComponent
{
    private UIGrid Grd_Award;
    private GameObject Gobj_AwardComp;
    private List<PVPRankAwardsComponent> awardsList;
    public GameObject Gobj_Award;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Grd_Award = mRootObject.transform.FindChild("GetAwardsGroup").GetComponent<UIGrid>();
        Gobj_AwardComp = mRootObject.transform.FindChild("GetAwardsGroup/gobj_AwardComp").gameObject;
        Gobj_AwardComp.SetActive(false);
        Gobj_Award = mRootObject.transform.FindChild("GetAwardsGroup").gameObject;
    }

    public void UpdateInfo(ArenaAwardInfo info)
    {
        if (awardsList == null)
        {
            awardsList = new List<PVPRankAwardsComponent>();
        }
        Main.Instance.StartCoroutine(UpdateRankComps(info));
    }

    private System.Collections.IEnumerator UpdateRankComps(ArenaAwardInfo info)
    {
        if (Gobj_AwardComp.activeSelf)
            Gobj_AwardComp.SetActive(false);
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList((uint)info.drop_id);
        if (list.Count <= awardsList.Count)
        {
            for (int i = 0; i < awardsList.Count; i++)
            {
                PVPRankAwardsComponent comp = awardsList[i];
                if (i < list.Count)
                {
                    CommonItemData data = list[i];
                    comp.mRootObject.SetActive(true);
                    bool isSpecialID = false;
                    IDType idType = CommonFunction.GetTypeOfID(data.ID.ToString());
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
                        comp.UpdateInfo(CommonFunction.GetIconNameByID(data.ID), (int)ItemQualityEnum.White, data.Num);
                    }
                    else
                    {
                        comp.UpdateInfo(data.Icon, (int)data.Quality, data.Num);
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
            for (int i = 0; i < list.Count; i++)
            {
                CommonItemData data = list[i];
                PVPRankAwardsComponent comp = null;
                int index = awardsList.Count;
                if (i < index)
                {
                    comp = awardsList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(Gobj_AwardComp, Grd_Award.transform);
                    comp = new PVPRankAwardsComponent();
                    comp.MyStart(go);
                    awardsList.Add(comp);
                }
                if (comp == null) continue;
                bool isSpecialID = false;
                IDType idType = CommonFunction.GetTypeOfID(data.ID.ToString());
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
                    comp.UpdateInfo(CommonFunction.GetIconNameByID(data.ID), (int)ItemQualityEnum.White, data.Num);
                }
                else
                {
                    comp.UpdateInfo(data.Icon, (int)data.Quality, data.Num);
                }
                comp.mRootObject.SetActive(true);
            }
        }
        yield return null;
        Grd_Award.Reposition();
    }

    public override void Clear()
    {
        base.Clear();
    }
}