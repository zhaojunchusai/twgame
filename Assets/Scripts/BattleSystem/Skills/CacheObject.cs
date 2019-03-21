using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
public class CacheObject
{
    public List<GameObject> GameObjectList;
    public string StrName;

    private GameObject BaseObj;
    private GameObject WareHouse;

    public GameObject MyBaseObj
    {
        get
        {
            return this.BaseObj;
        }
    }
    public CacheObject(GameObject go, string _strName, int count)
    {
        this.BaseObj = go;
        this.BaseObj.SetActive(false);
        this.StrName = _strName;
        WareHouse = GameObject.Find("CacheWareHouse");
        GameObjectList = new List<GameObject>(count);
        for (int i = 0; i < count; i++)
        {
            GameObject vgo = Object.Instantiate(go) as GameObject;
            vgo.SetActive(false);
            GameObjectList.Add(vgo);
            if (WareHouse != null)
                vgo.transform.parent = WareHouse.transform;
            else
                vgo.transform.parent = null;
        }
    }
    ~ CacheObject()
    {
        this.GameObjectList.Clear();
        this.GameObjectList = null;
    }
    public GameObject nextFree
    {
        get
        {
            var freeObject = this.GameObjectList.Find((go) =>
            {
                if (go == null)
                {
                    //this.RemoveNull();
                    return false;
                }
                return !go.activeInHierarchy;
            });
            if (freeObject == null)
            {
                freeObject = Object.Instantiate(BaseObj) as GameObject;
                GameObjectList.Add(freeObject);
            }
            freeObject.SetActive(true);
            return freeObject;
        }
    }

    public void FreeObject(GameObject vGo) 
    {
        vGo.SetActive(false);
    }
    public void Delete()
    {
        if (this.GameObjectList == null)
            return;

        for (int i = 0; i < this.GameObjectList.Count; ++i)
        {
            GameObject delete = this.GameObjectList[i];
            if (delete != null)
            {
                delete.transform.parent = null;
                GameObject.Destroy(delete);
            }
        }
        if (this.BaseObj != null)
            this.BaseObj = null;
    }
    private void RemoveNull()
    {
        if (this.GameObjectList == null)
            return;

        this.GameObjectList.RemoveAll((go) => 
        {
            return !go;
        });
    }
    public bool BaseIsNull()
    {
        if (this.BaseObj == null)
            Debug.LogError(this.StrName);
        return this.BaseObj == null;
    }
}
public class GameObjectCache
{
    public List<CacheObject> CacheList;
    private GameObject WareHouse;
    public GameObjectCache()
    {
        this.CacheList = new List<CacheObject>();
        WareHouse = GameObject.Find("CacheWareHouse");
    }
     ~GameObjectCache()
    {
        this.CacheList = null;
    }
    public virtual void LoadGameObject(object strFilePath, System.Action<GameObject> onLoad,string fileName = "",int count = 1, ResourceLoadType type = ResourceLoadType.AssetBundle)
    {

    }
    public void FreeObject(GameObject vGo)
    {
        if (vGo == null)
            return;

        vGo.SetActive(false);
        if (WareHouse != null)
            vGo.transform.parent = WareHouse.transform;
        else
            vGo.transform.parent = null;
    }
    public CacheObject FindByName(string name)
    {
        if (name == "" && this.CacheList == null)
            return null;
        
        return this.CacheList.Find((ccObject) => 
        {
            if (ccObject == null)
                return false;
            return ccObject.StrName.Equals(name);
        });
    }
    public CacheObject FindByGameObject(GameObject vGo)
    {
        if (vGo == null && this.CacheList == null)
            return null;
        return this.CacheList.Find((ccObject) =>
        {
            if (ccObject == null)
                return false;
            return ccObject.GameObjectList.Contains(vGo) || ccObject.MyBaseObj.Equals(vGo);
        });
    }
    public virtual void Delete(bool isByName = false)
    {
        if (this.CacheList == null)
            return;
        List<string> ResourceNameList = new List<string>();
        for (int i = 0; i < this.CacheList.Count;++i )
        {
            CacheObject temp = this.CacheList[i];
            if(temp != null)
            {
                string FileName = ResPath.ReplaceFileName(temp.StrName, false);
                if (!string.IsNullOrEmpty(FileName))
                {
                    ResourceNameList.Add(FileName);
                }
                temp.Delete();
            }
            temp = null;
        }
        this.CacheList.Clear();
        if (isByName)
            ResourceLoadManager.Instance.ReleaseBundleForName(ResourceNameList);
        else
            ResourceLoadManager.Instance.ReleaseRequestBundle();
    }
    public virtual void InstanceGameObject(GameObject go)
    {
        return;
    }
}
public class EffectObjectCache : GameObjectCache
{
    private static EffectObjectCache pInstance;
    public static EffectObjectCache Instance
    {
        get
        {
            if (pInstance == null)
                pInstance = new EffectObjectCache();
            return pInstance;
        }
    }
    public override void LoadGameObject(object strFilePath, System.Action<GameObject> onLoad, string fileName = "", int count = 1, ResourceLoadType type = ResourceLoadType.AssetBundle)
    {
        if (((string)strFilePath).Equals("0") || string.IsNullOrEmpty((string)strFilePath))
        {
            return;
        }

        CacheObject tempCache = this.FindByName((string)strFilePath);
        if (tempCache == null || tempCache.BaseIsNull())
        {
            ResourceLoadManager.Instance.LoadEffect((string)strFilePath, (go) => 
            {
                if (go == null)
                    return;
                CacheObject myCache = new CacheObject(go, (string)strFilePath, count);
                GameObject freeObject = myCache.nextFree;
                if (freeObject != null)
                    this.InstanceGameObject(freeObject);
                this.CacheList.Add(myCache);
                onLoad(freeObject);
            });
        }
        else
        {
            GameObject freeObject = tempCache.nextFree;
            if (freeObject != null)
                this.InstanceGameObject(freeObject);

            onLoad(freeObject);
        }
    }
    public override void Delete(bool isByName = false)
    {
        base.Delete(isByName);
        EffectObjectCache.pInstance = null;
    }
    public override void InstanceGameObject(GameObject go)
    {
        base.InstanceGameObject(go);

        //TdSpine.SpineBase spine = go.GetComponent<TdSpine.SpineBase>();
        //if (spine != null)
        //    spine.ResetAlph(1);
    }
}
public class AloneObjectCache : GameObjectCache
{
    private static AloneObjectCache pInstance;
    public static AloneObjectCache Instance
    {
        get
        {
            if (pInstance == null)
                pInstance = new AloneObjectCache();
            return pInstance;
        }
    }
    public override void LoadGameObject(object baseObject, System.Action<GameObject> onLoad, string fileName = "", int count = 1, ResourceLoadType type = ResourceLoadType.AssetBundle)
    {
        CacheObject tempCache = this.FindByGameObject((GameObject)baseObject);
        if (tempCache == null || tempCache.BaseIsNull())
        {
            if (baseObject == null)
                return;
            CacheObject myCache = new CacheObject((GameObject)baseObject, fileName, count);
            GameObject freeObject = myCache.nextFree;
            if (freeObject != null)
                this.InstanceGameObject(freeObject);
            this.CacheList.Add(myCache);
            onLoad(freeObject);
        }
        else
        {
            GameObject freeObject = tempCache.nextFree;
            if (freeObject != null)
                this.InstanceGameObject(freeObject);
            onLoad(freeObject);
        }
    }
    public override void Delete(bool isByName = false)
    {
        base.Delete(false);
        AloneObjectCache.pInstance = null;
    }
    public virtual void DeleteByName(List<string> prefableName)
    {
        List<string> ResourceNameList = new List<string>();
        for (int i = this.CacheList.Count - 1; i >= 0; --i)
        {
            CacheObject temp = this.CacheList[i];
            if (temp != null)
            {
                if (!prefableName.Contains(temp.StrName))
                    continue;
                string FileName = ResPath.ReplaceFileName(temp.StrName, false);
                if (!string.IsNullOrEmpty(FileName))
                {
                    ResourceNameList.Add(FileName);
                }
                temp.Delete();
            }
            temp = null;
            this.CacheList.RemoveAt(i);
        }
        
        ResourceLoadManager.Instance.ReleaseBundleForName(ResourceNameList);
    }
}
public class CacheManage
{
    private static CacheManage pInstance;
    public static CacheManage Instance
    {
        get
        {
            if (pInstance == null)
                pInstance = new CacheManage();
            return pInstance;
        }
    }

    private List<string> EffectNameList;

    private bool EffectCompelete = false;

    public delegate void LoadComplete();
    public LoadComplete LoadCompleteDele;
    CacheManage()
    {
        this.EffectNameList = new List<string>();
    }
    public void PushEffect(string vName)
    {
        if (this.EffectNameList.Contains(vName))
            return;
        this.EffectNameList.Add(vName);
    }

    public void StartLoad()
    {
        if (this.EffectCompelete)
            return;
        Thread thread = new Thread(new ParameterizedThreadStart(Loading));
        thread.Start();
    }
    private void Loading(object ob)
    {
        if (this.EffectNameList == null)
            return;
        for (int i = 0; i < this.EffectNameList.Count; ++i )
        {
            EffectObjectCache.Instance.LoadGameObject(this.EffectNameList[i], (vGo) =>
            {
                EffectObjectCache.Instance.FreeObject(vGo);
            });
        }
    }
    private void EndLoad()
    {
        if(this.EffectCompelete)
        {
            if (this.LoadCompleteDele != null)
                this.LoadCompleteDele();

            this.EffectCompelete = false;
        }
    }
}