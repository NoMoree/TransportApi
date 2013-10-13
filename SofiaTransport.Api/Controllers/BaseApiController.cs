using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using CodeFirst.DataLayer;
using CodeFirst.Model;

namespace SofiaTransport.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected const string SessionKey = "hTe*$h3krbxeobewfAU0Et4w8Wbp$gqPZoPOiQ(ldpRXUbJu6z";

        #region bus tram trol
        static private readonly string[] bus =
        {
            "9",
            "11",
            "45",
            "60",
            /*"65",*/
            "72",
            "73",
            "74",
            "75",
            "76",
            "77",
            "78",
            "79",
            "82",
            "83",
            "84",
            "85",
            "86",
            /*"87",*/
            "88",
            "94",
            "101",
            "102",
            "108",
            "111",
            "113",
            "114",
            "120",
            "204",
            "213",
            "260",
            "280",
            "285",
            /*"294",*/
            "305",
            "306",
            "309",
            "310",
            "314",
            "384",
            "404",
            "413",
            "604"
        };
        static private readonly string[] trol =
        {
            "1",
            "2",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "11"
        };

        static private readonly string[] tram =
        {
            "1",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "10",
            "11",
            "12",
            "18",
            "19",
            "20",
            "22",
            "23"
        };
        #endregion

        protected T PerformOperationAndHandleExceptions<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                throw new HttpResponseException(errResponse);
            }
        }

        protected void InitOrUpdate()
        {
            var context = new SofiaTransportContext();

            using (context)
            {
                InitOrUpdateDays();
                //InitOrUpdateTvPrograms();
                //InitSchedulePrivate();

                #region oldWithCheck

                ////var dbMeta = context.DbMetadata.FirstOrDefault();
                ////var yesterday = DateTime.Now.AddDays(-1);
                ////var dayToday = (from day in context.Day
                ////                where (day.Date > yesterday)
                ////                select day.Id).FirstOrDefault();
                //if (dbMeta == null)
                //{
                //    InitOrUpdateDays();
                //    InitOrUpdateTvPrograms();
                //    InitSchedulePrivate();
                //    var newDbMeta = new CodeFirst.Model.DbMetadata()
                //    {
                //        LastUpdate = dayToday,//DateTime.Now,
                //        OnProgramIdChange = dayToday//DateTime.Now
                //    };
                //    context.DbMetadata.Add(newDbMeta);
                //    context.SaveChanges();
                //}
                ////#region If new Init DB data
                ////if (dbMeta.LastUpdate.Year != DateTime.Now.Year)
                ////{
                ////}
                ////#endregion
                //#region Else Update DB data If not it was not UPDATED TODAY
                //else if (dbMeta.LastUpdate != dayToday)//.DayOfYear != DateTime.Now.DayOfYear)
                //{
                //    InitOrUpdateDays();
                //    //InitOrUpdateTvPrograms();
                //    UpdateSchedulePrivate();
                //    dbMeta.LastUpdate = dayToday;
                //    context.SaveChanges();
                //}
                //#endregion 

                #endregion
            }
            //return context;
        }

        //protected HttpResponseMessage InitOrUpdateTvPrograms()
        //{
        //    var responseMsg = this.PerformOperationAndHandleExceptions(
        //        () =>
        //        {
        //            var context = new SofiaTransportContext();
        //            using (context)
        //            {
        //                var tvPrograms = context.Transport;

        //                string main = GetMainHtml();
        //                var allPrograms = GetListOfPrograms(main);
        //                List<TvProgramModel> programs = new List<TvProgramModel>();
        //                for (int i = 0; i < 20; i++)
        //                {
        //                    programs.Add(allPrograms[i]);
        //                }

        //                var models =
        //                    (from tv in tvPrograms
        //                     select new TvProgramModel()//tv.ProgramId, tv.Name)
        //                     {
        //                         Name = tv.Name,
        //                         ProgramId = tv.ProgramId
        //                     });
        //                bool isAdded;
        //                foreach (var program in programs)
        //                {
        //                    isAdded = false;
        //                    foreach (var model in models)
        //                    {
        //                        if (program.Name == model.Name)
        //                        {
        //                            if (program.ProgramId == model.ProgramId)
        //                            {
        //                                isAdded = true;
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                //TODO UpdateProgramChange
        //                                //model.ProgramId = program.ProgramId;
        //                                //context.SaveChanges();
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    if (!isAdded)
        //                    {
        //                        var newProgram = new CodeFirst.Model.Transport()
        //                        {
        //                            Name = program.Name,
        //                            ProgramId = program.ProgramId
        //                            //LastUpdatedDate = DateTime.Now
        //                        };
        //                        tvPrograms.Add(newProgram);
        //                        context.SaveChanges();

        //                    }
        //                }

        //                var response =
        //                    this.Request.CreateResponse(HttpStatusCode.OK);

        //                return response;
        //            }
        //        });
        //    return responseMsg;
        //}

        private static string ReadFrom( string name)
        {
            string path = @"C:\Temp\CORDOVA\Project\SofiaTransport\"+name+".txt";
            string output ="";

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                   output= sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

            return output;
        }


        private static Transport GetSpritBusStops(int id, string number, SofiaTransportContext context)
        {
            var response = Get("http://m.sofiatraffic.bg/schedules?tt=" + id + "&ln=" + number + "&s=Търсене");

            var splitByDirections = response.Split(new string[]
            {
                "\"stop\">"
            }, StringSplitOptions.None);

            if (splitByDirections.Count() < 3)
            {
                var j8748 = 0;
                return GetSpritBusStops(id, number, context);
            }

            int startIndexOfTransportId = splitByDirections[1].IndexOf("\" name=\"lid\"/>") - 7;
            string transportId = splitByDirections[1].Substring(startIndexOfTransportId, 7);
            if (!transportId.StartsWith("1"))
            {
                var j878 = 0;
            }
            Direction dir1 = CreatDirection(splitByDirections[1], context);
            dir1.FirstStop = dir1.Stops.First();
            dir1.LastStop = dir1.Stops.Last();

            Direction dir2 = CreatDirection(splitByDirections[2], context);
            dir2.FirstStop = dir2.Stops.First();
            dir2.LastStop = dir2.Stops.Last();

            if (dir1.FirstStop != dir2.LastStop ||
                dir1.LastStop != dir2.FirstStop)
            {
                var j78 = 0;
            }

            var directions = context.Directions;

            directions.Add(dir1);
            directions.Add(dir2);
            context.SaveChanges();

            var newTransport = new Transport()
            {
                SiteId = transportId,
                Name = number
            };

            newTransport.Directions.Add(dir1);
            newTransport.Directions.Add(dir2);

            //string[] stringOfAllStops = response.Substring(
            //    startIndexOfSelectTv,
            //    endIndexOfSelectTVs - startIndexOfSelectTv).Split(new string[] 
            //            { 
            //                "value=\""
            //            },
            //            StringSplitOptions.RemoveEmptyEntries
            //        );
            //foreach (string stopStr in stringOfAllStops)
            //{
            //    var split = stopStr.Split(new string[] 
            //            { 
            //                "value=\""
            //            }, StringSplitOptions.None);
            //}

            return newTransport;
        }

        private static Direction CreatDirection(string oneDirection, SofiaTransportContext context)
        {
            Direction newDirection = new Direction();

            int endOfDirectionIdString = oneDirection.LastIndexOf("value=\"");
            newDirection.DirectionId = oneDirection.Substring(endOfDirectionIdString + 7, 7);
            var endIndexOfList = oneDirection.IndexOf("</select>");

            if (endIndexOfList < 0)
            {
                return null;
            }

            var subString = oneDirection.Substring(0, endIndexOfList).Split(new string[]
            {
                "value=\""
            }, StringSplitOptions.None);

            var names = context.StopName;
            var stops = context.Stops;

            int lenght = subString.Length;

            List<int> DirectionStops = new List<int>();
            for (int i = 1; i < lenght; i++)
            {
                var newStop = extractNewStop(subString, i, stops, names);
                //DirectionStops.Append(newStop.Id + ",");
                newDirection.Stops.Add(newStop);
                context.SaveChanges();

                DirectionStops.Add(newStop.Id);

            }
            newDirection.DirectionStops = string.Join(",", DirectionStops);

            context.SaveChanges();

            return newDirection;
        }

        private static Stop extractNewStop(string[] subString, int i, DbSet<Stop> stops, DbSet<StopName> names)
        {
            var usableString = subString[i].Split(new string[]
            {
                "</option>"
            }, StringSplitOptions.None)[0];

            var stopId = usableString.Substring(0, 7).TrimEnd();
            var fullName = usableString.Substring(9, usableString.Length - 10).Trim().Split(new char[]
            {
                '(',
                ')'
            });
            string name = fullName[0].TrimEnd();
            int publicId = 0;
            if (fullName.Count() > 3)
            {
                for (int index = 1; index < fullName.Count() - 2; index++)
                {
                    name += fullName[index];
                }
                publicId = int.Parse(fullName[fullName.Count() - 2]);
            }
            else
            {
                publicId = int.Parse(fullName[1]);
            }

            var newStop = (from s in stops
                           where s.StopId == stopId
                           select s).FirstOrDefault();

            if (newStop == null)
            {
                var newName = (from n in names
                               where n.Name == name
                               select n).FirstOrDefault();
                if (newName == null)
                {
                    newName = new StopName()
                    {
                        Name = name
                    };
                    names.Add(newName);
                }

                newStop = new Stop()
                {
                    StopId = stopId,
                    Name = newName,
                    PublicId = publicId
                };

                stops.Add(newStop);
            }
            else
            {
                //#if (DEBUG)
                if (newStop.StopId == stopId)
                {
                    if (newStop.PublicId != publicId)
                    {
                        var lkjfdlsf = 243;
                    }
                }
                //                foreach (var item in debugAllMainFolders)
                //                {
                //                    Debug.WriteLine("{0,-20}{1,-90}{2,-39}", item.Name, item.Path, item.AccessToken);
                //                }
                //#endif
            }

            #region Before Optimizing (OldWorking)

            //var name = usableString.Substring(9, usableString.Length - 9).Trim();

            //var newStop = (from s in stops
            //               where s.StopId == stopId
            //               select s).FirstOrDefault();

            //if (newStop == null)
            //{
            //    var newName = (from n in names
            //                   where n.Name == name
            //                   select n).FirstOrDefault();
            //    if (newName == null)
            //    {
            //        newName = new StopName()
            //        {
            //            Name = name
            //        };
            //        names.Add(newName);
            //    }

            //    newStop = new Stop()
            //    {
            //        StopId = stopId,
            //        Name = newName
            //    };
            //    stops.Add(newStop);
            //} 

            #endregion

            return newStop;
        }

        protected static SchedulesModel GetSchedulesFromSite(string ransportId, string directionId, string stopId, int type)
        {
            Console.InputEncoding = System.Text.Encoding.GetEncoding("windows-1251");
            Console.OutputEncoding = System.Text.Encoding.GetEncoding("windows-1251");

            var response = Get("http://m.sofiatraffic.bg/schedules/vehicle-vt?s=" + stopId +
                                "&lid=" + ransportId +
                                "&vt=" + type + "&rid=" + directionId);

            //var response = ReadFrom("1");
            var split = response.Split(new string[] { "Разписание" }, StringSplitOptions.None);

            var output = new SchedulesModel();
            string main = split[0];
            int indexOfAtStart = main.IndexOf("Информация към") + 15;
            int indexOfAtEnd = main.IndexOf("<", indexOfAtStart);
            string informationAt = main.Substring(indexOfAtStart, indexOfAtEnd - indexOfAtStart).TrimEnd();

            int indexOfMainAtStart = main.IndexOf("<b>", indexOfAtEnd) + 3;
            int indexOfMainAtEnd = main.IndexOf("<", indexOfMainAtStart);

            string mainTime = main.Substring(indexOfMainAtStart, indexOfMainAtEnd - indexOfMainAtStart).TrimEnd();
            output.InfoAt = informationAt;
            output.MainTime = mainTime;


            var sub = "";
            if (split[1].Contains("Автобуси"))
            {
                var timing = split[1].Split(new string[] { "<b>" }, StringSplitOptions.None);
                if (timing.Length > 1)
                {
                    foreach (string t in timing.Skip(2))
                    {
                        int end = t.IndexOf("<");
                        string number = t.Substring(0, end);


                        var timeStart = t.IndexOf(";-&nbsp;") + 9;
                        var timeEnd = t.IndexOf("<br");
                        string time = t.Substring(timeStart, timeEnd - timeStart).Trim();

                        var newTrans = new TransportModel()
                            {
                                Type = 1,
                                Number = number,
                                Time = time
                            };

                        output.More.Add(newTrans);
                    }
                }
            }

            if (split[1].Contains("Трамваи"))
            {
                var timing = split[1].Split(new string[] { "<b>" }, StringSplitOptions.None);
                if (timing.Length > 1)
                {
                    foreach (string t in timing.Skip(2))
                    {
                        int end = t.IndexOf("<");
                        string number = t.Substring(0, end);


                        var timeStart = t.IndexOf(";-&nbsp;") + 9;
                        var timeEnd = t.IndexOf("<br />");
                        string time = t.Substring(timeStart, timeEnd - timeStart).Trim();

                        var newTrans = new TransportModel()
                        {
                            Type = 2,
                            Number = number,
                            Time = time
                        };

                        output.More.Add(newTrans);
                    }
                }
            }

            if (split[1].Contains("Тролейбуси"))
            {
                var timing = split[1].Split(new string[] { "<b>" }, StringSplitOptions.None);
                if (timing.Length > 1)
                {
                    foreach (string t in timing.Skip(2))
                    {
                        int end = t.IndexOf("<");
                        string number = t.Substring(0, end);


                        var timeStart = t.IndexOf(";-&nbsp;") + 9;
                        var timeEnd = t.IndexOf("<br />");
                        string time = t.Substring(timeStart, timeEnd - timeStart).Trim();

                        var newTrans = new TransportModel()
                        {
                            Type = 2,
                            Number = number,
                            Time = time
                        };

                        output.More.Add(newTrans);
                    }
                }
            }
            //"Информация към" //14

            //"пристигане:" indexOf("<b>") indexIf("</b>")

            return output;

            //var splitByDirections = response.Split(new string[]
            //{
            //    "\"stop\">"
            //}, StringSplitOptions.None);

            //if (splitByDirections.Count() < 3)
            //{
            //    var j8748 = 0;
            //    return GetSpritBusStops(id, number, context);
            //}

            //int startIndexOfTransportId = splitByDirections[1].IndexOf("\" name=\"lid\"/>") - 7;
            //string transportId = splitByDirections[1].Substring(startIndexOfTransportId, 7);
            //if (!transportId.StartsWith("1"))
            //{
            //    var j878 = 0;
            //}
            //Direction dir1 = CreatDirection(splitByDirections[1], context);
            //dir1.FirstStop = dir1.Stops.First();
            //dir1.LastStop = dir1.Stops.Last();

            //Direction dir2 = CreatDirection(splitByDirections[2], context);
            //dir2.FirstStop = dir2.Stops.First();
            //dir2.LastStop = dir2.Stops.Last();

            //if (dir1.FirstStop != dir2.LastStop ||
            //    dir1.LastStop != dir2.FirstStop)
            //{
            //    var j78 = 0;
            //}

            //var directions = context.Directions;

            //directions.Add(dir1);
            //directions.Add(dir2);
            //context.SaveChanges();

            //var newTransport = new Transport()
            //{
            //    SiteId = transportId,
            //    Name = number
            //};

            //newTransport.Directions.Add(dir1);
            //newTransport.Directions.Add(dir2);
            //return stops;
            //return stringOfAllPrograms;
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        protected static Transport GetBusStops(int type, string number, SofiaTransportContext context)
        {
            Console.InputEncoding = System.Text.Encoding.GetEncoding("windows-1251");
            Console.OutputEncoding = System.Text.Encoding.GetEncoding("windows-1251");

            Transport stops = GetSpritBusStops(type, number, context);

            return stops;
            //return stringOfAllPrograms;
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        protected HttpResponseMessage InitOrUpdateDays()
        {
            HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
                () =>
                {
                    var context = new SofiaTransportContext();
                    using (context)
                    {
                        //var dayContext = context.Day;
                        //if (main == "")
                        //{
                        //    main = GetMainHtml();
                        //}
                        int type;
                        var transports = context.Transport;

                        foreach (var line in bus)
                        {
                            type = 1;

                            var newTransport = (from t in transports
                                                where t.Name == line && t.Type == type
                                                select t).FirstOrDefault();
                            if (newTransport == null)
                            {
                                newTransport = GetBusStops(type, line, context);
                                newTransport.Type = type;
                                transports.Add(newTransport);
                                context.SaveChanges();
                            }
                        }


                        foreach (var line in trol)
                        {
                            type = 2;

                            var newTransport = (from t in transports
                                                where t.Name == line && t.Type == type
                                                select t).FirstOrDefault();
                            if (newTransport == null)
                            {
                                newTransport = GetBusStops(type, line, context);
                                newTransport.Type = type;
                                transports.Add(newTransport);
                                context.SaveChanges();
                            }
                        }


                        foreach (var line in tram)
                        {
                            type = 3;

                            var newTransport = (from t in transports
                                                where t.Name == line && t.Type == type
                                                select t).FirstOrDefault();
                            if (newTransport == null)
                            {
                                newTransport = GetBusStops(type, line, context);
                                newTransport.Type = type;
                                transports.Add(newTransport);
                                context.SaveChanges();
                            }
                        }
                        var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);

                        return response;
                    }
                });
            return responseMsg;
        }

        public static string Get(string resourceUrl)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "GET";

            var response = request.GetResponse();
            string responseString;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            return responseString;
        }
        /// <summary>
        /// ///////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        //protected HttpResponseMessage InitSchedulePrivate()
        //{
        //    var responseMsg = this.PerformOperationAndHandleExceptions(
        //        () =>
        //        {
        //            var context = new SofiaTransportContext();
        //            using (context)
        //            {
        //                //var dayContext = context.Directions;
        //                var tvPrograms = context.Transport;
        //                var days = context.Day;
        //                var allDays =
        //                   (from day in days
        //                    select new DayModel()
        //                    {
        //                        Id = day.Id,
        //                        Name = day.Name,
        //                        Date = day.Date
        //                    }).ToList();
        //                int programsCount = context.Transport.Count();
        //                var allDaysCount = allDays.Count();
        //                for (int i = 1; i < programsCount + 1; i++)//programsCount; i++)
        //                {
        //                    for (int thisDay = 1; thisDay < allDaysCount + 1; thisDay++)
        //                    {
        //                        var thisProgram = tvPrograms.FirstOrDefault(tv => tv.Id == i);
        //                        string schedulePage = GetShowsHtml(
        //                            thisProgram.ProgramId,
        //                            allDays[thisDay - 1].GetDateSiteFromat());
        //                        var newShows = GetListOfShows(schedulePage);
        //                        if (newShows == null || newShows.Count < 3)
        //                        {
        //                            break;
        //                        }
        //                        var newSchedule = new CodeFirst.Model.Direction()
        //                        {
        //                            Day = days.FirstOrDefault(day => day.Id == thisDay)
        //                        };
        //                        foreach (var show in newShows)
        //                        {
        //                            newSchedule.Stops.Add(new CodeFirst.Model.Stop()
        //                            {
        //                                Name = show.Name,
        //                                StarAt = show.StartAt
        //                                //,
        //                                //Transport = thisProgram,
        //                                //Day = newSchedule
        //                            });
        //                        }
        //                        thisProgram.Days.Add(newSchedule);
        //                        thisProgram.LastUpdatedDate = thisDay;// newSchedule.Day;
        //                        context.SaveChanges();
        //                    }
        //                }
        //                #region asd
        //                //foreach (var lastUpdate in programsLastUpdatedDateId)
        //                //{
        //                //}
        //                //var allInformationFromDB = (from tv in tvPrograms
        //                //                            select new CheckHelperModel()//tv.ProgramId, tv.Name)
        //                //                            {
        //                //                                Id = tv.Id,
        //                //                                ProgramId = tv.ProgramId,
        //                //                                Days = (from day in tv.Days
        //                //                                        where day.Date < DateTime.Now
        //                //                                        select new DataHelperModel()
        //                //                                        {
        //                //                                            GetDate = day.Date,
        //                //                                            Count = day.Stop.Count()
        //                //                                        })
        //                //                            });
        //                //foreach (CheckHelperModel program in allInformationFromDB)
        //                //{
        //                //}
        //                //.Where(day => day.Date >DateTime.Now)
        //                //day => day.Date < DateTime.Now)
        //                //select new DataHelperModel()
        //                //{
        //                //    GetDate = day.Date,
        //                //    Count = day.Stop.Count()
        //                //})
        //                //foreach (var item in collection)
        //                //{
        //                //}
        //                //var models =
        //                //    (from day in dayContext
        //                //     select new ProgramScheduleModel()//tv.ProgramId, tv.Name)
        //                //     {
        //                //         Name = day.Name,
        //                //         GetDate = day.Date
        //                //     });
        //                //bool isAdded;
        //                //foreach (var day in days)
        //                //{
        //                //    isAdded = false;
        //                //    foreach (var model in models)
        //                //    {
        //                //        if (day.Name == model.Name)
        //                //        {
        //                //            isAdded = true;
        //                //            break;
        //                //        }
        //                //    }
        //                //    if (!isAdded)
        //                //    {
        //                //        var newDay = new CodeFirst.Model.Direction()
        //                //        {
        //                //            Name = day.Name,
        //                //            Date = day.GetDate
        //                //        };
        //                //        dayContext.Add(newDay);
        //                //        context.SaveChanges();
        //                //    }
        //                //} 
        //                #endregion
        //                var response =
        //                    this.Request.CreateResponse(HttpStatusCode.OK);
        //                return response;
        //            }
        //        });
        //    return responseMsg;
        //}
        //protected HttpResponseMessage UpdateSchedulePrivate()
        //{
        //    var responseMsg = this.PerformOperationAndHandleExceptions(
        //        () =>
        //        {
        //            var context = new SofiaTransportContext();
        //            using (context)
        //            {
        //                //var dayContext = context.Directions;
        //                var tvPrograms = context.Transport;
        //                var days = context.Day;
        //                #region all
        //                //List<TvProgramModel> programs = new List<TvProgramModel>();
        //                //for (int i = 0; i < 20; i++)
        //                //{
        //                //    programs.Add(allPrograms[i]);
        //                //}
        //                //var allInformationFromDB = (from tv in tvPrograms
        //                //            select new TvProgramModel()//tv.ProgramId, tv.Name)
        //                //            {
        //                //                Name = tv.Name,
        //                //                ProgramId = tv.ProgramId,
        //                //                Directions = (from day in tv.Directions
        //                //                        select new ProgramScheduleModel()
        //                //                        {
        //                //                            Name = day.Name,
        //                //                            GetDate = day.Date,
        //                //                            Stop = (from show in day.Stop
        //                //                                    select new ShowModel()
        //                //                                    {
        //                //                                        Name = show.Name,
        //                //                                        StartAt = show.StarAt
        //                //                                    })
        //                //                       })
        //                //            }); 
        //                //var programs = (from tv in tvPrograms
        //                //                select tv);
        //                //new TvProgramModel()
        //                //{
        //                //});
        //                #endregion
        //                var tvs = (from tv in tvPrograms
        //                           orderby tv.Id
        //                           //where tv.LastUpdatedDate !=null
        //                           select new
        //                           {
        //                               Id = tv.Id,
        //                               lastUpdatedDate = tv.LastUpdatedDate
        //                           }).ToList();
        //                //tvs.Insert(0, new { Id = 0, lastUpdatedDate = 0 });
        //                int lastDayId = days.Count();
        //                var nDays = (from day in days
        //                             orderby day.Id
        //                             //where day.Id > (days.Last().Id - n)
        //                             where day.Id > (lastDayId - 3)
        //                             select new DayModel()
        //                             {
        //                                 Id = day.Id,
        //                                 Date = day.Date
        //                             }).ToList();
        //                //nDays.Insert(0, new DayModel()
        //                //{
        //                //    Id = 0,
        //                //    Date = DateTime.Now
        //                //});
        //                foreach (var tv in tvs)//.Skip(1))
        //                {
        //                    //5 6 7
        //                    //5   7
        //                    //     10  9       8 9 10    10-9 =1  3-1        10 8   10-8=2  3-2
        //                    //    7-5 = 2
        //                    if (tv.lastUpdatedDate < nDays.Last().Id)
        //                    {
        //                        foreach (var day in nDays.Skip(3 - (nDays.Last().Id - tv.lastUpdatedDate)))//(1))
        //                        {
        //                            var thisProgram = tvPrograms.FirstOrDefault(t => t.Id == tv.Id);
        //                            string schedulePage = GetShowsHtml(thisProgram.ProgramId,
        //                                day.GetDateSiteFromat());
        //                            var newShows = GetListOfShows(schedulePage);
        //                            if (newShows == null)
        //                            {
        //                                break;
        //                            }
        //                            var newSchedule = new CodeFirst.Model.Direction()
        //                            {
        //                                Day = days.FirstOrDefault(usr => usr.Id == day.Id)
        //                            };
        //                            foreach (var show in newShows)
        //                            {
        //                                newSchedule.Stops.Add(new CodeFirst.Model.Stop()
        //                                {
        //                                    Name = show.Name,
        //                                    StarAt = show.StartAt
        //                                });
        //                            }
        //                            thisProgram.Days.Add(newSchedule);
        //                            thisProgram.LastUpdatedDate = day.Id;
        //                            context.SaveChanges();
        //                        }
        //                    }
        //                }
        //                //select tv.LastUpdatedDate.Id).ToList();
        //                //int programsCount = tvs.Count;
        //                //for (int programToCheck = 1; programToCheck < programsCount; programToCheck++)
        //                //{
        //                //    if (tvs[programToCheck] < lastDayId)
        //                //    {
        //                //        for (int thisDay = tvs[programToCheck - 1]; thisDay < lastDayId; thisDay++)
        //                //        {
        //                //            var thisProgram = tvPrograms.FirstOrDefault(tv => tv.Id == programToCheck);
        //                //            string schedulePage = GetShowsHtml(thisProgram.ProgramId,
        //                //                nDays[lastDayId - thisDay].GetDateSiteFromat());
        //                //            var newShows = GetListOfShows(schedulePage);
        //                //            if (newShows == null)
        //                //            {
        //                //                break;
        //                //            }
        //                //            var newSchedule = new CodeFirst.Model.Direction()
        //                //            {
        //                //                Day = days.FirstOrDefault(usr => usr.Id == thisDay)
        //                //            };
        //                //            foreach (var show in newShows)
        //                //            {
        //                //                newSchedule.Stop.Add(new CodeFirst.Model.Stop()
        //                //                {
        //                //                    Name = show.Name,
        //                //                    StarAt = show.StartAt
        //                //                    //,
        //                //                    //Transport = thisProgram,
        //                //                    //Day = newSchedule
        //                //                });
        //                //            }
        //                //            thisProgram.Days.Add(newSchedule);
        //                //            thisProgram.LastUpdatedDate = thisDay;
        //                //            context.SaveChanges();
        //                //        }
        //                //    }
        //                //}
        //                //foreach (var lastUpdate in programsLastUpdatedDateId)
        //                //{
        //                //}
        //                //var allInformationFromDB = (from tv in tvPrograms
        //                //                            select new CheckHelperModel()//tv.ProgramId, tv.Name)
        //                //                            {
        //                //                                Id = tv.Id,
        //                //                                ProgramId = tv.ProgramId,
        //                //                                Days = (from day in tv.Days
        //                //                                        where day.Date < DateTime.Now
        //                //                                        select new DataHelperModel()
        //                //                                        {
        //                //                                            GetDate = day.Date,
        //                //                                            Count = day.Stop.Count()
        //                //                                        })
        //                //                            });
        //                //foreach (CheckHelperModel program in allInformationFromDB)
        //                //{
        //                //}
        //                #region asd
        //                //.Where(day => day.Date >DateTime.Now)
        //                //day => day.Date < DateTime.Now)
        //                //select new DataHelperModel()
        //                //{
        //                //    GetDate = day.Date,
        //                //    Count = day.Stop.Count()
        //                //})
        //                //foreach (var item in collection)
        //                //{
        //                //}
        //                //var models =
        //                //    (from day in dayContext
        //                //     select new ProgramScheduleModel()//tv.ProgramId, tv.Name)
        //                //     {
        //                //         Name = day.Name,
        //                //         GetDate = day.Date
        //                //     });
        //                //bool isAdded;
        //                //foreach (var day in days)
        //                //{
        //                //    isAdded = false;
        //                //    foreach (var model in models)
        //                //    {
        //                //        if (day.Name == model.Name)
        //                //        {
        //                //            isAdded = true;
        //                //            break;
        //                //        }
        //                //    }
        //                //    if (!isAdded)
        //                //    {
        //                //        var newDay = new CodeFirst.Model.Direction()
        //                //        {
        //                //            Name = day.Name,
        //                //            Date = day.GetDate
        //                //        };
        //                //        dayContext.Add(newDay);
        //                //        context.SaveChanges();
        //                //    }
        //                //} 
        //                #endregion
        //                var response =
        //                    this.Request.CreateResponse(HttpStatusCode.OK);
        //                return response;
        //            }
        //        });
        //    return responseMsg;
        //}
        //private static void TotatlUpdate(List<TvProgramModel> programs, List<DayModel> dates)
        //{
        //    string dnesTvShow;
        //    var showsForDay = new List<ShowModel>();
        //    foreach (var program in programs)
        //    {
        //        Console.WriteLine(program.Name);
        //        foreach (var day in dates)
        //        {
        //            dnesTvShow = GetShowsHtml(program.ProgramId, day.GetDateSiteFromat());
        //            showsForDay = GetListOfShows(dnesTvShow);
        //            foreach (var show in showsForDay)
        //            {
        //                //dates
        //                //Console.WriteLine("        {0} -  {1}", show.Time, show.Name);
        //            }
        //        }
        //    }
        //}
        //protected string GetMainHtml()
        //{
        //    return Get("http://www.dnes.bg/tv.php");
        //}
        //private static string Get(string url, IDictionary<string, string> headers = null)//, string mediaType = "application/json")
        //{
        //    HttpClient client = new HttpClient();
        //    // "http://www.dnes.bg/tv.php";
        //    var request = new HttpRequestMessage();
        //    request.RequestUri = new Uri(url);
        //    //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        //    request.Method = HttpMethod.Get;
        //    if (headers != null)
        //    {
        //        foreach (var header in headers)
        //        {
        //            request.Headers.Add(header.Key, header.Value);
        //        }
        //    }
        //    var response = client.SendAsync(request).Result;
        //    var contentString = response.Content.ReadAsStringAsync().Result;
        //    return contentString;
        //}
        //protected static string GetShowsHtml(int programId, string dayName)
        //{
        //    return Get("http://www.dnes.bg/tv.php?tv=" + programId + "&date=" + dayName);
        //}
        //protected static List<ShowModel> GetListOfShows(string contentString)
        //{
        //    var startIndexOfSelectTv = contentString.IndexOf("<div class=\"pad bProgram\">");
        //    startIndexOfSelectTv = contentString.IndexOf("info\">") + 6;
        //    var endIndexOfSelectTVs = contentString
        //        .IndexOf("<div class=\"c\"></div>", startIndexOfSelectTv) - 28;
        //    if (endIndexOfSelectTVs < 0)
        //    {
        //        return null;
        //    }
        //    var stringOfAllPrograms = contentString.Substring(
        //        startIndexOfSelectTv,
        //        endIndexOfSelectTVs - startIndexOfSelectTv)
        //        .Replace('\n', ' ')
        //        .Split(new string[] 
        //                { 
        //                    "</div> \t </div> <div class=\"b5 tv_line\">  \t<div class=\"info\">"
        //                },
        //                StringSplitOptions.RemoveEmptyEntries
        //            );
        //    var shows = new List<ShowModel>();
        //    string[] tempString = new string[2];
        //    foreach (var program in stringOfAllPrograms)
        //    {
        //        tempString = program.Split(new string[] { "</div> \t<div class=\"ttl\">" }, StringSplitOptions.None);
        //        shows.Add(new ShowModel()
        //        {
        //            Name = tempString[1],
        //            StartAt = tempString[0]
        //        });
        //    }
        //    return shows;
        //}
        //protected static List<DayModel> GetListOfDays(string contentString)//, string mediaType = "application/json")
        //{
        //    #region old - not used
        //    //var startIndexOfSelectTv = contentString.IndexOf("<select name=\"date\" style=\"font-size:11px\">");// +45;
        //    //startIndexOfSelectTv = contentString.IndexOf("<option value=\"", startIndexOfSelectTv) + 15;// +45;
        //    //var endIndexOfSelectTVs = contentString.IndexOf("</select>", startIndexOfSelectTv) - 3;
        //    //var stringOfAllPrograms = contentString.Substring(startIndexOfSelectTv,
        //    //                                                    endIndexOfSelectTVs - startIndexOfSelectTv)/*.Replace("\\n", "")*/.Replace(" selected", "").Replace("</option>", "").Split(new string[] { "<option value=\"" }, StringSplitOptions.RemoveEmptyEntries);
        //    //var dates = new List<ProgramScheduleModel>();
        //    //string[] tempProg = new string[2];
        //    //foreach (var date in stringOfAllPrograms)
        //    //{
        //    //    tempProg = date.Split(new string[] { "\">" }, StringSplitOptions.None);
        //    //    dates.Add(new ProgramScheduleModel()
        //    //        {
        //    //            Name = tempProg[1],
        //    //            GetDate = DateTime.Parse(tempProg[0])
        //    //        });
        //    //} 
        //    #endregion
        //    var startIndexOfSelectTv = contentString.IndexOf("<select name=\"date\" style=\"font-size:11px\">");// +45;
        //    startIndexOfSelectTv = contentString.IndexOf("<option value=\"", startIndexOfSelectTv) + 15;
        //    var endIndexOfSelectTVs = contentString.IndexOf("</select>", startIndexOfSelectTv) - 3;
        //    var stringOfAllPrograms = contentString.Substring(
        //        startIndexOfSelectTv,
        //        endIndexOfSelectTVs - startIndexOfSelectTv)/*.Replace("\\n", "")*/
        //        .Replace(" selected", "")
        //        .Replace("</option>", "")
        //        .Split(new string[] { "<option value=\"" }, StringSplitOptions.RemoveEmptyEntries);
        //    var dates = new List<DayModel>();
        //    //dates.Add(new DayModel()
        //    //{
        //    //    Name = "DB-LastDayInitiol",
        //    //    Date = DateTime.Now
        //    //});
        //    string[] tempProg = new string[2];
        //    foreach (var date in stringOfAllPrograms)
        //    {
        //        tempProg = date.Split(new string[] { "\">" }, StringSplitOptions.None);
        //        dates.Add(new DayModel()
        //        {
        //            Name = tempProg[1],
        //            Date = DateTime.Parse(tempProg[0])
        //        });
        //    }
        //    return dates;
        //}
        //protected static List<TvProgramModel> GetListOfPrograms(string contentString)
        //{
        //    var startIndexOfSelectTv = contentString.IndexOf("Всички</option>") + 17;
        //    var endIndexOfSelectTVs = contentString.IndexOf("</select>", startIndexOfSelectTv) - 3;
        //    var stringOfAllPrograms = contentString.Substring(startIndexOfSelectTv, endIndexOfSelectTVs - startIndexOfSelectTv)
        //        .Replace("\\n", "")
        //        .Split(new string[] { "<option value=\"" }, StringSplitOptions.RemoveEmptyEntries);
        //    var programs = new List<TvProgramModel>();
        //    //programs.Add(new TvProgramModel()
        //    //       {
        //    //           ProgramId = 0,
        //    //           Name = "nullProgramName"
        //    //       });
        //    string[] tempProg = new string[2];
        //    foreach (var program in stringOfAllPrograms)
        //    {
        //        tempProg = program.Split(new string[] { "\">" }, StringSplitOptions.None);
        //        programs.Add(new TvProgramModel()
        //        {
        //            ProgramId = int.Parse(tempProg[0]),
        //            Name = tempProg[1]
        //        });
        //    }
        //    return programs;
        //}
    }


}