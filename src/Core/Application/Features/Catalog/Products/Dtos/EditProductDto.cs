using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Catalog.Products.Dtos
{
    public record EditProductDto : ProductDto
    {
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
