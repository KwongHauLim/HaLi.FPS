using System.Text;

namespace HaLi.FPS
{
    /// <summary>
    /// For generate FPS QR code value
    /// </summary>
    public class Builder
    {
        private static string DataObject(string id, string value, int maxLength = 0)
        {
            if (maxLength > 0 && value.Length > maxLength)
                value = value.Substring(0, maxLength);

            id = id.PadLeft(2, '0');
            var len = value.Length.ToString().PadLeft(2, '0');
            return $"{id}{len}{value}";
        }

        private static string DataGroup(string id, params string[] objs)
        {
            return DataObject(id, string.Join("", objs));
        }

        /// <summary>
        /// Generate FPS QR code value, coding base on https://github.com/nessgor/fps-hk-qrcode/blob/master/js/emv-code.js
        /// </summary>
        public static string GetCode(FPS fps)
        {
            var sb = new StringBuilder();

            // EMV Interpreted Data
            // Each data object is made up of three individual fields
            // ID / Length/ Value combination

            // Fixed, "01" for FPS
            sb.Append(DataObject("00", "01"));
            // FPS QR Code type, default "12" to generate new code
            sb.Append(DataObject("01", fps.InitiationMethod == FPS.InitiationType.Dynamic ? "12" : "11"));

            // FPS Account information
            string operatorDomain = DataObject("00", "hk.com.hkicl");
            string fpsAccount = string.Empty;
            switch (fps.Identification)
            {
                case FPS.IdentifyMethod.FPS_ID:
                    fpsAccount = DataObject("02", fps.FpsID, 34);
                    break;
                case FPS.IdentifyMethod.MobileNumber:
                    fpsAccount = DataObject("01", fps.BankCode) + DataObject("03", fps.FpsMobile, 13);
                    break;
                case FPS.IdentifyMethod.EmailAddress:
                    fpsAccount = DataObject("01", fps.BankCode) + DataObject("04", fps.FpsEmail, 34);
                    break;
                default:
                    break;
            }
            string amountEditable = (fps.TransactionAmountEditable && fps.TransactionAmount.CompareTo(0M) > 0) ? DataObject("06", "1") : string.Empty;
            sb.Append(DataGroup("26", operatorDomain, fpsAccount, amountEditable));

            string categoryCode = DataObject("52", fps.MerchantCategoryCode, 4);
            string merchantCountry = DataObject("58", fps.CountryCode, 2);
            string merchantName = DataObject("59", fps.MerchantName, 25);
            string merchantCity = DataObject("60", fps.MerchantCity, 15);
            string transactionCurrency = DataObject("53", fps.Currency, 3);
            string transactionAmount = fps.TransactionAmount.CompareTo(0M) <= 0 ? string.Empty : DataObject("54", fps.TransactionAmount.ToString("0.00"));
            string transactionReference = DataObject("05", fps.ReferenceLabel, 25);
            string additionalData = DataObject("62", transactionReference);

            sb.Append(categoryCode);
            sb.Append(transactionCurrency);
            sb.Append(merchantCountry);
            sb.Append(merchantName);
            sb.Append(merchantCity);
            sb.Append(transactionAmount);
            sb.Append(additionalData);

            // CRC
            sb.Append("6304"); // CRC include these 4 chars
            sb.Append(new CRC().Calc16(sb.ToString()));

            return sb.ToString();
        }
    }
}
