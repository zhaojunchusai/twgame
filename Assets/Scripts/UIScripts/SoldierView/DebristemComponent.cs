using UnityEngine;
using System;
using System.Collections;
public class DebristemComponent : MonoBehaviour
{
    public uint id;
    private UILabel ui_Label_name;
    private UILabel ui_Label_count;
    private UISprite ui_quality;
    private UISprite quanlity_back;
    private UISprite ui_texture;
    private UISprite mark_career;

    GameObject CanComb;
    GameObject mark;
    public delegate void TouchDeleget(DebristemComponent comp);
    public TouchDeleget TouchEvent;

    public void MyStart(GameObject root)
    {
        ui_Label_name = root.transform.FindChild("Label_name").gameObject.GetComponent<UILabel>();
        ui_Label_count = root.transform.FindChild("count").gameObject.GetComponent<UILabel>();
        ui_texture = root.transform.FindChild("equipt").gameObject.GetComponent<UISprite>();
        ui_quality = root.transform.FindChild("equipt/quality").gameObject.GetComponent<UISprite>();
        quanlity_back = root.transform.FindChild("equipt/back").gameObject.GetComponent<UISprite>();
        CanComb = root.transform.FindChild("mark_canUpLv").gameObject;
        mark = root.transform.FindChild("mark").gameObject;
        mark_career = root.transform.FindChild("mark_career").gameObject.GetComponent<UISprite>();

        UIEventListener.Get(this.gameObject).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }
    public void SetInfo(Debris sd)
    {
        if (sd == null) return;
        this.id = sd.Att.id;
        if (ui_Label_name)
        {
            ui_Label_name.text = sd.Att.name;
        }
        if (ui_Label_count)
        {
            ui_Label_count.text = string.Format("{0}/{1}", sd.count, sd.Att.compound_count);
            if (sd.enabelCompound() == DebrisCheck.Ok)
                ui_Label_count.color = Color.green;
            else
                ui_Label_count.color = Color.white;
        }
        if (ui_texture)
        {
            CommonFunction.SetSpriteName(ui_texture,sd.Att.icon);
        }
        if (ui_quality)
        {
            CommonFunction.SetQualitySprite(ui_quality, sd.Att.quality, quanlity_back);
        }
        if(CanComb)
        {
            if (sd.enabelCompound() == DebrisCheck.Ok)
                CanComb.SetActive(true);
            else
                CanComb.SetActive(false);
        }
        if(mark_career)
        {
            //Soldier temp = Soldier.createByID(sd.Att.compound_Id);
            //if (temp != null)
            //{
            //    mark_career.gameObject.SetActive(true);
            //    CommonFunction.SetSpriteName(mark_career, CommonFunction.GetSoldierTypeIcon(temp.Att.Career));
            //}
            //else
                mark_career.gameObject.SetActive(false);
        }
    }

}