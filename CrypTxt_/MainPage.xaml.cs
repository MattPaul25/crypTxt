using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using CrypTxt_.Resources;

namespace CrypTxt_
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (DataStore.messageHolder != null) { txtMyMessage.Text = DataStore.messageHolder; }
            txtCurrentKey.Text = DataStore.CurrentKeyName;
            if (txtCurrentKey.Text != "")
            {
                txtKeyLabel.Text = "Current Key";
            }
        }

        private void btnCreateKey_Click(object sender, RoutedEventArgs e)
        {
            DataStore.messageHolder = txtMyMessage.Text;
            NavigationService.Navigate(new Uri("/KeyCreator.xaml", UriKind.Relative));
        }

        private void btnSelectKey_Click(object sender, RoutedEventArgs e)
        {
            DataStore.messageHolder = txtMyMessage.Text;
            NavigationService.Navigate(new Uri("/KeySelection.xaml", UriKind.Relative));
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (txtMyMessage.Text != "" && txtKeyLabel.Text != "")
            {
                DataStore.EncryptedMessage = txtMyMessage.Text;
                txtMyMessage.Text = Decipher.DecipherText();
            }
        }

        private void btnEncryptMessage_Click(object sender, RoutedEventArgs e)
        {
            if (txtMyMessage.Text != "" && txtKeyLabel.Text != "")
            {
                DataStore.UnencryptedMessage = txtMyMessage.Text;
                txtMyMessage.Text = Cipher.CipherText();
            }
        }

        private void btnSend_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SmsComposeTask sms = new SmsComposeTask();
            sms.To = "";
            sms.Body = "encr_" + txtCurrentKey.Text +"|||" + txtMyMessage.Text;
            string myToString = "";
            if (DataStore.KeyContacts != null)
            {
                int i = 0;
                while (i < DataStore.KeyContacts.Length)
                {
                    myToString = myToString + DataStore.KeyContacts[i] + "; ";
                    i++;
                }
                myToString = myToString.Substring(0, myToString.Length - 2);                
            }
            sms.To = myToString;
            DataStore.KeyContacts = null;
            sms.Show();
        }
        private void btnMessages_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Groups.xaml", UriKind.Relative));
        }
    }
}