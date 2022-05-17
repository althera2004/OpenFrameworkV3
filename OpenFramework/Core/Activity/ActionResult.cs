namespace OpenFrameworkV3.Core.Activity
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Net;
    using System.Xml.Serialization;
    using System.Net.Http;

    /// <summary>Implements a class that represents the result of an operation</summary>
    [Serializable]
    public class ActionResult
    {
        /// <summary>No action message</summary>
        public const string NoActionMessage = "No action";

        /// <summary>Initializes a new instance of the ActionResult class.</summary>
        public ActionResult()
        {
            this.Success = false;
            this.MessageError = "No action";
        }

        /// <summary>Gets a default action result for no action</summary>
        public static ActionResult NoAction
        {
            get
            {
                return new ActionResult { Success = false, MessageError = NoActionMessage };
            }
        }

        /// <summary>Gets a default action with success result</summary>
        public static ActionResult NoResult
        {
            get
            {
                return new ActionResult { Success = true, ReturnValue = string.Empty };
            }
        }

        /// <summary>Gets or sets a value indicating whether if the action has is success or fail</summary>
        [XmlElement(Type = typeof(bool), ElementName = "Success")]
        public bool Success { get; set; }

        /// <summary>Gets or sets a value indicating whether the message of result</summary>
        [XmlElement(Type = typeof(string), ElementName = "MessageError")]
        public string MessageError { get; set; }

        /// <summary>Gets or sets a value indicating the return value of action</summary>
        [XmlElement(Type = typeof(object), ElementName = "ReturnValue")]
        public object ReturnValue { get; set; }

        /// <summary>Gets a JSON representation of ActionResult object</summary>
        public string Json
        {
            get
            {
                var returnValue = Constant.JavaScriptNull;

                if (this.ReturnValue != null)
                {
                    switch (this.ReturnValue.GetType().Name.ToUpperInvariant())
                    {
                        case "STRING":
                            returnValue = string.Format(CultureInfo.InvariantCulture, @"""{0}""", Tools.Json.JsonCompliant(this.ReturnValue.ToString()));
                            break;
                        case "LONG":
                        case "INT":
                        case "DECIMAL":
                            returnValue = string.Format(CultureInfo.InvariantCulture, "{0}", returnValue);
                            break;
                        default:
                            returnValue = "object";
                            break;
                    }
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Success"":{0},  ""ReturnValue"":{1}, ""MessageError"":""{2}""}}",
                    this.Success ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    returnValue,
                    Tools.Json.JsonCompliant(this.MessageError));
            }
        }

        /// <summary>Gets a API response in JSON format</summary>
        public HttpResponseMessage APIResponse
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(this.Json, Encoding.UTF8, "application/json")
                };
            }
        }

        /// <summary>Sets the success of action to true</summary>
        public void SetSuccess()
        {
            this.SetSuccess(string.Empty);
        }

        /// <summary>Sets the success of action to true with a message</summary>
        /// <param name="message">Text of message</param>
        public void SetSuccess(string message)
        {
            this.Success = true;
            this.MessageError = string.Empty;
            this.ReturnValue = message;
        }

        /// <summary>Sets the success of action to true with a message</summary>
        /// <param name="newItemId">Identifier of new item added in database</param>
        public void SetSuccess(int newItemId)
        {
            this.Success = true;
            this.MessageError = string.Empty;
            this.ReturnValue = newItemId;
        }

        /// <summary>Sets the success of action to true with a message</summary>
        /// <param name="value">Value of return object</param>
        public void SetSuccess(object value)
        {
            this.Success = true;
            this.MessageError = string.Empty;
            this.ReturnValue = value;
        }

        /// <summary>Sets the success of action to true with a message</summary>
        /// <param name="newItemId">Identifier of new item added in database</param>
        public void SetSuccess(long newItemId)
        {
            this.Success = true;
            this.MessageError = string.Empty;
            this.ReturnValue = newItemId;
        }

        /// <summary>Sets the success of action to false with a message</summary>
        /// <param name="message">Text of message</param>
        public void SetFail(string message)
        {
            this.Success = false;
            this.MessageError = message;
        }

        /// <summary>Sets the success of action to false with a message</summary>
        /// <param name="ex">Exception that causes fail</param>
        public void SetFail(Exception ex)
        {
            this.Success = false;
            if (ex != null)
            {
                this.MessageError = ex.Message;
            }
            else
            {
                this.MessageError = string.Empty;
            }
        }
    }
}