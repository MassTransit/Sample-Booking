namespace TrackingService
{
    using System.Data.Entity;


    public class TrackingEventDbContext :
        DbContext
    {
        public TrackingEventDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new TrackingEventMap());
        }
    }
}