using Newtonsoft.Json;

namespace AKK.Models
{
    public class Video : Media
    {
        [JsonIgnore]
        public string FilePath { get; set; }
    }
}
