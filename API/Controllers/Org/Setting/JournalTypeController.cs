using Accounting.Application.JournalTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class JournalTypeController(ISender sender)
        : CoreController<GetByIdJournalTypeQuery, SearchJournalTypeQuery, GetListJournalTypeQuery, JournalTypeDto>(sender)
    {
    }
}
