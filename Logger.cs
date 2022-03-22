using System;
using System.IO;

namespace CrimeAnalyzer
{
    public static class Logger
    {
        public static bool logger(string message) {
            try {
                // Write to log file
                using(StreamWriter logFile = new StreamWriter("CrimeAnalyzer.log",true)) {
                    logFile.WriteLine(message);
                }
                return true;
            }
            catch(Exception ex) {
                Console.WriteLine($"Error writing logs: { ex.Message }");
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
    }
}