using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents the logic for the Contact Us messages.
    /// </summary>
    public class ContactsLogic : BaseLogic
    {

        /// <summary>
        /// Inserts a new contact us message.
        /// </summary>
        /// <param name="contact">The contact us message and details to insert.</param>
        public void InsertContactMessage(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException();

            contact.DateTime = DateTime.Now;
            contact.IsRead = false;

            DB.Contacts.Add(contact);
            DB.SaveChanges();
        }

        /// <summary>
        /// Gets all the contact us messages.
        /// </summary>
        /// <returns>List of all the contact us messages.</returns>
        public List<Contact> GetAllContacts()
        {
            return DB.Contacts.ToList();
        }

        /// <summary>
        /// Updates unread contact message.
        /// </summary>
        /// <param name="contactID">The ID of the contact message to update.</param>
        public void UpdateUnreadContact(int contactID)
        {
            Contact contact = DB.Contacts.Find(contactID);
            if (contact != null)
            {
                contact.IsRead = true;

                DB.Entry(contact).State = EntityState.Modified;
                DB.SaveChanges();
            }
        }

    }
}
