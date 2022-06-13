using ConsumindoApiReservas.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsumindoApiReservas.Controllers
{
    public class ReservasController : Controller
    {

        private readonly string apiUrl = "https://localhost:44375/api/reservas";
        public async Task<IActionResult> Index()
        {
            List<Reserva> listaReservas = new List<Reserva>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listaReservas = JsonConvert.DeserializeObject<List<Reserva>>(apiResponse);
                }
            }
            return View(listaReservas);
        }



        public IActionResult GetReserva()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetReserva(int id)
        {
            Reserva reserva = new Reserva();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reserva = JsonConvert.DeserializeObject<Reserva>(apiResponse);
                }
            }
            return View(reserva);
        }



        public IActionResult AddReserva()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddReserva(Reserva reserva)
        {
            Reserva reservaRecebida = new Reserva();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Key", "teste@2022");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reserva),
                                                 Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(apiUrl, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse.Contains("401"))
                    {
                        ViewBag.Result = apiResponse;
                        return View();
                    }
                    else
                    {
                        reservaRecebida = JsonConvert.DeserializeObject<Reserva>(apiResponse);
                    }
                }
            }
            return View(reservaRecebida);
        }



        [HttpGet]
        public async Task<IActionResult> UpdateReserva(int id)
        {
            Reserva reserva = new Reserva();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reserva = JsonConvert.DeserializeObject<Reserva>(apiResponse);
                }
            }
            return View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReserva(Reserva reserva)
        {
            Reserva reservaRecebida = new Reserva();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Key", "teste@2022");

                var content = new MultipartFormDataContent();

                content.Add(new StringContent(reserva.ReservaId.ToString()), "ReservaId");
                content.Add(new StringContent(reserva.Nome), "Nome");
                content.Add(new StringContent(reserva.InicioLocacao), "InicioLocacao");
                content.Add(new StringContent(reserva.FimLocacao), "FimLocacao");

                using (var response = await httpClient.PutAsync(apiUrl, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse.Contains("401"))
                    {
                        ViewBag.Result = apiResponse;
                        return View();
                    }
                    else
                    {
                        ViewBag.Result = "Reserva atualizada com Sucesso";
                        reservaRecebida = JsonConvert.DeserializeObject<Reserva>(apiResponse);
                    }
                }
            }
            return View(reservaRecebida);
        }




        [HttpPost]
        public async Task<IActionResult> DeleteReserva(int ReservaId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(apiUrl + "/" + ReservaId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
