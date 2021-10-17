using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Utility
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametersPaginationHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if(httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }
            double amount = await queryable.CountAsync();
            httpContext.Response.Headers.Add("totalAmountOfRecords", amount.ToString());

        }
    }
}
