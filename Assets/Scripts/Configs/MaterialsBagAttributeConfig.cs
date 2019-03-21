using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class MaterialsBagAttributeInfo 
{
	/// <summary>
	/// 消耗包ID
	/// </summary>
	public uint nId;
	/// <summary>
	/// 金钱类型（1=游戏币；2=钻石）
	/// </summary>
	public int costType;
	/// <summary>
	/// 金钱数值
	/// </summary>
	public int Cost;
	/// <summary>
	/// 材料1ID
	/// </summary>
    public Dictionary<uint, int> MaterialList;
}

public class MaterialsBagAttributeConfig : BaseConfig 
{
	private List<MaterialsBagAttributeInfo> _materialsBagAttributeList;

	public MaterialsBagAttributeConfig()
	{
		_materialsBagAttributeList = new List<MaterialsBagAttributeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_MATERIALBAGATTRIBUTE, ParseConfig);
	}
    public MaterialsBagAttributeInfo FindBynId(uint id)
    {
        return _materialsBagAttributeList.Find(a => { return a.nId == id; });
    }
    public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_materialsBagAttributeList  != null)
		{
			_materialsBagAttributeList.Clear();
			_materialsBagAttributeList = null;
		}
	}

	public List<MaterialsBagAttributeInfo> GetMaterialsBagAttributeList()
	{
		return _materialsBagAttributeList;
	}
    private void ParseConfig(string bytes)
    {
        try
        {
            string xmlstr = bytes;
            XmlDocument xmlDoc =  new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
				MaterialsBagAttributeInfo data = new MaterialsBagAttributeInfo();
	 			foreach (XmlElement xe in Element) 
				{
                    if (xe.Name == "id")
					{
						data.nId = uint.Parse(xe.InnerText);
					}
                    else if (xe.Name == "money_type")
					{
						data.costType = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "money_value")
					{
						data.Cost = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "material_list")
					{
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.MaterialList = new Dictionary<uint, int>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;
                            if (data.MaterialList.ContainsKey(uint.Parse(tt[0]))) continue;
                            data.MaterialList.Add(uint.Parse(tt[0]), int.Parse(tt[1]));
                        }
                    }
				}

				_materialsBagAttributeList.Add(data);
			}
        	base.LoadCompleted();
		}
        catch (Exception ex)
        {
            Debug.LogError("Load _materialsBagAttributeList XML Error:" + ex.Message);
        }
    }
}