using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UICenterOnChild))]
public class CenterChildControl : MonoBehaviour
{
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
    }

    public void OnCenter(GameObject go)
    {
        _preCenterObj = _curCenterObj;
        _curCenterObj = go;
        _preScale = go.transform.localScale;
    }

    private void ChangePre()
    {

    }

    private void ChangeCur()
    {

    }
}
