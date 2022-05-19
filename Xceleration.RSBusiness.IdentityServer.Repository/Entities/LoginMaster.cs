namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class LoginMaster
{
    public int LoginId { get; set; }
    public string WebUserId { get; set; }
    public string WebPassword { get; set; }
    public string EntityName { get; set; }
    public MemberMaster MemberMaster { get; set; }
}