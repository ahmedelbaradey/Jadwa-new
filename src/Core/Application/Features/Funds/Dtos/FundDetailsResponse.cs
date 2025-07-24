using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Funds.Dtos
{
    public record class FundDetailsResponse : FundDto
    {
        public int PropertiesNumber { get; set; }
        public int ResolutionsCount { get; set; }
        public int EvaluationsCount { get; set; }
        public int DocumentsCount { get; set; }
        public int MeetingsCount { get; set; }
        public int MembersCount { get; set; }

        public int MembersNotificationCount { get; set; }
        public int ResolutionsNotificationCount { get; set; }
        public int EvaluationsNotificationCount { get; set; }
        public int DocumentsNotificationCount { get; set; }
        public int MeetingsNotificationCount { get; set; }
        public List<FundNotificationDto> FundNotifications { get; set; }
        public List<FundHistoryDto> FundHistory { get; set; }
    }
}
