using System;
using System.Collections.Generic;
using static System.Math;

using System.Windows;

// Translation of the code in the wonderful explanation on
// http://preshing.com/20110831/penrose-tiling-explained/

namespace Penrose
{
    public enum TriangleType
    {
        Red, Blue
    }

    public class Triangle
    {
        public Vector A { get; }
        public Vector B { get; }
        public Vector C { get; }
        public TriangleType Type { get; }

        public Triangle(Vector a, Vector b, Vector c, TriangleType t)
        {
            this.A = a; this.B = b; this.C = c; this.Type = t;
        }
    }

    public class PenroseTilingGenerator
    {
        private static readonly double phi = (1 + Sqrt(5)) / 2;

        public IEnumerable<Triangle> Subdivide(IEnumerable<Triangle> triangles)
        {
            foreach (var t in triangles)
                foreach (var t2 in SubdivideTriangle(t))
                    yield return t2;
        }

        public IEnumerable<Triangle> SubdivideTriangle(Triangle t)
        {
            if (t.Type == TriangleType.Red)
            {
                var P = t.A + (t.B - t.A) / phi;
                yield return new Triangle(t.C, P, t.B, TriangleType.Red);
                yield return new Triangle(P, t.C, t.A, TriangleType.Blue);
            }
            else if (t.Type == TriangleType.Blue)
            {
                var Q = t.B + (t.A - t.B) / phi;
                var R = t.B + (t.C - t.B) / phi;

                yield return new Triangle(R, t.C, t.A, TriangleType.Blue);
                yield return new Triangle(Q, R, t.B, TriangleType.Blue);
                yield return new Triangle(R, Q, t.A, TriangleType.Red);
            }
        } 
    }
}
