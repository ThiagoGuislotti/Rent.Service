using Newtonsoft.Json;

namespace Rent.Service.Api.Responses
{
    public sealed record MessageResponse
    {
        [JsonProperty("mensagem")]
        public required string Message { get; init; }
    }
}