using Abstraction.Base.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Funds.Dtos
{
    public record ExistDateRequest : BaseDto
    {
        public DateTime ExitDate { get; set; }
    }
}
