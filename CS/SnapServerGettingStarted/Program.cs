using DevExpress.Snap;
using DevExpress.Snap.Core.API;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SnapServerGettingStarted.NWindDataSetTableAdapters;
using System;
using System.Data.SqlClient;

namespace SnapServerGettingStarted
{
    class Program
    {
        static void Main(string[] args)
        {
            #region #Main
            //Create a new SnapDocumentServer instance.
            SnapDocumentServer server = new SnapDocumentServer();
            SnapDocument document = server.Document;

            //Specify the server datasource.
            object datasource = GetDataSource();
            document.DataSource = datasource;

            Console.Write("Generating Document... ");
            // Generate the report.
            GenerateLayout(document);

            Console.WriteLine("Ok!");
            Console.Write("Press any key...");
            Console.ReadKey();

            document.ExportDocument("SnapDocumentServerTest.rtf", DocumentFormat.Rtf);
            System.Diagnostics.Process.Start("SnapDocumentServerTest.rtf");
            #endregion #Main
        }
        #region #GenerateLayout
        static void GenerateLayout(SnapDocument document)
        {
            // Add a Snap list to the document.
            SnapList list = document.CreateSnList(document.Range.End, @"List");
            list.BeginUpdate();
            list.EditorRowLimit = 100500;

            // Add a header to the Snap list.                                                                   
            SnapDocument listHeader = list.ListHeader;
            Table listHeaderTable = listHeader.Tables.Create(listHeader.Range.End, 1, 3);
            TableCellCollection listHeaderCells = listHeaderTable.FirstRow.Cells;
            listHeader.InsertText(listHeaderCells[0].ContentRange.End, "Product Name");
            listHeader.InsertText(listHeaderCells[1].ContentRange.End, "Units in Stock");
            listHeader.InsertText(listHeaderCells[2].ContentRange.End, "Unit Price");

            //Create the row template and fill it with data.
            SnapDocument listRow = list.RowTemplate;
            Table listRowTable = listRow.Tables.Create(listRow.Range.End, 1, 3);
            TableCellCollection listRowCells = listRowTable.FirstRow.Cells;
            listRow.CreateSnText(listRowCells[0].ContentRange.End, @"ProductName");
            listRow.CreateSnText(listRowCells[1].ContentRange.End, @"UnitsInStock");
            listRow.CreateSnText(listRowCells[2].ContentRange.End, @"UnitPrice \$ $0.00");

            list.EndUpdate();
            list.Field.Update();
        }
        #endregion #GenerateLayout

        #region #DataSource
        private static object GetDataSource()
        {
            NWindDataSet dataSource = new NWindDataSet();
            var connection = new SqlConnection();
            connection.ConnectionString = Properties.Settings.Default.NWindConnectionString;

            ProductsTableAdapter products = new ProductsTableAdapter();
            products.Connection = connection;
            products.Fill(dataSource.Products);

            return dataSource.Products;
        }
        #endregion #DataSource
    }
}
