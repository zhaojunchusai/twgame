using UnityEngine;
using System.Collections;

public class ChooseCharCenterControl : MonoBehaviour {

    private UICenterOnChild _uiCenterOnChild;
    private GameObject _preCenterObj;
    private GameObject _curCenterObj;
    private Vector3 _preScale;
    private Color _preColor;

    public Vector3 ToScale;
    public Color ToColor;

    void Awake()
    {
        _uiCenterOnChild = GetComponent<UICenterOnChild>();
        _uiCenterOnChild.onCenter = OnCenter;
    }

    public void OnCenter(GameObject go)
    {
        _preCenterObj = _curCenterObj;
        _curCenterObj = go;
        ChangePre();
        _preScale = go.transform.localScale;
        ChangeCur();
    }

    private void ChangePre()
    {
        if (_preCenterObj)
            _preCenterObj.transform.localScale = _preScale;
    }

    private void ChangeCur()
    {
        TweenScale tweenScale = _curCenterObj.GetComponent<TweenScale>();
        if (!tweenScale)
            tweenScale = _curCenterObj.AddComponent<TweenScale>();

        tweenScale.from = _curCenterObj.transform.localScale;
        tweenScale.to = ToScale;
        tweenScale.duration = 0.2f;
        tweenScale.PlayForward();
        //_curCenterObj.transform.localScale = ToScale;
    }
}
