using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    [Serializable]
    public class HupaiResponseItem
    {
        public int chairId;
        public bool win;
        public int roundTotalScore;
        public List<int> pengArray;
        public List<int> centerChiArray;
        public List<int> rightChiArray;
        public List<int> leftChiArray;
        public List<GangInfo> gangInfos;
        public List<int> paiArray;
        public HuInfo huInfo;

        public HupaiResponseItem()
        {
            gangInfos = new List<GangInfo>();
            pengArray = new List<int>();
            centerChiArray = new List<int>();
            paiArray = new List<int>();
            rightChiArray = new List<int>();
            leftChiArray = new List<int>();
        }
    }
}

public class HuInfo
{
    /*
              "way": "zimo", //胡牌方式，zimo， chi
              "type": "jihu", //牌类型， jihu，qingyise， shisanyao等
              "specialScene": "tianhu", //特殊场景，tianhu， gangbao， haidilaoyue等
              "huUid": 100308, //放炮uid
     * */
    public string way;
    public string type;
    public string specialScene;
    public int card;
    public int huUid;
}

public class GangInfo
{
    /*
     * "gangType": "minggang", //minggang bugangNum angang
					"cardIndex": 1,
					"gangUid": "100308"
     */
    public string gangType;
    public int cardIndex;
    public int gangUid;
}