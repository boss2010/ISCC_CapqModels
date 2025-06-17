namespace ViewModels
{
    public class TreatmentDataDTO
    {
        public long TreatmentDataID { get; set; }
        public long ID { get; set; }
        public long lot_ID { get; set; }
        public string lot_Number { get; set; }
        public long? RequestLotData_ID { get; set; }
        public string Notes { get; set; }
        public string companyName { get; set; }
        public string stationName { get; set; }

        public string stationPlace { get; set; }
        public byte treatment_MethodId { get; set; }
        public string treatment_MethodName { get; set; }
        public string treatment_TypeName { get; set; }

        public byte treatment_TypeId { get; set; }
        public string treatmentMaterial_Name { get; set; }
        public decimal? treatmentMat_Amount { get; set; }
        public long? Item_ShortName_ID { get; set; }
        public decimal? size { get; set; }
        public decimal? dose { get; set; }
        public int? exposure_Hour { get; set; }
        public int? exposure_Minute { get; set; }
        public int? exposure_Day { get; set; }
        public decimal? temperature { get; set; }
        public decimal? thermalSealNumber { get; set; }

        public Nullable<bool> IS_Total { get; set; }
        public bool? IS_Total_Android { get; set; }
        public bool? IS_From_Android { get; set; }
        public string TreatmentDataID_List { get; set; }//used for confirmation Saving
        public long RequestCommittee_ID { get; set; }
        public string Procedures { get; set; }
    }
}