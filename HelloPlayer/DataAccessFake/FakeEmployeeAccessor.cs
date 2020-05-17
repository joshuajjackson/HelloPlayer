using DataAccessInterface;
using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFake
{
    public class FakeEmployeeAccessor : IEmployeeAccessor
    {
        private List<Employee> employees;
        public FakeEmployeeAccessor()
        {
            employees = new List<Employee>()
            {
                new Employee()
                {
                    EmployeeID = 1,
                    FirstName = "Tony",
                    LastName = "Stark",
                    Email = "ironman@gmail.com",
                    PhoneNumber = "15554443322",
                    UserName = "IronMan",
                    Active = true,
                    Roles = new List<string>() {"Test"}
                },
                new Employee()
                {
                    EmployeeID = 2,
                    FirstName = "Ned",
                    LastName = "Flanders",
                    Email = "diddlydoo@gmail.com",
                    PhoneNumber = "16654554422",
                    UserName = "JesusLuvr",
                    Active = false,
                    Roles = new List<string>() {"Test"}
                }
            };
        }
        public int ActivateEmployee(int employeeID)
        {
            Employee employee = null;
            if(employeeID == null)
            {
                throw new Exception();
            }
            foreach (var r in employees)
            {
                if(employeeID == r.EmployeeID)
                {
                    employee = r;
                }
            }
            if (employee == null || employeeID != employee.EmployeeID)
            {
                throw new Exception();
            }
            employee.Active = true;

            if (employee.Active == true)
            {
                return 1;
            }
            return 0;
        }

        public int AddNewEmployee(Employee employee)
        {
            employees.Add(employee);
            return 1;
        }

        public Employee AuthenticateUser(string email, string passwordHash)
        {
            bool userName = email.Equals("ironman@gmail.com");
            bool hash = passwordHash.Equals("A7574A42198B7D7EEE2C037703A0B95558F195457908D6975E681E2055FD5EB9");

            if (userName && hash)
            {
                Employee employee = new Employee()
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
                return employee;
            }
            else
            {
                throw new ApplicationException("Invalid User");
            }
        }

        public int DeactivateEmployee(int employeeID)
        {
            Employee employee = null;
            if (employeeID == null)
            {
                throw new Exception();
            }
            foreach (var r in employees)
            {
                if (employeeID == r.EmployeeID)
                {
                    employee = r;
                }
            }
            if (employee == null || employeeID != employee.EmployeeID)
            {
                throw new Exception();
            }
            employee.Active = false;

            if (employee.Active == false)
            {
                return 1;
            }
            return 0;
        }

        public List<string> GetAllRoles()
        {
            List<string> skills = new List<string>() { "Test", "Test1" };
            return skills;
        }

        public List<Employee> GetCurrentEmployees(bool active = true)
        {
            var selectedEmployees = (from e in employees
                                      where e.Active == true
                                      select e).ToList();
            return selectedEmployees;
        }

        public List<string> GetEmployeeRolesByID(int employeeID)
        {
            List<string> getRoles = (from e in employees
                                     where e.EmployeeID == employeeID
                                     select e.Roles.ToString()).ToList();

            return getRoles;
        }

        public int InsertOrDeleteEmployeeRole(int employeeID, string role, bool delete = false)
        {
            throw new NotImplementedException();
        }

        public int UpdateEmployee(Employee ogEmployee, Employee newEmployee)
        {
            Employee employee = ogEmployee;

            try
            {
                ogEmployee = newEmployee;
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
        }

        public bool UpdatePasswordHash(int employeeID, string oldPasswordHash, string newPasswordHash)
        {
            throw new NotImplementedException();
        }
    }
}
