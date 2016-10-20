using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_Stereometry.Classes
{
    public static class HitTest
    {
        private static Point3D[] hitTestPoints = new Point3D[]
        {
            new Point3D(0,10,10),
            new Point3D(0,10,-10),
            new Point3D(0,-10,-10),
            new Point3D(0,-10,10)
        };

        private static Vector3D NormalVector() //static
        {
            Point3D p1 = hitTestPoints[0];
            Point3D p2 = hitTestPoints[1];
            Point3D p3 = hitTestPoints[2];
            Vector3D Vec1 = new Vector3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
            Vector3D Vec2 = new Vector3D(p3.X - p2.X, p3.Y - p2.Y, p3.Z - p2.Z);
            Vector3D Normal = Vector3D.CrossProduct(Vec2, Vec1);//new Vector3D(Vec1.Y * Vec2.Z - Vec1.Z * Vec2.Y, Vec1.Z * Vec2.X - Vec1.X * Vec2.Z, Vec1.X * Vec2.Y - Vec1.Y * Vec2.X);
            return Normal;
        }

        public static void SetPosition(double x)
        {
            for (int i = 0; i < 4; i++)
            {
                hitTestPoints[i].X = x;
            }
        }

        public static BB_Point3D Pointof3DIntersection(BB_Line3D Line)
        {
            double a = NormalVector().X;
            double b = NormalVector().Y;
            double c = NormalVector().Z;
            Point3D point = hitTestPoints[0];
            double d = -(a * point.X + b * point.Y + c * point.Z);
            Point3D positionVec1 = Database.allPoints[Line.Points[0]].toPoint3D();//position vector
            Point3D positionVec2 = Database.allPoints[Line.Points[1]].toPoint3D(); //2nd position vector
            Point3D directionVector = new Point3D(positionVec2.X - positionVec1.X, positionVec2.Y - positionVec1.Y, positionVec2.Z - positionVec1.Z);
            double t = (-d - positionVec1.X * a - positionVec1.Y * b - positionVec1.Z * c) / (directionVector.X * a + directionVector.Y * b + directionVector.Z * c); //t - the coef of the LineEq when it intersects the plane
            BB_Point3D PointofIntersection = new BB_Point3D(positionVec1.X + directionVector.X * t, positionVec1.Y + directionVector.Y * t, positionVec1.Z + directionVector.Z * t); //eq of a line <x1,y1,z1> + t*<x2,y2,z2>
            return PointofIntersection;
        }
    }
}
