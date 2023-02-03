namespace HW2Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Form1 x = new Form1();
            string text = " ";
            Assert.That(text, Is.EqualTo("1. HashSet method: 8000 unique numbers."));
            Assert.That(text, Is.EqualTo("2. O(1) storage method: 8000 unique numbers."));
            Assert.That(text, Is.EqualTo("3. Sorted method: 8000 unique numbers."));
        }
    }
}