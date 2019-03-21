using System.Collections.Generic;
using UnityEngine;

public class LineupItemComponent : ItemBaseComponent
{
    public UIGrid Grd_StarsGrid;     //星级父物体
    public UISprite Spt_StarSprite;  //星级模板
    public Transform Gobj_LeadershipGroup;
    public UILabel Lbl_LeadershipLabel;  //统御力
    public UISprite Spt_SelectSprite;
    public Transform Gobj_EnergyGroup;
    public UILabel Lbl_EnergyLabel;    //召唤能量
    public UILabel Lbl_NumLabel;
    public UILabel Lbl_LevelLabel;
    public UISprite Spt_MaskSprite;   //遮罩
    public UILabel lbl_Label_Step;
    protected UISprite Spt_BgSprite;
    private List<GameObject> starlist;

    private Soldier _soldierAtt;
    public Soldier soldierAtt
    {
        get
        {
            return _soldierAtt;
        }
    }

    public delegate void OnPressObj(BaseComponent comp, bool status);
    public OnPressObj onPressObj;

    /// <summary>
    /// 是否显示统御力
    /// </summary>
    public bool IsShowLeader
    {
        set
        {
            if (Gobj_LeadershipGroup.gameObject.activeSelf != value)
                Gobj_LeadershipGroup.gameObject.SetActive(value);
        }
    }
    /// <summary>
    /// 是否显示召唤能量
    /// </summary>
    public bool IsShowEnergy
    {
        set
        {
            if (Gobj_EnergyGroup.gameObject.activeSelf != value)
                Gobj_EnergyGroup.gameObject.SetActive(value);
        }
    }

    private bool _isSelect = false;
    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool IsSelect
    {
        set
        {
            _isSelect = value;
            Spt_SelectSprite.enabled = value;
            if (_isSelect)
            {
                CommonFunction.SetSpriteName(Spt_BgSprite, GlobalConst.SpriteName.OwnSoldierSelected);
            }
            else
            {
                CommonFunction.SetSpriteName(Spt_BgSprite, GlobalConst.SpriteName.OwnSoldierDefault);
            }
        }
        get
        {
            return _isSelect;
        }
    }

    private bool _isEnable = false;
    /// <summary>
    /// 是否可用
    /// </summary>
    public bool IsEnable
    {
        set
        {
            _isEnable = value;
            if (IsSelect)
            {
                IsMask = false;
            }
            else
            {
                IsMask = !IsEnable;
            }
        }
        get
        {
            return _isEnable;
        }
    }
    /// <summary>
    /// 是否添加遮罩
    /// </summary>
    public bool IsMask
    {
        set
        {
            Spt_MaskSprite.enabled = value;
        }
    }

    public int soldierCount;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_MaskSprite = mRootObject.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Grd_StarsGrid = mRootObject.transform.FindChild("StarsGrid").gameObject.GetComponent<UIGrid>();
        Spt_StarSprite = mRootObject.transform.FindChild("StarsGrid/StarSprite").gameObject.GetComponent<UISprite>();
        Gobj_LeadershipGroup = mRootObject.transform.FindChild("LeadershipGroup").gameObject.transform;
        Gobj_EnergyGroup = mRootObject.transform.FindChild("EnergyGroup").gameObject.transform;
        Spt_IconTexture = mRootObject.transform.FindChild("LeadershipGroup/IconSprite").gameObject.GetComponent<UISprite>();
        Lbl_LeadershipLabel = mRootObject.transform.FindChild("LeadershipGroup/LeadershipLabel").gameObject.GetComponent<UILabel>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabel = mRootObject.transform.FindChild("LevelBG/LevelLabel").gameObject.GetComponent<UILabel>();
        Lbl_EnergyLabel = mRootObject.transform.FindChild("EnergyGroup/EnergyLabel").gameObject.GetComponent<UILabel>();
        Lbl_NumLabel = mRootObject.transform.FindChild("NumLabel").gameObject.GetComponent<UILabel>();
        Spt_BgSprite = mRootObject.transform.FindChild("BGSprite").gameObject.GetComponent<UISprite>();
        lbl_Label_Step = mRootObject.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        IsShowEnergy = false;
        IsShowLeader = false;
        Spt_StarSprite.gameObject.SetActive(false);
        starlist = new List<GameObject>();
        //Clear();
    }

    public void AddPressEventListener(OnPressObj callBack)
    {
        UIEventListener.Get(mRootObject).onPress = PressEvent;
        onPressObj = callBack;
    }

    private void PressEvent(GameObject go, bool status)
    {
        if (onPressObj != null)
        {
            onPressObj(this, status);
        }
    }

    public void UpdateInfo(Soldier att)
    {
        _soldierAtt = att;
        if (_soldierAtt == null)
        {
            Debug.LogError("solider data is null!!!");
            Clear();
            return;
        }
        base.UpdateInfo(_soldierAtt.Att.Icon, _soldierAtt.Att.quality);
        UpdateStars(_soldierAtt.Att.Star);
        Lbl_LeadershipLabel.text = _soldierAtt.Att.leaderShip.ToString();
        Lbl_EnergyLabel.text = _soldierAtt.Att.call_energy.ToString();
        Lbl_LevelLabel.text = _soldierAtt.Lv.ToString();
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(this.lbl_Label_Step,att.StepNum);
        UpdateNum(0);
        IsSelect = false;
    }

    public void UpdateNum(int num)
    {
        soldierCount = num;
        if (soldierCount == 0 || soldierCount == 1)
        {
            Lbl_NumLabel.text = string.Empty;
        }
        else
        {
            Lbl_NumLabel.text = "x" + soldierCount.ToString();
        }
    }

    public void UpdateNum(int count, int total)
    {
        soldierCount = count;
        System.Text.StringBuilder sub = new System.Text.StringBuilder();
        sub.Append(count);
        sub.Append("/");
        sub.Append(total);
        Lbl_NumLabel.text = sub.ToString();
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

    public override void Clear()
    {
        base.Clear();
        IsShowEnergy = false;
        IsShowLeader = false;
        IsEnable = true;
        Lbl_LeadershipLabel.text = string.Empty;
        Lbl_EnergyLabel.text = string.Empty;
        IsSelect = false;
        //UpdateStars(0);

    }
}

public class ExpeEnemyLineupComponent : LineupItemComponent
{
    private UISprite Spt_Mask;
    private UISprite Spt_DeadMark;
    public bool IsDead
    {
        set
        {
            Spt_Mask.enabled = value;
            Spt_DeadMark.enabled = value;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_Mask = mRootObject.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Spt_DeadMark = mRootObject.transform.FindChild("DeadMark").gameObject.GetComponent<UISprite>();
    }
}