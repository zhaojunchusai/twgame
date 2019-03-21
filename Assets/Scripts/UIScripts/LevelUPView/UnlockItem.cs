using UnityEngine;
using System.Collections;

public class UnlockItem : MonoBehaviour 
{
    public UISprite  Icon;
    public UISprite Frame;
    public UILabel  TypeLabel;
    public GameObject Effect;
    public GameObject SptEffect;
    void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        Icon = this.transform.FindChild("Icon").gameObject.GetComponent<UISprite>();
        Frame = this.transform.FindChild("Frame").gameObject.GetComponent<UISprite>();
        TypeLabel = this.transform.FindChild("TypeLabel").gameObject.GetComponent<UILabel>();
        SptEffect = this.transform.FindChild("SPTEffect").gameObject;
        Effect = this.transform.FindChild("Effect").gameObject;
        Effect.gameObject.SetActive(false); SptEffect.gameObject.SetActive(false);
    }
    public void InitItem(string icon,string typeLabel)
    {
        TypeLabel.text = typeLabel;
        CommonFunction.SetSpriteName(Icon, icon);
    }
    public void OpenEffect()
    {
        SptEffect.SetActive(true);
        //Effect.gameObject.SetActive(true);
    }
}
