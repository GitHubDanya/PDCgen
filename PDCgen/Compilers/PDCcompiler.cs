using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows;
using System.Text.RegularExpressions;

namespace PDCgen
{
    public class PDCcompiler
    {
        //FlightplanReader flightplanReader = new FlightplanReader();
        //MainWindow mainWindow = new MainWindow();
        public static FlightplanReader flightplanReader { get; set; }
        public static MainWindow mainWindow { get; set; }
        public PDCcompiler()
        {
            
        }
        public string compilePDC()
        {
            string compiledPDC;
            StringBuilder sb = new StringBuilder();

            sb.Append(flightplanReader.RouteData[0]+" ");

            if (mainWindow.StartupInPDC)
            {
                sb.Append(" STARTUP APPROVED ");
            }

            sb.Append("CLEARED "+flightplanReader.RouteData[3]+" ");

            if (flightplanReader.RouteData[5] != "nil")
            {
                sb.Append(flightplanReader.RouteData[5]+" DEPARTURE FPL RTE CLB VIA SID ");
            }
            else { sb.Append("FLY RWY HDG EXP VECTORS THEN FPL RTE CLIMB "); }

            int altitude;

            if (int.TryParse(flightplanReader.RouteData[6], out altitude))
            {
                if (altitude <= 30)
                {
                    sb.Append(altitude+"00FT ");
                }
                else if (altitude >= 1000)
                {
                    sb.Append(altitude+"FT ");
                }
                else
                {
                    sb.Append("FL"+altitude+" ");
                }
            }

            if (mainWindow.designatorFRQ.Visibility == Visibility.Visible)
            {
                sb.Append("DEPARTURE ON "+mainWindow.designatorFRQ.Text+" ");
            }

            sb.Append("SQK "+mainWindow.designatorSQK.Text+" ATIS "+mainWindow.ATISinfo.Text+" WHEN READY CALL "+mainWindow.designatorIFRQ.Text+" | DO NOT READBACK");
            compiledPDC = sb.ToString();
            string newCompiledPDC = compiledPDC.Replace("\r", "");

            mainWindow.outputTextBox.Text = newCompiledPDC;
            mainWindow.UpdateLayout();

            return newCompiledPDC.ToUpper();
        }

        public string randomizeSqwk()
        {
            string result;
            Random random = new Random();
            int min = 0;
            int max = 7;
            result = $"{random.Next(min, max)}{random.Next(min, max)}{random.Next(min, max)}{random.Next(min, max)}";
            return result;
        }
    }
}
