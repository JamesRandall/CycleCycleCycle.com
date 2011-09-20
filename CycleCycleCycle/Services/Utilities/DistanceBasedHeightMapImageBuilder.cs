using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;
using CycleCycleCycle.Services.Implementation;

namespace CycleCycleCycle.Services.Utilities
{
    public class DistanceBasedHeightMapImageBuilder
    {
        private readonly IDistanceCalculator _distanceCalculator;

        public DistanceBasedHeightMapImageBuilder(IDistanceCalculator distanceCalculator)
        {
            _distanceCalculator = distanceCalculator;
        }

        public Bitmap HeightMapImage(Route route, int width, int height)
        {
            const double percentageSpace = 0.20;

            if (width > 1024 || height > 1024)
                throw new ServiceException("Requested height map is too large. Maximum size is 1024x1024 pixels");

            try
            {
                // work in progress
                /*Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);

                double minimumElevation = route.RoutePoints.Min(rp => rp.Elevation);
                double maximumElevation = route.RoutePoints.Max(rp => rp.Elevation);
                double elevationOffset = (maximumElevation - minimumElevation) * percentageSpace;
                minimumElevation = Math.Round(minimumElevation - elevationOffset / 2);
                maximumElevation = Math.Round(maximumElevation + elevationOffset / 2);

                double widthMetersPerPixel = route.Distance/(double) width;
                double heightMetersPerPixel = (double)height / (maximumElevation - minimumElevation);

                Brush backgroundBrush = new LinearGradientBrush(new Point(0, 0),
                    new Point(0, height),
                    Color.FromArgb(255, 0, 99, 248),
                    Color.FromArgb(255, 0, 164, 250));

                Brush foregroundBrush = new LinearGradientBrush(new Point(0, 0),
                                                                new Point(0, height),
                                                                Color.DarkGreen,
                                                                Color.LightGreen);

                graphics.FillRectangle(backgroundBrush, 0, 0, width, height);
                Pen pen = new Pen(foregroundBrush);
                double previousElevation = 0;
                GraphicsPath path = new GraphicsPath();
                Point oldPoint = new Point(0, 0);
                double currentDistance = 0.0;
                int routePointIndex = 0;
                List<RoutePoint> routePointsForPixel;
                for (int x = 0; x < width; x++)
                {
                    routePointsForPixel = new List<RoutePoint>();
                    double distance = 0;
                    while(distance < widthMetersPerPixel)
                    {
                        
                    }
                    

                    
                    IEnumerable<RoutePoint> routePoints = route.RoutePoints.Where(rp => rp.TimeRecorded >= currentDate && rp.TimeRecorded <= endDate);
                    double averageElevation = 0;
                    if (routePoints.Any())
                    {
                        averageElevation = routePoints.Average(rp => rp.Elevation);
                    }
                    else
                    {
                        averageElevation = previousElevation;
                    }
                    graphics.DrawLine(pen, (float)x, height, (float)x, height - (float)((averageElevation - minimumElevation) * heightMetersPerPixel));
                    Point point = new Point(x, (int)(height - (float)((averageElevation - minimumElevation) * heightMetersPerPixel)));

                    if (x >= 1)
                    {
                        Point newPoint = point;
                        path.AddLine(oldPoint, newPoint);
                        oldPoint = newPoint;
                    }
                    else
                    {
                        oldPoint = point;
                    }

                    currentDate = currentDate.AddMinutes(minutesPerPixel);
                    previousElevation = averageElevation;
                }
                Pen linePen = new Pen(Color.Black, 3);
                linePen.DashCap = DashCap.Round;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.DrawPath(linePen, path);

                return bitmap;*/
                throw new NotImplementedException("Work in progress!");
            }
            catch (Exception ex)
            {
                throw new ServiceException("Unable to build height profile bitmap.", ex);
            }
        }
    }
}