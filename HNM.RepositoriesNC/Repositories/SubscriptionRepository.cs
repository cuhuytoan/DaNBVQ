using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace HNM.RepositoriesNC.Repositories
{
    public interface ISubscriptionRepository : IRepositoryBase<Subscription>
    {
        Task<Subscription> GetByEmail(string email);
        Task<int> SubscriptionEmail(string email);
    }
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<Subscription> GetByEmail(string email)
        {
            return await HanomaContext.Subscription.FirstOrDefaultAsync(x=>x.Email.Equals(email));
        }
        public async Task<int> SubscriptionEmail(string email)
        {
            var result = new Subscription();
            var subscription = await HanomaContext.Subscription.FirstOrDefaultAsync(x => x.Email.Equals(email));
            var model = new Subscription
            {
                Email = email,
                CreateDate = DateTime.Now,
                Active = true
            };
            await HanomaContext.Subscription.AddAsync(model);
            await HanomaContext.SaveChangesAsync();
            return model.Subscription_ID;
        }
    }
}
