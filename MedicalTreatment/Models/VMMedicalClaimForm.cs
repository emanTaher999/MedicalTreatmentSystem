using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MedicalTreatment.Models
{
    public class VMMedicalClaimForm
    {
        public MEDICALCLAIMFORM medForm { get; set; }
        public List<MEDICALCLAIMFORMDETAIL> medFormDet { get; set; }
        public decimal? ResidualCeiling { get; set; }
        public decimal? ConsumerCeiling { get; set; }
    }
}