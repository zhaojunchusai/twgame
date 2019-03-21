using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastleViewItem_Shooter
{

    /// <summary>
    /// 
    /// </summary>
    private Transform Trans_Root;
    /// <summary>
    /// 遮罩
    /// </summary>
    private UISprite Spt_Item_Mask;
    /// <summary>
    /// 射手图标
    /// </summary>
    private Transform Trans_Left_Icon;
    /// <summary>
    /// 等级父级
    /// </summary>
    private Transform Obj_Left_Level;
    /// <summary>
    /// 当前等级
    /// </summary>
    private UILabel Lbl_Level_Value;
    /// <summary>
    /// 最大等级图标
    /// </summary>
    private UISprite Spt_Level_MAX;
    /// <summary>
    /// 当前攻击力
    /// </summary>
    private UILabel Lbl_Attack_Value;
    /// <summary>
    /// 攻击成长
    /// </summary>
    private UILabel Lbl_Attack_Next;
    /// <summary>
    /// 攻击距离
    /// </summary>
    private UILabel Lbl_Distance_Value;
    /// <summary>
    /// 描述
    /// </summary>
    private UILabel Lbl_Center_Description;
    /// <summary>
    /// 价格图标
    /// </summary>
    private UISprite Spt_Price_Icon;
    /// <summary>
    /// 解锁/升级价格
    /// </summary>
    private UILabel Lbl_Price_Value;
    /// <summary>
    /// 按钮
    /// </summary>
    private UIButton Btn_Right_UpLV;
    /// <summary>
    /// 按钮背景图
    /// </summary>
    private UISprite SPt_UpLV_BG;
    /// <summary>
    /// 按钮文字
    /// </summary>
    private UILabel Lbl_UpLV_Title;

    /// <summary>
    /// 射手ID
    /// </summary>
    public uint ShooterID;
    /// <summary>
    /// 射手当前等级
    /// </summary>
    private int mShooterLevel;
    /// <summary>
    /// 解锁需要城堡等级
    /// </summary>
    private int mUnLockLevel;
    /// <summary>
    /// 射手状态
    /// </summary>
    private EShooterStatus mShooterStatus;
    /// <summary>
    /// 是否设置数据
    /// </summary>
    private bool mIsSetInfo;
    /// <summary>
    /// 射手物件
    /// </summary>
    private RoleBase shooterRoleBase;

    private int mRoleSortingOrder = 1;
    private GameObject Go_ShooterOperateEffect;
    public CastleViewItem_Shooter(Transform vTargetTrans)
    {
        if (vTargetTrans == null)
            return;

        Trans_Root = vTargetTrans;
        Spt_Item_Mask = Trans_Root.FindChild("Item_Mask").GetComponent<UISprite>();
        Trans_Left_Icon = Trans_Root.FindChild("Item_Left/Left_Icon");
        Obj_Left_Level = Trans_Root.FindChild("Item_Left/Left_Level").GetComponent<Transform>();
        Lbl_Level_Value = Trans_Root.FindChild("Item_Left/Left_Level/Level_Value").GetComponent<UILabel>();
        Spt_Level_MAX = Trans_Root.FindChild("Item_Left/Left_Level/Level_MAX").GetComponent<UISprite>();
        Lbl_Attack_Value = Trans_Root.FindChild("Item_Center/Center_Attack/Attack_Value").GetComponent<UILabel>();
        Lbl_Attack_Next = Trans_Root.FindChild("Item_Center/Center_Attack/Attack_Next").GetComponent<UILabel>();
        Lbl_Distance_Value = Trans_Root.FindChild("Item_Center/Center_Distance/Distance_Value").GetComponent<UILabel>();
        Lbl_Center_Description = Trans_Root.FindChild("Item_Center/Center_Description").GetComponent<UILabel>();
        Spt_Price_Icon = Trans_Root.FindChild("Item_Right/Right_Price/Price_Icon").GetComponent<UISprite>();
        Lbl_Price_Value = Trans_Root.FindChild("Item_Right/Right_Price/Price_Value").GetComponent<UILabel>();
        Btn_Right_UpLV = Trans_Root.FindChild("Item_Right/Right_UpLV").GetComponent<UIButton>();
        SPt_UpLV_BG = Trans_Root.FindChild("Item_Right/Right_UpLV/UpLV_BG").GetComponent<UISprite>();
        Lbl_UpLV_Title = Trans_Root.FindChild("Item_Right/Right_UpLV/UpLV_Title").GetComponent<UILabel>();
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_CASTLELEVELUP, (GameObject gb) => { Go_ShooterOperateEffect = gb; });

        InitItemShowStatus();

        UIEventListener.Get(Btn_Right_UpLV.gameObject).onClick = ButtonEvent_ShooterOperate;
    }

    public void DestroyShooter()
    {
        if (Trans_Root != null)
            GameObject.Destroy(Trans_Root.gameObject);
    }

    public void DeleteRoleInfo()
    {
        if (shooterRoleBase != null)
            shooterRoleBase.UnInitialization();
    }

    /// <summary>
    /// 按钮点击事件
    /// </summary>
    /// <param name="vObj"></param>
    private float preClickTime = 0;
    private void ButtonEvent_ShooterOperate(GameObject vObj)
    {
        if (Time.time - preClickTime < 0.2f)
            return;
        preClickTime = Time.time;
        if (mShooterStatus == EShooterStatus.essLock)
        {
            CastleModule.Instance.SendUnlockShooterReq(ShooterID);
        }
        else
        {
            CastleModule.Instance.SendUpgradeShooterReq(ShooterID);
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, vObj.transform));
    }

    /// <summary>
    /// 刷新按钮状态
    /// </summary>
    public void RefreshBtnStatus()
    {
        if (!mIsSetInfo)
        {
            SetBtnStatus(true);
            return;
        }

        if (mShooterStatus == EShooterStatus.essUnLock)
        {
            if (PlayerData.Instance.mCastleInfo.mLevel <= mShooterLevel)
                SetBtnStatus(true);
            else
                SetBtnStatus(false);
        }
        else
        {
            if (PlayerData.Instance.mCastleInfo.mLevel < mUnLockLevel)
                SetBtnStatus(true);
            else
                SetBtnStatus(false);
        }
    }

    public void InitItemShowStatus()
    {
        ShooterID = 0;
        mShooterLevel = 0;
        mUnLockLevel = 0;
        mShooterStatus = EShooterStatus.essLock;
        mIsSetInfo = false;
        if (Spt_Item_Mask != null)
            Spt_Item_Mask.gameObject.SetActive(true);
        if (Obj_Left_Level != null)
            Obj_Left_Level.gameObject.SetActive(true);
        if (Lbl_Level_Value != null)
            Lbl_Level_Value.text = string.Empty;
        if (Spt_Level_MAX != null)
            Spt_Level_MAX.gameObject.SetActive(false);
        if (Lbl_Attack_Value != null)
            Lbl_Attack_Value.text = string.Empty;
        if (Lbl_Attack_Next != null)
            Lbl_Attack_Next.text = string.Empty;
        if (Lbl_Distance_Value != null)
            Lbl_Distance_Value.text = string.Empty;
        if (Lbl_Center_Description != null)
            Lbl_Center_Description.text = string.Empty;
        if (Spt_Price_Icon != null)
            Spt_Price_Icon.gameObject.SetActive(true);
        if (Lbl_Price_Value != null)
            Lbl_Price_Value.text = string.Empty;
        if (Btn_Right_UpLV != null)
            Btn_Right_UpLV.gameObject.SetActive(true);
        if (Lbl_UpLV_Title != null)
            Lbl_UpLV_Title.text = ConstString.CASTLE_SHOOTER_UNLOCK;//"解锁射手";
        RefreshBtnStatus();
    }

    private void Refresh_Shooter_Icon(SingleShooterInfo vShooterInfo)
    {
        if (vShooterInfo == null)
            return;
        if (Trans_Left_Icon == null)
            return;
        if ((shooterRoleBase != null) && (shooterRoleBase.Get_FightAttribute.KeyData == vShooterInfo.mAttribute.KeyData))
            return;
        if (shooterRoleBase != null)
            shooterRoleBase.UnInitialization();

        Main.Instance.StartCoroutine(DelayRefreshShooter(vShooterInfo));
    }

    private IEnumerator DelayRefreshShooter(SingleShooterInfo vShooterInfo)
    {
        yield return shooterRoleBase == null;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(vShooterInfo.mAttribute, 0, 10, ERoleType.ertShooter,
            EHeroGender.ehgNone, EFightCamp.efcNone, EFightType.eftUI, 0, Trans_Left_Icon, (obj) =>
            {
                shooterRoleBase = obj;
                shooterRoleBase.transform.localPosition = new Vector3(0, -25, 0);
                shooterRoleBase.Get_MainSpine.setSortingOrder(mRoleSortingOrder);
                OffestSortingOrder tempSortOrder = shooterRoleBase.gameObject.AddComponent<OffestSortingOrder>();
                tempSortOrder.offestOrder = 1;
                tempSortOrder.nguiLayer = OffestSortingOrder.NGUILayerEnum.Role;
            }));
    }

    /// <summary>
    /// 刷新界面显示
    /// </summary>
    /// <param name="vShooterInfo">射手数据[SingleShooterInfo]</param>
    public void RefreshUIStatus(SingleShooterInfo vShooterInfo)
    {
        InitItemShowStatus();
        //获取射手信息//
        if (vShooterInfo == null)
            return;
        //获取配置表信息//
        CastleAttributeInfo tmpCastleInfo = ConfigManager.Instance.mCastleConfig.FindByID(vShooterInfo.mID);
        if (tmpCastleInfo == null)
            return;
        mIsSetInfo = true;
        mRoleSortingOrder = UISystem.Instance.CastleView.view.UIPanel_Content.sortingOrder+1;
        ShooterID = vShooterInfo.mID;
        mShooterLevel = vShooterInfo.mLevel;
        mUnLockLevel = tmpCastleInfo.unlock_level;
        mShooterStatus = (EShooterStatus)vShooterInfo.mStatus;

        //遮罩//
        if (Spt_Item_Mask != null)
        {
            if (mShooterStatus == EShooterStatus.essUnLock)
                Spt_Item_Mask.gameObject.SetActive(false);
            else
                Obj_Left_Level.gameObject.SetActive(false);
        }
        //图标//
        Refresh_Shooter_Icon(vShooterInfo);
        //等级 攻击成长//
        if ((tmpCastleInfo.level_limit == vShooterInfo.mLevel) && (tmpCastleInfo.suffix_id == 0))
        {
            if (Spt_Level_MAX != null)
                Spt_Level_MAX.gameObject.SetActive(true);
        }
        else
        {
            if (Lbl_Level_Value != null)
                Lbl_Level_Value.text = vShooterInfo.mLevel.ToString();
            if (Lbl_Attack_Next != null)
                Lbl_Attack_Next.text = string.Format("+{0}", vShooterInfo.mAtt_Grow);
        }
        //攻击力//
        if (Lbl_Attack_Value != null)
            Lbl_Attack_Value.text = vShooterInfo.mAttribute.Attack.ToString();
        //攻击距离//
        if (Lbl_Distance_Value != null)
            Lbl_Distance_Value.text = vShooterInfo.mAttribute.AttDistance.ToString();
        //技能描述//
        if (Lbl_Center_Description != null)
        {
            if (mShooterStatus == EShooterStatus.essUnLock)
            {
                Skill tmpSkillData = Skill.createByID(tmpCastleInfo.unlock_positive.ID);
                if (tmpSkillData != null)
                {
                    Lbl_Center_Description.text = tmpSkillData.GetDescript(vShooterInfo.mLevel);
                }                
            }
            else
            {
                Lbl_Center_Description.text = string.Format(ConstString.UNLOCK_SHOOTER_HINT, tmpCastleInfo.unlock_level);
            }
        }
        //价格//
        if (Lbl_Price_Value != null)
        {
            MaterialsBagAttributeInfo tmpMaterialInfo = null;
            if (mShooterStatus == EShooterStatus.essUnLock)
            {
                if (tmpCastleInfo.levelup_cost.ContainsKey(vShooterInfo.mLevel - tmpCastleInfo.level))
                    tmpMaterialInfo = ConfigManager.Instance.mMaterialsBagData.FindBynId(tmpCastleInfo.levelup_cost[vShooterInfo.mLevel - tmpCastleInfo.level]);
            }
            else
            {
                tmpMaterialInfo = ConfigManager.Instance.mMaterialsBagData.FindBynId(tmpCastleInfo.unlock_cost_packid);
            }
            if (tmpMaterialInfo != null)
                Lbl_Price_Value.text = string.Format("X{0}",CommonFunction.GetTenThousandUnit(tmpMaterialInfo.Cost,10000));
        }
        //按钮信息//
        if (Lbl_UpLV_Title != null)
        {
            if (mShooterStatus == EShooterStatus.essUnLock)
                Lbl_UpLV_Title.text = ConstString.CASTLE_SHOOTER_UPLV;// "射手升级";
        }

        RefreshBtnStatus();
    }

    private void SetBtnStatus(bool vIsGray)
    {
        //设置按钮显示状态//
        if (SPt_UpLV_BG != null)
            CommonFunction.UpdateWidgetGray(SPt_UpLV_BG, vIsGray);
        if (Lbl_UpLV_Title != null)
        {
            if (vIsGray)
                CommonFunction.SetLabelColor_I(Lbl_UpLV_Title, 67, 67, 67, 255, 162, 162, 162, 255);
            else
                CommonFunction.SetLabelColor_I(Lbl_UpLV_Title, 111, 52, 14, 255, 234, 201, 93, 255);
        }
        //设置按钮点击状态//
        Btn_Right_UpLV.GetComponent<BoxCollider>().enabled = !vIsGray;
    }
    //=================================================================================//
    public void PlayShooterOperateEffect()
    {
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_ShooterOperateEffect, Trans_Left_Icon);
        //Debug.LogError(go.name + "NAME     " + go.transform.localPosition +"TR");
    }
}
