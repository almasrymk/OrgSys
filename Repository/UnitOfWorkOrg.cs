using Entity;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Repository
{
    public class UnitOfWorkOrg
    {
        public string Schema;

        public UnitOfWorkOrg(string _Schema)
        {
            Schema = _Schema;
        }
       
        private static Dictionary<string , object> Repositories { get; set; }

        public CurdOrg<Entity> GetRepo<Entity>() where Entity : BaseModel
        {
            string Key = typeof(Entity).Name + Schema;

            var repo = Repositories.GetValueOrDefault(Key);
            if (repo == null)
            {
                var Repository = CreateRepository(typeof(Entity).Name);
                Repositories.Add(Key, Repository);
            }

            return (CurdOrg<Entity>)repo;
        }

        public RepoT GetRepo<RepoT,Entity>() where Entity : BaseModel where RepoT : CurdOrg<Entity>
        {
            string Key = typeof(Entity).Name + Schema;

            var repo = Repositories.GetValueOrDefault(Key);
            if (repo == null)
            {              
                var Repository = CreateRepository(typeof(Entity).Name);
                Repositories.Add(Key, Repository);
            }

            return (RepoT) repo;
        }

        private object CreateRepository(string TypeName)
        {
            Assembly assembly = Assembly.Load("Repository");
            var RepositoryName = $"Repository.{TypeName}Repo";
            var type = assembly.GetType(RepositoryName);
            return Activator.CreateInstance(type, Schema);
        }
    }
}