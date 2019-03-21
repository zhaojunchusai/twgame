using UnityEngine;
using UnityEditor;

public class CreateAloneImage 
{
    [MenuItem("PXTools/Create Image Prefabs")]
    public static void BatchPrefab()
    {
        Object[] array = Selection.objects;
        for (int i = 0; i < array.Length; i++)
        {
            Texture texture = array[i] as Texture;
            GameObject go = new GameObject();
            go.name = texture.name;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            AloneImageHolder alone = go.AddComponent<AloneImageHolder>();
            alone.Texture = texture;
            Object tmp = PrefabUtility.CreatePrefab("Assets/ExportRes/Image/" + texture.name + ".prefab", go);
            PrefabUtility.InstantiatePrefab(tmp);
            GameObject.DestroyImmediate(go);
        }
    }

    [MenuItem("Tools/ReplaceTexture")]
    public static void ReplaceTexture()
    {
        Object[] array = Selection.objects;
        for (int i = 0; i < array.Length; i++) 
        {
            GameObject go = array[i] as GameObject;
            foreach (Transform tmp in go.transform) 
            {
                UITexture texture = tmp.GetComponent<UITexture>();
                if(texture != null)
                {
                    Debug.Log("gameobject name:" + texture.gameObject.name);
                    if (texture.gameObject.name.ToLower().Contains("icon")) 
                    {
                        texture.gameObject.AddComponent<UISprite>();
                        GameObject.Destroy(texture);
                    }
                   
                }
            }
        }
            
    }
}
