using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.EntitiesSL
{
    public class Activities2
    {
        [JsonProperty("Activity")]
        public string? activity { get; set; }

        [JsonProperty("ActivityType")]
        public string? activityType { get; set; }

        [JsonProperty("Subject")]
        public string? subject { get; set; }

        [JsonProperty("CardCode")]
        public string? identification { get; set; }

        [JsonProperty("U_DocEntry")]
        public int? documentOSF { get; set; }

        [JsonProperty("Details")]
        public string? comments { get; set; }

        [JsonProperty("ActivityDate")]
        public string? activityDate { get; set; }

        [JsonProperty("StartTime")]
        public string? startTime { get; set; }

        [JsonProperty("StartDate")]
        public string? startDate { get; set; }

        [JsonProperty("EndTime")]
        public string? endTime { get; set; }

        [JsonProperty("Duration")]
        public double? duration { get; set; }

        [JsonProperty("DurationType")]
        public string? durationType { get; set; }

        [JsonProperty("U_fechSolic")]
        public string? requestDate { get; set; }

        [JsonProperty("U_horSolic")]
        public string? requestTime { get; set; }

        [JsonProperty("U_tipoSolic")]
        public string? typeOfRequest { get; set; }

        [JsonProperty("U_motReprog")]
        public string? motReprog { get; set; }

        [JsonProperty("U_fallMult")]
        public string? fallMult { get; set; }

        [JsonProperty("U_rechaz")]
        public string? rechaz { get; set; }

        [JsonProperty("U_motRech")]
        public string? motRech { get; set; }

        [JsonProperty("U_ItemCodeTras")]
        public string? item { get; set; }

        [JsonProperty("Reminder")]
        public string? reminder { get; set; }

        [JsonProperty("origin")]
        public required Origin Origin { get; set; }

        [JsonProperty("destination")]
        public required Destination Destination { get; set; }

    }

    public class Origin
    {
        //ORIGEN

        [JsonProperty("U_tipoTraslOrg")]
        public string? typeTransfer { get; set; }

        [JsonProperty("U_orgInh")]
        public string? burialPlace { get; set; }

        [JsonProperty("U_orgAeroPuer")]
        public string? airport { get; set; }

        [JsonProperty("U_orgServPu")]
        public string? servicesPort { get; set; }

        [JsonProperty("U_orgSedeFun")]
        public string? funeralHome { get; set; }

        [JsonProperty("U_orgNomSede")]
        public string? nameBranchOtraFuner { get; set; }

        [JsonProperty("U_orgNomFuner")]
        public string? funeral { get; set; }

        [JsonProperty("U_orgTipViaje")]
        public string? typeTravel { get; set; }

        [JsonProperty("U_orgkilCubTras")]
        public int? coveredKilometers { get; set; }

        [JsonProperty("U_orgValCubTras")]
        public int? priceKilometer { get; set; }

        [JsonProperty("U_orgAerolinea")]
        public string? airline { get; set; }

        [JsonProperty("U_orgNumVuelo")]
        public int? flightNumber { get; set; }

        [JsonProperty("U_orghorVuelo")]
        public string? flightHour { get; set; }

        [JsonProperty("U_orgNumGuia")]
        public int? guideNumber { get; set; }

        [JsonProperty("U_orgSedeVel")]
        public string? sailingSite { get; set; }

        [JsonProperty("U_orgNomSedeVel")]
        public string? nameBranchVel { get; set; }

        [JsonProperty("U_orgSalaVel")]
        public string? placeOfVigil { get; set; }

        [JsonProperty("U_orgSedeExe")]
        public string? exequialHeadquarters { get; set; }

        [JsonProperty("U_orgNomSedeExe")]
        public string? nameBranchExe { get; set; }

        [JsonProperty("U_orgLugExe")]
        public string? exequiasPlace { get; set; }

        [JsonProperty("U_tipoLab")]
        public string? typeLaboratory { get; set; }

        [JsonProperty("U_laboratorio")]
        public string? name { get; set; }

        [JsonProperty("U_orgCuerpo")]
        public string? originBody { get; set; }

        [JsonProperty("U_orgUbicLoc")]
        public string? ubication { get; set; }

        [JsonProperty("U_orgPais")]
        public string? country { get; set; }

        [JsonProperty("U_orgSedBarr")]
        public string? districtOrHeadquarters { get; set; }

        [JsonProperty("U_orgTelf")]
        public string? phone { get; set; }

        [JsonProperty("U_depOrigen")]
        public string? department { get; set; }

        [JsonProperty("U_munOrigen")]
        public string? city { get; set; }

        [JsonProperty("U_direcOrg")]
        public string? address { get; set; }
    }

    public class Destination
    {
        //DESTINO

        [JsonProperty("U_tipoTraslDes")]
        public string? typeTransfer { get; set; }

        [JsonProperty("U_tipoLabDes")]
        public string? typeLaboratory { get; set; }

        [JsonProperty("U_desCuerpo")]
        public string? destinationBody { get; set; }

        [JsonProperty("U_laboratorioDes")]
        public string? name { get; set; }

        [JsonProperty("U_desInh")]
        public string? burialPlace { get; set; }

        [JsonProperty("U_desServPu")]
        public string? servicesPort { get; set; }

        [JsonProperty("U_desSedeFun")]
        public string? funeralHome { get; set; }

        [JsonProperty("U_desNomSede")]
        public string? nameBranchOtraFuner { get; set; }

        [JsonProperty("U_desNomFuner")]
        public string? funeral { get; set; }

        [JsonProperty("U_desTipViaje")]
        public string? typeTravel { get; set; }

        [JsonProperty("U_deskilCubTras")]
        public int? coveredKilometers { get; set; }

        [JsonProperty("U_desValCubTras")]
        public int? priceKilometer { get; set; }

        [JsonProperty("U_desAeroPuer")]
        public string? airport { get; set; }

        [JsonProperty("U_desAerolinea")]
        public string? airline { get; set; }

        [JsonProperty("U_desNumVuelo")]
        public int? flightNumber { get; set; }

        [JsonProperty("U_deshorVuelo")]
        public string? flightHour { get; set; }

        [JsonProperty("U_desNumGuia")]
        public int? guideNumber { get; set; }

        [JsonProperty("U_desSedeVel")]
        public string? sailingSite { get; set; }

        [JsonProperty("U_desNomSedeVel")]
        public string? nameBranchVel { get; set; }

        [JsonProperty("U_desSalaVel")]
        public string? placeOfVigil { get; set; }

        [JsonProperty("U_desLugExe")]
        public string? exequiasPlace { get; set; }

        [JsonProperty("U_desSedeExe")]
        public string? exequialHeadquarters { get; set; }

        [JsonProperty("U_desNomSedeExe")]
        public string? nameBranchExe { get; set; }

        [JsonProperty("U_desUbicLoc")]
        public string? ubication { get; set; }

        [JsonProperty("U_desPais")]
        public string? country { get; set; }

        [JsonProperty("U_desSedBarr")]
        public string? districtOrHeadquarters { get; set; }

        [JsonProperty("U_desTelf")]
        public string? phone { get; set; }

        [JsonProperty("U_depDest")]
        public string? department { get; set; }

        [JsonProperty("U_munDest")]
        public string? city { get; set; }

        [JsonProperty("U_direcDest")]
        public string? address { get; set; }
    }

    public class Activities
    {
        [JsonProperty("Activity")]
        public string? activity { get; set; }

        [JsonProperty("ActivityType")]
        public string? activityType { get; set; }

        [JsonProperty("Subject")]
        public string? subject { get; set; }

        [JsonProperty("CardCode")]
        public string? identification { get; set; }

        [JsonProperty("U_DocEntry")]
        public int? documentOSF { get; set; }

        [JsonProperty("Details")]
        public string? comments { get; set; }

        [JsonProperty("ActivityDate")]
        public string? activityDate { get; set; }

        [JsonProperty("StartTime")]
        public string? startTime { get; set; }

        [JsonProperty("StartDate")]
        public string? startDate { get; set; }

        [JsonProperty("EndTime")]
        public string? endTime { get; set; }

        [JsonProperty("Duration")]
        public double? duration { get; set; }

        [JsonProperty("DurationType")]
        public string? durationType { get; set; }

        [JsonProperty("U_fechSolic")]
        public string? requestDate { get; set; }

        [JsonProperty("U_horSolic")]
        public string? requestTime { get; set; }

        [JsonProperty("U_tipoSolic")]
        public string? typeOfRequest { get; set; }

        [JsonProperty("U_motReprog")]
        public string? motReprog { get; set; }

        [JsonProperty("U_fallMult")]
        public string? fallMult { get; set; }

        [JsonProperty("U_rechaz")]
        public string? rechaz { get; set; }

        [JsonProperty("U_motRech")]
        public string? motRech { get; set; }

        [JsonProperty("U_ItemCodeTras")]
        public string? item { get; set; }

        [JsonProperty("Reminder")]
        public string? reminder { get; set; }

        [JsonProperty("U_tipoLab")]
        public string? typeLaboratory { get; set; }

        // ORIGEN
        [JsonProperty("U_tipoTraslOrg")]
        public string? typeTransfer { get; set; }

        [JsonProperty("U_orgInh")]
        public string? orgBurialPlace { get; set; }

        [JsonProperty("U_orgAeroPuer")]
        public string? orgAirport { get; set; }

        [JsonProperty("U_orgServPu")]
        public string? orgServicesPort { get; set; }

        [JsonProperty("U_orgSedeFun")]
        public string? orgFuneralHome { get; set; }

        [JsonProperty("U_orgNomSede")]
        public string? orgNameBranchOtraFuner { get; set; }

        [JsonProperty("U_orgNomFuner")]
        public string? orgFuneral { get; set; }

        [JsonProperty("U_orgTipViaje")]
        public string? orgTypeTravel { get; set; }

        [JsonProperty("U_orgkilCubTras")]
        public int? orgCoveredKilometers { get; set; }

        [JsonProperty("U_orgValCubTras")]
        public int? orgPriceKilometer { get; set; }

        [JsonProperty("U_orgAerolinea")]
        public string? orgAirline { get; set; }

        [JsonProperty("U_orgNumVuelo")]
        public int? orgFlightNumber { get; set; }

        [JsonProperty("U_orghorVuelo")]
        public string? orgFlightHour { get; set; }

        [JsonProperty("U_orgNumGuia")]
        public int? orgGuideNumber { get; set; }

        [JsonProperty("U_orgSedeVel")]
        public string? orgSailingSite { get; set; }

        [JsonProperty("U_orgNomSedeVel")]
        public string? orgNameBranchVel { get; set; }

        [JsonProperty("U_orgSalaVel")]
        public string? orgPlaceOfVigil { get; set; }

        [JsonProperty("U_orgSedeExe")]
        public string? orgExequialHeadquarters { get; set; }

        [JsonProperty("U_orgNomSedeExe")]
        public string? orgNameBranchExe { get; set; }

        [JsonProperty("U_orgLugExe")]
        public string? orgExequiasPlace { get; set; }

        [JsonProperty("U_laboratorio")]
        public string? laboratory { get; set; }

        [JsonProperty("U_orgCuerpo")]
        public string? originBody { get; set; }

        [JsonProperty("U_orgUbicLoc")]
        public string? ubication { get; set; }

        [JsonProperty("U_orgPais")]
        public string? country { get; set; }

        [JsonProperty("U_orgSedBarr")]
        public string? districtOrHeadquarters { get; set; }

        [JsonProperty("U_orgTelf")]
        public string? phone { get; set; }

        [JsonProperty("U_depOrigen")]
        public string? department { get; set; }

        [JsonProperty("U_munOrigen")]
        public string? city { get; set; }

        [JsonProperty("U_direcOrg")]
        public string? address { get; set; }

        // DESTINO
        [JsonProperty("U_tipoTraslDes")]
        public string? transferTypeDestination { get; set; }

        [JsonProperty("U_tipoLabDes")]
        public string? typeLaboratoryDestination { get; set; }

        [JsonProperty("U_desUbicLoc")]
        public string? destinationLocation { get; set; }

        [JsonProperty("U_desCuerpo")]
        public string? destinationBody { get; set; }

        [JsonProperty("U_laboratorioDes")]
        public string? destinationNameLaboratory { get; set; }

        [JsonProperty("U_desInh")]
        public string? desBurialPlace { get; set; }

        [JsonProperty("U_desServPu")]
        public string? desServicesPort { get; set; }

        [JsonProperty("U_desSedeFun")]
        public string? desFuneralHome { get; set; }

        [JsonProperty("U_desNomSede")]
        public string? desNameBranchOtraFuner { get; set; }

        [JsonProperty("U_desNomFuner")]
        public string? desFuneral { get; set; }

        [JsonProperty("U_desTipViaje")]
        public string? desTypeTravel { get; set; }

        [JsonProperty("U_deskilCubTras")]
        public int? desCoveredKilometers { get; set; }

        [JsonProperty("U_desValCubTras")]
        public int? desPriceKilometer { get; set; }

        [JsonProperty("U_desAeroPuer")]
        public string? desAirport { get; set; }

        [JsonProperty("U_desAerolinea")]
        public string? desAirline { get; set; }

        [JsonProperty("U_desNumVuelo")]
        public int? desFlightNumber { get; set; }

        [JsonProperty("U_deshorVuelo")]
        public string? desFlightHour { get; set; }

        [JsonProperty("U_desNumGuia")]
        public int? desGuideNumber { get; set; }

        [JsonProperty("U_desSedeVel")]
        public string? desSailingSite { get; set; }

        [JsonProperty("U_desNomSedeVel")]
        public string? desNameBranchVel { get; set; }

        [JsonProperty("U_desSalaVel")]
        public string? desPlaceOfVigil { get; set; }

        [JsonProperty("U_desLugExe")]
        public string? desExequiasPlace { get; set; }

        [JsonProperty("U_desSedeExe")]
        public string? desExequialHeadquarters { get; set; }

        [JsonProperty("U_desNomSedeExe")]
        public string? desNameBranchExe { get; set; }

        [JsonProperty("U_desPais")]
        public string? destinationCountry { get; set; }

        [JsonProperty("U_desSedBarr")]
        public string? destinationDistrictOrHeadquarters { get; set; }

        [JsonProperty("U_desTelf")]
        public string? destinationPhone { get; set; }

        [JsonProperty("U_depDest")]
        public string? destinationDepartment { get; set; }

        [JsonProperty("U_munDest")]
        public string? destinationCity { get; set; }

        [JsonProperty("U_direcDest")]
        public string? destinationAddress { get; set; }
    }
}
