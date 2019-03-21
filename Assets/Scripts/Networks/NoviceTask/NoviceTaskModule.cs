using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;
using System;
using Assets.Script.Common;
public class NoviceTaskModule : Singleton<NoviceTaskModule>
{
    public NoviceTaskNetWork mNoviceTaskNetWork;
    public void Initialize()
    {
        if (mNoviceTaskNetWork == null)
        {
            mNoviceTaskNetWork = new NoviceTaskNetWork();
            mNoviceTaskNetWork.RegisterMsg();
        }
    }
    public void SendGetNewHandTasks()
    {
        GetNewHandTasksReq data = new GetNewHandTasksReq();
        mNoviceTaskNetWork.SendGetNewHandTasks(data);
    }
    public void ReceiveGetNewHandTasks(GetNewHandTasksResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance._NoviceTaskCompleteList = resp.new_hand_tasks;
            UISystem.Instance.NoviceTaskView.UpdateViewInfo();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void SendGetNewHandTasksAward(int pid, int cid)
    {
        GetNewHandTasksAwardReq data = new GetNewHandTasksAwardReq();
        data.pid = pid;
        data.cid = cid;
        mNoviceTaskNetWork.SendGetNewHandTasksAward(data);
    }
    public void ReceiveGetNewHandTasksAward(GetNewHandTasksAwardResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateDropData(resp.drop_item);
            CommonFunction.ShowItemOverflowTip(resp.drop_item.mail_list, true);
            for (int i = 0; i < PlayerData.Instance._NoviceTaskCompleteList.Count; i++)
            {
                NewHandTask task = PlayerData.Instance._NoviceTaskCompleteList[i];
                if (task == null)
                    continue;
                if (task.id == resp.pid)
                {
                    if (task.tasks.Count <= 0)
                    {
                        task.tasks.Add(resp.new_hand_task);
                    }
                    else
                    {
                        for (int j = 0; j < task.tasks.Count; j++)
                        {
                            TaskInfo info = task.tasks[j];
                            if (info == null)
                                continue;
                            if (info.id == resp.cid)
                            {
                                info = resp.new_hand_task;
                            }
                        }
                    }
                }
            }
            UISystem.Instance.NoviceTaskView.ReceiveAwardSuccess(resp.pid, resp.cid,resp.drop_item);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void Uninitialize()
    {
        if (mNoviceTaskNetWork != null)
            mNoviceTaskNetWork.RemoveMsg();
        mNoviceTaskNetWork = null;
    }
}