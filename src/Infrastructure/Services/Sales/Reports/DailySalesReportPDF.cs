using Core.Domain.Procedures;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Sales.Reports;

public class DailySalesReportPDF: IDocument
{
    private readonly IEnumerable<GetSalesSummaryDto> dailySalesSummary;

    public DailySalesReportPDF(IEnumerable<GetSalesSummaryDto> values)
    {
        dailySalesSummary = values;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Header().Text($"Rapport Journalier de Vente").FontSize(18).Bold().AlignCenter();

            page.Content().Column(col =>
            {
                col.Spacing(15);

                col.Item().Text($"Entreprise : Aurabe");
                col.Item().Text($"Succursale : Jacmel-001");
                col.Item().Text($"Date : {DateTime.Now.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("fr-FR"))}");

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(120);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Vendeur").Bold();
                        header.Cell().Text("Nb. Ventes").Bold();
                        header.Cell().Text("Montant (HTG)").Bold();
                    });

                    foreach (var entry in dailySalesSummary)
                    {
                        table.Cell().Text(entry.FullName);
                        table.Cell().Text(entry.NumberOfSales.ToString());
                        table.Cell().Text($"{entry.SalesTotalAmount:N2} HTG");
                    }
                });

                int totalSales = 0;
                decimal totalAmount = 0;
                foreach (var entry in dailySalesSummary)
                {
                    totalSales += entry.NumberOfSales;
                    totalAmount += entry.SalesTotalAmount;
                }

                col.Item().PaddingTop(15).Text($"Total des ventes : {totalSales}").Bold();
                col.Item().Text($"Montant total HTG : {totalAmount:N2} HTG").Bold();
            });

            page.Footer().AlignRight().Text($"Généré le {DateTime.Now:dd MMM yyyy HH:mm}");
        });
    }
}
