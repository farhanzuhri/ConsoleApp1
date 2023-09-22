using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    
    public class Employees
    {
        // Properties for employee data
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int Salary { get; set; }
        public decimal CommissionPct { get; set; }
        public int ManagerId { get; set; }
        public string JobId { get; set; }
        public int DepartmentId { get; set; }

        // Database connection string
        static string connectionString = "Data Source=DESKTOP-AJI5ESJ;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

        // GET ALL: Employees - Get all employees from the database
        public List<Employees> GetAll()
        {
            // Declare a list to store employees
            var employeesList = new List<Employees>();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query
            command.Connection = connection;
            command.CommandText = "SELECT * FROM employees";

            try
            {
                // Open the database connection
                connection.Open();

                // Execute the SQL query and read the results
                using var reader = command.ExecuteReader();

                // Check if there are rows in the result
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Read data from the query result and add it to the list of employees
                        employeesList.Add(new Employees
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                            PhoneNumber = reader.GetString(4),
                            HireDate = reader.GetDateTime(5),
                            Salary = reader.GetInt32(6),
                            CommissionPct = reader.GetDecimal(7),
                            ManagerId = reader.GetInt32(8),
                            JobId = reader.GetString(9),
                            DepartmentId = reader.GetInt32(10)
                        });
                    }
                    reader.Close();
                    connection.Close();

                    return employeesList;
                }
                reader.Close();
                connection.Close();

                return new List<Employees>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<Employees>();
        }

        // GET BY ID: Employee - Get one employee based on ID
        public Employees GetById(int id)
        {
            // Create a new employee object
            var employee = new Employees();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL query with a parameter for ID
            command.CommandText = "SELECT * FROM employees WHERE id = @id";

            try
            {
                // Add the ID parameter to the command
                command.Parameters.Add(new SqlParameter("@id", id));

                // Open the database connection
                connection.Open();

                // Start a transaction
                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

                    // Execute the SQL query and read the result
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Read data from the query result and populate the employee object
                            employee.Id = reader.GetInt32(0);
                            employee.FirstName = reader.GetString(1);
                            employee.LastName = reader.GetString(2);
                            employee.Email = reader.GetString(3);
                            employee.PhoneNumber = reader.GetString(4);
                            employee.HireDate = reader.GetDateTime(5);
                            employee.Salary = reader.GetInt32(6);
                            employee.CommissionPct = reader.GetDecimal(7);
                            employee.ManagerId = reader.GetInt32(8);
                            employee.JobId = reader.GetString(9);
                            employee.DepartmentId = reader.GetInt32(10);
                        }
                        reader.Close();
                        connection.Close();

                        return employee;
                    }

                    reader.Close();
                    connection.Close();

                    return employee;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if an error occurs
                    transaction.Rollback();
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return employee;
        }

        // INSERT: Employee - Insert a new employee into the database
        public string Insert(Employees employee)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query with parameters
            command.Connection = connection;
            command.CommandText = "INSERT INTO employees VALUES (@First_Name, @Last_Name, @Email, @Phone_Number, @Hire_Date, @Salary, @Commission_Pct, @Manager_Id, @Job_Id, @Department_Id);";

            try
            {
                // Add parameters for all employee properties
                command.Parameters.Add(new SqlParameter("@First_Name", employee.FirstName));
                command.Parameters.Add(new SqlParameter("@Last_Name", employee.LastName));
                command.Parameters.Add(new SqlParameter("@Email", employee.Email));
                command.Parameters.Add(new SqlParameter("@Phone_Number", employee.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@Hire_Date", employee.HireDate));
                command.Parameters.Add(new SqlParameter("@Salary", employee.Salary));
                command.Parameters.Add(new SqlParameter("@Commission_Pct", employee.CommissionPct));
                command.Parameters.Add(new SqlParameter("@Manager_Id", employee.ManagerId));
                command.Parameters.Add(new SqlParameter("@Job_Id", employee.JobId));
                command.Parameters.Add(new SqlParameter("@Department_Id", employee.DepartmentId));

                // Open the database connection
                connection.Open();

                // Start a transaction
                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

                    // Execute the SQL query and get the result
                    var result = command.ExecuteNonQuery();

                    // Commit the transaction
                    transaction.Commit();
                    connection.Close();

                    return result.ToString();
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if an error occurs
                    transaction.Rollback();
                    return $"Error Transaction: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // UPDATE: Employee - Update an existing employee in the database
        public string Update(Employees employee)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL update query with parameters
            command.CommandText = "UPDATE employees SET First_Name = @First_Name, Last_Name = @Last_Name, Email = @Email, Phone_Number = @Phone_Number, Hire_Date = @Hire_Date, Salary = @Salary, Commission_Pct = @Commission_Pct, Manager_Id = @Manager_Id, Job_Id = @Job_Id, Department_Id = @Department_Id WHERE Id = @Id;";

            try
            {
                // Add parameters for all employee properties and ID
                command.Parameters.Add(new SqlParameter("@First_Name", employee.FirstName));
                command.Parameters.Add(new SqlParameter("@Last_Name", employee.LastName));
                command.Parameters.Add(new SqlParameter("@Email", employee.Email));
                command.Parameters.Add(new SqlParameter("@Phone_Number", employee.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@Hire_Date", employee.HireDate));
                command.Parameters.Add(new SqlParameter("@Salary", employee.Salary));
                command.Parameters.Add(new SqlParameter("@Commission_Pct", employee.CommissionPct));
                command.Parameters.Add(new SqlParameter("@Manager_Id", employee.ManagerId));
                command.Parameters.Add(new SqlParameter("@Job_Id", employee.JobId));
                command.Parameters.Add(new SqlParameter("@Department_Id", employee.DepartmentId));
                command.Parameters.Add(new SqlParameter("@Id", employee.Id));

                // Open the database connection
                connection.Open();

                // Start a transaction
                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

                    // Execute the SQL query and get the result
                    var result = command.ExecuteNonQuery();

                    // Commit the transaction
                    transaction.Commit();
                    connection.Close();

                    return result.ToString();
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if an error occurs
                    transaction.Rollback();
                    return $"Error Transaction: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // DELETE: Employee - Delete an employee from the database based on ID
        public string Delete(int id)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL delete query with a parameter for ID
            command.CommandText = "DELETE FROM employees WHERE Id = @Id;";

            try
            {
                // Add the ID parameter to the command
                command.Parameters.Add(new SqlParameter("@Id", id));

                // Open the database connection
                connection.Open();

                // Start a transaction
                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

                    // Execute the SQL query and get the result
                    var result = command.ExecuteNonQuery();

                    // Commit the transaction
                    transaction.Commit();
                    connection.Close();

                    return result.ToString();
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if an error occurs
                    transaction.Rollback();
                    return $"Error Transaction: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
