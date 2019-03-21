using UnityEngine;
using System.Collections;

namespace Assets.Script.Common.StateMachine
{
    public class MainCityState : State
    {

        public static string StateName { get { return typeof(MainCityState).Name; } }

        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            OpenUIs();
            //SystemSettingModule.Instance.
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public void OpenUIs()
        {
            //PlaySoundManager.PlaySound(GlobalConst.NAME_MUSIC_MAIN);//播放音乐
            ResourceLoadManager.Instance.ReleaseRequestBundle();
            if (!UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_MAINCITY))
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_MAINCITY);
            //UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_TOPFUNC);
        }

        public void CloseUIs()
        {

        }

    }
}