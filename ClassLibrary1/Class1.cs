using WebScvAPI.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebScvAPI.Models;
using MediatR;
using WebScvAPI.Features.GetAllFeature;
using MongoDB.Driver;
using WebScvAPI.Features.GetByIdFeature;
using WebScvAPI.Features.DeleteFeature;
using WebScvAPI.Features.AddFileFeature;
using WebScvAPI.Features.ChangeFeature;

namespace WebCSVAPI.Unittests
{
    public class HomeControllerTests
    {
        [Fact]
        public async void GettallTest()
        {
            var mock = new Mock<IMediator>();


            mock.Setup(repo => repo.Send(It.IsAny<GetAllCommand>(), It.IsAny<CancellationToken>())).
                ReturnsAsync(await GetTestUsers());

            var controller = new WeatherForecastController(mock.Object);

            var result = await controller.GetAll();

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var jsonResult = Assert.IsType<JsonResult>(viewResult.Value);
            var model = Assert.IsAssignableFrom<IEnumerable<CsvModel>>(jsonResult.Value);
            Assert.Equal(1, model.Count());
        }

        [Fact]
        public async void GetByIndexTest()
        {
            var mock = new Mock<IMediator>();

            GetByIdCommand command = new GetByIdCommand();
            command.Id = "1";
            mock.Setup(repo => repo.Send(It.IsAny<GetByIdCommand>(), It.IsAny<CancellationToken>())).
                ReturnsAsync(await GetUser());

            var controller = new WeatherForecastController(mock.Object);

            var result = await controller.GetById(command.Id);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var jsonResult = Assert.IsType<JsonResult>(viewResult.Value);
            var model = Assert.IsAssignableFrom<CsvModel>(jsonResult.Value);
            Assert.Equal(command.Id, model.Id);
        }
        [Fact]
        public async void AddModelTest()
        {
            var mock = new Mock<IMediator>();

            AddModelCommand command = new AddModelCommand();
            CsvModel mod = new CsvModel { Id = "1" };
            mock.Setup(repo => repo.Send(It.IsAny<AddModelCommand>(), It.IsAny<CancellationToken>())).
                ReturnsAsync(mod);

            var controller = new WeatherForecastController(mock.Object);

            var result = await controller.Add(command, new CancellationToken());

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var jsonResult = Assert.IsType<JsonResult>(viewResult.Value);
            var model = Assert.IsAssignableFrom<CsvModel>(jsonResult.Value);
            Assert.Equal(mod.Id, model.Id);
        }
        [Fact]
        public async void ChangeModelTest()
        {
            var mock = new Mock<IMediator>();

            ChangeModelCommand command = new ChangeModelCommand();
            CsvModel mod = new CsvModel { Id = "1" };
            mock.Setup(repo => repo.Send(It.IsAny<ChangeModelCommand>(), It.IsAny<CancellationToken>())).
                ReturnsAsync(mod);

            var controller = new WeatherForecastController(mock.Object);

            var result = await controller.Change(command, new CancellationToken());

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var jsonResult = Assert.IsType<JsonResult>(viewResult.Value);
            var model = Assert.IsAssignableFrom<CsvModel>(jsonResult.Value);
            Assert.Equal(mod.Id, model.Id);
        }
        [Fact]
        public async void DeleteTest()
        {
            var mock = new Mock<IMediator>();

            DeleteModelCommand command = new DeleteModelCommand();
            command.Id = "1";
            var res = true;
            mock.Setup(repo => repo.Send(command, It.IsAny<CancellationToken>())).
                ReturnsAsync(res);

            var controller = new WeatherForecastController(mock.Object);

            var result = await controller.Delete(command, new CancellationToken());



            var viewResult = Assert.IsType<OkObjectResult>(result);
            var jsonResult = Assert.IsType<JsonResult>(viewResult.Value);
            var model = Assert.IsAssignableFrom<bool>(jsonResult.Value);
            Assert.Equal(res, model);
        }

        private async Task<List<CsvModel>> GetTestUsers()
        {
            var users = new List<CsvModel>
            {
                new CsvModel{}

            };
            return users;
        }
        private async Task<CsvModel> GetUser()
        {

            return new CsvModel { Id = "1" };
        }

    }
}