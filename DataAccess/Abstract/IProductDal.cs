using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    //3.Adım
    //IProductDal -Data access layer'ın product'ını temsil eder.
    public interface IProductDal:IEntityRepository<Product>
    {
        
    }
}
