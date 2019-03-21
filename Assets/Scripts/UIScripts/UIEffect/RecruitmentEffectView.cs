using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// File:RecruitmentEffectView.cs
/// 招募界面特效
/// add by 周羽翔
/// </summary>
public class RecruitmentEffectView : UIBase
{
    public static string UIName = "RecruitmentEffectView";
    public bool Ones = false;
    public float StateTime = 0.75F;
    public float CloseTime = 3.0F;
    public float TrailFlayTime = 0.75F;
    private GameObject _uiRoot;
    private GameObject Go_Trail;
    private UIButton Btn_CloseBtn;
    private Animator  UIAnim;
    private UISpriteAnimation Location_01;
    private UISpriteAnimation Location_02;
    private UISpriteAnimation Location_03;
    private UISpriteAnimation Location_04;
    private UISpriteAnimation Location_05;
    private UISpriteAnimation Location_06;
    private UISpriteAnimation Location_07;
    private UISpriteAnimation Location_08;
    private UISpriteAnimation Location_09;
    private UISpriteAnimation Location_00;
    private RecruitResultViewController _controller;
    private List<UISpriteAnimation> LocationList = new List<UISpriteAnimation>();
    public override void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/RecruitmentEffectView");
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_TRAILBLUE, (GameObject gb) => { Go_Trail = gb; });// Resources.Load(GlobalConst.DIR_EFFECT_TRAILBLUE) as GameObject;
        UIAnim = _uiRoot.gameObject.GetComponent<Animator >();
        Btn_CloseBtn = _uiRoot.transform.FindChild("BGMask").gameObject.GetComponent<UIButton>();
        Location_00 = _uiRoot.transform.FindChild("Location/location0").gameObject.GetComponent<UISpriteAnimation>();
        Location_01 = _uiRoot.transform.FindChild("Location/location1").gameObject.GetComponent<UISpriteAnimation>();
        Location_02 = _uiRoot.transform.FindChild("Location/location2").gameObject.GetComponent<UISpriteAnimation>();
        Location_03 = _uiRoot.transform.FindChild("Location/location3").gameObject.GetComponent<UISpriteAnimation>();
        Location_04 = _uiRoot.transform.FindChild("Location/location4").gameObject.GetComponent<UISpriteAnimation>();
        Location_05 = _uiRoot.transform.FindChild("Location/location5").gameObject.GetComponent<UISpriteAnimation>();
        Location_06 = _uiRoot.transform.FindChild("Location/location6").gameObject.GetComponent<UISpriteAnimation>();
        Location_07 = _uiRoot.transform.FindChild("Location/location7").gameObject.GetComponent<UISpriteAnimation>();
        Location_08 = _uiRoot.transform.FindChild("Location/location8").gameObject.GetComponent<UISpriteAnimation>();
        Location_09 = _uiRoot.transform.FindChild("Location/location9").gameObject.GetComponent<UISpriteAnimation>();
        AddLocationList(); 
        BtnEventBinding();
        _controller = UISystem.Instance.RecruitResultView;
  
    }
   
    public void PlayEffect()
    {
        UIAnim.Play(0);
        if (_controller.Go_RoleBaseList.Count == 1)
        {//招募一次特效
            Main.Instance.StartCoroutine(PlaySingleEffect(StateTime));
            Main.Instance.StartCoroutine(CloseView(CloseTime));
        }
        else
        {//招募十次
            Main.Instance.StartCoroutine(PlayMultipleEffect_Flay(StateTime));
            Main.Instance.StartCoroutine(PlayMultipleEffect_Effect(StateTime + TrailFlayTime-0.25F));
            Main.Instance.StartCoroutine(CloseView(CloseTime));
        }
    }
    private IEnumerator PlaySingleEffect(float time)
    {
        yield return new WaitForSeconds(time);
        _controller.Go_RoleBaseList[0].SetActive(true);
    }
    private IEnumerator PlayMultipleEffect_Flay(float time)//播放飞行特效
    {
      
        yield return new WaitForSeconds(time);
     
            for (int i = 0; i < LocationList.Count; i++)
            {
                GameObject go = ShowEffectManager.Instance.ShowEffect(Go_Trail, _uiRoot.transform);
                if(go)
                    iTween.MoveTo(go, LocationList[i].transform.position, TrailFlayTime);
            }
       
    }
    private IEnumerator PlayMultipleEffect_Effect(float time)//播放刷新特效并显示英雄
    {
        yield return new WaitForSeconds(time);
        if (LocationList.Count != _controller.Go_RoleBaseList.Count)
        {
            DebugUtil.LogError("服务器数据异常");
            yield break;
        }
        // int count = Mathf.Min(LocationList.Count, _controller.Go_RoleBaseList.Count);
        // i < count
        for (int i = 0; i < LocationList.Count; i++)
        {
            LocationList[i].gameObject.SetActive(true);
            LocationList[i].ResetToBeginning();
            LocationList[i].Play();
            _controller.Go_RoleBaseList[i].SetActive(true);
        }
    }
    private IEnumerator CloseView(float time)//关闭特效界面
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < _controller.Go_RoleBaseList.Count; i++)
        {
            if (_controller.Go_RoleBaseList[i]!=null)
           _controller. Go_RoleBaseList[i].SetActive(true);
        }
        UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_RECRUITMENTEFFECTVIEW);
    }
    private void AddLocationList()
    {
        LocationList.Clear();
        LocationList.Add(Location_00);
        LocationList.Add(Location_01);
        LocationList.Add(Location_02);
        LocationList.Add(Location_03);
        LocationList.Add(Location_04);
        LocationList.Add(Location_05);
        LocationList.Add(Location_06);
        LocationList.Add(Location_07);
        LocationList.Add(Location_08);
        LocationList.Add(Location_09);
        for(int i=0;i<LocationList.Count;i++ )
        {
            LocationList[i].gameObject.SetActive(false);
           
        }
    }
    private void BtnEventBinding()
    {
        UIEventListener.Get(Btn_CloseBtn.gameObject).onClick = ButtonEvent_CloseBtn;
    }
    private void ButtonEvent_CloseBtn(GameObject Btn)//点击遮罩关闭界面并显示隐藏人物
    {
        //for (int i = 0; i < _controller.Go_RoleBaseList.Count; i++)
        //{
          //  _controller.Go_RoleBaseList[i].SetActive(true);
       // }
       // UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_RECRUITMENTEFFECTVIEW);

    }
}
