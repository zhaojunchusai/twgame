using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class SoundResWindowController : EditorWindow
{
    public static SoundResWindowController window;

    [MenuItem("PXTools/Set Sound Resource ", false, 1)]
    public static void ExecuteTargetBuild()
    {
        if (window == null)
        {
            window = (SoundResWindowController)GetWindow(typeof(SoundResWindowController));
        }
        window.Show();
    }

    private static bool isThreeDSound = false;
    private static int compression = 96;

    void OnGUI()
    {
        GUI.Label(new Rect(0f, 10f, 100f, 20f), "3D Sound");
        isThreeDSound = GUI.Toggle(new Rect(100f, 10f, 100f, 20f), isThreeDSound, string.Empty);
        GUI.Label(new Rect(0f, 30f, 100f, 20f), "Compression(Kbps)");
        compression = (int)GUI.HorizontalSlider(new Rect(100f, 30f, 100f, 20f), compression, 32, 256);
        string tmp = GUI.TextField(new Rect(200f, 30f, 100f, 20f), compression.ToString(), 3);
        if (int.TryParse(tmp, out compression))
        {
            if (GUI.Button(new Rect(100f, 60f, 100f, 30f), "Create Prefabs"))
            {
                AssetImport();
            }
        }
    }


    void AssetImport()
    {
        Object[] soundPrefabs = Selection.GetFiltered(typeof(AudioClip), SelectionMode.DeepAssets);
        foreach (Object prefab in soundPrefabs)
        {
            AudioClip clip = prefab as AudioClip;
            string path = AssetDatabase.GetAssetPath(clip);
            AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
            audioImporter.threeD = isThreeDSound;
            audioImporter.compressionBitrate = compression * 1000;
            AssetDatabase.ImportAsset(path);
            CreatePrefab(clip);
        }
    }

    void CreatePrefab(AudioClip clip)
    {
        GameObject go = new GameObject();
        go.name = clip.name;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        AudioSource alone = go.AddComponent<AudioSource>();
        alone.clip = clip;
        Object tmp = PrefabUtility.CreatePrefab("Assets/ExportRes/Sound/" + clip.name + ".prefab", go);
        PrefabUtility.InstantiatePrefab(tmp);
        GameObject.DestroyImmediate(go);
        Debug.Log("Create Clip Prefab:" + clip.name + " Success!");
    }
}
