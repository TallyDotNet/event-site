using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using EventSite.Domain.Commands;
using EventSite.Domain.Infrastructure;
using EventSite.Infrastructure.Data.Export;
using EventSite.Infrastructure.Helpers;
using Rhino.Mocks;
using SpecEasy;

namespace EventSite.Tests.Infrastructure.Helpers
{
    public class ExcelFileBuilderSpecs : Spec<ExcelFileBuilder> {
        private string targetFileName;
        private Query<Page<TestType>> testQuery;
        private IExcelExporter<TestType> exportCommand; 
        private ITargetFile targetFile;

        protected override void BeforeEachExample()
        {
            base.BeforeEachExample();
            targetFile = Mock<ITargetFile>();
            exportCommand = Mock<IExcelExporter<TestType>>();
            testQuery = Mock <Query<Page<TestType>>>();
        }

        public void Test() {
            When("building an excel file result", () => SUT.Build(targetFile, testQuery, exportCommand));

            Given("the query returns less than 1 page of results.",() => Get<IApplicationBus>().Expect(bus => bus.Query(testQuery))
                .Return(new Page<TestType> {CurrentPage = 1, Items = new[] {new TestType(1, "one")}, TotalPages = 1}).Repeat.Once()).Verify(() =>
                {
                    Then("it should only invoke the query once.", () => testQuery.VerifyAllExpectations());
                    //Then("it should send the result obtained from the query to the excel exporter", () => exportCommand.AssertWasCalled(e => e.Process(), method => method.Repeat.Once()));
                });
        }

        public class TestType
        {
            public TestType(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
