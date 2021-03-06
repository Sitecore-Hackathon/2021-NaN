﻿namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Import
{

    using Hackathon.MLBox.Foundation.Common.Models;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    public class ExcelImportProcessor
    {
        public List<Customer> GetImportData(Stream fileStream)
        {
            return BuildObjectModel(ReadExcel(fileStream));
        }

        private List<Customer> BuildObjectModel(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            List<Customer> customers = new List<Customer>();

            var groupedData = dataTable.AsEnumerable().GroupBy(x => x.Field<string>("CustomerID"));
            foreach (IGrouping<string, DataRow> data in groupedData)
            {
                if (!string.IsNullOrEmpty(data.Key))
                {
                    int customerId = int.Parse(data.Key);
                    if (customerId <= 0)
                        continue;

                    var customer = new Customer
                    {
                        CustomerId = customerId,
                        Invoices = new List<Invoice>(),
                    };

                    foreach (DataRow record in data.ToList())
                    {
                        int.TryParse(record["Quantity"].ToString(), out var quantity);
                        decimal.TryParse(record["UnitPrice"].ToString(), out var unitPrice);
                        DateTime.TryParse(record["InvoiceDate"].ToString(), out var dt);
                        int.TryParse(record["InvoiceNo"].ToString(), out var number);

                        if (number > 0 && quantity > 0)
                        {
                            customer.Invoices.Add(new Invoice
                            {
                                Quantity = quantity,
                                Number = number,
                                Price = unitPrice,
                                TimeStamp = dt,
                                Currency = "EUR"
                            });
                        }
                    }

                    if (customer.Invoices.Any())
                        customers.Add(customer);
                }
            }

            return customers;
        }

        private List<Invoice> BuildProductModel(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            List<Invoice> products = new List<Invoice>();

            var groupedData = dataTable.AsEnumerable().GroupBy(x => x.Field<string>("StockCode"));
            foreach (IGrouping<string, DataRow> data in groupedData)
            {
                if (!string.IsNullOrEmpty(data.Key))
                {
                    var found = false;
                    foreach (var record in data)
                    {
                        var description = record["Description"].ToString();
                        int.TryParse(record["Quantity"].ToString(), out var quantity);
                        decimal.TryParse(record["UnitPrice"].ToString(), out var unitPrice);
                        int.TryParse(record["InvoiceNo"].ToString(), out var number);



                        if (number > 0 && quantity > 0 && !string.IsNullOrEmpty(description))
                        {
                            found = true;
                            products.Add(new Invoice
                            {
                                Quantity = quantity,
                                Number = number,
                                Price = unitPrice,
                            });
                        }

                        if(found) break;
                    }
                }
            }

            return products;
        }

        private DataTable ReadExcel(Stream stream)
        {
            DataTable dt = new DataTable();

            using (var excelPackage = new ExcelPackage(stream))
            {
                var ws = excelPackage.Workbook.Worksheets.First();

                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    dt.Columns.Add(firstRowCell.Text);
                }

                for (int rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = dt.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
            }

            return dt;
        }
    }
}
