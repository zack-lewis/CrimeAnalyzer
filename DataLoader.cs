using System;
using System.Collections.Generic;
using System.IO;

namespace CrimeAnalyzer
{
    public class DataLoader
    {
        public static List<stat> getStatsList(string in_file)
        {
            List<stat> output = new List<stat>();
            // Parse input file, ignore first line
            using(StreamReader in_stream = new StreamReader(in_file)){
                string line;
                int lineNum = 0;

                while((line = in_stream.ReadLine()) != null){
                    if(line.StartsWith("Year")) {
                        continue;
                    }

                    lineNum++;

                    string[] tempArray = line.Split(",");

                    try {
                        stat tempStat = new stat(Int32.Parse(tempArray[0]),Int32.Parse(tempArray[1]),Int32.Parse(tempArray[2]),Int32.Parse(tempArray[3]),Int32.Parse(tempArray[4]),Int32.Parse(tempArray[5]),Int32.Parse(tempArray[6]),Int32.Parse(tempArray[7]),Int32.Parse(tempArray[8]),Int32.Parse(tempArray[9]),Int32.Parse(tempArray[10]));
                        output.Add(tempStat);
                    }
                    catch(IndexOutOfRangeException ex) {
                        Logger.logger($"Error parsing data one line { lineNum }: { line }");
                        Logger.logger(ex.Message);
                    }
                }
            }

            return output;
        } // end getDataList()
    }
}