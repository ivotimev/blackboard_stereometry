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
    public static class Plane
    {
        private static BB_Polygon3D planePolygon;
        private static int[] selectedPoints = new int[3];
        private static MouseButtonEventHandler ellipseRightClickHandler;
        public static MouseButtonEventHandler EllipseRightClickHandler
        {
            get
            {
                return new MouseButtonEventHandler(PointRightClicked);
            }
        }
        private static MouseButtonEventHandler lineLeftClickHandler;
        public static MouseButtonEventHandler LineLeftClickHandler
        {
            get
            {
                return new MouseButtonEventHandler(LineLeftClicked);
            }
        }

        private static void BuildInitialPlane(Canvas canvas)
        {
            double max_X = 0;
            double min_X = 0;
            double max_Z = 0;
            double min_Z = 0;

            for (int i = Database.FirstPointIndex; i < Database.allPoints.Count; i++)
            {
                BB_Point3D point = Database.allPoints[i];

                if (point.Xstatic >= max_X) max_X = point.Xstatic;
                else if (point.Xstatic <= min_X) min_X = point.Xstatic;

                if (point.Zstatic >= max_Z) max_Z = point.Zstatic;
                else if (point.Zstatic <= min_Z) min_Z = point.Zstatic;
            }

            BB_Point3D planeLeftIn = new BB_Point3D(min_X, 0, min_Z, "LeftIn", 4, Brushes.AntiqueWhite);
            BB_Point3D planeRightIn = new BB_Point3D(max_X, 0, min_Z, "RightIn", 4, Brushes.AntiqueWhite);
            BB_Point3D planeRightOut = new BB_Point3D(max_X, 0, max_Z, "RightOut", 4, Brushes.AntiqueWhite);
            BB_Point3D planeLeftOut = new BB_Point3D(min_X, 0, max_Z, "LeftOut", 4, Brushes.AntiqueWhite);

            Database.AddStaticPoint(planeLeftIn);
            selectedPoints[0] = Database.allPoints.Count - 1;
            Database.AddStaticPoint(planeRightIn);
            selectedPoints[1] = Database.allPoints.Count - 1;

            BB_Line3D line1 = new BB_Line3D(Database.allPoints.Count - 1, Database.allPoints.Count - 2, 2, true);
            Database.addLine(line1);

            Database.AddStaticPoint(planeRightOut);
            selectedPoints[2] = Database.allPoints.Count - 1;

            BB_Line3D line2 = new BB_Line3D(Database.allPoints.Count - 1, Database.allPoints.Count - 2, 2, true);
            Database.addLine(line2);

            Database.AddStaticPoint(planeLeftOut);

            BB_Line3D line3 = new BB_Line3D(Database.allPoints.Count - 1, Database.allPoints.Count - 2, 2, true);
            Database.addLine(line3);
            BB_Line3D line4 = new BB_Line3D(Database.allPoints.Count - 1, line1.Points[1], 2, true);
            Database.addLine(line4);

            planePolygon = new BB_Polygon3D();

            int[] planePolygonPoints = new int[4];
            int[] planePolygonLines = new int[4];
            for (int i = 0; i < 4; i++)
            {
                planePolygonPoints[i] = Database.allPoints.Count - 1 - i;
                planePolygonLines[i] = Database.allLines.Count - 1 - i;
            }

            planePolygon.PointsPolygon = planePolygonPoints;
            planePolygon.LinesPolygon = planePolygonLines;

            planePolygon.Basis = true;

            planePolygon.planeCnsPoly = true;
            Database.addPolygon(planePolygon);

            BB_Camera.Draw(canvas);
        }
        private static void BuildPlane(Canvas canvas)
        {
            BB_Point3D planeLeftIn;
            BB_Point3D planeRightIn;
            BB_Point3D planeRightOut;
            BB_Point3D planeLeftOut;

            double max_X = 0;
            double min_X = 0;
            double max_Z = 0;
            double min_Z = 0;

            for (int i = Database.FirstPointIndex; i < Database.allPoints.Count; i++)
            {
                BB_Point3D point = Database.allPoints[i];

                if (point.Xstatic >= max_X) max_X = point.Xstatic;
                else if (point.Xstatic <= min_X) min_X = point.Xstatic;

                if (point.Zstatic >= max_Z) max_Z = point.Zstatic;
                else if (point.Zstatic <= min_Z) min_Z = point.Zstatic;
            }

            Point3D leftInCostraint1 = new Point3D(min_X, -10, min_Z);
            Point3D leftInCostraint2 = new Point3D(min_X, 10, min_Z);

            Point3D rightInCostraint1 = new Point3D(max_X, -10, min_Z);
            Point3D rightInCostraint2 = new Point3D(max_X, 10, min_Z);

            Point3D rightOutCostraint1 = new Point3D(max_X, -10, max_Z);
            Point3D rightOutCostraint2 = new Point3D(max_X, 10, max_Z);

            Point3D leftOutCostraint1 = new Point3D(min_X, -10, max_Z);
            Point3D leftOutCostraint2 = new Point3D(min_X, 10, max_Z);

            planeLeftIn = Pointof3DIntersection(leftInCostraint1, leftInCostraint2);
            planeRightIn = Pointof3DIntersection(rightInCostraint1, rightInCostraint2);            
            planeRightOut = Pointof3DIntersection(rightOutCostraint1, rightOutCostraint2);           
            planeLeftOut = Pointof3DIntersection(leftOutCostraint1, leftOutCostraint2);
            
            Database.allPoints[planePolygon.PointsPolygon[0]] = planeLeftIn;
            Database.allPoints[planePolygon.PointsPolygon[1]] = planeRightIn;
            Database.allPoints[planePolygon.PointsPolygon[2]] = planeRightOut;
            Database.allPoints[planePolygon.PointsPolygon[3]] = planeLeftOut;

            BB_Camera.Draw(canvas);
        }

        public static void MakePlaneSelected(Canvas canvas, RadioButton makePlaneButton)
        {
            BuildInitialPlane(canvas);
            foreach (var point in Database.allPoints)
            {
                point.screenPoint.MouseRightButtonDown += EllipseRightClickHandler;
            }
            foreach (var line in Database.allLines)
            {
                line.canvasLine.MouseLeftButtonDown += LineLeftClickHandler;
            }
        }

        private static int pointsSelected = 0;

        public static void PointRightClicked(object sender, MouseButtonEventArgs e)
        {
            if(sender is Ellipse)
            {
                Ellipse clicked = sender as Ellipse;
                int SelectedIndex = int.Parse(clicked.Name.Remove(0, 1));
                clicked.Fill = new SolidColorBrush(Colors.Aqua);
                selectedPoints[pointsSelected] = SelectedIndex;
                pointsSelected += 1;

                Canvas mainCanvas = clicked.Parent as Canvas;

                BB_Camera.Draw(mainCanvas);
                BuildPlane(mainCanvas);

                if(pointsSelected == 3)
                {
                    MakePlaneDeselected(Application.Current.MainWindow.FindName("i_hand") as RadioButton);
                }
            }
        }
        public static void LineLeftClicked(object sender, MouseButtonEventArgs e)
        {
            if(sender is Line)
            {
                Line clicked = sender as Line;
                int selectedIndex = int.Parse(clicked.Name.Remove(0, 1));
                Point mousePos = Mouse.GetPosition(clicked.Parent as Canvas);
                double Yoana = Math.Round((-BB_Camera.x_offset + mousePos.X) / BB_Camera.Zoom, 4); //в чест на Йоана Костова
                HitTest.SetPosition(Yoana);
                BB_Point3D selectedPoint = HitTest.Pointof3DIntersection(Database.allLines[selectedIndex]);

                selectedPoint.Radius = 10;
                selectedPoint.Visible = true;
                Canvas mainCanvas = clicked.Parent as Canvas;
                Database.addPoint(selectedPoint);

                selectedPoints[pointsSelected] = Database.allPoints.Count - 1;
                pointsSelected += 1;

                BB_Camera.Draw(mainCanvas);
                BuildPlane(mainCanvas);

                if(pointsSelected == 3)
                {
                    MakePlaneDeselected(Application.Current.MainWindow.FindName("i_hand") as RadioButton);
                }
            }
        }

        private static BB_Point3D Pointof3DIntersection(Point3D linePoint1, Point3D linePoint2)
        {
            double a = NormalVector().X;
            double b = NormalVector().Y;
            double c = NormalVector().Z;
            Point3D point = Database.allPoints[selectedPoints[1]].toStaticPoint3D();
            double d = -(a * point.X + b * point.Y + c * point.Z);
            Point3D directionVector = new Point3D(linePoint2.X - linePoint1.X, linePoint2.Y - linePoint1.Y, linePoint2.Z - linePoint1.Z);
            double t = (-d - linePoint1.X * a - linePoint1.Y * b - linePoint1.Z * c) / (directionVector.X * a + directionVector.Y * b + directionVector.Z * c); //t - the coef of the LineEq when it intersects the plane
            Point3D newPoint = new Point3D(linePoint1.X + directionVector.X * t, linePoint1.Y + directionVector.Y * t, linePoint1.Z + directionVector.Z * t);
            newPoint = BB_Camera.TransformToDynamicCoord(newPoint);
            BB_Point3D PointofIntersection = new BB_Point3D(newPoint.X, newPoint.Y, newPoint.Z);
            //BB_Point3D PointofIntersection = new BB_Point3D(linePoint1.X + directionVector.X * t, linePoint1.Y + directionVector.Y * t, linePoint1.Z + directionVector.Z * t); //eq of a line <x1,y1,z1> + t*<x2,y2,z2>
            return PointofIntersection;
        }

        private static Vector3D NormalVector()
        {
            Point3D p1 = Database.allPoints[selectedPoints[0]].toStaticPoint3D();
            Point3D p2 = Database.allPoints[selectedPoints[1]].toStaticPoint3D();
            Point3D p3 = Database.allPoints[selectedPoints[2]].toStaticPoint3D();
            Vector3D Vec1 = new Vector3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
            Vector3D Vec2 = new Vector3D(p3.X - p2.X, p3.Y - p2.Y, p3.Z - p2.Z);
            Vector3D Normal = Vector3D.CrossProduct(Vec2, Vec1);//new Vector3D(Vec1.Y * Vec2.Z - Vec1.Z * Vec2.Y, Vec1.Z * Vec2.X - Vec1.X * Vec2.Z, Vec1.X * Vec2.Y - Vec1.Y * Vec2.X);
            return Normal;
        }

        public static void MakePlaneDeselected(RadioButton i_hand)
        {
            selectedPoints = new int[3];
            pointsSelected = 0;
            planePolygon = new BB_Polygon3D();

            i_hand.IsChecked = true;

            foreach (var point in Database.allPoints)
            {
                point.screenPoint.MouseRightButtonDown -= EllipseRightClickHandler;
            }
            foreach (var line in Database.allLines)
            {
                line.canvasLine.MouseLeftButtonDown -= LineLeftClickHandler;
            }
        }
    }
}
