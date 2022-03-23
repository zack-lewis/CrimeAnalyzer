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
            catch(FileNotFoundException ex) {
                Logger.logger($"Cannot load file: { ex.Message }");
                return;
            }
            catch(Exception ex) {
                Logger.logger($"Otherwise unhandled error ({ ex.GetType().Name }): { ex.Message }");
                //Logger.logger(ex.StackTrace);
            }

            // Try writing the report (console for debug)
            try {
                if(debug) {
                    Console.WriteLine("Crime Analyzer Report\n");
                    Console.Write(getRange());
                    Console.WriteLine(countYears());
                    Console.WriteLine(getYearsMurderLT15k());
                    Console.WriteLine(getRobberyGT500k());
                    Console.WriteLine(getViolentCrimePerCapita2010());
                    Console.WriteLine(getMurderAverageAll());
                    Console.WriteLine(getMurderAverage1994to1997());
                    Console.WriteLine(getMurderAverage2010to2013());
                    Console.WriteLine(getMinTheft1999to2004());
                    Console.WriteLine(getMaxTheft1999to2004());
                    Console.WriteLine(getHighestMotorVehicleTheftLine());
                }
                else {
                    using(StreamWriter reportWriter = new StreamWriter(out_file,false)) {
                        // Questions
                        // The questions to be answered by the application that are to be used as the basis of a report that the application generates are the following.   
                        reportWriter.WriteLine("Crime Analyzer Report\n");
                        //     What is the range of years included in the data?
                        reportWriter.Write(getRange());
                        //     How many years of data are included?
                        reportWriter.WriteLine(countYears());
                        //     What years is the number of murders per year less than 15000?
                        reportWriter.WriteLine(getYearsMurderLT15k());
                        //     What are the years and associated robberies per year for years where the number of robberies is greater than 500000?
                        reportWriter.WriteLine(getRobberyGT500k());
                        //     What is the violent crime per capita rate for 2010? Per capita rate is the number of violent crimes in a year divided by the size of the population that year.
                        reportWriter.WriteLine(getViolentCrimePerCapita2010());
                        //     What is the average number of murders per year across all years?
                        reportWriter.WriteLine(getMurderAverageAll());
                         //     What is the average number of murders per year for 1994 to 1997?
                        reportWriter.WriteLine(getMurderAverage1994to1997());
                        //     What is the average number of murders per year for 2010 to 2013?
                        reportWriter.WriteLine(getMurderAverage2010to2013());
                        //     What is the minimum number of thefts per year for 1999 to 2004?
                        reportWriter.WriteLine(getMinTheft1999to2004());
                        //     What is the maximum number of thefts per year for 1999 to 2004?
                        reportWriter.WriteLine(getMaxTheft1999to2004());
                        //     What year had the highest number of motor vehicle thefts?
                        reportWriter.WriteLine(getHighestMotorVehicleTheftLine());
                    }
                }
            }
            catch(Exception ex) {
                Logger.logger($"Error ({ ex.GetType().Name }): { ex.Message } \n\t { ex.StackTrace }");
            }
        } // end Main

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
        
        public static string countYears() {
            string output = "";
            int years = 0;
            years = (from i in stats
                    select i.getYear()).Count();
            output = $"{ years } years \n";
            return output;
        }

        public static string getYearsMurderLT15k() {
            
            List<int> yearsMurderLT15000 = (from s in stats
                        where s.getMurder() < 15000
                        select s.getYear()).ToList<int>();

            string output = "Years murders per year < 15000: ";

            foreach(int y in yearsMurderLT15000) {
                output += $"{ y.ToString() }, ";
                
            }

            output = output.Remove(output.Length - 2, 1);

            return output;
        }

        public static string getRobberyGT500k() {
            List<CrimeStat> robberyGT500k = (from s in stats
                                    where s.getRobbery() > 500000
                                    select s).ToList<CrimeStat>();
            string output = "Robberies per year > 500000: ";
            foreach(CrimeStat s in robberyGT500k) {
                output += $"{ s.getYear() } = { s.getRobbery() }, ";
            }
            output = output.Remove(output.Length - 2, 1);

            return output;
        }

        public static string getViolentCrimePerCapita2010() {
            List<CrimeStat> record2010 = (from s in stats where(s.getYear() == 2010) select s).ToList();

            double violentCrime2010 = record2010[0].getViolentCrime();
            double population2010 = record2010[0].getPopulation();
            int year2010 = record2010[0].getYear(); // This is a dummy check
            double vcPerCapita2010 = violentCrime2010/population2010;
        
            string output = $"Violent Crime per capita rate ({ year2010 }): { vcPerCapita2010 }";

            return output;
        }

        public static string getMurderAverageAll() {
            List<int> murderAllYears = (from s in stats
                                        select s.getMurder()).ToList<int>();
            double murderAvgAll = murderAllYears.Average();
            string output = $"Average murder per year (all years): { murderAvgAll }";

            return output;
        }

        public static string getMurderAverage1994to1997() {
            List<int> murders1994to1997 = (from s in stats where (s.getYear() >= 1994 && s.getYear() <= 1997)
                                        select s.getMurder()).ToList<int>();
            double murderAvg1994to1997 = murders1994to1997.Average();
            string output = $"Average murder per year (1994-1997): { murderAvg1994to1997 }";

            return output;
        }

        public static string getMurderAverage2010to2013() {
            List<int> murders2010to2013 = (from s in stats where (s.getYear() >= 2010 && s.getYear() <= 2013)
                                        select s.getMurder()).ToList<int>();
            double murderAvg2010to2013 = murders2010to2013.Average();
            string output = $"Average murder per year (2010-2013): { murderAvg2010to2013 }";

            return output;
        }

        public static string getMinTheft1999to2004() {
            List<int> theft1999to2004 = (from s in stats where(s.getYear() >=1999 && s.getYear() <=2004) select s.getTheft()).ToList<int>();
            int minTheft1999to2004 = theft1999to2004.Min();
            string output = $"Minimum thefts per year (1999-2004): { minTheft1999to2004 }";

            return output;
        }

        public static string getMaxTheft1999to2004() {
            List<int> theft1999to2004 = (from s in stats where(s.getYear() >=1999 && s.getYear() <=2004) select s.getTheft()).ToList<int>();
            int maxTheft1999to2004 = theft1999to2004.Max();
            string output = $"Maximum thefts per year (1999-2004): { maxTheft1999to2004 }";

            return output;
        }

        public static string getHighestMotorVehicleTheftLine() {
            List<int> motorVehicleTheftYearsAsc = (from s in stats orderby s.getMotorVehicleTheft() descending select s.getYear()).ToList<int>();
            int highestMotorVehicleTheft = motorVehicleTheftYearsAsc[0];
            string output = $"Year of highest number of motor vehicle thefts: { highestMotorVehicleTheft }";

            return output;
        }

    }
}
