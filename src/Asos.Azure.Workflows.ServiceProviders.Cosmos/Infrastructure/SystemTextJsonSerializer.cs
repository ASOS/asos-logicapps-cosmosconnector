using System.IO;
using System.Text.Json;
using Microsoft.Azure.Cosmos;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos.Infrastructure
{
    public class SystemTextJsonSerializer : CosmosSerializer
    {
        private readonly JsonSerializerOptions _options;
        
        public SystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }
        
        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                using var memory = new MemoryStream((int)stream.Length);
                stream.CopyTo(memory);

                var utf8Json = memory.ToArray();

                return JsonSerializer.Deserialize<T>(utf8Json, _options);
            }
        }

        public override Stream ToStream<T>(T input)
        {
            var utf8Json = JsonSerializer.SerializeToUtf8Bytes(input, _options);
            return new MemoryStream(utf8Json);
        }
    }
}