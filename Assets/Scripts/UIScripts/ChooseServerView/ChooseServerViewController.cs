using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class ChooseServerViewController : UIBase
{
    private enum DefaultShowServerType
    {
        Recommend,
        New
    }

    public ChooseServerView view;
    private List<ServerItem> _serverItems = new List<ServerItem>();
    private bool _openOnce = false;
    private DefaultShowServerType showType = DefaultShowServerType.Recommend;
    private List<ServerDetail> _tmpShowList = new List<ServerDetail>();
    private Transform _selectedServer;
    private Transform _selectedType;
    public override void Initialize()
    {
        if (view == null)
            view = new ChooseServerView();
        view.Initialize();
        BtnEventBinding();
        view.Tex_MaskTex.mainTexture = ResourceLoadManager.Instance.LoadResources(GlobalConst.SpriteName.LoginBG) as Texture;
        InitServerItems();
        SetLastLogin();
        ShowServerTypeList();
        _openOnce = true;
        //PlayOpenAnim();
    }

    private void InitServerItems()
    {
        if (_openOnce)
            return;
        view.GetSelectedMarkOnce();
        _selectedServer = view.Spt_ServerItemSelectedMark.transform;
        _selectedType = view.Spt_ServerTypeSelectedMark.transform;
        _serverItems.Clear();
        _serverItems.Add(AddServerItemCom(view.ServerItem0));
        _serverItems.Add(AddServerItemCom(view.ServerItem1));
        _serverItems.Add(AddServerItemCom(view.ServerItem2));
        _serverItems.Add(AddServerItemCom(view.ServerItem3));
        _serverItems.Add(AddServerItemCom(view.ServerItem4));
        _serverItems.Add(AddServerItemCom(view.ServerItem5));
        _serverItems.Add(AddServerItemCom(view.ServerItem6));
        _serverItems.Add(AddServerItemCom(view.ServerItem7));
        _serverItems.Add(AddServerItemCom(view.ServerItem8));
        _serverItems.Add(AddServerItemCom(view.ServerItem9));
    }

    private ServerItem AddServerItemCom(GameObject go)
    {
        if (go.GetComponent<ServerItem>() == null)
        {
            go.AddComponent<ServerItem>();
        }
        return go.GetComponent<ServerItem>();
    }

    private void SetLastLogin()
    {
        if (LoginModule.Instance.CurServer == null)
        {
            view.Lbl_BtnLastLoginSvrName.text = "";
            view.Lbl_BtnLastLoginSvrStatus.text = "";
            view.Spt_BtnLastLoginSvrBG.gameObject.SetActive(false);
            return;
        }
        view.Lbl_BtnLastLoginSvrName.text = LoginModule.Instance.CurServer.desc;
        view.Lbl_BtnLastLoginSvrStatus.text =
            LoginModule.Instance.GetServerStateString(LoginModule.Instance.CurServer.status);
        view.Spt_BtnLastLoginSvrBG.gameObject.SetActive(true);
    }

    private void ShowServerTypeList()
    {
        if (LoginModule.Instance.ServerList == null || LoginModule.Instance.ServerList.Count < 1)
        {
            UISystem.Instance.CloseGameUI(ChooseServerView.UIName);
            return;
        }

        if (_openOnce)
            return;
        int count = LoginModule.Instance.ServerList.Count / 10;
        if (LoginModule.Instance.ServerList.Count > count * 10)
            InstantiateServerType(count, count * 10, LoginModule.Instance.ServerList.Count - 1);
        for (int i = count - 1; i >= 0; i--)
        {
            InstantiateServerType(i, i * 10, i * 10 + 9);
        }
        if (showType == DefaultShowServerType.New)
            ShowServerList(count);
        else
        {
            ButtonEvent_RecomendServer(null);
        }
        view.Gobj_ServerTypeItem.SetActive(false);
    }
    private void InstantiateServerType(int id, int min, int max)
    {
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_ServerTypeItem, view.Grd_ServerTypeList.transform);
        go.name = string.Format("item{0:D3}", id);
        go.GetComponentInChildren<UILabel>().text = string.Format(ConstString.FORMAT_SERVER_TYPE, min + 1, max + 1);
        UIEventListener.Get(go).onClick = ServerTypeClick;
    }
    public void SetSelectedServerItemMark(Transform transform)
    {
        _selectedServer.parent = transform;
        _selectedServer.localPosition = Vector3.zero;
        _selectedServer.gameObject.SetActive(true);
    }
    public void SetSelectedTypeMark(Transform transform)
    {
        _selectedType.parent = transform;
        _selectedType.localPosition = Vector3.zero;
        _selectedType.gameObject.SetActive(true);
        view.Gobj_SelectedRecommend.SetActive(false);
    }

    private void ShowRecommendServerList()
    {
        _tmpShowList.Clear();
        List<ServerDetail> listall = LoginModule.Instance.ServerList;
        List<int> listrecommend = LoginModule.Instance.RecommendSvrIDs;
        List<ServerDetail> detail = listall.FindAll(r => listrecommend.Contains(r.area_id));
        _tmpShowList.AddRange(detail);
        ShowServerList(_tmpShowList);
    }

    private void ShowServerList(List<ServerDetail> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (i >= _serverItems.Count)
                break;

            if (!_serverItems[i].gameObject.activeSelf)
                _serverItems[i].gameObject.SetActive(true);
            _serverItems[i].InitItem(list[i]);
        }

        for (int i = list.Count; i < _serverItems.Count; i++)
        {
            if (_serverItems[i].gameObject.activeSelf)
                _serverItems[i].gameObject.SetActive(false);
        }
    }
    private void ShowServerList(int pageid)
    {
        _tmpShowList.Clear();
        int min = Mathf.Min(pageid * 10 + 9, LoginModule.Instance.ServerList.Count - 1);
        for (int i = min; i >= pageid * 10; i--)
        {
            _tmpShowList.Add(LoginModule.Instance.ServerList[i]);
        }
        ShowServerList(_tmpShowList);
    }

    private void ServerTypeClick(GameObject go)
    {
        int id = int.Parse(go.name.Substring(4, 3));
        ShowServerList(id);
        SetSelectedTypeMark(go.transform);
        view.Lbl_ServerListTitle.text = go.GetComponentInChildren<UILabel>().text;
    }

    public void ButtonEvent_LastLoginSvr(GameObject btn)
    {
        CloseUI();
    }

    public void ButtonEvent_RecomendServer(GameObject btn)
    {
        ShowRecommendServerList();
        _selectedType.gameObject.SetActive(false);
        view.Gobj_SelectedRecommend.SetActive(true);
        view.Lbl_ServerListTitle.text = view.Lbl_BtnRecomendServerName.text;
    }

    public void CloseUI()
    {
        UISystem.Instance.LoginView.UpdateServerInfo();
        UISystem.Instance.DelGameUI(ChooseServerView.UIName);
    }

    public override void Uninitialize()
    {

    }
    public override void Destroy()
    {
        view = null;
        _serverItems.Clear();
        _openOnce = false;
        showType = DefaultShowServerType.Recommend;
        _tmpShowList.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_LastLoginSvr.gameObject).onClick = ButtonEvent_LastLoginSvr;
        UIEventListener.Get(view.Btn_RecomendServer.gameObject).onClick = ButtonEvent_RecomendServer;
    }


    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();
    //}

}
