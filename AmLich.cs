using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGoTiengViet
{
    class AmLich
    {
        public long jdFromDate(int dd, int mm, int yy)
        {           
            var a = (int)(14 - mm) / 12;
            var y = yy + 4800 - a;
            var m = mm + 12 * a - 3;
            var jd = dd
                + (int)((153 * m + 2) / 5.0f) 
                + (365 * y) 
                + (int)(y / 4.0f) - (int)(y / 100.0f) + (int)(y / 400.0f) - 32045;
            if (jd < 2299161)
            {
                jd = dd + (int)((153 * m + 2) / 5.0f) + 365 * y + (int)(y / 4.0f) - 32083;
            }
            return jd;
        }
        public int[] jdToDate(long jd)
        {
            long a, b, c, d, e, m;
            int day, month, year;
            // After 5/10/1582, Gregorian calendar
            if (jd > 2299160)
            { 
                a = jd + 32044;
                b = (int)((4 * a + 3) / 146097.0f);
                c = a - (int)((b * 146097) / 4.0f);
            }
            else
            {
                b = 0;
                c = jd + 32082;
            }
            d = (int)((4 * c + 3) / 1461.0f);
            e = c - (int)((1461 * d) / 4.0f);
            m = (int)((5 * e + 2) / 153.0f);
            day = (int)(e - (int)((153 * m + 2) / 5.0f) + 1);
            month = (int)(m + 3 - 12 * (int)(m / 10.0f));
            year = (int)(b * 100 + d - 4800 + (int)(m / 10.0f));
            return new int[] { day, month, year };
        }

        // Tinh ngay Soc
        public int getNewMoonDay(int k, int timeZone)
        {
            // T, T2, T3, dr, Jd1, M, Mpr, F, C1, deltat, JdNew;
            var T = k / 1236.85f; // Time in Julian centuries from 1900 January 0.5
            var T2 = T * T;
            var T3 = T2 * T;
            var dr = Math.PI / 180.0f;
            var Jd1 = 2415020.75933 + 29.53058868 * k + 0.0001178 * T2 - 0.000000155 * T3;
            Jd1 = Jd1 + 0.00033 * Math.Sin((166.56 + 132.87 * T - 0.009173 * T2) * dr); // Mean new moon
            var M = 359.2242 + 29.10535608 * k - 0.0000333 * T2 - 0.00000347 * T3; // Sun's mean anomaly
            var Mpr = 306.0253 + 385.81691806 * k + 0.0107306 * T2 + 0.00001236 * T3; // Moon's mean anomaly
            var F = 21.2964 + 390.67050646 * k - 0.0016528 * T2 - 0.00000239 * T3; // Moon's argument of latitude
            var C1 = (0.1734 - 0.000393 * T) * Math.Sin(M * dr) + 0.0021 * Math.Sin(2 * dr * M);
            C1 = C1 - 0.4068 * Math.Sin(Mpr * dr) + 0.0161 * Math.Sin(dr * 2 * Mpr);
            C1 = C1 - 0.0004 * Math.Sin(dr * 3 * Mpr);
            C1 = C1 + 0.0104 * Math.Sin(dr * 2 * F) - 0.0051 * Math.Sin(dr * (M + Mpr));
            C1 = C1 - 0.0074 * Math.Sin(dr * (M - Mpr)) + 0.0004 * Math.Sin(dr * (2 * F + M));
            C1 = C1 - 0.0004 * Math.Sin(dr * (2 * F - M)) - 0.0006 * Math.Sin(dr * (2 * F + Mpr));
            C1 = C1 + 0.0010 * Math.Sin(dr * (2 * F - Mpr)) + 0.0005 * Math.Sin(dr * (2 * Mpr + M));
            double deltat;
            if (T < -11)
            {
                 deltat = 0.001 + 0.000839 * T + 0.0002261 * T2 - 0.00000845 * T3 - 0.000000081 * T * T3;
            }
            else
            {
                deltat = -0.000278 + 0.000265 * T + 0.000262 * T2;
            };
            var JdNew = Jd1 + C1 - deltat;
            return (int)(JdNew + 0.5 + timeZone / 24.0f);
        }

        // Trung khi
        public int getSunLongitude(double jdn, int timeZone)
        {
            //double T, T2, dr, M, L0, DL, L;
            var T = (jdn - 2451545.5 - timeZone / 24) / 36525; // Time in Julian centuries from 2000-01-01 12:00:00 GMT
            var T2 = T * T;
            var dr = Math.PI / 180; // degree to radian
            var M = 357.52910 + 35999.05030 * T - 0.0001559 * T2 - 0.00000048 * T * T2; // mean anomaly, degree
            var L0 = 280.46645 + 36000.76983 * T + 0.0003032 * T2; // mean longitude, degree
            var DL = (1.914600 - 0.004817 * T - 0.000014 * T2) * Math.Sin(dr * M);
            DL = DL + (0.019993 - 0.000101 * T) * Math.Sin(dr * 2 * M) + 0.000290 * Math.Sin(dr * 3 * M);
            var L = L0 + DL; // true longitude, degree
            L = L * dr;
            L = L - Math.PI * 2 * ((int)(L / (Math.PI * 2))); // Normalize to (0, 2*PI)
            return (int)(L / Math.PI * 6);
        }

        // Tim ngay bat dau thang 11 am lich
        public int getLunarMonth11(int yy, int timeZone)
        {            
            var off = jdFromDate(31, 12, yy) - 2415021;  // truoc 31/12/yy
            var k = (int)(off / 29.530588853);
            var nm = getNewMoonDay(k, timeZone); // tim ngay soc truoc 31/12/yy
            var sunLong = getSunLongitude(nm, timeZone); // sun longitude at local midnight
            if (sunLong >= 9) // Neu thang bat dau vau ngay soc do khong co dong chi, 
            {
                nm = getNewMoonDay(k - 1, timeZone); // thi lui 1 thang va tinh lai ngay soc
            }
            return nm;
        }

        // Xac dinh thang nhuan
        public int getLeapMonthOffset(long a11, int timeZone)
        {
            int k, last, arc, i;
            k = (int)((a11 - 2415021.076998695) / 29.530588853 + 0.5);
            last = 0;
            i = 1; // We start with the month following lunar month 11
            arc = getSunLongitude(getNewMoonDay(k + i, timeZone), timeZone);
            do
            {
                last = arc;
                i++;
                arc = getSunLongitude(getNewMoonDay(k + i, timeZone), timeZone);
            } while (arc != last && i < 14);
            return i - 1;
        }

        // Duong lich sang Am lich 
        public int[] convertSolar2Lunar(int dd, int mm, int yy, int timeZone)
        {           
            var dayNumber = jdFromDate(dd, mm, yy);
            var k = (int)((dayNumber - 2415021.076998695) / 29.530588853);
            var monthStart = getNewMoonDay(k + 1, timeZone);
            if (monthStart > dayNumber)
            {
                monthStart = getNewMoonDay(k, timeZone);
            }
            var a11 = getLunarMonth11(yy, timeZone);
            var b11 = a11;

            int lunarYear;
            if (a11 >= monthStart)
            {
                lunarYear = yy;
                a11 = getLunarMonth11(yy - 1, timeZone);
            }
            else
            {
                lunarYear = yy + 1;
                b11 = getLunarMonth11(yy + 1, timeZone);
            }
            var lunarDay = (int)(dayNumber - monthStart + 1);
            var diff = (int)((monthStart - a11) / 29);
            var lunarLeap = 0;
            var lunarMonth = diff + 11;
            if (b11 - a11 > 365)
            {
                var leapMonthDiff = getLeapMonthOffset(a11, timeZone);
                if (diff >= leapMonthDiff)
                {
                    lunarMonth = diff + 10;
                    if (diff == leapMonthDiff)
                    {
                        lunarLeap = 1;
                    }
                }
            }
            if (lunarMonth > 12)
            {
                lunarMonth = lunarMonth - 12;
            }
            if (lunarMonth >= 11 && diff < 4)
            {
                lunarYear -= 1;
            }
            return new int[] { lunarDay, lunarMonth, lunarYear };
        }

        // Am lich sang Duong lich
        public int[] convertLunar2Solar(int lunarDay, int lunarMonth, int lunarYear, int lunarLeap, int timeZone)
        {
            int k, off, leapOff;            
            long a11, b11, leapMonth, monthStart;
            if (lunarMonth < 11)
            {
                a11 = getLunarMonth11(lunarYear - 1, timeZone);
                b11 = getLunarMonth11(lunarYear, timeZone);
            }
            else
            {
                a11 = getLunarMonth11(lunarYear, timeZone);
                b11 = getLunarMonth11(lunarYear + 1, timeZone);
            }
            off = lunarMonth - 11;
            if (off < 0)
            {
                off += 12;
            }
            if (b11 - a11 > 365)
            {
                leapOff = getLeapMonthOffset(a11, timeZone);
                leapMonth = leapOff - 2;
                if (leapMonth < 0)
                {
                    leapMonth += 12;
                }
                if (lunarLeap != 0 && lunarMonth != leapMonth)
                {
                    return new int[] { 0, 0, 0 };
                }
                else if (lunarLeap != 0 || off >= leapOff)
                {
                    off += 1;
                }
            }
            k = (int)(0.5 + (a11 - 2415021.076998695) / 29.530588853);
            monthStart = getNewMoonDay(k + off, timeZone);
            return jdToDate(monthStart + lunarDay - 1);
        }
    }
}
