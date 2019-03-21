using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CaptureTerritoryInfoViewController : UIBase
{
    public CaptureTerritoryInfoView view;

    private StageInfo stageInfo;

    private List<GateEnemyInfoComponent> enemy_dic;
    private List<GateAwardsComponent> awards_dic;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new CaptureTerritoryInfoView();
            view.Initialize();
        }
        BtnEventBinding();
        CaptureTerritoryModule.Instance.CampaignTimerUpdate += UpdateCDTime;
        view.Gobj_EnemyInfoComp.SetActive(false);
        view.Gobj_AwardsInfoComp.SetActive(false);
        if (enemy_dic == null)
            enemy_dic = new List<GateEnemyInfoComponent>();
        if (awards_dic == null)
            awards_dic = new List<GateAwardsComponent>();
        UpdateCDTime();
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_EXOTICADVANTUREINFOVIEW);
    }

    #region ButtonEvent

    private void ButtonEvent_EnemyComp(GateEnemyInfoComponent baseComp, bool isPress)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (baseComp == null) return;
        HintManager.Instance.SeeDetail(baseComp.mRootObject, isPress, baseComp.MonsterInfo);
    }

    private void ButtonEvent_AwardComp(GateAwardsComponent baseComp, bool isPress)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (baseComp == null) return;
        HintManager.Instance.SeeDetail(baseComp.mRootObject, isPress, baseComp.ItemID);
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CAPTURE_TERRITORY_INFO);
    }

    public void ButtonEvent_ReadyBattle(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
        UISystem.Instance.PrepareBattleView.UpdateViewInfo(EFightType.eftCaptureTerritory, stageInfo);
    }
    #endregion



    #region Update Event

    /// <summary>
    /// 更新信息
    /// </summary>
    /// <param name="stage">关卡数据</param>
    /// <param name="awards">掉落数据</param>
    /// <param name="corpsname">军团名</param>
    /// <param name="corpsIntegral">军团积分</param>
    /// <param name="myIntegral">我的积分</param>
    public void UpdateViewInfo(StageInfo stage, List<CommonItemData> awards, string corpsname, int corpsIntegral, int myIntegral)
    {
        stageInfo = stage;
        if (stageInfo == null)
            return;
        view.Lbl_GateCountLabel.text = string.Empty;
        view.Lbl_TitleLabel.text = stageInfo.Name;
        view.Lbl_StageDesc.text = stageInfo.Describe;
        view.Lbl_MyIntegralNum.text = myIntegral.ToString();
        view.Lbl_IntegralNum.text = corpsIntegral.ToString();
        view.Lbl_CorpsName.text = corpsname;
        UpdateEnemyCast();
        Main.Instance.StartCoroutine(UpdateAwards(awards));
    }


    private void UpdateEnemyCast()
    {
        if (stageInfo.EnemySquad.Count <= enemy_dic.Count)
        {
            for (int index = 0; index < enemy_dic.Count; index++)
            {
                GateEnemyInfoComponent comp = enemy_dic[index];
                if (index < stageInfo.EnemySquad.Count)
                {
                    SingleEnemyInfo _singleinfo = stageInfo.EnemySquad[index];
                    if (_singleinfo == null) continue;
                    MonsterAttributeInfo _monster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(_singleinfo.MonsterID);
                    if (_monster == null) continue;
                    if (_singleinfo.IsBoss == 1)
                    {
                        comp.UpdateInfo(_monster, true);
                    }
                    else
                    {
                        comp.UpdateInfo(_monster, false);
                    }
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.Clear();
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int enemyObj_count = enemy_dic.Count;
            for (int index = 0; index < stageInfo.EnemySquad.Count; index++)
            {
                SingleEnemyInfo _singleinfo = stageInfo.EnemySquad[index];
                if (_singleinfo == null) continue;
                MonsterAttributeInfo _monster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(_singleinfo.MonsterID);
                if (_monster == null) continue;
                GateEnemyInfoComponent comp = null;
                if (index < enemyObj_count)
                {
                    comp = enemy_dic[index];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_EnemyInfoComp, view.Grd_EnemyGrid.transform);
                    comp = new GateEnemyInfoComponent(go);
                    go.name = _monster.ID.ToString();
                    enemy_dic.Add(comp);
                }
                if (_singleinfo.IsBoss == 1)
                {
                    comp.UpdateInfo(_monster, true);
                }
                else
                {
                    comp.UpdateInfo(_monster, false);
                }
                comp.AddPressLisetener(ButtonEvent_EnemyComp);
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_EnemyGrid.repositionNow = true;
    }

    /// <summary>
    /// 更新掉落包数据
    /// </summary>
    private IEnumerator UpdateAwards(List<CommonItemData> list)
    {
        if (list != null)
        {
            if (list.Count <= awards_dic.Count)
            {
                for (int i = 0; i < awards_dic.Count; i++)
                {
                    GateAwardsComponent comp = awards_dic[i];
                    if (comp == null) continue;
                    if (i < list.Count)
                    {
                        CommonItemData award = list[i];
                        bool isSpecialID = false;
                        IDType idType = CommonFunction.GetTypeOfID(award.ID.ToString());
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
                        } if (isSpecialID)
                        {
                            comp.UpdateCompInfo(award.ID, award.SubType, CommonFunction.GetIconNameByID(award.ID), 1);
                        }
                        else
                        {
                            comp.UpdateCompInfo(award.ID, award.SubType, award.Icon, (int)award.Quality);
                        }
                        if (!comp.mRootObject.activeSelf)
                            comp.mRootObject.SetActive(true);
                    }
                    else
                    {
                        comp.Clear();
                        if (comp.mRootObject.activeSelf)
                            comp.mRootObject.SetActive(false);
                    }
                }
            }
            else
            {
                int count = GlobalCoefficient.GateAwardsCountLimit;
                if (list.Count <= count)
                {
                    count = list.Count;
                }
                for (int i = 0; i < count; i++)
                {
                    CommonItemData award = list[i];
                    GateAwardsComponent comp = null;
                    if (i < awards_dic.Count)
                    {
                        comp = awards_dic[i];
                    }
                    else
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Gobj_AwardsInfoComp, view.Grd_AwardsGrid.transform);
                        comp = new GateAwardsComponent(go);
                        awards_dic.Add(comp);
                    }
                    if (comp == null) continue;
                    bool isSpecialID = false;
                    IDType idType = CommonFunction.GetTypeOfID(award.ID.ToString());
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
                        comp.UpdateCompInfo(award.ID, award.SubType, CommonFunction.GetIconNameByID(award.ID), (int)ItemQualityEnum.White);
                    }
                    else
                    {
                        comp.UpdateCompInfo(award.ID, award.SubType, award.Icon, (int)award.Quality);
                    }
                    comp.AddPressLisetener(ButtonEvent_AwardComp);
                    comp.mRootObject.SetActive(true);
                }
            }
        }
        yield return null;
        view.Grd_AwardsGrid.repositionNow = true;
    }

    private void UpdateCDTime()
    {
        view.Lbl_GateCountLabel.text = CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.CampaignTimer);
    }

    #endregion

    public override void Uninitialize()
    {
        stageInfo = null;
        CaptureTerritoryModule.Instance.CampaignTimerUpdate -= UpdateCDTime;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        enemy_dic.Clear();
        awards_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_ReadyBattle.gameObject).onClick = ButtonEvent_ReadyBattle;
    }


}
