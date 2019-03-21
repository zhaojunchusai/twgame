using UnityEngine;
using System.Collections;
namespace Assets.Script.Common.StateMachine 
{
    public class InitGameState : State
    {
        public static string StateName { get { return typeof(InitGameState).Name; } }

        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
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

    }

}

