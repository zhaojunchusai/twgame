using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class DrowEquipInfo
{
	/// <summary>
	/// 类型
	/// </summary>
	public int type;
	/// <summary>
	/// 消耗道具ID
	/// </summary>
	public int Item_id;
	/// <summary>
    /// 消耗道具数量
	/// </summary>
    public int Item_num;
    /// <summary>
    /// 消耗金钱类型
    /// </summary>
    public int Gold_type;
    /// <summary>
    /// 消耗金钱数量
    /// </summary>
    public int Gold_num;
}

public class DrowEquipConfig : BaseConfig 
{
	private List<DrowEquipInfo> _DrowEquipList;

	public DrowEquipConfig()
	{
		_DrowEquipList = new List<DrowEquipInfo>();
        Initialize(GlobalConst.Config.DIR_XML_DROWEQUIP, ParseConfig);
	}

    public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

    public DrowEquipInfo FindByType(int vType)
    {
        return this._DrowEquipList.Find((data) => { if (data == null)return false; return data.type == vType; });
    }
	public void Uninitialize()
	{
		 if (_DrowEquipList  != null)
		{
			_DrowEquipList.Clear();
			_DrowEquipList = null;
		}
	}

	public List<DrowEquipInfo> GetSacrificialSystemList()
	{
		return _DrowEquipList;
	}

    private void ParseConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            { 
				DrowEquipInfo data = new DrowEquipInfo();
	 			foreach (XmlElement xe in Element) 
				{
                    if (xe.Name == "extract_type")
					{
						data.type = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "item_id")
					{
						data.Item_id = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "item_num")
					{
						data.Item_num = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "money_type")
                    {
                        data.Gold_type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "money_num")
                    {
                        data.Gold_num = int.Parse(xe.InnerText);
                    }
                }

				_DrowEquipList.Add(data);
			}

        	base.LoadCompleted();
       		Debug.Log("Load ChapterMonster XML Success!----count:" + _DrowEquipList.Count);
		}

        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}