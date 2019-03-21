using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public class MaterialBag
{
    public enum MaterialResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        OK,
        /// <summary>
        /// 钻石不足
        /// </summary>
        diamond,
        /// <summary>
        /// 金币不足
        /// </summary>
        coin,
        /// <summary>
        /// 材料不足
        /// </summary>
        material,
        /// <summary>
        /// 没有ID
        /// </summary>
        noId
    }
    /// <summary>
    /// 强化消耗包结果获取<<消耗结果判断,消耗包属性>List<<材料类型，需求个数>>>
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public static KeyValuePair<KeyValuePair<MaterialResult, MaterialsBagAttributeInfo>, List<KeyValuePair<Item, int>>> getResult(uint Id)
    {
        MaterialsBagAttributeInfo tempInf =  ConfigManager.Instance.mMaterialsBagData.FindBynId(Id);
        List<KeyValuePair<Item, int>> tempList = new List<KeyValuePair<Item, int>>();

        if (tempInf == null) return new KeyValuePair<KeyValuePair<MaterialResult, MaterialsBagAttributeInfo>, List<KeyValuePair<Item, int>>>(new KeyValuePair<MaterialResult, MaterialsBagAttributeInfo>(MaterialResult.noId, tempInf), tempList); ;
        MaterialResult tempResult = MaterialResult.OK;
        switch(tempInf.costType)
        {
            case 1:if(PlayerData.Instance._Gold < tempInf.Cost) tempResult = MaterialResult.coin; break;
            case 2:if(PlayerData.Instance._Diamonds < tempInf.Cost) tempResult = MaterialResult.diamond;break;
        }

        foreach(var temp in tempInf.MaterialList)
        {
            Item tempIt = PlayerData.Instance._MaterialBag.Find((Item it) => { return it.id == temp.Key; });
            if(tempIt == null)
            {
                tempIt = new Item();
                tempIt.id = (uint)temp.Key;
                tempIt.num = 0;
            }
            if(tempIt.num < temp.Value)
            {
                tempResult = MaterialResult.material;
            }
            
            tempList.Add(new KeyValuePair<Item, int>(tempIt, temp.Value));
        }
        return new KeyValuePair<KeyValuePair<MaterialResult, MaterialsBagAttributeInfo>, List<KeyValuePair<Item, int>>>(new KeyValuePair<MaterialResult, MaterialsBagAttributeInfo>(tempResult,tempInf),tempList);
    }
}