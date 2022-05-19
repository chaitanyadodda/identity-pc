namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class MemberMaster
{
    public int MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string PhotoPath { get; set; }
    public string EmailId { get; set; }
    public string MobileNo { get; set; }
    public MemberDetail MemberDetails { get; set; }
    public LoginMaster LoginMaster { get; set; }
    public int AddressId { get; set; }
    public AddressMaster AddressMaster { get; set; }
}