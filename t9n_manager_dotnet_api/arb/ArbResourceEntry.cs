using System;

namespace arb
{
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
}
