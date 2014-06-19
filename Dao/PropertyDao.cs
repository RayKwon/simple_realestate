using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using SimpleRealestate.Models;
using System.Data.SqlClient;
using System.Data;

namespace SimpleRealestate.Dao
{
    public class PropertyDao : IPropertyDao
    {
        private const string ConnectionString = @"Data Source=A803\SQLEXPRESS;Initial Catalog=SimpleRealestate;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
        //private string ConnectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_DefaultConnection").ToString();

        public SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
       
        public PropertyInfo GetPropertyById(int id)
        {
            using (var connection = GetOpenConnection())
            {
                var sql = @"select * from PropertyInfo 
                            left join Agent on PropertyInfo.AgentId = Agent.Id 
                            where PropertyInfo.Id = @id
                            select * from Photo where PropertyId = @id";

                PropertyInfo propertyInfo;
                using (var multi = connection.QueryMultiple(sql, new { id = id }))
                {
                    propertyInfo = multi.Read<PropertyInfo, Agent, PropertyInfo>((property, agent) =>
                    {
                        property.Agent = agent;
                        return property;
                    }).SingleOrDefault();
                    propertyInfo.Photos = multi.Read<Photo>().ToList();
                };
                return propertyInfo;
            }
        }

        public int AddEnquiry(int propertyId, string email, string comment)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Execute("insert into Enquiry(PropertyId, Email, Comment) values(@propertyId, @email, @comment)", new { propertyId=propertyId, email=email, comment=comment });
            }
        }


        public List<PropertyInfo> SearchProperties(PropertyType propertyType, string address, int? minPrice, int? maxPrice)
        {
            using (var connection = GetOpenConnection())
            {
                var sql = @"select * from PropertyInfo 
                               where 1 = 1";
                if (propertyType == PropertyType.Rent || propertyType == PropertyType.Sale)
                {
                    sql += @" and PropertyType = @propertyType";
                }
                if (!String.IsNullOrWhiteSpace(address))
                {
                    sql += @" and Address like CONCAT('%', @address, '%')";
                }
                if (minPrice.HasValue && maxPrice.HasValue)
                {
                    sql += @" and Price between @minPrice and @maxPrice";
                }
                sql += @" select * from Photo";

                List<PropertyInfo> propertyInfos;
                using (var multi = connection.QueryMultiple(sql, new { propertyType=propertyType, address = address, minPrice = minPrice, maxPrice = maxPrice }))
                {
                    propertyInfos = multi.Read<PropertyInfo>().ToList();
                    var photos = multi.Read<Photo>();
                    propertyInfos.ForEach((p) =>
                    {
                        p.Photos = photos.Where(photo => photo.PropertyId == p.Id).ToList();
                    });
                };
                return propertyInfos;
            }
        }
    }
}