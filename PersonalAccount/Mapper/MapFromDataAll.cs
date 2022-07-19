using PersonalAccount.Data;

namespace PersonalAccount.Mapper;

public static class MapFromDataAll
{
    public static List<PrintXlsx> MapToFromatForPrint(this List<DataAll> dataAlls)
    {
        var ticketXlsxes = new List<PrintXlsx>();

        foreach (var d in dataAlls)
        {
            var ticket = new PrintXlsx
            {
                OperationId = d.OperationId,
                PassengerDocumentNumber=d.PassengerDocumentNumber,
                Name = d.Name,
                Surname = d.Surname,
                Sender = d.Sender,
                ValidationStatus =d.ValidationStatus,
                Time = d.Time,
                Type = d.Type,
                TicketNumber =d.TicketNumber,
                DepartDatetime = d.DepartDatetime,
                AirlineCode = d.AirlineCode,
                CityFromName=d.CityFromName,
                CityToName=d.CityToName
            };
            ticketXlsxes.Add(ticket);
        }
        return ticketXlsxes;
    } 
}