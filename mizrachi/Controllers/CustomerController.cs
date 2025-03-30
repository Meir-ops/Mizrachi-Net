using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using SqlCommand= Microsoft.Data.SqlClient.SqlCommand;
using SqlConnection= Microsoft.Data.SqlClient.SqlConnection;
using SqlConnectionStringBuilder= Microsoft.Data.SqlClient.SqlConnectionStringBuilder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace mizrachi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        public CustomerController()
        {
        }

        // GET method to retrieve customer details
        [HttpGet(Name= "GetCustomer")]
        public string GetAsync(string? id)
        {
            var builder= new SqlConnectionStringBuilder
            {
                DataSource= "MEIR\\SQLEXPRESS",
                InitialCatalog= "WideWorldImporters;Trusted_Connection=SSPI;Encrypt=False;TrustServerCertificate=True;"
            };

            var connectionString= "data source=MEIR\\SQLEXPRESS;initial catalog=WideWorldImporters;trusted_connection=True;TrustServerCertificate=True;";
            string table= "";
            using (SqlConnection connection= new SqlConnection(connectionString))
            {
                connection.Open();
                var sql= "select * from [Application].[People]";
                if (id != null)
                {
                    sql += " where PersonID= " + id;
                }
                DataTable dt= new DataTable();
                SqlCommand cmd= new SqlCommand(sql, connection);
                dt.Load(cmd.ExecuteReader());
                table= JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            Console.WriteLine("\nQuery data example:");
            Console.WriteLine(table);
            return table;
        }
        // DELETE method to remove a customer
        [HttpDelete]
        public IActionResult DeleteAsync(string? id)
        {
            var connectionString= "data source=MEIR\\SQLEXPRESS;initial catalog=WideWorldImporters;trusted_connection=True;TrustServerCertificate=True;";
            string table= "";
            using (SqlConnection connection= new SqlConnection(connectionString))
            {
                connection.Open();
                string sql= "DELETE FROM [Application].[People] WHERE PersonID=" + id;
                using (SqlConnection conn= new(connectionString))
                {
                    SqlCommand cmd= new(sql, conn);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
           

            return StatusCode((int)HttpStatusCode.OK);
        }

        // POST method to create a new customer
        [HttpPost]
        public IActionResult CreateUser([FromBody] Customers customers)
        {
            var connectionString= "data source=MEIR\\SQLEXPRESS;initial catalog=WideWorldImporters;trusted_connection=True;TrustServerCertificate=True;";
            string table= "";
            using (SqlConnection connection= new SqlConnection(connectionString))
            {
                connection.Open();

                string sql= "INSERT INTO [Application].[People]  ( PersonID, FullName  , PreferredName  , IsPermittedToLogon  , LogonName  , IsExternalLogonProvider  , IsSystemUser  , IsEmployee  , IsSalesperson  , UserPreferences  , PhoneNumber  , FaxNumber  , EmailAddress    , LastEditedBy ) VALUES ";
                sql += "(@PersonID, @FullName  , @PreferredName  , @IsPermittedToLogon  , @LogonName  , @IsExternalLogonProvider  , @IsSystemUser  , @IsEmployee  , @IsSalesperson  , @UserPreferences  , @PhoneNumber  , @FaxNumber  , @EmailAddress   , @LastEditedBy )";

                SqlCommand cmd= new SqlCommand(sql, connection);
                cmd.Parameters.Add("@PersonID", SqlDbType.Int).Value= getLastPersonID()+1;
                cmd.Parameters.Add("@FullName", SqlDbType.VarChar).Value= customers.FullName;
                cmd.Parameters.Add("@PreferredName", SqlDbType.VarChar).Value= customers.PreferredName;
                cmd.Parameters.Add("@IsPermittedToLogon", SqlDbType.Bit).Value= customers.IsPermittedToLogon;
                cmd.Parameters.Add("@LogonName", SqlDbType.VarChar).Value= customers.LogonName;
                cmd.Parameters.Add("@IsExternalLogonProvider", SqlDbType.Bit).Value= customers.IsExternalLogonProvider;
                cmd.Parameters.Add("@HashedPassword", SqlDbType.VarChar).Value= customers.HashedPassword;
                cmd.Parameters.Add("@IsSystemUser", SqlDbType.Bit).Value= customers.IsSystemUser;
                cmd.Parameters.Add("@IsEmployee", SqlDbType.Bit).Value= customers.IsEmployee;
                cmd.Parameters.Add("@IsSalesperson", SqlDbType.Bit).Value= customers.IsSalesperson;
                cmd.Parameters.Add("@UserPreferences", SqlDbType.VarChar).Value= customers.UserPreferences;
                cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar).Value= customers.PhoneNumber;
                cmd.Parameters.Add("@FaxNumber", SqlDbType.VarChar).Value= customers.FaxNumber;
                cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value= customers.EmailAddress;
                //cmd.Parameters.Add("@CustomFields", SqlDbType.VarChar).Value= customers.CustomFields;
                //cmd.Parameters.Add("@OtherLanguages", SqlDbType.VarChar).Value= "[\"Swedish\"]";
                cmd.Parameters.Add("@LastEditedBy", SqlDbType.Int).Value= 3261;

                cmd.ExecuteNonQuery();
            }
            return StatusCode((int)HttpStatusCode.OK);
        }

        // PUT: Updates an existing customer based on provided details
        [HttpPut]
        public IActionResult Update(Customers customers)
        {
            var connectionString= "data source=MEIR\\SQLEXPRESS;initial catalog=WideWorldImporters;trusted_connection=True;TrustServerCertificate=True;";
            string table= "";
            using (SqlConnection connection= new SqlConnection(connectionString)) 
            {
                connection.Open();

                string sql= "UPDATE [Application].[People] SET";
                if (customers.PersonID != null)
                    sql += " PersonID=@PersonID,";
                if (customers.FullName != null && customers.FullName.Length > 0)
                    sql += " FullName=@FullName,";
                if (customers.PreferredName != null && customers.PreferredName.Length > 0)
                    sql += " PreferredName=@PreferredName,";
                if(!customers.IsPermittedToLogon || !customers.IsPermittedToLogon)
                    sql += " IsPermittedToLogon=@IsPermittedToLogon,";
                if (customers.LogonName != null && customers.LogonName.Length > 0)
                    sql += " LogonName=@LogonName,";
                if (!customers.IsExternalLogonProvider || !customers.IsExternalLogonProvider)
                    sql += " IsExternalLogonProvider=@IsExternalLogonProvider,";
                if (!customers.IsSystemUser || !customers.IsSystemUser)
                    sql += " IsSystemUser=@IsSystemUser,";
                if (!customers.IsEmployee || !customers.IsEmployee)
                    sql += " IsEmployee=@IsEmployee,";
                if (!customers.IsSalesperson || !customers.IsSalesperson)
                    sql += " IsSalesperson=@IsSalesperson,";
                if (customers.UserPreferences != null && customers.UserPreferences.Length > 0)
                    sql += " UserPreferences=@UserPreferences,";
                if (customers.PhoneNumber != null && customers.PhoneNumber.Length > 0)
                    sql += " PhoneNumber=@PhoneNumber,";
                if (customers.FaxNumber != null && customers.FaxNumber.Length > 0)
                    sql += " FaxNumber=@FaxNumber,";
                if (customers.EmailAddress != null && customers.EmailAddress.Length > 0)
                    sql += " EmailAddress=@EmailAddress,";
                if (customers.LastEditedBy != null)
                    sql += " LastEditedBy=@LastEditedBy";
                sql += " WHERE PersonID=" + customers.PersonID;

                SqlCommand cmd= new SqlCommand(sql, connection);
                if (customers.PersonID != null)
                    cmd.Parameters.Add("@PersonID", SqlDbType.Int).Value= customers.PersonID;
                if (customers.FullName != null && customers.FullName.Length > 0)
                    cmd.Parameters.Add("@FullName", SqlDbType.VarChar).Value= customers.FullName;
                if (customers.PreferredName != null && customers.PreferredName.Length > 0)
                    cmd.Parameters.Add("@PreferredName", SqlDbType.VarChar).Value= customers.PreferredName;
                if (!customers.IsPermittedToLogon || !customers.IsPermittedToLogon)
                    cmd.Parameters.Add("@IsPermittedToLogon", SqlDbType.Bit).Value= customers.IsPermittedToLogon;
                if (customers.LogonName != null && customers.LogonName.Length > 0)
                    cmd.Parameters.Add("@LogonName", SqlDbType.VarChar).Value= customers.LogonName;
                if (!customers.IsExternalLogonProvider || !customers.IsExternalLogonProvider)
                    cmd.Parameters.Add("@IsExternalLogonProvider", SqlDbType.Bit).Value= customers.IsExternalLogonProvider;
                if (!customers.IsSystemUser || !customers.IsSystemUser)
                    cmd.Parameters.Add("@IsSystemUser", SqlDbType.Bit).Value= customers.IsSystemUser;
                if (!customers.IsEmployee || !customers.IsEmployee)
                    cmd.Parameters.Add("@IsEmployee", SqlDbType.Bit).Value= customers.IsEmployee;
                if (!customers.IsSalesperson || !customers.IsSalesperson)
                    cmd.Parameters.Add("@IsSalesperson", SqlDbType.Bit).Value= customers.IsSalesperson;
                if (customers.UserPreferences != null && customers.UserPreferences.Length > 0)
                    cmd.Parameters.Add("@UserPreferences", SqlDbType.VarChar).Value= customers.UserPreferences;
                if (customers.PhoneNumber != null && customers.PhoneNumber.Length > 0)
                    cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar).Value= customers.PhoneNumber;
                if (customers.FaxNumber != null && customers.FaxNumber.Length > 0)
                    cmd.Parameters.Add("@FaxNumber", SqlDbType.VarChar).Value= customers.FaxNumber;
                if (customers.EmailAddress != null && customers.EmailAddress.Length > 0)
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value= customers.EmailAddress;
                if (customers.LastEditedBy != null)
                    cmd.Parameters.Add("@LastEditedBy", SqlDbType.Int).Value= 1;

                cmd.ExecuteNonQuery();
            }
            
            return StatusCode((int)HttpStatusCode.OK);
        }

        // Method to get the last PersonID from the database
        public static int getLastPersonID()
        {
            var connectionString= "data source=MEIR\\SQLEXPRESS;initial catalog=WideWorldImporters;trusted_connection=True;TrustServerCertificate=True;";

            var newProdID= 0;
            const string sql=
                "SELECT MAX(PersonID) FROM  [WideWorldImporters].[Application].[People]";
            using (SqlConnection conn= new(connectionString))
            {
                SqlCommand cmd= new(sql, conn);
                try
                {
                    conn.Open();
                    newProdID= (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return newProdID;
        }
    } 
}
