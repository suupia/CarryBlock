using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Exp.Tests
{
    public class ExpTests
    {
        private ExpContainer _ec;
        private int _counter;
        
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
            _ec = new ExpContainer(initialExp: 100, initialThreshold: 100);
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
            _ec.Register(IncrementCallback, callImmediately: false);
            Assert.That(_counter, Is.EqualTo(0));
            
            
            _ec.Trigger();
            Assert.That(_counter, Is.EqualTo(1));
        }
        
        [Test]
        public void TestRegisterMultiple()
        {
            _ec.Register(IncrementCallback, callImmediately: false);
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
            int pre  =0, now = 0;
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
        
        void IncrementCallback(int preLevel, int nowLevel)
        {
            _counter++;
        }
    }

}

