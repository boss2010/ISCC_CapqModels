using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Ex_CountryConstrainVM
    {
        public long IDCountry { get; set; }
        public string CountryNameAr { get; set; }
        public string ItemName { get; set; }
        public string CountryNameEn { get; set; }
        public short? Country_Id { get; set; }
        public long? Item_ShortName_ID { get; set; }
        public long? Item_ID { get; set; }
        public string ShortName_Ar { get; set; }

        public string? ConstrainText_Ar { get; set; }

        public string? ConstrainText_En { get; set; }

        public string? InSide_Certificate_Ar { get; set; }

        public string? InSide_Certificate_En { get; set; }
        ///////////Ex_CountryConstrains//////////
        public long ID { get; set; }

        /// <summary>
        /// الدولة المستوردة
        /// </summary>
        public short? Import_Country_ID { get; set; }

        /// <summary>
        /// دولة عبور
        /// </summary>
        public short? TransportCountry_ID { get; set; }

        /// <summary>
        /// product or plant ID manual no relation
        /// </summary>
        public long Item_ShortName_id { get; set; }

        public long? ItemCategories_ID { get; set; }

        /// <summary>
        /// هل محطة معتمدة
        /// </summary>
        public bool? IsStationAccreditation { get; set; }

        /// <summary>
        /// هل مزرعة معتمدة
        /// </summary>
        public bool? IsFarmAccreditation { get; set; }

        /// <summary>
        /// هل شركة معتمدة
        /// </summary>
        public bool? IsCompanyAccreditation { get; set; }

        public bool IsActive { get; set; }

        public long User_Creation_Id { get; set; }

        public DateTime User_Creation_Date { get; set; }

        public long? User_Updation_Id { get; set; }

        public DateTime? User_Updation_Date { get; set; }

        public long? User_Deletion_Id { get; set; }

        public DateTime? User_Deletion_Date { get; set; }
    }
}
