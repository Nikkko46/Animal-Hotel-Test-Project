using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Animal_Hotel.source
{
    public partial class PetDashboard : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;
        private LoginRemember _loginManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            LoadPetData();

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

        private void LoadPetData()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM PetProfiles WHERE OwnerUsername = @username";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", Session["username"]);

                    try 
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            lblPetName.Text = reader["PetName"].ToString();
                            lblPetType.Text = reader["PetType"].ToString();
                            lblPetBreed.Text = reader["PetBreed"].ToString();
                            lblPetAge.Text = reader["PetAge"].ToString();
                            lblPetGender.Text = reader["PetGender"].ToString();
                            lblPetWeight.Text = reader["PetWeight"].ToString() + " kg";
                            lblMedicalConditions.Text = reader["MedicalConditions"].ToString();
                            lblVaccinationStatus.Text = reader["VaccinationStatus"].ToString();
                            lblDietaryNeeds.Text = reader["DietaryNeeds"].ToString();
                            lblBehavioralNotes.Text = reader["BehavioralNotes"].ToString();
                            
                            if (reader["LastCheckup"] != DBNull.Value)
                            {
                                lblLastCheckup.Text = Convert.ToDateTime(reader["LastCheckup"]).ToString("dd/MM/yyyy");
                            }

                            string petPicPath = "~/PetPictures/" + reader["PetID"].ToString() + ".jpg";
                            if (File.Exists(Server.MapPath(petPicPath)))
                            {
                                imgPet.ImageUrl = petPicPath;
                            }
                            else
                            {
                                imgPet.ImageUrl = "../img/default_pet.jpg";
                            }
                        }

                        else
                        {
                            Response.Write("<script>alert('No pet profile found. Please create one.');window.location='CreatePetProfile.aspx';</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Error loading pet data: " + ex.Message + "');</script>");
                    }
                }
            }
        }

        protected void btnUploadPetPicture_Click(object sender, EventArgs e)
        {
            if (filePetPicture.HasFile)
            {
                try
                {
                    string petId = GetPetId();
                    string fileExtension = Path.GetExtension(filePetPicture.FileName).ToLower();
                    string uploadPath = Server.MapPath("../img/");

                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        Response.Write("<script>alert('Only JPG and PNG files are allowed!');</script>");
                        return;
                    }

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    string exJpgPath = Path.Combine(uploadPath, petId + ".jpg");
                    string exPngPath = Path.Combine(uploadPath, petId + ".png");

                    if (File.Exists(exJpgPath))
                    {
                        File.Delete(exJpgPath);
                    }
                    if (File.Exists(exPngPath))
                    {
                        File.Delete(exPngPath);
                    }

                    string fileName = petId + fileExtension;
                    string fullPath = Path.Combine(uploadPath, fileName);
                    filePetPicture.SaveAs(fullPath);
                    imgPet.ImageUrl = "../img/" + fileName;
                    Response.Write("<script>alert('Pet picture updated successfully!');</script>");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error uploading picture: " + ex.Message + "');</script>");
                }
            }
        }

        private string GetPetId()
        {
            string petId = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT PetID FROM PetProfiles WHERE OwnerUsername = @username";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", Session["username"]);
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            petId = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error getting pet ID: " + ex.Message + "');</script>");
            }
            return petId;
        }

        protected void btnUpdatePet_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE PetProfiles 
                           SET MedicalConditions = @medicalConditions,
                               VaccinationStatus = @vaccinationStatus,
                               DietaryNeeds = @dietaryNeeds,
                               BehavioralNotes = @behavioralNotes,
                               LastCheckup = @lastCheckup,
                               PetWeight = @weight
                           WHERE OwnerUsername = @username";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        var dateLastCheckup = Convert.ToDateTime(txtLastCheckup.Text, new CultureInfo("ro-RO"));
                        var dateVaccination = Convert.ToDateTime(txtVaccinationStatus.Text, new CultureInfo("ro-RO"));

                        cmd.Parameters.AddWithValue("@medicalConditions", txtMedicalConditions.Text);
                        cmd.Parameters.AddWithValue("@vaccinationStatus", dateVaccination);
                        cmd.Parameters.AddWithValue("@dietaryNeeds", txtDietaryNeeds.Text);
                        cmd.Parameters.AddWithValue("@behavioralNotes", txtBehavioralNotes.Text);
                        cmd.Parameters.AddWithValue("@lastCheckup", dateLastCheckup);
                        cmd.Parameters.AddWithValue("@weight", txtPetWeight.Text);
                        cmd.Parameters.AddWithValue("@username", Session["username"]);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        Response.Write("<script>alert('Pet information updated successfully!');</script>");
                        LoadPetData();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error updating pet information: " + ex.Message + "');</script>");
            }
        }

        protected void btnDeletePet_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM PetProfiles WHERE OwnerUsername = @username";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", Session["username"]);
                        con.Open();
                        cmd.ExecuteNonQuery();

                        string petId = GetPetId();
                        string jpgPath = Server.MapPath("~/PetPictures/" + petId + ".jpg");
                        string pngPath = Server.MapPath("~/PetPictures/" + petId + ".png");
                        if (File.Exists(jpgPath))
                        {
                            File.Delete(jpgPath);
                        }

                        if (File.Exists(pngPath))
                        {
                            File.Delete(pngPath);
                        }


                        Response.Write("<script>alert('Pet profile deleted successfully!');window.location='Dashboard.aspx';</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error deleting pet profile: " + ex.Message + "');</script>");
            }
        }

        private bool _isEditMode = false;
        protected void btnToggleEdit_Click(object sender, EventArgs e)
        {
            _isEditMode = !_isEditMode;
            ToggleEditMode(_isEditMode);
        }

        private void ToggleEditMode(bool editMode)
        {
            lblMedicalConditions.Visible = !editMode;
            txtMedicalConditions.Visible = editMode;
            lblVaccinationStatus.Visible = !editMode;
            txtVaccinationStatus.Visible = editMode;
            lblDietaryNeeds.Visible = !editMode;
            txtDietaryNeeds.Visible = editMode;
            lblBehavioralNotes.Visible = !editMode;
            txtBehavioralNotes.Visible = editMode;
            lblLastCheckup.Visible = !editMode;
            txtLastCheckup.Visible = editMode;
            lblPetWeight.Visible = !editMode;
            txtPetWeight.Visible = editMode;
            btnUpdatePet.Visible = editMode;
            btnToggleEdit.Text = editMode ? "Cancel" : "Edit";

            if (editMode)
            {
                txtMedicalConditions.Text = lblMedicalConditions.Text;
                txtVaccinationStatus.Text = lblVaccinationStatus.Text;
                txtDietaryNeeds.Text = lblDietaryNeeds.Text;
                txtBehavioralNotes.Text = lblBehavioralNotes.Text;
                txtLastCheckup.Text = lblLastCheckup.Text;
                txtPetWeight.Text = lblPetWeight.Text.Replace(" kg", "");
            }
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
                        <a href='Dashboard.aspx' class='dropdown-item'>You</a>
                        <a class='dropdown-item'>Your Pet</a>
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