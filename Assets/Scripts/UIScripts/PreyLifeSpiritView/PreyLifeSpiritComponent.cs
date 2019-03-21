using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Common;
public class PreyLifeSpiritComponent : BaseComponent
{
    private UISprite Spt_UnuseTip;
    private UISprite Spt_Shading;
    private UISprite Spt_Frame;
    private UISprite Spt_Icon;
    private UILabel Lbl_Name;
    private Animation anim;
    private UISpriteAnimation gobj_PreyLifeSpiritEffect;
    private UISpriteAnimation gobj_LifeSpiritCollectEffect;
    private UISprite Spt_PreyLifeSpirit;
    private UISprite Spt_LifeSpiritCollect;
    private UISprite Spt_TypeMark;

    private PreyLifeSoulData mPreyLifeSoulData;
    public PreyLifeSoulData PreyLifeSoulData
    {
        get
        {
            return mPreyLifeSoulData;
        }
    }

    public bool IsPlayCollectAnim
    {
        get
        {
            if (anim.isPlaying || gobj_LifeSpiritCollectEffect.isPlaying)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_UnuseTip = mRootObject.transform.FindChild("Anim/BgGroup/UnuseTip").GetComponent<UISprite>();
        Spt_Shading = mRootObject.transform.FindChild("Anim/BaseComp/Shading").GetComponent<UISprite>();
        Spt_Frame = mRootObject.transform.FindChild("Anim/BaseComp/Frame").GetComponent<UISprite>();
        Spt_Icon = mRootObject.transform.FindChild("Anim/BaseComp/Icon").GetComponent<UISprite>();
        Lbl_Name = mRootObject.transform.FindChild("Anim/Name").GetComponent<UILabel>();
        anim = mRootObject.transform.FindChild("Anim").GetComponent<Animation>();
        gobj_PreyLifeSpiritEffect = mRootObject.transform.FindChild("PreyLifeSpiritEffect").GetComponent<UISpriteAnimation>();
        gobj_LifeSpiritCollectEffect = mRootObject.transform.FindChild("LifeSpiritCollectAtlasEffect").GetComponent<UISpriteAnimation>();
        Spt_PreyLifeSpirit = mRootObject.transform.FindChild("PreyLifeSpiritEffect").GetComponent<UISprite>();
        Spt_LifeSpiritCollect = mRootObject.transform.FindChild("LifeSpiritCollectAtlasEffect").GetComponent<UISprite>();
        Spt_TypeMark = mRootObject.transform.FindChild("TypeMark").GetComponent<UISprite>();
        gobj_LifeSpiritCollectEffect.gameObject.SetActive(false);
        gobj_PreyLifeSpiritEffect.gameObject.SetActive(false);
        anim.Stop();
    }

    public void UpdateCompInfo(PreyLifeSoulData data)
    {
        mPreyLifeSoulData = data;
        if (mPreyLifeSoulData == null)
        {
            Clear();
        }
        else
        {
            Spt_UnuseTip.enabled = false;
            CommonFunction.SetSpriteName(Spt_Icon, mPreyLifeSoulData.info.icon);
            CommonFunction.SetQualitySprite(Spt_Frame, mPreyLifeSoulData.info.quality, Spt_Shading);
            Lbl_Name.text = mPreyLifeSoulData.info.name;
            CommonFunction.SetLifeSpiritTypeMark(Spt_TypeMark, mPreyLifeSoulData.info.godEquip);
        }
    }

    public void PlayPreyAnim()
    {
        anim.Stop();
        gobj_LifeSpiritCollectEffect.gameObject.SetActive(false);
        gobj_PreyLifeSpiritEffect.gameObject.SetActive(true);
        gobj_PreyLifeSpiritEffect.RebuildSpriteList();
        gobj_PreyLifeSpiritEffect.ResetToBeginning();
        gobj_PreyLifeSpiritEffect.Play();
        Scheduler.Instance.AddTimer(0.3f, false, StopAnim);
    }

    public void PlayCollectAnim()
    {
        gobj_PreyLifeSpiritEffect.gameObject.SetActive(false);
        gobj_LifeSpiritCollectEffect.gameObject.SetActive(true);
        anim.Stop();
        anim.Play();
        gobj_LifeSpiritCollectEffect.RebuildSpriteList();
        gobj_LifeSpiritCollectEffect.ResetToBeginning();
        gobj_LifeSpiritCollectEffect.Play();
    }

    public void StopAnim()
    {
        gobj_PreyLifeSpiritEffect.gameObject.SetActive(false);
        gobj_LifeSpiritCollectEffect.gameObject.SetActive(false);
        gobj_PreyLifeSpiritEffect.Pause();
        gobj_PreyLifeSpiritEffect.Pause();
        anim.Stop();
    }

    public void Clear()
    {
        Spt_TypeMark.enabled = false;
        mPreyLifeSoulData = null;
        Spt_UnuseTip.enabled = true;
        Lbl_Name.text = string.Empty;
        Spt_Icon.spriteName = string.Empty;
        Spt_Frame.spriteName = string.Empty;
        Spt_Shading.spriteName = string.Empty;
    }
}
