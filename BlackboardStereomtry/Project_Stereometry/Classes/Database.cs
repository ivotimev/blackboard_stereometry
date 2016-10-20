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
    public static class Database
    {

        public static List<BB_Point3D> allPoints = new List<BB_Point3D>();
        /*  public static BB_Point3D 
              GetRealPointCoords(int index)
          {
              BB_Point3D point = new BB_Point3D(allPoints[index].X, allPoints[index].Y, allPoints[index].Z);
              return BB_Camera.TransformToStaticCoord(point);
          }*/
        public static void AddStaticPoint(BB_Point3D point)
        {
            Point3D NewPoint = new Point3D(point.X, point.Y, point.Z);
            point.Xstatic = NewPoint.X;
            point.Ystatic = NewPoint.Y;
            point.Zstatic = NewPoint.Z;
            allPoints.Add(point);
            point.Index = allPoints.Count - 1;           
        }
        public static void addPoint(BB_Point3D point)
        {
            Point3D StaticCoord = new Point3D(point.X, point.Y, point.Z);
            StaticCoord = BB_Camera.TransformToStaticCoord(StaticCoord);
            point.AddStaticCoordinates(StaticCoord);
           
            allPoints.Add(point);
            point.Index = allPoints.Count - 1;

        }
        public static List<BB_Line3D> allLines = new List<BB_Line3D>();
        public static void addLine(BB_Line3D line)
        {
            allLines.Add(line);

        }
        public static List<BB_AuxiLine3D> AuxiLines = new List<BB_AuxiLine3D>();
        public static void addAuxiLine(BB_AuxiLine3D line)
        {
            AuxiLines.Add(line);

        }
        public static List<BB_Polygon3D> allPolygons = new List<BB_Polygon3D>();
        public static BB_Polygon3D hitTestPolygon = new BB_Polygon3D();

        public static void addPolygon(BB_Polygon3D polygon)
        {
            allPolygons.Add(polygon);
        }
        public static List<BB_Object3D> allObjects = new List<BB_Object3D>();

        public static void addObjects(BB_Object3D Object)
        {
            allObjects.Add(Object);
            BB_Polygon3D.VisibleFace();
        }
        public static void DeleteObject(int n)
        {
            for (int i = allObjects[n].Points.Length-1; i >=0; i--)
            {
                int ndqq = allObjects[n].Points[i];
                if (ndqq > FirstPointIndex) allPoints.RemoveAt(ndqq);
            }
            for (int i = allObjects[n].Lines.Length - 1; i >= 0; i--)
            {
                int ndqq = allObjects[n].Lines[i];
                allLines.RemoveAt(ndqq);
            }
            for (int i = allObjects[n].Polygons.Length - 1; i >= 0; i--)
            {
                int ndqq = allObjects[n].Polygons[i];
                allPolygons.RemoveAt(ndqq);
            }
            allObjects.RemoveAt(n);
            
        }
        public static void DeletePolygon(int n)
        {
            for (int i = 0; i < allPolygons[n].PointsPolygon.Length; i++)
            {
                if (allPolygons[n].PointsPolygon[i] > FirstPointIndex) allPoints.RemoveAt(allPolygons[n].PointsPolygon[i]);
            }
            for (int i = 0; i < allPolygons[n].LinesPolygon.Length; i++)
            {
                allLines.RemoveAt(allPolygons[n].LinesPolygon[i]);
            }
            allPolygons.RemoveAt(n);
        }
        public static Coordsystem CoordSystem = new Coordsystem();
         // public static Grid Grid = new Grid();

      //  public static int AuxiLineIndex;
        public static void DrawInitialObjects(Canvas canvas)
        {
            BB_Camera.Draw(canvas);
        }


        public static int FirstPointIndex = CoordSystem.PointsPolygon.Length;//+ Grid.PointsPolygon.Length;
        public static int PointNumber(int lastindex)
        {
            return lastindex - FirstPointIndex ;
        }

        public static string SmartName()
        {
            int i = 1;
            int lastindex = allPoints.Count - 1;
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            if (PointNumber(lastindex + 1) < 26) return alpha[PointNumber(lastindex + 1)].ToString();
            else { return alpha[PointNumber(lastindex + 1)%26].ToString() + i.ToString(); i++; }
        }
        
    }
}
