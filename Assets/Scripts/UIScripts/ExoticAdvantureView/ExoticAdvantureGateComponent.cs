using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
public class ExoticAdvantureGateComponent : BaseComponent
{
    private UISprite Spt_PassMark;
    private UISprite Spt_BossIcon;
    private UISlider Sld_BloodSlider;
    private UILabel Lbl_GateProgress;
    private UILabel Lbl_BossName;
    private UISprite Spt_SldBG;
    private UISprite Spt_SldFG;

    private int lockStatus = 0;
    /// <summary>
    /// 1已通过 2已解锁,未通过 
    /// </summary>
    public int LockStatus
    {
        get
        {
            return lockStatus;
        }
    }

    private StageInfo stageInfo = null;
    public StageInfo StageInfo
    {
        get
        {
            return stageInfo;
        }
    }

    private UnionPveDgnInfo dgnData;
    public UnionPveDgnInfo DgnData
    {
        get
        {
            return dgnData;
        }
    }

    private MonsterAttributeInfo _monster;
    public MonsterAttributeInfo Monster
    {
        get
        {
            return _monster;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_BossIcon = mRootObject.transform.FindChild("BossIcon").GetComponent<UISprite>();
        Sld_BloodSlider = mRootObject.transform.FindChild("BloodSlider").GetComponent<UISlider>();
        Lbl_BossName = mRootObject.transform.FindChild("BossName").GetComponent<UILabel>();
        Lbl_GateProgress = mRootObject.transform.FindChild("GateProgress").GetComponent<UILabel>();
        Spt_PassMark = mRootObject.transform.FindChild("PassMark").GetComponent<UISprite>();
        Spt_SldBG = mRootObject.transform.FindChild("BloodSlider/Background").GetComponent<UISprite>();
        Spt_SldFG = mRootObject.transform.FindChild("BloodSlider/Foreground").GetComponent<UISprite>();
    }

    public void UpdateCompInfo(UnionPveDgnInfo info, StageInfo stage, int status)
    {
        dgnData = info;
        stageInfo = stage;
        lockStatus = status;
        if (stage.EnemySquad == null || stage.EnemySquad.Count == 0)
        {
            Debug.LogError("Config Error");
        }
        else
        {
            for (int i = 0; i < stage.EnemySquad.Count; i++)
            {
                SingleEnemyInfo enemyInfo = stage.EnemySquad[i];
                if (enemyInfo == null) continue;
                MonsterAttributeInfo monsterInfo = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(enemyInfo.MonsterID);
                _monster = monsterInfo;
                if (monsterInfo != null)
                {
                    if (monsterInfo.IsBoss == GlobalConst.MONSTER_TYPE_BOSS)
                    {
                        //Lbl_BossName.text = monsterInfo.Name;
                    }
                }
            }
        }
        CommonFunction.SetSpriteName(Spt_BossIcon, stageInfo.BossIcon);
        Spt_BossIcon.MakePixelPerfect();
        if (Monster == null || lockStatus == 1)
        {
            UpdateProgress(0);
            //Lbl_BossName.text = string.Empty;
        }
        else
        {
            int surplusHP = Monster.HP - info.total_hurt;
            float progress = (float)surplusHP / (float)Monster.HP;
            UpdateProgress(progress);
        }
        Lbl_BossName.text = stageInfo.Name;
        UpdateLockStatus();
    }

    private void UpdateLockStatus()
    {
        switch (LockStatus)
        {
            case 1:
                {
                    Spt_PassMark.enabled = true;
                    Spt_PassMark.MakePixelPerfect();
                    CommonFunction.UpdateWidgetGray(Spt_BossIcon, false);
                    CommonFunction.UpdateWidgetGray(Spt_SldBG, false);
                    CommonFunction.UpdateWidgetGray(Spt_SldFG, false);
                    Lbl_GateProgress.color = new Color(1, 225f / 255f, 152f / 255f);
                }
                break;
            case 2:
                {
                    Spt_PassMark.enabled = false;
                    CommonFunction.UpdateWidgetGray(Spt_BossIcon, false);
                    CommonFunction.UpdateWidgetGray(Spt_SldBG, false);
                    CommonFunction.UpdateWidgetGray(Spt_SldFG, false);
                    Lbl_GateProgress.color = new Color(1, 225f / 255f, 152f / 255f);
                }
                break;
            case 3:
                {
                    Spt_PassMark.enabled = false;
                    CommonFunction.UpdateWidgetGray(Spt_BossIcon, true);
                    CommonFunction.UpdateWidgetGray(Spt_SldBG, true);
                    CommonFunction.UpdateWidgetGray(Spt_SldFG, true);
                    Lbl_GateProgress.color = Color.gray;
                }
                break;
        }
    }

    private void UpdateProgress(float progress)
    {
        if (progress >= 1)
        {
            Sld_BloodSlider.value = 1;
            Lbl_GateProgress.text = string.Format(ConstString.PLAYEREXP, 100);
        }
        else if (progress <= 0)
        {
            Sld_BloodSlider.value = 0;
            Lbl_GateProgress.text = string.Format(ConstString.PLAYEREXP, 0);
        }
        else
        {
            if (progress < 0.001f)
            {
                progress = 0.001f;
                Sld_BloodSlider.value = progress;
            }
            else if ((0.999f <= progress) && (progress < 1f))
            {
                progress = 0.999f;
                Sld_BloodSlider.value = progress;
            }
            else
            {
                progress = Mathf.Ceil(progress * 1000) / 1000f;
                Sld_BloodSlider.value = progress;
            }
            Lbl_GateProgress.text = string.Format("{0:0.0%}", progress);
        }
    }


    public override void Clear()
    {
        base.Clear();
    }
}
