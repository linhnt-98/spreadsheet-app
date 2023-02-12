namespace HW3Tests
{
    using HW3;

    /// <summary>
    /// Test cases for HW3.
    /// </summary>
    public class Tests
    {
        private readonly FibonacciTextReader fibTextReader = new FibonacciTextReader(0);

        /// <summary>
        /// Normal Case Testing.
        /// </summary>
        [Test]
        public void FibNormalCaseTest()
        {
            Assert.That((int)this.fibTextReader.CalcFib(10), Is.EqualTo(55));
        }

        /// <summary>
        /// Edge Case Testing.
        /// </summary>
        [Test]
        public void FibEdgeCaseTest()
        {
            Assert.That((int)this.fibTextReader.CalcFib(1), Is.EqualTo(1));
        }

        /// <summary>
        /// Exceptional Case Testing.
        /// </summary>
        [Test]
        public void FibExceptionalCaseTest()
        {
            Assert.That((int)this.fibTextReader.CalcFib(-10), Is.EqualTo(0));
        }

        /// <summary>
        /// Special Case 0 Testing.
        /// </summary>
        [Test]
        public void FibSpecialCase0Test()
        {
            Assert.That((int)this.fibTextReader.CalcFib(0), Is.EqualTo(0));
        }
    }
}