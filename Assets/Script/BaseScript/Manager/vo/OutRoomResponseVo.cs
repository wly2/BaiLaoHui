using System;

namespace AssemblyCSharp
{
    [Serializable]
    public class OutRoomResponseVo
    {
        public int dwDissUserCout;
        public int[] dwDissChairID;
        public int dwNotAgreeUserCout;
        public int[] dwNotAgreeChairID;

        public OutRoomResponseVo()
        {
        }
    }
    public class DissloveRoomResponseVo
    {
        public int userId;
        public int result;
        public int tableId;
    }
}