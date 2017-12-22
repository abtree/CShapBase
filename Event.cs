using System.Windows;
using System;

namespace Event_
{
    //定义传递事件的Event
    public class CarInfoEventsArgs: EventArgs
    {
        public CarInfoEventsArgs(string car)
        {
            this.Car = car;
        }

        public string Car
        {
            get; private set;
        }
    }

    //定义一个事件的发布者
    public class CarDealer
    {
        public event EventHandler<CarInfoEventsArgs> NewCarInfo; //事件绑定句柄

        public void NewCar(string car)
        {
            Console.WriteLine("CarDealer, new car {0}", car);
            RaiseNewCarInfo(car);
        }

        protected virtual void RaiseNewCarInfo(string car)
        {
            EventHandler<CarInfoEventsArgs> newCarInfo = NewCarInfo;
            //检查事件是否有人订阅
            if (newCarInfo != null)
            {
                //触发事件回调
                newCarInfo(this, new CarInfoEventsArgs(car));
            }
        }
    }
    //定义一个事件的接收者
    public class Consumer
    {
        private string name;

        public Consumer(string name)
        {
            this.name = name;
        }

        public void NewCarIsHere(object sender, CarInfoEventsArgs e)
        {
            Console.WriteLine("{0}: car {1} is new", name, e.Car);
        }
    }

    #region 弱事件版本(用不起)
    //弱事件是为了方便系统垃圾回收 在事件发布者和接受者中间在加了一层
    //public class WeakCarInfoEventManager : WeakEventManager
    //{
    //    public static WeakCarInfoEventManager CurrentManager
    //    {
    //        get
    //        {
    //            var manager = GetCurrentManager(typeof(WeakCarInfoEventManager)) as WeakCarInfoEventManager;
    //            if(manager == null)
    //            {
    //                manager = new Event.WeakCarInfoEventManager();
    //                SetCurrentManager(typeof(WeakCarInfoEventManager), manager);
    //            }
    //            return manager;
    //        }
    //    }
    //    public static void AddListener(object source, IWeakEventListener listener)
    //    {
    //        CurrentManager.ProtectedAddListener(source, listener);
    //    }

    //    public static void RemoveListener(object source, IWeakEventListener listener)
    //    {
    //        CurrentManager.ProtectedRemoveListener(source, listener);
    //    }

    //    protected override void StartListening(object source)
    //    {
    //        (source as CarDealer).NewCarInfo += CarDealer_NewCarInfo;
    //    }

    //    void CarDealer_NewCarInfo(object sender, CarInfoEventsArgs e)
    //    {
    //        DeliverEvent(sender, e);
    //    }

    //    protected override void StopListener(object source)
    //    {
    //        (source as CarDealer).NewCarInfo -= CarDealer_NewCarInfo;
    //    }
    //}

    ////修改Consumer
    //public class Consumer1 : IWeakEventListener
    //{
    //    private string name;

    //    public Consumer(string name)
    //    {
    //        this.name = name;
    //    }

    //    public void NewCarIsHere(object sender, CarInfoEventsArgs e)
    //    {
    //        Console.WriteLine("{0}: car {1} is new", name, e.Car);
    //    }

    //    //绑定函数
    //    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    //    {
    //        NewCarIsHere(sender, e as CarInfoEventsArgs);
    //        return true;
    //    }
    //}
    #endregion
    class Event_
    {
        public static void Main(string[] args)
        {
            //先定义一个发布事件的
            var dealer = new CarDealer();
            //再定义一个接收事件的
            var michel = new Consumer("michel");
            //绑定接收事件
            dealer.NewCarInfo += michel.NewCarIsHere;

            //发布一辆新车
            dealer.NewCar("Ferrari");

            var sebastion = new Consumer("Sebastion");
            dealer.NewCarInfo += sebastion.NewCarIsHere;

            dealer.NewCar("Mercedes");

            //取消事件绑定
            dealer.NewCarInfo -= michel.NewCarIsHere;

            dealer.NewCar("Red Bull Racing");

            #region 弱事件版本(用不起)
            ////先定义一个发布事件的
            //var dealer = new CarDealer();
            ////再定义一个接收事件的
            //var michel = new Consumer("michel");
            ////绑定接收事件
            //WeakCarInfoEventManager.AddListener(dealer, michel);

            ////发布一辆新车
            //dealer.NewCar("Ferrari");

            //var sebastion = new Consumer("Sebastion");
            //WeakCarInfoEventManager.AddListener(dealer, sebastion);

            //dealer.NewCar("Mercedes");

            ////取消事件绑定
            //WeakCarInfoEventManager.RemoveListener(dealer, michel);

            //dealer.NewCar("Red Bull Racing");
            #endregion

            #region 弱事件模版 新的方法 好像也用不起（留待研究）
            ////先定义一个发布事件的
            //var dealer1 = new CarDealer();
            ////再定义一个接收事件的
            //var michel1 = new Consumer("michel");
            ////绑定接收事件
            //WeakEventManager<CarDealer, CarInfoEventsArgs>.AddHandler(dealer1, "NewCarInfo", michel1.NewCarIsHere);

            ////发布一辆新车
            //dealer1.NewCar("Ferrari");

            //var sebastion1 = new Consumer("Sebastion");
            //WeakEventManager<CarDealer, CarInfoEventsArgs>.AddHandler(dealer1, "NewCarInfo", sebastion1.NewCarIsHere);

            //dealer1.NewCar("Mercedes");

            ////取消事件绑定
            //WeakEventManager<CarDealer, CarInfoEventsArgs>.RemoveHandler(dealer1, "NewCarInfo", michel1.NewCarIsHere);

            //dealer1.NewCar("Red Bull Racing");
            #endregion
        }
    }
}
