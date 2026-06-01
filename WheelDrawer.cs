using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WheelPicker
{
    public static class WheelDrawer
    {
        private static readonly Brush[] SliceColors =
        {
            Brushes.CornflowerBlue,
            Brushes.LightCoral,
            Brushes.MediumSeaGreen,
            Brushes.Gold,
            Brushes.Plum,
            Brushes.Orange,
            Brushes.Turquoise,
            Brushes.Salmon
        };

        public static void DrawWheel(
            Canvas canvas,
            string[] items,
            double radius)
        {
            canvas.Children.Clear();

            double centerX = radius;
            double centerY = radius;

            double sliceAngle = 360.0 / items.Length;

            for (int i = 0; i < items.Length; i++)
            {
                double startAngle = i * sliceAngle;
                double endAngle = startAngle + sliceAngle;

                Path slice = CreateSlice(
                    centerX,
                    centerY,
                    radius,
                    startAngle,
                    endAngle,
                    SliceColors[i % SliceColors.Length]);

                canvas.Children.Add(slice);

                double labelAngle =
                    (startAngle + endAngle) / 2;

                double radians =
                    labelAngle * Math.PI / 180;

                double textRadius =
                    radius * 0.65;

                TextBlock label = new TextBlock
                {
                    Text = items[i],
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                };

                Canvas.SetLeft(
                    label,
                    centerX +
                    textRadius * Math.Cos(radians) - 30);

                Canvas.SetTop(
                    label,
                    centerY +
                    textRadius * Math.Sin(radians) - 10);

                canvas.Children.Add(label);
            }
        }

        private static Path CreateSlice(
            double centerX,
            double centerY,
            double radius,
            double startAngle,
            double endAngle,
            Brush fill)
        {
            Point center = new(centerX, centerY);

            double startRadians =
                startAngle * Math.PI / 180;

            double endRadians =
                endAngle * Math.PI / 180;

            Point startPoint = new(
                centerX + radius * Math.Cos(startRadians),
                centerY + radius * Math.Sin(startRadians));

            Point endPoint = new(
                centerX + radius * Math.Cos(endRadians),
                centerY + radius * Math.Sin(endRadians));

            PathFigure figure = new()
            {
                StartPoint = center
            };

            figure.Segments.Add(
                new LineSegment(startPoint, true));

            figure.Segments.Add(
                new ArcSegment(
                    endPoint,
                    new Size(radius, radius),
                    0,
                    endAngle - startAngle > 180,
                    SweepDirection.Clockwise,
                    true));

            figure.Segments.Add(
                new LineSegment(center, true));

            PathGeometry geometry = new();
            geometry.Figures.Add(figure);

            return new Path
            {
                Data = geometry,
                Fill = fill,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
        }
    }
}