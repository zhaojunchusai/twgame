using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SoldierIllViewController : UIBase 
{
    public SoldierIllView view;
    public List<Soldier> tempList = new List<Soldier>();
    private List<SoldierIllComponent> _soldierMapItemList = new List<SoldierIllComponent>();

    public override void Initialize()
    {

        if (view == null)
            view = new SoldierIllView();
        view.Initialize();
        BtnEventBinding();
        this.SetInfo(PlayerData.Instance._SoldierMap.GetSoldierMapList());
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
    }
    public void SetInfo(List<Soldier> _data)
    {
        this.tempList = _data;
        Main.Instance.StartCoroutine(CreatSoldierMap(tempList));
    }

    private IEnumerator CreatSoldierMap(List<Soldier> _data)
    {
        this.view.ScrView_EquipAndSkillScrollView.ResetPosition();
        yield return 0.5;
        int count = _data.Count;
        int itemCount = _soldierMapItemList.Count;
        int MaxCount = 24;
        int index = Mathf.CeilToInt((float)count / this.view.UIWrapContent_Grid.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.UIWrapContent_Grid.minIndex = -index;
        this.view.UIWrapContent_Grid.maxIndex = 0;
        this.view.UIWrapContent_Grid.cullContent = true;
        if (count % 6 != 0)
        {
            this.view.UIWrapContent_Grid.cullContent = false;
        }
        else
        {
            this.view.UIWrapContent_Grid.cullContent = true;
        }
        if (count > MaxCount)
        {
            this.view.UIWrapContent_Grid.enabled = true;
            count = MaxCount;
        }
        else
        {
            this.view.UIWrapContent_Grid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _soldierMapItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.item, this.view.Grd_Grid.transform);
                SoldierIllComponent item = vGo.GetComponent<SoldierIllComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<SoldierIllComponent>();
                    item.MyStart(vGo);
                }
                _soldierMapItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _soldierMapItemList[i].TouchEvent += OnItemClick;
            }
            else
            {
                if (_soldierMapItemList[i] != null)
                    _soldierMapItemList[i].gameObject.SetActive(true);
            }
            _soldierMapItemList[i].SetInfo(_data[i]);
        }
        yield return 0;
        this.view.UIWrapContent_Grid.ReGetChild();
        yield return 0;
        this.view.Grd_Grid.Reposition();
        yield return 0;
        this.view.ScrView_EquipAndSkillScrollView.ResetPosition();
        this.view.Grd_Grid.repositionNow = true;
        this.view.Grd_Grid.gameObject.SetActive(false);
        this.view.Grd_Grid.gameObject.SetActive(true);
    }
    public void OnItemClick(SoldierIllComponent comp)
    {
        if(comp != null)
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIERILLINFO);
        UISystem.Instance.SoldierIllInfoView.SetInfo(comp.soldier);
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
        SoldierIllComponent item = this._soldierMapItemList[wrapIndex];
        item.SetInfo(tempList[realIndex]);
    }
    public void ButtonEvent_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SOLDIERILLVIEW);
    }

    public override void Uninitialize()
    {
    }
    public override void Destroy()
    {
        base.Destroy();

        this.view = null;
        if (this.tempList != null)
            this.tempList.Clear();
        if (this._soldierMapItemList != null)
            this._soldierMapItemList.Clear();
    }

    public void BtnEventBinding()
    {
        this.view.UIWrapContent_Grid.onInitializeItem = SetItemInfo;

        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
    }
}
