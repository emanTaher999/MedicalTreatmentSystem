//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalTreatment.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JOBTITLE
    {
        public JOBTITLE()
        {
            this.INDIVIDUALS = new HashSet<INDIVIDUAL>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public string NAMEEN { get; set; }
        public string STATUS { get; set; }
        public System.DateTime LASTUPDATED { get; set; }
    
        public virtual ICollection<INDIVIDUAL> INDIVIDUALS { get; set; }
    }
}