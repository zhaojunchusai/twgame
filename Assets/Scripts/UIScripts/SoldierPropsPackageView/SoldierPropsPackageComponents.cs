using UnityEngine;
using System.Collections.Generic;
public class SoldierPropsPackageComponent : BaseComponent
{
    private UIGrid Grd_StarsGrid;
    private UISprite Spt_StarSprite;
    private UISprite Spt_BGSprite;
    //private UISprite Spt_IconSprite;
    private UILabel Lbl_LeadershipLabel;
    private UISprite Spt_QualitySprite;
    private UISprite Spt_IconTexture;
    private UISprite Spt_ItemBG;
    private UISprite Spt_SelectSprite;
    private UISprite Spt_LevelBG;
    private UILabel Lbl_LevelLabel;
    private UILabel Lbl_NumLabel;
    private UILabel Lbl_SoldierTypeLabel;
    private UILabel Lbl_SoldierName;
    private GameObject EquipChip;
    private GameObject SoldierChip;
    public UILabel lbl_Label_Step;
    private PropsPackageCommonData PropsPackageInfo;
    public PropsPackageCommonData _PropsPackageInfo
    {
        get
        {
            return PropsPackageInfo;
        }
    }

    private List<GameObject> starlist;

    private bool _isSelect = false;
    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool IsSelect
    {
        set
        {
            _isSelect = value;
            Spt_SelectSprite.enabled = _isSelect;
        }
        get
        {
            return _isSelect;
        }
    }



    private int[] SeatPos = { 3, 6, 5 };
    private int[] Carrer = { 2, 0, 1, 4 };

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        mRootObject = root;
        Grd_StarsGrid = mRootObject.transform.FindChild("StarsGrid").gameObject.GetComponent<UIGrid>();
        Spt_StarSprite = mRootObject.transform.FindChild("StarsGrid/StarSprite").gameObject.GetComponent<UISprite>();
        //Spt_IconSprite = mRootObject.transform.FindChild("LeadershipGroup/IconSprite").gameObject.GetComponent<UISprite>();
        Lbl_LeadershipLabel = mRootObject.transform.FindChild("LeadershipGroup/LeadershipLabel").gameObject.GetComponent<UILabel>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBG = mRootObject.transform.FindChild("ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabel = mRootObject.transform.FindChild("LevelBG/LevelLabel").gameObject.GetComponent<UILabel>();
        Spt_LevelBG = mRootObject.transform.FindChild("LevelBG").gameObject.GetComponent<UISprite>();
        Lbl_NumLabel = mRootObject.transform.FindChild("NumLabel").gameObject.GetComponent<UILabel>();
        Lbl_SoldierTypeLabel = mRootObject.transform.FindChild("SoldierTypeGroup/SoldierTypeLabel").gameObject.GetComponent<UILabel>();
        Lbl_SoldierName = mRootObject.transform.FindChild("SoldierName").gameObject.GetComponent<UILabel>();
        EquipChip = mRootObject.transform.FindChild("EquipChip").gameObject;
        SoldierChip = mRootObject.transform.FindChild("SoldierChip").gameObject;
        lbl_Label_Step = mRootObject.transform.FindChild("ItemBaseComp/Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        Clear();
        if (starlist == null)
            starlist = new List<GameObject>();
        Spt_StarSprite.gameObject.SetActive(false);
    }
    public void UpdateCompInfo(PropsPackageCommonData info)
    {
        PropsPackageInfo = info;
        if (info == null)
        {
            Clear();
            return;
        }
        mRootObject.SetActive(true);
        if (PropsPackageInfo.level <= 0)
            this.Spt_LevelBG.gameObject.SetActive(false);
        else
        {
            this.Spt_LevelBG.gameObject.SetActive(true);
            Lbl_LevelLabel.text = PropsPackageInfo.level.ToString();
        }
        if (info.isSoldier)
            this.lbl_Label_Step.text = CommonFunction.GetStepShow(this.lbl_Label_Step,0);
        else
            this.lbl_Label_Step.text = "";
        if(PropsPackageInfo.num < 2)
        {
            this.Lbl_NumLabel.text = "";
        }
        else
        {
            this.Lbl_NumLabel.text = string.Format("x{0}",CommonFunction.GetTenThousandUnit(PropsPackageInfo.num));
        }
        this.EquipChip.SetActive(PropsPackageInfo.isEquipChip);
        this.SoldierChip.SetActive(PropsPackageInfo.isSoldierChip);
        Lbl_SoldierName.text = PropsPackageInfo.name;
        CommonFunction.SetSpriteName(Spt_IconTexture, PropsPackageInfo.icon);
        CommonFunction.SetQualitySprite(Spt_QualitySprite, PropsPackageInfo.quality, Spt_ItemBG);
        UpdateNum(PropsPackageInfo.num);
        UpdateStars(PropsPackageInfo.star);
    }

    private void UpdateStars(int level)
    {
        if (level < starlist.Count)
        {
            for (int i = 0; i < starlist.Count; i++)
            {
                GameObject go = starlist[i];
                if (i < level)
                {
                    go.SetActive(true);
                }
                else
                {
                    go.SetActive(false);
                }
            }
        }
        else
        {
            int index = starlist.Count;
            for (int i = 0; i < level; i++)
            {
                GameObject go = null;
                if (i < index)
                {
                    go = starlist[i];
                }
                else
                {
                    go = CommonFunction.InstantiateObject(Spt_StarSprite.gameObject, Grd_StarsGrid.transform);
                    go.name = "star_" + i.ToString();
                    starlist.Add(go);
                }
                if (go == null) continue;
                go.SetActive(true);
            }
        }
        Grd_StarsGrid.repositionNow = true;
    }

    public void UpdateNum(int num)
    {
        if (num == 0 || num == 1)
        {
            Lbl_NumLabel.text = string.Empty;
        }
        else
        {
            Lbl_NumLabel.text = "x" + num.ToString();
        }
    }


    public override void Clear()
    {
        base.Clear();
        PropsPackageInfo = null;
        Lbl_LeadershipLabel.text = string.Empty;
        Lbl_LevelLabel.text = string.Empty;
        Lbl_SoldierName.text = string.Empty;
        IsSelect = false;
        mRootObject.SetActive(false);
    }

}
