using NUnit.Framework;

namespace Exp.Tests
{
    public class ExpTests
    {
        int _counter;
        ExpContainer _ec;

        [SetUp]
        public void SetUp()
        {
            _ec = new ExpContainer();
            _counter = 0;
        }

        [Test]
        public void TestInitialLevel()
        {
            Assert.That(_ec.Level, Is.EqualTo(1));
        }

        [Test]
        public void TestExpWithAdd()
        {
            Assert.That(_ec.Exp, Is.Zero);
            _ec.Add(100);
            Assert.That(_ec.Exp, Is.EqualTo(100));
            _ec.Add(200);
            Assert.That(_ec.Exp, Is.EqualTo(300));
        }


        [Test]
        public void TestThresholdExpWithInitialState()
        {
            Assert.That(_ec.ThresholdExpTo(-1), Is.EqualTo(0));
            Assert.That(_ec.ThresholdExpTo(0), Is.EqualTo(0));
            Assert.That(_ec.ThresholdExpTo(1), Is.EqualTo(100));
            Assert.That(_ec.ThresholdExpTo(2), Is.EqualTo(210));
            Assert.That(_ec.ThresholdExpTo(3), Is.EqualTo(331));
        }


        [Test]
        public void TestRequiresExpWithInitialState()
        {
            Assert.That(_ec.RequiresExpToNextLevel, Is.EqualTo(100));
            _ec.Add(100);
            Assert.That(_ec.RequiresExpToNextLevel, Is.EqualTo(110));
            _ec.Add(10);
            Assert.That(_ec.RequiresExpToNextLevel, Is.EqualTo(110));
            _ec.Add(100);
            Assert.That(_ec.RequiresExpToNextLevel, Is.EqualTo(121));
        }

        [Test]
        public void TestRemainingExpWithInitialState()
        {
            Assert.That(_ec.RemainingExpToNextLevel, Is.EqualTo(100));
            _ec.Add(80);
            Assert.That(_ec.RemainingExpToNextLevel, Is.EqualTo(20));
            _ec.Add(20);
            Assert.That(_ec.RemainingExpToNextLevel, Is.EqualTo(110));
            _ec.Add(100);
            Assert.That(_ec.RemainingExpToNextLevel, Is.EqualTo(10));
        }

        [Test]
        public void TestAdd100WithInitialState()
        {
            Assert.That(_ec.Add(100), Is.EqualTo(2));
            Assert.That(_ec.Level, Is.EqualTo(2));
        }

        [Test]
        public void TestAdd210WithInitialState()
        {
            Assert.That(_ec.Add(210), Is.EqualTo(3));
        }

        [Test]
        public void TestAdd331WithInitialState()
        {
            Assert.That(_ec.Add(331), Is.EqualTo(4));
        }

        [Test]
        public void TestAbnormalState()
        {
            _ec = new ExpContainer(100);
            Assert.That(_ec.Level, Is.EqualTo(2));
        }

        [Test]
        public void TestAdd300WithAbnormalState()
        {
            _ec = new ExpContainer(initialThreshold: 300);

            Assert.That(_ec.Add(100), Is.EqualTo(1));
            Assert.That(_ec.Add(200), Is.EqualTo(2));
        }

        [Test]
        public void TestRegister()
        {
            _ec.Register(IncrementCallback);
            Assert.That(_counter, Is.EqualTo(1));
        }

        [Test]
        public void TestRegisterWithoutCallImmediately()
        {
            _ec.Register(IncrementCallback, false);
            Assert.That(_counter, Is.EqualTo(0));


            _ec.Trigger();
            Assert.That(_counter, Is.EqualTo(1));
        }

        [Test]
        public void TestRegisterMultiple()
        {
            _ec.Register(IncrementCallback, false);
            _ec.Register(IncrementCallback);
            Assert.That(_counter, Is.EqualTo(2));
        }

        [Test]
        public void TestTrigger()
        {
            _ec.Register(IncrementCallback);
            Assert.That(_counter, Is.EqualTo(1));
            _ec.Trigger();
            Assert.That(_counter, Is.EqualTo(2));
        }


        [Test]
        public void TestUnregister()
        {
            _ec.Register(IncrementCallback);
            Assert.That(_counter, Is.EqualTo(1));

            _ec.Unregister(IncrementCallback);
            _ec.Trigger();
            Assert.That(_counter, Is.EqualTo(1));
        }

        [Test]
        public void TestCallbackPreLevelAndNowLevel()
        {
            int pre = 0, now = 0;
            _ec.Register((p, n) =>
            {
                pre = p;
                now = n;
            });
            Assert.That(pre, Is.EqualTo(1));
            Assert.That(now, Is.EqualTo(1));

            _ec.Unregister(IncrementCallback);
            _ec.Add(210);
            Assert.That(pre, Is.EqualTo(1));
            Assert.That(now, Is.EqualTo(3));
        }

        //Sample for callback
        void IncrementCallback(int preLevel, int nowLevel)
        {
            _counter++;
        }
    }
}