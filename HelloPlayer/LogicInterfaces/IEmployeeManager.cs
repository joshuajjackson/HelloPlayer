using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterfaces
{
    public interface IEmployeeManager
    {
        Employee AuthenticateUser(string email, string password);
        bool UpdatePassword(int employeeID, string newPassword, string oldPassword);
        List<Employee> GetCurrentEmployees(bool active = true);
        bool InsertEmployee(Employee newHire);
        bool UpdateEmployee(Employee employee, Employee newHire);
        bool ChangeEmployeeActiveStatus(bool isChecked, int employeeID);
        List<string> GetEmployeeRoles(int employeeID);
        List<string> GetEmployeeRoles();
        bool AddUserRole(int employeeID, string role);
        bool DeleteUserRole(int employeeID, string role);
    }
}
