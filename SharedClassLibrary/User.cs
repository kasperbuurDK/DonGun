using System.Text.Json.Serialization;

namespace SharedClassLibrary
{
    public class User
    {
        public int Id { get; set; }     // Can use GUID if SqlDb uses "Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID()"    
        public string? Name { get; set; } = default;        // User name is uniq
        public string? Password { get; set; } = default;
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
    }
}

