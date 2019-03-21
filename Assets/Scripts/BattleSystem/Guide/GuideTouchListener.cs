using UnityEngine;
class GuideTouchListener : MonoBehaviour
{
    public delegate void VoidDelegate(GameObject go);

    public VoidDelegate onClick;

    void OnClick() { if (onClick != null) onClick(gameObject); }
    static public GuideTouchListener Get(GameObject go)
    {
        GuideTouchListener listener = go.GetComponent<GuideTouchListener>();
        if (listener == null) listener = go.AddComponent<GuideTouchListener>();
        return listener;
    }
}