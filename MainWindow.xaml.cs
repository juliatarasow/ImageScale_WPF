using Microsoft.Win32;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageScale_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<ComboBoxItemData> items = new List<ComboBoxItemData>
            {
                new ComboBoxItemData { Text = "Keine Skalierung", Bild = "/icons/icon-none.png" },
                new ComboBoxItemData { Text = "Ausfüllen", Bild = "/icons/icon-fill.png" },
                new ComboBoxItemData { Text = "Einheitlich skalieren", Bild = "/icons/ico-uniform.png" },
                new ComboBoxItemData { Text = "Einheitlich ausfüllen", Bild = "/icons/ico-utfill.png" }
            };

            fillComboBox.ItemsSource = items;
        }

        public class ComboBoxItemData
        {
            public string? Text { get; set; }
            public string? Bild { get; set; }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            // Überprüfen, ob Daten gedroppt wurden
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    // Dateien extrahieren
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (files.Length > 0)
                    {
                        string filePath = files[0];

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            Uri path = new Uri(filePath);
                            BitmapImage bitmap = new BitmapImage();

                            // Bild initialisieren
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad; 
                            bitmap.UriSource = path;
                            bitmap.EndInit();

                            image.Source = bitmap;
                        }
                        else
                        {
                            MessageBox.Show("Ungültiger Dateipfad.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                catch (UriFormatException ex)
                {
                    MessageBox.Show($"Fehler beim Verarbeiten des Dateipfads: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ein Fehler ist aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Datei öffnen",
                Filter = "Bilddateien (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|Alle Dateien (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    if (filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                        filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        filePath.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(filePath);
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        image.Source = bitmapImage;
                    }
                    else
                    {
                        MessageBox.Show("Die Datei ist kein unterstütztes Bild.", "Ungültige Datei", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Fehler beim Öffnen der Datei: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnFillModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fillComboBox.SelectedItem is ComboBoxItemData selectedMode)
            {
                string selectedText = selectedMode.Text;

                switch (selectedText)
                {
                    case "Keine Skalierung":
                        image.Stretch = Stretch.None;
                        break;
                    case "Ausfüllen":
                        image.Stretch = Stretch.Fill;
                        break;
                    case "Einheitlich skalieren":
                        image.Stretch = Stretch.Uniform;
                        break;
                    case "Einheitlich ausfüllen":
                        image.Stretch = Stretch.UniformToFill;
                        break;
                }

                // Aktualisiere das Menü-Item für den aktuellen Anzeigemodus
                menuItemShowMode.Header = $"{selectedText}";
            }
        }
    }
}