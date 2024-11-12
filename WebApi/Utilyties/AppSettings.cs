namespace WebApi.Utilyties
{
    public class AppSettings
    {
        public  Jwt? Jwt { get; set; }
    }

    public class Jwt
    {
        public string? SecreteKey { get; set; }
        public string? Insuer { get; set; }
        public string? Audience { get; set; }
        public int TokenValidityInMinutes { get; set; }
    }
}
