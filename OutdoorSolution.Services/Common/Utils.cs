using OutdoorSolution.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OutdoorSolution.Services.Common
{
    // TODO remane!
    public static class Utils
    {
        public static DbGeography CreateDbPoint(GeographyDto geoDto)
        {
            var wellKnownText = String.Format("POINT ({0} {1})", 
                geoDto.Latitude.ToString(CultureInfo.InvariantCulture),
                geoDto.Longitude.ToString(CultureInfo.InvariantCulture));
            return DbGeography.FromText(wellKnownText);
        }

        public static GeographyDto CreateGeoDto(DbGeography dbGeo)
        {
            if (dbGeo != null && dbGeo.Longitude.HasValue && dbGeo.Latitude.HasValue)
            {
                return new GeographyDto()
                {
                    Longitude = dbGeo.Longitude.Value,
                    Latitude = dbGeo.Latitude.Value
                };
            }
            else
                return null;
        }

        /// <summary>
        /// Convert DbGeometry to array of Points
        /// </summary>
        /// <param name="dbGeo"></param>
        /// <returns></returns>
        public static IEnumerable<PointDto> ConvertDbGeometry(DbGeometry dbGeo)
        {
            if (!dbGeo.PointCount.HasValue || dbGeo.PointCount.Value == 0)
                return new PointDto[0];

            var points = new PointDto[dbGeo.PointCount.Value];
            for (int i = 1; i <= dbGeo.PointCount.Value; i++)
            {
                // we assume that this DbGeometry contains only points, not a collection of other DbGeometries
                var dbGeoPoint = dbGeo.PointAt(i);
                if (dbGeoPoint.SpatialTypeName.ToLower() != "point")
                    throw new Exception("Wrong spatial time in db geometry");

                var newPoint = new PointDto(dbGeoPoint.XCoordinate.Value, dbGeoPoint.YCoordinate.Value);
                points[i - 1] = newPoint;
            }   

            return points;
        }

        /// <summary>
        /// Creates LINESTRING geometry from array of points
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static DbGeometry CreateDbGeometry(IEnumerable<PointDto> points)
        {
            StringBuilder wellKnownText = new StringBuilder("LINESTRING (");

            foreach (var point in points)
	        {
                wellKnownText.Append(point.X + " " + point.Y + ",");
	        }
            wellKnownText[wellKnownText.Length - 1] = ')';

            return DbGeometry.FromText(wellKnownText.ToString());
        }

        public static bool IsHttpUrl(string url)
        {
            return url.StartsWith("http://") || url.StartsWith("https://");
        }
    }
}
