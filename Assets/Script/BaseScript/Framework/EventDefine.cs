public enum ESysEvent
{
    TestClientEvent,
    EnterWorld,
    ExpChange,
    PetFightChange,
    EnterBattle,
    EnterStage,
    BattleEnd,
    NpcTalkTip,
    BattleModeSwitch,
    BattleActionPhaseChange,
    TaskInfoChange,
    TaskBattle,
    Reward,
    MoneyChange,
    GoldChange,
    GemChange,
    ItemDataChange,
    ActPowerChange,
    BagItemDataChange,
    BagItemDataAllChange,
    UserEquipChange,
    ContainerCapacityChange,
    ActRefresh,
    ActJoin,
    MapMonster,
    MonsterRefresh,
}

/// <summary>
/// 触发事件定义
/// 因为涉及到美术的编辑，值定好之后不可变更
/// </summary>
public enum ETriggerEvent
{
    SongArtPartEnd,
}