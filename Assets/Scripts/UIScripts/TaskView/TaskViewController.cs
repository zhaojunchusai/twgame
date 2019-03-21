using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using System.Linq;
public class TaskViewController : UIBase
{
    const int MAXCOUNT = 6;
    const string TASKITEMNAME = "TaskItem";
    public TaskView view;
    private GameObject _taskItemObj;
    private List<TaskItem> _taskItemList = new List<TaskItem>();
    private List<fogs.proto.msg.TaskInfo> _taskData = new List<fogs.proto.msg.TaskInfo>();
    private List<fogs.proto.msg.TaskInfo> tempFinishedList = new List<fogs.proto.msg.TaskInfo>();
    private List<fogs.proto.msg.TaskInfo> tempUnFinishedList = new List<fogs.proto.msg.TaskInfo>();
    public override void Initialize()
    {
        if (view == null)
        {
            view = new TaskView();
            view.Initialize();
            BtnEventBinding();
        }
        LoadRes();
        SendGetTaskList();
        //PlayOpenAnim();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        //UISystem.Instance.ShowGameUI(TopFuncView.UIName);
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenTaskView);

    }

    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart(); view.Anim_TScale.PlayForward();
    //}

    public override void Destroy()
    {
        view = null;
        _taskItemList.Clear();
        _taskData.Clear();
        tempFinishedList.Clear();
        tempUnFinishedList.Clear();
    }

    public void LoadRes()
    {
        if (_taskItemObj == null)
            _taskItemObj = view.ScrView_TaskGroup.transform.FindChild(TASKITEMNAME).gameObject;
        if (_taskItemObj == null) Debug.LogError("_taskItemObj == null");
        _taskItemObj.SetActive(false);
    }

    public void SendGetTaskList()
    {
        //if (_taskData.Count <=0 || TaskModule.Instance.IsNameTask)
        // {
        TaskModule.Instance.SendGetTaskList();
        // }
    }

    public void ButtonEvent_BackBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(TaskView.UIName);
    }

    public void InitTaskItem(List<fogs.proto.msg.TaskInfo> _data)
    {
        Main.Instance.StartCoroutine(CreatTaskItem(_data));
    }

    private IEnumerator CreatTaskItem(List<fogs.proto.msg.TaskInfo> _data)
    {
        //view.ScrView_TaskGroup.ResetPosition();
        //yield return 0;
        tempFinishedList.Clear();
        _taskData.Clear();
        tempUnFinishedList.Clear();
        for (int i = 0; i < _data.Count; i++)
        {
            if (_data[i].status == TaskStatus.FINISHED)
                tempFinishedList.Add(_data[i]);
            else
                tempUnFinishedList.Add(_data[i]);
        }
        _taskData.AddRange(tempFinishedList.OrderBy(s => s.id).ToList());
        _taskData.AddRange(tempUnFinishedList.OrderBy(s => s.id).ToList());
        int count = _taskData.Count;
        int itemCount = _taskItemList.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_UIGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_UIGrid.minIndex = -index;
        view.UIWrapContent_UIGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_UIGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_UIGrid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                _taskItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(_taskItemObj, view.Grd_UIGrid.transform);
                TaskItem item = vGo.GetComponent<TaskItem>();
                vGo.SetActive(true);
                _taskItemList.Insert(i, item);
                vGo.name = i.ToString();
                _taskItemList[i].EventGetTaskAward = SendGetTaskAwards;
                _taskItemList[i].EventGoTotarget = GotoTarget;
            }
            else
            {
                _taskItemList[i].gameObject.SetActive(true);
            }
            _taskItemList[i].SetInfo(_taskData[i]);
        }
        view.UIWrapContent_UIGrid.ReGetChild();
        yield return 0;
        view.Grd_UIGrid.repositionNow = true;
        yield return 0;
        view.ScrView_TaskGroup.ResetPosition();
        view.Grd_UIGrid.repositionNow = true;
    }

    public void GotoTarget(string str)
    {
        CommonFunction.OpenTargetView(str);
    }

    public void SetTaskInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _taskData.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        TaskItem item = _taskItemList[wrapIndex];
        item.SetInfo(_taskData[realIndex]);
    }

    public void SendGetTaskAwards(uint taskid)
    {
        TaskData tempData = ConfigManager.Instance.mTaskConfig.GetTaskDataByID(taskid);
        if (tempData == null)
        {
            Debug.LogError(" SendGetTaskAwards TaskID = " + taskid);
            return;
        }
        List<CommonItemData> data = new List<CommonItemData>();
        data.AddRange(CommonFunction.GetCommonItemDataList(tempData.Awards));
        if (CommonFunction.GetItemOverflowTip(data))
        {
            return;
        }
        TaskModule.Instance.SendGetTaskAward(taskid);
    }

    public void ReceiveAward(GetTaskAwardsResp data)
    {
        TalkingDataManager.Instance.OnCompleted(data.id.ToString());
        InitTaskItem(data.tasks);
        PlayerData.Instance.UpdateItem(data.awards_common);
        PlayerData.Instance.MultipleAddWeapon(data.awards_equip);
        PlayerData.Instance.MultipleAddSoldier(data.awards_soldier);
        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        List<CommonItemData> _dataList = new List<CommonItemData>();
        _dataList.AddRange(CommonFunction.GetCommonItemDataList(data.awards_common, data.awards_equip, data.awards_soldier));
        if (data.add_exp != null)
        {
            CommonItemData expData = new CommonItemData(GlobalCoefficient.PlayerExpID, data.add_exp.add_exp);
            PlayerData.Instance.UpdateExpInfo(data.add_exp);
            _dataList.Add(expData);
        }
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(_dataList);
        UISystem.Instance.RecieveResultVertView.SetTitle(ConstString.RECEIVE_SUCCESS);
        UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_BG.gameObject).onClick = ButtonEvent_BackBtn;
        view.UIWrapContent_UIGrid.onInitializeItem = SetTaskInfo;
    }
}
