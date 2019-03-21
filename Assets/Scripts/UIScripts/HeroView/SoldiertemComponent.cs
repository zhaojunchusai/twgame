using UnityEngine;
using System;
using System.Collections;
public class SoldiertemComponent : MonoBehaviour
{
    public UInt64 uid;
    private UILabel ui_Label_name;
    private UILabel ui_Label_count;
    private UISprite ui_texture;
    private UISprite quality;
    private UISprite quality_back;
    GameObject mark;
    private UISprite mark_career;
    public UIGrid Grid_star;
    public UILabel lbl_Label_Step;
    public delegate void TouchDeleget(SoldiertemComponent comp);
    public TouchDeleget TouchEvent;
    Soldier tmpSoldier;
    public void MyStart(GameObject root)
    {
        //base.AutoSetGoProperty(this, this.gameObject);
        ui_Label_name = root.transform.FindChild("Label_name").gameObject.GetComponent<UILabel>();
        ui_texture = root.transform.FindChild("equipt").gameObject.GetComponent<UISprite>();
        ui_Label_count = root.transform.FindChild("equipt/Label").gameObject.GetComponent<UILabel>();

        quality = root.transform.FindChild("equipt/quality").gameObject.GetComponent<UISprite>();
        quality_back = root.transform.FindChild("equipt/back").gameObject.GetComponent<UISprite>();
        mark = root.transform.FindChild("mark").gameObject;
        mark_career = root.transform.FindChild("mark_career").gameObject.GetComponent<UISprite>();
        Grid_star = root.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
        lbl_Label_Step = root.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        UIEventListener.Get(this.gameObject).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }
    public void SetInfo(Soldier sd)
    {
        if (sd == null) return;
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(this.lbl_Label_Step,sd.StepNum);
        this.uid = sd.uId;
        this.tmpSoldier = sd;
        if (ui_Label_name)
        {
            ui_Label_name.text = sd.Att.Name;
        }
        if (ui_Label_count)
        {
            ui_Label_count.text = sd.Level.ToString();
        }
        if (ui_texture)
        {
            CommonFunction.SetSpriteName(ui_texture,sd.Att.Icon);
        }
        if (quality)
        {
            CommonFunction.SetQualitySprite(quality, sd.Att.quality, quality_back);
        }
        if (Grid_star != null)
        {
            var tempList = Grid_star.GetChildList();
            for (int i = 0; i < tempList.Count; ++i)
            {
                GameObject star = tempList[i].FindChild("SelectSprite").gameObject;
                if (i < sd.Att.Star)
                {
                    star.SetActive(true);
                }
                else
                {
                    star.SetActive(false);
                }
            }
        }
        if(mark_career)
        {
            if (CommonFunction.IsAlreadyBattle(this.uid))
                this.mark_career.gameObject.SetActive(true);
            else
                this.mark_career.gameObject.SetActive(false);
        }
    }
}