using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;

public class UnionReadinessItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_ItemBGSprite;
    [HideInInspector]public UILabel Lbl_PlayerNameLabel;
    [HideInInspector]public UILabel Lbl_PlayerLvLabel;
    [HideInInspector]public UILabel Lbl_PlayerFightPowerLabel;
    [HideInInspector]public UISprite Spt_SplitSprite;
    [HideInInspector]public UISprite Spt_DecRightSprite;
    [HideInInspector]public UISprite Spt_DecLeftSprite;
    [HideInInspector]public UIGrid Grd_SlotUIGrid;

    [HideInInspector]public UILabel Lbl_SlotLabel_0;
    [HideInInspector]public UILabel Lbl_SlotLabel_1;
    [HideInInspector]public UILabel Lbl_SlotLabel_2;
    [HideInInspector]public UILabel Lbl_SlotLabel_3;
    [HideInInspector]public UILabel Lbl_SlotLabel_4;
    [HideInInspector]public UILabel Lbl_SlotLabel_5;

    [HideInInspector]public UIGrid Grd_SoldiertUIGrid;
    [HideInInspector]public UnionSoldierItem UnionItem_UnionSoldierItem_0;
    [HideInInspector]public UnionSoldierItem UnionItem_UnionSoldierItem_1;
    [HideInInspector]public UnionSoldierItem UnionItem_UnionSoldierItem_2;
    [HideInInspector]public UnionSoldierItem UnionItem_UnionSoldierItem_3;
    [HideInInspector]public UnionSoldierItem UnionItem_UnionSoldierItem_4;
    [HideInInspector]public UnionSoldierItem UnionItem_UnionSoldierItem_5;

    [HideInInspector]public UIButton Btn_EventButton;
    [HideInInspector]public UISprite Spt_BtnEventButtonButtonBG;
    [HideInInspector]public UILabel Lbl_BtnEventButtonEventButtonLabel;

    private List<UnionSoldierItem> _soldierItemList;

    private bool _isSelf = true;
    public bool IsSelf
    {
        set
        {
            _isSelf = value;
            UpdateByIsSelf(_isSelf);
        }
        get
        {
            return _isSelf;
        }
    }

    public void Initialize()
    {
        Spt_ItemBGSprite = transform.FindChild("ItemBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_PlayerNameLabel = transform.FindChild("PlayerNameLabel").gameObject.GetComponent<UILabel>();
        Lbl_PlayerLvLabel = transform.FindChild("PlayerLvLabel").gameObject.GetComponent<UILabel>();
        Lbl_PlayerFightPowerLabel = transform.FindChild("PlayerFightPowerLabel").gameObject.GetComponent<UILabel>();
        Spt_SplitSprite = transform.FindChild("SplitSprite").gameObject.GetComponent<UISprite>();
        Spt_DecRightSprite = transform.FindChild("DecRightSprite").gameObject.GetComponent<UISprite>();
        Spt_DecLeftSprite = transform.FindChild("DecLeftSprite").gameObject.GetComponent<UISprite>();
        Grd_SlotUIGrid = transform.FindChild("SlotUIGrid").gameObject.GetComponent<UIGrid>();
        Lbl_SlotLabel_0 = transform.FindChild("SlotUIGrid/Slot_0/Label").gameObject.GetComponent<UILabel>();
        Lbl_SlotLabel_1 = transform.FindChild("SlotUIGrid/Slot_1/Label").gameObject.GetComponent<UILabel>();
        Lbl_SlotLabel_2 = transform.FindChild("SlotUIGrid/Slot_2/Label").gameObject.GetComponent<UILabel>();
        Lbl_SlotLabel_3 = transform.FindChild("SlotUIGrid/Slot_3/Label").gameObject.GetComponent<UILabel>();
        Lbl_SlotLabel_4 = transform.FindChild("SlotUIGrid/Slot_4/Label").gameObject.GetComponent<UILabel>();
        Lbl_SlotLabel_5 = transform.FindChild("SlotUIGrid/Slot_5/Label").gameObject.GetComponent<UILabel>();
        Grd_SoldiertUIGrid = transform.FindChild("SoldiertUIGrid").gameObject.GetComponent<UIGrid>();

        UnionItem_UnionSoldierItem_0 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_0").gameObject.GetComponent<UnionSoldierItem>();
        if (UnionItem_UnionSoldierItem_0 == null)
            UnionItem_UnionSoldierItem_0 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_0").gameObject.AddComponent<UnionSoldierItem>();

        UnionItem_UnionSoldierItem_1 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_1").gameObject.GetComponent<UnionSoldierItem>();
        if (UnionItem_UnionSoldierItem_1 == null)
            UnionItem_UnionSoldierItem_1 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_1").gameObject.AddComponent<UnionSoldierItem>();

        UnionItem_UnionSoldierItem_2 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_2").gameObject.GetComponent<UnionSoldierItem>();
        if (UnionItem_UnionSoldierItem_2 == null)
            UnionItem_UnionSoldierItem_2 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_2").gameObject.AddComponent<UnionSoldierItem>();

        UnionItem_UnionSoldierItem_3 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_3").gameObject.GetComponent<UnionSoldierItem>();
        if (UnionItem_UnionSoldierItem_3 == null)
            UnionItem_UnionSoldierItem_3 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_3").gameObject.AddComponent<UnionSoldierItem>();

        UnionItem_UnionSoldierItem_4 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_4").gameObject.GetComponent<UnionSoldierItem>();
        if (UnionItem_UnionSoldierItem_4 == null)
            UnionItem_UnionSoldierItem_4 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_4").gameObject.AddComponent<UnionSoldierItem>();

        UnionItem_UnionSoldierItem_5 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_5").gameObject.GetComponent<UnionSoldierItem>();
        if (UnionItem_UnionSoldierItem_5 == null)
            UnionItem_UnionSoldierItem_5 = transform.FindChild("SoldiertUIGrid/UnionSoldierItem_5").gameObject.AddComponent<UnionSoldierItem>();

        Btn_EventButton = transform.FindChild("EventButton").gameObject.GetComponent<UIButton>();
        Spt_BtnEventButtonButtonBG = transform.FindChild("EventButton/ButtonBG").gameObject.GetComponent<UISprite>();
        Lbl_BtnEventButtonEventButtonLabel = transform.FindChild("EventButton/EventButtonLabel").gameObject.GetComponent<UILabel>();
        SetLabelValues();
        _soldierItemList = new List<UnionSoldierItem>() { 
            UnionItem_UnionSoldierItem_0, 
            UnionItem_UnionSoldierItem_1, 
            UnionItem_UnionSoldierItem_2, 
            UnionItem_UnionSoldierItem_3, 
            UnionItem_UnionSoldierItem_4, 
            UnionItem_UnionSoldierItem_5 
        };
        UnionItem_UnionSoldierItem_0.Initialize();
        UnionItem_UnionSoldierItem_1.Initialize();
        UnionItem_UnionSoldierItem_2.Initialize();
        UnionItem_UnionSoldierItem_3.Initialize();
        UnionItem_UnionSoldierItem_4.Initialize();
        UnionItem_UnionSoldierItem_5.Initialize();
    }

    void Awake()
    {
        Initialize(); 
    }

    public void SetLabelValues()
    {
        Lbl_PlayerNameLabel.text = "";
        Lbl_PlayerLvLabel.text = string.Format(ConstString.RANK_LABLE_LEVEL, 0);
        Lbl_PlayerFightPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT, 0);
        Lbl_BtnEventButtonEventButtonLabel.text = ConstString.UNIONREADINESS_JOINBATTLA;

        Lbl_SlotLabel_0.text = ConstString.UNIONREADINESS_SLOT;
        Lbl_SlotLabel_1.text = ConstString.UNIONREADINESS_SLOT;
        Lbl_SlotLabel_2.text = ConstString.UNIONREADINESS_SLOT;
        Lbl_SlotLabel_3.text = ConstString.UNIONREADINESS_SLOT;
        Lbl_SlotLabel_4.text = ConstString.UNIONREADINESS_SLOT;
        Lbl_SlotLabel_5.text = ConstString.UNIONREADINESS_SLOT;
    }

    public void UpdateItem(ArenaPlayer playerInfo)
    {
        Lbl_PlayerNameLabel.text = playerInfo.hero.charname;
        Lbl_PlayerLvLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, playerInfo.hero.level);
        Lbl_PlayerFightPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT, playerInfo.combat_power);
        int infoCount = playerInfo.soldiers.Count;
        int itemCount = _soldierItemList.Count;
        for (int i = 0; i < itemCount; i++)
        {
            UnionSoldierItem soldierItem = _soldierItemList[i];
            soldierItem.gameObject.SetActive(true);
            if (i <= infoCount - 1)
            {
                ArenaSoldier arenaSoldier = playerInfo.soldiers[i];
                Soldier soldier = Soldier.createByID(arenaSoldier.soldier.id);
                soldier.Serialize(arenaSoldier.soldier);
                soldierItem.UpdateItem(soldier, arenaSoldier.num);
            }
            else
            {
                soldierItem.Clear();
                soldierItem.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateItem(string name, int level, int combat)
    {
      
    }

    public void UpdateButtonLabel(bool isJoinBattle)
    {
        if (isJoinBattle)
            Lbl_BtnEventButtonEventButtonLabel.text = ConstString.HINT_RIGHTBUTTON_CANCEL;
        else
            Lbl_BtnEventButtonEventButtonLabel.text =ConstString.UNIONREADINESS_JOINBATTLA;
    }

    public void Uninitialize()
    {

    }

    private void UpdateByIsSelf(bool isSelf)
    {
        Debug.Log("isSelf " + isSelf);
        Btn_EventButton.gameObject.SetActive(isSelf);
    }

    public void Clear()
    {
        Lbl_PlayerNameLabel.text = "";
        UpdateButtonLabel(false);
        Lbl_PlayerLvLabel.text = string.Format(ConstString.RANK_LABLE_LEVEL, 0);
        Lbl_PlayerFightPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT, 0);
        foreach (UnionSoldierItem item in _soldierItemList)
        {
            item.Clear();
            item.gameObject.SetActive(false);
        }
    }

}
