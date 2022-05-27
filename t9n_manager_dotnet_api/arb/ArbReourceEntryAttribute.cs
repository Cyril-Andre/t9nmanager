namespace arb
{
    public class ArbReourceEntryAttribute
    {
        private string _attributeName="";
        public ArbReourceEntryAttribute(string attributeName) { _attributeName = attributeName; }
        public string AttributeName => _attributeName;
        public string AttributeValue { get; set; }
    }
}
