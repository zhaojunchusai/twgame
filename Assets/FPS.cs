using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FPS : MonoBehaviour 
{
    public List<GameObject> ObjList = new List<GameObject>();
    public GameObject sss;
    private List<SkeletonAnimation> tmpList = new List<SkeletonAnimation>();

    public Vector2 scrollPosition = Vector2.zero;
    public const int m_KBSize = 1024 * 1024;
    public float updateInterval = 0.5F;
    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
    private float fps = 0.0F;
    int texturesMemSize = 0;
    int audioMemSize = 0;
    int meshesMemSize = 0;
	// Use this for initialization
	void Start () 
    {
        timeleft = updateInterval;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        if (timeleft <= 0.0) 
        {
            fps = accum / frames;
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
	}


    int tmpInitX = 0;

    public void ComputeMemSize() 
    {
        UnityEngine.Object[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture));
        UnityEngine.Object[] audioclips = Resources.FindObjectsOfTypeAll(typeof(AudioClip));
        UnityEngine.Object[] meshes = Resources.FindObjectsOfTypeAll(typeof(Mesh));

        for (int i = 0; i < textures.Length; ++i)
        {
            texturesMemSize += Profiler.GetRuntimeMemorySize(textures[i]);
        }
        for (int i = 0; i < audioclips.Length; ++i)
        {
            audioMemSize += Profiler.GetRuntimeMemorySize(audioclips[i]);
        }
        for (int i = 0; i < meshes.Length; ++i)
        {
            meshesMemSize += Profiler.GetRuntimeMemorySize(meshes[i]);
        }
 
    }

    void OnGUI() 
    {
        GUI.Box(new Rect(0, 0, 200, 200), "");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(200), GUILayout.Height(200));
        if (GUILayout.Button("ComputeMemSize")) 
        {
            ComputeMemSize();
        }
        GUILayout.Label(System.String.Format("{0:F2} FPS    ObjCount: [{1}]", fps, tmpList.Count));
        GUILayout.Label("AllocatedMem = " + (int)Profiler.GetTotalAllocatedMemory() / m_KBSize+"M");
        GUILayout.Label("ReservedMem = " + (int)Profiler.GetTotalReservedMemory() / m_KBSize + "M");
        GUILayout.Label("UsedReservedMem = " + (int)Profiler.GetTotalUnusedReservedMemory() / m_KBSize + "M");
        GUILayout.Label("monoUsedSize = " + (int)Profiler.GetMonoUsedSize() / m_KBSize + "M");
        GUILayout.Label("monoHeapSize  = " + (int)Profiler.GetMonoHeapSize() / m_KBSize + "M");
        GUILayout.Label("usedHeapSize = " + (int)Profiler.usedHeapSize / m_KBSize + "M");
        GUILayout.Label("texturesMemSize = " + texturesMemSize / m_KBSize + "M");
        GUILayout.Label("audioMemSize = " + audioMemSize / m_KBSize + "M");
        GUILayout.Label("meshesMemSize = " + meshesMemSize / m_KBSize + "M");
        GUILayout.EndScrollView();
    }
}
