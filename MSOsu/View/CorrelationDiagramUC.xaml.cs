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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSOsu.View
{
    /// <summary>
    /// Логика взаимодействия для CorrelativePleiad.xaml
    /// </summary>
    public partial class CorrelationDiagramUC : UserControl
    {
        private double[][] correlationsMatrix;
        public CorrelationDiagramUC(double[][] correlationsMatrix)
        {
            InitializeComponent();
            this.correlationsMatrix = correlationsMatrix;
            cnDiag.SizeChanged += (sender1, e1) => CanvasRedraw();
        } 

        private void CanvasRedraw()
        {
            cnDiag.Children.Clear();
            if (correlationsMatrix != null)
                FillCanvas(correlationsMatrix);
        }

        public void FillCanvas(double[][] correlationsMatrix)
        {
            double extraSpace = cnDiag.ActualWidth - cnDiag.ActualHeight;
            double size = Math.Min(cnDiag.ActualWidth, cnDiag.ActualHeight);
            double radius = size / 2; //радиус для рисования
            double centerX = cnDiag.ActualWidth / 2;
            double centerY = cnDiag.ActualHeight / 2;
            Point centerPoint = new Point(centerX, centerY); //точка по середине
            EllipseGeometry circleGeometry = new EllipseGeometry(centerPoint, radius, radius); //создает окружность данного радиуса

            GeometryGroup ggCircle = new GeometryGroup(); //будет содержать окружность
            ggCircle.Children.Add(circleGeometry);
            // добавление точек (вершин)
            int numOfPoints = correlationsMatrix.Length; //количество точек (выборок)
            double intervalLen = 2 * Math.PI / numOfPoints; //расстояние между точками
            double[] placeOnLine = new double[numOfPoints]; //значение угла в точках
            List<(double, double)> pointsCords = new List<(double,double)>(placeOnLine.Length); //координаты точек
            GeometryGroup ggPoints = new GeometryGroup(); //будет содержать точки

            for (int i = 0; i < numOfPoints; i++)
            {
                placeOnLine[i] = Math.PI / 2 - i * intervalLen; //вращение по часовой, с точки pi/2
                double x = centerX + radius * Math.Cos(placeOnLine[i]);
                double y = centerY + radius * Math.Sin(placeOnLine[i]);
                pointsCords.Add((x, y));
                ggPoints.Children.Add(new EllipseGeometry(new Point(pointsCords[i].Item1, pointsCords[i].Item2), 7, 7));

                Label lbl = new Label();
                lbl.Content = i == 0 ? "Y" : "X" + i;
                lbl.FontSize = 20;
                lbl.Foreground = Brushes.Black;
                cnDiag.Children.Add(lbl);
                double lblX = pointsCords[i].Item1;
                double lblY = pointsCords[i].Item2;
                lblX -= lbl.FontSize * 0.85;
                lblY -= lbl.FontSize;
                lblX += lbl.FontSize * 1.2  * Math.Cos(placeOnLine[i]);
                lblY += lbl.FontSize * 1.2 * Math.Sin(placeOnLine[i]);
                Canvas.SetZIndex(lbl, 100);
                Canvas.SetLeft(lbl, lblX);
                Canvas.SetTop(lbl, lblY);
            }
            Path pathPoints = new Path
            {
                Fill = Brushes.Black,
                Data = ggPoints
            };
            Path pathCircle = new Path
            {
                Stroke = Brushes.Black,
                Data = ggCircle
            };
            GeometryGroup ggLinesNotPos = new GeometryGroup();
            GeometryGroup ggLinesWeakPos = new GeometryGroup();
            GeometryGroup ggLinesMediumPos = new GeometryGroup();
            GeometryGroup ggLinesStrongPos = new GeometryGroup();
            GeometryGroup ggLinesVeryStrongPos = new GeometryGroup();
            GeometryGroup ggLinesNotNeg = new GeometryGroup();
            GeometryGroup ggLinesWeakNeg = new GeometryGroup();
            GeometryGroup ggLinesMediumNeg = new GeometryGroup();
            GeometryGroup ggLinesStrongNeg = new GeometryGroup();
            GeometryGroup ggLinesVeryStrongNeg = new GeometryGroup();
            for (int i = 0; i < numOfPoints - 1; i++)
            {
                for (int j = i + 1; j < numOfPoints; j++)
                {
                    Point start = new Point(pointsCords[i].Item1, pointsCords[i].Item2);
                    Point end = new Point(pointsCords[j].Item1, pointsCords[j].Item2);
                    double val = correlationsMatrix[i][j];
                    if (val < 0)
                    {
                        //if (val > -0.3)
                        //    ggLinesNotNeg.Children.Add(new LineGeometry(start, end));
                        if (val > -0.5 && val <= -0.3)
                            ggLinesWeakNeg.Children.Add(new LineGeometry(start, end));
                        if (val > -0.7 && val <= -0.5)
                            ggLinesMediumNeg.Children.Add(new LineGeometry(start, end));
                        if (val > -0.9 && val <= -0.7)
                            ggLinesStrongNeg.Children.Add(new LineGeometry(start, end));
                        if (val >= -1 && val <= -0.9)
                            ggLinesVeryStrongNeg.Children.Add(new LineGeometry(start, end));
                    }
                    else
                    {
                        //if (val < 0.3)
                        //    ggLinesNotPos.Children.Add(new LineGeometry(start, end));
                        if (val < 0.5 && val >= 0.3)
                            ggLinesWeakPos.Children.Add(new LineGeometry(start, end));
                        if (val < 0.7 && val >= 0.5)
                            ggLinesMediumPos.Children.Add(new LineGeometry(start, end));
                        if (val < 0.9 && val >= 0.7)
                            ggLinesStrongPos.Children.Add(new LineGeometry(start, end));
                        if (val <= 1 && val >= 0.9)
                            ggLinesVeryStrongPos.Children.Add(new LineGeometry(start, end));
                    }
                }
            }
            //Path pathLinesNP = new Path()
            //{
            //    Stroke = Brushes.White,
            //    Data = ggLinesNotPos,
            //    StrokeThickness = 2
            //};
            Path pathLinesWP = new Path()
            {
                Stroke = Brushes.GreenYellow,
                Data = ggLinesWeakPos,
                StrokeThickness = 2
            };
            Path pathLinesMP = new Path()
            {
                Stroke = Brushes.Yellow, 
                Data = ggLinesMediumPos,
                StrokeThickness = 2
            };
            Path pathLinesSP = new Path()
            {
                //Stroke = new SolidColorBrush(Color.FromArgb(0xFF,0xD5,0xF2,0xFF)),
                Stroke = Brushes.Orange,
                Data = ggLinesStrongPos,
                StrokeThickness = 2
            };
            Path pathLinesVSP = new Path()
            {
                Stroke = Brushes.Red,
                Data = ggLinesVeryStrongPos,
                StrokeThickness = 2
            };

            //Path pathLinesNN = new Path()
            //{
            //    Stroke = Brushes.White,
            //    Data = ggLinesNotNeg,
            //    StrokeThickness = 2
            //};
            Path pathLinesWN = new Path()
            {
                Stroke = Brushes.GreenYellow,
                Data = ggLinesWeakNeg,
                StrokeThickness = 2
            };
            Path pathLinesMN = new Path()
            {
                Stroke = Brushes.Yellow,
                Data = ggLinesMediumNeg,
                StrokeThickness = 2
            };
            Path pathLinesSN = new Path()
            {
                Stroke = Brushes.Orange,
                Data = ggLinesStrongNeg,
                StrokeThickness = 2
            };
            Path pathLinesVSN = new Path()
            {
                Stroke = Brushes.Red,
                Data = ggLinesVeryStrongNeg,
                StrokeThickness = 2
            };

            //pathLinesNN.StrokeDashArray = new DoubleCollection(new double[] { 4, 4 });
            pathLinesWN.StrokeDashArray = new DoubleCollection(new double[] { 4, 4 });
            pathLinesMN.StrokeDashArray = new DoubleCollection(new double[] { 4, 4 });
            pathLinesSN.StrokeDashArray = new DoubleCollection(new double[] { 4, 4 });
            pathLinesVSN.StrokeDashArray = new DoubleCollection(new double[] { 4, 4 });
            cnDiag.Children.Add(pathCircle);

            //cnDiag.Children.Add(pathLinesNP);
            cnDiag.Children.Add(pathLinesWP);
            cnDiag.Children.Add(pathLinesMP);
            cnDiag.Children.Add(pathLinesSP);
            cnDiag.Children.Add(pathLinesVSP);

            //cnDiag.Children.Add(pathLinesNN);
            cnDiag.Children.Add(pathLinesWN);
            cnDiag.Children.Add(pathLinesMN);
            cnDiag.Children.Add(pathLinesSN);
            cnDiag.Children.Add(pathLinesVSN);
            cnDiag.Children.Add(pathPoints);

        }
    }
}
