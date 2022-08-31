using System.Collections.Generic;

namespace FakeAPI.Api;

public record UserTable
{
    public string Id { get; set; } = "user_table";
    public UserName UserName { get; set; } = new("system");
    public ICollection<User> Users { get; set; } = new List<User>();
}