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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebScvAPI.Features.FiltrAndSortFeature;
using Microsoft.VisualBasic.FileIO;
using WebScvAPI.Filtrs;
namespace WebScvAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [TypeFilter(typeof(SampleExceptionFilter))]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// �������� ���������� � ���� ������ � �� �������� 
        /// </summary>
        ///  <remarks>
        /// ����� ������ �� ���������
        /// 
        /// ����� ���������� ������� � ����� value, � ������� �������� ������ � ����������� � ������ � �� �������� 
        /// 
        /// </remarks>
        
        [AllowAnonymous]
        [HttpGet]
        [Route("api/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            GetAllCommand client = new GetAllCommand();
            CancellationToken token = new CancellationToken();
            List<CsvModel> ev = await _mediator.Send(client, token);
            return Ok(new JsonResult(ev));
        }
        /// <summary>
        /// �������� ���������� � ����� �� ��� id
        /// </summary>
        ///  <remarks>
        /// ����� ��������� id - id ����� � ���� ������
        /// 
        /// ����� ����������  ���������� � ����� � ��� ��������, ���� �� ����
        /// 
        /// ���� ���, �� ���������� ��� 400 � ����������� � ������
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            GetByIdCommand client = new GetByIdCommand();
            CancellationToken token = new CancellationToken();
            client.Id = id;
            CsvModel ev = await _mediator.Send(client, token);
            return Ok(new JsonResult(ev));
        }
        /// <summary>
        /// ������� ���������� � ����� � ��� ���� �� ��� id
        /// </summary>
        ///  <remarks>
        /// ����� ��������� id - id ����� � ���� ������
        /// 
        /// ����� ���������� true � ������ ������ ��������
        /// 
        /// ���� �������� ���������, �� ���������� ��� 400 � ����������� � ������
        /// </remarks>
        [HttpDelete]
        [Route("api/Delete")]
        public async Task<IActionResult> Delete([FromForm] DeleteModelCommand client,
          CancellationToken token)
        {

            bool ev = await _mediator.Send(client, token);
            return Ok(new JsonResult(ev));
        }
        /// <summary>
        /// �������� ����� ����
        /// </summary>
        ///  <remarks>
        /// ����� ��������� 2 ���������:
        /// 
        /// Name - ��� �����
        /// 
        /// file - ���� � ������� csv
        /// 
        /// ����� ���������� ���������� � ����������� ����� � ��� ��������, ���� �������� �������
        /// 
        /// ���� �������� ���������, �� ���������� ��� 400 � ����������� � ������
        /// </remarks>
        [HttpPost]
        [Route("api/Add")]
        public async Task<IActionResult> Add([FromForm] AddModelCommand client,
           CancellationToken token)
        {          
            CsvModel ev = await _mediator.Send(client, token);
            return Ok(new JsonResult(ev ));
        }
        /// <summary>
        /// ����������� �(���) �����������  ����
        /// </summary>
        ///  <remarks>
        /// ����� ��������� 4 ���������:
        /// 
        /// Id - id �����
        /// 
        /// sort - ������� ��� ���������� ( ���� ���������� �� �����,  �� ���������� ������ �������)
        /// 
        /// � ���� ������� ����� - �������� �������� � �����, ������� ��������� � ����������
        /// 
        /// � ����� - �������� ASC (�� �����������) ��� DESC (�� ��������) ��� �����
        /// 
        /// filtrnumber - ������� ��� ���������� �������� ����� ( ���� ���������� �� �����,  �� ���������� ������ �������)
        /// 
        /// � ���� ������� ����� - �������� �������� � ��������� ���������� � �����, ������� ��������� � ���������� 
        /// 
        /// � ����� - ������� � ����� ���������� ��� ������� ����������: ������ - ��������, ������ ��������
        /// 
        /// filtrsting - ������� ��� ���������� �������� ����� ( ���� ���������� �� �����,  �� ���������� ������ �������)
        /// 
        /// � ���� ������� ����� - �������� �������� � ��������� ���������� � �����, ������� ��������� � ���������� 
        /// 
        /// � ����� - ���������� ���������, ������� ������ ��������������� ����
        /// 
        /// 
        /// ����� ���������� ��������������� � ��������������� ����, ���� �������� �������
        /// 
        /// ���� �������� ���������, �� ���������� ��� 400 � ����������� � ������
        /// </remarks>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/FiltrAndSort")]
        public async Task<IActionResult> FiltrAndSort([FromBody] FiltrAndSortCommand client,
           CancellationToken token)
        {
            CSVFile ev = await _mediator.Send(client, token);
            return File(ev.Finaldata, ev.File_type, ev.File_name);
        }
        /// <summary>
        /// �������� ����
        /// </summary>
        ///  <remarks>
        /// ����� ��������� 3 ���������:
        /// 
        /// Id - id �����
        /// 
        /// Name - ��� �����
        /// 
        /// file - ���� � ������� csv
        /// 
        /// ����� ���������� ���������� � �����������  ����� � ��� ��������, ���� �������� �������
        /// 
        /// ���� �������� ���������, �� ���������� ��� 400 � ����������� � ������
        /// </remarks>
        [HttpPut]
        [Route("api/Change")]
        public async Task<IActionResult> Change([FromForm] ChangeModelCommand client,
           CancellationToken token)
        {
            CsvModel ev = await _mediator.Send(client, token);
            return Ok(new JsonResult(ev));
        }
    }
}