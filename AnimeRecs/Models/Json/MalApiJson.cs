using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AnimeRecs.Models
{
    public class MalApiJson
    {
        [Required]
        public IList<MalApiJsonAnime> anime { get; set; }
    }
}