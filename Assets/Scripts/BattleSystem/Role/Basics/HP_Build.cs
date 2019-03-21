using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

public class HP_Build
{
    /// <summary>
    /// 血条物件名字
    /// </summary>
    private const string HP_OBJECT_NAME = "aloneres_HP_Build.assetbundle";
    /// <summary>
    /// 进度条
    /// </summary>
    public UISlider Sli_HP_Build;
    /// <summary>
    /// 血量显示
    /// </summary>
    public UILabel Lbl_HPValue;
    /// <summary>
    /// 最大血量
    /// </summary>
    private int maxHPValue;
    /// <summary>
    /// 当前血量
    /// </summary>
    private int curHPValue;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vParent">父物件[数据]</param>
    /// <param name="vPosition">位置</param>
    /// <param name="vIsShowValue">是否显示血量</param>
    public HP_Build(RoleAttribute vParent, Vector3 vPosition, bool vIsShowValue = true)
    {
        if (vParent == null)
            return;

        ResourceLoadManager.Instance.LoadAssetAlone(HP_OBJECT_NAME, 
            (obj) => {
                if ((obj != null) && (vParent != null))
                {
                    GameObject tmpObj = GameObject.Instantiate(obj) as GameObject;
                    tmpObj.transform.parent = vParent.transform;
                    tmpObj.transform.localPosition = vPosition;
                    tmpObj.transform.localScale = Vector3.one;
                    Sli_HP_Build = tmpObj.transform.GetComponent<UISlider>();
                    Lbl_HPValue = tmpObj.transform.FindChild("HPValue").GetComponent<UILabel>();
                    Lbl_HPValue.gameObject.SetActive(vIsShowValue);
                    maxHPValue = vParent.Get_MaxHPValue;
                    if (maxHPValue <= 0)
                        maxHPValue = 1;
                    RefreshHPValue(maxHPValue);
                }
            }, 
            (error) => {
                Debug.LogWarning(string.Format("HP_Build: [{0}, {1}]", error, vParent.name));
            });
    }

    /// <summary>
    /// 刷新血量显示
    /// </summary>
    /// <param name="vCurValue">当前血量</param>
    public void RefreshHPValue(int vCurValue)
    {
        curHPValue = vCurValue;
        if (curHPValue > maxHPValue)
            curHPValue = maxHPValue;
        if (curHPValue < 0)
            curHPValue = 0;

        Sli_HP_Build.value = (float)curHPValue / maxHPValue;
        Lbl_HPValue.text = string.Format("{0}/{1}", curHPValue, maxHPValue);
    }
}
