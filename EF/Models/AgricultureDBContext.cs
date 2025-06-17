using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF.Models;

public partial class AgricultureDBContext : DbContext
{
    public AgricultureDBContext()
    {
    }

    public AgricultureDBContext(DbContextOptions<AgricultureDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<A_AttachmentData_Ex_CheckRequest> A_AttachmentData_Ex_CheckRequests { get; set; }

    public virtual DbSet<A_AttachmentData_Ex_CommitteeResult_Infection> A_AttachmentData_Ex_CommitteeResult_Infections { get; set; }

    public virtual DbSet<A_AttachmentData_Im_CommitteeResult_Infection> A_AttachmentData_Im_CommitteeResult_Infections { get; set; }

    public virtual DbSet<A_AttachmentData_Station> A_AttachmentData_Stations { get; set; }

    public virtual DbSet<A_AttachmentDatum> A_AttachmentData { get; set; }

    public virtual DbSet<A_AttachmentDatum1> A_AttachmentData1 { get; set; }

    public virtual DbSet<A_AttachmentTableName> A_AttachmentTableNames { get; set; }

    public virtual DbSet<A_AttachmentTableType> A_AttachmentTableTypes { get; set; }

    public virtual DbSet<A_SystemCode> A_SystemCodes { get; set; }

    public virtual DbSet<A_SystemCodeType> A_SystemCodeTypes { get; set; }

    public virtual DbSet<A__User_Login> A__User_Logins { get; set; }

    public virtual DbSet<A__plant_Error_Save> A__plant_Error_Saves { get; set; }

    public virtual DbSet<AnalysisLab> AnalysisLabs { get; set; }

    public virtual DbSet<AnalysisLabType> AnalysisLabTypes { get; set; }

    public virtual DbSet<AnalysisType> AnalysisTypes { get; set; }

    public virtual DbSet<Andriod_Location> Andriod_Locations { get; set; }

    public virtual DbSet<Andriod_Operation> Andriod_Operations { get; set; }

    public virtual DbSet<Biological_Phase> Biological_Phases { get; set; }

    public virtual DbSet<Center> Centers { get; set; }

    public virtual DbSet<CommitteeEmployee> CommitteeEmployees { get; set; }

    public virtual DbSet<CommitteeResultType> CommitteeResultTypes { get; set; }

    public virtual DbSet<CommitteeType> CommitteeTypes { get; set; }

    public virtual DbSet<CompanyAccreditation> CompanyAccreditations { get; set; }

    public virtual DbSet<CompanyAccreditation_Committee> CompanyAccreditation_Committees { get; set; }

    public virtual DbSet<CompanyAccreditation_Payment> CompanyAccreditation_Payments { get; set; }

    public virtual DbSet<CompanyActivity> CompanyActivities { get; set; }

    public virtual DbSet<CompanyActivity1> CompanyActivities1 { get; set; }

    public virtual DbSet<CompanyActivityType> CompanyActivityTypes { get; set; }

    public virtual DbSet<Company_National> Company_Nationals { get; set; }

    public virtual DbSet<Company_National1> Company_Nationals1 { get; set; }

    public virtual DbSet<ContactType> ContactTypes { get; set; }

    public virtual DbSet<Continent> Continents { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<EX> Exes { get; set; }

    public virtual DbSet<EX_Choose_SampleDatum> EX_Choose_SampleData { get; set; }

    public virtual DbSet<EX_Choose_Treatment> EX_Choose_Treatments { get; set; }

    public virtual DbSet<EX_Constrain_Country_Item> EX_Constrain_Country_Items { get; set; }

    public virtual DbSet<EX_Constrain_Text> EX_Constrain_Texts { get; set; }

    public virtual DbSet<EX_Constrain_Type> EX_Constrain_Types { get; set; }

    public virtual DbSet<EX_Fees_Type> EX_Fees_Types { get; set; }

    public virtual DbSet<Enrollment_type> Enrollment_types { get; set; }

    public virtual DbSet<Ex_CertificateAddtion> Ex_CertificateAddtions { get; set; }

    public virtual DbSet<Ex_CertificateAddtionUser> Ex_CertificateAddtionUsers { get; set; }

    public virtual DbSet<Ex_CertificatesNewCountry> Ex_CertificatesNewCountries { get; set; }

    public virtual DbSet<Ex_CertificatesRequest> Ex_CertificatesRequests { get; set; }

    public virtual DbSet<Ex_CertificatesRequestsFile> Ex_CertificatesRequestsFiles { get; set; }

    public virtual DbSet<Ex_CertificatesRequestsLotDatum> Ex_CertificatesRequestsLotData { get; set; }

    public virtual DbSet<Ex_CertificatesRequestsPayment> Ex_CertificatesRequestsPayments { get; set; }

    public virtual DbSet<Ex_CertificatesRequestsPaymentsDetaile> Ex_CertificatesRequestsPaymentsDetailes { get; set; }

    public virtual DbSet<Ex_CertificatesRequestsPaymentsType> Ex_CertificatesRequestsPaymentsTypes { get; set; }

    public virtual DbSet<Ex_CheckRequest> Ex_CheckRequests { get; set; }

    public virtual DbSet<Ex_CheckRequestData_Extra> Ex_CheckRequestData_Extras { get; set; }

    public virtual DbSet<Ex_CheckRequest_Customs_Message> Ex_CheckRequest_Customs_Messages { get; set; }

    public virtual DbSet<Ex_CheckRequest_Datum> Ex_CheckRequest_Data { get; set; }

    public virtual DbSet<Ex_CheckRequest_Fee> Ex_CheckRequest_Fees { get; set; }

    public virtual DbSet<Ex_CheckRequest_Final_Result> Ex_CheckRequest_Final_Results { get; set; }

    public virtual DbSet<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; }

    public virtual DbSet<Ex_CheckRequest_Items_Lot_Category> Ex_CheckRequest_Items_Lot_Categories { get; set; }

    public virtual DbSet<Ex_CheckRequest_Items_Lot_Result> Ex_CheckRequest_Items_Lot_Results { get; set; }

    public virtual DbSet<Ex_CheckRequest_Lot_Result_Status> Ex_CheckRequest_Lot_Result_Statuses { get; set; }

    public virtual DbSet<Ex_CheckRequest_Organization_Distribution> Ex_CheckRequest_Organization_Distributions { get; set; }

    public virtual DbSet<Ex_CheckRequest_Organization_Distribution_Detial> Ex_CheckRequest_Organization_Distribution_Detials { get; set; }

    public virtual DbSet<Ex_CheckRequest_Organization_Distribution_Master> Ex_CheckRequest_Organization_Distribution_Masters { get; set; }

    public virtual DbSet<Ex_CheckRequest_Place> Ex_CheckRequest_Places { get; set; }

    public virtual DbSet<Ex_CheckRequest_Port> Ex_CheckRequest_Ports { get; set; }

    public virtual DbSet<Ex_CheckRequest_RefuseReason> Ex_CheckRequest_RefuseReasons { get; set; }

    public virtual DbSet<Ex_CheckRequest_SampleData_Confirm> Ex_CheckRequest_SampleData_Confirms { get; set; }

    public virtual DbSet<Ex_CheckRequest_SampleDatum> Ex_CheckRequest_SampleData { get; set; }

    public virtual DbSet<Ex_CheckRequest_Visa> Ex_CheckRequest_Visas { get; set; }

    public virtual DbSet<Ex_CheckRequset_Shipping_Method> Ex_CheckRequset_Shipping_Methods { get; set; }

    public virtual DbSet<Ex_CommitteeCheckLocation> Ex_CommitteeCheckLocations { get; set; }

    public virtual DbSet<Ex_CommitteeResult> Ex_CommitteeResults { get; set; }

    public virtual DbSet<Ex_CommitteeResult_Confirm> Ex_CommitteeResult_Confirms { get; set; }

    public virtual DbSet<Ex_CommitteeResult_Infection> Ex_CommitteeResult_Infections { get; set; }

    public virtual DbSet<Ex_Constran> Ex_Constrans { get; set; }

    public virtual DbSet<Ex_ContactDatum> Ex_ContactData { get; set; }

    public virtual DbSet<Ex_ContactDatum1> Ex_ContactData1 { get; set; }

    public virtual DbSet<Ex_CountryConstrain> Ex_CountryConstrains { get; set; }

    public virtual DbSet<Ex_CountryConstrain_AnalysisLabType> Ex_CountryConstrain_AnalysisLabTypes { get; set; }

    public virtual DbSet<Ex_CountryConstrain_ArrivalPort> Ex_CountryConstrain_ArrivalPorts { get; set; }

    public virtual DbSet<Ex_CountryConstrain_Text> Ex_CountryConstrain_Texts { get; set; }

    public virtual DbSet<Ex_CountryConstrain_Treatment> Ex_CountryConstrain_Treatments { get; set; }

    public virtual DbSet<Ex_Final_Result> Ex_Final_Results { get; set; }

    public virtual DbSet<Ex_List> Ex_Lists { get; set; }

    public virtual DbSet<Ex_List2> Ex_List2s { get; set; }

    public virtual DbSet<Ex_OpertaionType> Ex_OpertaionTypes { get; set; }

    public virtual DbSet<Ex_RequestCommittee> Ex_RequestCommittees { get; set; }

    public virtual DbSet<Ex_RequestCommittee_Fees_ENG> Ex_RequestCommittee_Fees_ENGs { get; set; }

    public virtual DbSet<Ex_RequestCommittee_Shift> Ex_RequestCommittee_Shifts { get; set; }

    public virtual DbSet<Ex_Request_TreatmentData_Confirm> Ex_Request_TreatmentData_Confirms { get; set; }

    public virtual DbSet<Ex_Request_TreatmentDatum> Ex_Request_TreatmentData { get; set; }

    public virtual DbSet<Ex_Visa> Ex_Visas { get; set; }

    public virtual DbSet<Family> Families { get; set; }

    public virtual DbSet<Farm_CheckList> Farm_CheckLists { get; set; }

    public virtual DbSet<Farm_Committee> Farm_Committees { get; set; }

    public virtual DbSet<Farm_Committee_CheckList> Farm_Committee_CheckLists { get; set; }

    public virtual DbSet<Farm_Committee_CheckList_Confirm> Farm_Committee_CheckList_Confirms { get; set; }

    public virtual DbSet<Farm_Committee_Constrain> Farm_Committee_Constrains { get; set; }

    public virtual DbSet<Farm_Committee_Examination> Farm_Committee_Examinations { get; set; }

    public virtual DbSet<Farm_Committee_Examination_Confirm> Farm_Committee_Examination_Confirms { get; set; }

    public virtual DbSet<Farm_Committee_Final_Result> Farm_Committee_Final_Results { get; set; }

    public virtual DbSet<Farm_Committee_Shift> Farm_Committee_Shifts { get; set; }

    public virtual DbSet<Farm_Company> Farm_Companies { get; set; }

    public virtual DbSet<Farm_Constrain> Farm_Constrains { get; set; }

    public virtual DbSet<Farm_Constrain_Text> Farm_Constrain_Texts { get; set; }

    public virtual DbSet<Farm_Country> Farm_Countries { get; set; }

    public virtual DbSet<Farm_Country_CheckList> Farm_Country_CheckLists { get; set; }

    public virtual DbSet<Farm_Fee> Farm_Fees { get; set; }

    public virtual DbSet<Farm_ItemCategory> Farm_ItemCategories { get; set; }

    public virtual DbSet<Farm_Request> Farm_Requests { get; set; }

    public virtual DbSet<Farm_Request_ItemCategory> Farm_Request_ItemCategories { get; set; }

    public virtual DbSet<Farm_Request_Refuse_Reason> Farm_Request_Refuse_Reasons { get; set; }

    public virtual DbSet<Farm_Request_Type> Farm_Request_Types { get; set; }

    public virtual DbSet<Farm_SampleData_Confirm> Farm_SampleData_Confirms { get; set; }

    public virtual DbSet<Farm_SampleData_Confirm_Item> Farm_SampleData_Confirm_Items { get; set; }

    public virtual DbSet<Farm_SampleData_Item> Farm_SampleData_Items { get; set; }

    public virtual DbSet<Farm_SampleDatum> Farm_SampleData { get; set; }

    public virtual DbSet<FarmsDatum> FarmsData { get; set; }

    public virtual DbSet<Farms_Organization_Distribution_Detial> Farms_Organization_Distribution_Detials { get; set; }

    public virtual DbSet<Farms_Organization_Distribution_Master> Farms_Organization_Distribution_Masters { get; set; }

    public virtual DbSet<FeesAmount_Fixed> FeesAmount_Fixeds { get; set; }

    public virtual DbSet<FeesType> FeesTypes { get; set; }

    public virtual DbSet<Fees_Action> Fees_Actions { get; set; }

    public virtual DbSet<Fees_Certificates_Payment_Detile> Fees_Certificates_Payment_Detiles { get; set; }

    public virtual DbSet<Fees_Money> Fees_Moneys { get; set; }

    public virtual DbSet<Fees_TableName> Fees_TableNames { get; set; }

    public virtual DbSet<Fees_Transaction> Fees_Transactions { get; set; }

    public virtual DbSet<Fees_Transactions_Detile> Fees_Transactions_Detiles { get; set; }

    public virtual DbSet<Fees_Transactions_Payment_Detile> Fees_Transactions_Payment_Detiles { get; set; }

    public virtual DbSet<Fees_Type_Action> Fees_Type_Actions { get; set; }

    public virtual DbSet<Fees_process> Fees_processes { get; set; }

    public virtual DbSet<FreeZone> FreeZones { get; set; }

    public virtual DbSet<FumigationUnit> FumigationUnits { get; set; }

    public virtual DbSet<Gas_ImportCompany> Gas_ImportCompanies { get; set; }

    public virtual DbSet<General_Admin> General_Admins { get; set; }

    public virtual DbSet<Governate> Governates { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<HagrContact> HagrContacts { get; set; }

    public virtual DbSet<Im_CheckRequest> Im_CheckRequests { get; set; }

    public virtual DbSet<Im_CheckRequestData_Extra> Im_CheckRequestData_Extras { get; set; }

    public virtual DbSet<Im_CheckRequest_Customs_Message> Im_CheckRequest_Customs_Messages { get; set; }

    public virtual DbSet<Im_CheckRequest_Datum> Im_CheckRequest_Data { get; set; }

    public virtual DbSet<Im_CheckRequest_Distribution> Im_CheckRequest_Distributions { get; set; }

    public virtual DbSet<Im_CheckRequest_Final_Result> Im_CheckRequest_Final_Results { get; set; }

    public virtual DbSet<Im_CheckRequest_Item> Im_CheckRequest_Items { get; set; }

    public virtual DbSet<Im_CheckRequest_Items_Lot_Category> Im_CheckRequest_Items_Lot_Categories { get; set; }

    public virtual DbSet<Im_CheckRequest_Items_Lot_Result> Im_CheckRequest_Items_Lot_Results { get; set; }

    public virtual DbSet<Im_CheckRequest_Lot_Result_Status> Im_CheckRequest_Lot_Result_Statuses { get; set; }

    public virtual DbSet<Im_CheckRequest_Manafest> Im_CheckRequest_Manafests { get; set; }

    public virtual DbSet<Im_CheckRequest_Port> Im_CheckRequest_Ports { get; set; }

    public virtual DbSet<Im_CheckRequest_RefuseReason> Im_CheckRequest_RefuseReasons { get; set; }

    public virtual DbSet<Im_CheckRequest_SampleData_Confirm> Im_CheckRequest_SampleData_Confirms { get; set; }

    public virtual DbSet<Im_CheckRequest_SampleDatum> Im_CheckRequest_SampleData { get; set; }

    public virtual DbSet<Im_CheckRequest_Visa> Im_CheckRequest_Visas { get; set; }

    public virtual DbSet<Im_CheckRequset_Shipping_Method> Im_CheckRequset_Shipping_Methods { get; set; }

    public virtual DbSet<Im_CommitteeCheckLocation> Im_CommitteeCheckLocations { get; set; }

    public virtual DbSet<Im_CommitteeResult> Im_CommitteeResults { get; set; }

    public virtual DbSet<Im_CommitteeResult_Confirm> Im_CommitteeResult_Confirms { get; set; }

    public virtual DbSet<Im_CommitteeResult_Infection> Im_CommitteeResult_Infections { get; set; }

    public virtual DbSet<Im_Committee_CustodyPlace> Im_Committee_CustodyPlaces { get; set; }

    public virtual DbSet<Im_Constrain_Initiator_Text> Im_Constrain_Initiator_Texts { get; set; }

    public virtual DbSet<Im_Constrain_Type> Im_Constrain_Types { get; set; }

    public virtual DbSet<Im_Constrains_Special> Im_Constrains_Specials { get; set; }

    public virtual DbSet<Im_CountryConstrain_ArrivalPort> Im_CountryConstrain_ArrivalPorts { get; set; }

    public virtual DbSet<Im_CountryConstrain_Text> Im_CountryConstrain_Texts { get; set; }

    public virtual DbSet<Im_CustodyPlace> Im_CustodyPlaces { get; set; }

    public virtual DbSet<Im_CustodyPlaceType> Im_CustodyPlaceTypes { get; set; }

    public virtual DbSet<Im_CustodyPlace_CheckRequest> Im_CustodyPlace_CheckRequests { get; set; }

    public virtual DbSet<Im_Execution> Im_Executions { get; set; }

    public virtual DbSet<Im_Execution_Item> Im_Execution_Items { get; set; }

    public virtual DbSet<Im_Final_Result> Im_Final_Results { get; set; }

    public virtual DbSet<Im_Initiator> Im_Initiators { get; set; }

    public virtual DbSet<Im_ItemsLotDivision> Im_ItemsLotDivisions { get; set; }

    public virtual DbSet<Im_Manafest> Im_Manafests { get; set; }

    public virtual DbSet<Im_OpertaionType> Im_OpertaionTypes { get; set; }

    public virtual DbSet<Im_PermissionItem> Im_PermissionItems { get; set; }

    public virtual DbSet<Im_PermissionItem_Division_Custody> Im_PermissionItem_Division_Custodies { get; set; }

    public virtual DbSet<Im_PermissionItem_Division_Custody_DismissCommittee> Im_PermissionItem_Division_Custody_DismissCommittees { get; set; }

    public virtual DbSet<Im_PermissionItem_Division_Custody_ReceiveCommittee> Im_PermissionItem_Division_Custody_ReceiveCommittees { get; set; }

    public virtual DbSet<Im_PermissionItems_Category> Im_PermissionItems_Categories { get; set; }

    public virtual DbSet<Im_PermissionRequest> Im_PermissionRequests { get; set; }

    public virtual DbSet<Im_PermissionRequest_History> Im_PermissionRequest_Histories { get; set; }

    public virtual DbSet<Im_PermissionRequest_RefuseReason> Im_PermissionRequest_RefuseReasons { get; set; }

    public virtual DbSet<Im_ProcedureType> Im_ProcedureTypes { get; set; }

    public virtual DbSet<Im_RequestCommittee> Im_RequestCommittees { get; set; }

    public virtual DbSet<Im_RequestCommittee_Procedure> Im_RequestCommittee_Procedures { get; set; }

    public virtual DbSet<Im_RequestCommittee_Shift> Im_RequestCommittee_Shifts { get; set; }

    public virtual DbSet<Im_RequestDat_Extra> Im_RequestDat_Extras { get; set; }

    public virtual DbSet<Im_RequestDatum> Im_RequestData { get; set; }

    public virtual DbSet<Im_Request_Port> Im_Request_Ports { get; set; }

    public virtual DbSet<Im_Request_TreatmentData_Confirm> Im_Request_TreatmentData_Confirms { get; set; }

    public virtual DbSet<Im_Request_TreatmentDatum> Im_Request_TreatmentData { get; set; }

    public virtual DbSet<Im_ScientificResearch> Im_ScientificResearches { get; set; }

    public virtual DbSet<Im_ScientificResearch_ItemPlant_Inseket_Lieble> Im_ScientificResearch_ItemPlant_Inseket_Liebles { get; set; }

    public virtual DbSet<Im_ScientificResearch_ItemPlant_Product> Im_ScientificResearch_ItemPlant_Products { get; set; }

    public virtual DbSet<Im_ScientificResearch_Organization> Im_ScientificResearch_Organizations { get; set; }

    public virtual DbSet<Im_ScientificResearch_Person> Im_ScientificResearch_People { get; set; }

    public virtual DbSet<Im_Store> Im_Stores { get; set; }

    public virtual DbSet<Im_SubDivission> Im_SubDivissions { get; set; }

    public virtual DbSet<Im_TransUnderCustodyReason> Im_TransUnderCustodyReasons { get; set; }

    public virtual DbSet<Im_Visa> Im_Visas { get; set; }

    public virtual DbSet<Im_Warehouse> Im_Warehouses { get; set; }

    public virtual DbSet<Im_choose_Constrain_> Im_choose_Constrain_s { get; set; }

    public virtual DbSet<InternationalTransportation> InternationalTransportations { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCategories_Group> ItemCategories_Groups { get; set; }

    public virtual DbSet<ItemCategories_Type> ItemCategories_Types { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<ItemPart> ItemParts { get; set; }

    public virtual DbSet<Item_Purpose> Item_Purposes { get; set; }

    public virtual DbSet<Item_ShortName> Item_ShortNames { get; set; }

    public virtual DbSet<Item_Status> Item_Statuses { get; set; }

    public virtual DbSet<Item_Type> Item_Types { get; set; }

    public virtual DbSet<Kingdom> Kingdoms { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<LiableItem> LiableItems { get; set; }

    public virtual DbSet<LiableItems_ShortName> LiableItems_ShortNames { get; set; }

    public virtual DbSet<LiableItems_Status> LiableItems_Statuses { get; set; }

    public virtual DbSet<MainCalssification> MainCalssifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Outlet> Outlets { get; set; }

    public virtual DbSet<Outlet_Employee> Outlet_Employees { get; set; }

    public virtual DbSet<Package_Material> Package_Materials { get; set; }

    public virtual DbSet<Package_Type> Package_Types { get; set; }

    public virtual DbSet<Pallet_Data_Ex_CheckRequest_Distribution> Pallet_Data_Ex_CheckRequest_Distributions { get; set; }

    public virtual DbSet<Pallet_Data_Organization__Distribution> Pallet_Data_Organization__Distributions { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PhylumSubphylum> PhylumSubphylums { get; set; }

    public virtual DbSet<PortNational> PortNationals { get; set; }

    public virtual DbSet<PortOrganization> PortOrganizations { get; set; }

    public virtual DbSet<Port_International> Port_Internationals { get; set; }

    public virtual DbSet<Port_Type> Port_Types { get; set; }

    public virtual DbSet<PublicOrganization_Type> PublicOrganization_Types { get; set; }

    public virtual DbSet<Public_Organization> Public_Organizations { get; set; }

    public virtual DbSet<QualitativeGroup> QualitativeGroups { get; set; }

    public virtual DbSet<Refuse_Reason> Refuse_Reasons { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Regional_Area> Regional_Areas { get; set; }

    public virtual DbSet<SecondaryClassification> SecondaryClassifications { get; set; }

    public virtual DbSet<ShiftTiming> ShiftTimings { get; set; }

    public virtual DbSet<Shipment_Mean> Shipment_Means { get; set; }

    public virtual DbSet<ShippingAgency> ShippingAgencies { get; set; }

    public virtual DbSet<ShippingCompany> ShippingCompanies { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<StationActivityType> StationActivityTypes { get; set; }

    public virtual DbSet<StationCompany> StationCompanies { get; set; }

    public virtual DbSet<StationContact> StationContacts { get; set; }

    public virtual DbSet<Station_Accreditation> Station_Accreditations { get; set; }

    public virtual DbSet<Station_Accreditation_CheckList> Station_Accreditation_CheckLists { get; set; }

    public virtual DbSet<Station_Accreditation_Committee> Station_Accreditation_Committees { get; set; }

    public virtual DbSet<Station_Accreditation_Committee_CheckList> Station_Accreditation_Committee_CheckLists { get; set; }

    public virtual DbSet<Station_Accreditation_Committee_CheckList_Confirm> Station_Accreditation_Committee_CheckList_Confirms { get; set; }

    public virtual DbSet<Station_Accreditation_Committee_Final_Result> Station_Accreditation_Committee_Final_Results { get; set; }

    public virtual DbSet<Station_Accreditation_Committee_Imge> Station_Accreditation_Committee_Imges { get; set; }

    public virtual DbSet<Station_Accreditation_Committee_Shift> Station_Accreditation_Committee_Shifts { get; set; }

    public virtual DbSet<Station_Accreditation_Data_Country> Station_Accreditation_Data_Countries { get; set; }

    public virtual DbSet<Station_Accreditation_Data_Item_ShortName> Station_Accreditation_Data_Item_ShortNames { get; set; }

    public virtual DbSet<Station_Accreditation_Datum> Station_Accreditation_Data { get; set; }

    public virtual DbSet<Station_Accreditation_Request> Station_Accreditation_Requests { get; set; }

    public virtual DbSet<Station_Accreditation_Request_Fee> Station_Accreditation_Request_Fees { get; set; }

    public virtual DbSet<Station_Accreditation_Request_Fees_ENG> Station_Accreditation_Request_Fees_ENGs { get; set; }

    public virtual DbSet<Station_Accreditation_Request_Type> Station_Accreditation_Request_Types { get; set; }

    public virtual DbSet<Station_CheckList> Station_CheckLists { get; set; }

    public virtual DbSet<Station_Committee> Station_Committees { get; set; }

    public virtual DbSet<Station_Constrain_Country_Item> Station_Constrain_Country_Items { get; set; }

    public virtual DbSet<Station_Constrain_Type> Station_Constrain_Types { get; set; }

    public virtual DbSet<Station_Datum> Station_Data { get; set; }

    public virtual DbSet<Station_Emp> Station_Emps { get; set; }

    public virtual DbSet<Station_Fees_Type> Station_Fees_Types { get; set; }

    public virtual DbSet<Station_List> Station_Lists { get; set; }

    public virtual DbSet<Station_Managing_Director> Station_Managing_Directors { get; set; }

    public virtual DbSet<Station_Requst> Station_Requsts { get; set; }

    public virtual DbSet<SteamingCompany> SteamingCompanies { get; set; }

    public virtual DbSet<SubPart> SubParts { get; set; }

    public virtual DbSet<SubPart_Type> SubPart_Types { get; set; }

    public virtual DbSet<Table_Action> Table_Actions { get; set; }

    public virtual DbSet<Table_Action_Log> Table_Action_Logs { get; set; }

    public virtual DbSet<Table_Action_Log_CheckRequest> Table_Action_Log_CheckRequests { get; set; }

    public virtual DbSet<Table_Action_Log_EX> Table_Action_Log_EXes { get; set; }

    public virtual DbSet<Table_Action_Log_Farm> Table_Action_Log_Farms { get; set; }

    public virtual DbSet<Table_Action_Log_Station> Table_Action_Log_Stations { get; set; }

    public virtual DbSet<Transport_Mean> Transport_Means { get; set; }

    public virtual DbSet<TreatmentMainType> TreatmentMainTypes { get; set; }

    public virtual DbSet<TreatmentMaterial> TreatmentMaterials { get; set; }

    public virtual DbSet<TreatmentMethod> TreatmentMethods { get; set; }

    public virtual DbSet<TreatmentType> TreatmentTypes { get; set; }

    public virtual DbSet<USER_TYPE> USER_TYPEs { get; set; }

    public virtual DbSet<Union> Unions { get; set; }

    public virtual DbSet<Union_Country> Union_Countries { get; set; }

    public virtual DbSet<UnitType> UnitTypes { get; set; }

    public virtual DbSet<View_List_Im_PermissionRequest> View_List_Im_PermissionRequests { get; set; }

    public virtual DbSet<Village> Villages { get; set; }

    public virtual DbSet<WebsiteTypeDetail> WebsiteTypeDetails { get; set; }

    public virtual DbSet<Websitetype> Websitetypes { get; set; }

    public virtual DbSet<pos_information> pos_informations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration =
                new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBConnection"));
        }
        catch
        {
            //ignore
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<A_AttachmentData_Ex_CheckRequest>(entity =>
        {
            entity.ToTable("A_AttachmentData_Ex_CheckRequest");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttachmentPath_Binary)
                .HasMaxLength(7000)
                .IsFixedLength();
            entity.Property(e => e.Attachment_TypeName).HasComment("نوع المرفق");
            entity.Property(e => e.EndDate)
                .HasComment("تاريخ النهاية")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.StartDate)
                .HasComment("تاريخ البداية")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("null-> for user , value -> if the admin add the row");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.A_AttachmentTableName).WithMany(p => p.A_AttachmentData_Ex_CheckRequests)
                .HasForeignKey(d => d.A_AttachmentTableNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_A_AttachmentData_Ex_CheckRequest_A_AttachmentTableName");

            entity.HasOne(d => d.A_AttachmentTableType).WithMany(p => p.A_AttachmentData_Ex_CheckRequests)
                .HasForeignKey(d => d.A_AttachmentTableType_ID)
                .HasConstraintName("FK_A_AttachmentData_Ex_CheckRequest_A_AttachmentTableType");
        });

        modelBuilder.Entity<A_AttachmentData_Ex_CommitteeResult_Infection>(entity =>
        {
            entity.ToTable("A_AttachmentData_Ex_CommitteeResult_Infection");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CommitteeResult).WithMany(p => p.A_AttachmentData_Ex_CommitteeResult_Infections)
                .HasForeignKey(d => d.Ex_CommitteeResult_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_A_AttachmentData_Ex_CommitteeResult_Infection_Ex_CommitteeResult");
        });

        modelBuilder.Entity<A_AttachmentData_Im_CommitteeResult_Infection>(entity =>
        {
            entity.ToTable("A_AttachmentData_Im_CommitteeResult_Infection", tb => tb.HasComment("صور فحص الاصابه"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Infection_Comment).HasComment("نوع المرفق");
            entity.Property(e => e.User_Creation_Date)
                .HasComment("null-> for user , value -> if the admin add the row")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("null-> for user , value -> if the admin add the row");

            entity.HasOne(d => d.Im_CommitteeResult).WithMany(p => p.A_AttachmentData_Im_CommitteeResult_Infections)
                .HasForeignKey(d => d.Im_CommitteeResult_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_A_AttachmentData_Im_CommitteeResult_Infection_Im_CommitteeResult");
        });

        modelBuilder.Entity<A_AttachmentData_Station>(entity =>
        {
            entity.ToTable("A_AttachmentData_Station", tb => tb.HasComment("اسم الجدول"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.A_AttachmentTableNameId).HasComment("اسم الجدول");
            entity.Property(e => e.A_AttachmentTableType_ID).HasComment("نوع المرفق");
            entity.Property(e => e.AttachmentPath).HasComment("مسار المرفق");
            entity.Property(e => e.AttachmentPath_Binary)
                .HasMaxLength(7000)
                .IsFixedLength();
            entity.Property(e => e.Attachment_Number).HasComment("رقم المرفق");
            entity.Property(e => e.Attachment_TypeName).HasComment("اسم المرفق");
            entity.Property(e => e.EndDate)
                .HasComment("تاريخ النهاية")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.RowId).HasComment("الرقم داخل الجدول");
            entity.Property(e => e.StartDate)
                .HasComment("تاريخ البداية")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date)
                .HasComment("null-> for user , value -> if the admin add the row")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("null-> for user , value -> if the admin add the row");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.A_AttachmentTableName).WithMany(p => p.A_AttachmentData_Stations)
                .HasForeignKey(d => d.A_AttachmentTableNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_A_AttachmentData_Station_A_AttachmentTableName");

            entity.HasOne(d => d.A_AttachmentTableType).WithMany(p => p.A_AttachmentData_Stations)
                .HasForeignKey(d => d.A_AttachmentTableType_ID)
                .HasConstraintName("FK_A_AttachmentData_Station_A_AttachmentTableType");
        });

        modelBuilder.Entity<A_AttachmentDatum>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("باص الملفات"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttachmentPath_Binary)
                .HasMaxLength(7000)
                .IsFixedLength();
            entity.Property(e => e.Attachment_TypeName).HasComment("نوع المرفق");
            entity.Property(e => e.EndDate)
                .HasComment("تاريخ النهاية")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.StartDate)
                .HasComment("تاريخ البداية")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date)
                .HasComment("null-> for user , value -> if the admin add the row")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("null-> for user , value -> if the admin add the row");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.A_AttachmentTableName).WithMany(p => p.A_AttachmentData)
                .HasForeignKey(d => d.A_AttachmentTableNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_A_AttachmentData_A_AttachmentTableName");

            entity.HasOne(d => d.A_AttachmentTableType).WithMany(p => p.A_AttachmentData)
                .HasForeignKey(d => d.A_AttachmentTableType_ID)
                .HasConstraintName("FK_A_AttachmentData_A_AttachmentTableType");
        });

        modelBuilder.Entity<A_AttachmentDatum1>(entity =>
        {
            entity.ToTable("A_AttachmentData", "rejection");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttachmentPath_Binary)
                .HasMaxLength(7000)
                .IsFixedLength();
            entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<A_AttachmentTableName>(entity =>
        {
            entity.ToTable("A_AttachmentTableName", tb => tb.HasComment("اسماء الجداول فى قواعد البيانات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.TableName).HasMaxLength(100);
        });

        modelBuilder.Entity<A_AttachmentTableType>(entity =>
        {
            entity.ToTable("A_AttachmentTableType", tb => tb.HasComment("نوع المرفق"));

            entity.Property(e => e.ID).ValueGeneratedNever();
        });

        modelBuilder.Entity<A_SystemCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SystemCode");

            entity.ToTable("A_SystemCode");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ValueName).HasMaxLength(150);
            entity.Property(e => e.ValueNameEN).HasMaxLength(150);

            entity.HasOne(d => d.SystemCodeType).WithMany(p => p.A_SystemCodes)
                .HasForeignKey(d => d.SystemCodeTypeId)
                .HasConstraintName("FK_SystemCode_SystemCodeType");
        });

        modelBuilder.Entity<A_SystemCodeType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SystemCodeType");

            entity.ToTable("A_SystemCodeType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.TableName).HasMaxLength(100);
        });

        modelBuilder.Entity<A__User_Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_A_User_Login");

            entity.ToTable("A__User_Login");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccessToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LogOut_Date).HasColumnType("datetime");
            entity.Property(e => e.Login_Date).HasColumnType("datetime");
        });

        modelBuilder.Entity<A__plant_Error_Save>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_plant_Error_Save");

            entity.ToTable("A__plant_Error_Save");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage).IsUnicode(false);
            entity.Property(e => e.FunctionName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.IsWeb).HasComment("1->web, 0->android");
            entity.Property(e => e.PageName).IsUnicode(false);
            entity.Property(e => e.User_Ip)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AnalysisLab>(entity =>
        {
            entity.ToTable("AnalysisLab", tb => tb.HasComment("معامل التحاليل"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Addrees_En).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fax).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasColumnType("numeric(11, 0)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AnalysisLabType>(entity =>
        {
            entity.ToTable("AnalysisLabType", tb => tb.HasComment("جدول ربط التحاليل بالمعامل"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.AnalysisLab).WithMany(p => p.AnalysisLabTypes)
                .HasForeignKey(d => d.AnalysisLabID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnalysisLabType_AnalysisLab");

            entity.HasOne(d => d.AnalysisType).WithMany(p => p.AnalysisLabTypes)
                .HasForeignKey(d => d.AnalysisTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnalysisLabType_AnalysisType");
        });

        modelBuilder.Entity<AnalysisType>(entity =>
        {
            entity.ToTable("AnalysisType", tb => tb.HasComment("التحاليل التي تتم على العينة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsRejectedAll).HasComment("هل يرفض الطلب كله في حالة وجود إصابة");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Andriod_Location>(entity =>
        {
            entity.ToTable("Andriod_Location");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsExport)
                .HasDefaultValue(true)
                .HasComment("1->Export , 0->Import");
            entity.Property(e => e.Operation_ID).HasComment("the action that user made 'Export, Treatment, Sample Data, ....");

            entity.HasOne(d => d.Operation).WithMany(p => p.Andriod_Locations)
                .HasForeignKey(d => d.Operation_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Andriod_Location_Andriod_Operation");
        });

        modelBuilder.Entity<Andriod_Operation>(entity =>
        {
            entity.ToTable("Andriod_Operation");

            entity.Property(e => e.Operation_Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Biological_Phase>(entity =>
        {
            entity.ToTable("Biological_Phase", tb => tb.HasComment("الطور الحيوي"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Center>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_LK_Center");

            entity.ToTable("Center", tb => tb.HasComment("المراكز"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.Govern_ID).HasComment("المحافظة");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مفعل");
            entity.Property(e => e.Outlet_ID).HasComment("المنافذ");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Govern).WithMany(p => p.Centers)
                .HasForeignKey(d => d.Govern_ID)
                .HasConstraintName("FK_Center_Governate");

            entity.HasOne(d => d.Outlet).WithMany(p => p.Centers)
                .HasForeignKey(d => d.Outlet_ID)
                .HasConstraintName("FK_Center_Outlet");
        });

        modelBuilder.Entity<CommitteeEmployee>(entity =>
        {
            entity.HasKey(e => new { e.Committee_ID, e.Employee_Id, e.OperationType });

            entity.ToTable("CommitteeEmployee", tb => tb.HasComment("أعضاء اللجنة"));

            entity.Property(e => e.OperationType).HasComment("from system code 20 (export :73 , Import 74, Farm 78) ");
            entity.Property(e => e.ISAdmin).HasComment("is admin for the current committee");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.OperationTypeNavigation).WithMany(p => p.CommitteeEmployees)
                .HasForeignKey(d => d.OperationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommitteeEmployee_A_SystemCode");
        });

        modelBuilder.Entity<CommitteeResultType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CommitteeResult");

            entity.ToTable("CommitteeResultType", tb => tb.HasComment("مرفوض/ مقبول .........."));

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar).HasMaxLength(20);
            entity.Property(e => e.Name_En)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<CommitteeType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CommitteePurpose");

            entity.ToTable("CommitteeType", tb => tb.HasComment("الغرض من اللجنة"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.type_Id).HasDefaultValue(93);

            entity.HasOne(d => d.type).WithMany(p => p.CommitteeTypes)
                .HasForeignKey(d => d.type_Id)
                .HasConstraintName("FK__Committee__type___34D49220");
        });

        modelBuilder.Entity<CompanyAccreditation>(entity =>
        {
            entity.ToTable("CompanyAccreditation", tb => tb.HasComment("اعتمادات الشركة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Company_ID).HasComment("الشركة");
            entity.Property(e => e.Country_ID).HasComment("الدولة");
            entity.Property(e => e.EndDate).HasComment("تاريخ نهاية الاعتماد");
            entity.Property(e => e.IsApproved).HasComment("null->ask for accredation\r\n0->not accepted\r\n1->Accepted");
            entity.Property(e => e.Item_ShortName_id).HasComment("product or plant ID manual no relation");
            entity.Property(e => e.StartDate).HasComment("تاريخ بداية الاعتماد");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyAccreditations)
                .HasForeignKey(d => d.Company_ID)
                .HasConstraintName("FK_CompanyAccreditation_Companies");

            entity.HasOne(d => d.Country).WithMany(p => p.CompanyAccreditations)
                .HasForeignKey(d => d.Country_ID)
                .HasConstraintName("FK_CompanyAccreditation_Country");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.CompanyAccreditations)
                .HasForeignKey(d => d.Item_ShortName_id)
                .HasConstraintName("FK_CompanyAccreditation_Item_ShortName");
        });

        modelBuilder.Entity<CompanyAccreditation_Committee>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Company_Committee");

            entity.ToTable("CompanyAccreditation_Committee");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount_Total)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Check_Date).HasComment("تاريخ الانتداب");
            entity.Property(e => e.Company_Accreditation_ID).HasComment("طلب الفحص");
            entity.Property(e => e.Delegation_Date).HasComment("تاريخ الفحص");
            entity.Property(e => e.EndTime).HasComment("انتهاء ساعة الفحص");
            entity.Property(e => e.IsApproved).HasComment("0 if exporter doesn't accept else 1");
            entity.Property(e => e.IsPaid).HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.StartTime).HasComment(" بداية ساعة الفحص ");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasComment("null->No committe ,0 if not done, 1 if investigation is done");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CommitteeType).WithMany(p => p.CompanyAccreditation_Committees)
                .HasForeignKey(d => d.CommitteeType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_Accreditation_Committee_CommitteeType");

            entity.HasOne(d => d.Company_Accreditation).WithMany(p => p.CompanyAccreditation_Committees)
                .HasForeignKey(d => d.Company_Accreditation_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_Accreditation_Committee_CompanyAccreditation");
        });

        modelBuilder.Entity<CompanyAccreditation_Payment>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Company_Payment");

            entity.ToTable("CompanyAccreditation_Payment");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.IS_OnlineOffline).HasComment("from web/system\r\n1->online\r\n0->offline");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Company_Committee).WithMany(p => p.CompanyAccreditation_Payments)
                .HasForeignKey(d => d.Company_Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_Payment_Company_Committee");
        });

        modelBuilder.Entity<CompanyActivity>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CompanyActivities");

            entity.ToTable("CompanyActivity", tb => tb.HasComment("نشاط الشركة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.CompActivityType_ID).HasComment("نوع النشاط");
            entity.Property(e => e.Company_ID).HasComment("الشركة");
            entity.Property(e => e.Enrollment_End).HasComment("تاريخ نهاية");
            entity.Property(e => e.Enrollment_Name).HasMaxLength(100);
            entity.Property(e => e.Enrollment_Number).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Enrollment_Start).HasComment("تاريخ بداية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MainActivityType).HasComment("نوع نشاط الرئيسى from systemcode 17");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CompActivityType).WithMany(p => p.CompanyActivities)
                .HasForeignKey(d => d.CompActivityType_ID)
                .HasConstraintName("FK_CompanyActivity_CompanyActivityType");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyActivities)
                .HasForeignKey(d => d.Company_ID)
                .HasConstraintName("FK_CompanyActivity_Company");

            entity.HasOne(d => d.Enrollment_type).WithMany(p => p.CompanyActivities)
                .HasForeignKey(d => d.Enrollment_type_ID)
                .HasConstraintName("FK_CompanyActivity_Enrollment_type");

            entity.HasOne(d => d.MainActivityTypeNavigation).WithMany(p => p.CompanyActivities)
                .HasForeignKey(d => d.MainActivityType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyActivity_A_SystemCode");
        });

        modelBuilder.Entity<CompanyActivity1>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CompanyActivities");

            entity.ToTable("CompanyActivity", "rejection");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Enrollment_Name).HasMaxLength(100);
            entity.Property(e => e.Enrollment_Number).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyActivity1s)
                .HasForeignKey(d => d.Company_ID)
                .HasConstraintName("FK_CompanyActivity_Company");
        });

        modelBuilder.Entity<CompanyActivityType>(entity =>
        {
            entity.ToTable("CompanyActivityType", tb => tb.HasComment("نوع نشاط الشركة"));

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(50)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Company_National>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Companies");

            entity.ToTable("Company_National", tb => tb.HasComment("شركة محلية"));

            entity.HasIndex(e => e.ID, "_dta_index_Company_National_15_1931258035__K1_2_3");

            entity.HasIndex(e => new { e.TaxesRecord, e.CommertialRecord }, "uk_Company_National").IsUnique();

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_Ar).HasComment("العنوان بالعربية");
            entity.Property(e => e.Address_En)
                .IsUnicode(false)
                .HasComment("العنوان بالانجليزية");
            entity.Property(e => e.CommertialRecord)
                .HasMaxLength(200)
                .HasComment("السجل التجاري");
            entity.Property(e => e.IS_OnlineOffline)
                .HasDefaultValue(false)
                .HasComment("from web/system\r\n1->online\r\n0->offline");
            entity.Property(e => e.IsApproved).HasComment("null->Company request \r\n0->not accepted\r\n1->Aceecpted\r\n ");
            entity.Property(e => e.IsTreatment).HasComment("it was deleted and i returned it again fz 8-9-2019");
            entity.Property(e => e.Name_Ar).HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.Owner_Ar).HasComment("الاسم بالعربية");
            entity.Property(e => e.Owner_En)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.TaxesRecord)
                .HasMaxLength(200)
                .HasComment("السجل الضريبي");
            entity.Property(e => e.User_Activation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Center).WithMany(p => p.Company_Nationals)
                .HasForeignKey(d => d.Center_ID)
                .HasConstraintName("FK_Company_National_Center");

            entity.HasOne(d => d.Village).WithMany(p => p.Company_Nationals)
                .HasForeignKey(d => d.Village_ID)
                .HasConstraintName("FK_Company_National_Village");
        });

        modelBuilder.Entity<Company_National1>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Companies");

            entity.ToTable("Company_National", "rejection");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_En).IsUnicode(false);
            entity.Property(e => e.CommertialRecord).HasMaxLength(200);
            entity.Property(e => e.Name_En).IsUnicode(false);
            entity.Property(e => e.Owner_En).IsUnicode(false);
            entity.Property(e => e.TaxesRecord).HasMaxLength(200);
            entity.Property(e => e.User_Activation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ContactType>(entity =>
        {
            entity.ToTable("ContactType", tb => tb.HasComment("نوع بيان الاتصال"));

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(50)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Continent>(entity =>
        {
            entity.Property(e => e.Descreption_En).IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("قارة");
            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En).HasMaxLength(200);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_LK_Country");

            entity.ToTable("Country", tb => tb.HasComment("الدول"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Is_IPPC).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Continents).WithMany(p => p.Countries)
                .HasForeignKey(d => d.Continents_ID)
                .HasConstraintName("FK_Country_Continents");

            entity.HasOne(d => d.Regional_Area).WithMany(p => p.Countries)
                .HasForeignKey(d => d.Regional_Area_ID)
                .HasConstraintName("FK_Country_Regional_Area");
        });

        modelBuilder.Entity<EX>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("EX");

            entity.Property(e => e.CheckRequest_Number)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Expr2).HasColumnType("smalldatetime");
            entity.Property(e => e.ShortName_En).IsUnicode(false);
        });

        modelBuilder.Entity<EX_Choose_SampleDatum>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();

            entity.HasOne(d => d.AnalysisLabType).WithMany(p => p.EX_Choose_SampleData)
                .HasForeignKey(d => d.AnalysisLabType_ID)
                .HasConstraintName("FK_EX_Choose_SampleData_AnalysisLabType");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.EX_Choose_SampleData)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_EX_Choose_SampleData_Ex_CheckRequest");

            entity.HasOne(d => d.Ex_CountryConstrain_AnalysisLabType).WithMany(p => p.EX_Choose_SampleData)
                .HasForeignKey(d => d.Ex_CountryConstrain_AnalysisLabType_ID)
                .HasConstraintName("FK_EX_Choose_SampleData_Ex_CountryConstrain_AnalysisLabType");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.EX_Choose_SampleData)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .HasConstraintName("FK_EX_Choose_SampleData_Item_ShortName");
        });

        modelBuilder.Entity<EX_Choose_Treatment>(entity =>
        {
            entity.ToTable("EX_Choose_Treatment", tb => tb.HasComment("اختيار المعالجه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IS_Optional).HasComment("التحليل اختيارى =0 \r\nالتحليل اجباري =1");
            entity.Property(e => e.Item_ShortName_id).HasComment("product or plant ID manual no relation");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.EX_Choose_Treatments)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_EX_Choose_Treatment_Ex_CheckRequest");

            entity.HasOne(d => d.Ex_CountryConstrain_Treatment).WithMany(p => p.EX_Choose_Treatments)
                .HasForeignKey(d => d.Ex_CountryConstrain_Treatment_id)
                .HasConstraintName("FK_EX_Choose_Treatment_Ex_CountryConstrain_Treatment");

            entity.HasOne(d => d.TreatmentMethods).WithMany(p => p.EX_Choose_Treatments)
                .HasForeignKey(d => d.TreatmentMethods_ID)
                .HasConstraintName("FK_EX_Choose_Treatment_TreatmentMethods");
        });

        modelBuilder.Entity<EX_Constrain_Country_Item>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_EX_Constrain_Type");

            entity.ToTable("EX_Constrain_Country_Item", tb => tb.HasComment("انواع الاشتراطات"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مفعل");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.EX_Constrain_Type).WithMany(p => p.EX_Constrain_Country_Items)
                .HasForeignKey(d => d.EX_Constrain_Type_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EX_Constrain_Type_EX_Constrain");
        });

        modelBuilder.Entity<EX_Constrain_Text>(entity =>
        {
            entity.ToTable("EX_Constrain_Text");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ConstrainText_En).IsUnicode(false);
            entity.Property(e => e.InSide_Certificate_En).IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.IsCertificate_Addtion).HasComment("شهادة الصحة النباتية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.EX_Constrain_Country_Item).WithMany(p => p.EX_Constrain_Texts)
                .HasForeignKey(d => d.EX_Constrain_Country_Item_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EX_Constrain_Text_EX_Constrain_Type");
        });

        modelBuilder.Entity<EX_Constrain_Type>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_EX_Constrain");

            entity.ToTable("EX_Constrain_Type", tb => tb.HasComment(" الاشتراطات"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مفعل");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<EX_Fees_Type>(entity =>
        {
            entity.ToTable("EX_Fees_Type");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Account_Type).HasComment("نوع الحساب من system code رقم 33");
            entity.Property(e => e.Fees_Action_ID).HasComment("نوع الوردية وقيمتها");
            entity.Property(e => e.Fees_Type).HasComment("صادر وارد ورية مهندس\r\nرقم 20\r\nفى system code\r\n");
            entity.Property(e => e.Value).HasColumnType("money");
        });

        modelBuilder.Entity<Enrollment_type>(entity =>
        {
            entity.ToTable("Enrollment_type");

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مفعل");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Ex_CertificateAddtion>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantCertificateAddtion");

            entity.ToTable("Ex_CertificateAddtion", tb => tb.HasComment("اضافات  الاشتراطات للشهادة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.AdminID).HasComment("رقم موظف الحجر");
            entity.Property(e => e.Certificate_AddtionOriginal).HasComment("نص الاشتراط الاصلى");
            entity.Property(e => e.Certificate_AddtionOriginalUpdate).HasComment("نص الاشتراطات بعد التعديل");
            entity.Property(e => e.Certificate_AddtionUpdateAdmin).HasComment("الاشتراط للحجر بعد التعديل");
            entity.Property(e => e.ConstrainID).HasComment("رقم الاشتراط الاصلى");
            entity.Property(e => e.Date_Accepted).HasColumnType("datetime");
            entity.Property(e => e.ISAccepted).HasDefaultValue(false);

            entity.HasOne(d => d.PlantCertificatesRequests).WithMany(p => p.Ex_CertificateAddtions)
                .HasForeignKey(d => d.PlantCertificatesRequestsID)
                .HasConstraintName("FK_PlantCertificateAddtion_PlantCertificatesRequests");
        });

        modelBuilder.Entity<Ex_CertificateAddtionUser>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantCertificateAddtionUser");

            entity.ToTable("Ex_CertificateAddtionUser", tb => tb.HasComment("الشروط الاضافية الجديدة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IS_Client_OR_Agree).HasComment("اضافه العميل 1 او رد الحجر 0\r\n");

            entity.HasOne(d => d.PlantCertificatesRequests).WithMany(p => p.Ex_CertificateAddtionUsers)
                .HasForeignKey(d => d.PlantCertificatesRequestsID)
                .HasConstraintName("FK_PlantCertificateAddtionUser_PlantCertificatesRequests");
        });

        modelBuilder.Entity<Ex_CertificatesNewCountry>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantCertificatesNewArrivePort");

            entity.ToTable("Ex_CertificatesNewCountry", tb => tb.HasComment("طلب تغير وجهه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CertificatesNewCountries)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_PlantCertificatesNewCountry_Ex_CheckRequest");
        });

        modelBuilder.Entity<Ex_CertificatesRequest>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantCertificatesRequests");

            entity.ToTable(tb => tb.HasComment("بيانات الشهادة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ISAccepted).HasDefaultValue(false);
            entity.Property(e => e.ISEnglish).HasComment("لغة الشهادة");
            entity.Property(e => e.IS_Containers).HasComment("حاويات");
            entity.Property(e => e.IS_Lot).HasComment("لوط");
            entity.Property(e => e.IS_Treatment).HasComment("معالجه");
            entity.Property(e => e.ImporeterCompanyAddress_AR)
                .HasComment("عنوان الشركة المستوردة باللغه العربية")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImporeterCompanyAddress_EN)
                .HasComment("عنوان الشركة المستوردة باللغه الأنجيلزية")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImportCompany_Name_AR)
                .HasComment("أسم الشركة المستوردة باللغه العربية")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImportCompany_Name_EN)
                .HasComment("أسم الشركة المستوردة باللغه الأنجيلزية")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Importer_Id).HasComment("رقم الشركة");
            entity.Property(e => e.Importing_Country).HasComment("الدولة المستوردة");
            entity.Property(e => e.InternationalTransportation_ID).HasComment("وسـائل النقل الدولية");
            entity.Property(e => e.Port_Access).HasComment("ميناء الوصول");
            entity.Property(e => e.Port_Type_Importing_Country).HasComment("نوع الميناء الدوله المستورده");
            entity.Property(e => e.Port_Type_Transit_Country).HasComment("نوع الميناء دوله العبور");
            entity.Property(e => e.Ship_Name)
                .HasMaxLength(50)
                .HasComment("تفاصيل أو رقم الرحلة")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ShipmentmeanID).HasComment(" وسيلة الشحن");
            entity.Property(e => e.ShippingPolicyNumber).HasComment("رقم بوليصة الشحن");
            entity.Property(e => e.Shipping_Port).HasComment("ميناء الشحن");
            entity.Property(e => e.Transit_Country).HasComment("دولة العبور");
            entity.Property(e => e.Transit_Port).HasComment("ميناء العبور");
            entity.Property(e => e.Transport_Mean_Id).HasComment("طــريقة النقل");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CertificatesRequests)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_PlantCertificatesRequests_Ex_CheckRequest");
        });

        modelBuilder.Entity<Ex_CertificatesRequestsFile>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantCertificatesRequestsFiles");

            entity.ToTable(tb => tb.HasComment("المرفقات الخاصة بالشهادة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.A_AttachmentTableType).WithMany(p => p.Ex_CertificatesRequestsFiles)
                .HasForeignKey(d => d.A_AttachmentTableTypeID)
                .HasConstraintName("FK_PlantCertificatesRequestsFiles_A_AttachmentTableType");

            entity.HasOne(d => d.PlantCertificatesRequests).WithMany(p => p.Ex_CertificatesRequestsFiles)
                .HasForeignKey(d => d.PlantCertificatesRequestsID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlantCertificatesRequestsFiles_PlantCertificatesRequests");
        });

        modelBuilder.Entity<Ex_CertificatesRequestsLotDatum>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantCertificatesRequestsLotData");

            entity.ToTable(tb => tb.HasComment("بيانات اللوطات"));

            entity.Property(e => e.ID).ValueGeneratedNever();

            entity.HasOne(d => d.Ex_CheckRequset_Shipping_Method).WithMany(p => p.Ex_CertificatesRequestsLotData)
                .HasForeignKey(d => d.Ex_CheckRequset_Shipping_MethodID)
                .HasConstraintName("FK_Ex_CertificatesRequestsLotData_Ex_CheckRequset_Shipping_Method2");

            entity.HasOne(d => d.PlantCertificatesRequests).WithMany(p => p.Ex_CertificatesRequestsLotData)
                .HasForeignKey(d => d.PlantCertificatesRequestsID)
                .HasConstraintName("FK_PlantCertificatesRequestsLotData_PlantCertificatesRequests");
        });

        modelBuilder.Entity<Ex_CertificatesRequestsPayment>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantCertificatesRequestsPayments");

            entity.ToTable(tb => tb.HasComment("تحصيل الرسوم الخاصه بالشهاده"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CertificatesRequestsPaymentsTypeNavigation).WithMany(p => p.Ex_CertificatesRequestsPayments)
                .HasForeignKey(d => d.Ex_CertificatesRequestsPaymentsType)
                .HasConstraintName("FK_Ex_CertificatesRequestsPayments_Ex_CertificatesRequestsPaymentsType");

            entity.HasOne(d => d.PlantCertificatesRequests).WithMany(p => p.Ex_CertificatesRequestsPayments)
                .HasForeignKey(d => d.PlantCertificatesRequestsID)
                .HasConstraintName("FK_PlantCertificatesRequestsPayments_PlantCertificatesRequests");
        });

        modelBuilder.Entity<Ex_CertificatesRequestsPaymentsDetaile>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Code_Bank)
                .HasMaxLength(50)
                .HasComment("كود العملية من البنك");
            entity.Property(e => e.IsSuccess_Bank).HasComment("0 تم رفض عملية البنك\r\n1 تم قبول العملية \r\nnull تم الارسال ولم الرد من البنك");
            entity.Property(e => e.OrderNumber).HasMaxLength(14);
            entity.Property(e => e.Payment_Type_ID).HasComment("from systemcode table 30\r\nنوع عملية الدفع فيزا - كاش");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_ID).HasComment("from systemcode table 3\r\nنوع الموظف حجر ولا شركة ولا فرد ولاهيئه");

            entity.HasOne(d => d.Ex_CertificatesRequestsPayments).WithMany(p => p.Ex_CertificatesRequestsPaymentsDetailes)
                .HasForeignKey(d => d.Ex_CertificatesRequestsPaymentsID)
                .HasConstraintName("FK_Ex_CertificatesRequestsPaymentsDetailes_Ex_CertificatesRequestsPayments");
        });

        modelBuilder.Entity<Ex_CertificatesRequestsPaymentsType>(entity =>
        {
            entity.ToTable("Ex_CertificatesRequestsPaymentsType");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Ex_CheckRequest>(entity =>
        {
            entity.ToTable("Ex_CheckRequest");

            entity.HasIndex(e => e.CheckRequest_Number, "UQ_EX_CheckRequest_Number").IsUnique();

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.CheckRequest_Number)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExportCompany).HasMaxLength(250);
            entity.Property(e => e.IsAccepted_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.NationalIDCompanyOwner).HasMaxLength(14);
            entity.Property(e => e.Notes_Reject).HasComment("اسباب الرفض");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_ID).HasComment("from A_SystemCode =3");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Outlet).WithMany(p => p.Ex_CheckRequests)
                .HasForeignKey(d => d.Outlet_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Outlet");
        });

        modelBuilder.Entity<Ex_CheckRequestData_Extra>(entity =>
        {
            entity.ToTable("Ex_CheckRequestData_Extra");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ImporeterCompanyAddress).HasMaxLength(100);
            entity.Property(e => e.ImporeterCompanyAddress_EN).HasMaxLength(100);
            entity.Property(e => e.ImportCompany).HasMaxLength(50);
            entity.Property(e => e.ImportCompany_EN).HasMaxLength(50);
            entity.Property(e => e.OwnerAddress).HasMaxLength(100);
            entity.Property(e => e.OwnerName).HasMaxLength(50);
            entity.Property(e => e.Reciever_Name).HasMaxLength(100);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest_Data).WithMany(p => p.Ex_CheckRequestData_Extras)
                .HasForeignKey(d => d.Ex_CheckRequest_Data_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequestData_Extra_Ex_CheckRequest_Data1");
        });

        modelBuilder.Entity<Ex_CheckRequest_Customs_Message>(entity =>
        {
            entity.ToTable("Ex_CheckRequest_Customs_Message");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Certificate_Number_Each_Product).HasMaxLength(50);
            entity.Property(e => e.Customs_Certificate_Number).HasMaxLength(50);
            entity.Property(e => e.Manifest_Number).HasMaxLength(50);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CertificatesRequests).WithMany(p => p.Ex_CheckRequest_Customs_Messages)
                .HasForeignKey(d => d.Ex_CertificatesRequests_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Customs_Message_Ex_CertificatesRequests");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Customs_Messages)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Customs_Message_Ex_CheckRequest1");

            entity.HasOne(d => d.Shipping_Agency).WithMany(p => p.Ex_CheckRequest_Customs_Messages)
                .HasForeignKey(d => d.Shipping_Agency_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Customs_Message_ShippingAgencies");
        });

        modelBuilder.Entity<Ex_CheckRequest_Datum>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.DelegateAddress)
                .HasMaxLength(100)
                .HasComment("عنوان مندوب صاحب الرسالة");
            entity.Property(e => e.DelegateName)
                .HasMaxLength(50)
                .HasComment("مندوب صاحب الرسالة");
            entity.Property(e => e.NationalIDCompanyOwner)
                .HasMaxLength(14)
                .HasComment("الرقم القومي لمندوب صاحب الرسالة");
            entity.Property(e => e.Ship_Name).HasMaxLength(50);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Data)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Data_Ex_CheckRequest1");

            entity.HasOne(d => d.ExportCountry).WithMany(p => p.Ex_CheckRequest_Data)
                .HasForeignKey(d => d.ExportCountry_Id)
                .HasConstraintName("FK_Ex_CheckRequest_Data_Country");

            entity.HasOne(d => d.InternationalTransportation).WithMany(p => p.Ex_CheckRequest_Data)
                .HasForeignKey(d => d.InternationalTransportation_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Data_InternationalTransportation");

            entity.HasOne(d => d.Shipment_Mean).WithMany(p => p.Ex_CheckRequest_Data)
                .HasForeignKey(d => d.Shipment_Mean_Id)
                .HasConstraintName("FK_Ex_CheckRequest_Data_Shipment_Mean");

            entity.HasOne(d => d.ShippingCompanies).WithMany(p => p.Ex_CheckRequest_Data)
                .HasForeignKey(d => d.ShippingCompanies_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Data_ShippingCompanies");

            entity.HasOne(d => d.Transport_Mean).WithMany(p => p.Ex_CheckRequest_Data)
                .HasForeignKey(d => d.Transport_Mean_Id)
                .HasConstraintName("FK_Ex_CheckRequest_Data_Transport_Mean");
        });

        modelBuilder.Entity<Ex_CheckRequest_Fee>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Fee_Value)
                .HasComment("قيمة الرسوم")
                .HasColumnType("money");
            entity.Property(e => e.Total_Amount)
                .HasComment("اجمالى الرسوم")
                .HasColumnType("money");
            entity.Property(e => e.Total_GrossWeight)
                .HasComment("الوزن الاجمالى للطلب")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.is_Fee_Rounding).HasComment("تقريب الرسوم");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Fees)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_Fees_Ex_CheckRequest");
        });

        modelBuilder.Entity<Ex_CheckRequest_Final_Result>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Ex_CheckRequest_Items_Final_Position");

            entity.ToTable("Ex_CheckRequest_Final_Result");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Final_Results)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Final_Result_Ex_CheckRequest");

            entity.HasOne(d => d.Ex_Final_Result).WithMany(p => p.Ex_CheckRequest_Final_Results)
                .HasForeignKey(d => d.Ex_Final_Result_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Final_Position_Ex_Final_Position");
        });

        modelBuilder.Entity<Ex_CheckRequest_Item>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Accept_User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Accept_User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Agriculture_Hand)
                .HasMaxLength(250)
                .HasComment("ناحية الزراعة");
            entity.Property(e => e.Fees).HasColumnType("money");
            entity.Property(e => e.GrossWeight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Item_Permission_Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Net_Weight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Net_WeightOld).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Package_Weight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Center).WithMany(p => p.Ex_CheckRequest_Items)
                .HasForeignKey(d => d.Center_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Center");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Items)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Ex_CheckRequest");

            entity.HasOne(d => d.FarmsData).WithMany(p => p.Ex_CheckRequest_Items)
                .HasForeignKey(d => d.FarmsData_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_FarmsData");

            entity.HasOne(d => d.Governate).WithMany(p => p.Ex_CheckRequest_Items)
                .HasForeignKey(d => d.Governate_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Governate");

            entity.HasOne(d => d.ItemCategories_Group).WithMany(p => p.Ex_CheckRequest_Items)
                .HasForeignKey(d => d.ItemCategories_Group_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_ItemCategories_Group");

            entity.HasOne(d => d.ItemCategory).WithMany(p => p.Ex_CheckRequest_Items)
                .HasForeignKey(d => d.ItemCategory_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_ItemCategories");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Ex_CheckRequest_Items)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Item_ShortName");

            entity.HasOne(d => d.Village).WithMany(p => p.Ex_CheckRequest_Items)
                .HasForeignKey(d => d.Village_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Village");
        });

        modelBuilder.Entity<Ex_CheckRequest_Items_Lot_Category>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Ex_CheckRequest_Items_Category");

            entity.ToTable("Ex_CheckRequest_Items_Lot_Category");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Based_Weight)
                .HasComment("مش مستخدم")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.GrossWeight)
                .HasComment("اجمالى الوزن القائم لللوطات")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.GrossWeightOld).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Grower_Number).HasMaxLength(50);
            entity.Property(e => e.Lot_Number).HasMaxLength(50);
            entity.Property(e => e.Net_Weight)
                .HasComment("الوزن الصافي لللوطات")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Number_Wooden_Package).HasMaxLength(50);
            entity.Property(e => e.Package_Based_Weight)
                .HasComment("وزن العبوة القائم")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Package_Count).HasComment("عدد العبوات");
            entity.Property(e => e.Package_Net_Weight)
                .HasComment("وزن العبوة الصافي")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Package_Weight)
                .HasComment("الوزن العبوه الفارغ")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.RejectReason).HasMaxLength(300);
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Waybill).HasMaxLength(100);

            entity.HasOne(d => d.Ex_CheckRequest_Items).WithMany(p => p.Ex_CheckRequest_Items_Lot_Categories)
                .HasForeignKey(d => d.Ex_CheckRequest_Items_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Category_Ex_CheckRequest_Items");

            entity.HasOne(d => d.Ex_CheckRequset_Shipping_Method).WithMany(p => p.Ex_CheckRequest_Items_Lot_Categories)
                .HasForeignKey(d => d.Ex_CheckRequset_Shipping_MethodID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Lot_Category_Ex_CheckRequset_Shipping_Method");
        });

        modelBuilder.Entity<Ex_CheckRequest_Items_Lot_Result>(entity =>
        {
            entity.ToTable("Ex_CheckRequest_Items_Lot_Result");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IS_Status_Committee).HasComment("الموقف مقبول او مرفوض");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest_Items_Lot_Category).WithMany(p => p.Ex_CheckRequest_Items_Lot_Results)
                .HasForeignKey(d => d.Ex_CheckRequest_Items_Lot_Category_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Lot_Result_Ex_CheckRequest_Items_Lot_Category");

            entity.HasOne(d => d.IS_StatusNavigation).WithMany(p => p.Ex_CheckRequest_Items_Lot_Results)
                .HasForeignKey(d => d.IS_Status)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Lot_Result_Ex_CheckRequest_Lot_Result_Status");
        });

        modelBuilder.Entity<Ex_CheckRequest_Lot_Result_Status>(entity =>
        {
            entity.ToTable("Ex_CheckRequest_Lot_Result_Status");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Is_Continue).HasComment("0 عدم استكمال الاعمال\r\nلا يمكن استكمال الاعمال 1");
        });

        modelBuilder.Entity<Ex_CheckRequest_Organization_Distribution>(entity =>
        {
            entity.ToTable("Ex_CheckRequest_Organization_Distribution");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Organization_ID).HasComment("رقم الجهة");
            entity.Property(e => e.Organization_Type_Id).HasComment("نوع الجهة");
            entity.Property(e => e.Quantity_Ton).HasComment("الكمية الصالحة للتصدير");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Organization_Distributions)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_Organization_Distribution_Ex_CheckRequest");
        });

        modelBuilder.Entity<Ex_CheckRequest_Organization_Distribution_Detial>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_CheckRequest_Organization_Distribution_Detials");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Quantity_Ton).HasComment("الكمية الصالحة للتصدير");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Organization_Distribution_Detials)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_Organization_Distribution_Detials_Ex_CheckRequest");

            entity.HasOne(d => d.Ex_CheckRequest_Organization_Distribution_Master).WithMany(p => p.Ex_CheckRequest_Organization_Distribution_Detials)
                .HasForeignKey(d => d.Ex_CheckRequest_Organization_Distribution_Master_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_Distribution_Im_CheckRequest_Organization_Distribution");
        });

        modelBuilder.Entity<Ex_CheckRequest_Organization_Distribution_Master>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_CheckRequest_Organization");

            entity.ToTable("Ex_CheckRequest_Organization_Distribution_Master");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Organization_ID).HasComment("رقم الجهة");
            entity.Property(e => e.Organization_Type_Id).HasComment("نوع الجهة");
            entity.Property(e => e.Totall_Quantity_Ton_Ex_CheckRequest).HasComment("الكمية الصالحة للتصدير");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Ex_CheckRequest_Organization_Distribution_Masters)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_Organization_Distribution_Item_ShortName");
        });

        modelBuilder.Entity<Ex_CheckRequest_Place>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Center_ID).HasComment("المركز");
            entity.Property(e => e.Examination_location)
                .HasMaxLength(250)
                .HasComment("مكان الفحص");
            entity.Property(e => e.Govern_ID).HasComment("المحافظة");
            entity.Property(e => e.PortNational_ID).HasComment(" الميناء للجشني");
            entity.Property(e => e.Station_Accreditation_Data_Examination_ID).HasComment("مسمسي الاعتماد للمحطة");
            entity.Property(e => e.Station_Examination_ID).HasComment("محطة الفحص");
            entity.Property(e => e.Station_Genshi_ID).HasComment("محطة الجشني");

            entity.HasOne(d => d.Center).WithMany(p => p.Ex_CheckRequest_Places)
                .HasForeignKey(d => d.Center_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Places_Center");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Places)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_Places_Ex_CheckRequest");

            entity.HasOne(d => d.Govern).WithMany(p => p.Ex_CheckRequest_Places)
                .HasForeignKey(d => d.Govern_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Places_Governate");

            entity.HasOne(d => d.PortNational).WithMany(p => p.Ex_CheckRequest_Places)
                .HasForeignKey(d => d.PortNational_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Places_PortNational");

            entity.HasOne(d => d.Station_Examination).WithMany(p => p.Ex_CheckRequest_PlaceStation_Examinations)
                .HasForeignKey(d => d.Station_Examination_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Places_Station3");

            entity.HasOne(d => d.Station_Genshi).WithMany(p => p.Ex_CheckRequest_PlaceStation_Genshis)
                .HasForeignKey(d => d.Station_Genshi_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Places_Station2");
        });

        modelBuilder.Entity<Ex_CheckRequest_Port>(entity =>
        {
            entity.ToTable("Ex_CheckRequest_Port");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest_Data).WithMany(p => p.Ex_CheckRequest_Ports)
                .HasForeignKey(d => d.Ex_CheckRequest_Data_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Port_Ex_CheckRequest_Data1");
        });

        modelBuilder.Entity<Ex_CheckRequest_RefuseReason>(entity =>
        {
            entity.ToTable("Ex_CheckRequest_RefuseReason");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_RefuseReasons)
                .HasForeignKey(d => d.Ex_CheckRequest_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_RefuseReason_Ex_CheckRequest");

            entity.HasOne(d => d.Refuse_Reason).WithMany(p => p.Ex_CheckRequest_RefuseReasons)
                .HasForeignKey(d => d.Refuse_Reason_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_RefuseReason_Refuse_Reason");
        });

        modelBuilder.Entity<Ex_CheckRequest_SampleData_Confirm>(entity =>
        {
            entity.ToTable("Ex_CheckRequest_SampleData_Confirm");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Ex_CheckRequest_SampleData).WithMany(p => p.Ex_CheckRequest_SampleData_Confirms)
                .HasForeignKey(d => d.Ex_CheckRequest_SampleData_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_SampleData_Confirm_Ex_CheckRequest_SampleData");
        });

        modelBuilder.Entity<Ex_CheckRequest_SampleDatum>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Admin_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.IS_From_Android).HasDefaultValue(false);
            entity.Property(e => e.Notes_Ar).HasMaxLength(300);
            entity.Property(e => e.Notes_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason_Ar).HasMaxLength(150);
            entity.Property(e => e.RejectReason_En)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Sample_BarCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.AnalysisLabType).WithMany(p => p.Ex_CheckRequest_SampleData)
                .HasForeignKey(d => d.AnalysisLabType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_SampleData_AnalysisLabType");

            entity.HasOne(d => d.Ex_RequestCommittee).WithMany(p => p.Ex_CheckRequest_SampleData)
                .HasForeignKey(d => d.Ex_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CheckRequest_SampleData_Ex_RequestCommittee");
        });

        modelBuilder.Entity<Ex_CheckRequest_Visa>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Ex_CheckRequest_Items_Lot_Visa");

            entity.ToTable("Ex_CheckRequest_Visa");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Ex_CheckRequest_Visas)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Lot_Visa_Ex_CheckRequest");

            entity.HasOne(d => d.Ex_Visa).WithMany(p => p.Ex_CheckRequest_Visas)
                .HasForeignKey(d => d.Ex_Visa_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Visa_Ex_Visa");

            entity.HasOne(d => d.Ex_VisaNavigation).WithMany(p => p.Ex_CheckRequest_Visas)
                .HasForeignKey(d => d.Ex_Visa_ID)
                .HasConstraintName("FK_Ex_CheckRequest_Items_Lot_Visa_Ex_Visa");
        });

        modelBuilder.Entity<Ex_CheckRequset_Shipping_Method>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Ex_CheckRequset_shippingmethod2");

            entity.ToTable("Ex_CheckRequset_Shipping_Method");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ContainerNumber).HasMaxLength(50);
            entity.Property(e => e.NavigationalNumber).HasMaxLength(50);
            entity.Property(e => e.ShipholdNumber).HasMaxLength(50);
            entity.Property(e => e.Total_Weight).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Ex_CommitteeCheckLocation>(entity =>
        {
            entity.ToTable("Ex_CommitteeCheckLocation");

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ex_CommitteeResult>(entity =>
        {
            entity.ToTable("Ex_CommitteeResult");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CommitteeResultType).WithMany(p => p.Ex_CommitteeResults)
                .HasForeignKey(d => d.CommitteeResultType_ID)
                .HasConstraintName("FK_Ex_CommitteeResult_CommitteeResultType");

            entity.HasOne(d => d.Committee).WithMany(p => p.Ex_CommitteeResults)
                .HasForeignKey(d => d.Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CommitteeResult_Ex_RequestCommittee");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Ex_CommitteeResults)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .HasConstraintName("FK_Ex_CommitteeResult_Item_ShortName");
        });

        modelBuilder.Entity<Ex_CommitteeResult_Confirm>(entity =>
        {
            entity.ToTable("Ex_CommitteeResult_Confirm");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);

            entity.HasOne(d => d.Ex_CommitteeResult).WithMany(p => p.Ex_CommitteeResult_Confirms)
                .HasForeignKey(d => d.Ex_CommitteeResult_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CommitteeResult_Confirm_Ex_CommitteeResult");
        });

        modelBuilder.Entity<Ex_CommitteeResult_Infection>(entity =>
        {
            entity.ToTable("Ex_CommitteeResult_Infection");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CommitteeResult).WithMany(p => p.Ex_CommitteeResult_Infections)
                .HasForeignKey(d => d.Ex_CommitteeResult_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CommitteeResult_Infection_Ex_CommitteeResult");

            entity.HasOne(d => d.Item).WithMany(p => p.Ex_CommitteeResult_Infections)
                .HasForeignKey(d => d.Item_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CommitteeResult_Infection_Item");
        });

        modelBuilder.Entity<Ex_Constran>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Ex_Constran");

            entity.Property(e => e.Ar_Name).HasMaxLength(150);
            entity.Property(e => e.ConstrainText_En).IsUnicode(false);
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Expr1).HasMaxLength(150);
            entity.Property(e => e.Expr2)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.InSide_Certificate_En).IsUnicode(false);
            entity.Property(e => e.ShortName_En).IsUnicode(false);
        });

        modelBuilder.Entity<Ex_ContactDatum>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Company_ContactType");

            entity.ToTable(tb => tb.HasComment("وسائل اتصال المصدر"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ContactType_ID).HasComment("نوع وسيلة الاتصال");
            entity.Property(e => e.ExporterType_Id).HasComment("0 if National company/ 1 if Public Organization");
            entity.Property(e => e.Exporter_ID).HasComment("المصدر(شركة أو هيئه عامة)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Value)
                .HasMaxLength(150)
                .HasComment("الرقم");

            entity.HasOne(d => d.ContactType).WithMany(p => p.Ex_ContactData)
                .HasForeignKey(d => d.ContactType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_ContactType_ContactType");

            entity.HasOne(d => d.ExporterType).WithMany(p => p.Ex_ContactData)
                .HasForeignKey(d => d.ExporterType_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exporter_Contact_A_SystemCode");
        });

        modelBuilder.Entity<Ex_ContactDatum1>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Company_ContactType");

            entity.ToTable("Ex_ContactData", "rejection");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Value).HasMaxLength(150);
        });

        modelBuilder.Entity<Ex_CountryConstrain>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CountryConstrain");

            entity.ToTable("Ex_CountryConstrain", tb => tb.HasComment("الاشترطات الدوليه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Import_Country_ID).HasComment("الدولة المستوردة");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.IsCompanyAccreditation)
                .HasDefaultValue(false)
                .HasComment("هل شركة معتمدة");
            entity.Property(e => e.IsFarmAccreditation)
                .HasDefaultValue(false)
                .HasComment("هل مزرعة معتمدة");
            entity.Property(e => e.IsStationAccreditation)
                .HasDefaultValue(false)
                .HasComment("هل محطة معتمدة");
            entity.Property(e => e.Item_ShortName_id).HasComment("product or plant ID manual no relation");
            entity.Property(e => e.TransportCountry_ID).HasComment("دولة عبور");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Ex_CountryConstrains)
                .HasForeignKey(d => d.Item_ShortName_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CountryConstrain_Item_ShortName");
        });

        modelBuilder.Entity<Ex_CountryConstrain_AnalysisLabType>(entity =>
        {
            entity.ToTable("Ex_CountryConstrain_AnalysisLabType", tb => tb.HasComment("تحاليل الاشتراطات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsAcive).HasComment("1 لو مفعل انه يظهر في الشهادة الزراعية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.AnalysisType).WithMany(p => p.Ex_CountryConstrain_AnalysisLabTypes)
                .HasForeignKey(d => d.AnalysisTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CountryConstrain_AnalysisLabType_AnalysisType");

            entity.HasOne(d => d.CountryConstrain).WithMany(p => p.Ex_CountryConstrain_AnalysisLabTypes)
                .HasForeignKey(d => d.CountryConstrain_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CountryConstrain_AnalysisLabType_Ex_CountryConstrain");
        });

        modelBuilder.Entity<Ex_CountryConstrain_ArrivalPort>(entity =>
        {
            entity.ToTable("Ex_CountryConstrain_ArrivalPort", tb => tb.HasComment("موانى تحديد ميناء وصول معين"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CountryConstrain).WithMany(p => p.Ex_CountryConstrain_ArrivalPorts)
                .HasForeignKey(d => d.Ex_CountryConstrain_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CountryConstrain_ArrivalPort_Ex_CountryConstrain");

            entity.HasOne(d => d.Port_International).WithMany(p => p.Ex_CountryConstrain_ArrivalPorts)
                .HasForeignKey(d => d.Port_International_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CountryConstrain_ArrivalPort_Port_International");
        });

        modelBuilder.Entity<Ex_CountryConstrain_Text>(entity =>
        {
            entity.ToTable("Ex_CountryConstrain_Text", tb => tb.HasComment("نصوص الاشتراطات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CountryConstrain).WithMany(p => p.Ex_CountryConstrain_Texts)
                .HasForeignKey(d => d.CountryConstrain_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CountryConstrain_Text_Ex_CountryConstrain");

            entity.HasOne(d => d.EX_Constrain_Text).WithMany(p => p.Ex_CountryConstrain_Texts)
                .HasForeignKey(d => d.EX_Constrain_Text_ID)
                .HasConstraintName("FK_Ex_CountryConstrain_Text_EX_Constrain_Text");
        });

        modelBuilder.Entity<Ex_CountryConstrain_Treatment>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CountryConstrain_Treatment");

            entity.ToTable("Ex_CountryConstrain_Treatment", tb => tb.HasComment("معالجات الاشتراطات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IS_Optional).HasComment("التحليل اختيارى =0 \r\nالتحليل اجباري =1");
            entity.Property(e => e.IsAcive).HasComment("1 لو مفعل انه يظهر في الشهادة الزراعية");
            entity.Property(e => e.TheDose)
                .HasComment("الجرعة")
                .HasColumnType("decimal(18, 3)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CountryConstrain).WithMany(p => p.Ex_CountryConstrain_Treatments)
                .HasForeignKey(d => d.CountryConstrain_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CountryConstrain_Treatment_CountryConstrain");

            entity.HasOne(d => d.TreatmentMethods).WithMany(p => p.Ex_CountryConstrain_Treatments)
                .HasForeignKey(d => d.TreatmentMethods_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_CountryConstrain_Treatment_TreatmentMethods");
        });

        modelBuilder.Entity<Ex_Final_Result>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Ex_Final_Position");

            entity.ToTable("Ex_Final_Result");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.En_Name).IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Ex_List>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Ex_List");

            entity.Property(e => e.Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.ExportCountryName).HasMaxLength(100);
            entity.Property(e => e.ImCheckRequest_Number)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImporterTypeName).HasMaxLength(150);
        });

        modelBuilder.Entity<Ex_List2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Ex_List2");

            entity.Property(e => e.Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.ExportCountryName).HasMaxLength(100);
            entity.Property(e => e.ImCheckRequest_Number)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImporterTypeName).HasMaxLength(150);
        });

        modelBuilder.Entity<Ex_OpertaionType>(entity =>
        {
            entity.ToTable("Ex_OpertaionType");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ex_RequestCommittee>(entity =>
        {
            entity.ToTable("Ex_RequestCommittee");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsApproved).HasDefaultValue(false);
            entity.Property(e => e.IsFinishedAll).HasDefaultValue(false);
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.Is_Cancel).HasComment("لايقاف او حذف اللجنة مربوط مع  A_SystemCode رقم 31\r\n");
            entity.Property(e => e.Is_Start_Android).HasComment("تعزر عمل اللجنه");
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CommitteeType).WithMany(p => p.Ex_RequestCommittees)
                .HasForeignKey(d => d.CommitteeType_ID)
                .HasConstraintName("FK_Ex_RequestCommittee_CommitteeType");

            entity.HasOne(d => d.ExCheckRequest).WithMany(p => p.Ex_RequestCommittees)
                .HasForeignKey(d => d.ExCheckRequest_ID)
                .HasConstraintName("FK_Ex_RequestCommittee_Ex_CheckRequest");

            entity.HasOne(d => d.ExCommitteeCheckLocation).WithMany(p => p.Ex_RequestCommittees)
                .HasForeignKey(d => d.ExCommitteeCheckLocation_ID)
                .HasConstraintName("FK_Ex_RequestCommittee_Ex_CommitteeCheckLocation");
        });

        modelBuilder.Entity<Ex_RequestCommittee_Fees_ENG>(entity =>
        {
            entity.ToTable("Ex_RequestCommittee_Fees_ENG");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.Num_Eng).HasDefaultValue(1);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Value).HasColumnType("money");

            entity.HasOne(d => d.Ex_Fees_Type).WithMany(p => p.Ex_RequestCommittee_Fees_ENGs)
                .HasForeignKey(d => d.Ex_Fees_Type_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_RequestCommittee_Fees_ENG_EX_Fees_Type");

            entity.HasOne(d => d.Ex_RequestCommittee).WithMany(p => p.Ex_RequestCommittee_Fees_ENGs)
                .HasForeignKey(d => d.Ex_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_RequestCommittee_Fees_ENG_Ex_RequestCommittee");
        });

        modelBuilder.Entity<Ex_RequestCommittee_Shift>(entity =>
        {
            entity.ToTable("Ex_RequestCommittee_Shift");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_RequestCommittee).WithMany(p => p.Ex_RequestCommittee_Shifts)
                .HasForeignKey(d => d.Ex_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_RequestCommittee_Shift_Ex_RequestCommittee");

            entity.HasOne(d => d.ShiftTiming).WithMany(p => p.Ex_RequestCommittee_Shifts)
                .HasForeignKey(d => d.ShiftTiming_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_RequestCommittee_Shift_ShiftTiming");
        });

        modelBuilder.Entity<Ex_Request_TreatmentData_Confirm>(entity =>
        {
            entity.ToTable("Ex_Request_TreatmentData_Confirm");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);

            entity.HasOne(d => d.Ex_Request_TreatmentData).WithMany(p => p.Ex_Request_TreatmentData_Confirms)
                .HasForeignKey(d => d.Ex_Request_TreatmentData_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_Request_TreatmentData_Confirm_Ex_Request_TreatmentData");
        });

        modelBuilder.Entity<Ex_Request_TreatmentDatum>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.IS_From_Android).HasDefaultValue(false);
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Size).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.Temperature).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TheDose).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.ThermalSealNumber).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.TreatmentMat_Amount).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_RequestCommittee).WithMany(p => p.Ex_Request_TreatmentData)
                .HasForeignKey(d => d.Ex_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ex_Request_TreatmentData_Im_RequestCommittee");

            entity.HasOne(d => d.TreatmentMat).WithMany(p => p.Ex_Request_TreatmentData)
                .HasForeignKey(d => d.TreatmentMat_ID)
                .HasConstraintName("FK_Ex_Request_TreatmentData_TreatmentMaterial");
        });

        modelBuilder.Entity<Ex_Visa>(entity =>
        {
            entity.ToTable("Ex_Visa", tb => tb.HasComment("التاشيره"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name).HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Family>(entity =>
        {
            entity.ToTable("Family", tb => tb.HasComment("العائله"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Order).WithMany(p => p.Families)
                .HasForeignKey(d => d.Order_ID)
                .HasConstraintName("FK_Family_Order");
        });

        modelBuilder.Entity<Farm_CheckList>(entity =>
        {
            entity.ToTable("Farm_CheckList");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ConstrainText_En).IsUnicode(false);
            entity.Property(e => e.Description_En).IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Farm_Committee>(entity =>
        {
            entity.ToTable("Farm_Committee", tb => tb.HasComment("تشكيل لجنة مزرعة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount_Total)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Delegation_Date).HasComment("تاريخ الفحص-تاريخ الانتداب");
            entity.Property(e => e.EndTime).HasComment("انتهاء ساعة الفحص");
            entity.Property(e => e.Farm_Request_ID).HasComment("طلب الفحص");
            entity.Property(e => e.IsApproved).HasComment("null->exporter not take action \r\n0 if exporter doesn't accept else 1\r\n\r\nnull تم طلب لجنة\r\n0 تم رفض الطلب من العميل\r\n1 تم قبول الطلب من العميل\r\n");
            entity.Property(e => e.IsFinishedAll)
                .HasDefaultValue(false)
                .HasComment("0 if exporter doesn't accept else 1  خاص ب شغل موظف الحجر في فحص الشحنه");
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.Is_Cancel).HasComment("تعزر عمل اللجنه");
            entity.Property(e => e.Is_Start_Android).HasComment("تعزر عمل اللجنه");
            entity.Property(e => e.Refuse_Reason_Nots).HasComment("اسباب الرفض");
            entity.Property(e => e.StartTime).HasComment(" بداية ساعة الفحص ");
            entity.Property(e => e.Status).HasComment("null->No committe ,0 if not done, 1 if investigation is done\r\n\r\nnull لم يتم تشكيل اللجنة\r\n0 تم التشكيل ولم يتم خروج اللجنة\r\n1 انتهاء عمل اللجنة\r\n");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.analysis_count)
                .HasDefaultValue((byte)0)
                .HasComment("عدد السحبات");

            entity.HasOne(d => d.CommitteeType).WithMany(p => p.Farm_Committees)
                .HasForeignKey(d => d.CommitteeType_ID)
                .HasConstraintName("FK_Farm_Committee_CommitteeType");

            entity.HasOne(d => d.Farm_Request).WithMany(p => p.Farm_Committees)
                .HasForeignKey(d => d.Farm_Request_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_Farm_Country_Request");
        });

        modelBuilder.Entity<Farm_Committee_CheckList>(entity =>
        {
            entity.ToTable("Farm_Committee_CheckList");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.FarmCommittee_ID).HasComment("لجنة المعالجة");
            entity.Property(e => e.IsAccepted).HasComment("0 if rejected else 1 \r\nlab will set the result");
            entity.Property(e => e.IsAccepted_Quarantine).HasComment("0 if rejected else 1 \r\nlab will set the result");
            entity.Property(e => e.Notes_Ar).HasMaxLength(300);
            entity.Property(e => e.Notes_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("Employee ID");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.FarmCommittee).WithMany(p => p.Farm_Committee_CheckLists)
                .HasForeignKey(d => d.FarmCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_CheckList_Farm_Committee");

            entity.HasOne(d => d.Farm_Country_CheckList).WithMany(p => p.Farm_Committee_CheckLists)
                .HasForeignKey(d => d.Farm_Country_CheckList_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_CheckList_Farm_Country_CheckList");
        });

        modelBuilder.Entity<Farm_Committee_CheckList_Confirm>(entity =>
        {
            entity.ToTable("Farm_Committee_CheckList_Confirm");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Farm_Committee_CheckList).WithMany(p => p.Farm_Committee_CheckList_Confirms)
                .HasForeignKey(d => d.Farm_Committee_CheckList_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_CheckList_Confirm_Farm_Committee_CheckList");
        });

        modelBuilder.Entity<Farm_Committee_Constrain>(entity =>
        {
            entity.ToTable("Farm_Committee_Constrain", tb => tb.HasComment("لجنه المزرعه"));

            entity.Property(e => e.ID).ValueGeneratedNever();

            entity.HasOne(d => d.Farm_Committee).WithMany(p => p.Farm_Committee_Constrains)
                .HasForeignKey(d => d.Farm_Committee_ID)
                .HasConstraintName("FK_Farm_Committee_Constrain_Farm_Committee");

            entity.HasOne(d => d.Farm_Constrain).WithMany(p => p.Farm_Committee_Constrains)
                .HasForeignKey(d => d.Farm_Constrain_ID)
                .HasConstraintName("FK_Farm_Committee_Constrain_Farm_Constrain");
        });

        modelBuilder.Entity<Farm_Committee_Examination>(entity =>
        {
            entity.ToTable("Farm_Committee_Examination", tb => tb.HasComment("معاينة المزرعة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.AdminFinalResult_Note).HasComment("Admin Note");
            entity.Property(e => e.Admin_Confirmation).HasComment("موقف الحجر");
            entity.Property(e => e.Admin_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Admin_User).HasComment("ادمن الحجر");
            entity.Property(e => e.EndDate).HasComment("تاريخ نهاية معاينة الصنف");
            entity.Property(e => e.FarmCommittee_ID).HasComment("لجنة المعالجة");
            entity.Property(e => e.IsAccepted).HasComment("0 if rejected else 1");
            entity.Property(e => e.IsAdminFinalResult).HasComment("null->exporter not take action 0 if exporter doesn't accept else 1");
            entity.Property(e => e.Notes).HasMaxLength(300);
            entity.Property(e => e.Quantity_Ton).HasComment("الانتاجية");
            entity.Property(e => e.StartDate).HasComment("تاريخ بداية معاينة الصنف");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.FarmCommittee).WithMany(p => p.Farm_Committee_Examinations)
                .HasForeignKey(d => d.FarmCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_Examination_Farm_Committee");

            entity.HasOne(d => d.Farm_Request_ItemCategories).WithMany(p => p.Farm_Committee_Examinations)
                .HasForeignKey(d => d.Farm_Request_ItemCategories_ID)
                .HasConstraintName("FK_Farm_Committee_Examination_Farm_Request_ItemCategories");
        });

        modelBuilder.Entity<Farm_Committee_Examination_Confirm>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Farm_Committee_Confirm");

            entity.ToTable("Farm_Committee_Examination_Confirm", tb => tb.HasComment("الموافقة على نتيجة الفحص"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Farm_Committee_Exmination).WithMany(p => p.Farm_Committee_Examination_Confirms)
                .HasForeignKey(d => d.Farm_Committee_Exmination_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_Confirm_Farm_Committee_Examination");
        });

        modelBuilder.Entity<Farm_Committee_Final_Result>(entity =>
        {
            entity.ToTable("Farm_Committee_Final_Result");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.FarmCommittee_ID).HasComment("لجنة المعالجة");
            entity.Property(e => e.ISAdmin).HasComment("is admin for the current committee");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("Employee ID");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.FarmCommittee).WithMany(p => p.Farm_Committee_Final_Results)
                .HasForeignKey(d => d.FarmCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_Final_Result_Farm_Committee");
        });

        modelBuilder.Entity<Farm_Committee_Shift>(entity =>
        {
            entity.ToTable("Farm_Committee_Shift");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Farm_Committee).WithMany(p => p.Farm_Committee_Shifts)
                .HasForeignKey(d => d.Farm_Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_Shift_Farm_Committee");

            entity.HasOne(d => d.ShiftTiming).WithMany(p => p.Farm_Committee_Shifts)
                .HasForeignKey(d => d.ShiftTiming_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Committee_Shift_ShiftTiming");
        });

        modelBuilder.Entity<Farm_Company>(entity =>
        {
            entity.ToTable("Farm_Company", tb => tb.HasComment("الشركة او الهيئة او الفرد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ExporterType_Id).HasComment("from systemcode table 3");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Farm).WithMany(p => p.Farm_Companies)
                .HasForeignKey(d => d.Farm_ID)
                .HasConstraintName("FK_Farm_Company_FarmsData");
        });

        modelBuilder.Entity<Farm_Constrain>(entity =>
        {
            entity.ToTable("Farm_Constrain", tb => tb.HasComment("اشترطات المزارع"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.Is_Preview).HasComment("المعاينة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.AnalysisType).WithMany(p => p.Farm_Constrains)
                .HasForeignKey(d => d.AnalysisType_ID)
                .HasConstraintName("FK_Farm_Constrain_AnalysisType");

            entity.HasOne(d => d.Country).WithMany(p => p.Farm_Constrains)
                .HasForeignKey(d => d.Country_Id)
                .HasConstraintName("FK_Farm_Constrain_Country");

            entity.HasOne(d => d.Farm_Constrain_Text).WithMany(p => p.Farm_Constrains)
                .HasForeignKey(d => d.Farm_Constrain_Text_ID)
                .HasConstraintName("FK_Farm_Constrain_Farm_Constrain_Text");

            entity.HasOne(d => d.Item).WithMany(p => p.Farm_Constrains)
                .HasForeignKey(d => d.Item_ID)
                .HasConstraintName("FK_Farm_Constrain_Item");
        });

        modelBuilder.Entity<Farm_Constrain_Text>(entity =>
        {
            entity.ToTable("Farm_Constrain_Text");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ConstrainText_En).IsUnicode(false);
            entity.Property(e => e.Description_En).IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Farm_Country>(entity =>
        {
            entity.ToTable("Farm_Country");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Country_ID).HasComment("ConstrainOwner(UnionId/CountryId/ or 0 if Local-Egypt)");
            entity.Property(e => e.IsAcceppted).HasComment("الحجر واقف على الدولة ولا لا");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Farm_Countries)
                .HasForeignKey(d => d.Country_ID)
                .HasConstraintName("FK_Farm_Country_Country");

            entity.HasOne(d => d.Farm_Request).WithMany(p => p.Farm_Countries)
                .HasForeignKey(d => d.Farm_Request_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Country_Farm_Request");
        });

        modelBuilder.Entity<Farm_Country_CheckList>(entity =>
        {
            entity.ToTable("Farm_Country_CheckList");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Farm_Country_CheckLists)
                .HasForeignKey(d => d.Country_ID)
                .HasConstraintName("FK_Farm_Country_CheckList_Country");

            entity.HasOne(d => d.Farm_CheckList).WithMany(p => p.Farm_Country_CheckLists)
                .HasForeignKey(d => d.Farm_CheckList_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Country_CheckList_Farm_CheckList");

            entity.HasOne(d => d.Item).WithMany(p => p.Farm_Country_CheckLists)
                .HasForeignKey(d => d.Item_ID)
                .HasConstraintName("FK_Farm_Country_CheckList_Item");
        });

        modelBuilder.Entity<Farm_Fee>(entity =>
        {
            entity.HasKey(e => e.FarmFeesID);

            entity.Property(e => e.User_Creation_Date)
                .HasComment("null-> for user , value -> if the admin add the row")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("null-> for user , value -> if the admin add the row");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.fees).HasColumnType("money");
        });

        modelBuilder.Entity<Farm_ItemCategory>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_FarmPlant");

            entity.ToTable(tb => tb.HasComment("اصناف المزرعة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Area_Acres).HasComment("مساحة العميل");
            entity.Property(e => e.Area_Acres_Quarant).HasComment("المساحة النهائية للحجر");
            entity.Property(e => e.Quantity_Ton).HasComment("");
            entity.Property(e => e.Quantity_Ton__Export).HasComment("الكمية الاجمالية الصالحة للتصدير");
            entity.Property(e => e.Quantity_Ton__Quarant).HasComment("الكمية للفدان بالطن للحجر");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Farm).WithMany(p => p.Farm_ItemCategories)
                .HasForeignKey(d => d.Farm_ID)
                .HasConstraintName("FK_FarmPlant_FarmsData");

            entity.HasOne(d => d.ItemCategories).WithMany(p => p.Farm_ItemCategories)
                .HasForeignKey(d => d.ItemCategories_ID)
                .HasConstraintName("FK_FarmPlant_PlantCategories");
        });

        modelBuilder.Entity<Farm_Request>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Farm_Country_Recoust");

            entity.ToTable("Farm_Request");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Fees).HasColumnType("money");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.IS_OnlineOffline)
                .HasDefaultValue(false)
                .HasComment("from web/system\r\n1->online\r\n0->offline");
            entity.Property(e => e.IsPaid).HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.IsStatus).HasComment("null لم يتم انتهاء العمل على الطلب\r\n0 تم رفض الطلب\r\n1 تم قبول الطلب\r\n");
            entity.Property(e => e.Is_Final_requst).HasComment("الموقف النهائي للطلب\r\nnull لم يتم العمل على الطلب\r\n0 يتم العمل على الطلب\r\n1 تم الانتهاء من العمل على الطلب");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Farm_Request_Type).WithMany(p => p.Farm_Requests)
                .HasForeignKey(d => d.Farm_Request_Type_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Request_Farm_Request_Type");

            entity.HasOne(d => d.FarmsData).WithMany(p => p.Farm_Requests)
                .HasForeignKey(d => d.FarmsData_ID)
                .HasConstraintName("FK_Farm_Country_Request_FarmsData");
        });

        modelBuilder.Entity<Farm_Request_ItemCategory>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Admin_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Admin_User).HasComment("ادمن الحجر");
            entity.Property(e => e.Area_Acres).HasComment("مساحة العميل");
            entity.Property(e => e.Area_Acres_Quarant).HasComment("المساحة النهائية للحجر");
            entity.Property(e => e.Quantity_Ton).HasComment("الكمية للطن");
            entity.Property(e => e.Quantity_Ton__Export).HasComment("الكمية الاجمالية الصالحة للتصدير");
            entity.Property(e => e.Quantity_Ton__Quarant).HasComment("الكمية للفدان بالطن للحجر");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Farm_ItemCategories).WithMany(p => p.Farm_Request_ItemCategories)
                .HasForeignKey(d => d.Farm_ItemCategories_ID)
                .HasConstraintName("FK_Farm_Request_ItemCategories_Farm_ItemCategories");

            entity.HasOne(d => d.Farm_Request).WithMany(p => p.Farm_Request_ItemCategories)
                .HasForeignKey(d => d.Farm_Request_ID)
                .HasConstraintName("FK_Farm_Request_ItemCategories_Farm_Request");
        });

        modelBuilder.Entity<Farm_Request_Refuse_Reason>(entity =>
        {
            entity.ToTable("Farm_Request_Refuse_Reason");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Farm_Request).WithMany(p => p.Farm_Request_Refuse_Reasons)
                .HasForeignKey(d => d.Farm_Request_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Request_Refuse_Reason_Farm_Request");

            entity.HasOne(d => d.Refuse_Reason).WithMany(p => p.Farm_Request_Refuse_Reasons)
                .HasForeignKey(d => d.Refuse_Reason_ID)
                .HasConstraintName("FK_Farm_Request_Refuse_Reason_Refuse_Reason");
        });

        modelBuilder.Entity<Farm_Request_Type>(entity =>
        {
            entity.ToTable("Farm_Request_Type");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_AR).HasMaxLength(50);
            entity.Property(e => e.Name_EN).HasMaxLength(50);
        });

        modelBuilder.Entity<Farm_SampleData_Confirm>(entity =>
        {
            entity.ToTable("Farm_SampleData_Confirm");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Farm_SampleData).WithMany(p => p.Farm_SampleData_Confirms)
                .HasForeignKey(d => d.Farm_SampleData_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_SampleData_Confirm_Farm_SampleData");
        });

        modelBuilder.Entity<Farm_SampleData_Confirm_Item>(entity =>
        {
            entity.ToTable("Farm_SampleData_Confirm_Item");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Farm_SampleData_Item).WithMany(p => p.Farm_SampleData_Confirm_Items)
                .HasForeignKey(d => d.Farm_SampleData_Item_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_SampleData_Confirm_Item_Farm_SampleData_Item");
        });

        modelBuilder.Entity<Farm_SampleData_Item>(entity =>
        {
            entity.ToTable("Farm_SampleData_Item");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Admin_Confirmation).HasComment("موقف الحجر");
            entity.Property(e => e.Admin_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Admin_User).HasComment("ادمن الحجر");
            entity.Property(e => e.FarmCommittee_ID).HasComment("لجنة المعالجة");
            entity.Property(e => e.IsAccepted).HasComment("0 if rejected else 1 \r\nموافقه المعمل");
            entity.Property(e => e.Notes_Ar)
                .HasMaxLength(300)
                .HasComment("ملاحظات الاندريد");
            entity.Property(e => e.Notes_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason_Ar)
                .HasMaxLength(150)
                .HasComment("سبب الرفض للمعمل ar");
            entity.Property(e => e.RejectReason_En)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("سبب الرفض للمعمل en");
            entity.Property(e => e.SampleRatio).HasComment("نسبة اخذ العينة");
            entity.Property(e => e.SampleSize).HasComment("حجم العينة");
            entity.Property(e => e.Sample_BarCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("البار كود");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.WithdrawDate).HasComment("تاريخ سحب العينة");

            entity.HasOne(d => d.AnalysisLabType).WithMany(p => p.Farm_SampleData_Items)
                .HasForeignKey(d => d.AnalysisLabType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_SampleData_Item_AnalysisLabType");

            entity.HasOne(d => d.FarmCommittee).WithMany(p => p.Farm_SampleData_Items)
                .HasForeignKey(d => d.FarmCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_SampleData_Item_Farm_Committee");

            entity.HasOne(d => d.Farm_Request_ItemCategories).WithMany(p => p.Farm_SampleData_Items)
                .HasForeignKey(d => d.Farm_Request_ItemCategories_ID)
                .HasConstraintName("FK_Farm_SampleData_Item_Farm_Request_ItemCategories");
        });

        modelBuilder.Entity<Farm_SampleDatum>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("عينة سحب المزرعة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Admin_Confirmation).HasComment("موقف الحجر");
            entity.Property(e => e.Admin_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Admin_User).HasComment("ادمن الحجر");
            entity.Property(e => e.FarmCommittee_ID).HasComment("لجنة المعالجة");
            entity.Property(e => e.IsAccepted).HasComment("0 if rejected else 1 \r\nموافقه المعمل");
            entity.Property(e => e.Notes_Ar)
                .HasMaxLength(300)
                .HasComment("ملاحظات الاندريد");
            entity.Property(e => e.Notes_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason_Ar)
                .HasMaxLength(150)
                .HasComment("سبب الرفض للمعمل ar");
            entity.Property(e => e.RejectReason_En)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("سبب الرفض للمعمل en");
            entity.Property(e => e.SampleRatio).HasComment("نسبة اخذ العينة");
            entity.Property(e => e.SampleSize).HasComment("حجم العينة");
            entity.Property(e => e.Sample_BarCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("البار كود");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.WithdrawDate).HasComment("تاريخ سحب العينة");

            entity.HasOne(d => d.AnalysisLabType).WithMany(p => p.Farm_SampleData)
                .HasForeignKey(d => d.AnalysisLabType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_SampleData_AnalysisLabType");

            entity.HasOne(d => d.FarmCommittee).WithMany(p => p.Farm_SampleData)
                .HasForeignKey(d => d.FarmCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_SampleData_Farm_Committee");

            entity.HasOne(d => d.Farm_Request_ItemCategories).WithMany(p => p.Farm_SampleData)
                .HasForeignKey(d => d.Farm_Request_ItemCategories_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_SampleData_Farm_Request_ItemCategories");
        });

        modelBuilder.Entity<FarmsDatum>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("المزرعة معتمدة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_Ar).HasMaxLength(100);
            entity.Property(e => e.Address_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FarmCode_14)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FileUpload).IsUnicode(false);
            entity.Property(e => e.GPSRead)
                .HasMaxLength(50)
                .HasComment("قراءة GPS");
            entity.Property(e => e.Govern_ID).HasComment("المحافظة");
            entity.Property(e => e.IS_OnlineOffline)
                .HasDefaultValue(false)
                .HasComment("from web/system\r\n1->online\r\n0->offline");
            entity.Property(e => e.IsApproved).HasComment("لو معتمدة 1");
            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasComment("is null for default 0 is stopped for a time 1 is stopped permantely");
            entity.Property(e => e.ThePivot)
                .HasMaxLength(50)
                .HasComment("الحوض أو البيفوت");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Village_ID).HasComment("المراكز");

            entity.HasOne(d => d.Center).WithMany(p => p.FarmsData)
                .HasForeignKey(d => d.Center_Id)
                .HasConstraintName("FK_FarmsData_Center");

            entity.HasOne(d => d.Govern).WithMany(p => p.FarmsData)
                .HasForeignKey(d => d.Govern_ID)
                .HasConstraintName("FK_FarmsData_Governate");

            entity.HasOne(d => d.Item).WithMany(p => p.FarmsData)
                .HasForeignKey(d => d.Item_ID)
                .HasConstraintName("FK_FarmsData_Item");

            entity.HasOne(d => d.Village).WithMany(p => p.FarmsData)
                .HasForeignKey(d => d.Village_ID)
                .HasConstraintName("FK_FarmsData_Village");
        });

        modelBuilder.Entity<Farms_Organization_Distribution_Detial>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Farm_Ex_CheckRequest_Distribution");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Quantity_Ton).HasComment("الكمية الصالحة للتصدير");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Farms_Organization_Distribution_Master).WithMany(p => p.Farms_Organization_Distribution_Detials)
                .HasForeignKey(d => d.Farms_Organization_Distribution_Master_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farm_Ex_CheckRequest_Distribution_Farms_Organization_Distribution");
        });

        modelBuilder.Entity<Farms_Organization_Distribution_Master>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Farms_Organization");

            entity.ToTable("Farms_Organization_Distribution_Master");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Organization_ID).HasComment("رقم الجهة");
            entity.Property(e => e.Organization_Type_Id).HasComment("نوع الجهة");
            entity.Property(e => e.Quantity_Ton_Farm).HasComment("الكمية الصالحة للتصدير");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Farm_ItemCategories).WithMany(p => p.Farms_Organization_Distribution_Masters)
                .HasForeignKey(d => d.Farm_ItemCategories_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Farms_Organization_Distribution_Farm_ItemCategories");
        });

        modelBuilder.Entity<FeesAmount_Fixed>(entity =>
        {
            entity.ToTable("FeesAmount_Fixed");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.IsPaidBefore).HasComment("هل تدفع عند الطلب ام بعده");
            entity.Property(e => e.MinAmount)
                .HasComment("الحد الادنى")
                .HasColumnType("money");
            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.WeightTo).HasComment("الوزن");

            entity.HasOne(d => d.FeesType).WithMany(p => p.FeesAmount_Fixeds)
                .HasForeignKey(d => d.FeesType_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeesAmount_Fixed_FeesType");
        });

        modelBuilder.Entity<FeesType>(entity =>
        {
            entity.ToTable("FeesType", tb => tb.HasComment("ثابت - معالجة - نبات - نوباتجية - سحب\r\n"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Fees_Action>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_FixedFeesAmount");

            entity.ToTable("Fees_Action", tb => tb.HasComment("تفاصيل الاجراءات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Account_Type).HasComment("نوع الحساب من  system code رقم 33");
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.IsPaidBefore).HasComment("هل تدفع عند الطلب ام بعده");
            entity.Property(e => e.MinAmount)
                .HasComment("الحد الادنى")
                .HasColumnType("money");
            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.WeightTo).HasComment("الوزن");

            entity.HasOne(d => d.Feer_Type_Action).WithMany(p => p.Fees_Actions)
                .HasForeignKey(d => d.Feer_Type_Action_ID)
                .HasConstraintName("FK_Fees_Action_Feer_Type_Action");

            entity.HasOne(d => d.FeesType).WithMany(p => p.Fees_Actions)
                .HasForeignKey(d => d.FeesType_Id)
                .HasConstraintName("FK_Fees_Action_FeesType");
        });

        modelBuilder.Entity<Fees_Certificates_Payment_Detile>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("تفاصيل الدفع للشهادات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.CardOrGroupNumber)
                .HasMaxLength(50)
                .HasComment("رقم البطاقه(pos) او المجموعه(كاش) علي حسب طريقه الدفع");
            entity.Property(e => e.PaymentDate).HasComment("تاريخ الدفع");
            entity.Property(e => e.ReferenceOrCouponNumber)
                .HasMaxLength(50)
                .HasComment("رقم مرجعي او القسيمه");

            entity.HasOne(d => d.Ex_CertificatesRequests).WithMany(p => p.Fees_Certificates_Payment_Detiles)
                .HasForeignKey(d => d.Ex_CertificatesRequests_ID)
                .HasConstraintName("FK_Fees_Certificates_Payment_Detiles_Ex_CertificatesRequests");
        });

        modelBuilder.Entity<Fees_Money>(entity =>
        {
            entity.ToTable("Fees_Money");

            entity.Property(e => e.Fees).HasColumnType("money");
        });

        modelBuilder.Entity<Fees_TableName>(entity =>
        {
            entity.ToTable("Fees_TableName");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.TableName).HasMaxLength(100);
        });

        modelBuilder.Entity<Fees_Transaction>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Account_Type).HasComment("نوع الحساب من system code رقم 33");
            entity.Property(e => e.Amount_Total)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Code_Bank)
                .HasMaxLength(50)
                .HasComment("كود العملية من البنك");
            entity.Property(e => e.IsSuccess_Bank).HasComment("0 تم رفض عملية البنك\r\n1 تم قبول العملية \r\nnull تم الارسال ولم الرد من البنك");
            entity.Property(e => e.OrderNumber).HasMaxLength(14);
            entity.Property(e => e.Payment_Type_ID).HasComment("from systemcode table 30\r\nنوع عملية الدفع فيزا - كاش");
            entity.Property(e => e.TableName_ID).HasComment("id table (fees_tablename)");
            entity.Property(e => e.Table_ID).HasComment("id الجدول الرئيسي اللي متحدد في (fess_tablename)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_ID).HasComment("from systemcode table 3\r\nنوع الموظف حجر ولا شركة ولا فرد ولاهيئه");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.TableName).WithMany(p => p.Fees_Transactions)
                .HasForeignKey(d => d.TableName_ID)
                .HasConstraintName("FK_Fees_Transactions_Fees_TableName");
        });

        modelBuilder.Entity<Fees_Transactions_Detile>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.TreatmentData_ID).HasComment("جاي من جدول Im_Request_TreatmentData عشان لو اكثر من لوط هيبقى كل لوط له id مختلف");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Fees_Action).WithMany(p => p.Fees_Transactions_Detiles)
                .HasForeignKey(d => d.Fees_Action_ID)
                .HasConstraintName("FK_Fees_Transactions_Detiles_Fees_Action");

            entity.HasOne(d => d.Fees_Transactions).WithMany(p => p.Fees_Transactions_Detiles)
                .HasForeignKey(d => d.Fees_Transactions_ID)
                .HasConstraintName("FK_Fees_Transactions_Detiles_Fees_Transactions");
        });

        modelBuilder.Entity<Fees_Transactions_Payment_Detile>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Fees_Transactions_Payment_Detailes");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.CardOrGroupNumber)
                .HasMaxLength(50)
                .HasComment("رقم البطاقه(pos) او المجموعه(كاش) علي حسب طريقه الدفع");
            entity.Property(e => e.PaymentDate).HasComment("تاريخ الدفع");
            entity.Property(e => e.ReferenceOrCouponNumber)
                .HasMaxLength(50)
                .HasComment("رقم مرجعي او القسيمه");

            entity.HasOne(d => d.pos_information).WithMany(p => p.Fees_Transactions_Payment_Detiles)
                .HasForeignKey(d => d.pos_information_id)
                .HasConstraintName("FK__Fees_Tran__Fees___1011E5CF");
        });

        modelBuilder.Entity<Fees_Type_Action>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Feer_Type_Action");

            entity.ToTable("Fees_Type_Action", tb => tb.HasComment("انواع الاجراءات"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Fees_process).WithMany(p => p.Fees_Type_Actions)
                .HasForeignKey(d => d.Fees_process_ID)
                .HasConstraintName("FK_Feer_Type_Action_Fees_process");
        });

        modelBuilder.Entity<Fees_process>(entity =>
        {
            entity.ToTable("Fees_process", tb => tb.HasComment("انواع عمليات الرسوم - صادر وارد مزارع محطات"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FreeZone>(entity =>
        {
            entity.ToTable("FreeZone");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fax).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En).HasMaxLength(100);
            entity.Property(e => e.Phone).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Gov).WithMany(p => p.FreeZones)
                .HasForeignKey(d => d.Gov_ID)
                .HasConstraintName("FK_FreeZone_Governate");
        });

        modelBuilder.Entity<FumigationUnit>(entity =>
        {
            entity.ToTable("FumigationUnit", tb => tb.HasComment("وحدات التبخير التابعة للحجر"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Capacity)
                .HasComment("السعة")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Maintenance).HasComment("حركة الصيانة مرتبط مع الشئون المالية");
            entity.Property(e => e.Status).HasComment("0 if work else 1");

            entity.HasOne(d => d.Outlet).WithMany(p => p.FumigationUnits)
                .HasForeignKey(d => d.Outlet_ID)
                .HasConstraintName("FK_FumigationUnit_Outlet");

            entity.HasOne(d => d.UnitType).WithMany(p => p.FumigationUnits)
                .HasForeignKey(d => d.UnitType_ID)
                .HasConstraintName("FK_FumigationUnit_UnitType");
        });

        modelBuilder.Entity<Gas_ImportCompany>(entity =>
        {
            entity.ToTable("Gas_ImportCompany", tb => tb.HasComment("شركات استيراد الغاز"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.AcceptanceDate).HasComment("تاريخ الموافقة");
            entity.Property(e => e.GasAmount)
                .HasComment("كمية الغاز المستوردة")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Gas_ImportCompanies)
                .HasForeignKey(d => d.Company_ID)
                .HasConstraintName("FK_Gas_ImportCompany_Company_National");
        });

        modelBuilder.Entity<General_Admin>(entity =>
        {
            entity.ToTable("General_Admin", tb => tb.HasComment("الإدارة العامة"));

            entity.Property(e => e.Address_Ar).HasComment("العنوان بالعربية");
            entity.Property(e => e.Address_En)
                .IsUnicode(false)
                .HasComment("العنوان بالانجليزية");
            entity.Property(e => e.Admin_ID).HasComment("رئيس/مدير الإدارة\r\nfrom HR employee table");
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Governate>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_LK_CountryCity");

            entity.ToTable("Governate", tb => tb.HasComment("المحافظة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group", tb => tb.HasComment("المجموعة الزراعيه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.SecClass).WithMany(p => p.Groups)
                .HasForeignKey(d => d.SecClass_ID)
                .HasConstraintName("FK_Group_SecondaryClassification");
        });

        modelBuilder.Entity<HagrContact>(entity =>
        {
            entity.ToTable("HagrContact");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ContactType_ID).HasComment("نوع وسيلة الاتصال");
            entity.Property(e => e.OutlitAdmin).HasComment("from systemcode table 5");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Value)
                .HasMaxLength(150)
                .HasComment("الرقم");

            entity.HasOne(d => d.ContactType).WithMany(p => p.HagrContacts)
                .HasForeignKey(d => d.ContactType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HagrContact_ContactType");

            entity.HasOne(d => d.OutlitAdminNavigation).WithMany(p => p.HagrContacts)
                .HasForeignKey(d => d.OutlitAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HagrContact_A_SystemCode");
        });

        modelBuilder.Entity<Im_CheckRequest>(entity =>
        {
            entity.ToTable("Im_CheckRequest", tb => tb.HasComment("طلب الفحص الوارد"));

            entity.HasIndex(e => e.CheckRequest_Number, "UQ_CheckRequest_Number").IsUnique();

            entity.HasIndex(e => new { e.User_Creation_Date, e.CheckRequest_Number, e.Outlet_ID, e.IsAccepted, e.ID }, "_dta_index_Im_CheckRequest_15_2006454372__K11_K3_K2_K8_K1");

            entity.HasIndex(e => new { e.ID, e.Outlet_ID }, "_dta_index_Im_CheckRequest_15_2006454372__K1_K2");

            entity.HasIndex(e => e.Outlet_ID, "_dta_index_Im_CheckRequest_15_2006454372__K2");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.CheckRequest_Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("رقم طلب الفحص");
            entity.Property(e => e.ExportCompany)
                .HasMaxLength(250)
                .HasComment("الشركة المصدرة");
            entity.Property(e => e.ExportCompanyAddress).HasComment("عنوان الشركة المصدرة");
            entity.Property(e => e.Im_OperationType).HasComment("نوع إذن الاستراد");
            entity.Property(e => e.IsAccepted_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Outlet).WithMany(p => p.Im_CheckRequests)
                .HasForeignKey(d => d.Outlet_ID)
                .HasConstraintName("FK_Im_CheckRequest_Outlet");
        });

        modelBuilder.Entity<Im_CheckRequestData_Extra>(entity =>
        {
            entity.ToTable("Im_CheckRequestData_Extra");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ImporeterCompanyAddress).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImporeterCompanyAddress_EN).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImportCompany).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImportCompany_EN).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OwnerAddress).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OwnerName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Reciever_Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequest_Data).WithMany(p => p.Im_CheckRequestData_Extras)
                .HasForeignKey(d => d.Im_CheckRequest_Data_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequestData_Extra_Im_CheckRequest_Data");
        });

        modelBuilder.Entity<Im_CheckRequest_Customs_Message>(entity =>
        {
            entity.ToTable("Im_CheckRequest_Customs_Message", tb => tb.HasComment("البيانات الجمركيه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Arrival_Date).HasComment("تاريخ الوصول ");
            entity.Property(e => e.Certificate_Number_Each_Product)
                .HasMaxLength(50)
                .HasComment("رقم الشهادة لكل منتج");
            entity.Property(e => e.Certification_Date).HasComment("تاريخ الشهاده الجمركيه");
            entity.Property(e => e.Customs_Certificate_Number)
                .HasMaxLength(50)
                .HasComment("رقم الشهاده الجمركيه");
            entity.Property(e => e.Im_CheckRequest_ID).HasComment("كود الطلب");
            entity.Property(e => e.Im_OperationType).HasComment("نوع الطلب");
            entity.Property(e => e.Manifest_Number).HasMaxLength(50);
            entity.Property(e => e.Shipment_Date).HasComment("تاريخ الشحن");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequest_Customs_Messages)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_Im_CheckRequest_Customs_Message_Im_CheckRequest");

            entity.HasOne(d => d.Shipping_Agency).WithMany(p => p.Im_CheckRequest_Customs_Messages)
                .HasForeignKey(d => d.Shipping_Agency_ID)
                .HasConstraintName("FK_Im_CheckRequest_Customs_Message_ShippingAgencies");
        });

        modelBuilder.Entity<Im_CheckRequest_Datum>(entity =>
        {
            entity.HasIndex(e => new { e.Importer_ID, e.ExportCountry_Id }, "_dta_index_Im_CheckRequest_Data_15_948406648__K2_K4_3_13");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.DelegateAddress)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DelegateName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Ship_Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TransitCountry_Id).HasComment("دولة العبور");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ExportCountry).WithMany(p => p.Im_CheckRequest_Data)
                .HasForeignKey(d => d.ExportCountry_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_Data_Country");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequest_Data)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_Im_CheckRequest_Data_Im_CheckRequest");

            entity.HasOne(d => d.InternationalTransportation).WithMany(p => p.Im_CheckRequest_Data)
                .HasForeignKey(d => d.InternationalTransportation_ID)
                .HasConstraintName("FK_Im_CheckRequest_Data_InternationalTransportation");

            entity.HasOne(d => d.Shipment_Mean).WithMany(p => p.Im_CheckRequest_Data)
                .HasForeignKey(d => d.Shipment_Mean_Id)
                .HasConstraintName("FK_Im_CheckRequest_Data_Shipment_Mean");

            entity.HasOne(d => d.ShippingCompanies).WithMany(p => p.Im_CheckRequest_Data)
                .HasForeignKey(d => d.ShippingCompanies_ID)
                .HasConstraintName("FK_Im_CheckRequest_Data_ShippingCompanies");

            entity.HasOne(d => d.Transport_Mean).WithMany(p => p.Im_CheckRequest_Data)
                .HasForeignKey(d => d.Transport_Mean_Id)
                .HasConstraintName("FK_Im_CheckRequest_Data_Transport_Mean");
        });

        modelBuilder.Entity<Im_CheckRequest_Distribution>(entity =>
        {
            entity.ToTable("Im_CheckRequest_Distribution");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date_Distribution).HasColumnType("datetime");
            entity.Property(e => e.GrossWeight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Net_Weight)
                .HasComment("الوزن الصافي")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequest_Distributions)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_Im_CheckRequest_Distribution_Im_CheckRequest");
        });

        modelBuilder.Entity<Im_CheckRequest_Final_Result>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_CheckRequest_Items_Final_Position");

            entity.ToTable("Im_CheckRequest_Final_Result", tb => tb.HasComment("الموقف النهائي لطلب الفحص"));

            entity.HasIndex(e => e.Im_CheckRequest_ID, "_dta_index_Im_CheckRequest_Final_Result_15_1966070190__K2");

            entity.HasIndex(e => new { e.Im_CheckRequest_ID, e.ID }, "_dta_index_Im_CheckRequest_Final_Result_15_1966070190__K2_K1");

            entity.HasIndex(e => new { e.Im_CheckRequest_ID, e.ID }, "_dta_index_Im_CheckRequest_Final_Result_15_1966070190__K2_K1_3");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Im_CheckRequest_ID).HasComment("جدول طلب الفحص الوارد");
            entity.Property(e => e.Im_Final_Result_ID).HasComment("جدول الموقف النهائي");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequest_Final_Results)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_Final_Result_Im_CheckRequest");

            entity.HasOne(d => d.Im_Final_Result).WithMany(p => p.Im_CheckRequest_Final_Results)
                .HasForeignKey(d => d.Im_Final_Result_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_Final_Position_Im_Final_Position");
        });

        modelBuilder.Entity<Im_CheckRequest_Item>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("نباتات طلب الفحص الوارد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Accept_User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Accept_User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Fees).HasColumnType("money");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.GrossWeight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Item_Permission_Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Net_Weight)
                .HasComment("الوزن الصافي")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Order_Text).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Package_Weight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequset_Shipping_Method).WithMany(p => p.Im_CheckRequest_Items)
                .HasForeignKey(d => d.Im_CheckRequset_Shipping_Method_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_IM_CheckRequset_shippingmethod");

            entity.HasOne(d => d.Im_Initiator).WithMany(p => p.Im_CheckRequest_Items)
                .HasForeignKey(d => d.Im_Initiator_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_Im_Initiator");
        });

        modelBuilder.Entity<Im_CheckRequest_Items_Lot_Category>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_CheckRequest_Items_Category");

            entity.ToTable("Im_CheckRequest_Items_Lot_Category", tb => tb.HasComment("تقسيم لوط وااصناف"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Based_Weight)
                .HasComment("مش مستخدم")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.GrossWeight)
                .HasComment("اجمالى الوزن القائم لللوطات")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Grower_Number)
                .HasMaxLength(50)
                .HasComment("رقم المزرعه من الصادر ,وبعض الحالات من الوارد");
            entity.Property(e => e.IsAccepted).HasComment("مقبول = 1 / مرفوض =0");
            entity.Property(e => e.Lot_Number)
                .HasMaxLength(50)
                .HasComment("رقم اللوط");
            entity.Property(e => e.Net_Weight)
                .HasComment("الوزن الصافي لللوطات")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Number_Wooden_Package)
                .HasMaxLength(50)
                .HasComment("عدد وحدات التعبئه الخشبيه ");
            entity.Property(e => e.Order_Text).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Package_Based_Weight)
                .HasComment("وزن العبوة القائم")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Package_Count).HasComment("عدد العبوات");
            entity.Property(e => e.Package_Material_ID).HasComment("مادة العبوة");
            entity.Property(e => e.Package_Net_Weight)
                .HasComment("وزن العبوة الفارغ")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Package_Type_ID).HasComment("نوع العبوة");
            entity.Property(e => e.Package_Weight)
                .HasComment("الوزن العبوه الصافي")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Reason_Entry)
                .HasComment("سبب الدخول")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RejectReason)
                .HasMaxLength(300)
                .HasComment("اسباب الرفض");
            entity.Property(e => e.Units_Number).HasComment("عدد الوحدات");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Waybill)
                .HasMaxLength(100)
                .HasComment("رقم بوليصه الشحن");

            entity.HasOne(d => d.Im_CheckRequest_Items).WithMany(p => p.Im_CheckRequest_Items_Lot_Categories)
                .HasForeignKey(d => d.Im_CheckRequest_Items_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_Category_Im_CheckRequest_Items");

            entity.HasOne(d => d.ItemCategory).WithMany(p => p.Im_CheckRequest_Items_Lot_Categories)
                .HasForeignKey(d => d.ItemCategory_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_Category_ItemCategories");
        });

        modelBuilder.Entity<Im_CheckRequest_Items_Lot_Result>(entity =>
        {
            entity.ToTable("Im_CheckRequest_Items_Lot_Result", tb => tb.HasComment("نتيجه اللوط"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IS_Status).HasComment("الموقف مقبول او مرفوض");
            entity.Property(e => e.IS_Status_Committee).HasComment("الموقف مقبول او مرفوض");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.IS_StatusNavigation).WithMany(p => p.Im_CheckRequest_Items_Lot_Results)
                .HasForeignKey(d => d.IS_Status)
                .HasConstraintName("FK_Im_CheckRequest_Items_Lot_Result_Im_CheckRequest_Lot_Result_Status");
        });

        modelBuilder.Entity<Im_CheckRequest_Lot_Result_Status>(entity =>
        {
            entity.ToTable("Im_CheckRequest_Lot_Result_Status");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Is_Continue).HasComment("0 عدم استكمال الاعمال\r\nلا يمكن استكمال الاعمال 1");
        });

        modelBuilder.Entity<Im_CheckRequest_Manafest>(entity =>
        {
            entity.ToTable("Im_CheckRequest_Manafest");

            entity.Property(e => e.ID).ValueGeneratedNever();

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequest_Manafests)
                .HasForeignKey(d => d.Im_CheckRequest_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_Manafest_Im_CheckRequest");

            entity.HasOne(d => d.Im_ManafestNavigation).WithMany(p => p.Im_CheckRequest_Manafests)
                .HasForeignKey(d => d.Im_Manafest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_Manafest_Im_Manafest");
        });

        modelBuilder.Entity<Im_CheckRequest_Port>(entity =>
        {
            entity.ToTable("Im_CheckRequest_Port");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsNational).HasComment("دولية ولا لا");
            entity.Property(e => e.Port_ID).HasComment("رقم الميناء");
            entity.Property(e => e.Port_Type_ID).HasComment("نوع المينا بحرى جوى مطار");
            entity.Property(e => e.ReqPortType_ID).HasComment("ميناء شحن وصور وعبور");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_CheckRequest_RefuseReason>(entity =>
        {
            entity.ToTable("Im_CheckRequest_RefuseReason", tb => tb.HasComment("اسباب رفض الطلب وارد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequest_RefuseReasons)
                .HasForeignKey(d => d.Im_CheckRequest_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_RefuseReason_Im_CheckRequest");

            entity.HasOne(d => d.Refuse_Reason).WithMany(p => p.Im_CheckRequest_RefuseReasons)
                .HasForeignKey(d => d.Refuse_Reason_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_RefuseReason_Refuse_Reason");
        });

        modelBuilder.Entity<Im_CheckRequest_SampleData_Confirm>(entity =>
        {
            entity.ToTable("Im_CheckRequest_SampleData_Confirm", tb => tb.HasComment("نتيجه سحب عينه للمساعد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Im_CheckRequest_SampleData).WithMany(p => p.Im_CheckRequest_SampleData_Confirms)
                .HasForeignKey(d => d.Im_CheckRequest_SampleData_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_SampleData_Confirm_Im_CheckRequest_SampleData");
        });

        modelBuilder.Entity<Im_CheckRequest_SampleDatum>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("نتيجه سحب عينه للوارد (admin)"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Admin_Confirmation).HasComment("موقف الحجر");
            entity.Property(e => e.Admin_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Admin_User).HasComment("ادمن الحجر");
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.IS_From_Android)
                .HasDefaultValue(false)
                .HasComment("مين رمي row (system or android)");
            entity.Property(e => e.IS_Total).HasComment("(0) in the case of all,(1) in the case of the part   في حاله الجزئي او الكلي");
            entity.Property(e => e.IS_Total_Android).HasComment("في حاله الفحص لو كلي واتحول الي جزئي");
            entity.Property(e => e.Im_RequestCommittee_ID).HasComment("كود اساسيات اللجنه");
            entity.Property(e => e.IsAccepted).HasComment("0 if rejected else 1 \r\nموافقه المعمل");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Item_ShortName_ID).HasComment("الاسم المختصر ");
            entity.Property(e => e.LotData_ID).HasComment("null for the whole request  /  كود بيانات الدفعه");
            entity.Property(e => e.Notes_Ar)
                .HasMaxLength(300)
                .HasComment("ملاحظات الاندريد");
            entity.Property(e => e.Notes_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.RejectReason_Ar)
                .HasMaxLength(150)
                .HasComment("سبب الرفض للمعمل ar\r\n");
            entity.Property(e => e.RejectReason_En)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("سبب الرفض للمعمل en");
            entity.Property(e => e.SampleRatio).HasComment("نسبة اخذ العينة");
            entity.Property(e => e.SampleSize).HasComment("حجم العينة");
            entity.Property(e => e.Sample_BarCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("البار كود");
            entity.Property(e => e.Syl_ALkhatima_Number).HasComment("رقم الختامه والسيل الملاحي");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.WithdrawDate).HasComment("تاريخ سحب العينة");

            entity.HasOne(d => d.AnalysisLabType).WithMany(p => p.Im_CheckRequest_SampleData)
                .HasForeignKey(d => d.AnalysisLabType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_SampleData_AnalysisLabType");

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_CheckRequest_SampleData)
                .HasForeignKey(d => d.Im_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CheckRequest_SampleData_Im_RequestCommittee");
        });

        modelBuilder.Entity<Im_CheckRequest_Visa>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_CheckRequest_Items_Lot_Visa");

            entity.ToTable("Im_CheckRequest_Visa", tb => tb.HasComment("عدد التاشيرات علي طلب الفحص"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Im_CheckRequest_ID).HasComment("جدول طلب الفحص الوارد");
            entity.Property(e => e.Im_Visa_ID).HasComment("جدول التاشيره");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequest_Visas)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_Lot_Visa_Im_CheckRequest");

            entity.HasOne(d => d.Im_Visa).WithMany(p => p.Im_CheckRequest_Visas)
                .HasForeignKey(d => d.Im_Visa_ID)
                .HasConstraintName("FK_Im_CheckRequest_Items_Lot_Visa_Im_Visa");
        });

        modelBuilder.Entity<Im_CheckRequset_Shipping_Method>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_IM_CheckRequset_shippingmethod");

            entity.ToTable("Im_CheckRequset_Shipping_Method", tb => tb.HasComment("اساليب الشحن لطلب الفحص الوارد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ContainerNumber)
                .HasMaxLength(50)
                .HasComment("رقم الحاوية");
            entity.Property(e => e.NavigationalNumber)
                .HasMaxLength(50)
                .HasComment("رقم السيل الملاحي");
            entity.Property(e => e.ShipholdNumber)
                .HasMaxLength(50)
                .HasComment("رقم عنبر السفينة");
            entity.Property(e => e.Total_Weight).HasColumnType("decimal(22, 6)");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.containers_ID).HasComment("حاوية او صب");
            entity.Property(e => e.containers_type_ID).HasComment("عبوات او بدون");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CheckRequset_Shipping_Methods)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_IM_CheckRequset_shippingmethod_Im_CheckRequest");
        });

        modelBuilder.Entity<Im_CommitteeCheckLocation>(entity =>
        {
            entity.ToTable("Im_CommitteeCheckLocation", tb => tb.HasComment("أماكن الفحص التي يحددها المنفذ"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Im_CommitteeResult>(entity =>
        {
            entity.ToTable("Im_CommitteeResult", tb => tb.HasComment("النتيجه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.AdminFinalResult_Note).HasComment("Admin Note");
            entity.Property(e => e.Committee_ID).HasComment("كود اساسيات اللجنه");
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IS_Total).HasComment("(0) in the case of all,(1) in the case of the part   في حاله الجزئي او الكلي");
            entity.Property(e => e.IS_Total_Android).HasComment("في حاله الفحص لو كلي واتحول الي جزئي");
            entity.Property(e => e.IsAdminResult).HasComment("null->exporter not take action 0 if exporter doesn't accept else 1");
            entity.Property(e => e.Item_ShortName_ID).HasComment("الاسم المختصر");
            entity.Property(e => e.LotData_ID).HasComment("null for the whole request  /  كود بيانات الدفعه");
            entity.Property(e => e.QuantitySize).HasComment("العدد");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Weight).HasComment("الوزن");

            entity.HasOne(d => d.CommitteeResultType).WithMany(p => p.Im_CommitteeResults)
                .HasForeignKey(d => d.CommitteeResultType_ID)
                .HasConstraintName("FK_Im_CommitteeResult_CommitteeResultType");

            entity.HasOne(d => d.Committee).WithMany(p => p.Im_CommitteeResults)
                .HasForeignKey(d => d.Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CommitteeResult_Im_RequestCommittee");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Im_CommitteeResults)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .HasConstraintName("FK_Im_CommitteeResult_Item_ShortName");
        });

        modelBuilder.Entity<Im_CommitteeResult_Confirm>(entity =>
        {
            entity.ToTable("Im_CommitteeResult_Confirm", tb => tb.HasComment("راي المساعد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);

            entity.HasOne(d => d.Im_CommitteeResult).WithMany(p => p.Im_CommitteeResult_Confirms)
                .HasForeignKey(d => d.Im_CommitteeResult_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CommitteeResult_Confirm_Im_CommitteeResult");
        });

        modelBuilder.Entity<Im_CommitteeResult_Infection>(entity =>
        {
            entity.ToTable("Im_CommitteeResult_Infection", tb => tb.HasComment("الاصابه للفحص"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CommitteeResult).WithMany(p => p.Im_CommitteeResult_Infections)
                .HasForeignKey(d => d.Im_CommitteeResult_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CommitteeResult_Infection_Im_CommitteeResult");

            entity.HasOne(d => d.Item).WithMany(p => p.Im_CommitteeResult_Infections)
                .HasForeignKey(d => d.Item_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CommitteeResult_Infection_Item");
        });

        modelBuilder.Entity<Im_Committee_CustodyPlace>(entity =>
        {
            entity.ToTable("Im_Committee_CustodyPlace", tb => tb.HasComment("لجنة معاينة مكان التحفظ"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Check_Date).HasComment("تاريخ الفحص");
            entity.Property(e => e.EndTime).HasComment("انتهاء ساعة الفحص");
            entity.Property(e => e.IsApproved).HasComment("0 if exporter doesn't accept else 1");
            entity.Property(e => e.IsPackage).HasComment("هل حاوية أم لا");
            entity.Property(e => e.Quantity).HasComment("العدد");
            entity.Property(e => e.StartTime).HasComment(" بداية ساعة الفحص ");
            entity.Property(e => e.Status).HasComment("0 if not done, 1 if investigation is done");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Weight).HasComment("الوزن");

            entity.HasOne(d => d.Im_CustodyPlace).WithMany(p => p.Im_Committee_CustodyPlaces)
                .HasForeignKey(d => d.Im_CustodyPlace_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_Committee_CustodyPlace_Im_CustodyPlace");
        });

        modelBuilder.Entity<Im_Constrain_Initiator_Text>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_Constrain_Text");

            entity.ToTable("Im_Constrain_Initiator_Text");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.IsActive1)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ConstrainText).WithMany(p => p.Im_Constrain_Initiator_Texts)
                .HasForeignKey(d => d.ConstrainText_ID)
                .HasConstraintName("FK_Im_Constrain_Text_Im_CountryConstrain_Text");

            entity.HasOne(d => d.Im_Initiator).WithMany(p => p.Im_Constrain_Initiator_Texts)
                .HasForeignKey(d => d.Im_Initiator_ID)
                .HasConstraintName("FK_Im_Constrain_Text_Im_Initiator");
        });

        modelBuilder.Entity<Im_Constrain_Type>(entity =>
        {
            entity.ToTable("Im_Constrain_Type", tb => tb.HasComment("انواع الاشتراطات"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مفعل");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_Constrains_Special>(entity =>
        {
            entity.ToTable("Im_Constrains_Special");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ConstrainText_En).IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_CountryConstrain_ArrivalPort>(entity =>
        {
            entity.ToTable("Im_CountryConstrain_ArrivalPort", tb => tb.HasComment("موانى تحديد ميناء وصول معين"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Id_QualitativeGroupNavigation).WithMany(p => p.Im_CountryConstrain_ArrivalPorts)
                .HasForeignKey(d => d.Id_QualitativeGroup)
                .HasConstraintName("FK_Im_CountryConstrain_ArrivalPort_QualitativeGroup");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Im_CountryConstrain_ArrivalPorts)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .HasConstraintName("FK_Im_CountryConstrain_ArrivalPort_Plant_ShortName");

            entity.HasOne(d => d.Port_National).WithMany(p => p.Im_CountryConstrain_ArrivalPorts)
                .HasForeignKey(d => d.Port_National_Id)
                .HasConstraintName("FK_Im_CountryConstrain_ArrivalPort_PortNational");
        });

        modelBuilder.Entity<Im_CountryConstrain_Text>(entity =>
        {
            entity.ToTable("Im_CountryConstrain_Text");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ConstrainText_En).IsUnicode(false);
            entity.Property(e => e.InSide_Certificate_En).IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_Constrain_Type).WithMany(p => p.Im_CountryConstrain_Texts)
                .HasForeignKey(d => d.Im_Constrain_Type_ID)
                .HasConstraintName("FK_Im_CountryConstrain_Text_Im_Constrain_Type");
        });

        modelBuilder.Entity<Im_CustodyPlace>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_CustodyPlace_1");

            entity.ToTable("Im_CustodyPlace", tb => tb.HasComment("اماكن التحفظ"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Ar_Desc).HasComment("الوصف عربى");
            entity.Property(e => e.DateStored)
                .HasComment("كمية/تاريخ")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.En_Desc)
                .IsUnicode(false)
                .HasComment("الوصف انجليزى");
            entity.Property(e => e.Im_CustodyPlaceType).HasComment("مخزن/ساحة");
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false)
                .HasComment("0 if exporter doesn't accept else 1");
            entity.Property(e => e.NationalID)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Owner_Name).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Quantity).HasComment("الكمية المخزنة/التاريخ");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasComment("0 if not done, 1 if investigation is done");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CustodyPlaceTypeNavigation).WithMany(p => p.Im_CustodyPlaces)
                .HasForeignKey(d => d.Im_CustodyPlaceType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CustodyPlace_Im_CustodyPlaceType");
        });

        modelBuilder.Entity<Im_CustodyPlaceType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_CustodyPlace");

            entity.ToTable("Im_CustodyPlaceType", tb => tb.HasComment("نوع مكان التحفظ"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_CustodyPlace_CheckRequest>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_CustodyPlace");

            entity.ToTable("Im_CustodyPlace_CheckRequest", tb => tb.HasComment("اماكن التحفظ للطلب"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Im_CheckRequest_ID).HasComment("رقم اذن الاستيراد");
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false)
                .HasComment("0 if exporter doesn't accept else 1");
            entity.Property(e => e.Station_ID).HasComment("المحطة");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasComment("0 if not done, 1 if investigation is done");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_CustodyPlace_CheckRequests)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_CustodyPlace_Im_CheckRequest");

            entity.HasOne(d => d.Im_CustodyPlace).WithMany(p => p.Im_CustodyPlace_CheckRequests)
                .HasForeignKey(d => d.Im_CustodyPlace_ID)
                .HasConstraintName("FK_Im_CustodyPlace_CheckRequest_Im_CustodyPlace");

            entity.HasOne(d => d.Station).WithMany(p => p.Im_CustodyPlace_CheckRequests)
                .HasForeignKey(d => d.Station_ID)
                .HasConstraintName("FK_Im_CustodyPlace_CheckRequest_Station");
        });

        modelBuilder.Entity<Im_Execution>(entity =>
        {
            entity.ToTable("Im_Execution", tb => tb.HasComment("لجنة الاعدام"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Execution_File)
                .HasMaxLength(8000)
                .IsFixedLength();
            entity.Property(e => e.Execution_Method).HasMaxLength(250);
            entity.Property(e => e.Execution_Place).HasMaxLength(250);

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_Executions)
                .HasForeignKey(d => d.Im_RequestCommittee_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_Execution_Im_RequestCommittee");
        });

        modelBuilder.Entity<Im_Execution_Item>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("عناصر لجنة الاعدام"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.GrossWeight).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Im_CheckRequest_Item).WithMany(p => p.Im_Execution_Items)
                .HasForeignKey(d => d.Im_CheckRequest_Item_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Im_Execut__Im_Ch__7B48A5B7");

            entity.HasOne(d => d.Im_CheckRequest_Items_Lot_Category).WithMany(p => p.Im_Execution_Items)
                .HasForeignKey(d => d.Im_CheckRequest_Items_Lot_Category_ID)
                .HasConstraintName("FK__Im_Execut__Im_Ch__7C3CC9F0");

            entity.HasOne(d => d.Im_Execution).WithMany(p => p.Im_Execution_Items)
                .HasForeignKey(d => d.Im_Execution_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_Execution_Items_Im_Execution");
        });

        modelBuilder.Entity<Im_Final_Result>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_Final_Position");

            entity.ToTable("Im_Final_Result", tb => tb.HasComment("الموقف النهائي لطلب الفحص"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name).HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasComment("الطلب فعال ام لا");
            entity.Property(e => e.Status).HasComment("تم ايقاف الطلب ام لا\r\n0 = مرفوض\r\n1 = مقبول");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_Initiator>(entity =>
        {
            entity.ToTable("Im_Initiator", tb => tb.HasComment("المناشىء"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ForbiddenReason).HasMaxLength(150);
            entity.Property(e => e.Initiator_Status).HasComment("حالة المنشأ\r\nfrom systemcode 16\r\n");
            entity.Property(e => e.IsActive).HasComment("");
            entity.Property(e => e.QualitativeGroup_Id).HasComment("المجموعة النوعية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Im_Initiators)
                .HasForeignKey(d => d.Country_Id)
                .HasConstraintName("FK_Im_Initiator_Country");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Im_Initiators)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .HasConstraintName("FK_Im_Initiator_Plant_ShortName");
        });

        modelBuilder.Entity<Im_ItemsLotDivision>(entity =>
        {
            entity.ToTable("Im_ItemsLotDivision", tb => tb.HasComment("تفاصيل اللوط"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Container_Number)
                .HasMaxLength(50)
                .HasComment("رقم الحاوية");
            entity.Property(e => e.Gross_Weight)
                .HasComment("الوزن القائم")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsAccepted).HasComment("مقبول = 1 / مرفوض =0");
            entity.Property(e => e.Lot_Number)
                .HasMaxLength(50)
                .HasComment("رقم اللوط");
            entity.Property(e => e.NavigationalFluid_Number)
                .HasMaxLength(50)
                .HasComment("رقم السيل الملاحي");
            entity.Property(e => e.Net_Weight)
                .HasComment("الوزن الصافي")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Package_Count).HasComment("عدد العبوات");
            entity.Property(e => e.Package_Material_ID).HasComment("مادة العبوة");
            entity.Property(e => e.Package_Type_ID).HasComment("نوع العبوة");
            entity.Property(e => e.Package_Weight)
                .HasComment("وزن العبوة")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RejectReason).HasMaxLength(300);
            entity.Property(e => e.ShipmentPolicy_Number)
                .HasMaxLength(50)
                .HasComment("رقم بوليصة الشحن");

            entity.HasOne(d => d.Im_PermissionItems).WithMany(p => p.Im_ItemsLotDivisions)
                .HasForeignKey(d => d.Im_PermissionItems_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ItemsLotDivision_Im_PermissionItems");
        });

        modelBuilder.Entity<Im_Manafest>(entity =>
        {
            entity.ToTable("Im_Manafest", tb => tb.HasComment("منافستو"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ArriveDate).HasComment("تاريخ الوصول");
            entity.Property(e => e.ArriveTime).HasComment("وقت الوصول");
            entity.Property(e => e.CompletionApplicationNum)
                .HasMaxLength(50)
                .HasComment("رقم طلب الاتمام");
            entity.Property(e => e.CustomsCertificate)
                .HasMaxLength(50)
                .HasComment("رقم الشهادة الجمركية");
            entity.Property(e => e.DischargeEndDate).HasComment("تاريخ نهاية التفريغ");
            entity.Property(e => e.EditRecord).HasComment("بيان التعديلات");
            entity.Property(e => e.ExaminationDate).HasComment("تاريخ تسديد الرسالة");
            entity.Property(e => e.GrossWeight)
                .HasComment("الوزن القائم")
                .HasColumnType("numeric(5, 3)");
            entity.Property(e => e.ImporterName)
                .HasMaxLength(50)
                .HasComment("اسم المستورد");
            entity.Property(e => e.IsTransit).HasComment("ترانزيت أم لا");
            entity.Property(e => e.Manafest_Num)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("رقم المنافيست");
            entity.Property(e => e.NavigationCompany)
                .HasMaxLength(50)
                .HasComment("شركة الملاحة");
            entity.Property(e => e.Net_Weight)
                .HasComment("الوزن الصافي")
                .HasColumnType("numeric(5, 3)");
            entity.Property(e => e.Origin)
                .HasMaxLength(50)
                .HasComment("المنشأ(دولة)");
            entity.Property(e => e.PlantName)
                .HasMaxLength(50)
                .HasComment("اسم الصنف");
            entity.Property(e => e.PolicyNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("رقم البوليصة");
            entity.Property(e => e.Quantity).HasComment("العدد");
            entity.Property(e => e.ShipName)
                .HasMaxLength(50)
                .HasComment("اسم الباخرة");
            entity.Property(e => e.ShipmentPort)
                .HasMaxLength(50)
                .HasComment("ميناء الشحن");
            entity.Property(e => e.SubmissionDate).HasComment("تاريخ تقديم المنافيست");
            entity.Property(e => e.ToHagrDate).HasComment("تاريخ التقدم للمنافيستو للحجر");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasComment("الوحدة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_OpertaionType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ImportOpertaionType");

            entity.ToTable("Im_OpertaionType", tb => tb.HasComment("أنواع عمليات الوارد"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.withPermission).HasComment("1 = permission request / 2 = check request / 3 = both permission and check request");
        });

        modelBuilder.Entity<Im_PermissionItem>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("النباتات والمنتجات للوارد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Accept_User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Accept_User_Creation_Id).HasComment("الموظف الذى وافق على الصنف وهذا ليس له علاقة بمن ادخل الصنف( يقرا من الاذن نفسه)");
            entity.Property(e => e.Accept_User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Country_ID).HasComment("ConstrainOwner(UnionId/CountryId/ or 0 if Local-Egypt)");
            entity.Property(e => e.GrossWeight)
                .HasComment("الوزن الاجمالى")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Im_Initiator_ID).HasComment("دوله المنشا");
            entity.Property(e => e.IsAccepted).HasComment("هل تم الموافقة على الصنف");
            entity.Property(e => e.Is_LotDivision).HasComment("1 if divided lots /0 if Sub");
            entity.Property(e => e.Item_Permission_Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("رقم اذن الاستيراد يأخذ رقم عند الموافقة");
            entity.Property(e => e.Order_Text).HasComment("الرتبة");
            entity.Property(e => e.Package_Count).HasComment("عدد العبوات");
            entity.Property(e => e.Package_Material_ID).HasComment("مادة العبوة");
            entity.Property(e => e.Package_Type_ID).HasComment("نوع العبوة");
            entity.Property(e => e.Package_Weight)
                .HasComment("وزن العبوة")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Size).HasComment("حجم الرسالة");
            entity.Property(e => e.SubPart_id).HasComment("جزء نباتى");
            entity.Property(e => e.Units_Number).HasComment("عدد الوحدات");

            entity.HasOne(d => d.Im_Initiator).WithMany(p => p.Im_PermissionItems)
                .HasForeignKey(d => d.Im_Initiator_ID)
                .HasConstraintName("FK_Im_PermissionItems_Im_Initiator");
        });

        modelBuilder.Entity<Im_PermissionItem_Division_Custody>(entity =>
        {
            entity.ToTable("Im_PermissionItem_Division_Custody", tb => tb.HasComment("تقسيم/ نقل الى مكان التحفظ"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Accept_Date).HasComment("تاريخ القبول");
            entity.Property(e => e.Accept_User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Accept_User_Creation_Id).HasComment("الموظف الذى وافق على الصنف وهذا ليس له علاقة بمن ادخل الصنف( يقرا من الاذن نفسه)");
            entity.Property(e => e.Accept_User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Driver_Name)
                .HasMaxLength(250)
                .HasComment("اسم السائق");
            entity.Property(e => e.Driver_National_Id)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Driver_Phone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("رقم تليفون السائق");
            entity.Property(e => e.GrossWeight)
                .HasComment("الوزن الاجمالي")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Im_CustodyPlace_Id).HasComment("كود اماكن التحفظ");
            entity.Property(e => e.IsAccepted)
                .HasDefaultValue(false)
                .HasComment("هل تم الموافقة على التقسيم\r\nحالة الطلب");
            entity.Property(e => e.Transport_Mean_Id).HasComment("كود وسائل النقل");
            entity.Property(e => e.Transport_Mean_Number).HasMaxLength(50);

            entity.HasOne(d => d.Im_CheckRequest_Item).WithMany(p => p.Im_PermissionItem_Division_Custodies)
                .HasForeignKey(d => d.Im_CheckRequest_Item_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionItem_Division_Custody_Im_CheckRequest_Items");

            entity.HasOne(d => d.Im_CustodyPlace).WithMany(p => p.Im_PermissionItem_Division_Custodies)
                .HasForeignKey(d => d.Im_CustodyPlace_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionItem_Division_Custody_Im_CustodyPlace");

            entity.HasOne(d => d.Transport_Mean).WithMany(p => p.Im_PermissionItem_Division_Custodies)
                .HasForeignKey(d => d.Transport_Mean_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionItem_Division_Custody_Transport_Mean");
        });

        modelBuilder.Entity<Im_PermissionItem_Division_Custody_DismissCommittee>(entity =>
        {
            entity.ToTable("Im_PermissionItem_Division_Custody_DismissCommittee", tb => tb.HasComment("لجنة الصرف"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.DismissTime).HasComment("وقت الخروج");
            entity.Property(e => e.Dismiss_Date).HasComment("تاريخ الخروج");
            entity.Property(e => e.Im_PermissionItem_Division_Custody_Id).HasComment("كود نقل مكان التحفظ");
            entity.Property(e => e.Im_RequestCommittee_Id).HasComment("كود اساسيات اللجنه");
            entity.Property(e => e.IsApproved).HasComment("1 if committe accept else 0");
            entity.Property(e => e.Lock_Lead)
                .HasMaxLength(50)
                .HasComment("ترصيص");
            entity.Property(e => e.Status).HasComment("1 if investigation is done,0 if not done\r\nif car is come or not");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_PermissionItem_Division_Custody).WithMany(p => p.Im_PermissionItem_Division_Custody_DismissCommittees)
                .HasForeignKey(d => d.Im_PermissionItem_Division_Custody_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionItem_Division_Custody_DismissCommittee_Im_PermissionItem_Division_Custody");

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_PermissionItem_Division_Custody_DismissCommittees)
                .HasForeignKey(d => d.Im_RequestCommittee_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionItem_Division_Custody_DismissCommittee_Im_RequestCommittee");
        });

        modelBuilder.Entity<Im_PermissionItem_Division_Custody_ReceiveCommittee>(entity =>
        {
            entity.ToTable("Im_PermissionItem_Division_Custody_ReceiveCommittee", tb => tb.HasComment("لجنة الاستلام"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.GrossWeight)
                .HasComment("الوزن الاجمالي")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Im_PermissionItem_Division_Custody_DismissCommittee_Id).HasComment("كود لجنه الصرف");
            entity.Property(e => e.Im_RequestCommittee_Id).HasComment("كود اساسيات اللجنه");
            entity.Property(e => e.IsApproved).HasComment("0 if exporter doesn't accept else 1");
            entity.Property(e => e.ReceiveTime).HasComment("وقت الاستلام");
            entity.Property(e => e.Receive_Date).HasComment("تاريخ الاستلام");
            entity.Property(e => e.Status).HasComment("0 if not done, 1 if investigation is done");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_PermissionItem_Division_Custody_DismissCommittee).WithMany(p => p.Im_PermissionItem_Division_Custody_ReceiveCommittees)
                .HasForeignKey(d => d.Im_PermissionItem_Division_Custody_DismissCommittee_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionItem_Division_Custody_ReceiveCommittee_Im_PermissionItem_Division_Custody_DismissCommittee");

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_PermissionItem_Division_Custody_ReceiveCommittees)
                .HasForeignKey(d => d.Im_RequestCommittee_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionItem_Division_Custody_ReceiveCommittee_Im_RequestCommittee");
        });

        modelBuilder.Entity<Im_PermissionItems_Category>(entity =>
        {
            entity.ToTable("Im_PermissionItems_Category");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.GrossWeight)
                .HasComment("الوزن الاجمالى")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Order_Text).HasComment("الرتبة");
            entity.Property(e => e.Package_Count).HasComment("عدد العبوات");
            entity.Property(e => e.Package_Material_ID).HasComment("مادة العبوة");
            entity.Property(e => e.Package_Type_ID).HasComment("نوع العبوة");
            entity.Property(e => e.Package_Weight)
                .HasComment("وزن العبوة")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Reason_Entry).HasComment("سبب الدخول");
            entity.Property(e => e.Size).HasComment("حجم الرسالة");
            entity.Property(e => e.Units_Number).HasComment("عدد الوحدات");

            entity.HasOne(d => d.Im_PermissionItems).WithMany(p => p.Im_PermissionItems_Categories)
                .HasForeignKey(d => d.Im_PermissionItems_ID)
                .HasConstraintName("FK_Im_PermissionItems_Category_Im_PermissionItems");

            entity.HasOne(d => d.ItemCategoryGroup).WithMany(p => p.Im_PermissionItems_Categories)
                .HasForeignKey(d => d.ItemCategoryGroup_ID)
                .HasConstraintName("FK_Im_PermissionItems_Category_ItemCategories_Group");

            entity.HasOne(d => d.ItemCategory).WithMany(p => p.Im_PermissionItems_Categories)
                .HasForeignKey(d => d.ItemCategory_ID)
                .HasConstraintName("FK_Im_PermissionItems_Category_ItemCategories");
        });

        modelBuilder.Entity<Im_PermissionRequest>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Import_PermissionRequest");

            entity.ToTable("Im_PermissionRequest", tb => tb.HasComment("إذن استيراد"));

            entity.HasIndex(e => e.ImPermission_Number, "Im_PermissionRequest_uk").IsUnique();

            entity.HasIndex(e => e.ImPermission_Number, "UQ_ImPermission_Number").IsUnique();

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Arrival_Date).HasComment("تاريخ وصول الشحنة");
            entity.Property(e => e.End_Date).HasComment("تاريخ اخر طباعه");
            entity.Property(e => e.ImPermission_Number).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Print_Count).HasComment("عدد مرات التجديد");
            entity.Property(e => e.Renewal_Status).HasComment("حاله التجديد");
            entity.Property(e => e.Start_Date).HasComment("تاريخ الاصدار");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_CheckRequest).WithMany(p => p.Im_PermissionRequests)
                .HasForeignKey(d => d.Im_CheckRequest_ID)
                .HasConstraintName("FK_Im_PermissionRequest_Im_CheckRequest");
        });

        modelBuilder.Entity<Im_PermissionRequest_History>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Im_PermissionRequest_History", tb => tb.HasComment("إذن استيرادوالتجديد"));

            entity.Property(e => e.Arrival_Date).HasComment("تاريخ وصول الشحنة");
            entity.Property(e => e.End_Date).HasComment("تاريخ اخر طباعه");
            entity.Property(e => e.ImPermission_Number).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Print_Count).HasComment("عدد مرات التجديد");
            entity.Property(e => e.Renewal_Status).HasComment("حاله التجديد");
            entity.Property(e => e.Start_Date).HasComment("تاريخ الاصدار");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_PermissionRequest_RefuseReason>(entity =>
        {
            entity.ToTable("Im_PermissionRequest_RefuseReason");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_PermissionRequest).WithMany(p => p.Im_PermissionRequest_RefuseReasons)
                .HasForeignKey(d => d.Im_PermissionRequest_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionRequest_RefuseReason_Im_PermissionRequest1");

            entity.HasOne(d => d.Refuse_Reason).WithMany(p => p.Im_PermissionRequest_RefuseReasons)
                .HasForeignKey(d => d.Refuse_Reason_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_PermissionRequest_RefuseReason_Refuse_Reason");
        });

        modelBuilder.Entity<Im_ProcedureType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_LotProcedure");

            entity.ToTable("Im_ProcedureType", tb => tb.HasComment("إجراءات تتم على اللوط (نقل تحت تحفظ/فحص/تحاليل/...)"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_RequestCommittee>(entity =>
        {
            entity.ToTable("Im_RequestCommittee", tb => tb.HasComment("اساسيات اللجان"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.CommitteeType_ID).HasComment("كود الغرض من اللجنه");
            entity.Property(e => e.Delegation_Date).HasComment("تاريخ الانتداب");
            entity.Property(e => e.EndTime).HasComment("انتهاء ساعة الفحص");
            entity.Property(e => e.ImCommitteeCheckLocation_ID).HasComment("كود اماكن الفحص");
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false)
                .HasComment("0 if exporter doesn't accept else 1 خاص ب نتيجه الفحص");
            entity.Property(e => e.IsFinishedAll)
                .HasDefaultValue(false)
                .HasComment("0 if exporter doesn't accept else 1 خاص ب شغل موظف الحجر في فحص الشحنه");
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.Is_Cancel).HasComment("لايقاف او حذف اللجنة مربوط مع  A_SystemCode رقم 31\r\n");
            entity.Property(e => e.Is_Start_Android).HasComment("تعزر عمل اللجنه");
            entity.Property(e => e.StartTime).HasComment(" بداية ساعة الفحص ");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasComment("0 if not done, 1 if investigation is done تم نزول اللجنه 1 ,0 وافق علي معاد اللجنه , null خاص ب العميل عدم الرد عل معاد اللجنه , ");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CommitteeType).WithMany(p => p.Im_RequestCommittees)
                .HasForeignKey(d => d.CommitteeType_ID)
                .HasConstraintName("FK_Im_RequestCommittee_CommitteeType");

            entity.HasOne(d => d.ImCheckRequest).WithMany(p => p.Im_RequestCommittees)
                .HasForeignKey(d => d.ImCheckRequest_ID)
                .HasConstraintName("FK_Im_RequestCommittee_Im_CheckRequest");

            entity.HasOne(d => d.ImCommitteeCheckLocation).WithMany(p => p.Im_RequestCommittees)
                .HasForeignKey(d => d.ImCommitteeCheckLocation_ID)
                .HasConstraintName("FK_Im_RequestCommittee_Im_CommitteeCheckLocation");
        });

        modelBuilder.Entity<Im_RequestCommittee_Procedure>(entity =>
        {
            entity.ToTable("Im_RequestCommittee_Procedure", tb => tb.HasComment("اجراءات اللجنة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Im_ProcedureType_ID).HasComment("إجراءات تتم على اللوط (نقل تحت تحفظ/فحص/تحاليل/...)");
            entity.Property(e => e.Reason_Text)
                .HasMaxLength(300)
                .HasComment("السبب");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_ProcedureType).WithMany(p => p.Im_RequestCommittee_Procedures)
                .HasForeignKey(d => d.Im_ProcedureType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_RequestCommittee_Procedure_Im_ProcedureType");

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_RequestCommittee_Procedures)
                .HasForeignKey(d => d.Im_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_RequestCommittee_Procedure_Im_RequestCommittee");
        });

        modelBuilder.Entity<Im_RequestCommittee_Shift>(entity =>
        {
            entity.ToTable("Im_RequestCommittee_Shift", tb => tb.HasComment("نبطشيات اللجنه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_RequestCommittee_Shifts)
                .HasForeignKey(d => d.Im_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_RequestCommittee_Shift_Im_RequestCommittee");

            entity.HasOne(d => d.ShiftTiming).WithMany(p => p.Im_RequestCommittee_Shifts)
                .HasForeignKey(d => d.ShiftTiming_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_RequestCommittee_Shift_ShiftTiming");
        });

        modelBuilder.Entity<Im_RequestDat_Extra>(entity =>
        {
            entity.ToTable("Im_RequestDat_Extra", tb => tb.HasComment("تفاصيل الشركة او الهيئة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Im_RequestData_ID).HasComment("طلب الفحص");
            entity.Property(e => e.ImporeterCompanyAddress).HasComment("عنوان مندوب صاحب الرسالة");
            entity.Property(e => e.ImporeterCompanyAddress_EN).HasComment("عنوان مندوب صاحب الرسالة");
            entity.Property(e => e.ImportCompany).HasComment("الشركة المستوردة");
            entity.Property(e => e.ImportCompany_EN).HasComment("الشركة المستوردة");
            entity.Property(e => e.OwnerAddress).HasComment("عنوان صلحب الرسالة");
            entity.Property(e => e.OwnerName).HasComment("صاحب الرسالة");
            entity.Property(e => e.Reciever_Name).HasComment("اسم المرسل إليه");

            entity.HasOne(d => d.Im_RequestData).WithMany(p => p.Im_RequestDat_Extras)
                .HasForeignKey(d => d.Im_RequestData_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_RequestDat_Extra_Im_RequestData");
        });

        modelBuilder.Entity<Im_RequestDatum>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("تفاصيل عناصر الوارد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.DelegateAddress)
                .HasMaxLength(100)
                .HasComment("عنوان مندوب صاحب الرسالة");
            entity.Property(e => e.DelegateName)
                .HasMaxLength(50)
                .HasComment("مندوب صاحب الرسالة");
            entity.Property(e => e.ExportCountry_Id).HasComment("الدولة المصدرة");
            entity.Property(e => e.Im_OperationType).HasComment("نوع إذن الاستراد");
            entity.Property(e => e.ImporterType_Id).HasComment("from systemcode table 3");
            entity.Property(e => e.Importer_ID).HasComment("الشركة/الهيئة/الفرد المستوردة");
            entity.Property(e => e.Ship_Name)
                .HasMaxLength(50)
                .HasComment("اسم الباخرة");
            entity.Property(e => e.Shipment_Mean_Id).HasComment("وسيلة الشحن");
            entity.Property(e => e.Transport_Mean_Id).HasComment("وسيلة النقل");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ExportCountry).WithMany(p => p.Im_RequestData)
                .HasForeignKey(d => d.ExportCountry_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_RequestData_Country");

            entity.HasOne(d => d.Shipment_Mean).WithMany(p => p.Im_RequestData)
                .HasForeignKey(d => d.Shipment_Mean_Id)
                .HasConstraintName("FK_Im_RequestData_Shipment_Mean");

            entity.HasOne(d => d.Transport_Mean).WithMany(p => p.Im_RequestData)
                .HasForeignKey(d => d.Transport_Mean_Id)
                .HasConstraintName("FK_Im_RequestData_Transport_Mean");
        });

        modelBuilder.Entity<Im_Request_Port>(entity =>
        {
            entity.ToTable("Im_Request_Port", tb => tb.HasComment("ميناء"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Im_RequestData_ID).HasComment("طلب الفحص");
            entity.Property(e => e.IsNational)
                .HasDefaultValue(1)
                .HasComment("21 National / 22 International");
            entity.Property(e => e.Port_ID).HasComment("الميناء");
            entity.Property(e => e.ReqPortType_ID).HasComment("نوع الميناء التصدير\r\ntransit/arrive/shipping");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_RequestData).WithMany(p => p.Im_Request_Ports)
                .HasForeignKey(d => d.Im_RequestData_ID)
                .HasConstraintName("FK_Im_Request_Port_Im_RequestData");

            entity.HasOne(d => d.IsNationalNavigation).WithMany(p => p.Im_Request_PortIsNationalNavigations)
                .HasForeignKey(d => d.IsNational)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_Request_Port_A_SystemCode1");

            entity.HasOne(d => d.ReqPortType).WithMany(p => p.Im_Request_PortReqPortTypes)
                .HasForeignKey(d => d.ReqPortType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_Request_Port_A_SystemCode");
        });

        modelBuilder.Entity<Im_Request_TreatmentData_Confirm>(entity =>
        {
            entity.ToTable("Im_Request_TreatmentData_Confirm");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);

            entity.HasOne(d => d.Im_Request_TreatmentData).WithMany(p => p.Im_Request_TreatmentData_Confirms)
                .HasForeignKey(d => d.Im_Request_TreatmentData_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_Request_TreatmentData_Confirm_Im_Request_TreatmentData");
        });

        modelBuilder.Entity<Im_Request_TreatmentDatum>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Company_ID).HasComment("شركة المعالجة");
            entity.Property(e => e.Fees_Actual).HasColumnType("money");
            entity.Property(e => e.IS_From_Android)
                .HasDefaultValue(false)
                .HasComment("مين رمي row (system or android)");
            entity.Property(e => e.IS_Total).HasComment("(0) in the case of all,(1) in the case of the part في حاله الجزئي او الكلي");
            entity.Property(e => e.IS_Total_Android).HasComment("في حاله الفحص لو كلي واتحول الي جزئي");
            entity.Property(e => e.Im_RequestCommittee_ID).HasComment("لجنة المعالجة");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Item_ShortName_ID).HasComment("الاسم المختصر ");
            entity.Property(e => e.Size)
                .HasComment("حجم الرسالة (متر مكعب / سم مكعب)")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.Station_ID).HasComment("مكان المعالجة(محطة معتمدة)");
            entity.Property(e => e.Temperature)
                .HasComment("درجة الحرارة")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TheDose)
                .HasComment("الجرعة")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.ThermalSealNumber)
                .HasComment("رقم الختم الحراري")
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.TreatmentMat_Amount)
                .HasComment("كمية المادة المستخدمة في المعالجة")
                .HasColumnType("decimal(22, 6)");
            entity.Property(e => e.TreatmentMat_ID).HasComment("مادة المعالجة");
            entity.Property(e => e.TreatmentMethod_ID).HasComment("طريقة المعالجة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Im_RequestCommittee).WithMany(p => p.Im_Request_TreatmentData)
                .HasForeignKey(d => d.Im_RequestCommittee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_Request_TreatmentData_Im_RequestCommittee");

            entity.HasOne(d => d.TreatmentMat).WithMany(p => p.Im_Request_TreatmentData)
                .HasForeignKey(d => d.TreatmentMat_ID)
                .HasConstraintName("FK_Im_Request_TreatmentData_TreatmentMaterial");
        });

        modelBuilder.Entity<Im_ScientificResearch>(entity =>
        {
            entity.ToTable("Im_ScientificResearch", tb => tb.HasComment("الرسائل العليمة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Org_Manager_Name_Ar)
                .HasMaxLength(200)
                .HasComment("المدير الحالى");
            entity.Property(e => e.Org_Manager_Name_En)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Person_Job_Title_Ar)
                .HasMaxLength(200)
                .HasComment("الوظيفة الحالية");
            entity.Property(e => e.PortNational_Id).HasComment("ميناء الدخول");
            entity.Property(e => e.Quantity).HasComment("العدد");
            entity.Property(e => e.Shipment_Mean_Id).HasComment("وسيلة الشحن");
            entity.Property(e => e.Shipment_Policy_Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("رقم بوليصة الشحن");
            entity.Property(e => e.Tax_Certificate_Number)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("رقم الشهادة الجمركية");
            entity.Property(e => e.Transport_Mean_Id).HasComment("وسيلة النقل");
            entity.Property(e => e.Transport_Mean_Name)
                .HasMaxLength(150)
                .HasComment("اسم وسيلة الشحن");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ImPermission).WithMany(p => p.Im_ScientificResearches)
                .HasForeignKey(d => d.ImPermission_ID)
                .HasConstraintName("FK_Im_ScientificResearch_Im_PermissionRequest");

            entity.HasOne(d => d.Im_ScientificResearch_Organization).WithMany(p => p.Im_ScientificResearches)
                .HasForeignKey(d => d.Im_ScientificResearch_Organization_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_Im_ScientificResearch_Organization");

            entity.HasOne(d => d.Im_ScientificResearch_Person).WithMany(p => p.Im_ScientificResearches)
                .HasForeignKey(d => d.Im_ScientificResearch_Person_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_Im_ScientificResearch_Person");

            entity.HasOne(d => d.PortNational).WithMany(p => p.Im_ScientificResearches)
                .HasForeignKey(d => d.PortNational_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_PortNational");

            entity.HasOne(d => d.Shipment_Mean).WithMany(p => p.Im_ScientificResearches)
                .HasForeignKey(d => d.Shipment_Mean_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_Shipment_Mean");

            entity.HasOne(d => d.Transport_Mean).WithMany(p => p.Im_ScientificResearches)
                .HasForeignKey(d => d.Transport_Mean_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_Transport_Mean");
        });

        modelBuilder.Entity<Im_ScientificResearch_ItemPlant_Inseket_Lieble>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_ScientificResearch_ItemPlant_Inseket");

            entity.ToTable("Im_ScientificResearch_ItemPlant_Inseket_Lieble", tb => tb.HasComment("افات/حشرات/كائنات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Biological_Phase_id).HasComment("الطور الحيوى");
            entity.Property(e => e.LiableItems_Status_Id).HasComment("الحالة");
            entity.Property(e => e.LiableItems_Strain_Id).HasComment("السلالة");
            entity.Property(e => e.Package_Type_Id).HasComment("نوع العبوة");
            entity.Property(e => e.Procedure_Summery)
                .HasComment("ملخص الاجراءات")
                .HasColumnType("text");
            entity.Property(e => e.Research_Type_Id).HasComment("نوع الرسالة from systemcode=18\r\n");
            entity.Property(e => e.Scientific_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم العلمى");

            entity.HasOne(d => d.Biological_Phase).WithMany(p => p.Im_ScientificResearch_ItemPlant_Inseket_Liebles)
                .HasForeignKey(d => d.Biological_Phase_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_ItemPlant_Inseket_Lieble_Biological_Phase");

            entity.HasOne(d => d.Im_ScientificResearch).WithMany(p => p.Im_ScientificResearch_ItemPlant_Inseket_Liebles)
                .HasForeignKey(d => d.Im_ScientificResearch_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_ItemPlant_Inseket_Im_ScientificResearch");

            entity.HasOne(d => d.LiableItems_Status).WithMany(p => p.Im_ScientificResearch_ItemPlant_Inseket_Liebles)
                .HasForeignKey(d => d.LiableItems_Status_Id)
                .HasConstraintName("FK_Im_ScientificResearch_ItemPlant_Inseket_LiableItems_Status");

            entity.HasOne(d => d.LiableItems_Strain).WithMany(p => p.Im_ScientificResearch_ItemPlant_Inseket_Liebles)
                .HasForeignKey(d => d.LiableItems_Strain_Id)
                .HasConstraintName("FK_Im_ScientificResearch_ItemPlant_Inseket_LiableItems_Strain");

            entity.HasOne(d => d.Package_Type).WithMany(p => p.Im_ScientificResearch_ItemPlant_Inseket_Liebles)
                .HasForeignKey(d => d.Package_Type_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_ItemPlant_Inseket_Lieble_Package_Type");

            entity.HasOne(d => d.Research_Type).WithMany(p => p.Im_ScientificResearch_ItemPlant_Inseket_Liebles)
                .HasForeignKey(d => d.Research_Type_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_ItemPlant_Inseket_Lieble_A_SystemCode");
        });

        modelBuilder.Entity<Im_ScientificResearch_ItemPlant_Product>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_ScientificResearch_Items");

            entity.ToTable("Im_ScientificResearch_ItemPlant_Product", tb => tb.HasComment("النبات/ منتج"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.PlantCategories)
                .HasComment("الأصناف الزراعية")
                .HasColumnType("ntext");
            entity.Property(e => e.PlantPart_Name)
                .HasMaxLength(150)
                .HasComment("اسم الجزء النباتى فى حالة النبات");
            entity.Property(e => e.Procedure_Summery)
                .HasComment("ملخص الاجراءات")
                .HasColumnType("text");
            entity.Property(e => e.ProdPlant__Name)
                .HasMaxLength(150)
                .HasComment("اسم النبات او المنتج");
            entity.Property(e => e.ProductStatus_Id).HasComment("حالة المنتج او النبات");
            entity.Property(e => e.Research_Type_Id).HasComment("نوع الرسالة from systemcode=18\r\n");
            entity.Property(e => e.Scientific_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم العلمى");

            entity.HasOne(d => d.Im_ScientificResearch).WithMany(p => p.Im_ScientificResearch_ItemPlant_Products)
                .HasForeignKey(d => d.Im_ScientificResearch_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_Items_Im_ScientificResearch");

            entity.HasOne(d => d.ProductStatus).WithMany(p => p.Im_ScientificResearch_ItemPlant_Products)
                .HasForeignKey(d => d.ProductStatus_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_ItemPlant_Product_ProductStatus");

            entity.HasOne(d => d.Research_Type).WithMany(p => p.Im_ScientificResearch_ItemPlant_Products)
                .HasForeignKey(d => d.Research_Type_Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_ScientificResearch_ItemPlant_Product_A_SystemCode");
        });

        modelBuilder.Entity<Im_ScientificResearch_Organization>(entity =>
        {
            entity.ToTable("Im_ScientificResearch_Organization", tb => tb.HasComment("الجهة المقدمة للطلب"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_En).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name_Ar).HasMaxLength(300);
            entity.Property(e => e.Name_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Phone_No)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_ScientificResearch_Person>(entity =>
        {
            entity.ToTable("Im_ScientificResearch_Person", tb => tb.HasComment("مقدم الطلب"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_En).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name_Ar).HasMaxLength(300);
            entity.Property(e => e.Name_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.National_Id)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Passport_No)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone_No)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_Store>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("المخازن"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fax).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Place)
                .HasMaxLength(50)
                .HasComment("مخزن/ساحة");
            entity.Property(e => e.StoreOwner).HasMaxLength(50);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_SubDivission>(entity =>
        {
            entity.ToTable("Im_SubDivission", tb => tb.HasComment("تقسيم الرسالة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Customs_Certificate).HasMaxLength(50);
            entity.Property(e => e.FilePath).IsUnicode(false);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShipmentPolicy_Number)
                .HasMaxLength(50)
                .HasComment("رقم بوليصة الشحن");

            entity.HasOne(d => d.Im_Item).WithMany(p => p.Im_SubDivissions)
                .HasForeignKey(d => d.Im_Item_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Im_SubDivission_Im_PermissionItems");
        });

        modelBuilder.Entity<Im_TransUnderCustodyReason>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_TransUnderCustodyReason");

            entity.ToTable("Im_TransUnderCustodyReason", tb => tb.HasComment("أسباب النقل تحت التحفظ"));

            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_Visa>(entity =>
        {
            entity.ToTable("Im_Visa", tb => tb.HasComment("التاشيره"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name).HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Im_Warehouse>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("المناشئ"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_AR).HasMaxLength(300);
            entity.Property(e => e.Address_EN)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fax).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Name_Ar).HasMaxLength(50);
            entity.Property(e => e.Name_En)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.StoreArea)
                .HasMaxLength(50)
                .HasComment("مخزن/ساحة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.WarehouseTypeNavigation).WithMany(p => p.Im_Warehouses)
                .HasForeignKey(d => d.WarehouseType)
                .HasConstraintName("FK_Im_Warehouses_A_SystemCode");
        });

        modelBuilder.Entity<Im_choose_Constrain_>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_Constrain_chooes");

            entity.ToTable("Im_choose_Constrain ");

            entity.Property(e => e.ID).ValueGeneratedNever();

            entity.HasOne(d => d.Im_Constrain_Initiator_Text).WithMany(p => p.Im_choose_Constrain_s)
                .HasForeignKey(d => d.Im_Constrain_Initiator_Text_Id)
                .HasConstraintName("FK_Im_Constrain_chooes_Im_Constrain_Initiator_Text");

            entity.HasOne(d => d.Im_Constrain_Type).WithMany(p => p.Im_choose_Constrain_s)
                .HasForeignKey(d => d.Im_Constrain_Type_Id)
                .HasConstraintName("FK_Im_Constrain_chooes_Im_Constrain_Type");

            entity.HasOne(d => d.Im_Constrains_Special).WithMany(p => p.Im_choose_Constrain_s)
                .HasForeignKey(d => d.Im_Constrains_Special_Id)
                .HasConstraintName("FK_Im_Constrain_chooes_Im_Constrains_Special");
        });

        modelBuilder.Entity<InternationalTransportation>(entity =>
        {
            entity.ToTable("InternationalTransportation");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TransferMethod)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Plant");

            entity.ToTable("Item", tb => tb.HasComment("النبات و المنتجات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.IsForbidden).HasComment("0 مسموح به\r\n1 ممنوع \r\n");
            entity.Property(e => e.IsPermissionRequest)
                .HasDefaultValue(false)
                .HasComment("هل له اذن استيراد -خاص بالوارد");
            entity.Property(e => e.Is_known_item).HasComment("معروف وغير معروف");
            entity.Property(e => e.Item_Code).HasMaxLength(3);
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.Picture).IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Family).WithMany(p => p.Items)
                .HasForeignKey(d => d.Family_ID)
                .HasConstraintName("FK_Plant_Family");

            entity.HasOne(d => d.Group).WithMany(p => p.Items)
                .HasForeignKey(d => d.Group_ID)
                .HasConstraintName("FK_Plant_Group");
        });

        modelBuilder.Entity<ItemCategories_Group>(entity =>
        {
            entity.ToTable("ItemCategories_Group", tb => tb.HasComment("المجموعة الصنفية"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Descreption_En).IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En).HasMaxLength(200);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ItemCategories_Type>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_LiableItems_Strain");

            entity.ToTable("ItemCategories_Type", tb => tb.HasComment("سلالة البند الخاضع"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantCategories");

            entity.ToTable(tb => tb.HasComment("الأصناف الزراعية"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Company_ID).HasComment("الجهة الطالبة للتسجيل");
            entity.Property(e => e.CurrentStatus).HasComment("if under protection 1 / 0 if not (تحت الحماية أو لا)");
            entity.Property(e => e.IsForbidden).HasComment(" 0 لو ممنوع 1 لو شغال");
            entity.Property(e => e.IsRegister).HasComment("هل مسجل ام لا");
            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Protect_Property).HasComment("قرار حماية الماكية ملف Pdf");
            entity.Property(e => e.Register_NumDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeOut).HasComment("نهاية المهلة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Company).WithMany(p => p.ItemCategories)
                .HasForeignKey(d => d.Company_ID)
                .HasConstraintName("FK_PlantCategories_Company_National");

            entity.HasOne(d => d.ItemCategories_Group).WithMany(p => p.ItemCategories)
                .HasForeignKey(d => d.ItemCategories_Group_ID)
                .HasConstraintName("FK_ItemCategories_ItemCategories_Group");

            entity.HasOne(d => d.ItemCategories_TypeNavigation).WithMany(p => p.ItemCategories)
                .HasForeignKey(d => d.ItemCategories_Type)
                .HasConstraintName("FK_ItemCategories_LiableItems_Strain");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemCategories)
                .HasForeignKey(d => d.Item_ID)
                .HasConstraintName("FK_PlantCategories_Plant");
        });

        modelBuilder.Entity<ItemPart>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantPart_1");

            entity.ToTable("ItemPart", tb => tb.HasComment("الجزء النباتى للكائنات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemParts)
                .HasForeignKey(d => d.Item_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlantPart_Plant");

            entity.HasOne(d => d.SubPart).WithMany(p => p.ItemParts)
                .HasForeignKey(d => d.SubPart_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlantPart_PlantPartType");
        });

        modelBuilder.Entity<Item_Purpose>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ExportPurpose");

            entity.ToTable("Item_Purpose", tb => tb.HasComment("الغرض"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Item_ShortName>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Plant_ShortName");

            entity.ToTable("Item_ShortName", tb => tb.HasComment("المسمى المختصر"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ExportStatus).HasComment("الموقف من التصدير");
            entity.Property(e => e.IS_ShortName).HasComment("له اسم مختصر ام لا");
            entity.Property(e => e.ImportStatus).HasComment("الموقف من الاستيراد");
            entity.Property(e => e.Is_ImportTaxFree)
                .HasDefaultValue(false)
                .HasComment("معفي من اذن الاستيراد");
            entity.Property(e => e.Item_ID).HasComment("الصنف");
            entity.Property(e => e.Item_Purpose_ID).HasComment("الغرض");
            entity.Property(e => e.Item_Status_ID).HasComment("الحالة");
            entity.Property(e => e.Product_ID).HasComment("المنتج");
            entity.Property(e => e.QualitativeGroup_Id).HasComment("مجموعة نوعية");
            entity.Property(e => e.Reason).HasComment("سبب الايقاف");
            entity.Property(e => e.ShortName_Ar).HasComment("الاسم العربى");
            entity.Property(e => e.ShortName_En)
                .IsUnicode(false)
                .HasComment("الاسم الاجنبى");
            entity.Property(e => e.SubPart_ID).HasComment("الجزء النباتى او الطور الحيوى");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Item).WithMany(p => p.Item_ShortNames)
                .HasForeignKey(d => d.Item_ID)
                .HasConstraintName("FK_Plant_ShortName_Plant");

            entity.HasOne(d => d.Item_Purpose).WithMany(p => p.Item_ShortNames)
                .HasForeignKey(d => d.Item_Purpose_ID)
                .HasConstraintName("FK_Plant_ShortName_PlantPurpose");

            entity.HasOne(d => d.Item_Status).WithMany(p => p.Item_ShortNames)
                .HasForeignKey(d => d.Item_Status_ID)
                .HasConstraintName("FK_Plant_ShortName_ProductStatus");

            entity.HasOne(d => d.QualitativeGroup).WithMany(p => p.Item_ShortNames)
                .HasForeignKey(d => d.QualitativeGroup_Id)
                .HasConstraintName("FK_Plant_ShortName_Item_SpecificGroup");

            entity.HasOne(d => d.SubPart).WithMany(p => p.Item_ShortNames)
                .HasForeignKey(d => d.SubPart_ID)
                .HasConstraintName("FK_Plant_ShortName_PlantPartType");
        });

        modelBuilder.Entity<Item_Status>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ProductStatus");

            entity.ToTable("Item_Status", tb => tb.HasComment("حالة المنتج"));

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasComment("");
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.En_Name)
                .HasMaxLength(200)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Item_Type>(entity =>
        {
            entity.ToTable("Item_Type", tb => tb.HasComment("نوع الصنف نبات, منتج, بند حي..."));

            entity.Property(e => e.Coler)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Kingdom>(entity =>
        {
            entity.ToTable("Kingdom", tb => tb.HasComment("المملكه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.ToTable("Level", tb => tb.HasComment("المستوي"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<LiableItem>(entity =>
        {
            entity.ToTable(tb => tb.HasComment(" "));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsAlive).HasComment("كائنات حية/غير حية");
            entity.Property(e => e.IsPermissionRequest)
                .HasDefaultValue(false)
                .HasComment("هل له اذن استيراد -خاص بالوارد");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.IsAliveNavigation).WithMany(p => p.LiableItems)
                .HasForeignKey(d => d.IsAlive)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LiableItems_A_SystemCode");

            entity.HasOne(d => d.LiableItems_Strain).WithMany(p => p.LiableItems)
                .HasForeignKey(d => d.LiableItems_Strain_Id)
                .HasConstraintName("FK_LiableItems_LiableItems_Strain");
        });

        modelBuilder.Entity<LiableItems_ShortName>(entity =>
        {
            entity.ToTable("LiableItems_ShortName");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ExportStatus).HasComment("الموقف من التصدير");
            entity.Property(e => e.HSCODE).HasMaxLength(50);
            entity.Property(e => e.ImportStatus).HasComment("الموقف من الاستيراد");
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.ScientficName_En).IsUnicode(false);
            entity.Property(e => e.ShortName_En).IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Biological_Phase).WithMany(p => p.LiableItems_ShortNames)
                .HasForeignKey(d => d.Biological_Phase_ID)
                .HasConstraintName("FK_LiableItems_ShortName_Biological_Phase");

            entity.HasOne(d => d.LiableItem).WithMany(p => p.LiableItems_ShortNames)
                .HasForeignKey(d => d.LiableItem_ID)
                .HasConstraintName("FK_LiableItems_ShortName_LiableItems");

            entity.HasOne(d => d.LiableItems_Status).WithMany(p => p.LiableItems_ShortNames)
                .HasForeignKey(d => d.LiableItems_Status_ID)
                .HasConstraintName("FK_LiableItems_ShortName_LiableItems_Status");

            entity.HasOne(d => d.PlantPurpose).WithMany(p => p.LiableItems_ShortNames)
                .HasForeignKey(d => d.PlantPurpose_ID)
                .HasConstraintName("FK_LiableItems_ShortName_PlantPurpose");
        });

        modelBuilder.Entity<LiableItems_Status>(entity =>
        {
            entity.ToTable("LiableItems_Status", tb => tb.HasComment("حالة البند الخاضع"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_En).IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MainCalssification>(entity =>
        {
            entity.ToTable("MainCalssification", tb => tb.HasComment("الصنف الاساسى"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Item_Type).WithMany(p => p.MainCalssifications)
                .HasForeignKey(d => d.Item_Type_ID)
                .HasConstraintName("FK_MainCalssification_Item_Type");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order", tb => tb.HasComment("الرتبة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Phylum).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Phylum_ID)
                .HasConstraintName("FK_Order_PhylumSubphylum");
        });

        modelBuilder.Entity<Outlet>(entity =>
        {
            entity.ToTable("Outlet", tb => tb.HasComment("المنفذ"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_Ar).HasComment("العنوان بالعربية");
            entity.Property(e => e.Address_En)
                .IsUnicode(false)
                .HasComment("العنوان بالانجليزية");
            entity.Property(e => e.Ar_Name).HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.GrAdmin_ID).HasComment("الادارة العامة");
            entity.Property(e => e.ID_HR).HasComment("رقم المنفذ بالنسبة لل HR");
            entity.Property(e => e.IsExport).HasComment("from system code 21\r\nصادر	/وارد	/صادر+ وارد");
            entity.Property(e => e.Supervisor_ID).HasComment("مشرف الفرع\r\nFrom HR employee table");

            entity.HasOne(d => d.GrAdmin).WithMany(p => p.Outlets)
                .HasForeignKey(d => d.GrAdmin_ID)
                .HasConstraintName("FK_Outlet_General_Admin");

            entity.HasOne(d => d.IsExportNavigation).WithMany(p => p.Outlets)
                .HasForeignKey(d => d.IsExport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Outlet_A_SystemCode");

            entity.HasOne(d => d.PortNational).WithMany(p => p.Outlets)
                .HasForeignKey(d => d.PortNational_ID)
                .HasConstraintName("FK_Outlet_PortNational");
        });

        modelBuilder.Entity<Outlet_Employee>(entity =>
        {
            entity.ToTable("Outlet_Employee", tb => tb.HasComment("موظفين المنافذ"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Employee_Id).HasComment("الموظف");
            entity.Property(e => e.Outlet_ID).HasComment("المنافذ");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Outlet).WithMany(p => p.Outlet_Employees)
                .HasForeignKey(d => d.Outlet_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Outlet_Employee_Outlet");
        });

        modelBuilder.Entity<Package_Material>(entity =>
        {
            entity.ToTable("Package_Material", tb => tb.HasComment("مادة العبوة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Package_Type>(entity =>
        {
            entity.ToTable("Package_Type", tb => tb.HasComment("نوع العبوة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Pallet_Data_Ex_CheckRequest_Distribution>(entity =>
        {
            entity.ToTable("Pallet_Data_Ex_CheckRequest_Distribution");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Quantity).HasComment("الكمية الصالحة للتصدير");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Pallet_Data_Ex_CheckRequest_Distributions)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pallet_Data_Ex_CheckRequest_Distribution_Ex_CheckRequest");

            entity.HasOne(d => d.Pallet_Data_Organization__Distribution).WithMany(p => p.Pallet_Data_Ex_CheckRequest_Distributions)
                .HasForeignKey(d => d.Pallet_Data_Organization__Distribution_ID)
                .HasConstraintName("FK_Pallet_Data_Ex_CheckRequest_Distribution_Pallet_Data_Organization__Distribution");
        });

        modelBuilder.Entity<Pallet_Data_Organization__Distribution>(entity =>
        {
            entity.ToTable("Pallet_Data_Organization__Distribution");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Organization_ID).HasComment("رقم الجهة المشتري");
            entity.Property(e => e.Organization_Type_Id).HasComment("نوع الجهة المشتري");
            entity.Property(e => e.Quantity).HasComment("كمية البالتات");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Ex_CheckRequest).WithMany(p => p.Pallet_Data_Organization__Distributions)
                .HasForeignKey(d => d.Ex_CheckRequest_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pallet_Data_Organization__Distribution_Ex_CheckRequest");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Im_Person");

            entity.ToTable("Person");

            entity.HasIndex(e => e.ID, "_dta_index_Person_15_757017878__K1_2_22");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Country_ID).HasComment("");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.IDNumber)
                .HasMaxLength(50)
                .HasComment("رقم قومى/ باسبور");
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.Phone).HasMaxLength(11);
            entity.Property(e => e.User_Activation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Country).WithMany(p => p.People)
                .HasForeignKey(d => d.Country_ID)
                .HasConstraintName("FK_Im_Person_Country");

            entity.HasOne(d => d.Person_IDTypeNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.Person_IDType)
                .HasConstraintName("FK_Person_A_SystemCode");
        });

        modelBuilder.Entity<PhylumSubphylum>(entity =>
        {
            entity.ToTable("PhylumSubphylum", tb => tb.HasComment("الشعبه"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Kingdom).WithMany(p => p.PhylumSubphylums)
                .HasForeignKey(d => d.Kingdom_ID)
                .HasConstraintName("FK_PhylumSubphylum_Kingdom");

            entity.HasOne(d => d.Level).WithMany(p => p.PhylumSubphylums)
                .HasForeignKey(d => d.Level_ID)
                .HasConstraintName("FK_PhylumSubphylum_Level");
        });

        modelBuilder.Entity<PortNational>(entity =>
        {
            entity.ToTable("PortNational", tb => tb.HasComment("ميناء محلى"));

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasComment("الميناء");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Govern_ID).HasComment("المحافظة");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar).HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PortOrgainzation_ID).HasComment("هيئات المواني");
            entity.Property(e => e.PortTypeID).HasComment("نوع الميناء");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Govern).WithMany(p => p.PortNationals)
                .HasForeignKey(d => d.Govern_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PortNational_Governate");

            entity.HasOne(d => d.PortOrgainzation).WithMany(p => p.PortNationals)
                .HasForeignKey(d => d.PortOrgainzation_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PortNational_PortOrganization");

            entity.HasOne(d => d.PortType).WithMany(p => p.PortNationals)
                .HasForeignKey(d => d.PortTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PortNational_Port_Type");
        });

        modelBuilder.Entity<PortOrganization>(entity =>
        {
            entity.ToTable("PortOrganization", tb => tb.HasComment("هيئة الموانئ"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("البريد الاليكتروني");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("الفاكس");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("التليفون");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Port_International>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Ports");

            entity.ToTable("Port_International", tb => tb.HasComment("المواني الدوليه"));

            entity.HasIndex(e => new { e.User_Deletion_Id, e.IsActive }, "_dta_index_Port_International_15_2068306528__K12_K6_1_4_5");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Country_ID).HasComment("الدولة");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PortTypeID).HasComment("نوع الميناء");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Port_Internationals)
                .HasForeignKey(d => d.Country_ID)
                .HasConstraintName("FK_Port_Country");

            entity.HasOne(d => d.PortType).WithMany(p => p.Port_Internationals)
                .HasForeignKey(d => d.PortTypeID)
                .HasConstraintName("FK_Ports_Port_Type");

            entity.HasOne(d => d.Regions).WithMany(p => p.Port_Internationals)
                .HasForeignKey(d => d.Regions_ID)
                .HasConstraintName("FK_Port_International_Regions");
        });

        modelBuilder.Entity<Port_Type>(entity =>
        {
            entity.ToTable("Port_Type", tb => tb.HasComment("أنواع المواني"));

            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PublicOrganization_Type>(entity =>
        {
            entity.ToTable("PublicOrganization_Type");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsExempt).HasComment("معفى من طلب إذن استيراد");
            entity.Property(e => e.Name_Ar).HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Public_Organization>(entity =>
        {
            entity.ToTable("Public_Organization", tb => tb.HasComment("الهيئات العامة"));

            entity.HasIndex(e => e.ID, "_dta_index_Public_Organization_15_1963258149__K1_2_3");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_Ar).HasComment("العنوان بالعربية");
            entity.Property(e => e.Address_En)
                .IsUnicode(false)
                .HasComment("العنوان بالانجليزية");
            entity.Property(e => e.IS_OnlineOffline)
                .HasDefaultValue(false)
                .HasComment("from web/system\r\n1->online\r\n0->offline");
            entity.Property(e => e.Name_Ar).HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.PersonResponsible_IDNumber).HasMaxLength(50);
            entity.Property(e => e.Person_Responsible_Country_ID).HasComment("");
            entity.Property(e => e.Person_Responsible_Name).HasMaxLength(500);
            entity.Property(e => e.User_Activation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.PublicOrgType).WithMany(p => p.Public_Organizations)
                .HasForeignKey(d => d.PublicOrgType_ID)
                .HasConstraintName("FK_Public_Organization_PublicOrganization_Type");
        });

        modelBuilder.Entity<QualitativeGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Item_SpecificGroup");

            entity.ToTable("QualitativeGroup", tb => tb.HasComment("مجموعة النوعية"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Refuse_Reason>(entity =>
        {
            entity.ToTable("Refuse_Reason", tb => tb.HasComment("اسباب رفض الطلب( صادر/وارد)"));

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasComment("");
            entity.Property(e => e.A_SystemCode_ID).HasComment("from systemcode table 20\r\n اذن استراد \r\n طلب فحص وارد 74\r\nfarm 78\r\n");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsExport).HasComment("هل يرفض الطلب كله في حالة وجود إصابة");
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("المناطق"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Country_ID).HasComment("الدولة");
            entity.Property(e => e.Descreption_En).IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("قارة");
            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En).HasMaxLength(200);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Regions)
                .HasForeignKey(d => d.Country_ID)
                .HasConstraintName("FK_Regions_Country");
        });

        modelBuilder.Entity<Regional_Area>(entity =>
        {
            entity.ToTable("Regional_Area");

            entity.Property(e => e.ID).HasComment("المناطق الاقليمية");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("قارة");
            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En).HasMaxLength(200);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<SecondaryClassification>(entity =>
        {
            entity.ToTable("SecondaryClassification", tb => tb.HasComment("تقسيم فرعى"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(300)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.MainClass).WithMany(p => p.SecondaryClassifications)
                .HasForeignKey(d => d.MainClass_ID)
                .HasConstraintName("FK_SecondaryClassification_MainCalssification");
        });

        modelBuilder.Entity<ShiftTiming>(entity =>
        {
            entity.ToTable("ShiftTiming");

            entity.Property(e => e.Day_Type).HasComment("0 ايام عطلات\r\n1 ايام عادية\r\n");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Shipment_Mean>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Shipment_Means");

            entity.ToTable("Shipment_Mean", tb => tb.HasComment("وسائل الشحن"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(300)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ShippingAgency>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name_En)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ShippingCompany>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("شركات الشحن الدولية"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_Ar)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name_En)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.ToTable("Station", tb => tb.HasComment("محطة"));

            entity.HasIndex(e => e.ID, "IX_Station");

            entity.HasIndex(e => e.ID, "IX_Station_1");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Address_Ar).HasComment("العنوان بالعربية");
            entity.Property(e => e.Address_En)
                .IsUnicode(false)
                .HasComment("العنوان بالانجليزية");
            entity.Property(e => e.Ar_Name).HasComment("الاسم بالعربية");
            entity.Property(e => e.Average_number_workers).HasComment("متوسط عدد العمال");
            entity.Property(e => e.CommertialRecord)
                .HasMaxLength(200)
                .HasComment("السجل التجاري");
            entity.Property(e => e.En_Name)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.EndDate_Industrial_License_Num).HasComment("تاريخ النهاية");
            entity.Property(e => e.Facility_area).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.Fast_cooling_refrigerators).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.Industrial_License_Num)
                .HasMaxLength(200)
                .HasComment("رقم الترخيص الصناعي");
            entity.Property(e => e.IsAccepted).HasComment("موافقة ورفض الطلب\r\n");
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false)
                .HasComment("لو معتمدة 1");
            entity.Property(e => e.Notes_Reject).HasComment("اسباب الرفض");
            entity.Property(e => e.Number_Shifts).HasComment("عدد الورديات");
            entity.Property(e => e.Number_working_Days).HasComment("عدد ايام العمل");
            entity.Property(e => e.Production_capacity).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.Seasonal_Annual).HasComment("موسمي 0 سنوي 1");
            entity.Property(e => e.StartDate_Industrial_License_Num).HasComment("تاريخ البداية");
            entity.Property(e => e.StationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Storage_fridge_capacity).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.TaxesRecord)
                .HasMaxLength(200)
                .HasComment("السجل الضريبي");
            entity.Property(e => e.The_num_of_production_lines).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.The_num_of_storage_refrigerators).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_Id).HasComment("from systemcode table 3");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.working_hours).HasComment("عدد ساعات العمل");
        });

        modelBuilder.Entity<StationActivityType>(entity =>
        {
            entity.ToTable("StationActivityType", tb => tb.HasComment("أنواع أنشطة المحطة"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.En_Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.TreatmentMethods).WithMany(p => p.StationActivityTypes)
                .HasForeignKey(d => d.TreatmentMethods_ID)
                .HasConstraintName("FK_StationActivityType_TreatmentMethods");
        });

        modelBuilder.Entity<StationCompany>(entity =>
        {
            entity.ToTable("StationCompany", tb => tb.HasComment("شركات المحطة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Company_Type_Id).HasComment("from systemcode table 3");
            entity.Property(e => e.Status).HasComment("2 تحت الدراسة\r\n1 مقبول\r\n0 مرفوض\r\n3 ايقاف\r\n\r\n\r\n");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Station_Accreditation).WithMany(p => p.StationCompanies)
                .HasForeignKey(d => d.Station_Accreditation_ID)
                .HasConstraintName("FK_StationCompany_Station_Accreditation");
        });

        modelBuilder.Entity<StationContact>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("وسائل اتصال المحطة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ContactType_ID).HasComment("نوع وسيلة الاتصال");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Value)
                .HasMaxLength(150)
                .HasComment("الرقم");

            entity.HasOne(d => d.ContactType).WithMany(p => p.StationContacts)
                .HasForeignKey(d => d.ContactType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StationContacts_ContactType");

            entity.HasOne(d => d.Station).WithMany(p => p.StationContacts)
                .HasForeignKey(d => d.StationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StationContacts_Station");
        });

        modelBuilder.Entity<Station_Accreditation>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Station_Accreditation_1");

            entity.ToTable("Station_Accreditation", tb => tb.HasComment(""));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.EndDate).HasComment("تاريخ النهاية");
            entity.Property(e => e.StartDate).HasComment("تاريخ البداية");
            entity.Property(e => e.Station_Accreditation_Data_ID).HasComment("المحطة");
            entity.Property(e => e.Station_ID).HasComment("المحطة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Station_Accreditation_Data).WithMany(p => p.Station_Accreditations)
                .HasForeignKey(d => d.Station_Accreditation_Data_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Station_Accreditation_Data1");

            entity.HasOne(d => d.Station).WithMany(p => p.Station_Accreditations)
                .HasForeignKey(d => d.Station_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Station1");
        });

        modelBuilder.Entity<Station_Accreditation_CheckList>(entity =>
        {
            entity.ToTable("Station_Accreditation_CheckList", tb => tb.HasComment("الاعتمادات وقوائم الاندريد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Station_Accreditation_Data).WithMany(p => p.Station_Accreditation_CheckLists)
                .HasForeignKey(d => d.Station_Accreditation_Data_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_CheckList_Station_Accreditation_Data");

            entity.HasOne(d => d.Station_CheckList).WithMany(p => p.Station_Accreditation_CheckLists)
                .HasForeignKey(d => d.Station_CheckList_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_CheckList_Station_CheckList");
        });

        modelBuilder.Entity<Station_Accreditation_Committee>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Station_Committee");

            entity.ToTable("Station_Accreditation_Committee", tb => tb.HasComment("لجنة اعتماد المحطات"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount_Total)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.Delegation_Date).HasComment("تاريخ الفحص-تاريخ الانتداب");
            entity.Property(e => e.EndTime).HasComment("انتهاء ساعة الفحص");
            entity.Property(e => e.IsAccepted).HasComment("0 if rejected else 1 \r\nlab will set the result");
            entity.Property(e => e.IsApproved).HasComment("null->ask for accredation\r\n0->not accepted\r\n1->Accepted");
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.Is_Cancel).HasComment("تعزر عمل اللجنه");
            entity.Property(e => e.Is_Start_Android).HasComment("تعزر عمل اللجنه");
            entity.Property(e => e.Notes_Refuse_Ar).HasMaxLength(300);
            entity.Property(e => e.Notes_Refuse_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StartTime).HasComment(" بداية ساعة الفحص ");
            entity.Property(e => e.Station_Accreditation_Request_ID).HasComment("طلب الفحص");
            entity.Property(e => e.Status).HasComment("null->No committe ,0 if not done, 1 if investigation is done");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CommitteeType).WithMany(p => p.Station_Accreditation_Committees)
                .HasForeignKey(d => d.CommitteeType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Committee_CommitteeType");

            entity.HasOne(d => d.Station_Accreditation_Request).WithMany(p => p.Station_Accreditation_Committees)
                .HasForeignKey(d => d.Station_Accreditation_Request_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Committee_Station_Accreditation");
        });

        modelBuilder.Entity<Station_Accreditation_Committee_CheckList>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Station_Accreditation_Committee");

            entity.ToTable("Station_Accreditation_Committee_CheckList", tb => tb.HasComment("نتائج لجنة اعتماد المحطات\r\n"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Committee_ID).HasComment("لجنة سحب العينة");
            entity.Property(e => e.IsAccepted).HasComment("0 if rejected else 1 \r\nlab will set the result");
            entity.Property(e => e.IsAccepted_Quarantine).HasComment("0 if rejected else 1 \r\nlab will set the result\r\nموقف الحجر\r\n");
            entity.Property(e => e.Notes_Ar).HasMaxLength(300);
            entity.Property(e => e.Notes_En)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Notes_Quarantine)
                .HasMaxLength(300)
                .HasComment("ملاحظات الحجر");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("Employee ID");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Committee).WithMany(p => p.Station_Accreditation_Committee_CheckLists)
                .HasForeignKey(d => d.Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_CommitteeData_InvestigateCommittee");

            entity.HasOne(d => d.Station_Accreditation_CheckList).WithMany(p => p.Station_Accreditation_Committee_CheckLists)
                .HasForeignKey(d => d.Station_Accreditation_CheckList_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Committee_Result_Station_Accreditation_CheckList");
        });

        modelBuilder.Entity<Station_Accreditation_Committee_CheckList_Confirm>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Station_Accreditation_CommitteeResult_Confirm");

            entity.ToTable("Station_Accreditation_Committee_CheckList_Confirm", tb => tb.HasComment("الموافقة على نتيجة الفحص"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.IsAccepted).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Station_Accreditation_CommitteeResult).WithMany(p => p.Station_Accreditation_Committee_CheckList_Confirms)
                .HasForeignKey(d => d.Station_Accreditation_CommitteeResult_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_CommitteeResult_Confirm_Station_Accreditation_CommitteeResult");
        });

        modelBuilder.Entity<Station_Accreditation_Committee_Final_Result>(entity =>
        {
            entity.ToTable("Station_Accreditation_Committee_Final_Result");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ISAdmin).HasComment("is admin for the current committee");
            entity.Property(e => e.IsAccepted)
                .HasDefaultValue(false)
                .HasComment("is admin for the current committee");
            entity.Property(e => e.Station_Accreditation_Committee_ID).HasComment("لجنة المعالجة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("Employee ID");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Station_Accreditation_Committee).WithMany(p => p.Station_Accreditation_Committee_Final_Results)
                .HasForeignKey(d => d.Station_Accreditation_Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Committee_Final_Result_Station_Accreditation_Committee");
        });

        modelBuilder.Entity<Station_Accreditation_Committee_Imge>(entity =>
        {
            entity.ToTable("Station_Accreditation_Committee_Imge");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Infection_Comment).HasComment("نوع المرفق");
            entity.Property(e => e.User_Creation_Date)
                .HasComment("null-> for user , value -> if the admin add the row")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.User_Creation_Id).HasComment("null-> for user , value -> if the admin add the row");

            entity.HasOne(d => d.Station_Accreditation_Committee).WithMany(p => p.Station_Accreditation_Committee_Imges)
                .HasForeignKey(d => d.Station_Accreditation_Committee_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Committee_Imge_Station_Accreditation_Committee");
        });

        modelBuilder.Entity<Station_Accreditation_Committee_Shift>(entity =>
        {
            entity.ToTable("Station_Accreditation_Committee_Shift");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ShiftTiming).WithMany(p => p.Station_Accreditation_Committee_Shifts)
                .HasForeignKey(d => d.ShiftTiming_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Committee_Shift_ShiftTiming");

            entity.HasOne(d => d.Station_Accreditation_Committee).WithMany(p => p.Station_Accreditation_Committee_Shifts)
                .HasForeignKey(d => d.Station_Accreditation_Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Committee_Shift_Station_Accreditation_Committee");
        });

        modelBuilder.Entity<Station_Accreditation_Data_Country>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_StationActivityCountry1");

            entity.ToTable("Station_Accreditation_Data_Country", tb => tb.HasComment("بلد نشاط المحطة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Station_Accreditation_Data_Countries)
                .HasForeignKey(d => d.CountryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StationActivityCountry_Country1");

            entity.HasOne(d => d.Station_Accreditation_Data).WithMany(p => p.Station_Accreditation_Data_Countries)
                .HasForeignKey(d => d.Station_Accreditation_Data_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Data_Country_Station_Accreditation_Data");
        });

        modelBuilder.Entity<Station_Accreditation_Data_Item_ShortName>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Station_Plant_Product1");

            entity.ToTable("Station_Accreditation_Data_Item_ShortName", tb => tb.HasComment("المسمى المختصر للاعتماد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Item_ShortName).WithMany(p => p.Station_Accreditation_Data_Item_ShortNames)
                .HasForeignKey(d => d.Item_ShortName_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Item_ShortName_Item_ShortName");

            entity.HasOne(d => d.Station_Accreditation_Data).WithMany(p => p.Station_Accreditation_Data_Item_ShortNames)
                .HasForeignKey(d => d.Station_Accreditation_Data_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Data_Item_ShortName_Station_Accreditation_Data");
        });

        modelBuilder.Entity<Station_Accreditation_Datum>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("مسمى الاعتماد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Accreditation_Type_ID).HasComment("A_SystemCode id =21");
            entity.Property(e => e.DescriptionMore_EN).IsUnicode(false);
            entity.Property(e => e.Description_Ar).HasComment("العنوان بالعربية");
            entity.Property(e => e.Description_En)
                .IsUnicode(false)
                .HasComment("العنوان بالانجليزية");
            entity.Property(e => e.Name_AR)
                .HasMaxLength(100)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.StationActivityType_ID).HasComment("نوع النشاط");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Accreditation_Type).WithMany(p => p.Station_Accreditation_Data)
                .HasForeignKey(d => d.Accreditation_Type_ID)
                .HasConstraintName("FK_Station_Accreditation_Data_A_SystemCode");

            entity.HasOne(d => d.StationActivityType).WithMany(p => p.Station_Accreditation_Data)
                .HasForeignKey(d => d.StationActivityType_ID)
                .HasConstraintName("FK_Station_Accreditation_Data_StationActivityType");
        });

        modelBuilder.Entity<Station_Accreditation_Request>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Station_Accreditation");

            entity.ToTable("Station_Accreditation_Request", tb => tb.HasComment("اعتماد المحطة"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Amount_Total)
                .HasDefaultValue(0m)
                .HasComment("المبلغ")
                .HasColumnType("money");
            entity.Property(e => e.EndDate).HasComment("تاريخ النهاية");
            entity.Property(e => e.IsAccepted).HasComment("موافقة ورفض الطلب\r\n");
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.Is_Final_requst).HasComment("الموقف النهائي للطلب\r\nnull لم يتم العمل على الطلب\r\n0 يتم العمل على الطلب\r\n1 تم الانتهاء من العمل على الطلب");
            entity.Property(e => e.StartDate).HasComment("تاريخ البداية");
            entity.Property(e => e.Station_Accreditation_Data_ID).HasComment("المحطة");
            entity.Property(e => e.Station_ID).HasComment("المحطة");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CommitteeType).WithMany(p => p.Station_Accreditation_Requests)
                .HasForeignKey(d => d.CommitteeType_ID)
                .HasConstraintName("FK_Station_Accreditation_Request_CommitteeType");

            entity.HasOne(d => d.Station_Accreditation_Data).WithMany(p => p.Station_Accreditation_Requests)
                .HasForeignKey(d => d.Station_Accreditation_Data_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Station_Accreditation_Data");

            entity.HasOne(d => d.Station_Accreditation_Request_Type).WithMany(p => p.Station_Accreditation_Requests)
                .HasForeignKey(d => d.Station_Accreditation_Request_Type_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Request_Station_Accreditation_Request_Type");

            entity.HasOne(d => d.Station).WithMany(p => p.Station_Accreditation_Requests)
                .HasForeignKey(d => d.Station_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Station");
        });

        modelBuilder.Entity<Station_Accreditation_Request_Fee>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Value).HasColumnType("money");

            entity.HasOne(d => d.Station_Accreditation_Request).WithMany(p => p.Station_Accreditation_Request_Fees)
                .HasForeignKey(d => d.Station_Accreditation_Request_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Request_Fees_Station_Accreditation_Request");

            entity.HasOne(d => d.Station_Fees_Type).WithMany(p => p.Station_Accreditation_Request_Fees)
                .HasForeignKey(d => d.Station_Fees_Type_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Request_Fees_Station_Fees_Type");
        });

        modelBuilder.Entity<Station_Accreditation_Request_Fees_ENG>(entity =>
        {
            entity.ToTable("Station_Accreditation_Request_Fees_ENG");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasComment("تم الانتهاء من الدفع");
            entity.Property(e => e.Num_Eng).HasDefaultValue(1);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Value).HasColumnType("money");

            entity.HasOne(d => d.Station_Accreditation_Committee).WithMany(p => p.Station_Accreditation_Request_Fees_ENGs)
                .HasForeignKey(d => d.Station_Accreditation_Committee_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Request_Fees_ENG_Station_Accreditation_Committee");

            entity.HasOne(d => d.Station_Fees_Type).WithMany(p => p.Station_Accreditation_Request_Fees_ENGs)
                .HasForeignKey(d => d.Station_Fees_Type_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Accreditation_Request_Fees_ENG_Station_Fees_Type");
        });

        modelBuilder.Entity<Station_Accreditation_Request_Type>(entity =>
        {
            entity.ToTable("Station_Accreditation_Request_Type");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name_AR).HasMaxLength(50);
            entity.Property(e => e.Name_EN).HasMaxLength(50);
        });

        modelBuilder.Entity<Station_CheckList>(entity =>
        {
            entity.ToTable("Station_CheckList", tb => tb.HasComment("شروط الاندريد"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.ConstrainText_En).IsUnicode(false);
            entity.Property(e => e.Description_En).IsUnicode(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("");
            entity.Property(e => e.Is_Androud).HasComment("شهادة الصحة النباتية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Station_Constrain_Country_Item).WithMany(p => p.Station_CheckLists)
                .HasForeignKey(d => d.Station_Constrain_Country_Item_ID)
                .HasConstraintName("FK_Station_CheckList_Station_Constrain_Country_Item1");
        });

        modelBuilder.Entity<Station_Committee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Station_Committee");
        });

        modelBuilder.Entity<Station_Constrain_Country_Item>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_EX_Station_Type");

            entity.ToTable("Station_Constrain_Country_Item", tb => tb.HasComment("انواع الاشتراطات"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مفعل");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Station_Type).WithMany(p => p.Station_Constrain_Country_Items)
                .HasForeignKey(d => d.Station_Type_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EX_Station_Type_EX_Constrain");
        });

        modelBuilder.Entity<Station_Constrain_Type>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_EX_Station");

            entity.ToTable("Station_Constrain_Type", tb => tb.HasComment(" الاشتراطات"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مفعل");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Station_Datum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Station_Data");

            entity.Property(e => e.Center_Ar_Name).HasMaxLength(150);
            entity.Property(e => e.Gov_Ar_Name).HasMaxLength(150);
            entity.Property(e => e.StationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Village_Ar_Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Station_Emp>(entity =>
        {
            entity.ToTable("Station_Emp");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Station).WithMany(p => p.Station_Emps)
                .HasForeignKey(d => d.Station_Id)
                .HasConstraintName("FK_Station_Emp_Station");
        });

        modelBuilder.Entity<Station_Fees_Type>(entity =>
        {
            entity.ToTable("Station_Fees_Type");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Account_Type).HasComment("نوع الحساب من  system code رقم 33");
            entity.Property(e => e.Fees_Action_ID).HasComment("نوع الوردية وقيمتها");
            entity.Property(e => e.Fees_Type).HasComment("صادر وارد ورية مهندس\r\nرقم 20\r\nفى system code\r\n");
            entity.Property(e => e.Value).HasColumnType("money");
        });

        modelBuilder.Entity<Station_List>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Station_List");

            entity.Property(e => e.Center_Ar_Name).HasMaxLength(150);
            entity.Property(e => e.Expr1)
                .HasMaxLength(53)
                .IsUnicode(false);
            entity.Property(e => e.Gov_Ar_Name).HasMaxLength(150);
            entity.Property(e => e.StationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Station_Accreditation_Data_Name).HasMaxLength(100);
            entity.Property(e => e.Station_Accreditation_Request_Type_Name).HasMaxLength(50);
            entity.Property(e => e.Village_Ar_Name).HasMaxLength(200);
            entity.Property(e => e.station_Status)
                .HasMaxLength(53)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Station_Managing_Director>(entity =>
        {
            entity.ToTable("Station_Managing_Director", tb => tb.HasComment("المدير المسئول"));

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasComment("المدير المسئول");
            entity.Property(e => e.Address_Ar).HasComment("العنوان بالعربية");
            entity.Property(e => e.Address_En)
                .IsUnicode(false)
                .HasComment("العنوان بالانجليزية");
            entity.Property(e => e.Ar_Name).HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.Managing_Director_NID).HasMaxLength(14);
            entity.Property(e => e.Mobile).HasMaxLength(11);

            entity.HasOne(d => d.Station).WithMany(p => p.Station_Managing_Directors)
                .HasForeignKey(d => d.StationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_Managing_Director_Station");
        });

        modelBuilder.Entity<Station_Requst>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Station_Requst");

            entity.Property(e => e.Station_Accreditation_Data_Name).HasMaxLength(100);
            entity.Property(e => e.Station_Accreditation_Request_Type_Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SteamingCompany>(entity =>
        {
            entity.ToTable("SteamingCompany", tb => tb.HasComment("شركات التبخير"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Company_ID).HasComment("شركة المكافحة");
            entity.Property(e => e.GasCompany_ID).HasComment("شركة استيراد الغاز");
            entity.Property(e => e.OutGas_Amount)
                .HasComment("كمية الغاز المنصرف للمعالجة")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OutGas_Date).HasComment("تاريخ الصرف");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Company).WithMany(p => p.SteamingCompanies)
                .HasForeignKey(d => d.Company_ID)
                .HasConstraintName("FK_SteamingCompany_Company_National");

            entity.HasOne(d => d.GasCompany).WithMany(p => p.SteamingCompanies)
                .HasForeignKey(d => d.GasCompany_ID)
                .HasConstraintName("FK_SteamingCompany_Gas_ImportCompany");
        });

        modelBuilder.Entity<SubPart>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_PlantPart");

            entity.ToTable("SubPart", tb => tb.HasComment("الجزء النباتي والطور الحيوى"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مسموح/غير مسموح");
            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.SubPart_Type).WithMany(p => p.SubParts)
                .HasForeignKey(d => d.SubPart_Type_ID)
                .HasConstraintName("FK_SubPart_SubPart_Type");
        });

        modelBuilder.Entity<SubPart_Type>(entity =>
        {
            entity.ToTable("SubPart_Type");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Descreption_Ar).HasComment("وصف أو تنويه");
            entity.Property(e => e.Descreption_En)
                .IsUnicode(false)
                .HasComment("وصف أو تنويه");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("مسموح/غير مسموح");
            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Table_Action>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Table_Ac__3214EC27682E7DA8");

            entity.ToTable("Table_Action");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NOTS).HasMaxLength(100);
            entity.Property(e => e.NOTS_Ar).HasMaxLength(100);
            entity.Property(e => e.NOTS_En).HasMaxLength(100);
            entity.Property(e => e.Name_Ar).HasMaxLength(100);
            entity.Property(e => e.Name_En)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ID_Table_NameNavigation).WithMany(p => p.Table_Actions)
                .HasForeignKey(d => d.ID_Table_Name)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_Action_A_AttachmentTableName");
        });

        modelBuilder.Entity<Table_Action_Log>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Table_Ac__3214EC27772B206F");

            entity.ToTable("Table_Action_Log");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NOTS).HasMaxLength(100);
            entity.Property(e => e.Type_log_ID).HasComment("from A_SystemCode =32");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_ID).HasComment("from A_SystemCode =3");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ID_Table_ActionNavigation).WithMany(p => p.Table_Action_Logs)
                .HasForeignKey(d => d.ID_Table_Action)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_Action_Log_A_AttachmentTableName");
        });

        modelBuilder.Entity<Table_Action_Log_CheckRequest>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Table_Ac__3214EC27772B205F");

            entity.ToTable("Table_Action_Log_CheckRequest");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Type_log_ID).HasComment("from A_SystemCode =32");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_ID).HasComment("from A_SystemCode =3");

            entity.HasOne(d => d.ID_Table_ActionNavigation).WithMany(p => p.Table_Action_Log_CheckRequests)
                .HasForeignKey(d => d.ID_Table_Action)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_Action_Log_CheckRequest_A_AttachmentTableName");
        });

        modelBuilder.Entity<Table_Action_Log_EX>(entity =>
        {
            entity.ToTable("Table_Action_Log_EX");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Type_log_ID).HasComment("from A_SystemCode =32");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_ID).HasComment("from A_SystemCode =3");

            entity.HasOne(d => d.ID_Table_ActionNavigation).WithMany(p => p.Table_Action_Log_EXes)
                .HasForeignKey(d => d.ID_Table_Action)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_Action_Log_EX_A_AttachmentTableName");
        });

        modelBuilder.Entity<Table_Action_Log_Farm>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Table_Action_Log_Farm");

            entity.ToTable("Table_Action_Log_Farm");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NOTS).HasMaxLength(100);
            entity.Property(e => e.Type_log_ID).HasComment("from A_SystemCode =32");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_ID).HasComment("from A_SystemCode =3");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ID_Table_ActionNavigation).WithMany(p => p.Table_Action_Log_Farms)
                .HasForeignKey(d => d.ID_Table_Action)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_Action_Log_Farm_A_AttachmentTableName");
        });

        modelBuilder.Entity<Table_Action_Log_Station>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Table_Action_Log_Station");

            entity.ToTable("Table_Action_Log_Station");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.NOTS).HasMaxLength(100);
            entity.Property(e => e.Type_log_ID).HasComment("from A_SystemCode =32");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Type_ID).HasComment("from A_SystemCode =3");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.ID_Table_ActionNavigation).WithMany(p => p.Table_Action_Log_Stations)
                .HasForeignKey(d => d.ID_Table_Action)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_Action_Log_Station_A_AttachmentTableName");
        });

        modelBuilder.Entity<Transport_Mean>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Transport_Means");

            entity.ToTable("Transport_Mean", tb => tb.HasComment("وسائل النقل"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<TreatmentMainType>(entity =>
        {
            entity.ToTable("TreatmentMainType", tb => tb.HasComment("المعالجة الرئيسية"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<TreatmentMaterial>(entity =>
        {
            entity.ToTable("TreatmentMaterial", tb => tb.HasComment("مادة المعالجة"));

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Item).WithMany(p => p.TreatmentMaterials)
                .HasForeignKey(d => d.Item_ID)
                .HasConstraintName("FK_TreatmentMaterial_Item");

            entity.HasOne(d => d.TreatmentMethods).WithMany(p => p.TreatmentMaterials)
                .HasForeignKey(d => d.TreatmentMethods_ID)
                .HasConstraintName("FK_TreatmentMaterial_TreatmentMethods");
        });

        modelBuilder.Entity<TreatmentMethod>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("طرق المعالجة"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.TreatmentType).WithMany(p => p.TreatmentMethods)
                .HasForeignKey(d => d.TreatmentType_ID)
                .HasConstraintName("FK_TreatmentMethods_TreatmentType");
        });

        modelBuilder.Entity<TreatmentType>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_StationTreatmentType");

            entity.ToTable("TreatmentType", tb => tb.HasComment("أنواع معالجات"));

            entity.Property(e => e.Ar_Name)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.MainType).WithMany(p => p.TreatmentTypes)
                .HasForeignKey(d => d.MainType_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TreatmentType_TreatmentMainType");
        });

        modelBuilder.Entity<USER_TYPE>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("USER_TYPE");

            entity.Property(e => e.ISAdmin).HasDefaultValue(false);
            entity.Property(e => e.User_Delete_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updata_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Union>(entity =>
        {
            entity.ToTable("Union", tb => tb.HasComment("الاتحاد "));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(150)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.En_Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Union_Country>(entity =>
        {
            entity.ToTable("Union_Country", tb => tb.HasComment("الاتحادات الدولية"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Country_ID).HasComment("الدولة");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Union_ID).HasComment("الاتحاد");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Union_Countries)
                .HasForeignKey(d => d.Country_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Union_Country_Country");

            entity.HasOne(d => d.Union).WithMany(p => p.Union_Countries)
                .HasForeignKey(d => d.Union_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Union_Country_Union");
        });

        modelBuilder.Entity<UnitType>(entity =>
        {
            entity.ToTable("UnitType", tb => tb.HasComment("تبع الوارد"));

            entity.Property(e => e.Name_Ar).HasMaxLength(200);
            entity.Property(e => e.Name_En).HasMaxLength(200);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<View_List_Im_PermissionRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_List_Im_PermissionRequest");

            entity.Property(e => e.ExportCountryName).HasMaxLength(100);
            entity.Property(e => e.ImPermission_Number).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ImporterTypeName).HasMaxLength(150);
            entity.Property(e => e.operationTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Village>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_LK_Village");

            entity.ToTable("Village", tb => tb.HasComment("القرية"));

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Ar_Name)
                .HasMaxLength(200)
                .HasComment("الاسم بالعربية");
            entity.Property(e => e.Center_ID).HasComment("المركز");
            entity.Property(e => e.En_Name)
                .HasMaxLength(200)
                .HasComment("الاسم بالانجليزية");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Center).WithMany(p => p.Villages)
                .HasForeignKey(d => d.Center_ID)
                .HasConstraintName("FK_Village_Center");
        });

        modelBuilder.Entity<WebsiteTypeDetail>(entity =>
        {
            entity.ToTable("WebsiteTypeDetail");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.User_Creation_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Deletion_Date).HasColumnType("smalldatetime");
            entity.Property(e => e.User_Updation_Date).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Websitetype>(entity =>
        {
            entity.ToTable("Websitetype");

            entity.Property(e => e.ID).ValueGeneratedNever();
        });

        modelBuilder.Entity<pos_information>(entity =>
        {
            entity.ToTable("pos_information", tb => tb.HasComment("ماكينات الدفع"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BankAccount).HasMaxLength(30);
            entity.Property(e => e.place).HasMaxLength(250);
            entity.Property(e => e.pos_number).HasMaxLength(100);

            entity.HasOne(d => d.Outlet).WithMany(p => p.pos_informations)
                .HasForeignKey(d => d.Outlet_ID)
                .HasConstraintName("FK_pos_information_Outlet");
        });
        modelBuilder.HasSequence("A__plant_Error_Save_SEQ")
            .StartsAt(118779L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A__User_Login_SEQ")
            .StartsAt(282517L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_Ex_CheckRequest_SEQ")
            .StartsAt(243L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_Ex_CommitteeResult_Infection_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_Im_CommitteeResult_Infection_SEQ")
            .StartsAt(21621L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_SEQ")
            .StartsAt(530293L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("A_AttachmentData_Station_SEQ")
            .StartsAt(2149L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("A_AttachmentTableName_SEQ")
            .StartsAt(27L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("A_AttachmentTableType_SEQ")
            .StartsAt(31L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("A_SystemCode_SEQ")
            .StartsAt(142L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("A_SystemCodeType_SEQ")
            .StartsAt(34L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("AnalysisLab_SEQ")
            .StartsAt(13L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("AnalysisLabType_SEQ")
            .StartsAt(21L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("AnalysisType_SEQ")
            .StartsAt(21L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Andriod_Location_SEQ")
            .StartsAt(401950L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Andriod_Operation_SEQ")
            .StartsAt(23L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Biological_Phase_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Center_SEQ")
            .StartsAt(324L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CommitteeEmployee_SEQ")
            .StartsAt(148950L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("CommitteeResultType_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("CommitteeType_SEQ")
            .StartsAt(15L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Company_National_SEQ")
            .StartsAt(29759L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CompanyAccreditation_Committee_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CompanyAccreditation_Payment_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CompanyAccreditation_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("CompanyActivity_SEQ")
            .StartsAt(33054L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("CompanyActivityType_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("ContactType_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Continents_SEQ")
            .StartsAt(12L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Country_SEQ")
            .StartsAt(233L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Enrollment_type_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificateAddtion_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificateAddtionUser_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_CertificatesNewCountry_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequests_SEQ")
            .StartsAt(2L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequestsFiles_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequestsLotData_SEQ")
            .StartsAt(5L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequestsPayments_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CertificatesRequestsPaymentsDetailes_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Ex_CertificatesRequestsPaymentsType_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_CheckRequest_Customs_Message_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Data_SEQ")
            .StartsAt(14210L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Fees_SEQ")
            .StartsAt(13956L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Final_Result_SEQ")
            .StartsAt(15093L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Items_Lot_Category_SEQ")
            .StartsAt(56363L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Items_Lot_Result_SEQ")
            .StartsAt(52197L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Items_SEQ")
            .StartsAt(28763L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_CheckRequest_Lot_Result_Status_SEQ")
            .StartsAt(12L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Organization_Distribution_Detials_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Organization_Distribution_Master_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Organization_Distribution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Places_SEQ")
            .StartsAt(14212L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_CheckRequest_Port_SEQ")
            .StartsAt(33790L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_RefuseReason_SEQ")
            .StartsAt(103L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_SampleData_Confirm_SEQ")
            .StartsAt(804L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_SampleData_SEQ")
            .StartsAt(1606L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_SEQ")
            .StartsAt(14215L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequest_Visa_SEQ")
            .StartsAt(12155L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequestData_Extra_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CheckRequset_Shipping_Method_SEQ")
            .StartsAt(28746L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("EX_Choose_SampleData_SEQ")
            .StartsAt(1456L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("EX_Choose_Treatment_SEQ")
            .StartsAt(66L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Ex_CommitteeCheckLocation_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CommitteeResult_Confirm_SEQ")
            .StartsAt(27858L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CommitteeResult_Infection_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CommitteeResult_SEQ")
            .StartsAt(62207L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("EX_Constrain_Country_Item_SEQ")
            .StartsAt(54L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("EX_Constrain_Text_SEQ")
            .StartsAt(180L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("EX_Constrain_Type_SEQ")
            .StartsAt(13L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_ContactData_SEQ")
            .StartsAt(69874L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_AnalysisLabType_SEQ")
            .StartsAt(145L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_ArrivalPort_SEQ")
            .StartsAt(3L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_SEQ")
            .StartsAt(1535L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_Text_SEQ")
            .StartsAt(5492L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_CountryConstrain_Treatment_SEQ")
            .StartsAt(244L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("EX_Fees_Type_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_Final_Result_SEQ")
            .StartsAt(19L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Ex_OpertaionType_SEQ")
            .StartsAt(4L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_Request_TreatmentData_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_Request_TreatmentData_SEQ")
            .StartsAt(21L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_RequestCommittee_Fees_ENG_SEQ")
            .StartsAt(6596L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_RequestCommittee_SEQ")
            .StartsAt(24373L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_RequestCommittee_Shift_SEQ")
            .StartsAt(15377L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Ex_Visa_SEQ")
            .StartsAt(3L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Family_SEQ")
            .StartsAt(2507L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_CheckList_SEQ")
            .StartsAt(24L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_CheckList_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_CheckList_SEQ")
            .StartsAt(11761L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Constrain_SEQ")
            .StartsAt(16196L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Examination_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Examination_SEQ")
            .StartsAt(71L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Final_Result_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_SEQ")
            .StartsAt(57L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Committee_Shift_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Company_SEQ")
            .StartsAt(403L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Constrain_SEQ")
            .StartsAt(3337L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Constrain_Text_SEQ")
            .StartsAt(27L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Country_CheckList_SEQ")
            .StartsAt(1975L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Country_SEQ")
            .StartsAt(33162L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Farm_Fees_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_ItemCategories_SEQ")
            .StartsAt(608L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Request_ItemCategories_SEQ")
            .StartsAt(500L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Request_Refuse_Reason_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_Request_SEQ")
            .StartsAt(436L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Farm_Request_Type_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_SampleData_Confirm_Item_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_SampleData_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_SampleData_Item_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farm_SampleData_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farms_Organization_Distribution_Detials_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Farms_Organization_Distribution_Master_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("FarmsData_SEQ")
            .StartsAt(349L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Action_SEQ")
            .StartsAt(2518L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Certificates_Payment_Detiles_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Fees_Money_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Fees_process_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Fees_TableName_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Transactions_Detiles_SEQ")
            .StartsAt(195572048336L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Transactions_Payment_Detiles_SEQ")
            .StartsAt(801L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Fees_Transactions_SEQ")
            .StartsAt(18213110801L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Fees_Type_Action_SEQ")
            .StartsAt(14L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("FeesAmount_Fixed_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("FeesType_SEQ")
            .StartsAt(20L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("FreeZone_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("FumigationUnit_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Gas_ImportCompany_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("General_Admin_SEQ")
            .StartsAt(63L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Governate_SEQ")
            .StartsAt(36L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Group_SEQ")
            .StartsAt(100L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("HagrContact_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_CheckRequest_Customs_Message_SEQ")
            .StartsAt(260850L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Data_SEQ")
            .StartsAt(264754L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Distribution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Final_Result_SEQ")
            .StartsAt(122918L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Items_Lot_Category_SEQ")
            .StartsAt(508742L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Items_Lot_Result_SEQ")
            .StartsAt(154755L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Items_SEQ")
            .StartsAt(468276L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_CheckRequest_Lot_Result_Status_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Manafest_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_CheckRequest_Port_SEQ")
            .StartsAt(547724L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_RefuseReason_SEQ")
            .StartsAt(48912L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_SampleData_Confirm_SEQ")
            .StartsAt(10438L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_SampleData_SEQ")
            .StartsAt(14290L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_SEQ")
            .StartsAt(264810L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequest_Visa_SEQ")
            .StartsAt(116578L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequestData_Extra_SEQ")
            .StartsAt(263105L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CheckRequset_Shipping_Method_SEQ")
            .StartsAt(432902L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Committee_CustodyPlace_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_CommitteeCheckLocation_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CommitteeResult_Confirm_SEQ")
            .StartsAt(34096L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CommitteeResult_Infection_SEQ")
            .StartsAt(67L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CommitteeResult_SEQ")
            .StartsAt(198514L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Constrain_Initiator_Text_SEQ")
            .StartsAt(70451L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_Constrain_Type_SEQ")
            .StartsAt(71L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Constrains_Special_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CountryConstrain_ArrivalPort_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CountryConstrain_Text_SEQ")
            .StartsAt(150L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CustodyPlace_CheckRequest_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_CustodyPlace_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_CustodyPlaceType_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Execution_Items_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Execution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Final_Result_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Initiator_SEQ")
            .StartsAt(129292L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ItemsLotDivision_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Manafest_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_OpertaionType_SEQ")
            .StartsAt(17L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItem_Division_Custody_DismissCommittee_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItem_Division_Custody_ReceiveCommittee_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItem_Division_Custody_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItems_Category_SEQ")
            .StartsAt(3841L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionItems_SEQ")
            .StartsAt(357926L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionRequest_RefuseReason_SEQ")
            .StartsAt(1948L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_PermissionRequest_SEQ")
            .StartsAt(370475L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_ProcedureType_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Request_Port_SEQ")
            .StartsAt(715484L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Request_TreatmentData_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Request_TreatmentData_SEQ")
            .StartsAt(11364L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestCommittee_Procedure_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestCommittee_SEQ")
            .StartsAt(148950L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestCommittee_Shift_SEQ")
            .StartsAt(159186L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestDat_Extra_SEQ")
            .StartsAt(358014L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_RequestData_SEQ")
            .StartsAt(357925L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_ItemPlant_Inseket_Lieble_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_ItemPlant_Product_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_Organization_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_Person_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_ScientificResearch_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Stores_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_SubDivission_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Im_TransUnderCustodyReason_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Im_Visa_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Im_Warehouses_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("InternationalTransportation_SEQ")
            .StartsAt(9639L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Item_Purpose_SEQ")
            .StartsAt(39L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Item_SEQ")
            .StartsAt(3250L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Item_ShortName_SEQ")
            .StartsAt(2044L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Item_Status_SEQ")
            .StartsAt(66L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Item_Type_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ItemCategories_Group_SEQ")
            .StartsAt(170L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ItemCategories_SEQ")
            .StartsAt(9090L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("ItemCategories_Type_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ItemPart_SEQ")
            .StartsAt(2702L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Kingdom_SEQ")
            .StartsAt(16L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Level_SEQ")
            .StartsAt(12L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("LiableItems_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("LiableItems_ShortName_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("LiableItems_Status_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("MainCalssification_SEQ")
            .StartsAt(23L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Order_SEQ")
            .StartsAt(447L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Outlet_Employee_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Outlet_SEQ")
            .StartsAt(245L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Package_Material_SEQ")
            .StartsAt(12L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Package_Type_SEQ")
            .StartsAt(21L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Pallet_Data_Ex_CheckRequest_Distribution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Pallet_Data_Organization__Distribution_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Person_SEQ")
            .StartsAt(1071L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PhylumSubphylum_SEQ")
            .StartsAt(74L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Port_International_SEQ")
            .StartsAt(22928L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Port_Type_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PortNational_SEQ")
            .StartsAt(34L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PortOrganization_SEQ")
            .StartsAt(5L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("pos_information_SEQ")
            .StartsAt(32L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Public_Organization_SEQ")
            .StartsAt(696L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("PublicOrganization_Type_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("QualitativeGroup_SEQ")
            .StartsAt(58L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Refuse_Reason_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Regional_Area_SEQ")
            .StartsAt(27L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Regions_SEQ")
            .StartsAt(59L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("SecondaryClassification_SEQ")
            .StartsAt(38L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("ShiftTiming_SEQ")
            .StartsAt(10L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Shipment_Mean_SEQ")
            .StartsAt(9L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ShippingAgencies_SEQ")
            .StartsAt(484L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("ShippingCompanies_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_CheckList_SEQ")
            .StartsAt(446L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_CheckList_Confirm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_CheckList_SEQ")
            .StartsAt(17488L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_Final_Result_SEQ")
            .StartsAt(610L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_Imge_SEQ")
            .StartsAt(753L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_SEQ")
            .StartsAt(956L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Committee_Shift_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Data_Country_SEQ")
            .StartsAt(3728L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Data_Item_ShortName_SEQ")
            .StartsAt(84L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Data_SEQ")
            .StartsAt(33L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Request_Fees_ENG_SEQ")
            .StartsAt(5L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Request_Fees_SEQ")
            .StartsAt(684L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_Request_SEQ")
            .StartsAt(1014L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Station_Accreditation_Request_Type_SEQ")
            .StartsAt(4L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Accreditation_SEQ")
            .StartsAt(363L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_CheckList_SEQ")
            .StartsAt(164L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Station_Constrain_Country_Item_SEQ")
            .StartsAt(17L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Station_Constrain_Type_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Emp_SEQ")
            .StartsAt(756L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("Station_Fees_Type_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_Managing_Director_SEQ")
            .StartsAt(880L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Station_SEQ")
            .StartsAt(505L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("StationActivityType_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("StationCompany_SEQ")
            .StartsAt(150L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("StationContacts_SEQ")
            .StartsAt(584L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("SteamingCompany_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("SubPart_SEQ")
            .StartsAt(125L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("SubPart_Type_SEQ")
            .StartsAt(16L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_CheckRequest_SEQ")
            .StartsAt(1622147L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_EX_SEQ")
            .StartsAt(10348L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_Farm_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_SEQ")
            .StartsAt(560730L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence("Table_Action_Log_Station_SEQ")
            .StartsAt(1785L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Table_Action_SEQ")
            .StartsAt(59L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("Transport_Mean_SEQ")
            .StartsAt(7L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("TreatmentMainType_SEQ")
            .StartsAt(6L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("TreatmentMaterial_SEQ")
            .StartsAt(15L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("TreatmentMethods_SEQ")
            .StartsAt(14L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("TreatmentType_SEQ")
            .StartsAt(8L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Union_Country_SEQ")
            .StartsAt(92L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Union_SEQ")
            .StartsAt(11L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<byte>("UnitType_SEQ")
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<short>("Village_SEQ")
            .StartsAt(4411L)
            .HasMin(1L)
            .IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
