using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rs.App.DotNet.WebApi.RouterFilters.ViewModels
{
    public class Hi
    {
        [Required]
        [MaxLength(12)]
        public string Name { get; set; }
        [RegularExpression(@"^[A-Za-z0-9]{4,12}$")]
        public string Regular { get; set; }
        [Range(0, 100)]
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public DateTime? Date1 { get; set; }
    }
}