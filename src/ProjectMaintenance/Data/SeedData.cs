using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectMaintenance.Data
{
    public class UserInfoData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class SeedData
    {
        public static readonly UserInfoData[] UserSeedData = new UserInfoData[]
        {
            new UserInfoData { Id = Guid.NewGuid().ToString(), Name = "TaliaK"},
            new UserInfoData { Id = Guid.NewGuid().ToString(), Name = "ZaydenC"},
            new UserInfoData { Id = Guid.NewGuid().ToString(), Name = "DavilaH"},
            new UserInfoData { Id = Guid.NewGuid().ToString(), Name = "KrzysztofP"}
        };
    }
}
