using GadevangTennisklub2025.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace UnitTestGadevangTennisklub
{
    [TestClass]
    public sealed class MemberTest
    {
        [TestMethod]
        public void Member_Creation_ValidParameters_ShouldSucceed()
        {
            // Arrange
            var mem = new Member("Hank Green", "Hank Green The Third", new DateOnly(1993, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(mem);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(mem, context, results, true);

            // Assert
            Assert.IsTrue(isValid, "Expected member to pass validation.");
        }

        [TestMethod]
        public void Member_Creation_NameTooShort_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "H", new DateOnly(1993, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("navn", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void Member_Creation_NameContainsNumber_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "Hank 2", new DateOnly(1993, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("navn", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void Member_Creation_NameContainsSymbol_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "Hank %", new DateOnly(1993, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("navn", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void Member_Creation_ValidParametersWithDashAndDotDk_ShouldSucceed()
        {
            // Arrange
            var mem = new Member("Hank Green", "Hank-Green The Third", new DateOnly(1993, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.dk", "123", "København", "Ja", 100);

            var context = new ValidationContext(mem);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(mem, context, results, true);

            // Assert
            Assert.IsTrue(isValid, "Expected member to pass validation.");
        }

        [TestMethod]
        public void Member_Creation_BirthdayInFuture_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "Hank Green the third", new DateOnly(4000, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("fødselsdag", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void Member_Creation_BirthdayTooOld_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "Hank Green the third", new DateOnly(1500, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("fødselsdag", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void Member_Creation_PhoneNot8Digits_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "Hank Green the third", new DateOnly(1975, 06, 12), "Senior", "Valby", "3030302000", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("telefon", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void Member_Creation_PhoneHasLetter_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "Hank Green the third", new DateOnly(1975, 06, 12), "Senior", "Valby", "3030A020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("telefon", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void Member_Creation_EmailDoesNotEndWithDotComOrDk_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "Hank Green the third", new DateOnly(1975, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("mail", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void Member_Creation_EmailDoesNotHaveAdd_ShouldFailValidation()
        {
            // Arrange
            var member = new Member("Hank Green", "Hank Green the third", new DateOnly(1975, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd.gmail.com", "123", "København", "Ja", 100);

            var context = new ValidationContext(member);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(member, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.ErrorMessage.Contains("mail", StringComparison.OrdinalIgnoreCase)));
        }
    }
}
