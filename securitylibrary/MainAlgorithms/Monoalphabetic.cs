using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        struct freq
        {
            public char charchter;
            public float num;
        }
        freq[] f = new freq[26];


        char[] Alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        public string Encrypt(string plainText, string key)
        {

            string cipherText = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (plainText[i] == Alpha[j])
                    {
                        cipherText += key[j];
                        break;
                    }

                }

            }
            return cipherText;

        }
        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            string plainText = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (cipherText[i] == key[j])
                    {
                        plainText += Alpha[j];
                        break;
                    }

                }

            }
            return plainText;
        }
        public string Analyse(string plainText, string cipherText)
        {
            char[] key1 = new char[26];
            bool check = false;
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            for (int i = 0; i < Alpha.Length; i++)
            {
                for (int j = 0; j < plainText.Length; j++)
                {
                    if (Alpha[i] == plainText[j])
                    {
                        key1[i] = cipherText[j];
                        break;
                    }
                    if (j == plainText.Length - 1)
                    {
                        key1[i] = ' ';
                        check = true;
                    }
                }
            }
            string q = "";
            if (check == true)
            {
                for (int i = 0; i < Alpha.Length; i++)
                {
                    for (int j = 0; j < key1.Length; j++)
                    {
                        if (Alpha[i] == key1[j])
                            break;
                        else if (j == key1.Length - 1)
                        {
                            q += Alpha[i];
                        }

                    }
                }
            }
           int count = 0;
            for (int j = 0; j < key1.Length; j++)
            {
                if (key1[j] == ' ')
                {
                    key1[j] = q[count];
                    count++;
                }
            }
            string g = "";
            for (int j = 0; j < key1.Length; j++)
            {
                g += key1[j];
            }
            return g;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name = "cipher" ></ param >
        /// < returns > Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            f[0].charchter = 'e'; f[0].num = 12.51f;
            f[1].charchter = 't'; f[1].num = 9.25f;
            f[2].charchter = 'a'; f[2].num = 8.04f;
            f[3].charchter = 'o'; f[3].num = 7.60f;
            f[4].charchter = 'i'; f[4].num = 7.26f;
            f[5].charchter = 'n'; f[5].num = 7.09f;
            f[6].charchter = 's'; f[6].num = 6.54f;
            f[7].charchter = 'r'; f[7].num = 6.12f;
            f[8].charchter = 'h'; f[8].num = 5.49f;
            f[9].charchter = 'l'; f[9].num = 4.14f;
            f[10].charchter = 'd'; f[10].num = 3.99f;
            f[11].charchter = 'c'; f[11].num = 3.06f;
            f[12].charchter = 'u'; f[12].num = 2.71f;
            f[13].charchter = 'm'; f[13].num = 2.53f;
            f[14].charchter = 'f'; f[14].num = 2.30f;
            f[15].charchter = 'p'; f[15].num = 2.00f;
            f[16].charchter = 'g'; f[16].num = 1.96f;
            f[17].charchter = 'w'; f[17].num = 1.92f;
            f[18].charchter = 'y'; f[18].num = 1.73f;
            f[19].charchter = 'b'; f[19].num = 1.54f;
            f[20].charchter = 'v'; f[20].num = 0.99f;
            f[21].charchter = 'k'; f[21].num = 0.67f;
            f[22].charchter = 'x'; f[22].num = 0.19f;
            f[23].charchter = 'j'; f[23].num = 0.16f;
            f[24].charchter = 'q'; f[24].num = 0.11f;
            f[25].charchter = 'z'; f[25].num = 0.09f;
            float z;
            cipher = cipher.ToLower();
            int count = 0;
            float[] pp = new float[26];
            int index = 0;
            for (int i = 0; i < Alpha.Length; i++)
            {
                count = 0;
                for (int j = 0; j < cipher.Length; j++)
                {
                    if (Alpha[i] == cipher[j])
                    {
                        count++;
                    }
                }
                z = (float)count / cipher.Length;
                z = z * 100;
                pp[index] = z;
                index++;
            }
            List<float> flist = new List<float>();
            for (int i = 0; i < f.Length; i++)
            {
                flist.Add(f[i].num);
            }
            char[] match = new char[26];
            for (int i = 0; i < pp.Length; i++)
            {
                float closest = flist.Aggregate((x, y) => Math.Abs(x - pp[i]) < Math.Abs(y - pp[i]) ? x : y);
                int indexx = flist.IndexOf(closest);
                for (int ff = 0; ff < f.Length; ff++)
                {
                    if (closest == f[ff].num)
                    {
                        match[i] = f[ff].charchter;
                    }
                }
                flist.RemoveAt(indexx);
            }
            string text = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (cipher[i] == Alpha[j])
                    {
                        text += match[j];
                    }
                }
            }
            return text;
        }
    }
}
