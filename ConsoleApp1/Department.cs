using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    public class Departments
    {
        // Properties for department data
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public int ManagerId { get; set; }

        // Database connection string
        static string connectionString = "Data Source=DESKTOP-AJI5ESJ;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

        // GET ALL: Departments - Get all departments from the database
        public List<Departments> GetAll()
        {
            // Declare a list to store departments
            var departments = new List<Departments>();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query
            command.Connection = connection;
            command.CommandText = "SELECT * FROM departments";

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
                        // Read data from the query result and add it to the list of departments
                        departments.Add(new Departments
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            LocationId = reader.GetInt32(2),
                            ManagerId = reader.GetInt32(3)
                        });
                    }
                    reader.Close();
                    connection.Close();

                    return departments;
                }
                reader.Close();
                connection.Close();

                return new List<Departments>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<Departments>();
        }

        // GET BY ID: Department - Get one department based on ID
        public Departments GetById(int id)
        {
            // Create a new department object
            var department = new Departments();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL query with a parameter for ID
            command.CommandText = "SELECT * FROM departments WHERE id = @id";

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
                            // Read data from the query result and populate the department object
                            department.Id = reader.GetInt32(0);
                            department.Name = reader.GetString(1);
                            department.LocationId = reader.GetInt32(2);
                            department.ManagerId = reader.GetInt32(3);
                        }
                        reader.Close();
                        connection.Close();

                        return department;
                    }

                    reader.Close();
                    connection.Close();

                    return department;
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

            return department;
        }

        // INSERT: Department - Insert a new department into the database
        public string Insert(string name, int locationId, int managerId)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query with parameters
            command.Connection = connection;
            command.CommandText = "INSERT INTO departments (name, location_id, manager_id) VALUES (@name, @locationId, @managerId);";

            try
            {
                // Add parameters for name, location ID, and manager ID
                command.Parameters.Add(new SqlParameter("@name", name));
                command.Parameters.Add(new SqlParameter("@locationId", locationId));
                command.Parameters.Add(new SqlParameter("@managerId", managerId));

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

        // UPDATE: Department - Update an existing department in the database
        public string Update(int id, string name, int locationId, int managerId)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL update query with parameters
            command.CommandText = "UPDATE departments SET name = @name, location_id = @locationId, manager_id = @managerId WHERE id = @id;";

            try
            {
                // Add parameters for name, location ID, manager ID, and ID
                command.Parameters.Add(new SqlParameter("@name", name));
                command.Parameters.Add(new SqlParameter("@locationId", locationId));
                command.Parameters.Add(new SqlParameter("@managerId", managerId));
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

        // DELETE: Department - Delete a department from the database based on ID
        public string Delete(int id)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL delete query with a parameter for ID
            command.CommandText = "DELETE FROM departments WHERE id = @id;";

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
