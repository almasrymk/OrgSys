using System.Linq;
using X.PagedList;
using Domain.Entities;
using Application.DTOs;
using System.Collections.Generic;

namespace Service
{
    public class PreferenceService : BaseOrgService<PreferenceDto, Preference>
    {
        public PreferenceService(string Schema) : base(Schema) { }

        public override List<PreferenceDto> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.GetList(e => (e.Reference == "" + textSearch && e.TypeId == TypeId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.All).Select(e => e.Map<PreferenceDto>()).ToList();
        }

        public PreferenceDto GetByKey(string textSearch, string reference, long type, int userId)
        {
            return repo.Get(e => e.Key == "" + textSearch && (e.TypeId == type || type == 0) && (userId == 0 || e.UserId == userId) && (e.Reference == reference || "" + reference == "")).Map<PreferenceDto>();
        }
    }
}