using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Project_Stereometry.Classes;

namespace Project_Stereometry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BB_Camera.SetMouseEventHandlers(new MouseButtonEventHandler(Shapes_mouse_down), new MouseEventHandler(MainCanvas_MouseMove), new MouseButtonEventHandler(MainCanvas_MouseLeftButtonUp));
        }
   
        Point mouseDown_Position;
        Point mouse_GetPosition;
        public bool mouseDown = false;
       // TextBlock text1 = new TextBlock();
       // TextBlock text = new TextBlock();
        List<int> PolygonIndexes = new List<int>();
        bool MakePrism_Clicked = false;
        bool MakePyramid_Clicked = false;
        bool Phase2AuxiBool = false; // used when clicking in the second phase
        bool OnElipse;
        int Phase;//completed phases of building Prism
        int SelectedIndex;
        Point3D AuxiPoint; //makingpyramid phase 2
        
        private void Show_Message(string title, string content)
        {
            HelpBox_Title.Text = title;
            HelpBox_Content.Text = content;
            HelpBox.Visibility = Visibility.Visible;
        }

        private void Shapes_mouse_down(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse)
            {
                Ellipse clicked = sender as Ellipse;
                SelectedIndex = int.Parse(clicked.Name.Remove(0, 1));
                if ((bool)i_make_point.IsChecked)
                    if (e.RoutedEvent != MouseUpEvent) { Database.allPoints.RemoveAt(Database.allPoints.Count - 1); mouseDown = false; }
                if ((bool)i_hand.IsChecked)
                {
                    // Database.allPoints[SelectedIndex].Selected = true;
                    // BB_Camera.Draw(MainCanvas, new MouseButtonEventHandler(Shapes_mouse_down), new MouseEventHandler(MainCanvas_MouseMove), new /MouseButtonEventHandler(MainCanvas_MouseLeftButtonUp));
                }
                if (e.RoutedEvent != MouseUpEvent)
                {
                    if ((bool)i_make_line.IsChecked)
                    {
                        if (OnElipse)
                        {
    
                            PolygonIndexes.Add(SelectedIndex); Database.allPoints[SelectedIndex].Radius = 8;
                            if (PolygonIndexes.Count == 2)
                            {
                                Database.addLine(new BB_Line3D(PolygonIndexes[PolygonIndexes.Count - 2], PolygonIndexes.Last(), 2.2, true));
                                PolygonIndexes.Clear();
                            }

                            BB_Camera.Draw(MainCanvas);

                        }
                    }
                        if (MakePyramid_Clicked)
                        {
                        if (PolygonIndexes.Count > 2)
                        {
                            if (clicked.Name == "p" + PolygonIndexes[0].ToString())
                            {
                                Phase2AuxiBool = true;
                                Phase = 2;

                                Show_Message("Построяване на пирамида: Височина", "Издигнете височината на пирамидата чрез преместване на мишката. Потвърдете с ляв бутон. Ако искате пирамидата ви да е права натиснете два пъти.");
                                Database.addLine(new BB_Line3D(PolygonIndexes.Last(), PolygonIndexes[0], 2.2, false));
                                //bool b = false;
                                if (BasicMethods.ReversPosition(PolygonIndexes[0], PolygonIndexes[1], PolygonIndexes[2])) { PolygonIndexes.Reverse();}
                                int[] Lines = new int[PolygonIndexes.Count];

                                for (int i = 0; i < PolygonIndexes.Count; i++)
                                {
                                    Database.allLines[Database.allLines.Count - PolygonIndexes.Count + i].Points[0] = PolygonIndexes[i];
                                    Database.allLines[Database.allLines.Count - PolygonIndexes.Count + i].Points[1] = PolygonIndexes[(i + 1) % PolygonIndexes.Count];
                                    Lines[i] = Database.allLines.Count - PolygonIndexes.Count + i;

                                }
                                Database.addPolygon(new BB_Polygon3D(PolygonIndexes.ToArray(), Lines));
                                Database.allPolygons.Last().Basis = true;
                                BB_Point3D centroid = BasicMethods.CalculateCentroid(Database.allPolygons.Last());
                                Database.AddStaticPoint(centroid);
                                AuxiPoint = Database.allPoints.Last().toPoint3D();
                                mouseDown_Position = Mouse.GetPosition(MainCanvas);
                                mouse_GetPosition = Mouse.GetPosition(MainCanvas);
                                BB_Camera.Draw(MainCanvas);

                            }
                            else if (Phase == 1 && OnElipse)
                            {
                                if (Database.allPoints[SelectedIndex].Ystatic == 0)
                                {
                                    PolygonIndexes.Add(SelectedIndex);
                                    if (PolygonIndexes.Count > 1)
                                    {
                                        Database.addLine(new BB_Line3D(PolygonIndexes[PolygonIndexes.Count - 2], PolygonIndexes.Last(), 2.2, false));
                                    }
                                    BB_Camera.Draw(MainCanvas);
                                }
                            }
                        }
                        else if (Phase == 1 && OnElipse)
                        {
                            if (Database.allPoints[SelectedIndex].Ystatic == 0)
                            {
                                PolygonIndexes.Add(SelectedIndex);
                                if (PolygonIndexes.Count > 1)
                                {
                                    Database.addLine(new BB_Line3D(PolygonIndexes[PolygonIndexes.Count - 2], PolygonIndexes.Last(), 2.2, false));
                                }
                                BB_Camera.Draw(MainCanvas);
                            }
                        }
                    
                }
                    if (MakePrism_Clicked)
                    {
                        if (PolygonIndexes.Count > 2)
                        {
                            if (clicked.Name == "p" + PolygonIndexes[0].ToString())
                            {
                                Phase2AuxiBool = true;
                                Phase = 2;

                                Show_Message("Построяване на призма: Височина", "Издигнете височината на призмата чрез преместване на мишката. Потвърдете с ляв бутон. Ако искате призмата ви да е права натиснете два пъти.");
                                Database.addLine(new BB_Line3D(PolygonIndexes.Last(), PolygonIndexes[0], 2.2, false));
                                //bool b = false;
                                if (BasicMethods.ReversPosition(PolygonIndexes[0], PolygonIndexes[1], PolygonIndexes[2])) { PolygonIndexes.Reverse();}
                                int[] Lines = new int[PolygonIndexes.Count];

                                for (int i = 0; i < PolygonIndexes.Count; i++)
                                {
                                    Database.allLines[Database.allLines.Count - PolygonIndexes.Count + i].Points[0] = PolygonIndexes[i];
                                    Database.allLines[Database.allLines.Count - PolygonIndexes.Count + i].Points[1] = PolygonIndexes[(i + 1) % PolygonIndexes.Count];
                                    Lines[i] = Database.allLines.Count - PolygonIndexes.Count + i;

                                }

                                Database.addPolygon(new BB_Polygon3D(PolygonIndexes.ToArray(), Lines));
                                Database.allPolygons.Last().Basis = true;
                                BasicMethods.CopyPolygon(Database.allPolygons.Last());
                                mouseDown_Position = Mouse.GetPosition(MainCanvas);
                                mouse_GetPosition = Mouse.GetPosition(MainCanvas);

                            }
                            else if (Phase == 1 && OnElipse)
                            {
                                if (Database.allPoints[SelectedIndex].Ystatic == 0)
                                {
                                    PolygonIndexes.Add(SelectedIndex);
                                    if (PolygonIndexes.Count > 1)
                                    {
                                        Database.addLine(new BB_Line3D(PolygonIndexes[PolygonIndexes.Count - 2], PolygonIndexes.Last(), 2.2, false));
                                    }
                                    BB_Camera.Draw(MainCanvas);
                                }
                            }
                        }
                        else if (Phase == 1 && OnElipse)
                        {
                            if (Database.allPoints[SelectedIndex].Ystatic == 0)
                            {
                                PolygonIndexes.Add(SelectedIndex);
                                if (PolygonIndexes.Count > 1)
                                {
                                    Database.addLine(new BB_Line3D(PolygonIndexes[PolygonIndexes.Count - 2], PolygonIndexes.Last(), 2.2, false));
                                }
                                BB_Camera.Draw(MainCanvas);
                            }
                        }
                    }
                }
                
            }
        }
        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown_Position = Mouse.GetPosition(MainCanvas);
            if (MakePrism_Clicked)
            {
                if (Phase == 1)
                {
                    if (OnElipse == false)
                    {
                    Database.addPoint(BasicMethods.FindThePointOntheBasis(mouseDown_Position));
                    PolygonIndexes.Add(Database.allPoints.Count - 1);
                    if (PolygonIndexes.Count > 1)
                    {
                            Database.addLine(new BB_Line3D(PolygonIndexes[PolygonIndexes.Count - 2], PolygonIndexes.Last(), 2.2, false));
                    }
                    BB_Camera.Draw(MainCanvas);
                }
                    
                }
                if (Phase == 3)
                {
                    BB_Object3D.CreatePrism(Database.allPolygons.Count - 2, Database.allPolygons.Count - 1, MainCanvas);
                    MakePrism_Clicked = false; i_hand.IsChecked = true;
                    HelpBox.Visibility = Visibility.Collapsed;
                    MakePrismActive.Visibility = Visibility.Collapsed;
                    BB_Camera.Draw(MainCanvas);
                }
                
                if (Phase == 2 && !Phase2AuxiBool)
                {
                    Phase = 3;// MakePrism_Clicked = false;
                    Show_Message("Построяване на призма: Наклон", "Изберете положение за горната основа на призмата и потвърдете с ляв бутон. Така ще получите наклонена призма.");
                    Phase2AuxiBool = false;


                }                               
            }
            if (MakePyramid_Clicked)
            {
                if (Phase == 1)
                {
                    if (OnElipse == false)
                    {
                        Database.addPoint(BasicMethods.FindThePointOntheBasis(mouseDown_Position));
                        PolygonIndexes.Add(Database.allPoints.Count - 1);
                        if (PolygonIndexes.Count > 1)
                        {
                            Database.addLine(new BB_Line3D(PolygonIndexes[PolygonIndexes.Count - 2], PolygonIndexes.Last(), 2.2, false));
                        }
                        BB_Camera.Draw(MainCanvas);
                    }

                }
                if (Phase == 3)
                {
                    BB_Object3D.CreatePyramid(Database.allPolygons.Count - 1, Database.allPoints.Count - 1, MainCanvas);
                    MakePyramid_Clicked = false; i_hand.IsChecked = true;
                    PolygonIndexes.Clear();
                    HelpBox.Visibility = Visibility.Collapsed;
                    MakePyramidActive.Visibility = Visibility.Collapsed;
                    BB_Camera.Draw(MainCanvas);
                }

                if (Phase == 2 && !Phase2AuxiBool)
                {
                    Phase = 3;// MakePrism_Clicked = false;
                    Show_Message("Построяване на пирамида: Наклон", "Изберете положение за върха на пирамидата и потвърдете с ляв бутон. Така ще получите наклонена пирамида.");
                    Phase2AuxiBool = false;


                }

            }
            if ((bool)i_make_point.IsChecked)
            {
                //  if (OnElipse==false&&(sender is Ellipse)==false)
                {

                    //Database.addPoint(BasicMethods.FindThePointOntheBasis(Mouse.GetPosition(MainCanvas)));
                    Database.addPoint(BasicMethods.FindThePointOntheBasis(Mouse.GetPosition(MainCanvas)));
                    BB_Camera.Draw(MainCanvas);
                    mouse_GetPosition = Mouse.GetPosition(MainCanvas);
                    mouseDown = true;
                    mouseDown_Position = Mouse.GetPosition(MainCanvas);
                }
            }
            if ((bool)i_hand.IsChecked)//|| (bool)i_make_point.IsChecked)
            {
                mouseDown = true;              
                BB_Camera.Draw(MainCanvas);
            }
            if ((bool)i_make_line.IsChecked)
            {
                if (OnElipse == false)
                {
                    Database.addPoint(BasicMethods.FindThePointOntheBasis(mouseDown_Position));
                    PolygonIndexes.Add(Database.allPoints.Count - 1);
                    if (PolygonIndexes.Count > 1)
                    {
                        Database.addLine(new BB_Line3D(PolygonIndexes[PolygonIndexes.Count - 2], PolygonIndexes.Last(), 2.2, true));
                    }
                    if (PolygonIndexes.Count == 2) PolygonIndexes.Clear();
                    BB_Camera.Draw(MainCanvas);
                }
            }


        }

        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            mouseDown = false;
            if ((bool)i_make_point.IsChecked) BB_Camera.Draw(MainCanvas);
        }

        private void MainCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Viewport_Grid_Scale.ScaleX += (e.Delta / Math.Abs(e.Delta)) * 0.1;
            Viewport_Grid_Scale.ScaleY += (e.Delta / Math.Abs(e.Delta)) * 0.1;

            BB_Camera.Zoom = BB_Camera.Zoom + (e.Delta / Math.Abs(e.Delta)) * 10;
            MainCanvas.Children.Clear();
            BB_Camera.Draw(MainCanvas);
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e) // MOUSE MOVE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            if (mouseDown && Keyboard.IsKeyDown(Key.Z))
            {
                BB_Camera.x_offset -= Math.Round(mouseDown_Position.X - Mouse.GetPosition(MainCanvas).X);
                BB_Camera.y_offset -= Math.Round(mouseDown_Position.Y - Mouse.GetPosition(MainCanvas).Y);
                Viewport_Grid_Translate.X -= Math.Round(mouseDown_Position.X - Mouse.GetPosition(MainCanvas).X);
                Viewport_Grid_Translate.Y -= Math.Round(mouseDown_Position.Y - Mouse.GetPosition(MainCanvas).Y);
                MainCanvas.Children.Clear();
                BB_Camera.Draw(MainCanvas);
                mouseDown_Position = Mouse.GetPosition(MainCanvas);


            }
            if (sender is Ellipse)
            {
                Ellipse clicked = sender as Ellipse;
                if (e.RoutedEvent == MouseEnterEvent)
                {

                    OnElipse = true;
                    clicked.RenderTransformOrigin = new Point(0.5, 0.5);
                    clicked.RenderTransform = new ScaleTransform(2, 2);
                    if (Phase == 1 && PolygonIndexes.Count > 0) //creating auxilines in phase 1
                    {
                        SelectedIndex = int.Parse(clicked.Name.Remove(0, 1));
                        mouseDown_Position = Mouse.GetPosition(MainCanvas);
                        BB_AuxiLine3D AuxiLine = new BB_AuxiLine3D(Database.allPoints[PolygonIndexes.Last()].toPoint3D(), Database.allPoints[SelectedIndex].toPoint3D());
                        MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                        AuxiLine.Draw(MainCanvas, BB_Camera.Zoom, BB_Camera.x_offset, BB_Camera.y_offset);
                    }
                }
                else if (e.RoutedEvent == MouseLeaveEvent)
                {
                    OnElipse = false;
                    clicked.RenderTransform = new ScaleTransform(1, 1);
                }
            }
            else if (MakePrism_Clicked) // MAKE PRISM!!!!!!!!!!!!!!!!!!!!!!!!!!!
            {
                if (Phase == 1 && PolygonIndexes.Count > 0 && OnElipse == false) //creating auxilines in phase 1
                {
                    
                    mouseDown_Position = Mouse.GetPosition(MainCanvas);
                    BB_AuxiLine3D AuxiLine = new BB_AuxiLine3D(Database.allPoints[PolygonIndexes.Last()].toPoint3D(), BasicMethods.FindThePointOntheBasis(mouseDown_Position).toPoint3D());
                    if (Phase2AuxiBool == true) MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1); //using phase2auxibool for another reason(checking if there is an Auxiline to remove
                    Phase2AuxiBool = true;
                    AuxiLine.Draw(MainCanvas, BB_Camera.Zoom, BB_Camera.x_offset, BB_Camera.y_offset);
                }
                if (Phase == 2) //starting second phase
                {
                    
                    BB_Point3D point = new BB_Point3D();
                    Phase2AuxiBool = false;
                    //if (Mouse.GetPosition(MainCanvas).Y < mouseDown_Position.Y)
                    for (int i = 0; i < Database.allPolygons.Last().PointsPolygon.Length; i++)
                    {                        
                        Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Ystatic += (mouse_GetPosition.Y - Mouse.GetPosition(MainCanvas).Y) / BB_Camera.Zoom;
                        if (Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Ystatic < 0) Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Ystatic = 0;
                        if (BasicMethods.GotoRoundCoords(Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Ystatic))
                        Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Ystatic = Math.Round((Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Ystatic));
                       // else Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Ystatic = Math.Round((Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Ystatic),2);
                        
                    }
                    BB_AuxiLine3D AuxiLine = new BB_AuxiLine3D(Database.allPoints[Database.allPolygons.Last().PointsPolygon.Last()].toPoint3D(), Database.allPoints[Database.allPolygons[Database.allPolygons.Count - 2].PointsPolygon.Last()].toPoint3D());
                    AuxiLine.dashed = true;
                    BB_Camera.Draw(MainCanvas);
                    AuxiLine.Draw(MainCanvas, BB_Camera.Zoom, BB_Camera.x_offset, BB_Camera.y_offset);
                    double Ystatic = Database.allPoints.Last().Ystatic;
                    Database.allPoints.Last().AddTextNearthePoint(MainCanvas, Math.Round(Ystatic, 2).ToString());
                    bool b = BasicMethods.GotoRoundCoords(Database.allPoints.Last().Ystatic);
                    if (!b) mouse_GetPosition = Mouse.GetPosition(MainCanvas);
                }
                if (Phase == 3)
                {
                   // double X = Math.Round((-BB_Camera.x_offset + Mouse.GetPosition(MainCanvas).X) / BB_Camera.Zoom, 4);
                    //double Y = Math.Round((BB_Camera.y_offset - Mouse.GetPosition(MainCanvas).Y) / BB_Camera.Zoom, 4);
                    for (int i = 0; i < Database.allPolygons.Last().PointsPolygon.Length; i++)
                    {
                        Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].X -= (mouse_GetPosition.X - Mouse.GetPosition(MainCanvas).X) / BB_Camera.Zoom;
                        Database.allPoints[Database.allPolygons.Last().PointsPolygon[i]].Y += (mouse_GetPosition.Y - Mouse.GetPosition(MainCanvas).Y) / BB_Camera.Zoom;
                    }
                    BB_Camera.Draw(MainCanvas);
                    mouse_GetPosition = Mouse.GetPosition(MainCanvas);
                }
            }
            else if (MakePyramid_Clicked)                 // MAKE PYRAMID !!!!!!!!!!!!!!!!!!!!!!!!1
            {
                if (Phase == 1 && PolygonIndexes.Count > 0 && OnElipse == false) //creating auxilines in phase 1
                {

                    mouseDown_Position = Mouse.GetPosition(MainCanvas);
                    BB_AuxiLine3D AuxiLine = new BB_AuxiLine3D(Database.allPoints[PolygonIndexes.Last()].toPoint3D(), BasicMethods.FindThePointOntheBasis(mouseDown_Position).toPoint3D());
                    if (Phase2AuxiBool == true) MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                    Phase2AuxiBool = true;
                    AuxiLine.Draw(MainCanvas, BB_Camera.Zoom, BB_Camera.x_offset, BB_Camera.y_offset);
                }
                if (Phase == 2) //starting second phase
                {
                    BB_Point3D point = new BB_Point3D();
                    Phase2AuxiBool = false;

                        Database.allPoints.Last().Ystatic += (mouse_GetPosition.Y - Mouse.GetPosition(MainCanvas).Y) / BB_Camera.Zoom;
                        if (Database.allPoints.Last().Ystatic < 0) Database.allPoints.Last().Ystatic = 0;
                        if (BasicMethods.GotoRoundCoords(Database.allPoints.Last().Ystatic))
                        Database.allPoints.Last().Ystatic = Math.Round(Database.allPoints.Last().Ystatic);
                  
                    BB_AuxiLine3D AuxiLine = new BB_AuxiLine3D(AuxiPoint, Database.allPoints.Last().toPoint3D());
                    AuxiLine.dashed = true;
                    BB_Camera.Draw(MainCanvas);
                    AuxiLine.Draw(MainCanvas, BB_Camera.Zoom, BB_Camera.x_offset, BB_Camera.y_offset);
                    double Ystatic = Database.allPoints.Last().Ystatic;
                    Database.allPoints.Last().AddTextNearthePoint(MainCanvas, Math.Round(Ystatic, 2).ToString());
                    bool b = BasicMethods.GotoRoundCoords(Database.allPoints.Last().Ystatic);
                    if (!b) mouse_GetPosition = Mouse.GetPosition(MainCanvas);
                }
                if (Phase == 3)
                {
                        Database.allPoints.Last().X -= (mouse_GetPosition.X - Mouse.GetPosition(MainCanvas).X) / BB_Camera.Zoom;
                        Database.allPoints.Last().Y += (mouse_GetPosition.Y - Mouse.GetPosition(MainCanvas).Y) / BB_Camera.Zoom;
                    
                    BB_Camera.Draw(MainCanvas);
                    mouse_GetPosition = Mouse.GetPosition(MainCanvas);
                }
            }
            if ((bool)i_hand.IsChecked)
                if (mouseDown)
            {
                    double angleX = (mouseDown_Position.Y - Mouse.GetPosition(MainCanvas).Y) / 2;
                    double angleY = (mouseDown_Position.X - Mouse.GetPosition(MainCanvas).X) / 2;
                    BB_Camera.MassRotation(angleX, angleY, 0);
                    rotateX.Angle += angleX;
                    rotateY.Axis = new Vector3D(0, Math.Cos((rotateX.Angle) / 180 * Math.PI), Math.Sin((rotateX.Angle) / 180 * Math.PI));
                    rotateY.Angle += angleY;
                BasicMethods.LineIntersection();
                    BB_Polygon3D.VisibleFace();
                    MainCanvas.Children.Clear();
                    BB_Camera.Draw(MainCanvas);
                    mouseDown_Position = Mouse.GetPosition(MainCanvas);
            }

            if ((bool)i_make_point.IsChecked)
            {
                if (mouseDown)
                {
                    
                    Database.allPoints.Last().Ystatic += (mouse_GetPosition.Y - Mouse.GetPosition(MainCanvas).Y) / BB_Camera.Zoom;
                    BB_AuxiLine3D AuxiLine = new BB_AuxiLine3D(Database.allPoints.Last().toPoint3D(), BasicMethods.FindThePointOntheBasis(mouseDown_Position).toPoint3D());
                    AuxiLine.dashed = true;
                    BB_Camera.Draw(MainCanvas);
                    AuxiLine.Draw(MainCanvas, BB_Camera.Zoom, BB_Camera.x_offset, BB_Camera.y_offset);
                    double Ystatic = Math.Round(Database.allPoints.Last().Ystatic, 2);
                    Database.allPoints.Last().AddTextNearthePoint(MainCanvas, Ystatic.ToString());
                    mouse_GetPosition = Mouse.GetPosition(MainCanvas);

                }
                
            }
            if ((bool)i_make_line.IsChecked)
            {
                if (PolygonIndexes.Count != 0 && OnElipse == false)
                {
                    mouseDown_Position = Mouse.GetPosition(MainCanvas);
                    BB_AuxiLine3D AuxiLine = new BB_AuxiLine3D(Database.allPoints[PolygonIndexes.Last()].toPoint3D(), BasicMethods.FindThePointOntheBasis(mouseDown_Position).toPoint3D());
                    if (Phase2AuxiBool == true) MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                    Phase2AuxiBool = true;
                    AuxiLine.Draw(MainCanvas, BB_Camera.Zoom, BB_Camera.x_offset, BB_Camera.y_offset);
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            BB_Camera.x_offset = MainCanvas.ActualWidth / 2;
            BB_Camera.y_offset = MainCanvas.ActualHeight / 2 + MainCanvas.ActualHeight / 6;
            Viewport_Grid_Translate.Y = MainCanvas.ActualHeight / 6;
            BB_Camera.Draw(MainCanvas);


        }
       
        //PRISMS
        private void Cube_button_Click(object sender, RoutedEventArgs e)
        {
            BasicMethods.DeleteAllObjects();
            BB_Object3D.CreateRightPrism(2, 4, MainCanvas);
            //   BB_Line3D.AddLineFromStaticCoord(new BB_Point3D(-1, 0, -1), new BB_Point3D(2, 0, 2), MainCanvas,2.2);
            BB_Camera.Draw(MainCanvas);
        }

        double height;
        int nAngle;
        bool firstClick = true;

        private void RightPrism_Button_Click(object sender, RoutedEventArgs e)
        {
            if (firstClick)
            {
                BasicMethods.DeleteAllObjects();
                height = RP_Height_Slider.Value;
                nAngle = (int)RP_Verticies_Slider.Value;
                BB_Object3D.CreateRightPrism(height, nAngle, MainCanvas);
                RightPrismOptions.Visibility = Visibility.Visible;
                BB_Camera.Draw(MainCanvas);
                firstClick = false;
            }
            else
            {
                RightPrismOptions.Visibility = Visibility.Collapsed;
                firstClick = true;
            }
        }

        private void RP_Verticies_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!firstClick)
            {
                Slider slider = sender as Slider;
                nAngle = (int)slider.Value;
                BasicMethods.DeleteAllObjects();
                BB_Object3D.CreateRightPrism(height, nAngle, MainCanvas);
                BB_Camera.Draw(MainCanvas);
            }          
        }

        private void RP_Height_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!firstClick)
            {
                Slider slider = sender as Slider;
                height = slider.Value;
                BasicMethods.DeleteAllObjects();
                BB_Object3D.CreateRightPrism(height, nAngle, MainCanvas);
                BB_Camera.Draw(MainCanvas);
            }            
        }

        private void RightPyramid_Button_Click(object sender, RoutedEventArgs e)
        {
            if (firstClick)
            {
                BasicMethods.DeleteAllObjects();
                height = RPy_Height_Slider.Value;
                nAngle = (int)RPy_Verticies_Slider.Value;
                BB_Object3D.CreateRightPyramid(height, nAngle, MainCanvas);
                RightPyramidOptions.Visibility = Visibility.Visible;
                BB_Camera.Draw(MainCanvas);
                firstClick = false;
            }
            else
            {
                RightPyramidOptions.Visibility = Visibility.Collapsed;
                firstClick = true;
            }
            BB_Camera.Draw(MainCanvas);
        }

        private void RPy_Verticies_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!firstClick)
            {
                Slider slider = sender as Slider;
                nAngle = (int)slider.Value;
                BasicMethods.DeleteAllObjects();
                BB_Object3D.CreateRightPyramid(height, nAngle, MainCanvas);
                BB_Camera.Draw(MainCanvas);
            }
        }

        private void RPy_Height_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!firstClick)
            {
                Slider slider = sender as Slider;
                height = slider.Value;
                BasicMethods.DeleteAllObjects();
                BB_Object3D.CreateRightPyramid(height, nAngle, MainCanvas);
                BB_Camera.Draw(MainCanvas);
            }
        }

        private void MakePrism_Button_Click(object sender, RoutedEventArgs e)
        {
            if(MakePrism_Clicked == true)
            {
                MakePrism_Clicked = false;
                MakePrismActive.Visibility = Visibility.Collapsed;
                BasicMethods.DeleteAllObjects();
                BB_Camera.Draw(MainCanvas);
                HelpBox.Visibility = Visibility.Collapsed;
                i_hand.IsChecked = true;
            }
            else
            {
                MakePrismActive.Visibility = Visibility.Visible;
                BasicMethods.DeleteAllObjects();
                MakePrism_Clicked = true;
                Phase = 1;
                i_hand.IsChecked = false;
                Show_Message("Построяване на призма: Основа", "Постройте основата на призмата като отбелязвате точките и върху разграфената равнина. Това е първата стъпка от построяване на призма.");
                PolygonIndexes.Clear();
                Phase2AuxiBool = false;
            }          
        }

        //PYRAMIDS
        private void Tetrahedron_Button_Click(object sender, RoutedEventArgs e)
        {
            BasicMethods.DeleteAllObjects();
            BB_Object3D.CreateTetrahedron(MainCanvas);
            BB_Camera.Draw(MainCanvas);

        }

        private void MakePyramid_Button_Click(object sender, RoutedEventArgs e)
        {
            if(MakePyramid_Clicked == true)
            {
                MakePyramid_Clicked = false;
                MakePyramidActive.Visibility = Visibility.Collapsed;
                BasicMethods.DeleteAllObjects();
                BB_Camera.Draw(MainCanvas);
                HelpBox.Visibility = Visibility.Collapsed;
                i_hand.IsChecked = true;
            }
            else
            {
                MakePyramidActive.Visibility = Visibility.Visible;
                BasicMethods.DeleteAllObjects();
                MakePyramid_Clicked = true;
                Phase = 1;
                i_hand.IsChecked = false;
                Show_Message("Построяване на пирамида: Основа", "Постройте основата на пирамидата като отбелязвате точките и върху разграфената равнина. Това е първата стъпка от построяване на пирамида.");
                PolygonIndexes.Clear();
                Phase2AuxiBool = false;
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            i_hand.IsChecked = true;

            Database.DrawInitialObjects(MainCanvas);

            BB_Camera.MassRotation(-15, 60, 0);
            rotateX.Angle += -15;
            rotateY.Axis = new Vector3D(0, Math.Cos((rotateX.Angle) / 180 * Math.PI), Math.Sin((rotateX.Angle) / 180 * Math.PI));
            rotateY.Angle += 60;
            BB_Camera.Draw(MainCanvas);


        }
        private void v_system_Click(object sender, RoutedEventArgs e)
        {
            if (Database.CoordSystem.Visible == true)
                Database.CoordSystem.Visible = false;
            else Database.CoordSystem.Visible = true;
            BB_Camera.Draw(MainCanvas);
        }

        private void v_grid_Click(object sender, RoutedEventArgs e)
        {
            if (Viewport_Grid.Visibility == Visibility.Visible)
            {
                Viewport_Grid.Visibility = Visibility.Collapsed;
            }
            else
            {
                Viewport_Grid.Visibility = Visibility.Visible;
            }
        }

        private void v_background_Click(object sender, RoutedEventArgs e)
        {
            v_background.Visibility = Visibility.Collapsed;
            Backgrounds.Visibility = Visibility.Visible;
            BasicMethods.DeleteAllObjects();
            BB_Camera.Draw(MainCanvas);
        }

        private void Instruments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
          if(additionalOptions.Visibility == Visibility.Visible)
            {
                additionalOptions.Visibility = Visibility.Collapsed;
            }
          else
            {
                additionalOptions.Visibility = Visibility.Visible;
            }
        }

        private void HelpBox_Close_Button_Click(object sender, RoutedEventArgs e)
        {
            HelpBox.Visibility = Visibility.Collapsed;
        }

        private void Black_Background_Click(object sender, RoutedEventArgs e)
        {
            v_background.Visibility = Visibility.Visible;
            Backgrounds.Visibility = Visibility.Collapsed;
        }

        private void Grey_Background_Click(object sender, RoutedEventArgs e)
        {
            v_background.Visibility = Visibility.Visible;
            Backgrounds.Visibility = Visibility.Collapsed;
        }

        private void White_Background_Click(object sender, RoutedEventArgs e)
        {
            v_background.Visibility = Visibility.Visible;
            Backgrounds.Visibility = Visibility.Collapsed;
        }
        private void i_make_point_Checked(object sender, RoutedEventArgs e)
        {
            mouseDown = false;
        }
        private void i_make_line_Checked(object sender, RoutedEventArgs e)
        {
            PolygonIndexes.Clear();
            Phase = 1;
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void MainCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PolygonIndexes.Count > 0 && Phase == 1)
            {
                Database.allPoints.RemoveAt(Database.allPoints.Count - 1);
                for (int i = 0; i < PolygonIndexes.Count - 1; i++)
        {
                    Database.allPoints.RemoveAt(Database.allPoints.Count - 1);
                    Database.allLines.RemoveAt(Database.allLines.Count - 1);
                }
                PolygonIndexes.Clear();
                BB_Camera.Draw(MainCanvas);
            }


        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape &&( MakePyramid_Clicked || MakePrism_Clicked))
            {
                MakePrism_Clicked = false;
                MakePrismActive.Visibility = Visibility.Collapsed;
                MakePyramid_Clicked = false;
                MakePyramidActive.Visibility = Visibility.Collapsed;
                BasicMethods.DeleteAllObjects();
                HelpBox.Visibility = Visibility.Collapsed;
                i_hand.IsChecked = true;
                BB_Camera.Draw(MainCanvas);
            }
            }

        private void i_make_plane_Checked(object sender, RoutedEventArgs e)
        {
            Plane.MakePlaneSelected(MainCanvas, i_make_plane);
        }

        private void i_make_plane_Unchecked(object sender, RoutedEventArgs e)
        {
            Plane.MakePlaneDeselected(i_hand);
        }
    }
}
