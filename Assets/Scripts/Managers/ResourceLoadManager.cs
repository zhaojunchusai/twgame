
using UnityEngine;
using System.IO;
using System.Collections;
using Assets.Script.Common;
using System.Collections.Generic;
using System.Text;

public enum ResourceLoadType
{
    Resource,
    AssetBundle,
}

public class ResourceType
{
    /// <summary>
    /// 角色资源
    /// </summary>
    public const string Character = "GAMEOBJECT_CHARACTER";
    /// <summary>
    ///  特效资源
    /// </summary>
    public const string Effect = "GAMEOBJECT_EFFECT";
    /// <summary>
    /// UI资源
    /// </summary>
    public const string UIWindow = "GAMEOBJECT_NGUIWINDOW";
    /// <summary>
    /// 声音资源
    /// </summary>
    public const string Audio = "GAMEOBJECT_AUDIO";
    /// <summary>
    /// 独立图片资源
    /// </summary>
    public const string AloneImage = "GAMEOBJECT_ALONE_IMAGE";
    /// <summary>
    /// 独立资源 
    /// </summary>
    public const string Alone = "GAMEOBJECT_ALONE";
    /// <summary>
    /// 独立图集资源
    /// </summary>
    public const string AloneAtlas = "GAMEOBJECT_ALONE_ATLAS";
    /// <summary>
    /// 配置表
    /// </summary>
    public const string Config = "XML_Config";
    public const string NGUIFont = "Font_NGUI";
    public const string Font = "Font_B";
}

public sealed class ResourceLoadManager : MonoSingleton<ResourceLoadManager>
{
    public delegate void DelegateLoadComplete(string path, object obj);
    public DelegateLoadComplete OnLoadComplete;
    private Dictionary<string, Object> cacheResList = new Dictionary<string, Object>();
    private Dictionary<string, Object> loadedResList = new Dictionary<string, Object>();
    private List<UIAtlas> _uiAtlas = new List<UIAtlas>();

    private Dictionary<string, RequestBundle> requestLoadList = new Dictionary<string, RequestBundle>();
    private Dictionary<string, RequestBundle> dependentRequestList = new Dictionary<string, RequestBundle>();

    private class RequestBundle
    {
        public string name;
        public bool isLoadStart = false;
        public bool isLoadComplet = false;
        public AssetBundle assetbundle;
        public string dependentName;
        public string resType;
    }

    public int LoadedCount = 0;
    public IEnumerator PreLoadAllViews()
    {
        LoadedCount = 0;
        //yield return StartCoroutine(PreLoadExternalViews());
        yield return StartCoroutine(PreLoadLocalViews());
    }

    public IEnumerator PreLoadLocalViews()
    {
        System.Text.StringBuilder sub_path = new StringBuilder();
        if (Application.platform == RuntimePlatform.Android)  //anroid平台StreamingAssets文件夹  无法进行IO操作
        {
            sub_path.Append(ResPath.ANDROIDPATH);
            sub_path.Append(ResPath.DIR_VIEW);
            List<AssetResourceData> assetDataList = PreLoadManager.Instance.GetPreAssetList();
            for (int i = 0; i < assetDataList.Count; i++)
            {
                AssetResourceData assetData = assetDataList[i];
                string androidPath = sub_path.ToString() + assetData.assetName;
                yield return LoadPrecacheResource(assetData.assetName, ResourceType.UIWindow, true);
            }
        }
        else
        {
            sub_path.Append(ResPath.WPRELOCALPATH);
            sub_path.Append(ResPath.DIR_VIEW);
            List<AssetResourceData> assetDataList = PreLoadManager.Instance.GetPreAssetList();
            for (int i = 0; i < assetDataList.Count; i++)
            {
                AssetResourceData assetData = assetDataList[i];
                string androidPath = sub_path.ToString() + assetData.assetName;
                yield return LoadPrecacheResource(assetData.assetName, ResourceType.UIWindow, true);
            }
            //string directoryPath = sub_path.ToString();
            //if (!Directory.Exists(Path.GetDirectoryName(directoryPath)))
            //    Directory.CreateDirectory(Path.GetDirectoryName(directoryPath));
            //DirectoryInfo directoryinfo = new DirectoryInfo(directoryPath);
            //FileInfo[] fileInfos = directoryinfo.GetFiles("*.assetbundle");
            //for (int i = 0; i < fileInfos.Length; i++)
            //{
            //    yield return null;
            //    LoadPrecacheResource(fileInfos[i].Name, ResourceType.UIWindow, true);
            //}
        }
    }

    public List<UIAtlas> GetLoadUIAtlas() 
    {
        return _uiAtlas;
    }

    //public IEnumerator PreLoadExternalViews()
    //{
    //    System.Text.StringBuilder sub_path = new StringBuilder();
    //    sub_path.Append(ResPath.WLOCALPATH);
    //    sub_path.Append(ResPath.DIR_VIEW);

    //    List<AssetResourceData> assetDataList = PreLoadManager.Instance.GetPreAssetList();
    //    for (int i = 0; i < assetDataList.Count; i++)
    //    {
    //        AssetResourceData assetData = assetDataList[i];
    //        string androidPath = sub_path.ToString() + assetData.assetName;
    //        yield return LoadPrecacheResource(assetData.assetName, ResourceType.UIWindow, true);
    //    }
    //    //string directoryPath = sub_path.ToString();
    //    //if (!Directory.Exists(Path.GetDirectoryName(directoryPath)))   //Application.persistentDataPath 路径下 各平台IO都是可以直接用的
    //    //    Directory.CreateDirectory(Path.GetDirectoryName(directoryPath));
    //    //DirectoryInfo directoryinfo = new DirectoryInfo(directoryPath);
    //    //FileInfo[] fileInfoArray = directoryinfo.GetFiles("*" + ResPath.FILESUFFIX);
    //    //for (int i = 0; i < fileInfoArray.Length; i++)
    //    //{
    //    //    yield return null;
    //    //    LoadPrecacheResource(fileInfoArray[i].Name, ResourceType.UIWindow, true);
    //    //}
    //}

    private Object LoadPrecacheResource(string requestName, string resType, bool isCache)
    {
        string fileName = ResPath.ReplaceFileName(requestName, false);
        try
        {
            if (cacheResList.ContainsKey(fileName))
                return cacheResList[fileName];
            if (loadedResList.ContainsKey(fileName))
            {
                return loadedResList[fileName];
            }
            if (requestLoadList.ContainsKey(fileName))
            {
                return null;
            }
            if (dependentRequestList.ContainsKey(fileName))
            {
                RequestBundle bundle = dependentRequestList[fileName];
                if (bundle != null && bundle.isLoadStart)
                {
                    return null;
                }
                return null;
            }
            RequestBundle requestBundle = new RequestBundle();
            requestBundle.name = fileName;
            requestBundle.isLoadStart = true;
            requestBundle.isLoadComplet = false;
            requestBundle.assetbundle = null;
            requestBundle.resType = resType;
            if (requestName.Contains(ResPath.PREFIX_ATLAS) || requestName.Contains(ResPath.PREFIX_FONT) || requestName.Contains(ResPath.PREFIX_NGUIFONT))
            {
                ///以上资源认为是依赖资源 在所有资源加载完后 在进行释放
                if (dependentRequestList.ContainsKey(fileName) == false)
                    dependentRequestList.Add(fileName, requestBundle);
            }
            else
            {
                if (requestLoadList.ContainsKey(fileName) == false)
                    requestLoadList.Add(fileName, requestBundle);
            }
            string assetPath = string.Empty;
            float startTime = Time.realtimeSinceStartup;
            ResourceLoadPathType pathType = ResPath.AssetPath(fileName, resType, out assetPath);
            byte[] fileBytes = null;
            switch (pathType)
            {
                case ResourceLoadPathType.PersistentDataPath:
                    {
                        fileBytes = File.ReadAllBytes(assetPath);
                    } break;
                case ResourceLoadPathType.StreamingAssets:
                    {
                        if (Application.platform == RuntimePlatform.Android)
                        {
#if UNITY_ANDROID
                            fileBytes = CallAndroidFunManager.Instance.GetFileBytes(assetPath);
                            if (fileBytes.Length == 1)
                            {
                                Debug.LogError("Get Android FileBytes Error（fileBytes.Length == 1） By FileName:" + fileName + "-----File Path:" + assetPath);
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
                        Object obj = Resources.Load(assetPath) as Object;
                        if (obj != null)
                        {
                            if (isCache)
                            {
                                if (!cacheResList.ContainsKey(fileName))
                                {
                                    cacheResList.Add(fileName, obj);
                                }
                            }
                            else
                            {
                                if (!loadedResList.ContainsKey(fileName))
                                {
                                    loadedResList.Add(fileName, obj);
                                }
                            }
                            if (requestLoadList.ContainsKey(fileName))
                            {
                                requestLoadList.Remove(fileName);
                            }
                            return obj;
                        }                       
                    }
                    break;
            }
            if (fileBytes != null)
            {
                AssetBundle assetBundle = AssetBundle.CreateFromMemoryImmediate(fileBytes);
                requestBundle.assetbundle = assetBundle;
                requestBundle.isLoadComplet = true;
                StringHolder holder = (StringHolder)assetBundle.Load("DependentBundleNames", typeof(StringHolder));
                if (holder != null && holder.content != null && holder.content.Length > 0)
                {
                    for (int i = 0; i < holder.content.Length; i++)
                    {
                        string dependContent = holder.content[i];
                        string strDependName = ResPath.ReplaceFileName(dependContent, false);
                        string dependAssetPath = string.Empty;
                        string dependAssetType = resType;
                        if (dependentRequestList.ContainsKey(strDependName))
                        {
                            dependentRequestList[strDependName].dependentName = fileName;
                        }
                        if (cacheResList.ContainsKey(strDependName) == false)
                        {
                            LoadPrecacheResource(dependContent, dependAssetType, true);
                        }
                    }
                }
                Object obj = assetBundle.mainAsset;
                if (obj != null)
                {
                    string tmpName = ResPath.ReplaceFileName(fileName, false);
                    if (isCache)
                    {
                        if (!cacheResList.ContainsKey(fileName))
                        {
                            cacheResList.Add(fileName, obj);
                        }
                    }
                    else
                    {
                        if (!loadedResList.ContainsKey(fileName))
                        {
                            loadedResList.Add(fileName, obj);
                        }
                    }
                    if (requestLoadList.ContainsKey(tmpName))
                    {
                        requestLoadList.Remove(tmpName);
                    }
                    if (dependentRequestList.ContainsKey(tmpName))
                    {
                        dependentRequestList.Remove(tmpName);
                    }
                    GameObject vGo = obj as GameObject;
                    if (vGo != null)
                    {
                        UIAtlas tempAtlas = vGo.GetComponent<UIAtlas>();
                        if (tempAtlas != null)
                        {
                            _uiAtlas.Add(tempAtlas);
                        }
                    }
                    float endTime = Time.realtimeSinceStartup;
                    if ((endTime - startTime) > 0.1f)
                    {
                        Debug.LogWarning("Load Asset Name:" + fileName + "--------useTime:" + (endTime - startTime) + "s------Size:" + Mathf.CeilToInt((fileBytes.Length / (float)1024)) + "k");
                    }
                    LoadedCount++;
                    if (requestName.Contains(ResPath.PREFIX_ATLAS) || requestName.Contains(ResPath.PREFIX_FONT) || requestName.Contains(ResPath.PREFIX_NGUIFONT))
                    {
                        ReleaseDependentBundle();
                    }
                    else
                    {
                        assetBundle.Unload(false);
                    }
                    if (obj != null)
                        return obj;
                }
            }
        }
        catch (System.Exception e)
        {
            if (dependentRequestList.ContainsKey(fileName))
            {
                dependentRequestList.Remove(fileName);
            }
            if (requestLoadList.ContainsKey(fileName))
            {
                requestLoadList.Remove(fileName);
            }
            Debug.LogError(e.Message);
        }

        Debug.LogError("can not get asset by name:" + requestName);
        return null;
    }


    private void ReleaseDependentBundle()
    {
        Dictionary<string, RequestBundle> dic = new Dictionary<string, RequestBundle>();
        foreach (KeyValuePair<string, RequestBundle> tmp in dependentRequestList)
        {
            RequestBundle bundle = tmp.Value;
            if (string.IsNullOrEmpty(bundle.dependentName))
                continue;
            if ((cacheResList.ContainsKey(bundle.dependentName) == false) && (loadedResList.ContainsKey(bundle.dependentName) == false))
            {
                dic.Add(tmp.Key, tmp.Value);
            }
            else
            {
                bundle.assetbundle.Unload(false);
            }
        }
        dependentRequestList = dic;
    }

    public void ReleaseRequestBundle()
    {
        loadedResList.Clear();
        Resources.UnloadUnusedAssets();
    }

    public void ReleaseBundleForName(List<string> nameList)
    {
        if (nameList != null) 
        {
            for (int i = 0; i < nameList.Count; i++)
            {
                if (loadedResList.ContainsKey(nameList[i]))
                {
                    loadedResList.Remove(nameList[i]);
                }
            }
        }
        Resources.UnloadUnusedAssets();
    }

    public bool isCache(string name)
    {
        return cacheResList.ContainsKey(name);
    }

    public Object LoadResources(string vPath, bool vIsCache = true)
    {
        Object obj = null;
        vIsCache = false;
        if (cacheResList.ContainsKey(vPath))
        {
            cacheResList.TryGetValue(vPath, out obj);
            if (obj == null)
            {
                Debug.LogError("can not get object by path:" + vPath);
                cacheResList.Remove(vPath);    //如果为NULL  则移除
            }
        }
        else
        {
            obj = Resources.Load(vPath) as Object;
            if (obj == null)
            {
                Debug.LogError("can not get object by path:" + vPath);
            }
            else
            {
                if (vIsCache)
                {
                    if (!cacheResList.ContainsKey(vPath))
                        cacheResList.Add(vPath, obj);
                }
            }
        }
        return obj;
    }

    public UIAtlas GetAtlasBySpriteName(string vName)
    {
        for (int i = 0; i < _uiAtlas.Count; i++)
        {
            if (_uiAtlas[i] == null) continue;
            int count = _uiAtlas[i].spriteList.Count;
            for (int j = 0; j < count; j++)
            {
                if (_uiAtlas[i].spriteList[j].name.Equals(vName))
                    return _uiAtlas[i];
            }
        }
        return null;
    }

    public string GetABNameByType(string fileName, string resType)
    {
        string tmpName = fileName;
        if (string.IsNullOrEmpty(fileName))
            return tmpName;
        System.Text.StringBuilder sub_asset = new StringBuilder();  //最终输出路径 assetbundle
        switch (resType)
        {
            case ResourceType.UIWindow:
                {
                    if (!fileName.Contains(ResPath.PREFIX_VIEW))
                    {
                        sub_asset.Append(ResPath.PREFIX_VIEW);
                        sub_asset.Append(fileName);
                        tmpName = sub_asset.ToString();
                    }
                    break;
                }
            case ResourceType.AloneImage:
                {
                    if (!fileName.Contains(ResPath.PREFIX_IMAGE))
                    {
                        sub_asset.Append(ResPath.PREFIX_IMAGE);
                        sub_asset.Append(fileName);
                        tmpName = sub_asset.ToString();
                    }
                    break;
                }
            case ResourceType.Character:
                {
                    if (!fileName.Contains(ResPath.PREFIX_ROLE))
                    {
                        sub_asset.Append(ResPath.PREFIX_ROLE);
                        sub_asset.Append(fileName);
                        tmpName = sub_asset.ToString();
                    }
                    break;
                }
            case ResourceType.Effect:
                {
                    if (!fileName.Contains(ResPath.PREFIX_EFFECT))
                    {
                        sub_asset.Append(ResPath.PREFIX_EFFECT);
                        sub_asset.Append(fileName);
                        tmpName = sub_asset.ToString();
                    }
                    break;
                }
            case ResourceType.Audio:
                {
                    if (!fileName.Contains(ResPath.PREFIX_SOUND))
                    {
                        sub_asset.Append(ResPath.PREFIX_SOUND);
                        sub_asset.Append(fileName);
                        tmpName = sub_asset.ToString();
                    }
                    break;
                }
            case ResourceType.Alone:
                {
                    if (!fileName.Contains(ResPath.PREFIX_ALONERES))
                    {
                        sub_asset.Append(ResPath.PREFIX_ALONERES);
                        sub_asset.Append(fileName);
                        tmpName = sub_asset.ToString();
                    }
                    break;
                }
        }
        tmpName = ResPath.ReplaceFileName(tmpName, false);
        return tmpName;
    }

    public void LoadSound(string vName, ResourceLoadType type, System.Action<GameObject> onLoad)
    {
        LoadAssetSound(vName, onLoad, null);
    }

    private void LoadAssetSound(string vFileName, System.Action<GameObject> onLoaded, System.Action<string> onLoadError)
    {
        vFileName = GetABNameByType(vFileName, ResourceType.Audio);
        if (cacheResList.ContainsKey(vFileName))
        {
            GameObject mGO = cacheResList[vFileName] as GameObject;
            if (mGO != null)
            {
                if (onLoaded != null)
                    onLoaded(mGO);
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError("get object success but gameobject is null!");
                }
            }
        }
        else if (loadedResList.ContainsKey(vFileName))
        {
            GameObject mGO = loadedResList[vFileName] as GameObject;
            if (mGO != null)
            {
                if (onLoaded != null)
                    onLoaded(mGO);
            }
            else
            {
                if (onLoadError != null)
                {
                    onLoadError("get object success but gameobject is null!");
                }
            }
        }
        else
        {
            Object sound = LoadPrecacheResource(vFileName, ResourceType.Audio, false);
            if (sound != null)
            {
                if (onLoaded != null)
                    onLoaded((GameObject)sound);
            }
            else
            {
                if (onLoadError != null)
                    onLoadError("can not get sound:" + vFileName);
            }
        }
    }

    public GameObject LoadView(string vName)
    {
        if (string.IsNullOrEmpty(vName))
            return null;
        string tmpFileName = GetABNameByType(vName, ResourceType.UIWindow);
        if (cacheResList.ContainsKey(ResPath.ReplaceFileName(tmpFileName, false)))
        {
            GameObject go = cacheResList[tmpFileName] as GameObject;
            if (go == null)
            {
                Debug.LogError("file name is true but get object is null:" + vName);
            }
            return go;
        }
        else
        {
            Object objAsset = LoadPrecacheResource(tmpFileName, ResourceType.UIWindow, false);
            if (objAsset != null)
                return (GameObject)objAsset;
        }
        return null;
    }

    public void LoadEffect(string vName, System.Action<GameObject> onLoad, ResourceLoadType type = ResourceLoadType.AssetBundle)
    {
        LoadAssetEffect(vName, onLoad, null);
    }

    private void LoadAssetEffect(string vFileName, System.Action<GameObject> onLoaded, System.Action<string> onLoadError)
    {
        vFileName = GetABNameByType(vFileName, ResourceType.Effect);
        if (cacheResList.ContainsKey(vFileName))
        {
            GameObject mGO = cacheResList[vFileName] as GameObject;
            if (mGO != null)
            {
                if (onLoaded != null)
                {
                    onLoaded(mGO);
                }
                return;
            }
            else
            {
                cacheResList.Remove(vFileName);
            }
        }
        if (loadedResList.ContainsKey(vFileName))
        {
            GameObject mGO = loadedResList[vFileName] as GameObject;
            if (mGO != null)
            {
                if (onLoaded != null)
                {
                    onLoaded(mGO);
                }
                return;
            }
            else
            {
                loadedResList.Remove(vFileName);
            }
        }
        Object effect = LoadPrecacheResource(vFileName, ResourceType.Effect, false);
        if (effect != null)
        {
            GameObject go = effect as GameObject;
            if (RuntimePlatform.IPhonePlayer == Application.platform || RuntimePlatform.WindowsEditor == Application.platform || RuntimePlatform.OSXEditor == Application.platform)  //重新指定一下Shader 编辑器下会变成粉色 add by taiwei 
            {
                SkeletonAnimation _animation = go.GetComponent<SkeletonAnimation>();
                if (_animation == null || _animation.skeletonDataAsset == null || _animation.skeletonDataAsset.atlasAssets == null)
                {
                    Debug.LogWarning("(Editor mode)Ignore Log: Can not get SkeletonAnimation on " + go.name);
                }
                else
                {
                    for (int i = 0; i < _animation.skeletonDataAsset.atlasAssets.Length; i++)
                    {
                        AtlasAsset _asset = _animation.skeletonDataAsset.atlasAssets[i];
                        for (int j = 0; j < _asset.materials.Length; j++)
                        {
                            string name = _asset.materials[j].shader.name;
                            Shader shader = Shader.Find(name);
                            _asset.materials[j].shader = shader;
                        }
                    }
                }
            }
            if (onLoaded != null)
                onLoaded(go);
            else
            {
                onLoadError("can not get sound:" + vFileName);
            }
        }
    }


    /// <summary>
    /// 加载独立的Texture
    /// </summary>
    /// <param name="vName"></param>
    /// <param name="onload"></param>
    /// <param name="type"></param>
    public void LoadAloneImage(string vName, System.Action<Texture> onload, bool isCache = false, ResourceLoadType type = ResourceLoadType.AssetBundle)
    {
        LoadAssetAloneImage(vName, onload, (error) => { Debug.LogError(error); }, isCache);
    }

    private void LoadAssetAloneImage(string vFileName, System.Action<Texture> onLoaded, System.Action<string> onLoadError, bool isCache)
    {
        vFileName = GetABNameByType(vFileName, ResourceType.AloneImage);
        if (cacheResList.ContainsKey(vFileName))
        {
            GameObject mGO = cacheResList[vFileName] as GameObject;

            if (mGO == null)
            {
                Debug.LogError("load success but object is null" + vFileName);
                return;
            }
            AloneImageHolder holder = mGO.GetComponent<AloneImageHolder>();
            if (holder == null)
            {
                if (onLoadError != null)
                    onLoadError("load Success temptex is null  desc:" + vFileName);
            }
            if (holder.SaveToBytes)
            {
                //如果此图片包承载的是原始二进制图片数据，则手动创建纹理并载入图片数据
                Texture2D tex = new Texture2D(holder.Desc_ImageWidth, holder.Desc_ImageHeight);
                tex.name = holder.Desc_ImageName;
                tex.LoadImage(holder.ImageData);
                holder.Texture = tex;
            }
            Texture temptex = holder.Texture;
            if (temptex != null)
            {
                if (onLoaded != null) onLoaded(temptex);
            }
            else
            {
                if (onLoadError != null)
                    onLoadError("load Success temptex is null  desc:" + vFileName);
            }
        }
        else
        {
            GameObject mGO = LoadPrecacheResource(vFileName, ResourceType.AloneImage, isCache) as GameObject;
            if (mGO == null)
            {
                Debug.LogError("load success but object is null" + vFileName);
                return;
            }
            AloneImageHolder holder = mGO.GetComponent<AloneImageHolder>();
            if (holder == null)
            {
                if (onLoadError != null)
                    onLoadError("load Success temptex is null  desc:" + vFileName);
            }
            if (holder.SaveToBytes)
            {
                //如果此图片包承载的是原始二进制图片数据，则手动创建纹理并载入图片数据
                Texture2D tex = new Texture2D(holder.Desc_ImageWidth, holder.Desc_ImageHeight);
                tex.name = holder.Desc_ImageName;
                tex.LoadImage(holder.ImageData);
                holder.Texture = tex;
            }

            Texture temptex = holder.Texture;
            if (temptex != null)
            {
                if (onLoaded != null) onLoaded(temptex);
            }
            else
            {
                if (onLoadError != null)
                    onLoadError("load Success temptex is null  desc:" + vFileName);
            }
        }
    }

    /// <summary>
    /// 加载角色资源
    /// </summary>
    /// <param name="vName"></param>
    /// <param name="loadtype"></param>
    /// <param name="onLoad"></param>
    public void LoadCharacter(string vName, ResourceLoadType loadtype, System.Action<GameObject> onLoad)
    {
        LoadAssetCharacter(vName, onLoad, (error) => { });
    }
    private void LoadAssetCharacter(string vFileName, System.Action<GameObject> onLoaded, System.Action<string> onLoadError)
    {
        vFileName = GetABNameByType(vFileName, ResourceType.Character);
        if (cacheResList.ContainsKey(vFileName))
        {
            GameObject mGO = cacheResList[vFileName] as GameObject;
            if (mGO != null)
            {
                if (onLoaded != null) onLoaded(mGO);
            }
            else
            {
                if (onLoadError != null)
                    onLoadError("load error role is null  desc:" + vFileName);
            }
        }
        else if (loadedResList.ContainsKey(vFileName))
        {
            GameObject mGO = loadedResList[vFileName] as GameObject;
            if (mGO != null)
            {
                if (onLoaded != null) onLoaded(mGO);
            }
            else
            {
                if (onLoadError != null)
                    onLoadError("load error role is null  desc:" + vFileName);
            }
        }
        else
        {
            Object assetbundle = LoadPrecacheResource(vFileName, ResourceType.Character, false);
            if (assetbundle != null)
            {
                GameObject go = assetbundle as GameObject;
                if (go != null && onLoaded != null) //因为当角色资源加载时间慢 ，可能场景已经Destroy了
                {
                    if (RuntimePlatform.IPhonePlayer == Application.platform || RuntimePlatform.WindowsEditor == Application.platform || RuntimePlatform.OSXEditor == Application.platform)    //重新指定一下Shader  编辑器下会变成粉色add by taiwei
                    {
                        SkeletonAnimation _animation = go.GetComponent<SkeletonAnimation>();
                        if (_animation == null || _animation.skeletonDataAsset == null)
                        {
                            Debug.LogWarning("(Editor mode)Ignore Log: Can not get SkeletonAnimation on " + go.name);
                        }
                        else
                        {
                            for (int i = 0; i < _animation.skeletonDataAsset.atlasAssets.Length; i++)
                            {
                                AtlasAsset _asset = _animation.skeletonDataAsset.atlasAssets[i];
                                for (int j = 0; j < _asset.materials.Length; j++)
                                {
                                    string name = _asset.materials[j].shader.name;
                                    Shader shader = Shader.Find(name);
                                    _asset.materials[j].shader = shader;
                                }
                            }
                        }
                    }
                    if (onLoaded != null)
                        onLoaded(go);
                }
            }
            else
            {
                if (onLoadError != null)
                    onLoadError("load error role is null  desc:" + vFileName);
            }
        }
    }

    /// <summary>
    /// 加载独立资源 返回Object对象  因为不清楚独立资源类型
    /// </summary>
    /// <param name="vFileName"></param>
    /// <param name="onLoaded"></param>
    /// <param name="onLoadError"></param>
    public void LoadAssetAlone(string vFileName, System.Action<Object> onLoaded, System.Action<string> onLoadError)
    {
        vFileName = GetABNameByType(vFileName, ResourceType.Alone);
        Object mGO = LoadPrecacheResource(vFileName, ResourceType.Alone, false);
        if (mGO != null)
        {
            if (onLoaded != null) onLoaded(mGO);
        }
        else
        {
            if (onLoadError != null)
                onLoadError("load error alone res is null  desc:" + vFileName);
        }
    }
}
