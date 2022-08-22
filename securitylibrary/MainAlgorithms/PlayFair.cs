using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        struct freq
        {
            public char first { set; get; }
            public char second{ set; get; }
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            string PlainText = "";
            int index = 0;
            char[,] matrix = new char[5, 5];
            char[] charArray = key.ToCharArray();
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] uniqueChars = new char[25];
            //remove repeat chars
            for (int k = 0; k < charArray.Length; k++)
            {
                if (Array.IndexOf(uniqueChars, charArray[k]) == -1)
                {
                    uniqueChars[index] = charArray[k];
                    index++;
                }
                else
                    continue;

            }
            for (int k = 0; k < alpha.Length; k++)
            {
                if (Array.IndexOf(uniqueChars, alpha[k]) == -1)
                {
                    if (alpha[k] == 'i')
                    {
                        if (Array.IndexOf(uniqueChars, 'j') > -1)
                        {
                            continue;
                        }
                    }
                    if (alpha[k] == 'j')
                    {
                        if (Array.IndexOf(uniqueChars, 'i') > -1)
                        {
                            continue;
                        }
                    }
                    uniqueChars[index] = alpha[k];
                    index++;
                }
                else
                    continue;
            }
            index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = uniqueChars[index];
                    index++;
                }

            }
            //decrypt
            int c = 0;
            for (int s = 0; s < cipherText.Length / 2; s++)
            {
                int rowOfFirstChar = 0;
                int rowOfSecondChar = 0;
                int colOfFirstChar = 0;
                int colOfSecondChar = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (matrix[i, j] == cipherText[c])
                        {
                            rowOfFirstChar = i;
                            colOfFirstChar = j;
                        }
                        else if (matrix[i, j] == cipherText[c + 1])
                        {
                            rowOfSecondChar = i;
                            colOfSecondChar = j;
                        }

                    }
                }
                //case -> same row
                if (rowOfFirstChar == rowOfSecondChar)
                {
                    //first char
                    if (colOfFirstChar == 0)
                    {
                        PlainText += matrix[rowOfFirstChar, 4];
                    }
                    else
                        PlainText += matrix[rowOfFirstChar, colOfFirstChar - 1];
                    //second char
                    if (colOfSecondChar == 0)
                    {
                        PlainText += matrix[rowOfSecondChar, 4];
                    }
                    else
                        PlainText += matrix[rowOfSecondChar, colOfSecondChar - 1];
                }
                // case -> same col
                else if (colOfFirstChar == colOfSecondChar)
                {
                    //first char
                    if (rowOfFirstChar == 0)
                    {
                        PlainText += matrix[4, colOfFirstChar];
                    }
                    else
                        PlainText += matrix[rowOfFirstChar - 1, colOfFirstChar];
                    //second char
                    if (rowOfSecondChar == 0)
                    {
                        PlainText += matrix[4, colOfSecondChar];
                    }
                    else
                        PlainText += matrix[rowOfSecondChar - 1, colOfSecondChar];
                }
                else
                {
                    PlainText += matrix[rowOfFirstChar, colOfSecondChar];
                    PlainText += matrix[rowOfSecondChar, colOfFirstChar];
                }
                c += 2;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////
            freq[] pairs = new freq[PlainText.Length / 2];
            string temp = "";
            int count = 0;
            for (int i = 0; i < PlainText.Length; i = i + 2)
            {
                pairs[count].first = PlainText[i];
                pairs[count].second = PlainText[i + 1];
                count++;
                if (i == PlainText.Length - 1)
                    break;
            }
            for (int k = 0; k < pairs.Length; k++)
            {
                temp += pairs[k].first;
                if (pairs[k].second == 'x' && k != pairs.Length - 1)
                {
                    if (pairs[k].first == pairs[k + 1].first)
                    {
                        continue;
                    }
                    else
                    {
                        temp += pairs[k].second;
                    }
                }
                else
                {
                    temp += pairs[k].second;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////
            if (temp[temp.Length - 1] == 'x')
                temp = temp.TrimEnd(temp[temp.Length - 1]);

            return temp.ToLower();
        }

        public string Encrypt(string plainText, string key)
        {
            string CipherText = "";
            int index = 0;
            char[,] matrix = new char[5, 5];
            char[] charArray = key.ToLower().ToCharArray();
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] uniqueChars = new char[25];
            //remove repeat chars
            for (int k = 0; k < charArray.Length; k++)
            {
                if (Array.IndexOf(uniqueChars, charArray[k]) == -1)
                {
                    uniqueChars[index] = charArray[k];
                    index++;
                }
                else
                    continue;

            }
            for (int k = 0; k < alpha.Length; k++)
            {
                if (Array.IndexOf(uniqueChars, alpha[k]) == -1)
                {
                    if (alpha[k] == 'i')
                    {
                        if (Array.IndexOf(uniqueChars, 'j') > -1)
                            continue;
                    }
                    if (alpha[k] == 'j')
                    {
                        if (Array.IndexOf(uniqueChars, 'i') > -1)
                            continue;
                    }
                    uniqueChars[index] = alpha[k];
                    index++;
                }
                else
                    continue;

            }
            index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = uniqueChars[index];
                    index++;
                }

            }
            ///////apply algorithm

            for (int k = 0; k < plainText.Length - 1; k = k + 2)
            {
                if (plainText[k] == plainText[k + 1])
                {
                    plainText = plainText.Insert(k + 1, "x");

                }

            }
            if (plainText.Length % 2 != 0)
            {
                plainText += "x";
            }
            int c = 0;
            for (int s = 0; s < plainText.Length / 2; s++)
            {
                int rowOfFirstChar = 0;
                int rowOfSecondChar = 0;
                int colOfFirstChar = 0;
                int colOfSecondChar = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (matrix[i, j] == plainText[c])
                        {
                            rowOfFirstChar = i;
                            colOfFirstChar = j;
                        }
                        else if (matrix[i, j] == plainText[c + 1])
                        {
                            rowOfSecondChar = i;
                            colOfSecondChar = j;
                        }

                    }
                }
                //case -> same row
                if (rowOfFirstChar == rowOfSecondChar)
                {
                    //first char
                    if (colOfFirstChar == 4)
                    {
                        CipherText += matrix[rowOfFirstChar, 0];
                    }
                    else
                        CipherText += matrix[rowOfFirstChar, colOfFirstChar + 1];
                    //second char
                    if (colOfSecondChar == 4)
                    {
                        CipherText += matrix[rowOfSecondChar, 0];
                    }
                    else
                        CipherText += matrix[rowOfSecondChar, colOfSecondChar + 1];
                }
                // case -> same col
                else if (colOfFirstChar == colOfSecondChar)
                {
                    //first char
                    if (rowOfFirstChar == 4)
                    {
                        CipherText += matrix[0, colOfFirstChar];
                    }
                    else
                        CipherText += matrix[rowOfFirstChar + 1, colOfFirstChar];
                    //second char
                    if (rowOfSecondChar == 4)
                    {
                        CipherText += matrix[0, colOfSecondChar];
                    }
                    else
                        CipherText += matrix[rowOfSecondChar + 1, colOfSecondChar];
                }
                else
                {
                    CipherText += matrix[rowOfFirstChar, colOfSecondChar];
                    CipherText += matrix[rowOfSecondChar, colOfFirstChar];
                }
                c += 2;
            }
            return CipherText.ToUpper();

        }
    }
}
