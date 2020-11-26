namespace MightyAttributes
{
    public static class IconName
    {
        #region Icons

        public const string HELP = "_Help@2x";
        public const string EYE = "ViewToolOrbit";
        public const string CS_SCRIPT_ICON = "cs Script Icon";
        public const string REFRESH = "TreeEditor.Refresh";
        public const string SAVE = "SaveActive";
        public const string FOLDER = "Folder Icon";
        public const string SETTINGS = "Settings";
        public const string TRASH = "TreeEditor.Trash";
        public const string PLUS = "Toolbar Plus";
        public const string MINUS = "Toolbar Minus";
        public const string RECORD = "Animation.Record";
        public const string PLAY = "d_PlayButton";
        public const string LOCK = "LockIcon-On";
        public const string UNLOCK = "LockIcon";
        public const string STAR_LIST = "CustomSorting";
        public const string UP_ARROW = "UpArrow";
        public const string IN = "SceneLoadIn";
        public const string OUT = "SceneLoadOut";
        public const string UNITY = "UnityLogoLarge";
        public const string SCENE = "SceneAsset Icon";

        #endregion /Icons

        #region Loading

        public const string LOADING_0 = "WaitSpin00";
        public const string LOADING_1 = "WaitSpin01";
        public const string LOADING_2 = "WaitSpin02";
        public const string LOADING_3 = "WaitSpin03";
        public const string LOADING_4 = "WaitSpin04";
        public const string LOADING_5 = "WaitSpin05";
        public const string LOADING_6 = "WaitSpin06";
        public const string LOADING_7 = "WaitSpin07";
        public const string LOADING_8 = "WaitSpin08";
        public const string LOADING_9 = "WaitSpin09";
        public const string LOADING_10 = "WaitSpin10";
        public const string LOADING_11 = "WaitSpin11";

        public static string Loading(int index)
        {
            switch (index)
            {
                case 0: return LOADING_0;
                case 1: return LOADING_1;
                case 2: return LOADING_2;
                case 3: return LOADING_3;
                case 4: return LOADING_4;
                case 5: return LOADING_5;
                case 6: return LOADING_6;
                case 7: return LOADING_7;
                case 8: return LOADING_8;
                case 9: return LOADING_9;
                case 10: return LOADING_10;
                case 11: return LOADING_11;
            }

            return LOADING_0;
        }

        public static void NextLoadingIndex(ref int index)
        {
            if (index < 0 || index > 11) index = -1;
            ++index;
        }

        public static string NextLoadingIcon(ref int index)
        {
            NextLoadingIndex(ref index);
            return Loading(index);
        }

        #endregion /Loading
    }
}