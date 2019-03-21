using UnityEngine;
using fogs.proto.msg;
using System.Collections;
using System.Collections.Generic;

public class ServerHegemonyInfoViewController : UIBase
{
    public ServerHegemonyInfoView view;

    private ArenaPlayer arenaPlayer;

    private ExpeEnemyPlayerComponent enemyComp;
    /// <summary>
    /// 敌方阵容
    /// </summary>
    private List<ExpeEnemyLineupComponent> enemy_dic;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new ServerHegemonyInfoView();
            view.Initialize();
        }
        if (enemyComp == null)
        {
            enemyComp = new ExpeEnemyPlayerComponent(view.Gobj_PlayerInfoComp);
        }
        if (enemy_dic == null)
            enemy_dic = new List<ExpeEnemyLineupComponent>();
        view.Gobj_EnemyInfoComp.SetActive(false);
        BtnEventBinding();
    }

    public void UpdateViewInfo(ArenaPlayer info, string title)
    {
        arenaPlayer = info;
        if (info == null)
            return;
        view.Lbl_Title.text = title;
        UpdateView();
        UpdateEnemyCast();
    }

    private void UpdateView()
    {
        if (arenaPlayer.soldiers != null && arenaPlayer.hero != null)
        {
            enemyComp.UpdateInfo(arenaPlayer.hero.icon, arenaPlayer.hero.icon_frame, arenaPlayer.hero.level);
            // enemyComp.UpdateInfo(CommonFunction.GetHeroIconNameByGender((EHeroGender)PlayerData.Instance._ExpeditionInfo.enemies.hero.gender), PlayerData.Instance._ExpeditionInfo.enemies.hero.level);

            view.Lbl_PlayerName.text = arenaPlayer.hero.charname;
            if (string.IsNullOrEmpty(arenaPlayer.hero.unionname))
            {
                view.Lbl_UnionName.text = ConstString.HINT_NO;
            }
            else
            {
                view.Lbl_UnionName.text = arenaPlayer.hero.unionname;
            }
        }
        else
        {
            view.Lbl_PlayerName.text = string.Empty;
            view.Lbl_UnionName.text = string.Empty;
        }
    }

    private void UpdateEnemyCast()
    {
        if (enemy_dic.Count < GlobalCoefficient.LineupSoldierLimit)
        {
            for (int index = enemy_dic.Count; index < GlobalCoefficient.LineupSoldierLimit; index++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_EnemyInfoComp, view.Grd_EnemyGrid.transform);
                ExpeEnemyLineupComponent comp = new ExpeEnemyLineupComponent();
                comp.MyStart(go);
                go.name = "enemy_" + index;
                enemy_dic.Add(comp);
                go.SetActive(true);
            }
        }
        List<ArenaSoldier> soldiers = new List<ArenaSoldier>();
        List<ArenaSoldier> left = new List<ArenaSoldier>();
        List<ArenaSoldier> right = new List<ArenaSoldier>();
        for (int i = 0; i < arenaPlayer.soldiers.Count; i++)
        {
            ArenaSoldier soldier = arenaPlayer.soldiers[i];
            if (soldier.num <= 0)
            {
                right.Add(soldier);
            }
            else
            {
                left.Add(soldier);
            }
        }
        soldiers.AddRange(left);
        soldiers.AddRange(right);
        for (int i = 0; i < enemy_dic.Count; i++)
        {
            ExpeEnemyLineupComponent comp = enemy_dic[i];
            if (i < soldiers.Count)
            {
                fogs.proto.msg.ArenaSoldier enemySoldier = soldiers[i];
                Soldier tmpEnemySoldier = Soldier.createByID(enemySoldier.soldier.id);
                tmpEnemySoldier.Serialize(enemySoldier.soldier);
                tmpEnemySoldier.SerializeShowInfo(enemySoldier.soldier.attr);
                comp.UpdateInfo(tmpEnemySoldier);
                comp.UpdateNum(enemySoldier.num);
                comp.IsShowEnergy = true;
                comp.IsShowLeader = false;
                comp.IsSelect = false;
                comp.IsEnable = true;
                if (enemySoldier.num <= 0)
                {
                    comp.IsDead = true;
                }
                else
                {
                    comp.IsDead = false;
                }
                comp.mRootObject.SetActive(true);
            }
            else
            {
                comp.mRootObject.SetActive(false);
            }
        }
        view.Grd_EnemyGrid.repositionNow = true;
    }

    public void ButtonEvent_ReadyBattle(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
        UISystem.Instance.PrepareBattleView.UpdateViewInfo(EFightType.eftServerHegemony, arenaPlayer);
    }

    public void ButtonEvent_CloseView(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SERVERHEGEMONYINFO);
    }

    public override void Uninitialize()
    {
        arenaPlayer = null;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        enemyComp = null;
        enemy_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_ViewMask.gameObject).onClick = ButtonEvent_CloseView;
        UIEventListener.Get(view.Btn_ReadyBattle.gameObject).onClick = ButtonEvent_ReadyBattle;
    }
}
