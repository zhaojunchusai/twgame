/**********************
 * Zhu Huamao 2015-9-19
 * 
 * *************************/
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggled Depth")]
public class ToggleDepth : MonoBehaviour {

    public List<GameObject> targets;
    public int depthUp = 1;
    public int depthDown = 0;

    void Awake()
    {
        if (targets != null)
        {
#if UNITY_EDITOR
            NGUITools.SetDirty(this);
#endif
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        UIToggle toggle = GetComponent<UIToggle>();
        EventDelegate.Add(toggle.onChange, Toggle);
    }

    public void Toggle()
    {
        bool val = UIToggle.current.value;

        if (enabled)
        {
            for (int i = 0; i < targets.Count; ++i)
                Set(targets[i], val);
        }
    }

    void Set(GameObject go, bool state)
    {
        if (go != null)
        {
            UIWidget[] widgets = go.GetComponents<UIWidget>();
            if (widgets == null)
                return;
            int depth = state ? depthUp : depthDown;
            for (int i = 0; i < widgets.Length; i++)
            {
                widgets[i].depth = depth;
            }
        }
    }
}
