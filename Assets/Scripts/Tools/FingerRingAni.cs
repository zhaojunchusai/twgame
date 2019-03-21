using UnityEngine;
using System.Collections.Generic;

public class FingerRingAni : MonoBehaviour
{
    public List<string> SpriteNameList;
    public List<float> SpriteTimerList;
    public float RepeatRate = 0.05f;
    public TweenPosition FingerAni;
    public UISprite FingerRing;
    private float _factor = 0;
    private bool _start = false;
    private int spIndex = 0;
    void Start()
    {
        return;
        if (SpriteNameList == null || SpriteNameList.Count <= 0)
        {
            _start = false;
            return;
        }

        SetSprite(SpriteNameList[0]);
        SetDelayStart();
        InvokeRepeating("MyUpdate", RepeatRate, RepeatRate);
    }

    void OnEnable()
    {
        if (!GetFingerAni())
            return;
        FingerAni.enabled = true;
    }
    void OnDisable()
    {
        if (!GetFingerAni())
            return;
        FingerAni.enabled = false;
    }

    bool GetFingerAni()
    {
        if (FingerAni == null  
            && transform.FindChild("Finger") != null 
            && transform.FindChild("Finger").gameObject != null)
            FingerAni = transform.FindChild("Finger").gameObject.GetComponent<TweenPosition>();
        return FingerAni != null;
    }

    void MyUpdate()
    {
        if (!_start)
            return;
        _factor += RepeatRate;
        if (_factor >= SpriteTimerList[spIndex])
        {
            _factor = 0;
            ++spIndex;
            spIndex = spIndex % SpriteNameList.Count;
            SetSprite(SpriteNameList[spIndex]);
        }
    }

    void SetSprite(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            FingerRing.enabled = false;
            return;
        }
        else
        {
            FingerRing.enabled = true;
        }
        CommonFunction.SetSpriteName(FingerRing, name);
    }

    void SetDelayStart()
    {
        _start = SpriteNameList.Count > 1;
    }
}

