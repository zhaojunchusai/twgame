using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;

public class NoviceTaskViewController : UIBase
{
    public class NoviceTaskData
    {
        public NoviceTaskInfo info;
        public NewHandTask pod;
    }

    public class NoviceSubtasksData
    {
        public NoviceSubtasksInfo info;
        public TaskInfo pod;
    }

    public NoviceTaskView view;

    private List<NoviceTaskComponent> noviceCompList;
    private List<NoviceSubtasksComponent> subtasksCompList;

    private List<NoviceTaskData> noviceTaskDataList;

    private NoviceTaskData currentTaskData;

    private bool isOpeningTask = false;
    private bool isOpenSubtasks = false;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new NoviceTaskView();
            view.Initialize();
            BtnEventBinding();
        }
        if (noviceCompList == null)
            noviceCompList = new List<NoviceTaskComponent>();
        if (subtasksCompList == null)
            subtasksCompList = new List<NoviceSubtasksComponent>();
        noviceTaskDataList = new List<NoviceTaskData>();
        InitView();
        Scheduler.Instance.AddUpdator(UpdateDeadlineTip);
        NoviceTaskModule.Instance.SendGetNewHandTasks();
    }

    private void InitView()
    {
        view.Gobj_NoviceTaskItem.SetActive(false);
        view.Gobj_TaskAwardsItem.SetActive(false);
        currentTaskData = null;
        isOpenSubtasks = false;
        isOpeningTask = false;
        view.UIWrapContent_ActivitesGrid.onInitializeItem = UpdateWrapNoviceTaskComp;
    }

    #region Button Event
    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_NOVICETASKVIEW);
    }

    public void ButtonEvent_NoviceTask(BaseComponent baseComp)
    {
        NoviceTaskComponent comp = baseComp as NoviceTaskComponent;
        if (comp == null)
            return;
        currentTaskData = new NoviceTaskData();
        currentTaskData.info = comp.NoviceTaskData;
        currentTaskData.pod = comp.NoviceTaskPOD;
        UpdateSelectComp(false);
    }
    public void ButtonEvent_Subtasks(BaseComponent baseComp)
    {
        NoviceSubtasksComponent comp = baseComp as NoviceSubtasksComponent;
        if (comp == null)
            return;

        long deadline = (long)PlayerData.Instance._NoviceTaskEndTime - Main.mTime;
        if (deadline <= 0)  //已经过了新手任务领取时间 
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NOVICETASK_DEADLINETIP_NOAWARD);
            return;
        }
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList((uint)comp.SubtasksData.dropID);
        if (!CommonFunction.GetItemOverflowTip(list, true))
        {
            NoviceTaskModule.Instance.SendGetNewHandTasksAward(currentTaskData.info.ID, comp.SubtasksData.ID);
        }
    }
    #endregion

    #region Update Event

    public void UpdateViewInfo()
    {
        List<NoviceTaskInfo> list = ConfigManager.Instance.mNoviceTaskConfig.GetNoviceTaskInfoList();
        for (int i = 0; i < list.Count; i++)
        {
            NoviceTaskInfo info = list[i];
            NoviceTaskData data = new NoviceTaskData();
            data.info = info;
            NewHandTask pod = PlayerData.Instance._NoviceTaskCompleteList.Find((tmp) =>
            {
                if (tmp == null) return false;
                return tmp.id == info.ID;
            });
            if (pod == null)   //找不到数据 则说明没有完成
            {
                data.pod = new NewHandTask();
                data.pod.id = info.ID;
                data.pod.finish_num = 0;
            }
            else
            {
                data.pod = pod;
            }
            noviceTaskDataList.Add(data);
        }
        Main.Instance.StartCoroutine(UpdateNoviceTaskCompList());
    }

    private void UpdateWrapNoviceTaskComp(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_ActivitesGrid.enabled) return;
        if (realIndex >= noviceTaskDataList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        NoviceTaskComponent comp = noviceCompList[wrapIndex];
        NoviceTaskData data = noviceTaskDataList[realIndex];
        comp.UpdateCompInfo(data.info, data.pod);
        comp.IsSelect = comp.NoviceTaskData.ID.Equals(currentTaskData.info.ID);
    }

    private IEnumerator UpdateNoviceTaskCompList()
    {
        int count = noviceTaskDataList.Count;
        int itemCount = noviceCompList.Count;
        int MAXCOUNT = 7;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_ActivitesGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_ActivitesGrid.minIndex = -index;
        view.UIWrapContent_ActivitesGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_ActivitesGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_ActivitesGrid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                noviceCompList[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            NoviceTaskData info = noviceTaskDataList[i];
            NoviceTaskComponent comp = null;
            if (i < itemCount)
            {
                comp = noviceCompList[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_NoviceTaskItem, view.UIWrapContent_ActivitesGrid.transform);
                vGo.name = "NoviceTaskComp_" + i.ToString();
                comp = new NoviceTaskComponent();
                comp.MyStart(vGo);
                comp.AddEventListener(ButtonEvent_NoviceTask);
                noviceCompList.Add(comp);
            }
            comp.mRootObject.SetActive(true);
            comp.IsSelect = false;
            comp.UpdateCompInfo(info.info, info.pod);
        }
        view.UIWrapContent_ActivitesGrid.ReGetChild();
        yield return 0;
        view.Grd_ActivitesGrid.Reposition();
        yield return 0;
        view.ScrView_ActivitesScrollView.ResetPosition();
        UpdateSelectComp(true);

    }

    private void UpdateSelectComp(bool isDefault)
    {
        if ((noviceCompList == null) || (noviceCompList.Count <= 0))
            return;
        if (isDefault)
        {
            currentTaskData = new NoviceTaskData();
            NoviceTaskComponent comp = noviceCompList[0];
            currentTaskData.info = comp.NoviceTaskData;
            currentTaskData.pod = comp.NoviceTaskPOD;
            comp.IsSelect = true;
        }
        else
        {
            for (int i = 0; i < noviceCompList.Count; i++)
            {
                NoviceTaskComponent comp = noviceCompList[i];
                comp.IsSelect = false;
                if (comp.NoviceTaskData.ID == currentTaskData.info.ID)
                {
                    comp.IsSelect = true;
                }
            }
        }
        view.Lbl_BannerDescLabel.text = currentTaskData.info.desc;
        UpdateNoviceSubtasks();
    }

    private void UpdateNoviceSubtasks()
    {
        List<NoviceSubtasksInfo> list = ConfigManager.Instance.mNoviceSubtasksConfig.GetNoviceSubtasksList(currentTaskData.info.subTasks);
        if (list == null)
        {
            list = new List<NoviceSubtasksInfo>();
        }
        List<NoviceSubtasksData> subtasks = new List<NoviceSubtasksData>();
        List<NoviceSubtasksData> _subFinishTasks = new List<NoviceSubtasksData>();
        List<NoviceSubtasksData> _unFinishTasks = new List<NoviceSubtasksData>();
        List<NoviceSubtasksData> _awardTasks = new List<NoviceSubtasksData>();
        for (int i = 0; i < list.Count; i++)
        {
            NoviceSubtasksInfo info = list[i];
            if (info == null)
            {
                continue;
            }
            NoviceSubtasksData data = new NoviceSubtasksData();
            data.info = info;
            data.pod = currentTaskData.pod.tasks.Find((tmp) =>
            {
                if (tmp == null) return false;
                return tmp.id == info.ID;
            });
            if (data.pod == null)
            {
                data.pod = new TaskInfo();
                data.pod.id = (uint)info.ID;
                data.pod.status = TaskStatus.UNFINISHED;
            }
            switch(data.pod.status)
            {
                case TaskStatus.AWARDED:
                    _awardTasks.Add(data);
                    break;
                case TaskStatus.FINISHED:
                    _subFinishTasks.Add(data);
                    break;
                case TaskStatus.UNFINISHED:
                    _unFinishTasks.Add(data);
                    break;
        }
        } 
        subtasks.AddRange(_subFinishTasks);
        subtasks.AddRange(_unFinishTasks);
        subtasks.AddRange(_awardTasks);
        Main.Instance.StartCoroutine(UpdateNoviceSubtasksCompList(subtasks));
    }

    private IEnumerator UpdateNoviceSubtasksCompList(List<NoviceSubtasksData> list)
    {
        isOpenSubtasks = true;
        int count = list.Count;
        int itemCount = subtasksCompList.Count;
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                if (!isOpenSubtasks) yield break;
                subtasksCompList[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (!isOpenSubtasks) yield break;
            NoviceSubtasksData info = list[i];
            NoviceSubtasksComponent comp = null;
            if (i < itemCount)
            {
                comp = subtasksCompList[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_TaskAwardsItem, view.Grd_DescGrid.transform);
                vGo.name = "SubtasksComp_" + i.ToString();
                comp = new NoviceSubtasksComponent();
                comp.MyStart(vGo);
                comp.OnSelectObj = ButtonEvent_Subtasks;
                subtasksCompList.Add(comp);
            }
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(info.info, info.pod);
        }
        isOpenSubtasks = false;
        view.Grd_DescGrid.Reposition();
        yield return 0;
        view.ScrView_DescScrollView.ResetPosition();
    }

    private void UpdateDeadlineTip()
    {
        long deadline = (long)PlayerData.Instance._NoviceTaskEndTime - Main.mTime;

        if (deadline <= 0)
        {
            deadline = 0;
            view.Lbl_DeadLineTip.text = "[f5b800]" + string.Format(ConstString.NOVICETASK_DEADLINETIP, "[44d83d]" + 0 + "[-]", "[44d83d]" + 0 + "[-]") + "[-]";
            Scheduler.Instance.RemoveUpdator(UpdateDeadlineTip);
        }
        else
        {
            long day = deadline / 86400;
            long time = deadline % 86400;
            string str = CommonFunction.GetTimeString(time);
            view.Lbl_DeadLineTip.text = "[f5b800]" + string.Format(ConstString.NOVICETASK_DEADLINETIP, "[44d83d]" + day + "[-]", "[44d83d]" + str + "[-]") + "[-]";
        }
    }

    #endregion

    public void ReceiveAwardSuccess(int pid, int cid, DropList dropItem)
    {
        for (int i = 0; i < subtasksCompList.Count; i++)
        {
            NoviceSubtasksComponent comp = subtasksCompList[i];
            if (comp == null)
                continue;
            if (cid == comp.SubtasksData.ID)
            {
                comp.SubtasksPOD.status = TaskStatus.AWARDED;
                comp.UpdateAwardBtnStatus();
                break;
            }
        }
        for (int i = 0; i < PlayerData.Instance._NoviceTaskCompleteList.Count; i++)
        {
            NewHandTask task = PlayerData.Instance._NoviceTaskCompleteList[i];
            if (task == null)
                continue;
            if (task.id == currentTaskData.info.ID)
            {
                currentTaskData.pod = task;
            }
        }
        for (int i = 0; i < noviceCompList.Count; i++)
        {
            NoviceTaskComponent comp = noviceCompList[i];
            if (comp.NoviceTaskData.ID == currentTaskData.info.ID)
            {
                comp.NoviceTaskPOD = currentTaskData.pod;
                comp.UpdateNotifyStatus();
                break;
            }
        }
        List<fogs.proto.msg.ItemInfo> specialList = new List<fogs.proto.msg.ItemInfo>();
        specialList.AddRange(dropItem.special_list);
        specialList.AddRange(dropItem.item_list);
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(specialList, dropItem.equip_list, dropItem.soldier_list);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_SUCCESS);
        UpdateNoviceSubtasks();
    }

    public override void Uninitialize()
    {
        Main.Instance.StopCoroutine(UpdateNoviceTaskCompList());
        Scheduler.Instance.RemoveUpdator(UpdateDeadlineTip);
        noviceTaskDataList.Clear();
        isOpenSubtasks = false;
        isOpeningTask = false;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        noviceCompList.Clear();
        subtasksCompList.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
    }


}
