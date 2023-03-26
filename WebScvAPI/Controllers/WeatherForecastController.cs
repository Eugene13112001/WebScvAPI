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
        /// Получить информацию о всех файлах и их столбцах 
        /// </summary>
        ///  <remarks>
        /// Метод ничего не принимает
        /// 
        /// Метод возвращает словарь с полем value, в коротом хранится список с информацией о файлах и их столбцах 
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
        /// Получить информацию о файле по его id
        /// </summary>
        ///  <remarks>
        /// Метод принимает id - id файла в базе данных
        /// 
        /// Метод возвращает  информацию о файле и его столбцах, если он есть
        /// 
        /// Если нет, то возвращает код 400 с информацией о ошибке
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
        /// Удалить информацию о файле и сам файл по его id
        /// </summary>
        ///  <remarks>
        /// Метод принимает id - id файла в базе данных
        /// 
        /// Метод возвращает true в случае успеха операции
        /// 
        /// Если операция неуспешна, то возвращает код 400 с информацией о ошибке
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
        /// Добавить новый файл
        /// </summary>
        ///  <remarks>
        /// Метод принимает 2 параметра:
        /// 
        /// Name - имя файла
        /// 
        /// file - файл в формате csv
        /// 
        /// Метод возвращает информацию о добавленном файле и его столбцах, если операция успешна
        /// 
        /// Если операция неуспешна, то возвращает код 400 с информацией о ошибке
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
        /// Фильтровать и(или) сортировать  файл
        /// </summary>
        ///  <remarks>
        /// Метод принимает 4 параметра:
        /// 
        /// Id - id файла
        /// 
        /// sort - словарь для сортировки ( если сортировка не нужна,  то посылается пустой словарь)
        /// 
        /// В этом словаре ключи - название столбцов в файле, которые участвуют в сортировке
        /// 
        /// А ключи - значения ASC (по позрастанию) или DESC (по убыванию) для полей
        /// 
        /// filtrnumber - словарь для фильтрации числовых полей ( если фильтрация не нужна,  то посылается пустой словарь)
        /// 
        /// В этом словаре ключи - название столбцов с числовыми значениями в файле, которые участвуют в фильтрации 
        /// 
        /// А ключи - массивы с двумя значениями для задания промежутка: первое - минимиум, второе максимум
        /// 
        /// filtrsting - словарь для фильтрации строчных полей ( если фильтрация не нужна,  то посылается пустой словарь)
        /// 
        /// В этом словаре ключи - название столбцов с строчными значениями в файле, которые участвуют в фильтрации 
        /// 
        /// А ключи - регулярные выражения, которым должны соответствовать поля
        /// 
        /// 
        /// Метод возвращает отсортированный и отфильтрованный файл, если операция успешна
        /// 
        /// Если операция неуспешна, то возвращает код 400 с информацией о ошибке
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
        /// Изменить файл
        /// </summary>
        ///  <remarks>
        /// Метод принимает 3 параметра:
        /// 
        /// Id - id файла
        /// 
        /// Name - имя файла
        /// 
        /// file - файл в формате csv
        /// 
        /// Метод возвращает информацию о обновленном  файле и его столбцах, если операция успешна
        /// 
        /// Если операция неуспешна, то возвращает код 400 с информацией о ошибке
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