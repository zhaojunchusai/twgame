using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class BaseConfig
{

    public virtual void Initialize(string vFileName, System.Action<string> callback)   //modify by taiwei
    {
        ConfigManager.Instance._CounterTool.AddCount();

        byte[] fileBytes = null;
        string assetPath = string.Empty;
        ResourceLoadPathType pathType = ResPath.AssetPath(vFileName, ResourceType.Config, out assetPath);
        switch (pathType)
        {
            case ResourceLoadPathType.PersistentDataPath:
                fileBytes = File.ReadAllBytes(assetPath);
                break;
            case ResourceLoadPathType.StreamingAssets:
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
#if UNITY_ANDROID
                        fileBytes = CallAndroidFunManager.Instance.GetFileBytes(assetPath);
                        if (fileBytes.Length == 1)
                        {
                            Debug.LogError("Get Android FileBytes Error（fileBytes.Length == 1） By FileName:" + vFileName + "-----File Path:" + assetPath);
                            fileBytes = null;
                        }
#endif
                    }
                    else
                    {
                        fileBytes = File.ReadAllBytes(assetPath);
                    }
                }
                break;
            case ResourceLoadPathType.Resource:
                {
                    TextAsset textAsset = Resources.Load(assetPath) as TextAsset;
                    if (textAsset == null)
                    {
                        Debug.LogError("can not find file ：" + assetPath);
                        LoadCompleted();
                        return;
                    }
                    if (callback != null)
                    {
                        callback(textAsset.text);
                    }
                    return;
                }
        }
        if (fileBytes == null)
        {
            Debug.LogError("Get FileBytes Error By FileName:" + vFileName + "-----File Path:" + assetPath);
            LoadCompleted();
            return;
        }
        AssetBundle assetBundle = AssetBundle.CreateFromMemoryImmediate(fileBytes);  //同步加载  UNITY4.5.1版本后支持 modify by taiwei
        if (assetBundle != null)
        {
            string name = ResPath.ReplaceFileName(vFileName, false, true);
            TextAsset textAsset = assetBundle.Load(name, typeof(TextAsset)) as TextAsset;
            if (textAsset != null && callback != null)
            {
                string configData = textAsset.text;
                callback(configData);
                assetBundle.Unload(true);
            }
            else
            {
                Debug.LogError("can not find file ：" + assetPath);
                LoadCompleted();
                return;
            }
        }
    }


    protected void LoadCompleted()
    {
        ConfigManager.Instance._CounterTool.DelCount();
    }
}