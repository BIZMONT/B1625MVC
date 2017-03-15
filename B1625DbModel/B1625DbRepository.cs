using B1625DbModel.Entities;
using B1625DbModel.Initializers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625DbModel
{
    public class B1625DbRepository
    {
        private B1625DbContext DbContext;
        private int _publicationsPerPage = 20; //TODO: make this like property

        public B1625DbRepository()
        {
            Database.SetInitializer(new BasicContentInitializer());
            DbContext = new B1625DbContext();
        }
        public ICollection<Publication> GetFreshPublications(int page)
        {
            return DbContext.Publications.OrderBy(p => p.PublicationDate).Skip(_publicationsPerPage * (page - 1)).Take(_publicationsPerPage).ToList();
        }

        public ICollection<Publication> GetHotPublications(int page)
        {

            return DbContext.Publications.OrderBy(p=>p.Title).Skip(_publicationsPerPage * (page - 1)).Take(_publicationsPerPage).ToList(); //TODO: Make better filtering
        }

        public object GetBestPublications(int page)
        {
            return DbContext.Publications.OrderBy(p=>p.Rating).Skip(_publicationsPerPage * (page - 1)).Take(_publicationsPerPage).ToList(); //TODO: Make better filtering
        }

        public Publication GetPublication(long id)
        {
            return DbContext.Publications.Find(id);
        }

        public bool RateUpPublication(long id, string username) 
        {
            //TODO: Rate with user
            return false;
        }
    }
}
