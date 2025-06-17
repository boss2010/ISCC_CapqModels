namespace ViewModels
{
    public class _x0040_temp_table_Lot
    {
        public long ID { get; set; }
        public bool is_Total { get; set; }
        public long Lot_ID { get; set; }
        public string Lot_Number { get; set; }
        public int Package_Count { get; set; }
        public decimal? Net_Weight { get; set; }
        public decimal? Gross_Weight { get; set; }
        public double Package_Based_Weight { get; set; }
        public string PackageWeight { get; set; }
        public string Package_Net_Weight { get; set; }
        public bool IsAccepted { get; set; }
        public decimal? Based_Weight { get; set; }
        public decimal? Package_Weight { get; set; }
        public string Shipment_Mean_Name { get; set; }
        public string Transport_Mean_Name { get; set; }
        public string Package_Material_Name { get; set; }
        public string Package_Type_Name { get; set; }
        public string NavigationalNumber { get; set; }
        public string ContainerNumber { get; set; }
        public string ShipholdNumber { get; set; }
        public string ContainerName { get; set; }
        public int? Units_Number { get; set; }

        public double? Size { get; set; }
        public string Category_Name { get; set; }
        public string CategoryGroup_Name { get; set; }
        public string ContaierType_Nmae { get; set; }
        public string Item_Category_Register { get; set; }
        public bool Is_ContainerNumberDisplay { get; set; }
        public string Order_Text { get; set; }
        public string Grower_Number { get; set; }
        public string Number_Wooden_Package { get; set; }

        //Eslam
        public Nullable<short> ExportCountry_Id { get; set; }
        public Nullable<short> TransitCountry_Id { get; set; }
    }
}