using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class QueriesResponse
    {
        public int QueriesId { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public Dictionary<string, string> Param { get; set; }
        public PagResponse Pag { get; set; }
        public string HostApiQueries { get; set; }
    }

    public class PagResponse
    {
        public int Inicio { get; set; }
        public int Fin { get; set; }
    }

}
