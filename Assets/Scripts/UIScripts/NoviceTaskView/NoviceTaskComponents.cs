using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
public class NoviceTaskComponent : BaseComponent
{
    private GameObject Gobj_NoviceTaskItem;
    private UISprite Spt_ItemBG_1;
    private UILabel Lbl_NameLabel;
    private UILabel Lbl_ProgressLabel;
    private UISprite Spt_Notify;
    private UISprite Spt_ChooseSprite;

    private NoviceTaskInfo noviceTaskData;
    public NoviceTaskInfo NoviceTaskData
    {
        get
        {
            return noviceTaskData;
        }
    }
    private NewHandTask noviceTaskPOD;
    public NewHandTask NoviceTaskPOD
    {
        get
        {
            return noviceTaskPOD;
        }
        set
        {
            noviceTaskPOD = value;
        }
    }

    public bool IsSelect
    {
        set
        {
            Spt_ChooseSprite.enabled = value;
        }
        get
        {
            return Spt_ChooseSprite.enabled;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_ItemBG_1 = mRootObject.transform.FindChild("ItemBG_1").gameObject.GetComponent<UISprite>();
        Lbl_NameLabel = mRootObject.transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_ProgressLabel = mRootObject.transform.FindChild("ProgressLabel").gameObject.GetComponent<UILabel>();
        Spt_Notify = mRootObject.transform.FindChild("Notify").gameObject.GetComponent<UISprite>();
        Spt_ChooseSprite = mRootObject.transform.FindChild("ChooseSprite").gameObject.GetComponent<UISprite>();
        IsSelect = false;
    }

    public void UpdateCompInfo(NoviceTaskInfo info, NewHandTask pod)
    {
        noviceTaskData = info;
        noviceTaskPOD = pod;
        if ((info == null) || (pod == null))
        {
            Clear();
            return;
        }
        Lbl_NameLabel.text = NoviceTaskData.taskName;
        if (pod.finish_num >= NoviceTaskData.subTasks.Length)
        {
            Lbl_ProgressLabel.text = "[359f21]" + NoviceTaskPOD.finish_num.ToString() + "/" + NoviceTaskData.subTasks.Length.ToString() + "[-]";
        }
        else
        {
            Lbl_ProgressLabel.text = "[c4ad87]" + NoviceTaskPOD.finish_num.ToString() + "/" + NoviceTaskData.subTasks.Length.ToString() + "[-]";
        }
        UpdateNotifyStatus();
    }

    public void UpdateNotifyStatus()
    {
        bool status = false;
        for (int i = 0; i < noviceTaskPOD.tasks.Count; i++)
        {
            TaskInfo taskInfo = noviceTaskPOD.tasks[i];
            if (taskInfo == null)
                continue;
            if (taskInfo.status == TaskStatus.FINISHED)
            {
                status = true;
                break;
            }
        }
        Spt_Notify.enabled = status;
    }

    public override void Clear()
    {
        base.Clear();
    }
}



public class NoviceSubtasksComponent : BaseComponent
{
    private UISprite Spt_AwardsBGSprite;
    private UIButton Btn_GetAwardButton;
    private UISprite Spt_BtnGetAwardButtonBGSprite;
    private UILabel Lbl_BtnLabel;
    private UIGrid Grd_ItemsGrid;
    private GameObject Gobj_Item;
    private UILabel Lbl_DescLabel;
    private NoviceSubtasksInfo subtasksData;
    public NoviceSubtasksInfo SubtasksData
    {
        get
        {
            return subtasksData;
        }

    }

    private TaskInfo subtasksPOD;
    public TaskInfo SubtasksPOD
    {
        get
        {
            return subtasksPOD;
        }
        set
        {
            subtasksPOD = value;
        }
    }

    private List<NoviceSubtasksAwardsComponent> awardList;


    protected Color mLabelNormalColor = new Color(111 / 255f, 52 / 255f, 14 / 255f);
    protected Color mOutlineColor = new Color(234 / 255f, 201 / 255f, 93 / 255f);
    protected Color mLabelDisableColor = new Color(53f / 255f, 53f / 255f, 53f / 255f);
    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_AwardsBGSprite = mRootObject.transform.FindChild("AwardsBGSprite").gameObject.GetComponent<UISprite>();
        Btn_GetAwardButton = mRootObject.transform.FindChild("GetAwardButton").gameObject.GetComponent<UIButton>();
        Spt_BtnGetAwardButtonBGSprite = mRootObject.transform.FindChild("GetAwardButton/BGSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnLabel = mRootObject.transform.FindChild("GetAwardButton/FGLabel").gameObject.GetComponent<UILabel>();
        Grd_ItemsGrid = mRootObject.transform.FindChild("ItemsGrid").gameObject.GetComponent<UIGrid>();
        Gobj_Item = mRootObject.transform.FindChild("ItemsGrid/gobj_Item").gameObject;
        Lbl_DescLabel = mRootObject.transform.FindChild("DescLabel").gameObject.GetComponent<UILabel>();
        BtnEventBinding();
        Gobj_Item.SetActive(false);
    }

    private void BtnEventBinding()
    {
        UIEventListener.Get(Btn_GetAwardButton.gameObject).onClick = ButtonEvent_GetAwardButton;
    }

    public void UpdateCompInfo(NoviceSubtasksInfo info, TaskInfo pod)
    {
        subtasksData = info;
        subtasksPOD = pod;
        if (info == null || pod == null)
        {
            Clear();
            return;
        }
        UpdateAwardItems();
        UpdateAwardBtnStatus();
        Lbl_DescLabel.text = SubtasksData.desc;
    }

    public void UpdateAwardBtnStatus()
    {
        switch (subtasksPOD.status)
        {
            case TaskStatus.UNFINISHED:
                Spt_BtnGetAwardButtonBGSprite.color = Color.black;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_GETAWARD;
                Lbl_BtnLabel.color = mLabelDisableColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
                break;
            case TaskStatus.FINISHED:
                Spt_BtnGetAwardButtonBGSprite.color = Color.white;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_GETAWARD;
                Lbl_BtnLabel.color = mLabelNormalColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.Outline;
                Lbl_BtnLabel.effectColor = mOutlineColor;
                break;
            case TaskStatus.AWARDED:
                Spt_BtnGetAwardButtonBGSprite.color = Color.black;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                Lbl_BtnLabel.color = mLabelDisableColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
                break;
        }
    }

    private void UpdateAwardItems()
    {
        List<CommonItemData> itemDatas = CommonFunction.GetCommonItemDataList((uint)subtasksData.dropID);
        if (awardList == null)
            awardList = new List<NoviceSubtasksAwardsComponent>();
        int itemCount = awardList.Count;
        if (itemDatas.Count < itemCount)
        {
            for (int i = itemDatas.Count; i < itemCount; i++)
            {
                NoviceSubtasksAwardsComponent comp = awardList[i];
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < itemDatas.Count; i++)
        {
            CommonItemData data = itemDatas[i];
            NoviceSubtasksAwardsComponent comp = null;
            if (i < itemCount)
            {
                comp = awardList[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(Gobj_Item, Grd_ItemsGrid.transform);
                go.name = "award_" + i.ToString();
                comp = new NoviceSubtasksAwardsComponent();
                comp.MyStart(go);
                awardList.Add(comp);
            }
            if (comp == null)
                continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(data);
        }
        Grd_ItemsGrid.repositionNow = true;
    }


    private void ButtonEvent_GetAwardButton(GameObject btn)
    {
        if (SubtasksPOD.status != TaskStatus.FINISHED)
            return;
        if (OnSelectObj != null)
            OnSelectObj(this);
    }

    public override void Clear()
    {
        base.Clear();
    }
}

public class NoviceSubtasksAwardsComponent : BaseComponent
{
    private UISprite Spt_FreamSprite;
    private UISprite Spt_Mark;
    private UISprite Spt_IconBGSprite;
    private UISprite Spt_IconSprite;
    private UILabel Lbl_NumberLabel;

    private CommonItemData awardData;
    public CommonItemData AwardData
    {
        get
        {
            return awardData;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_FreamSprite = mRootObject.transform.FindChild("FreamSprite").gameObject.GetComponent<UISprite>();
        Spt_Mark = mRootObject.transform.FindChild("FreamSprite/Mark").gameObject.GetComponent<UISprite>();
        Spt_IconBGSprite = mRootObject.transform.FindChild("IconBGSprite").gameObject.GetComponent<UISprite>();
        Spt_IconSprite = mRootObject.transform.FindChild("IconSprite").gameObject.GetComponent<UISprite>();
        Lbl_NumberLabel = mRootObject.transform.FindChild("NumberLabel").gameObject.GetComponent<UILabel>();
        UIEventListener.Get(mRootObject).onPress = PressEvent_GameObject;
    }

    private void PressEvent_GameObject(GameObject go, bool state)
    {
        HintManager.Instance.SeeDetail(mRootObject, state, AwardData.ID);
    }

    public void UpdateCompInfo(CommonItemData data)
    {
        awardData = data;
        if (data == null)
        {
            Clear();
            return;
        }
        CommonFunction.SetQualitySprite(Spt_FreamSprite, data.Quality, Spt_IconBGSprite);
        CommonFunction.SetSpriteName(Spt_IconSprite, data.Icon);
        if (data.SubType == ItemTypeEnum.EquipChip)
        {
            Spt_Mark.enabled = true;
            CommonFunction.SetSpriteName(Spt_Mark, GlobalConst.SpriteName.MarkEquipChip);
            Spt_Mark.MakePixelPerfect();
        }
        else if (data.SubType == ItemTypeEnum.SoldierChip)
        {
            Spt_Mark.enabled = true;
            CommonFunction.SetSpriteName(Spt_Mark, GlobalConst.SpriteName.MarkSoldierChip);
            Spt_Mark.MakePixelPerfect();
        }
        else
        {
            Spt_Mark.enabled = false;
        }

        if (data.Num / 10000 > 0)
        {
            Lbl_NumberLabel.text = "x" + string.Format(ConstString.TASK_TENTHOUSAND, (data.Num / 10000).ToString());
        }
        else
        {
            Lbl_NumberLabel.text = "x" + data.Num.ToString();
        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}
