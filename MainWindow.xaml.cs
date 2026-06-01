using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WheelPicker
{
    public partial class MainWindow : Window
    {
        private readonly SoundPlayer _spinSound = new(@"Assets\spin.wav");

        private readonly Random _random = new();

        private string[] _items = [];

        private double _currentRotation = 0;

        private void LoadLibrary(string path)
        {
            try
            {

                ItemLibrary lib = LibraryLoader.Load(path);

                _items = lib.Items.ToArray();

                WheelDrawer.DrawWheel(
                    WheelCanvas,
                    _items,
                    250);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadLibraries()
        {
            LibraryComboBox.Items.Clear();

            string folder = "Libraries";

            if (!Directory.Exists(folder))
                return;

            foreach (string file in Directory.GetFiles(folder, "*.json"))
            {
                var library =
                    LibraryLoader.Load(file);

                LibraryComboBox.Items.Add(
                    new LibraryInfo
                    {
                        Name = library.Name,
                        Path = file
                    });
            }

            if (LibraryComboBox.Items.Count > 0)
                LibraryComboBox.SelectedIndex = 0;
        }

        private void LibraryComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LibraryComboBox.SelectedItem
                is not LibraryInfo selected)
                return;

            LoadLibrary(selected.Path);
        }

        private void LoadMusic_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoadLibraries();
        }

        public MainWindow()
        {
            InitializeComponent();
            
            LoadLibraries();
        }
        private string GetWinnerFromAngle(
            double wheelRotation)
        {
            double sliceAngle =
                360.0 / _items.Length;

            /*
            * Pointer is fixed at top.
            * Top = 270° in our wheel coordinate system.
            */

            double pointerAngle =
                (270 - wheelRotation + 360) % 360;

            int index =
                (int)(pointerAngle / sliceAngle);

            if (index < 0)
                index = 0;

            if (index >= _items.Length)
                index = _items.Length - 1;

            return _items[index];
        }

        private bool _isSpinning;

        private void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            if (_items.Length == 0)
            {
                ResultText.Text = "No items loaded";
                return;
            }

            if (_isSpinning)
                return;

            _isSpinning = true;

            _spinSound.Play();

            double extraRotation =
                _random.NextDouble() * 360.0;

            double targetAngle =
                _currentRotation +
                3600 +
                extraRotation;

            var animation = new DoubleAnimation
            {
                From = _currentRotation,
                To = targetAngle,
                Duration = TimeSpan.FromSeconds(6),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            animation.Completed += (_, _) =>
            {
                double finalAngle = targetAngle % 360;

                if (finalAngle < 0)
                    finalAngle += 360;

                string winner =
                    GetWinnerFromAngle(finalAngle);

                ResultText.Text = winner;

                _currentRotation = finalAngle;

                _isSpinning = false;
            };

            WheelRotation.BeginAnimation(
                RotateTransform.AngleProperty,
                animation);
        }
    }
}