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
    
    public partial class tb_message
    {
        public int id_message { get; set; }
        public int id_ad { get; set; }
        public string message { get; set; }
        public string email_from { get; set; }
        public Nullable<System.DateTime> date_received { get; set; }
        public Nullable<int> id_user { get; set; }
        public string status { get; set; }
    
        public virtual tb_ads tb_ads { get; set; }
    }
}
