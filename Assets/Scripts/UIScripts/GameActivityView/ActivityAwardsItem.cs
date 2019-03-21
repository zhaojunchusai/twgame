using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;

public class ActivityAwardsItem : GameActivityContentItemBase
{

    public bool mShowProgress = false;
    public bool mProgressRelayOnState = false;
    private int _maxValue = 0;
    private Vector3 _nBtnPos = new Vector3(150, 0, 0);
    private Vector3 _bBtnPos = new Vector3(150, -10, 0);

    [HideInInspector]public UISprite Spt_AwardsBGSprite;
    [HideInInspector]public UIButton Btn_GetAwardButton;
    [HideInInspector]public UILabel Lbl_BtnLabel;
    [HideInInspector]public UISprite Spt_BtnGetAwardButtonBGSprite;
    [HideInInspector]public UIGrid Grd_ItemsGrid;
    [HideInInspector]public UILabel Lbl_ProgressLabel;
    [HideInInspector]public UILabel Lbl_DescLabel;
    [HideInInspector]public MailAttachmentItem Item_0;
    [HideInInspector]public MailAttachmentItem Item_1;
    [HideInInspector]public MailAttachmentItem Item_2;

    private BoxCollider _boxCollider;

    private List<MailAttachmentItem> _itemList;
  
    public override void Initialize()
    {
        if (mIsInit)
            return;
        mIsInit = true;
        Spt_AwardsBGSprite = transform.FindChild("AwardsBGSprite").gameObject.GetComponent<UISprite>();
        Btn_GetAwardButton = transform.FindChild("GetAwardButton").gameObject.GetComponent<UIButton>();
        Spt_BtnGetAwardButtonBGSprite = transform.FindChild("GetAwardButton/BGSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnLabel = transform.FindChild("GetAwardButton/FGLabel").gameObject.GetComponent<UILabel>();
        Grd_ItemsGrid = transform.FindChild("ItemsGrid").gameObject.GetComponent<UIGrid>();
        Item_0 = transform.FindChild("ItemsGrid/Item_0").gameObject.AddComponent<MailAttachmentItem>();
        Item_1 = transform.FindChild("ItemsGrid/Item_1").gameObject.AddComponent<MailAttachmentItem>();
        Item_2 = transform.FindChild("ItemsGrid/Item_2").gameObject.AddComponent<MailAttachmentItem>();
        _boxCollider = this.GetComponent<BoxCollider>();
        Lbl_ProgressLabel = transform.FindChild("ProgressLabel").gameObject.GetComponent<UILabel>();
        Lbl_DescLabel = transform.FindChild("DescLabel").gameObject.GetComponent<UILabel>();
        SetLabelValues();

        Item_0.mHideNameLabel = true;
        Item_1.mHideNameLabel = true;
        Item_2.mHideNameLabel = true;
        _itemList = new List<MailAttachmentItem>() { Item_0, Item_1, Item_2 };

        
        UIEventListener.Get(Item_0.gameObject).onPress = PressItem;
        UIEventListener.Get(Item_1.gameObject).onPress = PressItem;
        UIEventListener.Get(Item_2.gameObject).onPress = PressItem;
        UIEventListener.Get(Btn_GetAwardButton.gameObject).onClick = ButtonEvent_GetAwardButton;
        UpdateState(GameAcitvityAwardStateEnum.CanNotReceive);
        HideItems();
    }

    void Awake()
    {
        Initialize(); 
    }

    public override void SetLabelValues()
    {
        Lbl_ProgressLabel.text = "0/0";
        Lbl_DescLabel.text = "";
        Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_GETAWARD_L;
    }

    public override void UnInitialize()
    {
        mPossibleAwardList.Clear();
    }
    
    public override void UpdateDesc(string text)
    {
        Initialize(); 
        if(string.IsNullOrEmpty(text))
            Lbl_DescLabel.text="";
        Lbl_DescLabel.text = text;
    }

    public override void UpdateState(GameAcitvityAwardStateEnum newState)
    {
        Initialize(); 
        base.UpdateState(newState);
        switch (_state)
        {
            case GameAcitvityAwardStateEnum.CanNotReceive:
                Spt_BtnGetAwardButtonBGSprite.color = Color.black;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_GETAWARD_L;
                Btn_GetAwardButton.collider.enabled = false;
                Lbl_BtnLabel.color = mLabelDisableColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
                break;
            case GameAcitvityAwardStateEnum.CanReceive:
                Spt_BtnGetAwardButtonBGSprite.color = Color.white;
                Btn_GetAwardButton.collider.enabled = true;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_GETAWARD_L;
                Lbl_BtnLabel.color = mLabelNormalColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.Outline;
                Lbl_BtnLabel.effectColor = mOutlineColor;
                break;
            case GameAcitvityAwardStateEnum.Recieved:
                Spt_BtnGetAwardButtonBGSprite.color = Color.black;
                Btn_GetAwardButton.collider.enabled = false;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                Lbl_BtnLabel.color = mLabelDisableColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
                break;
        }
    }
    GameActivityType _gameActivityType;
    public override void UpdateProgress(uint current)
    {
        Initialize();
        if (!mShowProgress)
            return;
        //单充奖励数值对应 比如单充30元的活动，充值40 50 100并不会激活
        if (mProgressRelayOnState)
        {
            if (_state != GameAcitvityAwardStateEnum.CanNotReceive)
                current = (uint)_maxValue;
            else
                current = 0;
        }
        else
        {
            if (_gameActivityType == GameActivityType.PVEGift)
                current = (uint)(_state == GameAcitvityAwardStateEnum.CanNotReceive ? 0 : 1);
            //current = (uint)Mathf.Min(_maxValue, current);           
        }
        Lbl_ProgressLabel.text = string.Format(ConstString.GAMEACTIVTIY_STATE_PROGRESS, CommonFunction.GetTenThousandUnit((int)current, 10000), CommonFunction.GetTenThousandUnit(_maxValue, 10000));
    }
    public override void UpdateProgress(ActivityReward data)
    {
        Initialize();

        if (_state == GameAcitvityAwardStateEnum.CanReceive)
        {
            Btn_GetAwardButton.enabled = true;
        }
        else
        {
            Btn_GetAwardButton.enabled = false;
        }
        Lbl_ProgressLabel.text = string.Format(ConstString.GAMEACTIVITY_RANK_SINGLECHARGETIME, data.had_get_times, data.all_can_times);
    }

    public void UpdateItemInfo(uint activityID, GameActivityReqData data, bool showProgress, GameActivityType type)
    {
        Initialize();
        _id = activityID;
        _signID = (uint)data.condition;
        mShowProgress = showProgress;
        _gameActivityType = type;
        if (mShowProgress)  
        {
            if (type == GameActivityType.PVEGift)
            {
                _maxValue = 1;
            }
            else
            {
                _maxValue = (int)data.condition;
            }
        }
        List<CommonItemData> list = new List<CommonItemData>();
        if(data.mDrops!=null&&data.mDrops.Count>0)
            foreach(var tmp in data.mDrops)
            {
                CommonItemData item = new CommonItemData(tmp.drop_id, tmp.drop_number);
                list.Add(item);
            }
        mPossibleAwardList.Clear();
        mPossibleAwardList.AddRange(list);
        //Debug.Log("ActivityAwardsItem DropID " + data.mDropID + " count "+ list.Count);
        //foreach (CommonItemData cData in list)
        //{
        //    Debug.Log("ActivityAwardsItem DropID " + cData.ID + " num " + cData.Num);
        //}
        int leng = Mathf.Min(list.Count, _itemList.Count);
        for (int i = 0; i < leng; i++)
        {
            MailAttachmentItem item = _itemList[i];
            CommonItemData itemData = list[i];
            item.gameObject.SetActive(true);
            item.UpdateItemInfo(itemData.ID, itemData.Num);

        }
        Grd_ItemsGrid.Reposition();
        UpdateItemStyle();
        UpdateDesc(ConstString.GAMEACTIVTIY_AWARD);
    }

    private void UpdateItemStyle()
    {
        Initialize();
        if (mShowProgress)
        {
            //Spt_AwardsBGSprite.height = (int)_bBGHeight;
            Btn_GetAwardButton.transform.localPosition = _bBtnPos;
            Lbl_ProgressLabel.gameObject.SetActive(true);
            //_boxCollider.size = _bColliderSize;
        }
        else
        {
            //Spt_AwardsBGSprite.height = (int)_nBGHeight;
            Btn_GetAwardButton.transform.localPosition = _bBtnPos;
            Lbl_ProgressLabel.text = "";
            Lbl_ProgressLabel.gameObject.SetActive(true);
            //_boxCollider.size = _nColliderSize;
        }
    }

    private void HideItems()
    {
        foreach (MailAttachmentItem item in _itemList)
        {
            item.gameObject.SetActive(false);
        }
    }
   
}
