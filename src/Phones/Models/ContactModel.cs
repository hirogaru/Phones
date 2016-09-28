using System;
using System.Collections.Generic;
using Phones.DataAccess;

namespace Phones.Models
{
    public interface IContactsModel
    {
        IEnumerable<Contacts> Contacts { get; set; }
        event EventHandler<EventArgs> ContactUpdated;
        void UpdateContacts();
    }

    /// <summary>
    /// Модель для использования представлением. Содержит данные и событие для уведомления классов об обновлении.
    /// </summary>
    public class ContactsModel : IContactsModel
    {
        public IEnumerable<Contacts> Contacts { get; set; }

        public event EventHandler<EventArgs> ContactUpdated = delegate { };

        public void UpdateContacts()
        {
            ContactUpdated(this, new EventArgs());
        }
    }
}

