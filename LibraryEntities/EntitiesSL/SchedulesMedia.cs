using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class SchedulesMedia
    {
        public string? hourStart { get; set; }

        public string? hourEnd { get; set; }

        public List<string>? media { get; set; }

        public List<string>? days { get; set; }
    }
}
