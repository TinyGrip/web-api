using OutdoorSolution.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Common
{
    public static class Utils
    {
        public static DbGeography CreateDbPoint(GeographyDto geoDto)
        {
            var wellKnownText = String.Format("POINT ({0} {1})", geoDto.Latitude, geoDto.Longitude);
            return DbGeography.FromText(wellKnownText);
        }
    }
}
