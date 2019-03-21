using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
/****
 * 争霸战备界面 在争霸战备阶段才能够进入
 * 进入之后更新城市名字，自己参战城市，本军团队友的出战阵容
 * 自己若未参战可以报名 选择上阵阵容和神器
 * 自己在本城市参战可以取消报名
 * 自己在其他城市参战，只能观看队友阵容，不可以操作
 * */
public class UnionReadinessViewController : UIBase
{
    const int MAXCOUNT = 5;
    const string ITEMNAME = "ReadinessItem_{0}";
    public UnionReadinessView view;
    private bool _isJionBattle = false;//是否参战
    private bool _isInThisCity = false;//是否在本城市参战
    private List<ArenaPlayer> _playerList;
    private List<UnionReadinessItem> _itemList;
    private int _city;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new UnionReadinessView();
            view.Initialize();
            BtnEventBinding();
            _playerList = new List<ArenaPlayer>();
            _itemList = new List<UnionReadinessItem>();
        }
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        Clear();
        _city = 0;
        view = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_BG.gameObject).onClick = ButtonEvent_ClosePanel;
    }

    public void OnUpdateView(int city, List<ArenaPlayer> list)
    {
        Clear();
        _city = city;
        //Debug.Log("OnUpdateView UnionModule.Instance.CharUnionInfo.pvp_city = " + UnionModule.Instance.CharUnionInfo.pvp_city + ";_city " + _city + ";list = " + list.Count);
        view.Lbl_TitleLb.text = ConstString.GUILDCITYNAME[city-1];
        _playerList.Clear();
        _playerList.AddRange(list);
        _isInThisCity = UnionModule.Instance.CharUnionInfo.pvp_city == _city;
        _isJionBattle = UnionModule.Instance.CharUnionInfo.pvp_city != 0;
        if (_isJionBattle && !_isInThisCity)//玩家如果在其他城市参战 需要显示玩家参战的城市名字
        {
            string yourcity = ConstString.GUILDCITYNAME[UnionModule.Instance.CharUnionInfo.pvp_city - 1];
            view.Lbl_CurrentCityLabel.text = string.Format(ConstString.UNIONREADINESS_YOURCITY, yourcity);
        }
        //Debug.Log("OnUpdateView _isInThisCity " + _isInThisCity + "_isJionBattle " + _isJionBattle);
        if (!_isJionBattle)
        {
            UnionReadinessItem selfItem = CreateItem(0);
            selfItem.Clear();
            selfItem.IsSelf = true;
            selfItem.UpdateButtonLabel(false);
            _itemList.Add(selfItem);
            UIEventListener.Get(selfItem.Btn_EventButton.gameObject).onClick = ButtonEvent_Battle;
        }
        else
        {
            //玩家在本城市参战 玩家信息在第一个
            if (_isInThisCity)
            {
                ReSortList(ref list);
            }
        }
        view.Lbl_TeamNumLabel.text = string.Format(ConstString.UNIONREADINESS_TAMENUM, list.Count);
        Main.Instance.StartCoroutine(UpdateReadinessContent(list));
    }

    private IEnumerator UpdateReadinessContent(List<ArenaPlayer> list)
    {
        if (list == null || list.Count < 1)
        {
            yield return null;
            view.Grd_UIGrid.Reposition();
            view.ScrView_ReadinessSrollView.ResetPosition();
            yield break;
        }
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            ArenaPlayer data = list[i];
            UnionReadinessItem item = CreateItem(i + 1);
            item.IsSelf = false;
            if (i == 0 && _isInThisCity)
            {
                UIEventListener.Get(item.Btn_EventButton.gameObject).onClick = ButtonEvent_Battle;
                item.IsSelf = true;
                item.UpdateButtonLabel(true);
            }
            item.UpdateItem(data);
            _itemList.Add(item);
            yield return null;
            view.Grd_UIGrid.Reposition();
        }
        view.Grd_UIGrid.Reposition();
        yield return null;
        view.ScrView_ReadinessSrollView.ResetPosition();
    }

    private void ButtonEvent_ClosePanel(GameObject go)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_UNIONREADINESSVIEW);
        Clear();
        _city = 0;
        UnionModule.Instance.OnSendOpenUnionPVP();
    }

    private void ButtonEvent_Battle(GameObject go)
    {
       
        if (_isJionBattle)
        {
            UnionModule.Instance.OnSendCancelUnionPVP();
        }
        else
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
            UISystem.Instance.PrepareBattleView.UpdateViewInfo(EFightType.eftHegemony, _city);
            //ButtonEvent_ShowPanel(go);
        }
    }

    private void UpdateRankItemInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _playerList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        UnionReadinessItem item = _itemList[wrapIndex];
        item.UpdateItem(_playerList[realIndex]);
        if (_isInThisCity && realIndex == 0)
        {
            item.IsSelf = true;
            item.UpdateButtonLabel(true);
        }
        else
        {
            item.IsSelf = false;
        }
    }

    private void Clear()
    {
        view.Lbl_TeamNumLabel.text = string.Format(ConstString.UNIONREADINESS_TAMENUM, 0);
        view.Lbl_CurrentCityLabel.text = "";
        DestroyItemList();
        _playerList.Clear();
        _isJionBattle = false;
        _isInThisCity = false;
        view.Grd_UIGrid.Reposition();
        view.ScrView_ReadinessSrollView.ResetPosition();
    }

    private UnionReadinessItem CreateItem(int index)
    {
        GameObject go = CommonFunction.InstantiateObject(view.Obj_ReadinessItem.gameObject, view.Grd_UIGrid.transform);
        UnionReadinessItem item = go.AddComponent<UnionReadinessItem>();
        item.name = string.Format(ITEMNAME, index);
        return item;
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
            UnionReadinessItem item = _itemList[i];
            _itemList.RemoveAt(i);
            GameObject.Destroy(item.gameObject);
        }
        _itemList.Clear();
    }

    private void ReSortList(ref List<ArenaPlayer> list)
    {
        if (list == null || list.Count < 1)
            return;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            ArenaPlayer player = list[i];
            if (player.hero.charid == PlayerData.Instance._RoleID)
            {
                list.RemoveAt(i);
                list.Insert(0, player);
                break;
            }
        }
    }
}
