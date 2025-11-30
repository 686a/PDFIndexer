using System.Text.Json.Serialization;

namespace PDFIndexerShared
{
    public struct OCRRegion
    {
        [JsonInclude]
        public string Text;
        [JsonInclude]
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public float Score;

        [JsonInclude]
        public int CenterX;
        [JsonInclude]
        public int CenterY;
        [JsonInclude]
        public int Width;
        [JsonInclude]
        public int Height;
        [JsonInclude]
        [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
        public float Angle;
    }
}
