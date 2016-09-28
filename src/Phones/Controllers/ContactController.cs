using System;
using System.Linq;
using System.Windows;
using Phones.DataAccess;
using Phones.Models;
using Phones.UI;
using Ninject;

namespace Phones.Controllers
{
    public interface IContactController
    {
        void ShowContactsView(Window owner);
        void ShowSearchResult(string searchString);
        void AddContact(Window owner, Contacts contact = null);
        bool AddContact(Contacts contact);
        bool DeleteContact(Guid contact);
    }

    /// <summary>
    /// Обрабатывает запросы пользователя, выполняет операции с моделью.
    /// </summary>
    public class ContactsController : IContactController
    {
        [Inject]
        public IRepository Repository
        {
            get; set;
        }

        private readonly IContactsModel _model;

        public ContactsController(IContactsModel contactModel)
        {
            if (contactModel == null) throw new ArgumentNullException("contactModel");
            _model = contactModel;
        }

        public void ShowContactsView(Window owner) //показать вью контактов
        {
            _model.Contacts = Repository.Contacts;

            ContactsWindow contactsView = new ContactsWindow(this, _model);
            contactsView.Owner = owner;
            contactsView.Show();
        }

        public void ShowSearchResult(string searchString = null) //поиск контакта;
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                _model.Contacts = SearchEngine.Search(searchString, Repository.Contacts);
            }
            else
            {
                _model.Contacts = Repository.Contacts;
            }
            _model.UpdateContacts();
        }

        public void AddContact(Window owner, Contacts newModel = null) //показать вью добавления/изменения контакта
        {
            if (newModel == null)
            {
                newModel = new Contacts()
                    {
                        contactName = "Name",
                        phoneNum = "00-00"
                    };
            }
            AddWindow addView = new AddWindow(this, newModel);
            addView.Owner = owner;
            addView.Show();
        }

        public bool AddContact(Contacts contact) //добавление/изменение контакта;
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(contact.contactName))
            {
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(contact.phoneNum))
            {
                isValid = false;
            }

            var anyExistContact = Repository.Contacts.Any(p => p.contactName == contact.contactName); 
            
            //проверяем по совпадении имени, но можно заменить на другие проверки
            if (anyExistContact && (contact.ID_Contact == Guid.Empty))
            {
                isValid = false;
            }

            if (isValid)
            {
                if (contact.ID_Contact == Guid.Empty) //если контакт новый
                {
                    contact.ID_Contact = Guid.NewGuid();
                    isValid = Repository.CreateContact(contact); 
                }
                else Repository.UpdateContact(contact); //если контакт существующий

                _model.UpdateContacts();
            }

            return isValid; //валидация не прошла
        }

        public bool DeleteContact(Guid idContact) //удаление контакта
        {
            Contacts deleteItem = Repository.Contacts //ищем контакт в базе
                 .FirstOrDefault(g => g.ID_Contact == idContact);

            if (deleteItem == null) return false; //если контакт не найден, то завершаем метод

            Repository.RemoveContact(idContact); //помечаем контакт удалённым

            _model.UpdateContacts();
            return true;
        }

    }
}
