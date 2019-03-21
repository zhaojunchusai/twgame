using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class LivenessItem : MonoBehaviour
{
    [HideInInspector]public GameObject Gobj_Completed;
    [HideInInspector]public UISprite Spt_gobj_Completed;
    [HideInInspector]public UISprite Spt_Completed;
    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UISprite Spt_FG;
    [HideInInspector]public UILabel Lbl_Title;
    [HideInInspector]public UILabel Lbl_PointCount;
    [HideInInspector]public UILabel Lbl_Desc;
    [HideInInspector]public UISprite Spt_IconFrame;
    [HideInInspector]public UISprite Spt_IconBG;
    [HideInInspector]public UISprite Spt_Icon;
    private Color _finishColor = Color.white;
    private Color _unFinishColor = new Color(160/255.0f,139/255.0f,113/255.0f);
    private Color _titleUnFinishColor = new Color(222 / 255.0f, 169 / 255.0f, 75 / 255.0f);
    private LivenessData _data;
    private LivenessTask _task;
    private bool _initialized = false;
    public void Initialize()
    {
        if(_initialized)
            return;
        _initialized = true;

        Gobj_Completed = transform.FindChild("gobj_Completed").gameObject;
        Spt_gobj_Completed = transform.FindChild("gobj_Completed").gameObject.GetComponent<UISprite>();
        Spt_Completed = transform.FindChild("gobj_Completed/Completed").gameObject.GetComponent<UISprite>();
        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Spt_FG = transform.FindChild("FG").gameObject.GetComponent<UISprite>();
        Lbl_Title = transform.FindChild("Title").gameObject.GetComponent<UILabel>();
        Lbl_PointCount = transform.FindChild("PointCount").gameObject.GetComponent<UILabel>();
        Lbl_Desc = transform.FindChild("Desc").gameObject.GetComponent<UILabel>();
        Spt_IconFrame = transform.FindChild("IconFrame").gameObject.GetComponent<UISprite>();
        Spt_IconBG = transform.FindChild("IconFrame/IconBG").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("IconFrame/Icon").gameObject.GetComponent<UISprite>();
        SetLabelValues();
        UIEventListener.Get(Spt_BG.gameObject).onClick = ClickItem;
    }

    public void InitItem(LivenessTask task)
    {
        Initialize();
        _task = task;
        _data = ConfigManager.Instance.mLivenessConfig.GetLivenessDataByID(task.id);
        Lbl_PointCount.text = string.Format(ConstString.FORMAT_LIVENESS_POINT, _data.LivenessPoint);
        Lbl_Desc.text = _data.Desc;
        CommonFunction.SetSpriteName(Spt_Icon, _data.Icon);
        UpdateTask();
    }

    private void UpdateTask()
    {
        Lbl_Title.text = string.Format(ConstString.FORMAT_LIVENESS_TASK_NAME, _data.Name, _task.num, _data.TimesLimit);
        if(_task.num >= _data.TimesLimit)
        {
            Gobj_Completed.SetActive(true);
            CommonFunction.SetSpriteName(Spt_BG, GlobalConst.SpriteName.TASK_FINISHBG);
            Lbl_Title.color = _finishColor;
            Lbl_PointCount.color = _finishColor;
        }
        else
        {
            Gobj_Completed.SetActive(false);
            Lbl_Title.color = _titleUnFinishColor;
            Lbl_PointCount.color = _unFinishColor;
            CommonFunction.SetSpriteName(Spt_BG, GlobalConst.SpriteName.TASK_UNFINISHBG);
        }

    }

    private void ClickItem(GameObject go)
    {
        CommonFunction.OpenTargetView(_data.OpenUI);
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = "";
        Lbl_PointCount.text = "";
        Lbl_Desc.text = "";
    }

    public void Uninitialize()
    {

    }


}
