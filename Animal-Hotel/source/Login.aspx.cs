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
    public partial class Login : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                TextBox1.Text = userCookie.Values["username"];
                Session["username"] = userCookie.Values["username"];
                Response.Redirect("Site.aspx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string enteredUsername = TextBox1.Text;
            string enteredPassword = txtPass.Text;
            string enteredHashedPassword = Hash.HashPassword(enteredPassword);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                 try
                {
                    con.Open();
                    string getPasswordQuery = "SELECT password FROM signup WHERE username = @username";
                    using (SqlCommand cmd = new SqlCommand(getPasswordQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@username", enteredUsername);
                        
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string storedHashedPassword = result.ToString();

                            if (storedHashedPassword == enteredHashedPassword)
                            {
                                Session["username"] = enteredUsername;

                                
                                HttpCookie userCookie = new HttpCookie("UserInfo");
                                userCookie.Values["username"] = enteredUsername;
                                userCookie.Expires = DateTime.Now.AddDays(30);
                                Response.Cookies.Add(userCookie);

                                Response.Write("<script>alert('Login successful!');</script>");
                                Response.Redirect("Site.aspx");
                            }
                            else
                            {
                                Response.Write("<script>alert('Invalid password!');</script>");
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Username not found!');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('An error occurred: " + ex.Message + "');</script>");
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("SignUp.aspx");
        }
    }
}