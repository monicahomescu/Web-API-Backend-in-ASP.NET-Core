using hwSDI.Models;
using hwSDI.Controllers;
using hwSDI.Validation;
using Moq;
using Moq.EntityFrameworkCore;

namespace Tests
{
    public class MovieTests
    {
        private Mock<ItemContext> _contextMock;
        private Mock<Valid> _validationMock;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<ItemContext>();
            _validationMock = new Mock<Valid>();
        }

        [Test]
        public async Task TestGetFilteredMoviesByReleaseYear()
        {
            var movies = new List<Movie>
            {
                new Movie
                {
                    MovieID = 1,
                    Title = "title1",
                    ReleaseYear = 2021,
                    Genre = "genre1",
                    Producer = "producer1",
                    LengthMinutes = 157
                },
                new Movie
                {
                    MovieID = 2,
                    Title = "title2",
                    ReleaseYear = 2023,
                    Genre = "genre2",
                    Producer = "producer2",
                    LengthMinutes = 139
                },
                new Movie
                {
                    MovieID = 3,
                    Title = "title3",
                    ReleaseYear = 2022,
                    Genre = "genre3",
                    Producer = "producer3",
                    LengthMinutes = 157
                },
                new Movie
                {
                    MovieID = 4,
                    Title = "title4",
                    ReleaseYear = 2015,
                    Genre = "genre4",
                    Producer = "producer4",
                    LengthMinutes = 125
                },
                new Movie
                {
                    MovieID = 5,
                    Title = "title5",
                    ReleaseYear = 2020,
                    Genre = "genre5",
                    Producer = "producer5",
                    LengthMinutes = 160
                }
            };

            _contextMock.Setup(x => x.Movies).ReturnsDbSet(movies);
            var ctrl = new MovieController(_contextMock.Object, _validationMock.Object);
            var res = await ctrl.GetFilteredMoviesByReleaseYear(2021);

            NUnit.Framework.Assert.AreEqual(2, res.Count);
            NUnit.Framework.Assert.AreEqual(2, res[0].MovieID);
            NUnit.Framework.Assert.AreEqual(3, res[1].MovieID);
            NUnit.Framework.Assert.AreEqual(2023, res[0].ReleaseYear);
            NUnit.Framework.Assert.AreEqual(2022, res[1].ReleaseYear);
        }

        [Test]
        public async Task TestGetMovieWithAvgScreeningSeat()
        {
            var movies = new List<Movie>
            {
                new Movie
                {
                    MovieID = 1,
                    Title = "title1",
                    ReleaseYear = 2021,
                    Genre = "genre1",
                    Producer = "producer1",
                    LengthMinutes = 157
                },
                new Movie
                {
                    MovieID = 2,
                    Title = "title2",
                    ReleaseYear = 2023,
                    Genre = "genre2",
                    Producer = "producer2",
                    LengthMinutes = 139
                }
            };

            var screenings = new List<Screening>
            {
                new Screening
                {
                    ScreeningID = 1,
                    Location = "location1",
                    Room = 1,
                    Seats = 60,
                    Date = "date1",
                    Time = "time1",
                    MovieID = 2
                },
                new Screening
                {
                    ScreeningID = 2,
                    Location = "location2",
                    Room = 2,
                    Seats = 40,
                    Date = "date2",
                    Time = "time2",
                    MovieID = 2
                },
                new Screening
                {
                    ScreeningID = 3,
                    Location = "location3",
                    Room = 3,
                    Seats = 80,
                    Date = "date3",
                    Time = "time3",
                    MovieID = 1
                },
                new Screening
                {
                    ScreeningID = 4,
                    Location = "location4",
                    Room = 4,
                    Seats = 60,
                    Date = "date4",
                    Time = "time4",
                    MovieID = 1
                },
                new Screening
                {
                    ScreeningID = 5,
                    Location = "location5",
                    Room = 5,
                    Seats = 40,
                    Date = "date5",
                    Time = "time5",
                    MovieID = 1
                },
            };

            _contextMock.Setup(x => x.Movies).ReturnsDbSet(movies);
            _contextMock.Setup(x => x.Screenings).ReturnsDbSet(screenings);
            var ctrl = new MovieController(_contextMock.Object, _validationMock.Object);
            var res = await ctrl.GetMovieWithAvgScreeningSeat();

            NUnit.Framework.Assert.AreEqual(2, res.Count);
            NUnit.Framework.Assert.AreEqual(2, res[0].MovieID);
            NUnit.Framework.Assert.AreEqual(1, res[1].MovieID);
            NUnit.Framework.Assert.AreEqual(50, res[0].AvgScreeningSeatNo);
            NUnit.Framework.Assert.AreEqual(60, res[1].AvgScreeningSeatNo);
        }
    }
}
