namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class DistrictMaster
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; }
    public IEnumerable<AddressMaster> AddressMasters { get; set; }
    public IEnumerable<CityMaster> CityMasters { get; set; }
}