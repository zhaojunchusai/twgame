using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GetEquipLocationEffectItem : MonoBehaviour
{
    [HideInInspector]public GameObject Lizi_R;
    [HideInInspector]public GameObject Lizi_Y;
    [HideInInspector]public GameObject Lizi_Z;
    [HideInInspector]public UISprite Spt_Light;
    [HideInInspector]public UILabel Lab_Name;
    [HideInInspector]public UISprite Spt_IconBG;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_Quality;
    [HideInInspector]public UISprite Spt_Type;
    [HideInInspector]public UISprite Spt_Chip;
    [HideInInspector]public UILabel Lab_Num;
    [HideInInspector]private bool _initialize = false;

	private void Initialize()
    {
        if (_initialize) 
            return;
        _initialize = true;
        Lizi_R = transform.FindChild("Lizi_R/Lizi_R").gameObject;
        Lizi_Y = transform.FindChild("Lizi_Y/Lizi_Y").gameObject;
        Lizi_Z = transform.FindChild("Lizi_Z/Lizi_Z").gameObject;
        Spt_Light = transform.FindChild("Item/Light").gameObject.GetComponent<UISprite>();
        Lab_Name = transform.FindChild("Item/Name").gameObject.GetComponent<UILabel>();
        Spt_IconBG = transform.FindChild("Item/IconBG").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Item/Icon").gameObject.GetComponent<UISprite>();
        Spt_Quality = transform.FindChild("Item/Quality").gameObject.GetComponent<UISprite>();
        Spt_Type = transform.FindChild("Item/Type").gameObject.GetComponent<UISprite>();
        Spt_Chip = transform.FindChild("Item/Chip").gameObject.GetComponent<UISprite>();
        Lab_Num = transform.FindChild("Item/Num").gameObject.GetComponent<UILabel>();
        Spt_Type.gameObject.SetActive(false);
        Lizi_R.gameObject.SetActive(false);
        Lizi_Y.gameObject.SetActive(false);
        Lizi_Z.gameObject.SetActive(false);
    }
    public void InitItem(Weapon data)
    {
        Initialize();
        SetEquipCommonInfo(data.Att.icon, data.Att.name);
        SetEquipQuality(data.Att.quality);
        Lab_Num.gameObject.SetActive(false);
        Spt_Chip.gameObject.SetActive(false);
        Spt_Type.gameObject.SetActive(false);
    }
    public void InitItem(PropsPackageCommonData data)
    {
        Initialize();
        SetEquipCommonInfo(data.icon, data.name);
        SetEquipQuality(data.quality);
        Spt_Chip.gameObject.SetActive(data.isEquipChip);
        Spt_Type.gameObject.SetActive(data.isSoldierChip);
        if (data.num < 2)
            Lab_Num.gameObject.SetActive(false);
        else
        {
            Lab_Num.gameObject.SetActive(true);
            Lab_Num.text = string.Format(ConstString.FORMAT_NUM_X,CommonFunction.GetTenThousandUnit(data.num));
        }
    }
    public void InitChipItem(fogs.proto.msg.ItemInfo chipData)
    {
        
        Initialize();
        ItemInfo data = ConfigManager.Instance.mItemData.GetItemInfoByID(chipData.id);
        SetEquipCommonInfo(data.icon, data.name);
        SetEquipQuality(data.quality);
        Lab_Num.gameObject.SetActive(true);
        Spt_Chip.gameObject.SetActive(true);
        Spt_Type.gameObject.SetActive(false);
        if (chipData.change_num < 1)
        {
            Lab_Num.text = "";
        }
        else
        {
            Lab_Num.text = string.Format(ConstString.FORMAT_NUM_X, chipData.change_num);
        }
    }

    private void SetEquipCommonInfo(string icon,string name)
    {
        CommonFunction.SetSpriteName(Spt_Icon, icon);
        Lab_Name.text = name;
    }
    private void SetEquipQuality(int quality)
    {
        SetEquipOBJState(Spt_Light.gameObject, true);
        CommonFunction.SetQualitySprite(Spt_Quality, quality, Spt_IconBG);
        switch(quality)
        {
            case 4:
                {
                    SetEquipOBJState(Lizi_R, false);
                    SetEquipOBJState(Lizi_Y, false);
                    SetEquipOBJState(Lizi_Z, true );
                    CommonFunction.SetSpriteName(Spt_Light, GlobalConst.SpriteName.RecruitSoldierQualityBgPurple);
                    break;
                }
            case 5:
                {
                    SetEquipOBJState(Lizi_R, false);
                    SetEquipOBJState(Lizi_Y, true );
                    SetEquipOBJState(Lizi_Z, false);
                    CommonFunction.SetSpriteName(Spt_Light, GlobalConst.SpriteName.RecruitSoldierQualityBgYellow);
                    break;
                }
            case 6:
                {
                    SetEquipOBJState(Lizi_R, true );
                    SetEquipOBJState(Lizi_Y, false);
                    SetEquipOBJState(Lizi_Z, false);
                    CommonFunction.SetSpriteName(Spt_Light, GlobalConst.SpriteName.RecruitSoldierQualityBgRed);
                    break;
                }
            default :
                {
                    SetEquipOBJState(Lizi_R, false);
                    SetEquipOBJState(Lizi_Y, false);
                    SetEquipOBJState(Lizi_Z, false);
                    SetEquipOBJState(Spt_Light.gameObject, false);
                    break;
                }
        }
    }
    private void SetEquipOBJState(GameObject go,bool state)
    {
        if (go.activeSelf != state)
            go.SetActive(state);
    }
    private void SetLabelValues()
    {

    }
    private void Uninitialize()
    {

    }
}
