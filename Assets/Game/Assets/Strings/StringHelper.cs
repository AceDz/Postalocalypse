using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    public static class StringHelper
    {
        /// <summary>
        /// Format a big number as a time
        /// </summary>
        /// <param name="value">Value to convert to time</param>
        /// <param name="showMinorZeros">If False, won't show seconds if there are hours</param>
        /// <returns>Value with format "{0}h {1:00}m {2:00}s"</returns>
        public static string FormatAsDigitalTime(double value, bool showSeconds = true)
        {
            double hours = Math.Floor(value / 3600);
            double mins = Math.Floor((value / 60) % 60);

            if (showSeconds)
            {
                double secs = Math.Floor(value % 60);
                return string.Format("{0:00}:{1:00}:{2:00}", hours, mins, secs);
            }
            else
            {
                return string.Format("{0:00}:{1:00}", hours, mins);
            }
        }


        public static string FormatAsTime(double value, bool showMinorZeros = true,
                string secsSuffix = "s", string minsSuffix = "m",
                string hoursSuffix = "h", string daysSuffix = "d")
        {
            if (value <= 0)
            {
                return "0" + secsSuffix;
            }
            else
            {

                double hours = Math.Floor(value / 3600);
                if (hours > 0)
                {
                    if (hours >= 24)
                    {
                        double days = Math.Floor(hours / 24);
                        //hours = hours % 24;
                        hours = hours - days * 24; // Better performance
                        if (showMinorZeros || hours > 0)
                        {
                            return string.Format("{0}{1} {2}{3}", days, daysSuffix, hours, hoursSuffix);
                        }
                        else
                        {
                            return string.Format("{0}{1}", days, daysSuffix);
                        }
                    }
                    else
                    {

                        double mins = Math.Floor((value / 60) % 60);
                        if (showMinorZeros || mins > 0)
                        {
                            return string.Format("{0}{1} {2:00}{3}", hours, hoursSuffix,
                                    mins, minsSuffix);
                        }
                        else
                        {
                            return string.Format("{0}{1}", hours, hoursSuffix);
                        }
                    }
                }
                else
                {
                    double mins = Math.Floor(value / 60);
                    //double secs = value % 60;
                    double secs = value - mins * 60; // Better performance
                    if (mins > 0)
                    {
                        if (showMinorZeros || secs > 0)
                        {
                            return string.Format("{0}{1} {2:00}{3}", mins, minsSuffix, secs, secsSuffix);
                        }
                        else
                        {
                            return string.Format("{0}{1}", mins, minsSuffix);
                        }

                    }
                    else
                    {
                        return string.Format("{0:0.0}{1}", secs, secsSuffix);
                    }
                }
            }
        }
    }
}