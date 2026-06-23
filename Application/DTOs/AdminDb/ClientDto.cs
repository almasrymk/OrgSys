using Domain.Entities;

namespace Application.DTOs
{
    public class ClientDto : Client
    {
        public string TypeActivityName { get; set; }
       
        public string NationalityName { get; set; }
    }
}