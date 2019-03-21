using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

/// <summary>
/// 战斗掉落物品信息
/// </summary>
public class DropItemInfo
{
    public string Icon;
    public int Quality;
    public int Num;
    public bool IsSoul;
    public DropItemInfo(string vIcon, int vQuality, int vNum = 1, bool vIsSoul = false)
    {
        Icon = vIcon;
        Quality = vQuality;
        Num = vNum;
        IsSoul = vIsSoul;
    }
}


/// <summary>
/// 战斗结算
/// </summary>
public class FightViewPanel_Result : Singleton<FightViewPanel_Result>
{
    /// <summary>
    /// 战斗结算-成功
    /// </summary>
    private const int FIGHT_RESULT_VICTORY = 1;


    /// <summary>
    /// 战斗界面
    /// </summary>
    private FightView fightView;
    /// <summary>
    /// 物品列表-主线/活动
    /// </summary>
    private List<GameObject> activityItemList = new List<GameObject>();
    /// <summary>
    /// 物品列表-无尽
    /// </summary>
    private List<GameObject> endlessItemList = new List<GameObject>();
    /// <summary>
    /// 物品列表-远征
    /// </summary>
    private List<GameObject> expeditionItemList = new List<GameObject>();
    /// <summary>
    /// 失败原因
    /// </summary>
    private string failReason;
    /// <summary>
    /// 战斗场景类型
    /// </summary>
    private EFightType fightType;
    /// <summary>
    /// 音效名字
    /// </summary>
    private string audioName;
    /// <summary>
    /// 结算数据
    /// </summary>
    private object resultData;
    /// <summary>
    /// 战斗配置表ID
    /// </summary>
    private uint fightID;
    /// <summary>
    /// 是否有邮件
    /// </summary>
    private bool isHaveMail = false;
    /// <summary>
    /// 延时操作函数列表
    /// </summary>
    private List<Assets.Script.Common.Scheduler.OnScheduler> listDelayFunc = new List<Scheduler.OnScheduler>();
    /// <summary>
    /// 是否跳过演示
    /// </summary>
    private bool isSkip = false;


    public void Initialize(FightView vFightView)
    {
        fightView = vFightView;
        InitPanel();
    }

    public void Uninitialize()
    { }

    public void Destroy()
    {
        fightView = null;
        activityItemList.Clear();
        endlessItemList.Clear();
        expeditionItemList.Clear();
        listDelayFunc.Clear();
    }


    /// <summary>
    /// 初始化显示
    /// </summary>
    public void InitPanel()
    {
        ReSetTweenInfos(fightView.UIPanel_FightResult.transform);

        fightView.Title_Alpha.ResetToBeginning();
        fightView.Title_Position.ResetToBeginning();
        fightView.Title_Rotation.ResetToBeginning();
        fightView.Title_Scale.ResetToBeginning();
        fightView.TitleWin_Position.ResetToBeginning();
        fightView.TitleWin_Alpha.ResetToBeginning();
        fightView.TitleWin_Rotation.ResetToBeginning();
        fightView.TitleWin_Scale.ResetToBeginning();
        fightView.FailHint_Position.ResetToBeginning();
        fightView.FailItem0_Scale.ResetToBeginning();
        fightView.FailItem1_Scale.ResetToBeginning();
        fightView.FailItem2_Scale.ResetToBeginning();
        fightView.FailItem3_Scale.ResetToBeginning();
        fightView.FailItem4_Scale.ResetToBeginning();

        isHaveMail = false;
        isSkip = false;
        SetClickCloseStatus(false);
        ReSetButtonStatus(false);
        fightView.SA_Shengli.gameObject.SetActive(false);
        fightView.Obj_Result_Activity.gameObject.SetActive(false);
        fightView.Obj_Result_Endless.gameObject.SetActive(false);
        fightView.Obj_Result_Expedition.gameObject.SetActive(false);
        fightView.Obj_Result_PVP.gameObject.SetActive(false);
        fightView.Obj_Result_Slave.gameObject.SetActive(false);
        fightView.Obj_Result_Union.gameObject.SetActive(false);
        fightView.Obj_Result_Fail.gameObject.SetActive(false);
        fightView.Obj_Result_CaptureTerritory.gameObject.SetActive(false);
        fightView.Obj_Result_CrossServerWar.gameObject.SetActive(false);
        fightView.Obj_Result_ServerHegemony.gameObject.SetActive(false);
        fightView.Obj_Result_Qualifying.gameObject.SetActive(false);
        fightView.UIPanel_FightResult.gameObject.SetActive(false);
        fightView.Lbl_Fail_Hint_Info.text = string.Empty;
        if (listDelayFunc == null)
        { listDelayFunc = new List<Scheduler.OnScheduler>(); }
        else
        { listDelayFunc.Clear(); }
    }

    public void InitTitleTween()
    {
        fightView.Title_Alpha.PlayForward();
        fightView.Title_Position.PlayForward();
        fightView.Title_Rotation.PlayForward();
        fightView.Title_Scale.PlayForward();
        fightView.TitleWin_Position.ResetToBeginning();
        fightView.TitleWin_Alpha.ResetToBeginning();
        fightView.TitleWin_Rotation.ResetToBeginning();
        fightView.TitleWin_Scale.ResetToBeginning();
    }

    public void InitOpenFailTween()
    {
        fightView.FailHint_Position.PlayForward();
        fightView.FailItem0_Scale.PlayForward();
        fightView.FailItem1_Scale.PlayForward();
        fightView.FailItem2_Scale.PlayForward();
        fightView.FailItem3_Scale.PlayForward();
        fightView.FailItem4_Scale.PlayForward();
    }

    /// <summary>
    /// 设置点击状态
    /// </summary>
    /// <param name="vStatus">状态</param>
    private void SetClickCloseStatus(bool vStatus)
    {
        fightView.Btn_Result_Hint.gameObject.SetActive(vStatus);
    }
    /// <summary>
    /// 设置主线副本结算按钮状态
    /// </summary>
    /// <param name="vFightResult"></param>
    /// <param name="vID"></param>
    /// <param name="vIsBoss"></param>
    private void SetActivityBtnStatus(int vFightResult, uint vID, bool vIsBoss)
    {
        bool tmpIsOpenNext = UISystem.Instance.FightView.IsOpenNextBtn();
        fightView.Btn_Activity_ReStart.gameObject.SetActive(true);
        fightView.Btn_Activity_Exit.gameObject.SetActive(true);
        fightView.Btn_Activity_Next.gameObject.SetActive(true);
        //位置[-100, 100][-150, 0, 150]//
        if (fightType == EFightType.eftActivity)
        {
            fightView.Btn_Activity_ReStart.transform.localPosition = new Vector3(-100, -112, 0);
            fightView.Btn_Activity_Exit.transform.localPosition = new Vector3(100, -112, 0);
            fightView.Btn_Activity_Next.gameObject.SetActive(false);
        }
        else
        {
            if (!vIsBoss)
            {//非BOSS关卡//
                if (vFightResult == FIGHT_RESULT_VICTORY)
                {
                    fightView.Btn_Activity_ReStart.gameObject.SetActive(false);
                    if (tmpIsOpenNext)
                    {
                        foreach (KeyValuePair<uint, uint> tmpInfo in FightRelatedModule.Instance.DicSpeFightResult)
                        {
                            if (vID != tmpInfo.Key)
                                continue;
                            if (GuideManager.Instance.IsGuideFinish(tmpInfo.Value))
                                break;
                            fightView.Btn_Activity_Exit.transform.localPosition = new Vector3(0, -112, 0);
                            fightView.Btn_Activity_Next.gameObject.SetActive(false);
                            return;
                        }
                        fightView.Btn_Activity_Exit.transform.localPosition = new Vector3(-100, -112, 0);
                        fightView.Btn_Activity_Next.transform.localPosition = new Vector3(100, -112, 0);
                    }
                    else
                    {
                        fightView.Btn_Activity_Exit.transform.localPosition = new Vector3(0, -112, 0);
                        fightView.Btn_Activity_Next.gameObject.SetActive(false);
                    }
                }
                else
                {
                    fightView.Btn_Activity_Next.gameObject.SetActive(false);
                    foreach (KeyValuePair<uint, uint> tmpInfo in FightRelatedModule.Instance.DicSpeTutorial)
                    {
                        if (vID != tmpInfo.Key)
                            continue;
                        if (GuideManager.Instance.IsGuideFinish(tmpInfo.Value))
                            break;
                        fightView.Btn_Activity_ReStart.transform.localPosition = new Vector3(0, -112, 0);
                        fightView.Btn_Activity_Exit.gameObject.SetActive(false);
                        return;
                    }
                    if (vID == GlobalConst.FIRST_STAGE_ID)
                    {
                        fightView.Btn_Activity_ReStart.transform.localPosition = new Vector3(0, -112, 0);
                        fightView.Btn_Activity_Exit.gameObject.SetActive(false);
                    }
                    else
                    {
                        fightView.Btn_Activity_ReStart.transform.localPosition = new Vector3(-100, -112, 0);
                        fightView.Btn_Activity_Exit.transform.localPosition = new Vector3(100, -112, 0);
                    }
                }
            }
            else
            {//BOSS关卡//
                if ((tmpIsOpenNext) && (vFightResult == FIGHT_RESULT_VICTORY))
                {
                    foreach (KeyValuePair<uint, uint> tmpInfo in FightRelatedModule.Instance.DicSpeFightResult)
                    {
                        if (vID != tmpInfo.Key)
                            continue;
                        if (GuideManager.Instance.IsGuideFinish(tmpInfo.Value))
                            break;
                        fightView.Btn_Activity_Exit.transform.localPosition = new Vector3(0, -112, 0);
                        fightView.Btn_Activity_ReStart.gameObject.SetActive(false);
                        fightView.Btn_Activity_Next.gameObject.SetActive(false);
                        return;
                    }
                    fightView.Btn_Activity_ReStart.transform.localPosition = new Vector3(-150, -112, 0);
                    fightView.Btn_Activity_Exit.transform.localPosition = new Vector3(0, -112, 0);
                    fightView.Btn_Activity_Next.transform.localPosition = new Vector3(150, -112, 0);
                }
                else
                {
                    if (vFightResult != FIGHT_RESULT_VICTORY)
                    {
                        foreach (KeyValuePair<uint, uint> tmpInfo in FightRelatedModule.Instance.DicSpeTutorial)
                        {
                            if (vID != tmpInfo.Key)
                                continue;
                            if (GuideManager.Instance.IsGuideFinish(tmpInfo.Value))
                                break;
                            fightView.Btn_Activity_ReStart.transform.localPosition = new Vector3(0, -112, 0);
                            fightView.Btn_Activity_Exit.gameObject.SetActive(false);
                            fightView.Btn_Activity_Next.gameObject.SetActive(false);
                            return;
                        }
                    }

                    fightView.Btn_Activity_ReStart.transform.localPosition = new Vector3(-100, -112, 0);
                    fightView.Btn_Activity_Exit.transform.localPosition = new Vector3(100, -112, 0);
                    fightView.Btn_Activity_Next.gameObject.SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// 获取掉落物品
    /// </summary>
    /// <param name="vDropData">掉落数据</param>
    /// <param name="vLblGold">显示金币组件-可忽略</param>
    /// <returns>掉落物品路径列表</returns>
    private List<DropItemInfo> GetIconListByDrop(fogs.proto.msg.DropList vDropData, UILabel vLblGold = null, UILabel vLblToken = null)
    {
        List<DropItemInfo> tmpResult = new List<DropItemInfo>();
        if (vDropData == null)
            return tmpResult;

        //判断是否需要提示邮件物品//
        if ((vDropData.mail_list != null) && (vDropData.mail_list.Count > 0))
        {
            isHaveMail = true;
            for (int i = 0; i < vDropData.mail_list.Count; i++)
            {
                if (vDropData.mail_list[i] == null)
                    continue;
                tmpResult.Add(new DropItemInfo(CommonFunction.GetIconNameByID(vDropData.mail_list[i].id), CommonFunction.GetQualityByID(vDropData.mail_list[i].id)));
            }
        }

        //显示掉落物品//
        if (vDropData.special_list != null)
        {
            for (int i = 0; i < vDropData.special_list.Count; i++)
            {
                fogs.proto.msg.ItemInfo tmpInfo = vDropData.special_list[i];
                if (tmpInfo == null)
                    continue;
                if (CommonFunction.IsCoinID(tmpInfo.id))
                {
                    if (vLblGold != null)
                        vLblGold.text = tmpInfo.change_num.ToString();
                    continue;
                }
                if (CommonFunction.IsUnionTokenID(tmpInfo.id))
                {
                    if (vLblToken != null)
                        vLblToken.text = tmpInfo.change_num.ToString();
                }
            }
        }

        if (vDropData.equip_list != null)
        {
            for (int i = 0; i < vDropData.equip_list.Count; i++)
            {
                EquipAttributeInfo tmpEquip = ConfigManager.Instance.mEquipData.FindById(vDropData.equip_list[i].id);
                if (tmpEquip == null)
                    continue;
                tmpResult.Add(new DropItemInfo(tmpEquip.icon, tmpEquip.quality));
            }
        }

        if (vDropData.item_list != null)
        {
            for (int i = 0; i < vDropData.item_list.Count; i++)
            {
                ItemInfo tmpItem = ConfigManager.Instance.mItemData.GetItemInfoByID(vDropData.item_list[i].id);
                if (tmpItem == null)
                    continue;
                tmpResult.Add(new DropItemInfo(tmpItem.icon, tmpItem.quality, vDropData.item_list[i].change_num, (tmpItem.type == (int)ItemTypeEnum.SoldierChip)));
            }
        }

        if (vDropData.soldier_list != null)
        {
            for (int i = 0; i < vDropData.soldier_list.Count; i++)
            {
                SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(vDropData.soldier_list[i].id);
                if (tmpSoldier == null)
                    continue;
                tmpResult.Add(new DropItemInfo(tmpSoldier.Icon, tmpSoldier.quality));
            }
        }

        return tmpResult;
    }

    /// <summary>
    /// 主线 活动
    /// </summary>
    private void Init_Activity()
    {
        CommonFunction.SetSpriteName(fightView.Spt_Activity_Star_0, GlobalConst.SpriteName.SPRITE_NAME_STAR_FAILURE);
        CommonFunction.SetSpriteName(fightView.Spt_Activity_Star_1, GlobalConst.SpriteName.SPRITE_NAME_STAR_FAILURE);
        CommonFunction.SetSpriteName(fightView.Spt_Activity_Star_2, GlobalConst.SpriteName.SPRITE_NAME_STAR_FAILURE);
        fightView.Spt_Activity_Star_0.gameObject.SetActive(false);
        fightView.Spt_Activity_Star_1.gameObject.SetActive(false);
        fightView.Spt_Activity_Star_2.gameObject.SetActive(false);
        fightView.Spt_Activity_Star_0.color = new Color(1, 1, 1, 0);
        fightView.Spt_Activity_Star_1.color = new Color(1, 1, 1, 0);
        fightView.Spt_Activity_Star_2.color = new Color(1, 1, 1, 0);
        fightView.Exp_Position.gameObject.SetActive(false);
        fightView.Gold_Position.gameObject.SetActive(false);

        fightView.Spt_Activity_ItemBG.gameObject.SetActive(false);
        for (int i = 0; i < activityItemList.Count; i++)
            GameObject.Destroy(activityItemList[i]);
        activityItemList.Clear();

        fightView.Lbl_Activity_Hint.text = string.Empty;
        fightView.Lbl_Activity_Gold.text = "0";
        fightView.Lbl_Activity_EXP.text = "0";

        fightView.Gold_Position.gameObject.transform.localPosition = new Vector3(500, 22, 0);
        fightView.Exp_Position.gameObject.transform.localPosition = new Vector3(500, 22, 0);
        fightView.Restart_Scale.gameObject.transform.localScale = Vector3.zero;
        fightView.Exit_Scale.gameObject.transform.localScale = Vector3.zero;
        fightView.Next_Scale.gameObject.transform.localScale = Vector3.zero;

        fightView.Star0_Alpha.ResetToBeginning();
        fightView.Star0_Scale.ResetToBeginning();
        fightView.Star1_Alpha.ResetToBeginning();
        fightView.Star1_Scale.ResetToBeginning();
        fightView.Star2_Alpha.ResetToBeginning();
        fightView.Star2_Scale.ResetToBeginning();
        fightView.ItemBG_Scale.ResetToBeginning();
        fightView.Gold_Position.ResetToBeginning();
        fightView.Exp_Position.ResetToBeginning();
        fightView.Restart_Scale.ResetToBeginning();
        fightView.Exit_Scale.ResetToBeginning();
        fightView.Next_Scale.ResetToBeginning();
        fightView.Obj_Result_Activity.gameObject.SetActive(false);

        fightView.UIPanel_Activity_ItemList.GetComponent<UIScrollView>().ResetPosition();
    }
    public void Refresh_Activity(object vResultData, EFightType vFightType, uint vID)
    {
        InitPanel();
        Init_Activity();
        fightType = vFightType;
        DungeonRewardResp tmpResultData = (DungeonRewardResp)vResultData;
        if (tmpResultData == null)
            return;
        StageInfo tmpStageInfo = ConfigManager.Instance.mStageData.GetInfoByID(vID);
        if (tmpStageInfo == null)
            return;

        SetActivityBtnStatus(tmpResultData.fight_result, vID, tmpStageInfo.IsBoss);
        List<DropItemInfo> tmpDropList = new List<DropItemInfo>();
        Scheduler.Instance.AddTimer(3, false, DelayOpenButton);
        if (tmpResultData.fight_result != FIGHT_RESULT_VICTORY)
        {
            OpenActivity();
            Refresh_Fail();
            SetDelayFunc(2.5f, false, OpenFail);
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Lose));
            TalkingDataManager.Instance.OnFailed(vID.ToString(), failReason);
            return;
        }
        else
        {
            TalkingDataManager.Instance.OnCompleted(vID.ToString());
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Win));
        }
        fightView.Exp_Position.gameObject.SetActive(true);
        fightView.Gold_Position.gameObject.SetActive(true);
        ShowVictoryAndFailure(true);

        //三星条件//
        uint tmpStarCount = tmpResultData.star_level;
        if (tmpStarCount < 3)
        {
            fightView.Lbl_Activity_Hint.text = CommonFunction.GetFightMaxCondition(fightType, tmpStageInfo);
        }
        //战斗星级//
        if (tmpStarCount >= 1)
        {
            Main.Instance.StartCoroutine(DelayShowAudio(2.1f, GlobalConst.Sound.AUDIO_Star_1));
            CommonFunction.SetSpriteName(fightView.Spt_Activity_Star_0, GlobalConst.SpriteName.SPRITE_NAME_STAR_VICTORY);
        }
        if (tmpStarCount >= 2)
        {
            Main.Instance.StartCoroutine(DelayShowAudio(2.3f, GlobalConst.Sound.AUDIO_Star_2));
            CommonFunction.SetSpriteName(fightView.Spt_Activity_Star_1, GlobalConst.SpriteName.SPRITE_NAME_STAR_VICTORY);
        }
        if (tmpStarCount >= 3)
        {
            Main.Instance.StartCoroutine(DelayShowAudio(2.5f, GlobalConst.Sound.AUDIO_Star_3));
            CommonFunction.SetSpriteName(fightView.Spt_Activity_Star_2, GlobalConst.SpriteName.SPRITE_NAME_STAR_VICTORY);
        }
        fightView.Spt_Activity_Star_0.gameObject.SetActive(true);
        fightView.Spt_Activity_Star_1.gameObject.SetActive(true);
        fightView.Spt_Activity_Star_2.gameObject.SetActive(true);

        //掉落物品//
        tmpDropList = GetIconListByDrop(tmpResultData.drop_items, fightView.Lbl_Activity_Gold);

        //经验//
        if (tmpResultData.exp != null)
            fightView.Lbl_Activity_EXP.text = tmpResultData.exp.add_exp.ToString();

        //显示物品//
        int tmpDropListCount = tmpDropList.Count;
        int tmpInitPosX = 0;
        if (tmpDropListCount < 5)
        {
            tmpInitPosX = (5 - tmpDropListCount) * 50;
        }
        for (int i = 0; i < tmpDropListCount; i++)
        {
            if (i >= tmpDropListCount)
                continue;
            GameObject tmpItemObj = CommonFunction.InstantiateObject(fightView.Spt_Activity_ItemBG.gameObject, fightView.Grd_Activity_ItemGrid.transform);
            if (tmpItemObj == null)
                continue;
            CommonFunction.SetQualitySprite(tmpItemObj.transform.FindChild("Activity_ItemQuality").GetComponent<UISprite>(), tmpDropList[i].Quality, tmpItemObj.transform.GetComponent<UISprite>());
            CommonFunction.SetSpriteName(tmpItemObj.transform.FindChild("Activity_Item").GetComponent<UISprite>(), tmpDropList[i].Icon);
            tmpItemObj.transform.FindChild("Activity_ItemNum").GetComponent<UILabel>().text = tmpDropList[i].Num.ToString();
            tmpItemObj.transform.FindChild("Activity_ItemSoul").gameObject.SetActive(tmpDropList[i].IsSoul);
            tmpItemObj.gameObject.transform.localPosition = new Vector3(100 * i + tmpInitPosX, 0, 0);
            tmpItemObj.SetActive(true);
            activityItemList.Add(tmpItemObj);
        }

        fightView.UIPanel_Activity_ItemList.transform.localPosition = new Vector3(0, -42, 0);
        fightView.UIPanel_Activity_ItemList.clipOffset = new Vector3(0, 0);

        OpenActivity();
        ////更新数据//
        resultData = vResultData;
        fightID = vID;
        SetDelayFunc(2.4f, false, OpenUpLvPn);
        if (tmpResultData.fight_result == FIGHT_RESULT_VICTORY)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.FightOver);
        }
    }
    
    /// <summary>
    /// 新手引导
    /// </summary>
    /// <param name="vResultData"></param>
    /// <param name="vID"></param>
    public void Refresh_NewGuide(int vResultData, uint vID)
    {
        InitPanel();
        Init_Activity();
        fightType = EFightType.eftNewGuide;
        Scheduler.Instance.AddTimer(3, false, DelayOpenButton);
        SetActivityBtnStatus(vResultData, vID, false);

        Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Win));
        fightView.Exp_Position.gameObject.SetActive(true);
        fightView.Gold_Position.gameObject.SetActive(true);

        if (vResultData == FIGHT_RESULT_VICTORY)
            ShowVictoryAndFailure(true);
        else
            ShowVictoryAndFailure(false);

        //三星条件//
        fightView.Spt_Activity_Star_0.gameObject.SetActive(false);
        fightView.Spt_Activity_Star_1.gameObject.SetActive(false);
        fightView.Spt_Activity_Star_2.gameObject.SetActive(false);

        //掉落物品//
        if (fightView.Lbl_Activity_Gold != null)
            fightView.Lbl_Activity_Gold.text = "0";
        //经验//
        if (fightView.Lbl_Activity_EXP != null)
            fightView.Lbl_Activity_EXP.text = "0";
        fightView.UIPanel_Activity_ItemList.transform.localPosition = new Vector3(0, -42, 0);
        fightView.UIPanel_Activity_ItemList.clipOffset = new Vector3(0, 0);

        OpenActivity();
        //更新数据//
        if (vResultData == FIGHT_RESULT_VICTORY)
            GuideManager.Instance.CheckTrigger(GuideTrigger.FightOver);
    }
    public void Refresh_NewGuide(object vResultData, uint vID)
    {
        DungeonRewardResp tmpResultData = (DungeonRewardResp)vResultData;
        if (tmpResultData == null)
            return;
        Refresh_NewGuide(tmpResultData.fight_result, vID);
    }

    /// <summary>
    /// 无尽
    /// </summary>
    private void Init_Endless()
    {
        fightView.Spt_Endless_ItemBG.gameObject.SetActive(false);
        for (int i = 0; i < endlessItemList.Count; i++)
            GameObject.Destroy(endlessItemList[i]);
        endlessItemList.Clear();
        fightView.Lbl_Endless_EXP.text = "0";
        fightView.Lbl_Endless_Gold.text = "0";
        fightView.Lbl_Endless_Num.text = "0";
        fightView.Lbl_Endless_CurScore.text = "0";
        fightView.Lbl_Endless_MaxScore.text = "0";
        fightView.EndlessNum_Position.gameObject.transform.localPosition = new Vector3(500, 115, 0);
        fightView.Detail_Scale.gameObject.transform.localScale = Vector3.zero;

        fightView.EndlessCurScore_Scale.ResetToBeginning();
        fightView.EndlessExp_Scale.ResetToBeginning();
        fightView.EndlessGold_Scale.ResetToBeginning();
        fightView.EndlessMaxScore_Scale.ResetToBeginning();
        fightView.EndlessNum_Position.ResetToBeginning();
        fightView.Detail_Scale.ResetToBeginning();
        fightView.Obj_Result_Endless.gameObject.SetActive(false);

        fightView.UIPanel_Endless_ItemList.GetComponent<UIScrollView>().ResetPosition();
    }
    public void Refresh_Endless(EndlessDungeonRewardResp vResultData, uint vID, int vFightNum)
    {
        InitPanel();
        Init_Endless();
        fightType = EFightType.eftEndless;
        if (vResultData == null)
            return;
        Scheduler.Instance.AddTimer(3, false, DelayOpenButton);
        List<DropItemInfo> tmpDropList = new List<DropItemInfo>();

        if (vResultData.fight_result == FIGHT_RESULT_VICTORY)
        {
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Win));
            ShowVictoryAndFailure(true);
        }
        else
        {
            Refresh_FailReason();
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Lose));
            ShowVictoryAndFailure(false);
        }
        SetClickCloseStatus(true);

        //积分信息//
        fightView.Lbl_Endless_Num.text = vFightNum.ToString();
        fightView.Lbl_Endless_CurScore.text = vResultData.cur_grade.ToString();
        if (vResultData.info != null)
            fightView.Lbl_Endless_MaxScore.text = vResultData.info.high_grade.ToString();

        //掉落物品//
        tmpDropList = GetIconListByDrop(vResultData.drop_items, fightView.Lbl_Endless_Gold);

        //经验//
        if (vResultData.exp != null)
            fightView.Lbl_Endless_EXP.text = vResultData.exp.add_exp.ToString();

        //显示物品//
        int tmpDropListCount = tmpDropList.Count;
        int tmpInitPosX = 0;
        if (tmpDropListCount < 5)
        {
            tmpInitPosX = (5 - tmpDropListCount) * 50;
        }
        for (int i = 0; i < tmpDropListCount; i++)
        {
            if (i >= tmpDropListCount)
                continue;
            GameObject tmpItemObj = CommonFunction.InstantiateObject(fightView.Spt_Endless_ItemBG.gameObject, fightView.Grd_Endless_ItemGrid.transform);
            if (tmpItemObj == null)
                continue;
            CommonFunction.SetQualitySprite(tmpItemObj.transform.FindChild("Endless_ItemQuality").GetComponent<UISprite>(), tmpDropList[i].Quality, tmpItemObj.transform.GetComponent<UISprite>());
            CommonFunction.SetSpriteName(tmpItemObj.transform.FindChild("Endless_Item").GetComponent<UISprite>(), tmpDropList[i].Icon);
            tmpItemObj.transform.FindChild("Endless_ItemSoul").gameObject.SetActive(tmpDropList[i].IsSoul);
            tmpItemObj.gameObject.transform.localPosition = new Vector3(100 * i + tmpInitPosX, 0, 0);
            tmpItemObj.SetActive(true);
            endlessItemList.Add(tmpItemObj);
        }

        fightView.UIPanel_Endless_ItemList.transform.localPosition = new Vector3(0, -42, 0);
        fightView.UIPanel_Endless_ItemList.clipOffset = new Vector2(0, 0);
        OpenEndless();
    }

    /// <summary>
    /// 远征
    /// </summary>
    private void Init_Expedition()
    {
        fightView.Spt_Expedition_ItemBG.gameObject.SetActive(false);
        for (int i = 0; i < expeditionItemList.Count; i++)
            GameObject.Destroy(expeditionItemList[i]);
        expeditionItemList.Clear();
        fightView.ExpeditionItem_Scale.transform.localScale = Vector3.zero;
        fightView.ExpeditionItem_Scale.ResetToBeginning();
        fightView.ExpeditionHint_Position.ResetToBeginning();
        fightView.Obj_Result_Expedition.gameObject.SetActive(false);
    }
    public void Refresh_Expedition(object vResultData)
    {
        InitPanel();
        Init_Expedition();
        fightType = EFightType.eftExpedition;
        ExpeditionResultResp tmpResultData = (ExpeditionResultResp)vResultData;
        if (tmpResultData == null)
            return;
        Scheduler.Instance.AddTimer(2.5f, false, DelayOpenButton);
        SetClickCloseStatus(true);
        if (tmpResultData.exp_res == FIGHT_RESULT_VICTORY)
        {
            ShowVictoryAndFailure(true);
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Win));
        }
        else
        {
            Refresh_FailReason();
            ShowVictoryAndFailure(false);
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Lose));
        }
        //显示战损//
        if (tmpResultData.dead_soldiers != null)
        {
            for (int i = 0; i < tmpResultData.dead_soldiers.Count; i++)
            {
                SoldierList tmpResultInfo = tmpResultData.dead_soldiers[i];
                if (tmpResultInfo == null)
                    continue;
                SoldierAttributeInfo tmpSoliderInfo = PlayerData.Instance._SoldierDepot.FindByUid(tmpResultInfo.uid).Att;
                if (tmpSoliderInfo == null)
                    continue;
                GameObject tmpItemObj = CommonFunction.InstantiateObject(fightView.Spt_Expedition_ItemBG.gameObject, fightView.Obj_Result_Expedition);
                if (tmpItemObj == null)
                    continue;
                tmpItemObj.transform.localPosition = new Vector3((i % 3 - 1) * 95, 45 - (i / 3 * 95), 0);
                tmpItemObj.transform.localScale = Vector3.zero;
                CommonFunction.SetQualitySprite(tmpItemObj.transform.FindChild("Expedition_ItemQuality").GetComponent<UISprite>(), tmpSoliderInfo.quality, tmpItemObj.transform.GetComponent<UISprite>());
                CommonFunction.SetSpriteName(tmpItemObj.transform.FindChild("Expedition_Item").GetComponent<UISprite>(), tmpSoliderInfo.Icon);
                tmpItemObj.SetActive(true);
                tmpItemObj.transform.FindChild("Expedition_Num").GetComponent<UILabel>().text = tmpResultInfo.num.ToString();
                expeditionItemList.Add(tmpItemObj);
            }
        }
        OpenExpedition();
    }

    /// <summary>
    /// PVP
    /// </summary>
    private void Init_PVP()
    {
        fightView.PVPHint_Position.gameObject.transform.localPosition = new Vector3(500, 75, 0);
        fightView.PVPPre_Scale.gameObject.transform.localScale = new Vector3(1, 0, 1);
        fightView.PVPCur_Scale.gameObject.transform.localScale = new Vector3(1, 0, 1);

        fightView.PVPUphint_Scale.gameObject.transform.localScale = Vector3.zero;
        fightView.PVPUpicon_Scale.gameObject.transform.localScale = Vector3.zero;
        fightView.PVPCur_Scale.ResetToBeginning();
        fightView.PVPHint_Position.ResetToBeginning();
        fightView.PVPPre_Scale.ResetToBeginning();
        fightView.PVPUphint_Scale.ResetToBeginning();
        fightView.PVPUpicon_Scale.ResetToBeginning();
        fightView.TP_PVPHisHint.ResetToBeginning();
        fightView.TS_PVPHisPre.ResetToBeginning();
        fightView.TS_PVPHisCur.ResetToBeginning();
        fightView.TS_PVPHisUpIcon.ResetToBeginning();
        fightView.TP_PVPRewHint.ResetToBeginning();
        fightView.TS_PVPRewCur.ResetToBeginning();
        fightView.TS_PVPRewUpIcon.ResetToBeginning();
        fightView.Lbl_PVP_Hint.gameObject.SetActive(false);
        fightView.Spt_PVP_UpIcon.gameObject.SetActive(false);
        fightView.Spt_PVP_BG_UpHint.gameObject.SetActive(false);
        fightView.Lbl_PVP_Pre.text = "";
        fightView.Lbl_PVP_Cur.text = "";
        fightView.Lbl_UpHint.text = string.Empty;
        fightView.Lbl_PVP_Hint.transform.parent.localPosition = Vector3.zero;

        fightView.Lbl_PVP_His_Pre.text = "";
        fightView.Lbl_PVP_His_Cur.text = "";
        fightView.Lbl_PVP_His_Hint.transform.parent.gameObject.SetActive(false);
        fightView.Lbl_PVP_Rew_Cur.text = "";
        fightView.Lbl_PVP_Rew_Hint.transform.parent.gameObject.SetActive(false);
        fightView.Obj_Result_PVP.gameObject.SetActive(false);
    }
    public void Refresh_PVP(object vResultData)
    {
        InitPanel();
        Init_PVP();
        fightType = EFightType.eftPVP;
        ArenaRewardResp tmpResuleData = (ArenaRewardResp)vResultData;
        if (tmpResuleData == null)
            return;
        Scheduler.Instance.AddTimer(3.5f, false, DelayOpenButton);
        if (tmpResuleData.arena_result == ArenaResult.AR_WIN)
        {
            ShowPVPVictoryStatus(tmpResuleData);
        }
        else
        {
            Refresh_Fail();
            SetDelayFunc(2.5f, false, OpenFail);
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Lose));
        }

        SetClickCloseStatus(true);
        OpenPvP();
    }
    private void ShowPVPVictoryStatus(ArenaRewardResp vResuleData)
    {
        if (vResuleData == null)
            return;
        int tmpRankCur = vResuleData.rank;
        uint tmpRankBest = PVPModule.Instance.PvpData.best_rank;
        if (tmpRankCur < tmpRankBest)
        {
            fightView.Lbl_Fail_Hint_Info.text = ConstString.FIGHT_FAIL_REASON_PVPHINT;
            fightView.Lbl_PVP_His_Hint.transform.parent.gameObject.SetActive(true);
            fightView.Lbl_PVP_Rew_Hint.transform.parent.gameObject.SetActive(true);
            fightView.Lbl_PVP_Pre.transform.parent.localPosition = new Vector3(0, 55, 0);
            fightView.Lbl_PVP_His_Pre.text = tmpRankBest.ToString();
            fightView.Lbl_PVP_His_Cur.text = tmpRankCur.ToString();
            fightView.Lbl_PVP_Rew_Cur.text = vResuleData.diamond.ToString();
            if (vResuleData.diamond > 0)
            {
                SetDelayFunc(3.5f, false, DelayShowPVPRewardHint);
            }
        }
        fightView.Lbl_PVP_Pre.text = PVPModule.Instance.PvpData.rank.ToString();
        fightView.Lbl_PVP_Cur.text = vResuleData.rank.ToString();
        fightView.Lbl_UpHint.text = string.Format(ConstString.FIGHTVIEW_PVP_RANK_UP, PVPModule.Instance.PvpData.rank - vResuleData.rank);
        fightView.Lbl_PVP_Hint.gameObject.SetActive(true);
        fightView.Spt_PVP_UpIcon.gameObject.SetActive(true);
        fightView.Spt_PVP_BG_UpHint.gameObject.SetActive(true);
        ShowVictoryAndFailure(true);
        Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Win));
    }

    /// <summary>
    /// 奴隶战场
    /// </summary>
    private void Init_Slave()
    {
        fightView.SlaveHint_Position.gameObject.transform.localPosition = new Vector3(1000, 0, 0);
        fightView.SlaveHint_Position.ResetToBeginning();
        fightView.Lbl_Slave_Hint.text = string.Empty;
        fightView.Obj_Result_Slave.gameObject.SetActive(false);
    }
    public void Refresh_Slave(object vResultData)
    {
        InitPanel();
        Init_Slave();
        fightType = EFightType.eftSlave;
        EnslaveFightOverResp tmpResult = (EnslaveFightOverResp)vResultData;
        if (tmpResult == null)
            return;
        Scheduler.Instance.AddTimer(3f, false, DelayOpenButton);
        if (tmpResult.real_result == FIGHT_RESULT_VICTORY)
        {
            ShowVictoryAndFailure(true);
            switch (tmpResult.type)
            {
                case EnslaveFightType.EFT_ENSLAVE://奴役//
                    {
                        fightView.Lbl_Slave_Hint.text = string.Format(ConstString.PRISON_RESULT_WIN_ENSLAVE, tmpResult.name);
                    }
                    break;
                case EnslaveFightType.EFT_SAVE://救援//
                    {
                        fightView.Lbl_Slave_Hint.text = string.Format(ConstString.PRISON_RESULT_WIN_SAVE, tmpResult.name, tmpResult.gold);
                    }
                    break;
                case EnslaveFightType.EFT_AGAINST://反抗//
                    {
                        fightView.Lbl_Slave_Hint.text = string.Format(ConstString.PRISON_RESULT_WIN_AGAINST, tmpResult.name);
                    }
                    break;
            }
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Win));
        }
        else
        {
            Refresh_Fail();
            SetDelayFunc(2.5f, false, OpenFail);
            Main.Instance.StartCoroutine(DelayShowAudio(0.1f, GlobalConst.Sound.AUDIO_UI_Battle_Lose));
        }
        SetClickCloseStatus(true);
        OpenSlave();
    }

    /// <summary>
    /// 异域探险
    /// </summary>
    private void Init_Union()
    {
        fightView.Obj_Result_Union.gameObject.SetActive(false);
        fightView.Lbl_Union_HurtValue.text = "0";
        fightView.Lbl_Union_RewardValueGold.text = "0";
        fightView.Lbl_Union_RewardValueToken.text = "0";
        fightView.TP_Union_HurtTitle.ResetToBeginning();
        fightView.TP_Union_RewardTitle.ResetToBeginning();
    }
    public void Refresh_Union(object vResultData)
    {
        InitPanel();
        Init_Union();
        fightType = EFightType.eftUnion;
        UnionPveDgnRewardResp tmpResult = (UnionPveDgnRewardResp)vResultData;
        if (tmpResult == null)
            return;
        InitTitleTween();
        fightView.TP_Union_HurtTitle.PlayForward();
        fightView.TP_Union_RewardTitle.PlayForward();
        Scheduler.Instance.AddTimer(2.5f, false, DelayOpenButton);
        fightView.Lbl_Union_HurtValue.text = tmpResult.damage.ToString();
        GetIconListByDrop(tmpResult.drop_items, fightView.Lbl_Union_RewardValueGold, fightView.Lbl_Union_RewardValueToken);
        if (tmpResult.fight_result == FIGHT_RESULT_VICTORY)
        {
            fightView.Spt_Union_RewardIconToken.gameObject.SetActive(true);
            ShowVictoryAndFailure(true);
        }
        else
        {
            fightView.Spt_Union_RewardIconToken.gameObject.SetActive(false);
            ShowVictoryAndFailure(false);
        }

        SetClickCloseStatus(true);
        fightView.Obj_Result_Union.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
    }

    /// <summary>
    /// 攻城略地
    /// </summary>
    private void Init_CaptureTerritory()
    {
        fightView.Obj_Result_CaptureTerritory.gameObject.SetActive(false);
        fightView.Lbl_Cur_Integral.text = string.Format(ConstString.CAPTURETERRITORY_SCORE, 0);
        fightView.TP_Cur_Integral.ResetToBeginning();
    }
    public void Refresh_CaptureTerritory(object vResultData)
    {
        InitPanel();
        fightType = EFightType.eftCaptureTerritory;
        Init_CaptureTerritory();
        CampaignRewardResp tmpResult = (CampaignRewardResp)vResultData;
        if (tmpResult == null)
            return;
        Scheduler.Instance.AddTimer(1.5f, false, DelayOpenButton);
        fightView.Lbl_Cur_Integral.text = string.Format(ConstString.CAPTURETERRITORY_SCORE, tmpResult.score);
        CaptureTerritoryModule.Instance.SetFightCDTimer(tmpResult.next_tick);
        fightView.TP_Cur_Integral.PlayForward();
        SetClickCloseStatus(true);
        if (SceneManager.Instance.Get_CurScene != null)
        {
            if (SceneManager.Instance.Get_CurScene._SceneStatus == ESceneStatus.essVictory)
            {
                ShowVictoryAndFailure(true);
            }
            else
            {
                ShowVictoryAndFailure(false);
            }
        }
        else
        {
            ShowVictoryAndFailure(true);
        }
        fightView.Obj_Result_CaptureTerritory.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
        if (tmpResult.result != (int)ErrorCodeEnum.SUCCESS)
        {
            ErrorCode.ShowErrorTip(tmpResult.result);
        }
    }

    /// <summary>
    /// 全服争霸
    /// </summary>
    private void Init_ServerHegemony()
    {
        fightView.Obj_Result_ServerHegemony.gameObject.SetActive(false);
        fightView.Lbl_ST_FightHurtValue.text = "";
        fightView.Lbl_ST_BUFFHurtValue.text = "";
        fightView.Lbl_ST_TotleHurtValue.text = "";
        fightView.Lbl_ST_Award.text = "";
        fightView.Lbl_ST_AwardValue.text = "";
        fightView.TP_ST_Title.ResetToBeginning();
        fightView.TP_ST_FightHurt.ResetToBeginning();
        fightView.TP_ST_FightHurtValue.ResetToBeginning();
        fightView.TP_ST_BUFFHurt.ResetToBeginning();
        fightView.TP_ST_BUFFHurtValue.ResetToBeginning();
        fightView.TP_ST_TotleHurt.ResetToBeginning();
        fightView.TP_ST_TotleHurtValue.ResetToBeginning();
        fightView.TP_ST_Award.ResetToBeginning();
        fightView.TP_ST_AwardValue.ResetToBeginning();
    }
    public void Refresh_ServerHegemony(object vResultData)
    {
        InitPanel();
        fightType = EFightType.eftHegemony;
        Init_ServerHegemony();
        Scheduler.Instance.AddTimer(1, false, DelayOpenButton);
        BillingResp tmpResult = (BillingResp)vResultData;
        if (tmpResult != null)
        {
            fightView.Lbl_ST_FightHurtValue.text = FightRelatedModule.Instance.ServerHegemonyHurtValue.ToString();
            fightView.Lbl_ST_BUFFHurtValue.text = (tmpResult.totalhurt - FightRelatedModule.Instance.ServerHegemonyHurtValue).ToString();
            fightView.Lbl_ST_TotleHurtValue.text = tmpResult.totalhurt.ToString();
            fightView.Lbl_ST_AwardValue.text = tmpResult.currencystruc.number.ToString();
            if (tmpResult.currencystruc.type == (int)ECurrencyType.Gold)
            {
                fightView.Lbl_ST_Award.text = ConstString.SUPERMACY_GETGOLD;
                PlayerData.Instance.UpdateGold((int)tmpResult.currencystruc.totalnum);
            }
            else if (tmpResult.currencystruc.type == (int)ECurrencyType.Diamond)
            {
                fightView.Lbl_ST_Award.text = ConstString.SUPERMACY_GETDIAMOND;
                PlayerData.Instance.UpdateDiamond((int)tmpResult.currencystruc.totalnum);
            }
            if (tmpResult.result != (int)ErrorCodeEnum.SUCCESS)
            {
                ErrorCode.ShowErrorTip(tmpResult.result);
            }
        }
        SetClickCloseStatus(true);
        if (SceneManager.Instance.Get_CurScene != null)
        {
            if (SceneManager.Instance.Get_CurScene._SceneStatus == ESceneStatus.essVictory)
            {
                ShowVictoryAndFailure(true);
            }
            else
            {
                ShowVictoryAndFailure(false);
            }
        }
        else
        {
            ShowVictoryAndFailure(true);
        }
        fightView.TP_ST_Title.PlayForward();
        fightView.TP_ST_FightHurt.PlayForward();
        fightView.TP_ST_FightHurtValue.PlayForward();
        fightView.TP_ST_BUFFHurt.PlayForward();
        fightView.TP_ST_BUFFHurtValue.PlayForward();
        fightView.TP_ST_TotleHurt.PlayForward();
        fightView.TP_ST_TotleHurtValue.PlayForward();
        fightView.TP_ST_Award.PlayForward();
        fightView.TP_ST_AwardValue.PlayForward();
        fightView.Obj_Result_ServerHegemony.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);

        //判斷是否需要自動退出//
        if (SupermacyModule.Instance.isAutoBattle)
        {
            Scheduler.Instance.AddTimer(3, false, UISystem.Instance.FightView.DelayToSupermacy);
        }
    }

    /// <summary>
    /// 排位赛
    /// </summary>
    private void Init_Qualifying()
    {
        fightView.Obj_Result_Qualifying.gameObject.SetActive(false);
        fightView.Lbl_Qualifying_UpHint.text = "";
        fightView.Lbl_Qualifying_Pre.text = "";
        fightView.Lbl_Qualifying_Cur.text = "";
        fightView.TP_Qualifying_Title.ResetToBeginning();
        fightView.TS_Qualifying_Pre.ResetToBeginning();
        fightView.TS_Qualifying_Cur.ResetToBeginning();
        fightView.TS_Qualifying_UpIcon.ResetToBeginning();
        fightView.TS_Qualifying_BG_UpHint.ResetToBeginning();
    }
    public void Refresh_Qualifying(object vResultData)
    {
        InitPanel();
        Init_Qualifying();
        fightType = EFightType.eftQualifying;
        Scheduler.Instance.AddTimer(1, false, DelayOpenButton);
        //赋值 提示//
        EndPoleResp tmpResult = (EndPoleResp)vResultData;
        int tmpValuePre = PlayerData.Instance.QualifyingScore;
        int tmpValueCur = tmpResult.score;
        string tmpFormatStr = ConstString.QUALIFYING_RESULT_ADDINTEGRAL;
        if (tmpResult != null)
        {
            if (tmpResult.result != (int)ErrorCodeEnum.SUCCESS)
            {
                ErrorCode.ShowErrorTip(tmpResult.result);
            }
            else
            {
                if (tmpResult.pole_result == PoleResult.ATTACK_WIN || tmpResult.pole_result == PoleResult.REVENGE_WIN)
                {
                    fightView.Obj_Result_Qualifying.transform.localPosition = new Vector3(200, 0, 0);
                    ShowVictoryAndFailure(true);
                }
                else
                {
                    tmpFormatStr = ConstString.QUALIFYING_RESULT_REDUCEINTEGRAL;
                    fightView.Obj_Result_Qualifying.transform.localPosition = new Vector3(200, 60, 0);
                    Refresh_Fail();
                    SetDelayFunc(2.5f, false, OpenFail);
                }
            }
        }
        fightView.Lbl_Qualifying_Pre.text = tmpValuePre.ToString();
        fightView.Lbl_Qualifying_Cur.text = tmpValueCur.ToString();
        if (tmpValuePre <= tmpValueCur)
        {
            fightView.Lbl_Qualifying_UpHint.text = string.Format(tmpFormatStr, tmpValueCur - tmpValuePre);
        }
        else
        {
            fightView.Lbl_Qualifying_UpHint.text = string.Format(tmpFormatStr, tmpValuePre - tmpValueCur);
        }
        SetClickCloseStatus(true);
        fightView.TP_Qualifying_Title.PlayForward();
        fightView.TS_Qualifying_Pre.PlayForward();
        fightView.TS_Qualifying_Cur.PlayForward();
        fightView.TS_Qualifying_UpIcon.PlayForward();
        fightView.TS_Qualifying_BG_UpHint.PlayForward();
        fightView.Obj_Result_Qualifying.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
    }

    /// <summary>
    /// 跨服战场
    /// </summary>
    private void Init_CrossServerWar()
    {
        fightView.Obj_Result_CrossServerWar.gameObject.SetActive(false);
        fightView.Lbl_CSW_IntegralValue.text = string.Empty;
        fightView.Lbl_CSW_AwardValue.text = string.Empty;
        fightView.TP_CSW_IntegralTitle.ResetToBeginning();
        fightView.TP_CSW_AwardTitle.ResetToBeginning();
    }
    public void Refresh_CrossServerWar(object vResultData)
    {
        InitPanel();
        Init_CrossServerWar();
        fightType = EFightType.eftCrossServerWar;
        SetClickCloseStatus(true);
        Scheduler.Instance.AddTimer(0.5f, false, DelayOpenButton);

        if (CrossServerWarModule.Instance.isBattleStart)
        {
            fightView.Lbl_CSW_IntegralValue.text = ((int)UISystem.Instance.FightView.curValue_Score).ToString();
            if (((CombatSettlementResp)vResultData).gains != null)
            {
                fightView.Lbl_CSW_AwardValue.text = ((CombatSettlementResp)vResultData).gains.change_num.ToString();
                PlayerData.Instance.UpdateItem(((CombatSettlementResp)vResultData).gains);
            }
            else
            {
                fightView.Lbl_CSW_AwardValue.text = "0";
            }
        }
        else
        {
            fightView.Lbl_CSW_IntegralValue.text = "0";
            fightView.Lbl_CSW_AwardValue.text = "0";
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CAPTURETERRITORY_OVER);
        }
        fightView.TP_CSW_IntegralTitle.PlayForward();
        fightView.TP_CSW_AwardTitle.PlayForward();
        ShowVictoryAndFailure(true);

        fightView.Obj_Result_CrossServerWar.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
    }


    /// <summary>
    /// 刷新失败原因显示
    /// </summary>
    private void Refresh_FailReason()
    {
        failReason = string.Empty;
        if (SceneManager.Instance.Get_CurScene == null)
            return;

        switch (SceneManager.Instance.Get_CurScene.Get_FightFailReason)
        {
            case EPVEFinishStatus.epvefsDieHero:
                {
                    fightView.Lbl_Fail_Hint_Info.text = ConstString.FIGHT_FAIL_REASON_DIE_HERO;
                }
                break;
            case EPVEFinishStatus.epvefsDieBarracksSelf:
                {
                    fightView.Lbl_Fail_Hint_Info.text = ConstString.FIGHT_FAIL_REASON_DIE_BARRACKS;
                }
                break;
            case EPVEFinishStatus.epvefsDieEscort:
                {
                    fightView.Lbl_Fail_Hint_Info.text = ConstString.FIGHT_FAIL_REASON_DIE_ESCORT;
                }
                break;
            case EPVEFinishStatus.epvefsDieTransfer:
                {
                    fightView.Lbl_Fail_Hint_Info.text = ConstString.FIGHT_FAIL_REASON_DIE_TRANSFER;
                }
                break;
            case EPVEFinishStatus.epvefsDieSoldier:
                {
                    fightView.Lbl_Fail_Hint_Info.text = ConstString.FIGHT_FAIL_REASON_DIE_SOLDIER;
                }
                break;
            case EPVEFinishStatus.epvefsOutTime:
                {
                    fightView.Lbl_Fail_Hint_Info.text = ConstString.FIGHT_FAIL_REASON_OUTTIME;
                }
                break;
        }
        failReason = fightView.Lbl_Fail_Hint_Info.text;
    }
    /// <summary>
    /// 刷新失败显示
    /// </summary>
    private void Refresh_Fail()
    {
        Refresh_FailReason();
        ShowVictoryAndFailure(false);
        fightView.Spt_Fail_Hint_Activity.gameObject.SetActive(true);
        fightView.Obj_Result_Fail.transform.localPosition = new Vector3(210, 0, 0);
        if (fightType == EFightType.eftPVP)
        {
            CommonFunction.SetSpriteName(fightView.Spt_Fail_Hint_Activity, GlobalConst.SpriteName.SPRITE_NAME_HINT_PVP);
        }
        else if (fightType == EFightType.eftQualifying)
        {
            fightView.Spt_Fail_Hint_Activity.gameObject.SetActive(false);
            fightView.Obj_Result_Fail.transform.localPosition = new Vector3(210, -50, 0);
        }
        else
        {
            CommonFunction.SetSpriteName(fightView.Spt_Fail_Hint_Activity, GlobalConst.SpriteName.SPRITE_NAME_HINT_ACTIVITY);
        }
        fightView.Spt_Fail_Hint_Activity.MakePixelPerfect();
    }


    //-------------------------------//
    private IEnumerator DelayShowAudio(float vDelayTime, string vAudioName)
    {
        if (!string.IsNullOrEmpty(vAudioName))
        {
            yield return new WaitForSeconds(vDelayTime);
            if (!isSkip)
            {
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(vAudioName));
            }
        }
    }

    private void OpenUpLvPn()
    {
        Scheduler.Instance.RemoveTimer(OpenUpLvPn);
        DungeonRewardResp tmpResultData = (DungeonRewardResp)resultData;
        if (tmpResultData != null)
        {
            PlayerData.Instance.UpdateBattleSettle(fightType, fightID, tmpResultData.star_level, tmpResultData.drop_items, tmpResultData.exp, tmpResultData.ph_power, tmpResultData.today_elite_dgn);
        }
    }

    private void OpenActivity()
    {
        InitTitleTween();
        fightView.Star0_Alpha.PlayForward();
        fightView.Star0_Scale.PlayForward();
        fightView.Star1_Alpha.PlayForward();
        fightView.Star1_Scale.PlayForward();
        fightView.Star2_Alpha.PlayForward();
        fightView.Star2_Scale.PlayForward();
        fightView.ItemBG_Scale.PlayForward();
        fightView.Gold_Position.PlayForward();
        fightView.Exp_Position.PlayForward();
        fightView.Restart_Scale.PlayForward();
        fightView.Exit_Scale.PlayForward();
        fightView.Next_Scale.PlayForward();
        fightView.Obj_Result_Activity.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
    }

    private void OpenEndless()
    {
        InitTitleTween();
        fightView.EndlessCurScore_Scale.PlayForward();
        fightView.EndlessExp_Scale.PlayForward();
        fightView.EndlessGold_Scale.PlayForward();
        fightView.EndlessMaxScore_Scale.PlayForward();
        fightView.EndlessNum_Position.PlayForward();
        fightView.Detail_Scale.PlayForward();
        fightView.Obj_Result_Endless.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
    }
    
    private void OpenExpedition()
    {
        InitTitleTween();
        fightView.ExpeditionItem_Scale.PlayForward();
        fightView.ExpeditionHint_Position.PlayForward();
        fightView.Obj_Result_Expedition.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
    }
    
    private void OpenPvP()
    {
        InitTitleTween();
        fightView.PVPCur_Scale.PlayForward();
        fightView.PVPHint_Position.PlayForward();
        fightView.PVPPre_Scale.PlayForward();
        fightView.PVPUphint_Scale.PlayForward();
        fightView.PVPUpicon_Scale.PlayForward();
        fightView.TP_PVPHisHint.PlayForward();
        fightView.TS_PVPHisPre.PlayForward();
        fightView.TS_PVPHisCur.PlayForward();
        fightView.TS_PVPHisUpIcon.PlayForward();
        fightView.TP_PVPRewHint.PlayForward();
        fightView.TS_PVPRewCur.PlayForward();
        fightView.TS_PVPRewUpIcon.PlayForward();
        fightView.Obj_Result_PVP.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
    }
    private void DelayShowPVPRewardHint()
    {
        Scheduler.Instance.RemoveTimer(DelayShowPVPRewardHint);
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_SHOWREWARDHINT);
    }
    
    private void OpenSlave()
    {
        InitTitleTween();
        fightView.SlaveHint_Position.PlayForward();
        fightView.Obj_Result_Slave.gameObject.SetActive(true);
        fightView.UIPanel_FightResult.gameObject.SetActive(true);
    }

    private void OpenFail()
    {
        Scheduler.Instance.RemoveTimer(OpenFail);
        fightView.Obj_Result_Fail.gameObject.SetActive(true);

        fightView.Btn_FailItem1.gameObject.SetActive(false);
        fightView.Btn_FailItem3.gameObject.SetActive(false);
        if (MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.LifeSoulSystm))
        {
            fightView.Btn_FailItem0.transform.localPosition = new Vector3(-110, 0, 0);
            fightView.Btn_FailItem2.transform.localPosition = new Vector3(0, 0, 0);
            fightView.Btn_FailItem4.transform.localPosition = new Vector3(110, 0, 0);
        }
        else
        {
            fightView.Btn_FailItem0.transform.localPosition = new Vector3(-150, 0, 0);
            fightView.Btn_FailItem2.transform.localPosition = new Vector3(-40, 0, 0);
            fightView.Btn_FailItem3.transform.localPosition = new Vector3(70, 0, 0);
            fightView.Btn_FailItem4.transform.localPosition = new Vector3(180, 0, 0);
            fightView.Btn_FailItem3.gameObject.SetActive(true);
        }

        InitOpenFailTween();
    }

    private void OpenWinTitleEffect()
    {
        Scheduler.Instance.RemoveTimer(OpenWinTitleEffect);
        fightView.SA_Shengli.gameObject.transform.localPosition = Vector3.zero;
        fightView.SA_Shengli.gameObject.SetActive(true);
        fightView.SA_Shengli.Reset();
    }

    private void DelayOpenButton()
    {
        Scheduler.Instance.RemoveTimer(DelayOpenButton);
        listDelayFunc.Clear();
        ReSetButtonStatus(true);
        if (isHaveMail)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPOVERFLOW);
        }
    }

    private void ReSetButtonStatus(bool vStatus)
    {
        if (fightView == null)
            return;
        if ((fightView.Btn_Result_Hint != null) && (fightView.Btn_Result_Hint.GetComponent<BoxCollider>() != null))
        {
            fightView.Btn_Result_Hint.GetComponent<BoxCollider>().enabled = vStatus;
        }
        if ((fightView.Btn_Activity_ReStart != null) && (fightView.Btn_Activity_ReStart.GetComponent<BoxCollider>() != null))
        {
            fightView.Btn_Activity_ReStart.GetComponent<BoxCollider>().enabled = vStatus;
        }
        if ((fightView.Btn_Activity_Exit != null) && (fightView.Btn_Activity_Exit.GetComponent<BoxCollider>() != null))
        {
            fightView.Btn_Activity_Exit.GetComponent<BoxCollider>().enabled = vStatus;
        }
        if ((fightView.Btn_Activity_Next != null) && (fightView.Btn_Activity_Next.GetComponent<BoxCollider>() != null))
        {
            fightView.Btn_Activity_Next.GetComponent<BoxCollider>().enabled = vStatus;
        }
    }




    public void ShowVictoryAndFailure(bool isVictory)
    {
        if (isVictory)
        {
            fightView.Spt_Result_Title.gameObject.SetActive(false);
            fightView.Result_TitleWin.gameObject.SetActive(true);
            if (fightType == EFightType.eftFriend)
            {
                CommonFunction.SetSpriteName(fightView.Result_TitleWin, GlobalConst.SpriteName.SPRITE_NAME_FINAL);
            }
            else
            {
                CommonFunction.SetSpriteName(fightView.Result_TitleWin, GlobalConst.SpriteName.SPRITE_NAME_VICTORY);
            }
            switch (fightType)
            {
                case EFightType.eftMain:
                case EFightType.eftActivity:
                    { SetDelayFunc(0.4f, false, OpenWinTitleEffect); }
                    break;
                case EFightType.eftEndless:
                    { SetDelayFunc(0.5f, false, OpenWinTitleEffect); }
                    break;
                case EFightType.eftExpedition:
                    { SetDelayFunc(0.3f, false, OpenWinTitleEffect); }
                    break;
                case EFightType.eftPVP:
                    { SetDelayFunc(0.25f, false, OpenWinTitleEffect); }
                    break;
                default:
                    { SetDelayFunc(0.25f, false, OpenWinTitleEffect); }
                    break;
            }
            fightView.TitleWin_Position.PlayForward();
            fightView.TitleWin_Alpha.PlayForward();
            fightView.TitleWin_Rotation.PlayForward();
            fightView.TitleWin_Scale.PlayForward();
        }
        else
        {
            fightView.Spt_Result_Title.gameObject.SetActive(true);
            fightView.Result_TitleWin.gameObject.SetActive(false);
            if ((fightType == EFightType.eftEndless) || (fightType == EFightType.eftUnion) || (fightType == EFightType.eftFriend) ||
                (fightType == EFightType.eftHegemony) || (fightType == EFightType.eftCaptureTerritory) || (fightType == EFightType.eftCrossServerWar))
            {
                CommonFunction.SetSpriteName(fightView.Spt_Result_Title, GlobalConst.SpriteName.SPRITE_NAME_FINAL);
            }
            else
            {
                CommonFunction.SetSpriteName(fightView.Spt_Result_Title, GlobalConst.SpriteName.SPRITE_NAME_FAILURE);
            }
            fightView.Title_Alpha.PlayForward();
            fightView.Title_Scale.PlayForward();
            fightView.Title_Position.PlayForward();
            fightView.Title_Rotation.PlayForward();
        }
    }
    //结算界面调整----------------------------------------------------//
    private void SetDelayFunc(float vDelayTime, bool vIsLoop, Assets.Script.Common.Scheduler.OnScheduler vFunc)
    {
        if (vFunc != null)
        {
            Scheduler.Instance.AddTimer(vDelayTime, vIsLoop, vFunc);
            listDelayFunc.Add(vFunc);
        }
    }

    public void FindTweenOperate()
    {
        if (isSkip)
        {
            return;
        }
        isSkip = true;

        if (listDelayFunc != null)
        {
            for (int i = 0; i < listDelayFunc.Count; i++)
            {
                if (listDelayFunc[i] != null)
                {
                    Scheduler.Instance.RemoveTimer(listDelayFunc[i]);
                    listDelayFunc[i]();
                }
            }
            listDelayFunc.Clear();
        }
        Scheduler.Instance.RemoveTimer(DelayOpenButton);
        Scheduler.Instance.AddTimer(0.5f, false, DelayOpenButton);
        fightView.SA_Shengli.gameObject.transform.localPosition = new Vector3(0, 3000, 0);
        Scheduler.Instance.AddTimer(0.3f, false, DelayComponentOperate);
    }

    private void DelayComponentOperate()
    {
        AddComponentOperate(fightView.UIPanel_FightResult.transform);
    }
    private void AddComponentOperate(Transform vTrans)
    {
        if (vTrans != null)
        {
            int tmpCount = vTrans.GetChildCount();
            for (int i = 0; i < tmpCount; i++)
            {
                Transform tmpChild = vTrans.GetChild(i);
                if (tmpChild != null)
                {
                    if (tmpChild.GetComponent<TweenOperate>() != null)
                    {
                        tmpChild.GetComponent<TweenOperate>().Stop();
                    }
                    AddComponentOperate(tmpChild);
                }
            }
        }
    }

    private void ReSetTweenInfos(Transform vTrans)
    {
        if (vTrans != null)
        {
            int tmpCount = vTrans.GetChildCount();
            for (int i = 0; i < tmpCount; i++)
            {
                Transform tmpChild = vTrans.GetChild(i);
                if (tmpChild != null)
                {
                    if (tmpChild.GetComponent<TweenOperate>() != null)
                    {
                        tmpChild.GetComponent<TweenOperate>().ReSet();
                    }
                    AddComponentOperate(tmpChild);
                }
            }
        }
    }
    //结算界面调整----------------------------------------------------//
}
