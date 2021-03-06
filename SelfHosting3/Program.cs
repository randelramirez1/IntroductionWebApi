﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.Http;
using System.Net.Http;

namespace SelfHosting3
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8999");
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // using asp.net like hosting
            // HttpSelfHostServer is using a different constructor compared to the previous project (SelfHost1)
            var server = new HttpSelfHostServer(config);
            var task = server.OpenAsync();
            task.Wait();
            Console.WriteLine("Server is up and running");
            Console.WriteLine("Hit enter to call server with client");
            Console.ReadLine();

            // HttpSelfHostServer, derives from HttpMessageHandler => multiple level of inheritance
            var client = new HttpClient(server);
            client.GetAsync("http://localhost:8999/api/my").ContinueWith(t =>
            {
                var result = t.Result;
                result.Content.ReadAsStringAsync()
                    .ContinueWith(rt =>
                    {
                        Console.WriteLine("client got response" + rt.Result);
                    });
            });

            Console.ReadLine();
        }
    }
}
