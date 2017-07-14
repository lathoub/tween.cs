using System;

namespace Tween
{
    public static class Easing
    {
        public class Linear
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double None(double k)
            {
                return k;
            }
        }

        public class Quadratic
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                return k * k;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                return k * (2 - k);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                if ((k *= 2) < 1)
                {
                    return 0.5 * k * k;
                }

                return -0.5 * (--k * (k - 2) - 1);
            }
        }

        public class Cubic
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                return k * k * k;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                return --k * k * k + 1;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                if ((k *= 2) < 1)
                {
                    return 0.5 * k * k * k;
                }

                return 0.5 * ((k -= 2) * k * k + 2);
            }
        }

        public class Quartic
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                return k * k * k * k;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                return 1 - (--k * k * k * k);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                if ((k *= 2) < 1)
                {
                    return 0.5 * k * k * k * k;
                }

                return -0.5 * ((k -= 2) * k * k * k - 2);
            }
        }

        public class Quintic
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                return k * k * k * k * k;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                return --k * k * k * k * k + 1;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                if ((k *= 2) < 1)
                {
                    return 0.5 * k * k * k * k * k;
                }

                return 0.5 * ((k -= 2) * k * k * k * k + 2);
            }
        }

        public class Sinusoidal
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                return 1 - Math.Cos(k * Math.PI / 2.0);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                return Math.Sin(k * Math.PI / 2);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                return 0.5 * (1 - Math.Cos(Math.PI * k));
            }
        }

        public class Expotential
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                return (Math.Abs(k) < double.Epsilon) ? 0 : Math.Pow(1024, k - 1);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                return (Math.Abs(k - 1) < double.Epsilon) ? 1 : 1 - Math.Pow(2, -10 * k);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                if (Math.Abs(k) < double.Epsilon)
                {
                    return 0;
                }

                if (Math.Abs(k - 1) < double.Epsilon)
                {
                    return 1;
                }

                if ((k *= 2) < 1)
                {
                    return 0.5 * Math.Pow(1024, k - 1);
                }

                return 0.5 * (-Math.Pow(2, -10 * (k - 1)) + 2);
            }
        }

        public class Circular
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                return 1 - Math.Sqrt(1 - k * k);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                return Math.Sqrt(1 - (--k * k));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                if ((k *= 2) < 1)
                {
                    return -0.5 * (Math.Sqrt(1 - k * k) - 1);
                }

                return 0.5 * (Math.Sqrt(1 - (k -= 2) * k) + 1);
            }
        }

        public class Elastic
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                if (Math.Abs(k) < double.Epsilon) // k == 0
                {
                    return 0;
                }

                if (Math.Abs(k - 1) < double.Epsilon) // k == 1
                {
                    return 1;
                }

                return -Math.Pow(2.0, 10.0 * (k - 1)) * Math.Sin((k - 1.1) * 5.0 * Math.PI);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                if (Math.Abs(k) < double.Epsilon) // k == 0
                {
                    return 0;
                }

                if (Math.Abs(k - 1) < double.Epsilon) // k == 1
                {
                    return 1;
                }

                return Math.Pow(2, -10 * k) * Math.Sin((k - 0.1) * 5 * Math.PI) + 1;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                if (Math.Abs(k) < double.Epsilon)
                {
                    return 0;
                }

                if (Math.Abs(k - 1) < double.Epsilon)
                {
                    return 1;
                }

                k *= 2;

                if (k < 1)
                {
                    return -0.5 * Math.Pow(2, 10 * (k - 1)) * Math.Sin((k - 1.1) * 5 * Math.PI);
                }

                return 0.5 * Math.Pow(2, -10 * (k - 1)) * Math.Sin((k - 1.1) * 5 * Math.PI) + 1;
            }
        }

        public class Back
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                const double s = 1.70158;

                return k * k * ((s + 1) * k - s);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                const double s = 1.70158;

                return --k * k * ((s + 1) * k + s) + 1;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                const double s = 1.70158 * 1.525;

                if ((k *= 2) < 1)
                {
                    return 0.5 * (k * k * ((s + 1) * k - s));
                }

                return 0.5 * ((k -= 2) * k * ((s + 1) * k + s) + 2);
            }
        }

        public class Bounce
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double In(double k)
            {
                return 1 - Out(1 - k);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double Out(double k)
            {
                if (k < (1 / 2.75))
                {
                    return 7.5625 * k * k;
                }
                else if (k < (2 / 2.75f))
                {
                    return 7.5625 * (k -= (1.5 / 2.75)) * k + 0.75;
                }
                else if (k < (2.5 / 2.75))
                {
                    return 7.5625 * (k -= (2.25 / 2.75)) * k + 0.9375;
                }
                else
                {
                    return 7.5625 * (k -= (2.625 / 2.75)) * k + 0.984375;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="k"></param>
            /// <returns></returns>
            public static double InOut(double k)
            {
                if (k < 0.5)
                {
                    return In(k * 2) * 0.5;
                }

                return Out(k * 2 - 1) * 0.5 + 0.5;
            }
        }
    }
}
