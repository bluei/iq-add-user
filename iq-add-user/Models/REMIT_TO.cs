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
    
    public partial class REMIT_TO
    {
        public long ID { get; set; }
        public Nullable<long> VENDOR_ID { get; set; }
        public string ATTN { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string ADDR3 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string COUNTRY { get; set; }
        public string CURRENCY_ID { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string FAX_NUMBER { get; set; }
        public string PRIME_CONTACT { get; set; }
        public string ECODE { get; set; }
        public Nullable<long> EID { get; set; }
        public Nullable<System.DateTime> EDATE_TIME { get; set; }
        public string ECOPY { get; set; }
        public string USE_USA_MASK { get; set; }
        public string BANK_ACCT_NO { get; set; }
        public string DEFAULT_REMIT_TO { get; set; }
        public string CD_CHECK_REPNAME { get; set; }
        public string CD_CHECK_REMITANCE { get; set; }
        public string AP_REMITTANCE_TYPE { get; set; }
        public string BIC_SWIFT { get; set; }
        public string IBAN { get; set; }
        public string CLEARING_NO { get; set; }
        public string INTL_PAY_TYPE { get; set; }
        public byte[] CONTACT_PHOTO { get; set; }
        public string EFT_EMAIL_ADDR { get; set; }
        public string REMIT_TO_NO { get; set; }
    
        public virtual VENDOR VENDOR { get; set; }
    }
}