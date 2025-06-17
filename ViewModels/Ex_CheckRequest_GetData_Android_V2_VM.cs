using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Ex_CheckRequest_GetData_Android_V2_VM
    {
        
            public bool ExportAbroad { get; set; }
        public Nullable<byte> IsExport { get; set; }
        public Nullable<long> CheckRequest_Id { get; set; }
        public string CheckRequest_Number { get; set; }
        public Nullable<bool> IsAcceppted { get; set; }
        //abeer 13-12-2022
        // public string Outlet_Name { get; set; }
        // public string Outlet_Address { get; set; }
        //public string Govern_Name { get; set; }
        //  public string General_Admin_Name { get; set; }

        public Nullable<int> ImporterType_Id { get; set; }
        public string Reciever_Name { get; set; }
        public string ImportCompany_Address { get; set; }
        public string ExportCountry_Name { get; set; }
        public string TransientCountry_Name { get; set; }
        public string port_arrive_Name { get; set; }
        public string port_transient_Name;

        public string PortNational_Shippment_Name { get; set; }

        public string Ship_Name { get; set; }
        public string Committee_Type { get; set; }
        public Nullable<System.DateTime> Check_Date { get; set; }
        public string RequestCommittee_Status { get; set; }
        public string Outlet_Name { get; set; }
        public string Govern_Name { get; set; }
        public List<_x0040_Item_Data> Item_Data { get; set; } = new List<_x0040_Item_Data>();

     
    }
}
