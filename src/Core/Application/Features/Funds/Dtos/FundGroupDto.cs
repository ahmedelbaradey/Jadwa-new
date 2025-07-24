using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Funds.Dtos
{
    public class FundGroupDto
    {
        public string Title { get; set; }
        public bool HasNotification { get; set; }
        public List<SingleFundResponse> Funds { get; set; }
        public int Count { get; set; } //=> Funds?.Count ?? 0;
    }
}
