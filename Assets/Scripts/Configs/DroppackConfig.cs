using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 掉落类型
/// </summary>
public enum DropFactorType 
{
    None = 0,
    /// <summary>
    /// 权重掉落
    /// </summary>
    Weight = 1,
    /// <summary>
    /// 概率
    /// </summary>
    Probability = 2,
}


public enum DropItemType 
{
    None = 0,
    /// <summary>
    /// 士兵
    /// </summary>
    Soldier = 1,
    /// <summary>
    /// 装备
    /// </summary>
    Equip = 2,
    /// <summary>
    /// 道具
    /// </summary>
    Item = 3,
    /// <summary>
    /// 数值
    /// </summary>
    Number = 5,
    /// <summary>
    /// 命魂
    /// </summary>
    LifeSoul = 6,
     /// <summary>
    /// 宠物
    /// </summary>
    Pet = 7,
    /// <summary>
    /// 子包
    /// </summary>
    DropPack = 9,
}
   


public class DroppackInfo 
{
	/// <summary>
	/// 包ID
	/// </summary>
	public uint DropBagID;
	/// <summary>
	/// 掉落类型
	/// </summary>
    public DropFactorType FactorType;
	/// <summary>
	/// 权重/概率（1/10000）
	/// </summary>
	public int DropFactorValue;
	/// <summary>
	/// 包/道具类型
	/// </summary>
    public DropItemType ItemType;
	/// <summary>
	/// 子包/道具ID
	/// </summary>
	public uint ItemID;
	/// <summary>
	/// 数量下限
	/// </summary>
	public int ItemLowerLimit;
	/// <summary>
	/// 数量上限
	/// </summary>
	public int ItemUpperLimit;
}

public class DroppackConfig : BaseConfig 
{
    private List<DroppackInfo> _droppackList;
    private List<DroppackInfo> _sonpackList;

	public DroppackConfig()
	{
        _droppackList = new List<DroppackInfo>();
        _sonpackList = new List<DroppackInfo>();
        Initialize(GlobalConst.Config.DIR_XML_DROPPACK, ParseConfig);
        //Initialize(GlobalConst.Config.DIR_XML_GAMEACTIVITY_DROPPACK, ParseConfig);
        Initialize(GlobalConst.Config.DIR_XML_SONPACK, ParseConfigSon);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}
	
	public List<DroppackInfo> GetDroppackList()
	{
		return _droppackList;
	}

    public List<DroppackInfo> GetDropPackByID(uint dropID)
    {
        List<DroppackInfo> info = _droppackList.FindAll((DroppackInfo _droppack) => _droppack.DropBagID == dropID);
        return info;
    }

    public List<DroppackInfo> GetSonPackByID(uint dropID)
    {
        List<DroppackInfo> info = _sonpackList.FindAll((DroppackInfo _droppack) => _droppack.DropBagID == dropID);
        return info;
    }

    public DroppackInfo DontUseOldFunc_GetDropPackByID(uint dropID)
    {
        DroppackInfo info = _droppackList.Find((DroppackInfo _droppack) => _droppack.DropBagID == dropID);
        if (info == null)
        {
            Debug.LogError("can not get DroppackInfo by ID:" + dropID.ToString());
        }
        return info;
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
                //Debug.LogWarning("=================================================");
				DroppackInfo data = new DroppackInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "DropBagID")
					{
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
						data.DropBagID = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "DropFactorType")
					{
                        int type = 0;
                        if (int.TryParse(xe.InnerText, out type))
                        {
                            data.FactorType = (DropFactorType)type;
                        }
                        else 
                        {
                            Debug.LogError("error FactorType data!!");
                        }
					}
					else if (xe.Name == "DropFactorValue")
					{
						data.DropFactorValue = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "DropItemType")
					{
                        int type = 0;
                        if (int.TryParse(xe.InnerText, out type))
                        {
                            data.ItemType = (DropItemType)type;
                        }
                        else
                        {
                            Debug.LogError("error ItemType data!!");
                        }
					}
					else if (xe.Name == "ItemID")
					{
						data.ItemID = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "ItemLowerLimit")
					{
						data.ItemLowerLimit = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "ItemUpperLimit")
					{
						data.ItemUpperLimit = int.Parse(xe.InnerText);
					}
				}

				_droppackList.Add(data);
			}

        	base.LoadCompleted();
		}
        catch (Exception ex)
        {
            Debug.LogError("Load Droppack XML Error:" + ex.Message);
        }
    }

    private void ParseConfigSon(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                DroppackInfo data = new DroppackInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "DropBagID")
                    {
                        data.DropBagID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "DropFactorType")
                    {
                        int type = 0;
                        if (int.TryParse(xe.InnerText, out type))
                        {
                            data.FactorType = (DropFactorType)type;
                        }
                        else
                        {
                            Debug.LogError("error FactorType data!!");
                        }
                    }
                    else if (xe.Name == "DropFactorValue")
                    {
                        data.DropFactorValue = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "DropItemType")
                    {
                        int type = 0;
                        if (int.TryParse(xe.InnerText, out type))
                        {
                            data.ItemType = (DropItemType)type;
                        }
                        else
                        {
                            Debug.LogError("error ItemType data!!");
                        }
                    }
                    else if (xe.Name == "ItemID")
                    {
                        data.ItemID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ItemLowerLimit")
                    {
                        data.ItemLowerLimit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ItemUpperLimit")
                    {
                        data.ItemUpperLimit = int.Parse(xe.InnerText);
                    }
                }

                _sonpackList.Add(data);
            }

            base.LoadCompleted();
        }

        catch (Exception ex)
        {
            Debug.LogError("Load Droppack XML Error:" + ex.Message);
        }
    }
    public void Uninitialize()
    {
        if (_droppackList != null)
        {
            _droppackList.Clear();
            _droppackList = null;
        }
    }

}