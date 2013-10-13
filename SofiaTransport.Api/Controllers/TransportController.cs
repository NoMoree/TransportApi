using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using CodeFirst.DataLayer;

namespace SofiaTransport.Api.Controllers
{
    public class TransportController : BaseApiController
    {
        #region UpdateDb used
        
        [HttpGet]
        public HttpResponseMessage Update()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    InitOrUpdate();
                    
                    var response =
                        this.Request.CreateResponse(HttpStatusCode.OK);
                    
                    return response;
                });
            return responseMsg;
        }
        
        #endregion
        
        #region GetDataType 1,2,3
        
        [HttpGet]
        public List<LineResponse> GetDataType1()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new SofiaTransportContext();
                
                var transports = context.Transport;
                var directions = context.Directions;
                var stops = context.Stops;
                var stopNames = context.StopName;
                
                List<string> listOfStopsStrings = (from t in transports
                                                   where t.Type == 1
                                                   from d in t.Directions
                                                   select d.DirectionStops).ToList();
                
                var response = (from t in transports
                                where t.Type == 1
                                select new LineResponse()
                                {
                                    Name = t.Name,
                                    DirectionResponces = (from d in t.Directions
                                                          select new DirectionResponse()
                                                          {
                                                              FirstStop = d.FirstStop.Id,
                                                              LastStop = d.LastStop.Id
                                                              //,
                                                              //StopsIds = (from id in d.DirectionStops.Split(',')
                                                              //            select int.Parse(id))
                                                              /*(from s in d.Stops
                                                              select s.Id)*/
                                                          })
                                }).ToList();
                
                int index = 0;
                for (int i = 0; i < response.Count(); i++)
                {
                    foreach (var dir in response[i].DirectionResponces)
                    {
                        var splited = listOfStopsStrings[index++].Split(',').Select(t => int.Parse(t));
                        dir.StopsIds = splited;
                    }
                }
                
                return response;
            });
            return responseMsg;
        }
        
        [HttpGet]
        public List<LineResponse> GetDataType2()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new SofiaTransportContext();
                
                var transports = context.Transport;
                var directions = context.Directions;
                var stops = context.Stops;
                var stopNames = context.StopName;
                
                List<string> listOfStopsStrings = (from t in transports
                                                   where t.Type == 2
                                                   from d in t.Directions
                                                   select d.DirectionStops).ToList();
                
                var response = (from t in transports
                                where t.Type == 2
                                select new LineResponse()
                                {
                                    Name = t.Name,
                                    DirectionResponces = (from d in t.Directions
                                                          select new DirectionResponse()
                                                          {
                                                              FirstStop = d.FirstStop.Id,
                                                              LastStop = d.LastStop.Id
                                                              /*,
                                                              StopsIds = (from id in d.DirectionStops.Split(',')
                                                              select int.Parse(id))*/
                                                              /*(from s in d.Stops
                                                              select s.Id)*/
                                                          })
                                }).ToList();
                
                int index = 0;
                //List<int
                for (int i = 0; i < response.Count(); i++)
                {
                    //for (int j = 0; j < response[i].DirectionResponces.Count(); j++)
                    //{
                    foreach (var dir in response[i].DirectionResponces)
                    {
                        var splited = listOfStopsStrings[index++].Split(',').Select(t => int.Parse(t));
                        dir.StopsIds = splited;
                    }
                    //    var splited = listOfStopsStrings[index++].Split(',').Select(t => int.Parse(t));
                    //    response[i].DirectionResponces[j].StopsIds = splited;
                    //}
                }
                //    foreach (var direction in transport.DirectionResponces)
                //    {
                
                //        direction.StopsIds=(from id in listOfStopsStrings[0].Split(',')
                //                                                          select int.Parse(id))
                //    }
                //}
                /*  response[0].DirectionResponces[0].*/
                
                return response;
            });
            return responseMsg;
        }
        
        [HttpGet]
        public List<LineResponse> GetDataType3()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new SofiaTransportContext();
                
                var transports = context.Transport;
                var directions = context.Directions;
                var stops = context.Stops;
                var stopNames = context.StopName;
                
                List<string> listOfStopsStrings = (from t in transports
                                                   where t.Type == 3
                                                   from d in t.Directions
                                                   select d.DirectionStops).ToList();
                
                var response = (from t in transports
                                where t.Type == 3
                                select new LineResponse()
                                {
                                    Name = t.Name,
                                    DirectionResponces = (from d in t.Directions
                                                          select new DirectionResponse()
                                                          {
                                                              FirstStop = d.FirstStop.Id,
                                                              LastStop = d.LastStop.Id
                                                              //,
                                                              //StopsIds = (from id in d.DirectionStops.Split(',')
                                                              //            select int.Parse(id))
                                                              /*(from s in d.Stops
                                                              select s.Id)*/
                                                          })
                                }).ToList();
                
                int index = 0;
                for (int i = 0; i < response.Count(); i++)
                {
                    foreach (var dir in response[i].DirectionResponces)
                    {
                        var splited = listOfStopsStrings[index++].Split(',').Select(t => int.Parse(t));
                        dir.StopsIds = splited;
                    }
                }
                
                return response;
            });
            return responseMsg;
        }
        
        #endregion
        
        [HttpGet]
        public List<string> GetNames()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new SofiaTransportContext();
                
                var transports = context.Transport;
                var directions = context.Directions;
                var stops = context.Stops;
                var stopNames = context.StopName;
                
                var response = (from n in stopNames
                                select n.Name).ToList();
                
                return response;
            });
            return responseMsg;
        }
        
        [HttpGet]
        public List<RelationResponse> GetRelations()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new SofiaTransportContext();
                
                var transports = context.Transport;
                var directions = context.Directions;
                var stops = context.Stops;
                var stopNames = context.StopName;
                
                var response = (from s in stops
                                select new RelationResponse()
                                {
                                    Name = s.Name.Id,
                                    PublicId = s.PublicId
                                }).ToList();
                
                return response;
            });
            return responseMsg;
        }
        
        [HttpPost]
        public SchedulesModel GetSchedules(SchedulesRequestModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new SofiaTransportContext();
                
                int selectedTypeId = 1;
                if (model.SelectedTypeId == 1)
                {
                    //tram
                    selectedTypeId = 3;
                }
                else if (model.SelectedTypeId == 2)
                {
                    selectedTypeId = 2;
                    //trolei
                }
                
                var transports = context.Transport;
                var directions = context.Directions;
                var stops = context.Stops;
                var stopNames = context.StopName;
                
                var transport = (from t in transports
                                 where t.Type == selectedTypeId && t.Name == model.SelectedNumber
                                 select t).FirstOrDefault();
                string transportId = "";
                string directionId = "";
                string stopId = "";
                if (transport != null)
                {
                    transportId = transport.SiteId;
                    directionId = transport.Directions.First().DirectionId;
                    stopId = (from s in stops
                              where s.Id == model.StopId
                              select s.StopId).First();
                    var response = GetSchedulesFromSite(transportId, directionId, stopId, selectedTypeId);
                    
                    return response;
                }
                
                return new SchedulesModel();
            });
            return responseMsg;
        }
    }
    
    public class SchedulesRequestModel
    {
        [DataMember(Name = "selectedTypeId")]
        public int SelectedTypeId { get; set; }
        
        [DataMember(Name = "selectedNumber")]
        public string SelectedNumber { get; set; }
        
        [DataMember(Name = "selectedDirectionId")]
        public int SelectedDirectionId { get; set; }
        
        [DataMember(Name = "stopId")]
        public int StopId { get; set; }
    }
    
    [DataContract(Name = "s")]
    public class SchedulesModel
    {
        [DataMember(Name = "t")]
        public string MainTime { get; set; }
        
        [DataMember(Name = "a")]
        public string InfoAt { get; set; }
        
        [DataMember(Name = "more")]
        public List<TransportModel> More { get; set; }
        
        public SchedulesModel()
        {
            this.More = new List<TransportModel>();
        }
    }
    
    [DataContract(Name = "t")]
    public class TransportModel
    {
        /*[DataMember(Name = "t")]
        public string Time { get; set; }
        */
        [DataMember(Name = "a")]
        public string Time { get; set; }
        
        [DataMember(Name = "n")]
        public string Number { get; set; }
        
        [DataMember(Name = "t")]
        public int Type { get; set; }
    }
    
    [DataContract(Name = "line")]
    public class RelationResponse
    {
        [DataMember(Name = "i")]
        public int PublicId { get; set; }
        
        [DataMember(Name = "n")]
        public int Name { get; set; }
    }
    
    [DataContract(Name = "line")]
    public class LineResponse
    {
        [DataMember(Name = "d")]
        public IEnumerable<DirectionResponse> DirectionResponces { get; set; }
        
        //public List<DirectionResponse> DirectionResponces { get; set; }
        
        [DataMember(Name = "n")]
        public string Name { get; set; }
    }
    
    [DataContract(Name = "d")]
    public class DirectionResponse
    {
        [DataMember(Name = "f")]
        public int FirstStop { get; set; }
        
        [DataMember(Name = "l")]
        public int LastStop { get; set; }
        
        [DataMember(Name = "s")]
        public IEnumerable<int> StopsIds { get; set; }
    }
}