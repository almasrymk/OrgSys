using Domain.Entities;
using Application.DTOs;
using System.Linq.Expressions;
using System;

namespace Service
{
    public class SafeService : BaseOrgService<SafeModelView, Safe>
    {
        public SafeService(string Schema) : base(Schema) { }

        public override Expression<Func<Safe, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }
    }
}