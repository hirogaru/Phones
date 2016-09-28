using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Phones.DataAccess;
using Phones.Models;
using Phones.Controllers;
using Ninject;

namespace UnitTestProjectPhones
{
    [TestClass]
    public class UnitTests
    {
        static IKernel AppKernel;
        
        [TestInitialize]
        public void RegisterServices()
        {
            AppKernel = new StandardKernel();
            AppKernel.Bind<IRepository>().To<TestRepository>();
            AppKernel.Bind<ContactsController>().ToSelf().WithConstructorArgument("contactModel", new ContactsModel());
        }

        [TestMethod]
        public void AddContactTest()
        {
            ContactsController controller = AppKernel.Get<ContactsController>();

            Contacts contact = new Contacts() //создаём тестовый контакт
            {
                contactName = "Wilco",
                phoneNum = "55-66"
            };

            Assert.AreEqual(true, controller.AddContact(contact)); //если метод успешно завершён
        }

        [TestMethod]
        public void EditContactTest()
        {
            ContactsController controller = AppKernel.Get<ContactsController>();

            Contacts contact = new Contacts() //создаём тестовый контакт
            {
                ID_Contact = Guid.Parse("14444444-4444-4444-4444-444444444444"),
                contactName = "Wilco",
                phoneNum = "55-66"
            };

            Assert.AreEqual(true, controller.AddContact(contact)); //если метод успешно завершён
        }

        [TestMethod]
        public void DeleteContactTest()
        {
            ContactsController controller = AppKernel.Get<ContactsController>();

            Guid idContact = Guid.Parse("14444444-4444-4444-4444-444444444444"); //создаём идентификатор существующего в базе контакта

            Assert.AreEqual(true, controller.DeleteContact(idContact)); //если метод успешно завершён
        }

        [TestMethod]
        public void SearchTest()
        {
            var data = new List<Contacts>();
            for (var i = 28; i < 34; i++) //создаём тестовый список
            {
                data.Add(new Contacts { ID_Contact = Guid.NewGuid(), contactName = "Person" + i.ToString(), phoneNum = "00-00" });
            }
            var list = SearchEngine.Search("PeRson3", data.AsQueryable()); //проверяем поиск(должно быть нечуствительным к регистру)

            Assert.AreEqual(list.Count(), 4); //кол-во найденых итемов
            Assert.AreEqual(list.ToList()[0].contactName, "Person30"); //проверяем первый найденый итем
        }
    }
}
