using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    public class Jobs
    {
        // Properties for job data
        public string Id { get; set; }
        public string Title { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }

        // Database connection string
        static string connectionString = "Data Source=DESKTOP-AJI5ESJ;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

        // GET ALL: Jobs - Get all jobs from the database
        public List<Jobs> GetAll()
        {
            // Declare a list to store jobs
            var jobsList = new List<Jobs>();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query
            command.Connection = connection;
            command.CommandText = "SELECT * FROM jobs";

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
                        // Read data from the query result and add it to the list of jobs
                        jobsList.Add(new Jobs
                        {
                            Id = reader.GetString(0),
                            Title = reader.GetString(1),
                            MinSalary = reader.GetInt32(2),
                            MaxSalary = reader.GetInt32(3)
                        });
                    }
                    reader.Close();
                    connection.Close();

                    return jobsList;
                }
                reader.Close();
                connection.Close();

                return new List<Jobs>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<Jobs>();
        }

        // GET BY ID: Jobs - Get one job based on ID
        public Jobs GetById(char id)
        {
            // Create a new job object
            var job = new Jobs();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL query with a parameter for ID
            command.CommandText = "SELECT * FROM jobs WHERE id = @id";

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
                            // Read data from the query result and populate the job object
                            job.Id = reader.GetString(0);
                            job.Title = reader.GetString(1);
                            job.MinSalary = reader.GetInt32(2);
                            job.MaxSalary = reader.GetInt32(3);
                        }
                        reader.Close();
                        connection.Close();

                        return job;
                    }

                    reader.Close();
                    connection.Close();

                    return job;
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

            return job;
        }

        // INSERT: Jobs - Insert a new job into the database
        public string Insert(char id, string title, int minSalary, int maxSalary)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query with parameters
            command.Connection = connection;
            command.CommandText = "INSERT INTO jobs VALUES (@id, @title, @minSalary, @maxSalary);";

            try
            {
                // Add parameters for ID, title, minSalary, and maxSalary
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@title", title));
                command.Parameters.Add(new SqlParameter("@minSalary", minSalary));
                command.Parameters.Add(new SqlParameter("@maxSalary", maxSalary));

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

        // UPDATE: Jobs - Update an existing job in the database
        public string Update(char id, string title, int minSalary, int maxSalary)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL update query with parameters
            command.CommandText = "UPDATE jobs SET title = @title, min_salary = @minSalary, max_salary = @maxSalary WHERE id = @id;";

            try
            {
                // Add parameters for title, minSalary, maxSalary, and ID
                command.Parameters.Add(new SqlParameter("@title", title));
                command.Parameters.Add(new SqlParameter("@minSalary", minSalary));
                command.Parameters.Add(new SqlParameter("@maxSalary", maxSalary));
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

        // DELETE: Jobs - Delete a job from the database based on ID
        public string Delete(char id)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL delete query with a parameter for ID
            command.CommandText = "DELETE FROM jobs WHERE id = @id;";

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
