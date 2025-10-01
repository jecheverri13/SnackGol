using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class FuneralsRequest
    {

        [JsonProperty("U_tipoMisa")]
        public string? typeMass { get; set; }

        [JsonProperty("U_ELugar")]
        public string? location { get; set; }

        [JsonProperty("U_nELugar")]
        public string? locationName { get; set; }

        [JsonProperty("U_sedeMisa")]
        public string? branch { get; set; }

        [JsonProperty("U_sedeExeNom")]
        public string? branchName { get; set; }

        [JsonProperty("U_EDpto")]
        public string? department { get; set; }

        [JsonProperty("U_nEDpto")]
        public string? departmentName { get; set; }

        [JsonProperty("U_EMuni")]
        public string? city { get; set; }

        [JsonProperty("U_nEMuni")]
        public string? cityName { get; set; }

        [JsonProperty("U_EFecha")]
        public string? date { get; set; }

        [JsonProperty("U_EHora")]
        public string? startTime { get; set; }

        [JsonProperty("U_horFiMisa")]
        public string? endTime { get; set; }

        [JsonProperty("U_dirMisa")]
        public string? address { get; set; }

        [JsonProperty("U_OfiReli")]
        public string? religiousService { get; set; }

        [JsonProperty("U_tipoSacerd")]
        public string? typePriest { get; set; }

        [JsonProperty("U_sacerdMisa")]
        public string? codePriest { get; set; }

        [JsonProperty("U_nomSacerMisa")]
        public string? namePriest { get; set; }

        [JsonProperty("U_coroMisa")]
        public string? choir { get; set; }

        [JsonProperty("U_catafMisa")]
        public string? isCatafalque { get; set; }

        [JsonProperty("U_Homilia")]
        public string? isHomily { get; set; }

        [JsonProperty("U_misaConme")]
        public string? isMemorialMass { get; set; }

        [JsonProperty("U_FechaTentMisaConm")]
        public string? dateMemorialMass { get; set; }
    }


    public class AdditionalsCemeteryPark
    {

        [JsonProperty("U_IUbic")]
        public string? cemetary { get; set; }

        [JsonProperty("U_nIUbic")]
        public string? cemetaryName { get; set; }

        [JsonProperty("U_IDpto")]
        public string? department { get; set; }

        [JsonProperty("U_nIDpto")]
        public string? departmentName { get; set; }

        [JsonProperty("U_IMuni")]
        public string? city { get; set; }

        [JsonProperty("U_nIMuni")]
        public string? cityName { get; set; }

        [JsonProperty("U_IFecha")]
        public string? dateInhumation { get; set; }

        [JsonProperty("U_IDir")]
        public string? address { get; set; }

        [JsonProperty("U_ILugar")]
        public string? finalDestination { get; set; }

        [JsonProperty("U_nILugar")]
        public string? finalDestinationName { get; set; }

        [JsonProperty("U_IHora")]
        public string? hourInhumation { get; set; }

        [JsonProperty("U_SFecha")]
        public string? dateExhumation { get; set; }

        [JsonProperty("U_SHora")]
        public string? hourExhumation { get; set; }
    }

    public class Wake
    {

        [JsonProperty("U_sedeVel")]
        public string? branch { get; set; }

        [JsonProperty("U_sedeVelNom")]
        public string? branchName { get; set; }

        [JsonProperty("U_VLugar")]
        public string? location { get; set; }

        [JsonProperty("U_nVLugar")]
        public string? locationName { get; set; }

        [JsonProperty("U_deptVel")]
        public string? department { get; set; }

        [JsonProperty("U_ndeptVel")]
        public string? departmentName { get; set; }

        [JsonProperty("U_dirVel")]
        public string? address { get; set; }

        [JsonProperty("U_munVel")]
        public string? city { get; set; }

        [JsonProperty("U_nmunVel")]
        public string? cityName { get; set; }

        [JsonProperty("U_VFecha")]
        public string? entryDate { get; set; }

        [JsonProperty("U_horVelEntr")]
        public string? entryTime { get; set; }

        [JsonProperty("U_VFechaS")]
        public string? departureDate { get; set; }

        [JsonProperty("U_horVelSal")]
        public string? departureTime { get; set; }

        [JsonProperty("U_VSerVir")]
        public string? virtualWake { get; set; }

    }

    public class AdditionalsWake
    {

        [JsonProperty("U_VSalaExc")]
        public string? locationExc { get; set; }

        [JsonProperty("U_nVSalaExc")]
        public string? locationNameExc { get; set; }

        [JsonProperty("U_VFechaExc")]
        public string? entryDateExc { get; set; }

        [JsonProperty("U_horVelEntExc")]
        public string? entryTimeExc { get; set; }

        [JsonProperty("U_VFechaSalExc")]
        public string? departureDateExc { get; set; }

        [JsonProperty("U_horVelSalExc")]
        public string? departureTimeExc { get; set; }

        [JsonProperty("U_VSalaR")]
        public string? locationRee { get; set; }

        [JsonProperty("U_nVSalaR")]
        public string? locationNameRee { get; set; }

        [JsonProperty("U_VFechaR")]
        public string? entryDateRee { get; set; }

        [JsonProperty("U_horVelEntRe")]
        public string? entryTimeRee { get; set; }

        [JsonProperty("U_VFechaSalR")]
        public string? departureDateRee { get; set; }

        [JsonProperty("U_horVelSalRe")]
        public string? departureTimeRee { get; set; }

        [JsonProperty("U_VlabPltBaj")]
        public string? locationPB { get; set; }

        [JsonProperty("U_nVlabPltBaj")]
        public string? locationNamePB { get; set; }

        [JsonProperty("U_VFechaPltBaj")]
        public string? entryDatePB { get; set; }

        [JsonProperty("U_horVelEntPltBaj")]
        public string? entryTimePB { get; set; }

        [JsonProperty("U_VFechaSalPltBaj")]
        public string? departureDatePB { get; set; }

        [JsonProperty("U_horVelSalPltBaj")]
        public string? departureTimePB { get; set; }

    }
}
