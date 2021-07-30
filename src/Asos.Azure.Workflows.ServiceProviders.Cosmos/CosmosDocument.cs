using System;
using System.Text.Json.Serialization;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos
{
    public class CosmosDocument
    {
        private static readonly DateTime UnixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private double _ts;

        public CosmosDocument()
        {
        }
        
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("_rid")] public string ResourceId { get; set; }

        [JsonPropertyName("_self")] public string SelfLink { get; set; }

        [Newtonsoft.Json.JsonIgnore] public string AltLink { get; set; }

        [JsonPropertyName("_ts")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Timestamp
        {
            get => UnixStartTime.AddSeconds(_ts);
            set => _ts = (value - UnixStartTime).TotalSeconds;
        }

        [JsonPropertyName("_etag")] public string ETag { get; set; }
    }
}