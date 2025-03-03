<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Animal_Hotel.source.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

    <script src="../js/common_src.js"></script>
    <link rel="stylesheet" href="../styles/login.css" />
</head>
<body>
     <div class="bg-decorations">
    </div>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="container">
                <div class="header">
                    <h1>Animal Hotel - Unknown</h1>
                    <p>Your pets' home away from home</p>
                </div>
                <div class="input-group">
                    <label for="TextBox1">
                    <i class="fas fa-user"></i>Username
                    </label>
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="input-field" placeholder="Enter your username"></asp:TextBox>
                </div>
                <div class="input-group">
                    <label for="txtPass">
                        <i class="fas fa-lock"></i> Password
                    </label>
                    <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="input-field" placeholder="Enter your password"></asp:TextBox>
                </div>
                
                <div class="show-password">
                    <input type="checkbox" id="showPassword" onchange="document.getElementById('txtPass').type=this.checked ? 'text' : 'password'" />
                    <label for="showPassword">Show Password</label>
                </div>
                <br />
                <div class="button-group">
                    <asp:Button ID="Button1" runat="server" Text="Login" OnClick="Button1_Click" CssClass="btn btn-login" />
                    <asp:Button ID="Button2" runat="server" Text="Sign Up" OnClick="Button2_Click" CssClass="btn btn-signup" />
                </div>

                <div class="pet-icons">
                    <a href="Cats.aspx" class="pet-icons"><i class="fas fa-cat"></i></a>
                    <a href="Dogs.aspx" class="pet-icons"><i class="fas fa-dog"></i></a>
                    <a href="Others.aspx" class="pet-icons"><i class="fas fa-paw"></i></a>
                    <a href="Fish.aspx" class="pet-icons"><i class="fas fa-fish"></i></a>
                    <a href="Birds.aspx" class="pet-icons"><i class="fas fa-dove"></i></a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>


