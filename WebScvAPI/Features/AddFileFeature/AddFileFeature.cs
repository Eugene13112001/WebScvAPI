using Microsoft.Extensions.Logging;
using WebScvAPI.Containers;
using WebScvAPI.Models;
using FluentValidation;
using MediatR;
namespace WebScvAPI.Features.AddFileFeature
{
    public class AddModelCommand : IRequest<CsvModel>
    {


        public IFormFileCollection file { get; set; }

        public  string Name { get; set; }



        public class AddModelCommandHandler : IRequestHandler<AddModelCommand, CsvModel>
        {
            private readonly ICSVServiceReader reder;
            private readonly ICSVServiceWriter writer;
            public readonly ICSVServiceData data;

            public AddModelCommandHandler(ICSVServiceReader reder, ICSVServiceWriter writer, ICSVServiceData data)
            {
                this.reder = reder ?? throw new ArgumentNullException(nameof(reder));
                this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
                this.data = data ?? throw new ArgumentNullException(nameof(data));
            }

            public async Task<CsvModel> Handle(AddModelCommand command, CancellationToken cancellationToken)
            {
                CsvModel model = await  reder.ReadCSV(command.file[0].OpenReadStream(), command.Name, command.Name);
                model.Path = await writer.WriteCSV(command.file[0].OpenReadStream(), model.Path);
                return  await data.Create(model);
                
            }


        }
        public class AddProductCommandValidator : AbstractValidator<AddModelCommand>
        {
            public AddProductCommandValidator()
            {

                RuleFor(c => c.Name).NotEmpty().WithMessage("Name: Имя не существет");
                RuleFor(c => c.file).NotEmpty().WithMessage("File: Файла не существет");


            }

            
        }

    }
}
