// --------------------------------
// <copyright file="UniqueSessionData.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Security
{
    using System;
    using System.Collections.Generic;

    /// <summary>Control of singleton session in application</summary>
    public struct UniqueSessionData : IEquatable<UniqueSessionData>
    {
        /// <summary>Gets or sets session token</summary>
        public Guid Token { get; set; }

        /// <summary>Gets or sets application user identifier</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets IP from session</summary>
        public string IP { get; set; }

        /// <summary>Gets or sets date and time of last connection</summary>
        public DateTime LastConnection { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UniqueSessionData && Equals((UniqueSessionData)obj);
        }

        public bool Equals(UniqueSessionData other)
        {
            return Token.Equals(other.Token) &&
                   UserId == other.UserId &&
                   IP == other.IP &&
                   LastConnection == other.LastConnection;
        }

        public override int GetHashCode()
        {
            var hashCode = 1410804994;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(Token);
            hashCode = hashCode * -1521134295 + UserId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IP);
            hashCode = hashCode * -1521134295 + LastConnection.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}