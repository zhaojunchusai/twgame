using UnityEngine;
using Assets.Script.Common;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common.StateMachine;
using fogs.proto.msg;
using CodeStage.AntiCheat.Detectors;
using System;
public class Main : MonoSingleton<Main>
{
    public StateMachine StateMachine = new StateMachine();

    public static long mTime;

    public int sleepTimeout = 0;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SetSleepTimeout();
    }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        ConfigManager.Instance.Initialize();
        BuglyManager.Instance.Initialize();
        CommandManager.Instance.Initialize();
        SceneManager.Instance.Initialize();
        RoleManager.Instance.Initialize();
        PlayerData.Instance.Initialize();
        SDKManager.Instance.Initialize();
        //SDKManager.Instance.Initialize();
        UISystem.Instance.Initialize();
        HintManager.Instance.Initialize();
        SoundManager.Instance.Initialize();
        ServerInfoManager.Instance.Initialize();
#if UNITY_ANDROID
        CallAndroidFunManager.Instance.Initialize();
#endif
#if UNITY_ANDROID || UNITY_IPHONE
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            SFRecharge.Instance.Initialize();
#endif
        UpdateVersionManager.Instance.Initialize();
        TalkingDataManager.Instance.Initialize();
        TalkManager.Instance.Initialize();

        InitGameState();
        StateMachine.SetGlobalStateState(GlobalState.StateName);
        BuglyManager.Instance.SetConfigDefault(GlobalConst.PLATFORM.ToString(), GlobalConst.VersionName, "", 10000);
        if (GlobalConst.IS_TESTAPK)
        {
            ChangeState(VersionSelectionState.StateName);
        }
        else
        {
            ServerInfoManager.Instance.SetServerInfo();
            if (GlobalConst.OPENDLC)
            {
                ChangeState(CheckUpdateState.StateName);
            }
            else
            {
                ChangeState(ReadResState.StateName);
            }
        }

        ObscuredCheatingDetector.StartDetection(OnObscuredTypeCheatingDetected);
    }
    private void OnObscuredTypeCheatingDetected()
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.WRONGWAY_GAEM);
        Scheduler.Instance.AddTimer(0.5f, false, () => { Application.Quit(); });
    }

    void Update()
    {
        Scheduler.Instance.Update();
        UpdateTimeTool.Instance.Update();
        StateMachine.Update();
        if (Application.platform == RuntimePlatform.Android)
        {
            if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SDKManager.Instance.AndroidKeyCode_Back();
                }
            }
        }
    }

    void LateUpdate()
    {
        Scheduler.Instance.LateUpdate();
    }

    public void InitTime(long time)
    {
        mTime = time;
        Scheduler.Instance.AddTimer(1, true, UpdateTime);
    }

    public void UpdateTime()
    {
        ++mTime;
    }

    public void ChangeState(string newStateName)
    {
        StateMachine.ChangeState(newStateName);
    }

    private void InitGameState()
    {
        StateMachine.AddState<BaseGamePlayState>();
        StateMachine.AddState<GlobalState>();
        StateMachine.AddState<GameSplashState>();
        StateMachine.AddState<LoginState>();
        StateMachine.AddState<CreateRoleState>();
        StateMachine.AddState<CheckUpdateState>();
        StateMachine.AddState<MainCityState>();
        StateMachine.AddState<ReadResState>();
        StateMachine.AddState<FightState>();
        StateMachine.AddState<VersionSelectionState>();
    }

    private void UnInitGmaeState()
    {
        StateMachine.RemoveState<BaseGamePlayState>();
        StateMachine.RemoveState<GlobalState>();
        StateMachine.RemoveState<GameSplashState>();
        StateMachine.RemoveState<LoginState>();
        StateMachine.RemoveState<CheckUpdateState>();
        StateMachine.RemoveState<MainCityState>();
        StateMachine.RemoveState<CreateRoleState>();
        StateMachine.RemoveState<ReadResState>();
        StateMachine.RemoveState<FightState>();
        StateMachine.RemoveState<VersionSelectionState>();
    }

    public void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            TalkingDataManager.Instance.Uninitialize();
        }
        else
        {
            TalkingDataManager.Instance.Initialize();
            if (PlayerData.Instance._AccountID != 0)
            {
                TalkingDataManager.Instance.SetAccount(PlayerData.Instance._AccountID.ToString());
            }
        }
    }

    public void OnApplicationQuit()
    {
        RecoverySleepTimeout();
    }

    public void QuitGame()
    {
        // TalkingDataManager.Instance.OnKill();
        Application.Quit();
    }

    public void LoginOut()
    {
        List<string> uiname = new List<string>();
        uiname.Add(HintView.UIName);
        uiname.Add(LoginView.UIName);
        UISystem.Instance.DelAllUIButOne(uiname);
        SDKManager.Instance.NormalLoginout();
        HintManager.Instance.Uninitialize();
        NetWorkManager.Instance.ModuleUninitialize();
        HintManager.Instance.Initialize();
        NetWorkManager.Instance.ModuleInitialize();
        PlayerData.Instance.Clear();
        Main.Instance.StateMachine.ChangeState(LoginState.StateName);
        GuideManager.Instance.Uninitialize();
        ResourceLoadManager.Instance.ReleaseRequestBundle();
    }

    public void SetSleepTimeout()
    {
        sleepTimeout = Screen.sleepTimeout;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void RecoverySleepTimeout()
    {
        Screen.sleepTimeout = sleepTimeout;
    }

    private void Uninitialize()
    {
        //GameModule.Instance.Uninitialize();
        NetWorkManager.Instance.Uninitialize();
        SceneManager.Instance.Uninitialize();
        RoleManager.Instance.Uninitialize();
        CommandManager.Instance.Uninitialize();
        PlayerData.Instance.Uninitialize();
        SoundManager.Instance.Uninitialize();
        StoreModule.Instance.Uninitialize();
        HintManager.Instance.Uninitialize();
        TalkingDataManager.Instance.Uninitialize();
        TalkManager.Instance.Uninitialize();
        SDKManager.Instance.UnInitialize();
        UnInitGmaeState();
    }
    private int scaleWidth = 0;
    private int scaleHeight = 0;
    public void setDesignContentScale()
    {
#if UNITY_ANDROID
        if (scaleWidth == 0 && scaleHeight == 0)
        {
            Debug.LogError("test by xxy");
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;
            int designWidth = 960;
            int designHeight = 540;
            float s1 = (float)designWidth / (float)designHeight;
            float s2 = (float)width / (float)height;
            if (s1 < s2)
            {
                designWidth = (int)Mathf.FloorToInt(designHeight * s2);
            }
            else if (s1 > s2)
            {
                designHeight = (int)Mathf.FloorToInt(designWidth / s2);
            }
            float contentScale = (float)designWidth / (float)width;
            if (contentScale < 1.0f)
            {
                scaleWidth = designWidth;
                scaleHeight = designHeight;
            }
        }
        if (scaleWidth > 0 && scaleHeight > 0)
        {
            if (scaleWidth % 2 == 0)
            {
                scaleWidth += 1;
            }
            else
            {
                scaleWidth -= 1;
            }
            Screen.SetResolution(scaleWidth, scaleHeight, true);
        }
#endif
    }

}
