using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class QueriesRequest
    {
        public string Name { get; set; }
        public string Module { get; set; }
        public Dictionary<string, string> Param { get; set; }
        public PagRequest Pag { get; set; }
        public string HostApiQueries { get; set; }
    }

    public class PagRequest
    {
        public int Inicio { get; set; }
        public int Fin { get; set; }
    }
}
