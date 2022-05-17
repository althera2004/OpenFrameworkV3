// --------------------------------
// <copyright file="Payment.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Globalization;
using System.Net;
using GlobalPayments.Api;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.PaymentMethods;
using OpenFrameworkV3.CommonData;
using OpenFrameworkV3.Core.Activity;

namespace OpenFrameworkV3.Billing
{
    public class Payment
    {
        public static ActionResult Validate(CreditCardData card, CreditCard creditCard)
        {
            var res = ActionResult.NoAction;

            try
            {
                ServicesContainer.ConfigureService(new GpEcomConfig
                {
                    MerchantId = creditCard.MerchantId,
                    AccountId = creditCard.AccountId,
                    SharedSecret = creditCard.SharedSecret,
                    ServiceUrl = creditCard.ServiceUrl, // "https://remote.sandbox.addonpayments.com/remote",
                    HostedPaymentConfig = new HostedPaymentConfig
                    {
                        Version = "2"
                    }
                });

                // check that a card is valid and active without charging an amount
                Transaction response = card.Verify()
                   .Execute();

                // get the response details to update the DB
                var result = response.ResponseCode; // 00 == Success
                var message = response.ResponseMessage; // [ test system ] AUTHORISED
                var schemeReferenceData = response.SchemeId; // MMC0F00YE4000000715
                                                             // TODO: save the card to Card Stroage

                res.SetSuccess();
            }

            catch(ApiException exce)
            {
                res.SetFail(exce);
            }

            return res;
        }

        public static ActionResult Go(string cardNumber, int expMonth, int expYear, string cnv, string name, decimal amount, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;

            var creditCard = CreditCard.GetMain(companyId, instanceName);


            ServicesContainer.ConfigureService(new GpEcomConfig
            {
                MerchantId = creditCard.MerchantId,
                AccountId = creditCard.AccountId,
                SharedSecret = creditCard.SharedSecret,
                ServiceUrl = creditCard.ServiceUrl, // "https://remote.sandbox.addonpayments.com/remote",
                //HostedPaymentConfig = new HostedPaymentConfig
                //{
                //    Version = "2"
                //}
            });

            var card = new CreditCardData
            {
                Number = cardNumber,
                ExpMonth = expMonth,
                ExpYear = expYear,
                Cvn = cnv,
                CardHolderName = name
            };

            //res = Validate(card, creditCard);

            try
            {
                // process an auto-capture authorization
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Transaction response = card.Charge(amount)
                   .WithCurrency("EUR")                   
                   .Execute();

                var result = response.ResponseCode; // 00 == Success
                var message = response.ResponseMessage; // [ test system ] AUTHORISED

                // get the response details to save to the DB for future requests
                var orderId = response.OrderId; // ezJDQjhENTZBLTdCNzNDQw
                var authCode = response.AuthorizationCode; // 12345
                var paymentsReference = response.TransactionId; // pasref 14622680939731425
                var schemeReferenceData = response.SchemeId; // MMC0F00YE4000000715

                var returnValue = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""respodeCode"":""{0}"", ""responseMessage"":""{1}"",""orderId"":""{2}"", ""authCode"":""{3}"", ""paymentsReference"":""{4}"", ""schemeReferenceData"":""{5}"",""timeStamp"":""{6}"",""Data"":""{7}""}}",
                    response.ResponseCode,
                    Tools.Json.JsonCompliant(response.ResponseMessage),
                    response.OrderId,
                    response.AuthorizationCode,
                    response.TransactionId,
                    response.SchemeId,
                    response.Timestamp,
                    creditCard.MerchantId+","+ creditCard.AccountId+","+ creditCard.SharedSecret+","+creditCard.SharedSecret+"*");

                res.SetSuccess(returnValue);
            }

            catch(ApiException exce)
            {
                res.SetFail(exce.Message +"|" + creditCard.MerchantId + "," + creditCard.AccountId + "," + creditCard.SharedSecret);
            }

            return res;
        }
    }
}