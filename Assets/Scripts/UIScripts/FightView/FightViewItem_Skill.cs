using UnityEngine;
using System.Collections;
using Assets.Script.Common;

/// <summary>
/// 战斗界面技能
/// </summary>
public class FightViewItem_Skill : MonoBehaviour
{
    /// <summary>
    /// 背景图
    /// </summary>
    private UISprite Spt_BG;
    /// <summary>
    /// 技能图片
    /// </summary>
    private UISprite Spt_Icon;
    /// <summary>
    /// 技能CD遮罩
    /// </summary>
    private UISprite Spt_Mask;
    /// <summary>
    /// 魔法背景
    /// </summary>
    private UISprite Spt_MagicBG;
    /// <summary>
    /// 魔法图标
    /// </summary>
    private UISprite Spt_MagicIcon;
    /// <summary>
    /// 魔法值
    /// </summary>
    private UILabel Lbl_Magic;
    private UILabel Lbl_AutoOrder;
    /// <summary>
    /// 技能释放特效
    /// </summary>
    private Transform Obj_SkillEffect;



    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool _initialized = false;
    /// <summary>
    /// 技能数据
    /// </summary>
    private SkillBaseData _SkillData;
    /// <summary>
    /// 战斗类型
    /// </summary>
    private EFightType _FightType;
    /// <summary>
    /// CD时间
    /// </summary>
    private float _CDTime;
    /// <summary>
    /// 点击时间
    /// </summary>
    private float _ClickTime;
    /// <summary>
    /// 暂停时间
    /// </summary>
    private float _PauseTime;
    /// <summary>
    /// 战斗界面信息
    /// </summary>
    private FightViewController pFightViewController;
    /// <summary>
    /// 按钮状态
    /// </summary>
    private EFightViewSkillStatus _ItemStatus;
    private EFightViewSkillStatus _PreStatus;
    private Transform mTrans;
    /// <summary>
    /// 暂停类型
    /// </summary>
    private EFightPauseType pauseType = EFightPauseType.efptNormal;
    /// <summary>
    /// 新手引导是否可以释放技能[false-不能释放技能 true-可以释放技能]
    /// </summary>
    private bool isCanUseSkill;
    private Color colorMagic = Color.white;
    private bool isUseSkill = false;
    private int magicValue = 0;
    private bool isAutoFight = false;
    private EFightCamp heroCamp;
    private float curFightSpeed;
    private float showCDTime;


    public SkillBaseData Get_SkillData
    {
        get
        {
            return _SkillData;
        }
    }


    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;

        mTrans = this.transform;
        Spt_BG = mTrans.FindChild("SkItem_BG").GetComponent<UISprite>();
        Spt_Icon = mTrans.FindChild("SkItem_Icon").GetComponent<UISprite>();
        Spt_Mask = mTrans.FindChild("SkItem_Mask").GetComponent<UISprite>();
        Spt_MagicBG = mTrans.FindChild("SkItem_MagicBG").GetComponent<UISprite>();
        Spt_MagicIcon = mTrans.FindChild("SkItem_MagicBG/SkItem_MagicIcon").GetComponent<UISprite>();
        Lbl_Magic = mTrans.FindChild("SkItem_MagicBG/SkItem_Magic").GetComponent<UILabel>();
        Lbl_AutoOrder = mTrans.FindChild("SkItem_AutoOrder").GetComponent<UILabel>();
        Obj_SkillEffect = mTrans.FindChild("SkItem_Effect");

        InitStatus();
        UIEventListener.Get(mTrans.gameObject).onClick = ButtonEvent_SkillComp;
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_NewGuideCanUseSkill, CommandEvent_NewGuideCanUseSkill);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
    }
    public void UnInitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_NewGuideCanUseSkill, CommandEvent_NewGuideCanUseSkill);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
        CancelInvoke("CheckEnergyIsEnough");
    }
    public void ButtonEvent_SkillComp(GameObject go)
    {
        if (!isAutoFight)
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, go.transform));
        if (!isCanUseSkill)
            return;
        if (SceneManager.Instance.Get_CurScene.Get_SceneStatus != ESceneStatus.essNormal)
            return;
        if (_ItemStatus != EFightViewSkillStatus.eskNormal)
            return;
        if ((pFightViewController == null) || (_SkillData == null))
            return;
        if (pFightViewController.Get_CurValue_Magic < magicValue)
            return;

        GuideManager.Instance.CheckTrigger(GuideTrigger.FightingUseSkill);
        //显示CD//
        _ClickTime = Time.time;
        _PauseTime = 0;
        _ItemStatus = EFightViewSkillStatus.eskCD;
        curFightSpeed = SceneManager.Instance.Get_FightSpeed;
        showCDTime = 0;
        Scheduler.Instance.AddUpdator(ShowSkillCD);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerMagic, new FightDecMP(heroCamp, -magicValue));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_ChangeCurPetPower, heroCamp);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightUseSkill, new UseSkillInfo(_SkillData.Equip, heroCamp));
        isUseSkill = true;
    }

    private void CommandEvent_FightSetPause(object vDataObj)
    {
        if (vDataObj != null)
        {
            pauseType = (EFightPauseType)vDataObj;
            if (pauseType == EFightPauseType.efptNoSkill)
                return;
        }
        else
            pauseType = EFightPauseType.efptNormal;

        if (_ItemStatus != EFightViewSkillStatus.eskPause)
        {
            _PreStatus = _ItemStatus;
            _ItemStatus = EFightViewSkillStatus.eskPause;
        }
        _PauseTime = Time.time;
    }
    private void CommandEvent_FightSetResume(object vDataObj)
    {
        if (_ItemStatus != EFightViewSkillStatus.eskPause)
            return;
        _ItemStatus = _PreStatus;
        //_ClickTime = Time.time - (_PauseTime - _ClickTime);
        _ClickTime = Time.time;
        pauseType = EFightPauseType.efptNormal;
    }
    private void CommandEvent_NewGuideCanUseSkill(object vDataObj)
    {
        isCanUseSkill = true;
    }
    private void CommandEvent_ChangeSpeed(object vDataObj)
    {
        curFightSpeed = (float)vDataObj;
    }

    /// <summary>
    /// 修改BoxCollider状态
    /// </summary>
    /// <param name="vStatus"></param>
    private void ReSetBoxColliderStatus(bool vStatus)
    {
        if (mTrans == null)
            return;
        if (mTrans.gameObject.GetComponent<BoxCollider>() == null)
            return;
        mTrans.gameObject.GetComponent<BoxCollider>().enabled = vStatus;
    }

    public void UpdateAutoOrder(bool showOrder, byte order = 0)
    {
        Lbl_AutoOrder.enabled = showOrder;
        Lbl_AutoOrder.text = order.ToString();
    }

    private void CheckEnergyIsEnough()
    {
        Color tmpColor;
        if (_ItemStatus != EFightViewSkillStatus.eskNormal)
            Obj_SkillEffect.gameObject.SetActive(false);

        if ((pFightViewController == null) || (_SkillData == null))
            return;
        if (pFightViewController.Get_CurValue_Magic < magicValue)
        {
            Lbl_Magic.color = Color.white;
            tmpColor = Color.black;
            Obj_SkillEffect.gameObject.SetActive(false);
        }
        else
        {
            tmpColor = Color.white;
            Lbl_Magic.color = colorMagic;
            if (_ItemStatus == EFightViewSkillStatus.eskNormal)
                Obj_SkillEffect.gameObject.SetActive(true);
        }

        Spt_BG.color = tmpColor;
        Spt_Icon.color = tmpColor;
        Spt_Mask.color = tmpColor;
        Spt_MagicBG.color = tmpColor;
        Spt_MagicIcon.color = tmpColor;
    }

    //检测是否能够释放技能//
    private void NewGuideCheckIsCanUseSkill()
    {
        if (isCanUseSkill)
            return;
        if ((pFightViewController == null) || (_SkillData == null))
            return;
        //检测魔法值是否足够//
        if (pFightViewController.Get_CurValue_Magic < magicValue)
            return;
        //新手引导检测是否有敌人在技能攻击范围//
        Skill tmpSkill = Skill.createByID(_SkillData.id);
        if (tmpSkill == null)
            return;
        if (tmpSkill.GetLockTarget(RoleManager.Instance.Get_Hero) == null)
            return;
        //通知新手引导可以释放技能//
        GuideManager.Instance.CheckTrigger(GuideTrigger.SpecialFightUseSkill);
        Scheduler.Instance.RemoveUpdator(NewGuideCheckIsCanUseSkill);
    }

    /// <summary>
    /// 显示CD
    /// </summary>
    private void ShowSkillCD()
    {
        if (_ItemStatus != EFightViewSkillStatus.eskCD)
            return;

        //float tmpValue = (_CDTime - SceneManager.Instance.Get_FightSpeed * (Time.time - _ClickTime)) / _CDTime;
        showCDTime += curFightSpeed * (Time.time - _ClickTime);
        _ClickTime = Time.time;
        float tmpValue = (_CDTime - showCDTime) / _CDTime;
        if (tmpValue <= 0)
        {
            tmpValue = 0;
            Scheduler.Instance.RemoveUpdator(ShowSkillCD);
            _ItemStatus = EFightViewSkillStatus.eskNormal;
        }
        Spt_Mask.fillAmount = tmpValue;
    }

    /// <summary>
    /// 初始化英雄技能显示
    /// </summary>
    public void InitStatus()
    {
        Spt_BG.gameObject.SetActive(true);
        Spt_Icon.spriteName = string.Empty;
        Spt_Mask.gameObject.SetActive(true);
        Spt_Mask.fillAmount = 0;
        Spt_MagicBG.gameObject.SetActive(false);
        Lbl_Magic.text = string.Empty;
        _ItemStatus = EFightViewSkillStatus.eskNormal;
        _PreStatus = EFightViewSkillStatus.eskNormal;
        ReSetBoxColliderStatus(true);
        isCanUseSkill = false;
        if (colorMagic == Color.white)
            colorMagic = Lbl_Magic.color;
        Obj_SkillEffect.gameObject.SetActive(false);
        _SkillData = null;
        heroCamp = EFightCamp.efcSelf;
        UpdateAutoOrder(false);
    }

    private void ShowStatus()
    {
        _CDTime = 0;
        _ClickTime = 0;

        if (_SkillData == null)
            return;
        _CDTime = _SkillData._CDTime;

        CommonFunction.SetSpriteName(Spt_Icon, _SkillData.Equip.Att.icon);
        if ((EFightType.eftPVP == _FightType) || (EFightType.eftSlave == _FightType) || (EFightType.eftServerHegemony == _FightType) || (EFightType.eftQualifying == _FightType))
        {
            _ItemStatus = EFightViewSkillStatus.eskLock;
            Spt_Mask.fillAmount = 1;
            return;
        }
        else
        {
            Spt_MagicBG.gameObject.SetActive(true);
            Lbl_Magic.text = magicValue.ToString();
        }

        Spt_Mask.fillAmount = 0;
        _ItemStatus = EFightViewSkillStatus.eskNormal;
    }

    /// <summary>
    /// 刷新英雄技能
    /// </summary>
    /// <param name="data"></param>
    /// <param name="vFightType"></param>
    /// <param name="vFightViewController"></param>
    public void RefreshInfo_ShowStatus(SkillBaseData data, EFightCamp vRoleCamp = EFightCamp.efcSelf)
    {
        InitStatus();
        _SkillData = data;
        heroCamp = vRoleCamp;
        pFightViewController = UISystem.Instance.FightView;
        _FightType = pFightViewController.Get_FightType;
        magicValue = 0;
        if (_SkillData != null)
        {
            magicValue = _SkillData._Magic;
            if (RoleManager.Instance.Get_Hero != null)
            {
                float tmpValue = magicValue * RoleManager.Instance.Get_Hero._Coeff_Mp_Cost + RoleManager.Instance.Get_Hero._Num_MP_Cost;
                magicValue = (int)tmpValue;
            }
        }
        ShowStatus();

        InvokeRepeating("CheckEnergyIsEnough", 0, 0.1f);
        PreLoadResource_Skill();
        ReSetBoxColliderStatus(true);

        if (pFightViewController == null)
            return;
        if (pFightViewController.Get_FightType != EFightType.eftNewGuide)
        {
            isCanUseSkill = true;
            return;
        }
        Scheduler.Instance.AddUpdator(NewGuideCheckIsCanUseSkill);
    }
    public void PreLoadResource_Skill()
    {
        if (_SkillData != null)
            _SkillData.CacheEffect();
    }
    public bool CheckIsCanUseSkill()
    {
        isAutoFight = true;
        isUseSkill = false;
        ButtonEvent_SkillComp(this.gameObject);
        isAutoFight = false;
        return isUseSkill;
    }

}