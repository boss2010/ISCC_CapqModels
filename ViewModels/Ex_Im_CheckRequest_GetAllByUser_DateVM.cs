using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ExportImportActivity_API
    {
        public class Ex_Im_CheckRequest_GetAllByUser_DateVM
        {
            public byte Row_Num { get; set; }
            //byte not bool
            public Nullable<byte> IsExport { get; set; }
            public string CheckRequest_Number { get; set; }
            public Nullable<long> checkRequest_Id { get; set; }
            public Nullable<long> Committee_ID { get; set; }
            public string Committee_Type_Name { get; set; }
            public Nullable<byte> Committee_Type_Id { get; set; }
            public string RequestCommittee_Status { get; set; }
            public Nullable<byte> RequestCommittee_Status_Id { get; set; }
            public string BarCode { get; set; }
            public string Emp_Committe { get; set; }
            public string Request_Treatment { get; set; }

            public CheckRequest_GetTreatment_Analsis_DTO Request_Treatment_Data { get; set; }

            public string FarmAnlysisCount_XMl { get; set; }

        }

        public class CheckRequest_GetTreatment_Analsis_DTO
        {
            public int row_num { get; set; }
            public Nullable<long> checkRequest_Id { get; set; }
            public Nullable<byte> IsExport { get; set; }
            public string Item_Data { get; set; }
            public Nullable<short> Analysis_Total { get; set; }
            public Nullable<short> Treatment_Total { get; set; }
            public Nullable<short> Check_Total { get; set; }
        }
    }
}
