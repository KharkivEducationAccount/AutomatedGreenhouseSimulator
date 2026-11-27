using Models.Entities;
using System.Collections.Concurrent;

namespace GreenhouseScada.Web.Services
{
    public class ActuatorStateCache
    {
        private readonly ConcurrentDictionary<string, ActuatorState> _states = new();

        public void Update(ActuatorState state)
        {
            _states[state.Topic] = state;
        }

        public List<ActuatorState> GetAll()
        {
            return _states.Values.ToList();
        }
    }
}