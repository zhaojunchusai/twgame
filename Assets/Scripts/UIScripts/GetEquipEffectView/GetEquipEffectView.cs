using UnityEngine;
using System.Collections;

public class GetEquipEffectView 
{
    public GameObject _uiRoot;
    public static string UIName = "GetEquipEffectView";
    public UISprite Spt_ViewBG;
    #region 缘宝阁
    //Ones
    public GameObject GO_OnesLottery;
    public UIButton Btn_OnesSingleBtn;
    public UIButton Btn_OnesMultipleBtn;
    public UISprite Spt_OnesSingleCurrencyIcon;
    public UILabel Lab_OnesSingleCurrencyNum;
    public UISprite Spt_OnesMultipleCurrencyIcon;
    public UILabel Lab_OnesMultipleCurrencyNum;
    public UIButton Btn_OnesLotteryAnim;
    public UIButton Btn_OnesLotteryResult;
    public UIButton Btn_OnesCloseBtn;
    public UISprite Spt_Ones_Button_Dis;
    public UISprite Spt_Ones_Bg;
    public GameObject Button;
    public GameObject PropsPackage;
    public GameObject PropsPackage_Mask;
    public GameObject Go_OnesLotteryAnim_Item;
    public GetEquipLocationEffectItem OnesLocationEffectAnimItem;
    public GameObject Go_OnesLotteryResult_Item;
    public GetEquipLocationEffectItem OnesLocationEffectResultItem;
    //Tens
    public GameObject GO_TensLottery;
    public UIButton Btn_TensSingleBtn;
    public UIButton Btn_TensMultipleBtn;
    public UISprite Spt_TensSingleCurrencyIcon;
    public UILabel Lab_TensSingleCurrencyNum;
    public UISprite Spt_TensMultipleCurrencyIcon;
    public UILabel Lab_TensMultipleCurrencyNum;
    public UIButton Btn_TensLotteryAnim;
    public UIButton Btn_TensLotteryResult;
    public UISprite Spt_Tens_Button_Dis;
    public UIButton Btn_TensCloseBtn;

    public GameObject Go_TensLotteryAnim_Item_0;
    public GameObject Go_TensLotteryAnim_Item_1;
    public GameObject Go_TensLotteryAnim_Item_2;
    public GameObject Go_TensLotteryAnim_Item_3;
    public GameObject Go_TensLotteryAnim_Item_4;
    public GameObject Go_TensLotteryAnim_Item_5;
    public GameObject Go_TensLotteryAnim_Item_6;
    public GameObject Go_TensLotteryAnim_Item_7;
    public GameObject Go_TensLotteryAnim_Item_8;
    public GameObject Go_TensLotteryAnim_Item_9;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_0;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_1;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_2;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_3;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_4;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_5;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_6;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_7;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_8;
    public GetEquipLocationEffectItem TensLocationEffectAnimItem_9;

    public GameObject Go_TensLotteryResult_Item_0;
    public GameObject Go_TensLotteryResult_Item_1;
    public GameObject Go_TensLotteryResult_Item_2;
    public GameObject Go_TensLotteryResult_Item_3;
    public GameObject Go_TensLotteryResult_Item_4;
    public GameObject Go_TensLotteryResult_Item_5;
    public GameObject Go_TensLotteryResult_Item_6;
    public GameObject Go_TensLotteryResult_Item_7;
    public GameObject Go_TensLotteryResult_Item_8;
    public GameObject Go_TensLotteryResult_Item_9;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_0;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_1;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_2;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_3;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_4;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_5;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_6;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_7;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_8;
    public GetEquipLocationEffectItem TensLocationEffectResultItem_9;
    #endregion

    #region 天将神兵
    public GameObject GO_Magical;
    public GameObject GO_MagicalResult;
    public UIButton Btn_Magical;
    public UIButton Btn_MagicalResult;
    public UIGrid UI_Magical_Item_Grid;
    public UIGrid UI_MagicalResult_Item_Grid;
    //Magical
    public GameObject Go_Magical_Item_0;
    public GameObject Go_Magical_Item_1;
    public GameObject Go_Magical_Item_2;
    public GameObject Go_Magical_Item_3;
    public GameObject Go_Magical_Item_4;
    public GameObject Go_Magical_Item_5;
    public GameObject Go_Magical_Item_6;
    public GameObject Go_Magical_Item_7;
    public GameObject Go_Magical_Item_8;
    public GameObject Go_Magical_Item_9;

    public UILabel Go_Magical_Item_0_Name;
    public UILabel Go_Magical_Item_1_Name;
    public UILabel Go_Magical_Item_2_Name;
    public UILabel Go_Magical_Item_3_Name;
    public UILabel Go_Magical_Item_4_Name;
    public UILabel Go_Magical_Item_5_Name;
    public UILabel Go_Magical_Item_6_Name;
    public UILabel Go_Magical_Item_7_Name;
    public UILabel Go_Magical_Item_8_Name;
    public UILabel Go_Magical_Item_9_Name;

    public UISprite Go_Magical_Item_0_Tex;
    public UISprite Go_Magical_Item_1_Tex;
    public UISprite Go_Magical_Item_2_Tex;
    public UISprite Go_Magical_Item_3_Tex;
    public UISprite Go_Magical_Item_4_Tex;
    public UISprite Go_Magical_Item_5_Tex;
    public UISprite Go_Magical_Item_6_Tex;
    public UISprite Go_Magical_Item_7_Tex;
    public UISprite Go_Magical_Item_8_Tex;
    public UISprite Go_Magical_Item_9_Tex;

    public UISprite Go_Magical_Item_0_Frame;
    public UISprite Go_Magical_Item_1_Frame;
    public UISprite Go_Magical_Item_2_Frame;
    public UISprite Go_Magical_Item_3_Frame;
    public UISprite Go_Magical_Item_4_Frame;
    public UISprite Go_Magical_Item_5_Frame;
    public UISprite Go_Magical_Item_6_Frame;
    public UISprite Go_Magical_Item_7_Frame;
    public UISprite Go_Magical_Item_8_Frame;
    public UISprite Go_Magical_Item_9_Frame;

    public UISprite Go_Magical_Item_0_FrameBG;
    public UISprite Go_Magical_Item_1_FrameBG;
    public UISprite Go_Magical_Item_2_FrameBG;
    public UISprite Go_Magical_Item_3_FrameBG;
    public UISprite Go_Magical_Item_4_FrameBG;
    public UISprite Go_Magical_Item_5_FrameBG;
    public UISprite Go_Magical_Item_6_FrameBG;
    public UISprite Go_Magical_Item_7_FrameBG;
    public UISprite Go_Magical_Item_8_FrameBG;
    public UISprite Go_Magical_Item_9_FrameBG;
    //Magical Result
    public GameObject Go_MagicalResult_Item_0;
    public GameObject Go_MagicalResult_Item_1;
    public GameObject Go_MagicalResult_Item_2;
    public GameObject Go_MagicalResult_Item_3;
    public GameObject Go_MagicalResult_Item_4;
    public GameObject Go_MagicalResult_Item_5;
    public GameObject Go_MagicalResult_Item_6;
    public GameObject Go_MagicalResult_Item_7;
    public GameObject Go_MagicalResult_Item_8;
    public GameObject Go_MagicalResult_Item_9;

    public UILabel Go_MagicalResult_Item_0_Name;
    public UILabel Go_MagicalResult_Item_1_Name;
    public UILabel Go_MagicalResult_Item_2_Name;
    public UILabel Go_MagicalResult_Item_3_Name;
    public UILabel Go_MagicalResult_Item_4_Name;
    public UILabel Go_MagicalResult_Item_5_Name;
    public UILabel Go_MagicalResult_Item_6_Name;
    public UILabel Go_MagicalResult_Item_7_Name;
    public UILabel Go_MagicalResult_Item_8_Name;
    public UILabel Go_MagicalResult_Item_9_Name;

    public UISprite Go_MagicalResult_Item_0_Tex;
    public UISprite Go_MagicalResult_Item_1_Tex;
    public UISprite Go_MagicalResult_Item_2_Tex;
    public UISprite Go_MagicalResult_Item_3_Tex;
    public UISprite Go_MagicalResult_Item_4_Tex;
    public UISprite Go_MagicalResult_Item_5_Tex;
    public UISprite Go_MagicalResult_Item_6_Tex;
    public UISprite Go_MagicalResult_Item_7_Tex;
    public UISprite Go_MagicalResult_Item_8_Tex;
    public UISprite Go_MagicalResult_Item_9_Tex;

    public UISprite Go_MagicalResult_Item_0_Frame;
    public UISprite Go_MagicalResult_Item_1_Frame;
    public UISprite Go_MagicalResult_Item_2_Frame;
    public UISprite Go_MagicalResult_Item_3_Frame;
    public UISprite Go_MagicalResult_Item_4_Frame;
    public UISprite Go_MagicalResult_Item_5_Frame;
    public UISprite Go_MagicalResult_Item_6_Frame;
    public UISprite Go_MagicalResult_Item_7_Frame;
    public UISprite Go_MagicalResult_Item_8_Frame;
    public UISprite Go_MagicalResult_Item_9_Frame;

    public UISprite Go_MagicalResult_Item_0_FrameBG;
    public UISprite Go_MagicalResult_Item_1_FrameBG;
    public UISprite Go_MagicalResult_Item_2_FrameBG;
    public UISprite Go_MagicalResult_Item_3_FrameBG;
    public UISprite Go_MagicalResult_Item_4_FrameBG;
    public UISprite Go_MagicalResult_Item_5_FrameBG;
    public UISprite Go_MagicalResult_Item_6_FrameBG;
    public UISprite Go_MagicalResult_Item_7_FrameBG;
    public UISprite Go_MagicalResult_Item_8_FrameBG;
    public UISprite Go_MagicalResult_Item_9_FrameBG;
    #endregion


    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/GetEquipEffectView");
        Spt_ViewBG = _uiRoot.transform.FindChild("ViewBG").gameObject.GetComponent<UISprite>();
        #region 缘宝阁
        //Ones
        GO_OnesLottery = _uiRoot.transform.FindChild("OnesLottery").gameObject;
        Button = _uiRoot.transform.FindChild("OnesLottery/Button").gameObject;
        PropsPackage = _uiRoot.transform.FindChild("OnesLottery/PropsPackage").gameObject;
        PropsPackage_Mask = _uiRoot.transform.FindChild("OnesLottery/PropsPackage/Back").gameObject;
        Btn_OnesSingleBtn = _uiRoot.transform.FindChild("OnesLottery/Button/SingleBtn").gameObject.GetComponent<UIButton>();
        Btn_OnesMultipleBtn = _uiRoot.transform.FindChild("OnesLottery/Button/MultipleBtn").gameObject.GetComponent<UIButton>();
        Spt_OnesSingleCurrencyIcon = _uiRoot.transform.FindChild("OnesLottery/Button/SingleCurrencyIcon").gameObject.GetComponent<UISprite>();
        Lab_OnesSingleCurrencyNum = _uiRoot.transform.FindChild("OnesLottery/Button/SingleCurrencyIcon/SingleCurrencyNum").gameObject.GetComponent<UILabel>();
        Spt_OnesMultipleCurrencyIcon = _uiRoot.transform.FindChild("OnesLottery/Button/MultipleCurrencyIcon").gameObject.GetComponent<UISprite>();
        Lab_OnesMultipleCurrencyNum = _uiRoot.transform.FindChild("OnesLottery/Button/MultipleCurrencyIcon/MultipleCurrencyNum").gameObject.GetComponent<UILabel>();
        Btn_OnesLotteryAnim = _uiRoot.transform.FindChild("OnesLottery/OnesLotteryAnim").gameObject.GetComponent<UIButton>();
        Btn_OnesLotteryResult = _uiRoot.transform.FindChild("OnesLottery/OnesLotteryResult").gameObject.GetComponent<UIButton>();
        Btn_OnesCloseBtn = _uiRoot.transform.FindChild("OnesLottery/Button/BackBtn").gameObject.GetComponent<UIButton>();
        Spt_Ones_Button_Dis = _uiRoot.transform.FindChild("OnesLottery/Button/MultipleBtn/Dis").gameObject.GetComponent<UISprite>();
        Spt_Ones_Bg = _uiRoot.transform.FindChild("OnesLottery/OnesLotteryAnim/Effect/Sprite1").gameObject.GetComponent<UISprite>();
        Go_OnesLotteryAnim_Item = _uiRoot.transform.FindChild("OnesLottery/OnesLotteryAnim/Item0").gameObject;
        OnesLocationEffectAnimItem = GetEquipLocationItemComponent(Go_OnesLotteryAnim_Item);
        Go_OnesLotteryResult_Item = _uiRoot.transform.FindChild("OnesLottery/OnesLotteryResult/Item0").gameObject;
        OnesLocationEffectResultItem = GetEquipLocationItemComponent(Go_OnesLotteryResult_Item);


        //Tens
        GO_TensLottery = _uiRoot.transform.FindChild("TensLottery").gameObject;
        Btn_TensCloseBtn = _uiRoot.transform.FindChild("TensLottery/Button/BackBtn").gameObject.GetComponent<UIButton>();
        Spt_Tens_Button_Dis = _uiRoot.transform.FindChild("TensLottery/Button/MultipleBtn/Dis").gameObject.GetComponent<UISprite>();
        Btn_TensLotteryAnim = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim").gameObject.GetComponent<UIButton>();
        Btn_TensLotteryResult = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult").gameObject.GetComponent<UIButton>();
        Btn_TensSingleBtn = _uiRoot.transform.FindChild("TensLottery/Button/SingleBtn").gameObject.GetComponent<UIButton>();
        Btn_TensMultipleBtn = _uiRoot.transform.FindChild("TensLottery/Button/MultipleBtn").gameObject.GetComponent<UIButton>();
        Spt_TensSingleCurrencyIcon = _uiRoot.transform.FindChild("TensLottery/Button/SingleCurrencyIcon").gameObject.GetComponent<UISprite>();
        Lab_TensSingleCurrencyNum=_uiRoot.transform.FindChild("TensLottery/Button/SingleCurrencyIcon/SingleCurrencyNum").gameObject.GetComponent<UILabel>();
        Spt_TensMultipleCurrencyIcon = _uiRoot.transform.FindChild("TensLottery/Button/MultipleCurrencyIcon").gameObject.GetComponent<UISprite>();
        Lab_TensMultipleCurrencyNum = _uiRoot.transform.FindChild("TensLottery/Button/MultipleCurrencyIcon/MultipleCurrencyNum").gameObject.GetComponent<UILabel>();

        Go_TensLotteryAnim_Item_0 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item0").gameObject;
        Go_TensLotteryAnim_Item_1 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item1").gameObject;
        Go_TensLotteryAnim_Item_2 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item2").gameObject;
        Go_TensLotteryAnim_Item_3 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item3").gameObject;
        Go_TensLotteryAnim_Item_4 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item4").gameObject;
        Go_TensLotteryAnim_Item_5 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item5").gameObject;
        Go_TensLotteryAnim_Item_6 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item6").gameObject;
        Go_TensLotteryAnim_Item_7 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item7").gameObject;
        Go_TensLotteryAnim_Item_8 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item8").gameObject;
        Go_TensLotteryAnim_Item_9 = _uiRoot.transform.FindChild("TensLottery/TensLotteryAnim/Item/Item9").gameObject;
        TensLocationEffectAnimItem_0 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_0);
        TensLocationEffectAnimItem_1 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_1);
        TensLocationEffectAnimItem_2 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_2);
        TensLocationEffectAnimItem_3 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_3);
        TensLocationEffectAnimItem_4 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_4);
        TensLocationEffectAnimItem_5 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_5);
        TensLocationEffectAnimItem_6 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_6);
        TensLocationEffectAnimItem_7 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_7);
        TensLocationEffectAnimItem_8 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_8);
        TensLocationEffectAnimItem_9 = GetEquipLocationItemComponent(Go_TensLotteryAnim_Item_9);

        Go_TensLotteryResult_Item_0 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item0").gameObject;
        Go_TensLotteryResult_Item_1 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item1").gameObject;
        Go_TensLotteryResult_Item_2 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item2").gameObject;
        Go_TensLotteryResult_Item_3 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item3").gameObject;
        Go_TensLotteryResult_Item_4 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item4").gameObject;
        Go_TensLotteryResult_Item_5 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item5").gameObject;
        Go_TensLotteryResult_Item_6 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item6").gameObject;
        Go_TensLotteryResult_Item_7 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item7").gameObject;
        Go_TensLotteryResult_Item_8 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item8").gameObject;
        Go_TensLotteryResult_Item_9 = _uiRoot.transform.FindChild("TensLottery/TensLotteryResult/Item/Item9").gameObject;
        TensLocationEffectResultItem_0 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_0);
        TensLocationEffectResultItem_1 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_1);
        TensLocationEffectResultItem_2 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_2);
        TensLocationEffectResultItem_3 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_3);
        TensLocationEffectResultItem_4 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_4);
        TensLocationEffectResultItem_5 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_5);
        TensLocationEffectResultItem_6 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_6);
        TensLocationEffectResultItem_7 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_7);
        TensLocationEffectResultItem_8 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_8);
        TensLocationEffectResultItem_9 = GetEquipLocationItemComponent(Go_TensLotteryResult_Item_9);
        #endregion
        #region 天将神兵
        GO_Magical = _uiRoot.transform.FindChild("Magical").gameObject;
        GO_MagicalResult = _uiRoot.transform.FindChild("MagicalResult").gameObject;
        Btn_Magical = _uiRoot.transform.FindChild("Magical").gameObject.GetComponent<UIButton>();
        Btn_MagicalResult = _uiRoot.transform.FindChild("MagicalResult").gameObject.GetComponent<UIButton>();
        UI_Magical_Item_Grid = _uiRoot.transform.FindChild("Magical/Item").gameObject.GetComponent<UIGrid>();
        UI_MagicalResult_Item_Grid = _uiRoot.transform.FindChild("MagicalResult").gameObject.GetComponent<UIGrid>();
        //Magical
        Go_Magical_Item_0 = _uiRoot.transform.FindChild("Magical/Item/0").gameObject;
        Go_Magical_Item_1 = _uiRoot.transform.FindChild("Magical/Item/1").gameObject;
        Go_Magical_Item_2 = _uiRoot.transform.FindChild("Magical/Item/2").gameObject;
        Go_Magical_Item_3 = _uiRoot.transform.FindChild("Magical/Item/3").gameObject;
        Go_Magical_Item_4 = _uiRoot.transform.FindChild("Magical/Item/4").gameObject;
        Go_Magical_Item_5 = _uiRoot.transform.FindChild("Magical/Item/5").gameObject;
        Go_Magical_Item_6 = _uiRoot.transform.FindChild("Magical/Item/6").gameObject;
        Go_Magical_Item_7 = _uiRoot.transform.FindChild("Magical/Item/7").gameObject;
        Go_Magical_Item_8 = _uiRoot.transform.FindChild("Magical/Item/8").gameObject;
        Go_Magical_Item_9 = _uiRoot.transform.FindChild("Magical/Item/9").gameObject;
        Go_Magical_Item_0_Tex = _uiRoot.transform.FindChild("Magical/Item/0/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_1_Tex = _uiRoot.transform.FindChild("Magical/Item/1/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_2_Tex = _uiRoot.transform.FindChild("Magical/Item/2/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_3_Tex = _uiRoot.transform.FindChild("Magical/Item/3/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_4_Tex = _uiRoot.transform.FindChild("Magical/Item/4/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_5_Tex = _uiRoot.transform.FindChild("Magical/Item/5/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_6_Tex = _uiRoot.transform.FindChild("Magical/Item/6/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_7_Tex = _uiRoot.transform.FindChild("Magical/Item/7/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_8_Tex = _uiRoot.transform.FindChild("Magical/Item/8/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_9_Tex = _uiRoot.transform.FindChild("Magical/Item/9/texture").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_0_Name = _uiRoot.transform.FindChild("Magical/Item/0/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_1_Name = _uiRoot.transform.FindChild("Magical/Item/1/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_2_Name = _uiRoot.transform.FindChild("Magical/Item/2/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_3_Name = _uiRoot.transform.FindChild("Magical/Item/3/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_4_Name = _uiRoot.transform.FindChild("Magical/Item/4/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_5_Name = _uiRoot.transform.FindChild("Magical/Item/5/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_6_Name = _uiRoot.transform.FindChild("Magical/Item/6/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_7_Name = _uiRoot.transform.FindChild("Magical/Item/7/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_8_Name = _uiRoot.transform.FindChild("Magical/Item/8/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_9_Name = _uiRoot.transform.FindChild("Magical/Item/9/texture/Name").gameObject.GetComponent<UILabel>();
        Go_Magical_Item_0_Frame = _uiRoot.transform.FindChild("Magical/Item/0/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_1_Frame = _uiRoot.transform.FindChild("Magical/Item/1/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_2_Frame = _uiRoot.transform.FindChild("Magical/Item/2/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_3_Frame = _uiRoot.transform.FindChild("Magical/Item/3/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_4_Frame = _uiRoot.transform.FindChild("Magical/Item/4/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_5_Frame = _uiRoot.transform.FindChild("Magical/Item/5/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_6_Frame = _uiRoot.transform.FindChild("Magical/Item/6/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_7_Frame = _uiRoot.transform.FindChild("Magical/Item/7/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_8_Frame = _uiRoot.transform.FindChild("Magical/Item/8/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_9_Frame = _uiRoot.transform.FindChild("Magical/Item/9/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_0_FrameBG = _uiRoot.transform.FindChild("Magical/Item/0/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_1_FrameBG = _uiRoot.transform.FindChild("Magical/Item/1/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_2_FrameBG = _uiRoot.transform.FindChild("Magical/Item/2/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_3_FrameBG = _uiRoot.transform.FindChild("Magical/Item/3/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_4_FrameBG = _uiRoot.transform.FindChild("Magical/Item/4/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_5_FrameBG = _uiRoot.transform.FindChild("Magical/Item/5/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_6_FrameBG = _uiRoot.transform.FindChild("Magical/Item/6/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_7_FrameBG = _uiRoot.transform.FindChild("Magical/Item/7/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_8_FrameBG = _uiRoot.transform.FindChild("Magical/Item/8/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_Magical_Item_9_FrameBG = _uiRoot.transform.FindChild("Magical/Item/9/texture/FrameBG").gameObject.GetComponent<UISprite>();
        //Magical Result
        Go_MagicalResult_Item_0 = _uiRoot.transform.FindChild("MagicalResult/0").gameObject;
        Go_MagicalResult_Item_1 = _uiRoot.transform.FindChild("MagicalResult/1").gameObject;
        Go_MagicalResult_Item_2 = _uiRoot.transform.FindChild("MagicalResult/2").gameObject;
        Go_MagicalResult_Item_3 = _uiRoot.transform.FindChild("MagicalResult/3").gameObject;
        Go_MagicalResult_Item_4 = _uiRoot.transform.FindChild("MagicalResult/4").gameObject;
        Go_MagicalResult_Item_5 = _uiRoot.transform.FindChild("MagicalResult/5").gameObject;
        Go_MagicalResult_Item_6 = _uiRoot.transform.FindChild("MagicalResult/6").gameObject;
        Go_MagicalResult_Item_7 = _uiRoot.transform.FindChild("MagicalResult/7").gameObject;
        Go_MagicalResult_Item_8 = _uiRoot.transform.FindChild("MagicalResult/8").gameObject;
        Go_MagicalResult_Item_9 = _uiRoot.transform.FindChild("MagicalResult/9").gameObject;
        Go_MagicalResult_Item_0_Tex = _uiRoot.transform.FindChild("MagicalResult/0/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_1_Tex = _uiRoot.transform.FindChild("MagicalResult/1/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_2_Tex = _uiRoot.transform.FindChild("MagicalResult/2/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_3_Tex = _uiRoot.transform.FindChild("MagicalResult/3/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_4_Tex = _uiRoot.transform.FindChild("MagicalResult/4/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_5_Tex = _uiRoot.transform.FindChild("MagicalResult/5/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_6_Tex = _uiRoot.transform.FindChild("MagicalResult/6/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_7_Tex = _uiRoot.transform.FindChild("MagicalResult/7/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_8_Tex = _uiRoot.transform.FindChild("MagicalResult/8/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_9_Tex = _uiRoot.transform.FindChild("MagicalResult/9/texture").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_0_Name = _uiRoot.transform.FindChild("MagicalResult/0/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_1_Name = _uiRoot.transform.FindChild("MagicalResult/1/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_2_Name = _uiRoot.transform.FindChild("MagicalResult/2/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_3_Name = _uiRoot.transform.FindChild("MagicalResult/3/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_4_Name = _uiRoot.transform.FindChild("MagicalResult/4/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_5_Name = _uiRoot.transform.FindChild("MagicalResult/5/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_6_Name = _uiRoot.transform.FindChild("MagicalResult/6/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_7_Name = _uiRoot.transform.FindChild("MagicalResult/7/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_8_Name = _uiRoot.transform.FindChild("MagicalResult/8/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_9_Name = _uiRoot.transform.FindChild("MagicalResult/9/texture/Name").gameObject.GetComponent<UILabel>();
        Go_MagicalResult_Item_0_Frame = _uiRoot.transform.FindChild("MagicalResult/0/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_1_Frame = _uiRoot.transform.FindChild("MagicalResult/1/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_2_Frame = _uiRoot.transform.FindChild("MagicalResult/2/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_3_Frame = _uiRoot.transform.FindChild("MagicalResult/3/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_4_Frame = _uiRoot.transform.FindChild("MagicalResult/4/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_5_Frame = _uiRoot.transform.FindChild("MagicalResult/5/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_6_Frame = _uiRoot.transform.FindChild("MagicalResult/6/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_7_Frame = _uiRoot.transform.FindChild("MagicalResult/7/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_8_Frame = _uiRoot.transform.FindChild("MagicalResult/8/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_9_Frame = _uiRoot.transform.FindChild("MagicalResult/9/texture/Frame").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_0_FrameBG = _uiRoot.transform.FindChild("MagicalResult/0/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_1_FrameBG = _uiRoot.transform.FindChild("MagicalResult/1/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_2_FrameBG = _uiRoot.transform.FindChild("MagicalResult/2/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_3_FrameBG = _uiRoot.transform.FindChild("MagicalResult/3/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_4_FrameBG = _uiRoot.transform.FindChild("MagicalResult/4/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_5_FrameBG = _uiRoot.transform.FindChild("MagicalResult/5/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_6_FrameBG = _uiRoot.transform.FindChild("MagicalResult/6/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_7_FrameBG = _uiRoot.transform.FindChild("MagicalResult/7/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_8_FrameBG = _uiRoot.transform.FindChild("MagicalResult/8/texture/FrameBG").gameObject.GetComponent<UISprite>();
        Go_MagicalResult_Item_9_FrameBG = _uiRoot.transform.FindChild("MagicalResult/9/texture/FrameBG").gameObject.GetComponent<UISprite>();
        #endregion
        this.Button.gameObject.SetActive(true);
    }
    private GetEquipLocationEffectItem GetEquipLocationItemComponent(GameObject go)
    {
        if (go.GetComponent <GetEquipLocationEffectItem >()==null )
        {
            return go.AddComponent<GetEquipLocationEffectItem>();
        }
        return go.GetComponent<GetEquipLocationEffectItem>();
    }






}
