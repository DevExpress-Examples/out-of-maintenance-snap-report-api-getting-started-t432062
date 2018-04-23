Imports DevExpress.Snap
Imports DevExpress.Snap.Core.API
Imports DevExpress.XtraRichEdit
Imports DevExpress.XtraRichEdit.API.Native
Imports SnapServerGettingStarted.NWindDataSetTableAdapters
Imports System
Imports System.Data.SqlClient

Namespace SnapServerGettingStarted
    Friend Class Program
        Shared Sub Main(ByVal args() As String)
'            #Region "#Main"
            Dim server As New SnapDocumentServer()
            Dim document As SnapDocument = server.Document

            Dim datasource As Object = GetDataSource()
            document.DataSource = datasource

            Console.Write("Generating Document... ")
            GenerateLayout(document)
            Console.WriteLine("Ok!")
            Console.Write("Press any key...")
            Console.ReadKey()
            System.Diagnostics.Process.Start("SnapDocumentServerTest.rtf")
'            #End Region ' #Main
        End Sub
        #Region "#GenerateLayout"
        Private Shared Sub GenerateLayout(ByVal document As SnapDocument)
            ' Add a Snap list to the document.
            Dim list As SnapList = document.CreateSnList(document.Range.End, "List")
            list.BeginUpdate()
            list.EditorRowLimit = 100500

            ' Add a header to the Snap list.                                                                   
            Dim listHeader As SnapDocument = list.ListHeader
            Dim listHeaderTable As Table = listHeader.Tables.Create(listHeader.Range.End, 1, 3)
            Dim listHeaderCells As TableCellCollection = listHeaderTable.FirstRow.Cells
            listHeader.InsertText(listHeaderCells(0).ContentRange.End, "Product Name")
            listHeader.InsertText(listHeaderCells(1).ContentRange.End, "Units in Stock")
            listHeader.InsertText(listHeaderCells(2).ContentRange.End, "Unit Price")

            'Create the row template and fill it with data
            Dim listRow As SnapDocument = list.RowTemplate
            Dim listRowTable As Table = listRow.Tables.Create(listRow.Range.End, 1, 3)
            Dim listRowCells As TableCellCollection = listRowTable.FirstRow.Cells
            listRow.CreateSnText(listRowCells(0).ContentRange.End, "ProductName")
            listRow.CreateSnText(listRowCells(1).ContentRange.End, "UnitsInStock")
            listRow.CreateSnText(listRowCells(2).ContentRange.End, "UnitPrice \$ $0.00")

            list.EndUpdate()
            list.Field.Update()
            document.ExportDocument("SnapDocumentServerTest.rtf", DocumentFormat.Rtf)
        End Sub
        #End Region ' #GenerateLayout

        #Region "#DataSource"
        Private Shared Function GetDataSource() As Object
            Dim dataSource As New NWindDataSet()
            Dim connection = New SqlConnection()
            connection.ConnectionString = My.Settings.Default.NWindConnectionString

            Dim products As New ProductsTableAdapter()
            products.Connection = connection
            products.Fill(dataSource.Products)

            Return dataSource.Products
        End Function
        #End Region ' #DataSource
    End Class
End Namespace
