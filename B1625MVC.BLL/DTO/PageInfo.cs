using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625MVC.BLL.DTO
{
    public class PageInfo
    {
        public PageInfo(int currentPage, int elementsPerPage)
        {
            CurrentPage = currentPage;
            ElementsPerPage = elementsPerPage;
        }

        public int CurrentPage { get; }
        public int ElementsPerPage { get; }
    }
}
