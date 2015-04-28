namespace EventSourcing.Tests.Domain
{
    public class HeaderKeys
    {
        public static readonly string CommitTimestamp = "CommitTimestamp";
        public static readonly string EventClrTypeName = "EventClrTypeName";
        public static readonly string AggregateClrTypeName = "AggregateClrTypeName";
        public static readonly string UserName = "UserName";
    }
}