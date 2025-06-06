﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Animal_Hotel.source
{
    public partial class CreatePetProfile : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;
        private LoginRemember _loginManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            if (!IsPostBack)
            {
                if (!ddlPetType.Items.Contains(new ListItem("Cat")))
                {
                    ddlPetType.Items.Add(new ListItem("Cat", "Cat"));
                }

                if (!ddlPetType.Items.Contains(new ListItem("Dog")))
                {
                    ddlPetType.Items.Add(new ListItem("Dog", "Dog"));
                }

                if (!ddlPetType.Items.Contains(new ListItem("Bird")))
                {
                    ddlPetType.Items.Add(new ListItem("Bird", "Bird"));
                }

                if (!ddlPetType.Items.Contains(new ListItem("Fish")))
                {
                    ddlPetType.Items.Add(new ListItem("Fish", "Fish"));
                }

                if (!ddlPetType.Items.Contains(new ListItem("Other")))
                {
                    ddlPetType.Items.Add(new ListItem("Other", "Other"));
                }
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (Session["username"] == null)
            {
                Response.Write("<script>alert('Please log in to create a pet profile.');</script>");
                return;
            }
            
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string petType = ddlPetType.SelectedValue;
                
                if (petType == "Other" && !string.IsNullOrEmpty(ddlOtherType.SelectedValue))
                {
                    petType = $"Other, {ddlOtherType.SelectedValue}";
                }

                string query = @"INSERT INTO PetProfiles 
                    (OwnerUsername, PetName, PetType, PetBreed, PetAge, PetGender, PetWeight, 
                    MedicalConditions, VaccinationStatus, DietaryNeeds, BehavioralNotes) 
                    VALUES 
                    (@OwnerUsername, @PetName, @PetType, @PetBreed, @PetAge, @PetGender, @PetWeight, 
                    @MedicalConditions, @VaccinationStatus, @DietaryNeeds, @BehavioralNotes)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    var dateVaccination = Convert.ToDateTime(txtVaccinationStatus.Text, new CultureInfo("ro-RO"));

                    cmd.Parameters.AddWithValue("@OwnerUsername", Session["username"].ToString());
                    cmd.Parameters.AddWithValue("@PetName", txtPetName.Text);
                    cmd.Parameters.AddWithValue("@PetType", ddlPetType.SelectedValue);
                    cmd.Parameters.AddWithValue("@PetBreed", txtPetBreed.Text);
                    cmd.Parameters.AddWithValue("@PetAge", Convert.ToInt32(txtPetAge.Text));
                    cmd.Parameters.AddWithValue("@PetGender", ddlPetGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@PetWeight", Convert.ToDecimal(txtPetWeight.Text));
                    cmd.Parameters.AddWithValue("@MedicalConditions", txtMedicalConditions.Text);
                    cmd.Parameters.AddWithValue("@VaccinationStatus", dateVaccination);
                    cmd.Parameters.AddWithValue("@DietaryNeeds", txtDietaryNeeds.Text);
                    cmd.Parameters.AddWithValue("@BehavioralNotes", txtBehavioralNotes.Text);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        Response.Redirect("PetDashboard.aspx");
                        Response.Write("<script>alert('Pet Profile registered successfully!');</script>");
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                    }
                }
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
                        <a href='PetDashboard.aspx' class='dropdown-item'>Your Pet</a>
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