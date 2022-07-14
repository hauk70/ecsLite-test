using Leopotam.EcsLite;

namespace EcsTest
{
    public class Utils
    {
        public static T FetchFirst<T>(EcsWorld world) where T : struct
        {
            var filter = world.Filter<T>()
                .End();
            foreach (var targetEntity in filter)
            {
                return world.GetPool<T>().Get(targetEntity);
            }

            return default;
        }
    }
}