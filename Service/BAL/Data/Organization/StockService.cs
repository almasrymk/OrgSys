using Domain.Entities;
using Application.DTOs;
using System.Linq.Expressions;
using System;

namespace Service
{
    public class StockService : BaseOrgService<StockDto, Stock>
    {
        public StockService(string Schema) : base(Schema , "Branch") { }

        public override Expression<Func<Stock, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }
    }
}