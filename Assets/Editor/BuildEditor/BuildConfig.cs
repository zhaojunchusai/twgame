
/// <summary>
/// 打包用配置信息
/// </summary>
public class BuildConfig
{
    public string configName = "当前配置关键字(Key)";

    public string resAddress = "版本检查资源服务器地址";

    public string loginAddress = "登陆服务器地址";

    /// <summary>
    /// 请求的服务器列表类型
    /// </summary>
    public int loginServerType = 0;

    public bool isOpenCodeUpdate = true;
    public bool isOpenMD5 = true;
    public bool isOpenDLC = true;
    public bool isOpenGuide = true;
    public bool isOpenGM = false;
    public bool isOpenSDK = false;

    /// <summary>
    /// 版本显示名称(Bugly用)
    /// </summary>
    public string versionName = "";
    public string resAndroidPath = "TDTest/TD/Assetbundle/Android/";
    public string resIOSPath = "TDTest/TD/Assetbundle/Iphone/";
    public string resWinEditorPath = "TDTest/TD/Assetbundle/Editor/";
    public string resMacEditorPath = "TDTest/TD/Assetbundle/IOSEditor/";
    public bool isTDTest = false;
    public bool isReplaceRes = false;

    public string resPath = "";

    public int apkVersion = 0;

    /// <summary>
    /// 渠道类型
    /// </summary>
    public int channelType = 0;

    // icon路径
    public string iconPath = "Textures/GameIcon/";
    // 显示在电脑里的文件名
    public string fileName = "文件名称";
    // 显示在手机上的应用名
    public string appName = "应用名称";
    // 公司名
    public string companyName = "公司名称";
    // 处理好的SDK文件夹路径
    public string androidSDKPath = "点击按钮选择对应本渠道的SDK目录";
    // 包名
    public string bundleIdentifier = "请填写包名";
    // 包版本名
    public string bundleVersion = "请填写版本名";
    // 版本号
    public int bundleVersionCode = 1;
    // keystore路径
    public string keystorePath = "";
    // keystore密码
    public string keystorePassword = "";
    // keystore别名
    public string keyName = "";
    // keystore别名密码
    public string keyPassword = "";
    public BuildConfig()
    {
    }

    public BuildConfig(string name)
    {
        this.configName = name;
    }
}



