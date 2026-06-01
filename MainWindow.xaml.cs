using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace WheelPicker
{
    public partial class MainWindow : Window
    {
        private readonly Random _random = new();

        private string[] _items = [];

        private double _currentRotation = 0;

        private void LoadLibrary(string path)
        {
            Library lib = LibraryLoader.Load(path);

            _items = lib.Items.ToArray();

            WheelDrawer.DrawWheel(
                WheelCanvas,
                _items,
                250);
        }

        private void LoadMusic_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadLibrary("Libraries/music.json");
        }

        public MainWindow()
        {
            InitializeComponent();
            
            LoadLibrary("Libraries/music.json");
        }

        private void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = _random.Next(_items.Length);

            double sliceAngle = 360.0 / _items.Length;

            double targetAngle =
                (_currentRotation + 3600) -
                (selectedIndex * sliceAngle);

            var animation = new DoubleAnimation
            {
                From = _currentRotation,
                To = targetAngle,
                Duration = TimeSpan.FromSeconds(5),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            animation.Completed += (_, _) =>
            {
                ResultText.Text = _items[selectedIndex];
                _currentRotation = targetAngle % 360;
            };

            WheelRotation.BeginAnimation(
                System.Windows.Media.RotateTransform.AngleProperty,
                animation);
        }
    }
}