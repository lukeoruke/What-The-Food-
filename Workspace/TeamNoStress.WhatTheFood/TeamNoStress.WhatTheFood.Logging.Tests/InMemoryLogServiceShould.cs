using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNoStress.WhatTheFood.Logging.Implementations;
// 3 hour mark for database connection
namespace TeamNoStress.WhatTheFood.Logging.Tests
{
    [TestClass]
    public class InMemoryLogServiceShould
    {
        [TestMethod]
        public void Instantiate()
        {
            // Arrange
            var expected = typeof(ILogService);
            // Act
            var instance = new InMemoryLogService();
            // Assert
            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, expected);
        }


        [TestMethod]
        public void getNoLogs()
        {
            //Red-Green-Refactor Process. Update units test and code until unit test is green. Then refactor for performance if conditons require/allow
            //AKA Test Driven development(TDD)

            // tripple A format
            // Arrange (What needs to be set up to run the test)
            var logService = new InMemoryLogService();
            var expectedCount = 0;
            // Act
            var actualFetch = logService.GetAllLogs();

            // Assert
            Assert.IsTrue(actualFetch.Count == expectedCount);
        }

        [TestMethod]
        public void LogValidDescription()
        {
            //Red-Green-Refactor Process. Update units test and code until unit test is green. Then refactor for performance if conditons require/allow
            //AKA Test Driven development(TDD)
            
            // tripple A format
            // Arrange (What needs to be set up to run the test)
            var logService = new InMemoryLogService();
            var expectedCount = 1;
            var expectedLogMessage = "Test Log Entry";
            // Act
            var actual = logService.Log("Test Log Entry");
            var actualFetch = logService.GetAllLogs();

            // Assert
            Assert.IsTrue(actualFetch.Count == expectedCount);
            Assert.IsTrue(actualFetch[0].Contains(expectedLogMessage));
        }
    }
}