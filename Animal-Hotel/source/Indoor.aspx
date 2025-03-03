<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Indoor.aspx.cs" Inherits="Animal_Hotel.source.Indoor" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Unknown - Pet Hotel & Lounge</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

    <link href="https://fonts.googleapis.com/css2?family=Pacifico&family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <link rel="stylesheet" href="../styles/main_style.css" />

    <script src="../js/common_src.js"></script>

    <link rel="stylesheet" href="../styles/reservationHotel.css" />
</head>
<body>
    <div class="bg-decorations">
    </div>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header class="header">
            <div class="nav-container">
                <nav class="nav-menu">
                    <div class='nav-item'>
                        <a href='#' class='nav-link'>Pets</a>
                        <div class='dropdown'>
                            <div class='dropdown-item has-submenu'>Catalogue
                            <div class='sub-dropdown'>
                                <a href='Cats.aspx' class='dropdown-item'>Cats</a>
                                <a href='Dogs.aspx' class='dropdown-item'>Dogs</a>
                                <a href='Birds.aspx' class='dropdown-item'>Birds</a>
                                <a href='Fish.aspx' class='dropdown-item'>Fish</a>
                                <a href='Others.aspx' class='dropdown-item'>Others</a>
                            </div>
                            </div>
                            <a href='Guests.aspx' class='dropdown-item'>Guests</a>
                            <a href='FluffsOfTheMonth.aspx' class='dropdown-item'>Fluffs of the Month</a>
                        </div>
                    </div>

                    <div class='nav-item'>
                        <a href='#' class='nav-link'>Services</a>
                        <div class='dropdown'>
                            <div class='dropdown-item has-submenu'>Hotel
                            <div class='sub-dropdown'>
                                <a class='dropdown-item'>Indoor</a>
                                <a href='Outdoor.aspx' class='dropdown-item'>Outdoor</a>
                                </div>
                            </div>
                            <div class='dropdown-item has-submenu'>Lounge
                            <div class='sub-dropdown'>
                                <a href='Spa.aspx' class='dropdown-item'>Spa</a>
                                <a href='Clinic.aspx' class='dropdown-item'>Clinic</a>
                                <a href='PlayZone.aspx' class='dropdown-item'>Play Zone</a>
                            </div>
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
                    </div>
                </nav>
                
                <div class="user-section">
                    <div class="social-icons">
                        <a href="#" class="social-icon"><i class="fab fa-facebook"></i></a>
                        <a href="#" class="social-icon"><i class="fab fa-instagram"></i></a>
                        <a href="#" class="social-icon"><i class="fab fa-twitter"></i></a>
                    </div>
                    <asp:Panel ID="pnlLoggedIn" runat="server" Visible="false">
                        <span class="user-name">
                            <asp:Label ID="lblUsername" runat="server"></asp:Label>
                        </span>
                        <asp:Button ID="btnLogout" runat="server" Text="Logout" 
                            CssClass="logout-btn" OnClick="btnLogout_Click" />
                    </asp:Panel>
                    <asp:Panel ID="pnlLoggedOut" runat="server">
                        <a href="Login.aspx" class="nav-link">Login</a>
                        <a href="SignUp.aspx" class="nav-link">Register</a>
                    </asp:Panel>
                </div>
            </div>
        </header>
        <div class="content">
            <div class="welcome-section">
                <h1>Indoor Pet Accommodation</h1>
                <p class="tagline">Luxury, Comfort, and Care for Your Indoor Companions</p>
            </div>
            
            <div class="features-container">
                <div class="feature-section">
                    <img src="images/luxury-suite.jpg" alt="Luxury Pet Suite" class="feature-image">
                    <div class="feature-content">
                        <h2>Luxury Pet Suites</h2>
                        <p>Our climate-controlled suites offer the perfect environment for your indoor pets. Each room is equipped with comfortable bedding, personal space, and monitoring systems.</p>
                    </div>
                </div>
            
                <div class="feature-section reverse">
                    <img src="images/playroom.jpg" alt="Indoor Play Area" class="feature-image">
                    <div class="feature-content">
                        <h2>Indoor Play Areas</h2>
                        <p>Specially designed play zones where your pets can exercise and socialize under professional supervision. Features climbing structures, toys, and rest areas.</p>
                    </div>
                </div>
            
                <div class="amenities-grid">
                    <div class="amenity-item">
                        <img src="images/feeding.jpg" alt="Premium Feeding" class="amenity-image">
                        <h3>Premium Feeding</h3>
                        <p>Customized meal plans and feeding schedules</p>
                    </div>
                    <div class="amenity-item">
                        <img src="images/monitoring.jpg" alt="24/7 Monitoring" class="amenity-image">
                        <h3>24/7 Monitoring</h3>
                        <p>Round-the-clock care and surveillance</p>
                    </div>
                    <div class="amenity-item">
                        <img src="images/grooming.jpg" alt="Grooming Services" class="amenity-image">
                        <h3>Grooming Services</h3>
                        <p>Regular grooming and maintenance</p>
                    </div>
                </div>
            
                <div class="pricing-overview">
                    <h2>What's Included</h2>
                    <ul class="benefits-list">
                        <li><i class="fas fa-check"></i> Climate-controlled accommodation</li>
                        <li><i class="fas fa-check"></i> Daily health check-ups</li>
                        <li><i class="fas fa-check"></i> Premium food and fresh water</li>
                        <li><i class="fas fa-check"></i> Regular exercise sessions</li>
                        <li><i class="fas fa-check"></i> Professional pet care staff</li>
                        <li><i class="fas fa-check"></i> Photo updates via email</li>
                    </ul>
                </div>
            </div>
            <div class="reservation-container">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <h2>Indoor Pet Reservation</h2>
                        <div class="reservation-options">
                            <asp:RadioButtonList ID="rblReservationType" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="rblReservationType_SelectedIndexChanged">
                                <asp:ListItem Text="Manual Entry" Value="manual" />
                                <asp:ListItem Text="Select from My Pets" Value="existing" />
                            </asp:RadioButtonList>
                        </div>
                
                        <asp:Panel ID="pnlManualEntry" runat="server" Visible="false" CssClass="reservation-form">
                            <div class="form-group">
                                <label for="ddlPetType">Pet Type:</label>
                                <asp:DropDownList ID="ddlPetType" runat="server" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlPetType_SelectedIndexChanged">
                                    <asp:ListItem Text="Select Type" Value="" />
                                    <asp:ListItem Text="Cat" Value="Cat" />
                                    <asp:ListItem Text="Dog" Value="Dog" />
                                    <asp:ListItem Text="Bird" Value="Bird" />
                                    <asp:ListItem Text="Fish" Value="Fish" />
                                    <asp:ListItem Text="Other" Value="Other" />
                                </asp:DropDownList>
                            </div>
                
                            <asp:Panel ID="pnlOtherType" runat="server" Visible="false" CssClass="form-group">
                                <label for="ddlOtherType">Specify Type:</label>
                                <asp:DropDownList ID="ddlOtherType" runat="server" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlOtherType_SelectedIndexChanged">
                                    <asp:ListItem Text="Select Category" Value="" />
                                    <asp:ListItem Text="Mammal" Value="Mammal" />
                                    <asp:ListItem Text="Reptile" Value="Reptile" />
                                    <asp:ListItem Text="Arachnid" Value="Arachnid" />
                                    <asp:ListItem Text="Amphibian" Value="Amphibian" />
                                </asp:DropDownList>
                            </asp:Panel>
                
                            <div class="form-group" id="breedSection" runat="server">
                                <label for="txtBreed">Breed:</label>
                                <asp:TextBox ID="txtBreed" runat="server" CssClass="form-control" list="breedList"></asp:TextBox>
                                <datalist id="breedList" runat="server">
                                </datalist>
                            </div>
                
                            <div class="form-group">
                                <label for="txtPetName">Pet Name:</label>
                                <asp:TextBox ID="txtPetName" runat="server"></asp:TextBox>
                            </div>
                
                            <div class="form-group">
                                <label for="txtAge">Age:</label>
                                <asp:TextBox ID="txtAge" runat="server" Type="number"></asp:TextBox>
                            </div>
                
                            <div class="form-group">
                                <label for="ddlGender">Gender:</label>
                                <asp:DropDownList ID="ddlGender" runat="server">
                                    <asp:ListItem Text="Select Gender" Value="" />
                                    <asp:ListItem Text="Male" Value="Male" />
                                    <asp:ListItem Text="Female" Value="Female" />
                                </asp:DropDownList>
                            </div>
                
                            <div class="form-group">
                                <label for="txtWeight">Weight (kg):</label>
                                <asp:TextBox ID="txtWeight" runat="server" Type="number" Step="0.01"></asp:TextBox>
                            </div>
                        </asp:Panel>
                
                        <asp:Panel ID="pnlExistingPets" runat="server" Visible="false" CssClass="existing-pets">
                            <asp:GridView ID="gvPets" runat="server" AutoGenerateColumns="false" 
                                OnRowCommand="gvPets_RowCommand" CssClass="pet-grid">
                                <Columns>
                                    <asp:BoundField DataField="PetName" HeaderText="Name" />
                                    <asp:BoundField DataField="PetType" HeaderText="Type" />
                                    <asp:BoundField DataField="PetBreed" HeaderText="Breed" />
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" 
                                                CommandName="SelectPet" 
                                                CommandArgument='<%# Eval("PetID") %>'
                                                CssClass="select-btn" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                
                        <div class="form-group">
                            <label for="txtAdditionalInfo">Additional Information:</label>
                            <asp:TextBox ID="txtAdditionalInfo" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        </div>

                        <div class="form-group date-container">
                            <div class="date-input">
                                <label for="txtCheckIn">Check-in Date:</label>
                                <asp:TextBox ID="txtCheckIn" runat="server" Type="date" CssClass="date-picker" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <div class="date-input">
                                <label for="txtCheckOut">Check-out Date:</label>
                                <asp:TextBox ID="txtCheckOut" runat="server" Type="date" CssClass="date-picker" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                
                        <div class="form-actions">
                            <asp:Button ID="btnSubmit" runat="server" Text="Make Reservation" 
                                OnClick="btnSubmit_Click" CssClass="submit-btn" />
                        </div>
                
                        <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <footer class="footer">
            <div class="footer-container">
                <div class="hotel-emblem">Unknown</div>
                <div class="language-currency">
                    <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged">
                        <asp:ListItem Text="English" Value="en" />
                        <asp:ListItem Text="German" Value="de" />
                        <asp:ListItem Text="Dutch" Value="nl" />
                        <asp:ListItem Text="Italian" Value="it" />
                        <asp:ListItem Text="Romanian" Value="ro" />
                    </asp:DropDownList>
                    
                    <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                        <asp:ListItem Text="EUR" Value="EUR" />
                        <asp:ListItem Text="USD" Value="USD" />
                        <asp:ListItem Text="CAD" Value="CAD" />
                        <asp:ListItem Text="CHF" Value="CHF" />
                        <asp:ListItem Text="GBP" Value="GBP" />
                        <asp:ListItem Text="RON" Value="RON" />
                        <asp:ListItem Text="AUD" Value="AUD" />
                    </asp:DropDownList>
                </div>
                
                <div class="pet-icons">
                    <a href="Cats.aspx" class="pet-icons"><i class="fas fa-cat"></i></a>
                    <a href="Dogs.aspx" class="pet-icons"><i class="fas fa-dog"></i></a>
                    <a href="Others.aspx" class="pet-icons"><i class="fas fa-paw"></i></a>
                    <a href="Fish.aspx" class="pet-icons"><i class="fas fa-fish"></i></a>
                    <a href="Birds.aspx" class="pet-icons"><i class="fas fa-dove"></i></a>
                </div>
            </div>
        </footer>
    </form>

    <script src="../js/reservationHotel.js"></script>
</body>
</html>