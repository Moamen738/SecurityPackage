using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>, ICryptographicTechnique<string, string>
    {
        int[,] ListTomatrix;
        int[,] m2m = new int[2, 2];
        int[,] temp = new int[2, 2];
        int[,] m3m = new int[3, 3];
        int[,] transpose = new int[3, 3];
        List<int> FinalList;
        int[,] keymatrix;

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            int[,] plain = new int[2, plainText.Count / 2];
            int[,] cipher = new int[2, plainText.Count / 2];
            int[,] key = new int[2, 2];
            int c = 0;
            for (int i = 0; i < plainText.Count / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (c == plainText.Count)
                        break;
                    plain[j, i] = plainText[c];
                    c++;
                }
            }
            c = 0;
            for (int i = 0; i < plainText.Count / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (c == plainText.Count)
                        break;
                    cipher[j, i] = cipherText[c];
                    c++;
                }
            }



            for (int i = 0; i < plainText.Count / 2; i++)
            {
                for (int row = 0; row < 2; row++)
                {
                    m2m[row, 0] = plain[row, i];
                }

                for (int j = i + 1; j < plainText.Count / 2; j++)
                {
                    for (int row = 0; row < 2; row++)
                    {
                        m2m[row, 1] = plain[row, j];
                    }
                    if (GCD(26, (int)Find_determinant(m2m, 2) % 26) == 1)
                    {
                        for (int row = 0; row < 2; row++)
                        {
                            temp[row, 0] = cipher[row, i];
                            temp[row, 1] = cipher[row, j];
                        }
                        int[,] inverse = findInverseMatrix_2x2(m2m);
                        for (int cm = 0; cm < 2; cm++)
                        {
                            for (int gc = 0; gc < 2; gc++)
                            {
                                int temp = 0;
                                for (int k = 0; k < 2; k++)
                                {
                                    temp += this.temp[cm, k] * inverse[k, gc];
                                }
                                key[cm, gc] = temp;
                                key[cm, gc] %= 26;
                            }

                        }
                        break;
                    }

                }
                break;
            }
            List<int> returnkey = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    returnkey.Add(key[i, j]);
                }
            }

            if (returnkey[0] == 0 && returnkey[1] == 0 && returnkey[2] == 0 && returnkey[3] == 0)
                throw new InvalidAnlysisException();
            return returnkey;
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            Convert_ListToMatrix(key, Convert.ToInt32(Math.Sqrt(key.Count)));
                if (Convert.ToInt32(Math.Sqrt(key.Count)) == 3)
                {
                    findInverseMatrix_3x3();
                    findTransposeMatrix();
                    keymatrix = transpose;
                }
                else
                {
                    findInverseMatrix_2x2();
                    keymatrix = m2m;
                }
                FinalList = new List<int>(Convert.ToInt32(Math.Sqrt(key.Count)));
                for (int i = 0; i < Convert.ToInt32(Math.Sqrt(key.Count)); i++)
                {
                    for (int j = 0; j < Convert.ToInt32(Math.Sqrt(key.Count)); j++)
                    {
                        FinalList.Add(keymatrix[i, j] % 26);
                    }
                }

            if (FinalList[0] == 0 && FinalList[1] == 0 && FinalList[2] == 0 && FinalList[3] == 0)
                throw new InvalidAnlysisException();

            List<int> plain = new List<int>();
                List<int> cipher;
                int colum = Convert.ToInt32(Math.Sqrt(FinalList.Count));
                int element = 0;
                for (int i = 0; i < FinalList.Count; i++)
                {
                    if (cipherText.Count == 0)
                    {
                        break;
                    }
                    cipher = cipherText.GetRange(0, colum);
                    for (int j = 0; j < colum; j++)
                    {
                        element += FinalList[i] * cipher[j];
                        i++;
                    }
                    if (i == FinalList.Count)
                    {
                        i = 0;
                        cipherText.RemoveRange(0, colum);
                    }
                    i--;
                    while (element < 0) element += 26;
                    plain.Add(element % 26);
                    element = 0;
                }
                return plain;
         

        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> cipher = new List<int>();
            List<int> plain;
            int colum = Convert.ToInt32(Math.Sqrt(key.Count));
            int element = 0;
            for (int i = 0; i < key.Count; i++)
            {
                if (plainText.Count == 0)
                {
                    break;
                }
                plain = plainText.GetRange(0, colum);
                for (int j = 0; j < colum; j++)
                {
                    element += key[i] * plain[j];
                    i++;
                }
                if (i == key.Count)
                {
                    i = 0;
                    plainText.RemoveRange(0, colum);
                }
                i--;
                cipher.Add(element % 26);
                element = 0;
            }
            return cipher;
        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            int cm = 0;
            int[,] m3m = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (cm == plainText.Count)
                        break;
                    m3m[j, i] = plainText[cm];
                    cm++;
                }
            }
            cm = 0;
            int[,] mx2m = new int[3, 3];
            int[,] key = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (cm == plainText.Count)
                        break;
                    mx2m[j, i] = cipherText[cm];
                    cm++;
                }
            }
            ListTomatrix = m3m;
            findInverseMatrix_3x3();
            findTransposeMatrix();
            int[,] inverse = transpose;
            for (int i = 0; i < 3; i++)
            {
                for (int c = 0; c < 3; c++)
                {
                    int temp = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        temp += mx2m[i, k] * inverse[k, c];
                    }
                    key[i, c] = temp;
                    key[i, c] %= 26;
                }

            }
            List<int> returnkey = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    returnkey.Add(key[i, j]);
                }
            }
            if (returnkey[0] == 0 && returnkey[1] == 0 && returnkey[2] == 0 && returnkey[3] == 0)
                throw new InvalidAnlysisException();
            return returnkey;
        }

        public void Convert_ListToMatrix(List<int> list, int rowcol)
        {
            int counter = 0;


            ListTomatrix = new int[rowcol, rowcol];
            for (int i = 0; i < rowcol; i++)
            {
                for (int j = 0; j < rowcol; j++)
                {
                    ListTomatrix[i, j] = list[counter];
                    counter++;
                }
            }
        }
        public void findInverseMatrix_3x3()
        {
            //find determenant of the matrix
            double det = Find_determinant(ListTomatrix, 3);
            while (det < 0)
                det += 26;
            // find multiplicative inverse
            int b = Multiplicative_Inverse(Convert.ToInt32(det), 26);

            int h = 0, y = 0, counter = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int mx = 0; mx < 3; mx++)
                        {
                            if (mx != j && k != i)
                            {
                                m2m[h, y] = ListTomatrix[k, mx];
                                counter++;
                                y++;
                                if (counter == 2)
                                {
                                    h++;
                                    y = 0;
                                }
                            }
                        }
                    }
                    counter = 0;
                    h = 0; y = 0;
                    double sign = Math.Pow(-1, i + j);
                    int value = b * Convert.ToInt32(sign) * Convert.ToInt32(Find_determinant(m2m, 2)) % 26;
                    if (value < 0)
                        value += 26;
                    m3m[i, j] = value;
                }
            }
        }
        public void findTransposeMatrix()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    transpose[j, i] = m3m[i, j];
                }
            }

        }
        public void findInverseMatrix_2x2()
        {
            m2m[0, 0] = ListTomatrix[1, 1];
            m2m[0, 1] = -1 * ListTomatrix[0, 1];
            m2m[1, 0] = -1 * ListTomatrix[1, 0];
            m2m[1, 1] = ListTomatrix[0, 0];


            //find determenant of the matrix
            double det = Find_determinant(ListTomatrix, 2);
            while (det < 0)
                det += 26;
            int x = Multiplicative_Inverse((int)det, 26);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    m2m[i, j] = x * m2m[i, j];
                }
            }

        }
        static public int[,] findInverseMatrix_2x2(int[,] m)
        {
            int[,] m2m = new int[2, 2];
            m2m[0, 0] = m[1, 1];
            m2m[0, 1] = -1 * m[0, 1];
            m2m[1, 0] = -1 * m[1, 0];
            m2m[1, 1] = m[0, 0];


            //find determenant of the matrix
            double det = Find_determinant(m, 2) % 26;
            while (det < 0)
                det += 26;
            int x = Multiplicative_Inverse((int)det, 26);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    m2m[i, j] = x * m2m[i, j];
                    m2m[i, j] %= 26;

                }

            }
            return m2m;
        }
        static public double Find_determinant(int[,] A, int N)
        {
            double res;
            if (N == 1)
                res = A[0, 0];
            else if (N == 2)
            {
                res = A[0, 0] * A[1, 1] - A[1, 0] * A[0, 1];
            }
            else
            {
                res = 0;
                for (int j1 = 0; j1 < N; j1++)
                {
                    int[,] m = new int[N - 1, N - 1];
                    for (int i = 1; i < N; i++)
                    {
                        int j2 = 0;
                        for (int j = 0; j < N; j++)
                        {
                            if (j == j1)
                                continue;
                            m[i - 1, j2] = A[i, j];
                            j2++;
                        }
                    }
                    res += Math.Pow(-1.0, 1.0 + j1 + 1.0) * A[0, j1] * Find_determinant(m, N - 1);
                }
            }
            return res;
            //double result;
            //if (size == 1)
            //    result = matrix[0, 0];
            //else if (size == 2)
            //{
            //    result = matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
            //}
            //else
            //{
            //    result = 0;
            //    for (int i = 0; i < size; i++)
            //    {
            //        int[,] m = new int[size - 1, size - 1];
            //        for (int j = 1; j < size; j++)
            //        {
            //            int index = 0;
            //            for (int k = 0; k < size; k++)
            //            {
            //                if (k == i)
            //                    continue;
            //                m[j - 1, index] = matrix[j, k];
            //                index++;
            //            }
            //        }
            //        result += Math.Pow(-1.0, 1.0 + i + 1.0) * matrix[0, i] * Find_determinant(m, size - 1);
            //    }
            //}
            //return result;
        }
        static public int Multiplicative_Inverse(int det, int B3)
        {
            for (int i = 1; i < 27; i++)
            {
                int c = i * det % B3;
                if (c == 1)
                {
                    return i;
                }
            }
            return 0;
        }
        public int GCD(int p, int q)
        {
            if (q == 0)
            {
                return p;
            }

            int r = p % q;

            return GCD(q, r);
        }



        #region bouns
        public string Encrypt(string plainText, string key)
        {
            char[] Alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            List<int> plain = new List<int>(plainText.Length);
            List<int> Key1 = new List<int>(plainText.Length);
            List<int> cipher = new List<int>(plainText.Length);

            for (int i = 0; i < plainText. Length; i++)
            {
                for (int j = 0; j < Alpha. Length; j++)
                {
                    if (Alpha[j] == plainText[i]) plain.Add(j);
                }
            }

            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (Alpha[j] == key[i]) Key1.Add(j);
                }
            }

            cipher = Encrypt(plain, Key1);
            string ciphertext = "";

            for (int i = 0; i < cipher.Count; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (j == cipher[i]) ciphertext += Alpha[j];
                }
            }

            return ciphertext;
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();

            char[] Alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            List<int> cipher = new List<int>(cipherText.Length);
            List<int> Key1 = new List<int>(key.Length);
            List<int> plain = new List<int>(cipherText.Length);

            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (Alpha[j] == cipherText[i]) cipher.Add(j);
                }
            }

            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (Alpha[j] == key[i]) Key1.Add(j);
                }
            }

            plain = Decrypt(cipher, Key1);
            string plaintext = "";

            for (int i = 0; i < plain.Count; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (j == plain[i]) plaintext += Alpha[j];
                }
            }

            return plaintext;
        }

        public string Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            plainText  = plainText.ToLower();

            char[] Alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            List<int> cipher = new List<int>(cipherText.Length);
            List<int> Key1 = new List<int>(4);
            List<int> plain = new List<int>(plainText.Length);

            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (Alpha[j] == cipherText[i]) cipher.Add(j);
                }
            }

            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (Alpha[j] == plainText[i]) plain.Add(j);
                }
            }

            Key1 = Analyse(plain, cipher);
            string key = "";

            for (int i = 0; i < Key1.Count; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (j == Key1[i]) key += Alpha[j];
                }
            }
            return key;


        }

        public string Analyse3By3Key(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();

            char[] Alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            List<int> cipher = new List<int>(cipherText.Length);
            List<int> Key1 = new List<int>(4);
            List<int> plain = new List<int>(plainText.Length);

            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (Alpha[j] == cipherText[i]) cipher.Add(j);
                }
            }

            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (Alpha[j] == plainText[i]) plain.Add(j);
                }
            }

            Key1 = Analyse3By3Key(plain, cipher);
            string key = "";

            for (int i = 0; i < Key1.Count; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    if (j == Key1[i]) key += Alpha[j];
                }
            }
            return key;
        }
        #endregion

    }
}
