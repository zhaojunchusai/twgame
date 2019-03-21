using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class SystemSettingModule : Singleton<SystemSettingModule>
{
    private static string UIName = "SystemSettingView";
    public string _BlockName;
    public SystemSettingNetWork mNetWork;
    public delegate void ShieldPlayerDelegate();
    public event ShieldPlayerDelegate ShieldPlayerEvent;
    public List<FrameInfo> _frameList;
    public List<IconInformation> _iconList;
    public bool selfChangeFrame;
    public bool selfChangeIcon;

    public void Initialize()
    {
        if (mNetWork == null)
        {
            mNetWork = new SystemSettingNetWork();
            mNetWork.RegisterMsg();
        }
        if (_frameList == null)
        {
            _frameList = new List<FrameInfo>();
        }
        else
        {
            _frameList.Clear();
        }
        if (_iconList == null)
        {
            _iconList = new List<IconInformation>();
        }
        else
        {
            _iconList.Clear();
        }
    }
    public void Uninitialize()
    {
        mNetWork.RemoveMsg();
        mNetWork = null;
        if (_frameList != null)
        {
            _frameList.Clear();
            _frameList = null;
        }
        if (_iconList != null)
        {
            _iconList.Clear();
            _iconList = null;
        }
    }
    public void SendChangeNameRequest(string newName)
    {
        ChangeCharnameReq req = new ChangeCharnameReq();
        //req.accid = 1; 
        //req.area_id = 3;
        req.charname = newName;
        mNetWork.SendNewNameReq(req);
    }
    public void ReceiveChangeNameResp(ChangeCharnameResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)//成功
        {
            UISystem.Instance.SystemSettingView.HideChangeNameView();//隐藏修改姓名界面
            PlayerData.Instance.UpdatePlayerNickName(resp.charname);
            PlayerData.Instance._FreeRenameNum = (int)resp.curr_freerename_times;
            if (resp.update_item != null)
            {
                IDType type = CommonFunction.GetTypeOfID(resp.update_item.id.ToString());
                switch (type)
                {
                    case IDType.Gold:
                        PlayerData.Instance.UpdateGold(resp.update_item.num);
                        break;
                    case IDType.Diamond:
                        PlayerData.Instance.UpdateDiamond(resp.update_item.num);
                        break;
                    case IDType.Free:
                        break;
                    default:
                        DebugUtil.LogError("ReceiveChangeNameResp resp.update_item.id " + resp.update_item.id);
                        break;
                }
            }
            UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
            UISystem.Instance.MainCityView.UpdateName();
            UISystem.Instance.SystemSettingView.InitPlayerInfo();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CHANGENAME_SUCCESS);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }

    }

    public void SendFeedBackRequest(string title, string content)
    {
        SuggestReq req = new SuggestReq();
        req.accid = PlayerData.Instance._AccountID;
        req.title = title;
        req.content = content;
        mNetWork.SendFeedBackReq(req);
    }
    public void ReceiveFeedBackResp(SuggestResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FEEDBACK_SUCCESS);
            PlayerData.Instance.FeedbackNextTime = resp.suggest_cool_to;
            UISystem.Instance.SystemSettingView.CloseFeedBack();
            UISystem.Instance.SystemSettingView.FeedbackCount(resp.times);
            Debug.Log("Resp.times =" + resp.times + " resp.suggest_cool_to  " + resp.suggest_cool_to + "    ds  " + Main.mTime);

        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }

    }
    public void SendCDKeyRequest(string cdKey)
    {
        UseCDKeyReq req = new UseCDKeyReq();
        req.cdkey = cdKey;
        mNetWork.SendCDKeyReq(req);
    }
    public void ReceiveCDKeyResp(UseCDKeyResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            //PlayerData.Instance.UpdateItem(resp.item_list);
            UISystem.Instance.SystemSettingView.CloseCDKey();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CDKEY_SUCCESS);
        }
        //else if(resp.result==(int )ErrorCodeEnum.CDKeyInputEmpty)//输入码为空
        //{

        //}
        //else if (resp .result ==(int )ErrorCodeEnum.CDKeyInvalid)//CDK无效
        //{

        //}
        //else if(resp .result ==(int)ErrorCodeEnum.CDKeyUsed)//CDK已使用
        //{

        //}
        //else if(resp .result ==(int )ErrorCodeEnum.CDKeyPastDue)//CDK已过期
        //{

        //}
        //else if(resp .result ==(int )ErrorCodeEnum.CDKeyEarly)//CDK尚未开放
        //{

        //}
        else
        {
            //ErrorCode.ShowErrorTip(resp.result);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CDKEY_WRONG);
        }
        UISystem.Instance.SystemSettingView.CloseCDKey();
    }
    public void SendExitRequest()
    {
        if (mNetWork == null)
        {
            Main.Instance.LoginOut();
            return;
        }
        PlayerLogoutReq req = new PlayerLogoutReq();
        mNetWork.SendExitReq(req);
    }
    public void ReceiveExitResp(NotifyPlayerOffline resp)
    {
        if (resp.reason == (int)ErrorCodeEnum.SUCCESS)
        {
            Main.Instance.LoginOut();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.reason);
        }

    }
    public void SendRandomName()
    {
        LoginModule.Instance.SendCharname(LoginModule.Instance.CurServerId);
    }
    public void OnRandomResponse(string name)
    {
        if (UISystem.Instance.SystemSettingView != null)
        {
            UISystem.Instance.SystemSettingView.OnUpdateNickName(name);
        }
    }
    public void SendIconChangeRequset(uint id)
    {
        ChangeIconReq req = new ChangeIconReq();
        req.type = IconType.FRAME;
        req.new_id = id;
        mNetWork.SendChangeReq(req);
    }
    public void ReceiveChangeResp(ChangeIconResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (resp.type == IconType.FRAME)
            {
                PlayerData.Instance.FrameID = resp.new_id;
                if (selfChangeFrame)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ICON_CHANGEOK);
                    UISystem.Instance.SystemSettingView.view.UIPanel_IconChangeView.gameObject.SetActive(false);
                }

                UISystem.Instance.MainCityView.ShowIconChange();
                UISystem.Instance.TopFuncView.ShowIconBG();
                if(UISystem.Instance.UIIsOpen(UIName))
                    UISystem.Instance.SystemSettingView.SetSystemPlayerIcon();
                selfChangeFrame = false;
            }
            if (resp.type == IconType.ICON)
            {
                PlayerData.Instance.HeadID = resp.new_id;
                if (selfChangeIcon)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ICON_HEADCHANGEOK);
                    UISystem.Instance.SystemSettingView.view.UIPanel_HeadChangeView.SetActive(false);
                    UISystem.Instance.SystemSettingView.CloseHeadChange();
                }

                UISystem.Instance.MainCityView.UpdateHeadIcon();
                if (UISystem.Instance.UIIsOpen(UIName))
                    UISystem.Instance.SystemSettingView.InitPlayerInfo();
                selfChangeIcon = false;
            }

            if (resp.attr != null)
                PlayerData.Instance.UpdatePlayerAttribute(resp.attr);
            //Debug.LogError("更新主界面信息");
        }
    }
    public void SendHeadChangeRequset(uint id)
    {
        ChangeIconReq req = new ChangeIconReq();
        req.type = IconType.ICON;
        req.new_id = id;
        mNetWork.SendChangeReq(req);
    }
    public void SendGetIconListRequset()
    {
        GetIconListReq req = new GetIconListReq();
        req.type = IconType.ICON;
        mNetWork.SendGetIconListReq(req);
    }
    public void GetIconListResp(GetIconListResp resp)
    {

        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            //Debug.LogError("sssssssssssss=" + resp.list.Count);
            if (resp.type == IconType.ICON)
            {
                //UISystem.Instance.SystemSettingView.InitChangeHeadView(resp.list);

            }
        }
    }

    public void SendSetLocalNotificationRequset(NotifySwitch _Switch)
    {
        SetNotifyReq req = new SetNotifyReq();
        req.setting_switch = _Switch;
        mNetWork.SendSetLocalNotificationReq(req);
    }
    public void ReceiveSetLocalNotificationResp(SetNotifyResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.PHPowerSystemPush = resp.setting_switch.ph_power_switch;
            PlayerData.Instance.EnSlaveSystemPush = resp.setting_switch.enslave_switch;
            PlayerData.Instance.DrawEquipSystemPush = resp.setting_switch.draw_equip_switch;
            PlayerData.Instance.RecruitSystemPush = resp.setting_switch.recruit_switch;
            PlayerData.Instance.ShopFreshSystemPush = resp.setting_switch.shop_fresh_switch;
            PlayerData.Instance.UnionClashSystemPush = resp.setting_switch.union_clash_switch;
            PlayerData.Instance.GCLDSystemPush = resp.setting_switch.campaign_switch;
            //Debug.LogError("SU=    "+PlayerData.Instance.PHPowerSystemPush + "   " + PlayerData.Instance.EnSlaveSystemPush + "    " + PlayerData.Instance.DrawEquipSystemPush + "    " + PlayerData.Instance.RecruitSystemPush + "   " + PlayerData.Instance.ShopFreshSystemPush + "   " + PlayerData.Instance.UnionClashSystemPush);
            LocalNotificationManager.Instance.SPPresent();
            LocalNotificationManager.Instance.DrawEquip();
            LocalNotificationManager.Instance.GreenAgainst();
            LocalNotificationManager.Instance.BlueAgainst();
            LocalNotificationManager.Instance.SPRecoverMax();
            string[] str = resp.push_tag.ToArray();
            SDKManager.Instance.SetAliasAndTags(PlayerData.Instance._RoleID.ToString(), str);
        }
    }

    public void SendBlockItemRequest()//查询屏蔽玩家(打开界面)
    {
        mNetWork.SendBlockItemReq();


    }
    public void ReceiveBlockItemResp(GetScreenInfoListResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.SystemSettingView.InitRemoveBlockItem(resp.blocked_info_list);
        }
    }

    public void SendRemoveBlockRequest(string _accname, uint _area_id, string blockName)//取消屏蔽
    {
        UnblockedPlayerReq req = new UnblockedPlayerReq();
        _BlockName = blockName;
        req.accname = _accname;
        req.area_id = _area_id;
        mNetWork.SendRemoveBlockReq(req);
    }
    public void ReceiveRemoveBlockResp(UnblockedPlayerResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            for (int i = 0; i < PlayerData.Instance.BlockedPlayers.Count; i++)
            {
                OtherList blockedPlayer = PlayerData.Instance.BlockedPlayers[i];
                if (resp.accname.Equals(resp.accname))
                {
                    PlayerData.Instance.BlockedPlayers.RemoveAt(i);
                    break;
                }
            }
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.REMOVEBLOCKOK, _BlockName));
            UISystem.Instance.SystemSettingView.CloseRemoveBlockView();
        }
    }

    public void SendShieldingPlayersRequest(string _accname, uint _area_id, string _blockName)//屏蔽玩家
    {
        BlockedPlayerReq req = new BlockedPlayerReq();
        _BlockName = _blockName;
        req.accname = _accname;
        req.area_id = _area_id;
        mNetWork.SendShieldingPlayersReq(req);
    }
    public void ReceiveShieldingPlayerResp(BlockedPlayerResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.BLOCKPLAYER, _BlockName));
            PlayerData.Instance.UpdateBlockedPlayer(resp.accname, resp.area_id);
            if (ShieldPlayerEvent != null)
                ShieldPlayerEvent();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
            Debug.Log("SystemSettingModule ShieldingPlayer Result = " + resp.result);//1090->已屏蔽过玩家;1091->屏蔽人数超过上限
        }
    }

    /// <summary>
    /// 向服务器请求头像框列表
    /// </summary>
    public void SendEnterFrameReq()
    {
        Debug.Log("send SendEnterFrameReq requst");
        EnterFrameReq data = new EnterFrameReq();
        mNetWork.SendEnterFrameReq(data);
    }
    /// <summary>
    /// 服务器返回头像框列表
    /// </summary>
    /// <param name="resp"></param>
    public void ReceiveEnterFrameResp(EnterFrameResp data)
    {
        Debug.Log("receive SendEnterFrameReq requst");
        if (data == null)
        {
            Debug.LogError("none data");
        }
        else
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                try
                {
                    _frameList.Clear();
                    foreach (FrameInfo achieve in data.frameinfo)
                    {
                        FrameInfo tmp = new FrameInfo();
                        tmp.id = achieve.id;
                        tmp.status = achieve.status;
                        tmp.resettime = achieve.resettime;
                        _frameList.Add(tmp);

                        //Debug.LogError("id:" + tmp.id + "  status:" + tmp.status + "  resettime:" + tmp.resettime);

                    }
                    UISystem.Instance.SystemSettingView.InitFrameList(_frameList);
                }
                catch
                {
                    Debug.LogError("there's something wrong in frameinfo");
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }
    }
    /// <summary>
    /// 向服务器请求头像列表
    /// </summary>
    public void SendEnterIconReq()
    {
        Debug.Log("send SendEnterIconReq requst");
        EnterIconReq data = new EnterIconReq();
        mNetWork.SendEnterIconReq(data);
    }
    /// <summary>
    /// 服务器返回头像列表
    /// </summary>
    public void ReceiveEnterIconResp(EnterIconResp data)
    {
        Debug.Log("receive ReceiveEnterIconResp requst");
        if (data == null)
        {
            Debug.LogError("none data");
        }
        else
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                try
                {
                    _iconList.Clear();
                    foreach (IconInformation achieve in data.iconinfo)
                    {
                        IconInformation tmp = new IconInformation();
                        tmp.id = achieve.id;
                        tmp.status = achieve.status;
                        tmp.resettime = achieve.resettime;
                        _iconList.Add(tmp);
                    }
                    UISystem.Instance.SystemSettingView.InitIconList(_iconList);
                }
                catch
                {
                    Debug.LogError("there's something wrong in IconInfo");
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }
    }
}
