using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class JoinUnionViewController : UIBase
{
    public JoinUnionView view;

    public int Page;
    private List<UnionItem> _unionItems = new List<UnionItem>();
    private Dictionary<int, UnionItem> _unionItemDic = new Dictionary<int, UnionItem>();
    private const int COUNT_PER_PAGE = 10;
    public override void Initialize()
    {
        if (view == null)
            view = new JoinUnionView();
        view.Initialize();
        BtnEventBinding();
        view.ScrView_Items.onDragFinishedDown = ReqNewPage;
        SetCharJoinInfo();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenJoinUnionView);
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _unionItems.Clear();
        _unionItemDic.Clear();
    }
    
    public void SetCharJoinInfo()
    {
        view.Lbl_TimesCount.text = string.Format(ConstString.FORMAT_TODAY_UNION_APPLY_COUNT,
                                                 UnionModule.Instance.CharUnionInfo.apply_times,5);
    }

    public void RefreashItem(int id)
    {
        if(_unionItemDic.ContainsKey(id))
            _unionItemDic[id].Refreash();
        else
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("RefreashItem Can't find union id={0} dic count={1} ids=(", id, _unionItemDic.Count));
            List<int> list = new List<int>(_unionItemDic.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(string.Format(" {0}", list[i]));
            }
            sb.Append(")");
            Debug.LogError(sb.ToString());
        }
    }

    public void ShowUnionList()
    {
        if(Page <= 1)
            _unionItemDic.Clear();
        ShowUnionList(UnionModule.Instance.UnionList, Page);
    }

    public void ShowSearchUnion(BaseUnion union)
    {
        view.ScrView_Items.ResetPosition();
        for (int i = 1; i < _unionItems.Count; i++)
        {
            _unionItems[i].gameObject.SetActive(false);
        }

        if(_unionItems.Count >0)
        {
            _unionItems[0].gameObject.SetActive(true);
            _unionItems[0].Init(union);
            //TODO：
            Main.Instance.StartCoroutine(MaskEffect(0.75F));
            PlayEffect(_unionItems[0].transform);

            _unionItemDic.Clear();
            _unionItemDic.Add(union.id, _unionItems[0]);
        }

    }

    private void ShowUnionList(List<BaseUnion> list,int page)
    {
        int startIndex = COUNT_PER_PAGE*(page - 1);
        int endIndex = Mathf.Min(list.Count, COUNT_PER_PAGE * page);

        for (int i = startIndex; i < endIndex; i++)
        {
            if (_unionItemDic.ContainsKey(list[i].id)) 
            {
                Debug.LogError("ShowUnionList Error Name = " + list[i].name + " Key =  " + list[i].id);
                continue;
            }
            if (i >= _unionItems.Count)
            {
                UnionItem unionItem = InstantiateUnionItem();
                _unionItems.Add(unionItem);
            }
            if(!_unionItems[i].gameObject.activeSelf)
                _unionItems[i].gameObject.SetActive(true);
            _unionItems[i].gameObject.name = string.Format("item{0:D3}", i);
            _unionItems[i].Init(list[i]);
            _unionItemDic.Add(list[i].id,_unionItems[i]);
            if (i < 3)
            {
                if(i==0)
                {
                    Main.Instance.StartCoroutine(MaskEffect(0.75F));
                }
                PlayEffect(_unionItems[i].transform);
            }
        }

        for (int i = endIndex; i < _unionItems.Count; i++)
        {
            _unionItems[i].gameObject.SetActive(false);
        }

        view.Grd_Items.Reposition();
    }

    private UnionItem InstantiateUnionItem()
    {
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_UnionItem, view.Grd_Items.transform);
        if (go.GetComponent<UnionItem>() == null)
            return go.AddComponent<UnionItem>();
        else
            return go.GetComponent<UnionItem>();
    }

    private void ReqNewPage()
    {
        Page = Page + 1;
        if (COUNT_PER_PAGE * Page > UnionModule.Instance.UnionList.Count)
        {
            UnionModule.Instance.OnSendUnionPage(Page);
        }
        else
        {
            ShowUnionList(UnionModule.Instance.UnionList, Page);
        }
    }

    private void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(JoinUnionView.UIName);
    }

    private void ButtonEvent_CreateUnion(GameObject btn)
    {
        uint timer = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().LeaveUnionCD;
        if(Main.mTime < UnionModule.Instance.CharUnionInfo.exit_time + timer)
        {
            
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                string.Format(ConstString.ERR_EXIT_UNION_TIMER, timer/3600));
            return;
        }

        UISystem.Instance.ShowGameUI(CreateUnionView.UIName);
    }

    private void ButtonEvent_SearchUnion(GameObject btn)
    {
        if(string.IsNullOrEmpty(view.Ipt_UnionID.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_UNION_ID_EMPTY);
            return;
        }
        int id;
        if (int.TryParse(view.Ipt_UnionID.value, out id))
        {
            UnionModule.Instance.OnSendSearchUnion(id);
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_UNION_ID_NOT_NUM);            
        }
    }

    private void ButtonEvent_CancelSearch(GameObject btn)
    {
        Debug.Log(" ButtonEvent_CancelSearch " );
        Page = 1;
        _unionItemDic.Clear();
        ShowUnionList(UnionModule.Instance.UnionList, Page);
    }

    public override void Uninitialize()
    {
        if (view !=null&&view.ScrView_Items != null)
            view.ScrView_Items.onDragFinishedDown = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_CreateUnion.gameObject).onClick = ButtonEvent_CreateUnion;
        UIEventListener.Get(view.Btn_SearchUnion.gameObject).onClick = ButtonEvent_SearchUnion;
        UIEventListener.Get(view.Btn_CancelSearch.gameObject).onClick = ButtonEvent_CancelSearch;
    }

    //===================================================
    GameObject Go_Effect;
    public void PlayEffect(Transform form)
    {
        view.ScrView_Items.ResetPosition();
        if (Go_Effect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_JOINUNION, (GameObject gb) => { Go_Effect = gb; });
        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_Effect, form);
        go.transform.localPosition = new Vector3(0,3,0);
    }
    public IEnumerator MaskEffect(float time)
    {
        view.Mask.depth = 30;
        view.EffectMask.SetActive(true);
        yield return new WaitForSeconds(time);
        view.EffectMask.SetActive(false);
    }

}
