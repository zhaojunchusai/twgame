using UnityEngine;
using System;
using System.Collections;

public class CaptureTerritoryInfoView
{
    public static string UIName ="CaptureTerritoryInfoView";
    public GameObject _uiRoot;
    public GameObject Gobj_StageInfoGroup;
    public UIButton Btn_Close;
    public UILabel Lbl_TitleLabel;
    public UILabel Lbl_StageDesc;
    public UILabel Lbl_EnemyTip;
    public UIGrid Grd_EnemyGrid;
    public GameObject Gobj_EnemyInfoComp;
    public UILabel Lbl_AwardsTitle;
    public UIGrid Grd_AwardsGrid;
    public GameObject Gobj_AwardsInfoComp;
    public UIButton Btn_ReadyBattle;
    public GameObject Gobj_GateCountGroup;
    public UILabel Lbl_GateCountLabel;
    public GameObject Gobj_IntegralGroup;
    public UILabel Lbl_CorpsName;
    public UILabel Lbl_IntegralNum;
    public UILabel Lbl_MyIntegralNum;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CaptureTerritoryInfoView");
        Gobj_StageInfoGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup").gameObject;
        Btn_Close = _uiRoot.transform.FindChild("gobj_StageInfoGroup/Close").gameObject.GetComponent<UIButton>();
        Lbl_TitleLabel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
        Lbl_StageDesc = _uiRoot.transform.FindChild("gobj_StageInfoGroup/DescGroup/StageDesc").gameObject.GetComponent<UILabel>();
        Lbl_EnemyTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup/EnemyTip").gameObject.GetComponent<UILabel>();
        Grd_EnemyGrid = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup/EnemyGrid").gameObject.GetComponent<UIGrid>();
        Gobj_EnemyInfoComp = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup/EnemyGrid/gobj_EnemyInfoComp").gameObject;
        Lbl_AwardsTitle = _uiRoot.transform.FindChild("gobj_StageInfoGroup/AwardsGroup/AwardsTitle").gameObject.GetComponent<UILabel>();
        Grd_AwardsGrid = _uiRoot.transform.FindChild("gobj_StageInfoGroup/AwardsGroup/AwardsGrid").gameObject.GetComponent<UIGrid>();
        Gobj_AwardsInfoComp = _uiRoot.transform.FindChild("gobj_StageInfoGroup/AwardsGroup/AwardsGrid/gobj_AwardsInfoComp").gameObject;
        Btn_ReadyBattle = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/ReadyBattle").gameObject.GetComponent<UIButton>();
        Gobj_GateCountGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_GateCountGroup").gameObject;
        Lbl_GateCountLabel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_GateCountGroup/GateCountLabel").gameObject.GetComponent<UILabel>();
        Gobj_IntegralGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_IntegralGroup").gameObject;
        Lbl_CorpsName = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_IntegralGroup/gobj_TopRank/CorpsName").gameObject.GetComponent<UILabel>();
        Lbl_IntegralNum = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_IntegralGroup/gobj_Integral/IntegralNum").gameObject.GetComponent<UILabel>();
        Lbl_MyIntegralNum = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_IntegralGroup/gobj_MyIntegral/MyIntegralNum").gameObject.GetComponent<UILabel>();
       
    }

    public void Uninitialize()
    {
        
    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
