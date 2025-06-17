namespace ViewModels
{
    public class Itemsample_data
    {
        public long Sample_dataId { get; set; }
        public string AnalysisLab_Name { get; set; }
        public string AnalysisType_Name { get; set; }
        public string Sample_BarCode { get; set; }
        public double? SampleRatio { get; set; }
        public double? SampleSize { get; set; }
        public bool? IS_From_Android { get; set; }
        public bool? IS_Total { get; set; }
        public long? LotData_ID { get; set; }
        //abeer
        public long? AnalysisType_ID { get; set; }
        public string Lotnum { get; set; }
    }
}