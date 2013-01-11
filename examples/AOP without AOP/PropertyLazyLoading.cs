using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using MyCompany.BusinessDomain.Entity;
using MyCompany.BusinessDomain.Model.Common;
using MyCompany.BusinessDomain.Model.Negotiation;
using MyCompany.BusinessDomain.Model.Salary;

namespace MyCompany.BusinessDomain.Model.People
{
    public class Person : BaseEntity<Person>
    {
        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public Person()
        {
            ArbeidsForholds = new List<ArbeidsForhold>();
            Kompetanses = new List<Kompetanse>();
            Representantes = new List<Representant>();
        }
        // ReSharper restore DoNotCallOverridableMethodsInConstructor

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        #region Fields
        public virtual string Signatur { get; set; }
        public virtual string FodselsNr { get; set; }
        public virtual string Fornavn { get; set; }
        public virtual string Etternavn { get; set; }
        public virtual DateTime FodtDato { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefon { get; set; }
        public virtual string Mobil { get; set; }
        public virtual string AdresseLinje1 { get; set; }
        public virtual string AdresseLinje2 { get; set; }
        public virtual string AdresseLinje3 { get; set; }
        public virtual string AdressePostNr { get; set; }
        public virtual string AdressePoststed { get; set; }
        public virtual string AdresseLand { get; set; }
        #endregion

        #region References
        public virtual Kodeverk Kjonn { get; set; }
        #endregion

        #region Collections
        public virtual IList<ArbeidsForhold> ArbeidsForholds { get; set; }
        public virtual IList<Kompetanse> Kompetanses { get; set; }
        public virtual IList<Representant> Representantes { get; set; }
        #endregion
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        #region Code Contract
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            // ReSharper disable InvocationIsSkipped
            Contract.Invariant(ArbeidsForholds != null);
            Contract.Invariant(Kompetanses != null);
            Contract.Invariant(Representantes != null);
            // ReSharper restore InvocationIsSkipped
        }
        #endregion
    }
}
