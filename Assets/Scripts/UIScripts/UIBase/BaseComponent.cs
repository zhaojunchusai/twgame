using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/* File: BaseComponent.cs
 * Desc: 组件基础类,所有以组件形式挂载在Gameobject上的  均可继承于此
 * Date：2015-5-5 19:35
 * Add by taiwei;
 * modify by taiwei
 */
public class BaseComponent  
{
    public GameObject mRootObject;

    public delegate void OnClickObj(BaseComponent com);
    public OnClickObj OnSelectObj;

    public virtual void MyStart(GameObject root) 
    {
        mRootObject = root;
    }
    /// <summary>
    /// auto set every view public property 
    /// </summary>
    //protected void AutoSetGoProperty<T>(T component,GameObject root)
    //{
    //    //if (!root.activeSelf)
    //    //{
    //    //    root.SetActive(true);
            
    //    //}
    //    //Type tempt = component.GetType();
    //    //foreach (FieldInfo fi in tempt.GetFields())
    //    //{
    //    //    if (!fi.Name.Contains("ui_")) continue;
    //    //    Component tempcom = PXTools.FindScriptInChild(root, fi.FieldType, fi.Name.Replace("ui_", ""));
    //    //    if (tempcom == null)
    //    //    {
    //    //        Debug.LogError(fi.Name + ". is not find in " + tempt.Name);
    //    //        continue;
    //    //    }
    //    //    fi.SetValue(component, tempcom);
    //    //}
    //}

    public void AddEventListener(OnClickObj handleEvent) 
    {
        UIEventListener.Get(mRootObject).onClick = OnClick;
        OnSelectObj = handleEvent;
    }

    public virtual void OnClick(GameObject go) 
    {
        if (OnSelectObj != null) 
        {
            OnSelectObj(this);
        }
    }


    public virtual void Clear() 
    {

    }
}
