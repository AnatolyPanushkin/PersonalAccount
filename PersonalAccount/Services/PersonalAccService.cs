using System.Collections;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Mapper;

namespace PersonalAccount.Services;

public class PersonalAccService:IPersonalAccService
{
    private readonly AirCompaniesContext _context;

    public PersonalAccService(AirCompaniesContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<DataAll>> ByDocNumberAsync(ByDocNumber byDocNumber)
    {
        StreamReader stream = new StreamReader("SqlRows\\ByDocNumberSql.txt"); 
        
        string? buffer = await stream.ReadToEndAsync();
        
        string? parameterizedSqlRow = string.Format(buffer,byDocNumber.DocNumber) ;
        
        var result = await _context.DataAlls.FromSqlRaw(parameterizedSqlRow).ToListAsync();
        return result;
    }

    public async Task<ICollection<DataAll>> ByTicketNumberAsync(ByTicketNumber byTicketNumber)
    {
        string request = "";
        if (byTicketNumber.CheckBox)
        {
            StreamReader stream = new StreamReader("SqlRows\\ByTicketNumberAllTicketSql.txt");
            string? buffer = await stream.ReadToEndAsync();
            request = string.Format(buffer,byTicketNumber.TicketNumber) ;
        }
        else
        {
            StreamReader stream = new StreamReader("SqlRows\\ByTicketNumberSelectedTicketSql.txt");
            string? buffer = await stream.ReadToEndAsync();
            request = string.Format(buffer, byTicketNumber.TicketNumber) ; 
        }
        
        var result = await _context.DataAlls.FromSqlRaw(request).ToListAsync();
        return result;
    }

    public async Task<ICollection<PrintXlsx>> ByDocNumberPrintAsync(ByDocNumberPrint byDocNumberPrint)
    {
        StreamReader stream = new StreamReader("SqlRows\\ByDocNumberPrintSql.txt");
        string? buffer = await stream.ReadToEndAsync();
        string? parameterizedSqlRow = string.Format(buffer,byDocNumberPrint.AirlineCompanyIataCode, byDocNumberPrint.DocNumber) ;
        var result = await _context.DataAlls.FromSqlRaw(parameterizedSqlRow).ToListAsync();
        return result.MapToFromatForPrint();
    }

    public async Task<ICollection<PrintXlsx>> ByTicketNumberPrintAsync(ByTicketNumberPrint byTicketNumberPrint)
    {
        string request = "";
        if (byTicketNumberPrint.ByTicketNumberCheckBox)
        {
            StreamReader stream = new StreamReader("SqlRows\\ByTicketNumberPrintAllTicketSql.txt");
            string? buffer = await stream.ReadToEndAsync();
            request = string.Format(buffer,byTicketNumberPrint.AirlineCompanyIataCode, byTicketNumberPrint.TicketNumber) ;
            
        }
        else
        {
            StreamReader stream = new StreamReader("SqlRows\\ByTicketNumberPrintSelectedTicketSql.txt");
            string? buffer = await stream.ReadToEndAsync();
            request = string.Format(buffer,byTicketNumberPrint.AirlineCompanyIataCode, byTicketNumberPrint.TicketNumber) ;
        }
        
        
        var result = await _context.DataAlls.FromSqlRaw(request).ToListAsync();
        return result.MapToFromatForPrint();
    }

    public async Task<ICollection<DataAll>> XlsxByTicketNumber(ByTicketNumberPrint byTicketNumberPrint)
    {
        string request = "";
        if (byTicketNumberPrint.ByTicketNumberCheckBox)
        {
            StreamReader stream = new StreamReader("SqlRows\\ByTicketNumberPrintAllTicketSql.txt");
            string? buffer = await stream.ReadToEndAsync();
            request = string.Format(buffer,byTicketNumberPrint.AirlineCompanyIataCode, byTicketNumberPrint.TicketNumber) ;
            
        }
        else
        {
            StreamReader stream = new StreamReader("SqlRows\\ByTicketNumberPrintSelectedTicketSql.txt");
            string? buffer = await stream.ReadToEndAsync();
            request = string.Format(buffer,byTicketNumberPrint.AirlineCompanyIataCode, byTicketNumberPrint.TicketNumber) ;
        }
         
        var result = await _context.DataAlls.FromSqlRaw(request).ToListAsync();
        return result;
        
    }

    public async Task<ICollection<AirlineCompany>> GetAllAirCompanies()
    {
        return  await _context.AirlineCompany.Select(t => t).ToListAsync();
    }
    
}