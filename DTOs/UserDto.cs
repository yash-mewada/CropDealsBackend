// Dtos/UserDto.cs
public class UserDto
{
    public string Id { get; set; }
    public string Name { get; set; }          // Optional: Add if you store full name
    public string Email { get; set; }
    public string Role { get; set; }
    public string PhoneNumber { get; set; }
    public bool Status { get; set; }          // Or int if you use enums
}
