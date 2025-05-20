using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGadevangTennisklub
{
    [TestClass]
    public class CourtServiceTest
    {
        [TestMethod]
        public void TestAddCourt()
        {
            //Arrange
            ICourtService courtService = new CourtService();
            List<TennisField> courts = courtService.GetAllCourtsAsync().GetAwaiter().GetResult();

            //Act
            int numberOfCourtsBefore = courts.Count;
            TennisField newCourt = new TennisField("TestTestTest", "Udendørs Paddle");
            bool ok = courtService.CreateCourtAsync(newCourt).GetAwaiter().GetResult();
            courts = courtService.GetAllCourtsAsync().GetAwaiter().GetResult();
            int numberOfCourtsAfter = courts.Count;

            //Assert
            Assert.AreEqual(numberOfCourtsBefore + 1, numberOfCourtsAfter);
            Assert.IsTrue(ok);
            courtService.DeleteCourtAsync(courts.Last().CourtId).GetAwaiter();
        }
    }
}
