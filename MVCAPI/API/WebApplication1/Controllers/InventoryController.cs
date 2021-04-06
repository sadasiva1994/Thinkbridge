using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/Invntory")]
    public class InventoryController : ApiController
    {
        ThinkbridgeEntities db = new ThinkbridgeEntities();
        [HttpGet]
        [Route("GetInventoryDetails")]
        public HttpResponseMessage GetInventoryDetails()
        {
            try
            {
                var Itemdetails = db.Item_Details.ToList();

                if (Itemdetails != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Itemdetails);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Semester Record Not Found");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpPost]
        [Route("SaveItemData")]
        public HttpResponseMessage SaveInventory([FromBody] Item_Details details)
        {
            try
            {

                //Student_semester_details smd = new Student_semester_details()
                using (ThinkbridgeEntities entities = new ThinkbridgeEntities())
                {
                    entities.Item_Details.Add(details);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, details);
                    message.Headers.Location = new Uri(Request.RequestUri + details.Item_Name.ToString());
                    return message;
                }


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("UpdateInventDetals")]
        public HttpResponseMessage UpdateInventoryDetails([FromBody] Item_Details details)
        {
            try
            {
                using (ThinkbridgeEntities entities = new ThinkbridgeEntities())
                {
                    var entity = entities.Item_Details.FirstOrDefault(e => e.Item_id == details.Item_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Item Details is Not Found");
                    }
                    else
                    {
                        entity.Item_id = details.Item_id;
                        entity.Item_Name = details.Item_Name;
                        entity.Price = details.Price;
                        entity.Description = details.Description;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("DeleteInventDetals")]
        public HttpResponseMessage DeleteInventoryDetails(string id)
        {
            try
            {
                int idd = Convert.ToInt32(id);
                using (ThinkbridgeEntities entities = new ThinkbridgeEntities())
                {
                    var entity = entities.Item_Details.FirstOrDefault(e => e.Item_id == idd);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Item Details is Not Found");
                    }
                    else
                    {
                        entities.Item_Details.Attach(entity);
                        entities.Item_Details.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
