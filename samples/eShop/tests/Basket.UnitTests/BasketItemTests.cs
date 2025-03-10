using eShop.Basket.API.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basket.UnitTests
{
    [TestClass]
    public class BasketItemTests
    {
        [TestMethod]
        public void Validate_QuantityLessThanOne_ShouldReturnValidationError()
        {
            // Arrange
            var basketItem = new BasketItem { Quantity = 0 };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(basketItem, null, null);
            Validator.TryValidateObject(basketItem, validationContext, validationResults, true);

            // Assert
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("Invalid number of units", validationResults[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_QuantityEqualToOne_ShouldNotReturnValidationError()
        {
            // Arrange
            var basketItem = new BasketItem { Quantity = 1 };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(basketItem, null, null);
            Validator.TryValidateObject(basketItem, validationContext, validationResults, true);

            // Assert
            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void Validate_QuantityGreaterThanOne_ShouldNotReturnValidationError()
        {
            // Arrange
            var basketItem = new BasketItem { Quantity = 5 };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(basketItem, null, null);
            Validator.TryValidateObject(basketItem, validationContext, validationResults, true);

            // Assert
            Assert.AreEqual(0, validationResults.Count);
        }
    }
}
