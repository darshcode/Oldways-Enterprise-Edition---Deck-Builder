using System.Text.Json.Serialization;

namespace OldWays.Models
{
    public class Day
    {
        [JsonPropertyName("maxtemp_c")]
        public float MaxtempC { get; set; }

        [JsonPropertyName("mintemp_c")]
        public float MintempC { get; set; }

        public Condition Condition { get; set; }
    }
}
