using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class SweepResultItem_Reward : MonoBehaviour {

    /// <summary>
    /// 间距-无掉落物品
    /// </summary>
    private const int DISTANCE_COMMOM = 80;
    /// <summary>
    /// 间距-掉落物品Y坐标
    /// </summary>
    private const int DISTANCE_GOOD_Y = 100;
    /// <summary>
    /// 间距-掉落物品X坐标
    /// </summary>
    private const int DISTANCE_GOOD_X = 100;
    /// <summary>
    /// 初始位置-物品
    /// </summary>
    private const int INIT_GOOD_X = -200;
    private const int INIT_GOOD_Y = -47;
    /// <summary>
    /// 初始位置-分割线
    /// </summary>
    private const int INIT_LINE = 0;



    /// <summary>
    /// 主物件
    /// </summary>
    private Transform trans_Root;
    /// <summary>
    /// 战斗序列
    /// </summary>
    private UILabel Lbl_FightNum;
    /// <summary>
    /// 经验数值
    /// </summary>
    private UILabel Lbl_ExpValue;
    /// <summary>
    /// 金币数值
    /// </summary>
    private UILabel Lbl_GoldValue;
    /// <summary>
    /// 掉落物品背景图标
    /// </summary>
    private Transform trans_GoodsBG_Lan;
    /// <summary>
    /// 分割线
    /// </summary>
    private Transform trans_Goods_BG_Line;
    /// <summary>
    /// 单个掉落物品
    /// </summary>
    private Transform trans_Item;


    /// <summary>
    /// 掉落物品背景框列表
    /// </summary>
    private List<Transform> list_ItemBG = new List<Transform>();
    /// <summary>
    /// 掉落物品信息列表
    /// </summary>
    private List<Transform> list_ItemInfo = new List<Transform>();

    /// <summary>
    /// 初始化单场扫荡结果
    /// </summary>
    /// <param name="vFightIndex"></param>
    /// <param name="vExpValue"></param>
    /// <param name="vDropItemList"></param>
    public void Initialize(int vFightIndex, int vExpValue, DropList vDropItemList)
    {
        trans_Root = this.transform;
        if (trans_Root == null)
            return;

        Lbl_FightNum = trans_Root.FindChild("Item_Info/Info_FightNum").GetComponent<UILabel>();
        Lbl_ExpValue = trans_Root.FindChild("Item_Info/Info_EXP/EXP_Value").GetComponent<UILabel>();
        Lbl_GoldValue = trans_Root.FindChild("Item_Info/Info_Gold/Gold_Value").GetComponent<UILabel>();
        trans_GoodsBG_Lan = trans_Root.FindChild("Goods_BG_Lan").GetComponent<Transform>();
        trans_Goods_BG_Line = trans_Root.FindChild("Goods_BG_Line").GetComponent<Transform>();
        trans_Item = trans_Root.FindChild("Item_Goods").GetComponent<Transform>();

        RefreshUIStatus(vFightIndex, vExpValue, vDropItemList);
    }

    /// <summary>
    /// 显示单场扫荡结果
    /// </summary>
    /// <returns></returns>
    public int ShowUIStatus()
    {
        int tmpResult = DISTANCE_COMMOM;

        if (list_ItemBG != null)
        {
            for (int i = 0; i < list_ItemBG.Count; i++)
            {
                list_ItemBG[i].gameObject.SetActive(true);
                tmpResult += DISTANCE_GOOD_Y;
            }
        }
        if (list_ItemInfo != null)
        {
            for (int i = 0; i < list_ItemInfo.Count; i++)
                list_ItemInfo[i].gameObject.SetActive(true);
        }
        trans_Root.gameObject.SetActive(true);

        return tmpResult;
    }

    /// <summary>
    /// 初始化显示
    /// </summary>
    private void InitUIStatus()
    {
        if (trans_Root != null)
            trans_Root.gameObject.SetActive(false);
        if (Lbl_FightNum != null)
            Lbl_FightNum.text = string.Empty;
        if (Lbl_ExpValue != null)
            Lbl_ExpValue.text = string.Empty;
        if (Lbl_GoldValue != null)
            Lbl_GoldValue.text = string.Empty;
        if (trans_GoodsBG_Lan != null)
            trans_GoodsBG_Lan.gameObject.SetActive(false);
        if (trans_Item != null)
            trans_Item.gameObject.SetActive(false);
    }

    /// <summary>
    /// 刷新所有掉落物品信息
    /// </summary>
    /// <param name="vFightIndex"></param>
    /// <param name="vExpValue"></param>
    /// <param name="vDropItemList"></param>
    private void RefreshUIStatus(int vFightIndex, int vExpValue, DropList vDropItemList)
    {
        InitUIStatus();
        if (vFightIndex >= ConstString.SWEEPRESULT_TITLE_ARRAY.Length)
            vFightIndex = ConstString.SWEEPRESULT_TITLE_ARRAY.Length - 1;
        if (Lbl_FightNum != null)
            Lbl_FightNum.text = ConstString.SWEEPRESULT_TITLE_ARRAY[vFightIndex];
        if (Lbl_ExpValue != null)
            Lbl_ExpValue.text = vExpValue.ToString();

        if (vDropItemList == null)
            return;

        int tmpIndex = 0;
        if (vDropItemList.equip_list != null)
        {
            for (int i = 0; i < vDropItemList.equip_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vDropItemList.equip_list[i].id, 1))
                    tmpIndex++;
            }
        }
        if (vDropItemList.item_list != null)
        {
            for (int i = 0; i < vDropItemList.item_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vDropItemList.item_list[i].id, vDropItemList.item_list[i].change_num))
                    tmpIndex++;
            }
        }
        if (vDropItemList.mail_list != null)
        {
            for (int i = 0; i < vDropItemList.mail_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vDropItemList.mail_list[i].id, (int)vDropItemList.mail_list[i].num))
                    tmpIndex++;
            }
        }
        if (vDropItemList.soldier_list != null)
        {
            for (int i = 0; i < vDropItemList.soldier_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vDropItemList.soldier_list[i].id, 1))
                    tmpIndex++;
            }
        }
        if (vDropItemList.special_list != null)
        {
            for (int i = 0; i < vDropItemList.special_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vDropItemList.special_list[i].id, vDropItemList.special_list[i].change_num))
                    tmpIndex++;
            }
        }

        RefreshSingleItemBG(tmpIndex);
    }

    /// <summary>
    /// 刷新掉落物品背景框 分割线
    /// </summary>
    /// <param name="vCount"></param>
    private void RefreshSingleItemBG(int vCount)
    {
        if ((trans_Root == null) || (trans_GoodsBG_Lan == null))
            return;
        int tmpCount = vCount / 5;
        if (vCount % 5 != 0)
            tmpCount += 1;

        for (int i = 0; i < tmpCount; i++)
        {
            GameObject tmpObj = CommonFunction.InstantiateObject(trans_GoodsBG_Lan.gameObject, trans_Root);
            if (tmpObj == null)
                continue;
            tmpObj.transform.localPosition = new Vector3(0, INIT_GOOD_Y - DISTANCE_GOOD_Y * i, 0);
            list_ItemBG.Add(tmpObj.transform);
        }

        trans_Goods_BG_Line.transform.localPosition = new Vector3(0, INIT_LINE - DISTANCE_GOOD_Y * tmpCount, 0);
    }

    /// <summary>
    /// 刷新单个掉落物品信息
    /// </summary>
    /// <param name="vIndex">索引</param>
    /// <param name="vID">物品ID</param>
    /// <param name="vCount">数量</param>
    /// <returns></returns>
    private bool RefreshSingleItemInfo(int vIndex, uint vID, int vCount)
    {
        if ((trans_Root == null) || (trans_Item == null))
            return false;

        IDType tmpType = CommonFunction.GetTypeOfID(vID.ToString());
        if (tmpType == IDType.Gold)
        {
            if (Lbl_GoldValue != null)
                Lbl_GoldValue.text = vCount.ToString();
            return false;
        }

        GameObject tmpObj = CommonFunction.InstantiateObject(trans_Item.gameObject, trans_Root);
        if (tmpObj == null)
            return false;
        tmpObj.transform.localPosition = new Vector3(INIT_GOOD_X + DISTANCE_GOOD_X * (vIndex % 5), INIT_GOOD_Y - DISTANCE_GOOD_Y * (vIndex / 5), 0);
        string tmpIconName = CommonFunction.GetIconNameByID(vID);
        int tmpQuality = CommonFunction.GetQualityByID(vID);
        CommonFunction.SetSpriteName(tmpObj.transform.FindChild("Goods_Icon").GetComponent<UISprite>(), tmpIconName);
        CommonFunction.SetQualitySprite(tmpObj.transform.FindChild("Goods_Quality").GetComponent<UISprite>(), tmpQuality, tmpObj.GetComponent<UISprite>());
        tmpObj.transform.FindChild("Goods_Num").GetComponent<UILabel>().text = vCount.ToString();
        //显示武魂//
        tmpObj.transform.FindChild("Goods_Soul").gameObject.SetActive(false);
        if (tmpType == IDType.Prop)
        {
            ItemInfo tmpItem = ConfigManager.Instance.mItemData.GetItemInfoByID(vID);
            if (tmpItem != null)
                tmpObj.transform.FindChild("Goods_Soul").gameObject.SetActive(tmpItem.type == (int)ItemTypeEnum.SoldierChip);
        }
        tmpObj.gameObject.SetActive(true);
        list_ItemInfo.Add(tmpObj.transform); ;
        return true;
    }
}
