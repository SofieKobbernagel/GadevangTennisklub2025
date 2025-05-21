using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestGadevangTennisklub
{
    [TestClass]
    public class TeamServiceTest
    {
        private readonly TeamService _service = new TeamService();

        [TestMethod]
        public async Task GetAllTeamsAsync_ShouldReturnTeams()
        {
            var result = await _service.GetAllTeamsAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 0);
        }

        [TestMethod]
        public async Task CreateTeamAsync_ShouldReturnTrue_WhenTeamIsCreated()
        {
            // Arrange
            var team = new Team
            {
                Id = 999,
                Name = "Testhold",
                Description = "Test beskrivelse",
                MembershipType = "Seniorer",
                DayOfWeek = 3,
                TimeOfDay = new TimeOnly(17, 30),
                Length = 1.0,
                AttendeeRange = new[] { 2, 12 },
                Trainer = new Coach { Coach_Id = 3 }  // Use existing coach ID from your DB
            };

            // Act
            bool result = await _service.CreateTeamAsync(team);

            // Assert
            Assert.IsTrue(result, "Expected CreateTeamAsync to return true when team is created successfully.");
        }



        [TestMethod]
        public async Task DeleteTeamAsync_ShouldRemoveTeam()
        {
            // Arrange
            TeamService teamService = new TeamService();

            var team = new Team
            {
                Id = 9999, // Unique temporary ID
                Name = "Temp Deletion Team",
                MembershipType = "Seniorer",
                Length = 1,
                TimeOfDay = new TimeOnly(10, 0),
                DayOfWeek = 1, // Tuesday if your system uses 0 = Monday
                AttendeeRange = new int[] { 5, 15 },
                Description = "Test team for deletion",
                Trainer = new Coach { Coach_Id = 3 } // Ensure this coach exists
            };

            // Act
            bool created = await teamService.CreateTeamAsync(team);
            Assert.IsTrue(created, "Team creation failed during setup for deletion test.");

            Team deletedTeam = await teamService.DeleteTeamAsync(team.Id);
            Assert.IsNotNull(deletedTeam, "DeleteTeamAsync should return the deleted team.");

            Team checkTeam = await teamService.GetTeamFromIdAsync(team.Id);

            // Assert
            Assert.IsNull(checkTeam, "Team should no longer exist in the database after deletion.");
        }


        [TestMethod]
        public async Task UpdateTeamAsync_ShouldModifyTeam()
        {
            // Arrange
            TeamService teamService = new TeamService();

            var team = new Team
            {
                Id = 8888, // Unique test ID
                Name = "Update Test Team",
                MembershipType = "Seniorer",
                Length = 1.0,
                TimeOfDay = new TimeOnly(14, 0),
                DayOfWeek = 2, // Wednesday
                AttendeeRange = new int[] { 10, 20 },
                Description = "Before update",
                Trainer = new Coach { Coach_Id = 3 } // Must be a valid existing coach ID
            };

            // Act: Create the team
            bool created = await teamService.CreateTeamAsync(team);
            Assert.IsTrue(created, "Team was not created successfully for update test.");

            // Update values
            team.Description = "Updated description";
            team.Length = 1.5;

            bool updated = await teamService.UpdateTeamAsync(team);
            Assert.IsTrue(updated, "Team was not updated successfully.");

            // Reload and Assert
            Team updatedTeam = await teamService.GetTeamFromIdAsync(team.Id);
            Assert.IsNotNull(updatedTeam, "Updated team could not be retrieved from DB.");
            Assert.AreEqual("Updated description", updatedTeam.Description, "Description did not update as expected.");
            Assert.AreEqual(1.5, updatedTeam.Length, "Length did not update correctly.");
        }

        [TestMethod]
        public async Task GetTeamByIdAsync_ShouldReturnCorrectTeam()
        {
            var team = new Team
            {
                Name = "LookupTeam",
                Description = "Searchable",
                MembershipType = "Seniorer",
                DayOfWeek = 4,
                TimeOfDay = new TimeOnly(10, 0, 0),
                Length = 1.25,
                AttendeeRange = new int[] { 1, 8 }
            };

            await _service.CreateTeamAsync(team);
            var createdTeam = (await _service.GetAllTeamsAsync()).LastOrDefault(t => t.Name == "LookupTeam");

            var fetched = await _service.GetTeamFromIdAsync(createdTeam.Id);
            Assert.IsNotNull(fetched);
            Assert.AreEqual("LookupTeam", fetched.Name);

            await _service.DeleteTeamAsync(createdTeam.Id);
        }
    }
}
