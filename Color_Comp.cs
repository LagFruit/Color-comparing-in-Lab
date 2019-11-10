using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorComparison
{
    class ColorComp
    {

        private static double[] RGBtoCIELab(double[] RGB) // RGB 0-255 to CIE Lab
        {
            // ------------ RGB to XYZ ---------------

            //sR, sG and sB (Standard RGB) input range = 0 ÷ 255
            double sR = RGB[0];
            double sG = RGB[1];
            double sB = RGB[2];

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

            double[] XYZ = new double[3]{X, Y, Z}; // delete*************************************************

            foreach (var item in XYZ) // delete**************************************************************
            {
                Console.WriteLine(item.ToString());
            }

            // ------------- XYZ to CIE L*ab ------------------

            //Reference-X, Y and Z refer to specific illuminants and observers. Assuming Reference-X/Y/Z = 1.000
            //Common reference values are available below in this same page.
            //(For simplicity here, use equal energy - 100 each)

            double Reference_X = 1.000f;
            double Reference_Y = 1.000f;
            double Reference_Z = 1.000f;

            double var_X = X / Reference_X;
            double var_Y = Y / Reference_Y;
            double var_Z = Z / Reference_Z;

            if (var_X > 0.008856f) var_X = Math.Pow(var_X , 0.33333f);
            else var_X = (7.787f * var_X) + (16f / 116f);
            if (var_Y > 0.008856f) var_Y = Math.Pow(var_Y , 0.33333f);
            else var_Y = (7.787f * var_Y) + (16f / 116f);
            if (var_Z > 0.008856f) var_Z = Math.Pow(var_Z , 0.33333f);
            else var_Z = (7.787f * var_Z) + (16f / 116f);

            Console.WriteLine(var_X.ToString());
            Console.WriteLine(var_Y.ToString());
            Console.WriteLine(var_Z.ToString());

            double CIE_L = (116f * var_Y) - 16f;
            double CIE_a = 500f * (var_X - var_Y);
            double CIE_b = 200f * (var_Y - var_Z);

            double[] Lab = new double[3] { CIE_L, CIE_a, CIE_b }; // delete*************************************************

            foreach (var item in Lab) // delete**************************************************************
            {
                Console.WriteLine(item.ToString());
            }

            return (Lab);

        }

        // Radians to Degrees
        public static double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }

        // Degress to Radians
        public static double ConvertDegreesToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        // Function returns CIE-H° value
        public double CieLab2Hue(double var_a, double var_b)
        {
            double var_bias = 0;

            if (var_a >= 0 && var_b == 0) return 0;
            if (var_a < 0 && var_b == 0) return 180;
            if (var_a == 0 && var_b > 0) return 90;
            if (var_a == 0 && var_b < 0) return 270;
            if (var_a > 0 && var_b > 0) var_bias = 0;
            if (var_a < 0) var_bias = 180;
            if (var_a > 0 && var_b < 0) var_bias = 360;

            double r2d = ConvertRadiansToDegrees(Math.Atan(var_a / var_b));

            return (r2d + var_bias);
        }

        public double DeltaE00(double[] CIELab1, double[] CIELab2, double[] WHT) // Delta E 2000
        {
            // CIE-L*ab of Color#1
            double CIE_L1 = CIELab1[0];
            double CIE_a1 = CIELab1[1];
            double CIE_b1 = CIELab1[2];

            // CIE-L*ab of Color#2
            double CIE_L2 = CIELab2[0];
            double CIE_a2 = CIELab2[1];
            double CIE_b2 = CIELab2[2];

            // Wheight factors
            double WHT_L = WHT[0];
            double WHT_C = WHT[1];
            double WHT_H = WHT[2];

            double xC1 = Math.Pow((CIE_a1 * CIE_a1 + CIE_b1 * CIE_b1) , 0.5f);
            double xC2 = Math.Pow((CIE_a2 * CIE_a2 + CIE_b2 * CIE_b2) , 0.5f);
            double xCX = (xC1 + xC2) / 2f;
            double xGX = 0.5f * (1f - Math.Pow( (Math.Pow(xCX , 7f) / ( Math.Pow(xCX , 7f) + Math.Pow(25 , 7f) ) ) , 0.5f));

            double xNN = (1f + xGX) * CIE_a1;
            xC1 = Math.Pow((xNN * xNN + CIE_b1 * CIE_b1) , 0.5);
            double xH1 = CieLab2Hue(xNN, CIE_b1);

            xNN = (1 + xGX) * CIE_a2;
            xC2 = Math.Pow((xNN * xNN + CIE_b2 * CIE_b2), 0.5);
            double xH2 = CieLab2Hue(xNN, CIE_b2);

            double xDL = CIE_L2 - CIE_L1;
            double xDC = xC2 - xC1;

            double xDH;

            if ((xC1 * xC2) == 0)
            {
                xDH = 0;
}
            else
            {
                xNN = Math.Round(xH2 - xH1, 12);

                if (Math.Abs(xNN) <= 180)
                {
                    xDH = xH2 - xH1;
   }
                else
                {
                    if (xNN > 180) xDH = xH2 - xH1 - 360;
                    else xDH = xH2 - xH1 + 360;
                }
            }

            xDH = 2 * Math.Pow((xC1 * xC2) * Math.Sin(xDH / 2) , 0.5);
            double xLX = (CIE_L1 + CIE_L2) / 2;
            double xCY = (xC1 + xC2) / 2;

            double xHX;

            if ((xC1 * xC2) == 0)
            {
                xHX = xH1 + xH2;
}
            else
            {
                xNN = Math.Abs(Math.Round(xH1 - xH2, 12));
               if (xNN > 180)
               {
                    if ((xH2 + xH1) < 360) xHX = xH1 + xH2 + 360;
                    else xHX = xH1 + xH2 - 360;
               }
               else
               {
                    xHX = xH1 + xH2;
               }
               xHX /= 2;
            }

            double xTX = 1 - 0.17 * Math.Cos(xHX - 30) + 0.24 * Math.Cos(ConvertDegreesToRadians(2 * xHX)) + 0.32 * Math.Cos(ConvertDegreesToRadians(3 * xHX + 6)) - 0.20 * Math.Cos(4 * xHX - 63);
            double xPH = 30 * Math.Exp(-((xHX - 275) / 25) * ((xHX - 275) / 25));
            double xRC = 2 * Math.Pow((Math.Pow(xCY , 7) / (Math.Pow(xCY , 7) + Math.Pow(25 , 7))) , 0.5);
            double xSL = 1 + ((0.015 * ((xLX - 50) * (xLX - 50))) / Math.Pow((20 + ((xLX - 50) * (xLX - 50))) , 0.5));


            double xSC = 1 + 0.045 * xCY;
            double xSH = 1 + 0.015 * xCY * xTX;
            double xRT = -Math.Sin(ConvertDegreesToRadians(2 * xPH)) * xRC;

            xDL = xDL / (WHT_L * xSL);
            xDC = xDC / (WHT_C * xSC);
            xDH = xDH / (WHT_H * xSH);


            double delta_E00 = Math.Pow((Math.Pow(xDL , 2) + Math.Pow(xDC , 2) + Math.Pow(xDH , 2) + xRT * xDC * xDH) , 0.5);


            return delta_E00;
        }


        static void Main(string[] args)
        {
            double[] RGB = new double[3] { 127.5, 127.5, 127.5 };
            RGBtoCIELab(RGB);
            Console.ReadKey();
        }

    }
}
