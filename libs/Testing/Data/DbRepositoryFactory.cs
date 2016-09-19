using Shared;

namespace Testing.Data
{
    public static class DbRepositoryFactory
    {
        public static IDbRepository GetRepository()
        {
            if(GlobalOptions.DababaseMode == DbMode.Local)
            {
                return new LocalDbRepository();
            }
            return new RemoteDbRepository();
        }
    }
}
