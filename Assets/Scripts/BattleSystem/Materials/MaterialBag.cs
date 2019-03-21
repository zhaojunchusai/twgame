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
        /// �ɹ�
        /// </summary>
        OK,
        /// <summary>
        /// ��ʯ����
        /// </summary>
        diamond,
        /// <summary>
        /// ��Ҳ���
        /// </summary>
        coin,
        /// <summary>
        /// ���ϲ���
        /// </summary>
        material,
        /// <summary>
        /// û��ID
        /// </summary>
        noId
    }
    /// <summary>
    /// ǿ�����İ������ȡ<<���Ľ���ж�,���İ�����>List<<�������ͣ��������>>>
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