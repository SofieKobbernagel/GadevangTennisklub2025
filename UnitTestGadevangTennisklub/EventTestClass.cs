using GadevangTennisklub2025.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGadevangTennisklub
{
    [TestClass]
    public sealed class EventTestClass
    {
        Event ev = new Event(10,"tennis",new DateTime(2025,5,12,14,0,0),"spil for pokker", 10);
        [TestMethod]
        public void MakeEmptyEventDates() 
        {
            Event e = new Event();

            Assert.IsNotNull(e.Date);
        }
        [TestMethod]

        public void MakeEventDates()
        {
           
            
            Assert.IsTrue(new DateTime(2025, 5, 12, 14, 0, 0)== ev.Date);
        }
        [TestMethod]

        public void MakeEventId()
        {
            Assert.IsTrue(10==ev.Id);
        }
        [TestMethod]

        public void EventsEdit()
        {
            ev.Title = "spil";
            ev.Date=ev.Date.AddDays(1);

            Assert.AreSame(ev.Title,"spil");
            Assert.AreNotSame(ev.Date, new DateTime(2025, 5, 12, 14, 0, 0));
        }
        [TestMethod]

        public void EmptyEventId()
        {
            Event e = new Event();

            Assert.IsTrue(e.Id == 0);
        }

    }
    }
