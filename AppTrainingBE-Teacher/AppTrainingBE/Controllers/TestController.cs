using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppTrainingBE.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpGet]
        [Route("Diccionarios")]
        public IActionResult GetDiccionarios()
        {
            List<object> listPer = new();

            Dictionary<string, object> dicRes = new Dictionary<string, object>();
            dicRes.Add("Nombre", "Juan Perez");
            dicRes.Add("Edad", 20);
            dicRes.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
            
            listPer.Add(dicRes);

            dicRes = new Dictionary<string, object>();
            dicRes.Add("Nombre", "Maria Lopez");
            dicRes.Add("Edad", 23);
            dicRes.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));

            return Ok(listPer);
        }
    }
}
