using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTables.Queryable
{
    public interface IDataTablesQueryable<T> : IQueryable<T>
    {
        IQueryable<T> SourceQueryable { get; }
        DataTablesRequest<T> Request { get; }
    }
}
