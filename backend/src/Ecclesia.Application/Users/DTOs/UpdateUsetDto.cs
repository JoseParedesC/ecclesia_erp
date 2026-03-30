using System.Text.Json.Serialization;

namespace Ecclesia.Application.Users.DTOs;

public record UpdateUserDto(
    // [property: JsonPropertyName("id")]
    Guid Id,

    // [property: JsonPropertyName("name")]
    string Name,

    // [property: JsonPropertyName("userName")]
    string Username,

    // [property: JsonPropertyName("email")]
    string Email
    
);