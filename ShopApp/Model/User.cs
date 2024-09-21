using System;
using System.Collections.Generic;

namespace ShopApp.Model;

public partial class User
{
    public int UserId { get; set; }

    public string? UserLogin { get; set; }

    public string? UserPassword { get; set; }

    public string? UserRole { get; set; }

    public string? UserSalt { get; set; }
}
