using UnityEngine;
using System;
using System.Collections;

public class LivenessView
{
    public static string UIName ="LivenessView";
    public GameObject _uiRoot;
    //public UIPanel UIPanel_LivenessView;
    //public UITexture Tex_BG0;
    //public UISprite Spt_TitleBG;
    //public UILabel Lbl_Title;
    //public UISprite Spt_BG;
    //public UISprite Spt_FG;
    public UIButton Btn_Close;
    //public UISprite Spt_BtnCloseBackground;
    public UILabel Lbl_BtnCloseLabel;
    //public UISprite Spt_DecorationDown;
    //public UISprite Spt_DecorationUp;
    //public UIPanel UIPanel_ItemPan;
    //public UIScrollView ScrView_ItemPan;
    public UIGrid Grd_Items;
    //public UIPanel UIPanel_Layer3;
    //public UISprite Spt_BoxBG;
    public UISlider Slider_Progress;
    public UISprite Spt_SliderProgressBackground;
    //public UISprite Spt_SliderProgressForeground;
    public UILabel Lbl_ProgressDesc;
    public UIButton Btn_Box1;
    public UISprite Spt_BtnBox1Icon;
    //public UISprite Spt_BtnBox1NumBG;
    public UILabel Lbl_BtnBox1Num;
    public UIButton Btn_Box2;
    public UISprite Spt_BtnBox2Icon;
    public UILabel Lbl_BtnBox2Num;
    //public UISprite Spt_BtnBox2NumBG;
    public UIButton Btn_Box3;
    public UISprite Spt_BtnBox3Icon;
    //public UISprite Spt_BtnBox3NumBG;
    public UILabel Lbl_BtnBox3Num;
    public UIButton Btn_Box4;
    public UISprite Spt_BtnBox4Icon;
    public UILabel Lbl_BtnBox4Num;
    //public UISprite Spt_BtnBox4NumBG;
    public UIButton Btn_Box5;
    public UISprite Spt_BtnBox5Icon;
    //public UISprite Spt_BtnBox5NumBG;
    public UILabel Lbl_BtnBox5Num;
    public GameObject Gobj_LivenessItem;
    public TweenScale Anim_TScale;

    public GameObject Go_ReceiveBox1;
    public GameObject Go_ReceiveBox2;
    public GameObject Go_ReceiveBox3;
    public GameObject Go_ReceiveBox4;
    public GameObject Go_ReceiveBox5;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/LivenessView");
        Go_ReceiveBox1 = _uiRoot.transform.FindChild("Anim/Layer3/Box1/Receive").gameObject;
        Go_ReceiveBox2 = _uiRoot.transform.FindChild("Anim/Layer3/Box2/Receive").gameObject;
        Go_ReceiveBox3 = _uiRoot.transform.FindChild("Anim/Layer3/Box3/Receive").gameObject;
        Go_ReceiveBox4 = _uiRoot.transform.FindChild("Anim/Layer3/Box4/Receive").gameObject;
        Go_ReceiveBox5 = _uiRoot.transform.FindChild("Anim/Layer3/Box5/Receive").gameObject;


        //UIPanel_LivenessView = _uiRoot.GetComponent<UIPanel>(); Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        //Tex_BG0 = _uiRoot.transform.FindChild("BG0").gameObject.GetComponent<UITexture>();
        //Spt_TitleBG = _uiRoot.transform.FindChild("Anim/Layer1/TitleBG").gameObject.GetComponent<UISprite>();
        //Lbl_Title = _uiRoot.transform.FindChild("Anim/Layer1/Title").gameObject.GetComponent<UILabel>();
        //Spt_BG = _uiRoot.transform.FindChild("Anim/Layer1/BG").gameObject.GetComponent<UISprite>();
        //Spt_FG = _uiRoot.transform.FindChild("Anim/Layer1/FG").gameObject.GetComponent<UISprite>();
        Btn_Close = _uiRoot.transform.FindChild("Anim/Layer1/Close").gameObject.GetComponent<UIButton>();
        //Spt_BtnCloseBackground = _uiRoot.transform.FindChild("Anim/Layer1/Close/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnCloseLabel = _uiRoot.transform.FindChild("Anim/Layer1/Close/Label").gameObject.GetComponent<UILabel>();
        //Spt_DecorationDown = _uiRoot.transform.FindChild("Anim/Layer1/DecorationDown").gameObject.GetComponent<UISprite>();
        //Spt_DecorationUp = _uiRoot.transform.FindChild("Anim/Layer1/DecorationUp").gameObject.GetComponent<UISprite>();
        //UIPanel_ItemPan = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan").gameObject.GetComponent<UIPanel>();
        //ScrView_ItemPan = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan").gameObject.GetComponent<UIScrollView>();
        Grd_Items = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/Items").gameObject.GetComponent<UIGrid>();
        //UIPanel_Layer3 = _uiRoot.transform.FindChild("Anim/Layer3").gameObject.GetComponent<UIPanel>();
        //Spt_BoxBG = _uiRoot.transform.FindChild("Anim/Layer3/BoxBG").gameObject.GetComponent<UISprite>();
        Slider_Progress = _uiRoot.transform.FindChild("Anim/Layer3/Progress").gameObject.GetComponent<UISlider>();
        Spt_SliderProgressBackground = _uiRoot.transform.FindChild("Anim/Layer3/Progress/Background").gameObject.GetComponent<UISprite>();
        //Spt_SliderProgressForeground = _uiRoot.transform.FindChild("Anim/Layer3/Progress/Foreground").gameObject.GetComponent<UISprite>();
        Lbl_ProgressDesc = _uiRoot.transform.FindChild("Anim/Layer3/ProgressDesc").gameObject.GetComponent<UILabel>();
        Btn_Box1 = _uiRoot.transform.FindChild("Anim/Layer3/Box1").gameObject.GetComponent<UIButton>();
        Spt_BtnBox1Icon = _uiRoot.transform.FindChild("Anim/Layer3/Box1/Box/Icon").gameObject.GetComponent<UISprite>();
        //Spt_BtnBox1NumBG = _uiRoot.transform.FindChild("Anim/Layer3/Box1/NumBG").gameObject.GetComponent<UISprite>();
        Lbl_BtnBox1Num = _uiRoot.transform.FindChild("Anim/Layer3/Box1/Box/Num").gameObject.GetComponent<UILabel>();
        Btn_Box2 = _uiRoot.transform.FindChild("Anim/Layer3/Box2").gameObject.GetComponent<UIButton>();
        Spt_BtnBox2Icon = _uiRoot.transform.FindChild("Anim/Layer3/Box2/Box/Icon").gameObject.GetComponent<UISprite>();
        Lbl_BtnBox2Num = _uiRoot.transform.FindChild("Anim/Layer3/Box2/Box/Num").gameObject.GetComponent<UILabel>();
        //Spt_BtnBox2NumBG = _uiRoot.transform.FindChild("Anim/Layer3/Box2/NumBG").gameObject.GetComponent<UISprite>();
        Btn_Box3 = _uiRoot.transform.FindChild("Anim/Layer3/Box3").gameObject.GetComponent<UIButton>();
        Spt_BtnBox3Icon = _uiRoot.transform.FindChild("Anim/Layer3/Box3/Box/Icon").gameObject.GetComponent<UISprite>();
        //Spt_BtnBox3NumBG = _uiRoot.transform.FindChild("Anim/Layer3/Box3/NumBG").gameObject.GetComponent<UISprite>();
        Lbl_BtnBox3Num = _uiRoot.transform.FindChild("Anim/Layer3/Box3/Box/Num").gameObject.GetComponent<UILabel>();
        Btn_Box4 = _uiRoot.transform.FindChild("Anim/Layer3/Box4").gameObject.GetComponent<UIButton>();
        Spt_BtnBox4Icon = _uiRoot.transform.FindChild("Anim/Layer3/Box4/Box/Icon").gameObject.GetComponent<UISprite>();
        Lbl_BtnBox4Num = _uiRoot.transform.FindChild("Anim/Layer3/Box4/Box/Num").gameObject.GetComponent<UILabel>();
        //Spt_BtnBox4NumBG = _uiRoot.transform.FindChild("Anim/Layer3/Box4/NumBG").gameObject.GetComponent<UISprite>();
        Btn_Box5 = _uiRoot.transform.FindChild("Anim/Layer3/Box5").gameObject.GetComponent<UIButton>();
        Spt_BtnBox5Icon = _uiRoot.transform.FindChild("Anim/Layer3/Box5/Box/Icon").gameObject.GetComponent<UISprite>();
        //Spt_BtnBox5NumBG = _uiRoot.transform.FindChild("Anim/Layer3/Box5/NumBG").gameObject.GetComponent<UISprite>();
        Lbl_BtnBox5Num = _uiRoot.transform.FindChild("Anim/Layer3/Box5/Box/Num").gameObject.GetComponent<UILabel>();
        Gobj_LivenessItem = _uiRoot.transform.FindChild("Anim/Pre/LivenessItem").gameObject;
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        //Lbl_Title.text = "";
        Lbl_BtnCloseLabel.text = "";
        Lbl_ProgressDesc.text = "50/100";
        Lbl_BtnBox1Num.text = "";
        Lbl_BtnBox2Num.text = "";
        Lbl_BtnBox3Num.text = "";
        Lbl_BtnBox4Num.text = "";
        Lbl_BtnBox5Num.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
