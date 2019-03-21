using UnityEngine;
using System;
using System.Collections;

public class RecruitResultItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_Light;
    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public UISprite Spt_IconBG;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_Quality;
    [HideInInspector]public UISprite Spt_Type;
    [HideInInspector]public UISprite Spt_Chip;
    [HideInInspector]public UILabel Lbl_Num;
    [HideInInspector]public GameObject Gobj_R;
    [HideInInspector]public GameObject Gobj_Y;
    [HideInInspector]public GameObject Gobj_Z;

    private bool _initialized = false;
    private void Initialize()
    {
        if(_initialized)
            return;
        _initialized = true;
        Spt_Light = transform.FindChild("Item/Light").gameObject.GetComponent<UISprite>();
        Spt_BG = transform.FindChild("Item/BG").gameObject.GetComponent<UISprite>();
        Lbl_Name = transform.FindChild("Item/Name").gameObject.GetComponent<UILabel>();
        Spt_IconBG = transform.FindChild("Item/IconBG").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Item/Icon").gameObject.GetComponent<UISprite>();
        Spt_Quality = transform.FindChild("Item/Quality").gameObject.GetComponent<UISprite>();
        Spt_Type = transform.FindChild("Item/Type").gameObject.GetComponent<UISprite>();
        Spt_Chip = transform.FindChild("Item/Chip").gameObject.GetComponent<UISprite>();
        Lbl_Num = transform.FindChild("Item/Num").gameObject.GetComponent<UILabel>();
        Gobj_R = transform.FindChild("Lizi_R").gameObject;
        Gobj_Y = transform.FindChild("Lizi_Y").gameObject;
        Gobj_Z = transform.FindChild("Lizi_Z").gameObject;
        SetLabelValues();

    }

    public void InitItem(SoldierAttributeInfo info)
    {
        Initialize();
        SetCommonInfo(info.Icon, info.Name);
        SetQuality(info.quality);
        Lbl_Num.gameObject.SetActive(false);
        Spt_Chip.gameObject.SetActive(false);
        SetAttDistance(info.Career);
    }

    public void InitItem(fogs.proto.msg.ItemInfo info)
    {
        Initialize();
        ItemInfo data = ConfigManager.Instance.mItemData.GetItemInfoByID(info.id);
        SetCommonInfo(data.icon,data.name);
        SetQuality(data.quality);
        Lbl_Num.gameObject.SetActive(true);
        Lbl_Num.text = string.Format(ConstString.FORMAT_NUM_X,info.change_num);
        Spt_Type.gameObject.SetActive(false);
        Spt_Chip.gameObject.SetActive(true);
    }

    private void SetAttDistance(int dis)
    {
        Spt_Type.gameObject.SetActive(true);
        CommonFunction.SetSpriteName(Spt_Type, CommonFunction.GetSoldierTypeIcon(dis));
    }

    private void SetCommonInfo(string icon,string name)
    {
        CommonFunction.SetSpriteName(Spt_Icon, icon);
        Lbl_Name.text = name;
    }

    private void SetQuality(int quality)
    {
        CommonFunction.SetQualitySprite(Spt_Quality, quality, Spt_IconBG);
        switch (quality)
        {
            case 4:
                {
                    SetObjState(Gobj_R, false);
                    SetObjState(Gobj_Y, false);
                    SetObjState(Gobj_Z, true);
                    CommonFunction.SetSpriteName(Spt_Light, GlobalConst.SpriteName.RecruitSoldierQualityBgPurple);
                    break;
                }
            case 5:
                {
                    SetObjState(Gobj_R, false);
                    SetObjState(Gobj_Y, true);
                    SetObjState(Gobj_Z, false);
                    CommonFunction.SetSpriteName(Spt_Light, GlobalConst.SpriteName.RecruitSoldierQualityBgYellow);
                    break;
                }
            case 6:
                {
                    SetObjState(Gobj_R, true);
                    SetObjState(Gobj_Y, false);
                    SetObjState(Gobj_Z, false);
                    CommonFunction.SetSpriteName(Spt_Light, GlobalConst.SpriteName.RecruitSoldierQualityBgRed);
                    break;
                }
            default:
                {
                    SetObjState(Gobj_R, false);
                    SetObjState(Gobj_Y, false);
                    SetObjState(Gobj_Z, false);
                    SetObjState(Spt_Light.gameObject, false);
                    break;
                }

        }
    }

    private void SetObjState(GameObject go,bool state)
    {
        if(go.activeSelf != state)
            go.SetActive(state);
    }

    public void SetLabelValues()
    {
        Lbl_Name.text = "";
    }

    public void Uninitialize()
    {

    }


}
