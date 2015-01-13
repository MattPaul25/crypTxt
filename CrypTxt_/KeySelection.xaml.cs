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
    public partial class KeySelection : PhoneApplicationPage
    {
        public string Line;
        public string DeletableKey;
        public string[] MyKeys;

        public KeySelection()
        {
            InitializeComponent();
            readTextFile();
            string[] TheKeys;

        }
        void readTextFile()
        {
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("uKeys.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                try
                {

                    while ((Line = reader.ReadLine()) != null)
                    {
                        if (Line == "") { continue; }
                        Line = Line.Substring(0, StaticFunctions.Search(Line, " ") - 1);
                        lstKeySelector.Items.Add(Line);
                    }
                }
                catch { }
                fileStream.Close();
            }
        }
        void AssignKey(string KeyName)
        {
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("uKeys.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                try
                {

                    while ((Line = reader.ReadLine()) != null)
                    {
                        if (Line == "") { continue; }

                        string MyKeyName = Line.Substring(0, StaticFunctions.Search(Line, " ") - 1);
                        if (MyKeyName == KeyName)
                        {
                            int lineStart = StaticFunctions.Search(Line, " ", 1, false) + 1;
                            int linefin = Line.Length - (KeyName.Length);
                            string EncKey = Line.Substring(lineStart, linefin - 2);
                            DataStore.EncryptionKey = EncKey;
                        }
                    }
                }
                catch { }
                fileStream.Close();
            }
        }
        void DeleteKey(string KeyName)
        {
            int cnt = 0;
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("uKeys.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                try
                {

                    while ((Line = reader.ReadLine()) != null)
                    {
                        if (Line == "") { continue; }

                        string MyKeyName = Line.Substring(0, StaticFunctions.Search(Line, " ") - 1);
                        if (MyKeyName != DeletableKey)
                        {
                            cnt++;
                        }
                    }                 

                }
                catch { }
                fileStream.Close();
            }
            LoadKeyArr(cnt);
            WriteKeyText();

        }
        void LoadKeyArr(int cnt)
        {
           MyKeys = new string[cnt];
        int i = 0;

            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("uKeys.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                try
                {

                    while ((Line = reader.ReadLine()) != null)
                    {
                        if (Line == "") { continue; }

                        string MyKeyName = Line.Substring(0, StaticFunctions.Search(Line, " ") - 1);
                        if (MyKeyName != DeletableKey)
                        {
                            MyKeys[i] = Line;
                            i++;
                        }
                    }
                }
                catch { }
                fileStream.Close();
            }
        }
        void WriteKeyText()
        {
            int i = 0;

            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("uKeys.txt", FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                try
                {

                    while (i < MyKeys.Length)
                    {
                        writer.WriteLine(MyKeys[i]);
                        i++;
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString()); 
                }

                writer.Close();
            }
        }
    
        
        private void lstKeySelector_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           var YesNo =  MessageBox.Show("Delete the " + lstKeySelector.SelectedItem.ToString() + " Key?", "Delete?", MessageBoxButton.OKCancel);
           if (YesNo == MessageBoxResult.OK)
           {
               DeletableKey = lstKeySelector.SelectedItem.ToString();
               DeleteKey(DeletableKey);
               NavigationService.Navigate(new Uri("/KeySelection.xaml?" + DateTime.Now.Ticks, UriKind.Relative)); //add datetime.now.ticks to refresh the page
               
           }
        }

        private void btnSelectKey_Click(object sender, RoutedEventArgs e)
        {
            if (lstKeySelector.SelectedItem != null)
            {
                string keyName = lstKeySelector.SelectedItem.ToString();
                DataStore.CurrentKeyName = keyName;
                AssignKey(keyName);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void lblSelectKey_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	 NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}