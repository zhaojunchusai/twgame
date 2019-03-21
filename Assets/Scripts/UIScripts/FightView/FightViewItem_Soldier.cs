using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 战斗界面士兵
/// </summary>
public class FightViewItem_Soldier : MonoBehaviour
{

    /// <summary>
    /// 背景图
    /// </summary>
    private UISprite Spt_BG;
    /// <summary>
    /// 士兵图标
    /// </summary>
    private UISprite Spt_Icon;
    /// <summary>
    /// 遮罩
    /// </summary>
    private UISprite Spt_Mask;
    /// <summary>
    /// 星级
    /// </summary>
    private UISprite Spt_Star;
    /// <summary>
    /// 等级
    /// </summary>
    private UILabel Lbl_Level;
    /// <summary>
    /// 锁定图标
    /// </summary>
    private UISprite Spt_Lock;
    /// <summary>
    /// 数量[远征 PVP]
    /// </summary>
    private UILabel Lbl_Num;
    /// <summary>
    /// 魔法背景
    /// </summary>
    private UISprite Spt_PowerBG;
    /// <summary>
    /// 魔法图标
    /// </summary>
    private UISprite Spt_PowerIcon;
    /// <summary>
    /// 魔法值
    /// </summary>
    private UILabel Lbl_Power;
    /// <summary>
    /// 武将品质
    /// </summary>
    private UISprite Spt_Quality;
    /// <summary>
    /// Icon底板
    /// </summary>
    private UISprite Spt_IconBG;
    /// <summary>
    /// 职业图标
    /// </summary>
    private UISprite Spt_Career;
    /// <summary>
    /// 士兵召唤特效
    /// </summary>
    private Transform Obj_SoldierEffect;
    /// <summary>
    /// 士兵自动战斗排序
    /// </summary>
    private UILabel Lbl_AutoOrder;


    /// <summary>
    /// 士兵数据
    /// </summary>
    private Soldier fightSoldierInfo;
    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool _initialized = false;
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
    /// 士兵星级
    /// </summary>
    List<Transform> _StarList = new List<Transform>();
    /// <summary>
    /// 按钮状态
    /// </summary>
    private EFightViewSoldierStatus _ItemStatus;
    private EFightViewSoldierStatus _PreStatus;
    /// <summary>
    /// 暂停类型
    /// </summary>
    private EFightPauseType pauseType = EFightPauseType.efptNormal;
    /// <summary>
    /// 最大召唤数量
    /// </summary>
    private int maxSoldierNum = 0;
    /// <summary>
    /// 当前召唤数量
    /// </summary>
    private int curSoldierNum;
    private Color colorLevel = Color.white;
    private Color colorNum = Color.white;
    private Color colorPower = Color.white;
    private bool isCanCallSoldier = false;
    private bool isAutoFight = false;
    private float curFightSpeed;
    private float showCDTime;
    private EFightCamp fightCamp;


    public Soldier Get_FightSoldierInfo
    {
        get
        {
            return fightSoldierInfo;
        }
    }


    /// <summary>
    /// 初始化界面
    /// </summary>
    public void Initialize()
    {
        //----------------------判断是否已经初始化//
        if (_initialized)
            return;
        _initialized = true;
        fightCamp = EFightCamp.efcNone;

        //----------------------获取物件//
        Transform tmpTrans = this.transform;
        Spt_BG = tmpTrans.FindChild("SoItem_BG").GetComponent<UISprite>();
        Spt_Icon = tmpTrans.FindChild("SoItem_Icon").GetComponent<UISprite>();
        Spt_Mask = tmpTrans.FindChild("SoItem_Mask").GetComponent<UISprite>();
        Spt_Star = tmpTrans.FindChild("SoItem_Star").GetComponent<UISprite>();
        Lbl_Level = tmpTrans.FindChild("SoItem_Level").GetComponent<UILabel>();
        Spt_Lock = tmpTrans.FindChild("SoItem_Lock").GetComponent<UISprite>();
        Lbl_Num = tmpTrans.FindChild("SoItem_Num").GetComponent<UILabel>();
        Spt_PowerBG = tmpTrans.FindChild("SoItem_PowerBG").GetComponent<UISprite>();
        Spt_PowerIcon = tmpTrans.FindChild("SoItem_PowerBG/SoItem_PowerIcon").GetComponent<UISprite>();
        Lbl_Power = tmpTrans.FindChild("SoItem_PowerBG/SoItem_Power").GetComponent<UILabel>();
        Spt_Quality = tmpTrans.FindChild("SoItem_Quality").GetComponent<UISprite>();
        Spt_IconBG = tmpTrans.FindChild("SoItem_IconBG").GetComponent<UISprite>();
        Spt_Career = tmpTrans.FindChild("SoItem_Career").GetComponent<UISprite>();
        Lbl_AutoOrder = tmpTrans.FindChild("SoItem_AutoOrder").GetComponent<UILabel>();

        Obj_SoldierEffect = tmpTrans.FindChild("SoItem_Effect");
        pFightViewController = UISystem.Instance.FightView;
        //----------------------初始化显示//
        InitStatus();

        //----------------------绑定事件//
        UIEventListener.Get(tmpTrans.gameObject).onClick = ButtonEvent_SkillComp;
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_DeleteSingleSoldierNum, CommandEvent_DeleteSingleSoldierNum);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
    }

    public void UnInitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_DeleteSingleSoldierNum, CommandEvent_DeleteSingleSoldierNum);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
        CancelInvoke("CheckEnergyIsEnough");
    }

    private void CommandEvent_FightSetPause(object vDataObj)
    {
        if (vDataObj != null)
        {
            pauseType = (EFightPauseType)vDataObj;
            if (pauseType == EFightPauseType.efptNoSol)
                return;
        }
        else
            pauseType = EFightPauseType.efptNormal;

        if (_ItemStatus != EFightViewSoldierStatus.esoPause)
        {
            _PreStatus = _ItemStatus;
            _ItemStatus = EFightViewSoldierStatus.esoPause;
        }
        _PauseTime = Time.time;
    }
    private void CommandEvent_FightSetResume(object vDataObj)
    {
        if (_ItemStatus != EFightViewSoldierStatus.esoPause)
            return;
        _ItemStatus = _PreStatus;
        //_ClickTime = Time.time - (_PauseTime - _ClickTime);
        _ClickTime = Time.time;
        pauseType = EFightPauseType.efptNormal;
    }
    private void CommandEvent_DeleteSingleSoldierNum(object vDataObj)
    {
        //if (vDataObj == null)
        //    return;
        //uint tmpKey = (uint)vDataObj;
        //if (fightSoldierInfo == null)
        //    return;
        //if (fightSoldierInfo.showInfoSoldier == null)
        //    return;
        //if (fightSoldierInfo.showInfoSoldier.KeyData != tmpKey)
        //    return;
        //curSoldierNum -= 1;
        //if (curSoldierNum < 0)
        //    curSoldierNum = 0;
        //ShowSoldierNum();
        if (vDataObj == null)
            return;
        DeleteSingleSoldierInfo tmpInfo = (DeleteSingleSoldierInfo)vDataObj;
        if (fightSoldierInfo == null)
            return;
        if (fightSoldierInfo.showInfoSoldier == null)
            return;
        if (fightSoldierInfo.showInfoSoldier.KeyData != tmpInfo.ID)
            return;
        if (tmpInfo.FightCamp != fightCamp)
            return;
        curSoldierNum -= 1;
        if (curSoldierNum < 0)
            curSoldierNum = 0;
        ShowSoldierNum();
    }
    private void CommandEvent_ChangeSpeed(object vDataObj)
    {
        curFightSpeed = (float)vDataObj;
    }

    private void CheckEnergyIsEnough()
    {
        Color tmpColor;
        if (_ItemStatus != EFightViewSoldierStatus.esoNormal)
            Obj_SoldierEffect.gameObject.SetActive(false);

        if ((pFightViewController == null) || (fightSoldierInfo == null) || (fightSoldierInfo.Att == null))
            return;
        if ((pFightViewController.Get_CurValue_Energy < fightSoldierInfo.Att.call_energy) || (CheckIsFull()))//(curSoldierNum >= maxSoldierNum))
        {
            Obj_SoldierEffect.gameObject.SetActive(false);
            Lbl_Power.color = Color.white;
            Lbl_Num.color = Color.white;
            tmpColor = Color.black;
        }
        else
        {
            if (_ItemStatus == EFightViewSoldierStatus.esoNormal)
                Obj_SoldierEffect.gameObject.SetActive(true);
            tmpColor = Color.white;
            Lbl_Power.color = colorPower;
            Lbl_Num.color = colorNum;
        }

        Spt_BG.color = tmpColor;
        Spt_Icon.color = tmpColor;
        Spt_Quality.color = tmpColor;
        Spt_IconBG.color = tmpColor;
        Spt_Career.color = tmpColor;
        Spt_PowerBG.color = tmpColor;
        Spt_PowerIcon.color = tmpColor;
        for (int i = 0; i < _StarList.Count; i++)
            _StarList[i].GetComponent<UISprite>().color = tmpColor;
    }

    /// <summary>
    /// 显示CD
    /// </summary>
    private void ShowSoldierCD()
    {
        if (_ItemStatus != EFightViewSoldierStatus.esoCD)
            return;

        //float tmpValue = (_CDTime - SceneManager.Instance.Get_FightSpeed * (Time.time - _ClickTime)) / _CDTime;
        showCDTime += curFightSpeed * (Time.time - _ClickTime);
        _ClickTime = Time.time;
        float tmpValue = (_CDTime - showCDTime) / _CDTime;
        if (tmpValue <= 0)
        {
            tmpValue = 0;
            Scheduler.Instance.RemoveUpdator(ShowSoldierCD);
            _ItemStatus = EFightViewSoldierStatus.esoNormal;
            GuideManager.Instance.CheckTrigger(GuideTrigger.SoldierCallCD);
        }
        Spt_Mask.fillAmount = tmpValue;
    }

    private void ShowStatus()
    {
        _CDTime = 0;
        _ClickTime = 0;
        int tmpStarCount = 0;
        int tmpInitPos = 0;

        if (fightSoldierInfo == null)
            return;
        if (fightSoldierInfo.Att == null)
            return;

        _CDTime = CommonFunction.GetSecondTimeByMilliSecond(fightSoldierInfo.Att.coolLimit);
        tmpStarCount = fightSoldierInfo.Att.Star;
        tmpInitPos = -(tmpStarCount - 1) * 7;

        //背景图//
        Spt_BG.gameObject.SetActive(true);
        //士兵图标//
        CommonFunction.SetSpriteName(Spt_Icon, fightSoldierInfo.Att.Icon);
        //士兵职业//
        CommonFunction.SetSpriteName(Spt_Career, CommonFunction.GetSoldierTypeIcon(fightSoldierInfo.Att.Career));
        Spt_Career.MakePixelPerfect();
        //Spt_Career.gameObject.SetActive(true);
        //遮罩//
        if ((_FightType == EFightType.eftMain) || (_FightType == EFightType.eftActivity) || (_FightType == EFightType.eftNewGuide) || (_FightType == EFightType.eftEndless))
        {
            Spt_Mask.type = UIBasicSprite.Type.Filled;
            Spt_Mask.fillAmount = 0;
        }
        //品质框//
        CommonFunction.SetQualitySprite(Spt_Quality, fightSoldierInfo.Att.quality, Spt_IconBG);
        Spt_Quality.gameObject.SetActive(true);
        Spt_IconBG.gameObject.SetActive(true);
        //星级//
        if ((_FightType == EFightType.eftMain) || (_FightType == EFightType.eftActivity) || (_FightType == EFightType.eftNewGuide) ||
            (_FightType == EFightType.eftEndless) || (_FightType == EFightType.eftExpedition) || (_FightType == EFightType.eftUnion) ||
            (_FightType == EFightType.eftCaptureTerritory) || (_FightType == EFightType.eftCrossServerWar))
        {
            Spt_Star.gameObject.SetActive(false);
            for (int i = 0; i < tmpStarCount; i++)
            {
                if (i < _StarList.Count)
                {
                    _StarList[i].transform.localPosition = new Vector3(tmpInitPos + i * 14, -40, 0);
                    _StarList[i].gameObject.SetActive(true);
                }
                else
                {
                    GameObject tmpStar = CommonFunction.InstantiateObject(Spt_Star.gameObject, Spt_Star.transform.parent);
                    if (tmpStar == null)
                        continue;
                    tmpStar.transform.localPosition = new Vector3(tmpInitPos + i * 14, -40, 0);
                    tmpStar.gameObject.SetActive(true);
                    _StarList.Add(tmpStar.transform);
                }
            }
        }
        //能量//
        if ((_FightType == EFightType.eftMain) || (_FightType == EFightType.eftActivity) || (_FightType == EFightType.eftNewGuide) || (_FightType == EFightType.eftEndless))
        {
            Spt_PowerBG.gameObject.SetActive(true);
            Lbl_Power.text = fightSoldierInfo.Att.call_energy.ToString();
        }
        ////等级//
        //Lbl_Level.text = fightSoldierInfo.Level.ToString();
        //数量//
        ShowSoldierNum();
        //锁定图标//
        if ((_FightType == EFightType.eftExpedition) || (_FightType == EFightType.eftPVP) || (_FightType == EFightType.eftSlave) ||
            (_FightType == EFightType.eftUnion) || (_FightType == EFightType.eftCaptureTerritory) || (_FightType == EFightType.eftCrossServerWar) ||
            (_FightType == EFightType.eftServerHegemony) || (_FightType == EFightType.eftQualifying))
        {
            _ItemStatus = EFightViewSoldierStatus.esoLock;
            return;
        }
        _ItemStatus = EFightViewSoldierStatus.esoNormal;
    }

    /// <summary>
    /// 显示数量
    /// </summary>
    private void ShowSoldierNum()
    {
        if ((_FightType == EFightType.eftExpedition) || (_FightType == EFightType.eftPVP) || (_FightType == EFightType.eftSlave) ||
            (_FightType == EFightType.eftUnion) || (_FightType == EFightType.eftCaptureTerritory) || (_FightType == EFightType.eftCrossServerWar) ||
            (_FightType == EFightType.eftServerHegemony) || (_FightType == EFightType.eftQualifying))
        {
            Lbl_Num.text = string.Format("X{0}", curSoldierNum);
        }
        else
        {
            Lbl_Num.text = string.Format("{0}/{1}", curSoldierNum, maxSoldierNum);
        }
    }

    /// <summary>
    /// 刷新单个士兵按钮显示
    /// </summary>
    /// <param name="vSoldierInfo">士兵属性</param>
    public void RefreshInfo_ShowStatus(Soldier vSoldierInfo, EFightCamp vFightCamp, int vSoldierNum = 0)
    {
        InitStatus();
        if (vSoldierInfo == null)
            return;
        fightCamp = vFightCamp;
        fightSoldierInfo = vSoldierInfo;
        PreLoadResource();
        _FightType = pFightViewController.Get_FightType;
        if ((fightSoldierInfo != null) && (fightSoldierInfo.Att != null))
        {
            if (vSoldierInfo.Att.limitNum <= 0)
                maxSoldierNum = 5;
            else
                maxSoldierNum = vSoldierInfo.Att.limitNum;
        }
        curSoldierNum = vSoldierNum;
        ShowStatus();
        InvokeRepeating("CheckEnergyIsEnough", 0, 0.1f);

        this.transform.GetComponent<BoxCollider>().enabled = true;
    }

    public void UpdateAutoOrder(bool showOrder, byte order = 0)
    {
        if (Lbl_AutoOrder.enabled == showOrder)
        {
            return;
        }
        Lbl_AutoOrder.enabled = showOrder;
        Lbl_AutoOrder.text = order.ToString();
    }

    /// <summary>
    /// 初始化显示
    /// </summary>
    public void InitStatus()
    {
        Spt_BG.gameObject.SetActive(false);
        Spt_Icon.spriteName = string.Empty;
        Spt_Mask.gameObject.SetActive(true);
        Spt_Mask.type = UIBasicSprite.Type.Sliced;
        Spt_Star.gameObject.SetActive(false);
        Lbl_Level.text = string.Empty;
        Spt_Lock.gameObject.SetActive(false);
        Lbl_Num.text = string.Empty;
        Spt_PowerBG.gameObject.SetActive(false);
        Lbl_Power.text = string.Empty;
        Spt_Quality.gameObject.SetActive(false);
        Spt_IconBG.gameObject.SetActive(false);
        Spt_Career.gameObject.SetActive(false);

        for (int i = 0; i < _StarList.Count; i++)
        {
            if (_StarList[i] == null)
                continue;
            _StarList[i].gameObject.SetActive(false);
        }

        _ItemStatus = EFightViewSoldierStatus.esoNormal;
        _PreStatus = EFightViewSoldierStatus.esoNormal;
        fightSoldierInfo = null;
        Obj_SoldierEffect.gameObject.SetActive(false);
        maxSoldierNum = 0;
        curSoldierNum = 0;
        if (colorLevel == Color.white)
            colorLevel = Lbl_Level.color;
        if (colorNum == Color.white)
            colorNum = Lbl_Num.color;
        if (colorPower == Color.white)
            colorPower = Lbl_Power.color;
        UpdateAutoOrder(false);
    }

    /// <summary>
    /// 点击士兵事件
    /// </summary>
    /// <param name="vObj"></param>
    public void ButtonEvent_SkillComp(GameObject vObj)
    {
        if (SceneManager.Instance.Get_CurScene.Get_SceneStatus != ESceneStatus.essNormal)
            return;
        if (_ItemStatus != EFightViewSoldierStatus.esoNormal)
            return;
        if ((pFightViewController == null) || (fightSoldierInfo == null) || (fightSoldierInfo.Att == null))
            return;
        if (pFightViewController.Get_CurValue_Energy < fightSoldierInfo.Att.call_energy)
            return;
        if (CheckIsFull())
        {
            curSoldierNum = maxSoldierNum;
            return;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(fightSoldierInfo.Att.Music, vObj.transform));
        //if (!isAutoFight)
        //    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, vObj.transform));
        curSoldierNum += 1;
        ShowSoldierNum();

        //显示CD//
        _ClickTime = Time.time;
        _PauseTime = 0;
        _ItemStatus = EFightViewSoldierStatus.esoCD;
        curFightSpeed = SceneManager.Instance.Get_FightSpeed;
        showCDTime = 0;
        Scheduler.Instance.AddUpdator(ShowSoldierCD);
        //创建士兵//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(fightSoldierInfo.showInfoSoldier, fightSoldierInfo.uId,
                SceneManager.Instance.Get_CurScene.Get_OtherIndex, ERoleType.ertSoldier,
                EHeroGender.ehgNone, EFightCamp.efcSelf, _FightType));
        //通知扣除能量//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerEnergy, -fightSoldierInfo.Att.call_energy);
        GuideManager.Instance.CheckTrigger(GuideTrigger.FightingCallSoldier);
        isCanCallSoldier = true;
    }

    /// <summary>
    /// 检测是否能够召唤士兵
    /// </summary>
    /// <returns></returns>
    public bool CheckIsCanCallSoldier()
    {
        isAutoFight = true;
        isCanCallSoldier = false;
        ButtonEvent_SkillComp(this.gameObject);
        isAutoFight = false;
        return isCanCallSoldier;
    }

    /// <summary>
    /// 检测是否达到召唤上限
    /// </summary>
    /// <returns></returns>
    public bool CheckIsFull()
    {
        return curSoldierNum >= maxSoldierNum;
    }

    /// <summary>
    /// 预加载武将资源
    /// </summary>
    private void PreLoadResource()
    {
        PreLoadResource_Soldier();
        PreLoadResource_Skill();
        PreLoadResource_Trajectory();
    }
    public void PreLoadResource_Soldier()
    {
        if ((fightSoldierInfo != null) && (fightSoldierInfo.Att != null))
        {
            ResourceLoadManager.Instance.LoadCharacter(fightSoldierInfo.Att.Animation, ResourceLoadType.AssetBundle, (obj) =>
            {
                if (obj != null)
                {
                    AloneObjectCache.Instance.LoadGameObject(obj, (go) =>
                    {
                        AloneObjectCache.Instance.FreeObject(go);
                    }, "", 5);
                }
            });
        }
    }
    public void PreLoadResource_Trajectory()
    {
        if ((fightSoldierInfo != null) && (fightSoldierInfo.Att != null))
        {
            EffectObjectCache.Instance.LoadGameObject(fightSoldierInfo.Att.trajectory, (go) =>
            {
                EffectObjectCache.Instance.FreeObject(go);
            }, fightSoldierInfo.Att.trajectory, 10);
        }
    }
    public void PreLoadResource_Skill()
    {
        if ((fightSoldierInfo != null) && (fightSoldierInfo._skillsDepot != null) && (fightSoldierInfo._skillsDepot._skillsList != null))
        {
            foreach (Skill tmpSkill in fightSoldierInfo._skillsDepot._skillsList)
            {
                if (tmpSkill != null)
                {
                    tmpSkill.CacheEffect();
                }
            }
        }
    }
}

public class DeleteSingleSoldierInfo
{
    public uint ID;
    public EFightCamp FightCamp;

    public DeleteSingleSoldierInfo(uint vID, EFightCamp vFightCamp)
    {
        ID = vID;
        FightCamp = vFightCamp;
    }
}