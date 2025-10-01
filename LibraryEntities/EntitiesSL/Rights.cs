using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class Rights
    {
        [JsonProperty("DocEntry")]
        public string document { get; set; }

        [JsonProperty("U_Codigo")]
        public string code { get; set; }

        [JsonProperty("LineId")]
        public string line { get; set; }

        [JsonProperty("U_Nombre")]
        public string description { get; set; }

        [JsonProperty("U_basemin")]
        public float minBase { get; set; }

        [JsonProperty("U_valor")]
        public float value { get; set; }

        [JsonProperty("U_ValMin")]
        public float minValue { get; set; }

        [JsonProperty("U_alertaEnv")]
        public string? send { get; set; }

        [JsonProperty("U_mtdoPago")]
        public string? paymentMethod { get; set; }

        [JsonProperty("U_itemCodeDer")]
        public string? itemCode { get; set; }

        [JsonProperty("U_almacen")]
        public string? warehouse { get; set; }

        [JsonProperty("U_codSN")]
        public string? codeBP { get; set; }

        [JsonProperty("U_nombreSN")]
        public string? nameBP { get; set; }
    }
}
