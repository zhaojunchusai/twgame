//using UnityEngine;
//using System.Collections.Generic;
//using System;

///* File: LineupSelectPanel.cs
// * Desc: 阵容选择面板
// * Date: 2015-06-03 19:53
// * add by taiwei
// */
//public class LineupSelectPanel : BasePanel
//{
//    private Transform RootObject;
//    public Transform Btn_CloseButton;
//    public UILabel Lbl_TitleLabel;
//    public UIGrid Grd_RedaySoldierGrid;
//    public LoopScrollViewTool scrollviewTool;
//    public UIGrid Grd_OwnSoldierGrid;
//    public UILabel Lbl_LeadershipLimitLabel;
//    public Transform Btn_StartBattleButton;
//    public UILabel Lbl_StartBattleLabel;
//    public Transform Comp_OwnSoldierComp;
//    public Transform Comp_ReadySoldierComp;
//    public UIBoundary Boundary = new UIBoundary();

//    public List<Soldier> readysoldierlist;

//    private StageInfo stageinfo;
//    /// <summary>
//    /// 统御力
//    /// </summary>
//    private int leadership = 0;
//    /// <summary>
//    /// 当前拥有的可出战士兵
//    /// </summary>
//    private Dictionary<GameObject, LineupItemComponent> ownsoldier_dic;

//    /// <summary>
//    /// 已经准备出战队的士兵
//    /// </summary>
//    private Dictionary<GameObject, LineupItemComponent> ready_dic;

//    private bool isInit = false;

//    public override void MyStart()
//    {
//        base.MyStart();
//        Initialize();
//    }
//    private void Initialize()
//    {
//        RootObject = this.transform;
//        Btn_CloseButton = RootObject.FindChild("CloseButton").gameObject.GetComponent<Transform>();
//        Lbl_TitleLabel = RootObject.FindChild("TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
//        Grd_RedaySoldierGrid = RootObject.FindChild("InfoGroup/RedaySoldierGroup/RedaySoldierGrid").gameObject.GetComponent<UIGrid>();
//        Grd_OwnSoldierGrid = RootObject.FindChild("InfoGroup/OwnSoldierGroup/OwnSoldierScrollView/OwnSoldierGrid").gameObject.GetComponent<UIGrid>();
//        scrollviewTool = RootObject.FindChild("InfoGroup/OwnSoldierGroup/OwnSoldierScrollView/OwnSoldierGrid").gameObject.GetComponent<LoopScrollViewTool>();
//        Lbl_LeadershipLimitLabel = RootObject.FindChild("InfoGroup/LeadershipLimitGroup/LeadershipLimitLabel").gameObject.GetComponent<UILabel>();
//        Btn_StartBattleButton = RootObject.FindChild("InfoGroup/StartBattleButton").gameObject.GetComponent<Transform>();
//        Lbl_StartBattleLabel = RootObject.FindChild("InfoGroup/StartBattleButton/StartBattleLabel").gameObject.GetComponent<UILabel>();
//        Comp_OwnSoldierComp = RootObject.FindChild("InfoGroup/OwnSoldierGroup/OwnSoldierScrollView/OwnSoldierGrid/OwnSoldierComp").gameObject.GetComponent<Transform>();
//        Comp_ReadySoldierComp = RootObject.FindChild("InfoGroup/RedaySoldierGroup/RedaySoldierGrid/RedaySoldierComp").gameObject.GetComponent<Transform>();
//        SetLabelValues();
//        SetBoundary();
//        InitView();
//    }

//    private void InitView() 
//    {
//        ownsoldier_dic = new Dictionary<GameObject, LineupItemComponent>();
//        ready_dic = new Dictionary<GameObject, LineupItemComponent>();
//        readysoldierlist = new List<Soldier>();
//        Comp_ReadySoldierComp.gameObject.SetActive(false);
//        Comp_OwnSoldierComp.gameObject.SetActive(false);
//        BtnEventBinding();
//    }

//    public void SetBoundary()
//    {
//        Boundary.left = -512.5f;
//        Boundary.right = 512.5f;
//        Boundary.up = 288f;
//        Boundary.down = -294f;
//    }

//    public void SetLabelValues()
//    {
//        Lbl_TitleLabel.text = "New Label";
//        Lbl_LeadershipLimitLabel.text = "统御力\n999/999";
//        Lbl_StartBattleLabel.text = "出战";
//    }

//    public void UpdatePanelInfo(StageInfo data)
//    {
//        stageinfo = data;
//        UpdateLeadership();
//        UpdateOwnSoldier();
//        UpdateReadySoldier();
//        Lbl_TitleLabel.text = ConstString.GATE_LINEUPPANEL_TITLE;
//    }

//    /// <summary>
//    /// 更新当前拥有的士兵
//    /// </summary>
//    private void UpdateOwnSoldier() 
//    {
//        if (!isInit) // 已经生成过列表则不在需要生成
//        {
//            if (scrollviewTool == null)
//            {
//                scrollviewTool = Grd_OwnSoldierGrid.gameObject.AddComponent<LoopScrollViewTool>();
//            } 
//            scrollviewTool.item = Comp_OwnSoldierComp.gameObject;
//            scrollviewTool.MyStart();
//            scrollviewTool.UpdateItem(PlayerData.Instance._SoldierDepot._soldierList.Count, true);
//                scrollviewTool.SetDelegate(OwnSoldierScroll, OwnSoldierHandle);
//            for (int i = 0; i < scrollviewTool.GetItemList().Count; i++)
//            {
//                UIWidget widget = scrollviewTool.GetItemList()[i];
//                LineupItemComponent comp = new LineupItemComponent();
//                comp.MyStart(widget.gameObject);
//                ownsoldier_dic.Add(widget.gameObject, comp);
//            }
//            isInit = true;
//        }
//        scrollviewTool.Reset(PlayerData.Instance._SoldierDepot._soldierList.Count);
//        for (int i = 0; i < scrollviewTool.GetItemList().Count; i++)
//        {
//            UIWidget widget = scrollviewTool.GetItemList()[i];
//            LineupItemComponent comp = ownsoldier_dic[widget.gameObject];
//            if (comp == null)
//            {
//                comp = new LineupItemComponent();
//                comp.MyStart(widget.gameObject);
//            }
//            if (i < PlayerData.Instance._SoldierDepot._soldierList.Count)
//            {
//                comp.UpdateInfo(PlayerData.Instance._SoldierDepot._soldierList[i]);
//                comp.IsSelect = false;
//            }
//        }
//    }

//    private void DelOwnSoldierData(Soldier soldier) 
//    {
//        foreach (KeyValuePair<GameObject, LineupItemComponent> tmp in ownsoldier_dic) 
//        {
//            LineupItemComponent comp = tmp.Value;
//            if (comp.soldierAtt.Equals(soldier))
//            {
//                comp.IsSelect = false;
//                break;
//            }
//        }
//    }

//    /// <summary>
//    /// 更新准备出站的士兵
//    /// </summary>
//    private void UpdateReadySoldier() 
//    {
//        UpdateLeadership();
//        if (readysoldierlist.Count <= ready_dic.Count)
//        {
//            int index = 0;
//            foreach (KeyValuePair<GameObject, LineupItemComponent> tmp in ready_dic) 
//            {
//                LineupItemComponent comp = tmp.Value;
//                if (index < readysoldierlist.Count)
//                {
//                    comp.UpdateInfo(readysoldierlist[index]);
//                    comp.IsSelect = false;
//                    comp.mRootObject.SetActive(true);
//                }
//                else 
//                {
//                    comp.Clear();
//                    comp.mRootObject.SetActive(false);
//                }
//                index++;
//            }
//        }
//        else 
//        {
//            int ob_count = ready_dic.Count;   //已经生成的物件数量
//            int data_index = 0;
//            foreach (KeyValuePair<GameObject, LineupItemComponent> tmp in ready_dic)
//            {
//                LineupItemComponent comp = tmp.Value;
//                comp.UpdateInfo(readysoldierlist[data_index]);
//                comp.IsSelect = false;
//                comp.mRootObject.SetActive(true);
//                data_index++;
//            }
//            for (int i = ob_count; i < readysoldierlist.Count; i++) 
//            {
//                GameObject go = CommonFunction.InstantiateObject(Comp_ReadySoldierComp.gameObject,Grd_RedaySoldierGrid.transform);
//                LineupItemComponent comp = new LineupItemComponent();
//                comp.MyStart(go);
//                comp.UpdateInfo(readysoldierlist[i]);
//                UIEventListener.Get(go).onClick = ButtonEvent_ReadySoldier;
//                go.SetActive(true);
//                ready_dic.Add(go,comp);                
//            }
//            Grd_RedaySoldierGrid.Reposition();
//        }
//    }

//    /// <summary>
//    /// 更新准备出战的士兵数据 默认删除已出战士兵
//    /// 返回false 则说明已达出战士兵上线
//    /// </summary>
//    private bool UpdateReadySoldierData(Soldier data,bool isAdd = false) 
//    {
//        if (isAdd)
//        {
//            if (readysoldierlist.Count >= GlobalCoefficient.LineupSoldierLimit)
//            {
//                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,ConstString.GATE_LINEUPPANEL_SOLDIERLIMIT);
//                return false;
//            }
//            else 
//            {
//                readysoldierlist.Add(data);
//                return true;
//            }
//        }
//        else 
//        {
//            List<Soldier> _list = new List<Soldier>();
//            for (int i = 0; i < readysoldierlist.Count; i++) 
//            {
//                if (readysoldierlist[i].uId != data.uId) 
//                {
//                    _list.Add(readysoldierlist[i]);
//                }
//            }
//            readysoldierlist.Clear();
//            readysoldierlist.AddRange(_list);
//            return true;
//        }
//    }
//    /// <summary>
//    /// 更新统御力
//    /// </summary>
//    private void UpdateLeadership() 
//    {
//        leadership = 0;
//        for (int i = 0; i < readysoldierlist.Count; i++) 
//        {
//            leadership += readysoldierlist[i].Att.leaderShip;
//        }
//        //Debug.LogWarning("PlayerData.Instance._Level: " + PlayerData.Instance._Level);
//        Lbl_LeadershipLimitLabel.text = string.Format(ConstString.GATE_LINEUPANEL_LEADERSHIP, leadership, ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(PlayerData.Instance._Level).Leadership);
//    }

//    #region Button Event
//    /// <summary>
//    /// 当前拥有的士兵点击事件
//    /// </summary>
//    public void OwnSoldierHandle(GameObject go,int index) 
//    {
//        LineupItemComponent comp = ownsoldier_dic[go];
//        if (comp == null) 
//        {
//            Debug.LogError("can not get LineupItemComponent by:" + go.name);
//            return;
//        } 
//        if (UpdateReadySoldierData(comp.soldierAtt, !comp.IsSelect))
//        {
//            comp.IsSelect = !comp.IsSelect;
//            UpdateReadySoldier();
//        }
//    }

//    /// <summary>
//    /// 当前拥有的士兵滑动 更新数据
//    /// </summary>
//    public void OwnSoldierScroll(GameObject go)
//    {
//        LineupItemComponent _item = ownsoldier_dic[go];
//        LoopScrollViewData _scroll = go.GetComponent<LoopScrollViewData>();
//        if (_item == null || _scroll == null) return;
//        if (_scroll.index < PlayerData.Instance._SoldierDepot._soldierList.Count)
//        {
//            _item.Clear();
//            _item.UpdateInfo(PlayerData.Instance._SoldierDepot._soldierList[_scroll.index]);
//        }
//    }

//    /// <summary>
//    /// 已准备出站的士兵
//    /// </summary>
//    public void ButtonEvent_ReadySoldier(GameObject go)
//    {
//        LineupItemComponent comp = ready_dic[go];
//        if (comp == null)
//        {
//            Debug.LogError("can not get LineupItemComponent by:" + go.name);
//            return;
//        }
//        if (UpdateReadySoldierData(comp.soldierAtt, false))
//        {
//            DelOwnSoldierData(comp.soldierAtt);
//            UpdateReadySoldier();
//        }
//    }

//    public void ButtonEvent_CloseButton(GameObject btn) 
//    {
//        SetVisible(false);
//        Clear();
//    }

//    public void ButtonEvent_StartBattle(GameObject btn)
//    {
//        if (leadership > ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(PlayerData.Instance._Level).Leadership)
//        {
//            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,ConstString.GATE_LINEUPPANEL_OVERLEADERSHIP);
//            return;
//        }
//        List<UInt64> list = new List<UInt64>();
//        for (int i = 0; i < readysoldierlist.Count; i++)
//        {
//            list.Add(readysoldierlist[i].uId);
//        }
//        if (readysoldierlist.Count == 0)
//        {
//            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo,ConstString.GATE_LINEUPPANEL_NOSOLDIER,
//                () =>
//                {
//                    FightRelatedModule.Instance.SendMajorMapStart(stageinfo.GateID, (uint)readysoldierlist.Count, list);
//                },
//                null,
//                ConstString.HINT_LEFTBUTTON_GOON, 
//                ConstString.HINT_RIGHTBUTTON_CANCEL
//               );
//            return;
//        }
        
//        FightRelatedModule.Instance.SendMajorMapStart(stageinfo.GateID, (uint)readysoldierlist.Count, list);
//    }
//    #endregion

//    public void BtnEventBinding()
//    {
//        UIEventListener.Get(Btn_CloseButton.gameObject).onClick = ButtonEvent_CloseButton;
//        UIEventListener.Get(Btn_StartBattleButton.gameObject).onClick = ButtonEvent_StartBattle;
//    }

//    public void BtnRegist()
//    {
       
//        ButtonManager.Instance.RegistButton(Btn_CloseButton.gameObject);
//        ButtonManager.Instance.RegistButton(Btn_StartBattleButton.gameObject);
//    }

//    public void BtnUnRegist()
//    {
        
//        ButtonManager.Instance.UnregistButton(Btn_CloseButton.gameObject);
//        ButtonManager.Instance.UnregistButton(Btn_StartBattleButton.gameObject);
//    }

//    public void Uninitialize()
//    {

//    }
//    public void SetVisible(bool isVisible)
//    {
//         RootObject.gameObject.SetActive(isVisible);
//    }


//    public override void Clear()
//    {
//        base.Clear();
//        readysoldierlist.Clear();
//    }
//}
