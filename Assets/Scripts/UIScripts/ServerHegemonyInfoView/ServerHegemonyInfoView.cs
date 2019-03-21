using UnityEngine;
using System;
using System.Collections;

public class ServerHegemonyInfoView
{
    public static string UIName = "ServerHegemonyInfoView";
    public GameObject _uiRoot;
    public UISprite Spt_ViewMask;
    public UILabel Lbl_Title;
    public UIGrid Grd_EnemyGrid;
    public GameObject Gobj_EnemyInfoComp;
    public UIButton Btn_ReadyBattle;
    public GameObject Gobj_PlayerInfoComp;
    public UILabel Lbl_Name;
    public UILabel Lbl_PlayerName;
    public UILabel Lbl_UnionName;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ServerHegemonyInfoView");
        Spt_ViewMask = _uiRoot.transform.FindChild("ViewMask").gameObject.GetComponent<UISprite>();
        Lbl_Title = _uiRoot.transform.FindChild("Anim/StageInfoGroup/BG/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        Grd_EnemyGrid = _uiRoot.transform.FindChild("Anim/StageInfoGroup/EnemyGroup/EnemyGrid").gameObject.GetComponent<UIGrid>();
        Gobj_EnemyInfoComp = _uiRoot.transform.FindChild("Anim/StageInfoGroup/EnemyGroup/EnemyGrid/gobj_EnemyInfoComp").gameObject;
        Btn_ReadyBattle = _uiRoot.transform.FindChild("Anim/StageInfoGroup/ReadyBattle").gameObject.GetComponent<UIButton>();
        Gobj_PlayerInfoComp = _uiRoot.transform.FindChild("Anim/StageInfoGroup/Information/gobj_PlayerInfoComp").gameObject;
        Lbl_PlayerName = _uiRoot.transform.FindChild("Anim/StageInfoGroup/Information/Name/Label").gameObject.GetComponent<UILabel>();
        Lbl_UnionName = _uiRoot.transform.FindChild("Anim/StageInfoGroup/Information/Union/Label").gameObject.GetComponent<UILabel>();
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
