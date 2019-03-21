using System;
using System.Collections.Generic;
using UnityEngine;
using PlatformSDKNamespace.IdplayInterface;

namespace PlatformSDKNamespace.IdplayManager
{
    public class IdplaySDKManager : Singleton<IdplaySDKManager>
    {
        private IdplayUserData idplayUserData;

        private const string SDKAWARDRESPONSE_URL = "http://61.219.16.33:8080/ltd_test_recharge/andriod/p7725/bindAccount.php?binduser={0}_{1}_{2}"; //accid_accname_areaid 
        private string APKDOWNLOAD_URL
        {
            get
            {
                string url = string.Empty;
                switch (GlobalConst.PLATFORM)
                {
                    case TargetPlatforms.Android_7725OL:
                        {
                            url = "https://play.google.com/store/apps/details?id=com.idplay.legu.dtlqs";
                        } break;
                    case TargetPlatforms.Android_7725:
                        {
                            url = "https://play.google.com/store/apps/details?id=com.idplay.legu.binglin";
                        }
                        break;
                }
                return url;
            }
        } 

        public void Initialize()
        {
            IdplaySDK.Instance.Init();
        }
        #region SDK Request

        #region 登陆相关
        public void PlatformLogin(params object[] param)
        {
            IdplaySDK.Instance.PlatformLogin(param);
        }

        public void QuickEnterGame(string serverID, string roleID, string nickname, uint accountID, string accountName)
        {
            string url = string.Format(SDKAWARDRESPONSE_URL, accountID, accountName, serverID);
            IdplaySDK.Instance.QuickEnterGame(serverID, roleID, nickname, url);
        }

        public void SetServerID(string serverID, string responseURL = "")
        {
            IdplaySDK.Instance.SetServerID(serverID, responseURL);
        }

        public void SetRoleInfo(string roleID, string roleName)
        {
            IdplaySDK.Instance.SetRoleInfo(roleID, roleName);
        }

        public void NormalLoginout()
        {
            IdplaySDK.Instance.NormalLoginout();
        }

        public void QuitGame()
        {
            IdplaySDK.Instance.QuitGame();
        }

        #endregion

        #region 充值相关

        public void ShowRecharge(params object[] param)
        {
            if (param == null || param.Length < 4)
            {
                Debug.LogError("ShowRecharge error params !!!!!!!");
                return;
            }
            string info = (string)param[0];
            string state = (string)param[1];
            int flag = (int)param[2];
            bool iscard = (bool)param[3];
            IdplaySDK.Instance.ShowRecharge(info, state, flag, iscard);

			#if UNITY_IPHONE
			if(idplayUserData.isbind == 2)
			{
				OpenTouristBinding("");
			}
			#endif
        }

        public void EnableThirdPartyPayment(bool isopen)
        {
            IdplaySDK.Instance.EnableThirdPartyPayment(isopen);
        }

        public void NotifySDKRechargeInfo(string currency, string amount)
        {
            IdplaySDK.Instance.NotifySDKRechargeInfo(currency, amount);
        }

        #endregion

        #region 分享相关

        public void ShareToFacebook(int requestCode, List<string> list)
        {
            IdplaySDK.Instance.ShareToFacebook(requestCode, list);
        }

        public void InviteFriendsByFacebook(int requestCode = 200)
        {
            IdplaySDK.Instance.InviteFriendsByFacebook(requestCode);
        }

        public void InviteFriendsByFacebook(string friendID, int requestCode = 200)
        {
            IdplaySDK.Instance.InviteFriendByFacebook(requestCode, friendID);
        }

        public void GetFBInvitableFriends(int requestCode = 200)
        {
            IdplaySDK.Instance.GetFBInvitableFriends(requestCode);
        }

        public void GetFacebookFriends(int requestCode = 200)
        {
            IdplaySDK.Instance.GetFacebookFriends(requestCode);
        }
        public void ShareTextByLine(string content)
        {
            IdplaySDK.Instance.ShareTextByLine(content);
        }
        public void ShareImageByLine(string content)
        {
            IdplaySDK.Instance.ShareImageByLine(content);
        }

        #endregion

        #region 绑定相关

        /// <summary>
        /// 绑定状态  0：表已绑定  1：游客身份登陆 未绑定  2：FB身份登陆 未绑定
        /// </summary>
        public int IsBindingPlatform()
        {
			int code = 0;
#if UNITY_ANDROID
            code = IdplaySDK.Instance.IsBindingPlatform();
#elif UNITY_IPHONE
			code = idplayUserData.isbind == 2 ? 1 : 0;
#endif
            return code;
        }

        public void OpenTouristBinding(string content)
        {
            IdplaySDK.Instance.OpenTouristBinding(content);
        }

        #endregion

        #region 推送相关

        public void SetAliasAndTags(string alias, string[] tags)
        {
            IdplaySDK.Instance.SetAliasAndTags(alias, tags);
        }

        public void StopJPush()
        {
            IdplaySDK.Instance.StopJPush();
        }

        public void ResumeJPush()
        {
            IdplaySDK.Instance.ResumeJPush();
        }

        public bool IsStopJPush()
        {
            bool isStop = IdplaySDK.Instance.IsStopJPush();
            return isStop;
        }

        public void SetPushTime(int[] vWeekDays, int vStartHour, int vEndHour)
        {
            IdplaySDK.Instance.SetPushTime(vWeekDays, vStartHour, vStartHour);
        }

        public void SetSilenceTime(int vStartHour, int vStartMinute, int vEndHour, int vEndMinute)
        {
            IdplaySDK.Instance.SetSilenceTime(vStartHour, vStartMinute, vEndHour, vEndMinute);
        }

        public bool GetConnectionState()
        {
            bool status = IdplaySDK.Instance.GetConnectionState();
            return status;
        }

        #endregion

        #region 其他
        public void AndroidKeyCode_Back()
        {
            IdplaySDK.Instance.AndroidKeyCode_Back();
        }
        public bool IsAppInstall(string packagename)
        {
            return IdplaySDK.Instance.IsAppInstall(packagename);
        }

        public bool IsLoginActivity()
        {
            return IdplaySDK.Instance.IsLoginActivity();
        }

        public void ShowCustomerService() 
        {
            IdplaySDK.Instance.ShowCustomerService();
        }

        #endregion

        #endregion

        #region SDK Callback

        /// <summary>
        /// 获取并发送7725SDK返回会话签名
        /// </summary>
        public void Callback_PlatformLogin(string response)
        {
            if (!string.IsNullOrEmpty(response))
            {
                idplayUserData = null;

#if UNITY_ANDROID
                string[] userInfoArray = response.Split(';');
                if (userInfoArray == null || userInfoArray.Length <= 0)
                {
                    Debug.LogError("idplay  callback sign info is error -> " + response);
                    return;
                }
                idplayUserData = new IdplayUserData();
                for (int i = 0; i < userInfoArray.Length; i++)
                {
                    string userInfo = userInfoArray[i];
                    if (string.IsNullOrEmpty(userInfo))
                    {
                        continue;
                    }
					string[] userdata = userInfo.Split(':');
                    if (userdata != null && userdata.Length == 2) //约定格式  如果格式正确 则必定为长度2
                    {
                        if (userdata[0].Equals("sign"))
                        {
                            idplayUserData.sign = userdata[1];
                        }
                        else if (userdata[0].Equals("userid"))
                        {
                            idplayUserData.userid = userdata[1];
                        }
                        else if (userdata[0].Equals("openuid"))
                        {
                            idplayUserData.openuid = userdata[1];
                        }
                        else if (userdata[0].Equals("nickname"))
                        {
                            idplayUserData.nickname = userdata[1];
                        }
                        else if (userdata[0].Equals("sex"))
                        {
                            idplayUserData.sex = userdata[1];
                        }
                        else if (userdata[0].Equals("logintime"))
                        {
                            idplayUserData.logintime = userdata[1];
                        }
                        else if (userdata[0].Equals("logintype"))
                        {
                            idplayUserData.logintype = userdata[1];
                        }
                        else if (userdata[0].Equals("gamename"))
                        {
                            idplayUserData.gamename = userdata[1];
                        }
                    }
                }
#elif UNITY_IPHONE
				response =  GetUTF8String(response);
				string str = response.Substring(response.LastIndexOf('{') +1, response.IndexOf('}') - response.LastIndexOf('{') -1);
				string[] userInfoArray = str.Split(';');
				if (userInfoArray == null || userInfoArray.Length <= 0)
				{
					Debug.LogError("idplay  callback sign info is error -> " + response);
					return;
				}
				idplayUserData = new IdplayUserData();
				for (int i = 0; i < userInfoArray.Length; i++)
				{
					string userInfo = userInfoArray[i];
					if (string.IsNullOrEmpty(userInfo))
					{
						continue;
					}
					userInfo = ReplaceNouseLetter(userInfo);
					string[] userdata = userInfo.Split('=');
					if (userdata != null && userdata.Length == 2) //约定格式  如果格式正确 则必定为长度2
					{
						if (userdata[0].Contains("sign"))
						{
							idplayUserData.sign = userdata[1];
						}
						else if (userdata[0].Contains("userid"))
						{
							idplayUserData.userid = userdata[1];
						}
						else if (userdata[0].Contains("openuid"))
						{
							idplayUserData.openuid = userdata[1];
						}
						else if (userdata[0].Contains("nickname"))
						{
							idplayUserData.nickname = userdata[1];
						}
						else if (userdata[0].Contains("sex"))
						{
							idplayUserData.sex = GetSex(userdata[1]);
						}
						else if (userdata[0].Contains("logintime"))
						{
							idplayUserData.logintime = userdata[1];
						}
						else if (userdata[0].Contains("channel"))
						{
							idplayUserData.logintype = userdata[1];
						}
						else if (userdata[0].Contains("is_bind"))
						{
							idplayUserData.isbind = int.Parse(userdata[1]);
						}
					}
				}
#endif
                SDKManager.Instance.Callback_PlatformLogin();
            }
            else
            {
                Debug.LogError("idplay callback sign info is null or empty!");
            }
        }

		private string ReplaceNouseLetter(string str)
		{
			str = str.Replace ("\n","");
			str = str.Replace ("\"","");
			str = str.Replace (" ","");
			return str;
		}

		private string GetSex(string str)
		{
			string res="";
			string k = str.ToLower();
			switch(k)
			{
			case "\\u4fdd\\u5bc6":
				{

				res = "保密";
					break;
				}
			case "\\u7537":
				{
				res = "男";
					break;
				}
			case "\\u5973":
				{
				res = "女";
					break;
				}
			default:
				res = "傻逼";
				break;
			}
			return res;
		}

		private string GetUTF8String(string str)
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
			return System.Text.Encoding.UTF8.GetString(bytes);
		}
		
        public void CallBack_ObtainFriendInfos(string friendinfos)
        {
#if UNITY_IPHONE
			friendinfos = GetUTF8String (friendinfos);
#endif
            if (!string.IsNullOrEmpty(friendinfos))
            {
                List<object> friends = MiniJSON.Json.Deserialize(friendinfos) as List<object>;
//				Debug.LogError("---------unity----count=" +friends.Count);
//				foreach (var item in friends) {
//					Debug.LogError("---------------unityfriend---- " +item as String);
//				}
                SDKManager.Instance.CallBack_ObtainFriendInfos(friends);
            }
            else
            {
                Debug.LogError("idplay callback ObtainFriendInfos is null or empty!");
            }
        }

        public void CallBack_ObtainInvitableFriendInfos(string vFriendInfos)
        {
			#if UNITY_IPHONE
			vFriendInfos = GetUTF8String (vFriendInfos);
			#endif
            if (!string.IsNullOrEmpty(vFriendInfos))
            {
                List<object> friends = MiniJSON.Json.Deserialize(vFriendInfos) as List<object>;
//				Debug.LogError("---------unity----in-count=" +friends.Count);
//				foreach (var item in friends) {
//					Debug.LogError("---------------unityfriend---- " +item as String);
//				}
                SDKManager.Instance.CallBack_ObtainInvitableFriendInfos(friends);
            }
            else
            {
                Debug.LogError("idplay callback ObtainInvitableFriendInfos is null or empty!");
            }
        }

        public void CallBack_SetAliasAndTags(string vResult)
        {
            //{"tags":["binglin","lol"],"resultCode":0,"alias":"timo"}//
            Dictionary<string, object> result_dic = MiniJSON.Json.Deserialize(vResult) as Dictionary<string, object>;
            if (result_dic.ContainsKey("resultCode"))
            {
                string resultCode = result_dic["resultCode"].ToString();
                if (resultCode == "0")
                {
                    string alias = string.Empty;
                    if (result_dic.ContainsKey("alias"))
                    {
                        alias = result_dic["alias"].ToString();
                    }
                    List<string> tags = new List<string>();
                    if (result_dic.ContainsKey("tags"))
                    {
                        List<object> list = result_dic["tags"] as List<object>;
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                tags.Add(list[i].ToString());
                            }
                        }
                    }
                    SDKManager.Instance.CallBack_SetAliasAndTags(alias, tags.ToArray());
                }
                else
                {
                    Debug.LogError("SetAliasAndTags  Error：" + resultCode);
                }
            }
            else
            {
                Debug.LogError("CallBack_SetAliasAndTags parameter Error：" + vResult);
            }
        }
        public void CallBack_InviteFacebookFriend(string vResult)
        {
            //{"response":"success","resultCode":200}//
            Dictionary<string, object> dic = MiniJSON.Json.Deserialize(vResult) as Dictionary<string, object>;
            string resultCode = "0";
            string response = "failed";
            if (dic.ContainsKey("resultCode") && dic.ContainsKey("response"))
            {
                resultCode = dic["resultCode"].ToString();
                response = dic["response"].ToString();
                if (resultCode == "200")
                {
                    if (response == "success")
                    {
                        SDKManager.Instance.CallBack_InviteFacebookFriend();
                    }
                    else
                    {
                        Debug.Log("player canceled operating! ");
                    }
                }
                else
                {
                    Debug.LogError("InviteFacebookFriend  Error:" + resultCode);
                }
            }
            else
            {
                Debug.LogError("CallBack_InviteFacebookFriend parameter Error:" + vResult);
            }
        }

        public void CallBack_InviteFriendsByFacebook(string vResult)
        {
            //{"resultCode":200,"requestCode":200}
            Dictionary<string, object> dic = MiniJSON.Json.Deserialize(vResult) as Dictionary<string, object>;
            string resultCode = "0";
            if (dic.ContainsKey("resultCode"))
            {
                resultCode = dic["resultCode"] as string;
                if (resultCode == "200")
                {
                    SDKManager.Instance.CallBack_InviteFriendsByFacebook();
                }
                else
                {
                    Debug.LogError("InviteByFacebook  Error:" + resultCode);
                }
            }
            else
            {
                Debug.LogError("CallBack_InviteByFacebook parameter Error:" + vResult);
            }
        }

        public void CallBack_ShareToFacebook(string vResult)
        {
            SDKManager.Instance.CallBack_ShareToFacebook();
        }

		public void CallBack_BindingSuccess(string response)
		{
			idplayUserData.isbind = 1;
		}

        public void CallBack_GameExit(string response)
        {
            SDKManager.Instance.CallBack_GameExit();
        }

        #endregion

        public IdplayUserData GetSDKUserData()
        {
            return idplayUserData;
        }
        public string GetDownloadURL()
        {
#if UNITY_ANDROID
            return APKDOWNLOAD_URL;
#elif UNITY_IPHONE
#endif
            return string.Empty;
        }

        public void UnInitialize()
        {
            idplayUserData = null;
            IdplaySDK.Instance.UnInitialize();
        }
    }
}
