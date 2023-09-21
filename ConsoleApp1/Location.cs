using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    public class Locations
    {
        // Properties to represent location data
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string CountryId { get; set; }

        // Connection string for the database
        static string connectionString = "Data Source=DESKTOP-AJI5ESJ;Database=db_hr_dts;Integrated Security=True;Connect Timeout=30;";

        // GET ALL: Locations
        public List<Locations> GetAll()
        {
            // List to store locations
            var locations = new List<Locations>();

            // Create a SqlConnection and SqlCommand
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM locations";

            try
            {
                // Open the database connection
                connection.Open();

                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Add location data to the list
                        locations.Add(new Locations
                        {
                            Id = reader.GetInt32(0),
                            StreetAddress = reader.GetString(1),
                            PostalCode = reader.GetString(2),
                            City = reader.GetString(3),
                            StateProvince = reader.GetString(4),
                            CountryId = reader.GetString(5)
                        });
                    }
                    reader.Close();
                    connection.Close();

                    return locations;
                }
                reader.Close();
                connection.Close();

                return new List<Locations>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<Locations>();
        }

        // GET BY ID: Locations
        public Locations GetById(int id)
        {
            // Location object to store the result
            var location = new Locations();
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM locations WHERE id = @id";

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
                            // Fill location object with data
                            location.Id = reader.GetInt32(0);
                            location.StreetAddress = reader.GetString(1);
                            location.PostalCode = reader.GetString(2);
                            location.City = reader.GetString(3);
                            location.StateProvince = reader.GetString(4);
                            location.CountryId = reader.GetString(5);
                        }
                        reader.Close();
                        connection.Close();

                        return location;
                    }

                    reader.Close();
                    connection.Close();

                    return location;
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

            return location;
        }

        // INSERT: Locations
        public string Insert(string streetAddress, string postalCode, string city, string stateProvince, char countryId)
        {
            // Create a SqlConnection and SqlCommand
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand();

            command.Connection = connection;
            command.CommandText = "INSERT INTO locations (street_address, postal_code, city, state_province, country_id) VALUES (@streetAddress, @postalCode, @city, @stateProvince, @countryId);";

            try
            {
                // Add parameters to the command
                command.Parameters.Add(new SqlParameter("@streetAddress", streetAddress));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@stateProvince", stateProvince));
                command.Parameters.Add(new SqlParameter("@countryId", countryId));

                connection.Open();
                using var transaction = connection.BeginTransaction();
                try
                {
                    command.Transaction = transaction;

                    var result = command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();

                    return result.ToString();
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

        // UPDATE: Locations
        public string Update(int id, string streetAddress, string postalCode, string city, string stateProvince, char countryId)
        {
            // Create a SqlConnection and SqlCommand
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE locations SET street_address = @streetAddress, postal_code = @postalCode, city = @city, state_province = @stateProvince, country_id = @countryId WHERE id = @id;";

            try
            {
                // Add parameters to the command
                command.Parameters.Add(new SqlParameter("@streetAddress", streetAddress));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@stateProvince", stateProvince));
                command.Parameters.Add(new SqlParameter("@countryId", countryId));
                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                using var transaction = connection.BeginTransaction();
                try
                {
                    command.Transaction = transaction;

                    var result = command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();

                    return result.ToString();
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

        // DELETE: Locations
        public string Delete(int id)
        {
            // Create a SqlConnection and SqlCommand
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM locations WHERE id = @id;";

            try
            {
                // Add parameters to the command
                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                using var transaction = connection.BeginTransaction();
                try
                {
                    command.Transaction = transaction;

                    var result = command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();

                    return result.ToString();
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
    }
}
