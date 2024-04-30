using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public string[] ParsedData = new string[7];
        private bool isParsingToTextboxes = false;

        public bool tryParse(string flightplan)
        {
            try
            {
                string[] flightplanData = flightplan.Split('\n');
                for (int i = 0; i < ParsedData.Length; i++)
                {
                    if (flightplanData[i].IndexOf("Callsign:") != -1)
                    {
                        string tcallsign = flightplanData[i].Substring(10);
                        Regex regex = new Regex("[^a-zA-Z0-9()]");
                        string callsign = regex.Replace(tcallsign, "");
                        if (callsign.IndexOf('(') != -1)
                        {
                            callsign = callsign.Substring(0, callsign.IndexOf('('));
                        }
                        ParsedData[0] = callsign;
                        RouteData[0] = ParsedData[0];
                    }
                    else if (flightplanData[i].IndexOf("Flight Rules:") != -1)
                    {
                        ParsedData[1] = flightplanData[i].Substring(14);
                        RouteData[1] = ParsedData[1];
                    }
                    else if (flightplanData[i].IndexOf("Departing:") != -1)
                    {
                        ParsedData[2] = flightplanData[i].Substring(11);
                        RouteData[2] = ParsedData[2];
                    }
                    else if (flightplanData[i].IndexOf("Arriving:") != -1)
                    {
                        ParsedData[3] = flightplanData[i].Substring(10);
                        RouteData[3] = ParsedData[3];
                    }
                    else if (flightplanData[i].IndexOf("Route:") != -1)
                    {
                        ParsedData[4] = flightplanData[i].Substring(7);
                        RouteData[4] = ParsedData[4];
                        ParsedData[5] = getSID(ParsedData[4]);
                        RouteData[5] = ParsedData[5];
                    }
                    else if (flightplanData[i].IndexOf("Flight Level:") != -1)
                    {
                        ParsedData[6] = flightplanData[i].Substring(14);
                        RouteData[6] = ParsedData[6];
                    }

                }
                parseToTextboxes(ParsedData);
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
            isParsingToTextboxes = true;
            mainWindow.designatorCLL.Text = dataToParse[0];
            mainWindow.designatorDEP.Text = dataToParse[2];
            mainWindow.designatorARR.Text = dataToParse[3];
            mainWindow.designatorSID.Text = dataToParse[5];
            mainWindow.designatorALT.Text = dataToParse[6];
            isParsingToTextboxes = false;
        }

        public void parseToParsedData()
        {
            if (isParsingToTextboxes) { return; }
            ParsedData[0] = mainWindow.designatorCLL.Text;
            ParsedData[2] = mainWindow.designatorDEP.Text;
            ParsedData[3] = mainWindow.designatorARR.Text;
            ParsedData[5] = mainWindow.designatorSID.Text;
            ParsedData[6] = mainWindow.designatorALT.Text;
        }

        public string getSID(string route)
        {
            string compiledString;

            for (int i = 0; i < route.Length-2; i++)
            {
                try {
                    if (char.IsDigit(route[i]) == false)
                    {
                        continue;
                    }
                    if (char.IsLetter(route[i + 1]) == false)
                    {
                        continue;
                    }
                    if (char.IsLetter(route[i - 3]) == false)
                    {
                        continue;
                    }

                    compiledString = route.Substring(i - 5);
                    if (compiledString[1] == ' ')
                    {
                        compiledString = compiledString.Substring(1);
                    }
                    compiledString = compiledString.Trim();
                    string[] tempSeperation = compiledString.Split(' ');
                    compiledString = tempSeperation[0];
                    compiledString = compiledString.Trim();
                    compiledString = compiledString.TrimEnd(',', '>', '/');
                    if (compiledString.IndexOf('/') != -1)
                    {
                        compiledString = compiledString.Substring(0, compiledString.IndexOf('/'));
                    }
                    //string pattern = "^[a-zA-Z0-9]+$";
                    //if (Regex.IsMatch(compiledString, pattern) == false) { continue; }
                    if (compiledString.Length<5)
                    {
                        continue;
                    }
                    return compiledString;
                }
                catch
                {
                    continue;
                }
            }

            return "nil";
        }
    }
}
