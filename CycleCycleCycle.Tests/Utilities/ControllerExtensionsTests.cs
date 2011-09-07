using System;
using System.Security;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CycleCycleCycle.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CycleCycleCycle.Tests.Utilities
{
    [TestClass]
    public class ControllerExtensionsTests
    {
        #region ValidateSortOrder Tests

        [TestMethod]
        public void Test_ValidateSortOrder_ValidatesAscending()
        {
            // Act
            ControllerExtensions.ValidateSortOrder(null, "asc");

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_ValidateSortOrder_ValidatesMixedCaseAscending()
        {
            // Act
            ControllerExtensions.ValidateSortOrder(null, "aSc");

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_ValidateSortOrder_ValidatesDescending()
        {
            // Act
            ControllerExtensions.ValidateSortOrder(null, "desc");

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_ValidateSortOrder_ValidatesMixedCaseDescending()
        {
            // Act
            ControllerExtensions.ValidateSortOrder(null, "DeSc");

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_ValidateSortOrder_ThrowsExceptionOnInvalidSortOrder()
        {
            // Act and assert
            bool exceptionRaised = false;
            try
            {
                ControllerExtensions.ValidateSortOrder(null, "zaphod");
            }
            catch (SecurityException)
            {
                exceptionRaised = true;
            }

            // Assert
            Assert.IsTrue(exceptionRaised);
        }

        #endregion
    }
}
