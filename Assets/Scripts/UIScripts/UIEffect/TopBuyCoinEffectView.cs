using UnityEngine;
using System.Collections;
using Assets.Script.Common;
/// <summary>
/// File:TopBuyCoinEffectView.cs
/// 点金手特效
/// add by 周羽翔
/// </summary>
public class TopBuyCoinEffectView : UIBase 
{
    public static string UIName = "TopBuyCoinEffectView";
    public float CloseTime=3.0F;
    public GameObject _uiRoot;
    public Animator Anim;
    public UIButton Btn_CloseBtn;

    public override void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/TopBuyCoinEffectView");
        Anim = _uiRoot.gameObject.GetComponent<Animator>();
        Btn_CloseBtn = _uiRoot.transform.FindChild("BGMask").gameObject.GetComponent<UIButton>();
        //Main.Instance.StartCoroutine(CloseView(CloseTime));
        Scheduler.Instance.AddTimer(CloseTime, false, CloseViewTimer);
        BtnEventBinding();
    }
    private void CloseViewTimer()
    {
        
        UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_TOPBUYCOINEFFECTVIEW);
    }
    private void BtnEventBinding()
    {
        UIEventListener.Get(Btn_CloseBtn.gameObject).onClick = ButtonEvent_CloseBtn;
    }
    private void ButtonEvent_CloseBtn(GameObject Btn)
    {
        UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_TOPBUYCOINEFFECTVIEW);
        Scheduler.Instance.RemoveTimer(CloseViewTimer);
    }
}
