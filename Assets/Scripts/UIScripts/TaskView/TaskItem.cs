using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class TaskItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_ItemBG;
    [HideInInspector]public UISprite Spt_LabelBG;
    [HideInInspector]public UILabel Lbl_TitleLb;
    [HideInInspector]public UILabel Lbl_DesLb;
    [HideInInspector]public UILabel Lbl_AwardLb;
    [HideInInspector]public UISprite Spt_IconFrame;
    [HideInInspector]public UISprite Spt_IconBG;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_StateBG;
    [HideInInspector]public UIButton Btn_GetBtn;
    [HideInInspector]public UISprite Spt_BtnGetBtnBG;
    [HideInInspector]public UILabel Lbl_BtnGetBtnLb;
    [HideInInspector]public UILabel Lbl_LimitTime;
    [HideInInspector]public UIButton Btn_GoBtn;
    [HideInInspector]public UISprite Spt_BtnGoBtnBG;
    [HideInInspector]public UILabel Lbl_BtnGoBtnLb;
    [HideInInspector]public UILabel Lbl_ConditionLb;
    [HideInInspector]public UISprite Spt_Pattern;
    [HideInInspector]public UITable Tab_AwardItems;
    [HideInInspector]public MailAttachmentItem Item1;
    [HideInInspector]public MailAttachmentItem Item2;
    [HideInInspector]public MailAttachmentItem Item3;
    [HideInInspector]public MailAttachmentItem Item4;
    public delegate void HandleGotoTarget(string str);
    public delegate void HandleGetTaskAward(uint id);
    public HandleGotoTarget EventGoTotarget;
    public HandleGetTaskAward EventGetTaskAward;
    public bool isInitialize = false;
    private TaskData _taskdata = new TaskData();
    private List<MailAttachmentItem> _awardsItem = new List<MailAttachmentItem>();
    private int _tenthousand = 10000;
    private Color _taskNameUnFinishColor = new Color(227 / 255.0F, 169 / 255.0F, 75 / 255.0F);
    private Color _taskNmaeFinishColor = Color.white;
    private Color _taskDesUnFinishColor = new Color(160 / 255.0f, 139 / 255.0f, 113 / 255.0f);
    public void Initialize()
    {
        Spt_ItemBG = transform.FindChild("ItemBG").gameObject.GetComponent<UISprite>();
        Spt_LabelBG = transform.FindChild("LabelBG").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = transform.FindChild("TitleLb").gameObject.GetComponent<UILabel>();
        Lbl_DesLb = transform.FindChild("DesLb").gameObject.GetComponent<UILabel>();
        Lbl_AwardLb = transform.FindChild("AwardLb").gameObject.GetComponent<UILabel>();
        Spt_IconFrame = transform.FindChild("IconFrame").gameObject.GetComponent<UISprite>();
        Spt_IconBG = transform.FindChild("IconFrame/IconBG").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("IconFrame/Icon").gameObject.GetComponent<UISprite>();
        Spt_StateBG = transform.FindChild("IconFrame/StateBG").gameObject.GetComponent<UISprite>();
        Btn_GetBtn = transform.FindChild("GetBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnGetBtnBG = transform.FindChild("GetBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnGetBtnLb = transform.FindChild("GetBtn/Lb").gameObject.GetComponent<UILabel>();
        Lbl_LimitTime = transform.FindChild("LimitTime").gameObject.GetComponent<UILabel>();
        Btn_GoBtn = transform.FindChild("GoBtn").gameObject.GetComponent<UIButton>();
        Lbl_BtnGoBtnLb = transform.FindChild("GoBtn/Lb").gameObject.GetComponent<UILabel>();
        Spt_BtnGoBtnBG = transform.FindChild("GoBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_ConditionLb = transform.FindChild("ConditionLb").gameObject.GetComponent<UILabel>();
        Spt_Pattern = transform.FindChild("Pattern").gameObject.GetComponent<UISprite>();
        Tab_AwardItems = transform.FindChild("AwardItems").gameObject.GetComponent<UITable>();

        Item1 = GetComponent(Tab_AwardItems.transform.FindChild("Item_1").gameObject);
        Item2 = GetComponent(Tab_AwardItems.transform.FindChild("Item_2").gameObject);
        Item3 = GetComponent(Tab_AwardItems.transform.FindChild("Item_3").gameObject);
        Item4 = GetComponent(Tab_AwardItems.transform.FindChild("Item_4").gameObject);

        UIEventListener.Get(Item1.gameObject).onPress = PressItem;
        UIEventListener.Get(Item2.gameObject).onPress = PressItem;
        UIEventListener.Get(Item3.gameObject).onPress = PressItem;
        UIEventListener.Get(Item4.gameObject).onPress = PressItem;
        //SetLabelValues();
        //if (_awardsItem.Count <= 0 || _awardsItem[0]== null) 
        //{
            Item1.gameObject.SetActive(false);
            Item2.gameObject.SetActive(false);
            Item3.gameObject.SetActive(false);
            Item4.gameObject.SetActive(false);
            _awardsItem.Clear();
            _awardsItem.Add(Item1);
            _awardsItem.Add(Item2);
            _awardsItem.Add(Item3);
            _awardsItem.Add(Item4);
        //}
        UIEventListener.Get(Btn_GetBtn.gameObject).onClick = ButtonEvent_GetBtn;
        UIEventListener.Get(Btn_GoBtn.gameObject).onClick = ButtonEvent_GoBtn;
        isInitialize = true;
    }

    void Awake()
    {
        Initialize(); 
    }

    public void SetLabelValues()
    {
        Lbl_TitleLb.text = "邮件名称";
        Lbl_DesLb.text = "";
        Lbl_AwardLb.text = "奖励:";
        Lbl_BtnGetBtnLb.text = "领取";
        Lbl_LimitTime.text = "时间未到";
        Lbl_BtnGoBtnLb.text = "前往";
        Lbl_BtnGoBtnLb.text = "前往";
        Lbl_ConditionLb.text = "0/1";
    }

    public void SetInfo(fogs.proto.msg.TaskInfo data)
    {
        if (!isInitialize) Initialize();
        if (data == null) return;
        TaskData tempData = ConfigManager.Instance.mTaskConfig.GetTaskDataByID(data.id);
        if (tempData == null) 
        {
            Debug.LogError(data.id);
            return;
        } 
        _taskdata.CopyTo(tempData);
        SetTaskStateBG(data.status);
        SetTaskAwards(tempData.Awards);
        //Debug.LogError(_taskdata.Name + "   " + data.status);
        if (data.status == fogs.proto.msg.TaskStatus.FINISHED)
        {
            if (!Spt_StateBG.gameObject.activeSelf)
                Spt_StateBG.gameObject.SetActive(true);
            CommonFunction.SetSpriteName(Spt_StateBG, GlobalConst.SpriteName.TaskStateFinish);
            Btn_GetBtn.gameObject.SetActive(true);
            Lbl_LimitTime.gameObject.SetActive(false);
            Btn_GoBtn.gameObject.SetActive(false);
            Lbl_ConditionLb.gameObject.SetActive(true);
            switch (tempData.CompleteType)
            {
                case 0:
                    Lbl_ConditionLb.text =string.Format(ConstString.TASK_BRACKETS, GetTenThousandUnit(tempData.LevelJugde) + "/" + GetTenThousandUnit(tempData.LevelJugde));
                    break;
                case 1:
                    Lbl_ConditionLb.text = string.Format(ConstString.TASK_BRACKETS,tempData.LevelJugde.ToString() + "/" + tempData.LevelJugde.ToString());
                    break;
                case 2:
                    Lbl_ConditionLb.text = string.Format(ConstString.TASK_BRACKETS,tempData.LevelJugde.ToString() + "/" + tempData.LevelJugde.ToString());
                    break;
                case 11:
                case 12:
                case 13:
                case 21:
                case 22:
                case 41:
                case 42:
                    Lbl_ConditionLb.text = string.Format(ConstString.TASK_BRACKETS,GetTenThousandUnit(tempData.ValueJugde) + "/" + GetTenThousandUnit(tempData.ValueJugde));
                    break;
                case 31:
                case 32:
                case 33:
                case 34:
                case 43:
                case 44:
                    Lbl_ConditionLb.text = string.Format(ConstString.TASK_BRACKETS,"1/1");
                    break;
            }
        }
        else if (data.status == fogs.proto.msg.TaskStatus.UNFINISHED)
        {
            TalkingDataManager.Instance.OnBegin(data.id.ToString());
            if (Spt_StateBG.gameObject.activeSelf)
                Spt_StateBG.gameObject.SetActive(false);
            Btn_GetBtn.gameObject.SetActive(false);

            if (_taskdata.skip != GlobalConst.COMMON_UNCONDITIONAL)
            {
                if (_taskdata.TimeJugde != GlobalConst.COMMON_UNCONDITIONAL && !CommonFunction.IsInTimeInterval(_taskdata.TimeJugde))
                {
                    Btn_GoBtn.gameObject.SetActive(false);
                    Lbl_LimitTime.gameObject.SetActive(true);
                    Lbl_ConditionLb.gameObject.SetActive(false);
                }
                else
                {
                    Btn_GoBtn.gameObject.SetActive(true);
                    Lbl_LimitTime.gameObject.SetActive(false);
                    Lbl_ConditionLb.gameObject.SetActive(true);
                }

            }
            else
            {
                Btn_GoBtn.gameObject.SetActive(false);
                if (_taskdata.TimeJugde != GlobalConst.COMMON_UNCONDITIONAL && !CommonFunction.IsInTimeInterval(_taskdata.TimeJugde))
                {
                    Lbl_LimitTime.gameObject.SetActive(true);
                    Lbl_ConditionLb.gameObject.SetActive(false);
                }
                else
                {
                    Lbl_ConditionLb.gameObject.SetActive(true);
                    Lbl_LimitTime.gameObject.SetActive(false);
                }
            }
            switch (tempData.CompleteType)
            {
                case 0:
                    if (_taskdata.TimeJugde != GlobalConst.COMMON_UNCONDITIONAL)
                    {
                        Debug.LogError(tempData.ValueJugde);
                        Lbl_ConditionLb.text = "";
                    }
                    else
                    {
                        Lbl_ConditionLb.text =string.Format(ConstString.TASK_BRACKETS, GetTenThousandUnit(data.con_value) + "/" + GetTenThousandUnit(tempData.ValueJugde));
                    }

                    break;
                case 1:
                    Lbl_ConditionLb.text = string.Format(ConstString.TASK_BRACKETS,data.con_value.ToString() + "/" + tempData.LevelJugde.ToString());
                    break;
                case 2:
                    Lbl_ConditionLb.text =string.Format(ConstString.TASK_BRACKETS, data.con_value.ToString() + "/" + tempData.LevelJugde.ToString());
                    break;
                case 11:
                case 12:
                case 13:
                case 21:
                case 22:
                case 41:
                case 42:
                    Lbl_ConditionLb.text =string.Format(ConstString.TASK_BRACKETS, GetTenThousandUnit(data.con_value) + "/" + GetTenThousandUnit(tempData.ValueJugde));
                    break;
                case 31:
                case 32:
                case 33:
                case 34:
                case 43:
                case 44:
                    Lbl_ConditionLb.text = string.Format(ConstString.TASK_BRACKETS,"0/1");
                    break;
            }
        }
        else if (data.status == fogs.proto.msg.TaskStatus.AWARDED)
        {
            if (!Spt_StateBG.gameObject.activeSelf)
                Spt_StateBG.gameObject.SetActive(true);
            CommonFunction.SetSpriteName(Spt_StateBG, GlobalConst.SpriteName.TaskStateReceive);
            Btn_GoBtn.gameObject.SetActive(false);
            Lbl_LimitTime.gameObject.SetActive(false);
            Lbl_ConditionLb.gameObject.SetActive(false);
            Btn_GetBtn.gameObject.SetActive(false);
        }

        if (tempData.CompleteType == 41)
        {
            switch (data.status)
            {
                case fogs.proto.msg.TaskStatus.AWARDED:
                case fogs.proto.msg.TaskStatus.FINISHED:
                    Lbl_TitleLb.text = tempData.Name + CommonFunction.GetDayString(CommonFunction.GetLfetTime((long)PlayerData.Instance.MonthCardDeadline));
                    break;
                case fogs.proto.msg.TaskStatus.UNFINISHED:
                    Lbl_TitleLb.text = tempData.Name;
                    break;
            }
        }
        else
        {
            Lbl_TitleLb.text = tempData.Name;
        }
        Lbl_DesLb.text = tempData.Des;
        Lbl_AwardLb.text = ConstString.TASK_AWARDS;//string.Format(ConstString.TASK_AWARDS, tempData.AwardsDes);
        CommonFunction.SetSpriteName(Spt_Icon, tempData.Icon);
        // Spt_Icon.MakePixelPerfect();
        //Debug.LogError(tempData.Icon);
    }

    public void SetTaskAwards(uint dropid) 
    {
        List<CommonItemData> data = new List<CommonItemData>();
        data.AddRange(CommonFunction.GetCommonItemDataList(dropid));
        if (_taskdata.exp >0)
        {
            CommonItemData tempData = new CommonItemData(GlobalCoefficient.PlayerExpID, (int)_taskdata.exp);
            data.Add(tempData);
        }
        for (int i = 0; i < _awardsItem.Count; i++)
        {
            if (i < data.Count)
            {
                _awardsItem[i].gameObject.SetActive(true);
                _awardsItem[i].UpdateItemInfo(data[i].ID, data[i].Num,false);
            }
            else 
            {
                _awardsItem[i].gameObject.SetActive(false);
            }
        }
        Tab_AwardItems.repositionNow = true;
    }

    protected void PressItem(GameObject go, bool press)
    {
        MailAttachmentItem item = go.GetComponent<MailAttachmentItem>();
        HintManager.Instance.SeeDetail(go, press, item.ID);
    }

    public MailAttachmentItem GetComponent(GameObject go) 
    {
        MailAttachmentItem item = go.GetComponent<MailAttachmentItem>();
        if (item != null)
        {
            return item;
        }
        return go.AddComponent<MailAttachmentItem>();
    }

    public string GetTenThousandUnit(int value)
    {
        if (value >= _tenthousand)
            return string.Format(ConstString.TASK_TENTHOUSAND, value / _tenthousand);
        return value.ToString();

    }

    public void SetTaskStateBG(fogs.proto.msg.TaskStatus state) 
    {
        switch (state)
        {
            case fogs.proto.msg.TaskStatus.FINISHED:
            case fogs.proto.msg.TaskStatus.AWARDED:
                CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_FINISHBG);
                Lbl_TitleLb.color = _taskNmaeFinishColor;
                Lbl_DesLb.color = _taskNmaeFinishColor;
                Lbl_ConditionLb.color = _taskNmaeFinishColor;
                break;
            case fogs.proto.msg.TaskStatus.UNFINISHED:
                CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_UNFINISHBG);
                Lbl_TitleLb.color = _taskNameUnFinishColor;
                Lbl_DesLb.color = _taskDesUnFinishColor;
                Lbl_ConditionLb.color = _taskDesUnFinishColor;
                break;

        }
    }

    public void ButtonEvent_GetBtn(GameObject btn)
    {
        if (EventGetTaskAward != null)
            EventGetTaskAward(_taskdata.ID);
    }

    public void ButtonEvent_GoBtn(GameObject btn)
    {
        if (EventGoTotarget != null)
            EventGoTotarget(_taskdata.skip);
    }

    public void Uninitialize()
    {

    }


}
