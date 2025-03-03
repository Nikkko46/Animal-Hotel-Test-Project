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
    public partial class Fish : System.Web.UI.Page
    {
        private LoginRemember _loginManager;
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            if (!IsPostBack)
            {
                LoadFishBreeds();
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
                            <a class='dropdown-item'>Dogs</a>
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

        private List<FishBreed> GetFishBreeds()
        {
            List<FishBreed> breeds = new List<FishBreed>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM FishBreeds", con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return GetDefaultFishBreeds();
                            }

                            while (reader.Read())
                            {
                                breeds.Add(new FishBreed
                                {
                                    BreedID = reader.GetInt32(reader.GetOrdinal("BreedID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Type = reader.GetString(reader.GetOrdinal("Type")),
                                    ShortDescription = reader.GetString(reader.GetOrdinal("ShortDescription")),
                                    LongDescription = reader.GetString(reader.GetOrdinal("LongDescription")),
                                    ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                    Emoji = reader.GetString(reader.GetOrdinal("Emoji")),
                                    VisitorCount = reader.GetInt32(reader.GetOrdinal("VisitorCount")),
                                    Origin = reader.GetString(reader.GetOrdinal("Origin")),
                                    LifeSpan = reader.GetString(reader.GetOrdinal("LifeSpan")),
                                    Size = reader.GetString(reader.GetOrdinal("Size")),
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
                return GetDefaultFishBreeds();
            }

            return breeds;
        }

        private List<FishBreed> GetDefaultFishBreeds()
        {
            return new List<FishBreed>
            {
                new FishBreed
                {
                    BreedID = 1,
                    Name = "Neon Tetra",
                    Type = "Freshwater",
                    ShortDescription = "A small, vibrant fish popular for its striking blue and red colors",
                    LongDescription = "The Neon Tetra is a peaceful and colorful freshwater fish known for its vivid blue and red stripes. These schooling fish thrive in groups and add a vibrant touch to any aquarium. Easy to care for, they prefer slightly acidic water with stable temperatures, making them ideal for beginners.",
                    ImageUrl = "../img/neon_tetra.jpg",
                    Emoji = "🐕",
                    VisitorCount = 80,
                    Origin = "Orinoco and Amazon rivers",
                    LifeSpan = "4-5 years",
                    Size = "1.5 inches",
                    Weight = "1 gram",
                    Temperament = "Sweet, adaptable, playful, affectionate",
                    HealthConsiderations = "Osteochondrodysplasia, which affects cartilage and bone development"
                },
                new FishBreed
                {
                    BreedID = 2,
                    Name = "Clownfish",
                    Type = "Saltwater",
                    ShortDescription = "A bright orange and white fish, famous for its playful personality and association with anemones",
                    LongDescription = "The Clownfish is a popular saltwater species, instantly recognizable by its orange body with white bands and black accents. These hardy fish are known for their symbiotic relationship with sea anemones. Clownfish are relatively easy to care for, requiring a stable saltwater environment and occasional marine-specific feeding.",
                    ImageUrl = "../img/clownfish.jpg",
                    Emoji = "🐕‍🦺",
                    VisitorCount = 100,
                    Origin = "Indo-Pacifico Region, Indian and Pacific Oceans",
                    LifeSpan = "8-12 years",
                    Size = "3-4 inches",
                    Weight = "50-100 grams",
                    Temperament = "Gentle, quiet, dignified"
                },
                new FishBreed
                {
                    BreedID = 3,
                    Name = "Green Spotted Puffer",
                    Type = "Brackish Water",
                    ShortDescription = "A unique fish with a rounded body and bright green spots, requiring a mix of fresh and saltwater",
                    LongDescription = "The Green Spotted Puffer is an eye-catching fish with bright green spots on a yellowish body. These curious and intelligent fish thrive in brackish water, requiring moderate salinity levels. While they are known for their playful behavior, Puffers can be territorial and require special diets, including hard-shelled food to maintain their teeth.",
                    ImageUrl = "../img/green_spotted_puffer.jpg",
                    Emoji = "🐶",
                    VisitorCount = 10,
                    Origin = "Southeastern Asian Waters",
                    LifeSpan = "10-20 years",
                    Size = "2-3 inches non-bloated, 6-7 inches bloated",
                    Weight = "around 100-200 grams non-bloated",
                    Temperament = "Gentle, quiet, dignified"
                },
                new FishBreed
                {
                    BreedID = 4,
                    Name = "Goldfish",
                    Type = "Coldwater",
                    ShortDescription = "A classic coldwater fish known for its bright color and peaceful demeanor",
                    LongDescription = "The Goldfish is a staple in aquariums and ponds, prized for its beautiful gold, orange and white variations. These hardy fish prefer cool, well-oxygenated water and are highly adaptable. Goldfish are social and thrive in groups, making them a popular choice for beginners and experienced fishkeepers alike.",
                    ImageUrl = "../img/goldfish.jpg",
                    Emoji = "🦮",
                    VisitorCount = 120,
                    Origin = "Originally in the cold waters of the China, Korea and Eurasia, currently widespread",
                    LifeSpan = "10-15 years",
                    Size = "5-10 inches",
                    Weight = "100-300 grams",
                    Temperament = "Vocal, energetic, dignified"
                },

                new FishBreed
                {
                    BreedID = 5,
                    Name = "Angelfish",
                    Type = "Tropical Water",
                    ShortDescription = "An elegant fish with long fins and striking patterns, perfect for a heated aquarium",
                    LongDescription = "The Angelfish is a graceful tropical species with elongated fins and stunning patterns. Known for its unique triangular shape, this freshwater fish thrives in warm, stable water conditions. Angelfish are semi-aggressive and prefer a tank with plenty of plants and open swimming space, making them a centerpiece in any aquarium.",
                    ImageUrl = "../img/angelfish.jpg",
                    Emoji = "🦮",
                    VisitorCount = 70,
                    Origin = "Lower Amazon region",
                    LifeSpan = "8-12 years",
                    Size = "4-6 inches in length and 6-8 inches height",
                    Weight = "25-75 grams",
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
                    string query = "UPDATE FishBreeds SET VisitorCount = VisitorCount + 1 WHERE BreedID = @BreedID";
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
                FishBreed selectedBreed = GetFishBreeds().FirstOrDefault(b => b.Name == breedName);
                if (selectedBreed != null)
                {
                    IncrementVisitorCount(selectedBreed.BreedID);

                    string redirectPage = "Indoor.aspx";
                    string queryString = $"{redirectPage}?";
                    queryString += "autoFill=true";
                    queryString += $"&breedName={HttpUtility.UrlEncode(selectedBreed.Name)}";
                    queryString += "&petType=Fish";
                    queryString += "&manualEntry=true";

                    Response.Redirect(queryString);
                }
            }
        }

        private void LoadFishBreeds()
        {
            var breeds = GetFishBreeds();
            var html = new StringBuilder();

            html.Append(@"
                <div class='type-selector'>
                    <select id='typeSelect' class='form-control'>
                        <option value=''>Select Type</option>
                    <option value='fwater'>Freshwater Fish</option>
                    <option value='swater'>Saltwater Fish</option>
                    <option value='bwater'>Brackish Water Fish</option>
                    <option value='cwater'>Coldwater Fish</option>
                    <option value='twater'>Tropical Water Fish</option>
                    </select>
                    <button onclick='scrollToType()'><i class='fas fa-arrow-right'></i></button>
                </div>");

            var typeOrder = new[] { "Freshwater", "Saltwater", "Brackish  Water", "Coldwater", "Tropical Water" };
            var breedsByType = breeds
                .GroupBy(b => b.Type.ToLower())
                .OrderBy(g => Array.IndexOf(typeOrder, g.Key));

            foreach (var typeGroup in breedsByType)
            {
                html.Append($@"
                    <div id='{typeGroup.Key}-section' class='size-section'>
                        <h2 class='size-title'>{char.ToUpper(typeGroup.Key[0]) + typeGroup.Key.Substring(1)} Fish</h2>
                        <div class='breeds-container'>");

                foreach (var breed in typeGroup)
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

    public class FishBreed
    {
        public int BreedID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageUrl { get; set; }
        public string Emoji { get; set; }
        public int VisitorCount { get; set; }
        public string Origin { get; set; }
        public string LifeSpan { get; set; }
        public string Size { get; set; }
        public string Weight { get; set; }
        public string Temperament { get; set; }
        public string HealthConsiderations { get; set; }
        public string ActivityLevel { get; set; }
        public string Intelligence { get; set; }
        public DateTime DateAdded { get; set; }
    }
}