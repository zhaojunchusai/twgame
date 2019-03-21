using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetPathComponent : BaseComponent
{
    private GameObject Gobj_GateWay;
    private UISprite Spt_GetPathItemBG;
    private UILabel Lbl_ChapterName;
    private UILabel Lbl_StageName;
    private UILabel Lbl_Num;
    private UISprite Spt_Star1;
    private UISprite Spt_Star2;
    private UISprite Spt_Star3;
    private UISprite Spt_GateLock;
    private UISprite Spt_Line1;

    private GameObject Gobj_EnableGroup;
    private GameObject Gobj_UnenableGroup;
    private UIGrid Grid_Star;
    private UISprite Spt_EliteMark;
    private List<UISprite> starList = new List<UISprite>();

    private GameObject Gobj_ViewWay;
    private UISprite Spt_Line3;
    private UILabel Lbl_ViewDesc;
    private UISprite Spt_ViewIcon;
    private UISprite Spt_ViewLock;
    private UISprite Spt_RightSub;
    private UISprite Spt_LeftSub;

    private uint viewType = 0;
    public uint ViewType
    {
        get
        {
            return viewType;
        }
    }

    private StageData stagedata;
    public StageData StageData
    {
        get
        {
            return stagedata;
        }
    }

    private bool isGateLock = false;

    public bool IsGateLock
    {
        get
        {
            return isGateLock;
        }
        set
        {
            isGateLock = value;
            Gobj_EnableGroup.gameObject.SetActive(!isGateLock);
            Gobj_UnenableGroup.gameObject.SetActive(isGateLock);
            CommonFunction.UpdateWidgetGray(Spt_Line1, isGateLock);
            CommonFunction.UpdateWidgetGray(Spt_GetPathItemBG, isGateLock);
            if (isGateLock)
            {
                Lbl_ChapterName.color = Color.white;
                Lbl_StageName.color = Color.white;
            }
            else
            {
                Lbl_ChapterName.color = CommonFunction.ToColor(255, 222, 122);
                Lbl_StageName.color = CommonFunction.ToColor(255, 222, 122);
            }
            CommonFunction.UpdateWidgetGray(Spt_EliteMark, isGateLock);
        }
    }

    private bool isViewLock = false;
    public bool IsViewLock
    {
        get
        {
            return isViewLock;
        }
    }

    private bool isGate;
    public bool IsGate
    {
        get
        {
            return isGate;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_GetPathItemBG = mRootObject.transform.FindChild("GetPathItemBG").gameObject.GetComponent<UISprite>();
        Gobj_GateWay = mRootObject.transform.FindChild("GateWay").gameObject;
        Spt_Star1 = mRootObject.transform.FindChild("GateWay/EnableGroup/Star/Star1").gameObject.GetComponent<UISprite>();
        Spt_Star2 = mRootObject.transform.FindChild("GateWay/EnableGroup/Star/Star2").gameObject.GetComponent<UISprite>();
        Spt_Star3 = mRootObject.transform.FindChild("GateWay/EnableGroup/Star/Star3").gameObject.GetComponent<UISprite>();
        Gobj_EnableGroup = mRootObject.transform.FindChild("GateWay/EnableGroup").gameObject;
        Gobj_UnenableGroup = mRootObject.transform.FindChild("GateWay/UnenableGroup").gameObject;
        Lbl_Num = mRootObject.transform.FindChild("GateWay/EnableGroup/Num").gameObject.GetComponent<UILabel>();
        Lbl_ChapterName = mRootObject.transform.FindChild("GateWay/ChapterName").gameObject.GetComponent<UILabel>();
        Lbl_StageName = mRootObject.transform.FindChild("GateWay/StageName").gameObject.GetComponent<UILabel>();
        Spt_GateLock = mRootObject.transform.FindChild("GateWay/UnenableGroup/Lock").gameObject.GetComponent<UISprite>();
        Spt_Line1 = mRootObject.transform.FindChild("GateWay/Line1").gameObject.GetComponent<UISprite>();
        Spt_EliteMark = mRootObject.transform.FindChild("GateWay/EliteMark").gameObject.GetComponent<UISprite>();
        Grid_Star = mRootObject.transform.FindChild("GateWay/EnableGroup/Star").gameObject.GetComponent<UIGrid>();

        Gobj_ViewWay = mRootObject.transform.FindChild("ViewWay").gameObject;
        Spt_Line3 = mRootObject.transform.FindChild("ViewWay/Line3").gameObject.GetComponent<UISprite>();
        Lbl_ViewDesc = mRootObject.transform.FindChild("ViewWay/ViewDesc").gameObject.GetComponent<UILabel>();
        Spt_ViewIcon = mRootObject.transform.FindChild("ViewWay/ViewIcon").gameObject.GetComponent<UISprite>();
        Spt_ViewLock = mRootObject.transform.FindChild("ViewWay/ViewLock").gameObject.GetComponent<UISprite>();
        Spt_RightSub = mRootObject.transform.FindChild("ViewWay/RightSub").gameObject.GetComponent<UISprite>();
        Spt_LeftSub = mRootObject.transform.FindChild("ViewWay/LeftSub").gameObject.GetComponent<UISprite>();
    }


    /// <summary>
    /// 更新数据
    /// </summary>
    public void UpdateItem(StageData data)
    {
        isGate = true;
        Gobj_GateWay.SetActive(true);
        Gobj_ViewWay.SetActive(false);
        stagedata = data;
        if (stagedata == null)
        {
            mRootObject.SetActive(false);
            return;
        }
        if (stagedata.gateinfo == null)
        {
            stagedata.gateinfo = new fogs.proto.msg.PassDungeon();
            stagedata.gateinfo.dgn_id = stagedata.stageinfo.ID;
            stagedata.remainRaidTimes = stagedata.stageinfo.ChallengeCount;
            stagedata.gateinfo.star_level = 0;//副本评星
            IsGateLock = true;
        }
        else
        {
            IsGateLock = false;
        }
        Grid_Star.gameObject.SetActive(true);
        UpdateStar();
        UpdateNum();
        UpdateGate();
    }

    public void UpdateItem(uint type, string desc, string icon, bool isLock)
    {
        viewType = type;
        Lbl_ViewDesc.text = desc;
        isViewLock = isLock;
        isGate = false;
        Gobj_GateWay.SetActive(false);
        Gobj_ViewWay.SetActive(true);
        Spt_ViewLock.enabled = IsViewLock;
        UpdateViewIcon(icon);
        CommonFunction.UpdateWidgetGray(Spt_Line3, isViewLock);
        CommonFunction.UpdateWidgetGray(Spt_GetPathItemBG, isViewLock);
        CommonFunction.UpdateWidgetGray(Spt_ViewIcon, isViewLock);
        CommonFunction.UpdateWidgetGray(Spt_RightSub, isViewLock);
        CommonFunction.UpdateWidgetGray(Spt_LeftSub, isViewLock);
    }

    private void UpdateViewIcon(string icon)
    {
        CommonFunction.SetSpriteName(Spt_ViewIcon, icon);
        //ETaskOpenView type = (ETaskOpenView)viewType;
        //switch (type)
        //{
        //    case ETaskOpenView.LastestNormalDungeon:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_NormalBattle);
        //        } break;
        //    case ETaskOpenView.LastestAdvancedDungeon:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_Elite);
        //        } break;
        //    case ETaskOpenView.LastestProtectDungeon:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_Escort);
        //        } break;
        //    case ETaskOpenView.Expedition:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_Expedition);
        //        } break;
        //    case ETaskOpenView.Endless:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_Endless);
        //        } break;
        //    case ETaskOpenView.ActivityDungeon:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_Activity);
        //        } break;
        //    case ETaskOpenView.VipPage:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_Vip);
        //        } break;
        //    case ETaskOpenView.Shop_Nor:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_NormalStore);
        //        } break;
        //    case ETaskOpenView.Shop_Medal:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_MedalStore);
        //        } break;
        //    case ETaskOpenView.Shop_Honor:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_HonorStore);
        //        } break;
        //    case ETaskOpenView.Shop_Union:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_UnionStore);
        //        } break;
        //    case ETaskOpenView.Mall:
        //        {
        //            CommonFunction.SetSpriteName(Spt_ViewIcon, GlobalConst.SpriteName.GetPath_ViewIcon_Mall);
        //        } break;
        //}
    }

    /// <summary>
    /// 更新星星
    /// </summary>
    /// <param name="_gainWays"></param>
    public void UpdateStar()
    {
        if (starList.Count <= 0)
        {
            starList.Add(Spt_Star1);
            starList.Add(Spt_Star2);
            starList.Add(Spt_Star3);
        }
        if (stagedata.gateinfo.star_level <= starList.Count)
        {
            int count = starList.Count;
            for (int i = 0; i < count; i++)
            {
                UISprite sprite = starList[i];
                if (i < stagedata.gateinfo.star_level)
                {
                    sprite.gameObject.SetActive(true);
                }
                else
                {
                    sprite.gameObject.SetActive(false);
                }
            }
        }
        Grid_Star.Reposition();
        for (int i = 0; i < starList.Count; i++)
        {
            UISprite sprite = starList[i];
            sprite.enabled = false;
            sprite.enabled = true;
        }
    }
    /// <summary>
    /// 更新剩余次数
    /// </summary>
    /// <param name="_gainWays"></param>
    public void UpdateNum()
    {
        if (stagedata.stageinfo.ChallengeCount <= 0)
        {
            Lbl_Num.text = ConstString.GETPATH_NOLIMIT;
        }
        else
        {
            Lbl_Num.text = string.Format("{0}/{1}", stagedata.remainRaidTimes, stagedata.stageinfo.ChallengeCount);
        }
    }
    /// <summary>
    /// 更新关卡
    /// </summary>
    public void UpdateGate()
    {
        Lbl_StageName.text = stagedata.stageinfo.Name;
        switch ((MainBattleType)stagedata.stageinfo.IsElite)
        {
            case MainBattleType.Crusade:
                {
                    Spt_EliteMark.enabled = false;
                } break;
            case MainBattleType.Escort:
                {
                    Spt_EliteMark.enabled = true;
                    CommonFunction.SetSpriteName(Spt_EliteMark, GlobalConst.SpriteName.GATE_ESCORT_MARK);
                } break;
            case MainBattleType.EliteCrusade:
                {
                    Spt_EliteMark.enabled = true;
                    CommonFunction.SetSpriteName(Spt_EliteMark, GlobalConst.SpriteName.GATE_ELITECRUSADE_MARK);
                }
                break;
        }
        ChapterInfo chapter = ConfigManager.Instance.mChaptersData.GetChapterByID(stagedata.stageinfo.ChapterID);
        if (chapter == null)
            Lbl_ChapterName.text = string.Empty;
        else
        {
            Lbl_ChapterName.text = chapter.name;
        }
    }

    public override void Clear()
    {
        base.Clear();
        starList.Clear();
    }
}
