using HW5;
using SpreadsheetEngine;

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
            Assert.That(testInvalidInput.Evaluate(), Is.EqualTo(double.NaN));
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
            Assert.That(multiplicationOrderTest.Evaluate(), Is.EqualTo(18));
        }
    }
}