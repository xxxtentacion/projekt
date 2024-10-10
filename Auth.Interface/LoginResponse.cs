namespace Auth.Interface
{
    public class LoginResponse
    {
        public string message { get; set; }
        public string licenseKey { get; set; }
        public bool isAdmin { get; set; }
    }
}
