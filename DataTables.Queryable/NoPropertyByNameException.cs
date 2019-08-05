using System;

namespace DataTables.Queryable
{
    public class NoPropertyByNameException : Exception
    {
        public NoPropertyByNameException(string message) : base(message) { }
    }
}
