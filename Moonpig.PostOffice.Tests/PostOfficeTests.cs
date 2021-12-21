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
        private readonly ISupplierRepository _supplierRepository = new SupplierRepository(new DbContext());
        
        [Fact]
        public void OneProductWithLeadTimeOfOneDay()
        {
            var controller = new DispatchDateController(_supplierRepository);
            var date = controller.Get(new List<int>() {1}, DateTime.Now);
            date.Date.Date.ShouldBe(DateTime.Now.Date.AddDays(1));
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            var controller = new DispatchDateController(_supplierRepository);
            var date = controller.Get(new List<int>() { 2 }, DateTime.Now);
            date.Date.Date.ShouldBe(DateTime.Now.Date.AddDays(2));
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            var controller = new DispatchDateController(_supplierRepository);
            var date = controller.Get(new List<int>() { 3 }, DateTime.Now);
            date.Date.Date.ShouldBe(DateTime.Now.Date.AddDays(3));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            var controller = new DispatchDateController(_supplierRepository);
            var date = controller.Get(new List<int>() { 1 }, new DateTime(2018,1,26));
            date.Date.ShouldBe(new DateTime(2018, 1, 26).Date.AddDays(3));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            var controller = new DispatchDateController(_supplierRepository);
            var date = controller.Get(new List<int>() { 3 }, new DateTime(2018, 1, 25));
            date.Date.ShouldBe(new DateTime(2018, 1, 25).Date.AddDays(5));
        }

        [Fact]
        public void LeadTimeIsNotCountedOverSaturday()
        {
            // arrange
            var controller = new DispatchDateController(_supplierRepository);
            var orderDate = new DateTime(2018, 1, 6);
            var expectedDispatchDate = orderDate.AddDays(3);
            
            // act
            var response = controller.Get(new List<int>() { 1 }, orderDate);
            
            // assert
            response.Date.ShouldBe(expectedDispatchDate);
        }
        
        [Fact]
        public void LeadTimeIsNotCountedOverSunday()
        {
            // arrange
            var controller = new DispatchDateController(_supplierRepository);
            var orderDate = new DateTime(2018, 1, 7);
            var expectedDispatchDate = orderDate.AddDays(2);
            
            // act
            var response = controller.Get(new List<int>() { 1 }, orderDate);
            
            // assert
            response.Date.ShouldBe(expectedDispatchDate);
        }
        
        [Fact]
        public void SixDaysOfLeadTimeOverMultipleWeeks()
        {
            // arrange
            var controller = new DispatchDateController(_supplierRepository);
            var orderDate = new DateTime(2018, 1, 5);
            var expectedDispatchDate = orderDate.AddDays(10);
            
            // act
            var response = controller.Get(new List<int>() { 9 }, orderDate);
            
            // assert
            response.Date.ShouldBe(expectedDispatchDate);
        }
        
        [Fact]
        public void ElevenDaysOfLeadTimeOverMultipleWeeks()
        {
            // arrange
            var controller = new DispatchDateController(_supplierRepository);
            var orderDate = new DateTime(2018, 1, 5);
            var expectedDispatchDate = orderDate.AddDays(17);
            
            // act
            var response = controller.Get(new List<int>() { 10 }, orderDate);
            
            // assert
            response.Date.ShouldBe(expectedDispatchDate);
        }
    }
}
