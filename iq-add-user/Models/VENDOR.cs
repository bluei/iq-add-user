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
    
    public partial class VENDOR
    {
        public VENDOR()
        {
            this.EXP_USER = new HashSet<EXP_USER>();
            this.REMIT_TO = new HashSet<REMIT_TO>();
        }
    
        public long ID { get; set; }
        public string VENDORNO { get; set; }
        public string COMPANY { get; set; }
        public string ATTN { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string ADDR3 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string COUNTRY { get; set; }
        public string ZIP { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string FAX_NUMBER { get; set; }
        public string E_MAIL_ADDR { get; set; }
        public Nullable<decimal> CREDIT_LIMIT { get; set; }
        public Nullable<long> TERMS_ID { get; set; }
        public Nullable<decimal> YTD_PURCHASES { get; set; }
        public string FED_TAX_ID { get; set; }
        public string CUSER1 { get; set; }
        public string CUSER2 { get; set; }
        public string CUSER3 { get; set; }
        public Nullable<decimal> NUSER1 { get; set; }
        public Nullable<decimal> NUSER2 { get; set; }
        public string GL_ACCT { get; set; }
        public Nullable<decimal> TAXRATE { get; set; }
        public string QA_RATING { get; set; }
        public string VEN_RATING { get; set; }
        public Nullable<long> GLACCT_ID_EXP { get; set; }
        public string PRIME_CONTACT { get; set; }
        public string ECODE { get; set; }
        public Nullable<long> EID { get; set; }
        public Nullable<System.DateTime> EDATE_TIME { get; set; }
        public string ECOPY { get; set; }
        public string STATUS_ID { get; set; }
        public Nullable<long> CURRENCY_ID { get; set; }
        public string TAXABLE { get; set; }
        public Nullable<long> FREIGHT_ID { get; set; }
        public string FOB { get; set; }
        public Nullable<byte> STATEMENT_DATE { get; set; }
        public Nullable<long> ARCUSTO_ID { get; set; }
        public string INCLUDE_IN_1099 { get; set; }
        public string USE_USA_MASK { get; set; }
        public string PK_HIDE { get; set; }
        public string BANK_ACCT_NO { get; set; }
        public string WEB_SITE_URL { get; set; }
        public string FAX_NUMBER2 { get; set; }
        public string EMARKET_PLACE { get; set; }
        public string SUBJECT_TO_RATING { get; set; }
        public Nullable<long> TAX_CODE_ID { get; set; }
        public string CD_CHECK_REPNAME { get; set; }
        public string CD_CHECK_REMITANCE { get; set; }
        public string PO_REPORT { get; set; }
        public Nullable<long> AP_GLACCT_ID { get; set; }
        public string INFO_PO { get; set; }
        public Nullable<long> GLTEMPLATE_ID { get; set; }
        public string EVAL_SHORT_SHIPMENT { get; set; }
        public string ALERTMSG { get; set; }
        public string SERIAL_ASN_REQUIRED { get; set; }
        public string DISTLIST { get; set; }
        public string ARCH_AUTO_PO_REL_ACT { get; set; }
        public Nullable<long> EPLANT_ID { get; set; }
        public string RECEIVING_UD_LOT_NUM { get; set; }
        public string CUSER4 { get; set; }
        public string CUSER5 { get; set; }
        public string A1099_CODE { get; set; }
        public string VENDOR_PREFIX { get; set; }
        public string IS_OEM { get; set; }
        public Nullable<long> TRANS_CODE_ID { get; set; }
        public Nullable<long> CURRENCY_LANGUAGE_ID { get; set; }
        public string INFO_AP { get; set; }
        public string NAME_1099 { get; set; }
        public Nullable<long> DEFAULT_SHIP_TO { get; set; }
        public string USE_VENDOR_PORTAL_INVC { get; set; }
        public Nullable<System.DateTime> CREATED { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CHANGED { get; set; }
        public string CHANGEDBY { get; set; }
    
        public virtual EPLANT EPLANT { get; set; }
        public virtual ICollection<EXP_USER> EXP_USER { get; set; }
        public virtual ICollection<REMIT_TO> REMIT_TO { get; set; }
    }
}
