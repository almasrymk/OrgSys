using Application.Commands.Org.Setting.JournalType.Queries;
using Application.DTOs;
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
