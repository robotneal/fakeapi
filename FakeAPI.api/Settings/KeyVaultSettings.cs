using System;

namespace FakeAPI.Api;

public record KeyVaultSettings
{
    public string Uri { get; set; } = "";
    public string TennantId { get; set; } = "";
    public string ApplicationId { get; set; } = "";
    public string Secret { get; set; } = "";
}