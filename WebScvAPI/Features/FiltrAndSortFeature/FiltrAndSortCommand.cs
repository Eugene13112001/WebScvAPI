using FluentValidation;
using MediatR;
using WebScvAPI.Containers;
using WebScvAPI.Models;

namespace WebScvAPI.Features.FiltrAndSortFeature
{
    public class FiltrAndSortCommand : IRequest<CSVFile>
    {


        public string  Id { get; set; }

        public Dictionary<string, string> sort { get; set; }

        public Dictionary<string, double[]> filtrnumber { get; set; }
        public Dictionary<string, string> filtrsting { get; set; }



        public class FiltrAndSotCommandHandler : IRequestHandler<FiltrAndSortCommand, CSVFile>
        {
            private readonly ICSVServiceFiltr filtr;
            private readonly ICSVServiceWriter writer;
            public readonly ICSVServiceData data;

            public FiltrAndSotCommandHandler(ICSVServiceFiltr filtr, ICSVServiceWriter writer, ICSVServiceData data)
            {
                this.filtr = filtr ?? throw new ArgumentNullException(nameof(filtr));
                this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
                this.data = data ?? throw new ArgumentNullException(nameof(data));
            }

            public async Task<CSVFile> Handle(FiltrAndSortCommand command, CancellationToken cancellationToken)
            {
                CsvModel? model = await data.GetById(command.Id);
                if (model is null) throw new Exception("Не существует такого файла");
                foreach (string i in command.filtrnumber.Keys)
                {
                    if (!model.Values.ContainsKey(i)) throw new Exception(String.Format("Поля {} нет в файле", i));
                    if (model.Values[i]=="string") throw new Exception(String.Format("Поле {} не числовое", i));
                    if (command.filtrnumber[i].Length!=2) throw new Exception(String.Format("Неверная длина массива у поля {}"));
                    if (command.filtrnumber[i][0]> command.filtrnumber[i][1]) throw new Exception(String.Format("У поля {} мин. не может бытьбольше макс."));
                }
                foreach (string i in command.filtrsting.Keys)
                {
                    if (!model.Values.ContainsKey(i)) throw new Exception(String.Format("Поля {} нет в файле", i));
                    if (model.Values[i] == "number") throw new Exception(String.Format("Поле {} не строковое", i));
                    
                }
                
                return await filtr.Filtr(model.Path, model , command.filtrnumber, command.filtrsting, command.sort);

            }


        }
        public class AddProductCommandValidator : AbstractValidator<FiltrAndSortCommand>
        {
            public AddProductCommandValidator()
            {

                RuleFor(c => c.Id).NotEmpty().WithMessage("Name: Имя не существет");
                


            }


        }

    }
}
