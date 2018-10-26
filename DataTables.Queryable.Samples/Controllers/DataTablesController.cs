using System;
using System.Web.Mvc;

using Newtonsoft.Json;

using DataTables.Queryable.Samples.Database;
using DataTables.Queryable.Samples.Models;
using System.Data.Entity.SqlServer;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.SqlServerCompact;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataTables.Queryable.Samples.Controllers
{
    public class DataTablesController : Controller
    {
        /// <summary>
        /// Basic usage sample
        /// </summary>
        public JsonResult Sample1()
        {
            var request = new DataTablesRequest<Person>(Request.QueryString);

            AddDataTablesLogging(request);

            using (var ctx = new DatabaseContext())
            {
                var persons = ctx.Persons.Include("Office.Address").ToPagedList(request);
                return JsonDataTable(persons, request.Draw);
            }
        }

        /// <summary>
        /// Custom search predicate
        /// </summary>
        public JsonResult Sample2()
        {
            var request = new DataTablesRequest<Person>(Request.QueryString);

            AddDataTablesLogging(request);

            request.Columns[p => p.Name]
                .GlobalSearchPredicate = (p) => p.Name.StartsWith(request.GlobalSearchValue);
            request.Columns[p => p.Position]
                .GlobalSearchPredicate = (p) => p.Position.StartsWith(request.GlobalSearchValue);
            request.Columns[p => p.Office.Name]
                .GlobalSearchPredicate = (p) => p.Office.Name.StartsWith(request.GlobalSearchValue);

            using (var ctx = new DatabaseContext())
            {
                var persons = ctx.Persons.Include("Office").ToPagedList(request);
                return JsonDataTable(persons, request.Draw);
            }
        }

        /// <summary>
        /// Custom filtering with disabled search
        /// </summary>
        public JsonResult Sample3()
        {
            var request = new DataTablesRequest<Person>(Request.QueryString);

            AddDataTablesLogging(request);

            // get custom parameters from the request
            const string dateFormat = "yyyy-MM-dd";
            DateTime dateFrom = DateTime.ParseExact(request["dateFrom"], dateFormat, null);
            DateTime dateTo = DateTime.ParseExact(request["dateTo"], dateFormat, null);

            // define custom predicate to filter results:
            // get all persons hired in period between "dateFrom" and "dateTo"
            request.CustomFilterPredicate =
                (s) => s.StartDate >= dateFrom && s.StartDate <= dateTo;

            using (var ctx = new DatabaseContext())
            {
                var persons = ctx.Persons.Include("Office").ToPagedList(request);
                return JsonDataTable(persons, request.Draw);
            }
        }

        /// <summary>
        /// AJAX POST
        /// </summary>
        public JsonResult Sample4(DataTablesAjaxPostModel model)
        {
            var request = new DataTablesRequest<Person>(model);

            AddDataTablesLogging(request);

            using (var ctx = new DatabaseContext())
            {
                var persons = ctx.Persons.Include("Office.Address").ToPagedList(request);
                return JsonDataTable(persons, request.Draw);
            }
        }

        /// <summary>
        /// Async processing
        /// </summary>
        public async Task<JsonResult> Sample5()
        {
            var request = new DataTablesRequest<Person>(Request.QueryString);

            AddDataTablesLogging(request);

            using (var ctx = new DatabaseContext())
            {
                var persons = await ctx.Persons.Include("Office.Address").ToPagedListAsync(request);
                return JsonDataTable(persons, request.Draw);
            }
        }

        /// <summary>
        /// Helper method that converts <see cref="IPagedList{T}"/> collection to the JSON-serialized object in datatables-friendly format.
        /// </summary>
        /// <param name="model"><see cref="IPagedList{T}"/> collection of items</param>
        /// <param name="draw">Draw counter (optional).</param>
        /// <returns>JsonNetResult instance to be sent to datatables</returns>
        protected JsonNetResult JsonDataTable<T>(IPagedList<T> model, int draw = 0)
        {
            JsonNetResult jsonResult = new JsonNetResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jsonResult.Data = new
            {
                draw = draw,
                recordsTotal = model.TotalCount,
                recordsFiltered = model.TotalCount,
                data = model
            };
            return jsonResult;
        }

        /// <summary>
        /// Adds queries logging for DataTables.Queryable.
        /// </summary>
        private void AddDataTablesLogging<T>(DataTablesRequest<T> request)
        {
#if TRACE
            request.Log = (s) => Trace.WriteLine(s);
#endif
        }

        /// <summary>
        /// JsonNet-based JsonResult
        /// </summary>
        protected class JsonNetResult : JsonResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");

                var response = context.HttpContext.Response;

                response.ContentType = !String.IsNullOrEmpty(ContentType)
                    ? ContentType
                    : "application/json";

                if (ContentEncoding != null)
                    response.ContentEncoding = ContentEncoding;

                var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented);
                response.Write(serializedObject);
            }
        }
    }   
}