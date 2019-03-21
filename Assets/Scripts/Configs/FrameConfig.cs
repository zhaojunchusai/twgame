using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
public class FrameData
{
    public uint id;
    public uint type;
    public uint num;
    public string name;
    public string desc;

    public int const_time;
    public int hp;
    public int attack;
    public int hit;
    public int dodge;
    public int crit;
    public int crit_def;
    public int skill;
    public string effect_desc;
}


   public class FrameConfig : BaseConfig 
{

      private List<FrameData> _FrameList=new List<FrameData>();
	public FrameConfig ()
    {
        Initialize(GlobalConst.Config .DIR_XML_FRAME, ParseConfig);
    }
    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

	public void Uninitialize()
	{
	        _FrameList .Clear ();
	}

    //private void ParseConfig(WWW www)
    private void ParseConfig(string xmlstr)
    {
        try
        {
            //string xmlstr = System.Text.Encoding.UTF8.GetString(www.bytes);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            { 
				FrameData data =new FrameData();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "id")
					{
                       data.id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "type")
					{
                      data.type=uint.Parse (xe.InnerText);
					}
					else if (xe.Name == "num")
					{
                        data.num=uint.Parse (xe.InnerText );

					}
					else if (xe.Name == "name")
					{
						data.name =xe.InnerText ;
					}
                    else if (xe.Name == "desc")
                    {
                       data.desc =xe.InnerText;
                    }
				}
                
                data.const_time = int.Parse(CommonFunction.GetXmlElementStr(Element, "const_time"));
                data.hp = int.Parse(CommonFunction.GetXmlElementStr(Element, "hp")) / 100;
                data.attack = int.Parse(CommonFunction.GetXmlElementStr(Element, "attack")) / 100;
                data.hit = int.Parse(CommonFunction.GetXmlElementStr(Element, "hit")) / 100;
                data.dodge = int.Parse(CommonFunction.GetXmlElementStr(Element, "dodge")) / 100;
                data.crit = int.Parse(CommonFunction.GetXmlElementStr(Element, "crit")) / 100;
                data.crit_def = int.Parse(CommonFunction.GetXmlElementStr(Element, "crit_def")) / 100;
                data.skill = int.Parse(CommonFunction.GetXmlElementStr(Element, "skill"));
                data.effect_desc = CommonFunction.GetXmlElementStr(Element, "effect_desc");
                
                _FrameList.Add(data);
			}

        	base.LoadCompleted();
		}

        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    /// <summary>
    /// 通过ID获取边框ID
    /// </summary>
     public FrameData GetFrameDataByID(uint id)
    {
        FrameData info = _FrameList.Find((data) => { return data.id == id||data.id==id-1000; });
         if (info ==null )
         {
             Debug .LogError("can not get FrameData By ID:"+id);
         }
         return info ;

    }

     public List<FrameData> GetFrameList()
     {
         return _FrameList;
     }
    /// <summary>
    /// 通过ID获取持续时间
    /// </summary>
    public int GetConstTimeByID(uint id)
    {
        return _FrameList.Find((data) => { return data.id == id; }).const_time;
    }
    /// <summary>
    /// 通过ID获取头像框类
    /// </summary>
    public FrameData GetDateByID(uint id)
    {
        return _FrameList.Find((data) => { return data.id == id; });
    } 
}

