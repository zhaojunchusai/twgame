using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelUPViewController : UIBase 
{
    public LevelUPView view;
    private HeroAttributeInfo oldInfo;
    private HeroAttributeInfo newInfo;
    //private Vector3[] _unlockBgPos;
    //private Vector3[] _unlockFuncIconPos;
    private List<UnlockItem> _unlockItem;
    public override void Initialize()
    {
        if (view == null)
            view = new LevelUPView();
        if (_unlockItem == null)
            _unlockItem = new List<UnlockItem>();
        view.Initialize();
        //view.Spt_UnlockTypeSprite.pivot = UIWidget.Pivot.Left;
        //_unlockBgPos = new[] { new Vector3(0, -108, 0), new Vector3(-98, -108, 0) };
        //_unlockFuncIconPos = new [] { new Vector3(-70, -108, 0), new Vector3(-165,-108,0) };
        BtnEventBinding();
        //PlayOpenOffsetRootAnim();
        InitFX();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));

    }
    public void InitFX()
    {
        view.Spt_FX.ResetToBeginning();
        view.Spt_FX.Play();
    }
    public void ButtonEvent_TopCloseButton(GameObject btn)
    {
        CloseLevelUp();
        UISystem.Instance.CloseGameUI(LevelUPView.UIName);
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseLevelUpView);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close , view._uiRoot.transform.parent.transform));
    }

    public void ShowLevelUp(int oldLv,int newLv)
    {
        oldInfo = ConfigManager.Instance.mHeroData.GetHeroAttributeByLV((uint)oldLv);
        newInfo = ConfigManager.Instance.mHeroData.GetHeroAttributeByLV((uint)newLv);

        view.Lbl_LevelNumLabel.text = newLv.ToString();
        view.Lbl_SPRewardsNum.text = newInfo.GiftPhysical.ToString();

        view.Lbl_OldAttackLabel.text = oldInfo.Attack.ToString();
        view.Lbl_OldLeadershipLabel.text = oldInfo.Leadership.ToString();
        view.Lbl_OldLifeLabel.text = oldInfo.HP.ToString();
        view.Lbl_OldSPLimitLabel.text = oldInfo.MaxPhysical.ToString();

        view.Lbl_NewAttackLabel.text = oldInfo.Attack.ToString();
        view.Lbl_NewLeadershipLabel.text = oldInfo.Leadership.ToString();
        view.Lbl_NewLifeLabel.text = oldInfo.HP.ToString();
        view.Lbl_NewSPLimitLabel.text = oldInfo.MaxPhysical.ToString();

        Main.Instance.StartCoroutine(InitLabAnim(0.25f));

        List<OpenLevelData> list = ConfigManager.Instance.mOpenLevelConfig.GetOpenFuncByCharLv(newLv, oldLv);
        
        if(list.Count <=0)
        {
            //view.Spt_UnlockBGSprite.gameObject.SetActive(false);
            //view.Spt_UnlockTypeSprite.gameObject.SetActive(false);
            //view.Spt_UnlockTypeSprite1.gameObject.SetActive(false);
            view.Go_Unlock.SetActive(false);
        }
        else
        {
            view.Go_Unlock.SetActive(true);
            for(int i=0;i<list.Count;i++)
            {
               GameObject go = CommonFunction.InstantiateObject(view.Go_UnlockItem, view.UnlockUIGrid.transform);
               UnlockItem item = go.AddComponent<UnlockItem>();
               go.SetActive(true);
               if(i<3)
               {
                   item.OpenEffect();
               }
               //Debug.LogError("LevelUP:" + "list[i].icon=" + list[i].icon + "          list[i].name=" + list[i].name);
               item.InitItem(list[i].icon, list[i].name);
               _unlockItem.Add(item);
               if (ConfigManager.Instance.mMainCityUnlockConfig.NeedToShowUnlockEffect(list[i].id)
                   && !MainCityModule.Instance.UnlockBuildings.Contains(list[i].id))
               {
                   MainCityModule.Instance.UnlockBuildings.Add(list[i].id);
               }
            }
            MainCityModule.Instance.SendBuildingEffectRecord(true, MainCityModule.Instance.UnlockBuildings);
            view.UnlockUIGrid.Reposition();
            // 告诉服务器我这里有哪些特效要播放
        }
        //else if (list.Count == 1)
        //{
            //view.Spt_UnlockBGSprite.gameObject.SetActive(true);
            //view.Spt_UnlockTypeSprite.gameObject.SetActive(true);
            //view.Spt_UnlockTypeSprite1.gameObject.SetActive(false);
            //view.Spt_UnlockBGSprite.transform.localPosition = _unlockBgPos[0];
            //view.Spt_UnlockTypeSprite.transform.localPosition = _unlockFuncIconPos[0];
            //if (!CommonFunction.SetSpriteName(view.Spt_UnlockTypeSprite, CommonFunction.GetTargetUINameByID(list[0].id, true))) 
            //{
            //    // 这个是暂时这样写的 图片已经出完 需要策划更新配置
            //    view.Spt_UnlockTypeSprite.spriteName = "";
            //}
            //view.Spt_UnlockTypeSprite.MakePixelPerfect();
        //}
        //else
        //{
            //view.Spt_UnlockBGSprite.gameObject.SetActive(true);
            //view.Spt_UnlockTypeSprite.gameObject.SetActive(true);
            //view.Spt_UnlockTypeSprite1.gameObject.SetActive(true);
            //view.Spt_UnlockBGSprite.transform.localPosition = _unlockBgPos[1];
            //view.Spt_UnlockTypeSprite.transform.localPosition = _unlockFuncIconPos[1];
            //if (!CommonFunction.SetSpriteName(view.Spt_UnlockTypeSprite, CommonFunction.GetTargetUINameByID(list[0].id, true)))
            //{
            //    view.Spt_UnlockTypeSprite.spriteName = "";
            //}
            //if (!CommonFunction.SetSpriteName(view.Spt_UnlockTypeSprite1, CommonFunction.GetTargetUINameByID(list[1].id, true)))
            //{
            //    view.Spt_UnlockTypeSprite1.spriteName = "";
            //}
          
            //view.Spt_UnlockTypeSprite.MakePixelPerfect();
            //view.Spt_UnlockTypeSprite1.MakePixelPerfect();
       // }
      
    }
    public void CloseLevelUp()
    {
        if (_unlockItem == null) return;
        for(int i=_unlockItem.Count-1;i>=0;i--)
        {
            UnlockItem item = _unlockItem[i];
            _unlockItem.Remove(item);
            if(item.gameObject != null)
                GameObject.Destroy(item.gameObject);
        }
    }

    public IEnumerator InitLabAnim(float time)
    {
        yield return new WaitForSeconds(time );
        view.Lbl_NewAttackLabel.GetComponent<AddValueItem>().ShowAddValue(newInfo.Attack,oldInfo.Attack);
        view.Lbl_NewLeadershipLabel.GetComponent<AddValueItem>().ShowAddValue(newInfo.Leadership ,oldInfo.Leadership);
        view.Lbl_NewLifeLabel.GetComponent<AddValueItem>().ShowAddValue(newInfo.HP,oldInfo.HP);
        view.Lbl_NewSPLimitLabel.GetComponent<AddValueItem>().ShowAddValue(newInfo.MaxPhysical, oldInfo.MaxPhysical);


    }
    public override void Uninitialize()
    {
        
    }
    public override void Destroy()
    {
        _unlockItem.Clear();
        view = null;
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_TopCloseButton.gameObject).onClick = ButtonEvent_TopCloseButton;
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_TopCloseButton;
    }

    public void UpdateAttribute(Attribute oldAttr, Attribute newAttr)
    {

    }

    public void UpdateUnLockFunction(int level, int vipLevel)
    {

    }
    //界面动画
    //public void PlayOpenOffsetRootAnim()
    //{
    //    view.OffsetRoot_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.OffsetRoot_TScale.Restart();
    //    view.OffsetRoot_TScale.PlayForward();
    //}
}
