// --------------------------------
// <copyright file="ColumnsFacturaGet.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Billing
{
    /// <summary>Implements category of facturation concepts</summary>
    public partial class Invoice
    {
        /// <summary>Index of columns from Fact_Category get stored procedures</summary>
        public static class ColumnsInvoiceGet
        {
            /// <summary>Index column of "Id" column</summary>
            public const int Id = 0;

            /// <summary>Index column of "Id" column</summary>
            public const int CompanyId = 1;

            /// <summary>Index column of "Number" column</summary>
            public const int Number = 2;

            /// <summary>Index column of "Subject" column</summary>
            public const int Subject = 3;

            /// <summary>Index column of "ItemDefinitionId" column</summary>
            public const int ItemDefinitionId = 4;

            /// <summary>Index column of "ItemId" column</summary>
            public const int ItemId = 5;

            /// <summary>Index column of "Date" column</summary>
            public const int Date = 6;

            /// <summary>Index column of "PayerName" column</summary>
            public const int PayerName = 7;

            /// <summary>Index column of "PayerAddress" column</summary>
            public const int PayerAddress = 8;

            /// <summary>Index column of "PayerPostalCode" column</summary>
            public const int PayerPostalCode = 9;

            /// <summary>Index column of "PayerCity" column</summary>
            public const int PayerCity = 10;

            /// <summary>Index column of "PayerProvince" column</summary>
            public const int PayerProvince = 11;

            /// <summary>Index column of "PayerCountry" column</summary>
            public const int PayerCountry = 12;

            /// <summary>Index column of "PayerCIF" column</summary>
            public const int PayerCIF = 13;

            /// <summary>Index column of "PayerIBAN" column</summary>
            public const int PayerIBAN = 14;

            /// <summary>Index column of "PayerPhone" column</summary>
            public const int PayerPhone = 15;

            /// <summary>Index column of "PayerEmail" column</summary>
            public const int PayerEmail = 16;

            /// <summary>Index column of "ChargerName" column</summary>
            public const int ChargerName = 17;

            /// <summary>Index column of "ChargerAddress" column</summary>
            public const int ChargerAddress = 18;

            /// <summary>Index column of "ChargerPostalCode" column</summary>
            public const int ChargerPostalCode = 19;

            /// <summary>Index column of "ChargerCity" column</summary>
            public const int ChargerCity = 20;

            /// <summary>Index column of "ChargerProvince" column</summary>
            public const int ChargerProvince = 21;

            /// <summary>Index column of "ChargerCountry" column</summary>
            public const int ChargerCountry = 22;

            /// <summary>Index column of "ChargerCIF" column</summary>
            public const int ChargerCIF = 23;

            /// <summary>Index column of "ChargerIBAN" column</summary>
            public const int ChargerIBAN = 24;

            /// <summary>Index column of "ChargerPhone" column</summary>
            public const int ChargerPhone = 25;

            /// <summary>Index column of "ChargerEmail" column</summary>
            public const int ChargerEmail = 26;

            /// <summary>Index column of "Status" column</summary>
            public const int Status = 27;

            /// <summary>Index column of "BaseAmount" column</summary>
            public const int BaseAmount = 28;

            /// <summary>Index column of "TotalIVA" column</summary>
            public const int TotalIVA = 29;

            /// <summary>Index column of "UnbillingAmount" column</summary>
            public const int UnbillingAmount = 30;

            /// <summary>Index column of "Total" column</summary>
            public const int Total = 31;

            /// <summary>Index column of "CreatedBy" column</summary>
            public const int CreatedBy = 32;

            /// <summary>Index column of "CreatedByName" column</summary>
            public const int CreatedByName = 33;

            /// <summary>Index column of "CreatedByLastName" column</summary>
            public const int CreatedByLastName = 34;

            /// <summary>Index column of "CreatedByLastName2" column</summary>
            public const int CreatedByLastName2 = 35;

            /// <summary>Index column of "CreatedOn" column</summary>
            public const int CreatedOn = 36;

            /// <summary>Index column of "CreatedOn" column</summary>
            public const int ModifiedBy = 37;

            /// <summary>Index column of "ModifiedByName" column</summary>
            public const int ModifiedByName = 38;

            /// <summary>Index column of "ModifiedByLastName" column</summary>
            public const int ModifiedByLastName = 39;

            /// <summary>Index column of "ModifiedByLastName2" column</summary>
            public const int ModifiedByLastName2 = 40;

            /// <summary>Index column of "ModifiedOn" column</summary>
            public const int ModifiedOn = 41;

            /// <summary>Index column of "Active" column</summary>
            public const int Active = 42;

            /// <summary>Index column of "DateConfirmed" column</summary>
            public const int DateConfirmed = 43;

            /// <summary>Index column of "DateSend" column</summary>
            public const int DateSend = 44;

            /// <summary>Index column of "DatePay" column</summary>
            public const int DatePay = 45;

            /// <summary>Index column of "DateRefuse" column</summary>
            public const int DateRefuse = 46;

            /// <summary>Index column of "RefuseReason" column</summary>
            public const int RefuseReason = 47;

            /// <summary>Index column of "DateVto" column</summary>
            public const int DateVto = 48;

            /// <summary>Index column of "PaymentMethod" column</summary>
            public const int PaymentMethod = 49;

            /// <summary>Index column of "ChargerSwift" column</summary>
            public const int ChargerSwift = 50;

            /// <summary>Index column of "CECO" column</summary>
            public const int CECO = 51;

            /// <summary>Index column of "Notes" column</summary>
            public const int Notes = 52;

            /// <summary>Index column of "SEPA" column</summary>
            public const int SEPA = 53;

            /// <summary>Index column of "Type" column</summary>
            public const int Type = 54;

            /// <summary>Index column of "Devolucion" column</summary>
            public const int Devolucion = 55;

            /// <summary>Index column of "BlockReason" column</summary>
            public const int BlockReason = 56;
        }
    }
}