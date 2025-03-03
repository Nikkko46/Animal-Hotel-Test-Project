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
    public partial class Others : System.Web.UI.Page
    {
        private LoginRemember _loginManager;
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            if (!IsPostBack)
            {
                LoadOthersBreeds();
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
                            <a class='dropdown-item'>Others</a>
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

        private List<OthersBreed> GetOthersBreeds()
        {
            List<OthersBreed> breeds = new List<OthersBreed>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM OthersBreeds", con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return GetDefaultOthersBreeds();
                            }

                            while (reader.Read())
                            {
                                breeds.Add(new OthersBreed
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
                return GetDefaultOthersBreeds();
            }

            return breeds;
        }

        private List<OthersBreed> GetDefaultOthersBreeds()
        {
            return new List<OthersBreed>
            {
                new OthersBreed
                {
                    BreedID = 1,
                    Name = "Hedgehog",
                    Type = "Mammals",
                    ShortDescription = "A small, nocturnal mammal with a spiky coat, perfect for enthusiasts who love quirky pets",
                    LongDescription = "Hedgehogs are solitary, nocturnal mammals who are renowned for their spiky coats and adorable demeanor. These insectivores require a cozy and quiet environment to thrive. They're low-maintenance, but need specific care, such as temperature regulation and proper diet, to ensure their happiness and health.",
                    ImageUrl = "../img/hedgehog.jpg",
                    Emoji = "🦔",
                    VisitorCount = 20,
                    Origin = "Widespread",
                    LifeSpan = "2-5 years",
                    Size = "20-30 cm",
                    Weight = "0.6-1.2 kg",
                    Temperament = "Sweet, adaptable, playful, affectionate",
                    HealthConsiderations = "Osteochondrodysplasia, which affects cartilage and bone development",
                    IndoorOutdoor = "Indoor"
                },
                new OthersBreed
                {
                    BreedID = 2,
                    Name = "Leopard Gecko",
                    Type = "Reptiles",
                    ShortDescription = "A gentle, colorful reptile, known for its unique spots and friendly nature",
                    LongDescription = "Leopard Geckos are small, easy-to-care-for reptiles with a calm temperament. Their vibrant patterns and spots make them a favorite among reptile enthusiasts. These nocturnal creatures are docile and thrive in a well-regulated terrarium with proper heat and humidity levels. They're a great choice for both beginners and experienced reptile keepers.",
                    ImageUrl = "../img/leopard_gecko.jpg",
                    Emoji = "🦎",
                    VisitorCount = 15,
                    Origin = "Deserts from the Middle East to the Northwestern deserts of India",
                    LifeSpan = "10-15 years",
                    Size = "20-28 cm",
                    Weight = "60-80 grams",
                    Temperament = "Gentle, quiet, dignified",
                    IndoorOutdoor = "Indoor"
                },
                new OthersBreed
                {
                    BreedID = 3,
                    Name = "Tarantula",
                    Type = "Arachnids",
                    ShortDescription = "A fascinating arachnid with a calm temperament, ideal for exotic pet lovers",
                    LongDescription = "Tarantulas are large, hairy spiders often kept as exotic pets. Despite their intimidating appearance, they are generally docile and low-maintenance. Tarantulas require a secure enclosure, proper substrate and occasional live prey. They're a captivating choice for those interested in unique, quiet companions.",
                    ImageUrl = "../img/tarantula.jpg",
                    Emoji = "🕷️",
                    VisitorCount = 4,
                    Origin = "From the ancient Gondwana supercontinent",
                    LifeSpan = "10-15 years",
                    Size = "10-13 cm",
                    Weight = "70-85 grams",
                    Temperament = "Gentle, quiet, dignified",
                    IndoorOutdoor = "Indoor"
                },
                new OthersBreed
                {
                    BreedID = 4,
                    Name = "Axolotl",
                    Type = "Amphibian",
                    ShortDescription = "A unique aquatic amphibian known for its feathery gills and perpetual smile",
                    LongDescription = "Axolotls are fascinating amphibians native to Mexico, often referred to as 'walking fish' (though they are not fish). They are fully aquatic and thrive in cool, clean water tanks. Known for their feathery external gills and adorable appearance, axolotls are low-maintenance but require pristine water conditions and a proper diet. They're perfect for enthusiastsseeking a truly one-of-a-kind pet.",
                    ImageUrl = "../img/axolotl.jpg",
                    Emoji = "🦎",
                    VisitorCount = 2,
                    Origin = "Mexico",
                    LifeSpan = "6-15 years",
                    Size = "20-30 cm",
                    Weight = "150-300 grams",
                    Temperament = "Vocal, energetic, dignified",
                    IndoorOutdoor = "Indoor"
                },
                new OthersBreed
                {
                    BreedID = 5,
                    Name = "Hamster",
                    Type = "Mammals",
                    ShortDescription = "A small, active and playful rodent that's perfect for first-time pet owners",
                    LongDescription = "Hamsters are tiny, nocturnal rodents loved for their cute appearance and energetoc personality. They're easy to care for, requiring a cage with exercise wheels, tunnels and plenty of bedding for burrowing. Hamsters are solitary animals that enjoy exploration and occasional human interaction. With proper care and attention, they make excellent companions for kids and adults alike.",
                    ImageUrl = "../img/hamster.jpg",
                    Emoji = "🐹",
                    VisitorCount = 60,
                    Origin = "Southeast Europe, Middle East and Asia",
                    LifeSpan = "2-3 years",
                    Size = "5-10 cm",
                    Weight = "20-40 grams",
                    Temperament = "Vocal, energetic, dignified",
                    IndoorOutdoor = "Indoor"
                },
                new OthersBreed
                {
                    BreedID = 6,
                    Name = "Turtle",
                    Type = "Reptiles",
                    ShortDescription = "A calm, low-maintenance reptile with a long lifespan and unique charm",
                    LongDescription = "Turtles are peaceful reptiles known for their distinctive shells and slow movements. They can be aquatic, semi-aquatic or terrestrial, depending on the species. These hardy creatures require an appropriate enclosure with access to water, basking areas and a proper diet of greens or protein. Turtles are great choice for pet owners looking for a calm and long-lasting companion.",
                    ImageUrl = "../img/turtle.jpg",
                    Emoji = "🐢",
                    VisitorCount = 20,
                    Origin = "Widespread across the Oceans of the world",
                    LifeSpan = "Varying, but generally over 20 years",
                    Size = "Varying among species",
                    Weight = "Varying among species",
                    Temperament = "Vocal, energetic, dignified",
                    IndoorOutdoor = "Outdoor"
                }
            };
        }

        public void IncrementVisitorCount(int breedId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UPDATE OthersBreeds SET VisitorCount = VisitorCount + 1 WHERE BreedID = @BreedID";
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
                OthersBreed selectedBreed = GetOthersBreeds().FirstOrDefault(b => b.Name == breedName);
                if (selectedBreed != null)
                {
                    IncrementVisitorCount(selectedBreed.BreedID);

                    string redirectPage = selectedBreed.IndoorOutdoor.ToLower() == "indoor" ? "Indoor.aspx" : "Outdoor.aspx";
                    string queryString = $"{redirectPage}?";
                    queryString += "autoFill=true";
                    queryString += $"&breedName={HttpUtility.UrlEncode(selectedBreed.Name)}";
                    queryString += "&petType=Other";
                    queryString += $"&breedType={HttpUtility.UrlEncode(selectedBreed.Type)}";
                    queryString += "&manualEntry=true";

                    Response.Redirect(queryString);
                }
            }
        }

        private void LoadOthersBreeds()
        {
            var breeds = GetOthersBreeds();
            var html = new StringBuilder();

            html.Append(@"
                <div class='type-selector'>
                    <select id='typeSelect' class='form-control'>
                        <option value=''>Select Type</option>
                    <option value=''>Select Type</option>
                    <option value='Mammals'>Mammals</option>
                    <option value='Reptiles'>Reptiles</option>
                    <option value='Arachnids'>Arachnids</option>
                    <option value='Amphibians'>Amphibians</option>
                    </select>
                    <button onclick='scrollToType()'><i class='fas fa-arrow-right'></i></button>
                </div>");

            var typeOrder = new[] { "Mammals", "Reptiles", "Arachnids", "Amphibians" };
            var breedsByType = breeds
                .GroupBy(b => b.Type.ToLower())
                .OrderBy(g => Array.IndexOf(typeOrder, g.Key));

            foreach (var typeGroup in breedsByType)
            {
                html.Append($@"
                    <div id='{typeGroup.Key}-section' class='size-section'>
                        <h2 class='size-title'>{char.ToUpper(typeGroup.Key[0]) + typeGroup.Key.Substring(1)}</h2>
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
                                    <a href='{breed.IndoorOutdoor}.aspx?breed={breed.Name}' class='new-visitor-btn'>New Visitor</a>
                                </div>
                            </div>
                        </div>");
                }
                html.Append("</div></div>");
            }
            breedContainer.InnerHtml = html.ToString();
        }
    }

    public class OthersBreed
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
        public bool GoodWithChildren { get; set; }
        public bool GoodWithOtherPets { get; set; }
        public string IndoorOutdoor { get; set; }
        public string ActivityLevel { get; set; }
        public string NoiseLevel { get; set; }
        public string Intelligence { get; set; }
        public DateTime DateAdded { get; set; }
    }
}