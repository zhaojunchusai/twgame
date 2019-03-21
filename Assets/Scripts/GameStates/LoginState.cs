using UnityEngine;
using System.Collections;

namespace Assets.Script.Common.StateMachine
{
    public class LoginState : State
    {

        public static string StateName { get { return typeof(LoginState).Name; } }

        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            base.Enter();
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_LOGIN);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, GlobalConst.MIN_FIGHT_SPEED);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_LOGIN);
        }

        public override void Update()
        {
            base.Update();
            if (GlobalConst.IS_TESTAPK)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                  Main.Instance.ChangeState(VersionSelectionState.StateName); 
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_LOGIN);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_LoadAutoStatus);
        }

    }
}
