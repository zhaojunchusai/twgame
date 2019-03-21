using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class SacrificialSystemInfo 
{
	/// <summary>
	/// 类型
	/// </summary>
	public int type;
	/// <summary>
	/// 消耗能量
	/// </summary>
	public int exp_num;
    /// <summary>
    /// 货币消耗类型
    /// </summary>
    public int money_type;
	/// <summary>
	/// 消耗铜钱
	/// </summary>
	public int gold_num;
    /// <summary>
    /// 能量转换值对应的品质
    /// </summary>
    public int Quality_First;
	/// <summary>
	/// 能量转换值
	/// </summary>
    public Dictionary<int, int> exp_value_list;
    /// <summary>
    /// 能量转换值对应的品质
    /// </summary>
    public int Quality_Sec;
    /// <summary>
    /// 品质更高一级的能量转换值
    /// </summary>
    public Dictionary<int, int> exp_value_list2;
	/// <summary>
	/// 随机包
	/// </summary>
	public int drop_id;
	/// <summary>
	/// 规则
	/// </summary>
	public string desc;
    public string noMaterial;
}

public class SacrificialSystemConfig : BaseConfig 
{
	private List<SacrificialSystemInfo> _sacrificialSystemList;

	public SacrificialSystemConfig()
	{
		_sacrificialSystemList = new List<SacrificialSystemInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SACRIFICIALSYSYTEM, ParseConfig);
	}

    public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

    public SacrificialSystemInfo FindByType(int vType)
    {
        return this._sacrificialSystemList.Find((data) => { if (data == null)return false; return data.type == vType; });
    }
	public void Uninitialize()
	{
		 if (_sacrificialSystemList  != null)
		{
			_sacrificialSystemList.Clear();
			_sacrificialSystemList = null;
		}
	}

	public List<SacrificialSystemInfo> GetSacrificialSystemList()
	{
		return _sacrificialSystemList;
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
				SacrificialSystemInfo data = new SacrificialSystemInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "type")
					{
						data.type = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "exp_num")
					{
                        data.exp_num = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "money_type")
                    {
                        data.money_type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "gold_num")
					{
						data.gold_num = int.Parse(xe.InnerText);
					}
                    else if(xe.Name == "Quality_First")
                    {
                        data.Quality_First = int.Parse(xe.InnerText);
                    }
					else if (xe.Name == "exp_value_list")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.exp_value_list = new Dictionary<int, int>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;
                            if (data.exp_value_list.ContainsKey(int.Parse(tt[0]))) continue;
                            data.exp_value_list.Add(int.Parse(tt[0]), int.Parse(tt[1]));
                        }
					}
                    else if (xe.Name == "Quality_Sec")
                    {
                        data.Quality_Sec = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "exp_value_list2")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.exp_value_list2 = new Dictionary<int, int>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;
                            if (data.exp_value_list2.ContainsKey(int.Parse(tt[0]))) continue;
                            data.exp_value_list2.Add(int.Parse(tt[0]), int.Parse(tt[1]));
                        }
                    }
					else if (xe.Name == "drop_id")
					{
						data.drop_id = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "desc")
					{
                        data.desc = CommonFunction.ReplaceEscapeChar(xe.InnerText);
					}
                    else if (xe.Name == "noMatrial")
                    {
                        data.noMaterial = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
				}

				_sacrificialSystemList.Add(data);
			}

        	base.LoadCompleted();
       		Debug.Log("Load ChapterMonster XML Success!----count:" + _sacrificialSystemList.Count);
		}

        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}