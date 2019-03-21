using UnityEngine;
using System.Collections;
using System;

public class RankViewTypeBase : MonoBehaviour {

    protected Action<uint> _clickType;
    protected RankviewData _data;
    public RankviewData Data
    {
        get
        {
            return _data;
        }
    }
    public uint ID
    {
        get
        {
            return _data.ID;
        }
    }

    protected virtual void ClickTypeBtn(GameObject go)
    {

    }
}
