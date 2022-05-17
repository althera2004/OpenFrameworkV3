// --------------------------------
// <copyright file="Invoice.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.CommonData
{
    using System;

    public class IBAN
    {
        public string Value { get; set; }
        public string BankCode { get; private set; }

        public bool Validate
        {
            get
            {
                var res = isIBAN(this.Value);

                if (res)
                {
                    if(this.Value.IndexOf('-') != -1)
                    {
                        this.BankCode = this.Value.Split('-')[1];
                    }
                    else
                    {
                        this.BankCode = this.Value.Substring(4, 4);
                    }
                }

                return res;
            }
        }


        public static bool isIBAN(string iban)
        {
            string mysIBAN = iban.Replace(" ", "");

            if (mysIBAN.Length > 34 || mysIBAN.Length < 5)
            {
                return false;
            }
            else
            {
                string LaenderCode = mysIBAN.Substring(0, 2).ToUpper();
                string Pruefsumme = mysIBAN.Substring(2, 2).ToUpper();
                string BLZ_Konto = mysIBAN.Substring(4).ToUpper();
                if (!IsNumeric(Pruefsumme))
                {
                    return false;
                }

                if (!ISLaendercode(LaenderCode))
                {
                    return false;
                }

                string Umstellung = BLZ_Konto + LaenderCode + "00";
                string Modulus = IBANCleaner(Umstellung);

                if (98 - Modulo(Modulus, 97) != int.Parse(Pruefsumme))
                {
                    return false;  //Prüfsumme ist fehlerhaft 
                }
            }

            return true;
        }

        private static bool ISLaendercode(string code)
        {
            if (code.Length != 2)
                return false;
            else
            {
                code = code.ToUpper();
                string[] Laendercodes = { "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI",
                "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV",
                "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI",
                "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV",
                "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI",
                "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV",
                "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI",
                "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV",
                "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI",
                "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV",
                "EW", "EX", "EY", "EZ", "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI",
                "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV",
                "FW", "FX", "FY", "FZ", "GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI",
                "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV",
                "GW", "GX", "GY", "GZ", "HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI",
                "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV",
                "HW", "HX", "HY", "HZ", "IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II",
                "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV",
                "IW", "IX", "IY", "IZ", "JA", "JB", "JC", "JD", "JE", "JF", "JG", "JH", "JI",
                "JJ", "JK", "JL", "JM", "JN", "JO", "JP", "JQ", "JR", "JS", "JT", "JU", "JV",
                "JW", "JX", "JY", "JZ", "KA", "KB", "KC", "KD", "KE", "KF", "KG", "KH", "KI",
                "KJ", "KK", "KL", "KM", "KN", "KO", "KP", "KQ", "KR", "KS", "KT", "KU", "KV",
                "KW", "KX", "KY", "KZ", "LA", "LB", "LC", "LD", "LE", "LF", "LG", "LH", "LI",
                "LJ", "LK", "LL", "LM", "LN", "LO", "LP", "LQ", "LR", "LS", "LT", "LU", "LV",
                "LW", "LX", "LY", "LZ", "MA", "MB", "MC", "MD", "ME", "MF", "MG", "MH", "MI",
                "MJ", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV",
                "MW", "MX", "MY", "MZ", "NA", "NB", "NC", "ND", "NE", "NF", "NG", "NH", "NI",
                "NJ", "NK", "NL", "NM", "NN", "NO", "NP", "NQ", "NR", "NS", "NT", "NU", "NV",
                "NW", "NX", "NY", "NZ", "OA", "OB", "OC", "OD", "OE", "OF", "OG", "OH", "OI",
                "OJ", "OK", "OL", "OM", "ON", "OO", "OP", "OQ", "OR", "OS", "OT", "OU", "OV",
                "OW", "OX", "OY", "OZ", "PA", "PB", "PC", "PD", "PE", "PF", "PG", "PH", "PI",
                "PJ", "PK", "PL", "PM", "PN", "PO", "PP", "PQ", "PR", "PS", "PT", "PU", "PV",
                "PW", "PX", "PY", "PZ", "QA", "QB", "QC", "QD", "QE", "QF", "QG", "QH", "QI",
                "QJ", "QK", "QL", "QM", "QN", "QO", "QP", "QQ", "QR", "QS", "QT", "QU", "QV",
                "QW", "QX", "QY", "QZ", "RA", "RB", "RC", "RD", "RE", "RF", "RG", "RH", "RI",
                "RJ", "RK", "RL", "RM", "RN", "RO", "RP", "RQ", "RR", "RS", "RT", "RU", "RV",
                "RW", "RX", "RY", "RZ", "SA", "SB", "SC", "SD", "SE", "SF", "SG", "SH", "SI",
                "SJ", "SK", "SL", "SM", "SN", "SO", "SP", "SQ", "SR", "SS", "ST", "SU", "SV",
                "SW", "SX", "SY", "SZ", "TA", "TB", "TC", "TD", "TE", "TF", "TG", "TH", "TI",
                "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TQ", "TR", "TS", "TT", "TU", "TV",
                "TW", "TX", "TY", "TZ", "UA", "UB", "UC", "UD", "UE", "UF", "UG", "UH", "UI",
                "UJ", "UK", "UL", "UM", "UN", "UO", "UP", "UQ", "UR", "US", "UT", "UU", "UV",
                "UW", "UX", "UY", "UZ", "VA", "VB", "VC", "VD", "VE", "VF", "VG", "VH", "VI",
                "VJ", "VK", "VL", "VM", "VN", "VO", "VP", "VQ", "VR", "VS", "VT", "VU", "VV",
                "VW", "VX", "VY", "VZ", "WA", "WB", "WC", "WD", "WE", "WF", "WG", "WH", "WI",
                "WJ", "WK", "WL", "WM", "WN", "WO", "WP", "WQ", "WR", "WS", "WT", "WU", "WV",
                "WW", "WX", "WY", "WZ", "XA", "XB", "XC", "XD", "XE", "XF", "XG", "XH", "XI",
                "XJ", "XK", "XL", "XM", "XN", "XO", "XP", "XQ", "XR", "XS", "XT", "XU", "XV",
                "XW", "XX", "XY", "XZ", "YA", "YB", "YC", "YD", "YE", "YF", "YG", "YH", "YI",
                "YJ", "YK", "YL", "YM", "YN", "YO", "YP", "YQ", "YR", "YS", "YT", "YU", "YV",
                "YW", "YX", "YY", "YZ", "ZA", "ZB", "ZC", "ZD", "ZE", "ZF", "ZG", "ZH", "ZI",
                "ZJ", "ZK", "ZL", "ZM", "ZN", "ZO", "ZP", "ZQ", "ZR", "ZS", "ZT", "ZU", "ZV",
                "ZW", "ZX", "ZY", "ZZ" };

                if (Array.IndexOf(Laendercodes, code) == -1)
                    return false;
                else
                    return true;
            }
        }

        private static string IBANCleaner(string sIBAN)
        {
            for (int x = 65; x < 90; x++)
            {
                int replacewith = x - 64 + 9;
                string replace = ((char)x).ToString();
                sIBAN = sIBAN.Replace(replace, replacewith.ToString());
            }
            return sIBAN;
        }

        private static int Modulo(string sModulus, int iTeiler)
        {
            int iStart, iEnde, iErgebniss, iRestTmp, iBuffer;
            string iRest = string.Empty, sErg = string.Empty;

            sModulus= sModulus.Replace("-", string.Empty);

            iStart = 0;
            iEnde = 0;
            while (iEnde <= sModulus.Length - 1)
            {
                iBuffer = int.Parse(iRest + sModulus.Substring(iStart, iEnde - iStart + 1));

                if (iBuffer >= iTeiler)
                {
                    iErgebniss = iBuffer / iTeiler;
                    iRestTmp = iBuffer - iErgebniss * iTeiler;
                    iRest = iRestTmp.ToString();

                    sErg = sErg + iErgebniss.ToString();

                    iStart = iEnde + 1;
                    iEnde = iStart;
                }
                else
                {
                    if (sErg != "")
                    {
                        sErg = sErg + "0";
                    }

                    iEnde = iEnde + 1;
                }
            }

            if (iStart <= sModulus.Length)
            {
                iRest = iRest + sModulus.Substring(iStart);
            }

            return int.Parse(iRest);
        }

        private static bool IsNumeric(string value)
        {
            try
            {
                int.Parse(value);
                return (true);
            }
            catch
            {
                return (false);
            }
        }
    }
}