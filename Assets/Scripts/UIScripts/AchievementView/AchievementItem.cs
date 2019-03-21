using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;

public class AchievementItem : MonoBehaviour{

    #region  隐藏UI变量
    [HideInInspector]
    public UISprite Spt_ItemBG;
    //public UISprite Spt_LabelBG;
    [HideInInspector]
    public UILabel Lbl_TitleLb;
    [HideInInspector]
    public UILabel Lbl_DesLb;
    [HideInInspector]
    public UISprite Spt_StateBG;
    //[HideInInspector]
    //public UISprite Spt_IconFrame;
    //[HideInInspector]
    //public UISprite Spt_IconBG;
    [HideInInspector]
    public UISprite Spt_Icon;
    [HideInInspector]
    public UIButton Btn_GetBtn;
    //public UISprite Spt_BtnGetBtnBG;
    //public UILabel Lbl_BtnGetBtnLb;
    [HideInInspector]
    public UILabel Lbl_ConditionLb;
    //public UISprite Spt_Pattern;
    [HideInInspector]
    public UILabel Lbl_AwardLb;
    [HideInInspector]
    public UISprite Spt_FreamSprite;
    [HideInInspector]
    public UISprite Spt_IconBGSprite;
    [HideInInspector]
    public UISprite Spt_IconSprite;
    [HideInInspector]
    public UILabel Lbl_AwardDesc;
    [HideInInspector]
    public UILabel Lbl_ConstTimeDesc;
    [HideInInspector]
    public UILabel Lbl_TitleDesc;

    //[HideInInspector]
    //public GameObject Gobj_Item;

    #endregion

    private Color _nameUnFinishColor = new Color(227 / 255.0F, 169 / 255.0F, 75 / 255.0F);
    private Color _allFinishColor = Color.white;
    private Color _descUnFinishColor = new Color(160 / 255.0f, 139 / 255.0f, 113 / 255.0f);
    public int achievementID;
    private int reset_time;

    public delegate void HandleGetAchievementAward(int id);
    public HandleGetAchievementAward EventGetAchievementAward;


    public void Initialize()
    {
        Spt_ItemBG = transform.FindChild("ItemBG").gameObject.GetComponent<UISprite>();
        //Spt_LabelBG = transform.FindChild("LabelBG").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = transform.FindChild("TitleLb").gameObject.GetComponent<UILabel>();
        Lbl_DesLb = transform.FindChild("DesLb").gameObject.GetComponent<UILabel>();
        Lbl_TitleDesc = transform.FindChild("TitleDesc").gameObject.GetComponent<UILabel>();

        //Spt_IconFrame = transform.FindChild("IconFrame").gameObject.GetComponent<UISprite>();
        //Spt_IconBG = transform.FindChild("IconFrame/IconBG").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("IconFrame/Icon").gameObject.GetComponent<UISprite>();
        Spt_StateBG = transform.FindChild("StateBG").gameObject.GetComponent<UISprite>();

        Btn_GetBtn = transform.FindChild("GetBtn").gameObject.GetComponent<UIButton>();
        //Spt_BtnGetBtnBG = transform.FindChild("GetBtn/BG").gameObject.GetComponent<UISprite>();
        //Lbl_BtnGetBtnLb = transform.FindChild("GetBtn/Lb").gameObject.GetComponent<UILabel>();

        Lbl_ConditionLb = transform.FindChild("ConditionLb").gameObject.GetComponent<UILabel>();
        //Spt_Pattern = transform.FindChild("Pattern").gameObject.GetComponent<UISprite>();

        //Gobj_Item = transform.FindChild("AwardGroup/Item").gameObject;
        Lbl_AwardLb = transform.FindChild("AwardGroup/AwardLb").gameObject.GetComponent<UILabel>();
        Spt_FreamSprite = transform.FindChild("AwardGroup/Item/FreamSprite").gameObject.GetComponent<UISprite>();
        Spt_IconBGSprite = transform.FindChild("AwardGroup/Item/IconBGSprite").gameObject.GetComponent<UISprite>();
        Spt_IconSprite = transform.FindChild("AwardGroup/Item/IconSprite").gameObject.GetComponent<UISprite>();
        Lbl_AwardDesc = transform.FindChild("AwardGroup/AwardDesc").gameObject.GetComponent<UILabel>();
        Lbl_ConstTimeDesc = transform.FindChild("AwardGroup/ConstTimeDesc").gameObject.GetComponent<UILabel>();

        //SetLabelValues();

        UIEventListener.Get(Btn_GetBtn.gameObject).onClick = ButtonEvent_GetBtn;
    }
    private void SetLabelValues()
    {
        Lbl_TitleLb.text = "全服霸主";
        Lbl_DesLb.text = "成爲全服霸主";
        //Lbl_BtnGetBtnLb.text = "領取";
        Lbl_ConditionLb.text = "0/1";
        //Lbl_AwardLb.text = "獎勵:";
        Lbl_AwardDesc.text = "解鎖頭像";
        Lbl_ConstTimeDesc.text = "持續72小時";
        Lbl_TitleDesc.text = "【999:59:59】後重置";
        //Lbl_TitleLb.text = "成就";
    }
    public void Uninitialize()
    {

    }

    void Awake()
    {
        Initialize();
    }
    /// <summary>
    /// 根据成就状态设置信息
    /// </summary>
    /// <param name="data"></param>
    public void SetInfo(Achievementinfo data)
    {
        if (data != null)
        {
            achievementID = data.id;
            AchievementItemData tmp = ConfigManager.Instance.mAchievementConfig.FindDataByID(data.id);
            if (tmp != null)
            {
                SetLabelAndColor(data);

                Lbl_TitleLb.text = tmp.name;
                Lbl_DesLb.text = tmp.desc;

                if (tmp.award_type == 1)
                {
                    Lbl_AwardDesc.text = ConstString.ACHIEVEMENT_AWARDDESC_ICON;
                    Spt_IconSprite.gameObject.SetActive(true);
                    //Spt_FreamSprite.gameObject.SetActive(false);
                    CommonFunction.SetSpriteName(Spt_IconSprite, ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitByID((uint)tmp.award_id).icon);
                    string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, GlobalConst.SYSTEMFRAME);
                    CommonFunction.SetSpriteName(Spt_FreamSprite, Frame_A);
                }
                else if(tmp.award_type == 2){
                    Lbl_AwardDesc.text = ConstString.ACHIEVEMENT_AWARDDESC_FRAME;
                    Spt_IconSprite.gameObject.SetActive(false);
                    //Spt_FreamSprite.gameObject.SetActive(true);
                    string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, tmp.award_id);
                    CommonFunction.SetSpriteName(Spt_FreamSprite, Frame_A);
                }
                else
                {
                    Debug.LogError("award type is wrong");
                }

                CommonFunction.SetSpriteName(Spt_Icon, tmp.icon);
                StopTimeUpdate();
                switch (data.status)
                {
                    case 0:
                        CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_FINISHBG);
                        Spt_StateBG.gameObject.SetActive(true);
                        Btn_GetBtn.gameObject.SetActive(false);
                        Lbl_ConditionLb.gameObject.SetActive(false);
                        Lbl_ConstTimeDesc.gameObject.SetActive(false);
                        Lbl_TitleDesc.gameObject.SetActive(false);
                        break;
                    case 1:
                        //CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_FINISHBG);
                        Spt_StateBG.gameObject.SetActive(false);
                        Btn_GetBtn.gameObject.SetActive(true);
                        SetCourseLabel(true, tmp.id, data.cur_course, tmp.final_course);
                        SetTypeLabel(tmp.type, tmp.award_type, tmp.award_id);
                        break;
                    case 2:
                        //CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_UNFINISHBG);
                        Spt_StateBG.gameObject.SetActive(false);
                        Btn_GetBtn.gameObject.SetActive(false);
                        SetCourseLabel(true, tmp.id, data.cur_course, tmp.final_course);
                        SetTypeLabel(tmp.type, tmp.award_type, tmp.award_id);
                        break;
                    case 3:
                        //CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_FINISHBG);
                        Spt_StateBG.gameObject.SetActive(true);
                        Btn_GetBtn.gameObject.SetActive(false);
                        Lbl_ConditionLb.gameObject.SetActive(false);
                        Lbl_ConstTimeDesc.gameObject.SetActive(false);
                        Lbl_TitleDesc.gameObject.SetActive(true);
                        reset_time = data.resettime;
                        //Lbl_TitleDesc.text = string.Format(ConstString.ACHIEVEMENT_TIMECOUNTER, CommonFunction.GetTimeString(data.resettime - Main.mTime));
                        Lbl_ConstTimeDesc.gameObject.SetActive(true);
                        SetAwardLabel(tmp.award_type, tmp.award_id);
                        TimeUpdate();
                        Scheduler.Instance.AddTimer(1, true, TimeUpdate);
                        break;
                }
            }
            else
            {
                Debug.LogError("config info error"+ achievementID);
            }

            //Debug.LogError("id: " + achievementID + "~~~~~~~ name: " + tmp.name + "~~~~~~~~~ desc: " + tmp.desc);
        }
        else
        {
            Debug.LogError("achievementinfo lost");
        }
        
    }
    /// <summary>
    /// 文本颜色设置
    /// </summary>
    /// <param name="data"></param>
    private void SetLabelAndColor(Achievementinfo data)
    {
        if (data.status == 0 || data.status == 2)
        {
            CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_UNFINISHBG);
            Lbl_TitleLb.color = _nameUnFinishColor;
            Lbl_DesLb.color = _descUnFinishColor;
            Lbl_ConditionLb.color = _nameUnFinishColor;
            //Lbl_AwardLb.color = _nameUnFinishColor;
            //Lbl_AwardDesc.color = _nameUnFinishColor;
            //Lbl_ConstTimeDesc.color = _nameUnFinishColor;
            Lbl_TitleDesc.color = _nameUnFinishColor;
        }
        else if(data.status == 1 || data.status == 3)
        {
            CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_FINISHBG);
            Lbl_TitleLb.color = _allFinishColor;
            Lbl_DesLb.color = _allFinishColor;
            Lbl_ConditionLb.color = _allFinishColor;
            //Lbl_AwardLb.color = _nameUnFinishColor;
            //Lbl_AwardDesc.color = _nameUnFinishColor;
            //Lbl_ConstTimeDesc.color = _nameUnFinishColor;
            Lbl_TitleDesc.color = _allFinishColor;
        }
        else
        {
            Debug.LogError("data status error");
        }
    }
    /// <summary>
    /// 设置进程文本
    /// </summary>
    private void SetCourseLabel(bool isActive,int id,int cur,int fin)
    {
        if (isActive)
        {           
            if (id == 1200001 || id == 1210001)
            {
                string str2 = string.Format(ConstString.ACHIEVEMENT_YI, fin / 100000000);
                string str1 = cur >= 10000 ? string.Format(ConstString.ACHIEVEMENT_WAN, cur / 10000) : cur.ToString();
                Lbl_ConditionLb.text = string.Format(ConstString.ACHIEVEMENT_COURSE, str1, str2);
            }
            else if (id == 1190001 || id == 1190002)
            {
                string str = string.Format(ConstString.ACHIEVEMENT_WAN, fin / 10000);
                Lbl_ConditionLb.text = string.Format(ConstString.ACHIEVEMENT_COURSE, cur, str);
            }
            else
            {
                Lbl_ConditionLb.text = string.Format(ConstString.ACHIEVEMENT_COURSE, cur, fin);
            }
            Lbl_ConditionLb.gameObject.SetActive(true);
        }
        else
        {
            Lbl_ConditionLb.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 设置奖励附加说明
    /// </summary>
    private void SetAwardLabel(int type,int id)
    {
        if (type == 1)
        {
            Lbl_ConstTimeDesc.text = string.Format(ConstString.ACHIEVEMENT_CONSTTIME,
                (ConfigManager.Instance.mPlayerPortraitConfig.GetConstTimeByID((uint)id)) / 3600);
        }
        else if (type == 2)
        {
            Lbl_ConstTimeDesc.text = string.Format(ConstString.ACHIEVEMENT_CONSTTIME,
                (ConfigManager.Instance.mFrameConfig.GetConstTimeByID((uint)id)) / 3600);
        }
        else
        {
            Debug.LogError("award type error");
        }
    }
    /// <summary>
    /// 设置可重复执行任务文本说明
    /// </summary>
    private void SetTypeLabel(int type,int award_type,int award_id)
    {
        if (type == 1)
        {
            Lbl_ConstTimeDesc.gameObject.SetActive(false);
            Lbl_TitleDesc.gameObject.SetActive(false);
        }
        else if (type == 2)
        {
            Lbl_ConstTimeDesc.gameObject.SetActive(true);
            Lbl_TitleDesc.gameObject.SetActive(true);
            Lbl_TitleDesc.text = ConstString.ACHIEVEMENT_REDO;
            SetAwardLabel(award_type, award_id);
        }
        else
        {
            Debug.LogError("achievement type error");
        }
    }
    /// <summary>
    /// 领取成就奖励按钮事件
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_GetBtn(GameObject btn)
    {
        if (EventGetAchievementAward != null)
            EventGetAchievementAward(achievementID);
    }
    /// <summary>
    /// 重置时间跳动
    /// </summary>
    private void TimeUpdate()
    {
        //Debug.LogError(reset_time - Main.mTime);
        if (reset_time > Main.mTime)
        {
            Lbl_TitleDesc.text = string.Format(ConstString.ACHIEVEMENT_TIMECOUNTER, CommonFunction.GetTimeString(reset_time - Main.mTime));

        }
        else
        {
            //Debug.LogError("end and remove");
            StopTimeUpdate();
            ResetToUnfinished();
        }
    }
    /// <summary>
    /// 结束时间跳动
    /// </summary>
    public void StopTimeUpdate()
    {
        Scheduler.Instance.RemoveTimer(TimeUpdate);
    }
    /// <summary>
    /// 重置时间结束
    /// </summary>
    private void ResetToUnfinished()
    {
        CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_UNFINISHBG);
        Lbl_TitleLb.color = _nameUnFinishColor;
        Lbl_DesLb.color = _descUnFinishColor;
        Lbl_ConditionLb.color = _nameUnFinishColor;
        Lbl_TitleDesc.color = _nameUnFinishColor;
        Spt_StateBG.gameObject.SetActive(false);
        Lbl_ConditionLb.text = string.Format(ConstString.ACHIEVEMENT_COURSE, 0, ConfigManager.Instance.mAchievementConfig.FindDataByID(achievementID).final_course);
        Lbl_ConditionLb.gameObject.SetActive(true);
        Lbl_TitleDesc.text = ConstString.ACHIEVEMENT_REDO;
    }
}
