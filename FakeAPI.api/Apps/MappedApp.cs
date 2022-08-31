using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FakeAPI.Api;

public record MappedApp
{
    public AppName Id { get; set; } = new("");
    public UserName UserName { get; set; } = new("");
    public IEnumerable<Mapping> Mappings { get; set; } = new List<Mapping>();
    public bool RequireUserPassword { get; set; } = true;
}

public record MappedAppModel
{
    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public IEnumerable<Mapping>? Mappings { get; set; }

    [Required]
    public bool? RequireUserPassword { get; set; }
}