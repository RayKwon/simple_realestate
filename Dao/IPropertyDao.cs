using SimpleRealestate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleRealestate.Dao
{
    public interface IPropertyDao
    {
        PropertyInfo GetPropertyById(int id);
        int AddEnquiry(int propertyId, string email, string comment);
        List<PropertyInfo> SearchProperties(PropertyType propertyType, string address, int? minPrice, int? maxPrice);
    }
}