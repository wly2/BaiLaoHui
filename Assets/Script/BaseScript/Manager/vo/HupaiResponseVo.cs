using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    [Serializable]
    public class HupaiResponseVo
    {
        public uint endType; //结束类型0是流局，1是正常胡......
        public List<HupaiResponseItem> avatarList;
        public bool bAllGameEnd;

        public HupaiResponseVo()
        {
            avatarList = new List<HupaiResponseItem>();
        }
    }
}