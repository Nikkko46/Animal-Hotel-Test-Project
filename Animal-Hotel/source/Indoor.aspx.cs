using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Animal_Hotel.source
{
    public partial class Indoor : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;
        private LoginRemember _loginManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            if (!IsPostBack) 
            {
                if (Request.QueryString["autoFill"] == "true") 
                {
                    pnlManualEntry.Visible = true;
                    string petType = Request.QueryString["petType"];
                    string breedName = Request.QueryString["breedName"];
                    string breedType = Request.QueryString["breedType"];

                    if (!string.IsNullOrEmpty(petType))
                    {
                        ddlPetType.SelectedValue = petType;

                        if (petType == "Other")
                        {
                            pnlOtherType.Visible = true;
                            breedSection.Visible = false;

                            if (!string.IsNullOrEmpty(breedType))
                            {
                                ddlOtherType.SelectedValue = breedType;
                                ddlOtherType_SelectedIndexChanged(ddlOtherType, EventArgs.Empty);
                            }
                        }
                        else
                        {
                            pnlOtherType.Visible = false;
                            breedSection.Visible = true;

                            if (!string.IsNullOrEmpty(breedName))
                            {
                                LoadBreeds(petType);
                                txtBreed.Text = breedName;
                            }
                        }

                        ddlPetType_SelectedIndexChanged(ddlPetType, EventArgs.Empty);
                    }
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

        protected void rblReservationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReservationType.SelectedValue == "manual")
            {
                pnlManualEntry.Visible = true;
                pnlExistingPets.Visible = false;
            }
            else if (rblReservationType.SelectedValue == "existing")
            {
                pnlManualEntry.Visible = false;
                pnlExistingPets.Visible = true;
                LoadExistingPets();
            }
        }

        protected void ddlPetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPetType.SelectedValue == "Other")
            {
                pnlOtherType.Visible = true;
                breedSection.Visible = false;
            }
            else
            {
                pnlOtherType.Visible = false;
                breedSection.Visible = true;
                LoadBreeds(ddlPetType.SelectedValue);
            }
        }

        protected void ddlOtherType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlOtherType.SelectedValue))
            {
                LoadBreeds("Other-" + ddlOtherType.SelectedValue);
            }
        }

        private void LoadBreeds(string petType)
        {
             breedList.InnerHtml = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT PetBreed FROM PetProfiles WHERE PetType = @PetType AND IndoorOutdoor = 'Indoor'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PetType", petType);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string breed = reader["PetBreed"].ToString();
                        breedList.InnerHtml += $"<option value='{breed}'>";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error loading breeds: " + ex.Message;
                    lblMessage.CssClass = "message error";
                }
            }
        }

        private void LoadExistingPets()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT PetID, PetName, PetType, PetBreed FROM PetProfiles " +
                             "WHERE OwnerUsername = @username AND IndoorOutdoor = 'Indoor'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", Session["username"]?.ToString());

                try
                {
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvPets.DataSource = dt;
                    gvPets.DataBind();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error loading pets: " + ex.Message;
                    lblMessage.CssClass = "message error";
                }
            }
        }

        protected void gvPets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectPet")
            {
                int petId = Convert.ToInt32(e.CommandArgument);
                CreateReservation(petId);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateDates())
            {
                return;
            }

            DateTime checkIn = DateTime.Parse(txtCheckIn.Text);
            DateTime checkOut = DateTime.Parse(txtCheckOut.Text);

            if (rblReservationType.SelectedValue == "manual")
            {
                if (string.IsNullOrEmpty(ddlPetType.SelectedValue) ||
                    (ddlPetType.SelectedValue == "Other" && string.IsNullOrEmpty(ddlOtherType.SelectedValue)) ||
                    (ddlPetType.SelectedValue != "Other" && string.IsNullOrEmpty(txtBreed.Text)) ||
                    string.IsNullOrEmpty(txtPetName.Text) ||
                    string.IsNullOrEmpty(txtAge.Text) ||
                    string.IsNullOrEmpty(ddlGender.SelectedValue) ||
                    string.IsNullOrEmpty(txtWeight.Text))
                {
                    lblMessage.Text = "Please fill in all required fields.";
                    lblMessage.CssClass = "message error";
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        string insertPetQuery = @"INSERT INTO PetProfiles 
                            (OwnerUsername, PetName, PetType, PetBreed, PetAge, PetGender, 
                             PetWeight, IndoorOutdoor, MedicalConditions) 
                            VALUES 
                            (@username, @PetName, @PetType, @PetBreed, @Age, @Gender, 
                             @Weight, 'Indoor', @AdditionalInfo); 
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(insertPetQuery, con, transaction);
                        cmd.Parameters.AddWithValue("@username", Session["username"].ToString());
                        cmd.Parameters.AddWithValue("@PetName", txtPetName.Text);
                        cmd.Parameters.AddWithValue("@PetType", ddlPetType.SelectedValue);

                        if (ddlPetType.SelectedValue == "Other")
                        {
                            cmd.Parameters.AddWithValue("@PetBreed", ddlOtherType.SelectedValue);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@PetBreed", txtBreed.Text);
                        }

                        cmd.Parameters.AddWithValue("@Age", Convert.ToInt32(txtAge.Text));
                        cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                        cmd.Parameters.AddWithValue("@Weight", Convert.ToDecimal(txtWeight.Text));
                        cmd.Parameters.AddWithValue("@AdditionalInfo", txtAdditionalInfo.Text);

                        int petId = Convert.ToInt32(cmd.ExecuteScalar());

                        CreateReservation(petId, con, transaction);

                        transaction.Commit();
                        Response.Redirect("ReservationSuccess.aspx");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lblMessage.Text = "Error creating reservation: " + ex.Message;
                        lblMessage.CssClass = "message error";
                    }
                }
            }
        }

        private void CreateReservation(int petId, SqlConnection con = null, SqlTransaction transaction = null)
        {
            bool localConnection = false;
            SqlConnection connectionToUse = con;
            SqlTransaction transactionToUse = transaction;

            try
            {
                if (connectionToUse == null)
                {
                    connectionToUse = new SqlConnection(connectionString);
                    connectionToUse.Open();
                    localConnection = true;
                    transactionToUse = connectionToUse.BeginTransaction();
                }

                if (string.IsNullOrEmpty(txtCheckIn.Text) || string.IsNullOrEmpty(txtCheckOut.Text))
                {
                    throw new Exception("Please select both check-in and check-out dates.");
                }

                DateTime checkIn = DateTime.Parse(txtCheckIn.Text);
                DateTime checkOut = DateTime.Parse(txtCheckOut.Text);

                if (checkIn < DateTime.Today)
                {
                    throw new Exception("Check-in date cannot be in the past.");
                }

                if (checkOut <= checkIn)
                {
                    throw new Exception("Check-out date must be after check-in date.");
                }

                string insertReservationQuery = @"INSERT INTO Reservations 
                    (PetID, OwnerUsername, ReservationType, AdditionalNotes, ReservationDate, CheckInDate, CheckOutDate) 
                    VALUES 
                    (@PetID, @username, 'Indoor', @AdditionalInfo, GETDATE(), @CheckInDate, @CheckOutDate)";

                SqlCommand cmd = new SqlCommand(insertReservationQuery, connectionToUse, transactionToUse);
                cmd.Parameters.AddWithValue("@PetID", petId);
                cmd.Parameters.AddWithValue("@username", Session["username"].ToString());
                cmd.Parameters.AddWithValue("@AdditionalInfo", txtAdditionalInfo.Text);
                cmd.Parameters.AddWithValue("@CheckInDate", checkIn);
                cmd.Parameters.AddWithValue("@CheckOutDate", checkOut);

                cmd.ExecuteNonQuery();

                if (localConnection)
                {
                    transactionToUse.Commit();
                    Response.Redirect("ReservationSuccess.aspx");
                }
            }
            catch (Exception ex)
            {
                if (transactionToUse != null)
                {
                    transactionToUse.Rollback();
                }

                lblMessage.Text = "Error creating reservation: " + ex.Message;
                lblMessage.CssClass = "message error";
                if (localConnection)
                {
                    connectionToUse.Close();
                }
                throw;
            }
            finally
            {
                if (localConnection && connectionToUse != null)
                {
                    connectionToUse.Close();
                }
            }
        }

        private bool ValidateDates()
        {
            if (string.IsNullOrEmpty(txtCheckIn.Text) || string.IsNullOrEmpty(txtCheckOut.Text))
            {
                lblMessage.Text = "Please select both check-in and check-out dates.";
                lblMessage.CssClass = "message error";
                return false;
            }

            DateTime checkIn, checkOut;
            
            if (!DateTime.TryParse(txtCheckIn.Text, out checkIn) || !DateTime.TryParse(txtCheckOut.Text, out checkOut))
            {
                lblMessage.Text = "Please enter valid dates.";
                lblMessage.CssClass = "message error";
                return false;
            }

            if (checkIn < DateTime.Today)
            {
                lblMessage.Text = "Check-in date cannot be in the past.";
                lblMessage.CssClass = "message error";
                return false;
            }

            if (checkOut <= checkIn)
            {
                lblMessage.Text = "Check-out date must be after check-in date.";
                lblMessage.CssClass = "message error";
                return false;
            }

            return true;
        }

        private bool CheckAvailability(DateTime checkIn, DateTime checkOut, int petId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM Reservations 
                                WHERE PetID = @PetID 
                                AND ((CheckInDate BETWEEN @CheckIn AND @CheckOut) 
                                OR (CheckOutDate BETWEEN @CheckIn AND @CheckOut)
                                OR (@CheckIn BETWEEN CheckInDate AND CheckOutDate))";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PetID", petId);
                cmd.Parameters.AddWithValue("@CheckIn", checkIn);
                cmd.Parameters.AddWithValue("@CheckOut", checkOut);

                con.Open();
                int overlap = (int)cmd.ExecuteScalar();

                return overlap == 0;
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
                            <a class='dropdown-item'>Indoor</a>
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
