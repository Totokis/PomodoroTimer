using NUnit.Framework;

namespace Tests.EditMode
{
    public class TimerTests
    {
        public class TimerSetterMethod
        {
            [Test]
            public void Set_Pause_Time()
            {
                var value = 59;
                var timer = new PomodoroTimerModel();
                timer.SetPauseTime(value);
                Assert.AreEqual(value,timer.GetPauseTime());
            }
            [Test]
            public void Set_Work_Time()
            {
                var value = 59;
                var timer = new PomodoroTimerModel();
                timer.SetWorkTime(value);
                Assert.AreEqual(value,timer.GetWorkTime());
            }
            [Test]
            public void Set_More_Than_99_In_Pause()
            {
                var timer = new PomodoroTimerModel();
                var value = 200;
                timer.SetPauseTime(value);
                Assert.AreEqual(timer.MaxPauseValue, timer.GetPauseTime());
            }
            
            [Test]
            public void Set_More_Than_99_In_Work()
            {
                var timer = new PomodoroTimerModel();
                var value = 200;
                timer.SetWorkTime(value);
                Assert.AreEqual(timer.MaxWorkValue, timer.GetWorkTime());
            }

            [Test]
            public void Set_0_In_Pause()
            {
                var timer = new PomodoroTimerModel();
                var value = 0;
                timer.SetPauseTime(value);
                Assert.AreEqual(timer.MinPauseTime, timer.GetPauseTime());
            }
            
        }
    }
}
