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
    
    public partial class ORGANIZATIONPROFILE
    {
        public short ID { get; set; }
        public int ORGANIZATIONID { get; set; }
        public byte[] LOGO { get; set; }
        public byte[] FORMLOGO { get; set; }
        public string ADDRESS { get; set; }
        public string STATUS { get; set; }
        public System.DateTime LASTUPDATED { get; set; }
    
        public virtual ORGANIZATIONSTRUCTURE ORGANIZATIONSTRUCTURE { get; set; }
    }
}
