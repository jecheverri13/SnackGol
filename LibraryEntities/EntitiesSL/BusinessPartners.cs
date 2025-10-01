using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class BusinessPartners
    {
        [JsonProperty("CardCode")]
        public string? identification { get; set; }

        [JsonProperty("CardName")]
        public string? cardName { get; set; }

        [JsonProperty("FederalTaxID")]
        public  string? federalTaxID { get; set; }

        [JsonProperty("U_BPCO_TDC")]
        public  string? typeIdentification { get; set; }

        [JsonProperty("U_BPCO_1Apellido")]
        public string? lastName { get; set; }

        [JsonProperty("U_BPCO_2Apellido")]
        public  string? secondLastName { get; set; }

        [JsonProperty("U_BPCO_Nombre")]
        public  string? firstName { get; set; }

        [JsonProperty("U_BPCO_RTC")]
        public  string? taxRegime { get; set; }

        [JsonProperty("U_BPCO_TP")]
        public  string? typeOfPerson { get; set; }

        [JsonProperty("U_Depto")]
        public  string? department { get; set; }

        [JsonProperty("U_BPCO_CS")]
        public  string? city { get; set; }

        [JsonProperty("U_Barrio")]
        public  string? neighborhood { get; set; }

        [JsonProperty("U_BPCO_Address")]
        public  string? address { get; set; }

        [JsonProperty("U_dir1")]
        public  string? address1 { get; set; }

        [JsonProperty("U_dir2")]
        public  string? address2 { get; set; }

        [JsonProperty("U_dir3")]
        public  string? address3 { get; set; }

        [JsonProperty("U_dir4")]
        public  string? address4 { get; set; }

        [JsonProperty("U_dir5")]
        public  string? address5 { get; set; }

        [JsonProperty("U_tipoviv")]
        public  string? typeOfHousing { get; set; }

        [JsonProperty("U_estrato")]
        public  int? socialStratum { get; set; }

        [JsonProperty("U_sexo")]
        public string? gender { get; set; }

        [JsonProperty("U_FecExp")]
        public  string? expidationDate { get; set; }

        [JsonProperty("U_fecnac")]
        public string? birthDate { get; set; }

        [JsonProperty("Phone1")]
        public string? phone { get; set; }

        [JsonProperty("Phone2")]
        public string? alternativePhone { get; set; }

        [JsonProperty("Cellular")]
        public string? cellular { get; set; }

        [JsonProperty("U_estciv")]
        public string? civilStatus { get; set; }

        [JsonProperty("U_Ocupac")]
        public string? occupation { get; set; }

        [JsonProperty("U_addInFaElectronica_email_contacto_FE")]
        public string? email { get; set; }

        [JsonProperty("U_frontOfDocument")]
        public string? frontOfDocument { get; set; }

        [JsonProperty("U_backOfDocument")]
        public string? backOfDocument { get; set; }

        [JsonProperty("EmailAddress")]
        public string? emailAddress { get; set; }

        [JsonProperty("U_esPPE")]
        public string? esPPE { get; set; }

        [JsonProperty("U_recPubl")]
        public string? recPubl { get; set; }

        [JsonProperty("U_vincPPE")]
        public string? vincPPE { get; set; }

        [JsonProperty("BPAddresses")]
        public  List<BPAddresses>? oBPAddresses = new List<BPAddresses>();

        [JsonProperty("ContactEmployees")]
        public List<ContactEmployees>? ContactEmployees = new List<ContactEmployees>();

        [JsonProperty("CardType")]
        public string? cardType { get; set; }
    }

    public class BPAddresses
    {
        [JsonProperty("AddressName")]
        public string? addressName { get; set; }

        [JsonProperty("Street")]
        public string? street { get; set; }

        [JsonProperty("City")]
        public string? city { get; set; }

        [JsonProperty("Country")]
        public string? country { get; set; }

        [JsonProperty("AddressType")]
        public string? AddressType { get; set; }

        [JsonProperty("BPCode")]
        public string? BPCode { get; set; }

        [JsonProperty("RowNum")]
        public long? RowNum { get; set; }
    }

    public class ContactEmployees
    {

        [JsonProperty("Name")]
        public string? idContact { get; set; }

        [JsonProperty("FirstName")]
        public string? namePerson { get; set; }

        [JsonProperty("LastName")]
        public string? lastnamePerson { get; set; }

        [JsonProperty("E_Mail")]
        public string? emailPerson { get; set; }

        [JsonProperty("Phone1")]
        public string? phonePerson { get; set; }

        [JsonProperty("InternalCode")]
        public int? internalCode { get; set; }

        [JsonProperty("Remarks1")]
        public string? relationshipPerson { get; set; }
    }


}
