namespace FS.Statistics
{
    public class GameStatistic
    {
        public static int MonstersDefeat {  get; private set; }

        public static void Clear()
        {
            MonstersDefeat = 0;
        }

        public static void IncreaseMonsterEliminated()
        {
            MonstersDefeat += 1;
        }
    }
}