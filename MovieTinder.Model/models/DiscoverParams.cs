namespace MovieTinder.Model.models
{
    public class DiscoverParams
    {
        public SortType Sort;
        public SortDirectionType SortDirection;

        public enum SortType
        {
            popularity,
            release_date,
            revenue,
            primary_title,
            original_title,
            vote_average,
            vote_count
        }

        public enum SortDirectionType
        {
            desc,
            asc
        }
        public override string ToString() => $"&sort_by={Sort}.{SortDirection}";
    }
}