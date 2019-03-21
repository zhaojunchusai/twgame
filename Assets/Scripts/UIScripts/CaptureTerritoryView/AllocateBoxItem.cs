using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
public class AllocateBoxItem : MonoBehaviour
{
    [HideInInspector]public UILabel Lbl_Rank;
    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public UILabel Lbl_Score;
    [HideInInspector]public UISprite Spt_AllocateBg;
    [HideInInspector]public UILabel Lbl_SellItemCount;
    [HideInInspector]public UIButton Btn_ReduceButton;
    [HideInInspector]public UISprite Spt_BtnReduceButtonBG;
    [HideInInspector]public UILabel Lbl_BtnReduceButtonFG;
    [HideInInspector]public UIButton Btn_AddButton;
    [HideInInspector]public UISprite Spt_BtnAddButtonBG;
    [HideInInspector]public UILabel Lbl_BtnAddButtonFG;

    private bool _initialized = false;
    private int _num = 0;
    private int _addFactor = 0;
    private CampaignRankInfo _info; 
    public void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;
        Lbl_Rank = transform.FindChild("Rank").gameObject.GetComponent<UILabel>();
        Lbl_Name = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_Score = transform.FindChild("Score").gameObject.GetComponent<UILabel>();
        Spt_AllocateBg = transform.FindChild("Allocate/AllocateBg").gameObject.GetComponent<UISprite>();
        Lbl_SellItemCount = transform.FindChild("Allocate/SellItemCount").gameObject.GetComponent<UILabel>();
        Btn_ReduceButton = transform.FindChild("Allocate/ReduceButton").gameObject.GetComponent<UIButton>();
        Spt_BtnReduceButtonBG = transform.FindChild("Allocate/ReduceButton/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnReduceButtonFG = transform.FindChild("Allocate/ReduceButton/FG").gameObject.GetComponent<UILabel>();
        Btn_AddButton = transform.FindChild("Allocate/AddButton").gameObject.GetComponent<UIButton>();
        Spt_BtnAddButtonBG = transform.FindChild("Allocate/AddButton/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnAddButtonFG = transform.FindChild("Allocate/AddButton/FG").gameObject.GetComponent<UILabel>();
        SetLabelValues();

        UIEventListener.Get(Btn_ReduceButton.gameObject).onClick = ButtonEvent_ReduceButton;
        UIEventListener.Get(Btn_AddButton.gameObject).onClick = ButtonEvent_AddButton;
        UIEventListener.Get(Btn_AddButton.gameObject).onPress = PressAdd;
        UIEventListener.Get(Btn_ReduceButton.gameObject).onPress = PressReduce;
    }

    public void Init(CampaignRankInfo info,int rank)
    {
        Initialize();
        _info = info;
        Lbl_Rank.text = info.score > 0 ? (rank + 1).ToString() : ConstString.NOT_JOIN_FIGHT;
        Lbl_Name.text = string.Format("{0} Lv{1}", info.name, _info.level);
        Lbl_Score.text = info.score.ToString();
    }

    public void CleanNum()
    {
        _num = 0;
        _addFactor = 0;
        SetNum();
    }

    private void SetNum()
    {
        if (_num < 0)
        {
            _num = 0;
            return;
        }

        Lbl_SellItemCount.text = _num.ToString();
        AccidToNum data = new AccidToNum();
        data.accid = _info.id;
        data.num = _num;
        CaptureTerritoryModule.Instance.SetBoxAllocation(data);
    }

    private void CheckNum()
    {
        if (_num < 0)
            _num = 0;
    }

    public void SetLabelValues()
    {
        Lbl_Rank.text = "";
        Lbl_Name.text = "";
        Lbl_Score.text = "";
        Lbl_SellItemCount.text = "0";
        Lbl_BtnReduceButtonFG.text = "-";
        Lbl_BtnAddButtonFG.text = "+";
    }

    private void PressAdd(GameObject go, bool press)
    {
        if (press)
        {
            _addFactor = 1;
            Scheduler.Instance.AddTimer(0.25f, true, NumUpdate);
        }
        else
        {
            _addFactor = 0;
            Scheduler.Instance.RemoveTimer(NumUpdate);
        }
    }

    private void PressReduce(GameObject go, bool press)
    {
        if (press)
        {
            _addFactor = -1;
            Scheduler.Instance.AddTimer(0.25f, true, NumUpdate);
        }
        else
        {
            _addFactor = 0;
            Scheduler.Instance.RemoveTimer(NumUpdate);
        }
    }

    private void NumUpdate()
    {
        if (_addFactor > 0 && CaptureTerritoryModule.Instance.BoxNotEnoughAllocation())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TIP_NO_BOX_TO_ALLOCATE);
            return;
        }
        _num += _addFactor;
        SetNum();
    }

    public void ButtonEvent_ReduceButton(GameObject btn)
    {        
        _num -= 1;
        SetNum();
    }

    public void ButtonEvent_AddButton(GameObject btn)
    {
        if (CaptureTerritoryModule.Instance.BoxNotEnoughAllocation())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TIP_NO_BOX_TO_ALLOCATE);
            return;
        }
        _num += 1;
        SetNum();
    }

    public void Uninitialize()
    {

    }


}
