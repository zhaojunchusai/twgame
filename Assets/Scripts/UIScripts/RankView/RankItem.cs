using UnityEngine;
using System.Collections;
using fogs.proto.msg;
public class RankItem : MonoBehaviour
{
    const uint MAXRANKNUM = 10000;

    private bool _isSelf = true;
    public bool IsSelf
    {
        get { return _isSelf; }
        set
        {
            _isSelf = value;
            UpdateStyleByIsSelf(_isSelf);
        }
    }

    public RankType _type;

    public RankInfo mInformation;

    private Vector3 _levelMiddlePos = new Vector3(60f, 17f, 0f);
    private Vector3 _levelRightPos = new Vector3(184f, 17f, 0f);
    private Color _selfColor = new Color(1f, 1f, 197 / 255f);
    private Color _otherColor = new Color(160 / 255f, 139 / 255f, 113 / 255f);
    private Color _otherNameColor = new Color(227 / 255f, 169 / 255f, 75 / 255f);
    //private Color _otherPointColor = new Color(1f, 1f, 197 / 255f);
    //private Color _otherFightColor = new Color(197 / 255f, 138 / 255f, 0f);
    //private Color _otherPointColor = new Color(52 / 255f, 148 / 255f, 36 / 255f);
    private Vector3 _unionNamePos = new Vector3(-95, 17.5f, 0);
    private Vector3 _playerNamePos = new Vector3(-26, 17.5f, 0);

    [HideInInspector]
    public UISprite Spt_ItemBG;
    [HideInInspector]
    public UISprite Spt_LabelBG;
    [HideInInspector]
    public UILabel Lbl_NameLabel;
    [HideInInspector]
    public UILabel Lbl_UnionLabel;
    [HideInInspector]
    public UILabel Lbl_LevelLabel;
    [HideInInspector]
    public UILabel Lbl_LastWaveLabel;
    [HideInInspector]
    public UILabel Lbl_PointLabel;
    [HideInInspector]
    public UISprite Spt_RankNumBG1;
    [HideInInspector]
    public UISprite Spt_RankNumBG2;
    [HideInInspector]
    public UILabel Lbl_RankNumLabel;
    [HideInInspector]
    public UILabel Lbl_FightPowerLabel;
    [HideInInspector]
    public UILabel Lbl_IconLevelLabel;
    [HideInInspector]
    public UILabel Lbl_TimeLabel;
    [HideInInspector]
    public UILabel Lbl_OutRankLabel;
    [HideInInspector]
    public UISprite Spt_IconSprite;
    [HideInInspector]
    public UISprite Spt_FrameSprite;
    [HideInInspector]
    public UISprite Spt_State;
    [HideInInspector]
    public UISprite Spt_VIPSprite;
    [HideInInspector]
    public UILabel Lbl_VIPLabel;

    public void Initialize()
    {
        Spt_ItemBG = transform.FindChild("ItemBG").gameObject.GetComponent<UISprite>();
        Spt_LabelBG = transform.FindChild("LabelBG").gameObject.GetComponent<UISprite>();
        Lbl_NameLabel = transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_UnionLabel = transform.FindChild("UnionLabel").gameObject.GetComponent<UILabel>();
        Lbl_LevelLabel = transform.FindChild("LevelLabel").gameObject.GetComponent<UILabel>();
        Lbl_LastWaveLabel = transform.FindChild("LastWaveLabel").gameObject.GetComponent<UILabel>();
        Lbl_PointLabel = transform.FindChild("PointLabel").gameObject.GetComponent<UILabel>();
        Spt_RankNumBG1 = transform.FindChild("RankNumBG1").gameObject.GetComponent<UISprite>();
        Spt_RankNumBG2 = transform.FindChild("RankNumBG2").gameObject.GetComponent<UISprite>();
        Lbl_RankNumLabel = transform.FindChild("RankNumLabel").gameObject.GetComponent<UILabel>();
        Lbl_FightPowerLabel = transform.FindChild("FightPowerLabel").gameObject.GetComponent<UILabel>();
        Lbl_IconLevelLabel = transform.FindChild("IconGroup/LevelLabel").gameObject.GetComponent<UILabel>();
        Spt_IconSprite = transform.FindChild("IconGroup/IconSprite").gameObject.GetComponent<UISprite>();
        Spt_FrameSprite = transform.FindChild("IconGroup/FrameSprite").gameObject.GetComponent<UISprite>();
        Lbl_TimeLabel = transform.FindChild("TimeLabel").gameObject.GetComponent<UILabel>();
        Lbl_OutRankLabel = transform.FindChild("OutRankLabel").gameObject.GetComponent<UILabel>();
        Spt_State = transform.FindChild("IconGroup/state").gameObject.GetComponent<UISprite>();
        Spt_VIPSprite = transform.FindChild("VIPSprite").gameObject.GetComponent<UISprite>();
        Lbl_VIPLabel = transform.FindChild("VIPLabel").gameObject.GetComponent<UILabel>();

        SetLabelValues();
    }

    void Awake()
    {
        Initialize();
        mInformation = new RankInfo();
    }

    void Start()
    {

    }

    public void SetLabelValues()
    {
        Lbl_NameLabel.text = "";
        Lbl_UnionLabel.text = string.Format(ConstString.RANK_ITEMLABLE_UNION, "");
        Lbl_LevelLabel.text = string.Format(ConstString.RANK_ITEMLABLE_LEVEL, 0);
        Lbl_LastWaveLabel.text = string.Format(ConstString.RANK_ITEMLABLE_LASTWAVE, 0);
        Lbl_PointLabel.text = string.Format(ConstString.RANK_ITEMLABLE_POINT, 0);
        Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_ITEMLABLE_FIGHTPOWER, 0);
        Lbl_TimeLabel.text = string.Format(ConstString.RANK_LABEL_TIME, 0);
        Lbl_OutRankLabel.text = ConstString.RANK_LABEL_OUTRANK;
        Lbl_RankNumLabel.text = "0";
    }

    public void Uninitialize()
    {

    }

    public void UpdateItem(RankType type, RankInfo info)
    {
        //if (type == RankType.UNIONOVERLORD)
        //{
        //    type = RankType.EXPLORE_RANK;
        //    info.union_info = new RankUnionInfo();
        //    info.union_info.union_id = 1;
        //    info.union_info.union_name = "Test";
        //    info.union_info.chairman = "军团长名字";
        //    info.union_info.altar_status = AltarFlameStatus.DEPEND_STATUS;
        //    info.union_info.union_level = 21;
        //    info.union_info.icon = "";
        //    info.union_info.max_members = 43;
        //    info.union_info.members = 21;
        //    info.union_info.value = 12;
        //    info.union_info.value_time = "2015/2/2";
        //    info.union_info.host_union_name = "依附军团名";
        //}


        CommonFunction.CopyRankInfo(info, mInformation);
        _type = type;
        this.Spt_State.gameObject.SetActive(false);
        Lbl_LastWaveLabel.transform.localPosition = new Vector3(142, -19, 0);
        if (_type == RankType.UNIONOVERLORD)//軍團霸主榜//
        {
            UpdateUnionSupermacyInfo();
        }
        else if (_type == RankType.EXPLORE_RANK)//軍團探險榜//
        {
            UpdataExoticAdvanture(info);
        }
        else if (_type == RankType.GRID_RANK)//排位榜//
        {
            this.UpdataQualifyingRank(info);
        }
        else if (_type == RankType.POLE_RANK)
        {
            UpdateQualifyingInfo(_type, info);
        }
        else if (_type == RankType.CROSSSERVERWAR_UNION)
        {
            UpdateCrossServerWarUnionRankInfo();
        }
        else if (_type == RankType.CROSSSERVERWAR_PERSONAL)
        {
            UpdateCrossServerWarPersonalRankInfo();
        }
        else
        {
            Spt_RankNumBG1.enabled = true;
            Lbl_RankNumLabel.enabled = true;
            Lbl_RankNumLabel.transform.localPosition = new Vector3(-257f, -3.5f, 0f);
            Lbl_NameLabel.text = mInformation.name;
            SetVipLabel(mInformation.vip_level);
            if (mInformation.rank < MAXRANKNUM)
            {
                Lbl_RankNumLabel.text = mInformation.rank.ToString();
                Lbl_OutRankLabel.text = "";
            }
            else
            {
                Lbl_RankNumLabel.text = "";
                Lbl_OutRankLabel.gameObject.SetActive(true);
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_OUTRANK;
            }

            if (CommonFunction.XmlStringIsNull(mInformation.union_name))
                mInformation.union_name = ConstString.RANK_LABEL_NOUNION;

            Lbl_UnionLabel.text = string.Format(ConstString.RANK_ITEMLABLE_UNION, mInformation.union_name);
            Lbl_LevelLabel.text = string.Format(ConstString.RANK_ITEMLABLE_LEVEL, mInformation.level);
            Lbl_LastWaveLabel.text = string.Format(ConstString.RANK_ITEMLABLE_LASTWAVE, mInformation.high_victory);
            Lbl_PointLabel.text = string.Format(ConstString.RANK_ITEMLABLE_POINT, mInformation.high_grade);

            if (_type != RankType.COMBAT_RANK)
                Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_ITEMLABLE_FIGHTPOWER, CommonFunction.GetTenThousandUnit((int)info.combat_power, 10000000));
            else
                Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_ITEMLABLE_BFIGHTPOWER, CommonFunction.GetTenThousandUnit((int)info.combat_power, 10000000));

            Lbl_IconLevelLabel.text = info.level.ToString();
            string timeStr = CommonFunction.GetTimeString((long)info.use_time);
            Lbl_TimeLabel.text = string.Format(ConstString.RANK_LABEL_TIME, timeStr);
            UpdateNumBGByRank(mInformation.rank);
            UpdateLabelByType(_type);
            this.SetLabelByType(_type, mInformation);
            if (_type != RankType.CAMPAIGN_UNION)
                CommonFunction.SetHeadAndFrameSprite(Spt_IconSprite, Spt_FrameSprite, mInformation.icon, mInformation.frame, true);
        }


    }
    public void SetLabelByType(RankType type, RankInfo vInfo)
    {
        if (vInfo == null)
            return;
        if (type == RankType.CAMPAIGN_UNION)
        {
            Lbl_LastWaveLabel.transform.localPosition = new Vector3(102, -19, 0);
            this.Lbl_NameLabel.text = vInfo.union_name;
            SetVipLabel(0,false);
            Lbl_UnionLabel.text = ConstString.RANK_LABEL_CITYCOUNT + CommonFunction.GetTenThousandUnit((int)vInfo.high_victory, 10000000);
            Lbl_LevelLabel.text = "";
            Lbl_LastWaveLabel.text = "";
            Lbl_PointLabel.text = "";
            Lbl_LastWaveLabel.text = ConstString.RANK_LABEL_RANGKNUM + CommonFunction.GetTenThousandUnit((int)vInfo.high_grade, 10000000);
            Lbl_FightPowerLabel.text = "";
            Lbl_TimeLabel.text = "";
            Lbl_OutRankLabel.text = "";
            if (vInfo.high_grade.Equals(0))
            {
                Spt_RankNumBG1.gameObject.SetActive(false);
                Spt_RankNumBG2.gameObject.SetActive(false);
                Lbl_RankNumLabel.text = "";
                Lbl_OutRankLabel.gameObject.SetActive(true);
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONNOIN;
            }
            if (string.IsNullOrEmpty(vInfo.union_name) || vInfo.union_name.Equals(ConstString.RANK_LABEL_NOUNION))
            {
                Lbl_UnionLabel.text = ConstString.RANK_LABEL_CITYCOUNT + " 0";
                //Lbl_PointLabel.text = ConstString.RANK_LABEL_RANGKNUM;
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.QUESTION);
                CommonFunction.SetSpriteName(Spt_FrameSprite, GlobalConst.SpriteName.Quality_1);
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONHAD;
            }
            else
            {
                CommonFunction.SetSpriteName(Spt_IconSprite, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(vInfo.icon.ToString()));
                CommonFunction.SetSpriteName(Spt_FrameSprite, GlobalConst.SpriteName.Quality_1);
            }
            if (vInfo.altar_status == AltarFlameStatus.DEPEND_STATUS)
                this.Spt_State.gameObject.SetActive(true);
            else
                this.Spt_State.gameObject.SetActive(false);
        }
        if (type == RankType.CAMPAIGN_PLAYER)
        {
            Lbl_LastWaveLabel.transform.localPosition = new Vector3(102, -19, 0);
            Lbl_LevelLabel.text = "";
            Lbl_LastWaveLabel.text = "";
            Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_ITEMLABLE_BFIGHTPOWER, CommonFunction.GetTenThousandUnit((int)vInfo.combat_power, 10000000));
            Lbl_TimeLabel.text = "";
            this.Lbl_NameLabel.text = vInfo.name;
            SetVipLabel(vInfo.vip_level);
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_ITEMLABLE_UNION, vInfo.union_name);
            Lbl_PointLabel.text = "";
            Lbl_OutRankLabel.text = "";
            Lbl_LastWaveLabel.text = ConstString.RANK_LABEL_RANGKNUM + CommonFunction.GetTenThousandUnit((int)vInfo.high_grade, 10000000);
            if (vInfo.high_grade == 0)
            {
                Spt_RankNumBG1.gameObject.SetActive(false);
                Spt_RankNumBG2.gameObject.SetActive(false);
                Lbl_RankNumLabel.text = "";
                Lbl_OutRankLabel.gameObject.SetActive(true);
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONNOIN;
            }
            if (string.IsNullOrEmpty(vInfo.union_name) || vInfo.union_name.Equals(ConstString.RANK_LABEL_NOUNION))
            {
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONHAD;
            }
        }

        if (type == RankType.CAMPAIGN_ALL_PLAYER)
        {
            Lbl_LastWaveLabel.transform.localPosition = new Vector3(102, -19, 0);
            Lbl_LevelLabel.text = "";
            Lbl_LastWaveLabel.text = "";
            Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_ITEMLABLE_BFIGHTPOWER, CommonFunction.GetTenThousandUnit((int)vInfo.combat_power, 10000000));
            Lbl_TimeLabel.text = "";
            this.Lbl_NameLabel.text = vInfo.name;
            SetVipLabel(vInfo.vip_level);
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_ITEMLABLE_UNION, vInfo.union_name);
            Lbl_PointLabel.text = "";
            Lbl_OutRankLabel.text = "";
            Lbl_LastWaveLabel.text = ConstString.RANK_LABEL_RANGKNUM + CommonFunction.GetTenThousandUnit((int)vInfo.high_grade, 10000000);
            if (vInfo.high_grade == 0)
            {
                Spt_RankNumBG1.gameObject.SetActive(false);
                Spt_RankNumBG2.gameObject.SetActive(false);
                Lbl_RankNumLabel.text = "";
                Lbl_OutRankLabel.gameObject.SetActive(true);
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONNOIN;
            }
            if (string.IsNullOrEmpty(vInfo.union_name) || vInfo.union_name.Equals(ConstString.RANK_LABEL_NOUNION))
            {
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONHAD;
            }
        }

    }


    public void UpdateQualifyingInfo(RankType type, RankInfo info)
    {
        Lbl_OutRankLabel.text = "";
        Lbl_TimeLabel.text = string.Empty;
        Lbl_NameLabel.text = info.name;
        SetVipLabel(info.vip_level);
        if (CommonFunction.XmlStringIsNull(mInformation.union_name))
        {
            mInformation.union_name = ConstString.RANK_LABEL_NOUNION;
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_ITEMLABLE_UNION, ConstString.RANK_LABEL_NOUNION);
        }
        else
        {
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_ITEMLABLE_UNION, info.union_name);
        }
        Lbl_LastWaveLabel.text = ConstString.RANK_LABEL_RANGKNUM + info.high_grade.ToString();
        Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_ITEMLABLE_FIGHTPOWER, info.combat_power);
        Lbl_LevelLabel.text = "";
        Lbl_PointLabel.text = "";
        Spt_RankNumBG2.gameObject.SetActive(false);
        Spt_RankNumBG1.gameObject.SetActive(true);
        Lbl_RankNumLabel.transform.localPosition = new Vector3(-257f, 10f, 0f);
        if (info.rank <= 0)
        {
            Lbl_RankNumLabel.enabled = false;
            Spt_RankNumBG1.enabled = false;
            Lbl_OutRankLabel.text = ConstString.RANK_LABEL_OUTRANK;
        }
        else 
        {
            Lbl_RankNumLabel.enabled = true;
            Lbl_OutRankLabel.text = "";
            Spt_RankNumBG1.enabled = true;
            Lbl_RankNumLabel.text = info.rank.ToString();
        }
        Lbl_IconLevelLabel.text = info.level.ToString();
        CommonFunction.SetHeadAndFrameSprite(Spt_IconSprite, Spt_FrameSprite, mInformation.icon, mInformation.frame, true);
        QualifyingRankData data = ConfigManager.Instance.mQualifyingRankConfig.GetRankDataByPoint((int)info.high_grade);
        CommonFunction.SetSpriteName(Spt_IconSprite, CommonFunction.GetHeroIconNameByID(mInformation.icon, true));
        if (data != null)
        {
            CommonFunction.SetSpriteName(Spt_FrameSprite, data.frame);
            CommonFunction.SetSpriteName(Spt_RankNumBG1, data.icon);
        }
    }
    /// <summary>
    /// 更新跨服战军团榜
    /// </summary>
    public void UpdateCrossServerWarUnionRankInfo()
    {
        if (mInformation == null || mInformation.union_info == null) return;
        UpdateLabelByType(_type);
        if (CommonFunction.XmlStringIsNull(mInformation.union_info.union_name) || mInformation.union_name.Equals(ConstString.RANK_LABEL_NOUNION))
        {
            if (_isSelf)
            {
                mInformation.union_info.union_name = ConstString.RANK_LABEL_NOUNION;
                Lbl_RankNumLabel.text = "";
                Lbl_OutRankLabel.gameObject.SetActive(true);
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONHAD;
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.QUESTION);
                Lbl_PointLabel.text = string.Format(ConstString.RANK_GOODS,"--");
                Lbl_UnionLabel.text = string.Format(ConstString.RANK_TERRITORY, "--");
            }
            else
            {
                Debug.LogError("rankinfo's union name is wrong:" + mInformation.union_info.union_name);
            }
        }
        else
        {
            Lbl_RankNumLabel.transform.localPosition = new Vector3(-257f, -3.5f, 0f);
            if (mInformation.union_info.value <= 0)
            {
                if (_isSelf)
                {
                    Lbl_RankNumLabel.text = "";
                    Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONNOIN;
                }
                else
                {
                    Debug.LogError("The 0 damage union should not been shown~~~" + mInformation.union_info.union_name);
                }
            }
            else
            {
                Lbl_RankNumLabel.text = mInformation.union_info.rank.ToString();
                Lbl_OutRankLabel.text = "";
            }
            CommonFunction.SetSpriteName(Spt_IconSprite, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(mInformation.union_info.icon));
            Lbl_NameLabel.text = mInformation.union_info.union_name;
            SetVipLabel(0, false);
            Lbl_IconLevelLabel.text = mInformation.union_info.union_level.ToString();
            Lbl_PointLabel.text = string.Format(ConstString.RANK_GOODS, mInformation.high_grade);
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_TERRITORY, mInformation.high_victory);
            Spt_State.gameObject.SetActive(mInformation.union_info.altar_status == AltarFlameStatus.DEPEND_STATUS);
            UpdateNumBGByRank(mInformation.union_info.rank);
        }
        CommonFunction.SetSpriteName(Spt_FrameSprite, "Frame_10001_A");
    }
    /// <summary>
    /// 更新跨服战成员榜
    /// </summary>
    public void UpdateCrossServerWarPersonalRankInfo()
    {
        if (mInformation == null) return;
        UpdateLabelByType(_type);

        if (_isSelf && PlayerData.Instance.MyUnionID==0)
        {
            Lbl_RankNumLabel.text = "";
            Lbl_OutRankLabel.gameObject.SetActive(true);
            Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONHAD;
            Lbl_PointLabel.text = ConstString.RANK_LABEL_RANGKNUM + "--";
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_ITEMLABLE_BFIGHTPOWER, "--");
        }
        else
        {
            Lbl_RankNumLabel.transform.localPosition = new Vector3(-257f, -3.5f, 0f);
            if (mInformation.high_grade <= 0)
            {
                Lbl_RankNumLabel.text = "";
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONNOIN;
            }
            else
            {
                Lbl_RankNumLabel.text = mInformation.rank.ToString();
                Lbl_OutRankLabel.text = "";
            }
            Lbl_NameLabel.text = mInformation.name;
            SetVipLabel(mInformation.vip_level, true);
            Lbl_IconLevelLabel.text = mInformation.level.ToString();
            Lbl_PointLabel.text = ConstString.RANK_LABEL_RANGKNUM + CommonFunction.GetTenThousandUnit((int)mInformation.high_grade, 10000000);
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_ITEMLABLE_BFIGHTPOWER, mInformation.combat_power);
            UpdateNumBGByRank(mInformation.rank);
        }
        CommonFunction.SetHeadAndFrameSprite(Spt_IconSprite, Spt_FrameSprite, mInformation.icon, mInformation.frame, true);
    }
    /// <summary>
    /// 军团排行榜设置
    /// </summary>
    public void UpdateUnionSupermacyInfo()
    {
        if (mInformation == null||mInformation.union_info==null) return;
        UpdateLabelByType(_type);
        if (CommonFunction.XmlStringIsNull(mInformation.union_info.union_name) || mInformation.union_name.Equals(ConstString.RANK_LABEL_NOUNION))
        {
            if (_isSelf)
            {
                mInformation.union_info.union_name = ConstString.RANK_LABEL_NOUNION;
                Lbl_RankNumLabel.text = "";
                Lbl_OutRankLabel.gameObject.SetActive(true);
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONHAD;
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.QUESTION);
            }
            else
            {
                Debug.LogError("rankinfo's union name is wrong:" + mInformation.union_info.union_name);
            }
        }
        else
        {
            Lbl_RankNumLabel.transform.localPosition = new Vector3(-257f, -3.5f, 0f);
            if (mInformation.union_info.value <= 0)
            {
                if (_isSelf)
                {
                    Lbl_RankNumLabel.text = "";
                    Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONNOIN;
                }
                else
                {
                    Debug.LogError("The 0 damage union should not been shown~~~"+ mInformation.union_info.union_name);
                }
            }
            else
            {
                Lbl_RankNumLabel.text = mInformation.union_info.rank.ToString();
                Lbl_OutRankLabel.text = "";
            }
            CommonFunction.SetSpriteName(Spt_IconSprite, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(mInformation.union_info.icon));
        }

        try
        {
            Lbl_NameLabel.text = mInformation.union_info.union_name;
            SetVipLabel(0,false);
            Lbl_PointLabel.text = string.Format(ConstString.RANK_UNION_MEMBERNUM, mInformation.union_info.members, mInformation.union_info.max_members);
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_UNION_TOTALDAMAGE, mInformation.union_info.value);
            Lbl_IconLevelLabel.text = mInformation.union_info.union_level.ToString();

        }
        catch
        {
            Lbl_NameLabel.text = "--";
            SetVipLabel(0,false);
            Lbl_PointLabel.text = string.Format(ConstString.RANK_UNION_MEMBERNUM, "-", "-");
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_UNION_TOTALDAMAGE, "-");
            Lbl_IconLevelLabel.text = "-";

            Debug.LogError("rankinfo is error");
            Debug.LogError(mInformation.union_info.union_name + "----------" + 
                "member:" + mInformation.union_info.members + "//" + mInformation.union_info.max_members + "----------" +
                "damage:" + mInformation.union_info.value + "----------" + 
                "level:"+ mInformation.union_info.union_level);
        }

        CommonFunction.SetSpriteName(Spt_FrameSprite, "Frame_10001_A");
        Spt_State.gameObject.SetActive(mInformation.union_info.altar_status == AltarFlameStatus.DEPEND_STATUS);

        UpdateNumBGByRank(mInformation.union_info.rank);

    }

    /// <summary>
    /// 刷新探險榜
    /// </summary>
    private void UpdataExoticAdvanture(RankInfo vInfo)
    {
        string tmpProgress = "-";
        string tmpFightTime = "-";

        Spt_State.gameObject.SetActive(false);
        Lbl_OutRankLabel.text = "";
        Lbl_LevelLabel.text = "";
        Lbl_PointLabel.text = "";
        Lbl_TimeLabel.text = "";
        Lbl_RankNumLabel.text = "";
        Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_UNION_MEMBERNUM, "-", "-");
        CommonFunction.SetSpriteName(Spt_FrameSprite, "Frame_10001_A");

        if ((vInfo != null) && (vInfo.union_info != null))
        {
            if (CommonFunction.XmlStringIsNull(vInfo.union_info.union_name) || vInfo.union_info.union_name.Equals(ConstString.RANK_LABEL_NOUNION))
            {
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.QUESTION);
                Lbl_NameLabel.text = ConstString.RANK_LABEL_NOUNION;
                SetVipLabel(0,false);
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONHAD;
                Lbl_OutRankLabel.gameObject.SetActive(true);
            }
            else
            {
                if (vInfo.union_info.rank == 0)
                {
                    Lbl_OutRankLabel.text = ConstString.RANK_LABEL_OUTRANK;
                    Lbl_OutRankLabel.gameObject.SetActive(true);
                    Lbl_OutRankLabel.enabled = true;
                }
                else
                {
                    Lbl_RankNumLabel.text = vInfo.union_info.rank.ToString();
                    Lbl_RankNumLabel.enabled = true;
                    tmpProgress = string.Format(ConstString.RANK_EXPLORE_PROGRESS_GATECOUNT, vInfo.union_info.value);
                    tmpFightTime = vInfo.union_info.value_time;
                }
                Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_UNION_MEMBERNUM, vInfo.union_info.members, vInfo.union_info.max_members);
                CommonFunction.SetSpriteName(Spt_IconSprite, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(vInfo.union_info.icon.ToString()));
                Spt_State.gameObject.SetActive(vInfo.union_info.altar_status == AltarFlameStatus.DEPEND_STATUS);
                Lbl_NameLabel.text = vInfo.union_info.union_name;//军团名字//
                SetVipLabel(0,false);
            }

            Lbl_IconLevelLabel.text = vInfo.union_info.union_level.ToString();//等级//
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_EXPLORE_PROGRESS_TITLE, tmpProgress);//关卡进度//
            Lbl_LastWaveLabel.text = string.Format(ConstString.RANK_EXPLORE_FIGHTTIME, tmpFightTime);//通关时间//
            UpdateNumBGByRank(vInfo.union_info.rank);
            Lbl_LastWaveLabel.transform.localPosition = new Vector3(102, -19, 0);
        }
    }
    private void UpdataQualifyingRank(RankInfo vInfo)
    {
        Lbl_OutRankLabel.text = "";
        Lbl_LevelLabel.text = "";
        Lbl_PointLabel.text = "";
        Lbl_TimeLabel.text = "";
        Lbl_LastWaveLabel.text = "";

        if ((vInfo != null) && (vInfo.union_info != null))
        {
            if (CommonFunction.XmlStringIsNull(vInfo.union_info.union_name) || vInfo.union_info.union_name.Equals(ConstString.RANK_LABEL_NOUNION))
            {
                CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.QUESTION);
                Lbl_RankNumLabel.text = "";
                SetVipLabel(0,false);
                Spt_State.gameObject.SetActive(false);
                Lbl_NameLabel.text = ConstString.RANK_LABEL_NOUNION;
                Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONHAD;
                Lbl_OutRankLabel.gameObject.SetActive(true);
                Lbl_OutRankLabel.enabled = true;
            }
            else
            {
                CommonFunction.SetSpriteName(Spt_IconSprite, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(vInfo.union_info.icon.ToString()));
                Spt_State.gameObject.SetActive(vInfo.union_info.altar_status == AltarFlameStatus.DEPEND_STATUS);
                Lbl_NameLabel.text = vInfo.union_info.union_name;
                SetVipLabel(0,false);
                if (vInfo.union_info.rank > 0)
                    Lbl_RankNumLabel.text = vInfo.union_info.rank.ToString();
                else
                    Lbl_RankNumLabel.text = string.Empty;
                Lbl_RankNumLabel.enabled = true;

                if (vInfo.union_info.value.Equals(0))
                {
                    Lbl_OutRankLabel.gameObject.SetActive(true);
                    Lbl_OutRankLabel.text = ConstString.RANK_LABEL_UNIONNOIN;
                }
            }
            Lbl_IconLevelLabel.text = vInfo.union_info.union_level.ToString();
            Lbl_FightPowerLabel.text = string.Format(ConstString.RANK_UNION_MEMBERNUM, vInfo.union_info.members, vInfo.union_info.max_members);
            Lbl_UnionLabel.text = string.Format(ConstString.RANK_QUALIFYING, vInfo.union_info.value);
            CommonFunction.SetSpriteName(Spt_FrameSprite, "Frame_10001_A");
            UpdateNumBGByRank(vInfo.union_info.rank);
        }
    }
    public void Clear()
    {
        UpdateLabelByType(_type);
        UpdateNumBGByRank(0);
        Lbl_OutRankLabel.gameObject.SetActive(true);
        mInformation = null;
    }

    private void UpdateNumBGByRank(uint rank)
    {
        Spt_RankNumBG1.gameObject.SetActive(true);
        Spt_RankNumBG2.gameObject.SetActive(true);
        Spt_RankNumBG1.enabled = true;
        Spt_RankNumBG2.enabled = true;

        if (rank == 1)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG1, GlobalConst.SpriteName.RANK_BG1_1);
            CommonFunction.SetSpriteName(Spt_RankNumBG2, GlobalConst.SpriteName.RANK_BG2_1);
        }
        else if (rank == 2)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG1, GlobalConst.SpriteName.RANK_BG1_2);
            CommonFunction.SetSpriteName(Spt_RankNumBG2, GlobalConst.SpriteName.RANK_BG2_2);
        }
        else if (rank == 3)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG1, GlobalConst.SpriteName.RANK_BG1_3);
            CommonFunction.SetSpriteName(Spt_RankNumBG2, GlobalConst.SpriteName.RANK_BG2_3);
        }
        else
        {
            Spt_RankNumBG1.gameObject.SetActive(false);
            Spt_RankNumBG2.gameObject.SetActive(false);
        }
        if (_isSelf)
        {
            Spt_RankNumBG2.gameObject.SetActive(false);
        }
    }

    private void UpdateStyleByIsSelf(bool _isSelf)
    {
        if (_isSelf)
        {
            CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_FINISHBG);
            CommonFunction.SetSpriteName(Spt_LabelBG, GlobalConst.SpriteName.RANK_SPILT_YELLOW);
            Lbl_LevelLabel.color = _selfColor;
            Lbl_UnionLabel.color = _selfColor;
            Lbl_LastWaveLabel.color = _selfColor;
            Lbl_TimeLabel.color = _selfColor;
            Lbl_NameLabel.color = _selfColor;
            Lbl_PointLabel.color = _selfColor;
            //Lbl_PointLabel.effectStyle = UILabel.Effect.None;
            Lbl_FightPowerLabel.color = _selfColor;
            //Lbl_FightPowerLabel.effectStyle = UILabel.Effect.None;
        }
        else
        {
            CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_UNFINISHBG);
            CommonFunction.SetSpriteName(Spt_LabelBG, GlobalConst.SpriteName.RANK_SPILT_DARK);

            Lbl_NameLabel.color = _otherNameColor;

            Lbl_LevelLabel.color = _otherColor;
            Lbl_UnionLabel.color = _otherColor;
            Lbl_LastWaveLabel.color = _otherColor;
            Lbl_TimeLabel.color = _otherColor;

            Lbl_PointLabel.color = _otherColor;
            //Lbl_PointLabel.effectStyle = UILabel.Effect.Outline;
            //Lbl_PointLabel.effectColor = Color.black;

            Lbl_FightPowerLabel.color = _otherColor;
            //Lbl_FightPowerLabel.effectStyle = UILabel.Effect.Outline;
            //Lbl_FightPowerLabel.effectColor = Color.black;
        }
    }

    private void UpdateLabelByType(RankType type)
    {
        Lbl_LevelLabel.transform.localPosition = _levelMiddlePos;

        switch (type)
        {
            case RankType.LEVEL_RANK:
                Lbl_FightPowerLabel.text = "";
                Lbl_LastWaveLabel.text = "";
                Lbl_PointLabel.text = "";
                Lbl_TimeLabel.text = "";
                Lbl_LevelLabel.transform.localPosition = _levelRightPos;
                break;
            case RankType.ARENA_RANK:
                Lbl_PointLabel.text = "";
                Lbl_LastWaveLabel.text = "";
                Lbl_LevelLabel.text = "";
                Lbl_TimeLabel.text = "";
                break;
            case RankType.COMBAT_RANK:
                Lbl_PointLabel.text = "";
                Lbl_LastWaveLabel.text = "";
                Lbl_LevelLabel.text = "";
                Lbl_TimeLabel.text = "";
                break;
            case RankType.ENDLESS_A:
            case RankType.ENDLESS_B:
            case RankType.ENDLESS_C:
                Lbl_UnionLabel.text = "";
                Lbl_FightPowerLabel.text = "";
                Lbl_LevelLabel.text = "";
                break;
            case RankType.UNIONOVERLORD:
            case RankType.CROSSSERVERWAR_UNION:
            case RankType.CROSSSERVERWAR_PERSONAL:
                Lbl_LastWaveLabel.text = "";
                Lbl_LevelLabel.text = "";
                Lbl_TimeLabel.text = "";
                Lbl_FightPowerLabel.text = "";
                Lbl_RankNumLabel.enabled = true;
                Lbl_OutRankLabel.enabled = true;
                break;
        }
    }


    /// <summary>
    /// 获取军团成员信息
    /// </summary>
    public void ObtainUnionMembersInfo(int vPos)
    {
        RankModule.Instance.SendRankPalyerInfoRequset(_type, mInformation);
    }
    /// <summary>
    /// 设置玩家VIP等级相关显示
    /// </summary>
    /// <param name="isplayer"></param>
    /// <param name="viplv"></param>
    private void SetVipLabel(uint viplv = 1,bool isplayer=true)
    {
        Spt_VIPSprite.gameObject.SetActive(isplayer);
        Lbl_VIPLabel.gameObject.SetActive(isplayer);
        Lbl_VIPLabel.text = viplv.ToString();
        if (isplayer)
        {
            Lbl_NameLabel.transform.localPosition = _playerNamePos;
        }
        else
        {
            Lbl_NameLabel.transform.localPosition = _unionNamePos;
        }
    }
}
