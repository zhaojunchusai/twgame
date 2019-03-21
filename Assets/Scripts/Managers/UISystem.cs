using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Assets.Script.Common.StateMachine;

public class UISystem : Singleton<UISystem>
{
    /// <summary>
    /// 已经开启的界面（无论是否处于激活状态）
    /// </summary>
    public List<string> openedView = new List<string>();
    /// <summary>
    /// 已经开启并激活的界面
    /// </summary>
    public List<string> openingView = new List<string>();
    /// <summary>
    /// 已经加载完成的View
    /// </summary>
    public Dictionary<string, GameObject> mViewDic = new Dictionary<string, GameObject>();
    private Dictionary<string, UIBase> mViewBaseList = new Dictionary<string, UIBase>();
    public Transform parentTran;
    /// <summary>
    /// 攻略跳转界面
    /// </summary>
    private string WalkthroughJumpView = "";
    //private GameObject _shakeObj;
    //private ShakeGameObj _shakeCamera;
    //=============================================    UI    ===============================================//

    public MessageTwoChooseUIControl MessageTwoChooseBoxUI;
    public GetServerInfoUIControl GetServerInfoUI;
    public MessageBoxUIControl MessageBoxUI;
    public FightViewController FightView;
    public LoginViewController LoginView;
    public HintViewController HintView;
    public GetPathViewControl GetPathView;
    public MainCityViewController MainCityView;
    public HeroAttributeViewController HeroAttView;
    public SoldierAttViewController SoldierAttView;
    public SuitEquipAttViewController SuitEquipAttView;
    public DrowEquipViewController DrowEquipView;
    public SacrificialSystemController SacrificialSystemView;
    public GateViewController GateView;
    public EndlessViewController EndlessView;
    public BackPackViewController BackPackView;
    public GateInfoViewController GateInfoView;
    public ActivitiesViewController ActivitiesView;
    public TopFuncViewController TopFuncView;
    public ExpeditionInfoViewController ExpeditionInfoView;
    public LevelUPViewController LevelUPView;
    public ExpeditionViewController ExpeditionView;
    public RecruitViewController RecruitView;
    public StoreViewController StoreView;
    public MailViewController MailView;
    public MailInfoViewController MailInfoView;
    public MailBatchRecieveViewController MailBatchRecieveView;
    public RecieveResultVertViewController RecieveResultVertView;
    public RecruitResultViewController RecruitResultView;
    public TaskViewController TaskView;
    public AchievementViewController AchievementView;
    public VipRechargeViewController VipRechargeView;
    public SeeDetailViewController SeeDetailView;
    public PVPViewController PvpView;
    public SignViewController SignView;
    public CreateCharacterViewController CreateCharacterView;
    public SystemSettingViewController SystemSettingView;
    public RecruitmentEffectView RecuitmentEffect;
    public OpenChestsEffect OpenChestsEffect;
    public TopBuyCoinEffectView TopBuyCoinEffect;
    public ItemSellViewController ItemSellView;
    public LivenessViewController LivenessView;
    public CastleViewController CastleView;
    public ArtifactIntensifyViewController ArtifactIntensifyView;
    //public SoldierEquipDetailInfoViewController SoldierEquipDetailInfoView;
    public SoldierEquipAdvancedViewController SoldierEquipAdvancedView;
    public SoldierEquipIntensifyViewController SoldierEquipIntensifyView;
    public CheckVersionViewController CheckVersionView;
    public GMViewController GMView;
    public BuySPViewController BuySPView;
    public RegistAccountViewController RegistAccountView;
    public ChooseServerViewController ChooseServerView;
    public BuyCoinViewController BuyCoinView;
    public MenuViewController MenuView;
    public SweepResultViewController SweepResultView;
    public EndlessResultListViewController endlessResultListView;
    public GuideViewController GuideView;
    public SoldierIllInfoViewController SoldierIllInfoView;
    public SoldierIllViewController SoldierIllView;
    public GetSpecialItemViewController GetSpecialItemView;
    public GuildHegemonyViewController GuildHegemonyView;
    public SacrificialSystemEffectViewController SacrificialSystemEffectView;
    public FriendViewController FriendView;
    public FirendAddiewController FriendAdd;
    public FirendApplyViewController FriendApply;
    public FriendInviteViewController FriendInvite;
    public FunctionMenuViewController FunctionMenuView;
    /// <summary>
    /// 异域探险
    /// </summary>
    public ExoticAdvantureViewController ExoticAdvantureView;
    public ExoticAdvantureInfoViewController ExoticAdvantureInfoView;
    /// <summary>
    /// 角色信息
    /// </summary>
    public PlayerInfoViewController PlayerInfoView;
    public TextEffectView SoldierText;//武将甄选暂代
    /// <summary>
    /// 新手引导 对话界面
    /// </summary>
    public DialogueViewController DialogueView;
    public GameActivityViewController GameActivityView;
    public EquipDetailInfoViewController EquipDetailInfoView;
    /// <summary>
    /// 神器选择界面详细信息
    /// </summary>
    public ArtifactDetailViewController ArtifactDetailView;
    public AnnouncementViewController AnnouncementView;
    public ChatViewController ChatView;
    public RankViewController RankView;
    public JoinUnionViewController JoinUnionView;
    public CreateUnionViewController CreateUnionView;
    public UnionViewController UnionView;
    public UnionDonationViewController UnionDonationView;
    public UnionDonationRecordViewController UnionDonationRecordView;
    public UnionHallViewController UnionHallView;
    public UnionApplyViewController UnionApplyView;
    public ChangeUnionIconViewController ChangeUnionIconView;
    public ChangeUnionNameViewController ChangeUnionNameView;
    public ChangeUnionBadgeViewController ChangeUnionBadgeView;
    public UnionSettingViewController UnionSettingView;
    public PrepareBattleViewController PrepareBattleView;    /// <summary>
    /// 奴役系统界面
    /// </summary>
    public PrisonViewController PrisonView;
    public PrisonMarkViewController PrisonMarkView;
    public PrisonRuleViewController PrisonRuleView;
    public ChoosePrisonViewController ChoosePrisonView;
    public UnionRankViewController UnionRankView;
    public UnionReadinessViewController UnionReadinessView;
    public FirstPayViewController FirstPayView;
    public RuleViewController RuleView;
    public UnionHegemonyViewController UnionHegemonyView;
    public RechargeWebMaskController RechargeWebMask;
    public NoviceTaskViewController NoviceTaskView;
    public MallViewController MallView;
    public SimpleChatViewController SimpleChatView;
    public ServerHegemonyInfoViewController ServerHegemonyInfoView;

    public CaptureTerritoryViewController CaptureTerritoryView;
    public CaptureTerritoryInfoViewController CaptureTerritoryInfoView;
    public CaptureTokenViewController CaptureTokenView;
    public AllocateBoxViewController AllocateBoxView;
    public SoldierPropsPackageViewController SoldierPropsPackageView;
    public CaptureTerritoryRuleController CaptureTerritoryRule;
    public LifeSpiritIntensifyViewController LifeSpiritIntensifyView;

    public GameObject UISystemBG;
    public int mCurrentPanelCount = 0;
    public GetEquipEffectViewController GetEquipEffectView;
    public delegate void ViewCloseDelegate(string uiname);
    public event ViewCloseDelegate ViewCloseEvent;
    /// <summary>
    /// 攻略界面
    /// </summary>
    public WalkthroughViewController WalkthroughView;
    public CommentViewController CommentView;
    /// <summary>
    /// 全服霸主界面
    /// </summary>
    public SupermacyViewController SupermacyView;
    /// <summary>
    /// 攻城略地信息界面
    /// </summary>
    public CaptureCityInfoViewController CaptureCityInfoView;
    /// <summary>
    /// 排位赛
    /// </summary>
    public QualifyingViewController QualifyingView;
    public CaptureTerritoryCompleteViewController CaptureTerritoryCompleteView;
    public UnionPrisonViewController UnionPrisonView;
    public UnionPrisonInfoViewController UnionPrisonInfoView;
    public UnionPrisonChooseViewController UnionPrisonChooseView;
    public UnionMemberInfoViewController UnionMemberInfoView;


    public LifeSpiritPackViewController LifeSpiritPackView;
    public LifeSpiritViewController LifeSpiritView;
    public PreyLifeSpiritViewController PreyLifeSpiritView;
    public RecycleViewController RecycleView;
    public AdvanceTipViewController AdvanceTipView;
    public PetSystemViewController PetSystemView;
    public PetChooseViewController PetChooseView;
    /// <summary>
    /// 跨服战场界面
    /// </summary>
    public CrossServerWarViewController CrossServerWarView;

    //private UIBase _CurrentView;
    ///// <summary>
    ///// 当前界面
    ///// </summary>
    //public UIBase CurrentView
    //{
    //    get
    //    {
    //        return _CurrentView;
    //    }
    //}

    public void Initialize()
    {
        GameObject UI = GameObject.Find("UISystem");
        UI.name = "UISystem";
        //_shakeCamera = GameObject.Find("ShakeCamera").GetComponent<ShakeGameObj>();
        parentTran = GameObject.Find("Panel").transform;
    }

    public void Uninitialize()
    {

    }

    /// <summary>
    /// 实例化UI
    /// </summary>
    /// <param name="sUIPerfabName"></param>
    /// <returns></returns>
    public GameObject LoadViewPerfab(string UIName, Vector3 pos)
    {
        GameObject res = null;
        if (mViewDic.TryGetValue(UIName, out res))
            return res;
        GameObject tGo = Resources.Load(ResPath.GetViewPath() + UIName) as GameObject;
        if (tGo)
        {
            if (parentTran == null)
                parentTran = GameObject.Find(GlobalConst.UI.DIR_UI_UISYSTEMPAR).transform;
            res = CommonFunction.InstantiateObject(tGo, parentTran);
            res.name = UIName;
            res.transform.localScale = Vector3.one;
            res.SetActive(false);
            mViewDic.Add(UIName, res);
        }
        return res;
    }


    public GameObject LoadViewPerfab(string UIName)
    {
        GameObject res = null;
        if (mViewDic.TryGetValue(UIName, out res))
            return res;

        GameObject tGo = null;
        tGo = Resources.Load(ResPath.GetViewPath() + UIName) as GameObject;
        if (tGo)
        {
            if (parentTran == null)
                parentTran = GameObject.Find(GlobalConst.UI.DIR_UI_UISYSTEMPAR).transform;

            res = CommonFunction.InstantiateObject(tGo, parentTran);
            res.name = UIName;
            res.transform.localScale = Vector3.one;
            res.SetActive(false);
            mViewDic.Add(UIName, res);
        }
        return res;
    }

    /// <summary>
    /// 对已经打开的View中的Panel层级进行排序
    /// </summary>
    public void ResortViewPanel()
    {
        int viewcount = 1;
        for (int i = 0; i < openingView.Count; i++)
        {
            GameObject res = null;
            int _panelcount = 0;
            if (mViewDic.TryGetValue(openingView[i], out res))
            {
                // hint-50 seedetail-49 dialogue-48 guide-47
                if (res.name == ViewType.DIR_VIEWNAME_HINT || res.name == ViewType.DIR_VIEWNAME_SEEDETAIL
                    || res.name == ViewType.DIR_VIEWNAME_GUIDE || res.name == ViewType.DIR_VIEWNAME_DIALOGUE)
                {
                    continue;
                }
                //- 用于处理如下情况 ：（by mao）
                //- 例：打开商店UI，后会打开topUI，此时topUI层级会置于商店UI所有pan之上，导致商店UI二级面板打开后会被topUI遮住
                //- 类推其他需要显示topUI的面板也会有此问题
                //- 故，再次特殊判断，将topUI的层级改为和上一个UI 的层级一致
                //- 其中topUI的元素depth均从10层开始，避免上层UI的元素遮住它
                //if (res.name == ViewType.DIR_VIEWNAME_TOPFUNC && i > 0 && viewcount > 0 && GetLastUIName() != string.Empty && openingView[openingView.Count - 1] == ViewType.DIR_VIEWNAME_TOPFUNC)
                //{
                //    GameObject lastui = null;
                //    if (mViewDic.TryGetValue(GetLastUIName(), out lastui))
                //    {
                //        UIPanel toppan = res.GetComponent<UIPanel>();
                //        UIPanel pan = lastui.GetComponent<UIPanel>();
                //        toppan.sortingOrder = pan.sortingOrder;
                //        toppan.depth = pan.depth;
                //        continue;
                //    }
                //}
                UIPanel pn = res.GetComponent<UIPanel>();
                if (pn == null)
                    pn = res.AddComponent<UIPanel>();
                pn.depth = viewcount;
                foreach (UIPanel tpn in res.GetComponentsInChildren<UIPanel>(true))
                {
                    if (tpn.gameObject.name != pn.gameObject.name)
                    {
                        tpn.depth = viewcount + tpn.offsetdepth;
                        _panelcount = Mathf.Max(tpn.offsetdepth, _panelcount);
                    }
                }
            }
            viewcount += _panelcount + 1;
        }
        mCurrentPanelCount = viewcount;
    }

    public string GetLastUIName()
    {
        int index = openingView.Count - 1;
        if (index < 0) return string.Empty;
        for (int i = index; i >= 0; i--)
        {
            if (openingView[i] != ViewType.DIR_VIEWNAME_HINT && openingView[i] != ViewType.DIR_VIEWNAME_SEEDETAIL
                   && openingView[i] != ViewType.DIR_VIEWNAME_GUIDE && openingView[i] != ViewType.DIR_VIEWNAME_DIALOGUE && openingView[i] != ViewType.DIR_VIEWNAME_TOPFUNC)
            {
                return openingView[i];
            }
        }
        return string.Empty;
    }

    public void ResortViewOrder()
    {
        int viewOrderCount = 1;
        for (int i = 0; i < openingView.Count; i++)
        {
            GameObject res = null;
            int _ordercount = 0;
            if (mViewDic.TryGetValue(openingView[i], out res))
            {
                if (res.name == ViewType.DIR_VIEWNAME_HINT || res.name == ViewType.DIR_VIEWNAME_SEEDETAIL
                    || res.name == ViewType.DIR_VIEWNAME_GUIDE || res.name == ViewType.DIR_VIEWNAME_DIALOGUE)
                {
                    continue;
                }
                UIPanel pn = res.GetComponent<UIPanel>();
                if (pn == null)
                    pn = res.AddComponent<UIPanel>();
                pn.depth = viewOrderCount;
                pn.sortingOrder = viewOrderCount;

                UIPanel[] resPanels = res.GetComponentsInChildren<UIPanel>(true);
                if (resPanels == null) continue;
                for (int index = 0; index < resPanels.Length; index++)
                {
                    UIPanel tpn = resPanels[index];
                    if (tpn.gameObject.name != pn.gameObject.name)
                    {
                        tpn.sortingOrder = pn.sortingOrder;
                    }
                }
                OffestSortingOrder[] resOrders = res.GetComponentsInChildren<OffestSortingOrder>(true);
                if (resOrders == null) continue;
                for (int index = 0; index < resOrders.Length; index++)
                {
                    OffestSortingOrder tobj = resOrders[index];
                    if (tobj.gameObject.name != pn.gameObject.name)
                    {
                        switch (tobj.nguiLayer)
                        {
                            case OffestSortingOrder.NGUILayerEnum.UI:
                                {
                                    UIPanel cpn = tobj.gameObject.GetComponent<UIPanel>();
                                    if (cpn != null)
                                    {
                                        cpn.sortingOrder = viewOrderCount + tobj.offestOrder;
                                        if (tobj.offestOrder > _ordercount)
                                        {
                                            _ordercount = tobj.offestOrder;
                                        }
                                    }
                                }
                                break;
                            case OffestSortingOrder.NGUILayerEnum.Role:
                                {
                                    int minOrder = 0;
                                    SkeletonAnimation[] role = tobj.GetComponentsInChildren<SkeletonAnimation>(true);
                                    if (role != null && role.Length > 0)
                                    {
                                        int leng = role.Length;
                                        for (int j = 0; j < leng; j++)    //取得最小偏移值
                                        {
                                            SkeletonAnimation skelet = role[j];
                                            if (j == 0)
                                            {
                                                minOrder = skelet.renderer.sortingOrder;
                                            }
                                            else if (skelet.renderer.sortingOrder <= minOrder)
                                            {
                                                minOrder = skelet.renderer.sortingOrder;
                                            }
                                        }
                                        for (int j = 0; j < leng; j++)
                                        {
                                            SkeletonAnimation skelet = role[j];
                                            int offestOrder = skelet.renderer.sortingOrder - minOrder;
                                            skelet.renderer.sortingOrder = viewOrderCount + tobj.offestOrder + offestOrder;
                                            if (((tobj.offestOrder + offestOrder)) > _ordercount)
                                            {
                                                _ordercount = tobj.offestOrder + offestOrder;
                                            }
                                            //if (tobj.offestOrder > _ordercount)
                                            //{
                                            //    _ordercount = tobj.offestOrder;
                                            //}
                                        }
                                    }
                                }
                                break;
                            case OffestSortingOrder.NGUILayerEnum.Effect:
                                {
                                    int childMinOrder = 0;
                                    ParticleSystem[] childParticles = tobj.GetComponentsInChildren<ParticleSystem>(true);
                                    if (childParticles != null && childParticles.Length > 0)
                                    {
                                        int leng = childParticles.Length;
                                        for (int j = 0; j < leng; j++)    //取得最小偏移值
                                        {
                                            ParticleSystem child = childParticles[j];
                                            if (j == 0)
                                            {
                                                childMinOrder = child.renderer.sortingOrder;
                                            }
                                            else if (child.renderer.sortingOrder <= childMinOrder)
                                            {
                                                childMinOrder = child.renderer.sortingOrder;
                                            }
                                        }
                                        for (int j = 0; j < leng; j++)
                                        {
                                            ParticleSystem child = childParticles[j];
                                            int offestOrder = child.renderer.sortingOrder - childMinOrder;
                                            child.renderer.sortingOrder = viewOrderCount + tobj.offestOrder + offestOrder;
                                            if ((tobj.offestOrder + offestOrder) > _ordercount)
                                            {
                                                _ordercount = tobj.offestOrder + offestOrder;
                                            }
                                            //if (tobj.offestOrder > _ordercount)
                                            //{
                                            //    _ordercount = tobj.offestOrder;
                                            //}
                                        }
                                    }
                                    int selfMinOrder = 0;
                                    ParticleSystem[] selfParticles = tobj.GetComponents<ParticleSystem>();
                                    if (selfParticles != null && selfParticles.Length > 0)
                                    {
                                        int leng = selfParticles.Length;
                                        for (int j = 0; j < leng; j++)    //取得最小偏移值
                                        {
                                            ParticleSystem self = selfParticles[j];
                                            if (j == 0)
                                            {
                                                selfMinOrder = self.renderer.sortingOrder;
                                            }
                                            else if (self.renderer.sortingOrder <= childMinOrder)
                                            {
                                                selfMinOrder = self.renderer.sortingOrder;
                                            }
                                        }
                                        for (int j = 0; j < leng; j++)
                                        {
                                            ParticleSystem self = selfParticles[j];
                                            int offestOrder = self.renderer.sortingOrder - selfMinOrder;
                                            self.renderer.sortingOrder = viewOrderCount + tobj.offestOrder + offestOrder;
                                            if ((tobj.offestOrder + offestOrder) > _ordercount)
                                            {
                                                _ordercount = tobj.offestOrder + offestOrder;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            viewOrderCount += _ordercount + 1;
        }
        mCurrentPanelCount = viewOrderCount;
    }

    /// <summary>
    /// 显示一个View   并关闭指定界面
    /// </summary>
    /// <param name="vUIName">需要显示的View的名字</param>
    /// <param name="type">显示类型</param>
    /// <param name="closes">需要关闭的界面</param>
    public bool ShowGameUI(string vUIName)
    {
        bool result = false;
        GameObject vUIObj;

        if (vUIName != ViewType.DIR_VIEWNAME_CHECKVERSIONVIEW && vUIName != ViewType.DIR_VIEWNAME_HINT)
        {
            GameObject res = null;
            if (mViewDic.TryGetValue(vUIName, out res))
            {
                vUIObj = res;
            }
            else
            {
                GameObject go = ResourceLoadManager.Instance.LoadView(vUIName);
                if (go)
                {
                    if (parentTran == null)
                        parentTran = GameObject.Find(GlobalConst.UI.DIR_UI_UISYSTEMPAR).transform;
                    res = CommonFunction.InstantiateObject(go, parentTran);
                    res.name = vUIName;
                    res.transform.localScale = Vector3.one;
                    res.SetActive(false);
                    mViewDic.Add(vUIName, res);
                    vUIObj = res;
                }
                else
                {
                    vUIObj = null;
                }
            }
        }
        else
        {
            vUIObj = LoadViewPerfab(vUIName);
        }

        if (!vUIObj)
        {
            return result;
        }
        vUIObj.SetActive(true);
        if (!openingView.Contains(vUIName))
        {
            openingView.Add(vUIName);
        }
        else
        {
            //先移除再添加 以保证其界面打开的有序性
            openingView.Remove(vUIName);
            openingView.Add(vUIName);
        }
        if (!openedView.Contains(vUIName))
        {
            openedView.Add(vUIName);
        }
        else
        {
            //先移除再添加 以保证其界面打开的有序性
            openedView.Remove(vUIName);
            openedView.Add(vUIName);
        }
        return InitializeCurrentView(vUIName);
    }

    private bool InitializeCurrentView(string vUIName)
    {
        bool result = true;
        UIBase _CurrentView = new UIBase();
        switch (vUIName)
        {
            case ViewType.DIR_VIEWNAME_COMMON:
                {
                    if (HintView == null)
                        HintView = new HintViewController();
                    _CurrentView = HintView;
                }
                break;
            case ViewType.DIR_VIEWNAME_FIGHT:
                {
                    if (FightView == null)
                        FightView = new FightViewController();
                    _CurrentView = FightView;
                }
                break;
            case ViewType.DIR_VIEWNAME_LOGIN:
                {
                    if (LoginView == null)
                        LoginView = new LoginViewController();
                    _CurrentView = LoginView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CREATEROLE:
                {
                    if (CreateCharacterView == null)
                    {
                        CreateCharacterView = new CreateCharacterViewController();
                    }
                    _CurrentView = CreateCharacterView;
                }
                break;
            case ViewType.DIR_VIEWNAME_MAINCITY:
                {
                    if (MainCityView == null)
                    {
                        MainCityView = new MainCityViewController();
                    }
                    _CurrentView = MainCityView;
                }
                break;
            case ViewType.DIR_VIEWNAME_HEROATT:
                {
                    if (HeroAttView == null)
                        HeroAttView = new HeroAttributeViewController();
                    _CurrentView = HeroAttView;
                }
                break;
            case ViewType.DIR_VIEW_GATEQUIPEFFECT:
                {
                    if (GetEquipEffectView == null)
                        GetEquipEffectView = new GetEquipEffectViewController();
                    _CurrentView = GetEquipEffectView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SOLDIERATT:
                {
                    if (SoldierAttView == null)
                        SoldierAttView = new SoldierAttViewController();
                    _CurrentView = SoldierAttView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SUITEQUIPATT:
                {
                    if (SuitEquipAttView == null)
                        SuitEquipAttView = new SuitEquipAttViewController();
                    _CurrentView = SuitEquipAttView;
                }
                break;
            case ViewType.DIR_VIEW_DROWEQUIPVIEW:
                {
                    if (DrowEquipView == null)
                        DrowEquipView = new DrowEquipViewController();
                    _CurrentView = DrowEquipView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SACRIFICIAL:
                {
                    if (SacrificialSystemView == null)
                        SacrificialSystemView = new SacrificialSystemController();
                    _CurrentView = SacrificialSystemView;
                }
                break;
            case ViewType.DIR_VIEW_SACRIFICIALEFFECT:
                {
                    if (SacrificialSystemEffectView == null)
                        SacrificialSystemEffectView = new SacrificialSystemEffectViewController();
                    _CurrentView = SacrificialSystemEffectView;
                }
                break;
            case ViewType.DIR_VIEWNAME_GATE:
                {
                    if (GateView == null)
                        GateView = new GateViewController();
                    _CurrentView = GateView;
                }
                break;
            case ViewType.DIR_VIEWNAME_MENU:
                {
                    if (MenuView == null)
                        MenuView = new MenuViewController();
                    _CurrentView = MenuView;
                }
                break;

            case ViewType.DIR_VIEWNAME_BACKPACK:
                {
                    if (BackPackView == null)
                        BackPackView = new BackPackViewController();
                    _CurrentView = BackPackView;
                }
                break;
            case ViewType.DIR_VIEWNAME_GATEINFO:
                {
                    if (GateInfoView == null)
                        GateInfoView = new GateInfoViewController();
                    _CurrentView = GateInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_TOPBUYCOINEFFECTVIEW:
                {
                    if (TopBuyCoinEffect == null)
                        TopBuyCoinEffect = new TopBuyCoinEffectView();
                    _CurrentView = TopBuyCoinEffect;
                }
                break;
            case ViewType.DIR_VIEWNAME_LIVENESS:
                {
                    if (LivenessView == null)
                        LivenessView = new LivenessViewController();
                    _CurrentView = LivenessView;
                }
                break;
            case ViewType.DIR_VIEWNAME_GETPATH:
                {
                    if (GetPathView == null)
                        GetPathView = new GetPathViewControl();
                    _CurrentView = GetPathView;
                }
                break;
            case ViewType.DIR_VIEWNAME_ACTIVITIES:
                {
                    if (ActivitiesView == null)
                        ActivitiesView = new ActivitiesViewController();
                    _CurrentView = ActivitiesView;

                }
                break;
            case ViewType.DIR_VIEWNAME_ENDLESS:
                {
                    if (EndlessView == null)
                        EndlessView = new EndlessViewController();
                    _CurrentView = EndlessView;
                }
                break;
            case ViewType.DIR_VIEWNAME_TOPFUNC:
                {
                    if (TopFuncView == null)
                        TopFuncView = new TopFuncViewController();
                    _CurrentView = TopFuncView;
                }
                break;
            case ViewType.DIR_VIEWNAME_EXPEDITIONINFO:
                {
                    if (ExpeditionInfoView == null)
                        ExpeditionInfoView = new ExpeditionInfoViewController();
                    _CurrentView = ExpeditionInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_LEVELUP:
                {
                    if (LevelUPView == null)
                        LevelUPView = new LevelUPViewController();
                    _CurrentView = LevelUPView;

                }
                break;
            case ViewType.DIR_VIEWNAME_EXPEDITION:
                {
                    if (ExpeditionView == null)
                        ExpeditionView = new ExpeditionViewController();
                    _CurrentView = ExpeditionView;
                }
                break;
            case ViewType.DIR_VIEWNAME_RECRUITVIEW:
                if (RecruitView == null)
                    RecruitView = new RecruitViewController();
                _CurrentView = RecruitView;
                break;
            case ViewType.DIR_VIEWNAME_STORE:
                if (StoreView == null)
                    StoreView = new StoreViewController();
                _CurrentView = StoreView;
                break;
            case ViewType.DIR_VIEWNAME_MAILVIEW:
                if (MailView == null)
                    MailView = new MailViewController();
                _CurrentView = MailView;
                break;
            case ViewType.DIR_VIEWNAME_MAILINFOVIEW:
                if (MailInfoView == null)
                    MailInfoView = new MailInfoViewController();
                _CurrentView = MailInfoView;
                break;
            case ViewType.DIR_VIEWNAME_MAILBATCHRECIEVEVIEW:
                if (MailBatchRecieveView == null)
                    MailBatchRecieveView = new MailBatchRecieveViewController();
                _CurrentView = MailBatchRecieveView;
                break;
            case ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW:
                if (RecieveResultVertView == null)
                    RecieveResultVertView = new RecieveResultVertViewController();
                _CurrentView = RecieveResultVertView;
                break;
            case ViewType.DIR_VIEWNAME_RECRUITRESULTVIEW:
                if (RecruitResultView == null)
                    RecruitResultView = new RecruitResultViewController();
                _CurrentView = RecruitResultView;
                break;

            case ViewType.DIR_VIEWNAME_TASKVIEW:
                if (TaskView == null)
                    TaskView = new TaskViewController();
                _CurrentView = TaskView;
                break;
            case ViewType.DIR_VIEWNAME_ACHIEVEMENT:
                if (AchievementView == null)
                    AchievementView = new AchievementViewController();
                _CurrentView = AchievementView;
                break;
            case ViewType.DIR_VIEWNAME_RECHARGE:
                if (VipRechargeView == null)
                    VipRechargeView = new VipRechargeViewController();
                _CurrentView = VipRechargeView;
                break;
            case ViewType.DIR_VIEWNAME_SEEDETAIL:
                if (SeeDetailView == null)
                    SeeDetailView = new SeeDetailViewController();
                _CurrentView = SeeDetailView;
                break;
            case ViewType.DIR_VIEWNAME_PVPVIEW:
                {
                    if (PvpView == null)
                    {
                        PvpView = new PVPViewController();
                    }
                    _CurrentView = PvpView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SIGNVIEW:
                {
                    if (SignView == null)
                    {
                        SignView = new SignViewController();
                    }
                    _CurrentView = SignView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SYSTEMSETTINGVIEW:
                {
                    if (SystemSettingView == null)
                    {
                        SystemSettingView = new SystemSettingViewController();
                    }
                    _CurrentView = SystemSettingView;
                }
                break;
            case ViewType.DIR_VIEWNAME_RECRUITMENTEFFECTVIEW:
                {
                    if (RecuitmentEffect == null)
                    {
                        RecuitmentEffect = new RecruitmentEffectView();
                    }
                    _CurrentView = RecuitmentEffect;
                }
                break;
            case ViewType.DIR_VIEWNAME_OPENCHESTSEFFECT:
                {
                    if (OpenChestsEffect == null)
                    {
                        OpenChestsEffect = new OpenChestsEffect();
                    }
                    _CurrentView = OpenChestsEffect;
                }
                break;
            case ViewType.DIR_VIEWNAME_EXCHANGEGOLD:
                {

                }
                break;
            case ViewType.DIR_VIEWNAME_ITEMSELL:
                {
                    if (ItemSellView == null)
                    {
                        ItemSellView = new ItemSellViewController();
                    }
                    _CurrentView = ItemSellView;

                } break;
            case ViewType.DIR_VIEWNAME_CASTLEVIEW:
                {
                    if (CastleView == null)
                        CastleView = new CastleViewController();
                    _CurrentView = CastleView;
                }
                break;
            case ViewType.DIR_VIEWNAME_ARTIFACTINTENSIFY:
                {
                    if (ArtifactIntensifyView == null)
                    {
                        ArtifactIntensifyView = new ArtifactIntensifyViewController();
                    }
                    _CurrentView = ArtifactIntensifyView;
                }
                break;
            //case ViewType.DIR_VIEWNAME_SOLDIEREQUIPDETAILINFOVIEW:
            //    {
            //        if (SoldierEquipDetailInfoView == null)
            //        {
            //            SoldierEquipDetailInfoView = new SoldierEquipDetailInfoViewController();
            //        }
            //        _CurrentView = SoldierEquipDetailInfoView;
            //    }
            //    break;
            case ViewType.DIR_VIEW_SOLDIEREQUIPADVANCED:
                {
                    if (SoldierEquipAdvancedView == null)
                    {
                        SoldierEquipAdvancedView = new SoldierEquipAdvancedViewController();
                    }
                    _CurrentView = SoldierEquipAdvancedView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SOLDIERILLINFO:
                {
                    if (SoldierIllInfoView == null)
                    {
                        SoldierIllInfoView = new SoldierIllInfoViewController();
                    }
                    _CurrentView = SoldierIllInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SOLDIERILLVIEW:
                {
                    if (SoldierIllView == null)
                    {
                        SoldierIllView = new SoldierIllViewController();
                    }
                    _CurrentView = SoldierIllView;
                }
                break;
            case ViewType.DIR_VIEW_SOLDIEREQUIPINTENSIFY:
                {
                    if (SoldierEquipIntensifyView == null)
                    {
                        SoldierEquipIntensifyView = new SoldierEquipIntensifyViewController();
                    }
                    _CurrentView = SoldierEquipIntensifyView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CHECKVERSIONVIEW:
                if (CheckVersionView == null)
                    CheckVersionView = new CheckVersionViewController();
                _CurrentView = CheckVersionView;
                break;
            case ViewType.DIR_VIEWNAME_GMVIEW:
                if (GMView == null)
                    GMView = new GMViewController();
                _CurrentView = GMView;
                break;
            case ViewType.DIR_VIEWNAME_BUY_SP_VIEW:
                if (BuySPView == null)
                    BuySPView = new BuySPViewController();
                _CurrentView = BuySPView;
                break;
            case ViewType.DIR_VIEWNAME_REGIST_ACCOUNT_VIEW:
                if (RegistAccountView == null)
                    RegistAccountView = new RegistAccountViewController();
                _CurrentView = RegistAccountView;
                break;
            case ViewType.DIR_VIEWNAME_CHOOSE_SERVER_VIEW:
                if (ChooseServerView == null)
                    ChooseServerView = new ChooseServerViewController();
                _CurrentView = ChooseServerView;
                break;
            case ViewType.DIR_VIEWNAME_BUY_COIN_VIEW:
                if (BuyCoinView == null)
                    BuyCoinView = new BuyCoinViewController();
                _CurrentView = BuyCoinView;
                break;
            case ViewType.DIR_VIEWNAME_SWEEPRESULT:
                {
                    if (SweepResultView == null)
                        SweepResultView = new SweepResultViewController();
                    _CurrentView = SweepResultView;
                }
                break;
            case ViewType.DIR_VIEWNAME_ENDLESSRESULTLIST:
                {
                    if (endlessResultListView == null)
                        endlessResultListView = new EndlessResultListViewController();
                    _CurrentView = endlessResultListView;
                }
                break;
            case ViewType.DIR_VIEWNAME_GUIDE:
                {
                    if (GuideView == null)
                        GuideView = new GuideViewController();
                    _CurrentView = GuideView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SOLDIERSETLES://武将甄选暂代
                {
                    if (SoldierText == null)
                        SoldierText = new TextEffectView();
                    _CurrentView = SoldierText;
                }
                break;
            case ViewType.DIR_VIEWNAME_PLAYERINFO:
                {
                    if (PlayerInfoView == null)
                    {
                        PlayerInfoView = new PlayerInfoViewController();
                    }
                    _CurrentView = PlayerInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_DIALOGUE:
                {
                    if (DialogueView == null)
                    {
                        DialogueView = new DialogueViewController();
                    }
                    _CurrentView = DialogueView;
                }
                break;
            case ViewType.DIR_VIEWNAME_GAMEACTIVITY:
                {
                    if (GameActivityView == null)
                    {
                        GameActivityView = new GameActivityViewController();
                    }
                    _CurrentView = GameActivityView;
                }
                break;
            case ViewType.DIR_VIEWNAME_EQUIPDETAILINFO:
                {
                    if (EquipDetailInfoView == null)
                    {
                        EquipDetailInfoView = new EquipDetailInfoViewController();
                    }
                    _CurrentView = EquipDetailInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_ARTIFACTDETAILINFO:
                {
                    if (ArtifactDetailView == null)
                        ArtifactDetailView = new ArtifactDetailViewController();
                    _CurrentView = ArtifactDetailView;
                }
                break;
            case ViewType.DIR_VIEWNAME_PRISONVIEW:
                {
                    if (PrisonView == null)
                        PrisonView = new PrisonViewController();
                    _CurrentView = PrisonView;
                }
                break;
            case ViewType.DIR_VIEWNAME_PRISONRULEVIEW:
                {
                    if (PrisonRuleView == null)
                        PrisonRuleView = new PrisonRuleViewController();
                    _CurrentView = PrisonRuleView;
                }
                break;
            case ViewType.DIR_VIEWNAME_PRISONMARKVIEW:
                {
                    if (PrisonMarkView == null)
                        PrisonMarkView = new PrisonMarkViewController();
                    _CurrentView = PrisonMarkView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CHOOSEPRISONVIEW:
                {
                    if (ChoosePrisonView == null)
                        ChoosePrisonView = new ChoosePrisonViewController();
                    _CurrentView = ChoosePrisonView;
                }
                break;
            case ViewType.DIR_VIEWNAME_ANNCOUNCEMENT:
                {
                    if (AnnouncementView == null)
                        AnnouncementView = new AnnouncementViewController();
                    _CurrentView = AnnouncementView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CHATVIEW:
                {
                    if (ChatView == null)
                        ChatView = new ChatViewController();
                    _CurrentView = ChatView;
                }
                break;
            case ViewType.DIR_VIEWNAME_RANKVIEW:
                {
                    if (RankView == null)
                        RankView = new RankViewController();
                    _CurrentView = RankView;
                }
                break;
            case ViewType.DIR_VIEWNAME_JOINUNIONVIEW:
                {
                    if (JoinUnionView == null)
                        JoinUnionView = new JoinUnionViewController();
                    _CurrentView = JoinUnionView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CREATEUNIONVIEW:
                {
                    if (CreateUnionView == null)
                        CreateUnionView = new CreateUnionViewController();
                    _CurrentView = CreateUnionView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONVIEW:
                {
                    if (UnionView == null)
                        UnionView = new UnionViewController();
                    _CurrentView = UnionView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONDONATIONVIEW:
                {
                    if (UnionDonationView == null)
                        UnionDonationView = new UnionDonationViewController();
                    _CurrentView = UnionDonationView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONDONATIONRECVIEW:
                {
                    if (UnionDonationRecordView == null)
                        UnionDonationRecordView = new UnionDonationRecordViewController();
                    _CurrentView = UnionDonationRecordView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONHALLVIEW:
                {
                    if (UnionHallView == null)
                        UnionHallView = new UnionHallViewController();
                    _CurrentView = UnionHallView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONAPPLYVIEW:
                {
                    if (UnionApplyView == null)
                        UnionApplyView = new UnionApplyViewController();
                    _CurrentView = UnionApplyView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CHANGE_UNION_ICON_VIEW:
                {
                    if (ChangeUnionIconView == null)
                        ChangeUnionIconView = new ChangeUnionIconViewController();
                    _CurrentView = ChangeUnionIconView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CHANGE_UNION_NAME_VIEW:
                {
                    if (ChangeUnionNameView == null)
                        ChangeUnionNameView = new ChangeUnionNameViewController();
                    _CurrentView = ChangeUnionNameView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CHANGEUNIONBADGEVIEW:
                {
                    if (ChangeUnionBadgeView == null)
                        ChangeUnionBadgeView = new ChangeUnionBadgeViewController();
                    _CurrentView = ChangeUnionBadgeView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONSETTINGVIEW:
                {
                    if (UnionSettingView == null)
                        UnionSettingView = new UnionSettingViewController();
                    _CurrentView = UnionSettingView;
                }
                break;
            case ViewType.DIR_VIEWNAME_GETSPECIALITEM:
                {
                    if (GetSpecialItemView == null)
                        GetSpecialItemView = new GetSpecialItemViewController();
                    _CurrentView = GetSpecialItemView;
                }
                break;
            case ViewType.DIR_VIEWNAME_GUILDHEGEMONYVIEW:
                {
                    if (GuildHegemonyView == null)
                        GuildHegemonyView = new GuildHegemonyViewController();
                    _CurrentView = GuildHegemonyView;
                }
                break;
            case ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW:
                {
                    if (PrepareBattleView == null)
                        PrepareBattleView = new PrepareBattleViewController();
                    _CurrentView = PrepareBattleView;
                } break;
            case ViewType.DIR_VIEWNAME_EXOTICADVANTUREVIEW:
                {
                    if (ExoticAdvantureView == null)
                        ExoticAdvantureView = new ExoticAdvantureViewController();
                    _CurrentView = ExoticAdvantureView;
                } break;
            case ViewType.DIR_VIEWNAME_EXOTICADVANTUREINFOVIEW:
                {
                    if (ExoticAdvantureInfoView == null)
                        ExoticAdvantureInfoView = new ExoticAdvantureInfoViewController();
                    _CurrentView = ExoticAdvantureInfoView;
                } break;

            case ViewType.DIR_VIEWNAME_UNIONRANKVIEW:
                {
                    if (UnionRankView == null)
                        UnionRankView = new UnionRankViewController();
                    _CurrentView = UnionRankView;
                } break;
            case ViewType.DIR_VIEWNAME_UNIONREADINESSVIEW:
                {
                    if (UnionReadinessView == null)
                        UnionReadinessView = new UnionReadinessViewController();
                    _CurrentView = UnionReadinessView;
                } break;
            case ViewType.DIR_VIEWNAME_FIRSTPAYVIEW:
                {
                    if (FirstPayView == null)
                        FirstPayView = new FirstPayViewController();
                    _CurrentView = FirstPayView;
                } break;
            case ViewType.DIR_VIEWNAME_RULEVIEW:
                {
                    if (RuleView == null)
                    {
                        RuleView = new RuleViewController();
                    }
                    _CurrentView = RuleView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONHEGEMONYVIEW:
                {
                    if (UnionHegemonyView == null)
                        UnionHegemonyView = new UnionHegemonyViewController();
                    _CurrentView = UnionHegemonyView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SIMPLECHAT:
                {
                    if (SimpleChatView == null)
                        SimpleChatView = new SimpleChatViewController();
                    _CurrentView = SimpleChatView;
                }
                break;
            case ViewType.DIR_VIEWNAME_RECHARGEWEBMASK:
                {
                    if (RechargeWebMask == null)
                        RechargeWebMask = new RechargeWebMaskController();
                    _CurrentView = RechargeWebMask;
                }
                break;
            case ViewType.DIR_VIEWNAME_NOVICETASKVIEW:
                {
                    if (NoviceTaskView == null)
                        NoviceTaskView = new NoviceTaskViewController();
                    _CurrentView = NoviceTaskView;
                } break;
            case ViewType.DIR_VIEWNAME_MALLVIEW:
                {
                    if (MallView == null)
                        MallView = new MallViewController();
                    _CurrentView = MallView;
                }
                break;
            case ViewType.DIR_VIEWNAME_FRIENDADDVIEW:
                {
                    if (FriendAdd == null)
                        FriendAdd = new FirendAddiewController();
                    _CurrentView = FriendAdd;
                }
                break;
            case ViewType.DIR_VIEWNAME_FRIENDAPPLYVIEW:
                {
                    if (FriendApply == null)
                        FriendApply = new FirendApplyViewController();
                    _CurrentView = FriendApply;
                }
                break;
            case ViewType.DIR_VIEWNAME_FRIENDINVITEVIEW:
                {
                    if (FriendInvite == null)
                        FriendInvite = new FriendInviteViewController();
                    _CurrentView = FriendInvite;
                }
                break;
            case ViewType.DIR_VIEWNAME_FIRENDVIEW:
                {
                    if (FriendView == null)
                        FriendView = new FriendViewController();
                    _CurrentView = FriendView;
                }
                break;
            case ViewType.DIR_VIEWNAME_FUNCTIONMENUBVIEW:
                {
                    if (FunctionMenuView == null)
                        FunctionMenuView = new FunctionMenuViewController();
                    _CurrentView = FunctionMenuView;
                }
                break;
            case ViewType.DIR_VIEWNAME_WALKTHROUGHVIEW:
                {
                    if (WalkthroughView == null)
                        WalkthroughView = new WalkthroughViewController();
                    _CurrentView = WalkthroughView;
                }
                break;
            case ViewType.DIR_VIEWNAME_COMMENTVIEW:
                {
                    if (CommentView == null)
                        CommentView = new CommentViewController();
                    _CurrentView = CommentView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CAPTURE_TERRITORY:
                {
                    if (CaptureTerritoryView == null)
                        CaptureTerritoryView = new CaptureTerritoryViewController();
                    _CurrentView = CaptureTerritoryView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CAPTURE_TERRITORY_INFO:
                {
                    if (CaptureTerritoryInfoView == null)
                        CaptureTerritoryInfoView = new CaptureTerritoryInfoViewController();
                    _CurrentView = CaptureTerritoryInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CAPTURE_TOKEN:
                {
                    if (CaptureTokenView == null)
                        CaptureTokenView = new CaptureTokenViewController();
                    _CurrentView = CaptureTokenView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SERVERHEGEMONYINFO:
                {
                    if (ServerHegemonyInfoView == null)
                        ServerHegemonyInfoView = new ServerHegemonyInfoViewController();
                    _CurrentView = ServerHegemonyInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME__ALLOCATE_BOX:
                {
                    if (AllocateBoxView == null)
                        AllocateBoxView = new AllocateBoxViewController();
                    _CurrentView = AllocateBoxView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CAPTURE_CITY_INFO:
                {
                    if (CaptureCityInfoView == null)
                        CaptureCityInfoView = new CaptureCityInfoViewController();
                    _CurrentView = CaptureCityInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_SOLDIERPROPSPACKAGEVIEW:
                {
                    if (SoldierPropsPackageView == null)
                        SoldierPropsPackageView = new SoldierPropsPackageViewController();
                    _CurrentView = SoldierPropsPackageView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CAPTURE_TERRITORY_RULE:
                {
                    if (CaptureTerritoryRule == null)
                        CaptureTerritoryRule = new CaptureTerritoryRuleController();
                    _CurrentView = CaptureTerritoryRule;
                }
                break;
            case ViewType.DIR_VIEWNAME_SUPERMACY:
                {
                    if (SupermacyView == null)
                    {
                        SupermacyView = new SupermacyViewController();
                    }
                    _CurrentView = SupermacyView;
                }
                break;
            case ViewType.DIR_VIEWNAME_QUALIFYING:
                {
                    if (QualifyingView == null)
                    {
                        QualifyingView = new QualifyingViewController();
                    }
                    _CurrentView = QualifyingView;
                }
                break;
            case ViewType.DIR_VIEWNAME_CAPTURETERRITORYCAOMPLETE:
                {
                    if (CaptureTerritoryCompleteView == null)
                    {
                        CaptureTerritoryCompleteView = new CaptureTerritoryCompleteViewController();
                    }
                    _CurrentView = CaptureTerritoryCompleteView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONPRISONVIEW:
                {
                    if (this.UnionPrisonView == null)
                    {
                        this.UnionPrisonView = new UnionPrisonViewController();
                    }
                    _CurrentView = this.UnionPrisonView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONPRISONCHOOSEVIEW:
                {
                    if (this.UnionPrisonChooseView == null)
                    {
                        this.UnionPrisonChooseView = new UnionPrisonChooseViewController();
                    }
                    _CurrentView = this.UnionPrisonChooseView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONPRISONINFOVIEW:
                {
                    if (this.UnionPrisonInfoView == null)
                    {
                        this.UnionPrisonInfoView = new UnionPrisonInfoViewController();
                    }
                    _CurrentView = this.UnionPrisonInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_UNIONMEMBERINFO:
                {
                    if (this.UnionMemberInfoView == null)
                    {
                        this.UnionMemberInfoView = new UnionMemberInfoViewController();
                    }
                    _CurrentView = this.UnionMemberInfoView;
                }
                break;
            case ViewType.DIR_VIEWNAME_PREYLIFESPIRITVIEW:
                {
                    if (PreyLifeSpiritView == null)
                    {
                        PreyLifeSpiritView = new PreyLifeSpiritViewController();
                    }
                    _CurrentView = PreyLifeSpiritView;
                }
                break;
            case ViewType.DIR_VIEWNAME_RECYCLE:
                {
                    if (RecycleView == null)
                    {
                        RecycleView = new RecycleViewController();
                    }
                    _CurrentView = RecycleView;
                }
                break;
            case ViewType.DIR_VIEWNAME_ADVANCETIP:
                {
                    if (AdvanceTipView == null)
                    {
                        AdvanceTipView = new AdvanceTipViewController();
                    }
                    _CurrentView = AdvanceTipView;
                }
                break;
            case ViewType.DIR_VIEWNAME_LIFESPIRITPACKVIEW:
                {
                    if (LifeSpiritPackView == null)
                    {
                        LifeSpiritPackView = new LifeSpiritPackViewController();
                    }
                    _CurrentView = LifeSpiritPackView;
                }
                break;
            case ViewType.DIR_VIEWNAME_LIFESPIRITVIW:
                {
                    if (LifeSpiritView == null)
                    {
                        LifeSpiritView = new LifeSpiritViewController();
                    }
                    _CurrentView = LifeSpiritView;
                }
                break;
            case ViewType.DIR_VIEWNAME_LIFESPIRITINTENSIFY:
                {
                    if (LifeSpiritIntensifyView == null)
                    {
                        LifeSpiritIntensifyView = new LifeSpiritIntensifyViewController();
                    }
                    _CurrentView = LifeSpiritIntensifyView;
                }
                break;
            case ViewType.DIR_VIEWNAME_PETSYSTEM:
                {
                    if (PetSystemView == null)
                    {
                        PetSystemView = new PetSystemViewController();
                    }
                    _CurrentView = PetSystemView;
                } break;
            case ViewType.DIR_VIEWNAME_PETCHOOSE:
                {
                    if (PetChooseView == null)
                    {
                        PetChooseView = new PetChooseViewController();
                    }
                    _CurrentView = PetChooseView;
                } break;
            case ViewType.DIR_VIEWNAME_CROSSSERVERWAR:
                {
                    if (CrossServerWarView == null)
                    {
                        CrossServerWarView = new CrossServerWarViewController();
                    }
                    _CurrentView = CrossServerWarView;
                }
                break;
            default:
                {
                    result = false;
                    Debug.LogError("can not find view :" + vUIName);
                    break;
                }
        }
        if (result)
        {
            _CurrentView.UIName = vUIName;
            AddUI(_CurrentView);
            ResortViewOrder();
            ResortViewPanel();
            _CurrentView.Initialize();
            //ShowUIByType(vUIObj, type, vCurrentUI.GetUIBoundary(), vCurrentUI.ShowAnimationDown);
        }
        return result;
    }

    /// <summary>
    /// 关闭指定界面   不建议直接使用
    /// </summary>
    /// <param name="vUIName"></param>
    /// <param name="type"></param>
    public bool CloseGameUI(string vUIName, EShowUIType type = EShowUIType.Default)
    {
        bool result = false;
        if (!openingView.Contains(vUIName)) return result;
        GameObject res;
        if (mViewDic.TryGetValue(vUIName, out res))
        {
            res.SetActive(false);
            result = true;
        }
        openingView.Remove(vUIName);
        UnGameUI(vUIName);
        ResortViewOrder();
        ResortViewPanel();
        RefreshTop();

        if (ViewCloseEvent != null)
        {
            ViewCloseEvent(vUIName);
        }
        if ((!string.IsNullOrEmpty(WalkthroughJumpView)) && (vUIName.Equals(WalkthroughJumpView)))
        {
            ShowGameUI(ViewType.DIR_VIEWNAME_WALKTHROUGHVIEW);
            ClearWalkthroughJumpView();
        }
        return result;
    }

    public bool DelGameUI(string vUIName)
    {
        bool result = false;
        if (openingView.Contains(vUIName))
            openingView.Remove(vUIName);
        UnGameUI(vUIName);
        OnDestroyGameUI(vUIName);
        GameObject res;
        if (mViewDic.TryGetValue(vUIName, out res))
        {
            GameObject.Destroy(res);
            result = true;
        }
        mViewDic.Remove(vUIName);
        ResortViewPanel();
        ResortViewOrder();
        return result;
    }

    /// <summary>
    /// 关闭除HintView外的所有界面
    /// </summary>
    public void CloseAllUI()
    {
        //_CurrentView = null;
        ClearWalkthroughJumpView();
        List<string> openUI = new List<string>();
        openUI.AddRange(openingView);
        for (int i = 0; i < openUI.Count; i++)
        {
            string uiName = openUI[i];
            if (uiName != HintView.UIName)
                CloseGameUI(uiName);
        }
    }

    public void DelAllUI()
    {
        for (int i = 0; i < openedView.Count; i++)
        {
            DelGameUI(openedView[i]);
        }
        openingView.Clear();
        openedView.Clear();
    }

    public void DelAllUIButOne(List<string> uiname)
    {
        for (int i = 0; i < openedView.Count; i++)
        {
            if (!uiname.Contains(openedView[i]))
            {
                DelGameUI(openedView[i]);
            }
        }
        openingView.Clear();
        openedView.Clear();
        openingView.AddRange(uiname);
        openedView.AddRange(uiname);
    }

    private void AddUI(UIBase vUI)
    {
        if (!mViewBaseList.ContainsKey(vUI.UIName))
        {
            mViewBaseList.Add(vUI.UIName, vUI);
        }
        else
        {
            mViewBaseList.Remove(vUI.UIName);
            mViewBaseList.Add(vUI.UIName, vUI);
        }
    }

    private void RemoveUI(string vUIName)
    {
        if (mViewBaseList.ContainsKey(vUIName))
        {
            mViewBaseList.Remove(vUIName);
        }
    }

    /// <summary>
    /// 去初始化
    /// </summary>
    /// <param name="vUIName"></param>
    private bool UnGameUI(string vUIName)
    {
        bool result = false;
        UIBase tbase = null;
        if (mViewBaseList.TryGetValue(vUIName, out tbase))
        {
            tbase.Uninitialize();
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 销毁某一界面
    /// </summary>
    /// <param name="vUIName"></param>
    private bool OnDestroyGameUI(string vUIName)
    {
        bool result = false;
        UIBase tbase = null;
        if (mViewBaseList.TryGetValue(vUIName, out tbase))
        {
            tbase.Destroy();
            result = true;
        }
        return result;
    }

    private void RefreshTop()
    {
        if (openingView.Count > 1 &&
            ((openingView[openingView.Count - 1] == ViewType.DIR_VIEWNAME_TOPFUNC)
            || ((openingView[openingView.Count - 1] == ViewType.DIR_VIEWNAME_GUIDE) && (openingView[openingView.Count - 2] == ViewType.DIR_VIEWNAME_TOPFUNC) && !openingView.Contains(ViewType.DIR_VIEWNAME_RECRUITVIEW))
            ))
        {
            UIBase lastui = null;
            if (mViewBaseList.TryGetValue(GetLastUIName(), out lastui))
            {
                lastui.ReturnTop();
            }
        }
    }

    public void ResortSimpleChat()
    {
        if (openingView.Count > 3)
        {
            if (openingView.Contains(ViewType.DIR_VIEWNAME_MENU) && openingView.Contains(ViewType.DIR_VIEWNAME_SIMPLECHAT))
            {
                int index = openingView.IndexOf(ViewType.DIR_VIEWNAME_MENU);
                int index1 = openingView.IndexOf(ViewType.DIR_VIEWNAME_SIMPLECHAT);
                //Debug.LogError("menu index =" + index + "simplechat index =" + index1);
                //string tip = "";
                //for (int i = 0; i < openingView.Count; i++)
                //{
                //    tip += " " + openingView[i];
                //}
                //Debug.LogError(tip);
                if (index + 1 != index1)
                {
                    openingView.Remove(ViewType.DIR_VIEWNAME_SIMPLECHAT);
                    openingView.Insert(index + 1, ViewType.DIR_VIEWNAME_SIMPLECHAT);
                    ResortViewOrder();
                    ResortViewPanel();
                }
            }
        }
    }

    public void RefreshUIToTop(string sUIName)
    {
        if (openingView.Contains(sUIName))
        {
            openingView.Remove(sUIName);
            openingView.Add(sUIName);
            ResortViewOrder();
            ResortViewPanel();
        }
    }

    /// <summary>
    /// 判定某一界面是否打开
    /// </summary>
    /// <param name="vUIName"></param>
    public bool UIIsOpen(string vUIName)
    {
        bool result = openingView.Contains(vUIName);
        return result;
    }

    public void RecordWalkthroughJumpView(string vTypeView)
    {
        ClearWalkthroughJumpView();
        WalkthroughJumpView = vTypeView;
    }
    public void ClearWalkthroughJumpView()
    {
        WalkthroughJumpView = "";
    }
}