using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using B1625MVC.Model.Entities;
using B1625MVC.Model.Abstract;
using B1625MVC.Model.Initializers;
using System;

namespace B1625MVC.Model.Repositories
{
    public class B1625DbRepository : IB1625Repository
    {
        private B1625DbContext _dbContext;

        public B1625DbRepository()
        {
            Database.SetInitializer(new BasicContentInitializer());
            _dbContext = new B1625DbContext();
        }

        public IEnumerable<Publication> Publications
        {
            get
            {
                return _dbContext.Publications.ToList();
            }
        }

        public IEnumerable<UserAccount> Accounts
        {
            get
            {
                return _dbContext.Accounts.ToList();
            }
        }
    }
}
