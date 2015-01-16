using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypTxt_
{

public static class Cipher
        {
            //running encryption twice is not working...
            public static string CipherText()
            {
               //public method to run hidden methods
               return encryptMessage();
            }
            private static string encryptMessage()
            {
                string message = DataStore.UnencryptedMessage;
                //message = addRandChars(message); //removed because add random character logic is flawed with ascii characters
                //message = message.Length < 20 ? addCharacters(message) : message; //removed for reasons of not wanting to add trim command
                message = rigText(message);
                DataStore.CreateBlendArrs();
                message = RunBlendEncryption(message, DataStore.LeftBlends);
                DataStore.CreateArrs();
                string newMessage = "";
                for (int i = 0; i < message.Length; i++)
                {
                    string MyChar = message.Substring(i, 1);
                    int myIndex = StaticFunctions.findIndex(MyChar, DataStore.cArray);
                    newMessage = newMessage + DataStore.CryptArr[myIndex];
                }
                newMessage = RunBlendEncryption(newMessage, DataStore.RightBlends);
                return newMessage;
            }
            private static string addRandChars(string message)
            {
                //pulled due to creating conflicts with other characters (and having some non-allowable chars)
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                int myLen = DataStore.privChars.Length - 1;
                int i = 0;
                int myNum = rand.Next(0, 10);

                while (i < myNum)
                {
                    string myChar = DataStore.privChars[rand.Next(0, myLen)];
                    int insertionPoint = rand.Next(0, message.Length);
                    message = message.Insert(insertionPoint, myChar);
                    i++;
                }
                return message;
            }
            private static string addCharacters(string message)
            {
                //places spaces infront or behind the test in order to create new ordinal positions for rig text
                //pulled due to not wanting to run trim function at end
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                int num = 20 - message.Length;
                int i = 0;
                while (i < num)
                {
                    int ba = rand.Next(0, 2);
                    message = ba != 0 ? message + " " : " " + message;
                    i++;
                }
                return message;
            }
            private static string RunBlendEncryption(string message, string[] blnArr)
            {
                for (int i = 0; i < blnArr.Length; i++)
                {
                    string Func = blnArr[i];
                    message = Blend.ChooseBlender(Func, message, i);
                }
                return message;
            }
            private static string rigText(string SomeText)
            {
                //responsible for changing the characters before any other ciphering happens
                //changes by the placement the character is within the character array - 
                //then moves the character by that in the character array in dataStore class
                for (int i = 0; i < SomeText.Length; i++)
                {
                    string myChar = SomeText.Substring(i, 1);
                    int newIndex = StaticFunctions.findIndex(myChar, DataStore.cArray) + i;
                    while (newIndex >= DataStore.cArray.Length)
                    {
                        newIndex = newIndex - DataStore.cArray.Length;
                    }
                    myChar = DataStore.cArray[newIndex];
                    SomeText = StaticFunctions.ReplaceTextIndex(SomeText, myChar, i);
                }
                return SomeText;
            }
        }
public static class Decipher
        {
            //class that unravels the text baqsed on the key stored in data store
            public static string DecipherText()
            {
                //public string to keep other portions of the code
                return unEncryptMessage();
            }
            private static string unEncryptMessage()
            {
                //the unblending has created some errors that need fixing....
                string message = DataStore.EncryptedMessage;
                DataStore.CreateBlendArrs();
                message = RunBlendDecryption(message, DataStore.RightBlends);
                DataStore.CreateArrs(); //arrays are already created but thats assuming were first encrypting         
                string[,] charArray;
                int arraySize = 0;
                for (int i = 0; i < DataStore.CryptArr.Length; i++)
                {
                    //loop just gets the bounds of the array
                    string MyChar = DataStore.CryptArr[i];
                    int strCount = StaticFunctions.countString(message, MyChar);
                    if (strCount == 0) { continue; }
                    arraySize = arraySize + strCount;

                }
                charArray = createArray(arraySize, message);
                string[] sortedCharArray = boolSort(arraySize, charArray);
                message = string.Join("", sortedCharArray);
                message = RunBlendDecryption(message, DataStore.LeftBlends);
                message = dRigText(message);
                //message = message.Trim();
                //message = remRandChars(message);
                return message;
            }
            private static string remRandChars(string message)
            {
                foreach (string i in DataStore.privChars)
                {
                    message = message.Replace(i, "");
                }
                return message;
            }
            private static string RunBlendDecryption(string message, string[] blnArr)
            {
                for (int i = blnArr.Length - 1; i >= 0; i--)
                {
                    string func = blnArr[i];
                    message = Blend.ChooseReOrder(func, message, i);
                }
                return message;
            }
            private static string[] boolSort(int nLen, string[,] anArr)
            {
                //change for more efficient sort - i.e. insertion sort
                string[] nArray = new string[nLen];

                for (int i = 0; i < nLen; i++)
                {
                    int x = 0;
                    int cnt = 0;
                    while (x < nLen)
                    {
                        int arrI = Convert.ToInt32(anArr[i, 1]);
                        int arrX = Convert.ToInt32(anArr[x, 1]);
                        if (arrI > arrX || (arrI == arrX && x > i))
                        { cnt++; }
                        x++;
                    }
                    nArray[cnt] = anArr[i, 0];
                }
                return nArray;
            }
            private static string[,] createArray(int arraySize, string message)
            {
                string[,] newArr = new string[arraySize, 2];
                int cnt = 0;
                for (int i = 0; i < DataStore.CryptArr.Length; i++)
                {
                    string MyChar = DataStore.CryptArr[i];
                    int strCount = StaticFunctions.countString(message, MyChar);
                    if (strCount == 0) { continue; }
                    int j = 1;
                    while (j <= strCount)
                    {
                        if (strCount != 0)
                        {
                            newArr[cnt, 0] = DataStore.cArray[i];
                            newArr[cnt, 1] = StaticFunctions.Search(message, DataStore.CryptArr[i], j, true).ToString();
                            j++;
                            cnt++;
                        }
                    }
                }
                return newArr;
            }
            private static string dRigText(string SomeText)
            {
                for (int i = 0; i < SomeText.Length; i++)
                {
                    string myChar = SomeText.Substring(i, 1);
                    int newIndex = StaticFunctions.findIndex(myChar, DataStore.cArray) - i;
                    while (newIndex < 0)
                    {
                        newIndex = newIndex + DataStore.cArray.Length;
                    }
                    myChar = DataStore.cArray[newIndex];
                    SomeText = StaticFunctions.ReplaceTextIndex(SomeText, myChar, i);
                }

                return SomeText;
            }
        }
public static class Blend
        {
            //class is called from both cipher and decipher classes
            //blending before character conversions
            //each blend has a reverse (i.e. push has a pull) *swap is its own reverser
            //dump the string in this class - spits out a blended string depending on the key
            public static string ChooseBlender(string Func, string message, int i)
            {
                //function chooses which method to use based on the key created
                switch (Func)
                {
                    case "s":
                        message = Swap(message, i);
                        break;
                    case "o":
                        message = outIn(message);
                        break;
                    case "m":
                        message = mergeText(message);
                        break;
                    case "i":
                        message = inOut(message);
                        break;
                    case "p":
                        message = Push(message);
                        break;
                    case "l":
                        message = Pull(message);
                        break;
                }

                return message;
            }
            public static string ChooseReOrder(string Func, string message, int i)
            {
                //funtion chooses reorder based on character in keys on front or back of key string
                switch (Func)
                {
                    case "s":
                        message = Swap(message, i);
                        break;
                    case "o":
                        message = inOut(message);
                        break;
                    case "m":
                        message = unMergeText(message);
                        break;
                    case "i":
                        message = outIn(message);
                        break;
                    case "p":
                        message = Pull(message);
                        break;
                    case "l":
                        message = Push(message);
                        break;
                }
                return message;
            }
            private static string mergeText(string someText)
            {
                int strLength = someText.Length - 1;
                for (int i = 0; i <= strLength; i = i + 2)
                {
                    string myChar = someText.Substring(strLength, 1);
                    string left = StaticFunctions.Left(someText, i);
                    string right = StaticFunctions.Right(someText, i);
                    someText = left + myChar + right;
                    someText = someText.Substring(0, strLength + 1);
                }
                return someText;
            }
            private static string unMergeText(string someText)
            {
                string[] myStringArr = new string[someText.Length];
                int strLength = someText.Length - 1;
                int cntD = strLength;
                int cntU = 0;
                for (int i = 0; i <= strLength; i = i + 2)
                {
                    string myChar = someText.Substring(i, 1);
                    int x = i + 1;
                    try
                    {
                        string myChar2 = someText.Substring(x, 1);
                        myStringArr[cntU] = myChar2;
                    }
                    catch { }
                    myStringArr[cntD] = myChar;
                    cntD--;
                    cntU++;
                }
                someText = StaticFunctions.convertArray(myStringArr);
                return someText;
            }
            private static string Swap(string someText, int sNum)
            {
                string[] NewSet = StaticFunctions.SimpleSplit(someText);
                for (int i = 0; i < NewSet.Length; i = i + sNum + 1)
                {
                    string holder = NewSet[i];
                    int j = i + 1;
                    try
                    {
                        NewSet[i] = NewSet[j];
                        NewSet[j] = holder;
                    }
                    catch { }
                }
                return StaticFunctions.convertArray(NewSet);
            }
            private static string outIn(string someText)
            {
                int step;
                int x;
                int i = 0;
                int len = someText.Length;
                if (len % 2 == 1)
                {
                    x = (len + 1) / 2;
                    while (i < x / 2)
                    {
                        string lMove = someText.Substring(0, 1);
                        string rMove = someText.Substring(len - 1, 1);
                        someText = someText.Insert(len - x, rMove);
                        someText = someText.Substring(0, len);
                        someText = someText.Insert(x, lMove);
                        someText = someText.Substring(1, len);
                        i++;
                    }
                }
                else
                {
                    x = len / 2;
                    step = 1;
                    while (i < x / 2)
                    {
                        string lMove = someText.Substring(0, 1);
                        string rMove = someText.Substring(len - 1, 1);
                        someText = someText.Insert(x - step, rMove);
                        someText = someText.Substring(0, len);
                        someText = someText.Insert(x + step, lMove);
                        someText = someText.Substring(1, len);
                        i++;
                    }
                }

                return someText;
            }
            private static string inOut(string someText)
            {
                int step;
                int x;
                int i = 0;
                int len = someText.Length;
                if (len % 2 == 1)
                {
                    x = (len + 1) / 2;
                    step = 0;
                    while (i < x / 2)
                    {
                        string lMove = someText.Substring(x - 1, 1);
                        someText = someText.Remove(x - 1, 1);
                        someText = someText.Insert(0, lMove);
                        string rMove = someText.Substring(x - 1, 1); //
                        someText = someText.Remove(x - 1, 1);
                        someText = someText.Insert(len - 1, rMove);
                        i++;
                    }
                }
                else
                {
                    x = len / 2;
                    step = 1;


                    while (i < x / 2)
                    {
                        string lMove = someText.Substring(x, 1);
                        someText = someText.Remove(x, 1);
                        someText = someText.Insert(0, lMove);
                        string rMove = someText.Substring(x - step, 1);
                        someText = someText.Remove(x - step, 1);
                        someText = someText.Insert(len - 1, rMove);
                        i++;
                    }
                }
                return someText;
            }
            private static string Push(string someText)
            {

                int num = someText.Length % 2 == 1 ? (someText.Length + 1) / 2 : someText.Length / 2;
                int i = 0;
                while (i < num)
                {
                    string myChar = someText.Substring(0, 1);
                    someText = someText.Substring(1, someText.Length - 1) + myChar;
                    i++;
                }
                return someText;
            }
            private static string Pull(string someText)
            {
                int num = someText.Length % 2 == 1 ? (someText.Length + 1) / 2 : someText.Length / 2;
                int i = someText.Length % 2 == 1 ? (someText.Length) : someText.Length - 1;
                while (i >= num)
                {
                    string myChar = someText.Substring(someText.Length - 1, 1);
                    someText = myChar + someText.Substring(0, someText.Length - 1);
                    i--;
                }
                return someText;
            }
        }
public static class StaticFunctions
        {
            //static utility class to do some basic text manipulation
            //class is called from all other classes - does not call any classes though
            public static int Search(string yourString, string yourMarker, int yourInst = 1, bool caseSensitive = true)
            {
                //returns the placement of a string in another string
                int num = 0;
                int ginst = 1;
                int mlen = yourMarker.Length;
                int slen = yourString.Length;
                string tString = "";
                try
                {
                    if (caseSensitive == false)
                    {
                        yourString = yourString.ToLower();
                        yourMarker = yourMarker.ToLower();
                    }
                    while (num < slen)
                    {
                        tString = yourString.Substring(num, mlen);

                        if (tString == yourMarker && ginst == yourInst)
                        {
                            return num + 1;
                        }
                        else if (tString == yourMarker && yourInst != ginst)
                        {
                            ginst += 1;
                            num += 1;
                        }
                        else
                        {
                            num += 1;
                        }
                    }
                    return 0;
                }
                catch
                {
                    return 0;
                }
            }
            public static int countString(string yourString, string yourMarker)
            {
                int cnt = 0;
                int sLen = yourString.Length;
                int mLen = yourMarker.Length;
                for (int i = 0; i + mLen <= sLen; i++)
                {
                    if (yourString.Substring(i, mLen) == yourMarker)
                    {
                        cnt++;
                    }
                }
                return cnt;
            }
            public static string Left(string yourString, int PlaceMent)
            {
                return yourString.Substring(0, PlaceMent);
            }
            public static string Right(string yourString, int PlaceMent)
            {
                return yourString.Substring(PlaceMent, yourString.Length - PlaceMent);
            }
            public static int findIndex(string p, string[] anArr)
            {
                int x = anArr.Length;
                int i;
                for (i = 0; i < x; i++)
                {
                    if (anArr[i] == p)
                    {
                        break;
                    }
                }
                return i;
            }
            public static string[] SimpleSplit(string aString)
            {
                string[] newArr = new string[aString.Length];
                for (int i = 0; i < aString.Length; i++)
                {
                    newArr[i] = aString.Substring(i, 1);
                }
                return newArr;
            }
            public static string convertArray(string[] stringArr)
            {
                string newString = "";
                int i;
                for (i = 0; i < stringArr.Length; i++)
                {
                    newString = newString + stringArr[i];
                }
                return newString;
            }
            public static string ReplaceTextIndex(string yourString, string yourChar, int plcemnet)
            {
                int len = yourString.Length;
                string[] myStrngArr = SimpleSplit(yourString);
                myStrngArr[plcemnet] = yourChar;
                return convertArray(myStrngArr);
            }
        }

}

