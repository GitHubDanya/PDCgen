using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows;
using System.Text.RegularExpressions;
using PDCgen;

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
            string compiledPDC = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Append(flightplanReader.ParsedData[0]+" CLR TO " + flightplanReader.ParsedData[3]);
            if (mainWindow.designatorRWY.Text != "")
            {
                sb.Append(" RWY " + mainWindow.designatorRWY.Text);
            }
            if (flightplanReader.ParsedData[5] != "nil")
            {
                sb.Append(" DEP " + flightplanReader.ParsedData[5]+" ");
            } else
            {
                sb.Append(" FLY RWY HDG EXP VECTORS CLB ");
                int altitude;

                if (int.TryParse(flightplanReader.ParsedData[6], out altitude))
                {
                    if (altitude <= 30)
                    {
                        sb.Append(altitude + "00FT ");
                    }
                    else if (altitude >= 1000)
                    {
                        sb.Append(altitude + "FT ");
                    }
                    else
                    {
                        sb.Append("FL" + altitude + " ");
                    }
                }
            }

            if (mainWindow.designatorFRQ.Text != "")
            {
                sb.Append("DEP ON "+mainWindow.designatorFRQ.Text+" ");
            }

            sb.Append("SQUAWK "+mainWindow.designatorSQK.Text+" ATIS "+mainWindow.ATISinfo.Text);
            
            if (mainWindow.StartupInPDC)
            {
                sb.Append(" FOR PUSH CALL FREQ ");
            } else
            {
                sb.Append(" WHEN RDY CALL FREQ ");
            }

            sb.Append(mainWindow.designatorIFRQ.Text + " IF UNABLE REVERT TO VOICE");

            compiledPDC = sb.ToString();
            string newCompiledPDC = compiledPDC.Replace("\r", "");
            compiledPDC = newCompiledPDC.ToUpper();
            mainWindow.outputTextBox.Text = compiledPDC;
            mainWindow.UpdateLayout();

            return compiledPDC;
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

//sb.Append(flightplanReader.ParsedData[0] + " ");

//if (mainWindow.StartupInPDC)
//{
//    sb.Append("STARTUP APPROVED ");
//}

//sb.Append("CLEARED " + flightplanReader.ParsedData[3] + " ");

//if (flightplanReader.ParsedData[5] != "nil")
//{
//    sb.Append(flightplanReader.ParsedData[5] + " DEPARTURE "); //FPL RTE CLB VIA SID "
//    if (mainWindow.designatorRWY.Text != "")
//    {
//        sb.Append("RWY " + mainWindow.designatorRWY.Text + " FPL RTE CLB VIA SID ");
//    }
//    else
//    {
//        sb.Append("FPL RTE CLB VIA SID ");
//    }
//}
//else if (mainWindow.designatorRWY.Text != "")
//{
//    sb.Append("RWY " + mainWindow.designatorRWY.Text + " ");
//}
//else
//{
//    sb.Append("FLY RWY HDG EXP VECTORS THEN FPL RTE CLIMB ");
//}


//int altitude;

//if (int.TryParse(flightplanReader.ParsedData[6], out altitude))
//{
//    if (altitude <= 30)
//    {
//        sb.Append(altitude + "00FT ");
//    }
//    else if (altitude >= 1000)
//    {
//        sb.Append(altitude + "FT ");
//    }
//    else
//    {
//        sb.Append("FL" + altitude + " ");
//    }
//}

//if (mainWindow.designatorFRQ.Visibility == Visibility.Visible)
//{
//    sb.Append("DEPARTURE ON " + mainWindow.designatorFRQ.Text + " ");
//}
//sb.Append("SQK " + mainWindow.designatorSQK.Text + " ATIS " + mainWindow.ATISinfo.Text + " WHEN READY CALL " + mainWindow.designatorIFRQ.Text + " . . . RMK DO NOT READBACK");
//compiledPDC = sb.ToString();
//string newCompiledPDC = compiledPDC.Replace("\r", "");
//compiledPDC = newCompiledPDC.ToUpper();
//mainWindow.outputTextBox.Text = compiledPDC;
//mainWindow.UpdateLayout();

//return compiledPDC;