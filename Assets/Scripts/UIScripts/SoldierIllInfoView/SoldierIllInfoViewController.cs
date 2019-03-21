using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class SoldierIllInfoViewController : UIBase
{
    public SoldierIllInfoView view;
    private Soldier tempSoldier;
    private List<MapSkillComponent> _skillItemList = new List<MapSkillComponent>();
    private List<string> _loadResName = new List<string>();
    static int SkillMaxCount = 6;
    private List<Skill> tempList;
    private GameObject spine;
    private string animation = "";
    private TdSpine.MainSpine mainSpine;

    public override void Initialize()
    {
        if (view == null)
            view = new SoldierIllInfoView();
        view.Initialize();
        _loadResName.Clear();
        BtnEventBinding();
    }
    public void SetInfo(Soldier sd)
    {
        this.tempSoldier = sd;
        this.SetAtt();
        this.tempList = sd._skillsDepot._skillsList;
        Main.Instance.StartCoroutine(CreatSkillItem(tempList));
        this._SkeleAnimation();
    }
    private void SetAtt()
    {
        if (tempSoldier == null) return;

        ShowInfoSoldiers soldierInfAtt = tempSoldier.showInfoSoldier;
        SoldierAttributeInfo soldierAtt = tempSoldier.Att;
        if (soldierAtt == null || soldierInfAtt == null) return;
        this.view.Lbl_name.text = soldierAtt.Name;

        this.view.Lbl_Label_attribute1.text = soldierAtt.leaderShip.ToString();
        this.view.Lbl_Label_attribute2.text = soldierInfAtt.Attack.ToString();
        this.view.Lbl_Label_attribute3.text = soldierInfAtt.HP.ToString();
        this.view.Lbl_Label_attribute4.text = soldierInfAtt.Crit.ToString();
        this.view.Lbl_Label_attribute5.text = ((int)(soldierInfAtt.AttRate * 1000)).ToString();
        this.view.Lbl_Label_attribute6.text = soldierInfAtt.AttDistance.ToString();
        this.view.Lbl_Label_Fighting.text = tempSoldier.GetCombatPower().ToString();
        this.view.Lbl_Talent.text = soldierAtt.talent.ToString();
        soldierContent[0] = soldierAtt.SoldierPos;
        soldierContent[1] = soldierAtt.SoldierStory;
        Assets.Script.Common.Scheduler.Instance.AddFrame(1, false, SetSoldierLabel);
    }

    private string[] soldierContent = new string[2];
    private void SetSoldierLabel()
    {
        this.view.Lbl_Label_SoldierPos.text = soldierContent[0];
        this.view.Lbl_Label_SoldierStory.text = soldierContent[1];
        this.view.DetailTabel.repositionNow = true;
    }

    private void _SkeleAnimation()
    {
        if (this.view.Spt_player == null) return;
        if (this.tempSoldier == null) return;
        if (this.animation.Equals(this.tempSoldier.Att.Animation) && this.spine != null)
            return;
        _loadResName.Add(ResourceLoadManager.Instance.GetABNameByType(this.tempSoldier.Att.Animation, ResourceType.Character));
        if (this.spine != null)
        {
            GameObject.Destroy(this.spine);
            if (mainSpine != null)
                this.mainSpine.UnInit();
            this.mainSpine = null;
            this.spine = null;
        }

        ResourceLoadManager.Instance.LoadCharacter(this.tempSoldier.Att.Animation, ResourceLoadType.AssetBundle, (obj) =>
        {
            if (obj != null)
            {
                this.animation = this.tempSoldier.Att.Animation;
                GameObject go = CommonFunction.InstantiateObject(obj, this.view.Spt_player.transform);
                go.SetActive(true);
                this.spine = go;
                TdSpine.MainSpine tempmainSpine = go.GetComponent<TdSpine.MainSpine>();
                if (tempmainSpine == null)
                    tempmainSpine = go.AddComponent<TdSpine.MainSpine>();
                this.mainSpine = tempmainSpine;
                this.mainSpine.InitSkeletonAnimation();
                go.transform.localScale *= this.tempSoldier.Att.Scale;
                this.mainSpine.StartEvent += mainSpine_StartEvent;
                this.mainSpine.gameObject.SetActive(false);
                this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
                Main.Instance.StartCoroutine(SpineSorting());

            }
        });

        UIEventListener.Get(this.view.Spt_player.gameObject).onClick = (go) =>
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
        this.mainSpine.setSortingOrder(this.view.UIPanel_SoldierIllInfoView.sortingOrder + 1);
        this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
    }

    private IEnumerator CreatSkillItem(List<Skill> _data)
    {
        this.view.ScrView_EquipAndSkillScrollView.ResetPosition();
        yield return 0;
        int count = _data.Count;
        int itemCount = _skillItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.UIWrapContent_Grid.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.UIWrapContent_Grid.minIndex = -index;
        this.view.UIWrapContent_Grid.maxIndex = 0;

        if (count > SkillMaxCount)
        {
            this.view.UIWrapContent_Grid.enabled = true;
            count = SkillMaxCount;
        }
        else
        {
            this.view.UIWrapContent_Grid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _skillItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.item, this.view.Grd_Grid_EquipAndSkillScrollView.transform);
                MapSkillComponent item = vGo.GetComponent<MapSkillComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<MapSkillComponent>();
                    item.isSoldier = true;
                    item.MyStart(vGo);
                }
                _skillItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _skillItemList[i].TouchEvent += OnItemTouch;
            }
            else
            {
                _skillItemList[i].gameObject.SetActive(true);
            }
            _skillItemList[i].SetInfo(_data[i]);
        }

        if (count > 1)
            this.view.UIWrapContent_Grid.ReGetChild();
        yield return 0;
        this.view.Grd_Grid_EquipAndSkillScrollView.Reposition();
        this.view.Grd_Grid_EquipAndSkillScrollView.gameObject.SetActive(false);
        this.view.Grd_Grid_EquipAndSkillScrollView.gameObject.SetActive(true);
        yield return 0;
        this.view.ScrView_EquipAndSkillScrollView.ResetPosition();
    }
    private void OnItemTouch(MapSkillComponent comp)
    {
        return;
    }

    public override void Uninitialize()
    {
        if (this.spine != null)
        {
            GameObject.Destroy(this.spine);
        }
        if (mainSpine != null)
            this.mainSpine.UnInit();
        this.tempSoldier = null;
        spine = null;
        this.mainSpine = null;
        ReleaseBundle();
    }

    public void ReleaseBundle()
    {
        //Debug.LogError (_loadResName.Count);
        ResourceLoadManager.Instance.ReleaseBundleForName(_loadResName);
        _loadResName.Clear();
    }

    public void ButtonEvent_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SOLDIERILLINFO);
    }
    public void ButtonEvent_Button_left(GameObject btn)
    {
        int index = PlayerData.Instance._SoldierMap.FindIndex(this.tempSoldier);
        if (index <= 0)
        {
            index = PlayerData.Instance._SoldierMap.GetSoldierMapList().Count;
        }

        this.SetInfo(PlayerData.Instance._SoldierMap.GetSoldierMapList()[index - 1]);
    }
    public void ButtonEvent_Button_right(GameObject btn)
    {
        int index = PlayerData.Instance._SoldierMap.FindIndex(this.tempSoldier);
        if (index < 0 || index == PlayerData.Instance._SoldierMap.GetSoldierMapList().Count - 1)
        {
            index = -1;
        }

        this.SetInfo(PlayerData.Instance._SoldierMap.GetSoldierMapList()[index + 1]);
    }
    public void SetItemInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tempList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        MapSkillComponent item = _skillItemList[wrapIndex];
        item.SetInfo(tempList[realIndex]);
    }
    public void PressEvent_SuitEquipAtt(GameObject go, bool isPress)
    {
        if (isPress)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SUITEQUIPATT);
            if (UISystem.Instance.SuitEquipAttView != null)
                UISystem.Instance.SuitEquipAttView.UpdateInfo(this.tempSoldier);
        }
        else
        {
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SUITEQUIPATT);
        }
    }
    public override void Destroy()
    {
        base.Destroy();
        _loadResName.Clear();
        this.view = null;
        if (this._skillItemList != null)
            this._skillItemList.Clear();
        if (this.tempList != null)
            this.tempList.Clear();
    }
    public void BtnEventBinding()
    {
        this.view.UIWrapContent_Grid.onInitializeItem = SetItemInfo;

        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
        UIEventListener.Get(view.Btn_Button_left.gameObject).onClick = ButtonEvent_Button_left;
        UIEventListener.Get(view.Btn_Button_right.gameObject).onClick = ButtonEvent_Button_right;
        UIEventListener.Get(view.SuitEquipButton.gameObject).onPress = PressEvent_SuitEquipAtt;
    }
}
