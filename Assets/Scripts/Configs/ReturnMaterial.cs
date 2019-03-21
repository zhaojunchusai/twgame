using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class ReturnMaterialInfo
{
	/// <summary>
	/// 甄选产出品质
	/// </summary>
    public int level;
    /// <summary>
    /// 返还技能书ID，数量
    /// </summary>
    public List<KeyValuePair<uint, int>> ReturnMaterial;
    /// <summary>
    /// 升星返还
    /// </summary>
    public List<KeyValuePair<uint, int>> ReturnMaterialStar;
}

public class ReturnMaterialConfig : BaseConfig 
{
    private List<ReturnMaterialInfo> _returnMaterialList;

    public ReturnMaterialConfig()
	{
		_returnMaterialList = new List<ReturnMaterialInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SKILLRETURNMATERIALATTRIBUTE, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);
	}
    public ReturnMaterialInfo FindByLevel(int level)
    {
        return this._returnMaterialList.Find((info) => { if (info == null)return false; return info.level == level; });
    }
	public void Uninitialize()
	{
		 if (_returnMaterialList  != null)
		{
			_returnMaterialList.Clear();
			_returnMaterialList = null;
		}
	}

	public List<ReturnMaterialInfo> GetSoldierSelectList()
	{
		return _returnMaterialList;
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
				ReturnMaterialInfo data = new ReturnMaterialInfo();
	 			foreach (XmlElement xe in Element) 
				{
                    if (xe.Name == "level")
					{
                        data.level = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "return_material")
					{
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.ReturnMaterial = new List<KeyValuePair<uint, int>>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;

                            data.ReturnMaterial.Add(new KeyValuePair<uint, int>(uint.Parse(tt[0]), int.Parse(tt[1])));
                        }
					}
                    else if (xe.Name == "return_material_star")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.ReturnMaterialStar = new List<KeyValuePair<uint, int>>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;

                            data.ReturnMaterialStar.Add(new KeyValuePair<uint, int>(uint.Parse(tt[0]), int.Parse(tt[1])));
                        }
                    }
				}

				_returnMaterialList.Add(data);
			}
        	base.LoadCompleted();
		}
        catch (Exception ex)
        {
            Debug.LogError("Load returnMaterial XML Error:" + ex.Message);
        }
    }
}