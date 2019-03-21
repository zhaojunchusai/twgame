using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SoldierCenterPanel
{
    public UIPanel centerPanel;

    public Soldier soldier;
    public UILabel Lbl_name;
    public UILabel Lbl_level;
    public GameObject UIPanel_Panel_player;
    public GameObject player;
    public UISlider Slider_ProgressBar;
    public UILabel Lbl_SliderProgressBarLabel;
    public UILabel Lbl_Label_attribute1;
    public UILabel Lbl_Label_attribute2;
    public UILabel Lbl_Label_attribute3;
    public UILabel Lbl_Label_attribute4;
    public UILabel Lbl_Label_attribute5;
    public UILabel Lbl_Label_attribute6;
    public UILabel PowerLabel;
    public UILabel Debris_Lable;
    public UILabel TalentLabel;
    public UILabel lbl_Label_Step;
    public UILabel Lbl_StarButton;
    public UISprite Spt_StarButton;

    public UIGrid Grd_GridStar;
    public SDIntensifyPanel soldierIntensify;
    public SDUpStarPanel soldierUpStar;
    public SDSelectPanel soldierSelect;
    public UISprite StrengthButtonLabel;
    private GameObject spine;
    public GameObject StarPrompt;
    public GameObject StrengthPrompt;
    private TdSpine.MainSpine mainSpine;
    private string animation = "";
    private UILabel SoldierPos;
    private int[] SeatPos = { 3, 6, 5 };
    private int[] Carrer = { 2, 0, 1, 4 };
    public enum SoldierPosEnum
    {
        FANG = 0,
        Fuzhu = 1,
        GongGI = 2,
        JING = 3,
        KONG = 4,
        YUAN = 5,
        ZHONG = 6
    }
    public void Initialize(GameObject _uiRoot)
    {
        centerPanel = _uiRoot.transform.FindChild("Anim/center").gameObject.GetComponent<UIPanel>();

        Lbl_name = _uiRoot.transform.FindChild("Anim/center/name").gameObject.GetComponent<UILabel>();
        Lbl_level = _uiRoot.transform.FindChild("Anim/center/UpGroup/level").gameObject.GetComponent<UILabel>();
        UIPanel_Panel_player = _uiRoot.transform.FindChild("Anim/center/Panel_player").gameObject;
        player = UIPanel_Panel_player.transform.Find("player").gameObject;
        Slider_ProgressBar = _uiRoot.transform.FindChild("Anim/center/Progress Bar").gameObject.GetComponent<UISlider>();
        Lbl_SliderProgressBarLabel = _uiRoot.transform.FindChild("Anim/center/Progress Bar/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute1 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_1/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute2 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_2/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute3 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_3/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute4 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_1/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute5 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_2/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute6 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_3/Label_attribute").gameObject.GetComponent<UILabel>();
        Grd_GridStar = _uiRoot.transform.FindChild("Anim/center/StarLevelGroup").gameObject.GetComponent<UIGrid>();
        StrengthButtonLabel = _uiRoot.transform.FindChild("Anim/left/SoldierOb/StrengthButton/Label").gameObject.GetComponent<UISprite>();
        PowerLabel = _uiRoot.transform.FindChild("Anim/center/fighting/Label_Fighting").gameObject.GetComponent<UILabel>();
        Debris_Lable = _uiRoot.transform.FindChild("Anim/left/Count").gameObject.GetComponent<UILabel>();
        TalentLabel = _uiRoot.transform.FindChild("Anim/center/Talent").gameObject.GetComponent<UILabel>();
        StarPrompt = _uiRoot.transform.FindChild("Anim/left/SoldierOb/SelectButton/prompt").gameObject;
        StrengthPrompt = _uiRoot.transform.FindChild("Anim/left/SoldierOb/StrengthButton/prompt").gameObject;
        SoldierPos = _uiRoot.transform.FindChild("Anim/center/UpGroup/Position").gameObject.GetComponent<UILabel>();
        lbl_Label_Step = _uiRoot.transform.FindChild("Anim/center/StepBack/Step").gameObject.GetComponent<UILabel>();
        Lbl_StarButton = _uiRoot.transform.FindChild("Anim/left/SoldierOb/SelectButton/Label").gameObject.GetComponent<UILabel>();
        Spt_StarButton = _uiRoot.transform.FindChild("Anim/left/SoldierOb/SelectButton/Background").gameObject.GetComponent<UISprite>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        StarPrompt.gameObject.SetActive(false);
        StrengthPrompt.gameObject.SetActive(false);
    }
    public void init(GameObject _uiRoot, Soldier soldier)
    {
        if (soldierIntensify == null)
        {
            soldierIntensify = new SDIntensifyPanel();
            soldierIntensify.init(_uiRoot);
        }
        if (soldierUpStar == null)
        {
            soldierUpStar = new SDUpStarPanel();
            soldierUpStar.init(_uiRoot);
        }
        if (soldierSelect == null)
        {
            soldierSelect = new SDSelectPanel();
            soldierSelect.init(_uiRoot);
        }
        if (this.soldier != null)
        {
            this.soldier.UpdateAttributeEvent += soldier_UpdateAttributeEvent;
        }
    }

    void soldier_UpdateAttributeEvent()
    {
        this._setAtt();
    }
    public void RefreshPanel(Soldier soldier)
    {
        if (this.soldier != null)
            this.soldier.UpdateAttributeEvent -= soldier_UpdateAttributeEvent;

        this.soldier = soldier;
        if (this.soldier != null)
        {
            _setAtt();
            _SkeleAnimation();
        }
        if (this.soldier != null)
            this.soldier.UpdateAttributeEvent += soldier_UpdateAttributeEvent;
    }

  

    public void OnClose()
    {
        if (this.spine != null)
        {
            GameObject.Destroy(this.spine);
            if (this.soldier != null)
                this.soldier.UpdateAttributeEvent -= soldier_UpdateAttributeEvent;
        }
        this.animation = "";
    }
    public void _setAtt()
    {
        if (soldier == null) return;
        ShowInfoSoldiers soldierInfAtt = soldier.showInfoSoldier;
        SoldierAttributeInfo soldierAtt = soldier.Att;
        if (soldierAtt == null || soldierInfAtt == null) return;
        Lbl_name.text = soldierAtt.Name;

        Lbl_level.text = string.Format("Lv.{0}", soldier.Level.ToString());
        if (soldier._NextLvExp != 0)
            Slider_ProgressBar.value = (float)soldier._CurrentExp / soldier._NextLvExp;
        else
            Slider_ProgressBar.value = 1;

        Lbl_SliderProgressBarLabel.text = string.Format("{0}/{1}", soldier._CurrentExp.ToString(), soldier._NextLvExp.ToString());
        Lbl_Label_attribute1.text = soldierAtt.leaderShip.ToString();
        Lbl_Label_attribute2.text = soldierInfAtt.Attack.ToString();
        Lbl_Label_attribute3.text = soldierInfAtt.HP.ToString();
        Lbl_Label_attribute4.text = soldierInfAtt.Crit.ToString();
        Lbl_Label_attribute5.text = ((int)(soldierInfAtt.AttRate * 1000)).ToString();
        Lbl_Label_attribute6.text = soldierInfAtt.AttDistance.ToString();
        PowerLabel.text = soldierInfAtt.CombatPower.ToString();
        TalentLabel.text = soldierAtt.talent.ToString();
        Lbl_StarButton.text = this.soldier.IsMaxStep() ? ConstString.SOLDIERUPSTEP1 : ConstString.SOLDIERUPSTEP2;
        CommonFunction.SetSpriteName(this.Spt_StarButton, this.soldier.IsMaxStep() ? "ZCJ_bg_jiaanniu" : "CMN_bth_honganniu");
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(null,soldier.StepNum, true);
        if (Grd_GridStar != null)
        {
            var tempList = Grd_GridStar.GetChildList();
            for (int i = 0; i < tempList.Count; ++i)
            {
                GameObject star = tempList[i].FindChild("SelectSprite").gameObject;
                if (star == null)
                    continue;
                if (i < soldier.Att.Star)
                {
                    star.SetActive(true);
                }
                else
                {
                    star.SetActive(false);
                }
            }
        }
        if (StarPrompt)
        {
            if (this.soldier != null && this.soldier.enableUpStar() == SoldierCheck.Ok)
                this.StarPrompt.SetActive(true);
            else
                this.StarPrompt.SetActive(false);
        }

        if (StrengthPrompt != null)
        {
            StrengthPrompt.SetActive(false);
            if (CommonFunction.IsAlreadyBattle(soldier.uId))
            {
                if (!soldier.isMaxLevel())
                {
                    List<ulong> tmpList = CommonFunction.GetWholeMaterialSoldierUID(soldier.uId);
                    Soldier sd = PlayerData.Instance._SoldierDepot.TextStrong(soldier.uId, tmpList);
                    if ((sd != null) && (sd.Level > soldier.Level))
                    {
                        StrengthPrompt.SetActive(true);
                    }
                }
            }
        }

        if (this.soldier.Att.Stance <= this.SeatPos.Length && this.soldier.Att.Career <= this.Carrer.Length && this.soldier.Att.Stance > 0 && this.soldier.Att.Career > 0)
        {
            this.SoldierPos.text = (this.SeatPos[this.soldier.Att.Stance - 1] * 10 + this.Carrer[this.soldier.Att.Career - 1]).ToString();
        }
    }

    public void SetNull()
    {
        Lbl_name.text = "";
        Lbl_level.text = "";

        Slider_ProgressBar.value = 0;
        Lbl_SliderProgressBarLabel.text = "0/0";
        Lbl_Label_attribute1.text = "";
        Lbl_Label_attribute2.text = "";
        Lbl_Label_attribute3.text = "";
        Lbl_Label_attribute4.text = "";
        Lbl_Label_attribute5.text = "";
        Lbl_Label_attribute6.text = "";
        PowerLabel.text = "";
        Debris_Lable.text = "";
        TalentLabel.text = "";

        if (Grd_GridStar != null)
        {
            var tempList = Grd_GridStar.GetChildList();
            for (int i = 0; i < tempList.Count; ++i)
            {
                GameObject star = tempList[i].FindChild("SelectSprite").gameObject;
                star.SetActive(false);
            }
        }
        if (this.spine != null)
        {
            GameObject.Destroy(this.spine);
        }
        if (this.StarPrompt)
        {
            this.StarPrompt.SetActive(false);
        }
        if (StrengthPrompt != null)
        {
            StrengthPrompt.SetActive(false);
        }
        this.soldier = null;
    }
    private void _SkeleAnimation()
    {
        if (UIPanel_Panel_player == null) return;
        if (soldier == null) return;
        if (this.animation.Equals(soldier.Att.Animation) && this.spine != null)
            return;
        if (this.spine != null)
        {
            GameObject.Destroy(this.spine);

        }
        ResourceLoadManager.Instance.LoadCharacter(soldier.Att.Animation, ResourceLoadType.AssetBundle, (obj) =>
        {
            if (obj != null)
            {
                this.animation = soldier.Att.Animation;
                GameObject go = CommonFunction.InstantiateObject(obj, player.transform);
                go.SetActive(true);
                this.spine = go;
                TdSpine.MainSpine mainSpine = go.GetComponent<TdSpine.MainSpine>();
                if (mainSpine == null)
                    mainSpine = go.AddComponent<TdSpine.MainSpine>();
                this.mainSpine = mainSpine;
                this.mainSpine.InitSkeletonAnimation();
                this.mainSpine.StartEvent += mainSpine_StartEvent;
                this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
                go.transform.localScale *= this.soldier.Att.Scale;
                //this.mainSpine.SetColor(new Color(0,0.14f,1));
                Main.Instance.StartCoroutine(SpineSorting());

            }
        });

        UIEventListener.Get(UIPanel_Panel_player.transform.FindChild("player").gameObject).onClick = (go) =>
            {
                if (this.mainSpine == null) return;

                List<string> tempList = new List<string>();

                tempList.Add(GlobalConst.ANIMATION_NAME_MOVE);
                tempList.Add(GlobalConst.ANIMATION_NAME_VICTORY);
                //tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_BASIC);
                tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE);

                int index = UnityEngine.Random.Range(0, tempList.Count - 1);

                this.mainSpine.pushAnimation(tempList[index], true, 1);
                this.mainSpine.EndEvent += (string animationName) =>
                {
                    this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
                };
            };
    }

    void mainSpine_StartEvent(string animationName)
    {
        if (animationName.Equals(GlobalConst.ANIMATION_NAME_IDLE))
        {
            this.mainSpine.gameObject.SetActive(true);
            this.mainSpine.StartEvent -= mainSpine_StartEvent;
        }
    }



    private IEnumerator SpineSorting()
    {
        yield return 0;
        this.mainSpine.setSortingOrder(centerPanel.sortingOrder + 2);
        this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
    }

}
