using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TileInfo : MonoBehaviour {

    private UISprite Spt_TileSprite;
    private UISprite Spt_TypeTag;
    private UILabel Lbl_UnionName;
    private UISprite Spt_UnionIcon;
    private UISprite Spt_MilitaryRank1;
    private UISprite Spt_MilitaryRank2;
    private UISprite Spt_MilitaryRank3;
    private UISprite Spt_MilitaryRank4;

    private const string tile_unoccuppied = "KFZ_kongbaidikuai";
    private const string tile_occuppied_owns = "KFZ_lansedikuai";
    private const string tile_occuppied_theris = "KFZ_hongsedikuai";
    private const string type_arsenal = "KFZ_jianzhu1";
    private const string type_storage = "KFZ_jianzhu2";
    private const string type_camp = "KFZ_jianzhu3";

    private uint tile_id;
    public uint Tile_ID { get { return tile_id; } }
    private uint goods_num;
    private uint output;
    private string union_name;
    public uint owner_type;
    public uint border_type;
    private int type;
    private uint turns;
    public string tile_name;

    /// <summary>
    /// 初始化地块
    /// </summary>
    public void Initialize(uint id,uint _num,uint _output,int _type,string name)
    {
        Spt_TileSprite = transform.FindChild("TileSprite").GetComponent<UISprite>();
        Lbl_UnionName = transform.FindChild("UnionName").GetComponent<UILabel>();
        Spt_TypeTag = transform.FindChild("TypeTag").GetComponent<UISprite>();
        Spt_UnionIcon = transform.FindChild("UnionIcon").GetComponent<UISprite>();
        Spt_MilitaryRank1 = transform.FindChild("MilitaryRank1").GetComponent<UISprite>();
        Spt_MilitaryRank2 = transform.FindChild("MilitaryRank2").GetComponent<UISprite>();
        Spt_MilitaryRank3 = transform.FindChild("MilitaryRank3").GetComponent<UISprite>();
        Spt_MilitaryRank4 = transform.FindChild("MilitaryRank4").GetComponent<UISprite>();

        tile_id = id;
        tile_name = name;
        goods_num = _num;
        output = _output;
        type = _type;

        UIEventListener.Get(Spt_TileSprite.gameObject).onClick = ButtonEvent_ShowItemInfo;

        switch (_type)
        {
            case -1:
                Debug.LogError("error tile: this tile doesn't need show");
                Spt_TypeTag.gameObject.SetActive(false);
                break;
            case 0:
                Spt_TypeTag.gameObject.SetActive(false);
                break;
            case 1:
                Spt_TypeTag.gameObject.SetActive(true);
                CommonFunction.SetSpriteName(Spt_TypeTag, type_camp);
                break;
            case 2:
                Spt_TypeTag.gameObject.SetActive(true);
                CommonFunction.SetSpriteName(Spt_TypeTag, type_arsenal);
                break;
            case 3:
                Spt_TypeTag.gameObject.SetActive(true);
                CommonFunction.SetSpriteName(Spt_TypeTag, type_storage);
                break;
        }

    }
    /// <summary>
    /// 设置地块为未占领状态
    /// </summary>
    public void SetTile()
    {
        Lbl_UnionName.text = "";
        owner_type = 0;
        border_type = 0;
        turns = 0;

        CommonFunction.SetSpriteName(Spt_TileSprite, tile_unoccuppied);
        Lbl_UnionName.gameObject.SetActive(false);

        Spt_UnionIcon.gameObject.SetActive(false);
        Spt_MilitaryRank1.gameObject.SetActive(false);
        Spt_MilitaryRank2.gameObject.SetActive(false);
        Spt_MilitaryRank3.gameObject.SetActive(false);
        Spt_MilitaryRank4.gameObject.SetActive(false);

        CrossServerWarModule.Instance.fight_tile_num++;

        //TestSet();
    }
    /// <summary>
    /// 设置地块显示信息，并进行军团界面相关统计
    /// </summary>
    public void SetTile(uint owner,uint isBorder,string name,uint _turns,uint _point,uint _icon)
    {
        union_name = name;
        border_type = isBorder;
        turns = _turns;

        if (owner == UnionModule.Instance.MyUnionID)//己方占领地块
        {
            //己方占领地块类型
            owner_type = 1;
            //地块显示设置
            Lbl_UnionName.gameObject.SetActive(false);
            CommonFunction.SetSpriteName(Spt_TileSprite, tile_occuppied_owns);
            if (type > 0)
            {
                Spt_UnionIcon.gameObject.SetActive(false);
            }
            else
            {
                Spt_UnionIcon.gameObject.SetActive(true);
                string iconName = ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_icon).mIcon + "_s";
                CommonFunction.SetSpriteName(Spt_UnionIcon, iconName);
            }
            JudgeMilitaryRank(_point);
            //军团攻占信息计数
            if (isBorder == 1)
            {
                CrossServerWarModule.Instance.fight_tile_num++;
            }
            if (output == 0)
            {
                CrossServerWarModule.Instance.normal_tile_num++;
            }
            else
            {
                CrossServerWarModule.Instance.special_tile_num++;
            }
            CrossServerWarModule.Instance.normal_output += (int)goods_num + 
                (int)((Math.Max(1, turns) - 1) * ConfigManager.Instance.mCrossServerWarConfig.GetCrossServerWarSettingData().retain_gain);
        }
        else
        {
            if (string.IsNullOrEmpty(name))//信息错误，当做空地块处理
            {
                Debug.LogError("One tile's owner's name is empty");
                SetTile();
                //CommonFunction.SetSpriteName(Spt_TileSprite, tile_unoccuppied);
                //Lbl_UnionName.gameObject.SetActive(false);

                //Spt_UnionIcon.gameObject.SetActive(false);
                //Spt_MilitaryRank1.gameObject.SetActive(false);
                //Spt_MilitaryRank2.gameObject.SetActive(false);
                //Spt_MilitaryRank3.gameObject.SetActive(false);
                //Spt_MilitaryRank4.gameObject.SetActive(false);

                return;
            }
            else//敌方占领地块
            {
                //敌方占领地块类型
                owner_type = 2;
                //地块显示设置
                Lbl_UnionName.gameObject.SetActive(true);

                CommonFunction.SetSpriteName(Spt_TileSprite, tile_occuppied_theris);
                if (type > 0)
                {
                    Spt_UnionIcon.gameObject.SetActive(false);
                }
                else
                {
                    Spt_UnionIcon.gameObject.SetActive(true);
                    string iconName = ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_icon).mIcon + "_s";
                    CommonFunction.SetSpriteName(Spt_UnionIcon, iconName);
                }
                JudgeMilitaryRank(_point);
                SetTileName(false);
                //军团攻占信息计数
                if (isBorder == 1)
                {
                    CrossServerWarModule.Instance.fight_tile_num++;
                }
            }
        }
    }
    //点击地块，调用显示地块信息函数
    public void ButtonEvent_ShowItemInfo(GameObject btn)
    {
        ShowTileInfo();

    }
    //显示地块信息函数
    public void ShowTileInfo()
    {
        CrossServerWarModule.Instance.OnSendTileRankReq(tile_id);
        UISystem.Instance.CrossServerWarView.ShowTileInfo(owner_type, Tile_ID, goods_num, output, union_name, border_type, turns, tile_name);
    }
    /// <summary>
    /// 根据积分判定军衔
    /// </summary>
    /// <param name="point"></param>
    private void JudgeMilitaryRank(uint point)
    {
        uint j = ConfigManager.Instance.mCrossServerWarConfig.GetMilitaryRank(point);
        switch (j)
        {
            case 0:
                Spt_MilitaryRank1.gameObject.SetActive(false);
                Spt_MilitaryRank2.gameObject.SetActive(false);
                Spt_MilitaryRank3.gameObject.SetActive(false);
                Spt_MilitaryRank4.gameObject.SetActive(false);
                break;
            case 1:
                Spt_MilitaryRank1.gameObject.SetActive(true);
                Spt_MilitaryRank2.gameObject.SetActive(false);
                Spt_MilitaryRank3.gameObject.SetActive(false);
                Spt_MilitaryRank4.gameObject.SetActive(false);
                break;
            case 2:
                Spt_MilitaryRank1.gameObject.SetActive(true);
                Spt_MilitaryRank2.gameObject.SetActive(true);
                Spt_MilitaryRank3.gameObject.SetActive(false);
                Spt_MilitaryRank4.gameObject.SetActive(false);
                break;
            case 3:
                Spt_MilitaryRank1.gameObject.SetActive(true);
                Spt_MilitaryRank2.gameObject.SetActive(true);
                Spt_MilitaryRank3.gameObject.SetActive(true);
                Spt_MilitaryRank4.gameObject.SetActive(false);
                break;
            case 4:
                Spt_MilitaryRank1.gameObject.SetActive(false);
                Spt_MilitaryRank2.gameObject.SetActive(false);
                Spt_MilitaryRank3.gameObject.SetActive(false);
                Spt_MilitaryRank4.gameObject.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// 设置地块名在地图上的显示方式
    /// </summary>
    /// <param name="isWhole"></param>
    public void SetTileName(bool isWhole = false)
    {
        if (owner_type == 2)
        {
            if (isWhole)
            {
                Lbl_UnionName.fontSize = 12;
                Lbl_UnionName.text = union_name;
            }
            else
            {
                Lbl_UnionName.fontSize = 18;
                Lbl_UnionName.text = union_name.Substring(0, 2) + "...";
            }
        }
    }
    //=========================test=================================//
    
    public void TestSet()
    {
        if (type > 0)
        {
            int k = UnityEngine.Random.Range(0, 3);
            if (k != 2)
            {
                Spt_TypeTag.gameObject.SetActive(false);
                type = 0;
            }      
        }
        string _name;
        int i = UnityEngine.Random.Range(0, 10);
        uint _owner=0;
        List<UnionIconData> icons = ConfigManager.Instance.mUnionConfig.GetUnionIconList();
        uint _icon=0;
        if (i < 0)
        {
            _name = "";
        }
        else if (i > 60)
        {
            _name = "这是己方军团@1";
            _owner = (uint)UnionModule.Instance.MyUnionID;
            _icon = icons[0].mID;
        }
        else
        {
            _name = "军团没有名字@999";
            int k = UnityEngine.Random.Range(0, icons.Count);
            _icon = icons[k].mID;
        }
        uint _point = (uint)UnityEngine.Random.Range(1, 6000);

        SetTile(_owner, 0, _name, 0, _point, _icon);
    }
    
}

