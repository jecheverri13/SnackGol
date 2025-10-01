using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class FuneralServiceOrderGE
    {
        [JsonProperty("DocEntry")]
        public string? docEntry { get; set; }


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
        public string? contractingId { get; set; }

        [JsonProperty("U_NombreCo")]
        public string? contractingName { get; set; }

        [JsonProperty("U_TelContr")]
        public string? contractingPhone { get; set; }

        [JsonProperty("U_DirContr")]
        public string? contractingAdress { get; set; }

        [JsonProperty("U_Ciudad")]
        public string? contractingCity { get; set; }


        [JsonProperty("U_Email")]
        public string? contractingEmail { get; set; }

        [JsonProperty("U_Celular")]
        public string? contractingCellPhone { get; set; }


        [JsonProperty("U_EntConCl")]
        public string? clientAgreement { get; set; }

        [JsonProperty("U_NumContr")]
        public int? contractNumber { get; set; }

        [JsonProperty("U_Vigencia")]
        public int? validity { get; set; }

        [JsonProperty("U_CodPlan")]
        public string? planCode { get; set; }

        [JsonProperty("U_Plan")]
        public string? planName { get; set; }


        [JsonProperty("U_codPlanEquiv")]
        public string? planEquivalentCode { get; set; }

        [JsonProperty("U_convPart")]
        public string? particularAgreement { get; set; }


        [JsonProperty("U_NomEncSr")]
        public string? serviceManagerName { get; set; }

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
        public string? funeralChecklistType { get; set; }

        [JsonProperty("U_TipoBene")]
        public string? beneficiaryType { get; set; }

        [JsonProperty("U_TipDoc")]
        public string? documentType { get; set; }

        [JsonProperty("U_nTipDoc")]
        public string? documentNameType { get; set; }

        [JsonProperty("U_IdFallec")]
        public string? deceasedId { get; set; }

        [JsonProperty("U_NombreFa")]
        public string? deceasedName { get; set; }

        [JsonProperty("U_FechaNac")]
        public string? birthDateDeceased { get; set; }


        [JsonProperty("U_Edad")]
        public int? ageDeceased { get; set; }


        [JsonProperty("U_Parentes")]
        public string? relationshipCode { get; set; }

        [JsonProperty("U_nParentes")]
        public string? relationshipName { get; set; }

        [JsonProperty("U_Genero")]
        public string? gender { get; set; }

        [JsonProperty("U_FechaFal")]
        public string? dateDeath { get; set; }

        [JsonProperty("U_HoraFall")]
        public string? hourDeath { get; set; }

        [JsonProperty("U_CausaFal")]
        public string? codeCauseOfDeath { get; set; }

        [JsonProperty("U_nCausaFal")]
        public string? nameCauseOfDeath { get; set; }

        [JsonProperty("U_LugDefunc")]
        public string? placeOfDeath { get; set; }

        [JsonProperty("U_UbicLoc")]
        public string? codeLocationOfDeath { get; set; }

        [JsonProperty("U_nUbicLoc")]
        public string? nameLocationOfDeath { get; set; }

        [JsonProperty("U_paisFa")]
        public string? codeCountryOfDeath { get; set; }

        [JsonProperty("U_npaisFa")]
        public string? nameCountryOfDeath { get; set; }

        [JsonProperty("U_DptoFa")]
        public string? codeDepartamentOfDeath { get; set; }

        [JsonProperty("U_nDptoFa")]
        public string? nameDepartamentOfDeath { get; set; }

        [JsonProperty("U_CiudadFa")]
        public string? codeCityOfDeath { get; set; }

        [JsonProperty("U_nCiudadFa")]
        public string? nameCityOfDeath { get; set; }

        [JsonProperty("U_Ubicacio")]
        public string? addressLocation { get; set; }

        [JsonProperty("U_sedBarr")]
        public string? districtLocation { get; set; }

        [JsonProperty("U_Telf")]
        public string? telLocation { get; set; }

        [JsonProperty("U_Observac")]
        public string? externalObservations { get; set; }


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
        public required List<OK1_SF_ELEM_LIN_GE> OFSElements { get; set; }



    }


    public class OK1_SF_ELEM_LIN_GE
    {
        [JsonProperty("LineId")]
        public string? lineId { get; set; }

        [JsonProperty("U_Codigo")]
        public string? elementCode { get; set; }

        [JsonProperty("U_Nombre")]
        public string? elementName { get; set; }

        [JsonProperty("U_tipLinea")]
        public string? lineType { get; set; }

        [JsonProperty("U_tipOrden")]
        public string? orderType { get; set; }

        [JsonProperty("U_Cant")]
        public int? quantity { get; set; }

        [JsonProperty("U_CantCu")]
        public int? quantityCovered { get; set; }

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


        [JsonProperty("U_EsFactu")]
        public string? isBilled { get; set; }


        [JsonProperty("U_Valor")]
        public double? saleValue { get; set; }

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
        public string? itemCodeFather { get; set; }


        [JsonProperty("U_LineaPadre")]
        public int? idLineFather { get; set; }


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


    }
}
