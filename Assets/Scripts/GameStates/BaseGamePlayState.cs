using UnityEngine;
using System.Collections;


namespace Assets.Script.Common.StateMachine
{
    public class BaseGamePlayState : State
    {
        public static string StateName { get { return typeof(BaseGamePlayState).Name; } }

        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            OpenUIs();
            //switch (GameModule.Instance.mGameModeType)
            //{
            //    case GameModeType.PVE:
            //        BattleSystem.Instance.InitGameByPVE(GameModule.Instance.LevelID);
            //        break;
            //    case GameModeType.AsyPVP:
            //        BattleSystem.Instance.InitGameByAsyPVP();
            //        break;
            //}
        }

        public override void Update()
        {
            //BattleSystem.Instance.OnUpdate();
        }

        public override void Exit()
        {
            CloseUIs();
            base.Exit();
        }

        private void OpenUIs()
        {
            UISystem.Instance.CloseAllUI();
            UISystem.Instance.ShowGameUI(GlobalConst.UI.DIR_UINAME_GAMEUI);

        }

        private void CloseUIs()
        {
            UISystem.Instance.CloseGameUI(GlobalConst.UI.DIR_UINAME_GAMESETTLEMENTUI);
            UISystem.Instance.CloseGameUI(GlobalConst.UI.DIR_UINAME_GAMEUI);
        }


    }


}
