namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class CityMaster
{
    public int CityId { get; set; }
    public string CityName { get; set; }
    public IEnumerable<AddressMaster> AddressMasters { get; set; }
    public int StateId { get; set; }
    public StateMaster StateMaster { get; set; }
    public int DistrictId { get; set; }
    public DistrictMaster DistrictMaster { get; set; }
}