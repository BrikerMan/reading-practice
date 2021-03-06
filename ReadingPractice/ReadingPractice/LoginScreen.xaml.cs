﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ReadingPractice
{
    public partial class LoginScreen : UserControl
    {
        public ServerCommunication serverCommunication
        {
            get
            {
                return mainPage.serverCommunication;
            }
        }
        private MainPage mainPage;
        public event Action userLoggedIn;

        public string loginError
        {
            get
            {
                return this.loginErrors.Content.ToString();
            }
            set
            {
                if (value == "")
                {
                    this.loginErrors.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                }
                else
                {
                    this.loginErrors.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                    this.loginButton.IsEnabled = false;
                }
                this.loginErrors.Content = value;
            }
        }

        public string createAccountError
        {
            get
            {
                return this.createAccountErrors.Content.ToString();
            }
            set
            {
                if (value == "")
                {
                    this.createAccountErrors.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                }
                else
                {
                    this.createAccountErrors.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                    this.createAccountButton.IsEnabled = false;
                }
                this.createAccountErrors.Content = value;
            }
        }

        public bool allowCreateAccount
        {
            set
            {
                if (value)
                {
                    if (this.createValidUserNameEntered && this.createAccountPassword.Password != "" && this.createAccountPassword.Password == this.createAccountPassword2.Password && createAccountError == "")
                        this.createAccountButton.IsEnabled = true;
                    else
                        this.createAccountButton.IsEnabled = false;
                }
                else
                {
                    this.createAccountButton.IsEnabled = false;
                }
            }
        }

        public LoginScreen(MainPage mainPage)
        {
            InitializeComponent();
            this.loginButton.IsEnabled = false;
            this.createAccountButton.IsEnabled = false;
            this.loginErrors.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            this.createAccountErrors.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            this.loginErrors.FontWeight = FontWeights.Bold;
            this.createAccountErrors.FontWeight = FontWeights.Bold;
            this.mainPage = mainPage;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = loginUserNameTextBox.Text;
            string password = loginPasswordTextBox.Password;
            serverCommunication.doesUserExistAndIsPasswordCorrect(username, password,
            (doesUserExist, isPasswordCorrect) =>
            {
                if (!doesUserExist)
                {
                    loginError = "user " + username + " does not exist";
                }
                else if (!isPasswordCorrect)
                {
                    loginError = "password for " + username + " is not correct";
                }
                else
                {
                    serverCommunication.username = username;
                    userLoggedIn();
                }
            });
        }

        private void loginUserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loginUserNameTextBox.Text == "" || loginPasswordTextBox.Password == "")
            {
                loginButton.IsEnabled = false;
            }
            else
            {
                loginButton.IsEnabled = true;
            }
        }

        private void loginPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (loginUserNameTextBox.Text == "" || loginPasswordTextBox.Password == "")
            {
                loginButton.IsEnabled = false;
            }
            else
            {
                loginButton.IsEnabled = true;
            }
        }

        private void createAccountButton_Click(object sender, RoutedEventArgs e)
        {
            string username = createAccountUsername.Text;
            string password = createAccountPassword.Password;
            serverCommunication.createAccount(username, password, () =>
            {
                serverCommunication.username = username;
                userLoggedIn();
            });
            mainPage.closeLoginScreen();
        }

        bool createValidUserNameEntered = false;
        string createUserNameProposed = "";

        private void createAccountUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (createAccountUsername.Text == "" || createAccountPassword.Password == "" || createAccountPassword2.Password == "")
            {
                allowCreateAccount = false;
            }
            else
            {
                allowCreateAccount = true;
            }
            string username = createAccountUsername.Text;
            if (username != "")
            {
                serverCommunication.doesUserExist(username, (doesUserExist) =>
                {
                    createValidUserNameEntered = !doesUserExist;
                    createUserNameProposed = username;
                    passwordMatchCheck();
                });
            }
        }

        private void createValidUsernameEnteredCheck()
        {
            if (!createValidUserNameEntered)
            {
                createAccountError = "user " + createUserNameProposed + " already exists";
                allowCreateAccount = false;
            }
            else
            {
                createAccountError = "";
                allowCreateAccount = true;
            }
        }

        private void passwordMatchCheck()
        {
            if (createAccountPassword.Password != "" && createAccountPassword2.Password != "")
            {
                if (createAccountPassword.Password != createAccountPassword2.Password)
                {
                    createAccountError = "entered passwords don't match";
                    allowCreateAccount = false;
                    return;
                }
                else
                {
                    createAccountError = "";
                }
            }
            createValidUsernameEnteredCheck();
        }

        private void createAccountPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (createAccountUsername.Text == "" || createAccountPassword.Password == "" || createAccountPassword2.Password == "")
            {
                allowCreateAccount = false;
            }
            else
            {
                allowCreateAccount = true;
            }
            passwordMatchCheck();
        }

        private void createAccountPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (createAccountUsername.Text == "" || createAccountPassword.Password == "" || createAccountPassword2.Password == "")
            {
                allowCreateAccount = false;
            }
            else
            {
                allowCreateAccount = true;
            }
            passwordMatchCheck();
        }

        private void loginPasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (loginButton.IsEnabled && e.Key == Key.Enter)
            {
                loginButton_Click(null, null);
            }
        }

        private void createAccountUsername_KeyUp(object sender, KeyEventArgs e)
        {
            if (createAccountButton.IsEnabled && e.Key == Key.Enter)
            {
                createAccountButton_Click(null, null);
            }
        }
    }
}
