using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class LivenessViewController : UIBase 
{
    public LivenessView view;
    private UILabel[] _boxLivenessPoints;
    private UISprite[] _boxLivenessIcon;
    private GameObject[] _boxLivenessReceives;
    private GameObject[] _boxs;
    private List<LivenessItem> _livenessItems = new List<LivenessItem>();
    private List<LivenessAward> _livenessAwardConfigList;

    public override void Initialize()
    {
        if (view == null)
            view = new LivenessView();
        view.Initialize();
        BtnEventBinding();
        _boxLivenessPoints = new UILabel[] { view.Lbl_BtnBox1Num, view.Lbl_BtnBox2Num, view.Lbl_BtnBox3Num, view.Lbl_BtnBox4Num, view.Lbl_BtnBox5Num };
        _boxLivenessIcon = new UISprite[] { view.Spt_BtnBox1Icon, view.Spt_BtnBox2Icon, view.Spt_BtnBox3Icon, view.Spt_BtnBox4Icon, view.Spt_BtnBox5Icon };
        _boxLivenessReceives = new GameObject[] { view.Go_ReceiveBox1, view.Go_ReceiveBox2, view.Go_ReceiveBox3, view.Go_ReceiveBox4, view.Go_ReceiveBox5 };
        _boxs = new GameObject[] { view.Btn_Box1.gameObject, view.Btn_Box2.gameObject, view.Btn_Box3.gameObject, view.Btn_Box4.gameObject, view.Btn_Box5.gameObject, };
        TaskModule.Instance.NeedRefreshLiveness = true;
        InitUI();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL , view._uiRoot.transform.parent.transform));
       // PlayOpenAnim();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenLivenessView);
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(LivenessView.UIName);
    }

    public void InitUI()
    {
        if(TaskModule.Instance.NeedRefreshLiveness)
        {
            TaskModule.Instance.UpdateLivenessDataReq();
            return;
        }

        view.Lbl_ProgressDesc.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION,
                                                   PlayerData.Instance.LivenessInfo.liveness,
                                                   ConfigManager.Instance.mLivenessConfig.MaxLivenessPoint);
        view.Slider_Progress.value = (float) PlayerData.Instance.LivenessInfo.liveness/
                                     ConfigManager.Instance.mLivenessConfig.MaxLivenessPoint;
        _livenessAwardConfigList = ConfigManager.Instance.mLivenessConfig.GetLivenessAwardList();

        for (int i = 0; i < 5; i++)
        {
            if (i < _livenessAwardConfigList.Count)
            {
                SetLivenessBoxPos(i, _livenessAwardConfigList[i].LivenessPoint, ConfigManager.Instance.mLivenessConfig.MaxLivenessPoint);
            }
            else
            {
                SetLivenessBoxPos(i, 0, 0);                
            }
        }

        for (int i = 0; i < _livenessAwardConfigList.Count; i++)
        {
            SetLivenessBox(i, _livenessAwardConfigList[i]);
        }
        
        ShowTasks();
        SetLivenessReceive();
    }

    private void ShowTasks()
    {
        List<LivenessTask> list = PlayerData.Instance.LivenessInfo.liveness_task;
        list.Sort(SortTool.SortLivenessTask);
        for (int i = 0; i < list.Count; i++)
        {
            if(i >= _livenessItems.Count)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_LivenessItem, view.Grd_Items.transform);
                _livenessItems.Add(go.GetComponent<LivenessItem>());
            }
            if (!_livenessItems[i].gameObject.activeSelf)
            {
                _livenessItems[i].gameObject.SetActive(true);
            }

            _livenessItems[i].InitItem(list[i]);
        }

        for (int i = list.Count; i < _livenessItems.Count; i++)
        {
            _livenessItems[i].gameObject.SetActive(false);
        }

        view.Grd_Items.Reposition();
    }

    private void SetLivenessBoxPos(int index,uint point ,uint max)
    {

        if(point == 0&& max == 0)
        {
            _boxs[index].SetActive(false);
        }
        else
        {
            _boxs[index].SetActive(true);
            _boxs[index].transform.localPosition = new Vector3(
                view.Slider_Progress.transform.localPosition.x + 
                (view.Spt_SliderProgressBackground.width + view.Spt_SliderProgressBackground.transform.localPosition.x) * ((float)point/max)
                ,_boxs[index].transform.localPosition.y,0);

        }
    }

    private void SetLivenessBox(int index,LivenessAward award)
    {
        if(index >= 5)
            return;

        _boxLivenessPoints[index].text = award.LivenessPoint.ToString();
        
        if(PlayerData.Instance.LivenessInfo.award_list.Contains((int)award.ID))
        {
            CommonFunction.SetSpriteName(_boxLivenessIcon[index], GetBoxIconNameByState(index,true));
        }
        else
        {
            CommonFunction.SetSpriteName(_boxLivenessIcon[index], GetBoxIconNameByState(index, false));
        }
    }
    private string GetBoxIconNameByState(int index,bool state)
    {
        string str = "";
        switch (index)
        {
            case 0:
                {
                    str = state ? GlobalConst.SpriteName.LivenessBoxIconsOpen[index] : GlobalConst.SpriteName.LivenessBoxIcons[index];
                    break;
                }
            case 1:
                {
                    str = state ? GlobalConst.SpriteName.LivenessBoxIconsOpen[index] : GlobalConst.SpriteName.LivenessBoxIcons[index];
                    break;
                }
            case 2:
                {
                    str = state ? GlobalConst.SpriteName.LivenessBoxIconsOpen[3] : GlobalConst.SpriteName.LivenessBoxIcons[3];
                    break;
                }
            case 3:
                {
                    str = state ? GlobalConst.SpriteName.LivenessBoxIconsOpen[4] : GlobalConst.SpriteName.LivenessBoxIcons[4];
                    break;
                }
            case 4:
                {
                    str = state ? GlobalConst.SpriteName.LivenessBoxIconsOpen[5] : GlobalConst.SpriteName.LivenessBoxIcons[5];
                    break;
                }
            case 5:
                {
                    str = state ? GlobalConst.SpriteName.LivenessBoxIconsOpen[index] : GlobalConst.SpriteName.LivenessBoxIcons[index];
                    break;
                }
        }
        return str;
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close , view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(LivenessView.UIName);

    }

    public void ButtonEvent_Box(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        int index = int.Parse(btn.name.Substring(btn.name.Length - 1, 1)) -1;
        if(PlayerData.Instance.LivenessInfo.award_list.Contains((int)_livenessAwardConfigList[index].ID))
        {
            return;
        }

        if(PlayerData.Instance.LivenessInfo.liveness >= _livenessAwardConfigList[index].LivenessPoint)
        {
            List<CommonItemData> data = new List<CommonItemData>();
            data.AddRange(CommonFunction.GetCommonItemDataList(_livenessAwardConfigList[index].DropID));
            if (CommonFunction.GetItemOverflowTip(data))
            {
                return;
            }
            //领取
            TaskModule.Instance.LivenessRewardReq(_livenessAwardConfigList[index].ID);
        }
        else
        {
            //查看
            SeeBoxDetail(new CommonItemData(GlobalCoefficient.PlayerExpID,_livenessAwardConfigList[index].Exp),_livenessAwardConfigList[index].DropID);
        }
    }
    public void SetLivenessReceive()//设置可领取提示
    {

        for (int i=0;i <_boxLivenessReceives.Length ;i++)
        {
            _livenessAwardConfigList = ConfigManager.Instance.mLivenessConfig.GetLivenessAwardList();

          //  Debug.LogError("--------------------" + i + "--------------------");
           // Debug.LogError("PlayerData.Instance.LivenessInfo.liveness = " + PlayerData.Instance.LivenessInfo.liveness);
           // Debug.LogError("_livenessAwardConfigList[i].LivenessPoint = " + _livenessAwardConfigList[i].LivenessPoint);
            if (PlayerData.Instance.LivenessInfo.liveness >= _livenessAwardConfigList[i].LivenessPoint)
            {
              //  Debug.LogError("PlayerData.Instance.LivenessInfo.award_list.Contains((int)_livenessAwardConfigList[i].ID) = " + PlayerData.Instance.LivenessInfo.award_list.Contains((int)_livenessAwardConfigList[i].ID));
                if (PlayerData.Instance.LivenessInfo.award_list.Contains((int)_livenessAwardConfigList[i].ID))
                    _boxLivenessReceives[i].SetActive(false );    
                else 
                    _boxLivenessReceives[i].SetActive(true );
            }
            else
            {

                _boxLivenessReceives[i].SetActive(false);
            }

        }
        

    }
    private void SeeBoxDetail(CommonItemData data,uint dropid)
    {
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(dropid);
        list.Add(data);
        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, true, ConstString.RECEIVE_CANGET);
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (_livenessAwardConfigList != null)
            _livenessAwardConfigList.Clear();
        if (_livenessItems != null && _livenessItems.Count > 0)
        {
            int count = _livenessItems.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                LivenessItem item = _livenessItems[i];
                _livenessItems.Remove(item);
                GameObject.Destroy(item.gameObject);
            }
            _livenessItems.Clear();
        }
    }


    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_Box1.gameObject).onClick = ButtonEvent_Box;
        UIEventListener.Get(view.Btn_Box2.gameObject).onClick = ButtonEvent_Box;
        UIEventListener.Get(view.Btn_Box3.gameObject).onClick = ButtonEvent_Box;
        UIEventListener.Get(view.Btn_Box4.gameObject).onClick = ButtonEvent_Box;
        UIEventListener.Get(view.Btn_Box5.gameObject).onClick = ButtonEvent_Box;
    }
    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();
    //}

}
