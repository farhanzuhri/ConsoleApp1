using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    public class Histories
    {
        // Properties for history data
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int EmployeeId { get; set; }
        public DateTime EndDate { get; set; }
        public int DepartmentId { get; set; }
        public string JobId { get; set; }

        // Database connection string
        static string connectionString = "Data Source=DESKTOP-AJI5ESJ;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

        // GET ALL: Histories - Get all history records from the database
        public List<Histories> GetAll()
        {
            // Declare a list to store history records
            var histories = new List<Histories>();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query
            command.Connection = connection;
            command.CommandText = "SELECT * FROM histories";

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
                        // Read data from the query result and add it to the list of histories
                        histories.Add(new Histories
                        {
                            Id = reader.GetInt32(0),
                            StartDate = reader.GetDateTime(1),
                            EmployeeId = reader.GetInt32(2),
                            EndDate = reader.GetDateTime(3),
                            DepartmentId = reader.GetInt32(4),
                            JobId = reader.GetString(5) 
                        });
                    }
                    reader.Close();
                    connection.Close();

                    return histories;
                }
                reader.Close();
                connection.Close();

                return new List<Histories>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<Histories>();
        }

        // GET BY ID: Histories - Get one history record based on ID
        public Histories GetById(int id)
        {
            // Create a new history record object
            var history = new Histories();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL query with a parameter for ID
            command.CommandText = "SELECT * FROM histories WHERE id = @id";

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
                            // Read data from the query result and populate the history record object
                            history.Id = reader.GetInt32(0);
                            history.StartDate = reader.GetDateTime(1);
                            history.EmployeeId = reader.GetInt32(2);
                            history.EndDate = reader.GetDateTime(3);
                            history.DepartmentId = reader.GetInt32(4);
                            history.JobId = reader.GetString(5);
                        }
                        reader.Close();
                        connection.Close();

                        return history;
                    }

                    reader.Close();
                    connection.Close();

                    return history;
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

            return history;
        }

        // INSERT: Histories - Insert a new history record into the database
        public string Insert(DateTime startDate, int employeeId, DateTime endDate, int departmentId, char jobId)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query with parameters
            command.Connection = connection;
            command.CommandText = "INSERT INTO histories (start_date, employee_id, end_date, department_id, job_id) VALUES (@startDate, @employeeId, @endDate, @departmentId, @jobId);";

            try
            {
                // Add parameters for start date, employee ID, end date, department ID, and job ID
                command.Parameters.Add(new SqlParameter("@startDate", startDate));
                command.Parameters.Add(new SqlParameter("@employeeId", employeeId));
                command.Parameters.Add(new SqlParameter("@endDate", endDate));
                command.Parameters.Add(new SqlParameter("@departmentId", departmentId));
                command.Parameters.Add(new SqlParameter("@jobId", jobId.ToString()));

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

        // UPDATE: Histories - Update an existing history record in the database
        public string Update(int id, DateTime startDate, int employeeId, DateTime endDate, int departmentId, char jobId)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE histories SET start_date = @startDate, employee_id = @employeeId, end_date = @endDate, department_id = @departmentId, job_id = @jobId WHERE id = @id;";

            try
            {
                // Add parameters for start date, employee ID, end date, department ID, job ID, and ID
                command.Parameters.Add(new SqlParameter("@startDate", startDate));
                command.Parameters.Add(new SqlParameter("@employeeId", employeeId));
                command.Parameters.Add(new SqlParameter("@endDate", endDate));
                command.Parameters.Add(new SqlParameter("@departmentId", departmentId));
                command.Parameters.Add(new SqlParameter("@jobId", jobId.ToString()));
                command.Parameters.Add(new SqlParameter("@id", id));

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

        // DELETE: Histories - Delete a history record from the database based on ID
        public string Delete(int id)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL delete query with a parameter for ID
            command.CommandText = "DELETE FROM histories WHERE id = @id;";

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
