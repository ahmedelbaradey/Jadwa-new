

namespace Application.Features.Funds.Dtos
{
    public record GetFundResponse : AddFundRequest
    {
        public string AttachmentPath { get; set; }
        public string AttachmentName { get; set; }

    }
}
