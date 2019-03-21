using UnityEngine;
using System;
using System.Collections;

public class UnionPrisonChooseView
{
    public static string UIName ="UnionPrisonChooseView";
    public GameObject _uiRoot;

    public UIToggle Tog_TabStranger;
    public UIToggle Tog_TabEnemy;
    public UIToggle Tog_TabEscaped;

    public UIButton Btn_Button_Level;
    public UIButton Btn_Button_Power;
    public UIInput inPut;

    public UIScrollView ScrView_StrangerGroup;
    public UIGrid Grd_Grid_StrangerGroup;
    public UIWrapContent UIWrapContent_StrangerGroup;
    public GameObject Item_StrangerGroup;

    public UIScrollView ScrView_OtherGroup;
    public UIGrid Grd_Grid_OtherGroup;
    public UIWrapContent UIWrapContent_OtherGroup;
    public GameObject Item_OtherGroup;

    public UIPanel Panel_Effect;
    public GameObject Go_EffectMask;
    public GameObject Go_StrangerPos_1;
    public GameObject Go_StrangerPos_2;
    public GameObject Go_StrangerPos_3;
    public GameObject Go_StrangerPos_4;
    public GameObject Go_OtherEffectPos_1;
    public GameObject Go_OtherEffectPos_2;
    public GameObject Go_OtherEffectPos_3;
    public GameObject Go_OtherEffectPos_4;
    public GameObject Mask;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionPrisonChooseView");
        Panel_Effect = _uiRoot.transform.FindChild("Effect").gameObject.GetComponent<UIPanel>();
        Go_EffectMask = _uiRoot.transform.FindChild("Effect/EffectMask").gameObject;
        Go_StrangerPos_1 = _uiRoot.transform.FindChild("Effect/StrangerEffectPos/1").gameObject;
        Go_StrangerPos_2 = _uiRoot.transform.FindChild("Effect/StrangerEffectPos/2").gameObject;
        Go_StrangerPos_3 = _uiRoot.transform.FindChild("Effect/StrangerEffectPos/3").gameObject;
        Go_StrangerPos_4 = _uiRoot.transform.FindChild("Effect/StrangerEffectPos/4").gameObject;
        Go_OtherEffectPos_1 = _uiRoot.transform.FindChild("Effect/OtherEffectPos/1").gameObject;
        Go_OtherEffectPos_2 = _uiRoot.transform.FindChild("Effect/OtherEffectPos/2").gameObject;
        Go_OtherEffectPos_3 = _uiRoot.transform.FindChild("Effect/OtherEffectPos/3").gameObject;
        Go_OtherEffectPos_4 = _uiRoot.transform.FindChild("Effect/OtherEffectPos/4").gameObject;
        Tog_TabStranger = _uiRoot.transform.FindChild("TroggleGroup/TabStranger").gameObject.GetComponent<UIToggle>();
        Tog_TabEnemy = _uiRoot.transform.FindChild("TroggleGroup/TabEnemy").gameObject.GetComponent<UIToggle>();
        Tog_TabEscaped = _uiRoot.transform.FindChild("TroggleGroup/TabEscaped").gameObject.GetComponent<UIToggle>();

        Btn_Button_Level = _uiRoot.transform.FindChild("StrangerGroup/Button_Level").gameObject.GetComponent<UIButton>();
        Btn_Button_Power = _uiRoot.transform.FindChild("StrangerGroup/Button_Power").gameObject.GetComponent<UIButton>();
        inPut = _uiRoot.transform.FindChild("StrangerGroup/Input").gameObject.GetComponent<UIInput>();

        ScrView_StrangerGroup = _uiRoot.transform.FindChild("StrangerGroup/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Grid_StrangerGroup = _uiRoot.transform.FindChild("StrangerGroup/MaterialScroll/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_StrangerGroup = _uiRoot.transform.FindChild("StrangerGroup/MaterialScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        Item_StrangerGroup = _uiRoot.transform.FindChild("StrangerGroup/MaterialScroll/item").gameObject;

        ScrView_OtherGroup = _uiRoot.transform.FindChild("OtherGroup/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Grid_OtherGroup = _uiRoot.transform.FindChild("OtherGroup/MaterialScroll/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_OtherGroup = _uiRoot.transform.FindChild("OtherGroup/MaterialScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        Item_OtherGroup = _uiRoot.transform.FindChild("OtherGroup/MaterialScroll/item").gameObject;
        Mask = _uiRoot.transform.FindChild("BackGroundGroup/MarkSprite").gameObject;
        SetLabelValues();
    }

    public void SetLabelValues()
    {
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
