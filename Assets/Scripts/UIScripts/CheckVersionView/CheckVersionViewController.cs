using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Assets.Script.Common;
using Assets.Script.Common.StateMachine;
public class CheckVersionViewController : UIBase
{
    public CheckVersionView view;
    string[] _checking = { ConstString.CHECKVERSION_ING + ".", ConstString.CHECKVERSION_ING + "..", ConstString.CHECKVERSION_ING + "...", ConstString.CHECKVERSION_ING + "...." };
    string[] _readRes = { ConstString.CHECKVERSION_OVER + ".", ConstString.CHECKVERSION_OVER + "..", ConstString.CHECKVERSION_OVER + "...", ConstString.CHECKVERSION_OVER + "...." };
    private int _checkIndex = 0;
    private int _loadResIndex = 0;
    private UpdateFileState _currentState = UpdateFileState.None;
    public override void Initialize()
    {
        if (view == null)
            view = new CheckVersionView();
        view.Initialize();
        SetLogo();
        InitUI();
        SetTips();
        Scheduler.Instance.AddTimer(3, true, SetTips);
        _currentState = UpdateFileState.None;
        BtnEventBinding();
    }

    private void SetLogo()
    {

        switch (GlobalConst.SERVER_TYPE)
        {
            case EServerType.Nei251_8201:
            case EServerType.Nei251_9201:
            case EServerType.Nei251_10201:
            case EServerType.Nei251_18201:
            case EServerType.Nei252_10201:
            case EServerType.Nei249_10201:
            case EServerType.Wai_Sifu:
            case EServerType.Wai_SifuB:
                {
                    CommonFunction.SetSpriteName(view.Spt_Logo, GlobalConst.SpriteName.LogoNormal);
                    view.Spt_Logo.width = 406;
                    view.Spt_Logo.height = 232;
                    view.Spt_Logo.transform.localPosition = new Vector3(-1, 39, 0);
                    break;
                }
            case EServerType.Wai_7725Test:
            case EServerType.Wai_7725IOSTest:
            case EServerType.Wai_7725:
            case EServerType.Wai_JapanCP:
                {
                    CommonFunction.SetSpriteName(view.Spt_Logo, GlobalConst.SpriteName.Logo_P7725);
                    //view.Spt_Logo.MakePixelPerfect();
                    if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725)
                    {
                        view.Spt_Logo.transform.localPosition = new Vector3(-25, 220, 0);
                        view.Spt_Logo.width = 455;
                        view.Spt_Logo.height = 287;
                    }
                    else if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
                    {
                        view.Spt_Logo.transform.localPosition = new Vector3(0, 207, 0);
                    }
                } break;
            case EServerType.Wai_BeiJing:
                {
                    CommonFunction.SetSpriteName(view.Spt_Logo, GlobalConst.SpriteName.Logo_BingLin);
                    view.Spt_Logo.width = 366;
                    view.Spt_Logo.height = 250;
                    view.Spt_Logo.transform.localPosition = new Vector3(-1, 39, 0);
                    break;
                }
        }


        ////if (GlobalConst.PLATFORM == TargetPlatforms.Android_SF)
        ////{
        ////    CommonFunction.SetSpriteName(view.Spt_Logo, GlobalConst.SpriteName.LogoSf);
        ////    view.Spt_Logo.width = 294;
        ////    view.Spt_Logo.height = 266;
        ////}
        ////else
        //{
        //    CommonFunction.SetSpriteName(view.Spt_Logo, GlobalConst.SpriteName.LogoNormal);
        //    //view.Spt_Logo.MakePixelPerfect();
        //    view.Spt_Logo.width = 406;
        //    view.Spt_Logo.height = 232;
        //}
    }
    public void InitUI()
    {
        UpdateVersionManager.Instance.CheckFailure = UpdateFailure;
        UpdateVersionManager.Instance.UpdateCheckState = UpdateState;
        UpdateVersionManager.Instance.UpdateResValue = UpdateResValue;
        UpdateVersionManager.Instance.UpdateResSizeValue = UpdateResSizeValue;
        UpdateVersionManager.Instance.UpdateResError = UodateResError;
        UpdateVersionManager.Instance.UpdateDownResWarn = UpdateDownResWarn;
        view.Gobj_ProgressBar.gameObject.SetActive(false);
        SetChangeState(UpdateFileState.CheckVersion);
    }

    public void UpdateState(System.Object obj)
    {
        UpdateFileState state = (UpdateFileState)obj;
        _currentState = state;
        SetChangeState(state);
        switch (state)
        {
            case UpdateFileState.CheckVersion:
                _checkIndex = 0;
                break;
            case UpdateFileState.UpdateResource:
                view.Gobj_ProgressBar.gameObject.SetActive(true);
                break;
            case UpdateFileState.DownCode:
                view.Gobj_ProgressBar.gameObject.SetActive(true);
                SetHintByDownCode();
                break;
            case UpdateFileState.DownAPK:
                view.Gobj_ProgressBar.gameObject.SetActive(true);
                SetHintByDownAPK();
                break;
            case UpdateFileState.DownCodeOver:
                SetHintByDownCodeOver();
                break;
            case UpdateFileState.Over:
                _loadResIndex = 0;
                Scheduler.Instance.AddTimer(0.5F, true, SetHintByOver);
                Main.Instance.ChangeState(ReadResState.StateName);
                break;
        }

    }

    public void SetChangeState(UpdateFileState state)
    {
        if (state == UpdateFileState.CheckVersion)
            Scheduler.Instance.AddTimer(0.5F, true, SetHintbyStart);
        else
            Scheduler.Instance.RemoveTimer(SetHintbyStart);

        if (state != UpdateFileState.UpdateResource && view.Gobj_ProgressBar.activeSelf && state != UpdateFileState.DownCode && state != UpdateFileState.DownAPK)
            view.Gobj_ProgressBar.SetActive(false);
    }

    public void UpdateResValue(System.Object obj)
    {
        float value = (float)obj;
        view.ProgressBar_Version.value = value;
        //view.Tex_ProgressBarF.fillAmount = value;
        view.Lbl_ProgressLb.text = string.Format(ConstString.DOWNLOADRES_PROGRESS, (value * 100).ToString("0.0"));
    }

    public void UpdateResSizeValue(long obj, long obj2)
    {
        switch (_currentState)
        {
            case UpdateFileState.DownAPK:
                {
                    view.Lbl_Hint.text = string.Format(ConstString.CHECKVERSION_UPDATEAPK, obj, obj2);
                    break;
                }
            case UpdateFileState.DownCode:
                {
                    view.Lbl_Hint.text = string.Format(ConstString.CHECKVERSION_UPDATEAPK, obj, obj2);
                    break;
                }
            default:
                {
                    view.Lbl_Hint.text = string.Format(ConstString.CHECKVERSION_UPDATECODE, obj, obj2);
                    break;
                }
        }

    }

    public void UodateResError(System.Object obj)
    {
        view.Lbl_Hint.text = ConstString.DOWNLOADRES_ERROR;
    }

    public void UpdateDownResWarn(System.Object obj, long size)
    {
        UpdateFileState state = (UpdateFileState)obj;
        switch (state)
        {
            case UpdateFileState.DownCodeWarn:
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.CHECKVERSION_HINTWARN, size), () =>
                {
                    UpdateVersionManager.Instance.SetChangeState(UpdateFileState.DownCodeWarn);
                }, () =>
                {
                    Application.Quit();
                }, ConstString.CHECKVERSION_OK, ConstString.CHECKVERSION_NO);

                break;
            case UpdateFileState.DownResWarn:
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.CHECKVERSION_HINTWARN, size), () =>
                {
                    UpdateVersionManager.Instance.SetChangeState(UpdateFileState.DownResWarn);
                }, () =>
                {
                    Application.Quit();
                }, ConstString.CHECKVERSION_OK, ConstString.CHECKVERSION_NO);
                break;
            case UpdateFileState.DownApkWarn:
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.CHECKVERSION_HINTWARN, size), () =>
                {
                    UpdateVersionManager.Instance.SetChangeState(UpdateFileState.DownApkWarn);
                }, () =>
                {
                    Application.Quit();
                }, ConstString.CHECKVERSION_OK, ConstString.CHECKVERSION_NO);
                break;
        }
    }

    public void UpdateFailure(System.Object obj)
    {
        UpateErrorType state = (UpateErrorType)obj;
        switch (state)
        {
            case UpateErrorType.NotFindResXml:
                SetHintCheckError();
                break;
            case UpateErrorType.NotFindVersionXml:
                SetHintCheckError();
                break;
            case UpateErrorType.DownCodeError:
                SetHintCheckError();
                break;
            case UpateErrorType.VersionXmlWriteError:
                SetHintWriteError();
                break;
        }
    }

    public void SetHintbyStart()
    {
        view.Lbl_Hint.text = _checking[_checkIndex];
        _checkIndex++;
        _checkIndex = _checkIndex % 4;
    }

    public void SetHintByDownCode()
    {
        view.Lbl_Hint.text = ConstString.CHECKVERSION_UPDATEAPK;
    }

    public void SetHintByDownAPK()
    {
        view.Lbl_Hint.text = ConstString.CHECKVERSION_UPDATEAPK;
    }

    public void SetHintByDownCodeOver()
    {
        view.Lbl_Hint.text = ConstString.CHECKVERSION_UPDATECODEOVER;
        Main.Instance.StartCoroutine(RestartGame());
    }

    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1.0F);
#if UNITY_ANDROID
        CallAndroidFunManager.Instance.RestartGame();
#endif
    }

    public void SetHintByOver()
    {
        string loadResTip = _readRes[_loadResIndex];
        int allcount = PreLoadManager.Instance.GetPreAssetList().Count + 1;
        int loadedCount = ResourceLoadManager.Instance.LoadedCount;
        if (loadedCount >= allcount)
        {
            loadedCount = allcount;
        }
        float percent = (float)loadedCount / (float)allcount;
        view.Lbl_Hint.text = string.Format(loadResTip, percent.ToString("p"));
        _loadResIndex++;
        _loadResIndex = _loadResIndex % 4;
    }

    public void SetHintCheckError()
    {
        Scheduler.Instance.RemoveTimer(SetHintbyStart);
        view.Lbl_Hint.text = ConstString.CHECKVERSION_ERROR;
    }

    public void SetHintWriteError()
    {
        Scheduler.Instance.RemoveTimer(SetHintbyStart);
        view.Lbl_Hint.text = ConstString.CHECKVERSION_WARITE_ERROR;
    }

    public void SetTips()
    {
        view.Lbl_TipsLb.text = ConfigManager.Instance.mTipsConfig.GetTipsStr();
    }

    public override void Uninitialize()
    {
        Scheduler.Instance.RemoveTimer(SetTips);
        Scheduler.Instance.RemoveTimer(SetHintByOver);
    }

    public void BtnEventBinding()
    {

    }



}
