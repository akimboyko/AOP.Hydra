using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using MyCompany.BusinessDomain.Entity;

namespace MyCompany.BusinessDomain.Model.People
{
    public class Person : BaseEntity<Person>
    {
        #region Fields
        public virtual string Signature { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime Birthdate { get; set; }
        public virtual string Email { get; set; }
        #endregion
    }
}
