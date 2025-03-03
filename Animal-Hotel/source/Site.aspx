<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Site.aspx.cs" Inherits="Animal_Hotel.source.Site" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Unknown - Pet Hotel & Lounge</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

    <link href="https://fonts.googleapis.com/css2?family=Pacifico&family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    
    <script src="'https://api.openstreetmap.org/api/0.6/map?bbox=-0.489,-0.123,0.236,51.569'"></script>

    <link rel="stylesheet" href="../styles/main_style.css" />

    <script src="../js/common_src.js"></script>

    <style>
        .map-container {
            width: 100%;
            max-width: 225px;
            height: 100%;
            max-height: 225px;
            margin: 0 auto;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
    </style>
</head>
<body>
    <div class="bg-decorations">
    </div>

    <form id="form1" runat="server">
        <header class="header">
            <button class="menu-toggle" id="menuToggle">
                <i class="fas fa-bars"></i>
            </button>
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
                                <a href='Indoor.aspx' class='dropdown-item'>Indoor</a>
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
                    <div class="map-container" id="map"></div>
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
        <div class="content"></div>
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

    <script type="text/javascript">
        let map;
        let marker;
        const defaultLocation = { lat: not_set, lng: not_set };  //locatia hotelului, de editat!!!!!

        function initMap() {
            map = new OSM.maps.Map(document.getElementById('map'), {
                center: defaultLocation,
                zoom: 13,
                styles: [
                    {
                        "featureType": "all",
                        "elementType": "all",
                        "stylers": [
                            { "saturation": -100 },
                            { "lightness": 45 }
                        ]
                    }
                ]
            });

            marker = new OSM.maps.Marker({
                position: defaultLocation,
                map: map,
                title: 'Unknown Pet Hotel',
                icon: {
                    url: 'marker.png',
                    scaledSize: new OSM.maps.Size(32, 32)
                }
            });

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(
                    (position) => {
                        const userLocation = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        };

                        new OSM.maps.Marker({
                            position: userLocation,
                            map: map,
                            title: 'Your Location',
                            icon: {
                                path: OSM.maps.SymbolPath.CIRCLE,
                                scale: 7,
                                fillColor: '#4285F4',
                                fillOpacity: 1,
                                strokeWeight: 2,
                                strokeColor: '#ffffff'
                            }
                        });

                        const directionsService = new OSM.maps.DirectionsService();
                        const directionsRenderer = new OSM.maps.DirectionsRenderer({
                            map: map,
                            suppressMarkers: true
                        });

                        directionsService.route({
                            origin: userLocation,
                            destination: defaultLocation,
                            travelMode: OSM.maps.TravelMode.DRIVING
                        }, (response, status) => {
                            if (status === 'OK') {
                                directionsRenderer.setDirections(response);
                            }
                        });
                    },
                    () => {
                        console.log('Error: The Geolocation service failed.');
                    }
                );
            }
        }

        window.onload = initMap;
    </script>
</body>
</html>