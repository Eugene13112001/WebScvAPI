using FluentValidation;
using MediatR;
using MongoDB.Bson;
using WebScvAPI.Containers;
using WebScvAPI.Models;

namespace WebScvAPI.Features.GetByIdFeature
{
    public class GetByIdCommand : IRequest<CsvModel>
    {



        public string Id { get; set; }



        public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, CsvModel>
        {
            private readonly ICSVServiceReader reder;
            private readonly ICSVServiceWriter writer;
            public readonly ICSVServiceData data;

            public GetByIdCommandHandler(ICSVServiceReader reder, ICSVServiceWriter writer, ICSVServiceData data)
            {
                this.reder = reder ?? throw new ArgumentNullException(nameof(reder));
                this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
                this.data = data ?? throw new ArgumentNullException(nameof(data));
            }

            public async Task<CsvModel> Handle(GetByIdCommand command, CancellationToken cancellationToken)
            {

                CsvModel? model =  await data.GetById(command.Id);
                if (model is null) throw new Exception("Такого файла нет");
                
                return model;

            }


        }
        public class GetByIdCommandValidator : AbstractValidator<GetByIdCommand>
        {
            public GetByIdCommandValidator()
            {

                RuleFor(c => c.Id).NotEmpty().WithMessage("Id: Id не существет");



            }


        }

    }
}
