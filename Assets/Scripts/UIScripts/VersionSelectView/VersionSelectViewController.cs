using UnityEngine;
using System.Collections;
using Assets.Script.Common.StateMachine;

public class VersionSelectViewController : MonoBehaviour
{
    private static GameObject instance;
    private float _horizRatio;
    private float _vertRatio;
    private Vector2 _guiScale;
    private int _serverListCount = 0;
    private Vector2 _serverListScrollPosition;
    private ServerInfo _currentServerInfo;
    private byte _currentServerType;
    private string _serverType;
    private string _isOpenGM;
    private string _isOpenGuide;
    private bool _isHint;
    private float _beginHintTime = 0.0F;

    public static void Initialize()
    {
        if (instance == null)
        {
            instance = new GameObject("VersionSelectView");
            instance.AddComponent<VersionSelectViewController>();
        }
    }

    void Start() 
    {
        _horizRatio = Screen.width / (float)1024;
        _vertRatio = Screen.height / (float)576;
        _guiScale = new Vector2(_horizRatio, _vertRatio);
        _serverListCount = ServerInfoManager.Instance._serverInfoConfig.mServerInfoList.Count;
        _currentServerInfo = ServerInfoManager.Instance._serverInfoConfig.GetServerInfoByType((byte)GlobalConst.SERVER_TYPE);
        UpdateServerInfo();
    }

    void OnGUI()
    {
        if (Time.time - _beginHintTime > 5.0F)
            _isHint = false;
        GUIUtility.ScaleAroundPivot(_guiScale, Vector2.zero);

        GUILayout.Window(0, new Rect(20, 20, 984, 536), DoMyWindow, "测试包专用服务器列表选择");

    }

    void DoMyWindow(int windowID) 
    {
        GUILayout.Label("游戏平台: " + GlobalConst.PLATFORM);
        GUILayout.Label("服务器列表:");
        _serverListScrollPosition = GUILayout.BeginScrollView(_serverListScrollPosition, GUILayout.Height( (_serverListCount / 2 + _serverListCount%2) * 30+20 ));
        for (int i = 0; i < _serverListCount;)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(50);
            if (GUILayout.Button(ServerInfoManager.Instance._serverInfoConfig.mServerInfoList[i].serverName,GUILayout.Width(410),GUILayout.Height(30)))
            {
                _currentServerInfo = ServerInfoManager.Instance._serverInfoConfig.mServerInfoList[i];
                UpdateServerInfo();
            }
            GUILayout.Space(30);
            i++;
            if (i < _serverListCount)
            {
                if (GUILayout.Button(ServerInfoManager.Instance._serverInfoConfig.mServerInfoList[i].serverName,GUILayout.Width(410),GUILayout.Height(30)))
                {
                    _currentServerInfo = ServerInfoManager.Instance._serverInfoConfig.mServerInfoList[i];
                    UpdateServerInfo();
                }
                i++;
            }
           GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.Label("手动设置 风险很大不会设置的不要乱操作");
        GUILayout.BeginHorizontal();

        GUILayout.Label("服务器名:");
        _currentServerInfo.serverName = GUILayout.TextField(_currentServerInfo.serverName);

        GUILayout.Label("服务器类型:");
        _serverType = GUILayout.TextField(_serverType);

        GUILayout.Label("VersionName:");
        _currentServerInfo.versionName = GUILayout.TextField(_currentServerInfo.versionName);

        GUILayout.Label("打开或关闭GM(0:关闭 1:打开)");
        _isOpenGM = GUILayout.TextField(_isOpenGM, GUILayout.MinWidth(50));

        GUILayout.Label("打开或关闭新手引导(0:关闭 1:打开)");
        _isOpenGuide = GUILayout.TextField(_isOpenGuide, GUILayout.MinWidth(50));

        GUILayout.EndHorizontal();

        GUILayout.Label("服务器IP:");
       _currentServerInfo.serverAddress =  GUILayout.TextField(_currentServerInfo.serverAddress);

        GUILayout.Label("登录地址:");
        _currentServerInfo.loginAddress = GUILayout.TextField(_currentServerInfo.loginAddress);

        GUILayout.Label("资源路径:");
        _currentServerInfo.resPath = GUILayout.TextField(_currentServerInfo.resPath, GUILayout.MinWidth(200));
        if (_isHint) 
        {
            GUI.color = Color.red;
            GUILayout.Label("大坑B别乱搞 进不去的 自己检查 别问我怎么办");
            GUI.color = Color.white;
        }

        GUILayout.Label("以上修改只适用于当次 不会保存在本地防止误操作者");
        if (GUILayout.Button("保存本次修改", GUILayout.MinWidth(200), GUILayout.MinHeight(40)))
        {
           // Debug.LogError(_currentServerInfo.isOpenGM.ToString());
            SaveData();
        }

        if (GUILayout.Button("开始游戏之旅", GUILayout.MinWidth(200), GUILayout.MinHeight(40)))
        {
            EnterGame();
        }
    }

    public void EnterGame() 
    {
        if (!Verification())
        {
            _beginHintTime = Time.time;
            _isHint = true;
            return;
        }
        ServerInfoManager.Instance._currentServerinfo = _currentServerInfo;
        ServerInfoManager.Instance.SetServerInfo();
        if (GlobalConst.OPENDLC)
        {
            Main.Instance.ChangeState(CheckUpdateState.StateName);
        }
        else
        {
            Main.Instance.ChangeState(ReadResState.StateName);
        }
        UnInitialize();
    }


    public bool Verification() 
    {
        bool ispass = false;
        if (string.IsNullOrEmpty(_currentServerInfo.serverAddress)) return ispass;
        if (string.IsNullOrEmpty(_currentServerInfo.loginAddress)) return ispass;
        if (string.IsNullOrEmpty(_currentServerInfo.resPath)) return ispass;
        if (!byte.TryParse(_serverType, out _currentServerInfo.serverType)) 
        {
            _serverType = _currentServerInfo.serverType.ToString();
        }

        if (!byte.TryParse(_isOpenGM, out _currentServerInfo.isOpenGM))
        {
            _isOpenGM = _currentServerInfo.isOpenGM.ToString();
        }

        if (!byte.TryParse(_isOpenGuide, out _currentServerInfo.isOpenGuide))
        {
            _isOpenGuide = _currentServerInfo.isOpenGuide.ToString();
        }
        return true;
    }

    public void SaveData() 
    {
        if (!Verification()) 
        {
            _beginHintTime = Time.time;
            _isHint = true;

        }
        if (_currentServerInfo.isOpenGM > 1) 
        {
            _currentServerInfo.isOpenGM = 0;
            _isOpenGM = "0";
        }

        if (_currentServerInfo.isOpenGuide > 1)
        {
            _currentServerInfo.isOpenGuide = 0;
            _isOpenGuide ="0";
        }
    }

    public void UpdateServerInfo() 
    {
        _serverType = _currentServerInfo.serverType.ToString();
        _isOpenGM = _currentServerInfo.isOpenGM.ToString();
        _isOpenGuide = _currentServerInfo.isOpenGuide.ToString();
    }

    public static void UnInitialize()
    {
        GameObject.Destroy(instance);
    }
}
