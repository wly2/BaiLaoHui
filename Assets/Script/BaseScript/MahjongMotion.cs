using System;

namespace AssemblyCSharp
{
    [Serializable]
    public class MahjongMotion
    {
        public int cardID;
        public bool isNoMotion;
        public bool isLeftMotion;
        public bool isRightMotion;
        public bool isCenterMotion;
        public bool isPengMotion;
        public bool isGangMotion;
        public bool isChiHuMotion;
        public int chiCount;
        public int type;
    }
}