using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Products;

namespace  Abstraction.Contracts.Service.Catalog
{
    public interface IProductService : IBaseService<Product>    
    {
    
    }
}
