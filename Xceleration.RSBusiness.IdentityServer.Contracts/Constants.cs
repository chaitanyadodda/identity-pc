namespace Xceleration.RSBusiness.IdentityServer.Contracts;

public static class Constants
{
    public class ProtocolRoutePaths
    {
        public const string DiscoveryHost = "/.well-known/openid-configuration/host";
    }

    public class JwtBearerKeys
    {
        public const string RequestTokenUseKey = "requested_token_use";
    }

    public class TokenUse
    {
        public const string OnBehalfOf = "on_behalf_of";
    }

    public static class Properties
    {
        public const string ReturnUrl = "returnUrl";
        public const string Scheme = "scheme";
    }

    public class Database
    {
        public class ConnectionStrings
        {
            public const string PeopleCart = "PeopleCartDB";
            public const string IdentityServer = "IdentityServer";
            public const string RewardStationStorage = "RewardStationStorage";
        }

        public class StoreTableNames
        {
            public const string LoginMaster = "PC_LoginMaster";
            public const string MemberDetails = "PC_MemberDetails";
            public const string MemberMaster = "PC_MemberMaster";
            public const string AddressMaster = "PC_AddressMaster";
            public const string CityMaster = "PC_CityMaster";
            public const string CountryMaster = "PC_CountryMaster";
            public const string DistrictMaster = "PC_DistrictMaster";
            public const string StateMaster = "PC_StateMaster";

            public const string OrganizationSetting = "OrganizationSettings";
            public const string ClientOrganizationSetting = "ClientOrganizationSettings";
        }
    }
}