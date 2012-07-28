using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.MalApi
{
    public struct UncertainDate : IEquatable<UncertainDate>
    {
        public int? Year { get; private set; }
        public int? Month { get; private set; }
        public int? Day { get; private set; }

        public UncertainDate(int year)
            : this()
        {
            Year = year;
            Month = null;
            Day = null;
        }

        public UncertainDate(int year, int month)
            : this()
        {
            Year = year;
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("month", string.Format("Month cannot be {0}.", month));
            }
            Month = month;
            Day = null;
        }

        public UncertainDate(int year, int month, int day)
            : this()
        {
            Year = year;
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("month", string.Format("Month cannot be {0}.", month));
            }
            Month = month;

            if (day < 1 || day > 31)
            {
                throw new ArgumentOutOfRangeException("day", string.Format("Day cannot be {0}.", day));
            }
            Day = day;
        }

        public static UncertainDate Unknown { get { return new UncertainDate(); } }

        public static UncertainDate FromMalDateString(string malDateString)
        {
            string[] yearMonthDay = malDateString.Split('-');
            if (yearMonthDay.Length != 3)
            {
                throw new FormatException(string.Format("{0} is not in YYYY-MM-DD format.", malDateString));
            }

            int year = int.Parse(yearMonthDay[0]);
            int month = int.Parse(yearMonthDay[1]);
            int day = int.Parse(yearMonthDay[2]);

            if (year == 0)
                return UncertainDate.Unknown;
            if (month == 0)
                return new UncertainDate(year);
            if (day == 0)
                return new UncertainDate(year: year, month: month);
            else
                return new UncertainDate(year: year, month: month, day: day);
        }

        public bool Equals(UncertainDate other)
        {
            return this.Year == other.Year && this.Month == other.Month && this.Day == other.Day;
        }

        public override bool Equals(object obj)
        {
            if (obj is UncertainDate)
                return Equals((UncertainDate)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 23;
                if (Year != null) hash = hash * 17 + Year.Value;
                if (Month != null) hash = hash * 17 + Month.Value;
                if (Day != null) hash += hash * 17 + Day.Value;
                return hash;
            }
        }

        public override string ToString()
        {
            string year;
            string month;
            string day;

            if (Year == null)
                year = "0000";
            else
                year = Year.Value.ToString("D4");

            if (Month == null)
                month = "00";
            else
                month = Month.Value.ToString("D2");

            if (Day == null)
                day = "00";
            else
                day = Day.Value.ToString("D2");

            return string.Format("{0}-{1}-{2}", year, month, day);
        }
    }
}

/*
 Copyright 2012 Greg Najda

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/