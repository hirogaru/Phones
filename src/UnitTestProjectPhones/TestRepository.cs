using Phones.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProjectPhones
{
    class TestRepository : IRepository
    {
        public IQueryable<Contacts> Contacts
        {
            get
            {
                return (new List<Contacts> 
                { 
                    new Contacts { contactName ="Name", phoneNum = "00-00", ID_Contact = Guid.Parse("14444444-4444-4444-4444-444444444444"), IsDeleted = false } 
                }).AsQueryable();
            }

        }

        public bool CreateContact(Contacts instance)
        {
            return true;
        }

        public bool UpdateContact(Contacts instance)
        {
            return true;
        }

        public bool RemoveContact(Guid idContact)
        {
            return true;
        }
    }
}
