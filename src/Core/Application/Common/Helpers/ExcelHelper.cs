using ClosedXML.Excel;
using System.Data;

namespace Application.Common.Helpers
{

    public static class ExcelHelper
    {
        public static byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            var dataTable = ToDataTable(data);
            worksheet.Cell(1, 1).InsertTable(dataTable);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        private static DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            var dt = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var item in data)
            {
                var row = dt.NewRow();
                foreach (var prop in props)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
