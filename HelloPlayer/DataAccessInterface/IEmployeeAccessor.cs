using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterface
{
    public interface IEmployeeAccessor
    {
        Employee AuthenticateUser(string email, string passwordHash);
        bool UpdatePasswordHash(int employeeID, string oldPasswordHash, string newPasswordHash);
        List<Employee> GetCurrentEmployees(bool active = true);
        int AddNewEmployee(Employee employee);
        int UpdateEmployee(Employee ogEmployee, Employee newEmployee);
        int ActivateEmployee(int employeeID);
        int DeactivateEmployee(int employeeID);
        List<string> GetAllRoles();
        List<string> GetEmployeeRolesByID(int employeeID);
        int InsertOrDeleteEmployeeRole(int employeeID, string role, bool delete = false);
    }
}
