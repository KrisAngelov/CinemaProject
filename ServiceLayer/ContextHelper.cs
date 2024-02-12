using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;

namespace ServiceLayer
{
    public static class ContextHelper
    {
        private static CinemaDbContext dbContext;
        private static MovieContext movieContext;
        private static ReviewContext reviewContext;
        private static SeatContext seatContext;
        private static ShowtimeContext showtimeContext;
        private static TicketContext ticketContext;

        public static CinemaDbContext GetDbContext()
        {
            if (dbContext == null)
            {
                SetDbContext();
            }

            return dbContext;
        }

        public static void SetDbContext()
        {
            dbContext = new CinemaDbContext();
        }

        public static MovieContext GetMovieContext()
        {
            if (movieContext == null)
            {
                SetMovieContext();
            }

            return movieContext;
        }

        public static void SetMovieContext()
        {
            movieContext = new MovieContext(GetDbContext());
        }

        public static ReviewContext GetReviewContext()
        {
            if (reviewContext == null)
            {
                SetReviewContext();
            }

            return reviewContext;
        }

        public static void SetReviewContext()
        {
            reviewContext = new ReviewContext(GetDbContext());
        }

        public static SeatContext GetSeatContext()
        {
            if (seatContext == null)
            {
                SetSeatContext();
            }

            return seatContext;
        }

        public static void SetSeatContext()
        {
            seatContext = new SeatContext(GetDbContext());
        }

        public static ShowtimeContext GetShowtimeContext()
        {
            if (showtimeContext == null)
            {
                SetShowtimeContext();
            }

            return showtimeContext;
        }

        public static void SetShowtimeContext()
        {
            showtimeContext = new ShowtimeContext(GetDbContext());
        }

        public static TicketContext GetTicketContext()
        {
            if (ticketContext == null)
            {
                SetTicketContext();
            }

            return ticketContext;
        }

        public static void SetTicketContext()
        {
            ticketContext = new TicketContext(GetDbContext());
        }
    }
}