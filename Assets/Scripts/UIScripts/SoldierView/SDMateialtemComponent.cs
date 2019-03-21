using UnityEngine;
using System;
using System.Collections;
public class SDMateialtemComponent : MonoBehaviour
{
    public UInt64 uid;
    private UISprite ui_texture;
    private UISprite quality;
    private UISprite qualityBack;
    private UISprite mark_HadUp;
    public GameObject mark;
    public UIGrid Grid_star;
    public UILabel Level;
    public UILabel lbl_Label_Step;
    public delegate void TouchDeleget(SDMateialtemComponent comp);
    public TouchDeleget TouchEvent;

    public void MyStart(GameObject root)
    {
        ui_texture = root.transform.FindChild("ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        quality = root.transform.FindChild("ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        qualityBack = root.transform.FindChild("ArtifactComp/back").gameObject.GetComponent<UISprite>();
        mark_HadUp = root.transform.FindChild("HadUp").gameObject.GetComponent<UISprite>();
        mark = root.transform.FindChild("mark").gameObject;
        Grid_star = root.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
        Level = root.transform.FindChild("ArtifactComp/LevelBG/Label").gameObject.GetComponent<UILabel>();
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
        uid = sd.uId;
        if (ui_texture)
        {
            CommonFunction.SetSpriteName(ui_texture, sd.Att.Icon);
        }
        if (quality)
        {
            CommonFunction.SetQualitySprite(quality, sd.Att.quality, qualityBack);
        }
        if (Level)
        {
            Level.text = sd.Level.ToString();
        }
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(this.lbl_Label_Step,sd.StepNum);
        if (mark_HadUp)
        {
            if (CommonFunction.IsAlreadyBattle(sd.uId))
                mark_HadUp.gameObject.SetActive(true);
            else
                mark_HadUp.gameObject.SetActive(false);

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
    }
}