using UnityEngine;
using System.Collections;
using Assets.Script.Common;
using System.Collections.Generic;
/// <summary>
/// File：OpenChestsEffect.cs
/// 开启宝箱界面特效（远征及活跃宝箱界面）
/// add by 周羽翔
/// </summary>
public class OpenChestsEffect:UIBase 
{
    public GameObject _uiRoot;
    public static string UIName = "OpenChestsEffect";
    public float CloseTime = 5F;  //关闭切换界面时间
    public bool isLiveness = false;
    private UIButton Btn_CloseBtn;
    public bool AutoClose = false;
    public EventDelegate.Callback OnEffectFinish;

    public override void Initialize()
    {
        _uiRoot = _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/OpenChestsEffectView");
        Btn_CloseBtn = _uiRoot.transform.FindChild("BGMask").gameObject.GetComponent<UIButton>();
        InitChestsTex();
        BtnEventBinding();
        AutoClose = false;
        OnEffectFinish = null;
        Scheduler.Instance.AddTimer(CloseTime, false, CloseViewTimer);
       // Main.Instance.StartCoroutine(CloseView(CloseTime));//定时关闭界面并打开奖励领取界面
    }
    private void InitChestsTex()//初始化更新宝箱样式 
    {
    
    
    }
    private void BtnEventBinding()
    {
        UIEventListener.Get(Btn_CloseBtn.gameObject).onClick = ButtonEvent_CloseBtn;
    }
    private void ButtonEvent_CloseBtn(GameObject Btn)//点击遮罩关闭界面并打开奖励领取界面
    {
        if (AutoClose)
            return;

        CloseViewTimer();

    }
    private void CloseViewTimer()   //延时定时关闭界面并打开奖励领取界面
    {
        Scheduler.Instance.RemoveTimer(CloseViewTimer);
        UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_OPENCHESTSEFFECT);

        if(OnEffectFinish != null)
        {
            OnEffectFinish();
            return;
        }

        if (isLiveness)
        {
            UISystem.Instance.ExpeditionView.GetAwardsSuccess(FightRelatedModule.Instance.ExpeditionChestsData.gate_id, FightRelatedModule.Instance.ExpeditionChestsData.drop_item);
        }
        else
        {
            UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(TaskModule.Instance.Livenesslist);
        }
    }
}
