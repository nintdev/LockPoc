namespace LockPoc.Data
{
    public class Constants
    {
        public class DocumentTypes
        {
            public const string SalesDocument = nameof(SalesDocument);
            public const string Invoice = nameof(Invoice);    
        }

        public class ConnectionNames
        {
            public const string Connection = nameof(Connection);
            public const string MasterConnection = nameof(MasterConnection);
        }
    }
}