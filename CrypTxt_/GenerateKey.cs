using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypTxt_
{
    class GenerateKey
    {
        public GenerateKey()
        {
            genKey();
        }
        private static string EncryptionKey;
        private string genKey()
        {
            string randString = "";
            string newKey = "";
            foreach (string c in DataStore.cArray)
            {
                randString = randomGenerator(0, DataStore.cArray.Length - 1);
                newKey = newKey + "|||" + randString;
            }
            EncryptionKey = newKey + "|||";

            EncryptionKey = AddingBlenders() + EncryptionKey;
            EncryptionKey = EncryptionKey + AddingBlenders();
            DataStore.NewKeyMade = EncryptionKey;
            return EncryptionKey;
        }
        private string AddingBlenders()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            string myString = "";
            int LoopNum = rand.Next(1, 5);
            int i = 0;
            while (i <= LoopNum)
            {
                int bLen = DataStore.BlenderArr.Length;
                myString = myString + DataStore.BlenderArr[rand.Next(0, bLen)];
                i++;
            }
            return myString;
        }
        private string randomGenerator(int lwer, int uppr)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            string myString = "";
            int cnt = 1;
            int num = rand.Next(3, 6);
            while (cnt <= num)
            {
                int rnum = rand.Next(lwer, uppr);
                myString = myString + DataStore.cArray[rnum];
                cnt++;
            }
            return myString;
        }
    }
}
