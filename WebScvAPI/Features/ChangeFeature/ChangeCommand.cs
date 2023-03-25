using FluentValidation;
using MediatR;
using MongoDB.Bson;
using WebScvAPI.Containers;
using WebScvAPI.Models;

namespace WebScvAPI.Features.ChangeFeature
{
    public class ChangeModelCommand : IRequest<CsvModel>
    {
        public string Id { get; set; }

        public IFormFileCollection file { get; set; }

        public string Name { get; set; }



        public class ChangeModelCommandHandler : IRequestHandler<ChangeModelCommand, CsvModel>
        {
            private readonly ICSVServiceReader reder;
            private readonly ICSVServiceWriter writer;
            public readonly ICSVServiceData data;

            public ChangeModelCommandHandler(ICSVServiceReader reder, ICSVServiceWriter writer, ICSVServiceData data)
            {
                this.reder = reder ?? throw new ArgumentNullException(nameof(reder));
                this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
                this.data = data ?? throw new ArgumentNullException(nameof(data));
            }

            public async Task<CsvModel> Handle(ChangeModelCommand command, CancellationToken cancellationToken)
            {
                CsvModel? model = await this.data.GetById(command.Id);
                if (model is null) throw new Exception("Такого файла не существует");
                CsvModel newmodel = await reder.ReadCSV(command.file[0].OpenReadStream(), command.Name, command.Name);
                newmodel.Path = await writer.WriteCSV(command.file[0].OpenReadStream(), newmodel.Path);
                return await data.Update(model.Id, newmodel);

            }


        }
        public class ChangeModelCommandValidator : AbstractValidator<ChangeModelCommand>
        {
            public ChangeModelCommandValidator()
            {
                RuleFor(c => c.Id).NotEmpty().WithMessage("Id: Id не существет");
                RuleFor(c => c.Name).NotEmpty().WithMessage("Name: Имя не существет");
                RuleFor(c => c.file).NotEmpty().WithMessage("File: Файла не существет");


            }


        }

    }
}
