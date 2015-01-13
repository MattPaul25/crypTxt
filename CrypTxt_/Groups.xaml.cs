using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.IO.IsolatedStorage;


namespace CrypTxt_
{
    public partial class Groups : PhoneApplicationPage
    {
        public string Line;
        public Groups()
        {
            InitializeComponent();
            readTextFile();
        }
        void readTextFile()
        {
            try
            {
                IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("Contacts.txt", FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    try
                    {

                        while ((Line = reader.ReadLine()) != null)
                        {
                            if (Line == "") { continue; }
                            //Line = Line.Substring(0, StaticFunctions.Search(Line, " ") - 1);
                            lstContacts.Items.Add(Line);
                        }
                    }
                    catch { }
                    fileStream.Close();
                }
            }
            catch  { MessageBox.Show("No contacts have been added - Add them by assigning contacts to a key :) "); }
        }
        private void lstContacts_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        private void lblContacts_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}