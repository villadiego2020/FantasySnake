namespace FS.Statistics
{
    public class GameStatistic
    {
        public static int MonstersEliminated {  get; private set; }

        public static void IncreaseMonsterEliminated()
        {
            MonstersEliminated += 1;
        }
    }
}