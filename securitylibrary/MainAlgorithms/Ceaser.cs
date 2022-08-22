using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
      
        char[] index_Of_P = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        public string Encrypt(string plainText, int key)
        {
            int c;
            string cipherText = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < index_Of_P.Length; j++)
                {
                    if (plainText[i] == index_Of_P[j])
                    {
                        c = (j + key) % 26;
                        cipherText += index_Of_P[c];
                        break;
                    }

                }
            }
            
            return cipherText ;
        }
        
        public string Decrypt(string cipherText, int key)
        {
            cipherText = cipherText.ToLower();
            string plainText = "";
            int sum, c;
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < index_Of_P.Length; j++)
                {
                    if (cipherText[i] == index_Of_P[j])
                    {
                        sum = (j - key);
                        if (sum > 0)
                            c = sum % 26;
                        else
                        {
                            sum = sum +26;
                            c = sum % 26;
                        }
                        plainText += index_Of_P[c];
                        break;
                    }
                }
            }           
            return plainText;
        }

        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            int c = 0 , p = 0 , key;
                for (int j = 0; j < index_Of_P.Length; j++)
                {
                    if (cipherText[0] == index_Of_P[j])
                    {
                         c= j;
                    }
                    if (plainText[0] == index_Of_P[j])
                    { 
                         p = j;
                    }
                }
                int sum = c - p;
            if (sum > 0)
                key = sum % 26;
            else
            {
                sum = sum + 26;
                key = sum % 26;
            }
            return key;
        }
    }
}
