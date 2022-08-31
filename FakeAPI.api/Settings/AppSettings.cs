using System;

namespace FakeAPI.Api;

public record AppSettings
{
    public string CosmosConnection { get; set; } = "";

    public string DatabaseId { get; set; } = "";

    public string DatabaseContainerId { get; set; } = "";

    public KeyVaultSettings KeyVault { get; set; } = new KeyVaultSettings();
}