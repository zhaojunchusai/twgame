using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void CommandFunction(object vDataObj);

public class CommandManager : Singleton<CommandManager>
{
    private Dictionary<uint, List<CommandFunction>> _DicCommond = new Dictionary<uint, List<CommandFunction>>();

    public void Initialize()
    {
        if (_DicCommond == null)
            _DicCommond = new Dictionary<uint, List<CommandFunction>>();
        _DicCommond.Clear();
    }

    public void Uninitialize()
    {
        if (_DicCommond == null)
            return;
        _DicCommond.Clear();
    }

    /// <summary>
    /// 添加一个命令方法
    /// </summary>
    /// <param name="vKey"></param>
    /// <param name="vValue"></param>
    public void AddSingleCommand(uint vKey, CommandFunction vValue)
    {
        if (vValue == null)
            return;

        if (_DicCommond.ContainsKey(vKey))
        {
            if (_DicCommond[vKey].Contains(vValue))
                return;
            _DicCommond[vKey].Add(vValue);
        }
        else
        {
            List<CommandFunction> tmpList = new List<CommandFunction>();
            tmpList.Add(vValue);
            _DicCommond.Add(vKey, tmpList);
        }

        //Debug.LogWarning("Add Start-------------------------------------------------------------------------------");
        //foreach (KeyValuePair<uint, List<CommondFunction>> tmp in _DicCommond)
        //    Debug.LogWarning("tmp: " + tmp.Key.ToString());
        //Debug.LogWarning("Add End-------------------------------------------------------------------------------");
    }

    /// <summary>
    /// 销毁一个命令方法
    /// </summary>
    /// <param name="vKey"></param>
    /// <param name="vValue"></param>
    public void DelSingleCommand(uint vKey, CommandFunction vValue)
    {
        //Debug.LogWarning("Delete Start-------------------------------------------------------------------------------");
        //foreach (KeyValuePair<uint, List<CommondFunction>> tmp in _DicCommond)
        //    Debug.LogWarning("tmp: " + tmp.Key.ToString());
        //Debug.LogWarning("Delete End-------------------------------------------------------------------------------");
        if (vValue == null)
            return;

        if (!_DicCommond.ContainsKey(vKey))
            return;
        if (!_DicCommond[vKey].Contains(vValue))
            return;
        _DicCommond[vKey].Remove(vValue);
        if (_DicCommond[vKey].Count <= 0)
            _DicCommond.Remove(vKey);
    }

    /// <summary>
    /// 发送一个命令
    /// </summary>
    /// <param name="vKey"></param>
    /// <param name="vDataObj"></param>
    public void SendSingleCommand(uint vKey, object vDataObj = null)
    {
        //Debug.LogWarning("Send Start-------------------------------------------------------------------------------");
        //foreach (KeyValuePair<uint, List<CommondFunction>> tmp in _DicCommond)
        //    Debug.LogWarning("tmp: " + tmp.Key.ToString());
        //Debug.LogWarning("Send End-------------------------------------------------------------------------------");

        if (!_DicCommond.ContainsKey(vKey))
            return;
        if (_DicCommond[vKey] == null)
            return;

        //Debug.Log(string.Format("---Success to Send Single Command: [Key: {0}][Value: {1}]", vKey, vDataObj));
        for (int i = 0; i < _DicCommond[vKey].Count; i++)
            _DicCommond[vKey][i](vDataObj);
    }
}
