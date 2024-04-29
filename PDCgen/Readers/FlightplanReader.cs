using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace PDCgen
{
    public class FlightplanReader
    {
        public static MainWindow mainWindow { get; set; }
        public string[] RouteData = new string[7];

        public bool tryParse(string flightplan)
        {
            try
            {
                string[] parsedData = new string[7];
                string[] flightplanData = flightplan.Split('\n');
                for (int i = 0; i < parsedData.Length; i++)
                {
                    if (flightplanData[i].IndexOf("Callsign:") != -1)
                    {
                        parsedData[0] = flightplanData[i].Substring(10);
                        RouteData[0] = parsedData[0];
                    } else if (flightplanData[i].IndexOf("Flight Rules:") != -1)
                    {
                        parsedData[1] = flightplanData[i].Substring(14);
                        RouteData[1] = parsedData[1];
                    } else if (flightplanData[i].IndexOf("Departing:") != -1)
                    {
                        parsedData[2] = flightplanData[i].Substring(11);
                        RouteData[2] = parsedData[2];
                    } else if (flightplanData[i].IndexOf("Arriving:") != -1)
                    {
                        parsedData[3] = flightplanData[i].Substring(10);
                        RouteData[3] = parsedData[3];
                    } else if (flightplanData[i].IndexOf("Route:") != -1)
                    {
                        parsedData[4] = flightplanData[i].Substring(7);
                        RouteData[4] = parsedData[4];
                        parsedData[5] = getSID(parsedData[4]);
                        RouteData[5] = parsedData[5];
                    } else if (flightplanData[i].IndexOf("Flight Level:") != -1)
                    {
                        parsedData[6] = flightplanData[i].Substring(14);
                        RouteData[6] = parsedData[6];
                    }

                }

                parseToTextboxes(parsedData);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} {ex.StackTrace}", $"ERROR in {ex.Source}");
                return false;
            }
        }

        public void parseToTextboxes(string[] dataToParse)
        {
            mainWindow.designatorCLL.Text = dataToParse[0];
            mainWindow.designatorDEP.Text = dataToParse[2];
            mainWindow.designatorARR.Text = dataToParse[3];
            mainWindow.designatorSID.Text = dataToParse[5];
            mainWindow.designatorALT.Text = dataToParse[6];
        }

        public string getSID(string route)
        {
            string compiledString;

            for (int i = 0; i < route.Length; i++)
            {
                if (char.IsDigit(route[i]) == false)
                {
                    continue;
                }
                if (char.IsLetter(route[i+1]) == false)
                {
                    continue;
                }
                if (char.IsLetter(route[i - 3]) == false)
                {
                    continue;
                }

                compiledString = route.Substring(i-5);
                if (compiledString[1] == ' ')
                {
                    compiledString = compiledString.Substring(1);
                }
                compiledString = compiledString.Trim();
                string[] tempSeperation = compiledString.Split(' ');
                compiledString = tempSeperation[0];
                compiledString = compiledString.Trim();
                compiledString = compiledString.TrimEnd(',','>');
                string pattern = "^[a-zA-Z0-9]+$";
                if (Regex.IsMatch(compiledString, pattern) == false) { continue; }
                return compiledString;
            }

            return "nil";
        }
    }
}
