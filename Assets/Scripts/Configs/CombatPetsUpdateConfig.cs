using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
public class PetCommonMaterilData
{
    public uint materailID;

    public int num;
}

public class CombatPetsUpdateInfo
{
    public uint id;

    /// <summary>
    /// 货币类型
    /// </summary>
    public int cerrencyType;
    /// <summary>
    /// 货币值
    /// </summary>
    public int cerrencyNum;

    public List<PetCommonMaterilData> materials;

}


public class CombatPetsUpdateConfig : BaseConfig
{
    private List<CombatPetsUpdateInfo> mPetUpdateList;

    public CombatPetsUpdateConfig()
    {
        mPetUpdateList = new List<CombatPetsUpdateInfo>();
        Initialize(GlobalConst.Config.DIR_XML_PETSUPDATE, ParseConfig);
    }

    public override void Initialize(string vFileName, Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    private void ParseConfig(string xmlstr)
    {
        //try
        //{
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                CombatPetsUpdateInfo data = new CombatPetsUpdateInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name.Equals("id"))
                    {
                        data.id = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("type"))
                    {
                        data.cerrencyType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("value"))
                    {
                        data.cerrencyNum = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("costbagItem"))
                    {
                        data.materials = new List<PetCommonMaterilData>();
                        string[] matArr = xe.InnerText.Split(';');
                        if (matArr == null)
                            continue;
                        for (int i = 0; i < matArr.Length; i++)
                        {
                            string[] mat = matArr[i].Split(':');
                            if (mat == null || mat.Length < 2)
                                continue;
                            PetCommonMaterilData mData = new PetCommonMaterilData();
                            mData.materailID = uint.Parse(mat[0]);
                            mData.num = int.Parse(mat[1]);
                            data.materials.Add(mData);
                        }
                    }
                }
                mPetUpdateList.Add(data);
            }
            base.LoadCompleted();
        //}
        //catch (Exception ex)
        //{
        //    base.LoadCompleted();
        //    Debug.LogError("Load CombatPetsUpdate XML Error:" + ex.Message);
        //}
    }

    public CombatPetsUpdateInfo GetInfoByID(uint id)
    {
        CombatPetsUpdateInfo info = mPetUpdateList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.id == id;
        });
        if (info == null)
            Debug.LogError("can not get CombatPetsUpdateInfo by ID:" + id.ToString());
        return info;
    }

}
