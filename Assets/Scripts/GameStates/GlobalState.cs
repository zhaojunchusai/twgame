﻿using UnityEngine;
using System.Collections;
namespace Assets.Script.Common.StateMachine 
{
    public class GlobalState : State
    {

        public static string StateName { get { return typeof(GlobalState).Name; } }

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

