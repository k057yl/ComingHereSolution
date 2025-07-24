namespace ComingHereShared.Entities
{
    public static class ReputationHelper
    {
        public static string GetRank(int points)
        {
            return points switch
            {
                < 50 => "Новичок",
                < 200 => "Знаток",
                < 500 => "Местная легенда",
                _ => "Божество"
            };
        }
    }
}
