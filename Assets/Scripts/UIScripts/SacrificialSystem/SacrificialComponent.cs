using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SacrificialComponent : MonoBehaviour
{
    public UISprite Spt_Sprite;
    public UISprite Spt_IconTexture;
    public UISprite Spt_QualitySprite;
    public UISprite Spt_SelectSprite;
    public UISprite Spt_SelectBack;
    public UIGrid Grd_QualityGrid;
    public UISprite Spt_StarSprite;
    public UILabel Lbl_Level;
    public delegate void TouchDeleget(SacrificialComponent comp);
    public TouchDeleget TouchEvent;
    public GameObject isEquip;
    public GameObject isBattle;
    public SacrificialData data;
    public UILabel lbl_Label_Step;
    List<GameObject> starList;
    public void MyStart(GameObject _uiRoot)
    {
        Spt_Sprite = _uiRoot.transform.FindChild("ItemBaseComp/Sprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = _uiRoot.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = _uiRoot.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_SelectSprite = _uiRoot.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Spt_SelectBack = _uiRoot.transform.FindChild("SelectSprite/Sprite").gameObject.GetComponent<UISprite>();
        Grd_QualityGrid = _uiRoot.transform.FindChild("QualityGrid").gameObject.GetComponent<UIGrid>();
        Spt_StarSprite = _uiRoot.transform.FindChild("QualityGrid/StarSprite").gameObject.GetComponent<UISprite>();
        Lbl_Level = _uiRoot.transform.FindChild("LevelGroup/Level").gameObject.GetComponent<UILabel>();
        isEquip = _uiRoot.transform.FindChild("hadEquip").gameObject;
        isBattle = _uiRoot.transform.FindChild("isBattle").gameObject;
        lbl_Label_Step = _uiRoot.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        starList = new List<GameObject>();
        Spt_StarSprite.gameObject.SetActive(false);
        UIEventListener.Get(this.gameObject).onClick = OnTouch;
    }
    public void OnTouch(GameObject btn)
    {
        if (TouchEvent != null)
            TouchEvent(this);
    }
    public void SetInfo(Soldier sd, bool isMax)
    {
        this.data = new SacrificialData(sd);
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(lbl_Label_Step,sd.StepNum);
        this.SetInfo(this.data, isMax);
    }
    public void SetInfo(Weapon wp, bool isMax)
    {
        this.data = new SacrificialData(wp);
        lbl_Label_Step.gameObject.SetActive(false);

        this.SetInfo(this.data, isMax);
    }

    public void SetInfo(SacrificialData vData, bool isMax)
    {
        if (vData == null)
            return;
        this.data = vData;
        if (this.Lbl_Level)
        {
            this.Lbl_Level.text = vData.level.ToString();
        }
        if (this.Grd_QualityGrid && this.Spt_StarSprite)
        {
            this.SetStar(vData.star, this.Grd_QualityGrid, this.Spt_StarSprite.gameObject);
        }
        if (isEquip && vData.MyBool && vData.type == SacrificialDataEnum.Equip)
        {
            this.isEquip.SetActive(true);
        }
        else
        {
            this.isEquip.SetActive(false);
        }
        if (vData.type == SacrificialDataEnum.SoldierOrange || vData.type == SacrificialDataEnum.SoldierRed)
        {
            lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
            this.lbl_Label_Step.text = CommonFunction.GetStepShow(lbl_Label_Step,vData.step);
        }
        else
            lbl_Label_Step.gameObject.SetActive(false);

        if (isBattle && vData.MyBool && (vData.type == SacrificialDataEnum.SoldierOrange || vData.type == SacrificialDataEnum.SoldierRed))
        {
            this.isBattle.SetActive(true);
        }
        else
        {
            this.isBattle.SetActive(false);
        }

        if (this.Spt_IconTexture)
        {
            CommonFunction.SetSpriteName(this.Spt_IconTexture, vData.icon);
        }
        if (this.Spt_QualitySprite && this.Spt_Sprite)
        {
            CommonFunction.SetQualitySprite(this.Spt_QualitySprite, vData.quality, this.Spt_Sprite);
        }
        if (!this.Spt_SelectSprite.gameObject.activeSelf && isMax)
            SetGray(true);
        else
            SetGray(false);
    }
    public void SetGray(bool isGray)
    {
        CommonFunction.SetGameObjectGray(this.gameObject, isGray);
        if(!isGray)
            this.Spt_SelectBack.color = new Color(1, 1, 1, 0.39f);
    }
    private void SetStar(int star, UIGrid vGrid, GameObject vStar)
    {
        if (vGrid == null || vStar == null)
            return;

        List<GameObject> ChildList = starList;

        if (star > ChildList.Count)
        {
            for (int i = 0; i < star; i++)
            {
                if (i < ChildList.Count)
                {
                    ChildList[i].SetActive(true);
                }
                else
                {
                    GameObject tmpStar = GameObject.Instantiate(vStar) as GameObject;
                    tmpStar.transform.parent = vGrid.transform;
                    tmpStar.layer = vStar.layer;
                    tmpStar.transform.localScale = vStar.transform.localScale;
                    tmpStar.SetActive(true);
                    starList.Add(tmpStar);
                }
            }
        }
        else
        {
            for (int i = 0; i < ChildList.Count; ++i)
            {
                if (ChildList[i] != null)
                {
                    if (i >= star)
                        ChildList[i].SetActive(false);
                    else
                        ChildList[i].SetActive(true);
                }
            }
        }
        vGrid.hideInactive = true;
        vGrid.Reposition();
    }
}
