using UnityEngine;
using System.Collections;
using Assets.Script.Common;
using TdSpine;

/// <summary>
/// 敌方基地
/// </summary>
public class Barracks_Enemy : RoleAttribute
{
    private UITexture textureTop;
    private UITexture textureLow;
    private BoxCollider boxCollider;
    private int castleID;
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
        SetRoleCamp(EFightCamp.efcEnemy);
        SetRoleType(ERoleType.ertBarracks);
        //hpBuild = new BarracksHp();
        //hpBuild.Instance(this, ERoleType.ertBarracks, new Vector3(90, 200, 0));
        //hpBuild.SetCurAndMaxValue(fightAttribute.HP, this.Get_MaxHPValue);
        pRpleHP = new BarracksHp();
        pRpleHP.Instance(this, ERoleType.ertBarracks, new Vector3(90, 200, 0));
        pRpleHP.SetCurAndMaxValue(fightAttribute.HP, this.Get_MaxHPValue);

        roleLevel = 0;
    }

    protected override void FinalOperate()
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_Dead_Building, this.transform));
        //if (textureTop != null)
        //    SetSingleTexture(textureTop, string.Format("image_castle_0_{0}_L.assetbundle", castleID));
        //if (textureLow != null)
        //    SetSingleTexture(textureLow, string.Format("image_castle_0_{0}_R.assetbundle", castleID));
        if (textureTop != null)
            SetSingleTexture(textureTop, string.Format("{0}_0_L.assetbundle", baseName));
        if (textureLow != null)
            SetSingleTexture(textureLow, string.Format("{0}_0_R.assetbundle", baseName));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDieBarracksEnemy));
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
        textureTop = this.transform.FindChild("Panel_Top_Enemy/Texture_Top_Enemy").gameObject.GetComponent<UITexture>();
        textureLow = this.transform.FindChild("Panel_Low_Enemy/Texture_Low_Enemy").gameObject.GetComponent<UITexture>();
    }

    public void SetBarracksTexture(byte vBattleID, string vCastleName)
    {
        if (string.IsNullOrEmpty(vCastleName))
            return;
        castleID = vBattleID;
        baseName = string.Format("{0}_{1}", vCastleName, castleID);

        if (textureTop != null)
            SetSingleTexture(textureTop, baseName + "_L.assetbundle");
        if (textureLow != null)
            SetSingleTexture(textureLow, baseName + "_R.assetbundle");
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
                //this.gameObject.GetComponent<BoxCollider>().center = new Vector3(vTexture.localSize.x / 2, 0, 0);
                this.gameObject.GetComponent<BoxCollider>().center = new Vector3(200, 0, 0);
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
                    go.transform.localEulerAngles = new Vector3(0, 0, 0);
                    go.transform.localPosition = new Vector3(150, 0, 0);

                    SpineBase spine = go.GetComponent<SpineBase>();
                    if(spine == null)
                        spine = go.AddComponent<SpineBase>();

                    if (spine != null)
                    {
                        spine.InitSkeletonAnimation();
                        spine.setSortingOrder(40);
                        spine.repleaceAnimation("animation", false);
                        spine.EndEvent += (string s) => { if (go != null) EffectObjectCache.Instance.FreeObject(go); };
                    }
                }
            });
        }
    }

    /// <summary>
    /// 改变城堡颜色
    /// </summary>
    /// <param name="vColor">目标颜色[true-变红 false-还原]</param>
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
        //hpBuild.RefreshHp(vValue,type);
        return true;
    }

}
