using UnityEngine;
using System.Collections;

namespace Assets.Script.Common.StateMachine
{

    public class FightState : State
    {

        public static string StateName { get { return typeof(FightState).Name; } }

        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            SkillManage tmpSkillManage = SkillManage.Instance;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, true);
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, true);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_SceneDelete);
        }
    }
}