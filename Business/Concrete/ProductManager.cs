﻿//using Business.Abstract;
//using Business.BusinessAspects.Autofac;
//using Business.CCS;
//using Business.Constants;
//using Business.ValidationRules.FluentValidation;
//using Core.Aspects.Autofac.Validation;
//using Core.CrossCuttingConcerns.Validation;
//using Core.Utilities.Business;
//using Core.Utilities.Results;
//using DataAccess.Abstract;
//using Entities.Concrete;
//using Entities.DTOs;
//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Business.Concrete
//{
//    public class ProductManager : IProductService
//    //Bir EntityManager kendisi hariç başka entity'i enjekte edemez!!!
//    //Ama başka bir servisi enjekte edebilir.
//    {
//        IProductDal _productDal;
//        ICategoryService _categoryService;

//        public ProductManager(IProductDal productDal, ICategoryService categoryService)
//        {
//            _productDal = productDal;
//            _categoryService = categoryService;
//        }

//        //[SecuredOperation("product.add,admin")]
//        [ValidationAspect(typeof(ProductValidator))] //Attribute=parametreyi okuyup ilgili kodu okuyup işlem yapacak
//        public IResult Add(Product product)
//        {

//            //validation -doğrulama --Yukarıda ValidationAspects kullandığımız için bu satıra gerek kalmadı.
//            //ValidationTool.Validate(new ProductValidator(), product);

//            IResult result=BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfProductNameExists(product.ProductName),CheckIfCategoryLimitExceded());
//            if (result!=null)
//            {
//                return result;
//            }
//             _productDal.Add(product);
//             return new SuccessResult(Messages.ProductAdded);
//        }


//        //[CacheAspect] //key,value
//        public IDataResult<List<Product>> GetAll()
//        {
//            if (DateTime.Now.Hour==22)
//            {
//                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
//            }
//            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);
//        }

//        public IDataResult<List<Product>> GetAllByCategoryId(int id)
//        {
//            return new SuccessDataResult<List<Product>>( _productDal.GetAll(p => p.CategoryId == id));
//        }

//        public IDataResult<Product> GetById(int productId)
//        {
//            return new SuccessDataResult<Product>( _productDal.Get(p => p.ProductId == productId));
//        }

//        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
//        {
//            return new SuccessDataResult<List<Product>>( _productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
//        }

//        public IDataResult<List<ProductDetailDto>> GetProductDetails()
//        {
//            return new SuccessDataResult<List<ProductDetailDto>>( _productDal.GetProductDetails());
//        }

//        [ValidationAspect(typeof(ProductValidator))]
//        public IResult Update(Product product)
//        {

//            return new SuccessResult();
//        }

//        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
//        {
//            //iş kurallarını tekrar başka yerlerde yazarsan sphagetti olur
//            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
//            if (result >= 10)
//            {
//                return new ErrorResult(Messages.ProductCountOfCategoryError);
//            }
//            else
//            {
//                return new SuccessResult();
//            }
//        }

//        private IResult CheckIfProductNameExists(string productName)
//        {
//            bool result = _productDal.GetAll(p => p.ProductName == productName).Any();
//            if (!result)
//            {
//                return new ErrorResult(Messages.ProductNameAlreadyExists);
//            }
//            else
//            {
//                return new SuccessResult();
//            }
//        }

//        private IResult CheckIfCategoryLimitExceded()
//        {
//            var result = _categoryService.GetAll();
//            if (result.Data.Count>15)
//            {
//                return new ErrorResult(Messages.CategoryLimitExceded);
//            }
//            else
//            {
//                return new SuccessResult();
//            }
//        }
//    }
//}

using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        //00.25 Dersteyiz
        //Claim
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {

            //Aynı isimde ürün eklenemez
            //Eğer mevcut kategori sayısı 15'i geçtiyse sisteme yeni ürün eklenemez. ve 
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);

        }


        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        //[PerformanceAspect(5)]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 23)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            throw new NotImplementedException();
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            //Select count(*) from products where categoryId=1
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }

            return new SuccessResult();
        }
    }
}