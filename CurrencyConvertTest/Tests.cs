using CurrencyConvertLib;
using NUnit.Framework;
using System;

namespace CurrencyConvertTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConvert()
        {

            var conversionRates = new[]{
                 Tuple.Create(KnownCurrency.USD , KnownCurrency.EUR , 0.93d),
                 Tuple.Create(KnownCurrency.EUR , KnownCurrency.AED , 3.94d),
                 Tuple.Create(KnownCurrency.AED , KnownCurrency.CAD , 0.35d),
                 Tuple.Create(KnownCurrency.CAD , KnownCurrency.IRR , 33457.35d),
                 Tuple.Create(KnownCurrency.BTC , KnownCurrency.USD , 31617.71d),
                 Tuple.Create(KnownCurrency.ANG , KnownCurrency.AUD , 0.77)};

            var converter = CurrencyConverter.Instance();
            converter.UpdateConfiguration(conversionRates);


            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.USD, KnownCurrency.EUR, 1d), 2), 0.93);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.EUR, KnownCurrency.USD, 1d), 2), 1.08);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.EUR, KnownCurrency.AED, 1d), 2), 3.94);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.AED, KnownCurrency.CAD, 1d), 2), 0.35);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.CAD, KnownCurrency.IRR, 1d), 2), 33457.35);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.BTC, KnownCurrency.USD, 1d), 2), 31617.71);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.ANG, KnownCurrency.AUD, 1d), 2), 0.77);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.USD, KnownCurrency.IRR, 1d), 2), 42908.05);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.IRR, KnownCurrency.USD, 1d), 2), 0);
            //----------------------------------------
            Assert.AreEqual(Math.Round(converter.Convert(KnownCurrency.EUR, KnownCurrency.IRR, 1d), 2), 46137.69);

            Assert.Pass();
        }
    }
}