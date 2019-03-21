using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;

public class MailModule : Singleton<MailModule>
{
    private MailNetWork mNetWork;
    private List<MailInfo> _mailInfoList;
    private List<ulong> _attachtMaiIDlList;
    private bool _isNeedGetMailInfo;

    private ulong _currentID;
    public ulong CurrentID
    {
        get { return _currentID; }
    }

    public void Initialize()
    {
        if (mNetWork == null)
        {
            mNetWork = new MailNetWork();
            mNetWork.RegisterMsg();
        }
        _currentID = 0;
        _isNeedGetMailInfo = true;
        _mailInfoList = new List<MailInfo>();
        _attachtMaiIDlList = new List<ulong>();
    }

    public void Uninitialize()
    {
        mNetWork.RemoveMsg();
        mNetWork = null;
    }

    public void OnSendGetMail()
    {
        if (!_isNeedGetMailInfo)
        {
            return;
        }
        GetMailReq requset = new GetMailReq();
        requset.page = 1;
        mNetWork.OnSendGetMail(requset);
    }

    public void OnReceiveGetMail(GetMailResp response)
    {
        if (response.result == 0)
        {
            _mailInfoList.Clear();
            _mailInfoList.AddRange(response.mail_list);
            _attachtMaiIDlList.Clear();
            _isNeedGetMailInfo = false;
            int count = _mailInfoList.Count;
            for (int i = 0; i < count; i++)
            {
                MailInfo info = _mailInfoList[i];
                if (info.attachments != null && info.attachments.Count > 0)
                {
                    _attachtMaiIDlList.Add(info.mail_id);
                }
            }
            UISystem.Instance.MailView.OnReceiveGetMail(_mailInfoList);
        }
        else
        {
            ErrorCode.ShowErrorTip(response.result);
        }
    }

    public void OnSendReadMail(ulong id)
    {
        ReadMailReq request = new ReadMailReq();
        request.mail_id = id;
        mNetWork.OnSendReadMail(request);
    }

    public void OnReceiveReadMail(ReadMailResp response)
    {
        if (response.result == 0)
        {
            _currentID = response.mail_id;
            UpdateMailInfoStatus(_currentID);
            UISystem.Instance.MailView.OnReceiveReadMail(response.mail_id);
            UISystem.Instance.ShowGameUI(MailInfoView.UIName);
            MailInfo info = GetMailInfoByID(_currentID);
            UISystem.Instance.MailInfoView.OnUpdateMailInfo(info);
        }
        else
        {
            ErrorCode.ShowErrorTip(response.result);
        }
    }

    public void OnSendDeleteMail(ulong id)
    {
        DeleteMailReq request = new DeleteMailReq();
        request.mail_id = id;
        mNetWork.OnSendDeleteMail(request);
    }

    public void OnReceiveDeleteMail(DeleteMailResp response)
    {
        if (response.result == 0)
        {
            List<ulong> list = new List<ulong>();
            list.Add(response.mail_id);
            UISystem.Instance.MailView.OnDeleteMails(list);
            DeleteMailInfoByID(response.mail_id);
        }
        else
        {
            ErrorCode.ShowErrorTip(response.result);
        }
    }

    public void OnSendGetMailAtt(ulong id)
    {
        GetMailAttReq request = new GetMailAttReq();
        request.mail_id = id;
        mNetWork.OnSendGetMailAtt(request);
    }

    public void OnReceiveGetMailAtt(GetMailAttResp response)
    {
        if (response.result == 0)
        {
            List<ulong> delList = new List<ulong>();
            delList.Add(_currentID);
            UISystem.Instance.MailView.OnDeleteMails(delList);
            UISystem.Instance.CloseGameUI(MailInfoView.UIName);
            UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
            RecieveResultVertViewController contrl = UISystem.Instance.RecieveResultVertView;
            MailInfo info = GetMailInfoByID(_currentID);
            int count = info.attachments.Count;
            List<CommonItemData> itemList = new List<CommonItemData>();
            for (int i = 0; i < count; i++)
            {
                Attachment attach = info.attachments[i];
                itemList.Add(new CommonItemData(attach.id, (int)attach.num));
            }
            contrl.UpdateRecieveInfo(itemList);
            DeleteMailInfoByID(_currentID);
            UpdatePlayerPackage(response.update_item,response.update_equip, response.update_soldierequip, response.update_sodier, response.update_soul);
            if(_attachtMaiIDlList.Contains(_currentID))
            {
                _attachtMaiIDlList.Remove(_currentID);
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.RECIEVE_FAILED);
            ErrorCode.ShowErrorTip(response.result);
        }
    }

    public void OnSendOneKeyGetMailAtt()
    {
        foreach (ulong mail in _attachtMaiIDlList)
        {
            Debug.Log("MailID " + mail);
        }
        if (_attachtMaiIDlList.Count < 1)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.EMAIL_NOATTACHMENTS);
            return;
        }

        OneKeyReadMailReq request = new OneKeyReadMailReq();
        request.mail_id_list.AddRange(_attachtMaiIDlList);
        mNetWork.OnSendOneKeyGetMailAtt(request);

    }

    public void OnReceiveOneKeyGetMailAtt(OneKeyReadMailResp response)
    {
        if (response.result == 0)
        {
            UISystem.Instance.ShowGameUI(MailBatchRecieveView.UIName);
            MailBatchRecieveViewController contrl = UISystem.Instance.MailBatchRecieveView;
            List<MailInfo> successMailList = new List<MailInfo>();
            List<MailInfo> failedMailList = new List<MailInfo>();
            int count = response.success_mail_idlist.Count;
            for (int i = 0; i < count; i++)
            {
                MailInfo info = GetMailInfoByID(response.success_mail_idlist[i]);
                if (info == null)
                    Debug.LogError("OnOneKeyReadMailResponse cannt find the mail info that id = " + response.success_mail_idlist[i]);
                else
                {
                    successMailList.Add(info);
                    if (_attachtMaiIDlList.Contains(response.success_mail_idlist[i]))
                    {
                        _attachtMaiIDlList.Remove(response.success_mail_idlist[i]);
                    }
                }
            }
            count = response.fail_mail_idlist.Count;
            for (int i = 0; i < count; i++)
            {
                MailInfo info = GetMailInfoByID(response.fail_mail_idlist[i]);
                if (info == null)
                    Debug.LogError("OnOneKeyReadMailResponse cannt find the mail info that id = " + response.fail_mail_idlist[i]);
                else
                    failedMailList.Add(info);
            }
            contrl.UpdateBatchRecieveInfo(successMailList, failedMailList);
            UISystem.Instance.MailView.OnDeleteMails(response.success_mail_idlist);
            DeletaMailsInfo(response.success_mail_idlist);
            UpdatePlayerPackage(response.update_item, response.update_equip, response.update_soldierequip, response.update_soldier, response.update_soul);
        }
        else
        {
            ErrorCode.ShowErrorTip(response.result);
        }
    }

    public void OnReceiveNewMailNotify(NotifyRefresh resp)
    {
        _isNeedGetMailInfo = true;
    }

    private MailInfo GetMailInfoByID(ulong id)
    {
        if (_mailInfoList == null || _mailInfoList.Count < 1)
            return null;
        int count = _mailInfoList.Count;
        for (int i = 0; i < count; i++)
        {
            MailInfo info = _mailInfoList[i];
            if (info.mail_id == id)
                return info;
        }
        return null;
    }

    private void DeleteMailInfoByID(ulong id)
    {
        if (_mailInfoList == null || _mailInfoList.Count < 1)
            return;
        for (int i = _mailInfoList.Count - 1; i >= 0; i--)
        {
            MailInfo info = _mailInfoList[i];
            if (info.mail_id == id)
            {
                _mailInfoList.Remove(info);
                break;
            }
        }
    }

    private bool UpdateMailInfoStatus(ulong id)
    {
        if (_mailInfoList == null || _mailInfoList.Count < 1)
            return false;
        for (int i = _mailInfoList.Count - 1; i >= 0; i--)
        {
            MailInfo info = _mailInfoList[i];
            if (info.mail_id == id)
            {
                info.status = 0;
                return true;
            }
        }
        return false;
    }

    private void UpdatePlayerPackage(List<fogs.proto.msg.ItemInfo> itemList, List<Equip> equipList, List<Equip> s_equipList,
        List<fogs.proto.msg.Soldier> soldierList,List<fogs.proto.msg.LifeSoul> soulList)
    {
        PlayerData.Instance.UpdateItem(itemList);
        PlayerData.Instance.MultipleAddWeapon(equipList);
        PlayerData.Instance.MultipleAddSoldier(soldierList);
        PlayerData.Instance.MultipleAddWeapon(s_equipList);
        PlayerData.Instance._LifeSoulDepot.AddPackLifeSouls(soulList);
    }

    public void DeletaMailsInfo(List<ulong> list)
    {
        if (list == null || list.Count < 1)
            return;
        foreach (ulong id in list)
        {
            DeleteMailInfoByID(id);
        }
    }

}

