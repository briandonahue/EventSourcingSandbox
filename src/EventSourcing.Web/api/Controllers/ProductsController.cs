using System.Web.Http;
using EventSourcing.Web.Data;

namespace EventSourcing.Web.api.Controllers
{
    public class ProductsController: ApiController
    {
        public ProductInfo[] Get()
        {
            return ProductsDb.GetProducts();
        }
         
    }
}