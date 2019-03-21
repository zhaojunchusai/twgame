using UnityEngine;
using System.Collections;

public class ScaleMainCity : MonoBehaviour {
    public enum ScaleMainCityType
    {
        MainCity,
        Guide,
    }

    public ScaleMainCityType scaleMainCityType = ScaleMainCityType.MainCity;
    private float initSize;
    private float trueSize;

    void Start ()
	{
        initSize = (float)576/1024;
	    trueSize = (float)Screen.height/Screen.width;
        if (Mathf.Abs(trueSize - initSize) < 0.001)
        {
            return;
        }
        if(scaleMainCityType == ScaleMainCityType.MainCity)
        {
            float factor = Mathf.Max(trueSize, initSize)/Mathf.Min(trueSize, initSize);
            transform.localScale = factor * Vector3.one;
        }
	}

    public void SetGuideViewScale(bool normalOne)
    {
        if (Mathf.Abs(trueSize - initSize) < 0.001)
        {
            return;
        }
        if (scaleMainCityType == ScaleMainCityType.Guide && !normalOne)
        {
            float factor = Mathf.Max(trueSize, initSize) / Mathf.Min(trueSize, initSize);
            transform.localScale = factor * Vector3.one;
        }
        else if (scaleMainCityType == ScaleMainCityType.Guide && normalOne)
        {
            transform.localScale = Vector3.one;
        }
    }
}
