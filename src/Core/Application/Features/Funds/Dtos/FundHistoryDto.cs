using Abstraction.Base.Dto;


namespace Application.Features.Funds.Dtos
{
    public record class FundHistoryDto :BaseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
