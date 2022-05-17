// --------------------------------
// <copyright file="Notification.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Messaging
{
    public class Notification
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public bool Readed { get; set; }
    }
}
