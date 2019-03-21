using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;
public enum UnionPrisonType
{
    /// <summary>
    /// 位置锁定
    /// </summary>
    LockEnemyRebelUnionResp,
    /// <summary>
    /// 神坛请求
    /// </summary>
    QueryAltarResp,
    /// <summary>
    /// 匹配请求
    /// </summary>
    MatchAltarUnionReq,
    /// <summary>
    /// 依附军团操作
    /// </summary>
    AltarHandleDependReq,
    /// <summary>
    /// 战斗请求
    /// </summary>
    AltarFightResp,
    /// <summary>
    /// 军团奴役记录
    /// </summary>
    QueryAltarRecordResp,
    /// <summary>
    /// 安慰
    /// </summary>
    AltarHandleDependReq_Comfor,
    /// <summary>
    /// 调戏
    /// </summary>
    AltarHandleDependReq_Molest,
}

public class UnionPrisonModule : Singleton<UnionPrisonModule>
{
    public string _BlockName;
    public UnionPrisonNetWork mNetWork;
    public UnionAltarInfo AltarInfo;
    public int max_handle_times;
    public List<AltarRecord> altar_records;
    public bool Prompt = false;
    public int pos;
    public bool had_privilege_fight = false;
    public List<fogs.proto.msg.MatchAltarUnionInfo> StrangeList = new List<MatchAltarUnionInfo>();
    public List<fogs.proto.msg.MatchAltarUnionInfo> Enemy_union = new List<MatchAltarUnionInfo>();
    public List<fogs.proto.msg.MatchAltarUnionInfo> Rebel_union = new List<MatchAltarUnionInfo>();

    public delegate void UnionPrisonControlDelegate(UnionPrisonType type, int errorCode);
    public event UnionPrisonControlDelegate UnionPrisonControlEvent;

    public List<AltarRecord> Altar_records
    {
        get { return this.altar_records; }
        set 
        {
            this.altar_records = value;
            this.altar_records.Sort((left, right) =>
            {
                if (left == null || right == null)
                {
                    if (left == null)
                        return -1;
                    else
                        return 1;
                }
                if (left.time != right.time)
                {
                    if (left.time > right.time)
                        return -1;
                    else
                        return 1;
                }
                return 0;
                
            });
        }
    }
    public void Initialize()
    {
        if (mNetWork == null)
        {
            mNetWork = new UnionPrisonNetWork();
            mNetWork.RegisterMsg();
        }
        if (AltarInfo == null)
            AltarInfo = new UnionAltarInfo();
        if (altar_records == null)
            altar_records = new List<AltarRecord>();
        if (this.Enemy_union == null)
            this.Enemy_union = new List<MatchAltarUnionInfo>();
        if (this.Rebel_union == null)
            this.Rebel_union = new List<MatchAltarUnionInfo>();
        if (this.StrangeList == null)
            this.StrangeList = new List<MatchAltarUnionInfo>();
    }
    public void Uninitialize()
    {
        mNetWork.RemoveMsg();
        mNetWork = null;
        AltarInfo = null;
        altar_records = null;
        this.Enemy_union = null;
        this.Rebel_union = null;
        this.StrangeList = null;
    }
    public DependPosition GetAltarInfoByPos(int pos)
    {
        if (this.AltarInfo == null || this.AltarInfo.depend_position == null)
            return null;
        return this.AltarInfo.depend_position.Find((tmp) => { if (tmp == null)return false; return tmp.position_id == pos; });
    }
    /// <summary>
    /// 神坛请求
    /// </summary>
    /// <param name="data"></param>
    public void SendQueryAltarReq()
    {
        QueryAltarReq temp = new QueryAltarReq();
        mNetWork.SendQueryAltarReq(temp);
    }
    public void ReceiveQueryAltarResp(Packet data)
    {
        QueryAltarResp tData = Serializer.Deserialize<QueryAltarResp>(data.ms);
        if (tData.result == 0)
        {
            this.had_privilege_fight = tData.had_privilege_fight > 0;
            this.AltarInfo.flame_position = tData.flame_position;
            this.AltarInfo.depend_position.Clear();
            this.AltarInfo.depend_position.AddRange(tData.depend_position);
            this.max_handle_times = tData.max_handle_times;
            this.Altar_records = tData.altar_records;
            this.Altar_records.Sort((left, right) =>
            {
                if (left == null || right == null)
                {
                    if (left == null)
                        return 1;
                    else
                        return -1;
                }
                if (left.time != right.time)
                {
                    if (left.time > right.time)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });
            if (this.Altar_records.Count > 50)
            {
                this.Altar_records.RemoveRange(50, this.Altar_records.Count - 50);
            }
            if (this.UnionPrisonControlEvent != null)
                this.UnionPrisonControlEvent(UnionPrisonType.QueryAltarResp, 0);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 匹配请求
    /// </summary>
    /// <param name="data"></param>
    public void SendMatchAltarUnionReq(MatchAltarUnionType type)
    {
        MatchAltarUnionReq temp = new MatchAltarUnionReq();
        temp.match_type = type;
        mNetWork.SendMatchAltarUnionReq(temp);
    }
    public void ReceiveMatchAltarUnionResp(Packet data)
    {
        MatchAltarUnionResp tData = Serializer.Deserialize<MatchAltarUnionResp>(data.ms);
        if (tData.result == 0)
        {
            if(!UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_UNIONPRISONCHOOSEVIEW))
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONPRISONCHOOSEVIEW);
            }
            if(tData.match_type == MatchAltarUnionType.NORMAL)
            {
                this.StrangeList = tData.union_infos;
                if (UISystem.Instance.UnionPrisonChooseView != null)
                    UISystem.Instance.UnionPrisonChooseView.InitStrangerItem();
            }
            if(tData.match_type == MatchAltarUnionType.ENEMY)
            {
                this.Enemy_union = tData.union_infos;
                if (UISystem.Instance.UnionPrisonChooseView != null)
                    UISystem.Instance.UnionPrisonChooseView.InitOtherItem(this.Enemy_union);

            }
            if(tData.match_type == MatchAltarUnionType.REBEL)
            {
                this.Rebel_union = tData.union_infos;
                if (UISystem.Instance.UnionPrisonChooseView != null)
                    UISystem.Instance.UnionPrisonChooseView.InitOtherItem(this.Rebel_union);
            }

            if (this.UnionPrisonControlEvent != null)
                this.UnionPrisonControlEvent(UnionPrisonType.MatchAltarUnionReq, 0);

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 操作依附军团请求（安慰、调戏）
    /// </summary>
    /// <param name="data"></param>
    public void SendAltarHandleDependReq(HandleDependType type,int position_id )
    {
        AltarHandleDependReq temp = new AltarHandleDependReq();
        temp.handle_type = type;
        temp.position_id = position_id;
        mNetWork.SendAltarHandleDependReq(temp);
    }
    public void ReceiveAltarHandleDependResp(Packet data)
    {
        AltarHandleDependResp tData = Serializer.Deserialize<AltarHandleDependResp>(data.ms);
        if (tData.result == 0)
        {
            this.Altar_records = tData.altar_records;
            this.AltarInfo.depend_position.Clear();
            this.AltarInfo.depend_position.AddRange(tData.position_infos);
            if(tData.handle_type == HandleDependType.COMFORT)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNIONPRISON_COMMOFRSUCCESS);
                if (this.UnionPrisonControlEvent != null)
                    this.UnionPrisonControlEvent(UnionPrisonType.AltarHandleDependReq_Comfor, 0);
            }
            else
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UINONPRISON_REVER);
                if (this.UnionPrisonControlEvent != null)
                    this.UnionPrisonControlEvent(UnionPrisonType.AltarHandleDependReq_Molest, 0);
            }
            PlayerData.Instance.UpdateItem(tData.money_info);
            if (this.UnionPrisonControlEvent != null)
                this.UnionPrisonControlEvent(UnionPrisonType.AltarHandleDependReq, 0);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    public void SendAltarFightReq(int be_fight_id)
    {
        AltarFightReq temp = new AltarFightReq();
        temp.be_fight_id = be_fight_id;
        temp.position_id = this.pos;
        this.pos = 0;
        mNetWork.SendAltarFightReq(temp);
    }
    public void ReceiveAltarFightResp(Packet data)
    {
        AltarFightResp tData = Serializer.Deserialize<AltarFightResp>(data.ms);
        if (tData.result == 0)
        {
            this.AltarInfo.flame_position = tData.flame_position;
            this.AltarInfo.depend_position.Clear();
            this.AltarInfo.depend_position.AddRange(tData.depend_position);
            this.Altar_records = tData.altar_records;
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONHEGEMONYVIEW);
            UISystem.Instance.UnionHegemonyView.OnShowHegemonyBattle(tData.base_unions[0], tData.base_unions[1], tData.fight_resp);
            if (this.UnionPrisonControlEvent != null)
                this.UnionPrisonControlEvent(UnionPrisonType.AltarFightResp, 0);

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    public void SendLockEnemyRebelUnionReq(int lock_union_id, LockUnionType type, AltarLockStatus lock_status)
    {
        LockEnemyRebelUnionReq temp = new LockEnemyRebelUnionReq();
        temp.lock_union_id = lock_union_id;
        temp.type = type;
        temp.lock_status = lock_status;
        mNetWork.SendLockEnemyRebelUnionReq(temp);
    }
    public void ReceiveLockEnemyRebelUnionResp(Packet data)
    {
        LockEnemyRebelUnionResp tData = Serializer.Deserialize<LockEnemyRebelUnionResp>(data.ms);
        if (tData.result == 0)
        {
            if(tData.type == LockUnionType.LOCK_ENEMY_UNION)
            {
                this.Enemy_union = tData.union_infos;
                UISystem.Instance.UnionPrisonChooseView.InitOtherItem(this.Enemy_union);
            }
            if(tData.type == LockUnionType.LOCK_REBEL_UNION)
            {
                this.Rebel_union = tData.union_infos;
                UISystem.Instance.UnionPrisonChooseView.InitOtherItem(this.Rebel_union);
            }
            
            if (this.UnionPrisonControlEvent != null)
                this.UnionPrisonControlEvent(UnionPrisonType.LockEnemyRebelUnionResp, 0);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    public void SendQueryAltarRecordReq()
    {
        QueryAltarRecordReq temp = new QueryAltarRecordReq();
        mNetWork.SendQueryAltarRecordReq(temp);
    }
    public void ReceiveQueryAltarRecordResp(Packet data)
    {
        QueryAltarRecordResp tData = Serializer.Deserialize<QueryAltarRecordResp>(data.ms);
        if (tData.result == 0)
        {
            this.Altar_records = tData.altar_records;
            this.Altar_records.Sort((left,right) =>
            {
                if(left == null || right == null)
                {
                    if (left == null)
                        return 1;
                    else
                        return -1;
                }
                if(left.time != right.time)
                {
                    if (left.time > right.time)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });
            if (this.Altar_records.Count > 50)
            {
                this.Altar_records.RemoveRange(50,this.Altar_records.Count - 50);
            }
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PRISONMARKVIEW);
            UISystem.Instance.PrisonMarkView.InitUnionPrisonMarkItem();
            if (this.UnionPrisonControlEvent != null)
                this.UnionPrisonControlEvent(UnionPrisonType.QueryAltarRecordResp, 0);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    public void SendDeleteDependUnionReq(int position_id)
    {
        DeleteDependUnionReq temp = new DeleteDependUnionReq();
        temp.position_id = position_id;
        mNetWork.SendDeleteDependUnionReq(temp);
    }
    public void ReceiveDeleteDependUnionResp(Packet data)
    {
        DeleteDependUnionResp tData = Serializer.Deserialize<DeleteDependUnionResp>(data.ms);
        if (tData.result == 0)
        {
            this.AltarInfo.depend_position.Clear();
            this.AltarInfo.depend_position.AddRange(tData.depend_positions);
            if (UISystem.Instance.UnionPrisonView != null)
                UISystem.Instance.UnionPrisonView.InitUI();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    public void SendAltarSerachUnionReq(string serach_name)
    {
        AltarSerachUnionReq temp = new AltarSerachUnionReq();
        temp.serach_name = serach_name;
        mNetWork.SendAltarSerachUnionReq(temp);
    }
    public void ReceiveAltarSerachUnionResp(Packet data)
    {
        AltarSerachUnionResp tData = Serializer.Deserialize<AltarSerachUnionResp>(data.ms);
        if (tData.result == 0)
        {
            List<MatchAltarUnionInfo> tmpList = new List<MatchAltarUnionInfo>();
            tmpList.Add(tData.serach_union);
            this.StrangeList = tmpList;
            if (UISystem.Instance.UnionPrisonChooseView != null)
                UISystem.Instance.UnionPrisonChooseView.InitStrangerItem();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    public void SendQueryAltarRecruitReq()
    {
        QueryAltarRecruitReq temp = new QueryAltarRecruitReq();
        mNetWork.SendQueryAltarRecruitReq(temp);
    }
    public void ReceiveQueryAltarRecruitResp(Packet data)
    {
        QueryAltarRecruitResp tData = Serializer.Deserialize<QueryAltarRecruitResp>(data.ms);
        if(tData != null)
        {
            if(UISystem.Instance.RecruitView != null && UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_RECRUITVIEW))
            {
                UISystem.Instance.RecruitView.SetUnionPrison(tData);
            }
        }
    }
}
