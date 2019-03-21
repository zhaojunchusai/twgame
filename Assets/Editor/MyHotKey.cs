using UnityEngine;
using UnityEditor;

public class MyHotKey :Editor{

    //在文件浏览器中打开
    //ctrl + alt + f 
    //alt + ctrl + f
    [MenuItem("Tools/Show In Explorer #&f &#f")]
    static void ShowInExplorer()
    {
        Object[] gos = Selection.objects;
        for (int i = 0; i < gos.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(gos[i]);
            if (!string.IsNullOrEmpty(path))
            {
                string tmp = Application.dataPath;
                tmp = tmp.Substring(0, tmp.LastIndexOf('/') + 1) + path;
                tmp = tmp.Replace('/', '\\');
                System.Diagnostics.Process.Start("explorer.exe", "/select," + tmp);
            }
        }        
    }

    [MenuItem("Tools/Check Effect Shader")]
    static void CheckEffectShader()
    {
        Shader shader = Shader.Find("Mobile/Particles/Additive");
        Transform[] gos = Selection.GetTransforms(SelectionMode.Deep);
        for (int i = 0; i < gos.Length; i++)
        {
            ParticleSystem particle = gos[i].GetComponent<ParticleSystem>();
            if (particle != null)
            {
                if (!particle.renderer.sharedMaterial.shader.name.Contains("Mobile"))
                {
                    particle.renderer.sharedMaterial.shader = shader;
                    EditorUtility.SetDirty(gos[i]);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}
