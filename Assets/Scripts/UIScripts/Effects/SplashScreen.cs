using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public enum SplashType
    {
        LoadNextLevelThenFadeOut,
        FadeOutThenLoadNextLevel
    }

    public string NextLevelName = ""; // logo显示完成后要加载的Scene
	public Texture2D[] TextureList; // 要显示的logo
    public float fadeSpeed = 0.3f; // 渐变速度（完成度/秒，1表示1秒完成渐变，0.5表示需要2秒完成渐变）
    public float waitTime = 0.5f; // 淡出前等待时间（秒）
    public bool waitForInput = false; // 为true表示需要等待输入才结束Logo画面
	public Color sceneBackgroundColor = new Color(1,1,1);
    public SplashType splashType;
    private float timeFadingInFinished = 0.0f;
	private int currentIndex;
    private float alpha = 0.0f;

    private enum FadeStatus
    {
        FadeIn,
        FadeWaiting,
        FadeOut
    }
    private FadeStatus status = FadeStatus.FadeIn;

    private Camera oldCam;
    private GameObject oldCamGO;

    private Rect splashLogoPos = new Rect();
    public enum LogoPositioning
    {
        Centered,
        Stretched
    }
    public LogoPositioning logoPositioning;

    private bool loadingNextLevel = false;

    void Start()
    {
        if (TextureList == null || TextureList.Length == 0)
        {
		   
        }
		
        oldCam = Camera.main;
        oldCamGO = Camera.main.gameObject;
        if (splashType == SplashType.LoadNextLevelThenFadeOut)
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(Camera.main);
        }
        if ((Application.levelCount <= 1) || (NextLevelName == ""))
        {
            Debug.Log("I need to have a level to load or the value of level To load is wrong!");
            return;
        }
    }

    void Update()
    {
        switch(status)
        {
            case FadeStatus.FadeIn:
                alpha += fadeSpeed * Time.deltaTime;
            break;
            case FadeStatus.FadeWaiting:
                if (Time.time >= timeFadingInFinished + waitTime)
                {
					if( !waitForInput || (waitForInput && currentIndex == TextureList.Length - 1 && Input.anyKey) ){
						status = FadeStatus.FadeOut;
					}
                }
            break;
            case FadeStatus.FadeOut:
                alpha += -fadeSpeed * Time.deltaTime;
            break;
        }
    }

    void OnGUI()
    {

        if (logoPositioning == LogoPositioning.Centered)
        {
            splashLogoPos.x = (Screen.width * 0.5f) - (TextureList[currentIndex].width * 0.5f);
            splashLogoPos.y = (Screen.height * 0.5f) - (TextureList[currentIndex].height * 0.5f);
       
            splashLogoPos.width = TextureList[currentIndex].width;
            splashLogoPos.height = TextureList[currentIndex].height;
        }
        else
        {
            splashLogoPos.x = 0;
            splashLogoPos.y = 0;
           
            splashLogoPos.width = Screen.width;
            splashLogoPos.height = Screen.height;
        }
		
        if (TextureList[currentIndex] != null)
        {
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Mathf.Clamp01(alpha));
            GUI.DrawTexture(splashLogoPos, TextureList[currentIndex]);
            if (alpha > 1.0f)
            {
                status = FadeStatus.FadeWaiting;
                timeFadingInFinished = Time.time;
                alpha = 1.0f;
                if (splashType == SplashType.LoadNextLevelThenFadeOut && currentIndex == TextureList.Length - 1)
                {
                    oldCam.depth = -1000;
                    loadingNextLevel = true;
                    Application.LoadLevel(NextLevelName);
                }
            }
            if (alpha < 0.0f)
            {
				if(currentIndex == TextureList.Length - 1){
					if (splashType == SplashType.FadeOutThenLoadNextLevel)
					{
                        Application.LoadLevel(NextLevelName);
					}
					else
					{
						Destroy(oldCamGO); // somehow this doesn't work
						Destroy(this);
					}
				} else {
					currentIndex++;
					status = FadeStatus.FadeIn;
					alpha = 0.0f;
				}
            }
        }
    }
}