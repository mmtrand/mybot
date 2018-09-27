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
                Name = "ReaserchPaltform"
            },
            new BnppApplication()
            {
                Id = 2,
                Name = "TraderApp"
            },
            new BnppApplication()
            {
                Id = 3,
                Name = "Portfolio Manager"
            }
        };
    }
}