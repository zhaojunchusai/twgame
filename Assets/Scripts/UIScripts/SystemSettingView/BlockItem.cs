using UnityEngine;
using System.Collections.Generic;

public class BlockItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_ItemBGSprite;
    [HideInInspector]public UISprite Spt_IconBGSprite;
    [HideInInspector]public UISprite Spt_IconSprite;
    [HideInInspector]public UISprite Spt_IconFreamSprite;
    [HideInInspector]public UISprite Spt_LevelSprite;
    [HideInInspector]public UILabel Lbl_LevelLabel;
    [HideInInspector]public UILabel Lbl_NameLabel;
    [HideInInspector]public UILabel Lbl_FightPowerLabel;
    [HideInInspector]public UISprite Spt_FightPowerSprite;
    [HideInInspector]public UILabel Lbl_CorpsLabel;
    [HideInInspector]public UISprite Spt_CorpsSprite;
    [HideInInspector]public UISprite Spt_VIPSprite;
    [HideInInspector]public UILabel Lbl_VIPLabel;
    [HideInInspector]public UIButton Btn_RemoveButton;
    [HideInInspector]public UISprite Spt_BtnRemoveButtonRemoveBGSprite;
    [HideInInspector]public UISprite Spt_BtnRemoveButtonRemoveFGSprite;

    public void Initialize()
    {
        //Spt_ItemBGSprite = transform.FindChild("ItemBGSprite").gameObject.GetComponent<UISprite>();
        //Spt_IconBGSprite = transform.FindChild("IconGroup/IconBGSprite").gameObject.GetComponent<UISprite>();
        //Spt_IconSprite = transform.FindChild("IconGroup/IconSprite").gameObject.GetComponent<UISprite>();
        //Spt_IconFreamSprite = transform.FindChild("IconGroup/IconFreamSprite").gameObject.GetComponent<UISprite>();
        //Spt_LevelSprite = transform.FindChild("IconGroup/LevelSprite").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabel = transform.FindChild("IconGroup/LevelLabel").gameObject.GetComponent<UILabel>();
        Lbl_NameLabel = transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_FightPowerLabel = transform.FindChild("FightPowerLabel").gameObject.GetComponent<UILabel>();
        //Spt_FightPowerSprite = transform.FindChild("FightPowerSprite").gameObject.GetComponent<UISprite>();
        Lbl_CorpsLabel = transform.FindChild("CorpsLabel").gameObject.GetComponent<UILabel>();
        //Spt_CorpsSprite = transform.FindChild("CorpsSprite").gameObject.GetComponent<UISprite>();
        //Spt_VIPSprite = transform.FindChild("VIPSprite").gameObject.GetComponent<UISprite>();
        Lbl_VIPLabel = transform.FindChild("VIPLabel").gameObject.GetComponent<UILabel>();
        Btn_RemoveButton = transform.FindChild("RemoveButton").gameObject.GetComponent<UIButton>();
        //Spt_BtnRemoveButtonRemoveBGSprite = transform.FindChild("RemoveButton/RemoveBGSprite").gameObject.GetComponent<UISprite>();
        //Spt_BtnRemoveButtonRemoveFGSprite = transform.FindChild("RemoveButton/RemoveFGSprite").gameObject.GetComponent<UISprite>();
        //SetLabelValues();

        UIEventListener.Get(Btn_RemoveButton.gameObject).onClick = ButtonEvent_RemoveButton;
    }

    void Awake()
    {
        Initialize(); 
    }

    public void SetLabelValues()
    {
        Lbl_LevelLabel.text = "0";
        Lbl_NameLabel.text = "角色名";
        Lbl_FightPowerLabel.text = "0";
        Lbl_CorpsLabel.text = "大西瓜啊";
        Lbl_VIPLabel.text = "0";
    }

    public void ButtonEvent_RemoveButton(GameObject btn)
    {

    }

    public void Uninitialize()
    {

    }

    public void UpdateItemInfo() { 
    
    }

}
