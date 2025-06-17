namespace ViewModels
{
    public class _x0040_Item_Data
    {

        //public string item_Category_Name;
        //public string item_CategoryGroup_Name;
        //public string item_Category_Register;
        //public string contaier_type_Name;

        // public long ID { get; set; }

        public string Sub_Name { get; set; }
        public string ItemState_Name { get; set; }

        public long Item_ShortName_id { get; set; }
        public byte? Item_Type_ID { get; set; }

        public string Item_Type_Name { get; set; }
        public string Item_ShortName_Name { get; set; }
        public string Item_Type_Color { get; set; }
        public string Item_Name { get; set; }
        public string Scientific_Name { get; set; }
        public int ItemStatus_ID { get; set; }
        public int Purpose_ID { get; set; }
        public string Purpose_Name { get; set; }

        public IEnumerable<_x0040_temp_table_Lot> Lot_Data { get; set; }
        public IEnumerable<TreatmentDataDTO> TreatmentLot { get; set; }
        public IEnumerable<Itemsample_data> Sample_Data { get; set; }

        public IEnumerable<conData> Constrain_Data { get; set; }
        public IEnumerable<TakenAnalysis> TakenAnalysis { get; set; }
        public long Initiator_ID { get; set; }
        public string Initiator_CountryName { get; set; }
        public long? Im_CheckRequset_Shipping_Method_ID { get; set; }
        public int IsExport { get; set; }
        public int Isanalysis { get; set; } //per anylsis type (if total=1 will be divided according to lots count)
        public int Istreatment { get; set; }//per treatment type (if total=1 will be divided according to lots count)

        public int Has_Result { get; set; }  /* for conformation  0-> no data ,  1-> Item Res  2->Lot res */
        public bool Has_Confirm { get; set; }//admin had set data
        public string Procedures { get; set; }
    }
}