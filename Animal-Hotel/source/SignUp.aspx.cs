using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Animal_Hotel.source
{
    public partial class SignUp : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;
        string gender = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            gender = "male";
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            gender = "female";
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            try
            {

                if (!ValidateInput())
                {
                    return;
                }

                if (string.IsNullOrEmpty(gender))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Please select a gender!');", true);
                    return;
                }

                string hashedPassword = Hash.HashPassword(txtPass.Text);
                string hashedConfirmPassword = Hash.HashPassword(txtConfPass.Text);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO [dbo].[signup]
                                ([name], [address], [gender], [phone], [email], 
                                    [username], [password], [confirmPassword])
                                VALUES 
                                (@name, @address, @gender, @phone, @email, 
                                    @username, @password, @confirmPassword)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", txtAdd.Text.Trim());
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@username", txtUser.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", hashedPassword.ToString());
                        cmd.Parameters.AddWithValue("@confirmPassword", hashedConfirmPassword.ToString());

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            Session["UserName"] = txtName.Text.Trim();
                            Session["UserAddress"] = txtAdd.Text.Trim();
                            Session["UserGender"] = gender;
                            Session["UserPhone"] = txtPhone.Text.Trim();
                            Session["UserEmail"] = txtEmail.Text.Trim();
                            Session["username"] = txtUser.Text.Trim();

                            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                "alert('Registration successful!'); window.location='Login.aspx';", true);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                "alert('Registration failed. Please try again.');", true);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                LogError(sqlEx);
                string userMessage = "Database error occurred. ";

                switch (sqlEx.Number)
                {
                    case 2627:
                        userMessage += "Username or email already exists.";
                        break;
                    case 4060:
                        userMessage += "Cannot access the database.";
                        break;
                    default:
                        userMessage += "Please try again later.";
                        break;
                }
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    $"alert('{userMessage}');", true);
            }

            catch (Exception ex)
            {
                LogError(ex);
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('An unexpected error occurred. Please try again later.');", true);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAdd.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUser.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtConfPass.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Please fill in all fields.');", true);
                return false;
            }

            if (txtPass.Text != txtConfPass.Text)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Passwords do not match.');", true);
                return false;
            }

            return true;
        }

        private bool TestDatabaseConnection()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    Response.Write($"<script>alert('Database Connection Error: {ex.Message}')</script>");
                    return false;
                }
            }
        }

        private void LogError(Exception ex)
        {
            try
            {
                string appDataPath = Server.MapPath("~/App_Data");
                if (!System.IO.Directory.Exists(appDataPath))
                {
                    System.IO.Directory.CreateDirectory(appDataPath);
                }

                string logPath = System.IO.Path.Combine(appDataPath, "ErrorLog.txt");
                string errorMessage = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] Error:\r\n" +
                                    $"Message: {ex.Message}\r\n" +
                                    $"Stack Trace: {ex.StackTrace}\r\n" +
                                    $"Source: {ex.Source}\r\n" +
                                    "----------------------------------------\r\n";

                System.IO.File.AppendAllText(logPath, errorMessage);
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Error occurred and logging failed. Please contact administrator.');", true);
            }
        }
    }
}