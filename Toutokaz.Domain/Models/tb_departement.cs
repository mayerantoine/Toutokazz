//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Toutokaz.Domain.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_departement
    {
        public tb_departement()
        {
            this.tb_commune = new HashSet<tb_commune>();
            this.tb_ads = new HashSet<tb_ads>();
        }
    
        public int id_departement { get; set; }
        public string departement { get; set; }
    
        public virtual ICollection<tb_commune> tb_commune { get; set; }
        public virtual tb_departement tb_departement1 { get; set; }
        public virtual tb_departement tb_departement2 { get; set; }
        public virtual ICollection<tb_ads> tb_ads { get; set; }
    }
}
