using UnityEngine;
using System.Collections;

namespace Assets.Script.Common.StateMachine
{
    public class CreateRoleState : State
    {

        public static string StateName { get { return typeof(CreateRoleState).Name; } }

        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            base.Enter();
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CREATEROLE);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_LOGIN);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_CREATEROLE);
        }

    }
}
