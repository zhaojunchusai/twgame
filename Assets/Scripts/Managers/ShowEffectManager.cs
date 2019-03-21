using UnityEngine;
using System.Collections;

public class ShowEffectManager : MonoSingleton<ShowEffectManager>
{

    public GameObject ShowEffect(string effectName,Vector3 effectPosition,Quaternion effectRotation,Transform effectParent)
    {
        GameObject effectPrefab =
            ResourceLoadManager.Instance.LoadResources(GlobalConst.Effect.DIR_EFFECT + effectName, false) as GameObject;
        GameObject se = GameObject.Instantiate(effectPrefab)as GameObject;

        se.transform.parent = effectParent;
        se.transform.localPosition = effectPosition;
        se.transform.localRotation = effectRotation;
        se.transform.localScale = Vector3.one;

        return se;
    }

    public GameObject ShowEffect(string effectname, Transform effectParent)
    {
        GameObject effectPrefab =
            ResourceLoadManager.Instance.LoadResources(GlobalConst.Effect.DIR_EFFECT + effectname, false) as GameObject;
        GameObject se = GameObject.Instantiate(effectPrefab) as GameObject;

        se.transform.parent = effectParent;
        se.transform.localPosition = Vector3.zero;
        se.transform.localRotation = Quaternion.identity;
        se.transform.localScale = Vector3.one;

        return se;
    }

    public GameObject ShowEffect(GameObject effectPrefab, Vector3 effectPosition, Quaternion effectRotation, Transform effectParent)
    {
        GameObject se = GameObject.Instantiate(effectPrefab) as GameObject;

        se.transform.parent = effectParent;
        se.transform.localPosition = effectPosition;
        se.transform.localRotation = effectRotation;
        se.transform.localScale = Vector3.one;

        return se;
    }

    public GameObject ShowEffect(GameObject effectPrefab, Vector3 worldPosition, Transform effectParent)
    {
        GameObject se = GameObject.Instantiate(effectPrefab) as GameObject;
        se.transform.parent = effectParent;
        se.transform.position = worldPosition;
        se.transform.localRotation = Quaternion.identity;
        se.transform.localScale = Vector3.one;

        return se;
    }

    public GameObject ShowEffect(GameObject effectPrefab, Transform effectParent)
    {
        if (effectPrefab == null) { Debug.LogError("effectPrefab == null"); return null; };
        GameObject se = GameObject.Instantiate(effectPrefab) as GameObject;
        se.transform.parent = effectParent;
        se.transform.localPosition = Vector3.zero;
        se.transform.localRotation = Quaternion.identity;
        se.transform.localScale = Vector3.one;
        se.SetActive(true);
        return se;
    }

    public void CloseEffect(GameObject effect)
    {
        GameObject.Destroy(effect);
    }
}

