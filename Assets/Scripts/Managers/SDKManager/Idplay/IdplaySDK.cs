using System;
using System.Collections.Generic;
using UnityEngine;
using PlatformSDKNamespace.IdplayManager;
using System.Runtime.InteropServices;

namespace PlatformSDKNamespace.IdplayInterface
{
    public class IdplaySDK : MonoSingleton<IdplaySDK>
    {

#if UNITY_EDITOR
#elif UNITY_ANDROID
        protected AndroidJavaObject idplay_sdkjo;
#elif UNITY_IOS
		[DllImport("__Internal")]
		private static extern void showLoginWindow(string str);
		[DllImport("__Internal")]
		private static extern void showQuickTool(bool show);
		[DllImport("__Internal")]
		private static extern void showBindingView();
		[DllImport("__Internal")]
		private static extern void setServerID(string serverID,string responseURL);
		[DllImport("__Internal")]
		private static extern void setRoleInfo(string roleID,string roleName);
		[DllImport("__Internal")]
		private static extern void loginout();
		[DllImport("__Internal")]
		private static extern void showAppleStoreView(string info,bool isMonthCard);
		[DllImport("__Internal")]
		private static extern void facebookInvitation(string ids);
		[DllImport("__Internal")]
		private static extern void getFacebookFriends();
		[DllImport("__Internal")]
		private static extern void getFacebookInvitableFriends();
		[DllImport("__Internal")]
		private static extern void facebookShare(string shareURL,string shareName,string shareDescription,string sharePictureUrl,string shareCaption);
		[DllImport("__Internal")]
		private static extern void lineShareText(string content);
		[DllImport("__Internal")]
		private static extern void showServiceView();
#endif

        public override void Init()
        {
            base.Init();
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("Initialize", gameObject.name);
#elif UNITY_IOS

#endif
        }


        #region  SDK Request

        #region  登陆相关流程
        public void PlatformLogin(params object[] param)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("LoginPlatform");
#elif UNITY_IPHONE
			showLoginWindow(gameObject.name);
#endif
        }

		public void ShowFloatTool(bool show)
		{
			#if UNITY_EDITOR
			
			#elif UNITY_ANDROID

			#elif UNITY_IPHONE
			showQuickTool(show);
			#endif
		}

        public void SetServerID(string serverID, string responseURL)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            if (string.IsNullOrEmpty(responseURL))
            {
                idplay_sdkjo.Call("SetServerID", serverID);
            }
            else
            {
                idplay_sdkjo.Call("SetServerID", serverID, responseURL);
            }
#elif UNITY_IPHONE
			setServerID( serverID, responseURL);
#endif
        }

        public void SetRoleInfo(string roleID, string roleName)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("SetRoleID", roleID, roleName);
#elif UNITY_IPHONE
			setRoleInfo( roleID, roleName);
#endif
        }

        public void QuickEnterGame(string serverID, string roleID, string nickname, string responseURL)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("QuickLogin", serverID, roleID, nickname, responseURL);
#elif UNITY_IPHONE
			setServerID( serverID, responseURL);
			setRoleInfo( roleID, nickname);
#endif
        }


        public void NormalLoginout()
        {
            //不在调用SDK 正常退出    modify by taiwei
            return;
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("NormalLogout");
#elif UNITY_IPHONE
			loginout();
#endif
        }

        public void QuitGame()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("GameExit");
#elif UNITY_IPHONE
			loginout();
#endif
        }
        #endregion

        #region  充值相关操作
        /// <summary>
        /// 充值操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="state">是否打开第三方支付 0-不打开 1-打开</param>
        /// <param name="tmpFlag">绑定完成是否关闭充值页面 1-绑定后关闭充值流程 0-不关闭继续充值流程</param>
        /// <param name="iscard">充值方式 false-普通 true-月卡</param>
        public void ShowRecharge(string info, string state, int tmpFlag, bool iscard)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("OpenDeposits", info, state, tmpFlag, iscard);
#elif UNITY_IPHONE
			showAppleStoreView( info, iscard);
#endif
        }

        /// <summary>
        /// 是否开启第三方充值操作
        /// </summary>
        /// <param name="isopen">true 开启</param>
        public void EnableThirdPartyPayment(bool isopen)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("SetEnableThirdPartyPayment", isopen);
#elif UNITY_IPHONE

#endif
        }

        public void NotifySDKRechargeInfo(string currency, string amount)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("OnPurchaseSuccess", currency, amount);
#elif UNITY_IPHONE

#endif
        }

        #endregion

        #region 分享相关

        public void ShareToFacebook(int requestCode, List<string> list)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("ShareToFacebook", requestCode, list.ToArray());
#elif UNITY_IPHONE
			facebookShare(list[3],list[0],list[2],list[4],list[1]);
#endif
        }

        public void InviteFriendsByFacebook(int requestCode)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("InviteByFacebook", requestCode);
#elif UNITY_IPHONE
#endif
        }

        public void InviteFriendByFacebook(int requestCode, string friendID)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("InviteFacebookFriend", requestCode, friendID);
#elif UNITY_IPHONE
			facebookInvitation(friendID);
#endif
        }

        public void GetFBInvitableFriends(int requestCode)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("GetFacebookInvitableFriends", requestCode);
#elif UNITY_IPHONE
			getFacebookInvitableFriends();
#endif
        }

        public void GetFacebookFriends(int requestCode)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("GetFacebookFriends", requestCode);
#elif UNITY_IPHONE
			getFacebookFriends();
#endif
        }

        public void ShareTextByLine(string content)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("ShareToLineByText", content);
#elif UNITY_IPHONE
#endif
        }

        public void ShareImageByLine(string image)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("ShareToLineByImage", image);
#elif UNITY_IPHONE
#endif
        }

        #endregion

        #region 绑定相关

        public int IsBindingPlatform()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            return idplay_sdkjo.Call<int>("IsBindingPlatform");
#elif UNITY_IPHONE
#endif
            return 0;
        }

        public void OpenTouristBinding(string vContent)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("OpenTouristBinding", vContent);
#elif UNITY_IPHONE
			showBindingView();
#endif
        }

        #endregion

        #region 推送相关

        public void SetAliasAndTags(string alias, string[] tags)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("SetAliasAndTags", alias, tags); ;
#elif UNITY_IPHONE

#endif
        }

        public void StopJPush()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("StopJPush");
#elif UNITY_IPHONE

#endif
        }

        public void ResumeJPush()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("ResumeJPush");
#elif UNITY_IPHONE

#endif
        }

        public bool IsStopJPush()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call<bool>("IsStopJPush");
#elif UNITY_IPHONE

#endif
            return false;
        }

        public void SetPushTime(int[] vWeekDays, int vStartHour, int vEndHour)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("SetPushTime", vWeekDays, vStartHour, vEndHour);
#elif UNITY_IPHONE

#endif
        }

        public void SetSilenceTime(int vStartHour, int vStartMinute, int vEndHour, int vEndMinute)
        {

#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("SetSilenceTime", vStartHour, vStartMinute, vEndHour, vEndMinute);
#elif UNITY_IPHONE

#endif
        }

        public bool GetConnectionState()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call<bool>("GetConnectionState");
#elif UNITY_IPHONE

#endif
            return false;
        }


        #endregion

        #region 其他
        public void AndroidKeyCode_Back()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("InputKeyCodeBack");
#elif UNITY_IPHONE
#endif
        }

        public bool IsAppInstall(string packagename)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            return idplay_sdkjo.Call<bool>("IsAppInstalled", packagename);
#elif UNITY_IPHONE
#endif
            return false;
        }


        public bool IsLoginActivity()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            return idplay_sdkjo.Call<bool>("IsLoginActivity");
#elif UNITY_IPHONE
#endif
            return false;
        }

        public void ShowCustomerService()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            GetActivity();
            idplay_sdkjo.Call("OpenCustomerService");
#elif UNITY_IPHONE
			showServiceView();
#endif
        }

        #endregion

        #endregion

        #region SDK Callback
        public void CallBack_ObtainSignInfos(string response)
        {
            IdplaySDKManager.Instance.Callback_PlatformLogin(response);
        }

        public void CallBack_ObtainFriendInfos(string friendinfos)
        {
            IdplaySDKManager.Instance.CallBack_ObtainFriendInfos(friendinfos);
        }

        public void CallBack_ObtainInvitableFriendInfos(string vFriendInfos)
        {
            IdplaySDKManager.Instance.CallBack_ObtainInvitableFriendInfos(vFriendInfos);
        }

        public void CallBack_SetAliasAndTags(string response)
        {
            IdplaySDKManager.Instance.CallBack_SetAliasAndTags(response);
        }
        public void CallBack_InviteFacebookFriend(string response)
        {
            IdplaySDKManager.Instance.CallBack_InviteFacebookFriend(response);
        }

        public void CallBack_InviteByFacebook(string response)
        {
            IdplaySDKManager.Instance.CallBack_InviteFriendsByFacebook(response);
        }

        public void CallBack_ShareToFacebook(string response)
        {
            IdplaySDKManager.Instance.CallBack_ShareToFacebook(response);
        }

		public void CallBack_BindingSuccess(string response)
		{
			IdplaySDKManager.Instance.CallBack_BindingSuccess(response);
		}

        public void CallBack_GameExit(string response)
        {
            IdplaySDKManager.Instance.CallBack_GameExit(response);
        }

        #endregion


        private void GetActivity()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            if (idplay_sdkjo == null)
            {
                AndroidJavaClass tmpJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                idplay_sdkjo = tmpJC.GetStatic<AndroidJavaObject>("currentActivity");
            }
#elif UNITY_IPHONE
#endif
        }

        public void UnInitialize()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            idplay_sdkjo = null;
#elif UNITY_IPHONE
#endif
        }
    }
}