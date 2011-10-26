using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AnimeRecs.Models
{
    public class MalApiJsonAnime
    {
        [Required] public int id { get; set; }
        public decimal score { get; set; }
        [Required] public string watched_status { get; set; }
        [Required] public string title { get; set; }
    }
}