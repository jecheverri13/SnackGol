using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class Affiliation
    {
        [JsonProperty("BusinessPartners")]
        public Holder? holder { get; set; }
         
        public string effectiveDate { get; set; }

        public string effectiveEndDate { get; set; }

        public string state { get; set; }

        public string customerEmail { get; set; }

        public int? affiliation { get; set; }

        public int idUser { get; set; }

        public double value { get; set; }

        public string quotas { get; set; }

        public string periodicity { get; set; }

        [JsonProperty("OK1_EXE_CONT_SEGUROCollection")]
        public List<Insurances>? insurances { get; set; }

        [JsonProperty("OK1_EXE_CONT_BENEFICollection")]
        public List<Beneficiaries>? assistances { get; set; }

        [JsonProperty("OK1_EXE_COMEN_CONTRCollection")]
        public List<Bitacora>? bitacora { get; set; }

        public Affiliation()
        {
            assistances = new List<Beneficiaries>();
            insurances = new List<Insurances>();
            bitacora = new List<Bitacora>();
        }
    }

    public class Holder
    {
        [JsonProperty("CardCode")]
        public string? document { get; set; }

        [JsonProperty("U_addInFaElectronica_email_contacto_FE")]
        public string? emailFE { get; set; }

        [JsonProperty("EmailAddress")]
        public string? emailAddress { get; set; }

        [JsonProperty("U_BPCO_CS")]
        public string? municipality { get; set; }

        [JsonProperty("U_Depto")]
        public string? department { get; set; }

        [JsonProperty("U_BPCO_Address")]
        public string? address { get; set; }

        [JsonProperty("Phone1")]
        public string? phone1 { get; set; }

        [JsonProperty("Cellular")]
        public string? cellular { get; set; }

        [JsonProperty("U_frontOfDocument")]
        public string? frontOfDocument { get; set; }

        [JsonProperty("U_laterDocument")]
        public string? backOfDocument { get; set; }

        [JsonProperty("U_dir1")]
        public string? U_dir1 { get; set; }

        [JsonProperty("U_dir2")]
        public string? U_dir2 { get; set; }

        [JsonProperty("U_dir3")]
        public string? U_dir3 { get; set; }

        [JsonProperty("U_dir4")]
        public string? U_dir4 { get; set; }

        [JsonProperty("U_dir5")]
        public string? U_dir5 { get; set; }

        [JsonProperty("BPAddresses")]
        public List<BPAddressesHolder>? BPAddresses { get; set; }

        [JsonProperty("ContactEmployees")]
        public List<ContactEmployeesHolder>? contactPersons { get; set; }
    }

    public class BPAddressesHolder
    {
        [JsonProperty("AddressName")]
        public string? AddressName { get; set; }

        [JsonProperty("Street")]
        public string? Street { get; set; }

        [JsonProperty("City")]
        public string? City { get; set; }

        [JsonProperty("State")]
        public string? State { get; set; }

        [JsonProperty("Country")]
        public string? Country { get; set; }

        [JsonProperty("AddressType")]
        public string? AddressType { get; set; }

        [JsonProperty("BPCode")]
        public string? BPCode { get; set; }

        [JsonProperty("RowNum")]
        public long? RowNum { get; set; }
    }


    public class ContactEmployeesHolder
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
        [JsonIgnore]
        public int? internalID { get; set; }
        [JsonIgnore]
        public string? code { get; set; }
    }
    public class Insurances
    {
        [JsonProperty("LineId")]
        public int? lineId { get; set; }

        [JsonProperty("U_nomSeg")]
        public string nameInsurance { get; set; }

        [JsonProperty("U_tipoSeg")]
        public string typeInsurance { get; set; }

        [JsonProperty("U_ocup")]
        public string? occupation { get; set; }

        [JsonProperty("U_observ")]
        public string? observations { get; set; }

        [JsonProperty("U_codSeg")]
        public string? codeInsurance { get; set; }

        [JsonProperty("U_fecIn")]
        public string? startDate { get; set; }

        [JsonProperty("U_fecFin")]
        public string? endDate { get; set; }

        [JsonProperty("U_fecIng")]
        public string? admissionDate { get; set; }

        [JsonProperty("U_fecNac")]
        public string? birthDate { get; set; }

        [JsonProperty("U_numDoc")]
        public string? identification { get; set; }

        [JsonProperty("U_parent")]
        public string? relationship { get; set; }

        [JsonProperty("U_papll")]
        public string? lastName { get; set; }

        [JsonProperty("U_pnom")]
        public string? firstName { get; set; }

        [JsonProperty("U_porcBen")]
        public string? percentage { get; set; }

        [JsonProperty("U_sexo")]
        public string? gender { get; set; }

        [JsonProperty("U_snom")]
        public string? secondName { get; set; }

        [JsonProperty("U_tddSeg")]
        public string? typeIdentification { get; set; } //Tipo de documento

        [JsonProperty("U_valor")]
        public double? value { get; set; }

        [JsonProperty("U_sapll")]
        public string? secondLastName { get; set; }

        [JsonProperty("U_fecRe")]
        public string? retirementDate { get; set; }
    }

    public class Beneficiaries
    {
        [JsonProperty("LineId")]
        public int? lineId { get; set; }

        [JsonProperty("U_aplCaren")]
        public string? applyDeficiency { get; set; }

        [JsonProperty("U_edad")]
        public string? age { get; set; }

        [JsonProperty("U_fecIng")]
        public string? dateOfEntryOrContract { get; set; }

        [JsonProperty("U_fecnaci")]
        public string? birthDate { get; set; }

        [JsonProperty("U_fecIngDB")]
        public string? dateCreatedOrModified { get; set; }

        [JsonProperty("U_fecTrs")]
        public string? transferDate { get; set; }

        [JsonProperty("U_nombre")]
        public string? firstName { get; set; }

        [JsonProperty("U_numdoc")]
        public string? identification { get; set; }

        [JsonProperty("U_pape")]
        public string? lastName { get; set; }

        [JsonProperty("U_parent")]
        public string? relationship { get; set; }

        [JsonProperty("U_sape")]
        public string? secondLastName { get; set; }

        [JsonProperty("U_sexo")]
        public string? gender { get; set; }

        [JsonProperty("U_snombre")]
        public string? secondName { get; set; }

        [JsonProperty("U_obser")]
        public string? observations { get; set; }

        [JsonProperty("U_tdbenef")]
        public string? group { get; set; } // Tipo de beneficiario

        [JsonProperty("U_tdd")]
        public string? typeIdentification { get; set; } //Tipo de documento

        [JsonProperty("U_valor")]
        public double? value { get; set; }

        [JsonProperty("U_codPlaAsis")]
        public string? assistancePlan { get; set; }

        [JsonProperty("U_parentCoEd")]
        public string? ageControlledRelationship { get; set; }

        [JsonProperty("U_exceEdad")]
        public string? ageExceptionApplies { get; set; }

        [JsonProperty("U_aplVlrExtra")]
        public string? extraValueApplies { get; set; }

        [JsonProperty("U_fecRet")]
        public string? retirementDate { get; set; }

        public string? retirementReason { get; set; }
    }

    public class Bitacora
    {
        [JsonProperty("U_coment")]
        public string? uComent { get; set; }

        [JsonProperty("U_fecha")]
        public string? uFecha { get; set; }

        [JsonProperty("U_hora")]
        public string? uHora { get; set; }

        [JsonProperty("U_usuario")]
        public string? uUsuario { get; set; }

        [JsonProperty("U_estActual")]
        public string? uEstActual { get; set; }

        [JsonProperty("U_estNovedad")]
        public string? uEstNovedad { get; set; }

        [JsonProperty("U_fecNov")]
        public string? uFecNov { get; set; }

        [JsonProperty("U_fecIniVig")]
        public string? uFecIniVig { get; set; }

        [JsonProperty("U_fecFinVig")]
        public string? uFecFinVig { get; set; }
    }

}
