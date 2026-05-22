using Core.Application.Interface.Repository.Sales;
using Core.Application.Interface.Services.Emails;
using Infrastructure.Services.Sales.Reports;
using Infrastructure.Utils.DateUtils;
using Quartz;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;


namespace Infrastructure.Jobs;

public class DailySummaryReports : IJob
{
    private readonly IEmailService _emailService;
    private readonly ISalesRepository _salesRepository;

    public DailySummaryReports(IEmailService emailService, ISalesRepository salesRepository)
    {
       _emailService = emailService;
       _salesRepository = salesRepository;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        DateTime dateStart = DateConverter.TodayHaitiToUTC(0,0);
        DateTime dateEnd   = DateConverter.TodayHaitiToUTC(23,59);
        var result = await _salesRepository.GetSalesSummaryByDateRangeAsync(dateStart, dateEnd);
       

        byte[] pdfBytes;
        using (var stream = new MemoryStream())
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var document = new DailySalesReportPDF(result);
            document.GeneratePdf(stream);
            pdfBytes = stream.ToArray();
        }
        var attachments = new List<(byte[] content, string fileName, string mimeType)>
        {
            (pdfBytes, $"{DateTime.Now.AddDays(-1):yyyyMMdd}-rapport-journalier-aurabe.pdf", "application/pdf")
        };

        await _emailService.SendEmailAsync(
    to: new List<string> { "WilbensonCharles7@gmail.com", "francketiennejeudy380@gmail.com", "wadlinepierressaint1@gmail.com", "djerry.g87@gmail.com" },
    subject: "(Rapport journalier de vente - Aurabe",
    body: "Bonsoir,\n\nVeuillez trouver ci-joint le rapport journalier des ventes du " + $"Date : {DateTime.Now.AddDays(-1).ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("fr-FR"))} " + " pour la succursale de Jacmel",
    attachments: attachments
);
    }
}
