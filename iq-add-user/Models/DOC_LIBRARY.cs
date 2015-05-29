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
    
    public partial class DOC_LIBRARY
    {
        public DOC_LIBRARY()
        {
            this.DOC_TEAM = new HashSet<DOC_TEAM>();
        }
    
        public long ID { get; set; }
        public string DESCRIP { get; set; }
        public string PATH { get; set; }
        public string DEFAULT_EXT { get; set; }
        public string TYPE { get; set; }
        public string REVISION_PATH { get; set; }
        public string KEEP_REVISIONS { get; set; }
        public string SEQ_WORKFLOW { get; set; }
        public string REPOSITORY_ALIAS { get; set; }
        public string REPOSITORY_DATA { get; set; }
        public string TEAM_MEMBER_ONLY { get; set; }
        public string CONVERT_TO_PDF { get; set; }
        public string PDF_PATH { get; set; }
        public Nullable<long> EPLANT_ID { get; set; }
        public Nullable<long> WF_TYPE_ID { get; set; }
        public string WF_KIND { get; set; }
        public string DOC_CHANGE_DESC_MANDATORY { get; set; }
        public string IS_REPOSITORY_UNRESTRICTED { get; set; }
        public string CONVERT_TO_PDF_DISPLAY { get; set; }
        public string IS_CONFIDENTIAL { get; set; }
    
        public virtual EPLANT EPLANT { get; set; }
        public virtual ICollection<DOC_TEAM> DOC_TEAM { get; set; }
    }
}
