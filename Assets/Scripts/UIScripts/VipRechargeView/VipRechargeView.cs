using UnityEngine;
using System;
using System.Collections;

public class VipRechargeView
{
    public static string UIName ="VipRechargeView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_VipRechargeView;
    public UISprite Spt_Mask;
    public UISprite Spt_BG;
    public UISprite Spt_FG;
    public UIButton Btn_Close;
    public UISprite Spt_BtnCloseBackground;
    public UISprite Spt_DecorationUp;
    public UISprite Spt_DecorationDown;
    public UISprite Spt_DecorationLeft;
    public UISprite Spt_DecorationRight;
    public GameObject Gobj_RechargePage;
    public UIPanel UIPanel_Recharge;
    public UIScrollView ScrView_Recharge;
    public UIGrid Grd_Recharge;
    public GameObject Gobj_VipPage;
    public UISprite Spt_DecorationL;
    public UISprite Spt_DecorationR;
    public UISprite Spt_LineUp;
    public UISprite Spt_LineDown;
    public UILabel Lbl_SeeVipLv;
    public UISprite Spt_SeeVipLvTip;
    public UISprite Spt_SeeVipLvTipBG;
    public UIButton Btn_Pre;
    public UISprite Spt_BtnPreBackground;
    public AotoPosition AotoPosition_BtnPreBackground;
    public UIButton Btn_Next;
    public UISprite Spt_BtnNextBackground;
    public AotoPosition AotoPosition_BtnNextBackground;
    public UIPanel UIPanel_DescPan;
    public UIScrollView ScrView_DescPan;
    public UIGrid Grd_VipDese;
    public UISprite Spt_CurVipLvTip;
    public UILabel Lbl_CurVipLv;
    public UIButton Btn_VipRecharge;
    public UISprite Spt_BtnVipRechargeBackground;
    public UILabel  Spt_BtnVipRechargeFG;
    public UISlider Slider_EXP;
    public UIEventListener UIEventListener_EXP;
    public UISprite Spt_SliderEXPBackground;
    public UISprite Spt_SliderEXPForeground;
    public UILabel  Spt_RechargeTip1;
    //public UISprite Spt_RechargeTip2;
    public UISprite Spt_NextVipLvTip;
    public UILabel Lbl_NextVipLv;
    public UILabel Lbl_LvUpDiaCount;
    public UILabel  Spt_Title;
    public GameObject Gobj_RechargeItem;
    public GameObject Gobj_VipDesc;
    public TweenScale VipRecharge_TScale;

    public GameObject Go_VipTrail;
    public GameObject Go_TopFuncFX;

    public UIScrollView ScrlView_VipContent;
    public UILabel Lbl_VipContent;
    public UILabel Lbl_TitleLvUpGift;
    public UILabel Lbl_TitleDayGift;
    public UIButton Btn_GetLvUp;
    public UILabel Lbl_BtnGetLvUp;
    public UISprite Spt_BtnGetLvUp;
    public UIButton Btn_GetDay;
    public UILabel Lbl_BtnGetDay;
    public UISprite Spt_BtnGetDay;
    public GameObject[] Gobj_Props;
    public GameObject[] Gobj_PropBGs;
    public UISprite[] Spt_PrepQualitys;
    public UISprite[] Spt_PrepIcons;
    public UISprite[] Spt_PrepMarks;
    public UILabel[] Lbl_PrepNums;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/VipRechargeView");
        UIPanel_VipRechargeView = _uiRoot.GetComponent<UIPanel>();
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_VIP, (GameObject gb) => { Go_VipTrail = gb; });
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_TOPFUNC, (GameObject gb) => { Go_TopFuncFX = gb; });
        VipRecharge_TScale = _uiRoot.transform.FindChild("VipRecharge").gameObject.GetComponent<TweenScale>();
        //Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Gobj_RechargeItem = _uiRoot.transform.FindChild("VipRecharge/Pre/RechargeItem").gameObject;
        Gobj_VipDesc = _uiRoot.transform.FindChild("VipRecharge/Pre/VipDese").gameObject;
        //Spt_BG = _uiRoot.transform.FindChild("VipRecharge/Other/BG").gameObject.GetComponent<UISprite>();
        //Spt_FG = _uiRoot.transform.FindChild("VipRecharge/Other/FG").gameObject.GetComponent<UISprite>();
        Btn_Close = _uiRoot.transform.FindChild("VipRecharge/Other/Close").gameObject.GetComponent<UIButton>();
        //Spt_BtnCloseBackground = _uiRoot.transform.FindChild("VipRecharge/Other/Close/Background").gameObject.GetComponent<UISprite>();
        //Spt_DecorationUp = _uiRoot.transform.FindChild("VipRecharge/Other/DecorationUp").gameObject.GetComponent<UISprite>();
        Spt_Title = _uiRoot.transform.FindChild("VipRecharge/Other/Title").gameObject.GetComponent<UILabel>();
       // Spt_DecorationDown = _uiRoot.transform.FindChild("VipRecharge/Other/DecorationDown").gameObject.GetComponent<UISprite>();
        //Spt_DecorationLeft = _uiRoot.transform.FindChild("VipRecharge/Other/DecorationLeft").gameObject.GetComponent<UISprite>();
        //Spt_DecorationRight = _uiRoot.transform.FindChild("VipRecharge/Other/DecorationRight").gameObject.GetComponent<UISprite>();
        Gobj_RechargePage = _uiRoot.transform.FindChild("VipRecharge/gobj_RechargePage").gameObject;
        //UIPanel_Recharge = _uiRoot.transform.FindChild("VipRecharge/gobj_RechargePage/Recharge").gameObject.GetComponent<UIPanel>();
        //ScrView_Recharge = _uiRoot.transform.FindChild("VipRecharge/gobj_RechargePage/Recharge").gameObject.GetComponent<UIScrollView>();
        Grd_Recharge = _uiRoot.transform.FindChild("VipRecharge/gobj_RechargePage/Recharge/Recharge").gameObject.GetComponent<UIGrid>();
        Gobj_VipPage = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage").gameObject;
        //Spt_DecorationL = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/SecTitle/DecorationL").gameObject.GetComponent<UISprite>();
        //Spt_DecorationR = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/SecTitle/DecorationR").gameObject.GetComponent<UISprite>();
        //Spt_LineUp = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/SecTitle/LineUp").gameObject.GetComponent<UISprite>();
        //Spt_LineDown = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/SecTitle/LineDown").gameObject.GetComponent<UISprite>();
        Lbl_SeeVipLv = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/SecTitle/SeeVipLv").gameObject.GetComponent<UILabel>();
        //Spt_SeeVipLvTip = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/SecTitle/SeeVipLvTip").gameObject.GetComponent<UISprite>();
        //Spt_SeeVipLvTipBG = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/SecTitle/SeeVipLvTipBG").gameObject.GetComponent<UISprite>();
        Btn_Pre = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/DirBtn/Pre").gameObject.GetComponent<UIButton>();
        //Spt_BtnPreBackground = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/DirBtn/Pre/Background").gameObject.GetComponent<UISprite>();
        //AotoPosition_BtnPreBackground = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/DirBtn/Pre/Background").gameObject.GetComponent<AotoPosition>();
        Btn_Next = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/DirBtn/Next").gameObject.GetComponent<UIButton>();
        //Spt_BtnNextBackground = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/DirBtn/Next/Background").gameObject.GetComponent<UISprite>();
        //AotoPosition_BtnNextBackground = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/DirBtn/Next/Background").gameObject.GetComponent<AotoPosition>();
        //UIPanel_DescPan = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/PanOffset/DescPan").gameObject.GetComponent<UIPanel>();
        ScrView_DescPan = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/PanOffset/DescPan").gameObject.GetComponent<UIScrollView>();
        Grd_VipDese = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/PanOffset/DescPan/VipDese").gameObject.GetComponent<UIGrid>();
        //Spt_CurVipLvTip = _uiRoot.transform.FindChild("VipRecharge/Title/CurVipLvTip").gameObject.GetComponent<UISprite>();
        Lbl_CurVipLv = _uiRoot.transform.FindChild("VipRecharge/Title/CurVipLv").gameObject.GetComponent<UILabel>();
        Btn_VipRecharge = _uiRoot.transform.FindChild("VipRecharge/Title/VipRecharge").gameObject.GetComponent<UIButton>();
        //Spt_BtnVipRechargeBackground = _uiRoot.transform.FindChild("VipRecharge/Title/VipRecharge/Background").gameObject.GetComponent<UISprite>();
        Spt_BtnVipRechargeFG = _uiRoot.transform.FindChild("VipRecharge/Title/VipRecharge/FG").gameObject.GetComponent<UILabel >();
        Slider_EXP = _uiRoot.transform.FindChild("VipRecharge/Title/EXP").gameObject.GetComponent<UISlider>();
        //UIEventListener_EXP = _uiRoot.transform.FindChild("VipRecharge/Title/EXP").gameObject.GetComponent<UIEventListener>();
        //Spt_SliderEXPBackground = _uiRoot.transform.FindChild("VipRecharge/Title/EXP/Background").gameObject.GetComponent<UISprite>();
        //Spt_SliderEXPForeground = _uiRoot.transform.FindChild("VipRecharge/Title/EXP/Foreground").gameObject.GetComponent<UISprite>();
        Spt_RechargeTip1 = _uiRoot.transform.FindChild("VipRecharge/Title/RechargeTip1").gameObject.GetComponent<UILabel >();
        //Spt_RechargeTip2 = _uiRoot.transform.FindChild("VipRecharge/Title/RechargeTip2").gameObject.GetComponent<UISprite>();
        Spt_NextVipLvTip = _uiRoot.transform.FindChild("VipRecharge/Title/NextVipLvTip").gameObject.GetComponent<UISprite>();
        Lbl_NextVipLv = _uiRoot.transform.FindChild("VipRecharge/Title/NextVipLv").gameObject.GetComponent<UILabel>();
        Lbl_LvUpDiaCount = _uiRoot.transform.FindChild("VipRecharge/Title/LvUpDiaCount").gameObject.GetComponent<UILabel>();

        ScrlView_VipContent = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/DescPan").gameObject.GetComponent<UIScrollView>();
        Lbl_VipContent = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/DescPan/VipContent").gameObject.GetComponent<UILabel>();
        Lbl_TitleLvUpGift = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/Gift/TitleLvUpGift").gameObject.GetComponent<UILabel>();
        Lbl_TitleDayGift = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/Gift/TitleDayGift").gameObject.GetComponent<UILabel>();
        Btn_GetLvUp = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/Gift/GetLvGift").gameObject.GetComponent<UIButton>();
        Lbl_BtnGetLvUp = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/Gift/GetLvGift/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnGetLvUp = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/Gift/GetLvGift/Background").gameObject.GetComponent<UISprite>();
        Btn_GetDay = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/Gift/GetDayGift").gameObject.GetComponent<UIButton>();
        Lbl_BtnGetDay = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/Gift/GetDayGift/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnGetDay = _uiRoot.transform.FindChild("VipRecharge/gobj_VipPage/NewVipPage/Gift/GetDayGift/Background").gameObject.GetComponent<UISprite>();

        Gobj_Props = new GameObject[6];
        Gobj_PropBGs = new GameObject[6];
        Spt_PrepQualitys = new UISprite[6];
        Spt_PrepIcons = new UISprite[6];
        Spt_PrepMarks = new UISprite[6];
        Lbl_PrepNums = new UILabel[6];

        string[] pathFormat = new string[6];
        pathFormat[0] = "VipRecharge/gobj_VipPage/NewVipPage/Gift/Grid/Prop{0}";
        pathFormat[1] = "VipRecharge/gobj_VipPage/NewVipPage/Gift/Grid/Prop{0}/PropBG";
        pathFormat[2] = "VipRecharge/gobj_VipPage/NewVipPage/Gift/Grid/Prop{0}/Quality";
        pathFormat[3] = "VipRecharge/gobj_VipPage/NewVipPage/Gift/Grid/Prop{0}/Icon";
        pathFormat[4] = "VipRecharge/gobj_VipPage/NewVipPage/Gift/Grid/Prop{0}/Mark";
        pathFormat[5] = "VipRecharge/gobj_VipPage/NewVipPage/Gift/Grid/Prop{0}/Num";
        for (int i = 0; i < 6; i++)
        {
            Gobj_Props[i] = _uiRoot.transform.FindChild(string.Format(pathFormat[0], i + 1)).gameObject;
            Gobj_PropBGs[i] = _uiRoot.transform.FindChild(string.Format(pathFormat[1], i + 1)).gameObject;
            Spt_PrepQualitys[i] = _uiRoot.transform.FindChild(string.Format(pathFormat[2], i + 1)).gameObject.GetComponent<UISprite>();
            Spt_PrepIcons[i] = _uiRoot.transform.FindChild(string.Format(pathFormat[3], i + 1)).gameObject.GetComponent<UISprite>();
            Spt_PrepMarks[i] = _uiRoot.transform.FindChild(string.Format(pathFormat[4], i + 1)).gameObject.GetComponent<UISprite>();
            Lbl_PrepNums[i] = _uiRoot.transform.FindChild(string.Format(pathFormat[5], i + 1)).gameObject.GetComponent<UILabel>();

        }
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_SeeVipLv.text = "6";
        Lbl_CurVipLv.text = "15";
        Lbl_NextVipLv.text = "15";
        Lbl_LvUpDiaCount.text = "50000";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
