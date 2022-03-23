using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrimeAnalyzer
{
    class Program
    {
        static List<CrimeStat> stats = null;

        static void Main(string[] args)
        {
            // Debug params
            bool debug = false;
            string debug_file = "CrimeData.csv";
            string debug_report = "Report.txt";

            // check args. If not 2, return help message and exit. 
            if (args.Length != 2 && !debug)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("CrimeAnalyzer <crime_csv_file_path> <output_report_file_path>");
                Console.WriteLine("<crime_csv_file_path>: CSV file with format ");
                Console.WriteLine("\tYear,Population,Violent Crime,Murder,Rape,Robbery,Aggravated Assault,Property Crime,Burglary,Theft,Motor Vehicle Theft");
                Console.WriteLine("<output_report_file_path>: Output file for full Crime Report");
                return;
            }

            string in_file;
            string out_file;

            if (args.Length != 2 && debug)
            {
                in_file = debug_file;
                out_file = debug_report;
            }
            else
            {
                in_file = args[0];
                out_file = args[1];
            }

            try {
                stats = DataLoader.getStatsList(in_file);
            }
            catch(Exception ex) {
                Logger.logger(ex.Message);
                Logger.logger(ex.StackTrace);
            }

            try {
                // Questions
                // The questions to be answered by the application that are to be used as the basis of a report that the application generates are the following.

                //     What is the range of years included in the data?
                string range = getRange();

                //     How many years of data are included?
                int years = countYears();
                string yearLine = $" ({ years } years) \n";

                //     What years is the number of murders per year less than 15000?
                List<int> yearsMurderLT15000 = getYearsMurderLT15k();
                string murderLT15k = "Years murders per year < 15000: ";
                foreach(int y in yearsMurderLT15000) {
                    murderLT15k += $"{ y.ToString() }, ";
                    
                }
                murderLT15k = murderLT15k.Remove(murderLT15k.Length - 2, 1);

                //     What are the years and associated robberies per year for years where the number of robberies is greater than 500000?
                List<CrimeStat> robberyGT500k = getRobberyGT500k();
                string robberyGT500kLine = "Robberies per year > 500000: ";
                foreach(CrimeStat s in robberyGT500k) {
                    robberyGT500kLine += $"{ s.getYear() } = { s.getRobbery() }, ";
                }
                robberyGT500kLine = robberyGT500kLine.Remove(robberyGT500kLine.Length - 2, 1);

                //     What is the violent crime per capita rate for 2010? Per capita rate is the number of violent crimes in a year divided by the size of the population that year.
                List<CrimeStat> record2010 = (from s in stats where(s.getYear() == 2010) select s).ToList();

                double violentCrime2010 = record2010[0].getViolentCrime();
                double population2010 = record2010[0].getPopulation();
                int year2010 = record2010[0].getYear(); // This is a dummy check
                double vcPerCapita2010 = violentCrime2010/population2010;
            
                string violentCrimePerCapita2010Line = $"Violent Crime per capita rate ({ year2010 }): { vcPerCapita2010 }";

                //     What is the average number of murders per year across all years?
                List<int> murderAllYears = (from s in stats
                                            select s.getMurder()).ToList<int>();
                double murderAvgAll = murderAllYears.Average();
                string murderAverageLine = $"Average murder per year (all years): { murderAvgAll }";


                //     What is the average number of murders per year for 1994 to 1997?
                List<int> murders1994to1997 = (from s in stats where (s.getYear() >= 1994 && s.getYear() <= 1997)
                                            select s.getMurder()).ToList<int>();
                double murderAvg1994to1997 = murders1994to1997.Average();
                string murderAverage1994to1997line = $"Average murder per year (1994-1997): { murderAvg1994to1997 }";

                //     What is the average number of murders per year for 2010 to 2013?
                List<int> murders2010to2013 = (from s in stats where (s.getYear() >= 2010 && s.getYear() <= 2013)
                                            select s.getMurder()).ToList<int>();
                double murderAvg2010to2013 = murders2010to2013.Average();
                string murderAverage2010to2013line = $"Average murder per year (2010-2013): { murderAvg2010to2013 }";

                //     What is the minimum number of thefts per year for 1999 to 2004?
                List<int> theft1999to2004 = (from s in stats where(s.getYear() >=1999 && s.getYear() <=2004) select s.getTheft()).ToList<int>();
                int minTheft1999to2004 = theft1999to2004.Min();
                string minTheft1999to2004Line = $"Minimum thefts per year (1999-2004): { minTheft1999to2004 }";

                //     What is the maximum number of thefts per year for 1999 to 2004?
                int maxTheft1999to2004 = theft1999to2004.Max();
                string maxTheft1999to2004Line = $"Minimum thefts per year (1999-2004): { maxTheft1999to2004 }";

                //     What year had the highest number of motor vehicle thefts?
                List<int> motorVehicleTheftYearsAsc = (from s in stats orderby s.getMotorVehicleTheft() descending select s.getYear()).ToList<int>();
                int highestMotorVehicleTheft = motorVehicleTheftYearsAsc[0];
                string highestMotorVehicleTheftLine = $"Year of highest number of motor vehicle thefts: { highestMotorVehicleTheft }";

                if(debug) {
                    Console.WriteLine("Crime Analyzer Report\n");
                    Console.Write(range);
                    Console.WriteLine(yearLine);
                    Console.WriteLine(murderLT15k);
                    Console.WriteLine(robberyGT500kLine);
                    Console.WriteLine(violentCrimePerCapita2010Line);
                    Console.WriteLine(murderAverageLine);
                    Console.WriteLine(murderAverage1994to1997line);
                    Console.WriteLine(murderAverage2010to2013line);
                    Console.WriteLine(minTheft1999to2004Line);
                    Console.WriteLine(maxTheft1999to2004Line);
                    Console.WriteLine(highestMotorVehicleTheftLine);
                }
                else {
                    using(StreamWriter reportWriter = new StreamWriter(out_file,false)) {

                        reportWriter.WriteLine("Crime Analyzer Report\n");
                        reportWriter.Write(range);
                        reportWriter.WriteLine(yearLine);
                        reportWriter.WriteLine(murderLT15k);
                        reportWriter.WriteLine(robberyGT500kLine);
                        reportWriter.WriteLine(violentCrimePerCapita2010Line);
                        reportWriter.WriteLine(murderAverageLine);
                        reportWriter.WriteLine(murderAverage1994to1997line);
                        reportWriter.WriteLine(murderAverage2010to2013line);
                        reportWriter.WriteLine(minTheft1999to2004Line);
                        reportWriter.WriteLine(maxTheft1999to2004Line);
                        reportWriter.WriteLine(highestMotorVehicleTheftLine);
                    }
                }
            }
            catch(Exception ex) {
                Logger.logger($"Error: { ex.Message } \n\t { ex.StackTrace }");
            }
        } // end Main

        private static List<CrimeStat> getRobberyGT500k()
        {
            List<CrimeStat> output = (from s in stats
                                    where s.getRobbery() > 500000
                                    select s).ToList<CrimeStat>();

            return output;    
        }

        public static string getRange() {
            string output = "";
            int min, max;
            min = (from s in stats 
                    select s.getYear()).Min();

            max = (from s in stats 
                    select s.getYear()).Max();  

            output = $"Period: { min } - { max }";
            return output;
        }
        
        public static int countYears() {
            int output = 0;

            output = (from i in stats
                    select i.getYear()).Count();

            return output;
        }

        public static List<int> getYearsMurderLT15k() {
            List<int> output;

            output = (from s in stats
                        where s.getMurder() < 15000
                        select s.getYear()).ToList<int>();

            return output;
        }

    }
}
