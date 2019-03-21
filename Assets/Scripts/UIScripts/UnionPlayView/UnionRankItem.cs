using UnityEngine;
using fogs.proto.msg;
using System.Collections;

public class UnionRankItem : MonoBehaviour
{
    const int MAXRANK = 10000;
    const float MAXLENGTH = 401;
    [SerializeField]
    private int _maxDamage = 1;
    private int _curDamage = 0;

    public UnionRankItemType _itemType;
    
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
    private int _rank = 0;
    private UnionRankType _type;
    public UnionRankType Type
    {
        get { return _type; }
    }
    private Color _selfColor = new Color(215 / 255f, 190 / 255f, 152 / 255f);
    private Color _otherNameColor = new Color(227 / 255f, 169 / 255f, 75 / 255f);
    private Color _otherFightColor = new Color(160 / 255f, 139 / 255f, 113 / 255f);

    [HideInInspector]public UISprite Spt_ItemBG;
    [HideInInspector]public UISprite Spt_LabelBG;
    [HideInInspector]public UILabel Lbl_NameLabel;
    [HideInInspector]public UILabel Lbl_UnionPointLabel;
    [HideInInspector]public UILabel Lbl_KillNumLabel;
    [HideInInspector]public UISprite Spt_RankNumBG2;
    [HideInInspector]public UISprite Spt_RankNumBG1;
    [HideInInspector]public UILabel Lbl_RankNumLabel;
    [HideInInspector]public UILabel Lbl_FightPowerLabel;
    [HideInInspector]public UISprite Spt_ProgressBG;
    [HideInInspector]public UISprite Spt_ProgressFG;
    [HideInInspector]public UILabel Lbl_ProgressLabel;
    [HideInInspector]public UILabel Lbl_WinNumLabel;
    [HideInInspector]public GameObject Obj_ProgressObj;
    [HideInInspector]public UILabel Lbl_LevelLabel;
    [HideInInspector]public UISprite Spt_IconSprite;
    [HideInInspector]public UISprite Spt_FrameSprite;
    [HideInInspector]public UILabel Lbl_OutRankLabel;

    public void Initialize()
    {
        Spt_ItemBG = transform.FindChild("ItemBG").gameObject.GetComponent<UISprite>();
        Spt_LabelBG = transform.FindChild("LabelBG").gameObject.GetComponent<UISprite>();
        Lbl_NameLabel = transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_UnionPointLabel = transform.FindChild("UnionPointLabel").gameObject.GetComponent<UILabel>();
        Lbl_KillNumLabel = transform.FindChild("KillNumLabel").gameObject.GetComponent<UILabel>();
        Spt_RankNumBG2 = transform.FindChild("RankNumBG2").gameObject.GetComponent<UISprite>();
        Spt_RankNumBG1 = transform.FindChild("RankNumBG1").gameObject.GetComponent<UISprite>();
        Lbl_RankNumLabel = transform.FindChild("RankNumLabel").gameObject.GetComponent<UILabel>();
        Lbl_FightPowerLabel = transform.FindChild("FightPowerLabel").gameObject.GetComponent<UILabel>();
        Obj_ProgressObj = transform.FindChild("DamageProgress").gameObject;
        Spt_ProgressBG = transform.FindChild("DamageProgress/ProgressBG").gameObject.GetComponent<UISprite>();
        Spt_ProgressFG = transform.FindChild("DamageProgress/ProgressFG").gameObject.GetComponent<UISprite>();
        Lbl_ProgressLabel = transform.FindChild("DamageProgress/ProgressLabel").gameObject.GetComponent<UILabel>();
        Lbl_WinNumLabel = transform.FindChild("WinNumLabel").gameObject.GetComponent<UILabel>();
        
        Lbl_LevelLabel = transform.FindChild("IconGroup/LevelLabel").gameObject.GetComponent<UILabel>();
        Spt_IconSprite = transform.FindChild("IconGroup/IconSprite").gameObject.GetComponent<UISprite>();
        Spt_FrameSprite = transform.FindChild("IconGroup/FrameSprite").gameObject.GetComponent<UISprite>();
        Lbl_OutRankLabel = transform.FindChild("OutRankLabel").gameObject.GetComponent<UILabel>();

        SetLabelValues();
    }

    void Awake()
    {
        Initialize();
        string str = SystemInfo.deviceUniqueIdentifier;
    }

    public void SetLabelValues()
    {
        Lbl_NameLabel.text = "";
        Lbl_UnionPointLabel.text = string.Format(ConstString.UNIONRANK_UNIONPOINT, 0);
        Lbl_KillNumLabel.text = string.Format(ConstString.UNIONRANK_KILLS, 0);
        Lbl_RankNumLabel.text = "";
        Lbl_FightPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT, 0);
        Lbl_ProgressLabel.text = "";
        Lbl_WinNumLabel.text = string.Format(ConstString.UNIONRANK_WINS, 0);
        Lbl_LevelLabel.text = "";
        Lbl_OutRankLabel.text = ConstString.RANK_LABEL_OUTRANK;
    }

    public void Uninitialize()
    {

    }

    public void UpdateItem<T>(T tInfo, UnionRankType type,int rank,int maxDamge)
    {
        Clear();
        _type = type;
        _rank = rank;
        UpdateByRankType(_type);
        Obj_ProgressObj.gameObject.SetActive(false);
        switch (_type)
        {
            case UnionRankType.Damage:
                UnionPveMaxDamage info = tInfo as UnionPveMaxDamage;
                UnionMember me1 = UnionModule.Instance.GetUnionMember(info.charid);
                _itemType = UnionRankItemType.PlayerRank;
                UpdateBaseInfo(me1.charname, me1.level, _rank, me1.icon_frame, me1.icon, "", ItemQualityEnum.White);
                Obj_ProgressObj.gameObject.SetActive(true);
                _curDamage = info.damage;
                _maxDamage = maxDamge > 0 ? maxDamge : 1;
                Lbl_ProgressLabel.text = info.damage.ToString();
                UpdateProgress(_curDamage, _maxDamage);
                break;
            case UnionRankType.Kill:
                UnionPvpKillRank info2 = tInfo as UnionPvpKillRank;
                UnionMember me2 = UnionModule.Instance.GetUnionMember((uint)info2.charid);
                _itemType = UnionRankItemType.PlayerRank;
                UpdateBaseInfo(me2.charname, me2.level, _rank, me2.icon_frame, me2.icon, "", ItemQualityEnum.White);
                Lbl_UnionPointLabel.text = string.Format(ConstString.UNIONRANK_PERSONALPOINT, info2.score);
                Lbl_KillNumLabel.text = string.Format(ConstString.UNIONRANK_KILLS, info2.kill_num);
                Lbl_FightPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT,CommonFunction.GetTenThousandUnit(info2.combat_power));
                //Debug.LogWarning("info2.kill_num " + info2.kill_num);
                break;
            case UnionRankType.Hegemony:
                UnionPvpRankInfo info3 = tInfo as UnionPvpRankInfo;
                BaseUnion union = info3.union;
                _itemType = UnionRankItemType.UnionRank;
                string uIconStr = ConfigManager.Instance.mUnionConfig.GetUnionIconByID(union.icon);
                UpdateBaseInfo(union.name, union.level, _rank, 0, 0, uIconStr, ItemQualityEnum.White);
                Lbl_WinNumLabel.text = string.Format(ConstString.UNIONRANK_WINS, info3.wins);
                Lbl_UnionPointLabel.text = string.Format(ConstString.UNIONRANK_UNIONPOINT, info3.score);
                break;
        }
    }
   
    private void UpdateByRankType(UnionRankType type)
    {
        switch (type)
        {
            case UnionRankType.Damage:
                Lbl_FightPowerLabel.text = "";
                Lbl_KillNumLabel.text = "";
                Lbl_UnionPointLabel.text = "";
                Lbl_WinNumLabel.text = "";
                Obj_ProgressObj.gameObject.SetActive(true);
                break;
            case UnionRankType.Kill: 
                Lbl_WinNumLabel.text = "";
                Obj_ProgressObj.gameObject.SetActive(false);
                break;
            case UnionRankType.Hegemony:
                Lbl_KillNumLabel.text = "";
                Lbl_FightPowerLabel.text = "";
                Obj_ProgressObj.gameObject.SetActive(false);
                break;
        }
    }

    private void UpdateProgress(int current, int max)
    {
        float amount = (float)current / max;
        amount = Mathf.Clamp01(amount);
        Spt_ProgressFG.width = (int)(amount * MAXLENGTH);
    }

    private void UpdateNumBGByRank(int rank)
    {

        if (!Spt_RankNumBG1.gameObject.activeSelf)
        {
            Spt_RankNumBG1.gameObject.SetActive(true);
            Spt_RankNumBG2.gameObject.SetActive(true);
        }
        if (_rank == 1)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG1, GlobalConst.SpriteName.RANK_BG1_1);
            CommonFunction.SetSpriteName(Spt_RankNumBG2, GlobalConst.SpriteName.RANK_BG2_1);
        }
        else if (_rank == 2)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG1, GlobalConst.SpriteName.RANK_BG1_2);
            CommonFunction.SetSpriteName(Spt_RankNumBG2, GlobalConst.SpriteName.RANK_BG2_2);
        }
        else if (_rank == 3)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG1, GlobalConst.SpriteName.RANK_BG1_3);
            CommonFunction.SetSpriteName(Spt_RankNumBG2, GlobalConst.SpriteName.RANK_BG2_3);
        }
        else
        {
            Spt_RankNumBG1.gameObject.SetActive(false);
            Spt_RankNumBG2.gameObject.SetActive(false);
        }

        if(_isSelf)
            Spt_RankNumBG2.gameObject.SetActive(false);
      
    }

    private void UpdateStyleByIsSelf(bool _isSelf)
    {
        if (_isSelf)
        {
            CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_FINISHBG);
            CommonFunction.SetSpriteName(Spt_LabelBG, GlobalConst.SpriteName.RANK_SPILT_YELLOW);
            CommonFunction.SetSpriteName(Spt_ProgressFG, GlobalConst.SpriteName.UNIONRANK_PROGRESS_GREEN);
            Lbl_NameLabel.color = _selfColor;
            Lbl_WinNumLabel.color = _selfColor;
            Lbl_KillNumLabel.color = _selfColor;
            Lbl_FightPowerLabel.color = _selfColor;
            Lbl_UnionPointLabel.color = _selfColor;
        }
        else
        {
            CommonFunction.SetSpriteName(Spt_ItemBG, GlobalConst.SpriteName.TASK_UNFINISHBG);
            CommonFunction.SetSpriteName(Spt_LabelBG, GlobalConst.SpriteName.RANK_SPILT_DARK);
            CommonFunction.SetSpriteName(Spt_ProgressFG, GlobalConst.SpriteName.UNIONRANK_PROGRESS_YELLOW);
            Lbl_NameLabel.color = _otherNameColor;
            Lbl_WinNumLabel.color = _otherNameColor;
            Lbl_KillNumLabel.color = _otherNameColor;
            Lbl_FightPowerLabel.color = _otherFightColor;
            Lbl_UnionPointLabel.color = _otherFightColor;
        }
    }

    public void UpdateBaseInfo(string name, int level, int rank, uint frameID, uint iconID,string unionIcon, ItemQualityEnum quality)
    {
        if (rank < UnionRankViewController.MAXRANK)
        {
            Lbl_RankNumLabel.text = rank.ToString();
            Lbl_OutRankLabel.gameObject.SetActive(false);
        }
        else
        {
            Lbl_RankNumLabel.text = "";
            Lbl_OutRankLabel.gameObject.SetActive(true);
        }
        UpdateNumBGByRank(rank);
        Lbl_LevelLabel.text = level.ToString();
        Lbl_NameLabel.text = name;
        if (_itemType == UnionRankItemType.PlayerRank)
        {
            CommonFunction.SetHeadAndFrameSprite(Spt_IconSprite, Spt_FrameSprite, iconID, frameID, true);
        }
        else
        {
            CommonFunction.SetSpriteName(Spt_IconSprite, unionIcon);
            CommonFunction.SetQualitySprite(Spt_FrameSprite, quality);
        }
    }

    public void Clear()
    {
        _rank = 0;
        _type = UnionRankType.Damage;
        _itemType = UnionRankItemType.PlayerRank;
        _maxDamage = 1;

        Lbl_NameLabel.text = "";
        Lbl_UnionPointLabel.text = "";
        Lbl_KillNumLabel.text = "";
        Lbl_WinNumLabel.text = "";
        Lbl_RankNumLabel.text = "";
        Lbl_FightPowerLabel.text = "";
        Lbl_ProgressLabel.text = "";
        Lbl_LevelLabel.text = "";

        CommonFunction.SetSpriteName(Spt_IconSprite, GlobalConst.SpriteName.Attribute_Attack);
        CommonFunction.SetQualitySprite(Spt_IconSprite, ItemQualityEnum.White);
    }
}
