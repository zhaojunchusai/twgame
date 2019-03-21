using UnityEngine;
using System.Collections;

public class OrderItem
{
    private const int ORDER_POSY_INIT = 45;     //排名初始Y坐标//
    private const int ORDER_POSY_DISTANCE = 28; //排名间距//


    private GameObject rootObj;
    private UISprite Spt_OSprite;
    private UILabel Lbl_Order;
    private UILabel Lbl_Nick;
    private UILabel Lbl_Hurt;


    private int index;


    public OrderItem(Transform vSourceTrans)
    {
        if (vSourceTrans != null)
        {
            rootObj = CommonFunction.InstantiateObject(vSourceTrans.gameObject, vSourceTrans.parent);
            if (rootObj != null)
            {
                Spt_OSprite = rootObj.transform.FindChild("OSprite").gameObject.GetComponent<UISprite>();
                Lbl_Order = rootObj.transform.FindChild("Order").gameObject.GetComponent<UILabel>();
                Lbl_Nick = rootObj.transform.FindChild("Nick").gameObject.GetComponent<UILabel>();
                Lbl_Hurt = rootObj.transform.FindChild("Damage").gameObject.GetComponent<UILabel>();
                InitInfo();
            }
        }
    }
    ~OrderItem()
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
            if (Lbl_Nick != null)
            {
                Lbl_Nick.text = string.Empty;
            }
            if (Lbl_Hurt != null)
            {
                Lbl_Hurt.text = string.Empty;
            }
            rootObj.SetActive(false);
        }
    }

    public bool ReSetInfo(int vIndex, uint vOrder, string vNick, uint vHurt)
    {
        if (rootObj != null)
        {
            index = vIndex;
            rootObj.name = string.Format("Order_{0}", index);
            rootObj.transform.localPosition = new Vector3(0, ORDER_POSY_INIT - ORDER_POSY_DISTANCE * index, 0);
            if (Lbl_Order != null)
            {
                Lbl_Order.text = string.Format(ConstString.SUPERMACY_RANK, vOrder);
            }
            if (Lbl_Nick != null)
            {
                Lbl_Nick.text = vNick;
            }
            if (Lbl_Hurt != null)
            {
                Lbl_Hurt.text = string.Format(ConstString.SUPERMACY_HURT, vHurt);
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
