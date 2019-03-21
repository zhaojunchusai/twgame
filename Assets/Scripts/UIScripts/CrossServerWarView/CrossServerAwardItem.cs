using UnityEngine;
using System.Collections;

public class CrossServerAwardItem : MonoBehaviour{

    private UISprite Spt_GoodsPropBG;
    private UISprite Spt_GoodsQuality;
    private UISprite Spt_GoodsIcon;
    private UISprite Spt_GoodsMark;
    private UILabel Lbl_GoodsNum;
    private UIDragScrollView DSV_GoodsDrag;

    private CommonItemData item;

    public void Initialize()
    {
        Spt_GoodsPropBG = transform.FindChild("GoodsPropBG").gameObject.GetComponent<UISprite>();
        Spt_GoodsQuality = transform.FindChild("GoodsQuality").gameObject.GetComponent<UISprite>();
        Spt_GoodsIcon = transform.FindChild("GoodsIcon").gameObject.GetComponent<UISprite>();
        Spt_GoodsMark = transform.FindChild("GoodsMark").gameObject.GetComponent<UISprite>();
        Lbl_GoodsNum = transform.FindChild("GoodsNum").gameObject.GetComponent<UILabel>();
        DSV_GoodsDrag = transform.FindChild("GoodsPropBG").gameObject.GetComponent<UIDragScrollView>();
    }

    public void SetItemInfo(CommonItemData _item, UIScrollView target=null)
    {
        item = _item;
        CommonFunction.ShowItemByInfo(_item, Spt_GoodsPropBG, Spt_GoodsIcon, Spt_GoodsQuality, Lbl_GoodsNum, Spt_GoodsMark);
        if(target!=null)
            DSV_GoodsDrag.scrollView = target;
        gameObject.SetActive(true);
        UIEventListener.Get(Spt_GoodsPropBG.gameObject).onPress = ButtonEvent_ShowItemInfo;
    }

    public void ButtonEvent_ShowItemInfo(GameObject go,bool press)
    {
        HintManager.Instance.SeeDetail(go, press, item.ID);
    }

}
