using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class GetSpecialItemViewController : UIBase 
{
    private bool IsUPStar = false;
    public GetSpecialItemView view;
    private GameObject Go_RoleBase;
    private List<CommonItemData> itemList = new List<CommonItemData>();
    private List<GameObject> starList = new List<GameObject>();
    private List<TweenScale> starTweenList = new List<TweenScale>();

    public override void Initialize()
    {
        if (view == null)
            view = new GetSpecialItemView();
        view.Initialize();
        BtnEventBinding();
        IsUPStar = false;
        InitColor(false);
        InitStarAnimList();
        view.Star.SetActive(false);
        view.Gobj_BGMask.SetActive(false);
        view.Lab_Title.text = ConstString.RECRUIT_TITLE;

    }
    public void InitColor(bool IsUPStar)
    {
        if (IsUPStar)
        {
            CommonFunction.UpdateTextureGray(view.OnesOne_1, true);
            CommonFunction.UpdateTextureGray(view.OnesOne_2, true);
            CommonFunction.UpdateTextureGray(view.OnesOne_3, true);
            CommonFunction.UpdateTextureGray(view.OnesOne_4, true);

        }
        else
        {
            CommonFunction.UpdateTextureGray(view.OnesOne_1, false );
            CommonFunction.UpdateTextureGray(view.OnesOne_2, false );
            CommonFunction.UpdateTextureGray(view.OnesOne_3, false );
            CommonFunction.UpdateTextureGray(view.OnesOne_4, false );

        }


    }
    public IEnumerator GetSoldierStar(float time,Soldier _soldier)
    {
        yield return new WaitForSeconds(time);
        for(int i=0;i<starList .Count;i++)
        {
            int _star = _soldier.Att.Star + 1;
            if (_star > 6) _star = 6;
            if (i<_star)
            {
                starList[i].gameObject.SetActive(true);
                starTweenList[i].ResetToBeginning();
                starTweenList[i].PlayForward();

            }
            else
            {
                starList[i].gameObject.SetActive(false);
            }
        }
        view.Star.SetActive(true);
        view.StarGrid.Reposition();
    }
    public void  InitStarAnimList()
    {
        starList.Clear();
        starTweenList.Clear();
        starList.Add(view.Star_1);
        starTweenList.Add(view.TweenStar_1);
        starList.Add(view.Star_2);
        starTweenList.Add(view.TweenStar_2);
        starList.Add(view.Star_3);
        starTweenList.Add(view.TweenStar_3);
        starList.Add(view.Star_4);
        starTweenList.Add(view.TweenStar_4);
        starList.Add(view.Star_5);
        starTweenList.Add(view.TweenStar_5);
        starList.Add(view.Star_6);
        starTweenList.Add(view.TweenStar_6);
        for (int i=0;i<starTweenList .Count;i++)
        {
            starTweenList[i].transform .localScale = Vector3.zero;
            starList[i].SetActive(false);
        }
    }
    public void SetInfo(List<CommonItemData> datas)
    {
        GuideManager.Instance.CheckTrigger(GuideTrigger.StartGetSpecialProp);
                itemList = datas;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_OPENCHESTSEFFECT);
        //UISystem.Instance.OpenChestsEffect.AutoClose = true;
        UISystem.Instance.OpenChestsEffect.OnEffectFinish = BoxEffectFinish;
    }

    public void SetInfo(CommonItemData datas)
    {
        GuideManager.Instance.CheckTrigger(GuideTrigger.StartGetSpecialProp);
        itemList = new List<CommonItemData>() { datas };
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_OPENCHESTSEFFECT);
        UISystem.Instance.OpenChestsEffect.AutoClose = true;
        UISystem.Instance.OpenChestsEffect.OnEffectFinish = BoxEffectFinish;

    }

    private void BoxEffectFinish()
    {
        UISystem.Instance.CloseGameUI(GetSpecialItemView.UIName);
        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(itemList, true, ConstString.RECEIVE);
    }

    public void SetInfo(Soldier data)
    {
        GuideManager.Instance.CheckTrigger(GuideTrigger.StartGetSpecialSoldier);
        int sortOrder = view.UIPanel_GetSpecialItemView.sortingOrder + 3;
        view.Gobj_GetSoldier.SetActive(true);
        RoleManager.Instance.CreateSingleRole(data.showInfoSoldier, data.uId, 1, ERoleType.ertSoldier, 0, 
            EHeroGender.ehgNone, EFightCamp.efcNone, EFightType.eftUI, view.Gobj_Single.transform, (vRoleBase) =>
            {
                vRoleBase.transform.localScale = Vector3.one;
                vRoleBase.Get_MainSpine.setSortingOrder(sortOrder);
                Go_RoleBase = vRoleBase.gameObject;
                ShowOnesEffect();
                Main.Instance.StartCoroutine(OpenOnesObj(2.150F, data.Att, -1));
            }, false);
    }
    public void SetInfoSoldierUPStar(Soldier data)
    {
        if (data == null) return;
        IsUPStar = true;
        InitColor(true);
        view.Lab_Title.text = ConstString.RECRUIT_UPSTAR;
        view.Gobj_BGMask.SetActive(true);
        GuideManager.Instance.CheckTrigger(GuideTrigger.StartGetSpecialSoldier);
        int sortOrder = view.UIPanel_GetSpecialItemView.sortingOrder + 3;
        view.Gobj_GetSoldier.SetActive(true);
        RoleManager.Instance.CreateSingleRole(data.showInfoSoldier, data.uId, 1, ERoleType.ertSoldier, 0,
            EHeroGender.ehgNone, EFightCamp.efcNone, EFightType.eftUI, view.Gobj_Single.transform, (vRoleBase) =>
            {
                vRoleBase.transform.localScale = Vector3.one;
                vRoleBase.Get_MainSpine.setSortingOrder(sortOrder);
                Go_RoleBase = vRoleBase.gameObject;
                ShowOnesEffect();
                Main.Instance.StartCoroutine(OpenOnesObj(2.15F, data.Att, -1));
            }, false);
        Main.Instance.StartCoroutine(GetSoldierStar(3.0F, data));
    }
    private IEnumerator OpenOnesObj(float time, SoldierAttributeInfo info, int num)
    {
        yield return new WaitForSeconds(time);
        if (Go_RoleBase)
        {
            Go_RoleBase.SetActive(true);
            SetSingleSoldierInfo(info, num);
            if (info.quality == 6) view.OnesOne_R.SetActive(true);
            if (info.quality == 5) view.OnesOne_Y.SetActive(true);
            if (info.quality == 4) view.OnesOne_Z.SetActive(true);
        }
        if (IsUPStar)
        {
            yield return new WaitForSeconds(2F);
            UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ButtonEvent_BackBtn;
        }
        else
        {
            yield return new WaitForSeconds(2F);
            UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ButtonEvent_BackBtn;
        }
    }
    private void SetSingleSoldierInfo(SoldierAttributeInfo info, int num)
    {
        if (info == null)
        {
            Debug.LogError("SetSingleSoldierInfo == null");
        }

        bool ischip = num >= 0;
        view.Gobj_SingleName.SetActive(true);
        view.Lbl_SingleName.text = info.Name;
        view.Spt_SingleType.gameObject.SetActive(true);
        view.Lbl_SingleNum.gameObject.SetActive(ischip);
        view.Spt_SingleChip.gameObject.SetActive(ischip);
        view.Lbl_SingleNum.text = string.Format(ConstString.FORMAT_NUM_X, num);
        CommonFunction.SetSpriteName(view.Spt_SingleType, CommonFunction.GetSoldierTypeIcon(info.Career));
        CommonFunction.SetSpriteName(view.Spt_SingleQuality, GlobalConst.SpriteName.SoldierRecruitQuality[info.quality - 1]);
        switch (info.quality)
        {
            case 4:
                {
                    CommonFunction.SetSpriteName(view.Spt_SingleLight, GlobalConst.SpriteName.RecruitSoldierQualityBgPurple);
                    view.Gobj_SingleEffPurple.SetActive(true);
                    view.Gobj_SingleEffYellow.SetActive(false);
                    view.Gobj_SingleEffRed.SetActive(false);
                    break;
                }
            case 5:
                {
                    CommonFunction.SetSpriteName(view.Spt_SingleLight, GlobalConst.SpriteName.RecruitSoldierQualityBgYellow);
                    view.Gobj_SingleEffPurple.SetActive(false);
                    view.Gobj_SingleEffYellow.SetActive(true);
                    view.Gobj_SingleEffRed.SetActive(false);
                    break;
                }
            case 6:
                {
                    CommonFunction.SetSpriteName(view.Spt_SingleLight, GlobalConst.SpriteName.RecruitSoldierQualityBgRed);
                    view.Gobj_SingleEffPurple.SetActive(false);
                    view.Gobj_SingleEffYellow.SetActive(false);
                    view.Gobj_SingleEffRed.SetActive(true);
                    break;
                }
            default:
                {
                    view.Gobj_SingleEffPurple.SetActive(false);
                    view.Gobj_SingleEffYellow.SetActive(false);
                    view.Gobj_SingleEffRed.SetActive(false);
                    view.Spt_SingleLight.gameObject.SetActive(false);
                    break;
                }
        }
    }

    private void ShowOnesEffect()
    {
        view.EffectGo.gameObject.SetActive(true);
        view.OnesEffect.gameObject.SetActive(true);
        view.Talpha_OnesSpt1.Restart();
        view.Tscale_OnesSpt1.Restart();
        view.Trotate_Oneslizi.Restart();
        view.Tscale_Oneslizi.Restart();
        view.Talpha_OnesSpt2.Restart();
        view.TScale_OnesSpt2.Restart();
        view.TScale_OnesSpt2.PlayForward();
        view.Talpha_OnesSpt1.PlayForward();
        view.Tscale_OnesSpt1.PlayForward();
        view.Trotate_Oneslizi.PlayForward();
        view.Tscale_Oneslizi.PlayForward();
        view.Talpha_OnesSpt2.PlayForward();
        view.SingleGobj.ResetToBeginning();
        view.SingleGobj.PlayForward();
        view.SingleItem.ResetToBeginning();
        view.SingleItem.PlayForward();
    }

    private void ButtonEvent_BackBtn(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(GetSpecialItemView.UIName);
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseGetSpecialSoldier);
    }

    public override void Uninitialize()
    {
        if (view == null || view.Gobj_SingleName == null) return;
        view.Gobj_SingleName.SetActive(false);
        view.Gobj_GetSoldier.SetActive(false);
        view.EffectGo.gameObject.SetActive(false);
        view.OnesEffect.gameObject.SetActive(false);

        if (Go_RoleBase != null)
            UnityEngine.Object.Destroy(Go_RoleBase);
        Go_RoleBase = null;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = null;
    }

    private void BtnEventBinding()
    {
    }

}
