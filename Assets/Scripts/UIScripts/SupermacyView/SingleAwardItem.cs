using UnityEngine;
using System.Collections;

/// <summary>
/// 单个奖励
/// </summary>
public class SingleAwardItem
{
    private const int INIT_POSY = -60;
    private const int DIS_POSY = 90;
    private const int INIT_POSX = -250;
    private const int DIS_POSX = 100;
    private const int LIMIT_LINECOUNT = 6;


    public GameObject Obj_Root;
    public UISprite Spt_Frame;
    public UISprite Spt_Quality;
    public UISprite Spt_Icon;
    public UISprite Spt_Sign;
    public UILabel Lbl_Value;


    private uint itemID;


    public SingleAwardItem(Transform vSourceTrans)
    {
        SetComponent(vSourceTrans);
    }

    private void SetComponent(Transform vSourceTrans)
    {
        if (vSourceTrans != null)
        {
            Obj_Root = CommonFunction.InstantiateObject(vSourceTrans.gameObject, vSourceTrans.parent);
            if (Obj_Root != null)
            {
                Spt_Frame = Obj_Root.transform.FindChild("Frame").gameObject.GetComponent<UISprite>();
                Spt_Quality = Obj_Root.transform.FindChild("Quality").gameObject.GetComponent<UISprite>();
                Spt_Icon = Obj_Root.transform.FindChild("Icon").gameObject.GetComponent<UISprite>();
                Spt_Sign = Obj_Root.transform.FindChild("Sign").gameObject.GetComponent<UISprite>();
                Lbl_Value = Obj_Root.transform.FindChild("Value").gameObject.GetComponent<UILabel>();
                UIEventListener.Get(Obj_Root.gameObject).onPress = ButtonEvent_PressItem;
                InitInfo();
            }
        }
    }

    public void InitInfo()
    {
        if (Obj_Root != null)
        {
            if (Spt_Quality != null)
            {
                Spt_Quality.gameObject.SetActive(false);
            }
            if (Spt_Icon != null)
            {
                Spt_Icon.gameObject.SetActive(false);
            }
            if (Spt_Sign != null)
            {
                Spt_Sign.gameObject.SetActive(false);
            }
            if (Lbl_Value != null)
            {
                Lbl_Value.text = string.Empty;
            }
            Obj_Root.SetActive(false);
        }
        itemID = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vIndex">索引</param>
    /// <param name="vInfo">獎勵物品信息</param>
    public void ReSetValue(int vIndex, CommonItemData vInfo)
    {
        if ((Obj_Root != null) && (vInfo != null))
        {
            itemID = vInfo.ID;
            CommonFunction.SetQualitySprite(Spt_Frame, (int)vInfo.Quality, Spt_Quality);
            CommonFunction.SetSpriteName(Spt_Icon, vInfo.Icon);
            if (Spt_Quality != null)
            {
                Spt_Quality.gameObject.SetActive(true);
                Spt_Frame.gameObject.SetActive(true);
            }
            if (Spt_Icon != null)
            {
                Spt_Icon.gameObject.SetActive(true);
            }
            if (Spt_Sign != null)
            {
                Spt_Sign.gameObject.SetActive(false);
            }
            if (vInfo.Type == IDType.Prop)
            {
                ItemInfo tmpItem = ConfigManager.Instance.mItemData.GetItemInfoByID(itemID);
                if (tmpItem != null)
                {
                    if (tmpItem.type == (int)ItemTypeEnum.EquipChip)
                    {
                        if (Spt_Sign != null)
                        {
                            CommonFunction.SetSpriteName(Spt_Sign, "Cmn_Icon_sui");
                            Spt_Sign.gameObject.SetActive(true);
                        }
                    }
                    else if (tmpItem.type == (int)ItemTypeEnum.SoldierChip)
                    {
                        if (Spt_Sign != null)
                        {
                            CommonFunction.SetSpriteName(Spt_Sign, "Cmn_Icon_hun");
                            Spt_Sign.gameObject.SetActive(true);
                        }
                    }
                }
            }
            if (Lbl_Value != null)
            {
                Lbl_Value.text = vInfo.Num.ToString();
            }

            Obj_Root.transform.localPosition = new Vector3(INIT_POSX + (vIndex % LIMIT_LINECOUNT) * DIS_POSX, INIT_POSY - (vIndex / LIMIT_LINECOUNT) * DIS_POSY, 0);
            Obj_Root.SetActive(true);
        }
    }

    private void ButtonEvent_PressItem(GameObject go, bool press)
    {
        HintManager.Instance.SeeDetail(go, press, itemID);
    }
}