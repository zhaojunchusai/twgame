using UnityEngine;
using System;
using System.Collections;

public class BuyCoinResultItem : MonoBehaviour
{
    [HideInInspector]public UIWidget UIWidget_BuyCoinResultItem;
    [HideInInspector]public TweenAlpha TAlpha_BuyCoinResultItem;
    [HideInInspector]public TweenPosition TPos_BuyCoinResultItem;
    [HideInInspector]public UISprite Spt_CritSprite;
    [HideInInspector]public UILabel Lbl_CritLabel;
    [HideInInspector]public UILabel Lbl_GetCoinLabel;
    [HideInInspector]public UISprite Spt_GetCoinSprite;

    public float duration = 0.35f;//暂留时间
    public bool isStay = false; //在屏幕上暂留着
    private bool _isInit = false;
    public void Initialize()
    {
        if (_isInit)
            return;
        
        UIWidget_BuyCoinResultItem = GetComponent<UIWidget>();
        TAlpha_BuyCoinResultItem = GetComponent<TweenAlpha>();
        TPos_BuyCoinResultItem = GetComponent<TweenPosition>();
        Spt_CritSprite = transform.FindChild("CritSprite").gameObject.GetComponent<UISprite>();
        Lbl_CritLabel = transform.FindChild("CritLabel").gameObject.GetComponent<UILabel>();
        Lbl_GetCoinLabel = transform.FindChild("GetCoinLabel").gameObject.GetComponent<UILabel>();
        Spt_GetCoinSprite = transform.FindChild("GetCoinSprite").gameObject.GetComponent<UISprite>();
        SetLabelValues();
        InitTweenComps();
        TPos_BuyCoinResultItem.AddOnFinished(DestroyItem);
        _isInit = true;
    }

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if (!isStay)
            return;
        if (duration > 0f)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            isStay = false;
            duration = 0f;
            ShowAnim();
        }
    }

    public void SetLabelValues()
    {
        Lbl_CritLabel.text = "0";
        Lbl_GetCoinLabel.text = "0";
    }

    public void InitTweenComps()
    {
        TAlpha_BuyCoinResultItem.Restart();
        TPos_BuyCoinResultItem.Restart();
        TAlpha_BuyCoinResultItem.enabled = false;
        TPos_BuyCoinResultItem.enabled = false;
    }

    public void Uninitialize()
    {

    }

    public void UpdateExchangeResult(int coin, int critNum,int depth)
    {
        if (!_isInit)
            Initialize();
        UIWidget_BuyCoinResultItem.depth += depth;
        if (critNum <= 1)
        {
            Lbl_CritLabel.text = "";
            Spt_CritSprite.gameObject.SetActive(false);
        }
        else
        {
            Lbl_CritLabel.text = critNum.ToString();
            Spt_CritSprite.gameObject.SetActive(true);
        }
        Lbl_GetCoinLabel.text = CommonFunction.GetTenThousandUnit(coin);
        isStay = true;
        duration = 0.5f;
    }

    private void ShowAnim()
    {
        TAlpha_BuyCoinResultItem.enabled = true;
        TPos_BuyCoinResultItem.enabled = true;
    }

    private void StopAnim()
    {
        InitTweenComps();
        this.transform.localPosition = Vector3.zero;
        UIWidget_BuyCoinResultItem.alpha = 1f;
    }

    public void MoveUp()
    {
        if (isStay)
            duration = 0f;
    }

    public void DestroyItem()
    {
        this.gameObject.SetActive(false);
        InitTweenComps();
        Destroy(this.gameObject);
    }


}
