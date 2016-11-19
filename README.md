# DataTables.Queryable

## What is it?
It is a .Net library for clever processing of requests from [datatables.net](https://www.datatables.net/) jQuery plugin on the server side (ASP.NET, [Nancy](https://github.com/NancyFx/Nancy/) or any other web server).

The library significally [reduces boilerplate code](https://github.com/AlexanderKrutov/DataTables.Queryable/wiki/Boilerplate-code-reducing) and helps to avoid writing same logic of parsing requests for different model types.

## How to use it?
```csharp
// ASP.NET action handler inside a controller:
public JsonResult DataTablesRequestAction()
{
    // make a DataTablesRequest object from the incoming Http query string
    var request = new DataTablesRequest<Person>(Request.QueryString);
    
    using (var ctx = new DatabaseContext())
    {
        // take persons from database and apply request filter for the records
        var persons = ctx.Persons
            .Filter(request)
            .ToPagedList(request.PageNumber, request.PageSize);
     
        // push back a result in JSON form applicable for datatables.net
        return JsonDataTable(persons);
    }
}
```
Need more info? [Welcome to the wiki](https://github.com/AlexanderKrutov/DataTables.Queryable/wiki/).

## How it works?
1. DataTables.Queryable parses incoming Http request and extracts related parameters (search text, columns ordering info, page number and number of records per page and etc.);
1. Dynamically builds an expression tree from the request parameters and information about model type `T` using reflection;
1. Filters provided `IQueryable<T>` with the expression tree.

Take a closer look [what happens inside](https://github.com/AlexanderKrutov/DataTables.Queryable/How-it-works/).

## Features
* Global search, individual column search, ordering by one or many columns, pagination
* Custom request parameters
* Custom search predicates
* Lazy data loading from provided `IQueryable<T>` (only filtered records will be extracted)
* Compatible with [Entity Framework](https://github.com/aspnet/EntityFramework6)

## License
DataTables.Queryable is licensed under [MIT license](LICENSE).
