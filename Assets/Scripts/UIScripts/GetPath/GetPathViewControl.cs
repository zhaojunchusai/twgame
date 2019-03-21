using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetPathViewControl : UIBase
{
    public class EquipInfoData
    {
        public uint id;
        public string name;
        public string icon;
        public bool islock;
        public bool isOpen = true;
    }

    public GetPathView view;
    public List<GetPathComponent> _getPathItems;

    private ItemInfo chipItemInfo;
    private EquipAttributeInfo equipItemInfo;
    private bool isOpening = false;
    private enum GetItemType
    {
        Material = 1,
        Equip = 2,
    }
    private GetItemType getType;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new GetPathView();
            view.Initialize();
        }
        isOpening = false;
        if (_getPathItems == null)
        {
            _getPathItems = new List<GetPathComponent>();
        }
        UISystem.Instance.ViewCloseEvent += UpdateControllerInfo;
        BtnEventBinding();
        view.Gobj_GetPathItem.SetActive(false); //PlayOpenAnim();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        //UISystem.Instance.ShowGameUI(TopFuncView.UIName);
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_GETPATH);
    }

    /// <summary>
    /// 初始化数据   
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type">1:碎片 2：小兵装备</param>
    public void UpdateViewInfo(uint id, int type)
    {
        getType = (GetItemType)type;
        switch (getType)
        {
            case GetItemType.Material:
                chipItemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(id);
                if (chipItemInfo == null)
                    return;
                break;
            case GetItemType.Equip:
                equipItemInfo = ConfigManager.Instance.mEquipData.FindById(id);
                if (equipItemInfo == null)
                    return;
                break;
        }
        UpdateControllerInfo(string.Empty);
    }

    private void UpdateControllerInfo(string uiname)
    {
        InstanceItems();
    }

    private void InstanceItems()
    {
        view.ScrollView_GetPathScrollView.disableDragIfFits = true;
        switch (getType)
        {
            case GetItemType.Equip:
                {
                    if (equipItemInfo.isHadWay)
                    {
                        if (equipItemInfo.getways != null && equipItemInfo.getways.Count > 0)
                        {
                            view.Grd_Grid.gameObject.SetActive(true);
                            Main.Instance.StartCoroutine(UpdateGateWayItems(equipItemInfo.getways));
                        }
                        else if (equipItemInfo.viewWays != null && equipItemInfo.viewWays.Count > 0)
                        {
                            List<EquipInfoData> list = SortViewWays(equipItemInfo.viewWays);

                            bool openStatus = false;
                            for (int i = 0; i < list.Count; i++)
                            {
                                EquipInfoData info = list[i];
                                if (info.isOpen)
                                {
                                    openStatus = true;
                                    break;
                                }
                            }
                            if (openStatus)   //有可能仅有一种获取途径但是没有到开放时间
                            {
                                view.Grd_Grid.gameObject.SetActive(true);
                                Main.Instance.StartCoroutine(UpdateViewWayItems(list));
                            }
                            else
                            {
                                view.Lab_GetPathDesc.gameObject.SetActive(true);//显示提示文字
                                view.Grd_Grid.gameObject.SetActive(false);
                                if (string.IsNullOrEmpty(equipItemInfo.desc_way))
                                {
                                    view.Lab_GetPathDesc.text = ConstString.GETPATH_NOGETPATH;
                                }
                                else
                                {
                                    view.Lab_GetPathDesc.text = equipItemInfo.desc_way;
                                }
                            }
                        }
                        else
                        {
                            view.Lab_GetPathDesc.gameObject.SetActive(true);//显示提示文字
                            view.Grd_Grid.gameObject.SetActive(false);
                            if (string.IsNullOrEmpty(equipItemInfo.desc_way))
                            {
                                view.Lab_GetPathDesc.text = ConstString.GETPATH_NOGETPATH;
                            }
                            else
                            {
                                view.Lab_GetPathDesc.text = equipItemInfo.desc_way;
                            }
                        }
                    }
                    else
                    {
                        view.Lab_GetPathDesc.gameObject.SetActive(true);//显示提示文字
                        view.Grd_Grid.gameObject.SetActive(false);
                        if (string.IsNullOrEmpty(equipItemInfo.desc_way))
                        {
                            view.Lab_GetPathDesc.text = ConstString.GETPATH_NOGETPATH;
                        }
                        else
                        {
                            view.Lab_GetPathDesc.text = equipItemInfo.desc_way;
                        }
                    }
                }
                break;
            case GetItemType.Material:
                {
                    if (chipItemInfo.ishad_way)
                    {
                        if (chipItemInfo.getways != null && chipItemInfo.getways.Count > 0)
                        {
                            view.Grd_Grid.gameObject.SetActive(true);
                            Main.Instance.StartCoroutine(UpdateGateWayItems(chipItemInfo.getways));
                        }
                        else if (chipItemInfo.view_ways != null && chipItemInfo.view_ways.Count > 0)
                        {
                            List<EquipInfoData> list = SortViewWays(chipItemInfo.view_ways);
                            bool openStatus = false;
                            for (int i = 0; i < list.Count; i++)
                            {
                                EquipInfoData info = list[i];
                                if (info.isOpen)
                                {
                                    openStatus = true;
                                    break;
                                }
                            }
                            if (openStatus)   //有可能仅有一种获取途径但是没有到开放时间
                            {
                                view.Grd_Grid.gameObject.SetActive(true);
                                Main.Instance.StartCoroutine(UpdateViewWayItems(list));
                            }
                            else
                            {
                                view.Lab_GetPathDesc.gameObject.SetActive(true);//显示提示文字
                                view.Grd_Grid.gameObject.SetActive(false);
                                if (string.IsNullOrEmpty(chipItemInfo.desc_way))
                                {
                                    view.Lab_GetPathDesc.text = ConstString.GETPATH_NOGETPATH;
                                }
                                else
                                {
                                    view.Lab_GetPathDesc.text = chipItemInfo.desc_way;
                                }
                            }
                        }
                        else
                        {
                            view.Lab_GetPathDesc.gameObject.SetActive(true);//显示提示文字
                            view.Grd_Grid.gameObject.SetActive(false);
                            if (string.IsNullOrEmpty(chipItemInfo.desc_way))
                            {
                                view.Lab_GetPathDesc.text = ConstString.GETPATH_NOGETPATH;
                            }
                            else
                            {
                                view.Lab_GetPathDesc.text = chipItemInfo.desc_way;
                            }
                        }
                    }
                    else
                    {
                        view.Lab_GetPathDesc.gameObject.SetActive(true);//显示提示文字
                        view.Grd_Grid.gameObject.SetActive(false);
                        if (string.IsNullOrEmpty(chipItemInfo.desc_way))
                        {
                            view.Lab_GetPathDesc.text = ConstString.GETPATH_NOGETPATH;
                        }
                        else
                        {
                            view.Lab_GetPathDesc.text = chipItemInfo.desc_way;
                        }
                    }
                }
                break;
        }
    }

    private IEnumerator UpdateGateWayItems(List<uint> gainWays)
    {
        if (gainWays == null)
            gainWays = new List<uint>();
        isOpening = true;
        List<StageData> stageDataList = PlayerData.Instance.GetAvailableGates();
        if (stageDataList == null)
            stageDataList = new List<StageData>();
        List<StageData> dataList = new List<StageData>();
        List<StageInfo> infoList = ConfigManager.Instance.mStageData.GetStageListByIDs(gainWays);
        view.Grd_Grid.gameObject.SetActive(true);
        if (infoList == null)
            infoList = new List<StageInfo>();
        for (int i = 0; i < infoList.Count; i++)
        {
            StageInfo info = infoList[i];
            if (info == null) continue;
            StageData stagedata = stageDataList.Find((tmp) =>
            {
                if (tmp == null) return false;
                return tmp.gateinfo.dgn_id == info.ID;
            });
            if (stagedata == null)
            {
                stagedata = new StageData();
                stagedata.gateinfo = null;
                stagedata.stageinfo = info;
                stagedata.remainRaidTimes = info.ChallengeCount;
            }
            dataList.Add(stagedata);
        }
        yield return null;
        if (!isOpening) yield break;
        view.Lab_GetPathDesc.gameObject.SetActive(false);
        if (_getPathItems == null)
            _getPathItems = new List<GetPathComponent>();
        int count = _getPathItems.Count;
        if (dataList.Count < count)
        {
            for (int i = dataList.Count; i < count; i++)
            {
                GetPathComponent comp = _getPathItems[i];
                if (!isOpening) yield break;
                if (comp == null) continue;
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            GetPathComponent comp = null;
            StageData stagedata = dataList[i];
            if (stagedata == null) continue;
            if (i < count)
            {
                comp = _getPathItems[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_GetPathItem, view.Grd_Grid.transform);//实例化Item
                go.name = "item_" + i.ToString();
                comp = new GetPathComponent();
                comp.MyStart(go);
                comp.AddEventListener(ButtonEvent_GetPathItem);
                _getPathItems.Add(comp);
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateItem(stagedata);
        }
        isOpening = false;
        yield return null;
        view.Grd_Grid.repositionNow = true;
        yield return null;
        view.ScrollView_GetPathScrollView.ResetPosition();
    }


    private IEnumerator UpdateViewWayItems(List<EquipInfoData> gainWays)
    {
        if (gainWays == null)
            gainWays = new List<EquipInfoData>();
        isOpening = true;
        view.Grd_Grid.gameObject.SetActive(true);
        view.Lab_GetPathDesc.gameObject.SetActive(false);
        if (_getPathItems == null)
            _getPathItems = new List<GetPathComponent>();
        int count = _getPathItems.Count;
        if (gainWays.Count < count)
        {
            for (int i = gainWays.Count; i < count; i++)
            {
                GetPathComponent comp = _getPathItems[i];
                if (!isOpening) yield break;
                if (comp == null) continue;
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < gainWays.Count; i++)
        {
            GetPathComponent comp = null;
            EquipInfoData viewWay = gainWays[i];
            if (i < count)
            {
                comp = _getPathItems[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_GetPathItem, view.Grd_Grid.transform);//实例化Item
                go.name = "item_" + i.ToString();
                comp = new GetPathComponent();
                comp.MyStart(go);
                comp.AddEventListener(ButtonEvent_GetPathItem);
                _getPathItems.Add(comp);
            }
            if (comp == null) continue;
            if (viewWay.isOpen == false)
            {
                comp.mRootObject.SetActive(false);
            }
            else
            {
                comp.mRootObject.SetActive(true);
            }
            string content = string.Empty;
            //bool islock = !CheckGetFunc(viewWay, out content);
            comp.UpdateItem(viewWay.id, viewWay.name, viewWay.icon, viewWay.islock);
        }
        isOpening = false;
        yield return null;
        view.Grd_Grid.repositionNow = true;
        yield return null;
        view.ScrollView_GetPathScrollView.ResetPosition();
    }

    private List<EquipInfoData> SortViewWays(List<uint> list)
    {
        List<EquipInfoData> ways = new List<EquipInfoData>();
        if (list == null || list.Count <= 0)
            return ways;
        List<uint> lockways = new List<uint>();
        List<uint> unlockways = new List<uint>();
        for (int i = 0; i < list.Count; i++)
        {
            uint way = list[i];
            string content = string.Empty;
            string hint = string.Empty;
            OpenLevelData opendata = null;
            bool islock = !CommonFunction.CheckFuncIsOpen(way, out content, out hint, out opendata);
            EquipInfoData data = new EquipInfoData();
            string lockColor = "[D80C0C]";
            string unlockColor = "[FFDE98]";
            if (opendata != null)
            {
                data.id = way;
                data.icon = opendata.func_icon;
                data.islock = islock;
                data.isOpen = !MainCityModule.Instance.LockFuncs.Contains((uint)opendata.functionType);
                if (islock)
                {
                    data.name = lockColor + content + "[-]";
                }
                else
                {
                    data.name = unlockColor + opendata.name + "[-]";
                }
                ways.Add(data);
            }
        }
        return ways;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_ButtonClose.gameObject).onClick = ButtonEvebr_ButtonClose;
    }


    /// <summary>
    /// 点击事件-跳转关卡
    /// </summary>
    /// <param name="Btn"></param>
    public void ButtonEvent_GetPathItem(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        GetPathComponent comp = baseComp as GetPathComponent;
        if (comp == null) return;
        if (comp.IsGate)
        {
            if (comp.IsGateLock) return;
            if (comp.StageData.stageinfo.UnlockLV > PlayerData.Instance._Level)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_LINEUPPANEL_LEVELLIMIT, comp.StageData.stageinfo.UnlockLV));
            }
            else
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
                UISystem.Instance.GateInfoView.UpdateViewInfo(comp.StageData.stageinfo, comp.StageData.gateinfo, comp.StageData.remainRaidTimes, EFightType.eftMain);
            }
        }
        else
        {
            if (comp.IsViewLock) return;
            CommonFunction.OpenTargetView((ETaskOpenView)comp.ViewType);
        }

        //UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_GETPATH);
    }


    public void ButtonEvebr_ButtonClose(GameObject btn)
    {
        //_getPathItems.Clear();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_GETPATH);
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
        isOpening = false;
        Main.Instance.StopCoroutine(UpdateViewWayItems(null));
        Main.Instance.StopCoroutine(UpdateGateWayItems(null));
        UISystem.Instance.ViewCloseEvent -= UpdateControllerInfo;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _getPathItems.Clear();
    }

    //界面动画

    //public void PlayOpenAnim()
    //{
    //    _view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    _view.Anim_TScale.Restart();
    //    _view.Anim_TScale.PlayForward();
    //}
}
