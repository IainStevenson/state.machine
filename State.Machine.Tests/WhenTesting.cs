using System;
using NUnit.Framework;

namespace State.Machine.Tests
{
    public class WhenTesting<T> where T : class
    {
        protected T UnitUnderTest;

        [SetUp]
        public void Setup()
        {
            GivenThat();
            When();
        }

        /// <summary>
        ///     Override this method and omit the base.GiventThat() to create a UnitUnderTest taht has paramterised constructors.
        /// </summary>
        protected virtual void GivenThat()
        {
            UnitUnderTest = Activator.CreateInstance<T>();
        }

        /// <summary>
        ///     Override this method to produce Actions in derived test classes. Include base.When() as needed.
        /// </summary>
        protected virtual void When()
        {
        }
    }
}