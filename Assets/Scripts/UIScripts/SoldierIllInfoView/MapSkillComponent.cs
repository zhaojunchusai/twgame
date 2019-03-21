using UnityEngine;
using System;
using System.Collections;
public class MapSkillComponent : MonoBehaviour
{
    public Skill skill;
    public int Slot;
    public uint id;
    private UILabel ui_Label_name;
    private UILabel ui_Label_descript;
    private UILabel ui_Label_UnLock;
    private UISprite ui_texture;
    private UISprite ui_quality;
    private UISprite ui_quality_back;
    GameObject mark_canUpLv;
    GameObject Icon;
    public bool isSoldier = false;
    public delegate void TouchDeleget(MapSkillComponent comp);
    public TouchDeleget TouchEvent;
    public void MyStart(GameObject root)
    {
        //base.AutoSetGoProperty(this, this.gameObject);
        ui_Label_name = root.transform.FindChild("Label_name").gameObject.GetComponent<UILabel>();
        ui_Label_descript = root.transform.FindChild("Label_descript").gameObject.GetComponent<UILabel>();
        if(!isSoldier)
        {
            ui_Label_UnLock = root.transform.FindChild("UnLock").gameObject.GetComponent<UILabel>();
        }
        Icon = root.transform.FindChild("equipt").gameObject;
        ui_texture = root.transform.FindChild("equipt").gameObject.GetComponent<UISprite>();
        ui_quality = root.transform.FindChild("equipt/quality").gameObject.GetComponent<UISprite>();
        ui_quality_back = root.transform.FindChild("equipt/back").gameObject.GetComponent<UISprite>();
        //mark_equiped = this.gameObject.transform.FindChild("mark_equiped").gameObject;
        mark_canUpLv = root.transform.FindChild("mark_canUpLv").gameObject;

        UIEventListener.Get(this.gameObject).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }
    public void SetInfo(Skill sk)
    {
        if (sk == null) return;
        this.id = sk.Att.nId;
        this.skill = sk;
        if (ui_Label_name)
        {
            ui_Label_name.text = sk.Att.Name;
        }
        if (ui_Label_descript)
        {
            //string descript = sk.GetDescript(sk.Level);
            //string result = "";
            //int count = 0;
            //foreach (Char temp in descript)
            //{
            //    if (count > 12)
            //        break;
            //    int n = System.Text.Encoding.UTF8.GetByteCount(temp.ToString());
            //    if (n == 1)
            //    {
            //        if (count < 12)
            //        {
            //            result += temp;
            //            ++count;
            //        }
            //    }
            //    else
            //    {
            //        if (count < 11)
            //        {
            //            result += temp;
            //            count += 2;
            //        }
            //    }
            //}

            //result += "...";

            ui_Label_descript.text = sk.Level.ToString();
        }
        if (ui_texture)
        {
            CommonFunction.SetSpriteName(ui_texture,sk.Att.Icon);
        }
        if (ui_quality)
        {
            CommonFunction.SetQualitySprite(ui_quality, sk.Att.Quality, ui_quality_back);
        }
        if (this.skill.soldier != null)
        {
            if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierSkill))
            {
                CommonFunction.UpdateWidgetGray(ui_texture, true);
                CommonFunction.UpdateWidgetGray(ui_quality, true);
                mark_canUpLv.SetActive(false);
            }
            else
            {
                CommonFunction.UpdateWidgetGray(ui_texture, false);
                CommonFunction.UpdateWidgetGray(ui_quality, false);
            }
        }

        if (sk.islock)
        {
            if (this.ui_Label_UnLock)
            {
                this.mark_canUpLv.SetActive(false);
                this.ui_Label_UnLock.gameObject.SetActive(true);
                this.ui_Label_UnLock.text = string.Format(ConstString.UNLOCK_SKILL_DESCRIPT, sk.GetLockLv());
            }
        }
        else
        {
            if (this.ui_Label_UnLock)
                this.ui_Label_UnLock.gameObject.SetActive(false);
        }
        UIEventListener.Get(ui_texture.gameObject).onPress = (GameObject go, bool state) => 
        {
            HintManager.Instance.SeeDetail(go, state,this.skill.GetDescript(this.skill.Level));
        };
    }
}
