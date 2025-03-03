<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Animal_Hotel.source.SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

    <script src="../js/common_src.js"></script>
    
    <link rel="stylesheet" href="../styles/signup.css" />
</head>
<body>
    <div class="bg-decorations">
    </div>

    <form id="form1" runat="server">
        <div class="signup-container">
            <div class="container">
                <div class="header">
                    <h1>Register an account and join our community!</h1>
                    <p>Let us know who we are fluffing with!</p>
                </div>
                <table align="center">
                    <tr>
                        <td>
                            <label for="txtName">
                                <i class="fa-solid fa-address-card"></i>Full Name
                            </label>
                        </td>
                        <td class="input-group">
                            <asp:TextBox ID="txtName" runat="server" placeholder="Enter your Full Name."></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                             <label for="txtAdd">
                                 <i class="fa-solid fa-map-pin"></i>Address
                             </label>
                        </td>
                        <td class="input-group">
                            <asp:TextBox ID="txtAdd" runat="server" placeholder="Enter your Address."></asp:TextBox></td>
                    </tr>
                    <tr>
                     <td>
                         <label for="RadioButton1 RadioButton2">
                            <i class="fa-solid fa-venus-mars"></i>Gender
                        </label>
                     </td>
                     <td>
                        <div class="genders">
                            <div class="gender-option">
                                <asp:RadioButton GroupName="user" ID="RadioButton1" runat="server" Text="Male" OnCheckedChanged="RadioButton1_CheckedChanged" />
                                <i class="fas fa-mars"></i> 
                            </div> <br />
                            <div class="gender-option">
                                <asp:RadioButton GroupName="user" ID="RadioButton2" runat="server" Text="Female" OnCheckedChanged="RadioButton2_CheckedChanged" />
                                <i class="fas fa-venus"></i> 
                            </div> <br /> 
                        </div>
                     </td>
                    </tr>
                    <tr>
                     <td>
                          <label for="txtPhone">
                              <i class="fa-solid fa-phone"></i>Phone
                          </label>
                     </td>
                     <td class="input-group">
                         <asp:TextBox ID="txtPhone" runat="server" placeholder="ex: +40 072 874 3214"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                             <label for="txtEmail">
                                <i class="fa-solid fa-at"></i>Email
                            </label>
                        </td>
                        <td class="input-group">
                            <asp:TextBox ID="txtEmail" runat="server" placeholder="example@example.com"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                             <label for="txtUser">
                                 <i class="fa-solid fa-user"></i>Username
                             </label>
                        </td>
                        <td class="input-group">
                            <asp:TextBox ID="txtUser" runat="server" placeholder="Enter a desired Username."></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                             <label for="txtPass">
                                 <i class="fa-solid fa-lock"></i>Password
                             </label>
                        </td>
                        <td class="input-group">
                            <asp:TextBox ID="txtPass" runat="server" placeholder="*****" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                             <label for="txtConfPass">
                                 <i class="fa-solid fa-lock"></i>Confirm Password
                             </label>
                        </td>
                        <td class="input-group">
                            <asp:TextBox ID="txtConfPass" runat="server" placeholder="Reenter your Password" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="right" class="reg-btn">
                            <asp:Button ID="btnReg" runat="server" Text="Sign Up" OnClick="btnReg_Click" CssClass="btn btn-signup" />
                        </td>
                    </tr>
                </table>

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
    <script type="text/javascript">
        function validatePasswords() {
            var password = document.getElementById('<%= txtPass.ClientID %>').value;
        var confirmPassword = document.getElementById('<%= txtConfPass.ClientID %>').value;

            if (password != confirmPassword) {
                alert("Passwords do not match!");
                return false;
            }
            return true;
        }
    </script>
</body>
</html>
