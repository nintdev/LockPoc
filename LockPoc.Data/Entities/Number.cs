using System;

namespace LockPoc.Data.Entities
{
    public class Number
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ulong LastIssuedNumber { get; set; }
        public int LastIssuedUserId { get; set; }
        public DateTime LastIssuedTimestamp { get; set; }
    }
}