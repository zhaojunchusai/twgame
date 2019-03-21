using UnityEngine;
using System.Collections;

public class ReplaceShader : MonoBehaviour 
{

	// Use this for initialization
	void Awake () 
    {
        StartReplaceShader();
	}

    public void StartReplaceShader() 
    {
        if (!Application.isEditor) return;
        //Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        //foreach (Renderer r in renderers)
        //{
        //    foreach (Material mat in r.sharedMaterials)
        //    {
        //        if (!mat)
        //        {
        //            Debug.LogError("Material is null:", gameObject);
        //            continue;
        //        }
        //        if (!mat.shader) continue;
        //        mat.shader = Shader.Find(mat.shader.name);
        //    }
        //}
        UITexture tmpTexture = gameObject.GetComponent<UITexture>();
        if (tmpTexture != null)
        {
            if (tmpTexture.material != null)
            {
                if (tmpTexture.material.shader != null)
                {
                    tmpTexture.material.shader = Shader.Find("Unlit/Transparent ColoredRGBASeparate");
                }
            }
        }
    }

}
