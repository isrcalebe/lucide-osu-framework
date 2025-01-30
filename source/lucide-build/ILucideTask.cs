using System.Threading.Tasks;

namespace Lucide.Build;

public interface ILucideTask
{
    Task<bool> RunAsync(string[] args);
}
