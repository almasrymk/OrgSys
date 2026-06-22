using Application.Commands.Org.Financials.Journal.Commands;
using Application.Commands.Org.Financials.Journal.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Journals
{
    [ApiController]
    [Route("[controller]")]
    public class JournalController(ISender sender) : BaseController<GetByIdJournalQuery,SearchJournalQuery, GetListJournalQuery, CreateJournalCommand, UpdateJournalCommand, DeleteJournalCommand, DeleteListJournalCommand, GetMaxJournalQuery, JournalModelView>(sender)
    {
     

    }
}