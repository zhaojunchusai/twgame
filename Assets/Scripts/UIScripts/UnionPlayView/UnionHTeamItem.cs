using UnityEngine;
using System;
using System.Collections;

public class UnionHTeamItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_NumBGSprite;
    [HideInInspector]public UILabel Lbl_NameLabel;
    [HideInInspector]public UISprite Spt_ItemBGSprite;
    [HideInInspector]public UILabel Lbl_CombatPowerLabel;
    [HideInInspector]public UILabel Lbl_OrderLabel;
    [HideInInspector]public UILabel Lbl_LevelLabel;

    public void Initialize()
    {
        Spt_NumBGSprite = transform.FindChild("NumBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_NameLabel = transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Spt_ItemBGSprite = transform.FindChild("ItemBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_CombatPowerLabel = transform.FindChild("CombatPowerLabel").gameObject.GetComponent<UILabel>();
        Lbl_OrderLabel = transform.FindChild("OrderLabel").gameObject.GetComponent<UILabel>();
        Lbl_LevelLabel = transform.FindChild("LevelLabel").gameObject.GetComponent<UILabel>();
        SetLabelValues();

    }

    void Awake()
    {
        Initialize(); 
    }

    public void SetLabelValues()
    {
        Lbl_NameLabel.text = "";
        Lbl_LevelLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, 0);
        Lbl_CombatPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT, 0);
        Lbl_OrderLabel.text = "0";
    }

    public void Uninitialize()
    {

    }

    public void UpdateItem(string name,int num,int level,int combatPower)
    {
        Lbl_NameLabel.text = name;
        Lbl_CombatPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT, CommonFunction.GetTenThousandUnit(combatPower));
        Lbl_OrderLabel.text = num.ToString();
        Lbl_LevelLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, level);
    }

    public void DestroyItem()
    {
        GameObject.Destroy(this.gameObject);
    }
}
