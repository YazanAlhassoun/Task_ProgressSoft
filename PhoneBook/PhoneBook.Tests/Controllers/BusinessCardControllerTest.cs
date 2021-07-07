using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBook;
using PhoneBook.Common;
using PhoneBook.Controllers;
using PhoneBook.Models;
using PhoneBook.Models.Repository;
using PhoneBooks.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PhoneBook.Tests.Controllers
{
    [TestClass]
    public class BusinessCardControllerTest
    {

        //  PhoneBookDBEntities db = new PhoneBookDBEntities();
      
        [TestMethod]
        public void GetIndexToView_List_BusinessCardList()
        {
            //// Arrange
            //BusinessCardDbReopsitory sut = new BusinessCardDbReopsitory(db);
            //IList<BusinessCard> model = new List<BusinessCard>();

            //// Act
            //var actual = sut.List().ToList(); 
            //var expected = model; 


            //// Assert
            //Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void Add_businessCard()
        {
            //// Arrange
            //BusinessCardDbReopsitory sut = new BusinessCardDbReopsitory(db);
            //BusinessCard model = new BusinessCard
            //{
            //    Name = "Bill Gates",
            //    Gender = "Male",
            //    DateOfBirth = Convert.ToDateTime(122),
            //    Email = "Bill.Gates@microsoft.com",
            //    Phone = "00962798888888",
            //    Photo = "photo.jpg",
            //    Address = "USA",
            //    UserId = "21e4fe5e-72fd-41cf-bf5a-ddf1fe320305"
            //};


            //// Act
            //SavingStatus actual = sut.Add(model);
            //SavingStatus expected = SavingStatus.Saved;


            //// Assert
            //Assert.AreEqual(actual, expected);
        }





        //    [TestMethod]
        //    public void Index()
        //    {
        //        // Arrange

        //        // Act


        //        // Assert

        //    }




    }
}
