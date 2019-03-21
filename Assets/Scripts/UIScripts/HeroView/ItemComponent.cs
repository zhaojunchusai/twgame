using UnityEngine;
using System;
using System.Collections;

public class ItemComponent : MonoBehaviour
{
    public UInt64 uid;
    private UILabel ui_Label_name;
    private UILabel ui_Label_lv;
    private UILabel ui_Label_lock;

    private GameObject SkillDescript;
    private UILabel skillDescript;
    private GameObject AttDescript;
    private UISprite ui_texture;
    private UISprite Spt_quality;
    private UISprite Quality_Back;
    private UIGrid Grid_star;
    private UILabel solLabel;

    GameObject mark_equiped;
    public delegate void TouchDeleget(ItemComponent comp);
    public TouchDeleget TouchEvent;

    public void MyStart(GameObject root)
    {
        ui_Label_name = root.transform.FindChild("Label_name").gameObject.GetComponent<UILabel>();
        ui_Label_lv = root.transform.FindChild("equipt/Label").gameObject.GetComponent<UILabel>();
        ui_Label_lock = root.transform.FindChild("equipt/Lock").gameObject.GetComponent<UILabel>();
        solLabel = root.transform.FindChild("Label_SolName").gameObject.GetComponent<UILabel>();
        ui_texture = root.transform.FindChild("equipt").gameObject.GetComponent<UISprite>();
        Spt_quality = root.transform.FindChild("equipt/quality").gameObject.GetComponent<UISprite>();
        Quality_Back = root.transform.FindChild("equipt/back").gameObject.GetComponent<UISprite>();
        mark_equiped = root.transform.FindChild("mark").gameObject;
        SkillDescript = root.transform.FindChild("AttributeComparisonGroup/BeforeSkill").gameObject;
        skillDescript = root.transform.FindChild("AttributeComparisonGroup/BeforeSkill/Label").gameObject.GetComponent<UILabel>();
        AttDescript = root.transform.FindChild("AttributeComparisonGroup/BeforeGroup").gameObject;
        Grid_star = root.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
        UIEventListener.Get(this.gameObject).onClick = OnTouch;
        solLabel.text = "";
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }
    public void SetInfo(Weapon wp)
    {
        if (wp == null) return;
        this.uid = wp.uId;
        if (ui_Label_name)
        {
            ui_Label_name.text = wp.Att.name;
            if(wp.isEquiped)
            {
                if(wp.Att.godEquip == 2)
                {
                    Soldier mySoldier = PlayerData.Instance._SoldierDepot._soldierList.Find
                        (
                        (Soldier sd) =>
                        {
                            if (sd == null) return false;
                            return sd._equippedDepot._EquiptList.Contains(wp);
                        }
                        );
                    if (mySoldier != null && solLabel)
                    {
                        solLabel.text = mySoldier.Att.Name;
                    }
                }
                else
                {
                    solLabel.text = "";
                }
            }
            else
            {
                solLabel.text = "";
            }
        }
        if (skillDescript)
        {
            if(wp._Skill != null)
            {
                this.AttDescript.SetActive(false);
                this.SkillDescript.SetActive(true);
                skillDescript.text = string.Format(ConstString.ARTFACT_SKILL_NAME, wp._skill.Att.Name);
            }
            else
            {
                this.AttDescript.SetActive(true);
                this.SkillDescript.SetActive(false);
                CommonFunction.SetAttributeGroup(this.AttDescript,wp.InfoAttribute,null,false);
            }
        }
        if (ui_texture)
        {
            CommonFunction.SetSpriteName(ui_texture, wp.Att.icon);
            CommonFunction.UpdateWidgetGray(ui_texture,wp.IsLock);
        }
        if (Spt_quality)
        {
            CommonFunction.SetQualitySprite(Spt_quality, wp.Att.quality, Quality_Back);
            CommonFunction.UpdateWidgetGray(Spt_quality, wp.IsLock);
        }
        if (ui_Label_lv)
        {
            ui_Label_lv.text = wp.Level.ToString();
        }
        if(mark_equiped)
        {
            if (wp.isEquiped)
                mark_equiped.SetActive(true);
            else
                mark_equiped.SetActive(false);
        }
        if (ui_Label_lock && wp.IsLock)
        {
            if(wp.godEquipLockInfo.vipLv > 0)
            {
                ui_Label_lock.text = string.Format("VIP{0}{1}", wp.godEquipLockInfo.vipLv, ConstString.VIP_XX_LOCK);
            }
            else
            {
                if (wp.godEquipLockInfo.level > 0)
                    ui_Label_lock.text = string.Format("{0}{1}", wp.godEquipLockInfo.level, ConstString.XXX_LOCK);
                else if(wp.godEquipLockInfo.GateID > 0)
                {
                    ui_Label_lock.text = ConstString.MOVE_LCOK;
                }
            }

        }
        else
        {
            ui_Label_lock.text = "";
        }
        if (Grid_star)
        {
           
            if (wp == null)
            {
                Grid_star.gameObject.SetActive(false);
                return;
            }
            if (wp.Att.godEquip != 2)
                return;
            else
            {
                Grid_star.gameObject.SetActive(true);
            }
            var tempList = Grid_star.GetChildList();
            for (int i = 0; i < tempList.Count; ++i)
            {
                GameObject star = tempList[i].FindChild("SelectSprite").gameObject;
                if (i < wp.Att.star)
                {
                    star.SetActive(true);
                }
                else
                {
                    star.SetActive(false);
                }
            }
        }

       
    }
}

