using System;
using System.Collections.Generic;
using UnityEngine;
public class PetMaterialComponent : BaseComponent
{
    private UISprite Spt_MatrialIcon;
    private UISprite Spt_MatrialFrame;
    private UISprite Spt_MatrialBG;
    private UILabel Lbl_MaterialCount;
    private UISprite Spt_ConsumeIcon;
    private UILabel Lbl_ConsumeNum;
    public GameObject Gobj_CompletionGroup;
    private UISprite Spt_CompletionIcon;
    private UILabel Lbl_CompletionNum;

    private GameObject Gobj_MaterialComp;

    private MoneyFlowData mConsumeData;
    public MoneyFlowData ConsumeData
    {
        get
        {
            return mConsumeData;
        }
    }

    private MoneyFlowData mCompletionData;
    public MoneyFlowData CompletionData
    {
        get
        {
            return mCompletionData;
        }
    }

    private uint itemID;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_MatrialIcon = mRootObject.transform.Find("gobj_MaterialComp/IconTexture").GetComponent<UISprite>();
        Spt_MatrialFrame = mRootObject.transform.Find("gobj_MaterialComp/QualitySprite").GetComponent<UISprite>();
        Spt_MatrialBG = mRootObject.transform.Find("gobj_MaterialComp/BgSprite").GetComponent<UISprite>();
        Lbl_MaterialCount = mRootObject.transform.Find("gobj_MaterialComp/CountLabel").GetComponent<UILabel>();
        Spt_ConsumeIcon = mRootObject.transform.Find("ConsumeGroup/Icon").GetComponent<UISprite>();
        Lbl_ConsumeNum = mRootObject.transform.Find("ConsumeGroup/Cost").GetComponent<UILabel>();
        Gobj_CompletionGroup = mRootObject.transform.Find("QuickMatItem").gameObject;
        Spt_CompletionIcon = mRootObject.transform.Find("QuickMatItem/Icon").GetComponent<UISprite>();
        Lbl_CompletionNum = mRootObject.transform.Find("QuickMatItem/Cost").GetComponent<UILabel>();
        Gobj_MaterialComp = mRootObject.transform.Find("gobj_MaterialComp").gameObject;
        UIEventListener.Get(Gobj_MaterialComp).onClick = ButtonEvent_Material;
    }


    public void UpdateCompInfo(uint itemid, int ownCount, int needCount, int consumeCurrencyType, int comsumeNum)
    {
        itemID = itemid;
        mCompletionData = new MoneyFlowData();
        mConsumeData = new MoneyFlowData();
        mConsumeData.Type = (ECurrencyType)consumeCurrencyType;
        mConsumeData.Number = comsumeNum;
        ItemInfo itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(itemid);
        if (itemInfo == null)
            return;
        CommonFunction.SetSpriteName(Spt_MatrialIcon, itemInfo.icon);
        CommonFunction.SetQualitySprite(Spt_MatrialFrame, itemInfo.quality, Spt_MatrialBG);
        CommonFunction.SetMoneyIcon(Spt_ConsumeIcon, (ECurrencyType)consumeCurrencyType);
        int ownCurrency = PlayerData.Instance.GetCurrencyNumByType((ECurrencyType)consumeCurrencyType);
        if (ownCurrency >= comsumeNum)
        {
            Lbl_ConsumeNum.text = "[CEAD52]" + CommonFunction.GetTenThousandUnit(ownCurrency) + "/" + CommonFunction.GetTenThousandUnit(comsumeNum) + "[-]";
        }
        else
        {
            Lbl_ConsumeNum.text = "[FF0000]" + CommonFunction.GetTenThousandUnit(ownCurrency) + "/" + CommonFunction.GetTenThousandUnit(comsumeNum) + "[-]";

        }
        if (ownCount >= needCount)
        {
            CommonFunction.SetMoneyIcon(Spt_CompletionIcon, itemInfo.price.Type);
            Lbl_CompletionNum.text = "0";
            Lbl_MaterialCount.text = "[AEFF2D]" + ownCount.ToString() + "/" + needCount.ToString() + "[-]";
            mCompletionData.Type = itemInfo.price.Type;
            mCompletionData.Number = 0;
        }
        else
        {
            int completionNum = (needCount - ownCount) * itemInfo.price.Number;
            Lbl_MaterialCount.text = "[FF0000]" + ownCount.ToString() + "/" + needCount.ToString() + "[-]";
            CommonFunction.SetMoneyIcon(Spt_CompletionIcon, itemInfo.price.Type);
            Lbl_CompletionNum.text = completionNum.ToString();
            mCompletionData.Type = itemInfo.price.Type;
            mCompletionData.Number = completionNum;
        }
    }

    private void ButtonEvent_Material(GameObject go)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
        UISystem.Instance.GetPathView.UpdateViewInfo(itemID, 1);
    }
}
