using StoreApp.Models;

namespace StoreApp.Modules;

public interface IConsoleModule
{
    public Logic.Logic Logic { get; set; }

    public void Default();
}

