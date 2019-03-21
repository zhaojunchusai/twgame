using UnityEngine;
using System;
using System.Collections;

public class GetSpecialItemView
{
    public const string UIName ="GetSpecialItemView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_GetSpecialItemView;
    public GameObject Gobj_GetSoldier;
    public GameObject Gobj_Single;
    public UISprite Spt_Mask;
   
    public UIButton Btn_BackBtn;
    public UISprite Spt_BtnBackBtnBG;
    public UIGrid StarGrid;
    public GameObject OnesEffect;
    public TweenAlpha Talpha_OnesSpt1;
    public TweenScale Tscale_OnesSpt1;
    public TweenScale Tscale_Oneslizi;
    public TweenRotation Trotate_Oneslizi;
    public TweenAlpha Talpha_OnesSpt2;
    public TweenScale TScale_OnesSpt2;
    public GameObject OnesOne;
    public GameObject OnesOne_R;
    public GameObject OnesOne_Z;
    public GameObject OnesOne_Y;
    public GameObject EffectGo;

    public UITexture OnesOne_1;
    public UITexture OnesOne_2;
    public UITexture OnesOne_3;
    public UITexture OnesOne_4;
    public GameObject Gobj_SingleName;
    public UISprite Spt_SingleLight;
    public UILabel Lbl_SingleName;
    public UILabel Lbl_SingleNum;
    public UISprite Spt_SingleType;
    public UISprite Spt_SingleQuality;
    public UISprite Spt_SingleChip;
    public GameObject Gobj_SingleEffRed;
    public GameObject Gobj_SingleEffYellow;
    public GameObject Gobj_SingleEffPurple;
    public GameObject Gobj_BGMask;
    public UILabel Lab_Title;

    public GameObject Star;
    public GameObject Star_1;
    public TweenScale TweenStar_1;
    public GameObject Star_2;
    public TweenScale TweenStar_2;
    public GameObject Star_3;
    public TweenScale TweenStar_3;
    public GameObject Star_4;
    public TweenScale TweenStar_4;
    public GameObject Star_5;
    public TweenScale TweenStar_5;
    public GameObject Star_6;
    public TweenScale TweenStar_6;

    public TweenScale SingleGobj;
    public TweenScale SingleItem;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/GetSpecialItemView");
        SingleItem = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Item").gameObject.GetComponent<TweenScale>();
        SingleGobj = _uiRoot.transform.FindChild("gobj_GetSoldier/gobj_Single").gameObject.GetComponent<TweenScale>();
        UIPanel_GetSpecialItemView = _uiRoot.GetComponent<UIPanel>();
        Gobj_GetSoldier = _uiRoot.transform.FindChild("gobj_GetSoldier").gameObject;
        Gobj_Single = _uiRoot.transform.FindChild("gobj_GetSoldier/gobj_Single").gameObject;
        Spt_Mask = _uiRoot.transform.FindChild("gobj_GetSoldier/BG").gameObject.GetComponent<UISprite>();
        Gobj_BGMask = _uiRoot.transform.FindChild("BGMask").gameObject;
        EffectGo = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect").gameObject;
        OnesEffect = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones").gameObject;
        Talpha_OnesSpt1 = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/Sprite1").gameObject.GetComponent<TweenAlpha>();
        Tscale_OnesSpt1 = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/Sprite1").gameObject.GetComponent<TweenScale>();
        Trotate_Oneslizi = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/lizi").gameObject.GetComponent<TweenRotation>();
        Tscale_Oneslizi = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/lizi").gameObject.GetComponent<TweenScale>();
        Talpha_OnesSpt2 = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/Sprite2").gameObject.GetComponent<TweenAlpha>();
        TScale_OnesSpt2 = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/Sprite2").gameObject.GetComponent<TweenScale>();
        OnesOne = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/One").gameObject;
        OnesOne_R = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/One/R").gameObject;
        OnesOne_Y = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/One/Y").gameObject;
        OnesOne_Z = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/One/Z").gameObject;
        OnesOne_1 = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/One/1").gameObject.GetComponent<UITexture>();
        OnesOne_2 = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/One/2").gameObject.GetComponent<UITexture>();
        OnesOne_3 = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/One/3").gameObject.GetComponent<UITexture>();
        OnesOne_4 = _uiRoot.transform.FindChild("gobj_GetSoldier/Effect/Ones/One/4").gameObject.GetComponent<UITexture>();
        Star = _uiRoot.transform.FindChild("gobj_GetSoldier/Star").gameObject;
        StarGrid = Star.GetComponent<UIGrid>();
        Star_1 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/1").gameObject;
        TweenStar_1 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/1/Star").gameObject.GetComponent<TweenScale>();
        Star_2 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/2").gameObject;
        TweenStar_2 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/2/Star").gameObject.GetComponent<TweenScale>();
        Star_3 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/3").gameObject;
        TweenStar_3 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/3/Star").gameObject.GetComponent<TweenScale>();
        Star_4 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/4").gameObject;
        TweenStar_4 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/4/Star").gameObject.GetComponent<TweenScale>();
        Star_5 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/5").gameObject;
        TweenStar_5 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/5/Star").gameObject.GetComponent<TweenScale>();
        Star_6 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/6").gameObject;
        TweenStar_6 = _uiRoot.transform.FindChild("gobj_GetSoldier/Star/6/Star").gameObject.GetComponent<TweenScale>();

        Lab_Title = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Item/TitleBG/Title").gameObject.GetComponent<UILabel>();
        Btn_BackBtn = _uiRoot.transform.FindChild("gobj_GetSoldier/BackBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnBackBtnBG = _uiRoot.transform.FindChild("gobj_GetSoldier/BackBtn/BG").gameObject.GetComponent<UISprite>();

        Gobj_SingleName = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName").gameObject;
        Spt_SingleLight = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Item/Light").gameObject.GetComponent<UISprite>();
        Lbl_SingleName = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Item/Name").gameObject.GetComponent<UILabel>();
        Lbl_SingleNum = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Item/Num").gameObject.GetComponent<UILabel>();
        Spt_SingleType = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Item/Type").gameObject.GetComponent<UISprite>();
        Spt_SingleQuality = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Item/Quality").gameObject.GetComponent<UISprite>();
        Spt_SingleChip = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Item/Chip").gameObject.GetComponent<UISprite>();
        Gobj_SingleEffRed = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Lizi_R").gameObject;
        Gobj_SingleEffYellow = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Lizi_Y").gameObject;
        Gobj_SingleEffPurple = _uiRoot.transform.FindChild("gobj_GetSoldier/SingleName/Lizi_Z").gameObject;
        OnesOne_R.SetActive(false);
        OnesOne_Y.SetActive(false);
        OnesOne_Z.SetActive(false);

        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_SingleName.text = "";
        Lbl_SingleNum.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
