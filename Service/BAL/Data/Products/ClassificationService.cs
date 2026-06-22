using Domain.Entities;
using Application.DTOs;
using System;
using System.Linq.Expressions;

namespace Service
{
    public class ClassificationService : BaseOrgService<ClassificationModelView, Classification>
    {
        public ClassificationService(string Schema) : base(Schema) { }

        public override Expression<Func<Classification, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => "" + textSearch == "" || e.Name.ToLower().Contains(textSearch.ToLower()) || e.Code.ToLower().Contains(textSearch.ToLower());
        }
    }
}