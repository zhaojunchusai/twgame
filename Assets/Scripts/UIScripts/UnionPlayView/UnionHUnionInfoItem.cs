using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
public class UnionHUnionInfoItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_UnionIconTexture;
    [HideInInspector]public UISprite Spt_UnionIconBG;
    [HideInInspector]public UISprite Spt_UnionQualitySprite;
    [HideInInspector]public UISprite Spt_ItemBGSprite;
    [HideInInspector]public UISprite Spt_StateSprite;
    [HideInInspector]public UISprite Spt_WinSprite;
    [HideInInspector]
    public GameObject Spt_FallObj;

    [HideInInspector]public UILabel Lbl_UnionNameLabel;
    [HideInInspector]public UILabel Lbl_UnionLvLabel;
    [HideInInspector]public UILabel Lbl_TeamCountLabel;
    [HideInInspector]public UILabel Lbl_WinCountLabel;

    private int _maxCount;

    private int _restCount;

    public int RestCount
    {
        get { return _restCount; }
        set { _restCount = value; }
    }
    private int _winCount;
    public int WinCount
    {
        get { return _winCount; }
        set { _winCount = value; }
    }
    public int MaxCount
    {
        get { return _maxCount; }
    }
    public void Initialize()
    {
        Spt_UnionIconTexture = transform.FindChild("UnionIconTexture").gameObject.GetComponent<UISprite>();
        Spt_UnionIconBG = transform.FindChild("UnionIconBG").gameObject.GetComponent<UISprite>();
        Spt_UnionQualitySprite = transform.FindChild("UnionQualitySprite").gameObject.GetComponent<UISprite>();
        Spt_ItemBGSprite = transform.FindChild("ItemBGSprite").gameObject.GetComponent<UISprite>();
        Spt_StateSprite = transform.FindChild("state").gameObject.GetComponent<UISprite>();
        Lbl_UnionNameLabel = transform.FindChild("UnionNameLabel").gameObject.GetComponent<UILabel>();
        Lbl_UnionLvLabel = transform.FindChild("UnionLvLabel").gameObject.GetComponent<UILabel>();
        Lbl_TeamCountLabel = transform.FindChild("TeamCountLabel").gameObject.GetComponent<UILabel>();
        Lbl_WinCountLabel = transform.FindChild("WinCountLabel").gameObject.GetComponent<UILabel>();
        Spt_WinSprite = transform.FindChild("WinSprite").gameObject.GetComponent<UISprite>();
        Spt_FallObj = transform.FindChild("FallSprite").gameObject;
        SetLabelValues();

    }

    void Awake()
    {
        Initialize(); 
    }

    public void SetLabelValues()
    {
        Lbl_UnionNameLabel.text = "";
        Lbl_TeamCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_TEAMCOUNT, 0, 0);
        Lbl_UnionLvLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, 0);
        Lbl_WinCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_WINCOUNT, 0);
        
    }

    public void Uninitialize()
    {

    }

    public void UpdataItem(BaseUnion info, int max, int rest, int winCount)
    {
        _maxCount = max;
        _restCount = rest;
        _winCount = winCount;
        Lbl_UnionNameLabel.text = info.name;
        Lbl_TeamCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_TEAMCOUNT, _restCount, _maxCount);
        Lbl_UnionLvLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, info.level);
        Lbl_WinCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_WINCOUNT, _winCount);
        CommonFunction.SetSpriteName(Spt_UnionIconTexture, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(info.icon));
        Spt_StateSprite.gameObject.SetActive(info.altar_status == AltarFlameStatus.DEPEND_STATUS);
        CommonFunction.SetQualitySprite(Spt_UnionQualitySprite, ItemQualityEnum.White);
        Spt_UnionIconTexture.gameObject.SetActive(true);
    }

    public void UpdateRestCount(int delta)
    {
        _restCount += delta;
        Lbl_TeamCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_TEAMCOUNT, _restCount, _maxCount);
    }

    public void UpdateWinCount(int delta)
    {
        _winCount += delta;
        Lbl_WinCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_WINCOUNT, _winCount);
    }
    public void ResetWinCount(int delta)
    {
        _winCount = delta;
        Lbl_WinCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_WINCOUNT, _winCount);
    }

    public void UpdateResult(bool isWin)
    {
        Spt_WinSprite.gameObject.SetActive(isWin);
        Spt_FallObj.SetActive(!isWin);
    }
    public void Clear()
    {
        _maxCount = 0;
        _restCount = 0;
        _winCount = 0;
        Lbl_UnionNameLabel.text = "";
        Lbl_TeamCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_TEAMCOUNT, _restCount, _maxCount);
        Lbl_UnionLvLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, 0);
        Lbl_WinCountLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_WINCOUNT, 0);
        Spt_UnionIconTexture.gameObject.SetActive(false);
        Spt_StateSprite.gameObject.SetActive(false);
        Spt_WinSprite.gameObject.SetActive(false);
        Spt_FallObj.SetActive(false);
        CommonFunction.SetQualitySprite(Spt_UnionQualitySprite, ItemQualityEnum.White);
    }
}
