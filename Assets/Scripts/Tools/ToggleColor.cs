/**********************
 * Zhu Huamao 2015-9-19
 * 
 * *************************/
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggled Color")]
public class ToggleColor : MonoBehaviour {

    public List<GameObject> targets;
    
    void Awake()
    {
        if (targets != null)
        {
#if UNITY_EDITOR
            NGUITools.SetDirty(this);
#endif
        }

#if UNITY_EDITOR
        if (!Application.isPlaying) return;
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
            if(widgets == null)
                return;
            Color color = state ? Color.white : new Color(64 / 255f, 64 / 255f, 64 / 255f);
            for (int i = 0; i < widgets.Length; i++)
            {
                widgets[i].color = color;
            }
        }
    }
}
