using UnityEngine;
using System.Collections;

public class AnnouncementViewController : UIBase 
{
    public AnnouncementView view;
    private EAnnouncementType _type;
    public override void Initialize()
    {
        if (view == null)
            view = new AnnouncementView();
        view.Initialize();
        BtnEventBinding();
    }

    public void ButtonEvent_SureButton(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ANNCOUNCEMENT);
        if (_type == EAnnouncementType.Login)
        {
            UISystem.Instance.LoginView.CheckNeedToAutoLogin();
        }
    }

    public override void Uninitialize()
    {

    }

    public void OnUpdateAnnouncement(string title, string content,EAnnouncementType type = EAnnouncementType.Login)
    {
        view.Lbl_TitleLabel.text = title;
        view.Lbl_ContentLabel.text = CommonFunction.ReplaceEscapeChar(content);
        view.ScrView_ScrollView.ResetPosition();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_SureButton.gameObject).onClick = ButtonEvent_SureButton;
        //UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_SureButton;
    }


}
