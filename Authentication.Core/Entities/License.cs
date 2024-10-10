namespace Authentication.Core.Entities
{
    public class License
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; }
        public bool IsBanned { get; set; }
        public bool IsAdmin { get; set; }
        public string ?HWID { get; set; }
    }
}
