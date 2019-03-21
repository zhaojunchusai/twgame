using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;
using ItemInfo = fogs.proto.msg.ItemInfo;
using Soldier = fogs.proto.msg.Soldier;

public class TaskModule : Singleton<TaskModule>
{
    public List<CommonItemData> Livenesslist = new List<CommonItemData>();
    private TaskNetWork mNetWork;
    private bool _isNewTask = true;
    public bool NeedRefreshLiveness = false;
    public bool IsSignAward = false;
    public uint LastTaskAwardID = 0;
    public Dictionary<ETaskOpenView, OpenFunctionType> DicTaskJumpTo;//跳转对应数据//
    
    public void Initialize()
    {
        if (mNetWork == null)
        {
            mNetWork = new TaskNetWork();
            mNetWork.RegisterMsg();
        }
        _isNewTask = true;
        NeedRefreshLiveness = false;
        ObtainJumpToInfo();
    }

    public void SendGetTaskList()
    {
        GetTaskListReq data = new GetTaskListReq();
        mNetWork.SendGetTaskList(data);
    }

    public void SendGetTaskAward(uint id)
    {
        GetTaskAwardsReq data = new GetTaskAwardsReq();
        data.id = id;
        mNetWork.SendGetTaskAward(data);
    }

    public void ReceiveTaskList(GetTaskListResp data)
    {

        if (data.result != 0)
        {
            ErrorCode.ShowErrorTip(data.result);
            return;
        }
        _isNewTask = false;
        UISystem.Instance.TaskView.InitTaskItem(data.tasks);
    }

    public void ReceiveAward(GetTaskAwardsResp data)
    {
        if (data.result != 0)
        {
            ErrorCode.ShowErrorTip(data.result);
            return;
        }
        LastTaskAwardID = data.id;
        UISystem.Instance.TaskView.ReceiveAward(data);
    }

    public void NotifyRefreshTask(NotifyRefresh resp)
    {
        _isNewTask = true;
    }

    public void NotifyLiveness(NotifyRefresh resp)
    {
        NeedRefreshLiveness = true;
        if (UISystem.Instance.UIIsOpen(LivenessView.UIName))
        {
            UISystem.Instance.LivenessView.InitUI();
        }
        else
        {
            UpdateLivenessDataReq(false);
        }
    }

    public void NotifySign(NotifyRefresh resp)
    {

    }

    public void UpdateLivenessDataReq(bool needmask = true)
    {
        Debug.Log("UpdateLivenessDataReq");
        NeedRefreshLiveness = false;
        mNetWork.UpdateLivenessDataReq(needmask);
    }
    public void UpdateLivenessDataResp(UpdateLivenessDataResp data)
    {
        Debug.Log("UpdateLivenessDataResp");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.LivenessInfo = data.liveness_info;
            if (UISystem.Instance.UIIsOpen(LivenessView.UIName))
            {
                UISystem.Instance.LivenessView.InitUI();
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void LivenessRewardReq(uint id)
    {
        Debug.Log("LivenessRewardReq");
        LivenessRewardReq req = new LivenessRewardReq();
        req.id = (int)id;
        mNetWork.LivenessRewardReq(req);
    }
    public void LivenessRewardResp(LivenessRewardResp data)
    {
        IsSignAward = false;
        Debug.Log("LivenessRewardResp");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.LivenessInfo.award_list.Add(data.id);
            PlayerData.Instance.UpdateItem(data.update_item);
            PlayerData.Instance.UpdateExpInfo(data.add_exp);
            PlayerData.Instance.MultipleAddSoldier(data.update_sodier);
            PlayerData.Instance.MultipleAddWeapon(data.update_equip);
            PlayerData.Instance.MultipleAddWeapon(data.update_soldierequip);

            if (UISystem.Instance.UIIsOpen(LivenessView.UIName))
            {

                UISystem.Instance.LivenessView.InitUI();

            }

            List<Equip> equips = new List<Equip>();
            equips.AddRange(data.update_equip);
            equips.AddRange(data.update_soldierequip);

            if (data.add_exp.add_exp > 0)
            {
                fogs.proto.msg.ItemInfo info = new fogs.proto.msg.ItemInfo();
                info.id = GlobalCoefficient.PlayerExpID;
                info.change_num = data.add_exp.add_exp;
                data.update_item.Add(info);
            }
            ShowAwardResult(data.update_item, equips, data.update_sodier);
            UISystem.Instance.LivenessView.SetLivenessReceive();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    private void ShowAwardResult(List<fogs.proto.msg.ItemInfo> itemInfos, List<Equip> equips, List<fogs.proto.msg.Soldier> soldiers)
    {
        // Livenesslist.Clear();
        Livenesslist = CommonFunction.GetCommonItemDataList(itemInfos, equips, soldiers);
        if (IsSignAward)//签到
        {
            UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(Livenesslist);
        }
        else//活跃
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_OPENCHESTSEFFECT);
            UISystem.Instance.OpenChestsEffect.isLiveness = false;
        }
        //  UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        // UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list);
    }
    /*
    public void LivenessNotify()
    {
        NeedRefreshLiveness = true;
        Debug.Log("LivenessNotify");
        if(UISystem.Instance.UIIsOpen(LivenessView.UIName))
        {
            UISystem.Instance.LivenessView.InitUI();
        }
    }
    */
    public void ContinuAwardReq()
    {
        IsSignAward = true;
        mNetWork.ContinuAwardReq();
    }
    public void ContinuAwardResp(ContinuAwardResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.LoginChangeInfo.get_continu_award = (int)ELoginAwardGetContinous.Got;
            PlayerData.Instance.UpdateItem(resp.iteminfo);
            if (resp.soldier != null)
                PlayerData.Instance.MultipleAddSoldier(resp.soldier);
            if (resp.equip != null)
                PlayerData.Instance.MultipleAddWeapon(resp.equip);

            UISystem.Instance.SignView.UpdateContinuousSignItemState();

            ShowAwardResult(resp.iteminfo, resp.equip, resp.soldier);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void CumulativeAwardReq(int num)
    {
        IsSignAward = true;
        CumulativeAwardReq req = new CumulativeAwardReq();
        req.num = num;
        mNetWork.CumulativeAwardReq(req);
    }
    public void CumulativeAwardResp(CumulativeAwardResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.LoginChangeInfo.get_cumulative_award.Add(resp.day_num);
            PlayerData.Instance.UpdateItem(resp.iteminfo);
            if (resp.soldier != null)
                PlayerData.Instance.MultipleAddSoldier(resp.soldier);
            if (resp.equip != null)
                PlayerData.Instance.MultipleAddWeapon(resp.equip);

            UISystem.Instance.SignView.UpdateTotalSignItemState();

            ShowAwardResult(resp.iteminfo, resp.equip, resp.soldier);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void SendNoRemindComment()
    {
        mNetWork.SendNoRemindComment();
    }
    public void ReceiveNoRemindComment(OnNeverRemindResp resp)
    {

    }

    public void SendCommentTaskFinish()
    {
        mNetWork.SendCommentTaskFinish();
    }
    public void ReceiveCommentTaskFinish()
    {

    }
    private void ObtainJumpToInfo()
    {
        DicTaskJumpTo = new Dictionary<ETaskOpenView, OpenFunctionType>();
        string tmpInfo = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.OPEN_VIEW_FUNC_RELATIONSHIP);
        string[] tmpArr = tmpInfo.Split(';');
        if ((tmpArr != null) && (tmpArr.Length > 0))
        {
            for (int i = 0; i < tmpArr.Length; i++)
            {
                if (string.IsNullOrEmpty(tmpArr[i]))
                    continue;
                string[] tmpInfoArr = tmpArr[i].Split(',');
                int tmpTaskID = 0;
                int tmpOpenID = 0;
                if ((tmpInfoArr != null) && (tmpInfoArr.Length >= 2))
                {
                    if (int.TryParse(tmpInfoArr[0], out tmpTaskID) && int.TryParse(tmpInfoArr[1], out tmpOpenID))
                    {
                        DicTaskJumpTo.Add((ETaskOpenView)tmpTaskID, (OpenFunctionType)tmpOpenID);
                    }
                }
            }
        }
    }
    public OpenFunctionType GetFuncTypeByTaskOpenView( ETaskOpenView viewtype)
    {
        if (DicTaskJumpTo.ContainsKey(viewtype))
            return DicTaskJumpTo[viewtype];
        Debug.LogError("ERROR; Get Func Type By Task Open View Failed! type =" + viewtype.ToString());
        return OpenFunctionType.None;
    }

    #region 主界面提示活跃任务弹窗功能
    private const bool _OPEN_LIVENESS_TIP_FUNC = false;
    private bool _allLivenessFinish = false;
    private List<LivenessTask> _unfinishLivenessTasks = null;
    private const float _TIP_TIMER_ON = 10f;
    private const float _TIP_TIMER_INTERVAL = 40f;
    public void StartLivenessTip()
    {
        //Debug.LogError("StartLivenessTip _openLivenessTipFunc=" + _OPEN_LIVENESS_TIP_FUNC
        //    + " func open =" + CommonFunction.CheckFuncIsOpen(OpenFunctionType.Liveness,false)
        //    );

        if (!_OPEN_LIVENESS_TIP_FUNC)
        {
            return;
        }

        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Liveness,false))
        {
            PlayerData.Instance.LevelUpEvent += LevelUpEvent;
            return;
        }

        Assets.Script.Common.Scheduler.Instance.AddTimer(_TIP_TIMER_INTERVAL, true, ShowLivenessTip);
        ShowLivenessTip();
    }
    private void LevelUpEvent(int old ,int now)
    {
        int lv = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Liveness).openLevel;
        if (lv > old && lv <= now)
        {
            Assets.Script.Common.Scheduler.Instance.AddFrame(2,false,StartLivenessTip);
            PlayerData.Instance.LevelUpEvent -= LevelUpEvent;
        }        
    }
    private void SetUnfinishLivenessTasks()
    {
        if (_unfinishLivenessTasks == null)
        {
            _unfinishLivenessTasks = new List<LivenessTask>();
        }
        _unfinishLivenessTasks.Clear();
        int numOfUnfinish = 0;
        LivenessData data;
        List<LivenessTask> list = PlayerData.Instance.LivenessInfo.liveness_task;
        for (int i = 0; i < list.Count; i++)
        {
            data = ConfigManager.Instance.mLivenessConfig.GetLivenessDataByID(list[i].id);
            if (list[i].num < data.TimesLimit)
            {
                numOfUnfinish++;
                if (data.OpenUIType != ETaskOpenView.None && !CommonFunction.CheckFuncIsOpen(GetFuncTypeByTaskOpenView(data.OpenUIType),false))
                    continue;
                if (data.OpenUIType == ETaskOpenView.Supermacy && !SupermacyModule.Instance.IsOpenActivity())
                    continue;
                _unfinishLivenessTasks.Add(list[i]);
            }
        }
        _allLivenessFinish = numOfUnfinish <= 0;
        //Debug.LogError("SetUnfinishLivenessTasks count=" + _unfinishLivenessTasks.Count);
    }
    private void ShowLivenessTip()
    {
        if (!_allLivenessFinish)
        {
            if(!UISystem.Instance.UIIsOpen(MenuView.UIName))
                return;
            SetUnfinishLivenessTasks();
            if (_unfinishLivenessTasks.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, _unfinishLivenessTasks.Count);
                UISystem.Instance.MenuView.ShowUnFinishLivenessTip(true, _unfinishLivenessTasks[index]);
                Assets.Script.Common.Scheduler.Instance.AddTimer(_TIP_TIMER_ON, false, CloseLivenessTip);
            }        
        }
        else
        {
            Assets.Script.Common.Scheduler.Instance.RemoveTimer(ShowLivenessTip);
        }
    }

    private void CloseLivenessTip()
    {
        if(UISystem.Instance.UIIsOpen(MenuView.UIName))
            UISystem.Instance.MenuView.ShowUnFinishLivenessTip(false,null);
    }

    #endregion

    public void Uninitialize()
    {
        mNetWork.RemoveMsg();
        mNetWork = null;
        DicTaskJumpTo = null;
        _allLivenessFinish = false;
        _unfinishLivenessTasks = null;
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(ShowLivenessTip);
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(CloseLivenessTip);
    }
}
