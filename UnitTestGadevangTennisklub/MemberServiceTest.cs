﻿using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Models.ViewModels;
using GadevangTennisklub2025.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGadevangTennisklub
{
    [TestClass]
    public sealed class MemberServiceTest
    {
       private readonly MemberService Mem;
   

        [TestMethod]
        public void GetAll_ShouldSucceed()
        {
            List<Member> memberList = Mem.GetAllMembersAsync().Result;
            Assert.IsNotNull(memberList);
        }

        [TestMethod]
        public void UpdateMember_ShouldSucced()
        {
            // Arrange
            var original = new Member("Hank Green", "Hank Green The Third", new DateOnly(1993, 06, 12),
                "Seniorer", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com",
                "123", "København", "Ja", 100);
            Mem.CreateMemberAsync(original).GetAwaiter().GetResult();

            var updated = new Member("Hank Green", "Hank Green The Third", new DateOnly(1993, 06, 12),
                "Seniorer", "Valby", "30303020", "2500", "Andet", "Slotsvej 4", "hank.green.the3rd@gmail.com",
                "123", "København", "Ja", 100);

            // Act
            Mem.UpdateMemberAsync(updated, 100).GetAwaiter().GetResult();
            Member result = Mem.GetMemberById(100).GetAwaiter().GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Andet", result.Gender);

        }

        [TestMethod]
        public void VerifyMember_ShouldSucced()
        {
            var mem = new Member("Hank Green", "Hank Green The Third", new DateOnly(1993, 06, 12), "Seniorer", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);
            Mem.CreateMemberAsync(mem).GetAwaiter().GetResult();
            Member validMember = Mem.VerifyMember("Hank Green","123");
            Assert.IsNotNull(validMember);
        }

        [TestMethod]
        public void VerifyMember_ShouldFail()
        {
            Member invalidMember = Mem.VerifyMember("peepeepoopoo", "123jisnianpn");
            Assert.IsNull(invalidMember);
        }

        [TestMethod]
        public void GetMemberById_ShouldFail()
        {
            Member invalidMember = Mem.GetMemberById(10007).Result;
            Assert.IsNull(invalidMember);
        }

        [TestMethod]
        public void GetMemberById_ShouldSucced()
        {
            var mem = new Member("Hank Green", "Hank Green The Third", new DateOnly(1993, 06, 12), "Seniorer", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);
            Mem.CreateMemberAsync(mem).GetAwaiter().GetResult();
            Member validMember = Mem.GetMemberById(100).GetAwaiter().GetResult();
            Assert.IsNotNull(validMember);
        }

        [TestMethod]
        public void DeleteMember_ShouldSucced()
        {
          
            var mem = new Member("Hank Green", "Hank Green The Third", new DateOnly(1993, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 1000);
       
            Mem.CreateMemberAsync(mem).GetAwaiter().GetResult();

            Mem.DeleteMemberAsync(1000).GetAwaiter().GetResult();

            var deletedMember = Mem.GetMemberById(1000).GetAwaiter().GetResult();
            Assert.IsNull(deletedMember);
        }

        [TestMethod]
        public void IsUniqueUsername_ShouldSucceed()
        {
            bool isUniqe = Mem.IsUsernameUnique("PeePeePooPoo").Result;
            Assert.IsTrue(isUniqe);
        }

        [TestMethod]
        public void IsUniqueUsername_ShouldFail()
        {
            var mem = new Member("Hank Green", "Hank Green The Third", new DateOnly(1993, 06, 12), "Senior", "Valby", "30303020", "2500", "Mand", "Slotsvej 4", "hank.green.the3rd@gmail.com", "123", "København", "Ja", 100);
            Mem.CreateMemberAsync(mem);
            bool isUniqe = Mem.IsUsernameUnique("Hank Green").Result;
            Assert.IsTrue(isUniqe);
        }

        [TestMethod]
        public void CreateMember_ShouldSucceed()
        {
            var mem = new Member(
                "Hank", "Hank Green The Third", new DateOnly(1993, 06, 12),
                "Seniorer", "Valby", "30303020", "2500", "Mand", "Slotsvej 4",
                "hank.green.the3rd@gmail.com", "123", "København", "Ja");

            Mem.CreateMemberAsync(mem).GetAwaiter().GetResult();

            var allMembers = Mem.GetAllMembersAsync().Result;
            var match = allMembers.FirstOrDefault(m => m.Username == "Hank");

            Assert.IsNotNull(match, "Member with ID 100 was not found after creation.");
            Assert.AreEqual("Hank Green", match.Name);
        }


    }
}

