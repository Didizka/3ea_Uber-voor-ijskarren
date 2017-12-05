using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.Orders.Repo;
using WebApi.Models.Reviews;

namespace WebApi.Models.Repositories
{
    public class DriverReviewRepository : IDriverReviewRepository
    {
        private List<DriverReview> _driveReviewList;

        private readonly ReviewContext context;


        public DriverReviewRepository()
        {
            InitializeData();
            this.context = context;

        }
        public IEnumerable<DriverReview> All
        {
            get { return _driveReviewList; }
        }

        public bool DoesItemExist(string id)
        {
            return _driveReviewList.Any(item => item.DriverReviewID == id);
        }


        public DriverReview Find(string id)
        {
            return _driveReviewList.FirstOrDefault(item => item.DriverReviewID == id);
        }

        public async Task<bool> Insert(DriverReview item)
        {
            _driveReviewList.Add(item);

            DriverReview currentReview = new DriverReview
            {
                //Customer = customer,
                DriverReviewID = "3",
                UserID = 80,
                DriverID = 81,
                Beoordeling = "Dit is een test",
                Score = 2,
                Done = true,

            };
            //used to save code to database
            await context.DriverReview.AddAsync(currentReview);
            await context.SaveChangesAsync();
            return true;
        }

        public void Update(DriverReview item)
        {
            var todoItem = this.Find(item.DriverReviewID);
            var index = _driveReviewList.IndexOf(todoItem);
            _driveReviewList.RemoveAt(index);
            _driveReviewList.Insert(index, item);
        }

        public void Delete(string id)
        {
            _driveReviewList.Remove(this.Find(id));
        }

        private void InitializeData()
        {
            _driveReviewList = new List<DriverReview>();

            var todoItem1 = new DriverReview
            {
                DriverReviewID = "0",
                UserID = 50,
                DriverID = 51,
                Beoordeling = "was a good driver",
                Score = 3,
                Done = true
            };

            var todoItem2 = new DriverReview
            {
                DriverReviewID = "1",
                UserID = 60,
                DriverID = 62,
                Beoordeling = "was a bad driver",
                Score = 4,
                Done = false
            };

            var todoItem3 = new DriverReview
            {
                DriverReviewID = "2",
                UserID = 70,
                DriverID = 71,
                Beoordeling = "Gave me free icecream :)",
                Score = 5,
                Done = false,
            };

            _driveReviewList.Add(todoItem1);
            _driveReviewList.Add(todoItem2);
            _driveReviewList.Add(todoItem3);
            }

        
    }
}
