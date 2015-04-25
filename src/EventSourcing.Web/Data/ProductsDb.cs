using System;

namespace EventSourcing.Web.Data
{
    public class ProductsDb
    {
        public static ProductInfo[] GetProducts()
        {
            return new[]
            {
                new ProductInfo {Id = new Guid("13b3e0b9-33b2-40cd-b84c-287a6afcac1f"), Name = "Product 1"},
                new ProductInfo {Id = new Guid("33d542a4-750a-40b6-9d8b-8a4b7ccb07e7"), Name = "Product 2"},
                new ProductInfo {Id = new Guid("b00651fb-6108-4b20-9477-6a8b86b38a3b"), Name = "Product 3"},
                new ProductInfo {Id = new Guid("93d1634d-2f57-4881-836f-6f58d5f37bce"), Name = "Product 4"},
                new ProductInfo {Id = new Guid("35905632-80df-41c0-870c-65e3ddaecf6c"), Name = "Product 5"},
                new ProductInfo {Id = new Guid("fcc68408-19cd-4bfd-89e1-344330f0ca55"), Name = "Product 6"},
                new ProductInfo {Id = new Guid("595affca-3b93-410b-9d0a-1271f89de14f"), Name = "Product 7"},
                new ProductInfo {Id = new Guid("da0e6384-da7f-4690-adf9-15a13803f0b1"), Name = "Product 8"},
                new ProductInfo {Id = new Guid("075e8d0f-4726-4447-9f8a-95b71197951d"), Name = "Product 9"},
                new ProductInfo {Id = new Guid("80f2db60-ad41-4e46-a332-f502710ed725"), Name = "Product 10"}
            };
        }
    }

    public class ProductInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}