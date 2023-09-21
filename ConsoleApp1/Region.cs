using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions; // Add using statement for System.Transactions
using System.Xml.Linq;

namespace ConsoleApp1 // Adjust the namespace name
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Database connection string
        static string connectionString = "Data Source=DESKTOP-AJI5ESJ;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

        // GET ALL: Region
        // Method to retrieve all regions from the database
        public List<Region> GetAll()
        {
            var regions = new List<Region>();

            // Establish a database connection using SqlConnection
            using var connection = new SqlConnection(connectionString);

            // Create a SqlCommand
            using var command = connection.CreateCommand();

            // Set the SQL query
            command.CommandText = "SELECT * FROM regions";

            try
            {
                // Open the database connection
                connection.Open();

                // Execute the SQL query and retrieve the results
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Create and add Region objects to the list
                        regions.Add(new Region
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }

                    // Close the reader and the connection
                    reader.Close();
                    connection.Close();

                    // Return the list of regions
                    return regions;
                }

                // Close the reader and the connection
                reader.Close();
                connection.Close();

                // If no data found, return an empty list
                return new List<Region>();
            }
            catch (Exception ex)
            {
                // Handle and display any errors
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Return an empty list in case of an error
            return new List<Region>();
        }

        // GET BY ID: Region
        // Method to retrieve a region by its ID from the database
        public Region GetById(int id)
        {
            var region = new Region();

            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM regions WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            region.Id = reader.GetInt32(0);
                            region.Name = reader.GetString(1);
                        }

                        reader.Close();
                        connection.Close();

                        return region;
                    }

                    reader.Close();
                    connection.Close();

                    return region;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return region;
        }

        // INSERT: Region
        // Method to insert a new region into the database
        public string Insert(string name)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO regions VALUES (@name);";

            try
            {
                command.Parameters.Add(new SqlParameter("@name", name));
                connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;
                    var result = command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();

                    return result > 0 ? "Insert Success" : "Insert Failed";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return $"Error Transaction: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // UPDATE: Region
        // Method to update a region's data in the database by ID
        public string Update(int id, string name)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE regions SET name = @name WHERE id = @id;";

            try
            {
                command.Parameters.Add(new SqlParameter("@name", name));
                command.Parameters.Add(new SqlParameter("@id", id));
                connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;
                    var result = command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();

                    return result > 0 ? "Update Success" : "Update Failed";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return $"Error Transaction: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // DELETE: Region
        // Method to delete a region by ID from the database
        public string Delete(int id)
        {
            // Create a SQL connection
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            // Set the SQL delete query with a parameter for ID
            command.CommandText = "DELETE FROM regions WHERE id = @id;";

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

                    return result > 0 ? "Delete Success" : "Delete Failed";
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
