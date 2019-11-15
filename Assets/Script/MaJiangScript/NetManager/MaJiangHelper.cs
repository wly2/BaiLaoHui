using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaJiangHelper{

    public static int MaJiangCardChange(MJ_PAI mJ_PAI)
    {
        switch (mJ_PAI)
        {
            case MJ_PAI.INVALID_PAI:
                break;
            case MJ_PAI.YI_TONG:
                return 18;
            case MJ_PAI.ER_TONG:
                return 19;
            case MJ_PAI.SAN_TONG:
                return 20;
            case MJ_PAI.SI_TONG:
                return 21;
            case MJ_PAI.WU_TONG:
                return 22;
            case MJ_PAI.LIU_TONG:
                return 23;
            case MJ_PAI.QI_TONG:
                return 24;
            case MJ_PAI.BA_TONG:
                return 25;
            case MJ_PAI.JIU_TONG:
                return 26;
            case MJ_PAI.YI_WAN:
                return 0;
            case MJ_PAI.ER_WAN:
                return 1;
            case MJ_PAI.SAN_WAN:
                return 2;
            case MJ_PAI.SI_WAN:
                return 3;
            case MJ_PAI.WU_WAN:
                return 4;
            case MJ_PAI.LIU_WAN:
                return 5;
            case MJ_PAI.QI_WAN:
                return 6;
            case MJ_PAI.BA_WAN:
                return 7;
            case MJ_PAI.JIU_WAN:
                return 8;
            case MJ_PAI.YI_SUO:
                return 9;
            case MJ_PAI.ER_SUO:
                return 10;
            case MJ_PAI.SAN_SUO:
                return 11;
            case MJ_PAI.SI_SUO:
                return 12;
            case MJ_PAI.WU_SUO:
                return 13;
            case MJ_PAI.LIU_SUO:
                return 14;
            case MJ_PAI.QI_SUO:
                return 15;
            case MJ_PAI.BA_SUO:
                return 16;
            case MJ_PAI.JIU_SUO:
                return 17;
            case MJ_PAI.DONG_FENG:
                return 27;
            case MJ_PAI.NAN_FENG:
                return 28;
            case MJ_PAI.XI_FENG:
                return 29;
            case MJ_PAI.BEI_FENG:
                return 30;
            case MJ_PAI.HONG_ZHONG:
                return 31;
            case MJ_PAI.FA_CAI:
                return 32;
            case MJ_PAI.BAI_BAN:
                return 33;
            default:
                break;
        }

        return -1;
    }

    public static MJ_PAI MaJiangCardToChange(int mJ_PAI)
    {
        MyDebug.Log("mJ_PAI:" + mJ_PAI);
        switch (mJ_PAI)
        {
            case 0:
                return MJ_PAI.YI_WAN;
            case 1:
                return MJ_PAI.ER_WAN;
            case 2:
                return MJ_PAI.SAN_WAN;
            case 3:
                return MJ_PAI.SI_WAN;
            case 4:
                return MJ_PAI.WU_WAN;
            case 5:
                return MJ_PAI.LIU_WAN;
            case 6:
                return MJ_PAI.QI_WAN;
            case 7:
                return MJ_PAI.BA_WAN;
            case 8:
                return MJ_PAI.JIU_WAN;
            case 9:
                return MJ_PAI.YI_SUO;
            case 10:
                return MJ_PAI.ER_SUO;
            case 11:
                return MJ_PAI.SAN_SUO;
            case 12:
                return MJ_PAI.SI_SUO;
            case 13:
                return MJ_PAI.WU_SUO;
            case 14:
                return MJ_PAI.LIU_SUO;
            case 15:
                return MJ_PAI.QI_SUO;
            case 16:
                return MJ_PAI.BA_SUO;
            case 17:
                return MJ_PAI.JIU_SUO;
            case 18:
                return MJ_PAI.YI_TONG;
            case 19:
                return MJ_PAI.ER_TONG;
            case 20:
                return MJ_PAI.SAN_TONG;
            case 21:
                return MJ_PAI.SI_TONG;
            case 22:
                return MJ_PAI.WU_TONG;
            case 23:
                return MJ_PAI.LIU_TONG;
            case 24:
                return MJ_PAI.QI_TONG;
            case 25:
                return MJ_PAI.BA_TONG;
            case 26:
                return MJ_PAI.JIU_TONG;
            case 27:
                return MJ_PAI.DONG_FENG;
            case 28:
                return MJ_PAI.NAN_FENG;
            case 29:
                return MJ_PAI.XI_FENG;
            case 30:
                return MJ_PAI.BEI_FENG;
            case 31:
                return MJ_PAI.HONG_ZHONG;
            case 32:
                return MJ_PAI.FA_CAI;
            case 33:
                return MJ_PAI.BAI_BAN;
        }

        return MJ_PAI.INVALID_PAI;
    }
}
