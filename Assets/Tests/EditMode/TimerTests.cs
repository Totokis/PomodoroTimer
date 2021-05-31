using NUnit.Framework;

namespace Tests.EditMode
{
    public class TimerTests
    {
        public class TimerSettersMethod
        {
            [Test]
            public void Pause_And_Work_Time_Default_Start_From_1()
            {
                var timer = new PomodoroTimerModel();
                Assert.AreEqual( 1,timer.GetWorkTime());
                Assert.AreEqual(1,timer.GetPauseTime());
            }
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
            
            [Test]
            public void Set_0_In_Work()
            {
                var timer = new PomodoroTimerModel();
                var value = 0;
                timer.SetWorkTime(value);
                Assert.AreEqual(timer.MinWorkValue, timer.GetWorkTime());
            }
            [Test]
            public void Set_Smaller_Than_0_Pause()
            {
                var timer = new PomodoroTimerModel();
                var value = -20;
                timer.SetPauseTime(value);
                Assert.AreEqual(timer.MinPauseTime, timer.GetPauseTime());
            }
            
            [Test]
            public void Set_Smaller_Than_0_Work()
            {
                var timer = new PomodoroTimerModel();
                var value = -20;
                timer.SetWorkTime(value);
                Assert.AreEqual(timer.MinWorkValue, timer.GetWorkTime());
            }
            
            [Test]
            public void Increment_By_Value_Pause_Time()
            {
                var timer = new PomodoroTimerModel();
                var previousValue = timer.GetPauseTime();
                var value = 10;
                timer.IncrementPause(value);
                Assert.AreEqual(previousValue + value,timer.GetPauseTime());
            }
            
            [Test]
            public void Increment_By_Value_Work_Time()
            {
                var timer = new PomodoroTimerModel();
                var previousValue = timer.GetWorkTime();
                var value = 10;
                timer.IncrementWork(value);
                Assert.AreEqual(previousValue + value,timer.GetWorkTime());
            }
            
            [Test]
            public void Decrement_By_Value_Pause_Time()
            {
                var timer = new PomodoroTimerModel();
                timer.SetPauseTime(50);
                var previousValue = timer.GetPauseTime();
                var value = 10;
                timer.DecrementPause(value);
                Assert.AreEqual(previousValue - value,timer.GetPauseTime());
            }
            
            [Test]
            public void Decrement_By_Value_Work_Time()
            {
                var timer = new PomodoroTimerModel();
                timer.SetWorkTime(50);
                var previousValue = timer.GetWorkTime();
                var value = 10;
                timer.DecrementWork(value);
                Assert.AreEqual(previousValue - value,timer.GetWorkTime());
            }
            
            [Test]
            public void Session_Number_Greater_Or_Equal_Than_1()
            {
                var timer = new PomodoroTimerModel();
                Assert.GreaterOrEqual(timer.NumberOfSessions,1);
            }
            
            [Test]
            public void Session_Number_Set_Always_Greater_Or_Equal_Than_1()
            {
                var timer = new PomodoroTimerModel();
                var value = -5;
                timer.SetNumberOfSessions(value);
                Assert.GreaterOrEqual(timer.NumberOfSessions,1);
            }
            
            [Test]
            public void Start_Timer_Twice_Not_Allowed()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                timer.CountDown();
                var preActualSession = timer.GetActualSession();
                timer.StartTimer();
                var actualSession = timer.GetActualSession();
                Assert.AreEqual(preActualSession,actualSession);
            }
        }

        public class CountersForMinutesAndSeconds
        {
            [Test]
            public void Double_Start_Check()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                Assert.AreEqual(timer.GetWorkTime(),timer.GetMinute());
            }
            [Test]
            public void Start_Counting_And_Check_Minute_Values()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                Assert.AreEqual(timer.GetWorkTime(),timer.GetMinute());
            }
            [Test]
            public void Start_Counting_And_Check_Seconds_Values()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                Assert.AreEqual(0,timer.GetSecond());
            }
            
            [Test]
            public void Start_Counting_And_Check_Session_Is_One()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                Assert.AreEqual(1,timer.GetActualSession());
            }
            [Test]
            public void Start_Counting_And_Check_State()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                Assert.AreEqual(true, timer.IsWorkTime);
            }

            [Test]
            public void Start_Work_Timer_And_Count_Down()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                var previousMinute = timer.GetMinute();
                timer.CountDown();
                var actualSecond = timer.GetSecond();
                var actualMinute = timer.GetMinute();
                Assert.AreEqual(59,actualSecond);
                Assert.AreEqual(previousMinute-1,actualMinute);
            }
            
            [Test]
            public void Set_Work_Timer_And_Count_Down()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                timer.SetTimer(59,59);
                var previousSecond = timer.GetSecond();
                var previousMinute = timer.GetMinute();
                timer.CountDown();
                var actualSecond = timer.GetSecond();
                var actualMinute = timer.GetMinute();
                Assert.AreEqual(previousSecond-1,actualSecond);
                Assert.AreEqual(previousMinute,actualMinute);
            }
            

            [Test]
            public void Pause_Timer()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                var previousSecond = timer.GetSecond();
                var previousMinute = timer.GetMinute();
                timer.PauseTimer();
                timer.CountDown();
                var actualSecond = timer.GetSecond();
                var actualMinute = timer.GetMinute();
                Assert.AreEqual(previousSecond,actualSecond);
                Assert.AreEqual(previousMinute,actualMinute);
            }
            
            [Test]
            public void Resume_Timer()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                timer.SetTimer(59,59);
                var previousSecond = timer.GetSecond();
                timer.PauseTimer();
                timer.ResumeTimer();
                timer.CountDown();
                var actualSecond = timer.GetSecond();
                Assert.AreEqual(previousSecond-1,actualSecond);
            }

            [Test]
            public void Timer_Gets_0_And_Is_Not_Work_Time()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                timer.SetTimer(0,0);
                timer.CountDown();
                Assert.IsFalse(timer.IsWorkTime);
            }
            
            [Test]
            public void Timer_Gets_0_And_Is_Pause_Time()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                timer.SetTimer(0,0);
                timer.CountDown();
                Assert.IsTrue(timer.IsPauseTime);
            }
            
            [Test]
            public void Pause_Timer_Gets_0_And_Is_Work_Time()
            {
                var timer = new PomodoroTimerModel();
                timer.SetPauseTime(10);
                timer.SetWorkTime(60);
                timer.StartTimer();
                timer.SetTimer(0,0);
                timer.CountDown();//work ends, pause starts
                timer.StartTimer();
                timer.SetTimer(0,0);
                timer.CountDown();//pause ends, work ends
                Assert.IsTrue(timer.IsWorkTime);
            }
            
            [Test]
            public void Work_Time_Ends_And_Start_Pause_Timer()
            {
                var timer = new PomodoroTimerModel();
                timer.SetPauseTime(10);
                timer.SetWorkTime(60);
                timer.StartTimer();
                timer.SetTimer(0,0);
                timer.CountDown();
                timer.StartTimer();
                Assert.AreEqual(timer.GetPauseTime(),timer.GetMinute());
            }
            
             
            [Test]
            public void Pause_Time_Ends_And_Start_Work_Timer()
            {
                var timer = new PomodoroTimerModel();
                timer.SetPauseTime(10);
                timer.SetWorkTime(60);
                timer.StartTimer();
                timer.SetTimer(0,0);
                timer.CountDown();//work ends, pause starts
                timer.StartTimer();
                timer.SetTimer(0,0);
                timer.CountDown();//pause ends, work ends
                timer.StartTimer();
                Assert.AreEqual(timer.GetWorkTime(),timer.GetMinute());
            }
            
            [Test]
            public void Count_Down_Disabled_When_Actual_Session_Is_0()
            {
                var timer = new PomodoroTimerModel();
                var preMinute = timer.GetMinute();
                var preSecond = timer.GetSecond();
                timer.CountDown();
                Assert.AreEqual(preSecond,timer.GetSecond());
            }

            [Test]
            public void Minutes_Ends_With_0()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                timer.SetTimer(0,1);
                timer.CountDown();
                timer.CountDown();
                Assert.AreEqual(0,timer.GetMinute());
                Assert.AreEqual(0,timer.GetSecond());
            }

            [Test]
            public void Count_Down_Dont_Switch_States()
            {
                var timer = new PomodoroTimerModel();
                timer.StartTimer();
                timer.SetTimer(0,1);
                timer.CountDown();
                timer.CountDown();
                var pauseTime=  timer.IsPauseTime;
                timer.CountDown();
                Assert.AreEqual(pauseTime,timer.IsPauseTime);
            }
            
            [Test]
            public void When_Sessions_Are_Done_Set_State_To_Done()
            {
                var timer = new PomodoroTimerModel();
                timer.SetPauseTime(10);
                timer.SetWorkTime(60);
                timer.StartTimer();
                timer.SetTimer(0,0);
                timer.CountDown();//work ends, pause starts
                Assert.IsTrue(timer.Done);
            }
        }
    }
}
