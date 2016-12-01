using Newtonsoft.Json;

namespace AKK.Models
{
    public class Video : Model
    {
        [JsonIgnore]
        public string FilePath { get; set; }

        public string FileUrl { get; set; }
    }
}
