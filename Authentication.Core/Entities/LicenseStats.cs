namespace Authentication.Core.Entities
{
    public class LicenseStats
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
    }
}
