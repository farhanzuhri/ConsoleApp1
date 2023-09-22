using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions; 
using System.Xml.Linq;

namespace ConsoleApp1.Models
{
    public class Countries
    {
        // Properties for country data
        public string Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }

        // Database connection string
        static string connectionString = "Data Source=DESKTOP-AJI5ESJ;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

        // GET ALL: Countries - Get all countries from the database
        public List<Countries> GetAll()
        {
            // Declare a list to store countries
            var countries = new List<Countries>();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query
            command.Connection = connection;
            command.CommandText = "SELECT * FROM countries";

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
                        // Read data from the query result and add it to the list of countries
                        countries.Add(new Countries
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            RegionId = reader.GetInt32(2)
                        });
                    }
                    reader.Close();
                    connection.Close();

                    return countries;
                }
                reader.Close();
                connection.Close();

                return new List<Countries>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<Countries>();
        }

        // GET BY ID: Country - Get one country based on ID
        public Countries GetById(int id)
        {
            // Create a new country object
            var country = new Countries();

            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL query with a parameter for ID
            command.CommandText = "SELECT * FROM countries WHERE id = @id";

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
                            // Read data from the query result and populate the country object
                            country.Id = reader.GetString(0);
                            country.Name = reader.GetString(1);
                            country.RegionId = reader.GetInt32(2);
                        }
                        reader.Close();
                        connection.Close();

                        return country;
                    }

                    reader.Close();
                    connection.Close();

                    return country;
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

            return country;
        }

        // INSERT: Country - Insert a new country into the database
        public string Insert(int id, string name, char regionId)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            // Set the command's connection and SQL query with parameters
            command.Connection = connection;
            command.CommandText = "INSERT INTO countries (id, name, region_id) VALUES (@id, @name, @regionId);";

            try
            {
                // Add parameters for ID, name, and region ID
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@name", name));
                command.Parameters.Add(new SqlParameter("@regionId", regionId));

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

        // UPDATE: Country - Update an existing country in the database
        public string Update(int id, string name, char regionId)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL update query with parameters
            command.CommandText = "UPDATE countries SET name = @name, region_id = @regionId WHERE id = @id;";

            try
            {
                // Add parameters for name, region ID, and ID
                command.Parameters.Add(new SqlParameter("@name", name));
                command.Parameters.Add(new SqlParameter("@regionId", regionId));
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

        // DELETE: Country - Delete a country from the database based on ID
        public string Delete(int id)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL delete query with a parameter for ID
            command.CommandText = "DELETE FROM countries WHERE id = @id;";

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
