
namespace ServerSideApiSsl.Database
{
    public class Sheet
    {
        public int Id { get; set; }    
        public int User { get; set; }      
        public string? Data { get; set; } = default;
        public int Type { get; set; }
    }
}
