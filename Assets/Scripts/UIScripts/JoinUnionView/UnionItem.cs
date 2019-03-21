using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class UnionItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UISprite Spt_Line;
    [HideInInspector]public UISprite Spt_IconBG;
    [HideInInspector]public UISprite Spt_IconFrame;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_State;

    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public UILabel Lbl_MasterName;
    [HideInInspector]public UILabel Lbl_MemberCount;
    [HideInInspector]public UILabel Lbl_Liveness;
    [HideInInspector]public UILabel Lbl_Condition;
    [HideInInspector]public UIButton Btn_Confirm;
    [HideInInspector]public UISprite Spt_BtnConfirmBackground;
    [HideInInspector]public UILabel Lbl_BtnConfirmLabel;

    private bool _initialized = false;
    private BaseUnion _data;
    public void Initialize()
    {
        if(_initialized)
            return;
        _initialized = true;

        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Spt_Line = transform.FindChild("Line").gameObject.GetComponent<UISprite>();
        Spt_IconBG = transform.FindChild("Icon/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = transform.FindChild("Icon/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Icon/Icon").gameObject.GetComponent<UISprite>();
        Spt_State = transform.FindChild("Icon/state").gameObject.GetComponent<UISprite>();
        Lbl_Name = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_MasterName = transform.FindChild("MasterName").gameObject.GetComponent<UILabel>();
        Lbl_MemberCount = transform.FindChild("MemberCount").gameObject.GetComponent<UILabel>();
        Lbl_Liveness = transform.FindChild("Liveness").gameObject.GetComponent<UILabel>();
        Lbl_Condition = transform.FindChild("Condition").gameObject.GetComponent<UILabel>();
        Btn_Confirm = transform.FindChild("Confirm").gameObject.GetComponent<UIButton>();
        Spt_BtnConfirmBackground = transform.FindChild("Confirm/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnConfirmLabel = transform.FindChild("Confirm/Label").gameObject.GetComponent<UILabel>();
        SetLabelValues();

        UIEventListener.Get(Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
    }

    public void Refreash()
    {
        SetBtnState();
    }

    public void Init(BaseUnion data)
    {
        Initialize();
        _data = data;
        Lbl_Name.text = _data.name;
		string icon = ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_data.icon);
        if (string.IsNullOrEmpty(icon))
        {
            icon = ConfigManager.Instance.mUnionConfig.GetMinUnionIconData().mIcon;
            Debug.LogError("Error: Can't find union icon -> {" + _data.icon + "}");
        }
        CommonFunction.SetSpriteName(Spt_Icon, icon);
        this.Spt_State.gameObject.SetActive(data.altar_status == AltarFlameStatus.DEPEND_STATUS);
        Lbl_MasterName.text = string.Format(ConstString.FORMAT_JOIN_UNION_CHARMAN, _data.chairman);
        Lbl_MemberCount.text = string.Format(ConstString.FORMAT_UNION_MEMBER_COUINT, _data.member_num, ConfigManager.Instance.mUnionConfig.GetUnionLvUpData(_data.level).mMemberCount);
        Lbl_Liveness.text = string.Format(ConstString.FORMAT_LAST_WEEK_LIVENESS, _data.prev_week_vitality);
        if (_data.limit_type == UnionLimitType.ULT_LEVEL_MANUAL || _data.limit_type == UnionLimitType.ULT_LEVEL_NONE)
        {
            Lbl_Condition.text = string.Format(ConstString.FORMAT_JOIN_UNION_CONDITION, _data.limit_level);
        }
        else
        {
            Lbl_Condition.text = string.Empty;
        }
        SetBtnState();
    }

    private bool btnActive = false;
    private void SetBtnState()
    {
        btnActive = true;
        if(_data.member_num >= ConfigManager.Instance.mUnionConfig.GetUnionLvUpData(_data.level).mMemberCount)
        {
            Lbl_BtnConfirmLabel.text = ConstString.UNION_APPLY_BTNSTATE_FULL;
            btnActive = false;
        }
        long applyTime =0;
       
        if (HasApplyUnion(UnionModule.Instance.CharUnionInfo.apply_unions,out applyTime))
        {
            Debug.LogWarning("applyTime " + applyTime + " main.mTime " + Main.mTime);
            //隔天重置 86400
            if (Main.mTime - applyTime >= ConfigManager.Instance.mUnionConfig.GetUnionBaseData().LeaveUnionCD)
            {
                Lbl_BtnConfirmLabel.text = ConstString.UNION_APPLY_BTNSTATE_APPLY;
                btnActive = true;
            }
            else
            {
                Lbl_BtnConfirmLabel.text = ConstString.UNION_APPLY_BTNSTATE_HAS_APPLY;
                btnActive = false;
            }
        }
        if ((_data.limit_type == UnionLimitType.ULT_LEVEL_MANUAL || _data.limit_type == UnionLimitType.ULT_LEVEL_NONE) && _data.limit_level > PlayerData.Instance._Level)
        {
            Lbl_BtnConfirmLabel.text = ConstString.UNION_APPLY_BTNSTATE_LEVEL_LIMIT;
            btnActive = false;
        }
        CommonFunction.SetBtnState(Btn_Confirm.gameObject, btnActive);
        if (btnActive)
            Lbl_BtnConfirmLabel.text = ConstString.UNION_APPLY_BTNSTATE_APPLY;
    }

    private bool HasApplyUnion(List<ApplyUnion> list, out long applyTime)
    {
        applyTime = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].union_id == _data.id)
            {
                applyTime = list[i].apply_tick;
                return true;
            }
        }
        return false;
    }

    public void SetLabelValues()
    {
        Lbl_Name.text = "";
        Lbl_MasterName.text = "";
        Lbl_MemberCount.text = "";
        Lbl_Liveness.text = "";
        Lbl_Condition.text = "";
        //Lbl_BtnConfirmLabel.text = "[734725]申请[-]";
    }

    public void ButtonEvent_Confirm(GameObject btn)
    {
        if (!btnActive)
            return;

        UnionModule.Instance.OnSendJoinUnion(_data.id);
    }

    public void Uninitialize()
    {

    }


}
