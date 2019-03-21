using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common;
public class RecruitResultViewController : UIBase
{
    public RecruitResultView view;
    public UIEventListener.VoidDelegate SingleEvent;
    public UIEventListener.VoidDelegate MultipleEvent;
    public Vector2 _propIconSize = new Vector2(45, 45);
    public Vector2 _goldIconSize = new Vector2(50, 50);
    public List<GameObject> Go_RoleBaseList = new List<GameObject>();

    public GameObject Once1, Once2;


    public List<GameObject> Go_NameAnim = new List<GameObject>();
    public List<RecruitResultItem> tenAnimItems = new List<RecruitResultItem>();
    public List<GameObject> Go_NameResult = new List<GameObject>();
    public List<RecruitResultItem> tenResultItems = new List<RecruitResultItem>();

    public List<UILabel> Lab_Name = new List<UILabel>();

    public override void Initialize()
    {
        if (view == null)
        {
            view = new RecruitResultView();
            view.Initialize();
            BtnEventBinding();
        }
        InitName();
        InitPn(false);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Collect_Up, view._uiRoot.transform.parent.transform));

    }
    public void InitName()
    {
        Go_NameResult.Clear(); tenResultItems.Clear();
        Go_NameResult.Add(view.TenNameResult_Name0); tenResultItems.Add(view.TenNameResult_Name0_Item);
        Go_NameResult.Add(view.TenNameResult_Name1); tenResultItems.Add(view.TenNameResult_Name1_Item);
        Go_NameResult.Add(view.TenNameResult_Name2); tenResultItems.Add(view.TenNameResult_Name2_Item);
        Go_NameResult.Add(view.TenNameResult_Name3); tenResultItems.Add(view.TenNameResult_Name3_Item);
        Go_NameResult.Add(view.TenNameResult_Name4); tenResultItems.Add(view.TenNameResult_Name4_Item);
        Go_NameResult.Add(view.TenNameResult_Name5); tenResultItems.Add(view.TenNameResult_Name5_Item);
        Go_NameResult.Add(view.TenNameResult_Name6); tenResultItems.Add(view.TenNameResult_Name6_Item);
        Go_NameResult.Add(view.TenNameResult_Name7); tenResultItems.Add(view.TenNameResult_Name7_Item);
        Go_NameResult.Add(view.TenNameResult_Name8); tenResultItems.Add(view.TenNameResult_Name8_Item);
        Go_NameResult.Add(view.TenNameResult_Name9); tenResultItems.Add(view.TenNameResult_Name9_Item);

        Go_NameAnim.Clear(); tenAnimItems.Clear();
        Go_NameAnim.Add(view.TenNameAnim_Name0); tenAnimItems.Add(view.TenNameAnim_Name0_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name1); tenAnimItems.Add(view.TenNameAnim_Name1_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name2); tenAnimItems.Add(view.TenNameAnim_Name2_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name3); tenAnimItems.Add(view.TenNameAnim_Name3_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name4); tenAnimItems.Add(view.TenNameAnim_Name4_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name5); tenAnimItems.Add(view.TenNameAnim_Name5_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name6); tenAnimItems.Add(view.TenNameAnim_Name6_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name7); tenAnimItems.Add(view.TenNameAnim_Name7_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name8); tenAnimItems.Add(view.TenNameAnim_Name8_Item);
        Go_NameAnim.Add(view.TenNameAnim_Name9); tenAnimItems.Add(view.TenNameAnim_Name9_Item);
        view.TenNameAnim_Objet.SetActive(false);
        view.TenNameResult_Objet.SetActive(false);
        view.OnceResult.SetActive(false);
        view.OnceAnim.SetActive(false);

    }
    public void ButtonEvent_OpenTenResult(GameObject btn)
    {
        view.TenNameAnim_Objet.SetActive(false);
        view.TenNameResult_Objet.SetActive(true);
        InitPn(true);
    }
    public void ButtonEvent_OpenOnceResult(GameObject Btn)
    {
        view.OnceAnim.SetActive(false);
        view.OnceResult.SetActive(true);
        InitPn(true);
    }
    public void SetOnceSoliderBG(string name, int quality,bool isChip,int chipNum)
    {
        view.AnimLabel.text = view.ResultLabel.text = name;
        CommonFunction.SetSpriteName(view.ResultQuality, GlobalConst.SpriteName.SoldierRecruitQuality[quality]);
        CommonFunction.SetSpriteName(view.AnimQuality, GlobalConst.SpriteName.SoldierRecruitQuality[quality]);
        if (isChip)
        {
            view.AnimChip.gameObject.SetActive(true);
            view.ResultChip.gameObject.SetActive(true);
            view.AnimNum.gameObject.SetActive(true);
            view.ResultNum.gameObject.SetActive(true);
            view.AnimNum.text = string.Format(ConstString.FORMAT_NUM_X, chipNum);
            view.ResultNum.text = string.Format(ConstString.FORMAT_NUM_X, chipNum);
        }
        else
        {
            view.AnimChip.gameObject.SetActive(false);
            view.ResultChip.gameObject.SetActive(false);
            view.AnimNum.text = string.Empty;
            view.ResultNum.text = string.Empty;
        }

    }
    public void ButtonEvent_BackBtn(GameObject btn)
    {
        DestoryItem();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        UISystem.Instance.RecruitView.IsBlueItemOpen = false;
        //CommonFunction.ClearChild(view.Gobj_Multiple.transform);
        //CommonFunction.ClearChild(view.Gobj_Single.transform);
        UISystem.Instance.CloseGameUI(RecruitResultView.UIName);
        if (!GuideManager.Instance.GuideIsRunning())
        {
            UISystem.Instance.RecruitView.InitItemStateSort();
        }
        else
        {
            Main.Instance.StartCoroutine(ItemInitSort());
        }

        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseRecruitResult);

    }
    public IEnumerator ItemInitSort()
    {
        yield return new WaitForEndOfFrame();
        yield return null;
        UISystem.Instance.RecruitView.InitItemSort(1);
    }


    public void ButtonEvent_SingleBtn(GameObject btn)
    {

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (PlayerData.Instance.IsSoldierFull())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.RECRUIT_LIMIT_EMAX);
            return;
        }
        if (SingleEvent != null)
            SingleEvent(btn);
        DestoryItem();

        Close(null, null);
        //CommonFunction.ClearChild(view.Gobj_Multiple.transform);
        //CommonFunction.ClearChild(view.Gobj_Single.transform);
    }

    public void ButtonEvent_MultipleBtn(GameObject btn)
    {

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (PlayerData.Instance.IsSoldierFull(10))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.RECRUIT_LIMIT_EMAX);
            return;
        }
        if (MultipleEvent != null)
            MultipleEvent(btn);

        DestoryItem();

        Close(null, null);
        //CommonFunction.ClearChild(view.Gobj_Multiple.transform);
        //CommonFunction.ClearChild(view.Gobj_Single.transform);
    }

    public void InitPn(bool isVisible)
    {
        if (this.view == null)
            return;
        view.Btn_BackBtn.gameObject.SetActive(isVisible);
        view.Spt_SingleCurrencyIcon.gameObject.SetActive(isVisible);
        view.Spt_MultipleCurrencyIcon.gameObject.SetActive(isVisible);
        view.Btn_SingleBtn.gameObject.SetActive(isVisible);
        view.Btn_MultipleBtn.gameObject.SetActive(isVisible);
    }

    //public void SetInfo(List<Soldier> soliders, List<fogs.proto.msg.ItemInfo> chips, RecruitData recruitData, UIEventListener.VoidDelegate singleEvent = null, UIEventListener.VoidDelegate multipleEvent = null)
    //{
    //    Go_RoleBaseList.Clear();
    //    SingleEvent = singleEvent;
    //    MultipleEvent = multipleEvent;
    //    int sortOrder = view.UIPanel_RecruitResultView.sortingOrder + 3;
    //    fogs.proto.msg.CostType type = CostType.CT_UseMoney;
    //    Item item = PlayerData.Instance.GetOwnItemByID((uint)recruitData.PropID);

    //    if (item != null && item.num >= recruitData.NeedPropCount)
    //    {
    //        type = CostType.CT_UseProp;
    //    }

    //    if ((soliders != null && soliders.Count == 1) && (chips == null || chips.Count == 0)
    //        || (soliders == null || soliders.Count == 0) && (chips != null && chips.Count == 1))
    //    {
    //        //view.Gobj_SingleName.SetActive(false);
    //        //view.Gobj_MultiName.SetActive(false);
    //        Soldier soldier;
    //        int num;
    //        if (soliders != null && soliders.Count == 1)
    //        {
    //            soldier = soliders[0];
    //            num = -1;
    //        }
    //        else
    //        {
    //            soldier = Soldier.createByID(ConfigManager.Instance.mItemData.GetItemInfoByID(chips[0].id).compound_Id);
    //            num = chips[0].change_num;
    //        }

    //        if (soldier != null)
    //            RoleManager.Instance.CreateSingleRole(soldier.showInfoSoldier, soldier.uId, 1, ERoleType.ertSoldier, 0, 
    //            EHeroGender.ehgNone, EFightCamp.efcNone, EFightType.eftUI, view.Gobj_Single.transform, (vRoleBase) =>
    //            {
    //                vRoleBase.transform.localScale = Vector3.one;
    //                vRoleBase.Get_MainSpine.setSortingOrder(sortOrder);
    //                //TODO:做特效
    //                Go_RoleBaseList.Add(vRoleBase.gameObject);
    //                ShowOnesEffect();
    //                Main.Instance.StartCoroutine(OpenOnesObj(2.0F, soldier.Att,num));
    //            }, false);
    //        else
    //        {
    //            Debug.LogError("recruit chip wrong!!!!!!! chip id=" + chips[0].id);
    //        }
    //    }
    //    else
    //    {
    //        //view.Gobj_MultiName.SetActive(true);
    //        //view.Gobj_SingleName.SetActive(false);
    //        int index = 0;
    //        int indexS = 0;
    //        int indexC = 0;
    //        int seed = 0;

    //        for (int i = 0; i < 10; i++)
    //        {
    //            if ( (soliders == null || indexS >= soliders.Count) )
    //            {
    //                if (chips == null || indexC >= chips.Count)
    //                {
    //                    Debug.LogError(string.Format("10 recruit count wrong! \n" +
    //                                                 " soliders== null :{0} ,indexS ={1},soliders.Count={2} \n" +
    //                                                 " chips == null :{3} ,indexC ={4} ,chips.Count ={5}",
    //                                                 soliders == null, indexS, soliders == null?0:soliders.Count,
    //                                                 chips == null, indexC, chips == null ? 0 : chips.Count));
    //                }
    //                else
    //                {
    //                    tenResultItems[index].InitItem(chips[indexC]);
    //                    ++indexC;
    //                }
    //            }
    //            else if (chips == null || indexC >= chips.Count)
    //            {
    //                //Debug.Log((tenResultItems == null).ToString() 
    //                //    + (tenResultItems[index] == null).ToString()                         
    //                //    + (soliders == null).ToString()
    //                //    + (soliders[indexS] == null).ToString()
    //                //    + (soliders[indexS].Att == null).ToString() 
    //                //    );
    //                tenResultItems[index].InitItem(soliders[indexS].Att);
    //                ++indexS;
    //            }
    //            else
    //            {
    //                seed = UnityEngine.Random.Range(0, 2);
    //                if(seed ==0)
    //                {
    //                    tenResultItems[index].InitItem(soliders[indexS].Att);
    //                    ++indexS;
    //                }
    //                else
    //                {
    //                    tenResultItems[index].InitItem(chips[indexC]);
    //                    ++indexC;
    //                }
    //            }
    //            index++;
    //        }

    //        for (int i = 0; i < soliders.Count; i++)
    //        {
    //            view.Grd_gobj_Multiple.Reposition();
    //        }

    //        ShowTensEffect();
    //        Main.Instance.StartCoroutine(CloesOne(2.250F));

    //    }
    //    Scheduler.Instance.AddTimer(2.7F, false, DisplayBtn);
    //    switch (type)
    //    {
    //        case CostType.CT_UseMoney:
    //            view.Lbl_SingleCurrencyNum.text = recruitData.CurrencyCount.ToString();
    //            view.Lbl_MultipleCurrencyNum.text = (recruitData.CurrencyCount * 9).ToString();
    //            switch (recruitData.CurrencyType)
    //            {
    //                case ECurrencyType.Gold:
    //                    CommonFunction.SetSpriteName(view.Spt_SingleCurrencyIcon, GlobalConst.SpriteName.Gold);
    //                    CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, GlobalConst.SpriteName.Gold);
    //                    break;
    //                case ECurrencyType.Diamond:
    //                    CommonFunction.SetSpriteName(view.Spt_SingleCurrencyIcon, GlobalConst.SpriteName.Diamond);
    //                    CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, GlobalConst.SpriteName.Diamond);
    //                    break;
    //            }
    //            view.Spt_BtnMultipleBtnDis.gameObject.SetActive(true);
    //            SetWidgetSize(view.Spt_MultipleCurrencyIcon, _goldIconSize);
    //            SetWidgetSize(view.Spt_SingleCurrencyIcon, _goldIconSize);
    //            break;
    //        case CostType.CT_UseProp:
    //            ItemInfo iteminfo = ConfigManager.Instance.mItemData.GetItemInfoByID((uint)recruitData.PropID);
    //            view.Lbl_SingleCurrencyNum.text =  string.Format(ConstString.FORMAT_NUMBER_FRACTION, item.num,
    //                                                         recruitData.NeedPropCount);//"X" + recruitData.NeedPropCount;
    //            CommonFunction.SetSpriteName(view.Spt_SingleCurrencyIcon, iteminfo.icon);

    //            if (item.num >= recruitData.NeedPropCount * 10)
    //            {
    //                view.Lbl_MultipleCurrencyNum.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, item.num,
    //                                                         recruitData.NeedPropCount*10);//"X" + recruitData.NeedPropCount * 10;
    //                CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, iteminfo.icon);
    //                view.Spt_BtnMultipleBtnDis.gameObject.SetActive(false);
    //            }
    //            else
    //            {
    //                view.Spt_BtnMultipleBtnDis.gameObject.SetActive(true);
    //                switch (recruitData.CurrencyType)
    //                {
    //                    case ECurrencyType.Gold:
    //                        CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, GlobalConst.SpriteName.Gold);
    //                        break;
    //                    case ECurrencyType.Diamond:
    //                        CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, GlobalConst.SpriteName.Diamond);
    //                        break;
    //                }
    //                view.Lbl_MultipleCurrencyNum.text = (recruitData.CurrencyCount * 9).ToString();
    //            }
    //            SetWidgetSize(view.Spt_MultipleCurrencyIcon, _propIconSize);
    //            SetWidgetSize(view.Spt_SingleCurrencyIcon, _propIconSize);
    //            break;
    //    }


    //}

    public void ShowTenAnim(List<Soldier> soliders, List<fogs.proto.msg.ItemInfo> chips, RecruitData recruitData, UIEventListener.VoidDelegate singleEvent = null, UIEventListener.VoidDelegate multipleEvent = null)
    {
        Go_RoleBaseList.Clear();
        SingleEvent = singleEvent;
        MultipleEvent = multipleEvent;
        int sortOrder = view.UIPanel_RecruitResultView.sortingOrder + 3;
        fogs.proto.msg.CostType type = CostType.CT_UseMoney;
        Item item = PlayerData.Instance.GetOwnItemByID((uint)recruitData.PropID);

        if (item != null && item.num >= recruitData.NeedPropCount)
        {
            type = CostType.CT_UseProp;
        }

        if ((soliders != null && soliders.Count == 1) && (chips == null || chips.Count == 0)
            || (soliders == null || soliders.Count == 0) && (chips != null && chips.Count == 1))
        {
            Soldier soldier;
            int num;
            bool ischip = false;
            if (soliders != null && soliders.Count == 1)
            {
                soldier = soliders[0];
                num = -1;
                ischip = false;
            }
            else
            {
                soldier = Soldier.createByID(ConfigManager.Instance.mItemData.GetItemInfoByID(chips[0].id).compound_Id);
                num = chips[0].change_num;
                ischip = true;
            }

            if (soldier != null)
            {
                view.OnceAnim.SetActive(true);
                RoleManager.Instance.CreateSingleRole(soldier.showInfoSoldier, soldier.uId, 1, ERoleType.ertSoldier, 0,
                  EHeroGender.ehgNone, EFightCamp.efcNone, EFightType.eftUI, view.OnceAnimSingle.transform, (vRoleBase) =>
                  {
                      vRoleBase.transform.localScale = Vector3.one;
                      vRoleBase.Get_MainSpine.setSortingOrder(sortOrder);
                      //TODO:做特效
                      Go_RoleBaseList.Add(vRoleBase.gameObject);
                      Once1 = vRoleBase.gameObject;
                  }, true);

                RoleManager.Instance.CreateSingleRole(soldier.showInfoSoldier, soldier.uId, 1, ERoleType.ertSoldier, 0,
  EHeroGender.ehgNone, EFightCamp.efcNone, EFightType.eftUI, view.OnceResultSingle.transform, (vRoleBase) =>
  {
      vRoleBase.transform.localScale = Vector3.one;
      vRoleBase.Get_MainSpine.setSortingOrder(sortOrder);
      //TODO:做特效
      Go_RoleBaseList.Add(vRoleBase.gameObject);
      Once2 = vRoleBase.gameObject;
  }, true);
                SetOnceSoliderBG(soldier.Att.Name, soldier.Att.quality - 1, ischip, num);
            }
            else
            {
                Debug.LogError("recruit chip wrong!!!!!!! chip id=" + chips[0].id);
            }
            Scheduler.Instance.AddTimer(1.5F, false, DisplayBtn);

        }


        else
        {
            view.TenNameAnim_Objet.SetActive(true);
            int index = 0;
            int indexS = 0;
            int indexC = 0;
            int seed = 0;

            for (int i = 0; i < 10; i++)
            {
                if ((soliders == null || indexS >= soliders.Count))
                {
                    if (chips == null || indexC >= chips.Count)
                    {
                        Debug.LogError(string.Format("10 recruit count wrong! \n" +
                                                     " soliders== null :{0} ,indexS ={1},soliders.Count={2} \n" +
                                                     " chips == null :{3} ,indexC ={4} ,chips.Count ={5}",
                                                     soliders == null, indexS, soliders == null ? 0 : soliders.Count,
                                                     chips == null, indexC, chips == null ? 0 : chips.Count));
                    }
                    else
                    {
                        tenResultItems[index].InitItem(chips[indexC]);
                        tenAnimItems[index].InitItem(chips[indexC]);
                        ++indexC;
                    }
                }
                else if (chips == null || indexC >= chips.Count)
                {
                    //Debug.Log((tenResultItems == null).ToString() 
                    //    + (tenResultItems[index] == null).ToString()                         
                    //    + (soliders == null).ToString()
                    //    + (soliders[indexS] == null).ToString()
                    //    + (soliders[indexS].Att == null).ToString() 
                    //    );
                    tenAnimItems[index].InitItem(soliders[indexS].Att);

                    tenResultItems[index].InitItem(soliders[indexS].Att);
                    ++indexS;
                }
                else
                {
                    seed = UnityEngine.Random.Range(0, 2);
                    if (seed == 0)
                    {
                        tenAnimItems[index].InitItem(soliders[indexS].Att);

                        tenResultItems[index].InitItem(soliders[indexS].Att);
                        ++indexS;
                    }
                    else
                    {
                        tenAnimItems[index].InitItem(chips[indexC]);
                        tenResultItems[index].InitItem(chips[indexC]);
                        ++indexC;
                    }
                }
                index++;
            }

            for (int i = 0; i < soliders.Count; i++)
            {
                //view.Grd_gobj_Multiple.Reposition();
            }
            Scheduler.Instance.AddTimer(2.5F, false, DisplayBtn);

        }
        switch (type)
        {
            case CostType.CT_UseMoney:
                view.Lbl_SingleCurrencyNum.text = recruitData.CurrencyCount.ToString();
                view.Lbl_MultipleCurrencyNum.text = (recruitData.CurrencyCount * 9).ToString();
                switch (recruitData.CurrencyType)
                {
                    case ECurrencyType.Gold:
                        CommonFunction.SetSpriteName(view.Spt_SingleCurrencyIcon, GlobalConst.SpriteName.Gold);
                        CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, GlobalConst.SpriteName.Gold);
                        break;
                    case ECurrencyType.Diamond:
                        CommonFunction.SetSpriteName(view.Spt_SingleCurrencyIcon, GlobalConst.SpriteName.Diamond);
                        CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, GlobalConst.SpriteName.Diamond);
                        break;
                }
                view.Spt_BtnMultipleBtnDis.gameObject.SetActive(true);
                SetWidgetSize(view.Spt_MultipleCurrencyIcon, _goldIconSize);
                SetWidgetSize(view.Spt_SingleCurrencyIcon, _goldIconSize);
                break;
            case CostType.CT_UseProp:
                ItemInfo iteminfo = ConfigManager.Instance.mItemData.GetItemInfoByID((uint)recruitData.PropID);
                view.Lbl_SingleCurrencyNum.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, item.num,
                                                             recruitData.NeedPropCount);//"X" + recruitData.NeedPropCount;
                CommonFunction.SetSpriteName(view.Spt_SingleCurrencyIcon, iteminfo.icon);

                if (item.num >= recruitData.NeedPropCount * 10)
                {
                    view.Lbl_MultipleCurrencyNum.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, item.num,
                                                             recruitData.NeedPropCount * 10);//"X" + recruitData.NeedPropCount * 10;
                    CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, iteminfo.icon);
                    view.Spt_BtnMultipleBtnDis.gameObject.SetActive(false);
                }
                else
                {
                    view.Spt_BtnMultipleBtnDis.gameObject.SetActive(true);
                    switch (recruitData.CurrencyType)
                    {
                        case ECurrencyType.Gold:
                            CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, GlobalConst.SpriteName.Gold);
                            break;
                        case ECurrencyType.Diamond:
                            CommonFunction.SetSpriteName(view.Spt_MultipleCurrencyIcon, GlobalConst.SpriteName.Diamond);
                            break;
                    }
                    view.Lbl_MultipleCurrencyNum.text = (recruitData.CurrencyCount * 9).ToString();
                }
                SetWidgetSize(view.Spt_MultipleCurrencyIcon, _propIconSize);
                SetWidgetSize(view.Spt_SingleCurrencyIcon, _propIconSize);
                break;
        }




    }






    public void DisplayBtn()
    {
        InitPn(true);
    }

    public void SetWidgetSize(UIWidget widget, Vector2 size)
    {
        widget.width = (int)size.x;
        widget.height = (int)size.y;
    }

    public override void Uninitialize()
    {

    }

    public void DestoryItem()
    {
        UnityEngine.Object.Destroy(Once1);
        UnityEngine.Object.Destroy(Once2);
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
    }

    public void BtnEventBinding()
    {

        UIEventListener.Get(view.Btn_BackBtn.gameObject).onClick = ButtonEvent_BackBtn;
        UIEventListener.Get(view.Btn_SingleBtn.gameObject).onClick = ButtonEvent_SingleBtn;
        UIEventListener.Get(view.Btn_MultipleBtn.gameObject).onClick = ButtonEvent_MultipleBtn;
        UIEventListener.Get(view.TenNameAnim_MaskSPT.gameObject).onClick = ButtonEvent_OpenTenResult;
        UIEventListener.Get(view.OnceAnimMask.gameObject).onClick = ButtonEvent_OpenOnceResult;

    }


}
