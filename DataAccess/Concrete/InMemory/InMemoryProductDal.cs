using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.InMemory
{
    //4. Adım
    //Bellek üzerinde Product ile ilgili veri erişim kodlarının yazılacağı yer.
    public class InMemoryProductDal : IProductDal
    {
        List<Product> _products; //Bu nesneyi classın içinde ama metodların dışında tanımladığımız için bu nesneye global değişken denir.
        public InMemoryProductDal() //ctor+tab+tab
        {
            _products = new List<Product> {
                new Product{ProductId=1,CategoryId=1,ProductName="Bardak",UnitPrice=15,UnitInStock=5},
                new Product{ProductId=2,CategoryId=3,ProductName="Kamera",UnitPrice=500,UnitInStock=3},
                new Product{ProductId=3,CategoryId=4,ProductName="Telefon",UnitPrice=150,UnitInStock=2},
                new Product{ProductId=4,CategoryId=5,ProductName="Klavye",UnitPrice=150,UnitInStock=65},
                new Product{ProductId=5,CategoryId=6,ProductName="Fare",UnitPrice=85,UnitInStock=1}
            };
        }
        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {
            Product productToDelete = null;
            //foreach (var p in _products)
            //{
            //    if (product.ProductId==p.ProductId)
            //    {
            //        productToDelete = p;
            //    }
            //}

            //bu tek satırlık kod yukarıdaki foreach'le aynı işi tek satırda yapıyor
            productToDelete = _products.SingleOrDefault(p=>p.ProductId==product.ProductId);
            _products.Remove(productToDelete);
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return _products;
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllByCategory(int categoryId)
        {
            //verilen şarta uyanları yeni bir liste yapıyor.
            return _products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public void Update(Product product)
        {
            //Gönderilen ürün idsine sahip olan listedeki ürünü bul
            Product productToUpdate = _products.SingleOrDefault(p => p.ProductId == product.ProductId);
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitInStock = product.UnitInStock;
        }
    }
}
