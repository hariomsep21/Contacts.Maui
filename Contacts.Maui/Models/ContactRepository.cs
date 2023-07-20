using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Contacts.Maui.Models
{
    public static class ContactRepository
    {
        public static List<Contact> _contacts = new List<Contact>()
        {
        new Contact { ContactId = 1, Name = "Hariom",  Email = "Hariom@gmail.com",  Phone = "123456789", Notification = "1" },
        new Contact { ContactId = 2, Name = "Akash",   Email = "Akash@gmail.com",   Phone = "234567890", Notification = "1" },
        new Contact { ContactId = 3, Name = "Harry",   Email = "Harry11@gmail.com", Phone = "3456789012", Notification = "1" },
        new Contact { ContactId = 4, Name = "Happy om",Email = "Happy@gmail.com",   Phone = "4567890123", Notification = "1" },
        new Contact { ContactId = 5, Name = "John",    Email = "john.doe@gmail.com",Phone = "789012345", Notification = "2" },
        new Contact { ContactId = 6, Name = "Emma",    Email = "emma.smith@gmail.com",Phone = "890123456", Notification = "3" }
        };
        public static List<Contact> GetContacts() => _contacts;

        public static Contact GetContactById(int contactId)
        {

            var contact = _contacts.FirstOrDefault(x => x.ContactId == contactId);
            if (contact != null)
            {
                return new Contact
                {
                    ContactId = contactId,
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Notification = contact.Notification,
                };
            }
            return null;
        }
        public static void UpdateContact(int contactId, Contact contact)
        {

            if (contactId != contact.ContactId) return;


            var contactToUpdate = _contacts.FirstOrDefault(x => x.ContactId == contactId);
            if (contactToUpdate != null)
            {
                contactToUpdate.Name = contact.Name;
                contactToUpdate.Email = contact.Email;
                contactToUpdate.Phone = contact.Phone;
                contactToUpdate.Notification = contact.Notification;
            }
            Shell.Current.GoToAsync("..");
        }

        public static void AddContact(Contact contact)
        {
            var maxId = _contacts.Max(x => x.ContactId);
            contact.ContactId = maxId + 1;
            _contacts.Add(contact);
        }

        public static void DeleteContact(int contactId)
        {
            var contact = _contacts.FirstOrDefault(x => x.ContactId == contactId);
            if (contact != null)
            {
                _contacts.Remove(contact);
            }
        }
        public static List<Contact> SearchContacts(string filterText)
        {
            var contacts = _contacts.Where(x => !string.IsNullOrWhiteSpace(x.Name) && x.Name.StartsWith(filterText, StringComparison.OrdinalIgnoreCase))?.ToList();

            if (contacts == null || contacts.Count <= 0)
            {
                contacts = _contacts.Where(x => !string.IsNullOrWhiteSpace(x.Email) && x.Email.StartsWith(filterText, StringComparison.OrdinalIgnoreCase))?.ToList();
            }
            else
            {
                return contacts;
            }

            if (contacts == null || contacts.Count <= 0)
            {
                contacts = _contacts.Where(x => !string.IsNullOrWhiteSpace(x.Notification) && x.Notification.StartsWith(filterText, StringComparison.OrdinalIgnoreCase))?.ToList();
            }
            else
            {
                return contacts;
            }

            if (contacts == null || contacts.Count <= 0)
            {
                contacts = _contacts.Where(x => !string.IsNullOrWhiteSpace(x.Phone) && x.Phone.StartsWith(filterText, StringComparison.OrdinalIgnoreCase))?.ToList();
            }
            else
            {
                return contacts;
            }
            return contacts;
        }

        public static int GetCallDuration(string phoneNumber)
        {
            var contact = _contacts.FirstOrDefault(x => x.Phone == phoneNumber);
            if (contact != null)
            {
                // Simulate call duration calculation (replace this with actual call duration retrieval)
                int callDuration = new Random().Next(1, 18); // Generating a random call duration between 10 to 180 seconds

                // Update the CallDuration property of the contact
                contact.CallDuration = callDuration;

                return callDuration;
            }

            return 0;
        }



    }
}
