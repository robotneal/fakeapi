using System.ComponentModel.DataAnnotations;

namespace FakeAPI.Api;

public record Mapping
{
    [MaxLength(10)]
    public string Method { get; set; } = "GET";

    [MaxLength(200)]
    public string Route { get; set; } = "/";

    public int ResponseCode { get; set; } = 200;

    [MaxLength(1000)]
    public string ResponseBody { get; set; } = "";


    public override string ToString() => $"{Method} {Route}";
}