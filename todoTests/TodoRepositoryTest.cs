using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using todoCore.Domain;

namespace todoTests
{
    [TestFixture]
    public class SimpletTest
    {
        [Test]
        public void first_test()
        {
            Assert.True(1 == (3 - 2));
        }
    }

    [TestFixture]
    public class TodoRespositoryTestWithSqliteSessionFactory
    {
        InMemorySqLiteSessionFactory sqliteSessionFactory;

        [SetUp]
        public void init()
        {
            sqliteSessionFactory = new InMemorySqLiteSessionFactory();
        }
        [TearDown]
        public void free()
        {
            sqliteSessionFactory.Dispose();
        }

        [Test]
        public void createTodo_byDefault_shouldCreate()
        {
            Todo f = new Todo
            {
                Name = "todo developement"
            };

            using (ISession session = sqliteSessionFactory.Session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Assert.That(f.Id == 0);

                    session.SaveOrUpdate(f);

                    transaction.Commit();

                    Assert.That(f.Id > 0);
                }

            }
        }
    }


        
    public class InMemorySqLiteSessionFactory : IDisposable
    {
        private Configuration _configuration;
        private ISessionFactory _sessionFactory;

        public ISession Session { get; set; }

        public InMemorySqLiteSessionFactory()
        {
            _sessionFactory = CreateSessionFactory();
            Session = _sessionFactory.OpenSession();
            ExportSchema();
        }
        public ISession reopen()
        {
            return _sessionFactory.OpenSession();
        }
        private void ExportSchema()
        {
            var export = new SchemaExport(_configuration);

            using (var file = new FileStream(@"c:\temp\create.objects.sql", FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(file))
                {
                    export.Execute(true, true, false, Session.Connection, sw);
                    sw.Close();
                }
            }
        }

        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                     .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                     .Mappings(m =>
                     {
                         m.FluentMappings.Conventions.Setup(c => c.Add(AutoImport.Never()));
                         m.FluentMappings.Conventions.AddAssembly(Assembly.GetExecutingAssembly());
                         m.HbmMappings.AddFromAssembly(Assembly.GetExecutingAssembly());

                         var assembly = Assembly.Load("todoCore");
                         m.FluentMappings.Conventions.AddAssembly(assembly);
                         m.FluentMappings.AddFromAssembly(assembly);
                         m.HbmMappings.AddFromAssembly(assembly);

                     })
                     .ExposeConfiguration(cfg => _configuration = cfg)
                     .BuildSessionFactory();
        }

        public void Dispose()
        {
            Session.Dispose();
            _sessionFactory.Close();
        }
    }
}
