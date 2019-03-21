using UnityEngine;
using System.Collections.Generic;
public class OwnSoldierLineupComponent : LineupItemComponent
{
    private GameObject SoldierTypeGroup;
    private UISprite Spt_SoldierType;
    private UILabel Lbl_SoldierType;

    private int[] SeatPos = { 3, 6, 5 };
    private int[] Carrer = { 2, 0, 1, 4 };

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_SoldierType = mRootObject.transform.FindChild("SoldierTypeGroup/SoldierTypeBG").gameObject.GetComponent<UISprite>();
        SoldierTypeGroup = mRootObject.transform.FindChild("SoldierTypeGroup").gameObject;
        Lbl_SoldierType = mRootObject.transform.FindChild("SoldierTypeGroup/SoldierTypeLabel").gameObject.GetComponent<UILabel>();
    }

    public void UpdateCompInfo(Soldier soldier) 
    {
        base.UpdateInfo(soldier);
        UpdateSoldierType(soldier.Att.Career);
    }
    private void UpdateSoldierType(int type)
    {
        this.Lbl_SoldierType.text = (this.SeatPos[this.soldierAtt.Att.Stance - 1] * 10 + this.Carrer[this.soldierAtt.Att.Career - 1]).ToString();
        //CommonFunction.SetSpriteName(Spt_SoldierType, CommonFunction.GetSoldierTypeIcon(type));
    }

    public override void Clear()
    {
        base.Clear();
    }
}
