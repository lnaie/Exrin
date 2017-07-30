using System.Collections.Generic;

namespace Exrin.Abstraction
{
    public interface IVisualDisplay
    {
        void Display(string key, IDictionary<string, string> values);
    }
}
