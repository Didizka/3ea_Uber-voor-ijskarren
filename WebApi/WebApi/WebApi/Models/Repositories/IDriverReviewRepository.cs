using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Repositories;
using WebApi.Models.Reviews;


namespace WebApi.Models.Reviews
{
    public interface IDriverReviewRepository
    {
        bool DoesItemExist(string id);
        //Task<DriverReview> DoesItemExist(string id);

        IEnumerable<DriverReview> All { get; }
        DriverReview Find(string id);
        Task<bool> Insert(DriverReview item);
        void Update(DriverReview item);
        void Delete(string id);
    }
}
