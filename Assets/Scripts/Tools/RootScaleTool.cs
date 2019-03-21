using UnityEngine;
using System.Collections;

public class RootScaleTool : MonoBehaviour 
{
    public Vector2 InitSize = Vector2.one;
    public bool runOnlyOnce = false;
    private UIRoot root;
    private float initScale = 1.0F;
    private float InitHeight =640.0F;
    private float _currentScale =1.0F;//当前的缩放//
    private Vector2 _scale = Vector2.one;

	void Awake () 
    {
        root = GetComponent<UIRoot>();
        if (null == root) enabled = false;
        InitHeight = root.manualHeight;//UIRoot组件上面手动输入的值//
        initScale = InitSize.x / InitSize.y;
	}
	
    void OnEnable()
    {
        if (!runOnlyOnce)
            return;

        _currentScale = ((float)Screen.width / (float)Screen.height) / initScale;//当前屏幕的长宽比例 与 需要的长宽比的比值//
        _scale.x = (float)Screen.width / InitSize.x;//获得当前屏幕的分辨率与输入的分辨率的比值//
        _scale.y = (float)Screen.height / InitSize.y;
        if (_scale.x > _scale.y)
            root.manualHeight = (int)(InitHeight * _currentScale);
        else
            root.manualHeight = (int)(InitHeight / _currentScale);
        //if (runOnlyOnce && Application.isPlaying) enabled = false;
    }

    void Update ()
    {
        if (runOnlyOnce)
            return;

        _currentScale = ((float)Screen.width / (float)Screen.height) / initScale;//当前屏幕的长宽比例 与 需要的长宽比的比值//
        _scale.x = (float)Screen.width / InitSize.x;//获得当前屏幕的分辨率与输入的分辨率的比值//
        _scale.y = (float)Screen.height / InitSize.y;
        if (_scale.x > _scale.y)
            root.manualHeight = (int)(InitHeight * _currentScale);
        else
            root.manualHeight = (int)(InitHeight / _currentScale);
        //if (runOnlyOnce && Application.isPlaying) enabled = false;
	}
}
