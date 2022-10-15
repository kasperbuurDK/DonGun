using System.Text.Json.Serialization;

namespace SharedClassLibrary
{
    public class User
    {
        [JsonPropertyName ("id")]
        public Guid Id { get; set; } = new Guid();
        [JsonPropertyName ("primaryKey")]
        public string? Name { get; set; } = default;        // User name is uniq
        public string? Password { get; set; } = default;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}