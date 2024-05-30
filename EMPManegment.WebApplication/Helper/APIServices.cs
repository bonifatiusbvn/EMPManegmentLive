﻿using EMPManagment.Web.Models.API;
using EMPManegment.Web.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace EMPManagment.Web.Helper
{
    public class APIServices
    {
        public APIServices(WebAPI webAPI, IWebHostEnvironment environment, UserSession userSession, IHttpContextAccessor httpContext)
        {
            WebAPI = webAPI;
            Environment = environment;
            UserSession = userSession;
            HttpContext = httpContext;
            if (HttpContext.HttpContext != null && HttpContext.HttpContext.User.Identity.IsAuthenticated)
                Token = UserSession.AccessToken;
        }
        private string Token { get; set; }

        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public UserSession UserSession { get; }
        public IHttpContextAccessor HttpContext { get; }

        public async Task<ApiResponseModel> GetAsync(dynamic id, string endpoint)
        {
            var model = new ApiResponseModel();

            try
            {
                HttpClient clients = WebAPI.Initil();
                var url = $"{clients.BaseAddress}{endpoint}";

                if (id != null)
                    url = $"{url}{id}";

                using var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var client = new HttpClient(httpClientHandler)
                {
                    Timeout = TimeSpan.FromMinutes(80)
                };

                if (!string.IsNullOrWhiteSpace(Token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<object>(responseContent);
                model = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);
                model.code = (int)response.StatusCode;
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ApiResponseModel> GetAsyncId(dynamic id, string endpoint)
        {
            var model = new ApiResponseModel();

            try
            {
                HttpClient clients = WebAPI.Initil();
                var url = $"{clients.BaseAddress}{endpoint}";

                if (id != null)
                    url = $"{url}?id={id}";

                using var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var client = new HttpClient(httpClientHandler)
                {
                    Timeout = TimeSpan.FromMinutes(80)
                };

                if (!string.IsNullOrWhiteSpace(Token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<object>(responseContent);
                model = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);
                model.code = (int)response.StatusCode;
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ApiResponseModel> PostAsync(dynamic input, string endpoint)
        {
            var model = new ApiResponseModel();

            try
            {

                StringContent data = new StringContent("");
                if (input != null)
                {
                    var json = JsonConvert.SerializeObject(input);
                    data = new StringContent(json, Encoding.UTF8, "application/json");
                }

                HttpClient clients = WebAPI.APIUrl();
                var url = $"{clients.BaseAddress}/{endpoint}";
                using var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var client = new HttpClient(httpClientHandler)
                {
                    Timeout = TimeSpan.FromMinutes(80)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var response = await client.PostAsync(url, data);
                var responseContent = await response.Content.ReadAsStringAsync();

                model = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);
                model.code = (int)response.StatusCode;


                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
