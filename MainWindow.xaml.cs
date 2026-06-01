using System;
using System.Linq;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WheelPicker
{
    public partial class MainWindow : Window
    {
        
        private MusicLibrary? _musicLibrary;

        private MusicGenre? _currentGenre;

        private bool _isSubgenreMode = false;

        private void LoadMusicHierarchy(string path)
        {
            _musicLibrary =
                MusicLibraryLoader.Load(path);

            _isSubgenreMode = false;

            _currentGenre = null;

            _items =
                _musicLibrary.Genres
                    .Select(g => g.Name)
                    .ToArray();

            WheelDrawer.DrawWheel(
                WheelCanvas,
                _items,
                250);
        }
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
                string name =
                    Path.GetFileNameWithoutExtension(file);

                LibraryComboBox.Items.Add(
                    new LibraryInfo
                    {
                        Name = name.Equals("music",
                            StringComparison.OrdinalIgnoreCase)
                            ? "Music"
                            : name,
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

                if (selected.Name == "Music")
                {
                    LoadMusicHierarchy(selected.Path);
                }
                else
                {
                    LoadLibrary(selected.Path);
                }
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

                if (_musicLibrary != null &&
                    !_isSubgenreMode)
                {
                    _currentGenre =
                        _musicLibrary.Genres
                            .First(g => g.Name == winner);

                    _items =
                        _currentGenre.Subgenres.ToArray();

                    WheelDrawer.DrawWheel(
                        WheelCanvas,
                        _items,
                        250);

                    ResultText.Text =
                        $"Selected Genre: {winner}\nSpin again for subgenre";

                    _isSubgenreMode = true;
                }
                else if (_musicLibrary != null &&
                        _isSubgenreMode)
                {
                    ResultText.Text =
                        $"{_currentGenre?.Name} > {winner}";

                    _items =
                        _musicLibrary.Genres
                            .Select(g => g.Name)
                            .ToArray();

                    WheelDrawer.DrawWheel(
                        WheelCanvas,
                        _items,
                        250);

                    _currentGenre = null;

                    _isSubgenreMode = false;
                }
                else
                {
                    ResultText.Text = winner;
                }

                _currentRotation = finalAngle;

                _isSpinning = false;
            };

            WheelRotation.BeginAnimation(
                RotateTransform.AngleProperty,
                animation);
                
        }

        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_musicLibrary == null)
                return;

            _items =
                _musicLibrary.Genres
                    .Select(g => g.Name)
                    .ToArray();

            WheelDrawer.DrawWheel(
                WheelCanvas,
                _items,
                250);

            _currentGenre = null;
            _isSubgenreMode = false;

            ResultText.Text = "";
        }
    }
}