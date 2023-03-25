using FluentValidation;
using MediatR;
using MongoDB.Bson;
using WebScvAPI.Containers;
using WebScvAPI.Models;

namespace WebScvAPI.Features.GetAllFeature
{
    public class GetAllCommand : IRequest<List<CsvModel>>
    {



        public class DeleteModelCommandHandler : IRequestHandler<GetAllCommand, List<CsvModel>>
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

            public async Task<List<CsvModel>> Handle(GetAllCommand command, CancellationToken cancellationToken)
            {

                
                return await data.Get();

            }


        }
        

    }
}
