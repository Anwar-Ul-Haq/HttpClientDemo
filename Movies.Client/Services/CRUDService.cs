﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.Services
{
    public class CRUDService : IIntegrationService
    {
        private static HttpClient _httpClient = new HttpClient();

        public CRUDService()
        {
            _httpClient.BaseAddress =  new Uri("http://localhost:57863");

            _httpClient.Timeout = new TimeSpan(0,0,30);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json" ));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml" , 0.9));






        }

        public async Task Run()
        {
            await GetResource();
        }


        public async Task GetResource()
        {
            var response = await _httpClient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();

          var content = await  response.Content.ReadAsStringAsync();
          var movies = new List<Movie>();
            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                movies= JsonConvert.DeserializeObject<List<Movie>>(content);
            }

            else if (response.Content.Headers.ContentType.MediaType == "application/xml")
            {
                var serializer = new XmlSerializer(typeof(List<Movie>));
                movies = (List<Movie>) serializer.Deserialize(new StringReader(content));
            }


        }



    }
}
