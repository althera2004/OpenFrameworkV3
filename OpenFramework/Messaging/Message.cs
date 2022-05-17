// --------------------------------
// <copyright file="Message.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Messaging
{
    using System;

    public class Message
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public int Priority { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public DateTime? SendDate { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public bool Portal { get; set; }
    }
}