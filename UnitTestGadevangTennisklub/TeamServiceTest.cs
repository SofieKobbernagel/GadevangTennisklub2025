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
            var team = new Team
            {
                Name = "Testhold",
                Description = "Test beskrivelse",
                MembershipType = "Seniorer",
                DayOfWeek = 3,
                TimeOfDay = new TimeOnly(17, 30),
                Length = 1.0,
                AttendeeRange = new[] { 2, 12 },
                Trainer = new Coach { Coach_Id = 3 }
            };

            bool result = false;
            Team created = null;

            try
            {
                result = await _service.CreateTeamAsync(team);
                Assert.IsTrue(result, "Expected CreateTeamAsync to return true when team is created successfully.");

                created = (await _service.GetAllTeamsAsync()).LastOrDefault(t => t.Name == "Testhold");
                Assert.IsNotNull(created, "Created team was not found.");
            }
            finally
            {
                if (created != null)
                    await _service.DeleteTeamAsync(created.Id);
            }
        }

        [TestMethod]
        public async Task DeleteTeamAsync_ShouldRemoveTeam()
        {
            var team = new Team
            {
                Name = "Temp Deletion Team",
                MembershipType = "Seniorer",
                Length = 1,
                TimeOfDay = new TimeOnly(10, 0),
                DayOfWeek = 1,
                AttendeeRange = new int[] { 5, 15 },
                Description = "Test team for deletion",
                Trainer = new Coach { Coach_Id = 3 }
            };

            Team createdTeam = null;

            try
            {
                bool created = await _service.CreateTeamAsync(team);
                Assert.IsTrue(created, "Team creation failed during setup for deletion test.");

                createdTeam = (await _service.GetAllTeamsAsync()).LastOrDefault(t => t.Name == "Temp Deletion Team");
                Assert.IsNotNull(createdTeam, "Failed to retrieve created team.");

                Team deletedTeam = await _service.DeleteTeamAsync(createdTeam.Id);
                Assert.IsNotNull(deletedTeam, "DeleteTeamAsync should return the deleted team.");

                Team checkTeam = await _service.GetTeamFromIdAsync(createdTeam.Id);
                Assert.IsNull(checkTeam, "Team should no longer exist in the database after deletion.");
            }
            finally
            {
                if (createdTeam != null)
                    await _service.DeleteTeamAsync(createdTeam.Id); // ensure cleanup if delete failed
            }
        }

        [TestMethod]
        public async Task UpdateTeamAsync_ShouldModifyTeam()
        {
            var team = new Team
            {
                Name = "Update Test Team",
                MembershipType = "Seniorer",
                Length = 1.0,
                TimeOfDay = new TimeOnly(14, 0),
                DayOfWeek = 2,
                AttendeeRange = new int[] { 10, 20 },
                Description = "Before update",
                Trainer = new Coach { Coach_Id = 3 }
            };

            Team createdTeam = null;

            try
            {
                bool created = await _service.CreateTeamAsync(team);
                Assert.IsTrue(created, "Team was not created successfully for update test.");

                createdTeam = (await _service.GetAllTeamsAsync()).LastOrDefault(t => t.Name == "Update Test Team");
                Assert.IsNotNull(createdTeam, "Failed to retrieve created team.");

                createdTeam.Description = "Updated description";
                createdTeam.Length = 1.5;

                bool updated = await _service.UpdateTeamAsync(createdTeam);
                Assert.IsTrue(updated, "Team was not updated successfully.");

                Team updatedTeam = await _service.GetTeamFromIdAsync(createdTeam.Id);
                Assert.IsNotNull(updatedTeam, "Updated team could not be retrieved.");
                Assert.AreEqual("Updated description", updatedTeam.Description);
                Assert.AreEqual(1.5, updatedTeam.Length);
            }
            finally
            {
                if (createdTeam != null)
                    await _service.DeleteTeamAsync(createdTeam.Id);
            }
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
                TimeOfDay = new TimeOnly(10, 0),
                Length = 1.25,
                AttendeeRange = new int[] { 1, 8 },
                Trainer = new Coach { Coach_Id = 3 }
            };

            Team createdTeam = null;

            try
            {
                await _service.CreateTeamAsync(team);
                createdTeam = (await _service.GetAllTeamsAsync()).LastOrDefault(t => t.Name == "LookupTeam");

                Assert.IsNotNull(createdTeam, "Failed to find created team.");

                var fetched = await _service.GetTeamFromIdAsync(createdTeam.Id);
                Assert.IsNotNull(fetched);
                Assert.AreEqual("LookupTeam", fetched.Name);
            }
            finally
            {
                if (createdTeam != null)
                    await _service.DeleteTeamAsync(createdTeam.Id);
            }
        }
    }
}
