using System;
using System.Linq;

namespace Phones.DataAccess
{
    public interface IRepository
    {
        IQueryable<Contacts> Contacts { get; }

        bool CreateContact(Contacts instance);

        bool UpdateContact(Contacts instance);

        bool RemoveContact(Guid idEmployer);
    }

    /// <summary>
    /// Реализация доступа ко всем таблицам, создание, удаление и изменение
    /// </summary>
    public class SqlRepository : IRepository
    {
        private readonly PhonesDBEntities _context = new PhonesDBEntities();
        public PhonesDBEntities Db
        {
            get { return _context; }
        }

        public IQueryable<Contacts> Contacts
        {
            get
            {
                return _context.Contacts.AsQueryable<Contacts>().Where(x => x.IsDeleted == false);
            }
        }

        public bool CreateContact(Contacts instance)
        {
            if (instance.ID_Contact != Guid.Empty)
            {
                _context.Contacts.Add(instance);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateContact(Contacts instance)
        {
            Contacts cache = _context.Contacts.Where(p => p.ID_Contact == instance.ID_Contact).FirstOrDefault();
            if (cache != null)
            {
                cache.contactName = instance.contactName;
                cache.phoneNum = instance.phoneNum;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveContact(Guid idContact)
        {
            Contacts instance = _context.Contacts.Where(p => p.ID_Contact == idContact).FirstOrDefault();
            if (instance != null)
            {
                instance.IsDeleted = true;
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
