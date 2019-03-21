using UnityEngine;
using System.Collections;

public class BuglyManager : MonoSingleton<BuglyManager> 
{
    private string _appID = "900007733";
    public void Initialize()
    {
        //BuglyAgent.ConfigDebugMode(true);
        BuglyAgent.RegisterLogCallback(CallbackDelegate.Instance.OnApplicationLogCallbackHandler);
        #if UNITY_IPHONE || UNITY_IOS
		BuglyAgent.InitWithAppId (_appID);
		#elif UNITY_ANDROID
		BuglyAgent.InitWithAppId (_appID);
		#endif
        BuglyAgent.EnableExceptionHandler();
    }

    public void SetUserID(string userid) 
    {
        BuglyAgent.SetUserId(userid);
    }

    public void SetConfigDefault(string channel, string version, string user, long delay) 
    {
        BuglyAgent.ConfigDefault(channel, version, user, delay);
    }

    public void Uninitialize()
    {
        
    }


}
