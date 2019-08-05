using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapsApp.Services.Interfaces
{
    public interface IAsyncInitialization
    {
        Task Initialize { get; }
    }    
}
