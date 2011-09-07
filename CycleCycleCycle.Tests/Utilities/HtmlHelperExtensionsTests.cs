using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CycleCycleCycle.Tests.Utilities
{
    [TestClass]
    public class HtmlHelperExtensionsTests
    {

        #region Test LongDistance

        [TestMethod]
        public void Test_LongDistance_ReturnsKilometerFor1000Meters()
        {
            // Act
            string result = HtmlHelperExtensions.LongDistance(null, 1000);

            // Assert
            Assert.AreEqual("1 km", result);
        }

        [TestMethod]
        public void Test_LongDistance_RoundsDown()
        {
            // Act
            string result = HtmlHelperExtensions.LongDistance(null, 2400);

            // Assert
            Assert.AreEqual("2 km", result);
        }

        [TestMethod]
        public void Test_LongDistance_RoundsUp()
        {
            // Act
            string result = HtmlHelperExtensions.LongDistance(null, 2600);

            // Assert
            Assert.AreEqual("3 km", result);
        }

        #endregion

        #region Distance

        [TestMethod]
        public void Test_Distance_Returns1000MetersFor1000Meters()
        {
            // Act
            string result = HtmlHelperExtensions.Distance(null, 1000);

            // Assert
            Assert.AreEqual("1000 m", result);
        }

        [TestMethod]
        public void Test_Distance_RoundsDown()
        {
            // Act
            string result = HtmlHelperExtensions.Distance(null, 2100.1);

            // Assert
            Assert.AreEqual("2100 m", result);
        }

        [TestMethod]
        public void Test_Distance_RoundsUp()
        {
            // Act
            string result = HtmlHelperExtensions.Distance(null, 2100.6);

            // Assert
            Assert.AreEqual("2101 m", result);
        }

        #endregion

        #region Rating Tests

        [TestMethod]
        public void Test_AverageRating_Succeeds()
        {
            // Act
            string result = HtmlHelperExtensions.Rating(null, 3);

            // Assert
            Assert.AreEqual("3/5", result);
        }

        [TestMethod]
        public void Test_AverageRating_RoundsDown()
        {
            // Act
            string result = HtmlHelperExtensions.Rating(null, 4.8);

            // Assert
            Assert.AreEqual("4/5", result);
        }

        [TestMethod]
        public void Test_AverageRating_NullReturnsUnrated()
        {
            // Act
            string result = HtmlHelperExtensions.Rating(null, null);

            // Assert
            Assert.AreEqual("Unrated", result);
        }

        #endregion

        #region TimeTaken tests

        [TestMethod]
        public void Test_TimeTaken_NullReturnsNotRecorder()
        {
            // Act
            string result = HtmlHelperExtensions.TimeTaken(null, null);

            // Assert
            Assert.AreEqual("No time recorded", result);
        }

        [TestMethod]
        public void Test_TimeTaken_ReturnsFormattedString()
        {
            // Act
            string result = HtmlHelperExtensions.TimeTaken(null, new TimeSpan(1,2,3,4));

            // Assert
            Assert.AreEqual("2 hours 3 minutes 4 seconds", result);
        }

        #endregion
    }
}
