using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.MVC.Sample.Models;
using DataTables.Queryable;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspNetCore.MVC.Sample.Controllers
{
    [ApiController]
    [Route("[Controller]/[action]")]
    public class DataTablesController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<DataTablesController> _logger;

        public DataTablesController(
            DatabaseContext context,
            ILogger<DataTablesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Basic usage sample
        /// </summary>
        [HttpGet]
        public DataTableResponseViewModel<Person> Sample1()
        {
            var request = new DataTablesRequest<Person>(Request.QueryString.Value);

            AddDataTablesLogging(request);

            var persons = _context.Persons.Include("Office.Address").ToPagedList(request);
            return BuildViewModel(persons, request.Draw);
        }

        /// <summary>
        /// Custom search predicate
        /// </summary>
        [HttpGet]
        public DataTableResponseViewModel<Person> Sample2()
        {
            var request = new DataTablesRequest<Person>(Request.QueryString.Value);

            AddDataTablesLogging(request);

            request.Columns[p => p.Name]
                .GlobalSearchPredicate = (p) => p.Name.StartsWith(request.GlobalSearchValue);
            request.Columns[p => p.Position]
                .GlobalSearchPredicate = (p) => p.Position.StartsWith(request.GlobalSearchValue);
            request.Columns[p => p.Office.Name]
                .GlobalSearchPredicate = (p) => p.Office.Name.StartsWith(request.GlobalSearchValue);

            var persons = _context.Persons.Include("Office").ToPagedList(request);
            return BuildViewModel(persons, request.Draw);
        }

        /// <summary>
        /// Custom filtering with disabled search
        /// </summary>       
        [HttpGet]
        public DataTableResponseViewModel<Person> Sample3()
        {
            var request = new DataTablesRequest<Person>(Request.QueryString.Value);

            AddDataTablesLogging(request);

            // get custom parameters from the request
            const string dateFormat = "yyyy-MM-dd";
            DateTime dateFrom = DateTime.ParseExact(request["dateFrom"], dateFormat, null);
            DateTime dateTo = DateTime.ParseExact(request["dateTo"], dateFormat, null);

            // define custom predicate to filter results:
            // get all persons hired in period between "dateFrom" and "dateTo"
            request.CustomFilterPredicate =
                (s) => s.StartDate >= dateFrom && s.StartDate <= dateTo;

            var persons = _context.Persons.Include("Office").ToPagedList(request);
            return BuildViewModel(persons, request.Draw);
        }

        /// <summary>
        /// AJAX POST
        /// </summary>
        [HttpPost]
        public DataTableResponseViewModel<Person> Sample4(DataTablesAjaxPostModel model)
        {
            var request = new DataTablesRequest<Person>(model);

            AddDataTablesLogging(request);

            var persons = _context.Persons.Include("Office.Address").ToPagedList(request);
            return BuildViewModel(persons, request.Draw);
        }

        /// <summary>
        /// Async processing
        /// </summary>
        [HttpGet]
        public async Task<DataTableResponseViewModel<Person>> Sample5(CancellationToken cancellationToken)
        {
            var request = new DataTablesRequest<Person>(Request.QueryString.Value);

            AddDataTablesLogging(request);

            var persons = await _context.Persons.Include("Office.Address").ToPagedListAsync(request, cancellationToken);
            return BuildViewModel(persons, request.Draw);
        }

        /// <summary>
        /// Helper method that converts <see cref="IPagedList{T}"/> collection to the view model in datatables-friendly format.
        /// </summary>
        /// <param name="model"><see cref="IPagedList{T}"/> collection of items</param>
        /// <param name="draw">Draw counter (optional).</param>
        /// <returns>JsonNetResult instance to be sent to datatables</returns>
        protected DataTableResponseViewModel<T> BuildViewModel<T>(IPagedList<T> model, int draw = 0)
        {
            return new DataTableResponseViewModel<T>
            {
                draw = draw,
                recordsTotal = model.TotalCount,
                recordsFiltered = model.TotalCount,
                data = model
            };
        }

        /// <summary>
        /// Adds queries logging for DataTables.Queryable.
        /// </summary>
        private void AddDataTablesLogging<T>(DataTablesRequest<T> request)
        {
            _logger.LogTrace("{DataTablesRequest}", request);
        }        

    }
}
