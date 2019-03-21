using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*****
 * 领取结果界面很多系统都在用，所以提供了一系列接口
 * 其他系统传一个List<CommonItemData>，由于可能会出现多个重复的物品，所以这边再做统计一次数量，保证每个物品只生成一个
 * 目前所有的物品都做成一个item来显示
 * 
 * *******/
public class RecieveResultVertViewController : UIBase
{
    public RecieveResultVertView view;
    private List<MailAttachmentItem> _itemList;
    private List<CommonItemData> _infoList;
    private Vector3 _gridCenterPos = new Vector3(2, 90, 0);
    private Vector3 _gridSkillBookTopLeftPos = new Vector3(-126, 35, 0);
    private Vector3 _gridSkillBookCenterPos = new Vector3(0, 35, 0);
    private bool _isUpdating = false;
    private float _orginalHeight = 445f;
    private Vector3 _btnNormalPos = new Vector3(7, -205f, 0);
    private Vector3 _leftSpPos = new Vector3(-168, -204f, 0);
    private Vector3 _rightSpPos = new Vector3(182, -204f, 0);
    private float offsetY = 30f;

    public override void Initialize()
    {
        if (view == null) 
        {
            view = new RecieveResultVertView();
            view.Initialize();
            view.Grd_ItemGrid.pivot = UIWidget.Pivot.Top;
            view.Grd_ItemGrid.transform.localPosition = _gridCenterPos;
            BtnEventBinding();
        }
        DestroyAttachItems();//PlayOpenOffsetRootAnim();
        _itemList = new List<MailAttachmentItem>();
        _infoList = new List<CommonItemData>();

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Get_Reward, view._uiRoot.transform.parent.transform));

        view.Spt_BGSprite.height = (int)_orginalHeight;
        view.Btn_SureButton.transform.localPosition = _btnNormalPos;
        view.Spt_DLSprite.transform.localPosition = _leftSpPos;
        view.Spt_DRSprite.transform.localPosition = _rightSpPos;

        GuideManager.Instance.CheckTrigger(GuideTrigger.GetReward);
    }

    public void SetTitle(string title)
    {
        view.Lbl_TitleLabel.text = title;
    }

    public void ButtonEvent_TopCloseButton(GameObject btn)
    {
        ClearResultInfo();
        UISystem.Instance.CloseGameUI(RecieveResultVertView.UIName);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close , view._uiRoot.transform.parent.transform));
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseRewardView);

    }

    public void ButtonEvent_SureButton(GameObject btn)
    {
        ClearResultInfo();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(RecieveResultVertView.UIName);
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseRewardView);
    }
    /// <summary>
    /// 更新领取结果 物品都以item格子的形式显示出来 
    /// </summary>
    /// <param name="list">道具的列表 可以有重复的道具</param>
    public void UpdateRecieveInfo(List<CommonItemData> list)
    {
        ClearResultInfo();
        SetTitle(ConstString.RECEIVE_RESULT);
        view.Lbl_ReturnSkillBookLabel.gameObject.SetActive(false);
        if (list == null || list.Count < 1)
        {
            return;
        }
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            TryAddInfoIntoList(list[i]);
        }
        Main.Instance.StartCoroutine(InitItemInspector(_infoList));
        view.Btn_SureButton.isEnabled = true;
    }
    /// <summary>
    /// 更新领取结果 
    /// </summary>
    /// <param name="list">物品内容</param>
    /// <param name="title">需要显示的标题</param>
    /// <param name="isActiveSureBtn">是否启用确认按钮</param>
    public void UpdateRecieveInfo(List<CommonItemData> list, bool isActiveSureBtn, string title)
    {
        UpdateRecieveInfo(list);
        view.Btn_SureButton.isEnabled = isActiveSureBtn;
        SetTitle(title);
    }
    /// <summary>
    /// 更新领取结果 
    /// </summary>
    /// <param name="list">物品内容</param>
    /// <param name="title">需要显示的标题</param>
    public void UpdateRecieveInfo(List<CommonItemData> list, string title)
    {
        UpdateRecieveInfo(list);
        SetTitle(title);
    }
    /// <summary>
    /// 远征天下 预览
    /// </summary>
    /// <param name="list">内容</param>
    /// <param name="title">标题</param>
    public void UpdateExpedetionPreviewInfo(List<CommonItemData> list, string title)
    {
        ClearResultInfo();
        if (list == null || list.Count < 1)
        {
            return;
        }
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            CommonItemData data = list[i];
            IDType iType = CommonFunction.GetTypeOfID(data.ID.ToString());
            switch (iType)
            {
                case IDType.Gold:
                case IDType.Diamond:
                case IDType.Exp:
                case IDType.SP:
                case IDType.Honor:
                case IDType.Medal:
                case IDType.UnionToken:
                case IDType.RecycleCoin:
                case IDType.SoldierExp:
                    TryAddInfoIntoList(data);
                    break;
            }
        }
        SetTitle(title);
        //配置尽量确保在除了item，还要掉落其他东西比如 金币 经验 战功等，单独掉落一个item也可以
        Main.Instance.StartCoroutine(InitItemInspector(_infoList, true));
    }

    public void UpdateRecieveInfo(List<CommonItemData> list, string title, string desc)
    {
        UpdateRecieveInfo(list);
        SetTitle(title);
        view.Lbl_DescLabel.text = desc;
        view.Spt_BGSprite.height = (int)(_orginalHeight + offsetY);
        Vector3 offset = new Vector3(0, -offsetY, 0);
        view.Btn_SureButton.transform.localPosition = _btnNormalPos + offset;
        view.Spt_DLSprite.transform.localPosition = _leftSpPos + offset;
        view.Spt_DRSprite.transform.localPosition = _rightSpPos + offset;
    }
    /// <summary>
    /// 甄选返还部分消耗的技能书 天降神兵要 有可能需要修改ReturnSkillBookLabel的内容
    /// </summary>
    /// <param name="list"></param>
    public void UpdateSkillBookInfo(List<CommonItemData> list)
    {
        ClearResultInfo();
        view.Lbl_ReturnSkillBookLabel.gameObject.SetActive(true);
        if (list == null || list.Count < 1)
            return;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            TryAddInfoIntoList(list[i]);
        }
        UpdateItemGridPosBySkillBook(_infoList.Count);
        Main.Instance.StartCoroutine(InitItemInspector(_infoList));
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_TopCloseButton;
        UIEventListener.Get(view.Btn_SureButton.gameObject).onClick = ButtonEvent_SureButton;
    }

    private void UpdateItemGridPosBySkillBook(int itemCount)
    {
        if (itemCount < 3)
        {
            view.Grd_ItemGrid.pivot = UIWidget.Pivot.Top;
            view.Grd_ItemGrid.transform.localPosition = _gridSkillBookCenterPos;
        }
        else
        {
            view.Grd_ItemGrid.pivot = UIWidget.Pivot.TopLeft;
            view.Grd_ItemGrid.transform.localPosition = _gridSkillBookTopLeftPos;
        }
    }
    /// <summary>
    /// 生成item
    /// </summary>
    /// <param name="list"></param>
    /// <param name="addEmpty">是否添加一个空物体在后面</param>
    /// <returns></returns>
    private IEnumerator InitItemInspector(List<CommonItemData> list,bool addEmpty = false)
    {
        if (list == null || list.Count < 1)
        {
            if (addEmpty)
            {
                GameObject obj = CommonFunction.InstantiateObject(view.Obj_Source, view.Grd_ItemGrid.transform);
                obj.name = string.Format(ConstString.RECIEVE_ITEM_NAME, 0);
                MailAttachmentItem item = obj.AddComponent<MailAttachmentItem>();
                item.ClearItemInfo();
                item.UpdateItemName("???");
                _itemList.Add(item);
                view.Btn_SureButton.isEnabled = true;
                yield return null;
                view.Grd_ItemGrid.Reposition();
                yield return null;
                view.ScrView_ScorllView.ResetPosition();
            }
            yield break;
        }

        int count = list.Count;
        _isUpdating = true;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject obj = CommonFunction.InstantiateObject(view.Obj_Source, view.Grd_ItemGrid.transform);
            MailAttachmentItem item = obj.AddComponent<MailAttachmentItem>();
            UIEventListener.Get(obj).onPress = PressItem;
            CommonItemData info = list[i];
            item.UpdateItemInfo(info.ID, (int)info.Num);
            obj.name = string.Format(ConstString.RECIEVE_ITEM_NAME, i);
            _itemList.Add(item);
            yield return null;
            view.Grd_ItemGrid.Reposition();
        }
        if (addEmpty)
        {
            yield return null;
            if (!_isUpdating)
                yield break;
            GameObject obj = CommonFunction.InstantiateObject(view.Obj_Source, view.Grd_ItemGrid.transform);
            obj.name = string.Format(ConstString.RECIEVE_ITEM_NAME, 0);
            MailAttachmentItem item = obj.AddComponent<MailAttachmentItem>();
            item.ClearItemInfo();
            item.UpdateItemName("???");
            _itemList.Add(item);
            view.Btn_SureButton.isEnabled = true;
        }
        view.Grd_ItemGrid.Reposition();
        _isUpdating = false;
        yield return null;
        view.ScrView_ScorllView.ResetPosition();
    }

    private void ClearResultInfo()
    {
        _isUpdating = false;
        view.Lbl_TitleLabel.text = ConstString.RECEIVE_RESULT;
        view.Lbl_ReturnSkillBookLabel.gameObject.SetActive(false);
        view.Lbl_DescLabel.text = "";
        _infoList.Clear();
        DestroyAttachItems();
        _itemList.Clear();
        view.Grd_ItemGrid.Reposition();
        view.ScrView_ScorllView.ResetPosition();
        view.Spt_BGSprite.height = (int)_orginalHeight;
        view.Btn_SureButton.transform.localPosition = _btnNormalPos;
        view.Spt_DLSprite.transform.localPosition = _leftSpPos;
        view.Spt_DRSprite.transform.localPosition = _rightSpPos;
        ClearAttachs();
    }

    private void DestroyAttachItems()
    {
        if (_itemList == null || _itemList.Count < 1)
            return;
        for (int i = _itemList.Count - 1; i >= 0; i--)
        {
            MailAttachmentItem item = _itemList[i];
            _itemList.RemoveAt(i);
            if (item != null && item.gameObject!= null)
                GameObject.Destroy(item.gameObject);
        }
    }

    private void TryAddInfoIntoList(CommonItemData source)
    {
        int count = _infoList.Count;
        bool isFind = false;
        for (int i = 0; i < count; i++)
        {
            CommonItemData data = _infoList[i];
            if (data.ID == source.ID)
            {
                isFind = true;
                data.Num += source.Num;
                break;
            }
        }
        if (!isFind)
            _infoList.Add(source);
    }

    //界面动画
    //public  void PlayOpenOffsetRootAnim()
    //{
    //    view.OffsetRoot_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.OffsetRoot_TScale.Restart();
    //    view.OffsetRoot_TScale.PlayForward();
    //}

    private void ClearAttachs()
    {
        int childCount = view.Grd_ItemGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(view.Grd_ItemGrid.GetChild(i).gameObject);
            }
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        _itemList.Clear();
        _infoList.Clear();
        view = null;
    }

    private void PressItem(GameObject go, bool press)
    {
        uint id = go.GetComponent<MailAttachmentItem>().ID;
        HintManager.Instance.SeeDetail(go, press, id);
    }
}
