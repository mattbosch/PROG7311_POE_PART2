﻿using POETEST_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace POETEST_MVC.Services.Data
{
    public class SecurityDAO
    {
        //global variable declaration and connection string declaration
        int globalFarmerID = 0;
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\POEdb.mdf;";
        //Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False

        //Method to find a user, given a username and password, if the user exists the role of the user will be retrieved.
        internal bool FindUser(User user, out string role)
        {
            bool success = false;
            role = null;

            string queryString = "SELECT * FROM dbo.Users WHERE username = @Username AND password = @Password";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        success = true;
                        if (reader.Read())
                        {
                            role = reader.GetString(reader.GetOrdinal("role"));
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

            return success;

        }

        //method to retrieve a userID, given a username and password.
        public int getUserID(User user)
        {
            int userID = 0;
            string queryString = "SELECT userID FROM dbo.Users WHERE username = @Username AND password = @Password";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                try
                {
                    con.Open();
                    userID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

            return userID;
        }

        //Method to retrieve the farmerID, given the userID.
        public int getFarmerID(int userID)
        {
            int newFarmerID = 0;
            string queryString = "SELECT farmerID FROM dbo.Farmers WHERE UserID = @userID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                cmd.Parameters.AddWithValue("@userID", userID);

                try
                {
                    con.Open();
                    newFarmerID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

            return newFarmerID;
        }

        //Method to fetch all the farmers from the Farmer table
        public List<Farmer> fetchFarmers()
        {
            List<Farmer> returnList = new List<Farmer>();

            string queryString = "SELECT * FROM dbo.Farmers";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Farmer farmer = new Farmer();
                            farmer.farmerID = reader.GetInt32(0);
                            farmer.FarmerName = reader.GetString(1);
                            farmer.UserID = reader.GetInt32(2);

                            returnList.Add(farmer);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

                return returnList;

        }

        //Method to fetch all records from the products table, with the corresponding farmerID.
        public List<Product> fetchProducts(int farmerID)
        {
            globalFarmerID = farmerID;
            List<Product> returnList = new List<Product>(); 

            string queryString = "SELECT * FROM dbo.Products WHERE farmerID = @farmerID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                try
                {
                    cmd.Parameters.AddWithValue("@farmerID", farmerID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Product product = new Product();
                            product.productID = reader.GetInt32(0);
                            product.product = reader.GetString(1);
                            product.type = reader.GetString(2);
                            product.date = reader.GetDateTime(3);
                            product.farmerID = reader.GetInt32(4);

                            returnList.Add(product);

                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

            return returnList;

        }

        //Method to create a new user
        public int createUser(User user)
        {
            int userID = 0;
            string queryString = "INSERT INTO dbo.Users VALUES (@Username, @Password, @Role)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT @@IDENTITY";
                    userID = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

            return userID;
        }

        //Method to create a new farmer and insert them into the database, while also creating a new user for the farmer to use as a log on.
        public void createFarmer(Farmer farmer)
        {
            User newUser = new User();
            newUser.Username = farmer.username;
            newUser.Password = farmer.password;
            newUser.Role = "Farmer";

            string queryString = "INSERT INTO dbo.Farmers VALUES (@FarmerName, @UserID)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                cmd.Parameters.AddWithValue("@FarmerName", farmer.FarmerName);
                cmd.Parameters.AddWithValue("@UserID", createUser(newUser));

                try
                {
                    con.Open();

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

        }

        //Method to create a new product and insert it into the database
        public void createProduct(Product product)
        {

            string queryString = "INSERT INTO dbo.Products VALUES (@productName, @type, @date, @farmerID)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                cmd.Parameters.AddWithValue("@productName", product.product);
                cmd.Parameters.AddWithValue("@type", product.type);
                cmd.Parameters.AddWithValue("@date", product.date);
                cmd.Parameters.AddWithValue("@farmerID", product.farmerID);

                try
                {
                    con.Open();

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

        }

        //Method to fetch the products within the specified date range from the database
        public List<Product> fetchProductsDateFilter(int farmerID, DateTime startDate, DateTime endDate)
        {
            List<Product> returnList = new List<Product>();

            string queryString = "SELECT * FROM dbo.Products WHERE (farmerID = @farmerID) AND (date BETWEEN @startDate AND @endDate)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, con);

                try
                {
                    cmd.Parameters.AddWithValue("@farmerID", farmerID);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Product product = new Product();
                            product.productID = reader.GetInt32(0);
                            product.product = reader.GetString(1);
                            product.type = reader.GetString(2);
                            product.date = reader.GetDateTime(3);
                            product.farmerID = reader.GetInt32(4);

                            returnList.Add(product);

                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

            return returnList;

        }


    }
}