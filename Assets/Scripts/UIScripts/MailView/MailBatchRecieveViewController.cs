using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class MailBatchRecieveViewController : UIBase 
{
    public MailBatchRecieveView view;
  
    private List<MailAttachmentItem> _successItemList;
    private List<MailAttachmentItem> _failedItemList;
    private List<CommonItemData> _successInfoList;
    private List<CommonItemData> _failedInfoList;

    private float _splitHeight = 71f;
    private float _successItemHeight = 0f;
    private float _currencyHeight = 0f;
    private int _currencyNumCount = 0;

    private bool _isUpdating = false;

    private Vector3 _splitOriginalPos = new Vector3(300f, 61f, 0f);
    private Vector3 _itemOriginalPos = new Vector3(33f, 30f, 0f);
    private float _waitTime = 1f;//出现item的间隔时间
    private float _rollDelay = 0.1f;//滑动的等待时间

    public override void Initialize()
    {
        if (view == null)
        {
            view = new MailBatchRecieveView();
            view.Initialize();
            BtnEventBinding();
        }
        _waitTime = 1f;
        _isUpdating = false;
        ClearMailAttachmentItems();
        _successItemList = new List<MailAttachmentItem>();
        _failedItemList = new List<MailAttachmentItem>();
        _successInfoList = new List<CommonItemData>();
        _failedInfoList = new List<CommonItemData>();
        _successItemHeight = view.Grd_SuccessGrid.cellHeight;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Get_Reward, view._uiRoot.transform.parent.transform));
    }

    public void ButtonEvent_TopCloseButton(GameObject btn)
    {
        // 点击停止表现
        if (_isUpdating)
        {
            _waitTime = Time.deltaTime;
        }
        else
        {
            ClearResultInfo();
            UISystem.Instance.CloseGameUI(MailBatchRecieveView.UIName);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        }
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _isUpdating = false;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_MaskBG.gameObject).onClick = ButtonEvent_TopCloseButton;
        UIEventListener.Get(view.Btn_SureButton.gameObject).onClick = ButtonEvent_TopCloseButton;
    }
    /// <summary>
    /// 显示批量领取结果 服务端发送成功的邮件id和失败的邮件id 客户端取出这些邮件的附件 然后表现
    /// </summary>
    /// <param name="successList"></param>
    /// <param name="failedList"></param>
    public void UpdateBatchRecieveInfo(List<MailInfo> successList, List<MailInfo> failedList)
    {

        view.GameObj_SplitObj.SetActive(false);
        if (successList != null && successList.Count > 0)
        {
            int count1 = successList.Count;
            for (int i = 0; i < count1; i++)
            {
                MailInfo info = successList[i];
                int leng1 = info.attachments.Count;
                for (int j = 0; j < leng1; j++)
                {
                    Attachment attach = info.attachments[j];
                    TryAddInfoIntoList(attach.id, (int)attach.num, _successInfoList);
                }
            }
        }

        if (failedList != null && failedList.Count > 0)
        {
            int count2 = failedList.Count;
            for (int i = 0; i < count2; i++)
            {
                MailInfo info = failedList[i];
                int leng1 = info.attachments.Count;
                for (int j = 0; j < leng1; j++)
                {
                    Attachment attach = info.attachments[j];
                    TryAddInfoIntoList(attach.id, (int)attach.num, _failedInfoList);
                }
            }
        }
        Main.Instance.StartCoroutine(InitItemsInspector(_successInfoList, _failedInfoList));
    }

    private IEnumerator InitItemsInspector(List<CommonItemData> successList, List<CommonItemData> failedList)
    {
        yield return new WaitForSeconds(_rollDelay);
        int lineCount = 0;
        _isUpdating = true;
        view.ScrView_AttachmentsScrollView.ResetPosition();
        if (successList != null && successList.Count > 0)
        {
            int count = successList.Count;
            for (int i = 0; i < count; i++)
            {
                if (!_isUpdating)
                    yield break;
                MailAttachmentItem item = CreateItems(view.Obj_Source, successList[i], view.Grd_SuccessGrid, i);
                _successItemList.Add(item);
                view.Grd_SuccessGrid.repositionNow = true;
                if (i == 0)
                {
                    view.ScrView_AttachmentsScrollView.ResetPosition();
                }
                int curLine = Mathf.CeilToInt(_successItemList.Count / 6f);
                if (curLine != lineCount)
                {
                    lineCount = curLine;
                    if (curLine >= 3)
                    {
                        yield return new WaitForSeconds(_rollDelay);
                        RollSrollView(lineCount);
                    }
                }
                float curTime = 0f;
                while (curTime < _waitTime)
                {
                    curTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
        if (failedList == null || failedList.Count <= 0)
        {
            view.GameObj_SplitObj.gameObject.SetActive(false);
            view.Grd_FailedGrid.gameObject.SetActive(false);
        }
        else
        {
            view.GameObj_SplitObj.SetActive(true);
            view.Grd_FailedGrid.gameObject.SetActive(true);
            UpdateSplitAndFailedGroupPos(lineCount);
            yield return new WaitForSeconds(_rollDelay);
            if (lineCount >= 3)
                view.ScrView_AttachmentsScrollView.SetDragAmount(0, 1f, false);
            int count2 = failedList.Count;
            for (int i = 0; i < count2; i++)
            {
                if (!_isUpdating)
                    yield break;
                MailAttachmentItem item = CreateItems(view.Obj_Source, failedList[i], view.Grd_FailedGrid, i);
                _failedItemList.Add(item);
                view.Grd_FailedGrid.Reposition();
                if (i == 0)
                {
                    view.ScrView_AttachmentsScrollView.ResetPosition();
                }
                if (lineCount >= 3)
                    view.ScrView_AttachmentsScrollView.SetDragAmount(0, 1f, false);
                yield return null;
            }
        }
        _isUpdating = false;
    }

    private void ClearResultInfo()
    {
        _isUpdating = false;
        ClearMailAttachmentItems();
        _successInfoList.Clear();
        _failedInfoList.Clear();
        _successItemList.Clear();
        _failedItemList.Clear();
        view.Grd_SuccessGrid.Reposition();
        view.Grd_FailedGrid.Reposition();
        view.ScrView_AttachmentsScrollView.ResetPosition(); 
    }

    private void ClearMailAttachmentItems()
    {
        if (_successItemList != null && _successItemList.Count > 0)
        {
            int count = _successItemList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                MailAttachmentItem item = _successItemList[i];
                _successItemList.Remove(item);
                item.gameObject.SetActive(false);
                GameObject.Destroy(item.gameObject);
            }
        }
        if (_failedItemList != null && _failedItemList.Count > 0)
        {
            int count = _failedItemList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                MailAttachmentItem item = _failedItemList[i];
                _failedItemList.Remove(item);
                GameObject.Destroy(item.gameObject);
            }
        }
    }

    private void TryAddInfoIntoList(uint id, int number, List<CommonItemData> targetList)
    {
        int count = targetList.Count;
        bool isFind = false;
        for (int i = 0; i < count; i++)
        {
            CommonItemData data = targetList[i];
            if (data.ID == id)
            {
                isFind = true;
                data.Num += number;
                break;
            }
        }
        if (!isFind)
        {
            CommonItemData data = new CommonItemData(id, number);
            targetList.Add(data);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lineOfSuccessItem"></param>
    /// <param name="lineOfCurrency"></param>
    private void UpdateSplitAndFailedGroupPos(int lineOfSuccessItem)
    {
        float fianlYOffest = -1 * lineOfSuccessItem * _successItemHeight;
        view.GameObj_SplitObj.transform.localPosition = new Vector3(_splitOriginalPos.x, _splitOriginalPos.y + fianlYOffest, 0);
        view.Grd_FailedGrid.transform.localPosition = new Vector3(_itemOriginalPos.x, view.GameObj_SplitObj.transform.localPosition.y - _splitHeight, 0);
    }

    private MailAttachmentItem CreateItems(GameObject source, CommonItemData info,  UIGrid parentGird, int index)
    {
        GameObject obj = CommonFunction.InstantiateObject(source, parentGird.transform);
        MailAttachmentItem item = obj.AddComponent<MailAttachmentItem>();
        item.mHideNameLabel = true;
        item.UpdateItemInfo(info.ID, (int)info.Num);
        UIEventListener.Get(obj).onPress = PressItem;
        obj.name = string.Format(ConstString.RECIEVE_ITEM_NAME, index);
        return item;
    }

    private void RollSrollView(int line)
    {
        if (line <= 2)
            return;
        view.ScrView_AttachmentsScrollView.SetDragAmount(0, 1f, false);
    }

    private void PressItem(GameObject go, bool press)
    {
        uint id = go.GetComponent<MailAttachmentItem>().ID;
        HintManager.Instance.SeeDetail(go, press, id);
    }
    //界面动画
    //public void PlayOpenOffsetRootAnim()
    //{
    //    view.OffsetRoot_TScale.gameObject.transform.localScale = Vector3.zero;
    //    view.OffsetRoot_TScale.Restart();
    //    view.OffsetRoot_TScale.PlayForward();
    //}
}
