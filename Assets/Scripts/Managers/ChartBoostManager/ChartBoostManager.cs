//using UnityEngine;
//using System.Collections;
//using ChartboostSDK;
//public class ChartBoostManager : MonoSingleton<ChartBoostManager>
//{

//    private string APP_ID
//    {
//        get
//        {
//#if UNITY_ANDROID
//            return "56c287faf7898270e74470e3";
//#elif UNITY_IPHONE
//return "57426d7604b016490c57735d";
//#endif
//            return string.Empty;
//        }
//    }

//    private string APP_SIGNATURE
//    {
//        get
//        {
//#if UNITY_ANDROID
//            return "017e6bcc8ff120358e6c0000293b3c9dbd90cf69";
//#elif UNITY_IPHONE
//return "9302e13ddac00c1b128b92ea3f0a00f478b95bc5";
//#endif
//            return string.Empty;
//        }
//    }


//    public void Initialize()
//    {
//        Chartboost.CreateWithAppId(APP_ID, APP_SIGNATURE);
//    }


//    public void Uninitialize()
//    {

//    }
//}
