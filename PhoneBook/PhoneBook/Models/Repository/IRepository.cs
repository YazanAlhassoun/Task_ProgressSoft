using PhoneBook.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Models.Repository
{
    public interface IRepository<TEntity>
    {
        IList<TEntity> List();
        TEntity Find(int id);
        SavingStatus Add(TEntity entity);  
        void Delete(int id);
    }
}
