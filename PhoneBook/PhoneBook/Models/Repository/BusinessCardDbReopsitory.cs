using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using PhoneBook.Common;

namespace PhoneBook.Models.Repository
{
    public class BusinessCardDbReopsitory : IRepository<BusinessCard>
    {
        private readonly PhoneBookDBEntities db;

        public BusinessCardDbReopsitory(PhoneBookDBEntities _db)
        {
            this.db = _db;
        }

        public SavingStatus Add(BusinessCard entity)
        {
            bool IsExists = db.BusinessCards.Where(m => m.Email.ToLower() == entity.Email.ToLower() && m.Phone == entity.Phone).Any();
            if (IsExists)
            {
                return SavingStatus.ExistsEmail;
                
            }
            db.BusinessCards.Add(entity);
            save();
            return SavingStatus.Saved;
        }

        public void Delete(int id)
        {
            var card = Find(id);
            db.BusinessCards.Remove(card);
            save();
        }

        public BusinessCard Find(int id)
        {
            return db.BusinessCards.SingleOrDefault(m => m.Id == id);
        }

        public IList<BusinessCard> List()
        {
            return db.BusinessCards.ToList();
        }

        public void save()
        {
            db.SaveChanges();
        }

       
    }
}