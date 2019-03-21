using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace Assets.Script.Common.StateMachine
{
    public class CheckUpdateState : State 
    {
        public static string StateName 
        { 
            get 
            { 
                return typeof(CheckUpdateState).Name; 
            } 
        }
        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            base.Enter();
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CHECKVERSIONVIEW);
            UpdateVersionManager.Instance.StartCheckVersion();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            UpdateVersionManager.Instance.Uninitialize();
        }
        public override string GetStateName()
        {
            return "CheckUpdateState";
        }
    }
}


