using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static string LongDistance(this HtmlHelper helper, double distance)
        {
            return String.Format("{0} km", Math.Round(distance/1000));
        }

        public static string Distance(this HtmlHelper helper, double distance)
        {
            return String.Format("{0} m", Math.Round(distance));
        }

        public static string Rating(this HtmlHelper helper, double? averageRating)
        {
            if (averageRating.HasValue)
                return String.Format("{0}/5", (int) Math.Floor(averageRating.Value));
            return "Unrated";
        }

        public static string TimeTaken(this HtmlHelper helper, TimeSpan? timeTaken)
        {
            if (timeTaken.HasValue)
            {
                return String.Format("{0} hours {1} minutes {2} seconds", timeTaken.Value.Hours, timeTaken.Value.Minutes,
                                     timeTaken.Value.Seconds);
            }
            return "No time recorded";
        }
    }
}