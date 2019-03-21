using UnityEngine;
using System.Collections;

public class WalkthroughItem {

    public UISprite Spt_LeftItem_Back;
    public UILabel Lbl_LeftItem_Title;
    public Transform trans;

    private WalkthroughViewController viewController;
    private WalkthroughInfo walkthroughInfo;


    public WalkthroughItem(WalkthroughViewController vViewController, Transform vTrans, WalkthroughInfo vInfo)
    {
        viewController = vViewController;
        trans = vTrans;
        if (trans != null)
        {
            UIEventListener.Get(trans.gameObject).onClick = ButtonEvent_SkillComp;
            Spt_LeftItem_Back = trans.FindChild("LeftItem_Back").GetComponent<UISprite>();
            Lbl_LeftItem_Title = trans.FindChild("LeftItem_Title").GetComponent<UILabel>();
        }
        walkthroughInfo = new WalkthroughInfo();
        walkthroughInfo.CopyTo(vInfo);
        ShowInfo();
    }

    public void Destroy()
    {
        trans = null;
        walkthroughInfo = null;
    }


    public void ButtonEvent_SkillComp(GameObject vBtn)
    {
        if (viewController != null)
        {
            viewController.RefreshSingleWalkthroughInfo(walkthroughInfo, trans);
        }
    }


    private void ShowInfo()
    {
        if (Lbl_LeftItem_Title != null)
        {
            if (walkthroughInfo != null)
            {
                Lbl_LeftItem_Title.text = walkthroughInfo.Name;
            }
            else
            {
                Lbl_LeftItem_Title.text = "";
            }
        }
    }
}