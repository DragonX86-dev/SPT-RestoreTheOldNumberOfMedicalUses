using System.Text.Json.Serialization;
using SPTarkov.Server.Core.Models.Common;

namespace MaxHpResourceEdit;

public record ItemConfig
{
    [JsonPropertyName("mongo_id")]
    public required MongoId MongoId { get; init; }
    
    [JsonPropertyName("value")]
    public required int Value { get; init; }
};