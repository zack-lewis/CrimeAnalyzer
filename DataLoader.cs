using System;
using System.Collections.Generic;
using System.IO;

namespace CrimeAnalyzer
{
    public class DataLoader
    {
        public static List<CrimeStat> getStatsList(string in_file)
        {
            List<CrimeStat> output = new List<CrimeStat>();
            // Parse input file, ignore first line
            using(StreamReader in_stream = new StreamReader(in_file)){
                string line;
                int lineNum = 1;

                while((line = in_stream.ReadLine()) != null){
                    if(line.StartsWith("Year")) {
                        continue;
                    }

                    lineNum++;

                    string[] tempArray = line.Split(",");

                    try {
                        CrimeStat tempStat = new CrimeStat(Int32.Parse(tempArray[0]),Int32.Parse(tempArray[1]),Int32.Parse(tempArray[2]),Int32.Parse(tempArray[3]),Int32.Parse(tempArray[4]),Int32.Parse(tempArray[5]),Int32.Parse(tempArray[6]),Int32.Parse(tempArray[7]),Int32.Parse(tempArray[8]),Int32.Parse(tempArray[9]),Int32.Parse(tempArray[10]));
                        output.Add(tempStat);
                    }
                    catch(IndexOutOfRangeException) {
                        Logger.logger($"Error parsing data on line { lineNum }: { line }");
                    }
                    catch(FormatException) {
                        Logger.logger($"Data Format inconsistent on line { lineNum }: { line }");
                    }
                    catch(Exception) {
                        throw;
                    }
                }
            }

            return output;
        } // end getDataList()
    }
}