using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.RecEngine.MalEvaluationRunner
{
    class Config
    {
        public ConfigConnectionStrings ConnectionStrings { get; set; }

        public class ConfigConnectionStrings
        {
            public string AnimeRecs { get; set; }
        }
    }
}
