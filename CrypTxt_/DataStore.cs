using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypTxt_
{
   public static class DataStore
    {
        public static string[] privChars = { "ß", "§", "đ", "Œ", "ň", "Ŀ", "ç", "ƞ" };
        public static string[] cArray = {  "a", "b", "c", "d", "e", "f", "g", "h", ":",
                                           "i", "j", "k", "l", "m", "n", "o", "p", "#",
                                           "q", "r", "s", "t", "u", "ß", "v", "w", "?", 
                                           "x", "y", "z", "A", "B", "đ", "C", "D", "_",
                                           "E", "F", "G", "H", "I", "J", "K", "ň", "-", 
                                           "L", "M", "N", "O", "P", "Q", "R", "/", "Ŀ",
                                           "S", "T", "U", "V", "W", "X", "4", "=", "ç", 
                                           "Y", "Z", " ", "0", "1", "2", "3", "5", "ƞ",
                                           "6", "7", "8", "9", "!", ".", "@","”" , "Ʃ",
                                           "$", "%", "~", "&", "*", ",", "—","–", "ƫ",
                                           "Ǡ","ǟ","Ǟ","ǝ","ǜ","Ǜ","ǚ","Ǚ","ǘ","Ǘ",
                                           "ǖ","Ǖ","ǔ","Ǔ","ǒ","Ǒ","ǐ","Ǐ","ǎ","Ǎ",
                                           "ǌ","ǋ","Ǌ","ǉ","ǈ","Ǉ","ǆ","ǅ","Ǆ","ǃ",
                                           "ǂ","ǁ","ǀ","ƿ","ƾ","ƽ","Ƽ","ƻ","ƺ","ƹ",
                                           "Ƹ","Ʒ","ƶ","Ƶ","ƴ","Ƴ","Ʋ","Ʊ","ư","Ư","¶",
                                           "´","³","²","±","°","¯","®","0","¬","«",
                                           "ª","©","¨","§","¦","¥","¤","£","¢", "'",
                                           "¡","0","Ÿ","ž","","Œ","›","š","™","µ"};
        public static string NewKeyMade;
        public static string messageHolder;
        public static string CurrentKeyName;
        public static string EncryptedMessage;
        public static string UnencryptedMessage;
        public static string EncryptionKey;
        public static string EncryptionKey_Conversion;
        public static string[] BlenderArr = {"s", "m", "u", "o", "i", "p", "l" };
        public static string[] KeyContacts;
        public static string[] CryptArr;
        public static string[] MatchArr;
        public static string[] RightBlends;
        public static string[] LeftBlends;
        public static void CreateBlendArrs()
        {
            string EncKey = DataStore.EncryptionKey;
            int plcement;
            try
            {
                plcement = StaticFunctions.Search(EncKey, "[[") - 1;
                string Contacts = EncKey.Substring(plcement, EncKey.Length - plcement);
                CreateContactArr(Contacts);
                EncKey = EncKey.Substring(0, plcement);
            }
            catch { }
            plcement = StaticFunctions.Search(EncKey, "|||") - 1;
            string blnds = StaticFunctions.Left(EncKey, plcement);
            EncKey = StaticFunctions.Right(EncKey, plcement);
            DataStore.LeftBlends = StaticFunctions.SimpleSplit(blnds);
            plcement = StaticFunctions.countString(EncKey, "|||");
            plcement = StaticFunctions.Search(EncKey, "|||", plcement) + 2;
            blnds = EncKey.Substring(plcement, EncKey.Length - plcement);
            DataStore.RightBlends = StaticFunctions.SimpleSplit(blnds);
            DataStore.EncryptionKey_Conversion = EncKey.Substring(0, EncKey.Length - DataStore.RightBlends.Length);
        }
        public static void CreateArrs()
        {

            string EncKey = DataStore.EncryptionKey_Conversion;
            int uppBound = DataStore.cArray.Length;
            string[] cryptArray2 = new string[uppBound];
            string[] matchArr2 = new string[uppBound];
            int cnt = 0;
            EncKey = EncKey.Substring(3);
            while (cnt < uppBound)
            {
                matchArr2[cnt] = DataStore.cArray[cnt];
                int nextMark = StaticFunctions.Search(EncKey, "|||") - 1;
                if (nextMark <= 0)
                {
                    cryptArray2[cnt] = EncKey;
                    break;
                }
                else
                {
                    cryptArray2[cnt] = EncKey.Substring(0, nextMark);
                    EncKey = EncKey.Substring(nextMark + 3, EncKey.Length - (nextMark + 3));
                    cnt++;
                }
            }
            DataStore.CryptArr = cryptArray2;
            DataStore.MatchArr = matchArr2;
        }
        public static void CreateContactArr(string contacts)
        {
            int i = 0;
            int ArrCount = StaticFunctions.countString(contacts, "[[");
            string[] ContactArr = new string[ArrCount];
            while (i < ArrCount)
            {
                int x = StaticFunctions.Search(contacts, "]]") - 3;
                ContactArr[i] = contacts.Substring(2, x);
                contacts = contacts.Substring(x + 4, contacts.Length - (x + 4));
                i++;
            }
            DataStore.KeyContacts = ContactArr;
        }
    }
}
