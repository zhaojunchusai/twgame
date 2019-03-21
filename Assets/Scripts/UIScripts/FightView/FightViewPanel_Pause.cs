using UnityEngine;
using System.Collections;

public class FightViewPanel_Pause : Singleton<FightViewPanel_Pause>
{
    private FightView fightView;

    public void Initialize(FightView vFightView)
    {
        fightView = vFightView;
        InitPanel();
    }

    public void Uninitialize()
    { }

    public void Destroy()
    {
        fightView = null;
    }

    /// <summary>
    /// 初始化界面显示
    /// </summary>
    public void InitPanel()
    {
        if (fightView == null)
            return;
        fightView.UIPanel_FightPause.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示界面
    /// </summary>
    /// <param name="vFightType"></param>
    /// <param name="vIsBlack"></param>
    public void OpenPanel(EFightType vFightType, bool vIsBlack = false)
    {
        InitPanel();

        if (vFightType == EFightType.eftEndless)
        {
            if (vIsBlack)
            {
                fightView.Btn_Pause_ReStart.gameObject.SetActive(false);
                fightView.Btn_Pause_Exit.gameObject.SetActive(false);
                fightView.Btn_Pause_Keep.gameObject.SetActive(false);
            }
            else
            {
                fightView.Btn_Pause_ReStart.gameObject.SetActive(false);
                fightView.Btn_Pause_Exit.gameObject.SetActive(true);
                fightView.Btn_Pause_Keep.gameObject.SetActive(true);
                fightView.Btn_Pause_Exit.transform.localPosition = new Vector3(-85, 0, 0);
                fightView.Btn_Pause_Keep.transform.localPosition = new Vector3(85, 0, 0);
            }
        }
        else if ((vFightType == EFightType.eftMain) || (vFightType == EFightType.eftActivity) || (vFightType == EFightType.eftExpedition))
        {
                fightView.UIPanel_FightPause.gameObject.SetActive(false);
                fightView.Btn_Pause_ReStart.gameObject.SetActive(true);
                fightView.Btn_Pause_Exit.gameObject.SetActive(true);
                fightView.Btn_Pause_Keep.gameObject.SetActive(true);
                fightView.Btn_Pause_ReStart.transform.localPosition = new Vector3(-170, 0, 0);
                fightView.Btn_Pause_Exit.transform.localPosition = new Vector3(0, 0, 0);
                fightView.Btn_Pause_Keep.transform.localPosition = new Vector3(170, 0, 0);
        }
        else if ((vFightType == EFightType.eftCaptureTerritory) || (vFightType == EFightType.eftCrossServerWar))
        {
            fightView.Btn_Pause_ReStart.gameObject.SetActive(false);
            fightView.Btn_Pause_Exit.gameObject.SetActive(true);
            fightView.Btn_Pause_Keep.gameObject.SetActive(true);
            fightView.Btn_Pause_Exit.transform.localPosition = new Vector3(-85, 0, 0);
            fightView.Btn_Pause_Keep.transform.localPosition = new Vector3(85, 0, 0);
        }
        fightView.Spt_Pause_BG.gameObject.SetActive(!vIsBlack);

        fightView.UIPanel_FightPause.gameObject.SetActive(true);
    }
    /// <summary>
    /// BOSS出场专用
    /// </summary>
    public void OpenPanel()
    {
        InitPanel();
        fightView.Btn_Pause_ReStart.gameObject.SetActive(false);
        fightView.Btn_Pause_Exit.gameObject.SetActive(false);
        fightView.Btn_Pause_Keep.gameObject.SetActive(false);
        fightView.Spt_Pause_BG.gameObject.SetActive(true);
        fightView.UIPanel_FightPause.gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public void ClosePanel()
    {
        fightView.UIPanel_FightPause.gameObject.SetActive(false);
    }

}
