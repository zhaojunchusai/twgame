using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleViewAwardItemComponent : BaseComponent
{
    private GameObject Gobj_SpecailAwardComp;
    private GameObject Gobj_NormalAwardComp;
    private UISprite Spt_SpecailIcon;
    private UILabel Lbl_AwardNum;
    private UISprite Spt_QualitySprite;
    private UISprite Spt_ItemBgSprite;
    private UISprite Spt_NormalIcon;
    private UILabel Lbl_CountLabel;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Gobj_NormalAwardComp = mRootObject.transform.FindChild("gobj_NormalAwardComp").gameObject;
        Gobj_SpecailAwardComp = mRootObject.transform.FindChild("gobj_SpecailAwardComp").gameObject;
        Spt_SpecailIcon = mRootObject.transform.FindChild("gobj_SpecailAwardComp/IconSprite").gameObject.GetComponent<UISprite>();
        Lbl_AwardNum = mRootObject.transform.FindChild("gobj_SpecailAwardComp/AwardNum").gameObject.GetComponent<UILabel>();
        Spt_QualitySprite = mRootObject.transform.FindChild("gobj_NormalAwardComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("gobj_NormalAwardComp/ItemBgSprite").gameObject.GetComponent<UISprite>();
        Spt_NormalIcon = mRootObject.transform.FindChild("gobj_NormalAwardComp/IconSprite").gameObject.GetComponent<UISprite>();
        Lbl_CountLabel = mRootObject.transform.FindChild("gobj_NormalAwardComp/CountLabel").gameObject.GetComponent<UILabel>();
    }

    public void UpdateCompInfo(CommonItemData data)
    {
        if (data == null) return;
        if (data.Quality == ItemQualityEnum.None)
        {
            Gobj_NormalAwardComp.SetActive(false);
            Gobj_SpecailAwardComp.SetActive(true);
            CommonFunction.SetSpriteName(Spt_SpecailIcon, data.Icon);
            Lbl_AwardNum.text = "x" + data.Num.ToString();
        }
        else
        {
            Gobj_NormalAwardComp.SetActive(true);
            Gobj_SpecailAwardComp.SetActive(false);
            CommonFunction.SetSpriteName(Spt_NormalIcon, data.Icon);
            CommonFunction.SetQualitySprite(Spt_QualitySprite, data.Quality, Spt_ItemBgSprite);
            Lbl_CountLabel.text = "x" + data.Num.ToString();
        }
    }
}

public class RuleViewAwardGroupComponent : BaseComponent
{
    private UILabel Lbl_Rank;
    private UITable UITable_Awards;
    private GameObject Gobj_AwardComp;
    public List<RuleViewAwardItemComponent> awards_dic;
    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_Rank = mRootObject.transform.FindChild("Rank").GetComponent<UILabel>();
        UITable_Awards = mRootObject.transform.FindChild("Awards").GetComponent<UITable>();
        Gobj_AwardComp = mRootObject.transform.FindChild("Awards/gobj_AwardComp").gameObject;
        Gobj_AwardComp.SetActive(false);
    }

    public void UpdateCompInfo(string rank, List<CommonItemData> list)
    {
        Lbl_Rank.text = rank;
        UpdateAwardItems(list);
    }

    private void UpdateAwardItems(List<CommonItemData> list)
    {
        if (awards_dic == null)
            awards_dic = new List<RuleViewAwardItemComponent>();
        int obj_Index = awards_dic.Count;
        if (list.Count <= obj_Index)
        {
            for (int i = list.Count; i < awards_dic.Count; i++)
            {
                RuleViewAwardItemComponent comp = awards_dic[i];
                if (comp == null) continue;
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            CommonItemData mItemData = list[i];
            if (mItemData == null) continue;
            RuleViewAwardItemComponent comp = null;
            if (i < obj_Index)
            {
                comp = awards_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(Gobj_AwardComp, UITable_Awards.transform);
                go.name = "AwardComp_" + i.ToString();
                comp = new RuleViewAwardItemComponent();
                comp.MyStart(go);
                awards_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(mItemData);
            comp.mRootObject.SetActive(true);
        }
        UITable_Awards.Reposition();
    }

    public void Destory()
    {
        awards_dic.Clear();
    }
}


public class RuleDescGroupComponent : BaseComponent
{
    private UILabel Lbl_RuleTitle;
    private UISprite Spt_RuleDesc;
    private UITable UITable_GetAwards;
    private GameObject Gobj_AwardGroup;
    private List<RuleViewAwardGroupComponent> awards_dic;
    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_RuleTitle = mRootObject.transform.FindChild("RuleTitleGroup/RuleTitle").GetComponent<UILabel>();
        UITable_GetAwards = mRootObject.transform.FindChild("GetAwards").GetComponent<UITable>();
        Spt_RuleDesc = mRootObject.transform.FindChild("RuleDescSprite").GetComponent<UISprite>();
        Gobj_AwardGroup = mRootObject.transform.FindChild("GetAwards/gobj_AwardGroup").gameObject;
        Gobj_AwardGroup.SetActive(false);
    }

    public void UpdateCompInfo(string title, List<CommonRuleAwardItem> list)
    {
        Lbl_RuleTitle.text = title;
        Main.Instance.StartCoroutine(UpdateAwardItems(list));
    }

    private IEnumerator UpdateAwardItems(List<CommonRuleAwardItem> list)
    {
        if (awards_dic == null)
            awards_dic = new List<RuleViewAwardGroupComponent>();
        int obj_Index = awards_dic.Count;
        if (list.Count <= obj_Index)
        {
            for (int i = list.Count; i < awards_dic.Count; i++)
            {
                RuleViewAwardGroupComponent comp = awards_dic[i];
                if (comp == null) continue;
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            CommonRuleAwardItem mItemData = list[i];
            if (mItemData == null) continue;
            RuleViewAwardGroupComponent comp = null;
            if (i < obj_Index)
            {
                comp = awards_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(Gobj_AwardGroup, UITable_GetAwards.transform);
                go.name = "AwardComp_" + i.ToString();
                comp = new RuleViewAwardGroupComponent();
                comp.MyStart(go);
                awards_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(mItemData.Rank, mItemData.Awards);
            comp.mRootObject.SetActive(true);
        }
        CalculateSpriteSize(list.Count);
        yield return null;
        UITable_GetAwards.repositionNow = true;
    }

    private void CalculateSpriteSize(int count)
    {
        Spt_RuleDesc.height = 80 * count + 80;
    }

    public void Destory()
    {
        awards_dic.Clear();
    }
}