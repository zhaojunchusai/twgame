using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class CaptureScoreItem : MonoBehaviour
{
    [HideInInspector]public UILabel Lbl_Rank;
    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public UILabel Lbl_Score;
    private CampaignRankInfo _info;
    private bool _initialized = false;

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
        SetLabelValues();
    }

    public void Init(CampaignRankInfo info, int rank)
    {
        Initialize();
        _info = info;
        Lbl_Rank.text = info.score > 0 ? rank.ToString() : ConstString.NOT_JOIN_FIGHT;
        Lbl_Name.text = info.name;
        Lbl_Score.text = info.score.ToString();
    }

    public void SetLabelValues()
    {
        Lbl_Rank.text = "";
        Lbl_Name.text = "";
        Lbl_Score.text = "";
    }

    public void Uninitialize()
    {

    }
}
