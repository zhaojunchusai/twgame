using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class MailInfoViewController : UIBase 
{
    const int MAXCOUNT = 6;
    public MailInfoView view;
    private MailType _type;
    private ulong _id;
    private List<MailAttachmentItem> _itemList;
    private bool _isAttach;//有没有附件
    private Vector4 _noAttachClip = new Vector4(-14, 0, 620, 260);
    private Vector4 _attachClip = new Vector4(-14, 70, 620, 112);
    private float _maxContextHeight = 224f;
    private List<CommonItemData> _awardList;
    private bool _isUpdating = false;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new MailInfoView();
            view.Initialize();
            BtnEventBinding();
            _awardList = new List<CommonItemData>();
            _itemList = new List<MailAttachmentItem>();
        }
        
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        _isUpdating = false;
        DestroyItemList();
        if (_awardList != null)
            _awardList.Clear();
        _awardList = null;
        view = null;
        base.Destroy();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_CloseButton;
        UIEventListener.Get(view.Btn_CloseButton.gameObject).onClick = ButtonEvent_CloseButton;
        UIEventListener.Get(view.Btn_DeleteButton.gameObject).onClick = ButtonEvent_DeleteButton;
        UIEventListener.Get(view.Btn_RecieveButton.gameObject).onClick = ButtonEvent_RecieveButton;
    }

    public void ButtonEvent_CloseButton(GameObject btn)
    {
        if (_isUpdating)
            return;
        UISystem.Instance.CloseGameUI(MailInfoView.UIName);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close , view._uiRoot.transform.parent.transform));
    }

    public void ButtonEvent_DeleteButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        UISystem.Instance.CloseGameUI(MailInfoView.UIName);
        MailModule.Instance.OnSendDeleteMail(_id);
    }

    public void ButtonEvent_RecieveButton(GameObject btn)
    {
        //TODO:稳定之后打开此功能
        if (CommonFunction.GetItemOverflowTip(_awardList))
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click , view._uiRoot.transform.parent.transform));

        MailModule.Instance.OnSendGetMailAtt(MailModule.Instance.CurrentID);
    }

    private void PressItem(GameObject go, bool press)
    {
        uint id = go.GetComponent<MailAttachmentItem>().ID;
        HintManager.Instance.SeeDetail(go, press, id);
    }

    public void OnUpdateMailInfo(MailInfo info)
    {
        if (info == null)
        {
            Debug.LogError("Cannt find the mail info  id = " + MailModule.Instance.CurrentID);
            return;
        }
        _type = (MailType)info.type;
        _id = info.mail_id;
        if (info.attachments.Count > 0)
            _isAttach = true;
        else
            _isAttach = false;
        UpdateTitleLabel();
        view.Lbl_TimeLabel.text = info.sentdate.Replace('-', '.');
        UpdateMailString(info.body);

        _awardList.Clear();
        
        _awardList.AddRange(ConvertToAwardList(info.attachments));
        Main.Instance.StartCoroutine(UpdateAttachItemsInspector(_awardList));

        if (_isAttach)
        {
            view.Btn_RecieveButton.gameObject.SetActive(true);
            view.Btn_DeleteButton.gameObject.SetActive(false);
            view.Spt_SplitSprite.gameObject.SetActive(true);
            view.UIPanel_ScrollView.baseClipRegion = _attachClip;
        }
        else
        {
            view.Btn_RecieveButton.gameObject.SetActive(false);
            view.Btn_DeleteButton.gameObject.SetActive(true);
            view.Spt_SplitSprite.gameObject.SetActive(false);
            view.UIPanel_ScrollView.baseClipRegion = _noAttachClip;
        }
        view.UITable_Table.Reposition();
        view.ScrView_ScrollView.ResetPosition();
    }

    private List<CommonItemData> ConvertToAwardList(List<Attachment> list)
    {
        List<CommonItemData> list2 = new List<CommonItemData>();
        if (list == null || list.Count < 1)
        {
            return list2;
        }
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            CommonItemData data = new CommonItemData();
            Attachment att = list[i];
            data.ID = att.id;
            data.Num = (int)att.num;
            list2.Add(data);
        }
        return list2;
    }

    private void DestroyItemList()
    {
        if (_itemList == null || _itemList.Count < 1)
        {
            return;
        }
        for (int i = _itemList.Count - 1; i >= 0; i--)
        {
            MailAttachmentItem item = _itemList[i];
            _itemList.RemoveAt(i);
            GameObject.Destroy(item.gameObject);
        }
        _itemList.Clear();
    }

    private IEnumerator UpdateAttachItemsInspector(List<CommonItemData> list)
    {
        DestroyItemList();
        if (list == null || list.Count < 1)
        {
            yield break;
        }
        int count = list.Count;
        _isUpdating = true;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject go = CommonFunction.InstantiateObject(view.MailAttachmentItem.gameObject, view.Grd_Grid.transform);
            MailAttachmentItem item = go.GetComponent<MailAttachmentItem>();
            item.UpdateItemInfo(list[i].ID, list[i].Num);
            UIEventListener.Get(go).onPress = PressItem;
            _itemList.Add(item);
        }
        _isUpdating = false;
        yield return null;
        view.Grd_Grid.Reposition();
        yield return null;
        view.ScrView_AttachScrollView.ResetPosition();
    }

    private void UpdateTitleLabel()
    {
        string str = "";
        switch (_type)
        {
            case MailType.SYSTEM_MAIL:
                str = ConstString.EMAIL_SYSTEM;
                break;
            case MailType.NOTICE_MAIL:
                str = ConstString.EMAIL_NOTICE;
                break;
            case MailType.PRIVATE_CHAT_MAIL:
                str = ConstString.EMAIL_PRIVATE;
                break;
            case MailType.UNION_MAIL:
                str = ConstString.EMAIL_UNION;
                break;
            case MailType.ARENA_MALL:
                str = ConstString.EMAIL_ARENA;
                break;
        }
        view.Lbl_MailTitleLabel.text = str;

    }

    private void UpdateMailString(string str)
    {
        view.Lbl_InfoLabel.text = str;
        if (!_isAttach)
        {
            if (view.Lbl_InfoLabel.height <= _maxContextHeight)
            {
                view.Lbl_TimeLabel.transform.localPosition = new Vector3(view.Lbl_TimeLabel.transform.localPosition.x, -_maxContextHeight - 15f, 0);
            }
            else
            {
                view.UITable_Table.Reposition();
            }
        }
        else
        {
            view.UITable_Table.Reposition();
        }
        view.ScrView_ScrollView.ResetPosition();
    }

   
//界面动画
    ////public void PlayOpenOffsetRootAnim()
    ////{
    ////    view.OffsetRoot_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    ////    view.OffsetRoot_TScale.Restart();
    ////    view.OffsetRoot_TScale.PlayForward();
    ////}
}
