using PersonalAccount.Data;

namespace PersonalAccount.Services;

public interface IPersonalAccService
{
    Task<ICollection<DataAll>> ByDocNumberAsync(ByDocNumber byDocNumber);
    Task<ICollection<DataAll>> ByTicketNumberAsync(ByTicketNumber byTicketNumber);
    Task<ICollection<PrintXlsx>> ByDocNumberPrintAsync(ByDocNumberPrint byDocNumberPrint);
    Task<ICollection<PrintXlsx>> ByTicketNumberPrintAsync(ByTicketNumberPrint byTicketNumberPrint);

    Task<ICollection<DataAll>> XlsxByTicketNumber(ByTicketNumberPrint byTicketNumberPrint);

    Task<ICollection<AirlineCompany>> GetAllAirCompanies();
}