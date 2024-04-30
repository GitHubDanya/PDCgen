using PDCgen;
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
using System.Threading;
using System.ComponentModel;

namespace PDCgen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int MANDATORY_VALUES = 6;

        public MainWindow()
        {
            InitializeComponent();
            data = new BindingData();
            DataContext = data;
            mainWindow = (MainWindow)Application.Current.MainWindow;
            reader = new FlightplanReader();
            compiler = new PDCcompiler();
            PDCcompiler.flightplanReader = reader;
            PDCcompiler.mainWindow = mainWindow;
            FlightplanReader.mainWindow = mainWindow;
        }

        public MainWindow mainWindow;
        public FlightplanReader reader;
        public PDCcompiler compiler;
        BindingData data;

        public bool StartupInPDC;

        public class BindingData : INotifyPropertyChanged
        {
            private string _rwyRow;
            public string rwyRow { get { return _rwyRow; } set
                {
                    if (_rwyRow != value)
                    {
                        _rwyRow = value;
                        OnPropertyChanged(nameof(rwyRow));
                    }
                }
            }

            private string _freqRow;
            public string freqRow
            {
                get { return _freqRow; }
                set
                {
                    if (_freqRow != value)
                    {
                        _freqRow = value;
                        OnPropertyChanged(nameof(freqRow));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

            private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && Keyboard.IsKeyDown(Key.V))
            {
                string flightplan = mainWindow.mainTextBox.Text.ToString();
                
                if (reader.tryParse(flightplan) == true)
                {
                   compiler.compilePDC();
                }
            }
        }
        public int getPositionOfOptionalValue(string designator)
        {
            if (designator == "designatorRWY")
            {
                return MANDATORY_VALUES + 1;
            }
            if (designator == "designatorFRQ")
            {
                if (mainWindow.designatorRWY.Visibility == Visibility.Visible)
                {
                    return MANDATORY_VALUES + 2;
                }
                else
                {
                    return MANDATORY_VALUES + 1;
                }
            }
            return mainWindow.designatorGrid.RowDefinitions.Count - MANDATORY_VALUES;
        }

        public void rwyInPDC_Checked(object sender, RoutedEventArgs e)
        {
            mainWindow.designatorGrid.RowDefinitions.Add(new RowDefinition());
            data.rwyRow = getPositionOfOptionalValue("designatorRWY").ToString();
            mainWindow.designatorRWY.Visibility = Visibility.Visible;
            mainWindow.RWYlbl.Visibility = Visibility.Visible;
            data.freqRow = getPositionOfOptionalValue("designatorFRQ").ToString();
            mainWindow.UpdateLayout();
            compiler.compilePDC();
        }

        public void rwyInPDC_Unchecked(object sender, RoutedEventArgs e)
        {
            mainWindow.designatorGrid.RowDefinitions.RemoveAt(1);
            mainWindow.designatorRWY.Visibility = Visibility.Collapsed;
            mainWindow.RWYlbl.Visibility = Visibility.Collapsed;
            data.freqRow = getPositionOfOptionalValue("designatorFRQ").ToString();
            mainWindow.designatorRWY.Text = "";
            mainWindow.UpdateLayout();
            compiler.compilePDC();
        }

        public void freqInPDC_Checked(object sender, RoutedEventArgs e)
        {
            mainWindow.designatorGrid.RowDefinitions.Add(new RowDefinition());
            data.freqRow = getPositionOfOptionalValue("designatorFRQ").ToString();
            mainWindow.designatorFRQ.Visibility = Visibility.Visible;
            mainWindow.FREQlbl.Visibility = Visibility.Visible;
            mainWindow.UpdateLayout();
            compiler.compilePDC();
        }

        public void freqInPDC_Unchecked(object sender, RoutedEventArgs e)
        {
            mainWindow.designatorGrid.RowDefinitions.RemoveAt(1);
            mainWindow.designatorFRQ.Visibility = Visibility.Collapsed;
            mainWindow.FREQlbl.Visibility = Visibility.Collapsed;
            mainWindow.designatorFRQ.Text = "";
            mainWindow.UpdateLayout();
            compiler.compilePDC();
        }
        public void startupInPDC_Checked(object sender, RoutedEventArgs e)
        {
            StartupInPDC = true;
            compiler.compilePDC();
        }

        public void startupInPDC_Unchecked(object sender, RoutedEventArgs e)
        {
            StartupInPDC = false;
            compiler.compilePDC();
        }

        public void copyBtn_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(outputTextBox.Text);
        }

        public void generateSqwk_Click(object sender, RoutedEventArgs e)
        {
            string sqk = compiler.randomizeSqwk();
            designatorSQK.Text = sqk;
            mainWindow.UpdateLayout();
            compiler.compilePDC();
        }

        public void recompile(object sender, RoutedEventArgs e)
        {
            reader.parseToParsedData();
            compiler.compilePDC();
        }
    }
}
