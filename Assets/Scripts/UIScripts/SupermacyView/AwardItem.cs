using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AwardItem
{
    private const int AWARD_POSY_INIT = 35;     //奖励初始Y坐标//
    private const int AWARD_POSY_DISTANCE = 27; //奖励间距//


    private GameObject rootObj;
    private UISprite Spt_OSprite;
    private UILabel Lbl_Order;
    private UISprite Spt_Award1;
    private UILabel Lbl_Award1;
    private UISprite Spt_Award2;
    private UILabel Lbl_Award2;


    private int index;


    public AwardItem(Transform vSourceTrans)
    {
        if (vSourceTrans != null)
        {
            rootObj = CommonFunction.InstantiateObject(vSourceTrans.gameObject, vSourceTrans.parent);
            if (rootObj != null)
            {
                Spt_OSprite = rootObj.transform.FindChild("ASprite").gameObject.GetComponent<UISprite>();
                Lbl_Order = rootObj.transform.FindChild("AOrder").gameObject.GetComponent<UILabel>();
                Spt_Award1 = rootObj.transform.FindChild("AIcon1").gameObject.GetComponent<UISprite>();
                Lbl_Award1 = rootObj.transform.FindChild("AValue1").gameObject.GetComponent<UILabel>();
                Spt_Award2 = rootObj.transform.FindChild("AIcon2").gameObject.GetComponent<UISprite>();
                Lbl_Award2 = rootObj.transform.FindChild("AValue2").gameObject.GetComponent<UILabel>();
                InitInfo();
            }
        }
    }
    ~AwardItem()
    {
        if (rootObj != null)
        {
            GameObject.Destroy(rootObj);
        }
    }

    public void InitInfo()
    {
        if (rootObj != null)
        {
            if (Lbl_Order != null)
            {
                Lbl_Order.text = string.Empty;
            }
            if (Lbl_Award1 != null)
            {
                Lbl_Award1.text = string.Empty;
            }
            if (Lbl_Award2 != null)
            {
                Lbl_Award2.text = string.Empty;
            }
            rootObj.SetActive(false);
        }
    }

    public bool ReSetInfo(int vIndex, uint vAwardID, bool vIsLast)
    {
        List<CommonItemData> tmpList = CommonFunction.GetCommonItemDataList(vAwardID);
        int tmpDiamondValue = 0;
        int tmpGoldValue = 0;
        if (tmpList != null)
        {
            for (int i = 0; i < tmpList.Count; i++)
            {
                if (tmpList[i] != null)
                {
                    if (tmpList[i].Type == IDType.Gold)
                    {
                        tmpGoldValue += tmpList[i].Num;
                    }
                    else if (tmpList[i].Type == IDType.Diamond)
                    {
                        tmpDiamondValue += tmpList[i].Num;
                    }
                }
            }
        }
        return ReSetInfo(vIndex, tmpGoldValue, tmpDiamondValue, vIsLast);
    }
    public bool ReSetInfo(int vIndex, int vGold, int vDiamond, bool vIsLast)
    {
        if (rootObj != null)
        {
            index = vIndex;
            rootObj.name = string.Format("Award_{0}", index);
            rootObj.transform.localPosition = new Vector3(0, AWARD_POSY_INIT - AWARD_POSY_DISTANCE * index, 0);
            if (Lbl_Order != null)
            {
                if (!vIsLast)
                {
                    Lbl_Order.text = string.Format(ConstString.SUPERMACY_RANK, vIndex + 1);
                }
                else
                {
                    Lbl_Order.text = ConstString.SUPERMACY_PARTICIPATION;
                }
            }
            if (Lbl_Award1 != null)
            {
                Lbl_Award1.text = string.Format("x{0}", vDiamond);
            }
            if (Lbl_Award2 != null)
            {
                Lbl_Award2.text = string.Format("x{0}", vGold);
            }
            if (Spt_OSprite != null)
            {
                Spt_OSprite.gameObject.SetActive(index % 2 == 0);
            }
            rootObj.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }

}
