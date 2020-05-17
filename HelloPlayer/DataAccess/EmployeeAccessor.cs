using DataAccessInterface;
using DataObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class EmployeeAccessor : IEmployeeAccessor
    {
        #region Login
        public Employee AuthenticateUser(string email, string passwordHash)
        {
            Employee result = null;
            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_authenticate_user", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;
            try
            {
                conn.Open();

                if (1 == Convert.ToInt32(cmd.ExecuteScalar()))
                {
                    result = getUserByEmail(email);
                }
                else
                {
                    throw new ApplicationException("Employee not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        #endregion Login

        #region EmployeeHandling
        private Employee getUserByEmail(string email)
        {
            Employee emp = null;
            var conn = DBConnector.GetConnection();
            var cmd1 = new SqlCommand("sp_select_employee_by_email");
            var cmd2 = new SqlCommand("sp_select_roles_by_EmployeeID");
            cmd1.Connection = conn;
            cmd2.Connection = conn;
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd1.Parameters["@Email"].Value = email;
            cmd2.Parameters.Add("@EmployeeID", SqlDbType.Int);
            try
            {
                conn.Open();
                var reader1 = cmd1.ExecuteReader();
                if (reader1.Read())
                {
                    emp = new Employee();
                    emp.EmployeeID = reader1.GetInt32(0);
                    emp.FirstName = reader1.GetString(1);
                    emp.LastName = reader1.GetString(2);
                    emp.PhoneNumber = reader1.GetString(3);
                    emp.Email = email;
                }
                else
                {
                    throw new ApplicationException("User not found");
                }
                reader1.Close();
                cmd2.Parameters["@EmployeeID"].Value = emp.EmployeeID;
                List<String> roles = new List<String>();
                var reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    string role = reader2.GetString(0);
                    roles.Add(role);
                }
                emp.Roles = roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return emp;
        }

        public List<Employee> GetCurrentEmployees(bool active = true)
        {
            List<Employee> emps = new List<Employee>();
            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_users_by_active");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Active", SqlDbType.Bit);
            cmd.Parameters["@Active"].Value = active;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var emp = new Employee();
                        emp.EmployeeID = reader.GetInt32(0);
                        emp.FirstName = reader.GetString(1);
                        emp.LastName = reader.GetString(2);
                        emp.UserName = reader.GetString(3);
                        emp.PhoneNumber = reader.GetString(4);
                        emp.Email = reader.GetString(5);
                        emp.Active = reader.GetBoolean(6);
                        emps.Add(emp);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return emps;
        }

        public int AddNewEmployee(Employee employee)
        {
            int employeeID = 0;

            var conn = DBConnector.GetConnection();

            var cmd = new SqlCommand("sp_insert_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@UserName", employee.UserName);
            cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", employee.Email);

            try
            {
                conn.Open();
                employeeID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return employeeID;
        }

        public int UpdateEmployee(Employee ogEmployee, Employee newEmployee)
        {
            int rows = 0;

            var conn = DBConnector.GetConnection();

            var cmd = new SqlCommand("sp_update_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmployeeID", ogEmployee.EmployeeID);

            cmd.Parameters.AddWithValue("@NewFirstName", newEmployee.FirstName);
            cmd.Parameters.AddWithValue("@NewLastName", newEmployee.LastName);
            cmd.Parameters.AddWithValue("@NewUserName", newEmployee.UserName);
            cmd.Parameters.AddWithValue("@NewPhoneNumber", newEmployee.PhoneNumber);
            cmd.Parameters.AddWithValue("@NewEmail", newEmployee.Email);

            cmd.Parameters.AddWithValue("@OldFirstName", ogEmployee.FirstName);
            cmd.Parameters.AddWithValue("@OldLastName", ogEmployee.LastName);
            cmd.Parameters.AddWithValue("@OldUserName", ogEmployee.UserName);
            cmd.Parameters.AddWithValue("@OldPhoneNumber", ogEmployee.PhoneNumber);
            cmd.Parameters.AddWithValue("@OldEmail", ogEmployee.Email);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }


        public int ActivateEmployee(int employeeID)
        {
            int rows = 0;

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_reactivate_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }

        public int DeactivateEmployee(int employeeID)
        {
            int rows = 0;

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_deactivate_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }

        public List<string> GetAllRoles()
        {
            List<string> roles = new List<string>();

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_all_employee_roles");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    roles.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }

        public List<string> GetEmployeeRolesByID(int employeeID)
        {
            List<string> roles = new List<string>();

            var conn = DBConnector.GetConnection();

            var cmd = new SqlCommand("sp_select_roles_by_employeeID");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeID"].Value = employeeID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    roles.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }

        public int InsertOrDeleteEmployeeRole(int employeeID, string role, bool delete = false)
        {
            int rows = 0;

            string cmdText = delete ? "sp_delete_employee_role" : "sp_create_employee_role";

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
            cmd.Parameters.AddWithValue("@RoleID", role);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
        #endregion EmployeeManagement

        #region PasswordManagement
        public bool UpdatePasswordHash(int employeeID, string oldPasswordHash, string newPasswordHash)
        {
            bool updateSuccess = false;
            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_update_password");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters["@EmployeeID"].Value = employeeID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                updateSuccess = (rows == 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return updateSuccess;
        }
        #endregion PasswordManagement
    }
}
