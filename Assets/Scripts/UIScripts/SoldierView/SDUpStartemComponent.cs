using UnityEngine;
using System;
using System.Collections;
public class SDUpStartemComponent : MonoBehaviour
{
    public UInt64 uID;
    private UISprite ui_texture;
    private UISprite quality;
    private UISprite qualityBack;
    public UIGrid Grid_star;
    public GameObject mark;
    public UILabel Label_Level;
    public delegate void TouchDeleget(SDUpStartemComponent comp);
    public TouchDeleget TouchEvent;
    public UILabel lbl_Label_Step;
    public UISprite Battle;
    public void MyStart(GameObject root)
    {
        //base.AutoSetGoProperty(this, this.gameObject);
        ui_texture = root.transform.FindChild("ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        quality = root.transform.FindChild("ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        qualityBack = root.transform.FindChild("ArtifactComp/back").gameObject.GetComponent<UISprite>();
        Battle = root.transform.FindChild("Battle").gameObject.GetComponent<UISprite>();
        Label_Level = root.transform.FindChild("ArtifactComp/LevelBG/Label").gameObject.GetComponent<UILabel>();
        GameObject LessLv = root.transform.FindChild("LessLv").gameObject;
        LessLv.SetActive(false);
        lbl_Label_Step = root.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        mark = root.transform.FindChild("mark").gameObject;
        Grid_star = root.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
        UIEventListener.Get(this.gameObject).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }
    public void SetGray(bool isGray)
    {
        CommonFunction.SetGameObjectGray(this.gameObject, isGray);
    }
    public void SetInfo(Soldier sd,bool isGray)
    {
        if (sd == null) return;
        this.uID = sd.uId;

        if (ui_texture)
        {
            CommonFunction.SetSpriteName(ui_texture, sd.Att.Icon);
        }
        if (quality)
        {
            CommonFunction.SetQualitySprite(quality, sd.Att.quality, qualityBack);
        }
        if(this.Label_Level)
        {
            this.Label_Level.text = sd.Level.ToString();
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
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(this.lbl_Label_Step,sd.StepNum);
        this.Battle.gameObject.SetActive(CommonFunction.IsAlreadyBattle(this.uID));
        SetGray(isGray);
    }
}