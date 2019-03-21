using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MailItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_ItemBGSprite;
    [HideInInspector]public UISprite Spt_IconBGSprite;
    [HideInInspector]public UISprite Spt_IconSprite;
    [HideInInspector]public UISprite Spt_IconFreamSprite;
    [HideInInspector]public UILabel Lbl_MailTitleLabel;
    [HideInInspector]public UILabel Lbl_MailDateLabel;
    [HideInInspector]
    public UILabel Lbl_MailDescLabel;
    [HideInInspector]
    public UILabel Lbl_DeadLine;

    private ulong _id;
    public ulong ID
    {
        get { return _id; }
    }

    private bool _isAttach;
    public bool IsAttach
    {
        get { return _isAttach; }
    }

    private bool _isRead;
    public bool IsRead
    {
        get { return _isRead; }
    }
    private bool _isInit = false;

    public void Initialize()
    {
        if (_isInit)
            return;
        _isInit = true;
        Spt_ItemBGSprite = transform.FindChild("ItemBGSprite").gameObject.GetComponent<UISprite>();
        Spt_IconBGSprite = transform.FindChild("IconBGSprite").gameObject.GetComponent<UISprite>();
        Spt_IconSprite = transform.FindChild("IconSprite").gameObject.GetComponent<UISprite>();
        Spt_IconFreamSprite = transform.FindChild("IconFreamSprite").gameObject.GetComponent<UISprite>();
        Lbl_MailTitleLabel = transform.FindChild("MailTitleLabel").gameObject.GetComponent<UILabel>();
        Lbl_MailDateLabel = transform.FindChild("MailDateLabel").gameObject.GetComponent<UILabel>();
        Lbl_MailDescLabel = transform.FindChild("MailDescLabel").gameObject.GetComponent<UILabel>();
        Lbl_DeadLine = transform.FindChild("deadline").gameObject.GetComponent<UILabel>();
        SetLabelValues();
    }

    void Awake()
    {
        Initialize(); 
    }

    public void SetLabelValues()
    {
        Lbl_MailTitleLabel.text = ConstString.EMAIL_NAME;
        Lbl_MailDateLabel.text = string.Format(ConstString.EMAIL_STR_DATA1, 0, 0); 
        Lbl_MailDescLabel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void UpdateMailItem(ulong id, bool isRead, bool isAttach, string title, string desc, string date,
        long deadline)
    {
        _id = id;
        _isRead = isRead;
        _isAttach = isAttach;
        Lbl_MailTitleLabel.text = title;
        Lbl_MailDateLabel.text = FormatDateStr(date);
        UpdateItemIcon(_isRead, _isAttach);
        Lbl_DeadLine.text = string.Format(ConstString.EMAIL_STR_DEADLINE, GetDeadLine(deadline));
        if (string.IsNullOrEmpty(desc))
            return;
        if (desc.Length > 5)
            desc = desc.Substring(0, 5);
        Lbl_MailDescLabel.text = string.Format(ConstString.EMAIL_STR_DESC, desc);
    }

    private string GetDeadLine(long deadline)
    {
        string result = "";
        Lbl_DeadLine.gameObject.SetActive(false);
        long rest = deadline - Main.mTime;
        if (rest > 0)
        {
            Lbl_DeadLine.gameObject.SetActive(true);
            if (rest / 86400 > 0)
            {
                result = string.Format(ConstString.FORMAT_TOTAL_LOGIN_DAY, rest / 86400);
            }
            else
            {
                result = CommonFunction.GetTimeString(rest);
            }
        }
        return result;
    }


    public void UpdateItemIcon(bool isRead, bool isAttach)
    {

        _isRead = isRead;
        _isAttach = isAttach;
        string spName = "";
        if (!_isRead && _isAttach)
            spName =GlobalConst.SpriteName.EMAIL_NOTREAD_ATTACH;
        else if (_isRead && _isAttach)
            spName = GlobalConst.SpriteName.EMAIL_READ_ATTACH;
        else if (!_isRead && !_isAttach)
            spName = GlobalConst.SpriteName.EMAIL_NOTREAD_NOATTACH;
        else if (_isRead && !_isAttach)
            spName = GlobalConst.SpriteName.EMAIL_READ_NOATTACH;

        CommonFunction.SetSpriteName(Spt_IconSprite,spName);
        Spt_IconSprite.width = 70;
        Spt_IconSprite.height = 70;
    }
    /// <summary>
    /// 格式化字符串 2015-07-04 15:20:18 => 7月4日
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string FormatDateStr(string str)
    {
        if (str.Length >= 10)
            str = str.Substring(0, 10).Trim();
        string[] array = str.Split('-');
        if (array == null || array.Length != 3)
            return str;

        return string.Format(ConstString.EMAIL_STR_DATA1, array[1], array[2]);

    }
}
