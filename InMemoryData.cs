using System.Collections.Generic;

namespace RequestResponse
{
    public  class InMemoryData
    {
        public static IList<BnppApplication> ApplicationList { get; set; } = new List<BnppApplication>()
        {
            new BnppApplication()
            {
                Id = 1,
                Name = "Reaserch Paltform",
                Description = "A transversal repository for company research publications and ratings",
                OwnerId = 1,
                SecurityId = 2,
                InstallerId = 3
            },
            new BnppApplication()
            {
                Id = 2,
                Name = "Trader App",
                Description = "Creates and tracks trades in real-time",
                OwnerId = 1,
                SecurityId = 2,
                InstallerId = 3
            },
            new BnppApplication()
            {
                Id = 3,
                Name = "Portfolio Manager",
                Description = "Manages portfolio positions, creates models, simulates trade strategies",
                OwnerId = 1,
                SecurityId = 2,
                InstallerId = 3
            },
            new BnppApplication()
            {
                Id = 4,
                Name = "Compliance Officer",
                Description = "Validate trades against company compliance rules",
                OwnerId = 1,
                SecurityId = 2,
                InstallerId = 3
            }
        };
        public static IList<BnppUser> UserList { get; set; } = new List<BnppUser>()
        {
            new BnppUser()
            {
                Id = 1,
                Name = "Mihail",
                SymphonyAccountName = "mihail.trandafir@externe.bnpparibas.com",
                ManagerId = 1
            },
            new BnppUser()
            {
                Id = 2,
                Name = "Paul",
                SymphonyAccountName = "paul.pritchett@bnpparibas.com",
                ManagerId = 1
            },
            new BnppUser()
            {
                Id = 3,
                Name = "Mohamed",
                SymphonyAccountName = "mohamed.chaibi@externe.bnpparibas.com",
                ManagerId = 1
            },
            new BnppUser()
            {
                Id = 4,
                Name = "Alan",
                SymphonyAccountName = "alan.goudie@bnpparibas.com",
                ManagerId = 1
            }
        };
        public static IList<BnppApplicationRole> ApplicationRoleList { get; set; } = new List<BnppApplicationRole>()
        {
            new BnppApplicationRole()
            {
                Id = 1,
                Name = "ReadOnly",
                ApplicationId = 1
            },
            new BnppApplicationRole()
            {
                Id = 2,
                Name = "Publisher",
                ApplicationId = 1
            },
            new BnppApplicationRole()
            {
                Id = 3,
                Name = "ReadOnly",
                ApplicationId = 2
            },
            new BnppApplicationRole()
            {
                Id = 4,
                Name = "Trader",
                ApplicationId = 2
            },
            new BnppApplicationRole()
            {
                Id = 5,
                Name = "ReadOnly",
                ApplicationId = 3
            },
            new BnppApplicationRole()
            {
                Id = 6,
                Name = "Manager",
                ApplicationId = 3
            },
            new BnppApplicationRole()
            {
                Id = 7,
                Name = "ReadOnly",
                ApplicationId = 4
            },
            new BnppApplicationRole()
            {
                Id = 8,
                Name = "Officer",
                ApplicationId = 4
            }
        };
    }
}