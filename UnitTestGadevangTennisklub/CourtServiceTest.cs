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
            List<TennisField> courts = courtService.GetAllCourtsAsync().Result;

            //Act
            int numberOfCourtsBefore = courts.Count;
            TennisField newCourt = new TennisField(30, "TestTestTest", "Udendørs Paddle");
            bool ok = courtService.CreateCourtAsync(newCourt).Result;
            courts = courtService.GetAllCourtsAsync().Result;
            int numberOfCourtsAfter = courts.Count;
            TennisField c = courtService.DeleteCourtAsync(newCourt.CourtId).Result;

            //Assert
            Assert.AreEqual(numberOfCourtsBefore + 1, numberOfCourtsAfter);
            Assert.IsTrue(ok);
            Assert.AreEqual(c.CourtId, newCourt.CourtId);
        }
    }
}
