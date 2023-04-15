using SpreadsheetEngine;
using static SpreadsheetEngine.Spreadsheet;
using System.Xml;

namespace SpreadsheetTest
{

    /// <summary>
    /// Testing class.
    /// </summary>
    public class Tests
    {
        // Instanciates a Spreadsheet.
        private readonly Spreadsheet spreadSheetTest = new Spreadsheet(4, 4);

        /// <summary>
        /// Tests if RowCount/ColumnCount are properly passed.
        /// </summary>
        [Test]
        public void TestSpreadSheet()
        {
            Assert.That(this.spreadSheetTest.RowCount, Is.EqualTo(4));
            Assert.That(this.spreadSheetTest.ColumnCount, Is.EqualTo(4));
        }

        /// <summary>
        /// Testing normal expression cases.
        /// </summary>
        [Test]
        public void ExpressionTreeNormalCaseTests()
        {
            ExpressionTree testAddition = new ExpressionTree("5+5");
            Assert.That(testAddition.Evaluate(), Is.EqualTo(10.0));

            ExpressionTree testSubtraction = new ExpressionTree("5-1");
            Assert.That(testSubtraction.Evaluate(), Is.EqualTo(4.0));

            ExpressionTree testMultiplication = new ExpressionTree("5*5");
            Assert.That(testMultiplication.Evaluate(), Is.EqualTo(25.0));

            ExpressionTree testDivision = new ExpressionTree("3/6");
            Assert.That(testDivision.Evaluate(), Is.EqualTo(0.5));

            ExpressionTree testAdditionSeries = new ExpressionTree("1+2+3+4");
            Assert.That(testAdditionSeries.Evaluate(), Is.EqualTo(10.0));

            ExpressionTree testMultiplicationSeries = new ExpressionTree("1*2*3*4");
            Assert.That(testMultiplicationSeries.Evaluate(), Is.EqualTo(24.0));

            ExpressionTree testDivisionSeries = new ExpressionTree("100/5/2/5");
            Assert.That(testDivisionSeries.Evaluate(), Is.EqualTo(2.0));
        }

        /// <summary>
        /// Testing boundary expression cases.
        /// </summary>
        [Test]
        public void ExpressionTreeBoundaryCaseTests()
        {
            ExpressionTree testDivisionBoundary = new ExpressionTree("0/65");
            Assert.That(testDivisionBoundary.Evaluate(), Is.EqualTo(0));

            ExpressionTree testMultiplicationBoundary = new ExpressionTree("0*55");
            Assert.That(testMultiplicationBoundary.Evaluate(), Is.EqualTo(0));
        }

        /// <summary>
        /// Testing edge expression cases.
        /// </summary>
        [Test]
        public void ExpressionTreeEdgeCaseTests()
        {
            ExpressionTree testInvalidExpression = new ExpressionTree("5/0");
            Assert.That(testInvalidExpression.Evaluate(), Is.EqualTo(double.PositiveInfinity));

            ExpressionTree testInvalidInput = new ExpressionTree("string");
            Assert.That(testInvalidInput.Evaluate(), Is.EqualTo(0));
        }

        /// <summary>
        /// Testing parentheses in expression.
        /// </summary>
        [Test]
        public void ParenthesesNormalCaseTests()
        {
            ExpressionTree additionParentTest = new ExpressionTree("7+(7+7)");
            Assert.That(additionParentTest.Evaluate(), Is.EqualTo(21));

            ExpressionTree multiplicationParentTest = new ExpressionTree("(6+6)*6");
            Assert.That(multiplicationParentTest.Evaluate(), Is.EqualTo(72));

            ExpressionTree divisionParentTest = new ExpressionTree("(6+6)/6");
            Assert.That(divisionParentTest.Evaluate(), Is.EqualTo(2));
        }

        /// <summary>
        /// Testing order of operations.
        /// </summary>
        [Test]
        public void ExpressionOrderTests()
        {
            ExpressionTree multiplicationOrderTest = new ExpressionTree("8+8*3+7");
            Assert.That(multiplicationOrderTest.Evaluate(), Is.EqualTo(39));

            ExpressionTree divisionOrderTest = new ExpressionTree("8+9/3+7");
            Assert.That(divisionOrderTest.Evaluate(), Is.EqualTo(18));
        }

        /// <summary>
        /// Tests if two cells has the same value.
        /// </summary>
        [Test]
        public void TestCellsSameValue()
        {
            this.spreadSheetTest.SetCellText(1, 1, "8");
            this.spreadSheetTest.SetCellText(1, 2, "=A1");
            Assert.That(this.spreadSheetTest.GetCell(1, 1).Text, Is.EqualTo(this.spreadSheetTest.GetCell(1, 2).Value));
        }

        /// <summary>
        /// Tests if addition works in an expression.
        /// </summary>
        [Test]
        public void TestCellsAddition()
        {
            this.spreadSheetTest.SetCellText(1, 1, "8");
            this.spreadSheetTest.SetCellText(1, 2, "2");
            this.spreadSheetTest.SetCellText(2, 1, "=A1+B1");
            Assert.That(this.spreadSheetTest.GetCell(2, 1).Text, Is.EqualTo("10"));
        }

        /// <summary>
        /// Tests if subtraction works in an expression.
        /// </summary>
        [Test]
        public void TestCellsSubtraction()
        {
            this.spreadSheetTest.SetCellText(1, 1, "8");
            this.spreadSheetTest.SetCellText(1, 2, "2");
            this.spreadSheetTest.SetCellText(2, 1, "=A1-B1");
            Assert.That(this.spreadSheetTest.GetCell(2, 1).Text, Is.EqualTo("6"));
        }

        /// <summary>
        /// Tests if multiplication works in an expression.
        /// </summary>
        [Test]
        public void TestCellsMultiplication()
        {
            this.spreadSheetTest.SetCellText(1, 1, "8");
            this.spreadSheetTest.SetCellText(1, 2, "2");
            this.spreadSheetTest.SetCellText(2, 1, "=A1*B1");
            Assert.That(this.spreadSheetTest.GetCell(2, 1).Text, Is.EqualTo("16"));
        }

        /// <summary>
        /// Tests if division works in an expression.
        /// </summary>
        [Test]
        public void TestCellsDivision()
        {
            this.spreadSheetTest.SetCellText(1, 1, "8");
            this.spreadSheetTest.SetCellText(1, 2, "2");
            this.spreadSheetTest.SetCellText(2, 1, "=A1/B1");
            Assert.That(this.spreadSheetTest.GetCell(2, 1).Text, Is.EqualTo("4"));
        }

        /// <summary>
        /// Testing colors.
        /// </summary>
        [Test]
        public void TestColorChange()
        {
            this.spreadSheetTest.setColor(1, 1, "Blue");
            Assert.That(this.spreadSheetTest.getColor(1, 1), Is.EqualTo("Blue"));

            this.spreadSheetTest.setColor(1, 1, "Red");
            Assert.That(this.spreadSheetTest.getColor(1, 1), Is.EqualTo("Red"));

            this.spreadSheetTest.setColor(1, 1, "Orange");
            Assert.That(this.spreadSheetTest.getColor(1, 1), Is.EqualTo("Orange"));

            this.spreadSheetTest.setColor(1, 1, "Yellow");
            Assert.That(this.spreadSheetTest.getColor(1, 1), Is.EqualTo("Yello"));
        }

        /// <summary>
        /// Tests saving cell to XML format.
        /// </summary>
        [Test]
        public void TestSaveCellToXML()
        {
            Spreadsheet spreadsheet = new Spreadsheet(5, 5);
            spreadsheet.CellPropertyChanged += (sender, e) => { };
            CreateCell testCell = new CreateCell(1, 1, string.Empty, 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            spreadsheet.CreateXML();
            string workingDirectory = Environment.CurrentDirectory;
            string directory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = directory + @"\Saves\spreadSheet.xml";
            Assert.That(File.Exists(path), Is.EqualTo(true));
        }

        /// <summary>
        /// Tests loading cell from XML.
        /// </summary>
        [Test]
        public void TestLoadCellFromXML()
        {
            Spreadsheet spreadsheet = new Spreadsheet(5, 5);
            spreadsheet.CellPropertyChanged += (sender, e) => { };
            CreateCell testCell = new CreateCell(1, 1, "Test", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            spreadsheet.CreateXML();
            testCell = new CreateCell(1, 1, "fail", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            spreadsheet.LoadXML();
            Assert.That(spreadsheet.GetCell(1, 1).CellText, Is.EqualTo("Test"));
        }

        /// <summary>
        /// Tests saving multiple cells.
        /// </summary>
        [Test]
        public void TestSaveCellsToXML()
        {
            Spreadsheet spreadsheet = new Spreadsheet(10, 10);
            spreadsheet.CellPropertyChanged += (sender, e) => { };
            CreateCell testCell = new CreateCell(1, 1, "test", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            testCell = new CreateCell(1, 2, "test1", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            testCell = new CreateCell(1, 3, "test2", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            spreadsheet.CreateXML();
            string workingDirectory = Environment.CurrentDirectory;
            string directory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = directory + @"\Saves\spreadSheet.xml";
            Assert.That(File.Exists(path), Is.EqualTo(true));
        }

        /// <summary>
        /// Tests loading multiple cells from XML.
        /// </summary>
        [Test]
        public void TestLoadCellsFromXML()
        {
            Spreadsheet spreadsheet = new Spreadsheet(10, 10);
            spreadsheet.CellPropertyChanged += (sender, e) => { };
            CreateCell testCell = new CreateCell(1, 1, "test", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            testCell = new CreateCell(1, 2, "test1", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            testCell = new CreateCell(1, 3, "test2", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            spreadsheet.CreateXML();
            testCell = new CreateCell(1, 4, "fail", 0xFFFFFFFF);
            spreadsheet.UpdateCell(testCell);
            spreadsheet.LoadXML();
            Assert.That(spreadsheet.GetCell(1, 3).CellText, Is.EqualTo("test2"));
        }

        /// <summary>
        /// Tests loading cell with different XML formatting.
        /// </summary>
        [Test]
        public void TestLoadCellDifferentXML()
        {
            Spreadsheet spread = new Spreadsheet(10, 10);
            spread.CellPropertyChanged += (sender, e) => { };
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadSheet");
                writer.WriteStartElement("cell");
                writer.WriteStartElement("bgcolor");
                writer.WriteString("0xFFFFFFFF");
                writer.WriteEndElement();
                writer.WriteStartElement("cellText");
                writer.WriteString("test");
                writer.WriteEndElement();
                writer.WriteStartElement("cellName");
                writer.WriteString("A2");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            string workingDirectory = Environment.CurrentDirectory;
            string directory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            doc.Save(directory + @"\Saves\spreadSheet.xml");
            spread.LoadXML();
            Assert.That(spread.GetCell(1, 1).CellText, Is.EqualTo("test"));
        }
    }

}