using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Models;
namespace ViewModels
{
    public class HomeVM
    {
        public List<ListSliderVM>? Slider { get; set; } = new List<ListSliderVM>();
        public List<ListIntroVM>? Intro { get; set; } = new List<ListIntroVM>();
        public List<ListNewsVM>? News { get; set; } = new List<ListNewsVM>();   
        public List<ListAvertisVM>? Advertis { get; set; } = new List<ListAvertisVM>();  
        public List<ListAlertVM>? Alert { get; set; } = new List<ListAlertVM>();    
    }
    ///////////////////////////////Slider///////////////////////////////////////
    public class ListSliderVM
    {
        public int ID { get; set; }
        public string? filepath { get; set; }
        public DateTime? User_Creation_Date { get; set; }
    }

    ///////////////////////////////Intro///////////////////////////////////////
    public class ListIntroVM
    {
        public int ID { get; set; }
        public string? TitleAr { get; set; }
        public string? descAr { get; set; }

        public string? filepath { get; set; }
        public DateTime? User_Creation_Date { get; set; }
    }

    ///////////////////////////////News///////////////////////////////////////
    public class ListNewsVM
    {
        public int ID { get; set; }
        public string? TitleAr { get; set; }
        public string? descAr { get; set; }

        public string? filepath { get; set; }
        public DateTime? User_Creation_Date { get; set; }
    }
    ///////////////////////////////Avertis///////////////////////////////////////
    public class ListAvertisVM
    {
        public int ID { get; set; }
        public string? TitleAr { get; set; }
        public string? descAr { get; set; }

        public string? filepath { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? User_Creation_Date { get; set; }
    }///////////////////////////////Alert///////////////////////////////////////
    public class ListAlertVM
    {
        public DateTime? Date { get; set; }
        public int ID { get; set; }
        public string? TitleAr { get; set; }
        public string? descAr { get; set; }

        public string? filepath { get; set; }
        public DateTime? User_Creation_Date { get; set; }
    }

}