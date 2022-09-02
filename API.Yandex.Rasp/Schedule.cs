using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpUtils;
using Newtonsoft.Json.Linq;
using System.Globalization;
namespace API.Yandex.Rasp
{
    public class Auth
    // Here we can pass the uid and api key 
    // uid is needed to check the stations on route and to check what is the next station
    // Сюда мы передаем ключ от api и uid
    // uid используется чтобы проверить станции по маршруту и проверить какая следующая станция
    {
        public string uid = "";
        public string key;
    }

    public struct Schedule
    {
        public string depart;
        public string title;
        public string arrival;
    };

    public struct Station
    {
        public string title;
        public string code;
    };
    public class Train
    {
        public static List<Schedule> station(DateTime date, string station, Auth auth)
        //This is the json parcer so it basicall gets an answer from the api and gets the needed data
        {
            //Лист для возврата расписания
            //A list to return the schedule in list
            string date_s = date.ToString("yyyy-MM-dd");
            List<Schedule> schedule = new List<Schedule>();
            string api_point = $"https://api.rasp.yandex.net/v3.0/schedule/?apikey={auth.key}&station={station}&transport_types=suburban&date={date_s}";
            var client = new RestClient();

            client.EndPoint = api_point;
            client.Method = HttpVerb.GET;
            client.ContentType = "text/xml";
            //Sending a request
            //Отправляем запрос
            var json = client.MakeRequest();
            //Parcing the answer
            //Парсим ответ
            dynamic responce = JObject.Parse(json);
            int length = 98;
            try
            {
                //We are collecting all the recordings and move them to list
                //В цикле собираем все записи и переносим их в вид списка
                for (int i = 0; i < length; i++)
                {
                    var route = new Schedule();
                    if (responce.schedule[i].direction == "прибытие") { route.arrival = responce.schedule[i].arrival; }
                    else { route.depart = responce.schedule[i].departure; }
                    schedule.Add(route);
                }
                return schedule;
            }
            catch (Exception ex)
            {
                return schedule;
            }
        }
        public static List<Schedule> route(DateTime date, string from, string to, Auth auth)
        {
            //Лист для возврата расписания
            List<Schedule> Route_list = new List<Schedule>();
            string date_s = date.ToString("yyyy-MM-dd");
            //Адрес к которому отправляем запрос
            string api_point = $"https://api.rasp.yandex.net/v3.0/search/?apikey={auth.key}&format=json&from={from}&to={to}&lang=ru_RU&page=1&date={date_s}";
            //Создаем новый клиент Rest
            var client = new RestClient();
            //Schedule of route
            //Настройки клиента 
            client.EndPoint = api_point;
            client.Method = HttpVerb.GET;
            client.ContentType = "text/xml";

            //Отправляем запрос
            var json = client.MakeRequest();
            //Парсим ответ
            dynamic responce = JObject.Parse(json);
            int length = responce.ToString().Length;
            //auth.Uid = stuff.uid.ToString();
            auth.uid = responce.segments[0].thread.uid;
            try
            {
                //В цикле собираем все записи и переносим их в вид списка
                for (int i = 0; i < length; i++)
                {
                    var route = new Schedule();
                    route.depart = responce.segments[i].departure;
                    route.arrival = responce.segments[i].arrival;
                    route.title = responce.segments[i].thread.short_title;
                    Route_list.Add(route);
                }
                return Route_list;

            }
            catch (Exception ex)
            {
                return Route_list;
            }
        }
        public static List<Station> stationID(Auth auth)
        {
            //Лист для возврата расписания
            List<Station> Station_list = new List<Station>();
            //Адрес к которому отправляем запрос
            string api_point = $"https://api.rasp.yandex.net/v3.0/thread/?apikey={auth.key}&format=json&uid={auth.uid}&lang=ru_RU&show_systems=all";
            //Создаем новый клиент Rest
            var client = new RestClient();
            //Schedule of route
            //Настройки клиента 
            client.EndPoint = api_point;
            client.Method = HttpVerb.GET;
            client.ContentType = "text/xml";

            //Отправляем запрос
            var json = client.MakeRequest();
            //Парсим ответ
            dynamic responce = JObject.Parse(json);
            int length = responce.ToString().Length;
            try
            {

                for (int i = 0; i < length; i++)
                {
                    var station = new Station();
                    station.code = responce.stops[i].station.code;
                    station.title = responce.stops[i].station.title;
                    Station_list.Add(station);
                }
                return Station_list;
            }
            catch (Exception ex)
            {
                return Station_list;
            }
        }
    }
    //TODO: Сделать нормальный класс для работы с автобусами
    public class get_bus 
    {
        public static List<Schedule> stop(DateTime date, string station, Auth auth)
        //This is the json parcer so it basicall gets an answer from the api and gets the needed data
        {
            //Лист для возврата расписания
            //A list to return the schedule in list
            string date_s = date.ToString("yyyy-MM-dd");
            List<Schedule> schedule = new List<Schedule>();
            string api_point = $"https://api.rasp.yandex.net/v3.0/schedule/?apikey={auth.key}&station={station}&transport_types=suburban&date={date_s}";
            var client = new RestClient();

            client.EndPoint = api_point;
            client.Method = HttpVerb.GET;
            client.ContentType = "text/xml";
            //Sending a request
            //Отправляем запрос
            var json = client.MakeRequest();
            //Parcing the answer
            //Парсим ответ
            dynamic responce = JObject.Parse(json);
            int length = 98;
            try
            {
                //We are collecting all the recordings and move them to list
                //В цикле собираем все записи и переносим их в вид списка
                for (int i = 0; i < length; i++)
                {
                    var route = new Schedule();
                    if (responce.schedule[i].direction == "прибытие") { route.arrival = responce.schedule[i].arrival; }
                    else { route.depart = responce.schedule[i].departure; }
                    schedule.Add(route);
                }
                return schedule;
            }
            catch (Exception ex)
            {
                return schedule;
            }
        }
    }
}
