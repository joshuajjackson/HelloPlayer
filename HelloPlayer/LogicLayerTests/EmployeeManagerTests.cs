using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DataAccessFake;
using DataAccessInterface;
using DataObject;
using Logic;
using LogicInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicLayerTests
{
    [TestClass]
    public class EmployeeManagerTests 
    {
        private IEmployeeAccessor employeeAccessor;
        public EmployeeManagerTests()
        {
            employeeAccessor = new FakeEmployeeAccessor();
        }
        [TestMethod]
        public void TestActivateEmployee()
        {
            //Arrange
            Employee employee = new Employee()
            {
                EmployeeID = 2,
            };
            IEmployeeManager employeeManager = new EmployeeManager(employeeAccessor);
            //Act
            bool expectedResults = true;
            bool isChecked = true;
            bool actualResult = employeeManager.ChangeEmployeeActiveStatus(isChecked, employee.EmployeeID);
            //Assert
            Assert.AreEqual(actualResult, expectedResults);
        }

        [TestMethod]
        public void TestDeActivateEmployee()
        {
            //Arrange
            Employee employee = new Employee()
            {
                EmployeeID = 1,
            };
            IEmployeeManager employeeManager = new EmployeeManager(employeeAccessor);
            //Act
            bool expectedResults = true;
            bool isChecked = false;
            bool actualResult = employeeManager.ChangeEmployeeActiveStatus(isChecked, employee.EmployeeID);
            //Assert
            Assert.AreEqual(actualResult, expectedResults);
        }

        [TestMethod]
        public void TestSelectAllRoles()
        {
            List<string> roles = new List<string>();
            roles = employeeAccessor.GetAllRoles();
            Assert.AreEqual(2, roles.Count);
        }

        [TestMethod]
        public void TestSelectAllActiveEmployees()
        {
            List<Employee> selectedEmployees = new List<Employee>();
            const bool active = true;
            selectedEmployees = employeeAccessor.GetCurrentEmployees(active);
            Assert.AreEqual(1, selectedEmployees.Count);
        }

        [TestMethod]
        public void TestGetEmployeeRolesByID()
        {
            int employeeID = 1;
            List<string> roles = new List<string>();
            roles = employeeAccessor.GetEmployeeRolesByID(employeeID);
            Assert.AreEqual(1, roles.Count);
        }

        [TestMethod]
        public void TestUpdateEmployee()
        {
            Employee oldEmployee = new Employee()
            {
                EmployeeID = 1,
                FirstName = "Tony",
                LastName = "Stark",
                Email = "ironman@gmail.com",
                PhoneNumber = "15554443322",
                UserName = "IronMan",
                Active = true,
                Roles = new List<string>() { "Test" }
            };
            Employee newEmployee = new Employee()
            {
                EmployeeID = 1,
                FirstName = "Tony",
                LastName = "Stark",
                Email = "ironman@gmail.com",
                PhoneNumber = "15554443322",
                UserName = "IronMan2",
                Active = true,
                Roles = new List<string>() { "Test" }
            };
            IEmployeeManager employeeManager = new EmployeeManager(employeeAccessor);
            bool expectedResults = true;
            bool actualResult = employeeManager.UpdateEmployee(oldEmployee, newEmployee);
            Assert.AreEqual(actualResult, expectedResults);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuthenticationUserNameException()
        {
            //Arrange            
            string email = "j.blue@RandoGuy.com";
            Employee employee = new Employee();
            //Value you want PasswordHash() to return
            //Hashing Password
            string goodPasswordHash = hashPassword("passwordtest");
            //Act
            employee = employeeAccessor.AuthenticateUser(email, goodPasswordHash);
            //Assert not needed   
        }

        private string hashPassword(string source)
        {
            string result = null;

            byte[] data;

            using (SHA256 sha256hash = SHA256.Create())
            {
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            var s = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            result = s.ToString().ToUpper();

            return result;
        }

        [TestMethod]
        public void TestAddNewEmployee()
        {
            // arrange
            IEmployeeManager employeeManager = new EmployeeManager(employeeAccessor);
            // act
            bool row = employeeManager.InsertEmployee(new Employee()
            {
                EmployeeID = 3,
                FirstName = "Frank",
                LastName = "Costello",
                Email = "goomba@gmail.com",
                PhoneNumber = "14445552233",
                UserName = "Goomba",
                Active = true,
                Roles = new List<string>() { "Test" }
            });
            // assert
            Assert.IsTrue(row);
        }
    }
}
