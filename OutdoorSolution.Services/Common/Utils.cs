using OutdoorSolution.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Common
{
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
    }
}
