using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.IO;
using Windows.ApplicationModel.Contacts;
using Microsoft.Phone.Tasks;
using System.Threading.Tasks;
namespace CrypTxt_
{
    public partial class KeyCreator : PhoneApplicationPage
    {
        PhoneNumberChooserTask phoneNumberChooserTask;
        public KeyCreator()
        {
            phoneNumberChooserTask = new PhoneNumberChooserTask();
            phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);
            InitializeComponent();
        }

        private void btnGenerateKey_Click(object sender, RoutedEventArgs e)
        {
            GenerateKey GK = new GenerateKey();
            txtNewKey.Text = DataStore.NewKeyMade;
        }

        private void btnSaveKey_Click(object sender, RoutedEventArgs e)
        {

            if (txtKeyName.Text != "")
            {
                IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

                if (myIsolatedStorage.FileExists("uKeys.txt"))
                {
                    IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("uKeys.txt", FileMode.Append, FileAccess.Write);
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        string someTextData = txtKeyName.Text + " " + txtNewKey.Text;
                        writer.WriteLine(someTextData);
                        writer.Close();
                    }
                }
                else
                {
                    using (StreamWriter writeKey = new StreamWriter(new IsolatedStorageFileStream("uKeys.txt", FileMode.Create, FileAccess.Write, myIsolatedStorage)))
                    {
                        writeKey.WriteLine(txtKeyName.Text + " " + txtNewKey.Text + "\n");
                        writeKey.Close();
                    }
                }

                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
       
        }
        private void btnAssignKey_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            phoneNumberChooserTask.Show();

        }
        void phoneNumberChooserTask_Completed(object sender, PhoneNumberResult e)
        {

            //TODO: move assigning people to keys and writing them to contacts file in own method that is called in the onSave_click event
            if (e.TaskResult == TaskResult.OK)
            {
                txtNewKey.Text = txtNewKey.Text + "[[" + e.DisplayName + e.PhoneNumber + "]]";
                IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

                //may replace with a data-bound list thing instead of text file... not sure yet.
                if (myIsolatedStorage.FileExists("Contacts.txt"))
                {
                    IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("Contacts.txt", FileMode.Append, FileAccess.Write);
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(e.DisplayName + e.PhoneNumber);
                        writer.Close();
                    }
                }
                else
                {
                    using (StreamWriter writeKey = new StreamWriter(new IsolatedStorageFileStream("Contacts.txt", FileMode.Create, FileAccess.Write, myIsolatedStorage)))
                    {
                        writeKey.WriteLine(e.DisplayName + e.PhoneNumber);
                        writeKey.Close();
                    }
                }
            }
        }

        private void lblKeyCreator_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }

}