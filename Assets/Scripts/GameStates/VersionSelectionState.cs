using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace Assets.Script.Common.StateMachine
{
    public class VersionSelectionState : State
    {
        public static string StateName { get { return typeof(VersionSelectionState).Name; } }
        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            base.Enter();
            ConfigManager.Instance.Uninitialize();
            VersionSelectViewController.Initialize();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            VersionSelectViewController.UnInitialize();
        }
        public override string GetStateName()
        {
            return "VersionSelectionState";
        }
    }
}


