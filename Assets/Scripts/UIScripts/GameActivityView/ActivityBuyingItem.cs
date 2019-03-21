using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ActivityBuyingItem : GameActivityContentItemBase
{
    [HideInInspector]public UISprite Spt_ArrowSprite;
    [HideInInspector]public UISprite Spt_AddSprite;
    [HideInInspector]public UIButton Btn_AwardsButton;
    [HideInInspector]public UISprite Spt_BtnAwardButtonBGSprite;
    [HideInInspector]public UILabel Lbl_BtnLabel;
    [HideInInspector]public UISprite Spt_AwardsBGSprite;
    [HideInInspector]public UIGrid Grd_ItemsGrid;
    [HideInInspector]public UILabel Lbl_DescLabel;
    [HideInInspector]public MailAttachmentItem Item_0;
    [HideInInspector]public MailAttachmentItem Item_1;
    [HideInInspector]public MailAttachmentItem Item_2;
    private List<MailAttachmentItem> _itemList;

    private uint _reqID;
    private uint _reqNum;
    private bool _result;

    public override void Initialize()
    {
        if (mIsInit)
            return;
        mIsInit = true;
        Spt_ArrowSprite = transform.FindChild("ArrowSprite").gameObject.GetComponent<UISprite>();
        Spt_AddSprite = transform.FindChild("AddSprite").gameObject.GetComponent<UISprite>();
        Btn_AwardsButton = transform.FindChild("AwardButton").gameObject.GetComponent<UIButton>();
        Spt_BtnAwardButtonBGSprite = transform.FindChild("AwardButton/BGSprite").gameObject.GetComponent<UISprite>(); 
        Lbl_BtnLabel = transform.FindChild("AwardButton/FGLabel").gameObject.GetComponent<UILabel>();
        Spt_AwardsBGSprite = transform.FindChild("AwardsBGSprite").gameObject.GetComponent<UISprite>();
        Grd_ItemsGrid = transform.FindChild("ItemsGrid").gameObject.GetComponent<UIGrid>();
        Item_0 = transform.FindChild("ItemsGrid/Item_0").gameObject.AddComponent<MailAttachmentItem>();
        Item_1 = transform.FindChild("ItemsGrid/Item_1").gameObject.AddComponent<MailAttachmentItem>();
        Item_2 = transform.FindChild("ItemsGrid/Item_2").gameObject.AddComponent<MailAttachmentItem>();
        SetLabelValues();
        Item_0.mHideNameLabel = true;
        Item_1.mHideNameLabel = true;
        Item_2.mHideNameLabel = true;
        UIEventListener.Get(Item_0.gameObject).onPress = PressItem;
        UIEventListener.Get(Item_1.gameObject).onPress = PressItem;
        UIEventListener.Get(Item_2.gameObject).onPress = PressItem;
        _itemList = new List<MailAttachmentItem>() { Item_0, Item_1, Item_2 };
        UIEventListener.Get(Btn_AwardsButton.gameObject).onClick = ButtonEvent_GetAwardButton;
    }

    void Awake()
    {
        Init();
    }

    void Init()
    {
        Initialize();
        HideItems();
    }
    private void ButtonEvent_GetAwardButton(GameObject go)
    {
        IDType type = CommonFunction.GetTypeOfID(_reqID.ToString());
        _result = false;
        switch (type)
        {
            case IDType.Gold:
                if (PlayerData.Instance._Gold < _reqNum)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_GOLD);
                }
                else
                    _result = true;
                break;
            case IDType.Diamond:
                if (PlayerData.Instance._Diamonds < _reqNum)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_DIAMOND);
                }
                else
                    _result = true;
                break;
            case IDType.Medal:
                if (PlayerData.Instance._Medal < _reqNum)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_MEDAL);
                }
                else
                    _result = true;
                break;
            case IDType.Honor:
                if (PlayerData.Instance._Honor < _reqNum)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_HONOR);
                }
                else
                    _result = true;
                break;
            case IDType.UnionToken:
                if (PlayerData.Instance.UnionToken < _reqNum)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_TOKEN);
                }
                else
                    _result = true;
                break;
            case IDType.RecycleCoin:
                if (PlayerData.Instance.RecycleCoin < _reqNum)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_RECYCLECOIN);
                }
                else
                    _result = true;
                break;
        }
        if (_result)
            GameActivityModule.Instance.OnSendActivityRewardRequest(_id, _signID);
    }

    public override void SetLabelValues()
    {
        Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_EXCHANGE;
    }

    public override void UnInitialize()
    {

    }

    public override void UpdateState(GameAcitvityAwardStateEnum newState)
    {
        base.UpdateState(newState);
        switch (_state)
        {
            case GameAcitvityAwardStateEnum.CanNotReceive:
                Spt_BtnAwardButtonBGSprite.color = Color.black;
                Btn_AwardsButton.collider.enabled = false;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_EXCHANGE;
                Lbl_BtnLabel.color = mLabelDisableColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
                break;
            case GameAcitvityAwardStateEnum.CanReceive:
                Spt_BtnAwardButtonBGSprite.color = Color.white;
                Btn_AwardsButton.collider.enabled = true;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_EXCHANGE;
                Lbl_BtnLabel.color = mLabelNormalColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.Outline;
                Lbl_BtnLabel.effectColor = mOutlineColor;
                break;
            case GameAcitvityAwardStateEnum.Recieved:
                Spt_BtnAwardButtonBGSprite.color = Color.black;
                Btn_AwardsButton.collider.enabled = false;
                Lbl_BtnLabel.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYEXCHANGE;
                Lbl_BtnLabel.color = mLabelDisableColor;
                Lbl_BtnLabel.effectStyle = UILabel.Effect.None;
                break;
        }

    }

    public void UpdateItemInfo(uint activityID, GameActivityReqData data, uint costID)
    {
        Init();
        _id = activityID;
        _signID = data.rewards_id;
        Item_0.gameObject.SetActive(true);
        Item_0.UpdateItemInfo(costID, data.condition);
        _reqID = costID;
        _reqNum = (uint)data.condition;
       
        List<CommonItemData> list = new List<CommonItemData>();
        if (data.mDrops != null && data.mDrops.Count > 0)
            foreach (var tmp in data.mDrops)
            {
                CommonItemData item = new CommonItemData(tmp.drop_id, tmp.drop_number);
                list.Add(item);
            }
        int leng = Mathf.Min(list.Count, _itemList.Count - 1);
        for (int i = 0; i < leng; i++)
        {
            MailAttachmentItem item = _itemList[i + 1];
            CommonItemData itemData = list[i];
            item.gameObject.SetActive(true);
            item.UpdateItemInfo(itemData.ID, itemData.Num);
        }
        bool activeAddSp = leng > 1;
        Spt_AddSprite.gameObject.SetActive(activeAddSp);
        Grd_ItemsGrid.Reposition();
    }
  
    private void HideItems()
    {
        foreach (MailAttachmentItem item in _itemList)
        {
            item.gameObject.SetActive(false);
        }
    }

    
}
