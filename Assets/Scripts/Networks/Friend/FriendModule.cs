using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class FriendModule : Singleton<FriendModule>
{
    public FriendNetWork mFriendNetWork;
    /// <summary>
    /// 总的好友信息
    /// </summary>
    public FriendsInfo MyFriendInfo;
    /// <summary>
    /// 好友列表
    /// </summary>
    public List<BasePlayerInfo> MyFriendList = new List<BasePlayerInfo>();
    /// <summary>
    /// 推荐好友列表
    /// </summary>
    public List<BasePlayerInfo> CommendFriendList = new List<BasePlayerInfo>();
    /// <summary>
    /// 好友申请列表
    /// </summary>
    public List<AppendFriends> MyAppendFriends = new List<AppendFriends>();
    /// <summary>
    /// 已经赠送过的好友列表
    /// </summary>
    public List<TickAccid> HadGiveList = new List<TickAccid>();
    /// <summary>
    /// 可领取的体力的好友列表
    /// </summary>
    public List<TickAccid> HadGetList = new List<TickAccid>();

    /// <summary>
    /// 获取好友邀请列表任务
    /// </summary>
    public List<TaskInfo> InvitCodeMissionList = new List<TaskInfo>();
    /// <summary>
    /// facebook好友邀请任务列表
    /// </summary>
    public List<TaskInfo> FBInvitMissionList = new List<TaskInfo>();
    /// <summary>
    /// LINE好友邀请任务列表
    /// </summary>
    public List<TaskInfo> LINEInvitMissionList = new List<TaskInfo>();
    /// <summary>
    /// 最大可赠送体力次数
    /// </summary>
    public int MAXGIVEPOWER = 10;
    /// <summary>
    /// 最大可获取体力次数
    /// </summary>
    public int MAXGETPOWER = 10;
    /// <summary>
    /// 最大可申请次数
    /// </summary>
    public int MAXAPPLYCOUNT = 20;
    /// <summary>
    /// 好友最大人数
    /// </summary>
    public int MAXFIRENDCOUNT = 999;
    /// <summary>
    /// 最大好友唤醒次数
    /// </summary>
    public int MAXFRIENDAWAKENUM = 10;
    /// <summary>
    /// FB邀请的好友个数
    /// </summary>
    public int FBInvitNum = 0;
    /// <summary>
    /// LINE邀请的好友个数
    /// </summary>
    public int LINEInvitNum = 0;
    public string FBName = "";
    public bool InvitOrWake = true;
    /// <summary>
    /// 唤醒剩余次数
    /// </summary>
    public int AwakeSurplurs = 10;
    public void Initialize()
    {
        if (mFriendNetWork == null)
        {
            mFriendNetWork = new FriendNetWork();
            mFriendNetWork.RegisterMsg();
        }
    }
    public void Uninitialize()
    {
        if (mFriendNetWork != null)
            mFriendNetWork.RemoveMsg();
        mFriendNetWork = null;
    }
    public TickAccid FindHadGiveListByAccid(uint accid)
    {
        if (this.HadGiveList == null)
            return null;
        return this.HadGiveList.Find((tmpInfo) => { if (tmpInfo == null) return false; return tmpInfo.accid == accid; });
    }
    public TickAccid FindHadGetListByAccid(uint accid)
    {
        if (this.HadGetList == null)
            return null;
        return this.HadGetList.Find((tmpInfo) => { if (tmpInfo == null) return false; return tmpInfo.accid == accid; });
    }
    public bool IsMyFriend(uint accid)
    {
        if (this.MyFriendList == null)
            return false;
        return MyFriendList.Find((info) => { if (info == null)return false; return info.accid == accid; }) != null;
    }
    /// <summary>
    /// 好友体力捐赠
    /// </summary>
    /// <param name="data"></param>
    public void SendDonateFriendsHpReq(fogs.proto.msg.FriendsOpType fireaccname, uint accid)
    {
        List<uint> tmpList = new List<uint>();
        if (fireaccname == FriendsOpType.FO_ALL)
        {
            if (MyFriendInfo.donate_ph_times >= this.MAXGIVEPOWER)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIREND_SUCCESS_10);
                return;
            }
            if (this.MyFriendList != null)
            {
                bool isHad = false;
                for (int i = 0; i < this.MyFriendList.Count; ++i)
                {
                    if (this.MyFriendList[i] == null)
                        continue;
                    if (this.FindHadGiveListByAccid(this.MyFriendList[i].accid) == null)
                    {
                        isHad = true;
                        break;
                    }
                }
                if (!isHad)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIREND_SUCCESS_13);
                    return;
                }
            }
            int count = (int)MyFriendInfo.donate_ph_times;
            for (int i = 0; i < MyFriendList.Count; ++i)
            {
                if (this.MAXGIVEPOWER - count <= tmpList.Count)
                    break;
                BasePlayerInfo tmpPInfo = MyFriendList[i];
                if (tmpPInfo == null)
                    continue;

                if (this.FindHadGiveListByAccid(tmpPInfo.accid) == null)
                    tmpList.Add(tmpPInfo.accid);
            }
        }
        else
        {
            if (this.FindHadGiveListByAccid(accid) != null)
                return;
            tmpList.Add(accid);
        }

        DonateFriendsHpReq tmpInfo = new DonateFriendsHpReq();
        tmpInfo.type = fireaccname;
        tmpInfo.accid.AddRange(tmpList);
        mFriendNetWork.SendDonateFriendsHpReq(tmpInfo);
    }
    public void ReceiveDonateFriendsHpResp(Packet data)
    {
        DonateFriendsHpResp tData = Serializer.Deserialize<DonateFriendsHpResp>(data.ms);
        if (tData.result == 0)
        {
            this.HadGiveList = tData.accid;
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitFriendTable();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIREND_SUCCESS_1);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 好友体力领取
    /// </summary>
    /// <param name="data"></param>
    public void SendRecieveFriendHpReq(FriendsOpType fireaccname, uint accid)
    {
        List<uint> tmpList = new List<uint>();
        if (MyFriendInfo == null || MyFriendList == null)
            return;

        if (fireaccname == FriendsOpType.FO_ALL)
        {
            if (MyFriendInfo.recieve_ph_times >= this.MAXGETPOWER)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIREND_SUCCESS_11);
                return;
            }
            if (this.HadGetList == null || this.HadGetList.Count <= 0)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIREND_SUCCESS_12);
                return;
            }
            int count = (int)MyFriendInfo.recieve_ph_times;
            for (int i = 0; i < MyFriendList.Count; ++i)
            {
                if (this.MAXGETPOWER - count <= tmpList.Count)
                    break;
                BasePlayerInfo tmpPInfo = MyFriendList[i];
                if (tmpPInfo == null)
                    continue;

                if (this.FindHadGetListByAccid(tmpPInfo.accid) != null)
                    tmpList.Add(tmpPInfo.accid);
            }

        }
        else
        {
            tmpList.Add(accid);
        }
        RecieveFriendHpReq tmpInfo = new RecieveFriendHpReq();
        tmpInfo.type = fireaccname;
        tmpInfo.accids.AddRange(tmpList);
        mFriendNetWork.SendRecieveFriendHpReq(tmpInfo);
    }
    public void ReceiveRecieveFriendHpResp(Packet data)
    {
        RecieveFriendHpResp tData = Serializer.Deserialize<RecieveFriendHpResp>(data.ms);
        if (tData.result == 0)
        {
            this.HadGetList = tData.accids;
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitFriendTable();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FIREND_SUCCESS_2, tData.recieve_ph_power));
            PlayerData.Instance.UpdateSP(tData.ph_power);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 申请添加好友
    /// </summary>
    /// <param name="data"></param>
    public void SendApplyAddFriendsReq(string accname, uint accid)
    {
        if (this.MyFriendList.Count >= this.MAXFIRENDCOUNT)
        {
            ErrorCode.ShowErrorTip((int)ErrorCodeEnum.FriendsOverFlow);
        }
        ApplyAddFriendsReq tmpInfo = new ApplyAddFriendsReq();
        tmpInfo.accid = (int)accid;
        tmpInfo.accname = accname;
        mFriendNetWork.SendApplyAddFriendsReq(tmpInfo);
    }
    public void ReceiveApplyAddFriendsResp(Packet data)
    {
        ApplyAddFriendsResp tData = Serializer.Deserialize<ApplyAddFriendsResp>(data.ms);
        if (tData.result == 0)
        {
            this.CommendFriendList.Find((tmpInfo) =>
            {
                if (tmpInfo == null)
                    return false;
                if (tmpInfo.accname.Equals(tData.accname))
                    tmpInfo.status = 1;
                return false;
            });
            if (UISystem.Instance.FriendAdd != null)
                UISystem.Instance.FriendAdd.InitMatch(this.CommendFriendList);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FIREND_SUCCESS_6));

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }

    }
    /// <summary>
    /// 获取好友列表
    /// </summary>
    /// <param name="data"></param>
    public void SendGetFriendsListReq()
    {
        GetFriendsListReq tmpInfo = new GetFriendsListReq();
        mFriendNetWork.SendGetFriendsListReq(tmpInfo);
    }
    public void ReceiveGetFriendsListResp(Packet data)
    {
        GetFriendsListResp tData = Serializer.Deserialize<GetFriendsListResp>(data.ms);
        if (tData.result == 0)
        {
            this.MyFriendInfo = tData.friends_info;
            this.MyFriendList = tData.friends_info.friends;
            this.HadGetList = tData.friends_info.bedonate_ph_info;
            this.HadGiveList = tData.friends_info.donate_ph_info;
            this.MyFriendList.Sort((left, right) =>
            {
                if (left == null || right == null)
                {
                    if (left == null)
                        return 1;
                    else
                        return -1;
                }
                if (left.level != right.level)
                {
                    if (left.level > right.level)
                        return -1;
                    else
                        return 1;
                }
                if (left.combat_power != right.combat_power)
                {
                    if (left.combat_power > right.combat_power)
                        return -1;
                    else
                        return 1;
                }
                return 1;
            });
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitFriendTable();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }

    }
    /// <summary>
    /// 获得好友申请列表
    /// </summary>
    /// <param name="data"></param>
    public void SendGetAppendFriendsListReq()
    {
        GetAppendFriendsListReq tmpInfo = new GetAppendFriendsListReq();

        mFriendNetWork.SendGetAppendFriendsListReq(tmpInfo);
    }
    public void ReceiveGetAppendFriendsListResp(Packet data)
    {
        GetAppendFriendsListResp tData = Serializer.Deserialize<GetAppendFriendsListResp>(data.ms);
        if (tData.result == 0)
        {
            this.MyAppendFriends = tData.append_list;
            if (this.MyAppendFriends != null && this.MyAppendFriends.Count > 1)
            {
                this.MyAppendFriends.Sort((left, right) =>
                {
                    if (left == null || right == null)
                    {
                        if (left == null)
                            return 1;
                        else
                            return -1;
                    }
                    if (left.append_friends == null || right.append_friends == null)
                    {
                        if (left.append_friends == null)
                            return 1;
                        else
                            return -1;
                    }
                    if (left.append_friends.level != right.append_friends.level)
                    {
                        if (left.append_friends.level > right.append_friends.level)
                            return -1;
                        else
                            return 1;
                    }
                    if (left.append_friends.combat_power != right.append_friends.combat_power)
                    {
                        if (left.append_friends.combat_power > right.append_friends.combat_power)
                            return -1;
                        else
                            return 1;
                    }
                    return 1;
                });
            }
            if (UISystem.Instance.FriendApply != null)
                UISystem.Instance.FriendApply.InitUI();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }

    }
    /// <summary>
    /// 好友申请一键和单个
    /// </summary>
    /// <param name="data"></param>
    public void SendAuditAppendFriendsReq(AuditFriendsOpType op_type, FriendsOpType type, AccnameToAccid accname_accids)
    {
        List<AccnameToAccid> AppendList = new List<AccnameToAccid>();
        if (this.MyFriendList.Count >= this.MAXFIRENDCOUNT)
        {
            ErrorCode.ShowErrorTip((int)ErrorCodeEnum.FriendsOverFlow);
        }
        if (type == FriendsOpType.FO_ALL)
        {
            if (this.MyAppendFriends != null)
            {
                for (int i = 0; i < this.MyAppendFriends.Count; ++i)
                {
                    if (MyAppendFriends[i] == null || MyAppendFriends[i].append_friends == null)
                        continue;

                    AccnameToAccid tmpAA = new AccnameToAccid();
                    tmpAA.accid = MyAppendFriends[i].append_friends.accid;
                    tmpAA.accname = MyAppendFriends[i].append_friends.accname;
                    AppendList.Add(tmpAA);
                }
            }
        }
        else
        {
            AppendList.Add(accname_accids);
        }

        AuditAppendFriendsReq tmpInfo = new AuditAppendFriendsReq();
        tmpInfo.op_type = op_type;
        tmpInfo.type = type;
        tmpInfo.accname_accids.AddRange(AppendList);
        mFriendNetWork.SendAuditAppendFriendsReq(tmpInfo);
    }
    public void ReceiveAuditAppendFriendsResp(Packet data)
    {
        AuditAppendFriendsResp tData = Serializer.Deserialize<AuditAppendFriendsResp>(data.ms);
        if (tData.result == 0)
        {
            this.MyAppendFriends = tData.append_friends;
            this.MyFriendList = tData.friends;
            if (UISystem.Instance.FriendApply != null)
                UISystem.Instance.FriendApply.InitUI();
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitFriendTable();
            if (tData.op_type == AuditFriendsOpType.AFO_ACCPET)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FIREND_SUCCESS_3));
            else
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FIREND_SUCCESS_5));

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 搜索玩家
    /// </summary>
    /// <param name="data"></param>
    public void SendSearchFriendsReq(string charname)
    {
        if (string.IsNullOrEmpty(charname))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PLEASE_INPUT_NAME);
            return;
        }
        SearchFriendsReq tmpInfo = new SearchFriendsReq();
        tmpInfo.charname = charname;
        mFriendNetWork.SendSearchFriendsReq(tmpInfo);
    }
    public void ReceiveSearchFriendsResp(Packet data)
    {
        SearchFriendsResp tData = Serializer.Deserialize<SearchFriendsResp>(data.ms);
        if (tData.result == 0)
        {
            List<BasePlayerInfo> tmpList = new List<BasePlayerInfo>();
            tmpList.Add(tData.friend);
            if (UISystem.Instance.FriendAdd != null)
            {

                UISystem.Instance.FriendAdd.InitFind(tmpList);

            }
            UISystem.Instance.FriendAdd.PlayEffect(UISystem.Instance.FriendAdd.view.Effect_1.transform);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }

    }
    /// <summary>
    /// 删除好友
    /// </summary>
    /// <param name="data"></param>
    public void SendDelFriendsReq(uint accid)
    {
        DelFriendsReq tmpInfo = new DelFriendsReq();

        tmpInfo.accid = accid;
        mFriendNetWork.SendDelFriendsReq(tmpInfo);
    }
    public void ReceiveDelFriendsResp(Packet data)
    {
        DelFriendsResp tData = Serializer.Deserialize<DelFriendsResp>(data.ms);
        if (tData.result == 0)
        {
            this.MyFriendList.RemoveAll((info) => { if (info == null || tData.friend == null)return false; return tData.friend.accid == info.accid; });
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitFriendTable();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FIREND_SUCCESS_9));

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }

    }
    /// <summary>
    /// 刷新推荐
    /// </summary>
    /// <param name="data"></param>
    public void SendRefreshRecommendFriendsReq()
    {
        RefreshRecommendFriendsReq tmpInfo = new RefreshRecommendFriendsReq();

        mFriendNetWork.SendRefreshRecommendFriendsReq(tmpInfo);
    }
    public void ReceiveRefreshRecommendFriendsResp(Packet data)
    {
        RefreshRecommendFriendsResp tData = Serializer.Deserialize<RefreshRecommendFriendsResp>(data.ms);
        if (tData.result == 0)
        {
            this.CommendFriendList = tData.re_friends;
            this.CommendFriendList.Sort((left, right) =>
            {
                if (left == null || right == null)
                {
                    if (left == null)
                        return 1;
                    else
                        return -1;
                }
                if (left.level != right.level)
                {
                    if (left.level > right.level)
                        return -1;
                    else
                        return 1;
                }
                if (left.combat_power != right.combat_power)
                {
                    if (left.combat_power > right.combat_power)
                        return -1;
                    else
                        return 1;
                }
                return 1;
            });
            int num = 3 <= tData.re_friends.Count ? 3 : tData.re_friends.Count;
            if (num > 0)
            {
                UISystem.Instance.FriendAdd.PlayEffect(UISystem.Instance.FriendAdd.view.Effect_1.transform);
                --num;
            }
            if (num > 0)
            {
                UISystem.Instance.FriendAdd.PlayEffect(UISystem.Instance.FriendAdd.view.Effect_2.transform);
                --num;
            }
            if (num > 0)
            {
                UISystem.Instance.FriendAdd.PlayEffect(UISystem.Instance.FriendAdd.view.Effect_3.transform);
                --num;
            }
            PlayerData.Instance.refresh_tick = tData.refresh_tick;

            if (UISystem.Instance.FriendAdd != null)
                UISystem.Instance.FriendAdd.InitMatch(this.CommendFriendList);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 输入邀请码
    /// </summary>
    public void SendVerifyInviteCodeReq(string code)
    {
        if (!string.IsNullOrEmpty(code) && code.Equals(PlayerData.Instance.InviteCode))
        {
            ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.VerifyInviteCodeIllegal);
            return;
        }

        VerifyInviteCodeReq tmpInfo = new VerifyInviteCodeReq();
        tmpInfo.code = code;
        mFriendNetWork.SendVerifyInviteCodeReq(tmpInfo);

    }
    public void ReceiveVerifyInviteCodeResp(Packet data)
    {
        VerifyInviteCodeResp tData = Serializer.Deserialize<VerifyInviteCodeResp>(data.ms);
        if (tData.result == 0)
        {

            PlayerData.Instance.InvitCodeNum = tData.invite_nums;
            this.InvitCodeMissionList = tData.task_list;
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitInviteCodeTable();

            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIREND_SUCCESS_7);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 获取邀请码好友任务列表
    /// </summary>
    /// <param name="data"></param>
    public void SendGetInviteCodeTaskListReq()
    {
        GetInviteCodeTaskListReq tmpInfo = new GetInviteCodeTaskListReq();

        mFriendNetWork.SendGetInviteCodeTaskListReq(tmpInfo);
    }
    public void ReceiveGetInviteCodeTaskListResp(Packet data)
    {
        GetInviteCodeTaskListResp tData = Serializer.Deserialize<GetInviteCodeTaskListResp>(data.ms);
        if (tData.result == 0)
        {
            this.InvitCodeMissionList = tData.task_list;
            PlayerData.Instance.InvitCodeNum = tData.invite_num;
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitInviteCodeTable();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 领取邀请码好友任务奖励
    /// </summary>
    /// <param name="id"></param>
    public void SendGetInviteCodeTaskAwardReq(uint id)
    {
        GetInviteCodeTaskAwardReq tmpInfo = new GetInviteCodeTaskAwardReq();
        tmpInfo.id = id;
        mFriendNetWork.SendGetInviteCodeTaskAwardReq(tmpInfo);
    }
    public void ReceiveGetInviteCodeTaskAwardResp(Packet data)
    {
        GetInviteCodeTaskAwardResp tData = Serializer.Deserialize<GetInviteCodeTaskAwardResp>(data.ms);
        if (tData.result == 0)
        {
            this.InvitCodeMissionList = tData.task_list;
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitInviteCodeTable();
            PlayerData.Instance.UpdateDropData(tData.awards_list);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
            List<fogs.proto.msg.ItemInfo> tmpItemInfoList = new List<fogs.proto.msg.ItemInfo>();
            tmpItemInfoList.AddRange(tData.awards_list.item_list);
            tmpItemInfoList.AddRange(tData.awards_list.special_list);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(CommonFunction.GetCommonItemDataList(tmpItemInfoList, tData.awards_list.equip_list, tData.awards_list.soldier_list));
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 获取最强整容
    /// </summary>
    /// <param name="data"></param>
    public void SendGetTopPlayerInfoReq(uint accid)
    {
        GetTopPlayerInfoReq tmpInfo = new GetTopPlayerInfoReq();
        tmpInfo.accid = accid;
        mFriendNetWork.SendGetTopPlayerInfoReq(tmpInfo);
    }
    public void ReceiveGetTopPlayerInfoResp(Packet data)
    {
        GetTopPlayerInfoResp tData = Serializer.Deserialize<GetTopPlayerInfoResp>(data.ms);
        if (tData.result == 0)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
            UISystem.Instance.PlayerInfoView.UpdateViewInfo(PlayerInfoTypeEnum.VIP, tData.player_info);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 获取FB好友列表
    /// </summary>
    public void SendGetFacebookFriends()
    {
        SDKManager.Instance.GetFBInvitableFriends();
    }
    public void ReceiveGetFacebookFriends(List<object> vFBFriendList)
    {
        PlayerData.Instance.FbfriendDepot.Searlize(vFBFriendList);
        SDKManager.Instance.GetFacebookFriends();
    }
    public void ReceiveGetObtainFBFreinds(List<object> vFBFriendList)
    {
        PlayerData.Instance.FbfriendDepot.MutilAdd(vFBFriendList);
    }
    public void ReceiveShareToFaceBook()
    {
        if (string.IsNullOrEmpty(this.FBName))
            return;

        if (this.InvitOrWake)
        {
            SendInviteThirdFriendsReq(ThirdFriendType.FACEBOOK, ThirdFriendsOpResultType.SUCCESS, this.FBName);
        }
        else
        {
            SendWakeThirdFriendsReq(ThirdFriendType.FACEBOOK, ThirdFriendsOpResultType.SUCCESS, this.FBName);
        }
    }
    /// <summary>
    /// 获取好友列表游戏信息
    /// </summary>
    /// <param name="vType"></param>
    /// <param name="vUserId"></param>
    public void SendGetThirdFriendsInfoReq(ThirdFriendType vType, List<string> vUserName)
    {
        GetThirdFriendsInfoReq tmpInfo = new GetThirdFriendsInfoReq();
        tmpInfo.type = vType;
        tmpInfo.name.AddRange(vUserName);
        mFriendNetWork.SendGetThirdFriendsInfoReq(tmpInfo);
    }
    public void ReceiveGetThirdFriendsInfoResp(Packet data)
    {
        GetThirdFriendsInfoResp tData = Serializer.Deserialize<GetThirdFriendsInfoResp>(data.ms);
        if (tData.result == 0)
        {
            this.AwakeSurplurs = (int)tData.wakeup_nums;
            if (PlayerData.Instance.FbfriendDepot != null)
                PlayerData.Instance.FbfriendDepot.ReceiveGetThirdFriendsInfoResp(tData.third_friends);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 邀请好友
    /// </summary>
    /// <param name="data"></param>
    public void SendInviteThirdFriendsReq(ThirdFriendType vType, ThirdFriendsOpResultType vResultType, string vName)
    {
        InviteThirdFriendsReq tmpInfo = new InviteThirdFriendsReq();
        tmpInfo.type = vType;
        tmpInfo.name = vName;
        tmpInfo.op_step = vResultType;
        mFriendNetWork.SendInviteThirdFriendsReq(tmpInfo);
    }
    public void ReceiveInviteThirdFriendsResp(Packet data)
    {
        InviteThirdFriendsResp tData = Serializer.Deserialize<InviteThirdFriendsResp>(data.ms);
        if (tData.result == 0)
        {
            if (tData.op_step == ThirdFriendsOpResultType.FAKE_SUCCESS)
            {
                if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
                {
                    if (tData.type == ThirdFriendType.FACEBOOK)
                    {
                        int timeDiffent = (int)(tData.next_tick - Main.mTime);
                        if (timeDiffent > 0)
                        {
                            string message = ConstString.FRIEND_CANOT_INVITE;
                            int day = timeDiffent / 86400;
                            if (day <= 0)
                            {
                                day = day / 3600;
                                if (day > 0)
                                    message = ConstString.FRIEND_CANOT_INVITE_Hour;
                                else
                                    message = ConstString.FRIEND_CANOT_INVITE_ONEHour;
                            }
                            if (day > 0)
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(message, day));
                            else
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, message);
                            return;
                        }
                        FBFriend tmpFBFriend = PlayerData.Instance.FbfriendDepot.FindByName(tData.name);
                        if (tmpFBFriend != null)
                            SDKManager.Instance.InviteFriendByFacebook(tmpFBFriend.Id);
                        this.FBName = tData.name;
                        this.InvitOrWake = true;
                    }
                    else
                    {
                        string message = "";
                        FriendSpecialInfo tmpSpecial = ConfigManager.Instance.mFriendSpecialData.FindById(FriendSpecialEnum.LINEINVITEDESCRIPT);
                        if (tmpSpecial != null)
                            message = tmpSpecial.Descript;
                        SDKManager.Instance.ShareTextByLine(string.Format(message, LoginModule.Instance.CurServer.desc, PlayerData.Instance._NickName, PlayerData.Instance.InviteCode));
                        SendInviteThirdFriendsReq(tData.type, ThirdFriendsOpResultType.SUCCESS, "");
                    }
                }
            }
            else
            {
                this.FBInvitMissionList = tData.fb_invite_tasks;
                this.LINEInvitMissionList = tData.line_invite_tasks;
                if (tData.type == ThirdFriendType.FACEBOOK)
                    ++this.FBInvitNum;
                else
                    ++this.LINEInvitNum;
                if (tData.type == ThirdFriendType.FACEBOOK)
                {
                    FBFriend tmpFb = PlayerData.Instance.FbfriendDepot.FindByName(tData.name);
                    if (tmpFb != null)
                        tmpFb.next_tick = tData.next_tick;
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FRIEND_INVITE_SUCCESS);
                }
                if (UISystem.Instance.FriendView != null)
                {
                    UISystem.Instance.FriendView.InitFBFriendLabel();
                    UISystem.Instance.FriendView.InitFriendMissionTable();
                }
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 唤醒好友
    /// </summary>
    /// <param name="data"></param>
    public void SendWakeThirdFriendsReq(ThirdFriendType vType, ThirdFriendsOpResultType vResultType, string vName)
    {
        WakeThirdFriendsReq tmpInfo = new WakeThirdFriendsReq();
        tmpInfo.type = vType;
        tmpInfo.name = vName;
        tmpInfo.op_step = vResultType;
        mFriendNetWork.SendWakeThirdFriendsReq(tmpInfo);
    }
    public void ReceiveWakeThirdFriendsResp(Packet data)
    {
        WakeThirdFriendsResp tData = Serializer.Deserialize<WakeThirdFriendsResp>(data.ms);
        if (tData.result == 0)
        {
            if (tData.op_step == ThirdFriendsOpResultType.FAKE_SUCCESS)
            {
                if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
                {
                    if (tData.type == ThirdFriendType.FACEBOOK)
                    {
                        if (this.AwakeSurplurs <= 0)
                        {
                            long timeCompare = tData.reset_tick - Main.mTime;
                            int day = (int)(timeCompare / 86400);
                            if (day <= 0)
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FRIEND_WAKE_TIMEOUTONEDAY);
                            else
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FRIEND_WAKE_TIMEOUT, day));
                            return;
                        }
                        FBFriend tmpFBFriend = PlayerData.Instance.FbfriendDepot.FindByName(tData.name);
                        if (tmpFBFriend != null)
                            SDKManager.Instance.InviteFriendByFacebook(tmpFBFriend.Id);
                        this.FBName = tData.name;
                        this.InvitOrWake = false;
                    }
                }
            }
            else
            {
                if (tData.type == ThirdFriendType.FACEBOOK)
                {
                    FBFriend tmpFb = PlayerData.Instance.FbfriendDepot.FindByName(tData.name);
                    if (tmpFb != null)
                        tmpFb.next_tick = tData.reset_tick;
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FRIEND_AWAKE_SUCCESS);
                }
                this.AwakeSurplurs = (int)tData.wakeup_nums;
                this.FBInvitMissionList = tData.fb_invite_tasks;
                this.LINEInvitMissionList = tData.line_invite_tasks;
                PlayerData.Instance.UpdateDropData(tData.awards_list);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
                List<fogs.proto.msg.ItemInfo> tmpItemInfoList = new List<fogs.proto.msg.ItemInfo>();
                tmpItemInfoList.AddRange(tData.awards_list.item_list);
                tmpItemInfoList.AddRange(tData.awards_list.special_list);
                UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(CommonFunction.GetCommonItemDataList(tmpItemInfoList, tData.awards_list.equip_list, tData.awards_list.soldier_list));
                if (UISystem.Instance.FriendView != null)
                {
                    UISystem.Instance.FriendView.InitFBFriendLabel();
                }
            }

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 获取FB和LINE好友任务列表
    /// </summary>
    /// <param name="data"></param>
    public void SendGetThirdFriendsTaskReq()
    {
        GetThirdFriendsTaskReq tmpInfo = new GetThirdFriendsTaskReq();
        mFriendNetWork.SendGetThirdFriendsTaskReq(tmpInfo);
    }
    public void ReceiveGetThirdFriendsTaskResp(Packet data)
    {
        GetThirdFriendsTaskResp tData = Serializer.Deserialize<GetThirdFriendsTaskResp>(data.ms);
        if (tData.result == 0)
        {
            this.FBInvitMissionList = tData.fb_invite_tasks;
            this.LINEInvitMissionList = tData.line_invite_tasks;
            this.FBInvitNum = tData.fb_invite_nums;
            this.LINEInvitNum = tData.line_invite_nums;
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitFriendMissionTable();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }
    /// <summary>
    /// 领取第三方好友任务奖励
    /// </summary>
    /// <param name="data"></param>
    public void SendGetThirdFriendsTaskAwardReq(ThirdFriendType vType, uint vId)
    {
        GetThirdFriendsTaskAwardReq tmpInfo = new GetThirdFriendsTaskAwardReq();
        tmpInfo.id = vId;
        tmpInfo.type = vType;
        mFriendNetWork.SendGetThirdFriendsTaskAwardReq(tmpInfo);
    }
    public void ReceiveGetThirdFriendsTaskAwardResp(Packet data)
    {
        GetThirdFriendsTaskAwardResp tData = Serializer.Deserialize<GetThirdFriendsTaskAwardResp>(data.ms);
        if (tData.result == 0)
        {
            if (tData.type == ThirdFriendType.LINE)
            {
                this.LINEInvitMissionList = tData.task_list;
                this.LINEInvitNum = tData.invite_nums;
            }
            else
            {
                this.FBInvitMissionList = tData.task_list;
                this.FBInvitNum = tData.invite_nums;
            }
            if (UISystem.Instance.FriendView != null)
                UISystem.Instance.FriendView.InitFriendMissionTable();
            PlayerData.Instance.UpdateDropData(tData.awards_list);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
            List<fogs.proto.msg.ItemInfo> tmpItemInfoList = new List<fogs.proto.msg.ItemInfo>();
            tmpItemInfoList.AddRange(tData.awards_list.item_list);
            tmpItemInfoList.AddRange(tData.awards_list.special_list);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(CommonFunction.GetCommonItemDataList(tmpItemInfoList, tData.awards_list.equip_list, tData.awards_list.soldier_list));
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
    }

}
