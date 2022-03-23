using System;

namespace CrimeAnalyzer
{
    public class CrimeStat
    {
        // CSV Inputs: Year,Population,Violent Crime,Murder,Rape,Robbery,Aggravated Assault,Property Crime,Burglary,Theft,Motor Vehicle Theft
        private int year;
        private int population;
        private int violentCrime;
        private int murder;
        private int rape;
        private int robbery;
        private int aggravatedAssault;
        private int propertyCrime;
        private int burglary;
        private int theft;
        private int motorVehicleTheft;
    
        public CrimeStat(int year) : this(year,0,0,0,0,0,0,0,0,0,0) {
        }

        public CrimeStat(int year, int population, int violentCrime, int murder, int rape, int robbery, int aggravatedAssault, int propertyCrime, int burglary, int theft, int motorVehicleTheft)
        {
            this.setYear(year);
            this.setPopulation(population);
            this.setViolentCrime(violentCrime);
            this.setMurder(murder);
            this.setRape(rape);
            this.setRobbery(robbery);
            this.setAggravatedAssault(aggravatedAssault);
            this.setPropertyCrime(propertyCrime);
            this.setBurglary(burglary);
            this.setTheft(theft);
            this.setMotorVehicleTheft(motorVehicleTheft);
        }

        public void setYear(int input) {
            if(input < 1970) {
                throw new ArgumentOutOfRangeException("Year cannot be before Jan 1, 1970. See https://en.wikipedia.org/wiki/Unix_time ");
            }
            else if(input > DateTime.Now.Year) {
                throw new ArgumentOutOfRangeException("If you know what crimes will happen in the future, either you are an accomplice or you are psychic. Either way, just no.");
            }

            this.year = input;
        }

        public int getYear() {
            return this.year;
        }

        public void setPopulation(int input) {
            if(input <= 0) {
                throw new ArgumentOutOfRangeException("Population cannot be 0 or negative");
            }

            this.population = input;
        }

        public int getPopulation() {
            return this.population;
        }

        public void setViolentCrime(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("Violent Crime cannot be negative");
            }

            this.violentCrime = input;
        }

        public int getViolentCrime() {
            return this.violentCrime;
        }

        public void setMurder(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("Murder cannot be negative");
            }

            this.murder = input;
        }

        public int getMurder() {
            return this.murder;
        }

        public void setRape(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("Rape cannot be negative");
            }

            this.rape = input;
        }

        public int getRape() {
            return this.rape;
        }

        public void setRobbery(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("Robbery cannot be negative");
            }

            this.robbery = input;
        }

        public int getRobbery() {
            return this.robbery;
        }

        public void setAggravatedAssault(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("Aggravated Assault cannot be negative");
            }

            this.aggravatedAssault = input;
        }

        public int getAggravatedAssault() {
            return this.aggravatedAssault;
        }

        public void setPropertyCrime(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("Property Crime cannot be negative");
            }

            this.propertyCrime = input;
        }

        public int getPropertyCrime() {
            return this.propertyCrime;
        }

        public void setBurglary(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("Burglary cannot be 0 or negative");
            }

            this.burglary = input;
        }

        public int getBurglary() {
            return this.burglary;
        }

        public void setTheft(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("Theft cannot be negative");
            }

            this.theft = input;
        }

        public int getTheft() {
            return this.theft;
        }

        public void setMotorVehicleTheft(int input) {
            if(input < 0) {
                throw new ArgumentOutOfRangeException("MotorVehicleTheft cannot be negative");
            }

            this.motorVehicleTheft = input;
        }

        public int getMotorVehicleTheft() {
            return this.motorVehicleTheft;
        }

    }
}