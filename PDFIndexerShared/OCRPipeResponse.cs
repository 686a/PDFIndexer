using System.Text.Json.Serialization;

namespace PDFIndexerShared
{
    public class OCRPipeResponse : PipeResponse
    {
        [JsonInclude]
        public string Text;
        [JsonInclude]
        public OCRRegion[] Regions;

        public OCRPipeResponse(string text)
        {
            Text = text;
            Regions = new OCRRegion[0];
        }

        [JsonConstructor]
        public OCRPipeResponse(string text, OCRRegion[] regions)
        {
            Text = text;
            Regions = regions;
        }

        //public override string ToJSON()
        //{
        //    return JsonSerializer.Serialize(this);
        //}
    }
}
