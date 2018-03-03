﻿using System;

namespace Colourful.Implementation.Conversion
{
    /// <summary>
    /// Converts from <see cref="LuvColor" /> to <see cref="XYZColor" />.
    /// </summary>
    public class LuvToXYZConverter : IColorConversion<LuvColor, XYZColor>
    {
        /// <summary>
        /// Converts from <see cref="LuvColor" /> to <see cref="XYZColor" />.
        /// </summary>
        public XYZColor Convert(LuvColor input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            // conversion algorithm described here: http://www.brucelindbloom.com/index.html?Eqn_Luv_to_XYZ.html
            double L = input.L, u = input.u, v = input.v;

            var u0 = Compute_u0(input.WhitePoint);
            var v0 = Compute_v0(input.WhitePoint);

            var Y = L > CIEConstants.Kappa * CIEConstants.Epsilon
                ? MathUtils.Pow3((L + 16) / 116)
                : L / CIEConstants.Kappa;

            var a = (52 * L / (u + 13 * L * u0) - 1) / 3;
            var b = -5 * Y;
            var c = -1 / 3d;
            var d = Y * (39 * L / (v + 13 * L * v0) - 5);

            var X = (d - b) / (a - c);
            var Z = X * a + b;

            if (double.IsNaN(X) || X < 0)
                X = 0;

            if (double.IsNaN(Y) || Y < 0)
                Y = 0;

            if (double.IsNaN(Z) || Z < 0)
                Z = 0;

            var result = new XYZColor(X, Y, Z);
            return result;
        }

        private static double Compute_u0(XYZColor input)
        {
            return 4 * input.X / (input.X + 15 * input.Y + 3 * input.Z);
        }

        private static double Compute_v0(XYZColor input)
        {
            return 9 * input.Y / (input.X + 15 * input.Y + 3 * input.Z);
        }

        #region Overrides

        /// <inheritdoc cref="object" />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return true;
        }

        /// <inheritdoc cref="object" />
        public override int GetHashCode()
        {
            return 1;
        }

        /// <inheritdoc cref="object" />
        public static bool operator ==(LuvToXYZConverter left, LuvToXYZConverter right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc cref="object" />
        public static bool operator !=(LuvToXYZConverter left, LuvToXYZConverter right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}