namespace Deste.Domain.Models;

public class ListResponse<TEntity>
{
    public int Total { get; set; }= 0;
    public List<TEntity> Items { get; set; } = new();

    public ListResponse(int total,List<TEntity> items)
    {
        Total = total;
        Items = items;
    }
    
}