using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class DonationRecItem : MonoBehaviour
{
    [HideInInspector]public UITable UITable_DonationRecItem;
    [HideInInspector]public UILabel Lbl_FirstPart;
    [HideInInspector]public UISprite Spt_IconUse;
    [HideInInspector]public UILabel Lbl_SecPart;
    [HideInInspector]public UISprite Spt_IconGet;
    [HideInInspector]public UILabel Lbl_ThirdPart;
    private bool _initialized = false;

    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;
        UITable_DonationRecItem = GetComponent<UITable>();
        Lbl_FirstPart = transform.FindChild("FirstPart").gameObject.GetComponent<UILabel>();
        Spt_IconUse = transform.FindChild("IconUse").gameObject.GetComponent<UISprite>();
        Lbl_SecPart = transform.FindChild("SecPart").gameObject.GetComponent<UILabel>();
        Spt_IconGet = transform.FindChild("IconGet").gameObject.GetComponent<UISprite>();
        Lbl_ThirdPart = transform.FindChild("ThirdPart").gameObject.GetComponent<UILabel>();
        SetLabelValues();

    }

    private UnionDonateRate _tmpDonate;
    private string _tmpMoneySp;

    public void Init(UnionDonateLog log)
    {
        Initialize();
        System.DateTime time = CommonFunction.GetDateTime(log.tick);
        Lbl_FirstPart.text = string.Format(ConstString.FORMAT_DONATE_RECORD_ONE, time.Year, time.Month, time.Day,
                                           time.Hour, time.Minute, time.Second, log.charname);
        if (log.num <= 0)
        {
            Debug.LogError("ERROR :union donate log .num <= 0  player name = " + log.charname);
            return;
        }
        if (log.type == UnionDonateType.UDT_DIAMOND)
        {
            _tmpDonate = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mDiamondDonate[log.num -1];
            _tmpMoneySp = GlobalConst.SpriteName.Diamond;
        }
        else
        {
            _tmpDonate = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mGoldDonate[log.num -1];
            _tmpMoneySp = GlobalConst.SpriteName.Gold;
        }
        Lbl_SecPart.text = string.Format(ConstString.FORMAT_DONATE_RECORD_TWO, _tmpDonate.Cost);
        Lbl_ThirdPart.text = string.Format(ConstString.FORMAT_DONATE_RECORD_THREE, _tmpDonate.UnionToken, _tmpDonate.SupplyNum);
        CommonFunction.SetSpriteName(Spt_IconUse, _tmpMoneySp);
        UITable_DonationRecItem.Reposition();
        
    }

    public void SetLabelValues()
    {
        Lbl_FirstPart.text = "";
        Lbl_SecPart.text = "";
        Lbl_ThirdPart.text = "";
    }

    public void Uninitialize()
    {

    }

}
