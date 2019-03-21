using UnityEngine;
using System.Collections;
namespace Assets.Script.Common.StateMachine
{
    public class GameSplashState : State
    {
        public static string StateName { get { return typeof(GameSplashState).Name; } }

        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            base.Enter();
            GetServerInfo();
        }

        public override void Update()
        {

            base.Update();
        }

        public override void Exit()
        {
            //UISystem.Instance.CloseGameUI(GlobalConst.DIR_UINAME_GETSERVERINFOUI);
            base.Exit();
        }

        public void GetServerInfo() 
        {
            UISystem.Instance.ShowGameUI(GlobalConst.UI.DIR_UINAME_GETSERVERINFOUI);
            //LoginModule.Instance.SendVersionNum();
        }

    }
}

