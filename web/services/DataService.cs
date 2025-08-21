using OpsWatcher.Web.Models;

namespace OpsWatcher.Web.services;

public class DataService
{
    private List<Item> _items;

    public List<Item> GetItems()
    {
        if (_items == null) return new List<Item>();
        return _items;
    }

    public void AddItems(List<Item> items)
    {
        _items = items;
    }
}