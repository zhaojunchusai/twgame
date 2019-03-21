using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GetEquipEffectViewController : UIBase 
{
    public GetEquipEffectView View;
    #region 缘宝阁
    private int ShowEffetNum = 0;
    public List<GetEquipLocationEffectItem> TensLotteryAnimItems = new List<GetEquipLocationEffectItem>();
    public List<GetEquipLocationEffectItem> TensLotteryResultItems = new List<GetEquipLocationEffectItem>();




    #endregion
    #region 天将神兵
    public List<GameObject> List_Magical_Item = new List<GameObject>();
    public List<UILabel>  List_Magical_Item_Name = new List<UILabel>();
    public List<UISprite> List_Magical_Item_Tex = new List<UISprite>();
    public List<UISprite> List_Magical_Item_Frame = new List<UISprite>();
    public List<UISprite> List_Magical_Item_FrameBG = new List<UISprite>();

    public List<GameObject> List_MagicalResult_Item = new List<GameObject>();
    public List<UILabel>  List_MagicalResult_Item_Name = new List<UILabel>();
    public List<UISprite> List_MagicalResult_Item_Tex = new List<UISprite>();
    public List<UISprite> List_MagicalResult_Item_Frame = new List<UISprite>();
    public List<UISprite> List_MagicalResult_Item_FrameBG = new List<UISprite>();
    #endregion
    public override void Initialize()
    {
       if (View ==null )
       {
           View = new GetEquipEffectView();
           View.Initialize();
       }
       this.View.PropsPackage.SetActive(false);
       this.View.Spt_ViewBG.gameObject.SetActive(true);
       this.View.Spt_Ones_Bg.gameObject.SetActive(true);
       InitBtn();
       InitList();
       InitView();
    }
    public void ShowMagicalEffect(List<Weapon> data)//传入List<Weapon>数据播放天将神兵动画
    {
        View.GO_Magical.SetActive(true);
        CommonFunction.SetSpriteName(View.Spt_ViewBG, GlobalConst.SpriteName.SacrifialEffectBG_Refining);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Collect_Up, View._uiRoot.transform.parent.transform));
        for (int i = 0; i < data.Count; i++)
        {
            List_Magical_Item[i].gameObject.SetActive(true);
            List_MagicalResult_Item[i].gameObject.SetActive(true);
            List_Magical_Item_Name[i].text = List_MagicalResult_Item_Name[i].text = data[i].Att.name;
            CommonFunction.SetSpriteName(List_Magical_Item_Tex[i], data[i].Att.icon);
            CommonFunction.SetSpriteName(List_MagicalResult_Item_Tex[i], data[i].Att.icon);
            CommonFunction.SetQualitySprite(List_Magical_Item_Frame[i], data[i].Att.quality, List_Magical_Item_FrameBG[i]);
            CommonFunction.SetQualitySprite(List_MagicalResult_Item_Frame[i], data[i].Att.quality, List_MagicalResult_Item_FrameBG[i]);
        }
        View.UI_Magical_Item_Grid.Reposition();
        Main.Instance.StartCoroutine(IEnumerator_Magical(2.0F));
    }
    public void ShowOnesLottery(PropsPackageCommonData vData)
    {
        SetOnesBtnScale(false);
        View.GO_OnesLottery.SetActive(true);
        View.Btn_OnesLotteryAnim.gameObject.SetActive(true);
        //InitOnesChooseView(View.Spt_OnesSingleCurrencyIcon, View.Lab_OnesSingleCurrencyNum);
        //InitTensChooseView(View.Spt_OnesMultipleCurrencyIcon, View.Lab_OnesMultipleCurrencyNum);
        this.View.Button.gameObject.SetActive(false);
        this.View.PropsPackage.SetActive(true);
        this.View.Spt_ViewBG.gameObject.SetActive(false);
        this.View.PropsPackage_Mask.gameObject.SetActive(false);
        this.View.Spt_Ones_Bg.gameObject.SetActive(false);
        UIEventListener.Get(this.View.PropsPackage_Mask.gameObject).onClick = EventButton_CloseView;

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Collect_Up, View._uiRoot.transform.parent.transform));
        Main.Instance.StartCoroutine(IEnumerator_OnesProp(1.25F));
        if (vData != null)
        {
            View.OnesLocationEffectAnimItem.InitItem(vData);
            View.OnesLocationEffectResultItem.InitItem(vData);
            return;
        }
    }
    public void ShowOnesLottery(List<Weapon> data ,List<fogs.proto.msg.ItemInfo>update_Chip ,List<fogs.proto.msg.ItemInfo> update_Item)//单次缘宝阁道具抽奖
    {
        //if (data == null && update_Chip == null && update_Item == null) return;
        SetOnesBtnScale(false);
        View.GO_OnesLottery.SetActive(true);
        View.Btn_OnesLotteryAnim.gameObject.SetActive(true);
        CommonFunction.SetSpriteName(View.Spt_ViewBG, GlobalConst.SpriteName.GetEquipEffectBG_YuanBaoGe);
        InitOnesChooseView(View.Spt_OnesSingleCurrencyIcon, View.Lab_OnesSingleCurrencyNum);
        InitTensChooseView(View.Spt_OnesMultipleCurrencyIcon, View.Lab_OnesMultipleCurrencyNum);
        this.View.Button.gameObject.SetActive(true);
        this.View.PropsPackage.SetActive(false);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Collect_Up, View._uiRoot.transform.parent.transform));
        Main.Instance.StartCoroutine (IEnumerator_OnesLottery(1.25F));
        if (data !=null && data.Count != 0 )
        {
            View.OnesLocationEffectAnimItem.InitItem(data[0]);
            View.OnesLocationEffectResultItem.InitItem(data[0]);
            return;
        }
        if (update_Chip!=null && update_Chip.Count != 0)
        {
            View.OnesLocationEffectAnimItem.InitChipItem(update_Chip[0]);
            View.OnesLocationEffectResultItem.InitChipItem(update_Chip[0]);
            return;

        }
        if (update_Item!=null && update_Item.Count != 0)
        {
            View.OnesLocationEffectAnimItem.InitChipItem(update_Item[0]);
            View.OnesLocationEffectResultItem.InitChipItem(update_Item[0]);
            return;
        }
        
    }
    public void ShowTensLottery(List<Weapon> data, List<fogs.proto.msg.ItemInfo> update_Chip, List<fogs.proto.msg.ItemInfo> update_Item)//十次缘宝阁道具抽奖
    {
        //if (data == null && update_Chip == null && update_Item == null) return;
        //Debug.LogError("a=" + data.Count + "     b= " + update_Chip.Count + "    C=" + update_Item.Count);
        ShowEffetNum = 0;
        SetTensBtnScale(false);
        View.GO_TensLottery.SetActive(true);
        View.Btn_TensLotteryAnim.gameObject.SetActive(true);
        CommonFunction.SetSpriteName(View.Spt_ViewBG, GlobalConst.SpriteName.GetEquipEffectBG_YuanBaoGe);
        InitOnesChooseView(View.Spt_TensSingleCurrencyIcon, View.Lab_TensSingleCurrencyNum);
        InitTensChooseView(View.Spt_TensMultipleCurrencyIcon, View.Lab_TensMultipleCurrencyNum);
        this.View.Button.gameObject.SetActive(true);
        this.View.PropsPackage.SetActive(false);

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Collect_Up, View._uiRoot.transform.parent.transform));
        if (data != null && data.Count != 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                //Debug.LogError(data[i].Att.name);
                TensLotteryAnimItems[ShowEffetNum].InitItem(data[i]);
                TensLotteryResultItems[ShowEffetNum].InitItem(data[i]);
                ShowEffetNum += 1;
            }
        }
        if (update_Chip != null && update_Chip.Count != 0)
        {
            for (int i = 0; i < update_Chip.Count; i++) 
            {
                //Debug.LogError(update_Chip[i].change_num+"        "+update_Chip[i].num);
                TensLotteryAnimItems[ShowEffetNum].InitChipItem(update_Chip[i]);
                TensLotteryResultItems[ShowEffetNum].InitChipItem(update_Chip[i]);
                ShowEffetNum += 1;
            }
        }
        if (update_Item != null && update_Item.Count != 0)
        {

            for (int i = 0; i < update_Item.Count; i++)
            {
          
                TensLotteryAnimItems[ShowEffetNum].InitChipItem(update_Item[i]);
                TensLotteryResultItems[ShowEffetNum].InitChipItem(update_Item[i]);
                ShowEffetNum += 1;
            }
        }
        Main.Instance.StartCoroutine(IEnumerator_TensLottery(2.0F));
    }
    #region Init
    private void InitBtn()
    {
        #region 缘宝阁
        //Ones
        UIEventListener.Get(View.Btn_OnesCloseBtn.gameObject).onClick = EventButton_CloseOnesLotteryAnim;
        UIEventListener.Get(View.Btn_OnesLotteryAnim.gameObject).onClick = EventButton_CloseOnesLotteryAnim;
        UIEventListener.Get(View.Btn_OnesSingleBtn.gameObject).onClick = EventButton_ChooseOnesLotteryBtn;
        UIEventListener.Get(View.Btn_OnesMultipleBtn.gameObject).onClick = EventButton_ChooseTensLotteryBtn;
        //UIEventListener.Get(View.Btn_OnesLotteryResult.gameObject).onClick = EventButton_CloseView;

        //Tens
        UIEventListener.Get(View.Btn_TensCloseBtn.gameObject).onClick = EventButton_CloseTensLotteryAnim;
        UIEventListener.Get(View.Btn_TensLotteryAnim.gameObject).onClick = EventButton_CloseTensLotteryAnim;
        UIEventListener.Get(View.Btn_TensSingleBtn.gameObject).onClick = EventButton_ChooseOnesLotteryBtn;
        UIEventListener.Get(View.Btn_TensMultipleBtn.gameObject).onClick = EventButton_ChooseTensLotteryBtn;
        //UIEventListener.Get(View.Btn_TensLotteryResult.gameObject).onClick = EventButton_CloseView;

            
        #endregion
        #region 天将神兵
        UIEventListener.Get(View.Btn_Magical.gameObject).onClick = EventButton_CloseMagical;
        UIEventListener.Get(View.Btn_MagicalResult.gameObject).onClick = EventButton_CloseView;
        #endregion
    }
    private void InitList()
    {
        #region 缘宝阁
        TensLotteryAnimItems.Clear();
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_0);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_1);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_2);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_3);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_4);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_5);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_6);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_7);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_8);
        TensLotteryAnimItems.Add(View.TensLocationEffectAnimItem_9);

        TensLotteryResultItems.Clear();
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_0);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_1);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_2);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_3);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_4);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_5);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_6);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_7);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_8);
        TensLotteryResultItems.Add(View.TensLocationEffectResultItem_9);

        #endregion
        #region 天将神兵
        List_Magical_Item.Clear();
        List_Magical_Item.Add(View.Go_Magical_Item_0);
        List_Magical_Item.Add(View.Go_Magical_Item_1);
        List_Magical_Item.Add(View.Go_Magical_Item_2);
        List_Magical_Item.Add(View.Go_Magical_Item_3);
        List_Magical_Item.Add(View.Go_Magical_Item_4);
        List_Magical_Item.Add(View.Go_Magical_Item_5);
        List_Magical_Item.Add(View.Go_Magical_Item_6);
        List_Magical_Item.Add(View.Go_Magical_Item_7);
        List_Magical_Item.Add(View.Go_Magical_Item_8);
        List_Magical_Item.Add(View.Go_Magical_Item_9);
        List_Magical_Item_Name.Clear();
        List_Magical_Item_Name.Add(View.Go_Magical_Item_0_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_1_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_2_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_3_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_4_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_5_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_6_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_7_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_8_Name);
        List_Magical_Item_Name.Add(View.Go_Magical_Item_9_Name);
        List_Magical_Item_Tex.Clear();
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_0_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_1_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_2_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_3_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_4_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_5_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_6_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_7_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_8_Tex);
        List_Magical_Item_Tex.Add(View.Go_Magical_Item_9_Tex);
        List_Magical_Item_Frame.Clear();
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_0_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_1_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_2_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_3_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_4_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_5_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_6_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_7_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_8_Frame);
        List_Magical_Item_Frame.Add(View.Go_Magical_Item_9_Frame);
        List_Magical_Item_FrameBG.Clear();
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_0_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_1_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_2_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_3_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_4_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_5_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_6_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_7_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_8_FrameBG);
        List_Magical_Item_FrameBG.Add(View.Go_Magical_Item_9_FrameBG);

        //MagicalResult:
        List_MagicalResult_Item.Clear();
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_0);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_1);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_2);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_3);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_4);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_5);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_6);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_7);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_8);
        List_MagicalResult_Item.Add(View.Go_MagicalResult_Item_9);
        List_MagicalResult_Item_Name.Clear();
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_0_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_1_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_2_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_3_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_4_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_5_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_6_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_7_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_8_Name);
        List_MagicalResult_Item_Name.Add(View.Go_MagicalResult_Item_9_Name);
        List_MagicalResult_Item_Tex.Clear();
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_0_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_1_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_2_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_3_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_4_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_5_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_6_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_7_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_8_Tex);
        List_MagicalResult_Item_Tex.Add(View.Go_MagicalResult_Item_9_Tex);
        List_MagicalResult_Item_Frame.Clear();
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_0_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_1_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_2_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_3_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_4_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_5_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_6_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_7_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_8_Frame);
        List_MagicalResult_Item_Frame.Add(View.Go_MagicalResult_Item_9_Frame);
        List_MagicalResult_Item_FrameBG.Clear();
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_0_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_1_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_2_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_3_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_4_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_5_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_6_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_7_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_8_FrameBG);
        List_MagicalResult_Item_FrameBG.Add(View.Go_MagicalResult_Item_9_FrameBG);
        #endregion
    }
    private void InitView()
    {
        #region 缘宝阁
        View.GO_OnesLottery.SetActive(false);
        View.Btn_OnesLotteryAnim.gameObject.SetActive(false);
        View.Btn_OnesLotteryResult.gameObject.SetActive(false);
        View.GO_TensLottery.SetActive(false);
        View.Btn_TensLotteryAnim.gameObject.SetActive(false);
        View.Btn_TensLotteryResult.gameObject.SetActive(false);
        #endregion
        #region 天将神兵
        View.GO_Magical.gameObject.SetActive(false);
        View.GO_MagicalResult.gameObject.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            List_Magical_Item[i].gameObject.SetActive(false);
            List_MagicalResult_Item[i].gameObject.SetActive(false);
        }
        #endregion
    }
    public override void Destroy()
    {
        base.Destroy();
        View = null;
    } 
    public override void Uninitialize()
    {
        #region 缘宝阁
        TensLotteryAnimItems.Clear();
        TensLotteryResultItems.Clear();
        #endregion
        #region 天将神兵
        List_Magical_Item.Clear();
        List_Magical_Item_Name.Clear();
        List_Magical_Item_Tex.Clear();
        List_Magical_Item_Frame.Clear();
        List_Magical_Item_FrameBG.Clear();

        List_MagicalResult_Item.Clear();
        List_MagicalResult_Item_Name.Clear();
        List_MagicalResult_Item_Tex.Clear();
        List_MagicalResult_Item_Frame.Clear();
        List_MagicalResult_Item_FrameBG.Clear();
        #endregion
    }

    #endregion
    #region Button
    private void EventButton_CloseView(GameObject Btn)//关闭界面
    {
        //Main.Instance.StopAllCoroutines();
        UISystem.Instance.CloseGameUI(GetEquipEffectView.UIName);
        UIEventListener.Get(this.View.PropsPackage_Mask.gameObject).onClick = null;
    }
    private void EventButton_CloseMagical(GameObject Btn)//关闭天降神兵动画
    {
        View.GO_Magical.SetActive(false);
        View.GO_MagicalResult.SetActive(true);
        View.UI_MagicalResult_Item_Grid.Reposition();
    }
    private void EventButton_CloseTensLotteryAnim(GameObject Btn)//关闭缘宝阁十次招募
    {
        SetTensBtnScale(true);
        Main.Instance.StopAllCoroutines();
        View.Btn_TensLotteryAnim.gameObject.SetActive(false);
        View.Btn_TensLotteryResult.gameObject.SetActive(true);
        UIEventListener.Get(View.Btn_TensCloseBtn.gameObject).onClick = EventButton_CloseView;

    }
    private void EventButton_CloseOnesLotteryAnim(GameObject Btn)//关闭元宝阁单次招募
    {
        SetOnesBtnScale(true);
        Main.Instance.StopAllCoroutines();
        View.Btn_OnesLotteryAnim.gameObject.SetActive(false);
        View.Btn_OnesLotteryResult.gameObject.SetActive(true);
        View.PropsPackage_Mask.gameObject.SetActive(true);
        UIEventListener.Get(View.Btn_OnesCloseBtn.gameObject).onClick = EventButton_CloseView;
    }
    private void EventButton_ChooseOnesLotteryBtn(GameObject Btn)//元宝阁单次招募
    {
        if (PlayerData.Instance.DrowEquipFreeCount > 0 && Main.mTime >= PlayerData.Instance.DrowEquipFreeTime)
        {
            DrowEquipModule.Instance.SendOneExtractEquipReq();
            return;
        }
        else
        {
            DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(0);

            if (info != null && info.Gold_type == 1 && PlayerData.Instance._Gold < info.Gold_num)
            {
                int num = PlayerData.Instance.GetItemCountByID((uint)info.Item_id);
                if (num < info.Item_num)
                {
                    ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold);
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
                    return;
                }
            }
        }
        DrowEquipModule.Instance.SendOneExtractEquipReq();
    }
    private void EventButton_ChooseTensLotteryBtn(GameObject Btn)//元宝阁十次招募
    {
        DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(1);

        if (info != null && info.Gold_type == 1 && PlayerData.Instance._Gold < info.Gold_num)
        {
            int num = PlayerData.Instance.GetItemCountByID((uint)info.Item_id);
            if (num < info.Item_num)
            {
                ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
                return;
            }
        }
        DrowEquipModule.Instance.SendMultipleExtractReq();
    }
    #endregion
    #region IEnumerator
    private IEnumerator Ienumerator_ReposGrid(float time)
    {
        yield return new WaitForSeconds(time);
        View.UI_Magical_Item_Grid.Reposition();
        //View.UI_MagicalResult_Item_Grid.Reposition();
    }
    private IEnumerator IEnumerator_Magical(float time)//天将神兵动画结束后直接关闭界面
    {
        yield return new WaitForSeconds(time);
        View.UI_Magical_Item_Grid.Reposition();
        UIEventListener.Get(View.Btn_Magical.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_Magical.gameObject).onClick = EventButton_CloseView;
    }
    private IEnumerator IEnumerator_TensLottery(float time)//缘宝阁十次动画结束后直接关闭界面
    {
        yield return new WaitForSeconds(time);
        SetTensBtnScale(true);
        UIEventListener.Get(View.Btn_TensLotteryAnim.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_TensCloseBtn.gameObject).onClick = EventButton_CloseView;
        //UIEventListener.Get(View.Btn_TensLotteryAnim.gameObject).onClick = EventButton_CloseView;
    }
    private IEnumerator IEnumerator_OnesLottery(float time)//缘宝阁单次动画结束后直接关闭界面
    {
        yield return new WaitForSeconds(time);
        SetOnesBtnScale(true);
        UIEventListener.Get(View.Btn_OnesLotteryAnim.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_OnesCloseBtn.gameObject).onClick = EventButton_CloseView;
        //UIEventListener.Get(View.Btn_OnesLotteryAnim.gameObject).onClick = EventButton_CloseView;
    }
    private IEnumerator IEnumerator_OnesProp(float time)//定向包抽取结果展示
    {
        yield return new WaitForSeconds(time);
        //SetOnesBtnScale(true);
        this.View.PropsPackage_Mask.gameObject.SetActive(true);
        //UIEventListener.Get(View.Btn_OnesLotteryAnim.gameObject).onClick = EventButton_CloseView;
    }

    #endregion
    private void InitOnesChooseView(UISprite Ones, UILabel OnesLab)
    {
        DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(0);
        if (info == null) return;
        if (PlayerData.Instance.DrowEquipFreeCount > 0 && Main.mTime >= PlayerData.Instance.DrowEquipFreeTime)
        {
            CommonFunction.SetSpriteName(Ones, GetMoneyIcon(1));
            OnesLab.text = ConstString.FREE;
            return;
        }
        int num = PlayerData.Instance.GetItemCountByID((uint)info.Item_id);
        if (num>=info.Item_num)
        {
            ItemInfo tmpinfo = ConfigManager.Instance.mItemData.GetItemInfoByID((uint)info.Item_id);
            if (tmpinfo == null) return;
            CommonFunction.SetSpriteName(Ones, tmpinfo.icon);
            OnesLab.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, num, info.Item_num);
            View.Spt_Ones_Button_Dis.gameObject.SetActive(false);
            return;
        }
        else
        {
            CommonFunction.SetSpriteName(Ones, GetMoneyIcon(info.Gold_type));
            OnesLab.text = info.Gold_num.ToString();
            View.Spt_Ones_Button_Dis.gameObject.SetActive(true);

        }
    }
    private void InitTensChooseView(UISprite Tens,UILabel TensLab)
    {
        View.Spt_Tens_Button_Dis.gameObject.SetActive(false);
        DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(1);
        if (info == null) return;
        int num = PlayerData.Instance.GetItemCountByID((uint)info.Item_id);
        if (num >= info.Item_num)
        {
            ItemInfo tmpinfo = ConfigManager.Instance.mItemData.GetItemInfoByID((uint)info.Item_id);
            if (tmpinfo == null) return;
            CommonFunction.SetSpriteName(Tens, tmpinfo.icon);
            TensLab.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, num, info.Item_num);
            View.Spt_Tens_Button_Dis.gameObject.SetActive(false);
        }
        else
        {
            CommonFunction.SetSpriteName(Tens, GetMoneyIcon(info.Gold_type));
            TensLab.text = info.Gold_num.ToString();
            View.Spt_Tens_Button_Dis.gameObject.SetActive(true);
        }

    }
    private string GetMoneyIcon(int type)
    {
        if (type == 2)
            return "ZCJ_icon_daibi_l";
        else
            return "ZCJ_icon_jinbi_l";

    }
    //按钮显示
    private void SetOnesBtnScale(bool IsOpen)
    {
        if(IsOpen)
        {
            View.Btn_OnesSingleBtn.gameObject.transform.localScale = Vector3.one;
            View.Btn_OnesMultipleBtn.gameObject.transform.localScale = Vector3.one;
            View.Spt_OnesMultipleCurrencyIcon.gameObject.transform.localScale = Vector3.one;
            View.Spt_OnesSingleCurrencyIcon.gameObject.transform.localScale = Vector3.one;
        }
        else
        {
            View.Btn_OnesSingleBtn.gameObject.transform.localScale = Vector3.zero;
            View.Btn_OnesMultipleBtn.gameObject.transform.localScale = Vector3.zero;
            View.Spt_OnesMultipleCurrencyIcon.gameObject.transform.localScale = Vector3.zero;
            View.Spt_OnesSingleCurrencyIcon.gameObject.transform.localScale = Vector3.zero;

        }
    }
    private void SetTensBtnScale(bool IsOpen)
    {
        if (IsOpen)
        {
            View.Btn_TensSingleBtn.gameObject.transform.localScale = Vector3.one;
            View.Btn_TensMultipleBtn.gameObject.transform.localScale = Vector3.one;
            View.Spt_TensMultipleCurrencyIcon.gameObject.transform.localScale = Vector3.one;
            View.Spt_TensSingleCurrencyIcon.gameObject.transform.localScale = Vector3.one;

        }
        else
        {
            View.Btn_TensSingleBtn.gameObject.transform.localScale = Vector3.zero;
            View.Btn_TensMultipleBtn.gameObject.transform.localScale = Vector3.zero;
            View.Spt_TensMultipleCurrencyIcon.gameObject.transform.localScale = Vector3.zero;
            View.Spt_TensSingleCurrencyIcon.gameObject.transform.localScale = Vector3.zero;

        }

    }

}
