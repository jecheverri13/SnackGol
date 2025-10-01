using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryEntities.EntitiesSL
{
    public class Contract
    {
        [JsonProperty("Series")]
        public int? series { get; set; }

        [JsonProperty("DocEntry")]
        public int? docEntry { get; set; }

        [JsonProperty("U_cenCosto")]
        public string? costCenter { get; set; }

        [JsonProperty("U_cenCosto1")]
        public string? costCenter1 { get; set; }

        [JsonProperty("U_cenCosto2")]
        public string? costCenter2 { get; set; }

        [JsonProperty("U_cenCosto3")]
        public string? costCenter3 { get; set; }

        [JsonProperty("U_porGstAd")]
        public string? administrativeExpense { get; set; }

        [JsonProperty("U_porDescA")]
        public string? administrativeDiscount { get; set; }

        [JsonProperty("U_clscon")]
        public string? classAffiliations { get; set; }

        [JsonProperty("U_contrant")]
        public string? contractor { get; set; }

        [JsonProperty("U_ncontrat")]
        public string? nameOfContractor { get; set; }

        [JsonProperty("U_conve")]
        public string? agreement { get; set; }

        [JsonProperty("U_nomcon")]
        public string? nameOfTheAgreement { get; set; }

        [JsonProperty("U_diaCorte")]
        public int cutOffDay { get; set; }

        [JsonProperty("U_estado")]
        public string? state { get; set; }

        [JsonProperty("U_fecFi")]
        public string? effectiveEndDate { get; set; }

        [JsonProperty("U_fecIn")]
        public string? effectiveDate { get; set; }

        [JsonProperty("U_formPag")]
        public string? formOfPayment { get; set; }

        [JsonProperty("U_grupo")]
        public string? group { get; set; }

        [JsonProperty("U_nomgru")]
        public string? groupName { get; set; }

        [JsonProperty("U_plan")]
        public string? plan { get; set; }

        [JsonProperty("U_nompla")]
        public string? planName { get; set; }

        [JsonProperty("U_suen")]
        public string? subUnit { get; set; }

        [JsonProperty("U_nomsuen")]
        public string? nameSubUnit { get; set; }

        [JsonProperty("U_uen")]
        public string? uen { get; set; }

        [JsonProperty("U_nomuen")]
        public string? nameUen { get; set; }

        [JsonProperty("U_vendedor")]
        public string? seller { get; set; }

        [JsonProperty("U_nomVnd")]
        public string? sellerName { get; set; }

        [JsonProperty("U_numContMovil")]
        public string? referenceMobile { get; set; }

        [JsonProperty("U_perFact")]
        public string? invoicingPeriod { get; set; }

        [JsonProperty("U_perpag")]
        public string? paymentPeriodicity { get; set; }

        [JsonProperty("U_nCuotas")]
        public string? numberOfQuotas { get; set; }

        [JsonProperty("U_nCuotasP")]
        public string? numberOfQuotasPayable { get; set; }

        [JsonProperty("U_plGracia")]
        public int periodOfGrace { get; set; }

        [JsonProperty("U_vigencia")]
        public string? termOfTheContract { get; set; }

        [JsonProperty("U_respon")]
        public string? invoicingManager { get; set; }

        [JsonProperty("U_tipliq")]
        public string? typeOfLiquidation { get; set; }

        [JsonProperty("U_sucur")]
        public string? branch { get; set; }

        [JsonProperty("U_sucurEmp")]
        public string? companyBranch { get; set; }

        [JsonProperty("U_R_RelPre")]
        public string? relationshipWithPredio { get; set; }

        [JsonProperty("U_R_Dir")]
        public string? address { get; set; }

        [JsonProperty("U_R_Barrio")]
        public string? neighborhood { get; set; }

        [JsonProperty("U_tipCont")]
        public string? typeOfContract { get; set; }

        [JsonProperty("U_nSemanas")]
        public string? weeks { get; set; }

        [JsonProperty("U_Origen")]
        public string? originOfCreation { get; set; }

        [JsonProperty("U_valor")]
        public double value { get; set; }

        [JsonProperty("U_R_Cuenta")]
        public string? accountNumber { get; set; }

        [JsonProperty("U_R_CuentaEssa")]
        public string? essaAccountNumber { get; set; }

        [JsonProperty("U_R_Ciclo")]
        public string? cycle { get; set; }

        [JsonProperty("U_R_EmpresaWeb")]
        public string? empresaWeb { get; set; }

        [JsonProperty("U_observ")]
        public string? remarks { get; set; }

        [JsonProperty("U_CreateDateWeb")]
        public string? dateOfWebCreation { get; set; }

        [JsonProperty("U_empNiv")]
        public string? companyLevels { get; set; }

        [JsonProperty("U_recaudo")]
        public string? meansOfCollection { get; set; }

        [JsonProperty("OK1_EXE_CONT_BENEFICollection")]
        public List<Ok1ExeContBenefic>? ok1ExeContBeneficCollection { get; set; }

        [JsonProperty("OK1_EXE_CONT_SEGUROCollection")]
        public List<Ok1ExeContSeguro>? ok1ExeContSeguroCollection { get; set; }

        [JsonProperty("OK1_EXE_COMEN_CONTRCollection")]
        public List<Ok1ExeComenContr>? ok1ExeComenContrCollection { get; set; }

        [JsonProperty("U_zonaCom")]
        public string? commercialZone { get; set; }

        [JsonProperty("U_desZonaCom")]
        public string? commercialZoneDesc { get; set; }

        public Contract()
        {
            ok1ExeContBeneficCollection = new List<Ok1ExeContBenefic>();
            ok1ExeContSeguroCollection = new List<Ok1ExeContSeguro>();
            ok1ExeComenContrCollection = new List<Ok1ExeComenContr>();
        }
    }

    public class ContractGet
    {
        [JsonProperty("Series")]
        public int? series { get; set; }

        [JsonProperty("DocEntry")]
        public int? docEntry { get; set; }

        [JsonProperty("U_cenCosto")]
        public string? costCenter { get; set; }

        [JsonProperty("U_cenCosto1")]
        public string? costCenter1 { get; set; }

        [JsonProperty("U_cenCosto2")]
        public string? costCenter2 { get; set; }

        [JsonProperty("U_cenCosto3")]
        public string? costCenter3 { get; set; }

        [JsonProperty("U_porGstAd")]
        public string? administrativeExpense { get; set; }

        [JsonProperty("U_porDescA")]
        public string? administrativeDiscount { get; set; }

        [JsonProperty("U_clscon")]
        public string? classAffiliations { get; set; }

        [JsonProperty("U_contrant")]
        public string? contractor { get; set; }

        [JsonProperty("U_ncontrat")]
        public string? nameOfContractor { get; set; }

        [JsonProperty("U_conve")]
        public string? agreement { get; set; }

        [JsonProperty("U_nomcon")]
        public string? nameOfTheAgreement { get; set; }

        [JsonProperty("U_diaCorte")]
        public int cutOffDay { get; set; }

        [JsonProperty("U_estado")]
        public string? state { get; set; }

        [JsonProperty("U_fecFi")]
        public string? effectiveEndDate { get; set; }

        [JsonProperty("U_fecIn")]
        public string? effectiveDate { get; set; }

        [JsonProperty("U_formPag")]
        public string? formOfPayment { get; set; }

        [JsonProperty("U_grupo")]
        public string? group { get; set; }

        [JsonProperty("U_nomgru")]
        public string? groupName { get; set; }

        [JsonProperty("U_plan")]
        public string? plan { get; set; }

        [JsonProperty("U_nompla")]
        public string? planName { get; set; }

        [JsonProperty("U_suen")]
        public string? subUnit { get; set; }

        [JsonProperty("U_nomsuen")]
        public string? nameSubUnit { get; set; }

        [JsonProperty("U_uen")]
        public string? uen { get; set; }

        [JsonProperty("U_nomuen")]
        public string? nameUen { get; set; }

        [JsonProperty("U_vendedor")]
        public string? seller { get; set; }

        [JsonProperty("U_nomVnd")]
        public string? sellerName { get; set; }

        [JsonProperty("U_numContMovil")]
        public string? referenceMobile { get; set; }

        [JsonProperty("U_perFact")]
        public string? invoicingPeriod { get; set; }

        [JsonProperty("U_perpag")]
        public string? paymentPeriodicity { get; set; }

        [JsonProperty("U_nCuotas")]
        public string? numberOfQuotas { get; set; }

        [JsonProperty("U_nCuotasP")]
        public string? numberOfQuotasPayable { get; set; }

        [JsonProperty("U_plGracia")]
        public int periodOfGrace { get; set; }

        [JsonProperty("U_vigencia")]
        public string? termOfTheContract { get; set; }

        [JsonProperty("U_respon")]
        public string? invoicingManager { get; set; }

        [JsonProperty("U_tipliq")]
        public string? typeOfLiquidation { get; set; }

        [JsonProperty("U_sucur")]
        public string? branch { get; set; }

        [JsonProperty("U_sucurEmp")]
        public string? companyBranch { get; set; }

        [JsonProperty("U_R_RelPre")]
        public string? relationshipWithPredio { get; set; }

        [JsonProperty("U_R_Dir")]
        public string? address { get; set; }

        [JsonProperty("U_R_Barrio")]
        public string? neighborhood { get; set; }

        [JsonProperty("U_tipCont")]
        public string? typeOfContract { get; set; }

        [JsonProperty("U_nSemanas")]
        public string? weeks { get; set; }

        [JsonProperty("U_Origen")]
        public string? originOfCreation { get; set; }

        [JsonProperty("U_valor")]
        public double value { get; set; }

        [JsonProperty("U_R_Cuenta")]
        public string? accountNumber { get; set; }

        [JsonProperty("U_R_CuentaEssa")]
        public string? essaAccountNumber { get; set; }

        [JsonProperty("U_R_Ciclo")]
        public string? cycle { get; set; }

        [JsonProperty("U_observ")]
        public string? remarks { get; set; }

        [JsonProperty("U_CreateDateWeb")]
        public string? dateOfWebCreation { get; set; }

        [JsonProperty("U_empNiv")]
        public string? companyLevels { get; set; }

        [JsonProperty("U_recaudo")]
        public string? meansOfCollection { get; set; }
        public double? quota { get; set; }

        [JsonProperty("OK1_EXE_CONT_BENEFICollection")]
        public List<Ok1ExeContBenefic>? ok1ExeContBeneficCollection { get; set; }

        [JsonProperty("OK1_EXE_CONT_SEGUROCollection")]
        public List<Ok1ExeContSeguro>? ok1ExeContSeguroCollection { get; set; }

        [JsonProperty("OK1_EXE_COMEN_CONTRCollection")]
        public List<Ok1ExeComenContr>? ok1ExeComenContrCollection { get; set; }

        [JsonProperty("U_modCuotas")]
        public string? modifyQuotas { get; set; }

        [JsonProperty("U_perPagoContr")]
        public string? modifyPeridocity { get; set; }

        public ContractGet()
        {
            ok1ExeContBeneficCollection = new List<Ok1ExeContBenefic>();
            ok1ExeContSeguroCollection = new List<Ok1ExeContSeguro>();
            ok1ExeComenContrCollection = new List<Ok1ExeComenContr>();
        }
    }

    public class Ok1ExeComenContr
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

    public class Ok1ExeContBenefic
    {
        [JsonProperty("LineId")]
        public int? LineId { get; set; }

        [JsonProperty("U_aplCaren")]
        public string? applyDeficiency { get; set; } = "Y";

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
        public string? ageRelationship { get; set; }

        [JsonProperty("U_exceEdad")]
        public string? ageExceptionApplies { get; set; }

        [JsonProperty("U_aplVlrExtra")]
        public string? extraValueApplies { get; set; }

        [JsonProperty("U_fecRet")]
        public string? retirementDate { get; set; }
    }

    public class Ok1ExeContSeguro
    {
        [JsonProperty("LineId")]
        public int? LineId { get; set; }

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

    public class PaymentsDueGet
    {
        [JsonProperty("PaymentsMade")]
        public double? paymentsMade { get; set; }

        [JsonProperty("PaymentsDue")]
        public double? paymentsDue { get; set; }

        [JsonProperty("Total")]
        public double? total { get; set; }

        [JsonProperty("InvoicedBalance")]
        public double? invoicedBalance { get; set; }

        [JsonProperty("BalanceDue")]
        public double? balanceDue { get; set; }

        [JsonProperty("value")]
        public double? value { get; set; } // Campo que no se mostrará en el resultado final

        [JsonProperty("payments")]
        public List<PaymentsDueLines>? payments { get; set; }

        [JsonProperty("invoces")]
        public List<PaymentsDuePendingInvoice>? invoices { get; set; }
    }

    public class PaymentsDueLines
    {
        [JsonProperty("QuotaNumber")]
        public int? quotaNumber { get; set; }

        [JsonProperty("InitDate")]
        public string? initDate { get; set; }

        [JsonProperty("EndDate")]
        public string? endDate { get; set; }

        [JsonProperty("value")]
        public double? value { get; set; }
    }

    public class PaymentsDuePendingInvoice
    {
        [JsonProperty("TransId")]
        public string? TransId { get; set; }

        [JsonProperty("nFactExe")]
        public string? nFactExe { get; set; }

        [JsonProperty("Fecha")]
        public string? Fecha { get; set; }

        [JsonProperty("Total")]
        public double? Total { get; set; }

        [JsonProperty("Saldo")]
        public double? Saldo { get; set; }

        [JsonProperty("Transaccion")]
        public string? Transaccion { get; set; }

        [JsonProperty("Fecha_Inicial")]
        public string? Fecha_Inicial { get; set; }

        [JsonProperty("Fecha_Final")]
        public string? Fecha_Final { get; set; }

        [JsonProperty("Usuario")]
        public string? Usuario { get; set; }

        [JsonProperty("Pagos_Aplicados")]
        public string? Pagos_Aplicados { get; set; }

        [JsonProperty("Permitir_Consulta_Efecty")]
        public string? Permitir_Consulta_Efecty { get; set; }
    }
}
