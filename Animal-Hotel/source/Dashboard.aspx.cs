using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

namespace Animal_Hotel.source
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;
        private LoginRemember _loginManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            if (!IsPostBack)
            {           
                LoadUserData();
                CheckForPetProfile();
            }

            if (!Page.IsPostBack)
            {
                BuildNavigationMenu();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            _loginManager.HandleLogout();
        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _loginManager.HandleLanguageChange(ddlLanguage.SelectedValue);
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            _loginManager.HandleCurrencyChange(ddlCurrency.SelectedValue);
        }

        private void LoadUserData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT name, address, phone, email, username FROM signup WHERE username = @username";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", Session["username"]);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblFullName.Text = reader["name"].ToString();
                                lblUserUsername.Text = reader["username"].ToString();
                                lblEmail.Text = reader["email"].ToString();
                                lblPhone.Text = reader["phone"].ToString();
                                lblAddress.Text = reader["address"].ToString();

                                string username = Session["username"].ToString();
                                string profilePicJpgPath = Server.MapPath("~/ProfilePictures/" + username + ".jpg");
                                string profilePicPngPath = Server.MapPath("~/ProfilePictures/" + username + ".png");
                                if (File.Exists(profilePicJpgPath))
                                {
                                    imgProfile.ImageUrl = "~/ProfilePictures/" + username + ".jpg";
                                }
                                else
                                {
                                    imgProfile.ImageUrl = "../img/default_profile.png";
                                }

                                if (File.Exists(profilePicPngPath))
                                {
                                    imgProfile.ImageUrl = "~/ProfilePictures/" + username + ".png";
                                }
                                else
                                {
                                    imgProfile.ImageUrl = "../img/default_profile.png";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error loading user data: " + ex.Message + "');</script>");
            }
        }

        private void CheckForPetProfile()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT * FROM PetProfiles WHERE OwnerUsername = @username";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", Session["username"]);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pnlPetExists.Visible = true;
                                pnlNoPet.Visible = false;

                                lblPetName.Text = reader["PetName"].ToString();
                                lblPetType.Text = reader["PetType"].ToString();
                                lblPetBreed.Text = reader["PetBreed"].ToString();
                                lblPetAge.Text = reader["PetAge"].ToString();

                                string petPicJpgPath = Server.MapPath("~/PetPictures/" + reader["PetID"].ToString() + ".jpg");
                                string petPicPngPath = Server.MapPath("~/PetPictures/" + reader["PetID"].ToString() + ".png");
                                if (File.Exists(petPicJpgPath))
                                {
                                    imgPet.ImageUrl = "~/PetPictures/" + reader["PetID"].ToString() + ".jpg";
                                }
                                else
                                {
                                    imgPet.ImageUrl = "~/img/default_pet.jpg";
                                }
                                if (File.Exists(petPicPngPath))
                                {
                                    imgPet.ImageUrl = "~/PetPictures/" + reader["PetID"].ToString() + ".png";
                                }
                                else
                                {
                                    imgPet.ImageUrl = "~/img/default_pet.jpg";
                                }
                            }
                            else
                            {
                                pnlPetExists.Visible = false;
                                pnlNoPet.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error checking pet profile: " + ex.Message + "');</script>");
            }
        }

        protected void btnUploadPicture_Click(object sender, EventArgs e)
        {
            if (fileProfilePicture.HasFile)
            {
                try
                {
                    string username = Session["username"].ToString();
                    string fileExtension = Path.GetExtension(fileProfilePicture.FileName).ToLower();
                    string uploadPath = Server.MapPath("~/ProfilePictures/");

                    if (fileExtension != ".jpg" && fileExtension != ".png")
                    {
                        Response.Write("<script>alert('Only JPG and PNG files are allowed!');</script>");
                        return;
                    }
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string existingJpgPath = Path.Combine(uploadPath, username + ".jpg");
                    string existingPngPath = Path.Combine(uploadPath, username + ".png");

                    if (File.Exists(existingJpgPath))
                    {
                        File.Delete(existingJpgPath);
                    }
                    if (File.Exists(existingPngPath))
                    {
                        File.Delete(existingPngPath);
                    }
                    string fileName = username + fileExtension;
                    string fullPath = Path.Combine(uploadPath, fileName);
                    fileProfilePicture.SaveAs(fullPath);
                    imgProfile.ImageUrl = "~/ProfilePictures/" + fileName;
                    Response.Write("<script>alert('Profile picture updated successfully!');</script>");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error uploading picture: " + ex.Message + "');</script>");
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE signup 
                                   SET PreferredContact = @PreferredContact, 
                                       EmergencyContact = @EmergencyContact 
                                   WHERE username = @username";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PreferredContact", ddlContactMethod.SelectedValue);
                        cmd.Parameters.AddWithValue("@EmergencyContact", txtEmergencyContact.Text);
                        cmd.Parameters.AddWithValue("@username", Session["username"]);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        Response.Write("<script>alert('Profile updated successfully!');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error updating profile: " + ex.Message + "');</script>");
            }
        }

        protected void btnCreatePetProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreatePetProfile.aspx");
        }

        protected void imgPet_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("PetDashboard.aspx");
        }

        private void BuildNavigationMenu()
        {
            Literal navMenu = new Literal();
            string menuHtml = @"
                <div class='nav-item'>
                    <a href='Home.aspx' class='nav-link'>Home</a>
                </div>
                
                <div class='nav-item'>
                    <a href='#' class='nav-link'>Pets</a>
                    <div class='dropdown'>
                        <a href='Catalogue.aspx' class='dropdown-item'>Catalogue</a>
                        <div class='sub-dropdown'>
                            <a href='Cats.aspx' class='dropdown-item'>Cats</a>
                            <a href='Dogs.aspx' class='dropdown-item'>Dogs</a>
                            <a href='Birds.aspx' class='dropdown-item'>Birds</a>
                            <a href='Fish.aspx' class='dropdown-item'>Fish</a>
                            <a href='Others.aspx' class='dropdown-item'>Others</a>
                        </div>
                        <a href='Guests.aspx' class='dropdown-item'>Guests</a>
                        <a href='FluffsOfTheMonth.aspx' class='dropdown-item'>Fluffs of the Month</a>
                    </div>
                </div>

                <div class='nav-item'>
                    <a href='#' class='nav-link'>Services</a>
                    <div class='dropdown'>
                        <a href='#' class='dropdown-item'>Hotel</a>
                        <div class='sub-dropdown'>
                            <a href='Indoor.aspx' class='dropdown-item'>Indoor</a>
                            <a href='Outdoor.aspx' class='dropdown-item'>Outdoor</a>
                        </div>
                        <a href='#' class='dropdown-item'>Lounge</a>
                        <div class='sub-dropdown'>
                            <a href='Spa.aspx' class='dropdown-item'>Spa</a>
                            <a href='Clinic.aspx' class='dropdown-item'>Clinic</a>
                            <a href='PlayZone.aspx' class='dropdown-item'>Play Zone</a>
                        </div>
                    </div>
                </div>

                <div class='nav-item'>
                    <a href='Pricing.aspx' class='nav-link'>Pricing</a>
                </div>

                <div class='nav-item'>
                    <a href='#' class='nav-link'>Dashboard</a>
                    <div class='dropdown'>
                        <a class='dropdown-item'>You</a>
                        <a href='PetProfile.aspx' class='dropdown-item'>Your Pet</a>
                    </div>
                </div>

                <div class='nav-item'>
                    <a href='#' class='nav-link'>About Us</a>
                    <div class='dropdown'>
                        <a href='LearnMore.aspx' class='dropdown-item'>Learn More</a>
                        <a href='FAQ.aspx' class='dropdown-item'>FAQs</a>
                        <a href='Contact.aspx' class='dropdown-item'>Contact Us</a>
                    </div>
                </div>";

            navMenu.Text = menuHtml;
            Control navContainer = FindControl("nav-menu");
            if (navContainer != null)
            {
                navContainer.Controls.Add(navMenu);
            }
        }
    }
}

