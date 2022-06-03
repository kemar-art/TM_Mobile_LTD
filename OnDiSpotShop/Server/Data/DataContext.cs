namespace OnDiSpotShop.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)   
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(

                 new Product
                 {
                     Id = 1,
                     Name = "iPhone X",
                     Description = "The iPhone X (Roman numeral X pronounced ten, also known as iPhone 10)[11][12] is a smartphone designed, developed and marketed by Apple Inc. The 11th generation of the iPhone, it was available to pre-order on October 27, 2017, and was released on November 3, 2017. The naming of the iPhone X (skipping the iPhone 9) is to mark the 10th anniversary of the iPhone. ",
                     ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/32/IPhone_X_vector.svg",
                     Price = 100
                 },
                 new Product
                 {
                     Id = 2,
                     Name = "Laptop",
                     Description = "A laptop, laptop computer, or notebook computer is a small, portable personal computer (PC) with a screen and alphanumeric keyboard. Laptops typically have a clam shell form factor with the screen mounted on the inside of the upper lid and the keyboard on the inside of the lower lid, although 2-in-1 PCs with a detachable keyboard are often marketed as laptops or as having a laptop mode. Laptops are folded shut for transportation, and thus are suitable for mobile use.[1] Its name comes from lap, as it was deemed practical to be placed on a person's lap when being used. Today, laptops are used in a variety of settings, such as at work, in education, for playing games, web browsing, for personal multimedia, and for general home computer use. ",
                     ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/50/Macbook_Air_M1_Silver_PNG.png",
                     Price = 100
                 },
                 new Product
                 {
                     Id = 3,
                     Name = "Samsung Galaxy Note 10",
                     Description = "The Samsung Galaxy Note 10 (stylized as Samsung Galaxy Note10) is a line of Android-based phablets designed, developed, produced, and marketed by Samsung Electronics as part of the Samsung Galaxy Note series. They were unveiled on 7 August 2019, as the successors to the Samsung Galaxy Note 9.[3] Details about the phablets were widely leaked in the months leading up to the phablets' announcement.",
                     ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/aa/Samsung_Galaxy_Phone.jpg",
                     Price = 100
                 }
               );
        }

        public DbSet<Product> Products { get; set; }
    }
}
