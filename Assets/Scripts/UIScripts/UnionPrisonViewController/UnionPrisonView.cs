using UnityEngine;
using System;
using System.Collections;

public class UnionPrisonView
{
    public static string UIName ="UnionPrisonView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionPrisonView;
    public UIButton Btn_RuleBtn;
    public UIButton Btn_Close;
    public UISprite Spt_BG;

    public class DependView
    {
        public UILabel Lbl_UnionName;
        public UILabel Lbl_UnionLeader;
        public UIButton Btn_Battle;
        public GameObject EmptyObject;
        public UILabel EmptyGroup_Lbl_Count;
        public UILabel EmptyGroup_Lbl_Descript;
        public UIButton Btn_Molest;
        public UIButton Btn_Comfort;
        public GameObject HadObject;
        public UILabel HadGroup_Lbl_Count;
        public UILabel HadGroup_Lbl_Level;
        public UILabel HadGroup_Lbl_Buff;
        public GameObject IconObject;
        public UISprite Spt_Sprite_Icon;
        public UISprite Spt_Sprite_Quality;
        public UISprite Spt_Sprite_Quality_Back;
        public UIButton Btn_Close;
        public UILabel Lbl_Time;
        public UISpriteAnimation effect_tiaoxi;
        public GameObject effect_anwei;
    }
    public DependView rightView = new DependView();

    public DependView leftView = new DependView();

    public class MiddleView
    {
        public UILabel Lbl_UnionName;
        public UILabel Lbl_UnionLeader;
        public UILabel EmptyGroup_Lbl_Count;
        public UILabel EmptyGroup_Lbl_Level;
        public UILabel EmptyGroup_Lbl_Buff;
        public UIButton Btn_Revolt;
        public UIButton Btn_Rescue;
        public UILabel HadGroup_Lbl_Descript;
        public UISprite Spt_Sprite_Icon;
        public UISprite Spt_Sprite_Quality;
        public UISprite Spt_Sprite_Quality_Back;
        public UILabel HadGroup_Lbl_Count;
        public GameObject EmptyGroup;
        public GameObject HadGroup;
    }
    public MiddleView middleView = new MiddleView();

    public class MarkView
    {
        public GameObject Btn_Back;
        public GameObject Item1;
        public GameObject Item2;
        public GameObject Item3;
        public UITable UITable_Table;
     }
    public MarkView markView = new MarkView();
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionPrisonView");
        UIPanel_UnionPrisonView = _uiRoot.GetComponent<UIPanel>();
        Btn_RuleBtn = _uiRoot.transform.FindChild("Anim/RuleBtn").gameObject.GetComponent<UIButton>();

        rightView.Lbl_UnionName = _uiRoot.transform.FindChild("Anim/DependRight/Title/UnionName/Label").gameObject.GetComponent<UILabel>();
        rightView.Lbl_UnionLeader = _uiRoot.transform.FindChild("Anim/DependRight/Title/UnionLeader/Label").gameObject.GetComponent<UILabel>();
        rightView.Btn_Battle = _uiRoot.transform.FindChild("Anim/DependRight/EmptyGroup/Battle").gameObject.GetComponent<UIButton>();
        rightView.EmptyGroup_Lbl_Count = _uiRoot.transform.FindChild("Anim/DependRight/EmptyGroup/SurplusCount/Count/Label").gameObject.GetComponent<UILabel>();
        rightView.EmptyGroup_Lbl_Descript = _uiRoot.transform.FindChild("Anim/DependRight/EmptyGroup/Descript/Label").gameObject.GetComponent<UILabel>();
        rightView.Btn_Molest = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Molest").gameObject.GetComponent<UIButton>();
        rightView.Btn_Comfort = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Comfort").gameObject.GetComponent<UIButton>();
        rightView.HadGroup_Lbl_Count = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/SurplusCount/Count/Label").gameObject.GetComponent<UILabel>();
        rightView.HadGroup_Lbl_Level = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Descript/Level/Label").gameObject.GetComponent<UILabel>();
        rightView.HadGroup_Lbl_Buff = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Descript/Buff/Label").gameObject.GetComponent<UILabel>();
        rightView.Spt_Sprite_Icon = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Altar/IconGroup/Sprite_Icon").gameObject.GetComponent<UISprite>();
        rightView.Spt_Sprite_Quality = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Altar/IconGroup/Sprite_Quality").gameObject.GetComponent<UISprite>();
        rightView.Spt_Sprite_Quality_Back = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Altar/IconGroup/Sprite_Quality_Back").gameObject.GetComponent<UISprite>();
        rightView.Btn_Close = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Altar/IconGroup/Close").gameObject.GetComponent<UIButton>();
        rightView.Lbl_Time = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Altar/IconGroup/Time").gameObject.GetComponent<UILabel>();
        rightView.EmptyObject = _uiRoot.transform.FindChild("Anim/DependRight/EmptyGroup").gameObject;
        rightView.HadObject = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup").gameObject;
        rightView.IconObject = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/Altar/IconGroup").gameObject;
        rightView.effect_tiaoxi = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/tiaoxi").gameObject.GetComponent<UISpriteAnimation>();
        rightView.effect_anwei = _uiRoot.transform.FindChild("Anim/DependRight/HadGroup/anwei").gameObject;
        rightView.effect_anwei.SetActive(false);
        leftView.Lbl_UnionName = _uiRoot.transform.FindChild("Anim/DependLeft/Title/UnionName/Label").gameObject.GetComponent<UILabel>();
        leftView.Lbl_UnionLeader = _uiRoot.transform.FindChild("Anim/DependLeft/Title/UnionLeader/Label").gameObject.GetComponent<UILabel>();
        leftView.Btn_Battle = _uiRoot.transform.FindChild("Anim/DependLeft/EmptyGroup/Battle").gameObject.GetComponent<UIButton>();
        leftView.EmptyGroup_Lbl_Count = _uiRoot.transform.FindChild("Anim/DependLeft/EmptyGroup/SurplusCount/Count/Label").gameObject.GetComponent<UILabel>();
        leftView.EmptyGroup_Lbl_Descript = _uiRoot.transform.FindChild("Anim/DependLeft/EmptyGroup/Descript/Label").gameObject.GetComponent<UILabel>();
        leftView.Btn_Molest = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Molest").gameObject.GetComponent<UIButton>();
        leftView.Btn_Comfort = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Comfort").gameObject.GetComponent<UIButton>();
        leftView.HadGroup_Lbl_Count = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/SurplusCount/Count/Label").gameObject.GetComponent<UILabel>();
        leftView.HadGroup_Lbl_Level = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Descript/Level/Label").gameObject.GetComponent<UILabel>();
        leftView.HadGroup_Lbl_Buff = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Descript/Buff/Label").gameObject.GetComponent<UILabel>();
        leftView.Spt_Sprite_Icon = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Altar/IconGroup/Sprite_Icon").gameObject.GetComponent<UISprite>();
        leftView.Spt_Sprite_Quality = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Altar/IconGroup/Sprite_Quality").gameObject.GetComponent<UISprite>();
        leftView.Spt_Sprite_Quality_Back = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Altar/IconGroup/Sprite_Quality_Back").gameObject.GetComponent<UISprite>();
        leftView.Btn_Close = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Altar/IconGroup/Close").gameObject.GetComponent<UIButton>();
        leftView.Lbl_Time = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Altar/IconGroup/Time").gameObject.GetComponent<UILabel>();
        leftView.EmptyObject = _uiRoot.transform.FindChild("Anim/DependLeft/EmptyGroup").gameObject;
        leftView.HadObject = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup").gameObject;
        leftView.IconObject = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/Altar/IconGroup").gameObject;
        leftView.effect_tiaoxi = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/tiaoxi").gameObject.GetComponent<UISpriteAnimation>(); ;
        leftView.effect_anwei = _uiRoot.transform.FindChild("Anim/DependLeft/HadGroup/anwei").gameObject;
        leftView.effect_anwei.SetActive(false);
        middleView.Lbl_UnionName = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/Title/UnionName/Label").gameObject.GetComponent<UILabel>();
        middleView.Lbl_UnionLeader = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/Title/UnionLeader/Label").gameObject.GetComponent<UILabel>();
        middleView.EmptyGroup_Lbl_Count = _uiRoot.transform.FindChild("Anim/DependMiddle/EmptyGroup/SurplusCount/Count/Label").gameObject.GetComponent<UILabel>();
        middleView.EmptyGroup_Lbl_Level = _uiRoot.transform.FindChild("Anim/DependMiddle/EmptyGroup/Descript/Level/Label").gameObject.GetComponent<UILabel>();
        middleView.EmptyGroup_Lbl_Buff = _uiRoot.transform.FindChild("Anim/DependMiddle/EmptyGroup/Descript/Buff/Label").gameObject.GetComponent<UILabel>();
        middleView.EmptyGroup = _uiRoot.transform.FindChild("Anim/DependMiddle/EmptyGroup").gameObject;

        middleView.Btn_Revolt = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/Revolt").gameObject.GetComponent<UIButton>();
        middleView.Btn_Rescue = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/Rescue").gameObject.GetComponent<UIButton>();
        middleView.HadGroup_Lbl_Descript = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/Descript/Label").gameObject.GetComponent<UILabel>();
        middleView.Spt_Sprite_Icon = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/Altar/IconGroup/Sprite_Icon").gameObject.GetComponent<UISprite>();
        middleView.Spt_Sprite_Quality = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/Altar/IconGroup/Sprite_Quality").gameObject.GetComponent<UISprite>();
        middleView.Spt_Sprite_Quality_Back = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/Altar/IconGroup/Sprite_Quality_Back").gameObject.GetComponent<UISprite>();
        middleView.HadGroup_Lbl_Count = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup/SurplusCount/Count/Label").gameObject.GetComponent<UILabel>();
        middleView.HadGroup = _uiRoot.transform.FindChild("Anim/DependMiddle/HadGroup").gameObject;

        markView.UITable_Table = _uiRoot.transform.FindChild("Anim/MarkGroup/Soldier/Table").gameObject.GetComponent<UITable>();

        markView.Item1 = _uiRoot.transform.FindChild("Anim/MarkGroup/Soldier/Table/Item_1").gameObject;
        markView.Item2 = _uiRoot.transform.FindChild("Anim/MarkGroup/Soldier/Table/Item_2").gameObject;
        markView.Item3 = _uiRoot.transform.FindChild("Anim/MarkGroup/Soldier/Table/Item_3").gameObject;
        markView.Btn_Back = _uiRoot.transform.FindChild("Anim/MarkGroup/Back").gameObject;
        Btn_Close = _uiRoot.transform.FindChild("Anim/Close").gameObject.GetComponent<UIButton>();
        Spt_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
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
