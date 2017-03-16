using System.Collections.Generic;

using B1625MVC.Model.Entities;

namespace B1625MVC.Model.Abstract
{
    public interface IB1625Repository
    {
        IEnumerable<Publication> Publications { get; }
    }
}
