using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            int first = 0, second = 0, lenghtOfKey = 0;
            for (int j = 0; j < plainText.Length; j++)
            {
                if (cipherText[0] == plainText[j])
                {
                    first = j;
                    for (int i = first + 1; i < plainText.Length - j; i++)
                    {
                        if (cipherText[1] == plainText[i])
                        {
                            second = i;
                            break;

                        }
                    }

                }
                lenghtOfKey = second - first;
                if (lenghtOfKey > 2)
                {
                    break;
                }
            }
            int columns = plainText.Length / lenghtOfKey;
            int index = 0, count = 0;
            string[] plain = new string[columns];
            string[] newPlain = new string[lenghtOfKey];
            string[] cipher = new string[lenghtOfKey];
            List<int> mainkey = new List<int>();
            for (int i = 0; i < lenghtOfKey; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    cipher[index] += cipherText[count];
                    count++;
                }
                index++;
            }
            count = 0;
            index = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < lenghtOfKey; j++)
                {
                    if (count >= plainText.Length) break;
                    plain[i] += plainText[index];
                    count++;
                    index++;
                }

            }
            count = 0;
            for (int i = 0; i < lenghtOfKey; i++)
            {
                for (int k = 0; k < columns; k++)
                {
                    newPlain[i] += plain[k][count];
                }
                count++;
            }

            for (int i = 0; i < newPlain.Length; i++)
            {
                for (int j = 0; j < cipher.Length; j++)
                {
                    if (newPlain[i] == cipher[j])
                    {
                        mainkey.Add(j + 1);
                        break;
                    }
                }

            }
            return mainkey;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            int length = key.Count();
            int check = cipherText.Length % length;
            int numOfColumn = cipherText.Length / length;
            if (check != 0) numOfColumn++;
            string[] column = new string[numOfColumn];
            string[] character = new string[length];
            string plain = "";
            int count = 0, indexInCipher;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < numOfColumn; j++)
                {
                    if (count >= cipherText.Length) break;
                    character[i] += cipherText[count];
                    count++;
                }
            }
            count = 0;
            for (int i = 0; i < length; i++)
            {
                indexInCipher = key[i];
                indexInCipher--;
                for (int j = 0; j < numOfColumn; j++)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        if (count == numOfColumn) count = 0;
                        if (count >= character[indexInCipher].Length) break;
                        column[j] += character[indexInCipher][count];
                        count++;
                    }
                }
            }
            for (int i = 0; i < column.Length; i++)
            {
                plain += column[i];
            }
            return plain;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            int length = key.Count();
            int check = plainText.Length % length;
            int numOfColumn = plainText.Length / length;
            if (check != 0) numOfColumn++;
            string[] column = new string[numOfColumn];
            int count = 0, index = 0;
            for (int i = 0; i < column.Length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (count >= plainText.Length) break;
                    column[i] += plainText[index];
                    count++;
                    index++;
                }
            }
            int indexInPlain;
            string cipher = "";
            for (int i = 1; i <= length; i++)
            {
                indexInPlain = key.IndexOf(i);
                for (int j = 0; j < column.Length; j++)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        if (indexInPlain >= column[j].Length) break;
                        cipher += column[j][indexInPlain];
                    }
                }
            }
            return cipher;
        }
    }
}
