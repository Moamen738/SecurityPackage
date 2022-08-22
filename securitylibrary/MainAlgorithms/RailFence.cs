using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            //cipherText = cipherText.ToLower();
            //int key = 2;
            //for (int i = 0; i < plainText.Length / 2; i++)
            //{
            //    if (plainText[i] != cipherText[i])
            //    {
            //        plainText = plainText.Remove(i, 1);
            //        key++;
            //    }
            //    if (plainText[i] == cipherText[i])
            //        plainText = plainText.Remove(i + 1, 1);
            //    if (plainText[0] == cipherText[0] && plainText[1] == cipherText[1])
            //        break;
            //}
            //return key;

            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            int check = plainText.Length % 2;
            int depth = plainText.Length / 2;
            if (check != 0) depth++;
            bool x = false;
            int count = 0;
            for (int i = 1; i < plainText.Length; i++)
            {
                for (int j = 1; j < cipherText.Length; j++)
                {
                    if (plainText[i] == cipherText[j])
                    {
                        if (j == depth)
                        {
                            count++;
                        }
                    }
                }
            }
            if (count >= 3 || check == 0)
            {
                return 2;
            }
            else return 3;


        }

        public string Decrypt(string cipherText, int key)
        {
            cipherText = cipherText.ToLower();
            int  index = 0;
            string[] column = new string[key];
            int check = cipherText.Length % key;
            int depth = cipherText.Length / key;
            if (check != 0) depth = depth + 1;
            string plain = "";
            for (int i = 0; i < key; i++)
            {
                for (int J = 0; J < depth; J++)
                {
                    if (index >= cipherText.Length) break;
                    column[i] += cipherText[index];
                    index++;
                }
                if (check != 0 && i == 0) depth--;
            }
            index = 0;
            int count = 0;
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (index == key)
                    {
                        index = 0;
                        count++;
                    }
                    if (count >= column[index].Length) break;
                    plain += column[index][count];
                    index++;
                }
            }
            return plain;

        }

        public string Encrypt(string plainText, int key)
        {
            string[] column = new string[key];
            int check = plainText.Length % key;
            int depth = plainText.Length / key;
            if (check != 0) depth = depth + 1;
            int index;
            for (int i = 0; i < key; i++)
            {
                index = i;
                for (int j = 0; j < depth; j++)
                {
                    if (index >= plainText.Length) break;
                    column[i] += plainText[index];
                    index = index + key;
                }
            }

            string cipher = "";
            for (int i = 0; i < key; i++)
            {
                cipher += column[i].ToString();
            }
            return cipher;
        }
    }
}
