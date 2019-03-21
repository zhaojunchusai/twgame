using UnityEngine;
using System.Collections;

public class CounterTool {

    private int _Count;
    public int Get_Count
    {
        get {
            return _Count;
        }
    }

    public CounterTool()
    {
        ReInitCounter();
    }

    /// <summary>
    /// 重置计数
    /// </summary>
    public void ReInitCounter()
    {
        _Count = 0;
    }

    /// <summary>
    /// 增加计数
    /// </summary>
    /// <param name="vCount"></param>
    public void AddCount(int vCount)
    {
        _Count += vCount;
    }
    public void AddCount()
    {
        AddCount(1);
    }

    /// <summary>
    /// 减少计数
    /// </summary>
    /// <param name="vCount"></param>
    public void DelCount(int vCount = 1)
    {
        if (_Count >= vCount)
            _Count -= vCount;
        else
            _Count = 0;
    }

}
