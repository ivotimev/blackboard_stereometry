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

    public static class BB_Camera 
    {
        //the zoom and the offsets determine how and where the object will be drawn on the screen
        public static double Zoom = 88;
        public static double x_offset= 507/2;
        public static double y_offset= 517/2;
        public static double SumAngleX = 0;
        public static double SumAngleY = 0;
        public static double SumAngleZ = 0;
        public static MouseButtonEventHandler shapesMouseDown;
        public static MouseEventHandler shapesMouseOver;
        public static MouseButtonEventHandler polygonMouseUp;

        public static void SetToDefault()
        {
            SumAngleX = 0;
            SumAngleY = 0;
            SumAngleZ = 0;
        }
        public static void Draw(Canvas Canvas)
        {
            BB_Polygon3D.VisibleFace();
            Canvas.Children.Clear();
            for (int i = 0; i < Database.allLines.Count; i++)
            {
                Database.allLines[i].canvasLine.Name = "l" + i.ToString();
                Database.allLines[i].Draw(Canvas, Zoom, x_offset, y_offset);
            }
            foreach (var line in Database.AuxiLines)
            {
                line.Draw(Canvas, Zoom, x_offset, y_offset);
            }
            foreach (var point in Database.allPoints)
            {
                point.Draw(Canvas);
            }
            foreach (var polygon in Database.allPolygons)
            {
                polygon.Draw(Canvas, Zoom, x_offset, y_offset, polygonMouseUp, shapesMouseDown);
            }
        }
        public static void MassRotation(double AngleX, double AngleY, double AngleZ)//rotates all points
        {
            SumAngleX += AngleX;
            SumAngleY += AngleY;
            SumAngleZ += AngleZ;
            //creates rotation matricies
            RotateTransform3D xRotation = new RotateTransform3D(new AxisAngleRotation3D(
                                  new Vector3D(2, 0, 0), AngleX));
            //the rotation around Y is actually a rotation around a Vector, starting at the begining of the
            //coordinate system and perpendicular to the x,z plane
            RotateTransform3D yRotation = new RotateTransform3D(new AxisAngleRotation3D(
                                 new Vector3D(0, Math.Cos(SumAngleX / 180 * Math.PI), Math.Sin(SumAngleX / 180 * Math.PI)), AngleY)); //new Vector3D(0, Math.Cos(AngleX / 180 * Math.PI), Math.Sin(AngleX / 180 * Math.PI)), AngleY))

            for (int i = 0; i < Database.allPoints.Count; i++)
            {
                Database.allPoints[i].Rotate(xRotation);
                Database.allPoints[i].Rotate(yRotation);
            }

        }
        public static Point3D TransformToDynamicCoord(Point3D point1)
        {
            //creates rotation matricies
            RotateTransform3D xRotation = new RotateTransform3D(new AxisAngleRotation3D(
                                  new Vector3D(2, 0, 0), SumAngleX));
            //the rotation around Y is actually a rotation around a Vector, starting at the begining of the
            //coordinate system and perpendicular to the x,z plane
            RotateTransform3D yRotation = new RotateTransform3D(new AxisAngleRotation3D(
                                 new Vector3D(0, Math.Cos(SumAngleX / 180 * Math.PI), Math.Sin(SumAngleX / 180 * Math.PI)), SumAngleY));

            Point3D point = new Point3D(point1.X, point1.Y, point1.Z);
            point = xRotation.Transform(point);
            point = yRotation.Transform(point);
            
            return point;

        }
        public static Point3D TransformToStaticCoord(Point3D point1) //initial
        {
            double sumx = -SumAngleX;
            double sumy = -SumAngleY;


            //creates rotation matricies
            RotateTransform3D xRotation = new RotateTransform3D(new AxisAngleRotation3D(
                                  new Vector3D(2, 0, 0), sumx));
            //the rotation around Y is actually a rotation around a Vector, starting at the begining of the
            //coordinate system and perpendicular to the x,z plane
            RotateTransform3D yRotation = new RotateTransform3D(new AxisAngleRotation3D(
                                 new Vector3D(0, Math.Cos(SumAngleX / 180 * Math.PI), Math.Sin(SumAngleX / 180 * Math.PI)), sumy));

            //BB_Point3D point = new BB_Point3D(point1.X, point1.Y, point1.Z);
            /*point.Rotate(yRotation);
            point.Rotate(xRotation);
            point.Y += 2;
            return point.toPoint3D();     
            */
            Point3D point = new Point3D(point1.X,point1.Y,point1.Z);
            point = yRotation.Transform(point);
            point = xRotation.Transform(point);
            return point;
        }
        public static void SetMouseEventHandlers(MouseButtonEventHandler shapes_mouse_down, MouseEventHandler shapes_mouse_over, MouseButtonEventHandler polygon_mouse_up)
        {
            shapesMouseDown = shapes_mouse_down;
            shapesMouseOver = shapes_mouse_over;
            polygonMouseUp = polygon_mouse_up;
        }

    }
    public class BB_Point3D
    {   //fields: //Point 3D contains the coordinates of the point in the 3D space
        Point3D coordinates;
       // public readonly Point3D InitialCoords;
        Point3D staticCoordinates;
        string name;
        Brush nameColor;
        //color and radius are attributes of the point on the canvas
        Color color = Colors.White;
        double radius;
        bool selected;//if the point is selected
        bool visible = true;
        int index;

        public Ellipse screenPoint = new Ellipse();

        //constructors:
        public BB_Point3D() {
            screenPoint.MouseDown += BB_Camera.shapesMouseDown;
            screenPoint.MouseUp += BB_Camera.shapesMouseDown;
            screenPoint.MouseEnter += BB_Camera.shapesMouseOver;
            screenPoint.MouseLeave += BB_Camera.shapesMouseOver;
        }
        public BB_Point3D(double X, double Y, double Z)
        {          
            coordinates = new Point3D(X, Y, Z);
           // staticCoordinates = BB_Camera.TransformToStaticCoord(coordinates);
            color = Colors.White;
            radius = 1;
            this.nameColor = Brushes.White;
            index = Database.allPoints.Count;

            screenPoint.MouseDown += BB_Camera.shapesMouseDown;
            screenPoint.MouseUp += BB_Camera.shapesMouseDown;
            screenPoint.MouseEnter += BB_Camera.shapesMouseOver;
            screenPoint.MouseLeave += BB_Camera.shapesMouseOver;
        }//basic point definition
        public BB_Point3D(double X, double Y, double Z, double Radius)
        {

            coordinates = new Point3D(X, Y, Z);
          //  staticCoordinates = BB_Camera.TransformToStaticCoord(coordinates);
            color = Colors.White;
            radius = Radius;
            this.nameColor = Brushes.White;
            index = Database.allPoints.Count;
            screenPoint.MouseDown += BB_Camera.shapesMouseDown;
            screenPoint.MouseUp += BB_Camera.shapesMouseDown;
            screenPoint.MouseEnter += BB_Camera.shapesMouseOver;
            screenPoint.MouseLeave += BB_Camera.shapesMouseOver;

        }//basic point definition
        public BB_Point3D(double X, double Y, double Z, string Name, double radius)
        {
            coordinates = new Point3D(X, Y, Z);
           // staticCoordinates = BB_Camera.TransformToStaticCoord(coordinates);
            name = Name;
            this.radius = radius;
            color = Colors.White;
            this.nameColor = Brushes.White;
            index = Database.allPoints.Count;

            screenPoint.MouseDown += BB_Camera.shapesMouseDown;
            screenPoint.MouseUp += BB_Camera.shapesMouseDown;
            screenPoint.MouseEnter += BB_Camera.shapesMouseOver;
            screenPoint.MouseLeave += BB_Camera.shapesMouseOver;
        }
        public BB_Point3D(double X, double Y, double Z, string Name, double radius, Brush nameColor)
        {
            coordinates = new Point3D(X, Y, Z);
         //   staticCoordinates = BB_Camera.TransformToStaticCoord(coordinates);
            name = Name;
            this.radius = radius;
            color = Colors.White;
            this.nameColor = nameColor;
            index = Database.allPoints.Count;

            screenPoint.MouseDown += BB_Camera.shapesMouseDown;
            screenPoint.MouseUp += BB_Camera.shapesMouseDown;
            screenPoint.MouseEnter += BB_Camera.shapesMouseOver;
            screenPoint.MouseLeave += BB_Camera.shapesMouseOver;
        }//point definition with color
        public BB_Point3D(double X, double Y, double Z, bool visible)
        { 
            coordinates = new Point3D(X, Y, Z);
           // staticCoordinates = BB_Camera.TransformToStaticCoord(coordinates);
            radius = 6;
            this.visible = visible;
            index = Database.allPoints.Count;

            screenPoint.MouseDown += BB_Camera.shapesMouseDown;
            screenPoint.MouseUp += BB_Camera.shapesMouseDown;
            screenPoint.MouseEnter += BB_Camera.shapesMouseOver;
            screenPoint.MouseLeave += BB_Camera.shapesMouseOver;
        }//point definition with color and radius

        //properties:
        public double X
        {
            get
            {
                return coordinates.X;
            }
            set
            {
                coordinates.X = value;

            }
        }
        public double Y
        {
            get
            {
                return coordinates.Y;
            }
            set
            {
                coordinates.Y = value;
            }
        }
        public double Z
        {
            get
            {
                return coordinates.Z;
            }
            set
            {
                coordinates.Z = value;
            }
        }
        public Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                try
                {
                    color = value;
                }
                catch
                {
                    color = Colors.White;
                }
            }
        }
        public double Radius
        {
            get
            {
                return radius;
            }
            set
            {

                    radius = value;
            }
        }
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                if(selected==true)
                {
                    //Radius *= 2;
                    Color = Colors.OrangeRed;
                }
                else
                {
                    //Radius /= 2;
                    Color = Colors.White;
                }
            }
        }
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }
        public double Xstatic
        {
            get
            {
              //  staticCoordinates = BB_Camera.TransformToStaticCoord(coordinates);
                return staticCoordinates.X;
            }
            set
            {
                staticCoordinates.X = value;
                coordinates = BB_Camera.TransformToDynamicCoord(staticCoordinates);
            }
        }
        public double Ystatic
        {
            get
            {
                //staticCoordinates = BB_Camera.TransformToStaticCoord(coordinates);
                return staticCoordinates.Y;
            }
            set
            {       
                staticCoordinates.Y = value;
                coordinates = BB_Camera.TransformToDynamicCoord(staticCoordinates);
            }
        }
        public double Zstatic
        {
            get
            {
                //staticCoordinates = BB_Camera.TransformToStaticCoord(coordinates);
                return staticCoordinates.Z;
            }
            set
            {
                staticCoordinates.Z = value;
                coordinates = BB_Camera.TransformToDynamicCoord(staticCoordinates);
            }
        }

        //methods:
        public void Draw(Canvas canvas)//draws the point on the canvas
        {
            double Zoom = BB_Camera.Zoom;
            //converts the 3d coordinates of the point to 2d ones isometricly, taking into account zoom(size increase/decrease) and pan(x or y offset)
            double x2d = BB_Camera.x_offset + Zoom * coordinates.X;
            double y2d = BB_Camera.y_offset - Zoom * coordinates.Y;

            //creates an elipse with color and radius
            screenPoint.Width = radius * Zoom / 100;
            screenPoint.Height = radius * Zoom / 100;
            screenPoint.Fill = new SolidColorBrush(color);

            //draws the elipse on the canvas
            canvas.Children.Add(screenPoint);
            screenPoint.Name = "p" + Index.ToString();
            
            //sets the elipse's position to the 2d coordinates of the point
            Canvas.SetTop(screenPoint, y2d - radius * Zoom / 100 / 2);
            Canvas.SetLeft(screenPoint, x2d - radius * Zoom / 100 / 2);
           // else screenPoint.Height = radius * Zoom / 100;
            //sets Point's name
            
            TextBlock Pname = new TextBlock();
            Pname.Text = name;
            Pname.Foreground = nameColor;           
            canvas.Children.Add(Pname);
            Canvas.SetTop(Pname, y2d - radius * Zoom / 100 / 2 + 7);
            Canvas.SetLeft(Pname, x2d - radius * Zoom / 100 / 2 + 7);
            // Pname.Opacity = 

            if (visible == false)
            { screenPoint.Visibility = Visibility.Hidden;
                Pname.Visibility = Visibility.Hidden;
            }
            else {
                screenPoint.Visibility = Visibility.Visible;
                Pname.Visibility = Visibility.Visible;
            }

        }
        public void Rotate(RotateTransform3D Rotation)//rotates the point a given angle around each of the axis
        {
            //we transform the point using the rotation matrix
            coordinates = Rotation.Transform(coordinates);
        }
        public Point3D toPoint3D()
        {
            return coordinates;
        }//in case we need to convert it to defaullt 3D point
        public Point3D toStaticPoint3D()
        {
            return staticCoordinates;
        }
        public void TranslateY(double Value)//nondeveloped
        {
            coordinates.Y += Value;
        }
        public void Project()//nondeveloped
        {

        }
        public void AddTextNearthePoint(Canvas canvas, string text)
        {
            double x2d = BB_Camera.x_offset + BB_Camera.Zoom * coordinates.X;
            double y2d = BB_Camera.y_offset - BB_Camera.Zoom * coordinates.Y;
            TextBlock Ptext = new TextBlock();
            Ptext.Text = text;
            Ptext.Foreground = Brushes.White;
            canvas.Children.Add(Ptext);
            Canvas.SetTop(Ptext, y2d - radius * BB_Camera.Zoom / 100 / 2 + 7);
            Canvas.SetLeft(Ptext, x2d - radius * BB_Camera.Zoom / 100 / 2 + 7);
    }
        public void AddStaticCoordinates(Point3D point)
        {
            staticCoordinates = point;
        }
    }

    public class BB_Line3D
    {
        //fields: //the line is defined by two points
        int[] points = new int[2];

        bool dashed;//is the line visible or not

        bool lonely;
       // public bool OnAVisibleSide;

        bool segment;//false = whole line; true = line segment

        //attributes that determine how the line would look when drawn on the canvas
        Color color;//the line's color
        double thickness;//the line's thickness
        double dashLenght = 5;//the dashes' lenght and frequency
        bool visible = true;
        //constructors:
        public BB_Line3D(int Point1, int Point2)//basic line definition - 2 points
        {
            this.points[0] = Point1;
            this.points[1] = Point2;
            this.dashed = false;
            this.color = Colors.White;
            this.thickness = 1;
            lonely = true;
            this.segment = true;

        }

        public BB_Line3D(int Point1, int Point2, double thickness, bool lonely)//basic line definition - 2 points
        {
            this.points[0] = Point1;
            this.points[1] = Point2;
            this.dashed = false;
            this.color = Colors.White;
            this.thickness = thickness;
            //     this.dashLenght = 1;
            this.segment = true;
            this.lonely = lonely;
        }
        public BB_Line3D(int Point1, int Point2, int IniAPCount)//definiton for building defaultObjects
        {
            this.points[0] = Point1 + IniAPCount;
            this.points[1] = Point2 + IniAPCount;
            this.dashed = false;
            this.color = Colors.White;
            this.thickness = 1;
            //   this.dashLenght = 1;
            this.segment = true;
            lonely = true;
        }
        public BB_Line3D(int Point1, int Point2, bool lonely)//basic line definition - 2 points
        {
            this.points[0] = Point1;
            this.points[1] = Point2;
            this.lonely = Lonely;
            this.color = Colors.White;
            this.thickness = 1;
            //   this.dashLenght = 1;
            this.segment = true;
            this.dashed = false;
            this.lonely = lonely;
        }
        public BB_Line3D(int Point1, int Point2, Color color, double thickness)//basic line definition - 2 points
        {
            this.points[0] = Point1;
            this.points[1] = Point2;
            this.dashed = false;
            this.color = color;
            this.thickness = thickness;
            lonely = false;
            //this.dashLenght = 1;
        }

        //properties:
        public int[] Points
        {
            get
            {
                return points;
            }
            set
            {
                this.points = Points;
            }
        }
        public bool Segment
        {
            get
            {
                return segment;
            }
            set
            {
                this.segment = Segment;
            }
        }
        public bool Dashed
        {
            get
            {
                return dashed;
            }
            set
            {
                this.dashed = value;
            }
        }
        public bool Lonely
        {
            get
            {
                return lonely;
            }
            set
            {
                this.lonely = value;
            }
        }
        public Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                try
                {
                    color = Color;
                }
                catch
                {
                    color = Colors.White;
                }
            }
        }
        public double Thickness
        {
            get
            {
                return this.thickness;
            }
            set
            {
                try
                {
                    this.thickness = value;
                }
                catch
                {
                    thickness = 1;
                }
            }
        }
        public double DashLenght
        {
            get
            {
                return this.dashLenght;
            }
            set
            {
                try
                {
                    this.dashLenght = value;
                }
                catch
                {
                    dashLenght = 1;
                }
            }
        }
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        //in the methods we add a reference to the list of points, so that we can get the coordinates of
        //the points the line is between

        //methods:
        public static double GetDashScaleCoefficient(Point3D point1, Point3D point2)//calculates the rate at whick the dashes of the line converge
        {
            double dx = Math.Abs(point1.X - point2.X);
            double dy = Math.Abs(point1.Y - point2.Y);
            double dz = Math.Abs(point1.Z - point2.Z);
            double _2dLenght = Math.Sqrt(dx * dx + dy * dy);
            double _3dLenght = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double k = _2dLenght * 2 / _3dLenght;
            return k;//the less of a line we can see, the more it's dashes converge - the less we can see of it, the smaller 2d lenght it has
                     //so me calculate how much the dashes have converged by dividing it's apparent lenght(2d) to its actual lenght(3d)
        }

        public Line canvasLine = new Line();
        public void Draw(Canvas Canvas, double Zoom, double PanX, double PanY)//draws the line on the canvas
        {

            //we convert the line's 3d coordinates to isometric 2d ones, finding it's proection on the screen
            double x2d_1 = PanX + Zoom * Database.allPoints[points[0]].X;
            double y2d_1 = PanY - Zoom * Database.allPoints[points[0]].Y;
            double x2d_2 = PanX + Zoom * Database.allPoints[points[1]].X;
            double y2d_2 = PanY - Zoom * Database.allPoints[points[1]].Y;

            // ! не знам дали има нужда от тва да я питаме дали е цяла права или само сегмент !
            // ! или поне мисля че дори и да е цяла права ще я рисуваме като сегмент, просто сегмент между двата края на екрана !

            canvasLine.Stroke = new SolidColorBrush(color);
            if (dashed) canvasLine.StrokeDashArray = new DoubleCollection() { dashLenght * GetDashScaleCoefficient(Database.allPoints[points[0]].toPoint3D(), Database.allPoints[points[1]].toPoint3D()) };
            else
            {
                canvasLine.StrokeDashArray.Clear();
            }//if the line is invisible
                                                                                                                                                                                                        //it sets it's dashed properties
            if (visible == false) canvasLine.Visibility = Visibility.Hidden;
            else canvasLine.Visibility = Visibility.Visible;
            canvasLine.StrokeThickness = thickness * Zoom / 100;
            canvasLine.X1 = x2d_1;
            canvasLine.Y1 = y2d_1;
            canvasLine.X2 = x2d_2;
            canvasLine.Y2 = y2d_2;
            Canvas.Children.Add(canvasLine);
        }

        public static void AddLineFromStaticCoord(BB_Point3D point1, BB_Point3D point2, Canvas canvas, double thickness)// MouseButtonEventHandler shapes_mouse_down, MouseEventHandler shapes_mouse_over)
        {
            Database.AddStaticPoint(point1);
            Database.AddStaticPoint(point2);
            Database.addLine(new BB_Line3D(Database.allPoints.Count - 2, Database.allPoints.Count - 1));
            Database.allLines.Last().Thickness = thickness;
          //  BB_Camera.Draw(canvas, shapes_mouse_down, shapes_mouse_over);
            // System.Windows.Media.
        }
        public static void AddLineFromDynamicCoord(BB_Point3D point1, BB_Point3D point2, Canvas canvas, double thickness)// MouseButtonEventHandler shapes_mouse_down, MouseEventHandler shapes_mouse_over)
        {
            Database.addPoint(point1);
            Database.addPoint(point2);
            Database.addLine(new BB_Line3D(Database.allPoints.Count - 2, Database.allPoints.Count - 1));
            Database.allLines.Last().Thickness = thickness;
           // BB_Camera.Draw(canvas, shapes_mouse_down, shapes_mouse_over);
            // System.Windows.Media.
        }
        public Point3D[] PointVectors()
        {
            Point3D[] PosVectors = new Point3D[3];
            PosVectors[0] = Database.allPoints[Points[0]].toPoint3D(); //posvector1
            PosVectors[1] = Database.allPoints[Points[1]].toPoint3D(); //posvector2
            PosVectors[2] = new Point3D(PosVectors[1].X - PosVectors[0].X, PosVectors[1].Y - PosVectors[0].Y, PosVectors[1].Z - PosVectors[0].Z); //direction vector
            return PosVectors;
        }

    }
    public class BB_AuxiLine3D
    {
        Point3D Coord1;
        Point3D Coord2;
         public bool dashed;
        public BB_AuxiLine3D(Point3D point1, Point3D point2)
        {
            Coord1 = point1;
            Coord2 = point2;
        }
        public void Draw(Canvas Canvas, double Zoom, double PanX, double PanY)//draws the line on the canvas
        {

            //we convert the line's 3d coordinates to isometric 2d ones, finding it's proection on the screen
            double x2d_1 = PanX + Zoom * Coord1.X;
            double y2d_1 = PanY - Zoom * Coord1.Y;
            double x2d_2 = PanX + Zoom * Coord2.X;
            double y2d_2 = PanY - Zoom * Coord2.Y;

            // ! не знам дали има нужда от тва да я питаме дали е цяла права или само сегмент !
            // ! или поне мисля че дори и да е цяла права ще я рисуваме като сегмент, просто сегмент между двата края на екрана !

            Line newLine = new Line();
            newLine.IsHitTestVisible = false;
            if (dashed == true) newLine.StrokeDashArray = new DoubleCollection() { 5 * BB_Line3D.GetDashScaleCoefficient(Coord1, Coord2) };
            newLine.Stroke = new SolidColorBrush(Colors.White);
            // newLine.StrokeDashArray = new DoubleCollection() { 5 * BB_Line3D.GetDashScaleCoefficient(Coord1, Coord2)  };//if the line is invisible
            //it sets it's dashed properties
            newLine.StrokeThickness = 2.2 * Zoom / 100;
            newLine.X1 = x2d_1;
            newLine.Y1 = y2d_1;
            newLine.X2 = x2d_2;
            newLine.Y2 = y2d_2;
            Canvas.Children.Add(newLine);
        }

    }
    public class BB_Polygon3D
    {
        /* the 3D polygon class will be used only to help us determine line visibility
        so for a polygon we will need to know how many verticies it has - nVerticies
        the indexes(stored in an array) of its verticies in the points list - pointsI
        and the indexes of its lines in the lines list - linesI
        */
        //fields:
        int[] pointsPolygon;
        int[] linesPolygon;
        bool visible = true;
        //  bool unvisibleFace = true;
        bool basis = false;
        //Vector3D normalVector;

        //constructors

        public BB_Polygon3D()
        {
            
        }
        public BB_Polygon3D(int[] Points, int[] Lines)
        {
            this.pointsPolygon = Points;
            this.linesPolygon = Lines;
            //  normalVector = FindNormalVector();

        }

        //properties
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                if (Visible == false)
                {
                    for (int i = 0; i < pointsPolygon.Length; i++)
                    {
                        Database.allPoints[PointsPolygon[i]].Visible = false;
                        if (i < LinesPolygon.Length) Database.allLines[LinesPolygon[i]].Visible = false;
                    }
                }
                else {
                    for (int i = 0; i < PointsPolygon.Length; i++)
                    {
                        Database.allPoints[PointsPolygon[i]].Visible = true;
                        if (i < LinesPolygon.Length) Database.allLines[LinesPolygon[i]].Visible = true;
                    }
                }
            }
        }
        public int[] PointsPolygon
        {
            get
            {
                return pointsPolygon;
            }
            set
            {
                this.pointsPolygon = value;
            }
        }
        public int[] LinesPolygon
        {
            get
            {
                return linesPolygon;
            }
            set
            {
                this.linesPolygon = value;
            }
        }
        public Vector3D NormalVector
        {
            get
            {
                return FindNormalVector();

            }
        }
        public Vector3D RealNormalVector
        {
            get
            {
                return FindRealNormalVector();

            }
        }
        public static void VisibleFace()
        {
            for (int i = 0; i < Database.allPolygons.Count; i++)
            {
                if (Database.allPolygons[i].RealNormalVector.Z < 0)
                    for (int j = 0; j < Database.allPolygons[i].LinesPolygon.Length; j++)
                        Database.allLines[Database.allPolygons[i].LinesPolygon[j]].Dashed = true;
            }
            for (int i = 0; i < Database.allPolygons.Count; i++)
            {
                if (Database.allPolygons[i].RealNormalVector.Z >= 0)
                    for (int j = 0; j < Database.allPolygons[i].LinesPolygon.Length; j++)
                        Database.allLines[Database.allPolygons[i].LinesPolygon[j]].Dashed = false;
            }

        }
        public bool Basis
        {
            get { return basis; }
            set { basis = value; }
        }
        //methods

        public static void CreateRegularPolygon(int n, double Yheight, Canvas canvas) // default apothem = 1;
        {

            int IniAPCount = Database.allPoints.Count;
            int[] pointsP = new int[n];
            int[] linesP = new int[n];
            double Xcoord = Math.Sqrt(Math.Pow((1 / (Math.Cos(3.14 / n))), 2) - 1); //math R=1/cos(180/n); x = s/2 
            double Zcoord = 1; //apothem

            BB_Point3D IniPoint = new BB_Point3D(Xcoord, Yheight, Zcoord); //first point to rotate
                                                                           //IniPoint.Rotate(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 360 / n))); // !!!!!!                
            BB_Point3D NewPoint1 = new BB_Point3D(IniPoint.X, IniPoint.Y, IniPoint.Z, Database.SmartName(), 5, Brushes.AntiqueWhite);// need another point cause there is a second rotation of IniPoint
            Database.AddStaticPoint(NewPoint1);   //adding points
            for (int i = 1; i < n; i++)
            {

                IniPoint.Rotate(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -360 / n)));
                BB_Point3D NewPoint = new BB_Point3D(IniPoint.X, IniPoint.Y, IniPoint.Z, Database.SmartName(), 8, Brushes.AntiqueWhite);
                Database.AddStaticPoint(NewPoint);

            }

            for (int i = 0; i < n; i++)    //adding lines
            {
                pointsP[i] = IniAPCount + i;
                if (i < n - 1)
                {
                    Database.addLine(new BB_Line3D(IniAPCount + i, IniAPCount + i + 1, 2.2, false));
                    linesP[i] = Database.allLines.Count - 1;
                }
                else
                {
                    Database.addLine(new BB_Line3D(IniAPCount + i, IniAPCount, 2.2, false));
                    linesP[i] = Database.allLines.Count - 1;
                }


            }
            BB_Polygon3D RegPolygon = new BB_Polygon3D(pointsP, linesP); //adding polygon
            if (Yheight == 0) RegPolygon.basis = true;
            Database.addPolygon(RegPolygon);
            //Camera.TransformToRealisticCoord(RegPolygon.PointsPolygon);

        }

        public bool PointonPolygon(Point3D point) //determines if point lies on a polygon(3D)
        {
            double m1, m2;
            double anglesum = 0, costheta = 0;
            Vector3D p1 = new Vector3D();
            Vector3D p2 = new Vector3D();
            double dotpr = 0;
            int n = PointsPolygon.Length;
            for (int i = 0; i < n; i++)
            {

                p1.X = Database.allPoints[pointsPolygon[i]].toStaticPoint3D().X - point.X;
                p1.Y = Database.allPoints[pointsPolygon[i]].toStaticPoint3D().Y - point.Y;
                p1.Z = Database.allPoints[pointsPolygon[i]].toStaticPoint3D().Z - point.Z;

                p2.X = Database.allPoints[pointsPolygon[(i + 1) % n]].toStaticPoint3D().X - point.X;
                p2.Y = Database.allPoints[pointsPolygon[(i + 1) % n]].toStaticPoint3D().Y - point.Y;
                p2.Z = Database.allPoints[pointsPolygon[(i + 1) % n]].toStaticPoint3D().Z - point.Z;

                m1 = Math.Round(p1.Length, 3);
                m2 = Math.Round(p2.Length, 3);
                if (m1 * m2 <= 0.0000001) return true;
                dotpr = Math.Round(Vector3D.DotProduct(p1, p2), 3);
                costheta = dotpr / (m1 * m2);
                //  if (costheta > -0.1 && costheta < 0.1) return true;
                anglesum += Math.Acos(costheta);

            }
            if (Math.Round(anglesum, 2) == 2 * 3.14) return true;
            //  if (Math.Round(anglesum, 2) > 2 * 3.11&&Math.Round(anglesum, 2) < 2 * 3.17) return true;
            else return false;

        }
        public bool PointonPolygon2D(Point3D point) //determines if point lies on a polygon(2D)
        {
            double m1, m2;
            double anglesum = 0;//, costheta = 0;
            Vector p1 = new Vector();
            Vector p2 = new Vector();
            // double dotpr = 0;
            int n = PointsPolygon.Length;
            for (int i = 0; i < n; i++)
            {

                p1.X = Database.allPoints[pointsPolygon[i]].toPoint3D().X - point.X;
                p1.Y = Database.allPoints[pointsPolygon[i]].toPoint3D().Y - point.Y;

                p2.X = Database.allPoints[pointsPolygon[(i + 1) % n]].toPoint3D().X - point.X;
                p2.Y = Database.allPoints[pointsPolygon[(i + 1) % n]].toPoint3D().Y - point.Y;

                m1 = p1.Length;
                m2 = p2.Length;
                if (m1 * m2 <= 0.0000001) return true;
                //dotpr = Math.Round(Vector.d, 3);

                // costheta = dotpr / (m1 * m2);
                anglesum += Vector.AngleBetween(p1, p2);

            }
            if (Math.Abs(Math.Round(anglesum)) == 360) return true;
            else return false;

        }
        private Vector3D FindNormalVector() //static
        {
            int p1 = pointsPolygon[0];
            int p2 = pointsPolygon[1];
            int p3 = pointsPolygon[2];
            Vector3D Vec1 = new Vector3D(Database.allPoints[p1].toStaticPoint3D().X - Database.allPoints[p2].toStaticPoint3D().X, Database.allPoints[p1].toStaticPoint3D().Y - Database.allPoints[p2].toStaticPoint3D().Y, Database.allPoints[p1].toStaticPoint3D().Z - Database.allPoints[p2].toStaticPoint3D().Z);
            Vector3D Vec2 = new Vector3D(Database.allPoints[p3].toStaticPoint3D().X - Database.allPoints[p2].toStaticPoint3D().X, Database.allPoints[p3].toStaticPoint3D().Y - Database.allPoints[p2].toStaticPoint3D().Y, Database.allPoints[p3].toStaticPoint3D().Z - Database.allPoints[p2].toStaticPoint3D().Z);
            Vector3D Normal = Vector3D.CrossProduct(Vec2, Vec1);//new Vector3D(Vec1.Y * Vec2.Z - Vec1.Z * Vec2.Y, Vec1.Z * Vec2.X - Vec1.X * Vec2.Z, Vec1.X * Vec2.Y - Vec1.Y * Vec2.X);
            if (basis == true) Normal = Normal * (-1);
            return Normal;
        }
        private Vector3D FindRealNormalVector() //dynamic
        {            
            int p1 = pointsPolygon[0];
            int p2 = pointsPolygon[1];
            int p3 = pointsPolygon[2];
            Vector3D Vec1 = new Vector3D(Database.allPoints[p1].X - Database.allPoints[p2].X, Database.allPoints[p1].Y - Database.allPoints[p2].Y, Database.allPoints[p1].Z - Database.allPoints[p2].Z);
            Vector3D Vec2 = new Vector3D(Database.allPoints[p3].X - Database.allPoints[p2].X, Database.allPoints[p3].Y - Database.allPoints[p2].Y, Database.allPoints[p3].Z - Database.allPoints[p2].Z);
            Vector3D Normal = Vector3D.CrossProduct(Vec2, Vec1);//new Vector3D(Vec1.Y * Vec2.Z - Vec1.Z * Vec2.Y, Vec1.Z * Vec2.X - Vec1.X * Vec2.Z, Vec1.X * Vec2.Y - Vec1.Y * Vec2.X);
            if (basis == true) Normal = Normal * (-1);
            
                return Normal;
        }
        public double Pointof3DIntersection(BB_Line3D Line)
        {
            double a = NormalVector.X;
            double b = NormalVector.Y;
            double c = NormalVector.Z;
            Point3D point = Database.allPoints[PointsPolygon[0]].toStaticPoint3D();
            double d = -(a * point.X + b * point.Y + c * point.Z);
            Point3D positionVec1 = Database.allPoints[Line.Points[0]].toStaticPoint3D();//position vector
            Point3D positionVec2 = Database.allPoints[Line.Points[1]].toStaticPoint3D(); //2nd position vector
            Point3D directionVector = new Point3D(positionVec2.X - positionVec1.X, positionVec2.Y - positionVec1.Y, positionVec2.Z - positionVec1.Z);
            double t = (-d - positionVec1.X * a - positionVec1.Y * b - positionVec1.Z * c) / (directionVector.X * a + directionVector.Y * b + directionVector.Z * c); //t - the coef of the LineEq when it intersects the plane
            BB_Point3D PointofIntersection = new BB_Point3D(positionVec1.X + directionVector.X * t, positionVec1.Y + directionVector.Y * t, positionVec1.Z + directionVector.Z * t); //eq of a line <x1,y1,z1> + t*<x2,y2,z2>
            if (PointonPolygon(PointofIntersection.toPoint3D())) { return t; }
            else return 0;
        }
        public bool LiesOnthePlane(BB_Line3D line)
        {
            double a = NormalVector.X;
            double b = NormalVector.Y;
            double c = NormalVector.Z;
            Point3D point = Database.allPoints[PointsPolygon[0]].toStaticPoint3D();
            double d = -(a * point.X + b * point.Y + c * point.Z);
            Point3D positionVec1 = Database.allPoints[line.Points[0]].toStaticPoint3D();//position vector
            Point3D positionVec2 = Database.allPoints[line.Points[1]].toStaticPoint3D();//2nd position vector
            if (Math.Round((a * positionVec1.X + b * positionVec1.Y + c * positionVec1.Z + d),2) == 0 && Math.Round((a * positionVec2.X + b * positionVec2.Y + c * positionVec2.Z + d),2) == 0) return true;
           // if (Math.Round(positionVec1.Y) == 0 && Math.Round(positionVec2.Y) == 0) return true;
            else return false;
        }
        public List<double> Pointof2DIntersection(BB_Line3D Line)
        {
            Point3D P0 = Line.PointVectors()[0];
            Point3D P1 = Line.PointVectors()[1];
            Point3D u = new Point3D(Line.PointVectors()[2].X, Line.PointVectors()[2].Y, Line.PointVectors()[2].Z);

            List<double> sIs = new List<double>();
            for (int i = 0; i < linesPolygon.Length; i++)
            {

                Vector3D Q0 = new Vector3D(Database.allLines[linesPolygon[i]].PointVectors()[0].X, Database.allLines[linesPolygon[i]].PointVectors()[0].Y, Database.allLines[linesPolygon[i]].PointVectors()[0].Z);
                Vector3D v = new Vector3D(Database.allLines[linesPolygon[i]].PointVectors()[2].X, Database.allLines[linesPolygon[i]].PointVectors()[2].Y, Database.allLines[linesPolygon[i]].PointVectors()[2].Z);
                Vector w = new Vector(P0.X - Q0.X, P0.Y - Q0.Y);
                double D = u.X * v.Y - v.X * u.Y;
                if (Math.Abs(D) < 0.00001)
                {
                    {           // S1 and S2 are parallel
                        if (w.Y * u.X - w.X * u.Y != 0 || v.Y * w.X - v.X * w.Y != 0)
                        {
                            return null;
                        }
                    }
                }
                double sI = (v.Y * w.X - v.X * w.Y) / (v.X * u.Y - v.Y * u.X);


                if (sI >= 0 && sI <= 1)
                {
                    double tI = (w.Y * u.X - w.X * u.Y) / (u.X * v.Y - u.Y * v.X);

                    if (tI >= 0 && tI <= 1)
                    {
                        double DP = Vector3D.DotProduct(((new Vector3D(P0.X + sI * u.X, P0.Y + sI * u.Y, P0.Z + sI * u.Z)) - (new Vector3D(Q0.X + tI * v.X, Q0.Y + tI * v.Y, Q0.Z + tI * v.Z))), RealNormalVector);
                        if (DP > -0.01)
                            sIs.Add(sI);
                    }
                }
            }
            if (PointonPolygon2D(P0)) if ((Vector3D.DotProduct(((Vector3D)Database.allPoints[Line.Points[0]].toStaticPoint3D() - (Vector3D)Database.allPoints[PointsPolygon[1]].toStaticPoint3D()), NormalVector) > 0)) sIs.Add(0);
            if (PointonPolygon2D(P1)) if ((Vector3D.DotProduct(((Vector3D)Database.allPoints[Line.Points[1]].toStaticPoint3D() - (Vector3D)Database.allPoints[PointsPolygon[1]].toStaticPoint3D()), NormalVector) > 0)) sIs.Add(1);


            if (sIs.Count != 0) return sIs;
            else return null;

        }

        public bool planeCnsPoly = false;
        public void Draw(Canvas Canvas, double Zoom, double PanX, double PanY, MouseButtonEventHandler polygon_mouse_up, MouseButtonEventHandler polygon_mouse_down)//draws the line on the canvas
        {
            PointCollection polygonPoints = new PointCollection();

            for (int i = 0; i < pointsPolygon.Length; i++)
            {
                polygonPoints.Add( new Point(PanX + Zoom * Database.allPoints[PointsPolygon[i]].X, PanY - Zoom * Database.allPoints[PointsPolygon[i]].Y));
            }
            //we convert the line's 3d coordinates to isometric 2d ones, finding it's proection on the screen

            // ! не знам дали има нужда от тва да я питаме дали е цяла права или само сегмент !
            // ! или поне мисля че дори и да е цяла права ще я рисуваме като сегмент, просто сегмент между двата края на екрана !
            if(RealNormalVector.Z >= 0)
            {
                Polygon newPolygon = new Polygon();
                SolidColorBrush polygonBrush = new SolidColorBrush();
                newPolygon.Points = polygonPoints;
                newPolygon.MouseLeftButtonUp += polygon_mouse_up;
                newPolygon.MouseLeftButtonDown += polygon_mouse_down;
                if (!planeCnsPoly)
                {
                    Style polygonStyle = Application.Current.MainWindow.FindResource("PolygonStyle") as Style;
                    newPolygon.Style = polygonStyle;
                }
                else
                {
                    Style polygonStyle = Application.Current.MainWindow.FindResource("PlaneConstructionPolygon") as Style;
                    newPolygon.Style = polygonStyle;
                }
                Canvas.Children.Add(newPolygon);
                Canvas.SetZIndex(newPolygon, -3); 
            }
            else if(planeCnsPoly)
            {
                Polygon newPolygon = new Polygon();
                SolidColorBrush polygonBrush = new SolidColorBrush();
                newPolygon.Points = polygonPoints;
                newPolygon.MouseLeftButtonUp += polygon_mouse_up;
                newPolygon.MouseLeftButtonDown += polygon_mouse_down;
                Style polygonStyle = Application.Current.MainWindow.FindResource("PlaneConstructionPolygon") as Style;
                newPolygon.Style = polygonStyle;
                Canvas.Children.Add(newPolygon);
                Canvas.SetZIndex(newPolygon, -3);
            }          
    }
    }
    public class Coordsystem : BB_Polygon3D
    {
        public Coordsystem()
        {
            BB_Point3D point1 = new BB_Point3D(10, 0, 0, false);
            BB_Point3D point2 = new BB_Point3D(-10, 0, 0, false);
            BB_Point3D point3 = new BB_Point3D(0, 0, 10, false);
            BB_Point3D point4 = new BB_Point3D(0, 0, -10, false);
            BB_Point3D point5 = new BB_Point3D(0, 10, 0, false);
            BB_Point3D point6 = new BB_Point3D(0, -10, 0, false);

            Database.AddStaticPoint(point1);
            Database.AddStaticPoint(point2);
            Database.AddStaticPoint(point3);
            Database.AddStaticPoint(point4);
            Database.AddStaticPoint(point5);
            Database.AddStaticPoint(point6);
            BB_Line3D line1 = new BB_Line3D(0, 1, Colors.Green, 0.8);
            BB_Line3D line2 = new BB_Line3D(2, 3, Colors.Yellow, 0.8);
            BB_Line3D line3 = new BB_Line3D(4, 5, Colors.Red, 0.8);
            Database.addLine(line1);
            Database.addLine(line2);
            Database.addLine(line3);
            for (int i = -10; i <= 10; i++)
            {
                Database.addPoint(new BB_Point3D(i, 0, 0, i.ToString(), 5));
                Database.addPoint(new BB_Point3D(0, i , 0, i.ToString(), 5));
                Database.addPoint(new BB_Point3D(0, 0, i, i.ToString(), 5));
            }
            int[] pp = new int[69];
            for (int i = 0; i < 69; i++)
            {
                pp[i] = i;
            }
            PointsPolygon = pp;
            LinesPolygon = new int[3] { 0, 1, 2 };
        }
    }
    public class Grid : BB_Polygon3D
    {
        public Grid()
        {
            int[] pp = new int[84];
            int[] lp = new int[42];
            for (int i = -10; i <= 10; i++)
            {
                BB_Point3D point1 = new BB_Point3D(i, 0, -10, false);
                BB_Point3D point2 = new BB_Point3D(i, 0, 10, false);
                Database.AddStaticPoint(point1);
                Database.AddStaticPoint(point2);
                Database.addLine(new BB_Line3D(Database.allPoints.Count - 1, Database.allPoints.Count - 2, Colors.White, 0.2));
                BB_Point3D point3 = new BB_Point3D(10, 0, i, false);
                BB_Point3D point4 = new BB_Point3D(-10, 0, i, false);
                Database.AddStaticPoint(point3);
                Database.AddStaticPoint(point4);
                Database.addLine(new BB_Line3D(Database.allPoints.Count - 1, Database.allPoints.Count - 2, Colors.White, 0.2));

                pp[(i + 10) * 4] = Database.allPoints.Count - 4;
                pp[4 * (i + 10) + 1] = Database.allPoints.Count - 3;
                pp[(i + 10) * 4 + 2] = Database.allPoints.Count - 2;
                pp[(i + 10) * 4 + 3] = Database.allPoints.Count - 1;
                lp[i + 10] = Database.allLines.Count - 2;
                lp[i + 31] = Database.allLines.Count - 1;

            }

            PointsPolygon = pp;
            LinesPolygon = lp;
        }
    }
    public class BB_Object3D
    {
        //fields
        string name;
        int[] pointsObject;
        int[] linesObject;
        int[] polygonsObject;

        //constructors

        //properties
        public int[] Points { get { return pointsObject; } set { pointsObject = value; } }
        public int[] Lines { get { return linesObject; } set { linesObject = value; } }
        public int[] Polygons { get { return polygonsObject; } set { polygonsObject = value; } }
        //methods
        public static void CreateCube(Canvas canvas)
        {
            CreateRightPrism(2, 4, canvas);
        }

        public static void CreateRightPrism(double height, int nAngle, Canvas canvas)
        {
            BB_Polygon3D.CreateRegularPolygon(nAngle, 0, canvas);
            int IndexP1 = Database.allPolygons.Count - 1;
            BB_Polygon3D.CreateRegularPolygon(nAngle, height, canvas);
            int IndexP2 = Database.allPolygons.Count - 1;
            CreatePrism(IndexP1, IndexP2, canvas);
        }
        public static void CreatePrism(int IndexP1,int IndexP2, Canvas canvas)
        {
            int nAngle = Database.allPolygons[IndexP1].PointsPolygon.Length;
            BB_Prism prism = new BB_Prism(new int[2 * nAngle], new int[3 * nAngle], new int[2 + nAngle]);
            for (int i = 0; i < nAngle; i++)
            {
                prism.Points[i] = Database.allPolygons[IndexP1].PointsPolygon[i]; //adding all the polygon1's points(indexes) to the prism
                prism.Points[i + nAngle] = Database.allPolygons[IndexP2].PointsPolygon[i]; //adding all the polygon2's points(indexes) to the prism
                prism.Lines[i] = Database.allPolygons[IndexP1].LinesPolygon[i]; //adding all the P1's lines (as a 1st group of lines)              
                Database.addLine(new BB_Line3D(prism.Points[i], prism.Points[i + nAngle], 2.2, false)); // creating the Lateral Edges
                prism.Lines[i + nAngle] = Database.allLines.Count - 1;                    //adding the lateral Edges (as a 2nd group of lines)
                prism.Lines[i + 2 * nAngle] = Database.allPolygons[IndexP2].LinesPolygon[i]; //adding all the P2's lines (as a 3th group of lines)
            }
            for (int i = 0; i < nAngle; i++)
            {
                int p1 = Database.allPolygons[IndexP1].PointsPolygon[i];
                int p12 = Database.allPolygons[IndexP1].PointsPolygon[(i + 1) % nAngle];
                int p2 = Database.allPolygons[IndexP2].PointsPolygon[i];
                int p22 = Database.allPolygons[IndexP2].PointsPolygon[(i + 1) % nAngle];
                Database.addPolygon(new BB_Polygon3D(new int[4] { p1, p12, p22, p2 },
                        new int[4] { prism.Lines[i], prism.Lines[(i + 1) % nAngle + nAngle], prism.Lines[i + 2 * nAngle], prism.Lines[i + nAngle] }));
                prism.Polygons[2 + i] = Database.allPolygons.Count - 1;
            }
            prism.Polygons[2 + nAngle - 1] = IndexP2;
            Database.addObjects(prism);
        }
       
        public static void CreatePyramid(int IndexP1,int PointIndex , Canvas canvas)
        {
            int nAngle = Database.allPolygons[IndexP1].PointsPolygon.Length;       
            
            BB_Pyramid pyramid = new BB_Pyramid(new int[nAngle + 1], new int[2 * nAngle], new int[1 + nAngle]);
            pyramid.Polygons[0] = IndexP1;
            pyramid.Points[nAngle] = PointIndex;
            for (int i = 0; i < nAngle; i++)
            {
                pyramid.Points[i] = Database.allPolygons[IndexP1].PointsPolygon[i]; //adding all the polygon1's points(indexes) to the prism                                                                                    //adding all the polygon2's points(indexes) to the prism
                pyramid.Lines[i] = Database.allPolygons[IndexP1].LinesPolygon[i]; //adding all the P1's lines (as a 1st group of lines)              
                Database.addLine(new BB_Line3D(pyramid.Points[i], pyramid.Points[nAngle], 2.2, false)); // creating the Lateral Edges
                pyramid.Lines[i + nAngle] = Database.allLines.Count - 1;                    //adding the lateral Edges (as a 2nd group of lines)
            }

            for (int i = 0; i < nAngle; i++)
            {
                int p1 = Database.allPolygons[IndexP1].PointsPolygon[i];
                int p12 = Database.allPolygons[IndexP1].PointsPolygon[(i + 1) % nAngle];
                Database.addPolygon(new BB_Polygon3D(new int[3] { p1, p12, pyramid.Points.Last() },
                        new int[3] { pyramid.Lines[i], pyramid.Lines[(i + 1) % nAngle + nAngle], pyramid.Lines[i + nAngle] }));
                pyramid.Polygons[1 + i] = Database.allPolygons.Count - 1;
            }
            Database.addObjects(pyramid);
        }
        public static void CreateRightPyramid(double height, int nAngle, Canvas canvas)
        {
            BB_Polygon3D.CreateRegularPolygon(nAngle, 0, canvas);
            int IndexP1 = Database.allPolygons.Count - 1;
            BB_Point3D point = new BB_Point3D(0, height, 0);
            Database.AddStaticPoint(point);
            CreatePyramid(IndexP1,Database.allPoints.Count -1 , canvas);
        }
        public static void CreateTetrahedron(Canvas canvas)
        {
            CreateRightPyramid(2 * Math.Sqrt(2), 3, canvas);
        }
        //inner classes
        public class BB_Prism : BB_Object3D
        {
            double height;
            double generatrix;
            // obrazuvashta
            public BB_Prism(int[] points, int[] lines, int[] polygons)
            {
                //this.height = height;
              //  this.generatrix = generatrix;
                Points = points;
                Lines = lines;
                Polygons = polygons;
            }
            public double Generatrix { get { return generatrix; } set { generatrix = value; } }
            public double Height { get { return height; } set { height = value; } }

        }
        public class BB_Pyramid : BB_Object3D
        {
            double height;
            double apothem;
            public BB_Pyramid(int[] points, int[] lines, int[] polygons)
            {
               // this.height = height;
                Points = points;
                Lines = lines;
                Polygons = polygons;
            }
            public double Generatrix { get { return apothem; } set { apothem = value; } }
            public double Height { get { return height; } set { height = value; } }
            
        }
    
    }
}