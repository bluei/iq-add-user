//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iq_add_user.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DOC_TEAM
    {
        public long ID { get; set; }
        public long DOC_LIBRARY_ID { get; set; }
        public long TEAM_MEMBER_ID { get; set; }
        public Nullable<byte> SEQ { get; set; }
        public string TYPE { get; set; }
    
        public virtual DOC_LIBRARY DOC_LIBRARY { get; set; }
        public virtual TEAM_MEMBER TEAM_MEMBER { get; set; }
    }
}