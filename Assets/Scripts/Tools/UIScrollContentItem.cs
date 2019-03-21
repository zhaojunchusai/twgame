using UnityEngine;
using System.Collections;

public class UIScrollContentItem : MonoBehaviour
{
    private int _depth = 0;
    public int Depth
    {
        get { return _depth; }
        set { _depth = value;DepthChange();}
    }

    protected virtual void DepthChange()
    {
        
    }
}
