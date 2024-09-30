namespace YG
{
    [System.Serializable]
    public class SavesYG
    {


        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;
        
        public int completedLevel = 0;
        public bool volumeOn = false;
        public int Mylanguage = 0;

        public static void SwithVolume()
        {
            YandexGame.savesData.volumeOn = !YandexGame.savesData.volumeOn;
            SaveDataFunc();
        }
        public static bool GetVolume()
        {
            return YandexGame.savesData.volumeOn;
        }
        public static int GetLanguage()
        {
            return YandexGame.savesData.Mylanguage;
        }
        public static void SetLanguage(int language)
        {
            YandexGame.savesData.Mylanguage = language;
            SaveDataFunc();
        }
        public static int GetCompletedLevel() 
        {
            return YandexGame.savesData.completedLevel;
        }
        public static void SetCompletedLevel(int level) 
        {
            YandexGame.savesData.completedLevel = level;
            SaveDataFunc();
        }
        private static void SaveDataFunc()
        {
            YandexGame.SaveProgress();
        }



    }
}
