using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

namespace Animal_Hotel.source
{
    public partial class Cats : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;
        private LoginRemember _loginManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            if (!IsPostBack)
            {
                LoadCatBreeds();
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

        private List<CatBreed> GetCatBreeds()
        {
            List<CatBreed> breeds = new List<CatBreed>();
            
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM CatBreeds", con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return GetDefaultCatBreeds();
                            }

                            while (reader.Read())
                            {
                                breeds.Add(new CatBreed
                                {
                                    BreedID = reader.GetInt32(reader.GetOrdinal("BreedID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    ShortDescription = reader.GetString(reader.GetOrdinal("ShortDescription")),
                                    LongDescription = reader.GetString(reader.GetOrdinal("LongDescription")),
                                    ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                    Emoji = reader.GetString(reader.GetOrdinal("Emoji")),
                                    VisitorCount = reader.GetInt32(reader.GetOrdinal("VisitorCount")),
                                    Origin = reader.GetString(reader.GetOrdinal("Origin")),
                                    LifeSpan = reader.GetString(reader.GetOrdinal("LifeSpan")),
                                    Weight = reader.GetString(reader.GetOrdinal("Weight")),
                                    Temperament = reader.GetString(reader.GetOrdinal("Temperament"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return GetDefaultCatBreeds();
            }

            return breeds;
        }

        private List<CatBreed> GetDefaultCatBreeds()
        {
            return new List<CatBreed>
            {
                new CatBreed
                {
                    BreedID = 1,
                    Name = "Scottish Fold",
                    ShortDescription = "Known for their distinctive folded ears and sweet expression",
                    LongDescription = "Scottish Fold cats are immediately recognizable by their unique folded ears, which give them an owl-like appearance. These medium-sized cats are known for their sweet personalities and adaptability. They're excellent companion cats who enjoy being around their humans but aren't typically overly demanding.",
                    ImageUrl = "../img/scottish_fold.jpg",
                    Emoji = "🐱",
                    VisitorCount = 120,
                    Origin = "Scotland",
                    LifeSpan = "11-14 years",
                    Weight = "6-13 pounds",
                    Temperament = "Sweet, adaptable, playful, affectionate",
                    GroomingNeeds = "Moderate",
                    HealthConsiderations = "Osteochondrodysplasia, which affects cartilage and bone development",
                    IndoorOutdoor = "Indoor"
                },
                new CatBreed
                {
                    BreedID = 2,
                    Name = "Persian",
                    ShortDescription = "Luxurious long-haired cats with sweet personalities",
                    LongDescription = "Persian cats are the epitome of luxury in the feline world, known for their long, flowing coats and dignified demeanor. These cats are perfect for those seeking a calm, gentle companion who enjoys a relaxed indoor lifestyle.",
                    ImageUrl = "../img/persian.jpg",
                    Emoji = "😺",
                    VisitorCount = 150,
                    Origin = "Persia (Iran)",
                    LifeSpan = "12-17 years",
                    Weight = "7-12 pounds",
                    Temperament = "Gentle, quiet, dignified",
                    IndoorOutdoor = "Indoor"
                },
                new CatBreed
                {
                    BreedID = 3,
                    Name = "Maine Coon",
                    ShortDescription = "The sweet and gentle giants of the cat world",
                    LongDescription = "Maine Coons are one of the largest domestic cat breeds, known for their impressive size and luxurious coats. Despite their size, they're often referred to as 'gentle giants' due to their sweet and friendly nature.",
                    ImageUrl = "../img/maine_coon.jpg",
                    Emoji = "😸",
                    VisitorCount = 180,
                    Origin = "USA",
                    LifeSpan = "10-13 years",
                    Weight = "8-18 pounds",
                    Temperament = "Sweet, adaptable, playful, affectionate",
                    IndoorOutdoor = "Indoor"
                },
                new CatBreed
                {
                    BreedID = 4,
                    Name = "Siamese",
                    ShortDescription = "Vocal and intelligent cats with striking features",
                    LongDescription = "Siamese cats are one of the most distinctive and recognizable breeds, known for their color points, blue eyes, and vocal nature. They're highly intelligent and form strong bonds with their humans.",
                    ImageUrl = "../img/siamese.jpg",
                    Emoji = "😽",
                    VisitorCount = 140,
                    Origin = "Thailand",
                    LifeSpan = "12-15 years",
                    Weight = "7-10 pounds",
                    Temperament = "Sweet, adaptable, playful, affectionate",
                    IndoorOutdoor = "Indoor"
                }
            };
        }

        public void IncrementVisitorCount(int breedId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UPDATE CatBreeds SET VisitorCount = VisitorCount + 1 WHERE BreedID = @BreedID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BreedID", breedId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void NewVisitor_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["breed"] != null)
            {
                string breedName = Request.QueryString["breed"];
                CatBreed selectedBreed = GetCatBreeds().FirstOrDefault(b => b.Name == breedName);
                if (selectedBreed != null)
                {
                    IncrementVisitorCount(selectedBreed.BreedID);

                    string redirectPage = selectedBreed.IndoorOutdoor.ToLower() == "Indoor" ? "Indoor.aspx" : "Outdoor.aspx";
                    string queryString = $"{redirectPage}?";
                    queryString += "autoFill=true";
                    queryString += $"&breedName={HttpUtility.UrlEncode(selectedBreed.Name)}";
                    queryString += "&petType=Cat";
                    queryString += "&manualEntry=true";

                    Response.Redirect(queryString);
                }
            }
        }

        private void LoadCatBreeds()
        {
            var breeds = GetCatBreeds();
            var html = new StringBuilder();

            foreach (var breed in breeds)
            {
                html.Append($@"
                    <div class='breed-card'>
                        <img src='{breed.ImageUrl}' alt='{breed.Name}' class='breed-image'>
                        <div class='breed-actions'>
                            <button class='breed-action-btn' onclick='changeEmoji(event, ""{breed.Emoji}"")'>
                                <i class='fas fa-sync'></i>
                            </button>
                            <button class='breed-action-btn' onclick='addToDashboard(event, ""{breed.Name}"")'>
                                <i class='fas fa-plus'></i>
                            </button>
                        </div>
                        <div class='breed-info'>
                            <h3 class='breed-name'>{breed.Name}</h3>
                            <p class='breed-description'>{breed.ShortDescription}</p>
                            <button class='toggle-details' onclick='toggleDetails(event)'>
                                <i class='fas fa-chevron-down'></i>
                            </button>
                            <div class='breed-details'>
                                <p>{breed.LongDescription}</p>
                            </div>
                            <div class='visitor-section'>
                                <span class='visitor-count'>{breed.VisitorCount} visitors</span>
                                <a href='{breed.IndoorOutdoor}.aspx?breed={breed.Name}' class='new-visitor-btn'>New Visitor</a>
                            </div>
                        </div>
                    </div>");
            }

            breedContainer.InnerHtml = html.ToString();
        }
    }

    public class CatBreed
    {
        public int BreedID { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageUrl { get; set; }
        public string Emoji { get; set; }
        public int VisitorCount { get; set; }
        public string Origin { get; set; }
        public string LifeSpan { get; set; }
        public string Weight { get; set; }
        public string Temperament { get; set; }
        public string GroomingNeeds { get; set; }
        public string HealthConsiderations { get; set; }
        public bool GoodWithChildren { get; set; }
        public bool GoodWithOtherPets { get; set; }
        public string IndoorOutdoor { get; set; }
        public string ActivityLevel { get; set; }
        public string NoiseLevel { get; set; }
        public string Intelligence { get; set; }
        public DateTime DateAdded { get; set; }
        }
}