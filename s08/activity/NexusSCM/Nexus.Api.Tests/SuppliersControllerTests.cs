﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.API.Controllers;
using Nexus.API.Data;
using Nexus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Api.Tests
{
    public class SuppliersControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
        [Fact]
        public async Task GetSupplier_WithExisitingId_ShouldReturnOkResultWithSupplier()
        {
            var dbContext = GetDbContext();
            var testSupplier = new Supplier
            {
                Id = 1,
                Name = "Test Corporation",
                Email = "test@corporation.com"
            };
            dbContext.Suppliers.Add(testSupplier);
            await dbContext.SaveChangesAsync();
            var controller = new SuppliersController(dbContext);
            var actionResult = await controller.GetSupplier(1);
            var OkResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedSupplier = Assert.IsType<Supplier>(OkResult.Value);
            Assert.Equal(1, returnedSupplier.Id);
            Assert.Equal("Test Corporation", returnedSupplier.Name);
        }


        [Fact]
        public async Task GetSupplier_WithNonExisitingId_ShouldReturnNotFoundWithSupplier()
        {
            var dbContext = GetDbContext();
            var controller = new SuppliersController(dbContext);
            var actionResult = await controller.GetSupplier(99);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Theory]
        [InlineData(null, "test@email.com")]
        [InlineData("Test Supplier", null)]
        [InlineData("Test Supplier", "test@supplier.com")]
        public async Task PostSupplier_WithInvalidModel_ShouldReturnBadRequest(string name, string email)
        {
            var dbContext = GetDbContext();
            var controller = new SuppliersController(dbContext);
            var invalidSupplier = new Supplier
            {
                Name = name,
                Email = email
            };
            controller.ModelState.AddModelError("Error", "Model state is invalid for test");
            var actionResult = await controller.PostSupplier(invalidSupplier);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }
    }
}
