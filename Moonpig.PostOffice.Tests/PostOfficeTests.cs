using Moonpig.PostOffice.Api.Services;
using Moonpig.PostOffice.Data;
using Moonpig.PostOffice.Data.Repositories;

namespace Moonpig.PostOffice.Tests
{
    using System;
    using System.Collections.Generic;
    using Api.Controllers;
    using Shouldly;
    using Xunit;

    public class PostOfficeTests
    {
        private static readonly ISupplierRepository _supplierRepository = new SupplierRepository(new DbContext());
        private static readonly IProductRepository _productRepository = new ProductRepository(new DbContext());
        private readonly SupplierService _supplierService = new SupplierService(_supplierRepository, _productRepository);
        private readonly DispatchDateController _controller;
        public PostOfficeTests()
        {
            _controller = new DispatchDateController(_supplierService);
        }

        [Fact]
        public void OneProductWithLeadTimeOfOneDayIncludeBankHoliday()
        {
            // arrange 
            var orderDate = new DateTime(2021, 12, 31);
            var expectedDate = new DateTime(2022, 1, 4);
            
            // act
            var date = _controller.Get(new List<int>() { 1 }, orderDate);
            date.Date.Date.ShouldBe(expectedDate);
        }
        
        [Fact]
        public void OneProductWithLeadTimeOfOneDay()
        {
            // arrange 
            var orderDate = new DateTime(2022, 1, 7);
            var expectedDate = new DateTime(2022, 1, 10);
            
            // act
            var date = _controller.Get(new List<int>() {1}, orderDate);
            
            // assert
            date.Date.Date.ShouldBe(expectedDate);
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            // arrange 
            var orderDate = new DateTime(2022, 1, 7);
            var expectedDate = new DateTime(2022, 1, 11);
            
            // act
            var date = _controller.Get(new List<int>() {2}, orderDate);
            
            // assert
            date.Date.Date.ShouldBe(expectedDate);
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            // arrange 
            var orderDate = new DateTime(2022, 1, 7);
            var expectedDate = new DateTime(2022, 1, 12);
            
            // act
            var date = _controller.Get(new List<int>() {3}, orderDate);
            
            // assert
            date.Date.Date.ShouldBe(expectedDate);
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            var date = _controller.Get(new List<int>() { 1 }, new DateTime(2018,1,26));
            date.Date.ShouldBe(new DateTime(2018, 1, 26).Date.AddDays(3));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            var date = _controller.Get(new List<int>() { 3 }, new DateTime(2018, 1, 25));
            date.Date.ShouldBe(new DateTime(2018, 1, 25).Date.AddDays(5));
        }

        [Fact]
        public void LeadTimeIsNotCountedOverSaturday()
        {
            // arrange
            var orderDate = new DateTime(2018, 1, 6);
            var expectedDispatchDate = orderDate.AddDays(3);
            
            // act
            var response = _controller.Get(new List<int>() { 1 }, orderDate);
            
            // assert
            response.Date.ShouldBe(expectedDispatchDate);
        }
        
        [Fact]
        public void LeadTimeIsNotCountedOverSunday()
        {
            // arrange
            var orderDate = new DateTime(2018, 1, 7);
            var expectedDispatchDate = orderDate.AddDays(2);
            
            // act
            var response = _controller.Get(new List<int>() { 1 }, orderDate);
            
            // assert
            response.Date.ShouldBe(expectedDispatchDate);
        }
        
        [Fact]
        public void SixDaysOfLeadTimeOverMultipleWeeks()
        {
            // arrange
            var orderDate = new DateTime(2018, 1, 5);
            var expectedDispatchDate = orderDate.AddDays(10);
            
            // act
            var response = _controller.Get(new List<int>() { 9 }, orderDate);
            
            // assert
            response.Date.ShouldBe(expectedDispatchDate);
        }
        
        [Fact]
        public void ElevenDaysOfLeadTimeOverMultipleWeeks()
        {
            // arrange
            var orderDate = new DateTime(2018, 1, 5);
            var expectedDispatchDate = orderDate.AddDays(17);
            
            // act
            var response = _controller.Get(new List<int>() { 10 }, orderDate);
            
            // assert
            response.Date.ShouldBe(expectedDispatchDate);
        }
    }
}
