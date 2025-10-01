using LibraryEntities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class OrderFuneralService2
    {

        [JsonProperty("Series")]
        public string? series { get; set; }

        [JsonProperty("DocNum")]
        public string? docNum { get; set; }

        [JsonProperty("U_VSede")]
        public string? branch { get; set; }

        [JsonProperty("U_VSedeNom")]
        public string? branchName { get; set; }

        [JsonProperty("U_tipSolicitud")]
        public string? orderType { get; set; }

        [JsonProperty("U_SolAnte")]
        public string? orderPrevious { get; set; }

        [JsonProperty("U_TipoSrv")]
        public string? serviceType { get; set; }

        [JsonProperty("U_conve")]
        public string? agreement { get; set; }

        [JsonProperty("U_Empresa")]
        public string? agreementName { get; set; }

        [JsonProperty("U_TipPrest")]
        public string? typeBenefit { get; set; }

        [JsonProperty("U_NivServ")]
        public string? serviceLevel { get; set; }

        [JsonProperty("U_TipoCubr")]
        public string? typeCoverage { get; set; }

        [JsonProperty("U_FechaVencCotz")]
        public string? expirationDate { get; set; }

        [JsonProperty("U_Usuario")]
        public string? user { get; set; }

        [JsonProperty("U_FechaSol")]
        public string? orderDate { get; set; }

        [JsonProperty("U_HoraSoli")]
        public string? orderTime { get; set; }

        [JsonProperty("U_Estado")]
        public string? orderState { get; set; }

        [JsonProperty("U_IdContra")]
        public string? identification { get; set; }

        [JsonProperty("U_NombreCo")]
        public string? contractingName { get; set; }

        [JsonProperty("U_TelContr")]
        public string? contractingPhone { get; set; }

        [JsonProperty("U_DirContr")]
        public string? address { get; set; }

        [JsonProperty("U_Ciudad")]
        public string? city { get; set; }


        [JsonProperty("U_Email")]
        public string? email { get; set; }

        [JsonProperty("U_Celular")]
        public string? phone { get; set; }


        [JsonProperty("U_EntConCl")]
        public string? clientAgreement { get; set; }

        [JsonProperty("U_NumContr")]
        public string contract { get; set; }

        [JsonProperty("U_Vigencia")]
        public string? validity { get; set; }

        [JsonProperty("U_CodPlan")]
        public string? plan { get; set; }

        [JsonProperty("U_Plan")]
        public string? planName { get; set; }

        [JsonProperty("U_codPlanEquiv")]
        public string? equivalentPlan { get; set; }

        [JsonProperty("U_convPart")]
        public string? particularAgreement { get; set; }


        [JsonProperty("U_NomEncSr")]
        public string? employee { get; set; }

        [JsonProperty("U_TelEncSr")]
        public string? serviceManagerPhone { get; set; }


        [JsonProperty("U_IdemIDCont")]
        public string? applicantEqualContracting { get; set; }

        [JsonProperty("U_SolIdent")]
        public string? applicantId { get; set; }

        [JsonProperty("U_SolNomb")]
        public string? applicantName { get; set; }

        [JsonProperty("U_SolTelef")]
        public string? applicantPhone { get; set; }

        [JsonProperty("U_DirSolic")]
        public string? applicantDirection { get; set; }

        [JsonProperty("U_EmailSolic")]
        public string? applicantEmail { get; set; }


        [JsonProperty("U_tipListCh")]
        public string? typeCheckList { get; set; }

        [JsonProperty("U_TipoBene")]
        public string? beneficiaryType { get; set; }

        [JsonProperty("U_TipDoc")]
        public string? typeIdentification { get; set; }

        [JsonProperty("U_nTipDoc")]
        public string? documentNameType { get; set; }

        [JsonProperty("U_IdFallec")]
        public string? deceasedId { get; set; }

        [JsonProperty("U_NombreFa")]
        public string? name { get; set; }

        [JsonProperty("U_FechaNac")]
        public string? birthDate { get; set; }

        [JsonProperty("U_Edad")]
        public string? age { get; set; }

        [JsonProperty("U_Parentes")]
        public string? relationship { get; set; }

        [JsonProperty("U_nParentes")]
        public string? relationshipName { get; set; }

        [JsonProperty("U_Genero")]
        public string? gender { get; set; }

        [JsonProperty("U_FechaFal")]
        public string? date { get; set; }

        [JsonProperty("U_BenNoReg")]
        public string? unregisteredBeneficiary { get; set; }

        [JsonProperty("U_HoraFall")]
        public string? hour { get; set; }

        [JsonProperty("U_CausaFal")]
        public string? cause { get; set; }

        [JsonProperty("U_nCausaFal")]
        public string? nameCauseOfDeath { get; set; }

        [JsonProperty("U_LugDefunc")]
        public string? place { get; set; }

        [JsonProperty("U_UbicLoc")]
        public string? location { get; set; }

        [JsonProperty("U_nUbicLoc")]
        public string? nameLocationOfDeath { get; set; }

        [JsonProperty("U_paisFa")]
        public string? codeCountryOfDeath { get; set; }

        [JsonProperty("U_npaisFa")]
        public string? nameCountryOfDeath { get; set; }

        [JsonProperty("U_DptoFa")]
        public string? department { get; set; }

        [JsonProperty("U_nDptoFa")]
        public string? nameDepartamentOfDeath { get; set; }

        [JsonProperty("U_CiudadFa")]
        public string? cityOfDeath { get; set; }

        [JsonProperty("U_nCiudadFa")]
        public string? nameCityOfDeath { get; set; }

        [JsonProperty("U_Ubicacio")]
        public string? addressLocation { get; set; }

        [JsonProperty("U_sedBarr")]
        public string? districtLocation { get; set; }

        [JsonProperty("U_Telf")]
        public string? phoneLocation { get; set; }

        [JsonProperty("U_Observac")]
        public string? comments { get; set; }

        [JsonProperty("U_cenCosto")]
        public string? costCenter { get; set; }

        [JsonProperty("U_cenCosto1")]
        public string? costCenter1 { get; set; }

        [JsonProperty("U_cenCosto2")]
        public string? costCenter2 { get; set; }

        [JsonProperty("U_cenCosto3")]
        public string? costCenter3 { get; set; }

        [JsonProperty("U_sedeVel")]
        public string? codeWakeLocation { get; set; }

        [JsonProperty("U_sedeVelNom")]
        public string? nameWakeLocation { get; set; }

        [JsonProperty("U_VLugar")]
        public string? codeWakeRoom { get; set; }

        [JsonProperty("U_nVLugar")]
        public string? nameWakeRoom { get; set; }

        [JsonProperty("U_deptVel")]
        public string? codeWakeDepartament { get; set; }

        [JsonProperty("U_ndeptVel")]
        public string? nameWakeDepartament { get; set; }

        [JsonProperty("U_munVel")]
        public string? codeWakeCity { get; set; }

        [JsonProperty("U_nmunVel")]
        public string? nameWakeCity { get; set; }

        [JsonProperty("U_dirVel")]
        public string? wakeDirection { get; set; }

        [JsonProperty("U_VFecha")]
        public string? wakeEntryDate { get; set; }

        [JsonProperty("U_horVelEntr")]
        public string? wakeEntryHour { get; set; }

        [JsonProperty("U_VFechaS")]
        public string? wakeCheckoutDate { get; set; }

        [JsonProperty("U_horVelSal")]
        public string? wakeCheckoutHour { get; set; }

        [JsonProperty("U_VSerVir")]
        public string? virtualWake { get; set; }

        [JsonProperty("U_VSalaExc")]
        public string? codeWakeRoomExcess { get; set; }

        [JsonProperty("U_nVSalaExc")]
        public string? nameWakeRoomExcess { get; set; }

        [JsonProperty("U_VFechaExc")]
        public string? wakeEntryDateExcess { get; set; }

        [JsonProperty("U_horVelEntExc")]
        public string? wakeEntryHourExcess { get; set; }

        [JsonProperty("U_VFechaSalExc")]
        public string? wakeCheckoutDateExcess { get; set; }

        [JsonProperty("U_horVelSalExc")]
        public string? wakeCheckoutHourExcess { get; set; }

        [JsonProperty("U_VSalaR")]
        public string? codeWakeRoomMeeting { get; set; }

        [JsonProperty("U_nVSalaR")]
        public string? nameWakeRoomMeeting { get; set; }

        [JsonProperty("U_VFechaR")]
        public string? wakeEntryDateMeeting { get; set; }

        [JsonProperty("U_horVelEntRe")]
        public string? wakeEntryHourMeeting { get; set; }

        [JsonProperty("U_VFechaSalR")]
        public string? wakeCheckoutDateMeeting { get; set; }

        [JsonProperty("U_horVelSalRe")]
        public string? wakeCheckoutHourMeeting { get; set; }

        [JsonProperty("U_VlabPltBaj")]
        public string? codeWakeRoomGroundFloor { get; set; }

        [JsonProperty("U_nVlabPltBaj")]
        public string? nameWakeRoomGroundFloor { get; set; }

        [JsonProperty("U_VFechaPltBaj")]
        public string? wakeEntryDateGroundFloor { get; set; }

        [JsonProperty("U_horVelEntPltBaj")]
        public string? wakeEntryHourGroundFloor { get; set; }

        [JsonProperty("U_VFechaSalPltBaj")]
        public string? wakeCheckoutDateGroundFloor { get; set; }

        [JsonProperty("U_horVelSalPltBaj")]
        public string? wakeCheckoutHourGroundFloor { get; set; }

        [JsonProperty("U_tipoMisa")]
        public string? typeMass { get; set; }

        [JsonProperty("U_sedeMisa")]
        public string? codeMassVenue { get; set; }

        [JsonProperty("U_sedeExeNom")]
        public string? nameMassVenue { get; set; }

        [JsonProperty("U_ELugar")]
        public string? codeChurch { get; set; }

        [JsonProperty("U_nELugar")]
        public string? nameChurch { get; set; }

        [JsonProperty("U_EDpto")]
        public string? codeMassDepartament { get; set; }

        [JsonProperty("U_nEDpto")]
        public string? nameMassDepartament { get; set; }

        [JsonProperty("U_EMuni")]
        public string? codeMassCity { get; set; }

        [JsonProperty("U_nEMuni")]
        public string? nameMassCity { get; set; }

        [JsonProperty("U_EFecha")]
        public string? MassDate { get; set; }

        [JsonProperty("U_EHora")]
        public string? MassStartHour { get; set; }

        [JsonProperty("U_horFiMisa")]
        public string? MassEndHour { get; set; }

        [JsonProperty("U_dirMisa")]
        public string? ChurchDirection { get; set; }

        [JsonProperty("U_OfiReli")]
        public string? religiousCeremony { get; set; }

        [JsonProperty("U_tipoSacerd")]
        public string? typePriest { get; set; }

        [JsonProperty("U_sacerdMisa")]
        public string? codePriest { get; set; }

        [JsonProperty("U_nomSacerMisa")]
        public string? namePriest { get; set; }

        [JsonProperty("U_coroMisa")]
        public string? choir { get; set; }

        [JsonProperty("U_catafMisa")]
        public string? catafalque { get; set; }

        [JsonProperty("U_Homilia")]
        public string? homily { get; set; }

        [JsonProperty("U_misaConme")]
        public string? commemorativeMass { get; set; }

        [JsonProperty("U_FechaTentMisaConm")]
        public string? commemorativeMassDate { get; set; }

        [JsonProperty("U_IUbic")]
        public string? codeCementery { get; set; }

        [JsonProperty("U_nIUbic")]
        public string? nameCementery { get; set; }

        [JsonProperty("U_IDpto")]
        public string? codeCementeryDepartament { get; set; }

        [JsonProperty("U_nIDpto")]
        public string? nameCementeryDepartament { get; set; }

        [JsonProperty("U_IMuni")]
        public string? codeCementeryCity { get; set; }

        [JsonProperty("U_nIMuni")]
        public string? nameCementeryCity { get; set; }

        [JsonProperty("U_ILugar")]
        public string? codeTypeCementery { get; set; }

        [JsonProperty("U_nILugar")]
        public string? nameTypeCementery { get; set; }

        [JsonProperty("U_IFecha")]
        public string? burialDate { get; set; }

        [JsonProperty("U_IHora")]
        public string? burialHour { get; set; }

        [JsonProperty("U_IDir")]
        public string? cementeryDirection { get; set; }

        [JsonProperty("U_SFecha")]
        public string? exhumationlDate { get; set; }

        [JsonProperty("U_SHora")]
        public string? exhumationHour { get; set; }

        [JsonProperty("OK1_SF_ELEM_LINCollection")]
        public List<OK1_SF_ELEM_LIN> elements { get; set; }

        [JsonProperty("OK1_SF_LOGALELLAMADCollection")]
        public List<Historical> history { get; set; }

        [JsonProperty("OK1_SF_CUSTDOCCollection")]
        public List<Documents> documents { get; set; }

    }

    public class OrderFuneralService
    {
        [JsonProperty("general")]
        public General General { get; set; }

        [JsonProperty("deceased")]
        public Deceased Deceased { get; set; }

        [JsonProperty("contractor")]
        public Contractor Contractor { get; set; }

        [JsonProperty("requester")]
        public Requester Requester { get; set; }

        [JsonProperty("OK1_SF_ELEM_LINCollection")]
        public required List<OK1_SF_ELEM_LIN> elements { get; set; }
    }

    public class PatchOrderFuneralService
    {
        [JsonProperty("general")]
        public General General { get; set; }

        [JsonProperty("deceased")]
        public Deceased Deceased { get; set; }

        [JsonProperty("contractor")]
        public Contractor Contractor { get; set; }

        [JsonProperty("requester")]
        public Requester Requester { get; set; }

        [JsonProperty("OK1_SF_ELEM_LINCollection")]
        public required List<Elements> elements { get; set; }
    }

    public class PatchElementsOFS
    {
        public required List<Elements> elements { get; set; }

        public required string comments { get; set; }

        public required string user { get; set; }
    }

    public class General
    {

        [JsonProperty("documentType")]
        public string documentType { get; set; }

        [JsonProperty("branch")]
        public string branch { get; set; }

        public string series { get; set; } = "";

        [JsonProperty("docNum")]
        public string docNum { get; set; } = "";

        [JsonProperty("orderType")]
        public string orderType { get; set; }

        [JsonProperty("serviceType")]
        public string serviceType { get; set; }

        [JsonProperty("contract")]
        public string contract { get; set; }

        [JsonProperty("orderPrevious")]
        public string orderPrevious { get; set; } = "";

        [JsonProperty("user")]
        public string user { get; set; }

        [JsonProperty("agreement")]
        public string? agreement { get; set; }

        [JsonProperty("employee")]
        public string employee { get; set; }

        [JsonProperty("typeBenefit")]
        public string typeBenefit { get; set; }

        [JsonProperty("typeCoverage")]
        public string typeCoverage { get; set; }

        [JsonProperty("plan")]
        public string? plan { get; set; }

        [JsonProperty("typeCheckList")]
        public string typeCheckList { get; set; }

        [JsonProperty("equivalentPlan")]
        public string equivalentPlan { get; set; }

        [JsonProperty("orderDate")]
        public string orderDate { get; set; }

        [JsonProperty("orderTime")]
        public string orderTime { get; set; }

        [JsonProperty("serviceLevel")]
        public string serviceLevel { get; set; }

        [JsonProperty("comments")]
        public string comments { get; set; }

        [JsonProperty("comments")]
        public string? cc1 { get; set; }

        public string? cc2 { get; set; }

        public string? cc3 { get; set; }

        public string? cc4 { get; set; }

        public string? allowsFunerals { get; set; }

        [JsonProperty("expirationDate")]
        public string? expirationDate { get; set; }

    }

    public class Deceased
    {

        [JsonProperty("typeIdentification")]
        public string? typeIdentification { get; set; }

        [JsonProperty("identification")]
        public string? deceasedId { get; set; }

        [JsonProperty("name")]
        public string? name { get; set; }

        [JsonProperty("birthDate")]
        public string? birthDate { get; set; }

        [JsonProperty("age")]
        public string? age { get; set; }

        [JsonProperty("relationship")]
        public string? relationship { get; set; }

        [JsonProperty("date")]
        public string? date { get; set; }

        [JsonProperty("hour")]
        public string? hour { get; set; }

        [JsonProperty("cause")]
        public string? cause { get; set; }

        [JsonProperty("location")]
        public string? location { get; set; }

        [JsonProperty("place")]
        public string? place { get; set; }

        [JsonProperty("districtLocation")]
        public string? districtLocation { get; set; }

        [JsonProperty("phoneLocation")]
        public string? phoneLocation { get; set; }

        [JsonProperty("addressLocation")]
        public string? addressLocation { get; set; }

        [JsonProperty("department")]
        public string? department { get; set; }

        [JsonProperty("city")]
        public string? cityOfDeath { get; set; }

        [JsonProperty("gender")]
        public string? gender { get; set; }

        [JsonProperty("unregisteredBeneficiary")]
        public string? unregisteredBeneficiary { get; set; }

        [JsonProperty("beneficiaryType")]
        public string? beneficiaryType { get; set; }
    }

    public class Contractor
    {
        [JsonProperty("typeIdentification")]
        public string typeIdentification { get; set; }

        [JsonProperty("identification")]
        public string identification { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("firstLastName")]
        public string firstLastName { get; set; }

        [JsonProperty("secondLastName")]
        public string secondLastName { get; set; }

        [JsonProperty("birthdate")]
        public string birthdate { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("gender")]
        public string gender { get; set; }

        [JsonProperty("department")]
        public string department { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }

        [JsonProperty("ownerEqualsRequester")]
        public string ownerEqualsRequester { get; set; }
    }

    public class Elements
    {
        public int? id { get; set; }

        public string? elementCode { get; set; }

        public string? elementName { get; set; }

        public string? type { get; set; }

        public string? orderType { get; set; }

        public int? quantity { get; set; }

        public int? quantityCovered { get; set; }

        public int? avQuantity { get; set; }

        public string? itemType { get; set; }

        public string? lineItem { get; set; }

        public string? lineItemName { get; set; }

        public string? itemCode { get; set; }

        public string? itemName { get; set; }

        public string? serialItem { get; set; }

        public string? store { get; set; }

        public string? cardCodeSupplier { get; set; }

        public string? cardNameSupplier { get; set; }

        public double? purchaseValue { get; set; }

        public string? isBilled { get; set; }

        public double? saleValue { get; set; }

        public double? discountRate { get; set; }

        public double? discountValue { get; set; }

        public double? subtotalValue { get; set; }

        public double? vatValue { get; set; }

        public double? totalValue { get; set; }

        public string? cardCodeClient { get; set; }

        public string? cardNameClient { get; set; }

        public int? idLine { get; set; }

        public string? itemCodeFather { get; set; }

        public int? lineFather { get; set; }

        public string? itemCodeFatherInChildLine { get; set; }

        public string? itemCodeChild { get; set; }

        public string? userAdi { get; set; }

        public string? confirmCoffin { get; set; }

        public string? allowsModifyingItem { get; set; }

        public string? additionalCoveredStatus { get; set; }

        public string ? isRenounce { get; set; }

        public bool? delete { get; set; }
        public int? lineId { get; set; }

    }

    public class OK1_SF_ELEM_LIN
    {
        [JsonProperty("LineId")]
        public int? lineId { get; set; }

        [JsonProperty("U_Codigo")]
        public string? elementCode { get; set; }

        [JsonProperty("U_Nombre")]
        public string? elementName { get; set; }

        [JsonProperty("U_tipLinea")]
        public string? type { get; set; }

        [JsonProperty("U_tipOrden")]
        public string? orderType { get; set; }

        [JsonProperty("U_Cant")]
        public int? quantity { get; set; }

        [JsonProperty("U_CantCu")]
        public int? quantityCovered { get; set; }

        [JsonProperty("U_cantDisp")]
        public int? avQuantity { get; set; }

        [JsonProperty("U_ArtAdMe")]
        public string? itemType { get; set; }

        [JsonProperty("U_Linea")]
        public string? lineItem { get; set; }

        [JsonProperty("U_LineaName")]
        public string? lineItemName { get; set; }

        [JsonProperty("U_ItemCode")]
        public string? itemCode { get; set; }

        [JsonProperty("U_ItemName")]
        public string? itemName { get; set; }

        [JsonProperty("U_RotuloCofre")]
        public string? serialItem { get; set; }

        [JsonProperty("U_almacen")]
        public string? store { get; set; }

        [JsonProperty("U_CardCode")]
        public string? cardCodeSupplier { get; set; }

        [JsonProperty("U_CardName")]
        public string? cardNameSupplier { get; set; }

        [JsonProperty("U_ValorCom")]
        public double? purchaseValue { get; set; }

        [JsonProperty("U_Renuncia")]
        public string? isRenounce { get; set; }

        [JsonProperty("U_EsFactu")]
        public string? isBilled { get; set; }


        [JsonProperty("U_Valor")]
        public double? value { get; set; }

        [JsonProperty("U_porcDto")]
        public double? discountRate { get; set; }

        [JsonProperty("U_VlrDto")]
        public double? discountValue { get; set; }

        [JsonProperty("U_ValorFac")]
        public double? subtotalValue { get; set; }

        [JsonProperty("U_IVA")]
        public double? vatValue { get; set; }

        [JsonProperty("U_totalLinea")]
        public double? totalValue { get; set; }

        [JsonProperty("U_CodSN")]
        public string? cardCodeClient { get; set; }

        [JsonProperty("U_NombreSN")]
        public string? cardNameClient { get; set; }

        [JsonProperty("U_IdLinea")]
        public int? idLine { get; set; }

        [JsonProperty("U_ItemCodePB")]
        public string? codeFather { get; set; }

        [JsonProperty("U_LineaPadre")]
        public int? lineFather { get; set; }

        [JsonProperty("U_ItemCodeP")]
        public string? itemCodeFatherInChildLine { get; set; }

        [JsonProperty("U_ItemCodeHC")]
        public string? itemCodeChild { get; set; }

        [JsonProperty("U_usuAdi")]
        public string? userAdi { get; set; }

        [JsonProperty("U_confirmCofre")]
        public string? confirmCoffin { get; set; }

        [JsonProperty("U_permModArt")]
        public string? allowsModifyingItem { get; set; }

        [JsonProperty("U_EADCub")]
        public string? additionalCoveredStatus { get; set; }

        [JsonProperty("U_nvend")]
        public string? vendorName { get; set; }

        [JsonProperty("U_vrserv")]
        public int? vendor { get; set; }
    }

    public class OrdenadalFuneralServiceHead
    {
        [JsonProperty("general")]
        public GeneralInfo General { get; set; }

        [JsonProperty("aditionalInfo")]
        public AdditionalInfo AdditionalInfo { get; set; }

        [JsonProperty("deceased")]
        public DeceasedInfo Deceased { get; set; }

        [JsonProperty("contractor")]
        public ContractorInfo Contractor { get; set; }
        [JsonProperty("requester")]
        public RequesterInfo Requester { get; set; }
    }

    public class GeneralInfo
    {
        [JsonProperty("docDocument")]
        public int DocDocument { get; set; }

        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("dateCreation")]
        public string DateCreation { get; set; }

        [JsonProperty("expirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("contract")]
        public string Contract { get; set; }

        [JsonProperty("OrderTime")]
        public string orderTime { get; set; }

    }

    public class AdditionalInfo
    {
        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("agreement")]
        public string Agreement { get; set; }

        [JsonProperty("levelService")]
        public string LevelService { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }

        [JsonProperty("planName")]
        public string PlanName { get; set; }

        [JsonProperty("typeService")]
        public string TypeService { get; set; }

        [JsonProperty("typeBenefit")]
        public string TypeBenefit { get; set; }

        [JsonProperty("typeCoverage")]
        public string TypeCoverage { get; set; }

        [JsonProperty("equivalentPlan")]
        public string EquivalentPlan { get; set; }

        [JsonProperty("agreementName")]
        public string AgreementName { get; set; }

    }

    public class DeceasedInfo
    {
        [JsonProperty("typeIdentification")]
        public string TypeIdentification { get; set; }

        [JsonProperty("identification")]
        public string Identification { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthdate")]
        public string Birthdate { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }

        [JsonProperty("relationship")]
        public string Relationship { get; set; }

        [JsonProperty("dateDeath")]
        public string DateDeath { get; set; }

        [JsonProperty("hour")]
        public string? Hour { get; set; }

        [JsonProperty("cause")]
        public string Cause { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("clinic")]
        public string Clinic { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("unregisteredBeneficiary")]
        public string UnregisteredBeneficiary { get; set; }
    }

    public class ContractorInfo
    {
        [JsonProperty("typeIdentification")]
        public string TypeIdentification { get; set; }

        [JsonProperty("identification")]
        public string Identification { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("birthDate")]
        public string BirthDate { get; set; }

        [JsonProperty("ownerEqualsRequester")]
        public string OwnerEqualsRequester { get; set; }

    }

    public class Historical
    {
        [JsonProperty("U_Descrip")]
        public required string uDescrip { get; set; }

        [JsonProperty("U_Fecha")]
        public required string uFecha { get; set; }

        [JsonProperty("U_Hora")]
        public required string uHora { get; set; }

        [JsonProperty("U_Usuario")]
        public required string uUsuario { get; set; }
    }

    public class Documents
    {
        [JsonProperty("U_documento")]
        public required string uDocumento { get; set; }

        [JsonProperty("U_obligatorio")]
        public required string uObligatorio { get; set; } = "Y";
    }

    public class RequesterInfo
    {

        [JsonProperty("identification")]
        public string Identification { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

    }

    public class Requester
    {

        [JsonProperty("identification")]
        public string identification { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

    }
}
