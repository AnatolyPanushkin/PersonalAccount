using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Services;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Configuration;
using DocumentFormat.OpenXml.InkML;
using OfficeOpenXml;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PersonalAccount.Controllers;

[ApiController]

[Route("v1/transactions/")]
[RequestSizeLimit(2048)]
public class PaController:ControllerBase
{
    private readonly IPersonalAccService _service;
    private readonly AirCompaniesContext _context;
   

    public PaController(IPersonalAccService service, AirCompaniesContext context)
    {
        _service = service;
        _context = context;
    }
    
    [HttpPost("by_ticket_number")]
    public async Task<ActionResult> ByTicketNumber(ByTicketNumber byTicketNumber)
    {
        var result =  _service.ByTicketNumberAsync(byTicketNumber);
        if (await Task.WhenAny(result, Task.Delay(60000)) == result)
        {
            return Ok(await result);
        }
        throw new HttpRequestException("Exceeded the limit of throttling time from server!");
        
        /*try
        {
            var result = _context.DataAlls
                .Where(t => t.TicketNumber != null 
                            && t.TicketNumber.Equals(Convert.ToString(ticketNumber)))
                .Select(t => t).ToList();
        
            return Ok(result);
        }
        catch (Exception e)
        {
            throw new BadHttpRequestException("error massage");
        }*/
        
    }

    [HttpPost("by_doc_number")]
    public async Task<IActionResult> ByDocNumber(ByDocNumber byDocNumber)
    {
        var result =  _service.ByDocNumberAsync(byDocNumber);
        if (await Task.WhenAny(result, Task.Delay(60000)) == result)
        {
            return Ok(await result);
        }
        throw new HttpRequestException("Exceeded the limit of throttling time from server!");
        /*try
        {
            var result = _context.DataAlls
                .Where(t => t.PassengerDocumentNumber != null 
                            && t.PassengerDocumentNumber.Equals(Convert.ToString(docNumber)))
                .Select(t => t).ToList();
        
            return Ok(result);
        }
        catch (Exception e)
        {
            throw new BadHttpRequestException("error message!");
        }*/
        
    }
    
    [HttpPost("by_doc_number_print")]
    public async Task<ActionResult> ByDocNumberPrint(ByDocNumberPrint byDocNumberPrint)
    {
        var result = await _service.ByDocNumberPrintAsync(byDocNumberPrint);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (ExcelPackage pck = new ExcelPackage())
        {
            pck.Workbook.Worksheets.Add("Transactions").Cells[1, 1].LoadFromCollection(result, true);
            var excelData = pck.GetAsByteArray();
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Transactions.xlsx";
            return File(excelData, contentType, fileName);
        }
        /*var transaction = _service.ByDocNumberPrintAsync(byDocNumberPrint);
        if (await Task.WhenAny(transaction, Task.Delay(60000)) == transaction)
        {
            
            var cd = new ContentDisposition()
            {
                FileName = $"{byDocNumberPrint.AirlineCompanyIataCode}airlineReport.csv",
                Inline = false
            };
            Response.Headers.Add("Content-Disposition",cd.ToString());
            Response.Headers.Add("Content-Type","text/csv");
            return Ok(await transaction);
        }
        throw new HttpRequestException("Exceeded the limit of throttling time from server!");*/
        
    }
    
    [HttpPost("by_ticket_number_print")]
    public async Task<ActionResult> ByTicketNumberPrint(ByTicketNumberPrint byTicketNumberPrint)
    {
        var result = await _service.ByTicketNumberPrintAsync(byTicketNumberPrint);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (ExcelPackage pck = new ExcelPackage())
        {
            pck.Workbook.Worksheets.Add("Transactions").Cells[1, 1].LoadFromCollection(result, true);
            var excelData = pck.GetAsByteArray();
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Transactions.xlsx";
            return File(excelData, contentType, fileName);
        }
        /*var contentRootPath = _hostingEnvironment.ContentRootPath;

        // "items" is a List<T> of DataObjects
        var items = await _service.ByTicketNumberPrintAsync(byTicketNumberPrint);

        var fileInfo = new ExcelFileCreator(contentRootPath).Execute(items);
        var bytes = System.IO.File.ReadAllBytes(fileInfo.FullName);

        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        HttpContext.Response.ContentType = contentType;
        HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

        var fileContentResult = new FileContentResult(bytes, contentType)
        {
            FileDownloadName = fileInfo.Name
        };

        return fileContentResult;*/
        /*var transaction =  _service.ByTicketNumberPrintAsync(byTicketNumberPrint);
        if (await Task.WhenAny(transaction, Task.Delay(60000)) == transaction)
        {
            var cd = new ContentDisposition()
            {
                FileName = $"{byTicketNumberPrint.AirlineCompanyIataCode}airlineReport_.csv",
                Inline = false
            };
            Response.Headers.Add("Content-Disposition",cd.ToString());
            Response.Headers.Add("Content-Type","text/csv");
            return Ok(await transaction);
        }
        throw new HttpRequestException("Exceeded the limit of throttling time from server!");*/
        
    }

    [HttpGet("xlsx/{ticketNumber}")]
    public IActionResult GetByTicketNumber(long ticketNumber)
    {
        var result = _context.DataAlls;
        
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (ExcelPackage pck = new ExcelPackage())
        {
            pck.Workbook.Worksheets.Add("Transactions").Cells[1, 1].LoadFromCollection(result, true);
            var excelData = pck.GetAsByteArray();
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Transactions.xlsx";
            return File(excelData, contentType, fileName);
            /*using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Headers.Add("content-disposition", "attachment; filename=\"Transactions.xlsx\"");
                Response.BinaryWrite(pck.GetAsByteArray());
                pck.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }*/

            
        }
    
        
        /*var result = _context.DataAlls
            .Where(t=>t.TicketNumber.Equals(Convert.ToString(ticketNumber))).Select(t=>t).ToList();

        using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
        {
            var worksheet = workbook.Worksheets.Add("Transactions");

            worksheet.Cell("A1").Value = "№";
            worksheet.Cell("B1").Value = "Документ";
            worksheet.Cell("C1").Value = "Фамилия";
            worksheet.Cell("D1").Value = "Имя";
            worksheet.Cell("E1").Value = "Отправитель";
            worksheet.Cell("F1").Value = "Статус";
            worksheet.Cell("G1").Value = "Дата операции";
            worksheet.Cell("H1").Value = "Тип операции";
            worksheet.Cell("I1").Value = "Номер билета";
            worksheet.Cell("J1").Value = "Время вылета";
            worksheet.Cell("K1").Value = "Код АК";
            worksheet.Cell("L1").Value = "Город вылета";
            worksheet.Cell("M1").Value = "Тип операции";
            worksheet.Cell("N1").Value = "Город прилета";
            
            worksheet.Row(1).Style.Font.Bold = true;

            //нумерация строк/столбцов начинается с индекса 1 (не 0)
            /*for (int i = 0; i < phoneBrands.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = phoneBrands[i].Title;
                worksheet.Cell(i + 2, 2).Value = string.Join(", ", phoneBrands[i].PhoneModels.Select(x => x.Title));
            }#1#

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Flush();

                return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = $"brands_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                };
            }
        }
        */

        /*var result = _context.DataAlls
            .Where(t=>t.TicketNumber.Equals(Convert.ToString(ticketNumber))).Select(t=>t);
        
        
        ExcelPackage excel = new ExcelPackage();
        ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Query Results");
        
        workSheet.Cells["A1"].LoadFromCollection(result.ToList(),true);
        string path = @"D:\Data\test.xlsx";
        Stream stream = ;
        excel.SaveAs(stream);
        stream.Close();
        return Ok();*/
    }

    [HttpPost("get_all_airlines")]
    public async Task<ActionResult> GetAllAirlines()
    {
        return Ok(await _service.GetAllAirCompanies());
    }
    
}