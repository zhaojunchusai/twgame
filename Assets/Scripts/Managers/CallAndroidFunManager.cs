using UnityEngine;
using System.Collections;

public class CallAndroidFunManager : MonoSingleton<CallAndroidFunManager> 
{
# if UNITY_ANDROID
    private AndroidJavaObject restartjo;
    private string _restart = "restart";
    private AndroidJavaObject readFilejo;
    private string _isFileExists = "isFileExists";
    private string _getBytes = "getBytes";

    public void Initialize() 
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (restartjo == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.pixone.restartgame.Restart");
                restartjo = jc.CallStatic<AndroidJavaObject>("getInstance", gameObject.name);
            }

            if (readFilejo == null) 
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.pixone.readfile.ReadFile");
                readFilejo = jc.CallStatic<AndroidJavaObject>("getInstance", gameObject.name);
            }
        }

    }

    public void RestartGame() 
    {
        restartjo.Call("restart");
    }


    public void RestartGame(int delay) 
    {
        restartjo.Call("restart", delay);
    }

    //全路径包含后缀名
    public byte[] GetFileBytes(string path) 
    {
        
        return readFilejo.Call<byte[]>("getBytes", path);   
    }

    //判断是否有此文件
    public bool isFileExists(string path) 
    {
        return readFilejo.Call<bool>("isFileExists", path);   
    }


# endif
}
