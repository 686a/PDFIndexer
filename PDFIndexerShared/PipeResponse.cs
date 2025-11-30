using System.Text.Json;
using System.Text.Json.Serialization;

namespace PDFIndexerShared
{
    public class PipeResponse
    {
        [JsonInclude]
        public int status;

        public PipeResponse()
        {
            status = 200;
        }

        [JsonConstructor]
        public PipeResponse(int status)
        {
            this.status = status;
        }

        public static string ToJSON<T>(T obj)
        {
            return JsonSerializer.Serialize<T>(obj);
        }

        public static T FromJSON<T>(string json)
        {
            return (T)JsonSerializer.Deserialize<T>(json);
        }
    }
}
