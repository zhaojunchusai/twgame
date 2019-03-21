using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;


public class SignViewController : UIBase 
{
    public SignView view;

    private List<SignItem> _continuousItems = new List<SignItem>();
    private List<SignItem> _totalItems = new List<SignItem>();

    private bool _firstOpen = true;
    public override void Initialize()
    {
        if (view == null) 
        {
            view = new SignView();
            view.Initialize();
            BtnEventBinding();
        }
     //   PlayOpenAnim();
       // UISystem.Instance.ShowGameUI(TopFuncView.UIName);
        if (_firstOpen)
        {
            _firstOpen = false;
            ShowTotalLoginAward();
            ShowContinousLoginAward();
        }
        view.Lbl_ContinuousLoginCount.text = string .Format (ConstString .FORMAT_CONTINUOUSDAY,PlayerData.Instance.LoginChangeInfo.continu_num);
        view.Lbl_TotalLoginCount.text = string .Format (ConstString .FORMAT_TOTALDAY, PlayerData.Instance.LoginChangeInfo.cumulative_num);
        UpdateContinuousSignItemState();
        UpdateTotalSignItemState();
        ShowExtraAward();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSignView);
    }
  
    private void ShowExtraAward()
    {
        view.Gobj_ExtraAward.SetActive(false);
        VipData vipData;
        uint vipLv = PlayerData.Instance._VipLv;
        int goldNum = 0;
        if (vipLv == 0)
        {
            vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(1);
            view.Lbl_ExtraAwardTip.text = ConstString.FORMAT_EXTRA_AWARD_0;
        }
        else
        {
            vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(vipLv);
            view.Lbl_ExtraAwardTip.text = string.Format(ConstString.FORMAT_EXTRA_AWARD_NOT0, vipLv);
        }

        if (vipData != null)
        {
            List<DroppackInfo> pack = ConfigManager.Instance.mDroppackData.GetDropPackByID(vipData.LoginExtraAward);
            //Debug.Log(vipData.LoginExtraAward);
            if (pack != null)
            {
                //Debug.Log(pack.Count);
                for (int i = 0; i < pack.Count; i++)
                {
                    //Debug.Log(pack[i].ItemID + " " + pack[i].ItemLowerLimit);
                    if (pack[i].ItemID == GlobalCoefficient.CoinID)
                    {
                        goldNum = pack[i].ItemLowerLimit;
                        break;
                    }
                }
            }
        }
        view.Lbl_ExtraAwardNum.text = goldNum.ToString();
        view.Table_ExtraAward.Reposition();

    }

    public void UpdateContinuousSignItemState()
    {
        for (int i = 0; i < _continuousItems.Count; i++)
        {
            _continuousItems[i].RefreshSignState();
        }
    }

    public void UpdateTotalSignItemState()
    {
        for (int i = 0; i < _totalItems.Count; i++)
        {
            _totalItems[i].RefreshSignState();
        }
    }

    private void ShowContinousLoginAward()
    {
        ShowLoginAwardItems(ConfigManager.Instance.mLoginAwardConfig.GetContinousLoginAwardList(), view.Grd_ContinuousLogin, _continuousItems);
    }

    private void ShowTotalLoginAward()
    {
        ShowLoginAwardItems(ConfigManager.Instance.mLoginAwardConfig.GetTotalLoginAwardList(), view.Grd_TotalLogin, _totalItems);
    }

    private void ShowLoginAwardItems(List<LoginAwardData> list,UIGrid grid,List<SignItem> itemlist)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = CommonFunction.InstantiateObject(view.gobj_SignItem, grid.transform);
            if (!go.activeSelf)
                go.SetActive(true);
            itemlist.Add(go.AddComponent<SignItem>());
            itemlist[i].InitItem(list[i]);
        }
        grid.Reposition();
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close , view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(SignView.UIName);
    }

    public override void Uninitialize()
    {

        
    }

    public override void Destroy()
    {
        _continuousItems.Clear();
        _totalItems.Clear();
        _firstOpen = true;
        view = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
    }
    //界面动画
   //public void PlayOpenAnim()
   // {
   //     view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
   //     view.Anim_TScale.Restart();
   //     view.Anim_TScale.PlayForward();
   // }

}
