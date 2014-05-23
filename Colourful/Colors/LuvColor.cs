﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using System.Globalization;

namespace Colourful
{
    /// <summary>
    /// CIE L*u*v* (1976) color
    /// </summary>
    public class LuvColor : IColorVector
    {
        /// <summary>
        /// D65 standard illuminant.
        /// Used when reference white is not specified explicitly.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly XYZColor DefaultWhitePoint = Illuminants.D65;

        #region Constructor

        /// <param name="l">L* (lightness)</param>
        /// <param name="u">a*</param>
        /// <param name="v">b*</param>
        /// <remarks>Uses <see cref="DefaultWhitePoint"/> as white point.</remarks>
        public LuvColor(double l, double u, double v) : this(l, u, v, DefaultWhitePoint)
        {
        }

        /// <param name="l">L* (lightness)</param>
        /// <param name="u">a*</param>
        /// <param name="v">b*</param>
        /// <param name="whitePoint">Reference white (see <see cref="Illuminants"/>)</param>
        public LuvColor(double l, double u, double v, XYZColor whitePoint)
        {
            L = l;
            this.u = u;
            this.v = v;
            WhitePoint = whitePoint;
        }

        #endregion

        #region Channels

        /// <summary>
        /// L* (lightness)
        /// </summary>
        /// <remarks>
        /// Ranges from 0 to 100.
        /// </remarks>
        public double L { get; private set; }

        /// <summary>
        /// u*
        /// </summary>
        public double u { get; private set; }

        /// <summary>
        /// v*
        /// </summary>
        public double v { get; private set; }

        /// <remarks><see cref="Illuminants"/></remarks>
        public XYZColor WhitePoint { get; private set; }

        /// <summary>
        /// <see cref="IColorVector"/>
        /// </summary>
        public Vector<double> Vector
        {
            get { return DenseVector.OfEnumerable(new[] { L, u, v }); }
        }

        #endregion

        #region Equality

        public bool Equals(LuvColor other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return L.Equals(other.L) && u.Equals(other.u) && v.Equals(other.v);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LuvColor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = L.GetHashCode();
                hashCode = (hashCode * 397) ^ u.GetHashCode();
                hashCode = (hashCode * 397) ^ v.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(LuvColor left, LuvColor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LuvColor left, LuvColor right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Luv [L={0:0.##}, u={1:0.##}, v={2:0.##}]", L, u, v);
        }

        #endregion
    }
}