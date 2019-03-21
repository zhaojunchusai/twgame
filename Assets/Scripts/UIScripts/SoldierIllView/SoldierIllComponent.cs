using UnityEngine;
using System;
using System.Collections;
public class SoldierIllComponent : MonoBehaviour
{
    public GameObject _uiRoot;
    public UISprite Spt_mark;
    public UILabel Lbl_Label_name;
    public UISprite Spt_equipt;
    public UISprite Spt_quality;
    public UISprite Spt_back;
    public Soldier soldier;
    public delegate void TouchDeleget(SoldierIllComponent comp);
    public TouchDeleget TouchEvent;

    public void MyStart(GameObject root)
    {
        if (root == null)
            return;

        this._uiRoot = root;
        Spt_mark = this._uiRoot.transform.FindChild("mark").gameObject.GetComponent<UISprite>();
        Lbl_Label_name = this._uiRoot.transform.FindChild("Label_name").gameObject.GetComponent<UILabel>();
        Spt_equipt = this._uiRoot.transform.FindChild("equipt").gameObject.GetComponent<UISprite>();
        Spt_quality = this._uiRoot.transform.FindChild("equipt/quality").gameObject.GetComponent<UISprite>();
        Spt_back = this._uiRoot.transform.FindChild("equipt/back").gameObject.GetComponent<UISprite>();

        UIEventListener.Get(this.gameObject).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }

    public void SetInfo(Soldier sd)
    {
        if (sd == null)
            return;
        this.soldier = sd;
        if (Lbl_Label_name)
        {
            Lbl_Label_name.text = sd.Att.Name;
        }
        if (Spt_equipt)
        {
            CommonFunction.SetSpriteName(Spt_equipt, sd.Att.Icon);
        }
        if (Spt_quality)
        {
            CommonFunction.SetQualitySprite(Spt_quality, sd.Att.quality, Spt_back);
        }
        if(Spt_mark)
        {
            if (!sd.IsNew)
                Spt_mark.gameObject.SetActive(false);
            else
                Spt_mark.gameObject.SetActive(true);
        }
    }
}