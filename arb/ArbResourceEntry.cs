using System;
using System.Collections.Generic;
using userManagement;

namespace arb
{
    public class ArbResourceCollection
    {
        public Tenant Tenant { get; set; }
        public string Locale { get; set; }
        public string Context { get; set; }
        public List<ArbResourceEntry> ArbEntries { get; set; } = new List<ArbResourceEntry>();

    }
    public class ArbResourceEntry
    {
        private Guid _internalId;
        private readonly bool _isNew = true;
        public ArbResourceEntry(Guid? id)
        {
            if (id.HasValue) { _internalId = id.Value; _isNew = false; }
        }

        public string Id { get; set; }
        public string Value { get; set; }
        public ArbReourceEntryAttribute Type { get; set; } = new ArbReourceEntryAttribute("type");
        public ArbReourceEntryAttribute Context { get; set; } = new ArbReourceEntryAttribute("context");
        public ArbReourceEntryAttribute Description { get; set; } = new ArbReourceEntryAttribute("description");
        public ArbReourceEntryAttribute Source_text { get; set; } = new ArbReourceEntryAttribute("source_text");
        public ArbReourceEntryAttribute Screen { get; set; } = new ArbReourceEntryAttribute("screen");


    }


    public class ArbReourceEntryAttribute
    {
        private string _attributeName="";
        public ArbReourceEntryAttribute(string attributeName) { _attributeName = attributeName; }
        public string AttributeName => _attributeName;
        public string AttributeValue { get; set; }
    }
}
