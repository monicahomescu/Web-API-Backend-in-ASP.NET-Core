using Microsoft.EntityFrameworkCore;

namespace hwSDI.Models
{
    public class DefaultData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ItemContext(serviceProvider.GetRequiredService<DbContextOptions<ItemContext>>()))
            {
                DataMovies(context);
                DataGuests(context);
                DataScreenings(context);
            }
        }

        public static void DataMovies(ItemContext context)
        {
            if (context.Movies.Any())
                return;

            context.Movies.AddRange(
                new Movie
                {
                    Title = "The Social Network",
                    ReleaseYear = 2010,
                    Genre = "Drama",
                    Producer = "Scott Rudin",
                    LengthMinutes = 121
                },
                new Movie
                {
                    Title = "Joker",
                    ReleaseYear = 2019,
                    Genre = "Thriller",
                    Producer = "Todd Phillips",
                    LengthMinutes = 122
                },
                new Movie
                {
                    Title = "Black Panther",
                    ReleaseYear = 2018,
                    Genre = "Action",
                    Producer = "Kevin Feige",
                    LengthMinutes = 135
                },
                new Movie
                {
                    Title = "Parasite",
                    ReleaseYear = 2019,
                    Genre = "Thriller",
                    Producer = "Bong Joon-ho",
                    LengthMinutes = 132
                },
                new Movie
                {
                    Title = "The Shawshank Redemption",
                    ReleaseYear = 1994,
                    Genre = "Drama",
                    Producer = "Niki Marvin",
                    LengthMinutes = 142
                },
                new Movie
                {
                    Title = "The Dark Knight",
                    ReleaseYear = 2008,
                    Genre = "Action",
                    Producer = "Christopher Nolan",
                    LengthMinutes = 152
                },
                new Movie
                {
                    Title = "Forrest Gump",
                    ReleaseYear = 1994,
                    Genre = "Drama",
                    Producer = "Wendy Finerman",
                    LengthMinutes = 142
                },
                new Movie
                {
                    Title = "The Matrix",
                    ReleaseYear = 1999,
                    Genre = "Action",
                    Producer = "Joel Silver",
                    LengthMinutes = 136
                },
                new Movie
                {
                    Title = "Inception",
                    ReleaseYear = 2010,
                    Genre = "Action",
                    Producer = "Christopher Nolan",
                    LengthMinutes = 148
                },
                new Movie
                {
                    Title = "The Godfather",
                    ReleaseYear = 1972,
                    Genre = "Drama",
                    Producer = "Albert S. Ruddy",
                    LengthMinutes = 175
                }
            );

            context.SaveChanges();
        }

        public static void DataGuests(ItemContext context)
        {
            if (context.Guests.Any())
                return;

            context.Guests.AddRange(
                new Guest
                {
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "555-1234",
                    Email = "johndoe@example.com",
                    Age = 36
                },
                new Guest
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    PhoneNumber = "555-5678",
                    Email = "janesmith@example.com",
                    Age = 28
                },
                new Guest
                {
                    FirstName = "Mike",
                    LastName = "Brown",
                    PhoneNumber = "555-2468",
                    Email = "mikebrown@example.com",
                    Age = 45
                },
                new Guest
                {
                    FirstName = "Emily",
                    LastName = "Garcia",
                    PhoneNumber = "555-7890",
                    Email = "emilygarcia@example.com",
                    Age = 22
                },
                new Guest
                {
                    FirstName = "David",
                    LastName = "Johnson",
                    PhoneNumber = "555-1357",
                    Email = "davidjohnson@example.com",
                    Age = 31
                },
                new Guest
                {
                    FirstName = "Sarah",
                    LastName = "Williams",
                    PhoneNumber = "555-3691",
                    Email = "sarahwilliams@example.com",
                    Age = 27
                },
                new Guest
                {
                    FirstName = "Michael",
                    LastName = "Lee",
                    PhoneNumber = "555-8024",
                    Email = "michaellee@example.com",
                    Age = 50
                },
                new Guest
                {
                    FirstName = "Lauren",
                    LastName = "Martinez",
                    PhoneNumber = "555-2468",
                    Email = "laurenmartinez@example.com",
                    Age = 24
                },
                new Guest
                {
                    FirstName = "Adam",
                    LastName = "Taylor",
                    PhoneNumber = "555-0987",
                    Email = "adamtaylor@example.com",
                    Age = 29
                },
                new Guest
                {
                    FirstName = "Rachel",
                    LastName = "Nguyen",
                    PhoneNumber = "555-7777",
                    Email = "rachelnguyen@example.com",
                    Age = 32
                }
            );

            context.SaveChanges();
        }

        public static void DataScreenings(ItemContext context)
        {
            if (context.Screenings.Any())
                return;

            var movie = context.Movies.First() ?? throw new Exception("No movies found!");
            int movieId = movie.MovieID;

            context.Screenings.AddRange(
                new Screening
                {
                    Location = "Cinema City",
                    Room = 3,
                    Seats = 150,
                    Date = "2023-04-05",
                    Time = "18:35",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "AMC Theatres",
                    Room = 1,
                    Seats = 100,
                    Date = "2023-04-06",
                    Time = "15:45",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "Regal Cinemas",
                    Room = 5,
                    Seats = 200,
                    Date = "2023-04-07",
                    Time = "20:00",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "Cinemark Theatres",
                    Room = 2,
                    Seats = 120,
                    Date = "2023-04-08",
                    Time = "17:00",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "Vue Cinemas",
                    Room = 6,
                    Seats = 180,
                    Date = "2023-04-09",
                    Time = "21:15",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "string",
                    Room = 0,
                    Seats = 10000000,
                    Date = "string",
                    Time = "string",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "Empire Theatres",
                    Room = 7,
                    Seats = 220,
                    Date = "2023-04-11",
                    Time = "16:45",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "AMC Theatres",
                    Room = 3,
                    Seats = 150,
                    Date = "2023-04-12",
                    Time = "18:30",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "Cinema City",
                    Room = 1,
                    Seats = 100,
                    Date = "2023-04-13",
                    Time = "15:45",
                    Movie = movie,
                    MovieID = movieId
                },
                new Screening
                {
                    Location = "Regal Cinemas",
                    Room = 6,
                    Seats = 180,
                    Date = "2023-04-14",
                    Time = "21:15",
                    Movie = movie,
                    MovieID = movieId
                }
            );

            context.SaveChanges();
        }
    }
}
