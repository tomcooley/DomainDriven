namespace DomainDriven.Repository
{
    public interface IPagedQuery<T>
    {
        PagedResult<T> Execute(int skip, int take);
    }
}