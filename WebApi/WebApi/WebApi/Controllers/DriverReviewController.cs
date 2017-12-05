using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Reviews;

namespace WebApi.Controllers
{
    [Route("api/driverReview")]
    public class DriverReviewController : Controller
    {
        private readonly IDriverReviewRepository _driverReviewRepositry;

        public DriverReviewController(IDriverReviewRepository driverReviewRepository)
        {
            _driverReviewRepositry = driverReviewRepository;
        }

        //////////////////////////////////// 
        ///     GET: api/Orders      ///////
        //////////////////////////////////// 
        [HttpGet]
        public IActionResult List()
        {
            return Ok(_driverReviewRepositry.All);
        }
        //////////////////////////////////// 
        ///     POST: api/Orders      //////
        //////////////////////////////////// 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DriverReview item)
        {
            await _driverReviewRepositry.Insert(item);
            //try
            //{
            //    if (item == null || !ModelState.IsValid)
            //    {
            //        //Checks if the post used the correct parameters
            //        return BadRequest(ErrorCode.ParametersAreNotValid.ToString());
            //    }
            //    bool itemExists = _driverReviewRepositry.DoesItemExist(item.DriverReviewID);
            //    if (itemExists)
            //    {
            //        //Checsks if DriverReviewID is already in use or not
            //        return StatusCode(StatusCodes.Status409Conflict, ErrorCode.ReviewIdInUse.ToString());
            //    }
            //    _driverReviewRepositry.Insert(item);
            //    return Ok(ErrorCode.ItemSuccesfullyCreated.ToString());
            //}
            //catch (Exception)
            //{
            //    return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            //}
            return Ok(item);
        }
        //////////////////////////////////// 
        ///     PUT: api/Orders      ///////
        //////////////////////////////////// 
        [HttpPut]
        public IActionResult Edit([FromBody] DriverReview item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.ParametersAreNotValid.ToString());
                }
                var existingItem = _driverReviewRepositry.Find(item.DriverReviewID);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _driverReviewRepositry.Update(item);
                return Ok(ErrorCode.ItemSuccesfullyUpdated.ToString());
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }
        //////////////////////////////////// 
        ///     DELETE: api/Orders      ////
        //////////////////////////////////// 
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var item = _driverReviewRepositry.Find(id);
                if (item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _driverReviewRepositry.Delete(id);
                return Ok(ErrorCode.ItemSuccesfullyDeleted.ToString());
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }




        public enum ErrorCode
        {
            ParametersAreNotValid,
            ReviewIdInUse,
            RecordNotFound,
            CouldNotCreateItem,
            CouldNotUpdateItem,
            CouldNotDeleteItem,
            ItemSuccesfullyDeleted,
            ItemSuccesfullyUpdated,
            ItemSuccesfullyCreated
        }

    }

}
