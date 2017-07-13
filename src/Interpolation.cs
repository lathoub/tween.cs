using System;

namespace Tween
{
    public static class Interpolation
    {
        private delegate int Interpolator1(int n);
        private delegate int Interpolator2(int n, int i);
        private delegate double Interpolator3(double p0, double p1, double t);
        private delegate double Interpolator5(double p0, double p1, double p2, double p3, double t);

        public static double Linear(double[] v, double k)
        {
            var m = v.Length - 1;
            var f = m * k;
            var i = (int) Math.Floor(f);
            Interpolator3 fn = Utils.Linear;

            if (k < 0)
            {
                return fn(v[0], v[1], f);
            }

            if (k > 1)
            {
                return fn(v[m], v[m - 1], m - f);
            }

            return fn(v[i], v[i + 1 > m ? m : i + 1], f - i);
        }

        public static double Bezier(double[] v, double k)
        {
            var b = 0.0;
            var n = v.Length - 1;
            Interpolator2 bn = Utils.Bernstein;

            for (var i = 0; i <= n; i++)
            {
                b += Math.Pow(1 - k, n - i) * Math.Pow(k, i) * v[i] * bn(n, i);
            }

            return b;
        }

        public static double CatmullRom(double[] v, double k)
        {
            var m = v.Length - 1;
            var f = m * k;
            var i = (int)Math.Floor(f);
            Interpolator5 fn = Utils.CatmullRom;

            if (Math.Abs(v[0] - v[m]) < double.Epsilon)
            {
                if (k < 0)
                {
                    i = (int)Math.Floor(f = m * (1 + k));
                }

                return fn(v[(i - 1 + m) % m], v[i], v[(i + 1) % m], v[(i + 2) % m], f - i);
            }
            else
            {
                if (k < 0)
                {
                    return v[0] - (fn(v[0], v[0], v[1], v[1], -f) - v[0]);
                }

                if (k > 1)
                {
                    return v[m] - (fn(v[m], v[m], v[m - 1], v[m - 1], f - m) - v[m]);
                }

                return fn(v[i != 0 ? i - 1 : 0], v[i], v[m < i + 1 ? m : i + 1], v[m < i + 2 ? m : i + 2], f - i);
            }
        }

        private static class Utils
        {
            internal static double Linear(double p0, double p1, double t)
            {
                return (p1 - p0) * t + p0;
            }

            internal static int Bernstein(int n, int i)
            {
                Interpolator1 fc = Factorial;
                return fc(n) / fc(i) / fc(n - i);
            }

            private static int Factorial(int n)
            {
                if (n <= 1)
                    return 1;
                return n * Factorial(n - 1);
            }

            internal static double CatmullRom(double p0, double p1, double p2, double p3, double t)
            {
                var v0 = (p2 - p0) * 0.5;
                var v1 = (p3 - p1) * 0.5;
                var t2 = t * t;
                var t3 = t * t2;

                return (2 * p1 - 2 * p2 + v0 + v1) * t3 + (-3 * p1 + 3 * p2 - 2 * v0 - v1) * t2 + v0 * t + p1;
            }
        }
    }
}
