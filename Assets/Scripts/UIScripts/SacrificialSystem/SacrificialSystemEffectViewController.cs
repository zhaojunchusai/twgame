using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

public class SacrificialSystemEffectViewController : UIBase
{
    public SacrificialSystemEffectView View;
    public GameObject Ones_1, Ones_2;
    public List<UISprite> List_CSM_ItemTex =new List<UISprite>();
    public List<UISprite> List_CSM_ItemFrame = new List<UISprite>();
    public List<UISprite> List_CSM_ItemFrameBG = new List<UISprite>();
    public List<UISprite> List_Item_Texture = new List<UISprite>();
    public List<UISprite> List_Item_Frame = new List<UISprite>();
    public List<UISprite> List_Item_FrameBG = new List<UISprite>();

    public List<UILabel> List_CSM_Name = new List<UILabel>();
    public List<UILabel> List_Item_Name = new List<UILabel>();
    public List<UISprite> List_CSM_Type = new List<UISprite>();
    public List<UISprite> List_Item_Type = new List<UISprite>();

    public List<GameObject> ListCSMItemGo = new List<GameObject>();
    public List<GameObject> ListItemGO = new List<GameObject>();
    public List<TweenScale> ListScale = new List<TweenScale>();
    private bool closeShowEffect = false;//关闭特效显示//


    public override void Initialize()
    {
        if (View == null)
        {
            View = new SacrificialSystemEffectView();
            View.Initialize();
        }
        InitList();
        CloseItem();
        InitBtn();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Collect_Up, View._uiRoot.transform.parent.transform));
    }
    public override void Uninitialize()
    {
        List_CSM_Type.Clear();
        List_Item_Type.Clear();
        List_CSM_Name.Clear();
        List_Item_Name.Clear();
        List_CSM_ItemTex.Clear();
        List_CSM_ItemFrame.Clear();
        List_CSM_ItemFrameBG.Clear();
        List_Item_Texture.Clear();
        List_Item_Frame.Clear();
        List_Item_FrameBG.Clear();
        ListCSMItemGo.Clear();
        ListItemGO.Clear();
        ListScale.Clear();
    }
    public override void Destroy()
    {
        base.Destroy();
        View = null;
    }


    public void ButtonEvent_CloseView(GameObject btn)
    {
        UISystem.Instance.SacrificialSystemView.SetEffectActive(true);
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEW_SACRIFICIALEFFECT);
        DestoryItem();
    }
    public void ButtonEvent_CloseChuanSongMeng(GameObject btn)
    {
        if (closeShowEffect)
        {
            ButtonEvent_CloseView(btn);
        }
        else
        {
            if (Ones_1 != null)
            {
                Ones_1.transform.parent = View.GO_Mask_OneRolePos.transform;
                Ones_1.transform.localScale = Vector3.one;
                Ones_1.transform.localPosition = Vector3.zero;
            }
            View.MaskOnes.SetActive(true);
            View.Btn_MaskBG.gameObject.SetActive(false);
            View.Btn_ChuanSongMeng.gameObject.SetActive(false);
            View.Btn_ItemBtn.gameObject.SetActive(true);
        }
    }


    public void ShowChuanSongMengEffect(List<Soldier> data)
    {
        CommonFunction.SetSpriteName(View.EffectBG, GlobalConst.SpriteName.SacrifialEffectBG_Men);

        closeShowEffect = false;
        View.Btn_ChuanSongMeng.gameObject.SetActive(true);
        View.Btn_MaskBG.gameObject.SetActive(true);
        View.Panel_Mask.depth = 30;
        View.ItemGrid.enabled = true;

        UISystem.Instance.SacrificialSystemView.SetEffectActive(false);
        if (data.Count == 1)//单张
        {
            View.Go_ItemOnes.SetActive(true);
            View.GO_CSMOnes.SetActive(true);
            View.Go_Onesone.SetActive(true);
            View.GO_CSMOnes.transform.position = Vector3.zero;
            View.Go_ItemOnes.transform.position = Vector3.zero;
            RoleManager.Instance.CreateSingleRole(data[0].showInfoSoldier, data[0].uId, 1, ERoleType.ertSoldier, 0, EHeroGender.ehgNone,
                EFightCamp.efcNone, EFightType.eftUI, View.GO_CSM_OneRolePos.transform, (vRoleBase) =>
                {
                    vRoleBase.transform.localScale = Vector3.one;
                    Ones_1 = vRoleBase.gameObject;
                }, true);
            View.Go_ItemOne.gameObject.SetActive(true);
            //RoleManager.Instance.CreateSingleRole(data[0].showInfoSoldier, data[0].uId, 1, ERoleType.ertSoldier, 0, EHeroGender.ehgNone,
            //    EFightCamp.efcNone, EFightType.eftUI, View.GO_Mask_OneRolePos.transform, (vRoleBase) =>
            //    {
            //        vRoleBase.transform.localScale = Vector3.one;
            //        Ones_2 = vRoleBase.gameObject;

            //    }, true);

            CommonFunction.SetSpriteName(View.Spt_CSMOnesType, CommonFunction.GetSoldierTypeIcon(data[0].Att.Career));
            CommonFunction.SetSpriteName(View.Spt_CSMOnesQuality, GlobalConst.SpriteName.SoldierRecruitQuality[data[0].Att.quality - 1]);
            CommonFunction.SetSpriteName(View.Spt_ItemOnesType, CommonFunction.GetSoldierTypeIcon(data[0].Att.Career));
            CommonFunction.SetSpriteName(View.Spt_ItemOnesQuality, GlobalConst.SpriteName.SoldierRecruitQuality[data[0].Att.quality - 1]);
            View.Lab_ItemOnesName.text = View.Lab_CSMOnesName.text = data[0].Att.Name;
        }
        else
        {
            for (int i = 0; i < data.Count; i++)
            {
                ListScale[i].ResetToBeginning();
                ListScale[i].PlayForward();
                ListCSMItemGo[i].SetActive(true);
                ListItemGO[i].gameObject.SetActive(true);
                List_CSM_ItemTex[i].gameObject.SetActive(true);
                CommonFunction.SetSpriteName(List_CSM_ItemTex[i], data[i].Att.Icon);
                CommonFunction.SetQualitySprite(List_CSM_ItemFrame[i], data[i].Att.quality, List_CSM_ItemFrameBG[i]);
                List_Item_Name[i].text = List_CSM_Name[i].text = data[i].Att.Name;
                List_Item_Texture[i].gameObject.SetActive(true);
                CommonFunction.SetSpriteName(List_Item_Texture[i], data[i].Att.Icon);
                CommonFunction.SetQualitySprite(List_Item_Frame[i], data[i].Att.quality, List_Item_FrameBG[i]);
                List_Item_Type[i].gameObject.SetActive(true);
                List_CSM_Type[i].gameObject.SetActive(true);
                CommonFunction.SetSpriteName(List_Item_Type[i], CommonFunction.GetSoldierTypeIcon(data[i].Att.Career));
                CommonFunction.SetSpriteName(List_CSM_Type[i], CommonFunction.GetSoldierTypeIcon(data[i].Att.Career));
            }
        }
        View.CSM_Grid.repositionNow = true;
        View.ItemGrid.repositionNow = true;
        View.CSM_Grid.Reposition();
        View.ItemGrid.Reposition();
        Scheduler.Instance.AddTimer(2, false, CloseShowEffect);
    }


    public void InitBtn()
    {
        UIEventListener.Get(View.Btn_ItemBtn.gameObject).onClick = ButtonEvent_CloseView;
        UIEventListener.Get(View.Btn_MaskBG.gameObject).onClick = ButtonEvent_CloseChuanSongMeng;

    }

    public void InitList()
    {
        ListScale.Clear();
        ListScale.Add(View.Sc_CSMItem_5);
        ListScale.Add(View.Sc_CSMItem_6);
        ListScale.Add(View.Sc_CSMItem_7);
        ListScale.Add(View.Sc_CSMItem_8);
        ListScale.Add(View.Sc_CSMItem_9);
        ListScale.Add(View.Sc_CSMItem_0);
        ListScale.Add(View.Sc_CSMItem_1);
        ListScale.Add(View.Sc_CSMItem_2);
        ListScale.Add(View.Sc_CSMItem_3);
        ListScale.Add(View.Sc_CSMItem_4);

        ListCSMItemGo.Clear();
        ListCSMItemGo.Add(View.Go_CSMItem_5);
        ListCSMItemGo.Add(View.Go_CSMItem_6);
        ListCSMItemGo.Add(View.Go_CSMItem_7);
        ListCSMItemGo.Add(View.Go_CSMItem_8);
        ListCSMItemGo.Add(View.Go_CSMItem_9);
        ListCSMItemGo.Add(View.Go_CSMItem_0);
        ListCSMItemGo.Add(View.Go_CSMItem_1);
        ListCSMItemGo.Add(View.Go_CSMItem_2);
        ListCSMItemGo.Add(View.Go_CSMItem_3);
        ListCSMItemGo.Add(View.Go_CSMItem_4);

        ListItemGO.Clear();
        ListItemGO.Add(View.Go_Item_5);
        ListItemGO.Add(View.Go_Item_6);
        ListItemGO.Add(View.Go_Item_7);
        ListItemGO.Add(View.Go_Item_8);
        ListItemGO.Add(View.Go_Item_9);
        ListItemGO.Add(View.Go_Item_0);
        ListItemGO.Add(View.Go_Item_1);
        ListItemGO.Add(View.Go_Item_2);
        ListItemGO.Add(View.Go_Item_3);
        ListItemGO.Add(View.Go_Item_4);

        List_CSM_Type.Clear();
        List_CSM_Type.Add(View.Spt_CSM_Type_5);
        List_CSM_Type.Add(View.Spt_CSM_Type_6);
        List_CSM_Type.Add(View.Spt_CSM_Type_7);
        List_CSM_Type.Add(View.Spt_CSM_Type_8);
        List_CSM_Type.Add(View.Spt_CSM_Type_9);
        List_CSM_Type.Add(View.Spt_CSM_Type_0);
        List_CSM_Type.Add(View.Spt_CSM_Type_1);
        List_CSM_Type.Add(View.Spt_CSM_Type_2);
        List_CSM_Type.Add(View.Spt_CSM_Type_3);
        List_CSM_Type.Add(View.Spt_CSM_Type_4);

        List_Item_Type.Clear();
        List_Item_Type.Add(View.Spt_Item_Type_5);
        List_Item_Type.Add(View.Spt_Item_Type_6);
        List_Item_Type.Add(View.Spt_Item_Type_7);
        List_Item_Type.Add(View.Spt_Item_Type_8);
        List_Item_Type.Add(View.Spt_Item_Type_9);
        List_Item_Type.Add(View.Spt_Item_Type_0);
        List_Item_Type.Add(View.Spt_Item_Type_1);
        List_Item_Type.Add(View.Spt_Item_Type_2);
        List_Item_Type.Add(View.Spt_Item_Type_3);
        List_Item_Type.Add(View.Spt_Item_Type_4);

        List_CSM_Name.Clear();
        List_CSM_Name.Add(View.Lab_CSM_Name_5);
        List_CSM_Name.Add(View.Lab_CSM_Name_6);
        List_CSM_Name.Add(View.Lab_CSM_Name_7);
        List_CSM_Name.Add(View.Lab_CSM_Name_8);
        List_CSM_Name.Add(View.Lab_CSM_Name_9);
        List_CSM_Name.Add(View.Lab_CSM_Name_0);
        List_CSM_Name.Add(View.Lab_CSM_Name_1);
        List_CSM_Name.Add(View.Lab_CSM_Name_2);
        List_CSM_Name.Add(View.Lab_CSM_Name_3);
        List_CSM_Name.Add(View.Lab_CSM_Name_4);

        List_Item_Name.Clear();
        List_Item_Name.Add(View.Lab_Item_Name_5);
        List_Item_Name.Add(View.Lab_Item_Name_6);
        List_Item_Name.Add(View.Lab_Item_Name_7);
        List_Item_Name.Add(View.Lab_Item_Name_8);
        List_Item_Name.Add(View.Lab_Item_Name_9);
        List_Item_Name.Add(View.Lab_Item_Name_0);
        List_Item_Name.Add(View.Lab_Item_Name_1);
        List_Item_Name.Add(View.Lab_Item_Name_2);
        List_Item_Name.Add(View.Lab_Item_Name_3);
        List_Item_Name.Add(View.Lab_Item_Name_4);

        List_CSM_ItemTex.Clear();
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_5);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_6);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_7);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_8);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_9);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_0);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_1);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_2);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_3);
        List_CSM_ItemTex.Add(View.Spt_CSM_ItemTex_4);

        List_CSM_ItemFrame.Clear();
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_5);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_6);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_7);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_8);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_9);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_0);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_1);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_2);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_3);
        List_CSM_ItemFrame.Add(View.Spt_CSM_ItemFrame_4);

        List_Item_Texture.Clear();
        List_Item_Texture.Add(View.Spt_ItemTex_5);
        List_Item_Texture.Add(View.Spt_ItemTex_6);
        List_Item_Texture.Add(View.Spt_ItemTex_7);
        List_Item_Texture.Add(View.Spt_ItemTex_8);
        List_Item_Texture.Add(View.Spt_ItemTex_9);
        List_Item_Texture.Add(View.Spt_ItemTex_0);
        List_Item_Texture.Add(View.Spt_ItemTex_1);
        List_Item_Texture.Add(View.Spt_ItemTex_2);
        List_Item_Texture.Add(View.Spt_ItemTex_3);
        List_Item_Texture.Add(View.Spt_ItemTex_4);

        List_Item_Frame.Clear();
        List_Item_Frame.Add(View.Spt_ItemFrame_5);
        List_Item_Frame.Add(View.Spt_ItemFrame_6);
        List_Item_Frame.Add(View.Spt_ItemFrame_7);
        List_Item_Frame.Add(View.Spt_ItemFrame_8);
        List_Item_Frame.Add(View.Spt_ItemFrame_9);
        List_Item_Frame.Add(View.Spt_ItemFrame_0);
        List_Item_Frame.Add(View.Spt_ItemFrame_1);
        List_Item_Frame.Add(View.Spt_ItemFrame_2);
        List_Item_Frame.Add(View.Spt_ItemFrame_3);
        List_Item_Frame.Add(View.Spt_ItemFrame_4);

        List_Item_FrameBG.Clear();
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_5);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_6);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_7);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_8);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_9);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_0);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_1);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_2);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_3);
        List_Item_FrameBG.Add(View.Spt_ItemFrameBG_4);

        List_CSM_ItemFrameBG.Clear();
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_5);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_6);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_7);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_8);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_9);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_0);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_1);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_2);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_3);
        List_CSM_ItemFrameBG.Add(View.Spt_CSM_ItemFrameBG_4);

    }
    
    public void CloseItem()
    {
        View.Go_ItemOnes.SetActive(false);
        View.GO_CSMOnes.SetActive(false);
        View.MaskOnes.SetActive(false);
        View.Btn_MaskBG.gameObject.SetActive(false);
        View.Btn_ItemBtn.gameObject.SetActive(false);
        View.Btn_ChuanSongMeng.gameObject.SetActive(false);
       for(int i=0;i<10;i++)
       {
           ListCSMItemGo[i].gameObject.SetActive(false);
           ListItemGO[i].gameObject.SetActive(false);
           List_Item_Type[i].gameObject.SetActive(false);
           List_Item_Texture[i].gameObject.SetActive(false);
           List_CSM_ItemTex[i].gameObject.SetActive(false);
       }
       View.Go_Onesone.gameObject.SetActive(false);
       View.Go_ItemOne.gameObject.SetActive(false);
    }

    private void CloseShowEffect()
    {
        closeShowEffect = true;
    }

    private void DestoryItem()
    {
        Object.Destroy(Ones_1);
        Object.Destroy(Ones_2);
    }
}