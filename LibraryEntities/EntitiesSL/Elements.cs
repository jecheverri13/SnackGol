using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class Article
    {
        [JsonProperty("Código de artículo")]
        public string code { get; set; }
        [JsonProperty("Descripción del artículo")]
        public string name { get; set; }
        [JsonProperty("Valor Venta Articulo")]
        public decimal price { get; set; }
        [JsonProperty("Valor Excedente")]
        public decimal exceValue { get; set; }
    }

    public class NewArticle
    {
        public string code { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal exceValue { get; set; }
    }

    public class ApiResponse
    {
        public List<Article> response { get; set; }
    }
}
