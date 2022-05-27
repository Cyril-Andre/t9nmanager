using System.Collections.Generic;
using System.Text.Json.Serialization;
using userManagement;

namespace arb
{
    public class ArbResourceCollection
    {
        public Tenant Tenant { get; set; }
        public string Locale { get; set; }
        public string Context { get; set; }//Should be Project Name
        public List<ArbResourceEntry> ArbEntries { get; set; } = new List<ArbResourceEntry>();
        [JsonIgnore]
        public Project Project { get; set; }

    }
}
