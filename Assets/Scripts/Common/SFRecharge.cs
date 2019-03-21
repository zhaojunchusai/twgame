using System.Text;
using UnityEngine;
using System.Collections;
using System.IO;

public class SFRecharge : MonoSingleton<SFRecharge> 
{
    private string _api = "http://357p.com/wap/?m=MUUK";
    private string _api2 = "http://357p.com/wap/?m=NENE&username={0}&fee={1}&pname={2}&jinzhua={3}&jinzhub={4}&jinzhuc={5}&qqyc=1&zhidu=1";

    private WWW _www;

    public delegate void RechargeCallback(bool result);
 
    public RechargeCallback  _callBcak;

    public WebViewBehavior webview;

    private WebViewCallback m_callback;

    public void Initialize()
    {
         if(webview == null)
             webview = gameObject.AddComponent<WebViewBehavior>();
         if(m_callback == null)
             m_callback = new WebViewCallback();
         webview.setCallback(m_callback); 
    }

    public void SetVisibility(bool visibility) 
    {
        if (webview != null)
            webview.SetVisibility(visibility);
    }

    public void SetMargins(int left, int top, int right, int bottom)
    {
        if (webview != null)
            webview.SetMargins(left, top, right, bottom);
    }

    public void OnSendRecharge(string money, string orderNumber,string serverName)
    {
        SendHttpRecharge(PlayerData.Instance._AccountID.ToString(), money, orderNumber, serverName);
    }

    public void SendHttpRecharge(string username, string money, string orderNumber, string serverName) 
    {
        string url = string.Format(_api2, username, money, serverName, orderNumber, orderNumber, PlayerData.Instance._AccountName);
        #if UNITY_ANDROID || UNITY_IPHONE
        if (webview != null) 
        {
            webview.SetVisibility(true);
            webview.LoadURL(url);
        }  
        #else
            Application.OpenURL(url);
        #endif
    }

    public IEnumerator OnSendRecharge(string username, string money, string orderNumber, string serverName) 
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("fee", money);
        form.AddField("pname", "S1");
        form.AddField("jinzhua", orderNumber);
        form.AddField("jinzhub", orderNumber);
        form.AddField("jinzhuc", PlayerData.Instance._AccountName);
        form.AddField("qqyc", "1");
        form.AddField("zhidu", "1");
        try
        {
            _www = new WWW(_api, form);
        }
        catch(System.Exception ex)
        {
            Debug.LogError(ex.Message);
            if (_callBcak !=null)
                _callBcak(false);
            yield break;
        }
        yield return _www;
        if (!string.IsNullOrEmpty(_www.error))
        {
            if (_callBcak != null)
               _callBcak(false);
            yield break;
        }
        if (_callBcak != null)
           _callBcak(true);
        Debug.LogError(_www.text);

    }

    public void Uninitialize()
    {
        if (webview != null)
            webview.OnDestroy();
        webview = null;
    }
}

class WebViewCallback: Kogarasi.WebView.IWebViewCallback
{
    public void onLoadStart(string url)
    {
        Debug.Log("call onLoadStart : " + url);
    }
    public void onLoadFinish(string url)
    {
        Debug.Log("call onLoadFinish : " + url);
    }
    public void onLoadFail(string url)
    {
        Debug.Log("call onLoadFail : " + url);
    }
}