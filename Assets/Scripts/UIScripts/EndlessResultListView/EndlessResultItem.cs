using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class EndlessResultItem : MonoBehaviour
{
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
    /// 设置界面
    /// </summary>
    /// <param name="vIndex">索引</param>
    /// <param name="vItem">数据</param>
    /// <param name="vInitPosY">初始位置</param>
    /// <returns>下一条奖励初始位置</returns>
    public float Initialize(int vIndex, EndlessDungeonRewardResp vItem, float vInitPosY = 0)
    {
        trans_Root = this.transform;
        if (trans_Root == null)
            return vInitPosY;
        trans_Root.localPosition = new Vector3(0, vInitPosY, 0);
        Lbl_FightNum = trans_Root.FindChild("Item_Info/Info_FightNum").GetComponent<UILabel>();
        Lbl_ExpValue = trans_Root.FindChild("Item_Info/Info_EXP/EXP_Value").GetComponent<UILabel>();
        Lbl_GoldValue = trans_Root.FindChild("Item_Info/Info_Gold/Gold_Value").GetComponent<UILabel>();
        trans_GoodsBG_Lan = trans_Root.FindChild("Goods_BG_Lan").GetComponent<Transform>();
        trans_Goods_BG_Line = trans_Root.FindChild("Goods_BG_Line").GetComponent<Transform>();
        trans_Item = trans_Root.FindChild("Item_Goods").GetComponent<Transform>();
        return vInitPosY - ShowUIStatus(vIndex, vItem);
    }

    private void InitUIStatus()
    {
        if (trans_Root != null)
            trans_Root.gameObject.SetActive(false);
        if (Lbl_FightNum != null)
            Lbl_FightNum.text = string.Empty;
        if (Lbl_ExpValue != null)
            Lbl_ExpValue.text = "0";
        if (Lbl_GoldValue != null)
            Lbl_GoldValue.text = "0";
        if (trans_GoodsBG_Lan != null)
            trans_GoodsBG_Lan.gameObject.SetActive(false);
        if (trans_Item != null)
            trans_Item.gameObject.SetActive(false);
    }

    private float ShowUIStatus(int vIndex, EndlessDungeonRewardResp vItem)
    {
        if (vIndex >= ConstString.ENDLESSRESULT_TITLE_ARRAY.Length)
        {
            vIndex = ConstString.ENDLESSRESULT_TITLE_ARRAY.Length - 1;
        }
        InitUIStatus();
        if (Lbl_FightNum != null)
            Lbl_FightNum.text = ConstString.ENDLESSRESULT_TITLE_ARRAY[vIndex];
        if (vItem == null)
            return 0;

        if (Lbl_ExpValue != null)
        {
            if (vItem.exp != null)
                Lbl_ExpValue.text = vItem.exp.add_exp.ToString();
        }

        if (vItem.drop_items == null)
            return 0;
        int tmpIndex = 0;
        if (vItem.drop_items.equip_list != null)
        {
            for (int i = 0; i < vItem.drop_items.equip_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vItem.drop_items.equip_list[i].id, 1))
                    tmpIndex++;
            }
        }
        if (vItem.drop_items.item_list != null)
        {
            for (int i = 0; i < vItem.drop_items.item_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vItem.drop_items.item_list[i].id, vItem.drop_items.item_list[i].change_num))
                    tmpIndex++;
            }
        }
        if (vItem.drop_items.mail_list != null)
        {
            for (int i = 0; i < vItem.drop_items.mail_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vItem.drop_items.mail_list[i].id, (int)vItem.drop_items.mail_list[i].num))
                    tmpIndex++;
            }
        }
        if (vItem.drop_items.soldier_list != null)
        {
            for (int i = 0; i < vItem.drop_items.soldier_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vItem.drop_items.soldier_list[i].id, 1))
                    tmpIndex++;
            }
        }
        if (vItem.drop_items.special_list != null)
        {
            for (int i = 0; i < vItem.drop_items.special_list.Count; i++)
            {
                if (RefreshSingleItemInfo(tmpIndex, vItem.drop_items.special_list[i].id, vItem.drop_items.special_list[i].change_num))
                    tmpIndex++;
            }
        }

        return RefreshSingleItemBG(tmpIndex);
    }

    /// <summary>
    /// 刷新掉落物品背景框 分割线
    /// </summary>
    /// <param name="vCount"></param>
    private float RefreshSingleItemBG(int vCount)
    {
        float tmpResult = DISTANCE_COMMOM;
        if ((trans_Root == null) || (trans_GoodsBG_Lan == null))
            return tmpResult;
        int tmpCount = vCount / 5;
        if (vCount % 5 != 0)
            tmpCount += 1;

        for (int i = 0; i < tmpCount; i++)
        {
            GameObject tmpObj = CommonFunction.InstantiateObject(trans_GoodsBG_Lan.gameObject, trans_Root);
            if (tmpObj == null)
                continue;
            tmpObj.transform.localPosition = new Vector3(0, INIT_GOOD_Y - DISTANCE_GOOD_Y * i, 0);
            tmpObj.SetActive(true);
            list_ItemBG.Add(tmpObj.transform);
            tmpResult += DISTANCE_GOOD_Y;
        }

        trans_Goods_BG_Line.transform.localPosition = new Vector3(0, INIT_LINE - DISTANCE_GOOD_Y * tmpCount, 0);
        return tmpResult;
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

        tmpObj.transform.FindChild("Goods_Soul").gameObject.SetActive(false);
        if (tmpType == IDType.Prop)
        {
            ItemInfo tmpItem = ConfigManager.Instance.mItemData.GetItemInfoByID(vID);
            if (tmpItem != null)
            {
                tmpObj.transform.FindChild("Goods_Soul").gameObject.SetActive(tmpItem.type == (int)ItemTypeEnum.SoldierChip);
            }
        }
        tmpObj.gameObject.SetActive(true);
        list_ItemInfo.Add(tmpObj.transform); ;
        return true;
    }
}
