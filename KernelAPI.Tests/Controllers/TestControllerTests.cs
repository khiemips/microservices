﻿using ExplicitKernelContext;
using KernelAPI.Context;
using KernelAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace KernelAPI.Tests.Controllers
{
    [TestClass]
    public class TestControllerTests
    {
        [TestMethod]
        public void StartValidation_ReturnOkResult()
        {
            var _mockStorageService = new Mock<IStorageService>();
            var _mockCppKernelFactory = new Mock<ICppKernelFactory>();
            var _mockCppKernel = new Mock<ICppKernel>();
            var _mockConfig = new Mock<IConfiguration>();

            _mockCppKernelFactory
                .Setup(x => x.Create(It.IsAny<KernelContext>()))
                .Returns(_mockCppKernel.Object);

            _mockCppKernel
                .Setup(x => x.StartValidation())
                .Verifiable();

            var controller = new TestController(_mockStorageService.Object, _mockCppKernelFactory.Object, _mockConfig.Object);
            var result = controller.StartValidation();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void StartValidation_ThrowNullException()
        {
            var _mockStorageService = new Mock<IStorageService>();
            var _mockCppKernelFactory = new Mock<ICppKernelFactory>();
            var _mockCppKernel = new Mock<ICppKernel>();
            var _mockConfig = new Mock<IConfiguration>();

            _mockCppKernelFactory
                .Setup(x => x.Create(It.IsAny<KernelContext>()))
                .Returns<ICppKernel>(null);

            var controller = new TestController(_mockStorageService.Object, _mockCppKernelFactory.Object, _mockConfig.Object);
            var result = controller.StartValidation();
        }
    }
}
