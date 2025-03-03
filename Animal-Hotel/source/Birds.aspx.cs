using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Animal_Hotel.source
{
    public partial class Birds : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;
        private LoginRemember _loginManager;

        protected void Page_Load(object sender, EventArgs e)
        {   
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            if (!IsPostBack)
            {
                LoadBirdBreeds();
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
                            <a class='dropdown-item'>Birds</a>
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

        private List<BirdBreed> GetBirdBreeds()
        {
            List<BirdBreed> breeds = new List<BirdBreed>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM BirdBreeds", con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return GetDefaultBirdBreeds();
                            }

                            while (reader.Read())
                            {
                                breeds.Add(new BirdBreed
                                {
                                    BreedID = reader.GetInt32(reader.GetOrdinal("BreedID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Size = reader.GetString(reader.GetOrdinal("Size")),
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
                return GetDefaultBirdBreeds();
            }

            return breeds;
        }

        private List<BirdBreed> GetDefaultBirdBreeds()
        {
            return new List<BirdBreed>
            {
                new BirdBreed
                {
                    BreedID = 1,
                    Name = "Parakeet",
                    Size = "Small",
                    ShortDescription = "Friendly and colorful, easy-to-care-for birds that make great companions",
                    LongDescription = "Parakeets, also known as budgies, are cheerful, vibrant birds known for their playful personalities and delightful chirping. They thrive in social environments and enjoy interacting with humans or ither birds, These low-maintenance companions are perfect for first-time bird owners and love to explore their surroundings with curiosity.",
                    ImageUrl = "../img/parakeet.jpg",
                    Emoji = "🐤",
                    VisitorCount = 30,
                    Origin = "Australia and India",
                    LifeSpan = "5-8 years",
                    Weight = "25-35 grams",
                    Temperament = "Sweet, adaptable, playful, affectionate",
                    GroomingNeeds = "Low",
                    HealthConsiderations = "Osteochondrodysplasia, which affects cartilage and bone development"
                },
                new BirdBreed
                {
                    BreedID = 2,
                    Name = "Conure",
                    Size = "Medium",
                    ShortDescription = "Playful and lively colorful birds known for their affectionate nature and entertaining antics.",
                    LongDescription = "Conures are medium-sized parrots that bring joy and energy wherever they go. These social birds are known for their charming personalities, vibrant feathers, and occasionally noisy squawks. They love to cuddle, play with toys and engage in interactive games, being perfect for bird-enthusiasts, making loyal and fun-loving companions.",
                    ImageUrl = "../img/conure.jpg",
                    Emoji = "🐦",
                    VisitorCount = 50,
                    Origin = "Central to South America",
                    LifeSpan = "10-25 years",
                    Weight = "70-200 grams",
                    Temperament = "Gentle, quiet, dignified"
                },
                new BirdBreed
                {
                    BreedID = 3,
                    Name = "Cockatoo",
                    Size = "Large",
                    ShortDescription = "Affectionate and intelligent large birds that bond deeply with their caretakers",
                    LongDescription = "Cockatoos are renowned for their affectionate nature and striking crests. Highly intelligent and social, these parrots thrive on interaction and attention. They love to mimic sounds, learn tricks and show off their playful personalities. However, their strong bonds with humans require time and commitment, making them ideal for dedicated bird lovers.",
                    ImageUrl = "../img/cockatoo.jpg",
                    Emoji = "🦜",
                    VisitorCount = 20,
                    Origin = "Southeast Asia and Oceania",
                    LifeSpan = "40-60 years",
                    Weight = "0.66-2.65 pounds",
                    Temperament = "Gentle, quiet, dignified"
                },
                new BirdBreed
                {
                    BreedID = 4,
                    Name = "Macaw Parrot",
                    Size = "Large",
                    ShortDescription = "Majestic and vibrant birds known for their intelligence and visually striking plummage",
                    LongDescription = "Macaws are the royalty of the parrot world, admired for their breathtaking colors and lively personalities. These intelligent parrots are capable of mimicking speech and performing tricks, making them engaging companions. With their playful nature, Macaws thrive in environments where they cam receive plenty of attention and mental stimulation, being ideal for experienced bird owners, making unforgettable lifelong friends.",
                    ImageUrl = "../img/macaw.jpg",
                    Emoji = "🦜",
                    VisitorCount = 15,
                    Origin = "Central and South Americas, (briefly) Mexico",
                    LifeSpan = "30-55 years",
                    Weight = "3-3.5 pounds",
                    Temperament = "Vocal, energetic, dignified"
                }
            };
        }

        public void IncrementVisitorCount(int breedId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UPDATE BirdBreeds SET VisitorCount = VisitorCount + 1 WHERE BreedID = @BreedID";
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
                BirdBreed selectedBreed = GetBirdBreeds().FirstOrDefault(b => b.Name == breedName);
                if (selectedBreed != null)
                {
                    IncrementVisitorCount(selectedBreed.BreedID);

                    string redirectPage = "Indoor.aspx";
                    string queryString = $"{redirectPage}?";
                    queryString += "autoFill=true";
                    queryString += $"&breedName={HttpUtility.UrlEncode(selectedBreed.Name)}";
                    queryString += "&petType=Bird";
                    queryString += "&manualEntry=true";

                    Response.Redirect(queryString);
                }
            }
        }

        private void LoadBirdBreeds()
        {
            var breeds = GetBirdBreeds();
            var html = new StringBuilder();

            html.Append(@"
                <div class='size-selector'>
                    <select id='sizeSelect' class='form-control'>
                        <option value=''>Select Size</option>
                        <option value='Small'>Small Birds</option>
                        <option value='Medium'>Medium Birds</option>
                        <option value='Large'>Large Birds</option>
                    </select>
                    <button onclick='scrollToSize()'><i class='fas fa-arrow-right'></i></button>
                </div>");

            var sizeOrder = new[] { "Small", "Medium", "Large" };
            var breedsBySize = breeds
                .GroupBy(b => b.Size.ToLower())
                .OrderBy(g => Array.IndexOf(sizeOrder, g.Key));

            foreach (var sizeGroup in breedsBySize)
            {
                html.Append($@"
                    <div id='{sizeGroup.Key}-section' class='size-section'>
                        <h2 class='size-title'>{char.ToUpper(sizeGroup.Key[0]) + sizeGroup.Key.Substring(1)} Birds</h2>
                        <div class='breeds-container'>");

                foreach (var breed in sizeGroup)
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
                                    <a href='Indoor.aspx?breed={breed.Name}' class='new-visitor-btn'>New Visitor</a>
                                </div>
                            </div>
                        </div>");
                }
                html.Append("</div></div>");
            }
            breedContainer.InnerHtml = html.ToString();
        }
    }

    public class BirdBreed
    {
        public int BreedID { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
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
        public string ActivityLevel { get; set; }
        public string NoiseLevel { get; set; }
        public string Intelligence { get; set; }
        public DateTime DateAdded { get; set; }
    }
}