using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticDashboard.Core
{
    public class Coordinates
    {
        [Display(Name = "Latitude")]
        [Required(ErrorMessage = "Please select a location on the map.")]
        public double Latitude { get; set; }

        [Display(Name = "Longitude")]
        [Required(ErrorMessage = "Please select a location on the map.")]
        public double Longitude { get; set; }
    }

    public class PHPortMap
    {
        public int Id { get; set; }
        public string Carrier { get; set; }
        public string SailingLeadtime { get; set; }

        public string OriginPort { get; set; }
        [Column(TypeName = "jsonb")]
        public Coordinates OriginPortCoordinates { get; set; }

        public string? DestinationPort { get; set; }
        [Column(TypeName = "jsonb")]
        public Coordinates? DestinationPortCoordinates { get; set; }

        public string? PictureLocation { get; set; }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
