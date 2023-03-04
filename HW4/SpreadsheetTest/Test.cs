using SpreadsheetEngine;

namespace SpreadsheetTest
{

    /// <summary>
    /// Testing class.
    /// </summary>
    public class Tests
    {
        // Instanciates a Spreadsheet.
        // Im kiddo now :D anw xong roai
        private readonly Spreadsheet spreadSheetTest = new Spreadsheet(4, 4);

        /// <summary>
        /// Tests if RowCount/ColumnCount are properly passed.
        /// </summary>
        [Test]
        public void TestSpreadSheet()
        {
            Assert.That(spreadSheetTest.RowCount, Is.EqualTo(4));
            Assert.That(spreadSheetTest.ColumnCount, Is.EqualTo(4));
        }
    }
}