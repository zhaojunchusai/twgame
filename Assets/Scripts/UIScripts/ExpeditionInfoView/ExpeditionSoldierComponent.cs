using UnityEngine;
using System.Collections.Generic;
public class ExpeditionSoldierComponent : LineupItemComponent
{
    public UISprite Spt_DeadMarkSprite;
    //public UISprite Spt_SoldierType;
    public UISprite Spt_LowLevelSprite;
    private GameObject SoldierTypeGroup;
    private UISprite Spt_SoldierType;
    private UILabel Lbl_SoldierType;
    private int[] SeatPos = { 3, 6, 5 };
    private int[] Carrer = { 2, 0, 1, 4 };

    private bool isDead = false;
    public bool IsDead 
    {
        get 
        {
            return isDead;
        }
        set 
        {
            isDead = value;
            IsEnable = !IsDead;
            Spt_DeadMarkSprite.enabled = isDead;
        }
    }

    public bool IsLowLevel 
    {
        get 
        {
            return Spt_LowLevelSprite.enabled;
        }
        set 
        {
            Spt_LowLevelSprite.enabled = value;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_DeadMarkSprite = mRootObject.transform.FindChild("DeadMarkSprite").gameObject.GetComponent<UISprite>();
        Spt_SoldierType = mRootObject.transform.FindChild("SoldierTypeGroup/SoldierTypeBG").gameObject.GetComponent<UISprite>();
        SoldierTypeGroup = mRootObject.transform.FindChild("SoldierTypeGroup").gameObject;
        Lbl_SoldierType = mRootObject.transform.FindChild("SoldierTypeGroup/SoldierTypeLabel").gameObject.GetComponent<UILabel>();
        Spt_LowLevelSprite = mRootObject.transform.FindChild("LowLevelSprite").gameObject.GetComponent<UISprite>();
    }

    public void UpdateCompInfo(Soldier att, int num = 0) 
    {
        if (att == null) 
        {
            Clear();
            return;
        }
        base.UpdateInfo(att);
        base.UpdateNum(num);
        UpdateSoldierType(att.Att.Career);

        
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
