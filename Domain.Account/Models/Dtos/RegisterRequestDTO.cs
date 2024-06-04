﻿namespace Domain.Account.Models.Dtos;

public class RegisterRequestDTO
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}
