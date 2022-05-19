namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class MemberDetail
{
    public int MemberId { get; set; }
    public int CorporateAccountId { get; set; }
    public int? CorporateMemberIsDeActive { get; set; }
    public MemberMaster MemberMaster { get; set; }
}