using Budget;
using NSubstitute;

namespace BudgetTests;

public class Tests
{
    private IBudgetRepo _budgetRepo;
    private BudgetService _budgetService;

    [SetUp]
    public void Setup()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }

    [Test]
    public void Test1()
    {
        _budgetRepo.GetAll().Returns(new List<BudgetModel>
        {
            new BudgetModel(yearMonth: "202301", amount: 3100)
        });
        var query = _budgetService.Query(new DateTime(2023, 01, 01), new DateTime(2023, 01, 31));
        decimal expected = 3100;
        Assert.That(query, Is.EqualTo(expected));
        Assert.Pass();
    }
}