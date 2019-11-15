using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataNN : Singleton<GameDataNN>
{
    public CMD_S_NN_GameStart gameStartInfo;
    public CMD_S_CallBanker callBanker;
    public CMD_NN_S_SendCard HandCard;//自由抢庄
    public CMD_S_AllCard MPhandCard;//明牌抢庄
    public CMD_NN_S_GameEnd gameEnd;
    public CMD_GR_PersonalTableEnd AllgameEnd;
    public CMD_S_StatusFreeNN statusFreeNN;
    public CMD_S_StatusCallNN statusCallNN;
    public CMD_S_StatusScoreNN statusScoreNN;
    public CMD_S_StatusPlayNN statusPlayNN;
    public CMD_S_AddScore xiazhuInofoNN;

}
