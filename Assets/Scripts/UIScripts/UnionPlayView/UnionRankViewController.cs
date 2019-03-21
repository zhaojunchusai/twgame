using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
/******************
 * 军团排行榜 有三个排行榜 异域探险伤害排行榜 军团争霸积分榜 军团击杀排行榜
 * 异域探险伤害排行榜：实时刷新，在军团内进行排名，没有打过异域探险，排行榜就不会有信息
 * 军团争霸积分榜：全区军团进行排名，在战斗阶段不可以查看，其他阶段可以，军团没有参加争霸战 就不会有信息
 * 军团击杀排行榜：军团内进行排名，没有报名参加争霸战就不会有信息
 * 
 */
public class UnionRankViewController : UIBase
{
    public const int MAXRANK = 10000;
    int MAXCOUNT = 5;
    const string ITEMNAME = "UnionRankItem_{0}";
    public UnionRankView view;
    private UnionRankType _type;
    private List<UnionRankItem> _itemList;
    private int _maxDamage = 1;

    private List<UnionPveMaxDamage> _unionPVEDamageList;//军团内：异域探险
    private List<UnionPvpKillRank> _unionKillList;//军团内：击杀
    private List<UnionPvpRankInfo> _unionRankList;//全区：争霸积分
    private UnionPveMaxDamage _selfPVEDamageInfo;
    private UnionPvpKillRank _selfKillInfo;
    private UnionPvpRankInfo _selfRankInfo;

    private Vector3 Item_Pos_H = new Vector3(10,76.5f,0);
   // private Vector3 ScrollView_Pos_H = new Vector3(10,-87.8f,0);
    private Vector3 ScrollView_Pos_H = new Vector3(0, 0, 0);
    private int ScrollSizeY_H = 220;

    private Vector3 Item_Pos = new Vector3(10,108,0);
    //private Vector3 ScrollView_Pos = new Vector3(10,-70,0);
    private Vector3 ScrollView_Pos = new Vector3(0, 17, 0);
    private int ScrollSizeY = 250;
    private bool isH = false;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new UnionRankView();
            view.Initialize();
            BtnEventBinding();
            _unionPVEDamageList = new List<UnionPveMaxDamage>();
            _unionKillList = new List<UnionPvpKillRank>();
            _unionRankList = new List<UnionPvpRankInfo>();
            _itemList = new List<UnionRankItem>();
        }
        this.view.Obj_SeasonInfo.SetActive(false);
        ChangeToHegemony(false);
        view.UIWrapContent_UIGrid.onInitializeItem = null;
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_ClosePanel;
    }

    public void ButtonEvent_ClosePanel(GameObject go)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_UNIONRANKVIEW);
        view.UIWrapContent_UIGrid.onInitializeItem = null;
        _maxDamage = 1;
        _unionPVEDamageList.Clear();
        _unionKillList.Clear();
        _unionRankList.Clear();
    }

    public void OnShowUnionRank(UnionRankType type)
    {
        _type = type;
        switch (_type)
        {
            case UnionRankType.Damage:
                view.Lbl_TitleLb.text = ConstString.UNIONRANK_TITLE_DAMAGE;
                break;
            case UnionRankType.Hegemony:
                view.Lbl_TitleLb.text = ConstString.UNIONRANK_TITLE_HEGEMONY;
                break;
            case UnionRankType.Kill:
                view.Lbl_TitleLb.text = ConstString.UNIONRANK_TITLE_KILL;
                break;
        }

    }

    public override void Destroy()
    {
        base.Destroy();
        DestroyItemList();
        _itemList.Clear();
        _maxDamage = 1;
        view = null;
    }
    /// <summary>
    /// 异域探险 伤害排行榜 军团内排行榜 一定要有自己的数据
    /// </summary>
    public void OnUpdateDamageRank(List<UnionPveMaxDamage> list)
    {
        DeleteEmptyPlayerInList(ref list, _type);
        _unionPVEDamageList.Clear();
        _unionPVEDamageList.AddRange(list);
        if (list == null || list.Count < 1)
        {
            view.Lbl_EmptyLabel.text = ConstString.UNIONRANK_LABEL_DAMAGEEMPTY;
            view.UnionRankItem_SelfItem.gameObject.SetActive(false);
            _maxDamage =1;
        }
        else
        {
            view.Lbl_EmptyLabel.text = "";
            view.UnionRankItem_SelfItem.gameObject.SetActive(true);
            _maxDamage = list[0].damage;
        }
        view.UIWrapContent_UIGrid.onInitializeItem = null;
        view.UIWrapContent_UIGrid.onInitializeItem = WrapEvent_UpdateDamageItem;

        int count = list.Count;
        int myRank = MAXRANK;
        UnionPveMaxDamage myInfo = null;
        for (int i = 0; i < count; i++)
        {
            UnionPveMaxDamage temp = list[i];
            if (temp.charid == PlayerData.Instance._RoleID)
            {
                myInfo = temp;
                myRank = i + 1;
                break;
            }
        }
        if (myInfo == null)
        {
            TryUpdateSelfInfo(_type);
        }
        else
        {
            view.UnionRankItem_SelfItem.UpdateItem(myInfo, _type, myRank,_maxDamage);
        }
        Main.Instance.StartCoroutine(UpdateRankContentInspector(list));
    }
    /// <summary>
    /// 军团争霸 击杀排行 军团内排行榜 一定要有自己的数据
    /// </summary>
    /// <param name="resp"></param>
    public void OnUpdateKillRank(List<UnionPvpKillRank> list)
    {
        DeleteEmptyPlayerInList(ref list, _type);
        _unionKillList.Clear();
        _unionKillList.AddRange(list);
        if (list == null || list.Count < 1)
        {
            view.Lbl_EmptyLabel.text = ConstString.UNIONRANK_LABEL_KILLEMPTY;
            view.UnionRankItem_SelfItem.gameObject.SetActive(false);
        }
        else
        {
            view.Lbl_EmptyLabel.text = "";
            view.UnionRankItem_SelfItem.gameObject.SetActive(true);
        }
        view.UIWrapContent_UIGrid.onInitializeItem = null;
        view.UIWrapContent_UIGrid.onInitializeItem = WrapEvent_UpdateKillItem;

        int count = list.Count;
        int myRank = MAXRANK;
        UnionPvpKillRank myInfo = null;
        for (int i = 0; i < count; i++)
        {
            UnionPvpKillRank temp = list[i];
            if (temp.charid == PlayerData.Instance._RoleID)
            {
                myInfo = temp;
                myRank = i + 1;
                break;
            }
        }
        if (myInfo == null)
        {
            TryUpdateSelfInfo(_type);
        }
        else
        {
            view.UnionRankItem_SelfItem.UpdateItem(myInfo, _type, myRank,_maxDamage);
        }
        Main.Instance.StartCoroutine(UpdateRankContentInspector(list));
    }
    /// <summary>
    /// 军团争霸 争霸排行 不一定有本军团的数据
    /// </summary>
    public void OnUpdateHegemonyRank(UnionPvpRankResp resp,List<UnionPvpRankInfo> list)
    {
        DeleteEmptyPlayerInList(ref list, _type);
        _unionRankList.Clear();
        _unionRankList.AddRange(list);
        if (resp != null && resp.season > 0)
        {
            ChangeToHegemony(true);
            this.view.Obj_SeasonInfo.SetActive(true);
            this.view.SeasonDes.text = string.Format(ConstString.UNIONSEASONINDEX,resp.season,resp.game_index);
        }
        if (list == null || list.Count < 1)
        {
            view.Lbl_EmptyLabel.text = ConstString.UNIONRANK_LABEL_HEGEMONYEMPTY;
            view.UnionRankItem_SelfItem.gameObject.SetActive(false);
        }
        else
        {
            view.Lbl_EmptyLabel.text = "";
            view.UnionRankItem_SelfItem.gameObject.SetActive(true);
        }
        view.UIWrapContent_UIGrid.onInitializeItem = WrapEvent_UpdateUnionRankItem;
        int count = list.Count;
        int myRank = MAXRANK;
        UnionPvpRankInfo myUnionInfo = null;
        for (int i = 0; i < count; i++)
        {
            UnionPvpRankInfo temp = list[i];
            if (temp.union.id == UnionModule.Instance.UnionInfo.base_info.id)
            {
                myUnionInfo = temp;
                myRank = i + 1;
                break;
            }
        }
        if (myUnionInfo == null)
        {
            //TODO: 对于未上榜的军团的数据 还没有测试过
            TryUpdateSelfInfo(_type);
        }
        else
        {
            view.UnionRankItem_SelfItem.UpdateItem(myUnionInfo, _type, myRank, _maxDamage);
        }
        //Debug.Log(list.Count);
        //foreach (UnionPvpRankInfo info in list)
        //{
        //    Debug.Log("ID = " + info.union.id + " name = "+ info.union.name + " win = " + info.wins);
        //}
        Main.Instance.StartCoroutine(UpdateRankContentInspector(list));
    }

    public void UpdateTitleLabel(string str)
    {
        view.Lbl_TitleLb.text = str;
    }

    private IEnumerator UpdateRankContentInspector<T>(List<T> list)
    {
        //if (list == null|| list.Count < 1)
        //    yield break;
        view.ScrView_ContentScrollView.ResetPosition();
        yield return null;
        int infoCount = list.Count;
        int itemCount = _itemList.Count;
        int index = Mathf.CeilToInt((float)infoCount / view.UIWrapContent_UIGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_UIGrid.minIndex = -index;
        view.UIWrapContent_UIGrid.maxIndex = 0;
        if (infoCount > MAXCOUNT)
        {
            view.UIWrapContent_UIGrid.enabled = true;
            infoCount = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_UIGrid.enabled = false;
        }
        if (itemCount > infoCount)
        {
            for (int i = infoCount; i < itemCount; i++)
            {
                _itemList[i].gameObject.SetActive(false);
            }
        }
        UnionRankItem item = null;
        for (int i = 0; i < infoCount; i++)
        {
            if (itemCount <= i)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Obj_SelfRankItem, view.Grd_UIGrid.transform);
                item = go.GetComponent<UnionRankItem>();
                item.name = string.Format(ITEMNAME, i);
                item.IsSelf = false;
                //UIEventListener.Get(go).onClick = ButtonEvent_RankItemClick;
                _itemList.Add(item);
            }
            else
            {
                item = _itemList[i];
                item.gameObject.SetActive(true);
            }
            item.UpdateItem(list[i], _type, i + 1, _maxDamage);
        }
        if (infoCount > 1)
            view.UIWrapContent_UIGrid.ReGetChild();
        yield return null;
        view.Grd_UIGrid.repositionNow = true;
        yield return null;
        view.ScrView_ContentScrollView.ResetPosition();
        view.Grd_UIGrid.repositionNow = true;
    }

    private void ButtonEvent_RankItemClick(GameObject go)
    {

    }

    private void DestroyItemList()
    {
        if (_itemList == null || _itemList.Count < 1)
        {
            return;
        }
        int count = _itemList.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            UnionRankItem item = _itemList[i];
            _itemList.Remove(item);
            GameObject.Destroy(item.gameObject);
        }
        _itemList.Clear();
    }

    private void WrapEvent_UpdateDamageItem(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _unionPVEDamageList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        UnionRankItem item = _itemList[wrapIndex];
        item.UpdateItem(_unionPVEDamageList[realIndex], _type, realIndex+1, _maxDamage);
        
    }

    private void WrapEvent_UpdateKillItem(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _unionKillList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        UnionRankItem item = _itemList[wrapIndex];
        item.UpdateItem(_unionKillList[realIndex], _type, realIndex + 1, _maxDamage);
    }

    private void WrapEvent_UpdateUnionRankItem(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _unionRankList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        UnionRankItem item = _itemList[wrapIndex];
        item.UpdateItem(_unionRankList[realIndex], _type, realIndex + 1, _maxDamage);
    }

    private void TryUpdateSelfInfo(UnionRankType type)
    {
        UnionRankItem self = view.UnionRankItem_SelfItem;
        self.Clear();
        switch (type)
        {
            case UnionRankType.Damage:
                UnionPveMaxDamage myDamageInfo = new UnionPveMaxDamage();
                myDamageInfo.charid = PlayerData.Instance._RoleID;
                myDamageInfo.damage = 0;
                self.UpdateItem(myDamageInfo, type, MAXRANK, _maxDamage);
                break;
            case UnionRankType.Hegemony:
                UnionPvpRankInfo myUnionInfo = new UnionPvpRankInfo();
                myUnionInfo.union = UnionModule.Instance.UnionInfo.base_info;
                myUnionInfo.wins = UnionModule.Instance.UnionInfo.wins;//
                myUnionInfo.score = UnionModule.Instance.UnionInfo.season_score;
                self.UpdateItem(myUnionInfo, type, MAXRANK, _maxDamage);
                break;
            case UnionRankType.Kill:
                UnionPvpKillRank myKillInfo = new UnionPvpKillRank();
                myKillInfo.charid = (int)PlayerData.Instance._RoleID;
                myKillInfo.combat_power = 0;
                myKillInfo.kill_num = 0;
                myKillInfo.score = 0;
                self.UpdateItem(myKillInfo, type, MAXRANK, _maxDamage);
                break;
        }
    }

    private void DeleteEmptyPlayerInList<T>(ref List<T> list, UnionRankType type)
    {
        if (list == null || list.Count < 1)
            return;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            UnionMember ue = null;
            switch (type)
            {
                case UnionRankType.Damage:
                    UnionPveMaxDamage info = list[i] as UnionPveMaxDamage;
                    ue = UnionModule.Instance.GetUnionMember(info.charid);
                    break;
                case UnionRankType.Kill:
                    UnionPvpKillRank info2 = list[i] as UnionPvpKillRank;
                    ue = UnionModule.Instance.GetUnionMember((uint)info2.charid);
                    break;
                case UnionRankType.Hegemony:
                    continue;
            }
            if (ue == null)
                list.RemoveAt(i);
        }
    }
    private void ChangeToHegemony(bool isChange)
    {
        if(isChange)
        {
            this.view.Obj_SelfItem.transform.localPosition = this.Item_Pos_H;
            this.view.UIPanel_ContentScrollView.baseClipRegion = new Vector4(this.view.UIPanel_ContentScrollView.baseClipRegion.x, this.view.UIPanel_ContentScrollView.baseClipRegion.y, this.view.UIPanel_ContentScrollView.baseClipRegion.z, (float)this.ScrollSizeY_H);
            this.view.Obj_Scroll.transform.localPosition = this.ScrollView_Pos_H;
            MAXCOUNT = 3;
        }
        else
        {
            this.view.Obj_SelfItem.transform.localPosition = this.Item_Pos;
            this.view.UIPanel_ContentScrollView.baseClipRegion = new Vector4(this.view.UIPanel_ContentScrollView.baseClipRegion.x, this.view.UIPanel_ContentScrollView.baseClipRegion.y, this.view.UIPanel_ContentScrollView.baseClipRegion.z, this.ScrollSizeY);
            this.view.Obj_Scroll.transform.localPosition = this.ScrollView_Pos;
            MAXCOUNT = 4;
        }
    }
}