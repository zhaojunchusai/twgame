using UnityEngine;
using System;
using System.Collections;
public class EQMateialtemComponent : MonoBehaviour
{
    public UInt64 uid;
    private UISprite ui_texture;
    private UISprite quality;
    private UISprite qualityBack;
    public GameObject mark;
    public UIGrid Grid_star;

    public delegate void TouchDeleget(EQMateialtemComponent comp);
    public TouchDeleget TouchEvent;

    void Awake()
    {
        MyStart(this.gameObject);
    }
    public void MyStart(GameObject root)
    {
        //base.AutoSetGoProperty(this, this.gameObject);
        ui_texture = root.transform.FindChild("ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        quality = root.transform.FindChild("ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        qualityBack = root.transform.FindChild("ArtifactComp/back").gameObject.GetComponent<UISprite>();
        mark = root.transform.FindChild("mark").gameObject;
        Grid_star = root.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();

        UIEventListener.Get(this.gameObject).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }
    public void SetInfo(Weapon wp)
    {
        if (wp == null) return;
        uid = wp.uId;
        if (ui_texture)
        {
            CommonFunction.SetSpriteName(ui_texture, wp.Att.icon);
        }
        if (quality)
        {
            CommonFunction.SetQualitySprite(quality, wp.Att.quality, qualityBack);
        }
        if (Grid_star != null)
        {
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