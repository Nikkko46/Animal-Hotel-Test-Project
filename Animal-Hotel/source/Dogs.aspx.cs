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
    public partial class Dogs : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AnimalHotelDB"].ConnectionString;
        private LoginRemember _loginManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginManager = new LoginRemember(this);
            _loginManager.HandlePageLoad();

            if (!IsPostBack)
            {
                LoadDogBreeds();
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

        private List<DogBreed> GetDogBreeds()
        {
            List<DogBreed> breeds = new List<DogBreed>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM DogBreeds", con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return GetDefaultDogBreeds();
                            }

                            while (reader.Read())
                            {
                                breeds.Add(new DogBreed
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
                return GetDefaultDogBreeds();
            }

            return breeds;
        }

        private List<DogBreed> GetDefaultDogBreeds()
        {
            return new List<DogBreed>
            {
                new DogBreed
                {
                    BreedID = 1,
                    Name = "Rottweiler",
                    Size = "Large",
                    ShortDescription = "A powerful and loyal working breed, known for its strength and intelligence",
                    LongDescription = "The Rottweiler is a robust and confident working dog originally bred to herd livestock and pull carts. They are highly intelligent, loyal, and protective, making them excellent guard dogs and companions. With proper training and socialization, Rottweilers can be affectionate and gentle family pets, excelling in various roles requiring strength and endurance such as security and service.",
                    ImageUrl = "../img/rottweiler.jpg",
                    Emoji = "🐕",
                    VisitorCount = 70,
                    Origin = "Germany",
                    LifeSpan = "9-10 years",
                    Weight = "80-135 pounds",
                    Temperament = "Sweet, adaptable, playful, affectionate",
                    GroomingNeeds = "Low",
                    HealthConsiderations = "Osteochondrodysplasia, which affects cartilage and bone development",
                    IndoorOutdoor = "Indoor"
                },
                new DogBreed
                {
                    BreedID = 2,
                    Name = "German Shepherd",
                    Size = "Large",
                    ShortDescription = "A versatile and intelligent breed, widely admired for its loyalty and working abilities",
                    LongDescription = "The German Shepherd is a highly adaptable and courageous dog known for its unmatched verstility. Originally bred for herding sheep, this breed excles in roles such as police, military and search-and-rescue work. They are incredibly loyal, intelligent and trainable, making them excellent family pets and protectors when properly trained and socialized.",
                    ImageUrl = "../img/german_shepherd.jpg",
                    Emoji = "🐕‍🦺",
                    VisitorCount = 130,
                    Origin = "Germany",
                    LifeSpan = "9-13 years",
                    Weight = "50-90 pounds",
                    Temperament = "Gentle, quiet, dignified",
                    IndoorOutdoor = "Indoor"
                },
                new DogBreed
                {
                    BreedID = 3,
                    Name = "French Bulldog",
                    Size = "XSmall",
                    ShortDescription = "A playful and affectionate companion, perfect for small spaces and families",
                    LongDescription = "The French Bulldog is a charming and compact breed with a playful personality and a heart full of love. They are well-suited to apartment living due to their small size and minimal exercise needs. Known for their bat-like ears and friendly demeanor, French Bulldogs are excellent companions for individuals and families alike, thriving on human interaction and affection.",
                    ImageUrl = "../img/french_bulldog.jpg",
                    Emoji = "🐶",
                    VisitorCount = 100,
                    Origin = "France",
                    LifeSpan = "10-12 years",
                    Weight = "16-28 pounds",
                    Temperament = "Gentle, quiet, dignified",
                    IndoorOutdoor = "Indoor"
                },
                new DogBreed
                {
                    BreedID = 4,
                    Name = "Siberian Husky",
                    Size = "Medium",
                    ShortDescription = "An energetic and friendly breed, famed for its striking appearance and sledding history.",
                    LongDescription = "The Siberian Husky is an energetic and outgoing breed, renowned for its wolf-like appearance and exceptional endurance. Bred as sled dogs in harsh Arctic conditions, and with a heavy coat of fur to protect against the freezing temperatures, they possess a strong work ethic and a playful nature. Huskies are highly social, thriving in active households that can provide the mental and physical stimulations they need to stay happy and healthy.",
                    ImageUrl = "../img/husky.jpg",
                    Emoji = "🦮",
                    VisitorCount = 60,
                    Origin = "Siberia, Russia",
                    LifeSpan = "12-15 years",
                    Weight = "30-60 pounds",
                    Temperament = "Vocal, energetic, dignified",
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
                    string query = "UPDATE DogBreeds SET VisitorCount = VisitorCount + 1 WHERE BreedID = @BreedID";
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
                DogBreed selectedBreed = GetDogBreeds().FirstOrDefault(b => b.Name == breedName);
                if (selectedBreed != null)
                {
                    IncrementVisitorCount(selectedBreed.BreedID);

                    string redirectPage = selectedBreed.IndoorOutdoor.ToLower() == "Indoor" ? "Indoor.aspx" : "Outdoor.aspx";
                    string queryString = $"{redirectPage}?";
                    queryString += "autoFill=true";
                    queryString += $"&breedName={HttpUtility.UrlEncode(selectedBreed.Name)}";
                    queryString += "&petType=Dog";
                    queryString += "&manualEntry=true";

                    Response.Redirect(queryString);
                }
            }
        }

        private void LoadDogBreeds()
        {
            var breeds = GetDogBreeds();
            var html = new StringBuilder();

            html.Append(@"
                <div class='size-selector'>
                    <select id='sizeSelect' class='form-control'>
                        <option value=''>Select Size</option>
                        <option value='XSmall'>XSmall Dogs</option>
                        <option value='Small'>Small Dogs</option>
                        <option value='Medium'>Medium Dogs</option>
                        <option value='Large'>Large Dogs</option>
                        <option value='XLarge'>XLarge Dogs</option>
                    </select>
                    <button onclick='scrollToSize()'><i class='fas fa-arrow-right'></i></button>
                </div>");
                
            var sizeOrder = new[] { "xsmall", "small", "medium", "large", "xlarge" };
            var breedsBySize = breeds
                .GroupBy(b => b.Size.ToLower())
                .OrderBy(g => Array.IndexOf(sizeOrder, g.Key));

            foreach (var sizeGroup in breedsBySize)
            {
                html.Append($@"
                    <div id='{sizeGroup.Key}-section' class='size-section'>
                        <h2 class='size-title'>{char.ToUpper(sizeGroup.Key[0]) + sizeGroup.Key.Substring(1)} Dogs</h2>
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

    public class DogBreed
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
        public string IndoorOutdoor { get; set; }
        public string ActivityLevel { get; set; }
        public string NoiseLevel { get; set; }
        public string Intelligence { get; set; }
        public DateTime DateAdded { get; set; }
    }
}