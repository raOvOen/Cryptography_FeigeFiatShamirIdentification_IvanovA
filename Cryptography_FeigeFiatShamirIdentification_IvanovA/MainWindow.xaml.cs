using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cryptography_FeigeFiatShamirIdentification_IvanovA
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private BigInteger[] s = new BigInteger[50];
        private bool[] b = new bool[50];
        private bool[] vector = new bool[50];
        private bool[] resultAnswer = new bool[50];
        private BigInteger[] v = new BigInteger[50];
        private int k = 3;
        private int t = 2;
        private int t0 = 2;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBoxInputP.Text = GenerateBigInt(BigInteger.Parse(TextBoxMaxNum.Text), Convert.ToInt32(TextBoxAcc.Text)).ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TextBoxInputQ.Text = GenerateBigInt(BigInteger.Parse(TextBoxMaxNum.Text), Convert.ToInt32(TextBoxAcc.Text)).ToString();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BigInteger n = CountN(BigInteger.Parse(TextBoxInputP.Text), BigInteger.Parse(TextBoxInputQ.Text));
            TextBoxInputN.Text = n.ToString();
        }
        
        private BigInteger CountN(BigInteger p, BigInteger q)
        {
            return BigInteger.Multiply(p,q);
        }
        private BigInteger GenerateBigInt(BigInteger Upper, int acc)
        {
            BigInteger result = RandomIntegerBelow(BigInteger.Pow(10, (int)Upper));
            while (MillerRabinTest(result, acc) == false)
            {
                result++;
            }
            return result;
        }
        private BigInteger RandomIntegerBelow(BigInteger N)
        {
            byte[] bytes = N.ToByteArray();
            BigInteger R;
            Random random = new Random();
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                R = new BigInteger(bytes);
            } while (R >= N);
            return R;
        }
        private bool MillerRabinTest(BigInteger n, int k)
        {
            if (n == 2 || n == 3)
                return true;
            if (n < 2 || n % 2 == 0)
                return false;
            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }
            for (int i = 0; i < k; i++)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] _a = new byte[n.ToByteArray().LongLength];
                BigInteger a;
                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);
                BigInteger x = BigInteger.ModPow(a, t, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            return true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            k = Convert.ToInt32(TextBoxAmountk.Text);
            t = Convert.ToInt32(TextBoxRoundst.Text);
            BigInteger n = BigInteger.Parse(TextBoxInputN.Text);
            Random rnd = new Random();
            for(int i = 0; i < k; i++)
            {
                do
                {
                    s[i] = RandomIntegerBelow(n - 1);
                }
                while ((i > 0 && (s[i - 1] == s[i])) || !IsCoprimeTest(s[i],n));
                //rnd.Next(3000);
                //RandomIntegerBelow(n - 1);
                int bi = rnd.Next(2);
                if (bi == 1)
                {
                    b[i] = true;
                    //v[i] = ((-1) * (s[i] * s[i])) % n;
                }
                else { 
                    b[i] = false;
                    //v[i] = (s[i] * s[i]) % n;
                }
                //MessageBox.Show(bi.ToString());
                //v[i] = (BigInteger.Pow((-1), bi) * (s[i] * s[i]) % n);
                v[i] = mod(BigInteger.Pow((-1), bi) * Euc(s[i]*s[i],n),n);
                //v[i] = BigInteger.ModPow((s[i] * s[i]), n - 2, n);
            }
            //MessageBox.Show(mod(-1*(21*21), n).ToString());
        }
        private static BigInteger Euc(BigInteger a, BigInteger n) // Расширенный алгоритм Евклида, упрощённый для шифра
        {
            BigInteger p = 1, r = 0, n_0 = n;
            while (a != 0 && n != 0)
            {
                if (a >= n)
                {
                    p -= r * (a / n);
                    a %= n;
                }
                else
                {
                    r -= p * (n / a);
                    n %= a;
                }
            }
            if (p < 0)
                p += n_0;
            if (r < 0)
                r += n_0;
            if (a != 0)
                return p;
            else
                return r;
        }
        private bool IsCoprimeTest(BigInteger num1, BigInteger num2)
        {
            BigInteger temp = 0;
            int q = 0;
            bool res = false;
            while (q == 0)
            {
                if (num1 < num2)
                {
                    temp = num1;
                    num1 = num2;
                    num2 = temp;
                }
                //MessageBox.Show(num1.ToString() + "||" + num2.ToString());
                num1 = num1 % num2;
                //MessageBox.Show(num1.ToString());
                if (num1 == 1) { q = 1; res = true; };
                if (num1 == 0) { q = 1; res = false; };
            }
            return res;
        }
        private BigInteger mod(BigInteger a, BigInteger b)
        {
            BigInteger r = a % b;
            return r < 0 ? r + b : r;
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string temp = "";
            for(int i=0; i < k; i++)
            {
                temp += s[i].ToString() + "\n";
            }
            MessageBox.Show(temp, "Si");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string temp = "";
            for (int i = 0; i < k; i++)
            {
                temp += b[i].ToString() + "\n";
            }
            MessageBox.Show(temp, "Bi");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string temp = "";
            for (int i = 0; i < k; i++)
            {
                temp += v[i].ToString() + "\n";
            }
            MessageBox.Show(temp, "Vi");
        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            BigInteger r = RandomIntegerBelow(BigInteger.Parse(TextBoxInputN.Text) - 1);
            TextBoxInputR.Text = r.ToString();
            BigInteger n = BigInteger.Parse(TextBoxInputN.Text);
            Random rnd = new Random();
            int bit = rnd.Next(2);
            TextBoxInputBit.Text = bit.ToString();
            BigInteger x = mod(BigInteger.Pow((-1), bit) * r*r, n);
            TextBoxInputX.Text = x.ToString();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < k; i++)
            {
                if (rnd.Next(2) == 1) 
                {
                    vector[i] = true;
                }
                else
                {
                    vector[i] = false;
                }
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            string temp = "";
            for (int i = 0; i < k; i++)
            {
                temp += vector[i].ToString() + "\n";
            }
            MessageBox.Show(temp, "Bi");
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            BigInteger r = BigInteger.Parse(TextBoxInputR.Text);
            BigInteger n = BigInteger.Parse(TextBoxInputN.Text);
            TextBoxInputY.Text = CountY(r, n).ToString();
        }
        private BigInteger CountY(BigInteger r, BigInteger n)
        {
            BigInteger result = 1;
            for(int i=0; i< k; i++)
            {
                if(vector[i]==true)
                {
                    result *= s[i];
                }
            }
            result *= r;
            return (result%n);
        }
        private BigInteger CountZ(BigInteger y, BigInteger n)
        {
            BigInteger result = 1;
            for (int i = 0; i < k; i++)
            {
                if (vector[i] == true)
                {
                    result *= v[i];
                }
            }
            result *= (y*y);
            return (result % n);
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            BigInteger n = BigInteger.Parse(TextBoxInputN.Text);
            string result = "";
            //result += CountZ(BigInteger.Parse(TextBoxInputY.Text), n).ToString() + "||" + BigInteger.Abs(BigInteger.Parse(TextBoxInputX.Text)).ToString() + "\n";
            if (CountZ(BigInteger.Parse(TextBoxInputY.Text),n) == BigInteger.Parse(TextBoxInputX.Text) || CountZ(BigInteger.Parse(TextBoxInputY.Text), n) == (-1)*BigInteger.Parse(TextBoxInputX.Text) + n)
            {
                result += "Success!";
                resultAnswer[t - t0] = true;
            }
            else
            {
                result += "Fail!";
                resultAnswer[t - t0] = false;
            }
            t0--;
            if (t0 > 0) { result += "\n" + t0.ToString() + " more times to finish identification!"; }
            else
            {
                ButtonResultAfterTRounds.IsEnabled = true;
            }
            LabelStatus.Content = "Status: [" + (t - t0).ToString() + "/" + t.ToString() + "]";
            if (CheckBoxMoreInfo.IsChecked == true)
            {
                MessageBox.Show(result, "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < t; i++)
            {
                if (resultAnswer[i] == false) { MessageBox.Show("Fail identification!", "Finish", MessageBoxButton.OK, MessageBoxImage.Error); ButtonResultAfterTRounds.IsEnabled = false; LabelStatus.Content = "Status: None"; goto exitglobal; }
            }
            ButtonResultAfterTRounds.IsEnabled = false;
            MessageBox.Show("Sucess identification!", "Finish", MessageBoxButton.OK, MessageBoxImage.Information);
            LabelStatus.Content = "Status: None";
        exitglobal:;
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            t = Convert.ToInt32(TextBoxRoundst.Text);
            t0 = t;
            if(RadioButtonAuto.IsChecked == true)
            {
                Button_Click_3(null, null);
                for(int i=0; i<t; i++)
                {
                    //Button_Click_3(null, null);
                    Button_Click_8(null, null);
                    Button_Click_7(null, null);
                    Button_Click_10(null, null);
                    Button_Click_11(null, null);
                }
                Button_Click_12(null, null);
            }
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Realization of Fiege - Fiat - Shamir identification algorithm by IvanovA (2021).\n\n- How to use it? \n\nThis algorithm is able to work in a short time with numbers up to 2^1024 (10^309). It is recommended to set the number of checks (Acc) equal to the square root of the specified number (For 10^309 - 1024). This is necessary for the correct operation of the simplicity test, which is made according to the Miller-Rabin algorithm. " + "\n" +
            "For it's work just follow the necessary steps indicated on the buttons (Number).\nYou may set necessary count of t rounds and necessary k(eys) amount. \nYou can also choose working mode. In Handle mode you need to do all the steps by yourself, in auto, after pressing '4) Start' button, program will make process of identificaton by itself. " +
            "\nMoreover, you can uncheck checkbox 'More information' to get less information about working process." +
            "\nIf you chose the number of rounds T greater than one, do not forget to re-pass all the steps starting from the 6th point to finally unlock 'Final) Result after t rounds' button."
            , "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
