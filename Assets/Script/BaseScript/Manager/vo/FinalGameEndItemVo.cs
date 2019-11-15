using System;

namespace AssemblyCSharp
{
    [Serializable]
    public class FinalGameEndItemVo
    {
        public int uuid;
        public int zimo;
        public int jiepao;
        public int dianpao;
        public int minggang;
        public int angang;
        public int scores;
        private string nickname;
        private string icon;
        private bool isWiner;
        private bool isPaosshou;
        private bool isMain;

        public FinalGameEndItemVo()
        {
        }

        public void SetIsWiner(bool winnerFlag)
        {
            isWiner = winnerFlag;
        }

        public void SetIsPaoshou(bool paoshouFlag)
        {
            isPaosshou = paoshouFlag;
        }

        public bool GetIsWiner()
        {
            return isWiner;
        }

        public bool GetIsPaoshou()
        {
            return isPaosshou;
        }

        public void SetNickname(string nicknameFlag)
        {
            nickname = nicknameFlag;
        }

        public void SetIcon(string iconFlag)
        {
            icon = iconFlag;
        }

        public string GetNickname()
        {
            return nickname;
        }

        public string GetIcon()
        {
            return icon;
        }

        public bool GetIsMain()
        {
            return isMain;
        }

        public void SetIsMain(bool flag)
        {
            isMain = flag;
        }
    }
}