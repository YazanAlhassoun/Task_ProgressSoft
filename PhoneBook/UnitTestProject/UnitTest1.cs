using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBook.Common;
using PhoneBook.Models;
using PhoneBook.Models.Repository;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private PhoneBookDBEntities db;


        [TestMethod]
        public void Add_businessCard()
        {
            // Arrange
            BusinessCardDbReopsitory sut = new BusinessCardDbReopsitory(db);
            BusinessCard model = new BusinessCard
            {
                Name = "Bill Gates",
                Gender = "Male",
                DateOfBirth = Convert.ToDateTime(122),
                Email = "Bill.Gates@microsoft.com",
                Phone = "00962798888888",
                Photo = "photo.jpg",
                Address = "USA",
                UserId = "21e4fe5e-72fd-41cf-bf5a-ddf1fe320305"
            };


            // Act
            SavingStatus actual = sut.Add(model);
            SavingStatus expected = SavingStatus.Saved;


            // Assert
            Assert.AreEqual(actual, expected);
        }


        [TestMethod]
        public void Index_List()
        {
            // Arrange
            BusinessCardDbReopsitory sut = new BusinessCardDbReopsitory(db);
            IList<BusinessCard> model = new List<BusinessCard>();

            // Act
            var actual = sut.List();
            var expected = model;


            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}