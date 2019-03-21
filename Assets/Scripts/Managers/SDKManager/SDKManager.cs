using System;
using System.Collections.Generic;
using UnityEngine;
using PlatformSDKNamespace.IdplayManager;
public class SDKManager : Singleton<SDKManager>
{
    public delegate void KeyCodeBackDelegate();
    public event KeyCodeBackDelegate KeyCodeBackEvent;
    public void Initialize()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.Initialize();
                } break;
        }
    }

    #region 登陆流程相关
    public void PlatformLogin()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.PlatformLogin();
                } break;
        }
    }

    public void QuickEnterGame(string serverID, string roleID, string nickname, uint accountID, string accountName)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.QuickEnterGame(serverID, roleID, nickname, accountID, accountName);
                } break;
        }
    }


    public void SetServerID(string serverID, string responseURL = "")
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.SetServerID(serverID, responseURL);
                } break;
        }

    }

    public void SetRoleInfo(string roleID, string roleName)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.SetRoleInfo(roleID, roleName);
                } break;
        }
    }

    public void NormalLoginout()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.NormalLoginout();
                } break;
        }

    }

    public void QuitGame()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.QuitGame();
                } break;
        }
    }

    #endregion

    #region 充值相关
    public void ShowRecharge(params object[] param)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.ShowRecharge(param);
                } break;
        }
    }

    public void EnableThirdPartyPayment(bool isopen)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.EnableThirdPartyPayment(isopen);
                } break;
        }
    }

    public void NotifySDKRechargeInfo(string currency, string amount)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.NotifySDKRechargeInfo(currency, amount);
                } break;
        }
    }

    #endregion

    #region 分享相关

    public void ShareToFacebook(List<string> list, int requestCode = 200)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.ShareToFacebook(requestCode, list);
                } break;
        }
    }

    public void InviteFriendsByFacebook(int requestCode = 200)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.InviteFriendsByFacebook(requestCode);
                } break;
        }
    }

    public void InviteFriendByFacebook(string friendID)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.InviteFriendsByFacebook(friendID);
                } break;
        }
    }

    public void GetFBInvitableFriends(int requestCode = 200)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.GetFBInvitableFriends(requestCode);
                } break;
        }
    }

    public void GetFacebookFriends(int requestCode = 200)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.GetFacebookFriends(requestCode);
                } break;
        }
    }
    public void ShareTextByLine(string content)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.ShareTextByLine(content);
                } break;
        }
    }
    public void ShareImageByLine(string content)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.ShareImageByLine(content);
                } break;
        }
    }

    #endregion

    #region 绑定相关

    /// <summary>
    /// 绑定状态  0：表已绑定  1：游客身份登陆 未绑定  2：FB身份登陆 未绑定
    /// </summary>
    public int IsBindingPlatform()
    {
//		return 1;
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    int code = IdplaySDKManager.Instance.IsBindingPlatform();
                    return code;
                } break;
        }
        return 0;
    }

    public void OpenTouristBinding(string content)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.OpenTouristBinding(content);
                } break;
        }
    }

    #endregion

    #region 推送相关

    public void SetAliasAndTags(string alias, string[] tags)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.SetAliasAndTags(alias, tags);
                } break;
        }
    }

    public void StopJPush()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.StopJPush();
                } break;
        }
    }

    public void ResumeJPush()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.ResumeJPush();
                } break;
        }
    }

    public bool IsStopJPush()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    bool isStop = IdplaySDKManager.Instance.IsStopJPush();
                    return isStop;
                } break;
        }
        return false;
    }

    public void SetPushTime(int[] vWeekDays, int vStartHour, int vEndHour)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.SetPushTime(vWeekDays, vStartHour, vStartHour);
                } break;
        }
    }

    public void SetSilenceTime(int vStartHour, int vStartMinute, int vEndHour, int vEndMinute)
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.SetSilenceTime(vStartHour, vStartMinute, vEndHour, vEndMinute);
                } break;
        }
    }

    public bool GetConnectionState()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    bool status = IdplaySDKManager.Instance.GetConnectionState();
                    return status;
                } break;
        }
        return false;
    }

    #endregion

    #region 其他
    public void AndroidKeyCode_Back()
    {
        if (KeyCodeBackEvent != null)
        {
            KeyCodeBackEvent();
        }
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    string stateName = Main.Instance.StateMachine.CurrentState().GetType().Name;
                    if (stateName == Assets.Script.Common.StateMachine.MainCityState.StateName || stateName == Assets.Script.Common.StateMachine.FightState.StateName)
                    {
                        IdplaySDKManager.Instance.AndroidKeyCode_Back();
                    }
                } break;
        }
    }

    /// <summary>
    /// 打开客服
    /// </summary>
    public void ShowCustomerService()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplaySDKManager.Instance.ShowCustomerService();
                } break;
        }
    }

    #endregion



    public SDKBaseUserData GetSDKUserData()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    return (SDKBaseUserData)IdplaySDKManager.Instance.GetSDKUserData();
                }
        }
        return null;
    }

    public string GetDownloadURL()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    return IdplaySDKManager.Instance.GetDownloadURL();
                } break;
        }
        return string.Empty;
    }

    public string GetSDKSignInfo()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    IdplayUserData data = IdplaySDKManager.Instance.GetSDKUserData();
                    string sign = data.userid + data.openuid + data.nickname + data.sex + data.logintime;
                    return sign;
                }
                break;
        }
        return string.Empty;
    }

    public bool IsAppInstall(string packagename = "jp.naver.line.android")
    {
        //正常情况下  该方法应该是通用工具  而不应该集中于某一个sdk  ->add by taiwei
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    return IdplaySDKManager.Instance.IsAppInstall(packagename);
                }
                break;
        }
        return false;
    }


    public bool IsLoginActivity()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    return IdplaySDKManager.Instance.IsLoginActivity();
                }
                break;
        }
        return false;
    }

    #region SDK CallBack

    public void Callback_PlatformLogin()
    {
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    string userID = IdplaySDKManager.Instance.GetSDKUserData().userid;
                    ServerListManager.Instance.StartDownloadServerList(userID);
                    LoginModule.Instance.isNormalLogin = false;
                    //开始选择分区流程//
                    UISystem.Instance.HintView.ShowLoading(false);
					//UISystem.Instance.LoginView.OpenServerBtn();
                } break;
        }
    }

    #region Idplay Callback
    public void CallBack_ObtainFriendInfos(List<object> list)
    {
        FriendModule.Instance.ReceiveGetObtainFBFreinds(list);
    }

    public void CallBack_ObtainInvitableFriendInfos(List<object> list)
    {
        FriendModule.Instance.ReceiveGetFacebookFriends(list);
    }

    public void CallBack_SetAliasAndTags(string alias, string[] tags)
    {

    }
    public void CallBack_InviteFacebookFriend()
    {
        FriendModule.Instance.ReceiveShareToFaceBook();
    }

    public void CallBack_InviteFriendsByFacebook()
    {
    }

    public void CallBack_ShareToFacebook()
    {

    }

    public void CallBack_GameExit()
    {
        Main.Instance.QuitGame();
    }

    #endregion
    #endregion

    public void UnInitialize()
    {

    }

}
