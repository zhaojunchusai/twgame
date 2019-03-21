using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecruitCardRotation : MonoBehaviour {
    public bool isNewbieGuide = true ;//是否新手引导
    public bool isScrollRight = true ;
    public  bool isChooseCountPnOpen = false;
    public bool isMoving = false;
    public int MaxSortOder1 = 18;
    public  int MaxSortOder2 = 19;
    public int MinSortOder1 = 13;
    public  int MinSortOder2 = 14;
    public int MaxDepth = 12;
    public int MinDepth = 8;
    private float MoveTime = 1.10F;
    //private bool IsPress = false;
    private int CardState = 0;
    private float DelayTimes;
    private float MaxInterval = 0.1F;//间隔时间
    private RecruitViewController Recruit;
    private Vector3 _CurrentPos = Vector3.zero;
    private Vector3 _MinScale = new Vector3(0.8F, 0.8F, 0.8F);
    private Vector3 _LeftPorition = new Vector3(-218, -104, 0);
    private Vector3 _RightPorition = new Vector3(241, -104, 0);
    private Vector3 _CenterPorition = new Vector3(0, -212, 0);
    private GameObject Item1;
    private GameObject Item2;
    private GameObject Item3;
    private GameObject _uiRoot;
    public Camera camera;
    public UIPanel Panel_MatchlessPn;
    public SkeletonAnimation MatchlessAnim;
    public UIPanel Panel_MatchlessLab;
    public GameObject MatchlessLight;
    public UIPanel Panel_Brave;
    public SkeletonAnimation BraveAnim;
    public UIPanel Panel_BraveLab;
    public GameObject BraveLight;
    public UIPanel Panel_PutDownRiot;
    public SkeletonAnimation PutDownRiotAnim;
    public UIPanel Panel_PutDownRiotLab;
    public GameObject PutDownRiotLight;
    private Vector3[] posArray;
    private Vector3[] sclArray;
    public int _itemIndex_1 = 1;
    public  int _itemIndex_2 = 0;
    public int _itemIndex_3 = 2;
    public int ItemIndex = 1;
    private GameObject BlueParcails;
    private GameObject PurpleParcails;
    public UIPanel Panel_PDRFreeSp;
    public UIPanel Panel_BraveFreeSp;
    public Vector3 MouseDownClick;
    public Vector3 MouseUpClick;
    void Awake()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/RecruitView");
        Panel_BraveFreeSp = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave/Lab/BraveFreeSp").gameObject.GetComponent<UIPanel>();
        Panel_PDRFreeSp = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot/Lab/PDRFreeSp").gameObject.GetComponent<UIPanel>();
        BlueParcails = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot/ParticleSystem").gameObject;
        PurpleParcails = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless/gobj_lizi/gobj_lizi").gameObject;
        Item1 = _uiRoot.transform.FindChild("Anim/gobj_Brave").gameObject;
        Item2 = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot").gameObject;
        Item3 = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn").gameObject;
        Panel_MatchlessPn = Item2.GetComponent<UIPanel>();
        Panel_Brave = Item1.GetComponent<UIPanel>();
        Panel_PutDownRiot = Item3.GetComponent<UIPanel>();
        camera = GameObject.Find("UISystem/UICamera").gameObject.GetComponent<Camera>();
        MatchlessAnim = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless/per").gameObject.GetComponent<SkeletonAnimation>();
        Panel_MatchlessLab = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless/Lab").gameObject.GetComponent<UIPanel>();
        MatchlessLight = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless/Light").gameObject;
        BraveAnim = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave/Anim1").gameObject.GetComponent<SkeletonAnimation>();
        Panel_BraveLab = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave/Lab").gameObject.GetComponent<UIPanel>();
        BraveLight = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave/Light").gameObject;
        PutDownRiotAnim = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot/Anim").gameObject.GetComponent<SkeletonAnimation>();
        Panel_PutDownRiotLab = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot/Lab").gameObject.GetComponent<UIPanel>();
        PutDownRiotLight = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot/Light").gameObject;
        //SetPutDownRiotSort(true);
        posArray = new Vector3[] { _CenterPorition, _LeftPorition, _RightPorition };
        sclArray = new Vector3[] {Vector3.one ,_MinScale , _MinScale  };
        //Item1.transform.localPosition = posArray[_itemIndex_1];
        //Item2.transform.localPosition = posArray[_itemIndex_2];
        //Item3.transform.localPosition = posArray[_itemIndex_3];
        //SetRotationPos(false);
    }
	void Update()
    {
        if (isChooseCountPnOpen)
            return;//TODO
        if (isMoving)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownClick = camera.ViewportToScreenPoint(Input.mousePosition); 
            MouseUpClick = camera.ViewportToScreenPoint(Input.mousePosition);
        }
     
        if (Input.GetMouseButtonUp(0))
        {
           
                MouseUpClick = camera.ViewportToScreenPoint(Input.mousePosition);
                //Debug.LogError(MouseUpClick.x - MouseDownClick.x);
                if ((MouseUpClick.x - MouseDownClick.x) < -100000)
                {
                    if (isScrollRight)
                    {
                        LeftRotationCard();
                        MouseUpClick = MouseDownClick = Vector3.zero;
                    }
                }
                if ((MouseUpClick.x - MouseDownClick.x) > 100000)
                {
                    if (isNewbieGuide)
                    {
                        RightRotationCard();
                        MouseUpClick = MouseDownClick = Vector3.zero;
                    }
                }
        }
      
	}
    private void LeftRotationCard()
    {
        GuideManager.Instance.CheckTrigger(GuideTrigger.RecruitDrag);
        isMoving = true;
        _itemIndex_1 = (_itemIndex_1 + 1) % 3;
        _itemIndex_2 = (_itemIndex_2 + 1) % 3;
        _itemIndex_3 = (_itemIndex_3 + 1) % 3;
        iTween.MoveTo(Item1,iTween .Hash("time", MoveTime, "position", posArray[_itemIndex_1], "islocal", true));
        iTween.MoveTo(Item2, iTween.Hash("time", MoveTime, "position", posArray[_itemIndex_2], "islocal", true));
        iTween.MoveTo(Item3, iTween.Hash("time", MoveTime, "position", posArray[_itemIndex_3], "islocal", true));
        iTween.ScaleTo(Item1, sclArray[_itemIndex_1], MoveTime);
        iTween.ScaleTo(Item2, sclArray[_itemIndex_2], MoveTime);
        iTween.ScaleTo(Item3, sclArray[_itemIndex_3], MoveTime);
        if (_itemIndex_1 == ItemIndex)
        {
            SetPutDownRiotSort(true);
        }
        if (_itemIndex_2 == ItemIndex)
        {
            SetMatchlessSort(true);
        }
        if (_itemIndex_3 == ItemIndex)
        {
            SetBraveSort(true);

        }
        Invoke("Reset", MoveTime);

    }
    private void RightRotationCard()
    {
        Debug.Log("RightRotationCard ");
        GuideManager.Instance.CheckTrigger(GuideTrigger.RecruitDrag);
        isMoving = true;
        _itemIndex_1 = (_itemIndex_1+2)%3;
        _itemIndex_2 = (_itemIndex_2+2)%3;
        _itemIndex_3 = (_itemIndex_3+2)%3;
        iTween.MoveTo(Item1, iTween.Hash("time", MoveTime, "position", posArray[_itemIndex_1], "islocal", true));
        iTween.MoveTo(Item2, iTween.Hash("time", MoveTime, "position", posArray[_itemIndex_2], "islocal", true));
        iTween.MoveTo(Item3, iTween.Hash("time", MoveTime, "position", posArray[_itemIndex_3], "islocal", true));
        iTween.ScaleTo(Item1, sclArray[_itemIndex_1], MoveTime);
        iTween.ScaleTo(Item2, sclArray[_itemIndex_2], MoveTime);
        iTween.ScaleTo(Item3, sclArray[_itemIndex_3], MoveTime);
        CardState -= 1;
        if (_itemIndex_1 == ItemIndex)
        {
            SetPutDownRiotSort(true);
        }
        if (_itemIndex_2 == ItemIndex)
        {
            SetMatchlessSort(true);
        }
        if (_itemIndex_3 == ItemIndex)
        {
            SetBraveSort(true);
        }
        Invoke("Reset", MoveTime);
    }
    public void SetMatchlessSort(bool IsShow)
    {
        if (IsShow)
        {
            Panel_MatchlessPn.sortingOrder = MaxSortOder1;
            Panel_PutDownRiot.depth = MaxDepth;
            MatchlessAnim.renderer.sortingOrder = MaxSortOder1;
            Panel_MatchlessLab.sortingOrder = MaxSortOder2;
            MatchlessLight.SetActive(true);
            SetBraveSort(false);
            PurpleParcails.SetActive(true);
            SetPutDownRiotSort(false);
        }
        else
        {
            Panel_MatchlessPn.sortingOrder = MinSortOder2;
            Panel_PutDownRiot.depth = MinDepth;
            MatchlessAnim.renderer.sortingOrder = MinSortOder1;
            Panel_MatchlessLab.sortingOrder = MinSortOder2;
            MatchlessLight.SetActive(false);
            PurpleParcails.SetActive(false);
        }
    }
    public void SetBraveSort(bool IsShow)
    {
        if (IsShow)
        {
            Panel_Brave.sortingOrder = MaxSortOder1;
            Panel_Brave.depth = MaxDepth;
            BraveAnim.renderer.sortingOrder = MaxSortOder1;
            Panel_BraveLab.sortingOrder = MaxSortOder2;
            Panel_BraveFreeSp.sortingOrder = MaxSortOder2;
            BraveLight.SetActive(true);
            SetMatchlessSort(false);
            SetPutDownRiotSort(false);
        }
        else
        {
            Panel_Brave.sortingOrder = MinSortOder2;
            Panel_Brave.depth = MinDepth ;
            Panel_BraveFreeSp.sortingOrder = MinSortOder2;

            BraveAnim.renderer.sortingOrder = MinSortOder1;
            Panel_BraveLab.sortingOrder = MinSortOder2;
            BraveLight.SetActive(false);
        }
    }
    public void SetPutDownRiotSort(bool IsShow)
    {
        if (IsShow)
        {
            Panel_MatchlessPn.depth = MaxDepth;
            Panel_PutDownRiot.sortingOrder = MaxSortOder1;
            PutDownRiotAnim.renderer.sortingOrder = MaxSortOder1;
            Panel_PDRFreeSp.sortingOrder = MaxSortOder2;
            Panel_PutDownRiotLab.sortingOrder = MaxSortOder2;
            PutDownRiotLight.SetActive(true);
            SetBraveSort(false);
            BlueParcails.SetActive(true );
            SetMatchlessSort(false);
        }
        else
        {
            Panel_MatchlessPn.depth = MinDepth;
            Panel_PDRFreeSp.sortingOrder = MinSortOder2;
            Panel_PutDownRiot.sortingOrder = MinSortOder2;
            BlueParcails.SetActive(false);
            PutDownRiotAnim.renderer.sortingOrder = MinSortOder1;
            Panel_PutDownRiotLab.sortingOrder = MinSortOder2;
            PutDownRiotLight.SetActive(false);
        }
    }

    public void  Reset()
    {
        ResetMousePos();
        isMoving = false;    
    }

    public void StopMove()
    {
        if (!isMoving)
            return;
        iTween.Stop(Item1);
        iTween.Stop(Item2);
        iTween.Stop(Item3);
        SetRotationPos(1);
    }

    public void ResetMousePos()
    {
        MouseDownClick = camera.ViewportToScreenPoint(Input.mousePosition);
        MouseUpClick = camera.ViewportToScreenPoint(Input.mousePosition);
    }

    public void SetRotationPos(int ItemColor)//1:绿色2：蓝色3：紫色
    {
        isNewbieGuide = isScrollRight = true;
        if (ItemColor ==1)
        {
            _itemIndex_1 = 0;
            _itemIndex_2 = 2;
            _itemIndex_3 = 1;
            SetBraveSort(true);
            ItemIndex = 1;
            //isNewbieGuide = true;
        }
        else if(ItemColor ==2)
        {
            _itemIndex_1 = 1;
            _itemIndex_2 = 0;
            _itemIndex_3 = 2;
            SetPutDownRiotSort(true);
            ItemIndex = 1;
        }
        else if(ItemColor ==3)
        {
            _itemIndex_1 = 2;
            _itemIndex_2 = 1;
            _itemIndex_3 = 0;
            SetMatchlessSort(true);

        }
        
        Item1.transform.localPosition = posArray[_itemIndex_1];
        Item2.transform.localPosition = posArray[_itemIndex_2];
        Item3.transform.localPosition = posArray[_itemIndex_3];
        Item1.transform.localScale = sclArray[_itemIndex_1];
        Item2.transform.localScale = sclArray[_itemIndex_2];
        Item3.transform.localScale = sclArray[_itemIndex_3];
    }
  
}
