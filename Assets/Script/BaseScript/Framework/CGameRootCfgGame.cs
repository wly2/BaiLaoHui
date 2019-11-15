using UnityEngine;
using System;

partial class CGameRootCfg
{
    public static CGameRootCfg mGame = new CGameRootCfg(

        #region 各系统初始化顺序定义

        delegate(Transform rootObj)
        {
            return new CGameSystem[]
            {
                CreateGameSys<CEventMgr>(rootObj),
                //-----------------UI管理器--------------------
                //CreateGameSys<UIManager>(rootObj),
                //CreateGameSys<UIMainSys>(rootObj),
                //CreateGameSys<UIBattleSys>(rootObj),
                //CreateGameSys<UITaskSys>(rootObj),
                //CreateGameSys<UIBagSys>(rootObj),
                //CreateGameSys<EquipMgr>(rootObj),
                //CreateGameSys<UIShopManager>(rootObj),
                //CreateGameSys<UISoulStoneSys>(rootObj),
                //CreateGameSys<PetDataSys>(rootObj),
                //CreateGameSys<UISpiritManager>(rootObj),
                //CreateGameSys<UISkillSys>(rootObj),
                //CreateGameSys<UIRankSys>(rootObj),
                //CreateGameSys<UICoinChangeSys>(rootObj),
                //CreateGameSys<SoloMissionDataHandler>(rootObj),
                //CreateGameSys<UIHeroInformationSys>(rootObj),
                //CreateGameSys<UICreateRoleSys>(rootObj),
                //CreateGameSys<AttributeUtils>(rootObj),
            };
        },

        #endregion

        #region 状态定义 & 状态下各系统的定义

        new CGameState(EStateType.Root,
            new Type[]
            {
                //typeof(LoadingSys),
                //typeof(TimerManager),
                //typeof(CEventMgr),
                //typeof(KernelEvent),
                //typeof(TrackUtil),
                //typeof(SoundManager),
                //typeof(ShowFPS),
                //typeof(LevelLoader),
                //typeof(GameMonsterManager),
                //typeof(NetworkManager),
                //typeof(ActivityMgr),
                //typeof(WorldPlayerManager),
                //typeof(TalkBattle),
                //typeof(BattleSceneLoader),
                //typeof(BattleManager),
                //typeof(TaskManager),
                //typeof(UIManager),
                //typeof(UIMainSys),
                //typeof(UIBattleSys),
                //typeof(UITaskSys),
                //typeof(UIBagSys),
                //typeof(EquipMgr),
                //typeof(UIShopManager),
                //typeof(UISoulStoneSys),
                //typeof(PetDataSys),
                //typeof(UISpiritManager),
                //typeof(UISkillSys),
                //typeof(UIRankSys),
                //typeof(UICoinChangeSys),
                //typeof(SoloMissionDataHandler),
                //typeof(UIHeroInformationSys),
                //typeof(UICreateRoleSys),
                //typeof(AttributeUtils),
            },
            new CGameState[]
            {
                new CGameState(EStateType.PreLoad,
                    new Type[] { }, null),
                new CGameState(EStateType.Login,
                    new Type[] { }, null),
                new CGameState(EStateType.Initial,
                    new Type[] { }, null),
                new CGameState(EStateType.PreMatch, //自由模式系统
                    new Type[] { }, null),
                new CGameState(EStateType.Match,
                    new Type[] { }, null),
                new CGameState(EStateType.Replay,
                    new Type[] { }, null),
                new CGameState(EStateType.CreateRole,
                    new Type[] { }, null),
            }
        )

        #endregion

    );
}