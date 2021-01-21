using System;
using System.Collections.Generic;
using System.Text;

namespace HaLi.FPS
{
    /// <summary>
    /// (Hong Kong) Faster Payments System 轉數快
    /// </summary>
    public class FPS
    {
        /// <summary>
        /// Should be fixed, HKICL is provider/operator of fps in hong kong
        /// </summary>
        public string OperatorDomain { get; set; } = "hk.com.hkicl";

        /// <summary>
        /// Payee identify include in qr code
        /// </summary>
        public enum IdentifyMethod
        {
            /// <summary>
            /// FPS(轉數快) Payee Identifier, should be 7 Alphanumeric, and future will change 10 Alphanumeric
            /// </summary>
            FPS_ID,
            /// <summary>
            /// Payee mobile number, * seems need bank support
            /// </summary>
            MobileNumber,
            /// <summary>
            /// Payee email address, * seems need bank support
            /// </summary>
            EmailAddress
        }
        /// <summary>
        /// Identify method
        /// </summary>
        public IdentifyMethod Identification { get; set; } = IdentifyMethod.FPS_ID;

        /// <summary>
        /// Payee FPS ID
        /// </summary>
        public string FpsID { get; set; }
        /// <summary>
        /// Bank Code which HK FPS supported, default "004" is HSBC 
        /// No use when use FpsID
        /// <see cref="https://www.hkicl.com.hk/eng/information_centre/clearing_code_and_branch_code_list.php">list here</see>
        /// </summary>
        public string BankCode { get; set; } = "004";
        /// <summary>
        /// Payee FPS Mobile Number, Format should be +852-xxxxxxxx
        /// No use when use FpsID
        /// </summary>
        public string FpsMobile { get; set; }
        /// <summary>
        /// Payee FPS Email address
        /// No use when use FpsID
        /// </summary>
        public string FpsEmail { get; set; }

        /// <summary>
        /// Document say not used in HK FPS, default "0000"
        /// </summary>
        public string MerchantCategoryCode { get; set; } = "0000";
        /// <summary>
        /// Document say not used in HK FPS, default "HK"
        /// </summary>
        public string CountryCode { get; set; } = "HK";
        /// <summary>
        /// Document say not used in HK FPS, default "NA"
        /// </summary>
        public string MerchantName { get; set; } = "NA";
        /// <summary>
        /// Document say not used in HK FPS, default "HK"
        /// </summary>
        public string MerchantCity { get; set; } = "HK";

        public enum InitiationType
        {
            /// <summary>
            /// Same QR Code is shown for more than one transaction
            /// </summary>
            Static,
            /// <summary>
            /// New QR Code is shown for each transaction
            /// </summary>
            Dynamic
        }
        public InitiationType InitiationMethod { get; set; } = InitiationType.Dynamic;

        /// <summary>
        /// Currency Code, visit <see cref="https://en.wikipedia.org/wiki/ISO_4217">ISO 4217</see> for details
        /// </summary>
        public string Currency { get; set; } = "344";
        /// <summary>
        /// Price, if assigned zero or below zero, means need input by payer himself 
        /// </summary>
        public decimal TransactionAmount { get; set; } = 0M;
        /// <summary>
        /// True to allow edit the amount by payer
        /// </summary>
        public bool TransactionAmountEditable { get; set; } = false;
        /// <summary>
        /// Document marked this is a optional field, but HSBC support say this MUST provide a unique value like invoice number as a reference
        /// </summary>
        public string ReferenceLabel { get; set; }

        public override string ToString()
        {
            return Builder.GetCode(this);
        }
    }
}
