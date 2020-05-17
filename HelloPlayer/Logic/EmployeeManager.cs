using DataAccess;
using DataAccessInterface;
using DataObject;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class EmployeeManager : IEmployeeManager
    {
        #region ClassObjects
        private IEmployeeAccessor _employeeAccessor;
        #endregion ClassObjects

        #region Constructors
        public EmployeeManager()
        {
            _employeeAccessor = new EmployeeAccessor();
        }

        public EmployeeManager(IEmployeeAccessor employeeAccessor)
        {
            _employeeAccessor = employeeAccessor;
        }
        #endregion Constructors

        #region Login
        public Employee AuthenticateUser(string email, string password)
        {
            Employee result = null;
            var passwordHash = hashPassword(password);
            password = null;
            try
            {
                result = _employeeAccessor.AuthenticateUser(email, passwordHash);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Login Failed.  Bad Username or Password.", ex);
            }
            return result;
        }

        #endregion Login

        #region EmployeeManagement
        public List<Employee> GetCurrentEmployees(bool active = true)
        {
            try
            {
                return _employeeAccessor.GetCurrentEmployees(active);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found", ex);
            }
        }

        public bool InsertEmployee(Employee newHire)
        {
            bool result = true;
            try
            {
                result = _employeeAccessor.AddNewEmployee(newHire) > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User not added", ex);
            }
            return result;
        }

        public bool UpdateEmployee(Employee employee, Employee newHire)
        {
            bool result = false;

            try
            {
                result = _employeeAccessor.UpdateEmployee(employee, newHire) == 1;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed", ex);
            }

            return result;
        }

        public bool ChangeEmployeeActiveStatus(bool isChecked, int employeeID)
        {
            bool result = false;
            try
            {
                if (isChecked)
                {
                    result = 1 == _employeeAccessor.ActivateEmployee(employeeID);
                }
                else
                {
                    result = 1 == _employeeAccessor.DeactivateEmployee(employeeID);
                }
                if (result == false)
                {
                    throw new ApplicationException("Employee record not updated.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed!", ex);
            }
            return result;
        }

        public List<string> GetEmployeeRoles(int employeeID)
        {
            List<string> roles = null;

            try
            {
                roles = _employeeAccessor.GetEmployeeRolesByID(employeeID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return roles;
        }

        public List<string> GetEmployeeRoles()
        {
            List<string> roles = null;

            try
            {
                roles = _employeeAccessor.GetAllRoles();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return roles;
        }

        public bool AddUserRole(int employeeID, string role)
        {
            bool result = false;
            try
            {
                result = (1 == _employeeAccessor.InsertOrDeleteEmployeeRole(employeeID, role));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Role not added!", ex);
            }
            return result;
        }

        public bool DeleteUserRole(int employeeID, string role)
        {
            bool result = false;
            try
            {
                result = (1 == _employeeAccessor.InsertOrDeleteEmployeeRole(employeeID, role, delete: true));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Role not removed!", ex);
            }
            return result;
        }
        #endregion EmployeeManagement

        #region PasswordManagement
        public bool UpdatePassword(int employeeID, string newPassword, string oldPassword)
        {
            bool isUpdated = false;
            string newPasswordHash = hashPassword(newPassword);
            string oldPasswordHash = hashPassword(oldPassword);
            try
            {
                isUpdated = _employeeAccessor.UpdatePasswordHash(employeeID, oldPasswordHash, newPasswordHash);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed", ex);
            }
            return isUpdated;
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
        #endregion PasswordManagement
    }
}
