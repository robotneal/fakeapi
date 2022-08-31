namespace FakeAPI.Api;

public record User
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set => _name = value.ToLower();
    }
}