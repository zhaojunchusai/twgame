using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using TdSpine;

/// <summary>
/// 己方基地
/// </summary>
public class Barracks_Self : RoleAttribute
{
    private UITexture textureTop;
    private UITexture textureLow;
    private List<Transform> shooterPosList = new List<Transform>();
    private BoxCollider boxCollider;
    private int battleID;
    //private RoleHPBase hpBuild;
    private string baseName;

    protected override void InitRoleCoefficient()
    {
        _Coeff_Attack_Hurt = 0;
        _Coeff_Attack_Restraint = 0;
        _Coeff_Attack_Crit = 0;
        _Coeff_Attack_AccuracyRate_1 = 0;
        _Coeff_Attack_AccuracyRate_2 = 0;
        _Coeff_Attack_CritRate_1 = 0;
        _Coeff_Attack_CritRate_2 = 0;
        _Coeff_Skill_Hurt = 0;
        _Coeff_Skill_Restraint = 0;
        _Coeff_Skill_Passive = 0;
    }

    public override void InitRoleAttribute(ShowInfoBase vFightAttribute)
    {
        base.InitRoleAttribute(vFightAttribute);
        if (EffectImmuneManage != null)
            EffectImmuneManage.AddFilter(SkillFilter_Barracks);
        hitAudio = GlobalConst.Sound.AUDIO_Hit_Building;
        SetRoleCamp(EFightCamp.efcSelf);
        SetRoleType(ERoleType.ertBarracks);
        //hpBuild = new BarracksHp();
        //hpBuild.Instance(this, ERoleType.ertBarracks, new Vector3(-90, 200, 0));
        //hpBuild.SetCurAndMaxValue(fightAttribute.HP, this.Get_MaxHPValue);
        pRpleHP = new BarracksHp();
        pRpleHP.Instance(this, ERoleType.ertBarracks, new Vector3(-90, 200, 0));


        if (SceneManager.Instance.Get_CurSceneType == EFightType.eftExpedition)
        {
            CastleAttributeInfo tmpInfo = ConfigManager.Instance.mCastleConfig.FindByID(FightRelatedModule.Instance.mCastleInfo.mID);
            if (tmpInfo != null)
            {
                this.Get_MaxHPValue = tmpInfo.hp_max + tmpInfo.hp_grow * (FightRelatedModule.Instance.mCastleInfo.mLevel - tmpInfo.level);
            }
        }

        pRpleHP.SetCurAndMaxValue(fightAttribute.HP, this.Get_MaxHPValue);

        roleLevel = 0;
    }

    protected override void FinalOperate()
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_Dead_Building, this.transform));
        
        //if (textureLow != null)
        //    SetSingleTexture(textureLow, string.Format("image_castle_0_{0}_R.assetbundle", battleID));
        //if (textureTop != null)
        //    SetSingleTexture(textureTop, string.Format("image_castle_0_{0}_L.assetbundle", battleID));
        if (textureLow != null)
            SetSingleTexture(textureLow, string.Format("{0}_0_R.assetbundle", baseName));
        if (textureTop != null)
            SetSingleTexture(textureTop, string.Format("{0}_0_L.assetbundle", baseName));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDieBarracksSelf));
    }

    public void ReSet()
    {
        if (textureTop != null)
        {
            textureTop.mainTexture = null;
            textureTop.gameObject.SetActive(false);
        }
        if (textureLow != null)
        {
            textureLow.mainTexture = null;
            textureLow.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 初始化组件
    /// </summary>
    public void SetLocalUseComponentsAndDatas()
    {
        textureTop = this.transform.FindChild("Panel_Top_Self/Texture_Top_Self").gameObject.GetComponent<UITexture>();
        textureLow = this.transform.FindChild("Panel_Low_Self/Texture_Low_Self").gameObject.GetComponent<UITexture>();
        shooterPosList.Add(this.transform.FindChild("ShooterPos_1"));
        shooterPosList.Add(this.transform.FindChild("ShooterPos_2"));
        shooterPosList.Add(this.transform.FindChild("ShooterPos_3"));
    }

    public void SetBarracksShowStatus(byte vBattleID)
    {
        battleID = vBattleID;
        if (SceneManager.Instance.Get_CurSceneType == EFightType.eftExpedition)
        {
            SetCastle(FightRelatedModule.Instance.mCastleInfo.mID);
            SetShooter(FightRelatedModule.Instance.mShooterList);
        }
        else
        {
            SetCastle(PlayerData.Instance.mCastleInfo.mID);
            SetShooter(PlayerData.Instance.mShooterList);
        }
    }
    //设置城堡显示//
    private void SetCastle(uint vID)
    {
        CastleAttributeInfo tmpCastleInfo = ConfigManager.Instance.mCastleConfig.FindByID(vID);
        if (tmpCastleInfo != null)
        {
            baseName = string.Format("{0}_{1}", tmpCastleInfo.fight_source, battleID);
            if (textureTop != null)
                SetSingleTexture(textureTop, baseName + "_L.assetbundle");
            if (textureLow != null)
                SetSingleTexture(textureLow, baseName + "_R.assetbundle");
        }
    }
    //设置射手显示//
    private void SetShooter(List<SingleShooterInfo> vShooterList)
    {
        if (vShooterList != null)
        {
            for (int i = 0; i < vShooterList.Count; i++)
            {
                if (vShooterList[i].mStatus == (int)EShooterStatus.essLock)
                    continue;
                if (i >= shooterPosList.Count)
                    continue;

                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                    new CData_CreateRole(vShooterList[i].mAttribute, 0, 20 - i, ERoleType.ertShooter, EHeroGender.ehgNone, 
                                        EFightCamp.efcSelf, SceneManager.Instance.Get_CurSceneType, 0, shooterPosList[i]));
            }
        }
    }

    private void SetSingleTexture(UITexture vTexture, string vName)
    {
        if ((vTexture == null) || (string.IsNullOrEmpty(vName)))
            return;
        ResourceLoadManager.Instance.LoadAloneImage(vName, (texture) => {
            vTexture.mainTexture = texture;
            vTexture.MakePixelPerfect();
            vTexture.gameObject.SetActive(true);
            if (boxCollider == null)
            {
                boxCollider = this.gameObject.AddComponent<BoxCollider>();
                this.gameObject.GetComponent<BoxCollider>().size = new Vector3(vTexture.localSize.x, 5000, 1000);
                //this.gameObject.GetComponent<BoxCollider>().center = new Vector3(-vTexture.localSize.x / 2, 0, 0);
                this.gameObject.GetComponent<BoxCollider>().center = new Vector3(-200, 0, 0);
            }
        });
    }

    protected override void HitOperate(HurtType vHurtType, int vCurHP)
    {
        if ((vHurtType != HurtType.Crite) && (vHurtType != HurtType.Normal))
            return;

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(hitAudio, this.transform));

        //闪红//
        if (vHurtType == HurtType.Normal)
        {
            SetBarracksColor(Color.red);
            Scheduler.Instance.AddTimer(0.1f, false, null, SetBarracksColor, Color.white);
        }

        if (vCurHP <= 0)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.CastleBeenDestory);
        }

        //播放特效//
        if ((vHurtType == HurtType.Crite) || (vCurHP <= 0))
        {
            string tmpEffectName = GlobalConst.Effect.GETHIT_CASTLE;
            if (vCurHP <= 0)
                tmpEffectName = GlobalConst.Effect.CASTLE_COLLAPSE;
            float tmpScaleValue = SkillTool.GetLocalScare(this);
            Vector3 tmpScale = new Vector3(1 / this.transform.localScale.x * tmpScaleValue, 1 / this.transform.localScale.y * tmpScaleValue, 1);

            EffectObjectCache.Instance.LoadGameObject(tmpEffectName, (GameObject gb) =>
            {
                GameObject go = CommonFunction.SetParent(gb, this.transform);
                if (go != null)
                {
                    go.transform.localScale = tmpScale;
                    go.transform.localEulerAngles = new Vector3(0, 180, 0);
                    go.transform.localPosition = new Vector3(-150, 0, 0);

                    SpineBase spine = go.GetComponent<SpineBase>();
                    if (spine == null)
                        spine = go.AddComponent<SpineBase>();
                    if (spine != null)
                    {
                        spine.InitSkeletonAnimation();
                        spine.setSortingOrder(40);
                        spine.repleaceAnimation("animation", false);
                        spine.EndEvent += (string s) => { if (go != null) UpdateTimeTool.Instance.DelayDelGameObject(go); };
                    }
                }
            });
        }
    }

    /// <summary>
    /// 改变城堡颜色
    /// </summary>
    /// <param name="vColor">目标颜色</param>
    private void SetBarracksColor(object vColor)
    {
        if (textureTop != null)
            textureTop.color = (Color)vColor;
        if (textureLow != null)
            textureLow.color = (Color)vColor;
    }

    public override bool ReSetRoleHP(int vValue, HurtType type, bool vIsShowChange = true, bool vIsInit = false)
    {
        base.ReSetRoleHP(vValue, type, vIsShowChange, vIsInit);
        //hpBuild.RefreshHp(vValue, type);
        return true;
    }
}