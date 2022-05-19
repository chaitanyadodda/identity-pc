namespace Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

public class UserDto
{
    public int MemberId { get; set; }
    public string SubjectId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string PhotoPath { get; set; }
    public string Email { get; set; }
    public int CorporateAccountId { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public string Name { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string PinCode { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string District { get; set; }
    public string State { get; set; }
    public string Phone { get; set; }
}