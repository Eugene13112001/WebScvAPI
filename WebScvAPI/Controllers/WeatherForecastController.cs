using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using WebScvAPI.Features.AddFileFeature;
using WebScvAPI.Features.ChangeFeature;
using WebScvAPI.Features.DeleteFeature;
using WebScvAPI.Features.GetAllFeature;
using WebScvAPI.Features.GetByIdFeature;
using WebScvAPI.Models;
namespace WebScvAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [HttpGet]
        [Route("api/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            GetAllCommand client = new GetAllCommand();
            CancellationToken token = new CancellationToken();
            List<CsvModel> ev = await _mediator.Send(client, token);
            return new JsonResult(ev);
        }
        [HttpGet]
        [Route("api/GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            GetByIdCommand client = new GetByIdCommand();
            CancellationToken token = new CancellationToken();
            client.Id = id;
            CsvModel ev = await _mediator.Send(client, token);
            return new JsonResult(ev);
        }
        [HttpDelete]
        [Route("api/Delete")]
        public async Task<IActionResult> Delete([FromForm] DeleteModelCommand client,
          CancellationToken token)
        {

            bool ev = await _mediator.Send(client, token);
            return new JsonResult(ev);
        }
        [HttpPost]
        [Route("api/Add")]
        public async Task<IActionResult> Add([FromForm] AddModelCommand client,
           CancellationToken token)
        {
           
            CsvModel ev = await _mediator.Send(client, token);
            return new JsonResult(ev );
        }
        [HttpPut]
        [Route("api/Change")]
        public async Task<IActionResult> Change([FromForm] ChangeModelCommand client,
           CancellationToken token)
        {

            CsvModel ev = await _mediator.Send(client, token);
            return new JsonResult(ev);
        }
    }
}