using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Asos.Azure.Workflows.ServiceProviders.Cosmos
{
    public sealed class UnixDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly DateTime UnixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            double num;
            try
            {
                num = Convert.ToDouble(reader.GetDouble(), CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("Failed to read unix datetime date");
            }

            return UnixStartTime.AddSeconds(num);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var totalSeconds = (long) (value - UnixStartTime).TotalSeconds;

            writer.WriteNumberValue(totalSeconds);
        }
    }
}