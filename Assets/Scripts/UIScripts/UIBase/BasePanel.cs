using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/* File: BaseComponent.cs
 * Desc: Panel基础类,所有以Panel形式挂存在的  均可继承于此
 * Date：2015-5-5 20:33
 * Add by taiwei;
 */
public class BasePanel : MonoBehaviour
{
    public virtual void MyStart() 
    {

    }

    /// <summary>
    /// auto set every view public property 
    /// </summary>
    protected void AutoSetGoProperty<T>(T component, GameObject root)
    {
        if (!root.activeSelf)
        {
            root.SetActive(true);
        }
        Type tempt = component.GetType();
        foreach (FieldInfo fi in tempt.GetFields())
        {
            if (!fi.Name.Contains("ui_")) continue;
            Component tempcom = PXTools.FindScriptInChild(root, fi.FieldType, fi.Name.Replace("ui_", ""));
            if (tempcom == null)
            {
                Debug.LogError(fi.Name + ". is not find in " + tempt.Name);
                continue;
            }
            fi.SetValue(component, tempcom);
        }
    }

    public virtual void Clear() 
    {

    }

}
