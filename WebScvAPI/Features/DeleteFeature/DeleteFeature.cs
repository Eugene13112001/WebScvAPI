using FluentValidation;
using MediatR;
using MongoDB.Bson;
using WebScvAPI.Containers;
using WebScvAPI.Models;

namespace WebScvAPI.Features.DeleteFeature
{
    public class DeleteModelCommand : IRequest<bool>
    {


        

        public string Id { get; set; }



        public class DeleteModelCommandHandler : IRequestHandler<DeleteModelCommand, bool>
        {
            private readonly ICSVServiceReader reder;
            private readonly ICSVServiceWriter writer;
            public readonly ICSVServiceData data;

            public DeleteModelCommandHandler(ICSVServiceReader reder, ICSVServiceWriter writer, ICSVServiceData data)
            {
                this.reder = reder ?? throw new ArgumentNullException(nameof(reder));
                this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
                this.data = data ?? throw new ArgumentNullException(nameof(data));
            }

            public async Task<bool> Handle(DeleteModelCommand command, CancellationToken cancellationToken)
            {
                
                CsvModel? model =  await data.GetById(command.Id);
                if (model is null) throw new Exception("Такого файла нет");
                File.Delete(model.Path);
                return await data.Remove(model.Id);

            }


        }
        public class DeleteProductCommandValidator : AbstractValidator<DeleteModelCommand>
        {
            public DeleteProductCommandValidator()
            {

                RuleFor(c => c.Id).NotEmpty().WithMessage("Id: Id не существет");
               


            }


        }

    }
}
