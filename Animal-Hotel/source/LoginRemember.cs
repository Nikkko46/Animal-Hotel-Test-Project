using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Animal_Hotel.source
{
    public class LoginRemember
    {
        private Page _page;
        private Panel _pnlLoggedIn;
        private Panel _pnlLoggedOut;
        private Label _lblUsername;
        private DropDownList _ddlLanguage;
        private DropDownList _ddlCurrency;

        public LoginRemember(Page page)
        {
            _page = page;
            InitializeControls();
        }

        private void InitializeControls()
        {
            _pnlLoggedIn = _page.FindControl("pnlLoggedIn") as Panel;
            _pnlLoggedOut = _page.FindControl("pnlLoggedOut") as Panel;
            _lblUsername = _page.FindControl("lblUsername") as Label;
            _ddlLanguage = _page.FindControl("ddlLanguage") as DropDownList;
            _ddlCurrency = _page.FindControl("ddlCurrency") as DropDownList;
        }

        public void HandlePageLoad()
        {
            if (!_page.IsPostBack)
            {
                HandleAuthenticationState();
                LoadUserPreferences();
            }
        }

        public void HandleAuthenticationState()
        {
            if (_pnlLoggedIn != null && _pnlLoggedOut != null)
            {
                if (HttpContext.Current.Session["username"] != null)
                {
                    _pnlLoggedIn.Visible = true;
                    _pnlLoggedOut.Visible = false;
                    if (_lblUsername != null)
                    {
                        _lblUsername.Text = HttpContext.Current.Session["username"].ToString();
                    }
                }
                else
                {
                    _pnlLoggedIn.Visible = false;
                    _pnlLoggedOut.Visible = true;
                }
            }
        }

        public void LoadUserPreferences()
        {
            if (_ddlLanguage != null && HttpContext.Current.Request.Cookies["PreferredLanguage"] != null)
            {
                _ddlLanguage.SelectedValue = HttpContext.Current.Request.Cookies["PreferredLanguage"].Value;
            }

            if (_ddlCurrency != null && HttpContext.Current.Request.Cookies["PreferredCurrency"] != null)
            {
                _ddlCurrency.SelectedValue = HttpContext.Current.Request.Cookies["PreferredCurrency"].Value;
            }
        }

        public void HandleLogout()
        {
            try
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.RemoveAll();

                System.Web.Security.FormsAuthentication.SignOut();

                if (HttpContext.Current.Request.Cookies["PreferredLanguage"] != null)
                {
                    HttpCookie languageCookie = new HttpCookie("PreferredLanguage");
                    languageCookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(languageCookie);
                }

                if (HttpContext.Current.Request.Cookies["PreferredCurrency"] != null)
                {
                    HttpCookie currencyCookie = new HttpCookie("PreferredCurrency");
                    currencyCookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(currencyCookie);
                }

                if (HttpContext.Current.Request.Cookies["UserInfo"] != null)
                {
                    HttpCookie userInfo = new HttpCookie("UserInfo");
                    userInfo.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(userInfo);
                }

                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetNoStore();

                HttpContext.Current.Response.RedirectPermanent("Login.aspx", false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                HttpContext.Current.Response.RedirectPermanent("Login.aspx", false);
            }
        }

        public void HandleLanguageChange(string selectedValue)
        {
            HttpContext.Current.Response.Cookies["PreferredLanguage"].Value = selectedValue;
            HttpContext.Current.Response.Cookies["PreferredLanguage"].Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
        }

        public void HandleCurrencyChange(string selectedValue)
        {
            HttpContext.Current.Response.Cookies["PreferredCurrency"].Value = selectedValue;
            HttpContext.Current.Response.Cookies["PreferredCurrency"].Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
        }
    }
}