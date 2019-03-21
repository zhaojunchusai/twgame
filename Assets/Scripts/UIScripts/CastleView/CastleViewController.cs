using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class CastleViewController : UIBase
{

    private const int MAX_SHOOTER_COUNT = 3;
    private const float INIT_SHOOTER_POSY = -160;
    private const float DISTANCE_SHOOTER = 140;
    public GameObject Go_UPCastleEffect;

    public CastleView view;
    private List<CastleViewItem_Shooter> ShooterInfoList = new List<CastleViewItem_Shooter>();



    public override void Initialize()
    {
        if (view == null)
        {
            view = new CastleView();
            view.Initialize();
        }

        BtnEventBinding();
        Refresh_ShowStatus();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenCastleView);

    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_CASTLEVIEW);
    }

    public override void Uninitialize()
    {
        if ((view != null) && ((view.Tex_Castle_Icon != null)))
        {
            view.Tex_Castle_Icon.mainTexture = null;
        }
        if (ShooterInfoList != null)
        {
            for (int i = 0; i < ShooterInfoList.Count; i++)
            {
                ShooterInfoList[i].DeleteRoleInfo();
            }
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        ShooterInfoList.Clear();
    }

    public override UIBoundary GetUIBoundary()
    {
        return view.Boundary;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Back.gameObject).onClick = ButtonEvent_Back;
        UIEventListener.Get(view.Btn_Castle_UpLV.gameObject).onClick = ButtonEvent_Castle_UpLV;
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Back(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CASTLEVIEW);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_HEROATT);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

    }

    /// <summary>
    /// 升级城堡
    /// </summary>
    /// <param name="btn"></param>
    private float preClickTime = 0;
    public void ButtonEvent_Castle_UpLV(GameObject btn)
    {
        if (Time.time - preClickTime < 0.2f)
            return;
        preClickTime = Time.time;
        CastleModule.Instance.SendUpgradeCastleReq(PlayerData.Instance.mCastleInfo.mID);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));

    }

    /// <summary>
    /// 设置城堡升级按钮状态
    /// </summary>
    /// <param name="vIsGray"></param>
    private void SetBtnUpLVStatus(bool vIsGray)
    {
        CommonFunction.UpdateWidgetGray(view.Spt_BtnCastle_UpLVUpLV_BG, vIsGray);
        if (vIsGray)
            CommonFunction.SetLabelColor_I(view.Lbl_BtnCastle_UpLVUpLV_Title, 67, 67, 67, 255, 162, 162, 162, 255);
        else
            CommonFunction.SetLabelColor_I(view.Lbl_BtnCastle_UpLVUpLV_Title, 111, 52, 14, 255, 234, 201, 93, 255);

        view.Btn_Castle_UpLV.GetComponent<BoxCollider>().enabled = !vIsGray;
    }

    /// <summary>
    /// 初始化界面显示
    /// </summary>
    private void Init_ShowStatus()
    {
        Init_Castle();
        Init_Shooter();
    }

    /// <summary>
    /// 刷新显示
    /// </summary>
    public void Refresh_ShowStatus()
    {
        Init_ShowStatus();
        Refresh_Castle();
        Refresh_Shooter();
    }

    /// <summary>
    /// 初始化城堡显示
    /// </summary>
    private void Init_Castle()
    {
        view.Spt_Castle_Icon_BG.gameObject.SetActive(false);
        view.Tex_Castle_Icon.gameObject.SetActive(false);
        view.Lbl_Castle_Price_Value.text = string.Empty;
        view.Lbl_Castle_Level_Value.text = string.Empty;
        view.Spt_Castle_Level_MAX.gameObject.SetActive(false);
        view.Lbl_Castle_HP_Value.text = string.Empty;
        view.Lbl_Castle_HP_Next.text = string.Empty;
        SetBtnUpLVStatus(true);
    }

    /// <summary>
    /// 初始化射手显示
    /// </summary>
    private void Init_Shooter()
    {
        for (int i = 0; i < ShooterInfoList.Count; i++)
            ShooterInfoList[i].DestroyShooter();
        ShooterInfoList.Clear();

        if (view.Obj_Shooter_Item == null)
            return;
        view.Obj_Shooter_Item.gameObject.SetActive(false);

        for (int i = 0; i < MAX_SHOOTER_COUNT; i++)
        {
            GameObject tmpShooter = CommonFunction.InstantiateObject(view.Obj_Shooter_Item.gameObject, view.Obj_Shooter_Item.parent);
            tmpShooter.transform.localPosition = new Vector3(0, INIT_SHOOTER_POSY + i * DISTANCE_SHOOTER, 0);
            tmpShooter.transform.localScale = Vector3.one;
            tmpShooter.transform.name = string.Format("{0}_{1}", view.Obj_Shooter_Item.name, i);
            tmpShooter.SetActive(true);
            CastleViewItem_Shooter tmpShooterOperate = new CastleViewItem_Shooter(tmpShooter.transform);
            ShooterInfoList.Add(tmpShooterOperate);
        }
    }

    /// <summary>
    /// 刷新城堡显示
    /// </summary>
    private void Refresh_Castle()
    {
        if (PlayerData.Instance.mCastleInfo == null)
            return;
        CastleAttributeInfo tmpCastleInfo = ConfigManager.Instance.mCastleConfig.FindByID(PlayerData.Instance.mCastleInfo.mID);
        if (tmpCastleInfo == null)
            return;

        //背景//
        view.Spt_Castle_Icon_BG.gameObject.SetActive(true);

        //图标//
        ResourceLoadManager.Instance.LoadAloneImage(CommonFunction.GetCastleFileName(), (texture) =>
        {
            if (texture != null)
            {
                view.Tex_Castle_Icon.mainTexture = texture;
                view.Tex_Castle_Icon.MakePixelPerfect();
                view.Tex_Castle_Icon.gameObject.SetActive(true);
            }
        });

        //价格//
        MaterialsBagAttributeInfo tmpMaterialInfo = null;
        if (tmpCastleInfo.levelup_cost.ContainsKey(PlayerData.Instance.mCastleInfo.mLevel - tmpCastleInfo.level))
            tmpMaterialInfo = ConfigManager.Instance.mMaterialsBagData.FindBynId(tmpCastleInfo.levelup_cost[PlayerData.Instance.mCastleInfo.mLevel - tmpCastleInfo.level]);
        if (tmpMaterialInfo != null)
            view.Lbl_Castle_Price_Value.text =CommonFunction.GetTenThousandUnit(tmpMaterialInfo.Cost,10000);

        //等级 HP成长//
        if ((PlayerData.Instance.mCastleInfo.mLevel == tmpCastleInfo.level_limit) && (tmpCastleInfo.suffix_id == 0))
        {
            view.Lbl_Castle_Level_Value.text = string.Empty;
            view.Spt_Castle_Level_MAX.gameObject.SetActive(true);
            view.Lbl_Castle_HP_Next.text = string.Empty;
        }
        else
        {
            view.Lbl_Castle_Level_Value.text = PlayerData.Instance.mCastleInfo.mLevel.ToString();
            view.Spt_Castle_Level_MAX.gameObject.SetActive(false);
            view.Lbl_Castle_HP_Next.text = string.Format("+{0}", PlayerData.Instance.mCastleInfo.mHP_Grow);
        }

        //HP//
        view.Lbl_Castle_HP_Value.text = PlayerData.Instance.mCastleInfo.mAttribute.HP.ToString();

        //按钮//
        SetBtnUpLVStatus(true);
        if (PlayerData.Instance.mCastleInfo.mLevel >= PlayerData.Instance._Level)
            return;
        if ((PlayerData.Instance.mCastleInfo.mLevel == tmpCastleInfo.level_limit) && (tmpCastleInfo.suffix_id == 0))
            return;
        SetBtnUpLVStatus(false);
    }

    /// <summary>
    /// 刷新射手显示
    /// </summary>
    private void Refresh_Shooter()
    {
        if (PlayerData.Instance.mShooterList == null)
            return;
        if (ShooterInfoList == null)
            return;

        for (int i = 0; i < ShooterInfoList.Count; i++)
        {
            if (i < PlayerData.Instance.mShooterList.Count)
                ShooterInfoList[i].RefreshUIStatus(PlayerData.Instance.mShooterList[i]);
            else
                ShooterInfoList[i].InitItemShowStatus();
        }
    }

    /// <summary>
    /// 升级城堡
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveUpgradeCastleInfo()
    {
        Refresh_Castle();
        PlayUPCastleEffect();///升级城堡特效
        for (int i = 0; i < ShooterInfoList.Count; i++)
        {
            ShooterInfoList[i].RefreshBtnStatus();
        }
    }

    /// <summary>
    /// 刷新单个射手信息
    /// </summary>
    /// <param name="vID"></param>
    /// <param name="vShooterInfo"></param>
    public void ReceiveSingleShooterInfo(uint vID, SingleShooterInfo vShooterInfo)
    {
        if (vShooterInfo == null)
            return;
        for (int i = 0; i < ShooterInfoList.Count; i++)
        {
            if (ShooterInfoList[i].ShooterID == vID)
            {
                ShooterInfoList[i].RefreshUIStatus(vShooterInfo);
            }
        }
    }

    //===========================================================================//
    public void PlayEffect(int id)
    {
        //1
        //2 Play

        for (int i = 0; i < ShooterInfoList.Count; i++)
        {
            if (ShooterInfoList[i].ShooterID == id)
            {
                ShooterInfoList[i].PlayShooterOperateEffect();
            }
        }

    }
    SetParticleSortingLayer Per;
    public void PlayUPCastleEffect()//升级城堡
    {
        if (Go_UPCastleEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_UPCASTLE, (GameObject gb) => { Go_UPCastleEffect = gb; });

        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_UPCastleEffect, view.EffectPoint.transform);
        Per = go.GetComponent<SetParticleSortingLayer>();
        CreatUpCastleLabel();
    }
    public void CreatUpCastleLabel()
    {
        GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.EffectPoint.transform);
        CommonFunction.ResetParticlePanelOrder(LabelObj, view._uiRoot, Per);
        IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
        LabelItem.UpdateItem(ConstString.hp_max + view.Lbl_Castle_HP_Next.text);
        LabelObj.SetActive(true);
    }
}
