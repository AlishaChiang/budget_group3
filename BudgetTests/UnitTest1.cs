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
    public void query_whole_month()
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
    
    [Test]
    public void query_partial_month()
    {
        _budgetRepo.GetAll().Returns(new List<BudgetModel>
        {
            new BudgetModel(yearMonth: "202301", amount: 3100)
        });
        var query = _budgetService.Query(new DateTime(2023, 01, 01), new DateTime(2023, 01, 08));
        decimal expected = 800;
        Assert.That(query, Is.EqualTo(expected));
        Assert.Pass();
    }   
    
    [Test]
    public void query_cross_month()
    {
        _budgetRepo.GetAll().Returns(new List<BudgetModel>
        {
            new BudgetModel(yearMonth: "202301", amount: 3100),new BudgetModel(yearMonth: "202302", amount: 2800),
            
        });
        var query = _budgetService.Query(new DateTime(2023, 01, 01), new DateTime(2023, 02, 08));
        decimal expected = 3900;
        Assert.That(query, Is.EqualTo(expected));
        Assert.Pass();
    }
    [Test]
    public void query_no_data()
    {
        _budgetRepo.GetAll().Returns(new List<BudgetModel>
        {
            new BudgetModel(yearMonth: "202301", amount: 3100)
            
        });
        var query = _budgetService.Query(new DateTime(2023, 02, 01), new DateTime(2023, 02, 08));
        decimal expected = 0;
        Assert.That(query, Is.EqualTo(expected));
        Assert.Pass();
    }
    [Test]
    public void query_invalid_date()
    {
        _budgetRepo.GetAll().Returns(new List<BudgetModel>
        {
            new BudgetModel(yearMonth: "202301", amount: 3100),
            new BudgetModel(yearMonth: "202302", amount: 280)
            
        });
        var query = _budgetService.Query(new DateTime(2023, 02, 01), new DateTime(2023, 01, 08));
        decimal expected = 0;
        Assert.That(query, Is.EqualTo(expected));
        Assert.Pass();
    }
}