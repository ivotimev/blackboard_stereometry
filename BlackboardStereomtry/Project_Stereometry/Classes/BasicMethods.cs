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
    public static class BasicMethods
    {
        public static void DeleteAllObjects()
        {
            int NumberDefPoints = Database.CoordSystem.PointsPolygon.Length;// + Database.Grid.PointsPolygon.Length;
            int NumberDefLines = Database.CoordSystem.LinesPolygon.Length;// + Database.Grid.LinesPolygon.Length;
           
            Database.allPoints.RemoveRange(NumberDefPoints, Database.allPoints.Count - NumberDefPoints);
            Database.allLines.RemoveRange(NumberDefLines, Database.allLines.Count - NumberDefLines);
            Database.allPolygons.Clear();
            Database.allObjects.Clear();
            Database.AuxiLines.Clear();

        }
        public static void AddPoint(double x, double y, double z)
        {
            Database.AddStaticPoint(new BB_Point3D(x, y, z));
            
        }

        public static void LineIntersection()
        {
            Database.AuxiLines.Clear();
            for (int i = 0; i < Database.allLines.Count; i++)
            {

                if (Database.allLines[i].Lonely == true)
                {
                    Point3D P0 = Database.allLines[i].PointVectors()[0];
                    Point3D u = Database.allLines[i].PointVectors()[2];       
                    List<double> listSI = new List<double>();
                  //  Database.allLines[i].OnAVisibleSide = false;
                    
                    for (int j = 0; j < Database.allPolygons.Count; j++)
                    {
                        if (Database.allPolygons[j].RealNormalVector.Z >= 0)
                        {
                            if (Database.allPolygons[j].LiesOnthePlane(Database.allLines[i])) { listSI.Clear(); break;  }
                            if (Database.allPolygons[j].Pointof2DIntersection(Database.allLines[i]) != null)
                            {
                                if (Database.allPolygons[j].Pointof3DIntersection(Database.allLines[i]) != 0)
                                {
                                    listSI.Add(Database.allPolygons[j].Pointof3DIntersection(Database.allLines[i]));
                                }
                                listSI.AddRange(Database.allPolygons[j].Pointof2DIntersection(Database.allLines[i]));
                            }
                        }
                    }                       
                    if(listSI.Count!=0)
                    {
                        Database.allLines[i].Dashed = true; 
                        listSI.Sort();
                        Point3D point1 = new Point3D(P0.X + listSI[0] * u.X, P0.Y + listSI[0] * u.Y, P0.Z + listSI[0] * u.Z);
                        Point3D point2 = new Point3D(P0.X + listSI.Last() * u.X, P0.Y + listSI.Last() * u.Y, P0.Z + listSI.Last() * u.Z);

                         if (listSI[0] != 0)
                        Database.addAuxiLine(new BB_AuxiLine3D(Database.allLines[i].PointVectors()[0], point1));
                         if (listSI.Last() != 1) 
                        Database.addAuxiLine(new BB_AuxiLine3D(Database.allLines[i].PointVectors()[1], point2));                          
                    }
                    else Database.allLines[i].Dashed = false;// Database.AuxiLines.Clear();
                }
                }
            }

        public static BB_Point3D FindThePointOntheBasis(Point point2d)
        {
            double X = Math.Round((-BB_Camera.x_offset + point2d.X)/BB_Camera.Zoom,4);
            double Y = Math.Round((BB_Camera.y_offset - point2d.Y)/ BB_Camera.Zoom,4);
            Point3D vec1 = new Point3D(0, 1, 0);
            Point3D vec2 = new Point3D(0, 0, 1);
            vec1=BB_Camera.TransformToDynamicCoord(vec1);
            vec2 = BB_Camera.TransformToDynamicCoord(vec2);
            double d = -(vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z);
            double z = (-d - vec1.X * X - vec1.Y * Y) / vec1.Z;

            return new BB_Point3D(X, Y, z,"",8);
        }
        public static void CopyPolygon(BB_Polygon3D Polygon)
        {
            int n = Polygon.PointsPolygon.Length;
            int[] points = new int[n];
            int[] lines = new int[n];
            for (int i = 0; i < n; i++)
            {
                BB_Point3D point = new BB_Point3D(Database.allPoints[Polygon.PointsPolygon[i]].X, Database.allPoints[Polygon.PointsPolygon[i]].Y , Database.allPoints[Polygon.PointsPolygon[i]].Z,"", 8);
                //  Database.addPoint(Database.allPoints[Polygon.PointsPolygon[i]]);
                Database.addPoint(point);
                points[i] = Database.allPoints.Count - 1;
//Database.addLine(new BB_Line3D(points[i], points[0] + i%(n-1)));
               // lines[i] = Database.allLines.Count - 1;
            }
            for(int i=0;i< n;i++)
            {
                Database.addLine(new BB_Line3D(points[i], points[(i+1)%n], 2.2,false));
                lines[i] = Database.allLines.Count - 1;
            }
            Database.addPolygon(new BB_Polygon3D(points, lines));

        }
        public static bool GotoRoundCoords(double Number)
        {
            int p = (int)(Math.Round(Number,2)*100);            
            if ((p % 100 >= 95 || p % 100 <= 05)) return true;
            else return false;

        }
        public static bool ReversPosition(int p1 , int p2, int p3 )
        {
            //first position - Vec1
            //Vector Vec0 = new Vector(Database.allPoints[p0].X, Database.allPoints[p0].Y);
            Vector3D Vec1 = new Vector3D(Database.allPoints[p1].X - Database.allPoints[p2].X, Database.allPoints[p1].Y - Database.allPoints[p2].Y, Database.allPoints[p1].Z - Database.allPoints[p2].Z);
            Vector3D Vec2 = new Vector3D(Database.allPoints[p3].X - Database.allPoints[p2].X, Database.allPoints[p3].Y - Database.allPoints[p2].Y, Database.allPoints[p3].Z - Database.allPoints[p2].Z);
            //second position - difference
            Vector3D Normal = Vector3D.CrossProduct(Vec2, Vec1);
            if(Normal.Z<0) return true;
             else return false;
            //return tgPhi;
        }
        public static BB_Point3D CalculateCentroid(BB_Polygon3D polygon)
        {
            double A = 0;
            int nAngle = polygon.PointsPolygon.Count();
            double Cx = 0;
            double Cz = 0;
            for (int i = 0; i < nAngle; i++)
            {
                BB_Point3D bb_point1 = Database.allPoints[polygon.PointsPolygon[i]];
                BB_Point3D bb_point2 = Database.allPoints[polygon.PointsPolygon[(i + 1) % nAngle]];
                double x1 = bb_point1.Xstatic;
                double y1 = bb_point1.Zstatic;
                double x2 = bb_point2.Xstatic;
                double y2 = bb_point2.Zstatic;

                double expressionA = (x1 * y2 - x2 * y1);
                A += expressionA;
                Cx += (x1 + x2) * expressionA;
                Cz += (y1 + y2) * expressionA;
            }
            A = 0.5 * A;
            Cx = Cx / (6 * A);
            Cz = Cz / (6 * A);           
            //MessageBox.Show(string.Format("{0},{1},{2},{3}", A, Cx, Cz, nAngle));
                       
          //  BB_Point3D point = new BB_Point3D(1, 0, 1);
          //  point.Radius = 8;
            return new BB_Point3D(Cx, 0, Cz,8);
        }
        public static BB_Point3D CalculateCentroid2(BB_Polygon3D polygon)
        {
            double Cx = 0;
            for (int i = 0; i < polygon.PointsPolygon.Count(); i++)
            {
                Cx += Database.allPoints[polygon.PointsPolygon[i]].Xstatic;
            }
            Cx /= polygon.PointsPolygon.Count();
            double Cz = 0;
            for (int i = 0; i < polygon.PointsPolygon.Count() - 1; i++)
            {
                Cz += Database.allPoints[polygon.PointsPolygon[i]].Zstatic;
            }
            Cz /= polygon.PointsPolygon.Count();
            return new BB_Point3D(Cx, 0, Cz, 11);
        }

        public static BB_Point3D FindPointOnLine()
        {

            return new BB_Point3D();
        }
    }       
}
