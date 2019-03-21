using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;

public class MailViewController : UIBase
{
    public MailView view;
    private List<MailItem> _mailList;
    private bool _isUpdating = false;
    private float _cellHeight = 0f;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new MailView();
            view.Initialize();
            BtnEventBinding();
            _mailList = new List<MailItem>();
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        _cellHeight = view.Grd_Grid.cellHeight;
        // PlayOpenAnim();
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        _isUpdating = false;
        ClearEmailItems();
        view = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_MaskSprite.gameObject).onClick = ButtonEvent_CloseMailViewButton;
        UIEventListener.Get(view.Btn_BatchReceiveButton.gameObject).onClick = ButtonEvent_BatchReceiveButton;
    }
    /// <summary>
    /// 打开界面都会调用这个方法
    /// </summary>
    public void OnSendGetMail()
    {
        MailModule.Instance.OnSendGetMail();
    }

    public void OnReceiveGetMail(List<MailInfo> list)
    {
        ClearEmailItems();
        if (list == null || list.Count < 1)
            return;
        Main.Instance.StartCoroutine(InitMailItemInspector(list));
    }

    public void OnUpdateMailInfo(List<MailInfo> list)
    {

    }

    public void OnSendReadMail(ulong id)
    {
        MailModule.Instance.OnSendReadMail(id);
    }
    /// <summary>
    /// 删掉需要删除的邮件
    /// </summary>
    /// <param name="list"></param>
    public void OnReceiveReadMail(ulong id)
    {
        MailItem item = GetMailItemByID(id);
        if (item == null)
        {
            Debug.LogError("Cannt find the mailitem id = " + id);
            return;
        }
        item.UpdateItemIcon(true, item.IsAttach);
    }
    /// <summary>
    /// 删掉成功领取的邮件
    /// </summary>
    /// <param name="list"></param>
    public void OnDeleteMails(List<ulong> list)
    {
        if (_mailList == null || _mailList.Count < 1)
            return;
        bool isDel = false;
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = _mailList.Count - 1; j >= 0; j--)
            {
                MailItem item = _mailList[j];
                if (item.ID == list[i])
                {
                    isDel = true;
                    item.UpdateItemIcon(true, item.IsAttach);
                    _mailList.Remove(item);
                    item.gameObject.SetActive(false);
                    GameObject.Destroy(item.gameObject);
                }
            }
        }
        if (isDel)
        {
            view.Grd_Grid.Reposition();
            view.Scr_ScorllView.ResetPosition();
        }
    }

    public void ButtonEvent_EmailItemButton(GameObject btn)
    {
        if (_isUpdating)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        MailItem item = btn.GetComponent<MailItem>();
        ulong id = item.ID;
        OnSendReadMail(id);
    }

    public void ButtonEvent_BatchReceiveButton(GameObject btn)
    {
        if (_isUpdating)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (_mailList == null || _mailList.Count < 0)
            return;
        MailModule.Instance.OnSendOneKeyGetMailAtt();
    }

    public void ButtonEvent_CloseMailViewButton(GameObject btn)
    {
        if (_isUpdating)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(MailView.UIName);
    }

    private IEnumerator InitMailItemInspector(List<MailInfo> list)
    {
        _isUpdating = true;
        yield return null;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject obj = CommonFunction.InstantiateObject(view.Obj_Source, view.Grd_Grid.transform);
            obj.name = string.Format(ConstString.EMAIL_ITEM_NAME, i);
            MailInfo info = list[i];
            MailItem item = obj.AddComponent<MailItem>();
            item.transform.localPosition = new Vector3(0, -_cellHeight * i, 0);
            bool isRead = info.status == 0;
            bool isAttach = info.attachments.Count > 0;
            item.Initialize();
            item.UpdateMailItem(info.mail_id, isRead, isAttach, info.title, info.body, info.sentdate,info.deadline);
            UIEventListener.Get(obj).onClick += ButtonEvent_EmailItemButton;
            _mailList.Add(item);
            yield return null;
            view.Grd_Grid.Reposition();
        }
        _isUpdating = false;
        yield return null;
        view.Scr_ScorllView.ResetPosition();

    }

    private MailItem GetMailItemByID(ulong id)
    {
        if (_mailList == null || _mailList.Count < 1)
        {
            Debug.LogWarning("GetMailItemByID  _mailList is null ???? ");
            return null;
        }
        for (int i = 0; i < _mailList.Count; i++)
        {
            MailItem item = _mailList[i];
            //Debug.LogWarning("id  = " + id + "\n itemid = " + item.ID);
            if (item.ID == id)
                return item;
        }
        return null;
    }

    private void ClearEmailItems()
    {
        if (_mailList == null || _mailList.Count < 1)
            return;
        for (int i = _mailList.Count - 1; i >= 0; i--)
        {
            MailItem item = _mailList[i];
            _mailList.Remove(item);
            item.gameObject.SetActive(false);
            GameObject.Destroy(item.gameObject);
        }
        _mailList.Clear();
        view.Grd_Grid.Reposition();
        view.Scr_ScorllView.ResetPosition();
    }
    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();
    //}
}
