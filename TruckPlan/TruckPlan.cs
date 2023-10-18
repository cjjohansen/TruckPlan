namespace TruckPlanning
{
    public record TruckPlanId(Guid Id);

    public class TruckPlan
    {
        public TruckPlanId  Id { get; init; }
        public DateOnly PlannedDate { get; protected set; }
        public Driver Driver { get; protected set; }

        public Truck Truck { get; protected set; }

        private TruckRoute _route;

        public TruckRoute Route { get { return _route; }  }



        protected TruckPlan(TruckPlanId id, Driver driver, Truck truck, DateOnly plannedDate)
        {
            Id = id;
            Driver = driver;
            Truck = truck;
            this.PlannedDate = plannedDate;

            _route = TruckRoute.Create();
        }

        public static TruckPlan CreateFrom(TruckPlanId id, Driver driver, Truck truck, DateOnly plannedDate  ) {

            return new TruckPlan(id, driver, truck, plannedDate);
        }


        public TruckPlan WithRoute( TruckRoute route )
        {
            _route = route;

            return this;
        }
    }  
}
