namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class AddressMaster
{
    public int AddressId { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }

    public string Address3 { get; set; }
    public string PinCode { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public int CountryId { get; set; }
    public CityMaster CityMaster { get; set; }
    public DistrictMaster DistrictMaster { get; set; }
    public CountryMaster CountryMaster { get; set; }
    public MemberMaster MemberMaster { get; set; }
}