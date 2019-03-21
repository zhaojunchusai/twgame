using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PrisonMarkComponent : MonoBehaviour
{
    public GameObject _uiRoot;

    public UILabel Lbl_Label_Time;
    public UISprite Spt_Sprite_Embellish;
    public UILabel Lbl_Label_Descript;

    private fogs.proto.msg.EnslaveRecord tmpRecord;
    private fogs.proto.msg.AltarRecord tmpAltarRecord;

    public void MyStart(GameObject root)
    {
        this._uiRoot = root;

        Lbl_Label_Time = _uiRoot.transform.FindChild("Label_Time").gameObject.GetComponent<UILabel>();
        Spt_Sprite_Embellish = _uiRoot.transform.FindChild("Sprite_Embellish").gameObject.GetComponent<UISprite>();
        Lbl_Label_Descript = _uiRoot.transform.FindChild("Label_Descript").gameObject.GetComponent<UILabel>();
    }
    public void SetInfo(fogs.proto.msg.EnslaveRecord record)
    {
        this.tmpRecord = record;
        if (this.tmpRecord == null)
            return;

        if (this.Lbl_Label_Time != null)
        {
            DateTime time = CommonFunction.GetDateTime((long)record.time);
            this.Lbl_Label_Time.text = string.Format("{0:D2}/{1:D2}  {2:D2}:{3:D2}:{4:D2}", time.Month, time.Day, time.Hour, time.Minute, time.Second);
        }
        if (this.Lbl_Label_Descript != null)
            this.Lbl_Label_Descript.text = this.GetMessageByType(this.tmpRecord.type);
        return;
    }
    public void SetInfo(fogs.proto.msg.AltarRecord record)
    {
        this.tmpAltarRecord = record;
        if (record == null)
            return;

        if (this.Lbl_Label_Time != null)
        {
            DateTime time = CommonFunction.GetDateTime((long)record.time);
            this.Lbl_Label_Time.text = string.Format("{0:D2}/{1:D2}  {2:D2}:{3:D2}:{4:D2}", time.Month, time.Day, time.Hour, time.Minute, time.Second);
        }
        if (this.Lbl_Label_Descript != null)
        {
            if (record.money_type < 2)
                record.money_type = 1;
            this.Lbl_Label_Descript.text = this.GetAltarRecordMessageByType(record.type, record.money_type);
        }
        return;
    }
    private string GetMessageByType(fogs.proto.msg.EnslaveType type)
    {
        string message = "";
        switch (type)
        {
            case fogs.proto.msg.EnslaveType.ET_CATCH: message = string.Format(ConstString.PRISON_CHATCH_ENSLAVE, this.tmpRecord.name); break;
            case fogs.proto.msg.EnslaveType.ET_RELEASE: message = string.Format(ConstString.PRISON_RELEASE_ENSLAVE, this.tmpRecord.name); break;
            case fogs.proto.msg.EnslaveType.ET_BE_GAINST: message = string.Format(ConstString.PRISON_FLEE_ENSLAVE, this.tmpRecord.name, this.tmpRecord.gold_num); break;
            case fogs.proto.msg.EnslaveType.ET_BE_SAVE: message = string.Format(ConstString.PRISON_BERELEASE_ENSLAVE, this.tmpRecord.name, this.tmpRecord.help_name, this.tmpRecord.gold_num, this.tmpRecord.help_name); break;
            case fogs.proto.msg.EnslaveType.ET_SELF_ENSLAVE: message = string.Format(ConstString.PRISON_BE_ENSLAVE, this.tmpRecord.host_name); break;
            case fogs.proto.msg.EnslaveType.ET_SELF_SAVE: message = string.Format(ConstString.PRISON_BE_RELEASE, this.tmpRecord.help_name); break;
            case fogs.proto.msg.EnslaveType.ET_SELF_AGINST: message = string.Format(ConstString.PRISON_HAD_RELEASE, this.tmpRecord.host_name); break;
            case fogs.proto.msg.EnslaveType.ET_TIME_OVER:
                string time = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_PRSION_FREETIME);
                int hour = 24;
                if (!string.IsNullOrEmpty(time))
                {
                    hour = int.Parse(time);
                    hour = hour / 3600;
                }
                message = string.Format(ConstString.PRISON_TIME_OUT, this.tmpRecord.name, hour, this.tmpRecord.gold_num); break;
            case fogs.proto.msg.EnslaveType.ET_HELP_ENSLAVE: message = string.Format(ConstString.PRISON_SLAVE_ENSLAVE, this.tmpRecord.name, this.tmpRecord.gold_num); break;
            case fogs.proto.msg.EnslaveType.ET_SELF_TIME_OVER: message = string.Format(ConstString.PRISON_ET_SELF_TIME_OVER); break;
            case fogs.proto.msg.EnslaveType.ET_BE_RELEASE: message = string.Format(ConstString.PRISON_ET_BE_RELEASE, this.tmpRecord.host_name); break;
            case fogs.proto.msg.EnslaveType.ET_CATCH_FAILE: message = string.Format(ConstString.ET_CATCH_FAILE, this.tmpRecord.name); break;
            case fogs.proto.msg.EnslaveType.ET_BE_AGAINST_FAIL: message = string.Format(ConstString.ET_BE_AGAINST_FAIL, this.tmpRecord.name); break;
            case fogs.proto.msg.EnslaveType.ET_BE_SAVE_FAIL: message = string.Format(ConstString.ET_BE_SAVE_FAIL, this.tmpRecord.help_name, this.tmpRecord.name); break;
            case fogs.proto.msg.EnslaveType.ET_BE_CATCH_FAILE: message = string.Format(ConstString.ET_BE_CATCH_FAILE, this.tmpRecord.host_name); break;
            case fogs.proto.msg.EnslaveType.ET_SELF_BE_SAVE_FAIL: message = string.Format(ConstString.ET_SELF_BE_SAVE_FAIL, this.tmpRecord.help_name); break;
            case fogs.proto.msg.EnslaveType.ET_SELF_AGAINST_FAIL: message = string.Format(ConstString.ET_SELF_AGAINST_FAIL, this.tmpRecord.host_name); break;
            case fogs.proto.msg.EnslaveType.ET_SAVE_FAIL: message = string.Format(ConstString.ET_SAVE_FAIL, this.tmpRecord.name); break;
        }
        return message;
    }
    private string GetAltarRecordMessageByType(fogs.proto.msg.EnslaveType type, int moneyType)
    {
        string message = "";
        switch (type)
        {
            case fogs.proto.msg.EnslaveType.ALTAR_BE_DEPEND: message = string.Format(ConstString.ALTAR_BE_DEPEND, this.tmpAltarRecord.handler_union_name); break;
            case fogs.proto.msg.EnslaveType.ALTAR_REVOLT: message = string.Format(ConstString.ALTAR_REVOLT, this.tmpAltarRecord.handler_name, this.tmpAltarRecord.be_handler_union_name); break;
            case fogs.proto.msg.EnslaveType.ALTAR_MOLEST:
                List<string> messageList = new List<string>();
                messageList.Add(ConstString.ALTAR_MOLEST1 + CommonFunction.GetConsumeTypeDesc((ECurrencyType)moneyType));
                messageList.Add(ConstString.ALTAR_MOLEST2 + CommonFunction.GetConsumeTypeDesc((ECurrencyType)moneyType));
                messageList.Add(ConstString.ALTAR_MOLEST3 + CommonFunction.GetConsumeTypeDesc((ECurrencyType)moneyType));
                message = string.Format(messageList[(this.tmpAltarRecord.random_type - 1) > 2 || (this.tmpAltarRecord.random_type - 1) < 0 ? 0 : (this.tmpAltarRecord.random_type - 1)], this.tmpAltarRecord.be_handler_name, this.tmpAltarRecord.money_num);
                break;
            case fogs.proto.msg.EnslaveType.ALTAR_BE_MOLEST: message = ""; break;
            case fogs.proto.msg.EnslaveType.ALTAR_COMFORT:
                List<string> tmp = new List<string>();
                tmp.Add(ConstString.ALTAR_COMFORT1 + CommonFunction.GetConsumeTypeDesc((ECurrencyType)moneyType));
                tmp.Add(ConstString.ALTAR_COMFORT2 + CommonFunction.GetConsumeTypeDesc((ECurrencyType)moneyType));
                tmp.Add(ConstString.ALTAR_COMFORT3 + CommonFunction.GetConsumeTypeDesc((ECurrencyType)moneyType));
                message = string.Format(tmp[(this.tmpAltarRecord.random_type - 1) > 2 || (this.tmpAltarRecord.random_type - 1) < 0 ? 0 : (this.tmpAltarRecord.random_type - 1)], this.tmpAltarRecord.be_handler_name, this.tmpAltarRecord.money_num);
                break;
            case fogs.proto.msg.EnslaveType.ALTAR_BE_COMFORT: message = ""; break;
            case fogs.proto.msg.EnslaveType.ALTAR_CHAIRMAN_BE_MOLEST:
                List<string> tmpString = new List<string>();
                tmpString.Add(ConstString.ALTAR_CHAIRMAN_BE_MOLEST1);
                tmpString.Add(ConstString.ALTAR_CHAIRMAN_BE_MOLEST2);
                tmpString.Add(ConstString.ALTAR_CHAIRMAN_BE_MOLEST3);
                message = string.Format(tmpString[(this.tmpAltarRecord.random_type - 1) > 2 || (this.tmpAltarRecord.random_type - 1) < 0 ? 0 : (this.tmpAltarRecord.random_type - 1)], this.tmpAltarRecord.handler_name, this.tmpAltarRecord.be_handler_name, CommonFunction.GetConsumeTypeDesc((ECurrencyType)moneyType));
                break;
        }
        return message;
    }
}