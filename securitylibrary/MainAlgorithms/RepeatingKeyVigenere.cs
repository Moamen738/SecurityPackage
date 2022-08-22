using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{


    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public char[,] Fillmatrix ()
        {
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            int index;
            char[,] matrix = new char[26, 26];
            int real = 0;
            for (int i = 0; i < alpha.Length; i++)
            {
                index = i;
                for (int J = 0; J < alpha.Length - i; J++)
                {
                    matrix[i, J] = alpha[index];
                    index++;
                    real = J;
                }
                if (i != 0)
                {
                    index = 0;
                    for (int x = 0; x < i; x++)
                    {
                        matrix[i, real + 1] = alpha[index];
                        index++;
                        real++;
                    }
                }
            }
            return matrix;
        }

        public string Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[,] matrix = Fillmatrix();
            string key = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < alpha.Length; j++)
                {
                    if (plainText[i] == alpha[j])
                    {
                        for (int x = 0; x < 26; x++)
                        {
                            if (matrix[x, j] == cipherText[i])
                            {
                                key += alpha[x];
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = 3; i < key.Length; i++)
            {
                if (key[0] == key[i] && key[1] == key[i + 1] && key[2] == key[i + 2])
                {
                    int cou = key.Length - i;
                    key = key.Remove(i, cou);

                }
            }
            return key;
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[,] matrix = Fillmatrix();
            string plain = "";
            int  counnt = cipherText.Length - key.Length;
            for (int q = 0; q < counnt; q++)
            {
                key += key[q];
            }
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < alpha.Length; j++)
                {
                    if (key[i] == alpha[j])
                    {
                        for (int x = 0; x < 26; x++)
                        {
                            if (matrix[j, x] == cipherText[i])
                            {
                                plain += alpha[x];
                                break;
                            }
                        }
                    }

                }
            }
            return plain;
        }

        public string Encrypt(string plainText, string key)
        {
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[,] matrix = Fillmatrix();
            int counnt = plainText.Length - key.Length;
            for (int q = 0; q < counnt; q++)
            {
                key += key[q];
            }
            int row = 0, col = 0;
            string cipher = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < alpha.Length; j++)
                {
                    if (plainText[i] == alpha[j])
                    {
                        col = j;
                    }
                    if (key[i] == alpha[j])
                    {
                        row = j;
                    }
                }
                cipher += matrix[row, col];
            }
            return cipher;
        }
    }
}