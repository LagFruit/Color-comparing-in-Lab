using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorComparison
{
    class Program
    {

        private static double[] RGBtoXYZ(int[] RGB) // RGB is int[3] array for inputs
        {
            //sR, sG and sB (Standard RGB) input range = 0 ÷ 255
            int sR = RGB[0];
            int sG = RGB[1];
            int sB = RGB[2];

            //double[] XYZ = new double[3]; // XYZ is float[3] array for outputs

            double var_R = sR / 255f;
            double var_G = sG / 255f;
            double var_B = sB / 255f;

            if (var_R > 0.04045f) var_R = Math.Pow(((var_R + 0.055f) / 1.055f), 2.4f);
            else var_R = var_R / 12.92f;
            if (var_G > 0.04045f) var_G = Math.Pow(((var_G + 0.055f) / 1.055f), 2.4f);
            else var_G = var_G / 12.92f;
            if (var_B > 0.04045f) var_B = Math.Pow(((var_B + 0.055f) / 1.055f), 2.4f);
            else var_B = var_B / 12.92f;

            var_R = var_R * 100f;
            var_G = var_G * 100f;
            var_B = var_B * 100f;

            double X = var_R * 0.4124f + var_G * 0.3576f + var_B * 0.1805f;
            double Y = var_R * 0.2126f + var_G * 0.7152f + var_B * 0.0722f;
            double Z = var_R * 0.0193f + var_G * 0.1192f + var_B * 0.9505f;

            double[] XYZ = new double[3]{X, Y, Z};

            foreach (var item in XYZ)
            {
                Console.WriteLine(item.ToString());
            }

            return (XYZ);
        }

        static void Main(string[] args)
        {
            int[] RGB = new int[3] { 100, 100, 100 };
            RGBtoXYZ(RGB);
            Console.ReadKey();
        }

    }
}
