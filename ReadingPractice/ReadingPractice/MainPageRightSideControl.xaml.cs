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
using System.Windows.Browser;

namespace ReadingPractice
{
    public partial class RightSidebarControl : UserControl
    {
        public MainPage mainPage;

        public WordDictionary wordDictionary
        {
            get
            {
                return mainPage.wordDictionary;
            }
        }

        /// <summary>
        /// do not add code to the constructor. Put things you want to occur on startup in performOnStartup
        /// </summary>
        public RightSidebarControl()
        {
            Visibility = Visibility.Collapsed;
            InitializeComponent();
        }

        public void performOnStartup()
        {
            //LanguageSelector.Items.Add("Select Language");
            LanguageSelector.Items.Add("Mandarin Chinese (Simplified)");
            LanguageSelector.SelectedIndex = 0;
            this.readSentencesTab.mainPage = mainPage;
            this.contributeSentencesTab.mainPage = mainPage;
            this.closedSentencesTab.mainPage = mainPage;
            this.readSentencesTab.performOnStartup();
            this.contributeSentencesTab.performOnStartup();
            this.closedSentencesTab.performOnStartup();
        }

        public void userLoggedIn(string username)
        {
            this.loggedInAs.Text = "Logged in as " + username;
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HtmlPage.Window.Navigate(HtmlPage.Document.DocumentUri);
            }
            catch (Exception ex)
            {

            }
        }



        double dMinWidthRightSideBar = 300;


        internal void Resize(double height, double width)
        {
//            this.Width = 5 * width / 11;
//            this.Height = height;

            this.LayoutRoot.Width = Math.Max(dMinWidthRightSideBar, 0.5 * width);
            this.LayoutRoot.Height = Math.Max(320,height);

            this.tabControl1.Width = this.LayoutRoot.Width - 1;
            this.tabControl1.Height = this.LayoutRoot.Height-80;
            this.readSentencesTab.Width = tabControl1.Width - 39;
//            this.readSentencesTab.Height = tabControl1.Height;

//            this.readSentencesTab.SentenceListViewer.Height = this.tabControl1.Height-200;
            //            this.readSentencesTab.Warnings.Width = tabControl1.Width;
            //            this.readSentencesTab.Resize(this.tabControl1.Width - 1);
            this.readSentencesScrollViewer.Width = this.tabControl1.Width - 12;
            this.readSentencesScrollViewer.Height = this.tabControl1.Height-37;


//            this.closedTab.Width = tabControl1.Width - 39;
            this.closedSentencesScrollViewer.Width = this.tabControl1.Width - 12;
            this.closedSentencesScrollViewer.Height = this.tabControl1.Height - 37;

//            this.contributeTab.Width = tabControl1.Width - 39;
            this.contributeSentencesScrollViewer.Width = this.tabControl1.Width - 12;
            this.contributeSentencesScrollViewer.Height = this.tabControl1.Height - 37;

            //this.contributeSentencesTab.Width = contributeSentencesScrollViewer.Width-5;
            //ContributedSentenceListViewer

            this.contributeSentencesTab.Resize(contributeSentencesScrollViewer.Height,
                contributeSentencesScrollViewer.Width);


            this.TopGrid.Width = this.tabControl1.Width-1;
        }
    }
}
