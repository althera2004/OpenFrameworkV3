// --------------------------------
// <copyright file="ScopeGroup.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    public class ScopeGroup
    {
        private List<Scope> scope;

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public string InstanceName { get; set; }

        public void ObtainScope()
        {
            this.scope = Scope.ByGroup(this.Id, this.InstanceName).ToList();
        }

        public ReadOnlyCollection<Scope> ScopeView
        {
            get
            {
                if(this.scope == null)
                {
                    this.scope = new List<Scope>();
                }

                return new ReadOnlyCollection<Scope>(this.scope);
            }
        }

        public void AddScope(Scope newScope)
        {
            if (this.scope == null)
            {
                this.scope = new List<Scope>();
            }

            this.scope.Add(newScope);
        }

        public ScopeGroup Empty
        {
            get
            {
                return new ScopeGroup
                {
                    Id = Constant.DefaultId,
                    Name = string.Empty,
                    Description = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Name"":""{1}"",""Active"":{2}}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    ConstantValue.Value(this.Active));
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Name"":""{1}"",""Description"":""{2}"",""CreatedBy"":{3},""CreatedOn"":""{4:dd/MM/yyyy}"",""ModifiedBy"":{5},""ModifiedOn"":""{6:dd/MM/yyyy}"",""Active"":{7}}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    Tools.Json.JsonCompliant(this.Description),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }
    }
}