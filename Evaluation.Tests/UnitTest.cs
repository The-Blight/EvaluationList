using EvaluationList.Domain.Entities.Scoring;
using EvaluationList.Domain.Entities.Staff;

namespace Evaluation.Tests;

using System;
using System.Linq;
using Xunit;

public class ExpertTests
{
    [Theory]
    [InlineData("Иван")]
    [InlineData("Анна-Мария")]
    [InlineData("John")]
    public void Expert_ValidFirstName_ShouldCreateSuccessfully(string validName)
    {
        var expert = new Expert
        {
            FirstName = validName,
            LastName = "Иванов"
        };

        Assert.Multiple
        (() => Assert.Equal(validName, expert.FirstName),
            () => Assert.Equal("Иванов", expert.LastName),
            () => Assert.NotEqual(Guid.Empty, expert.Id)
        );
    }

    [Theory]
    [InlineData("иван")]
    [InlineData(" иван")]
    [InlineData("Иван123")]
    [InlineData("")]
    public void Expert_InvalidFirstName_ShouldThrowArgumentException(string invalidName)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            var expert = new Expert
            {
                FirstName = invalidName,
                LastName = "Иванов"
            };
        });

        Assert.Contains("Некорректный формат имени", exception.Message);
    }
}

public class EvaluationListTests
{
    private readonly EvaluationList.Domain.EvaluationList _sheet;
    private readonly Criterion _testCriterion;

    public EvaluationListTests()
    {
        _sheet = new EvaluationList.Domain.EvaluationList
        {
            ExhibitionTitle = "Эврика",
            AgeGroup = "Мастер",
            Id = Guid.CreateVersion7()
        };

        _testCriterion = new Criterion
        {
            CriterionName = "Техническая сложность",
            MaxScore = 5.0,
            Id = Guid.CreateVersion7()
        };

        _sheet.AddCriterion(_testCriterion);
    }

    [Fact]
    public void AddAssessment_ValidScore_ShouldAddSuccessfully()
    {
        var assessment = new Assessment
        {
            ProjectId = Guid.CreateVersion7(),
            CriterionId = _testCriterion.Id,
            ExpertId = Guid.CreateVersion7(),
            Value = 4.5,
            Id = Guid.CreateVersion7()
        };

        _sheet.AddAssessment(assessment);

        Assert.Multiple(
            () => Assert.Single(_sheet.Assessments),
            () => Assert.Equal(4.5, _sheet.Assessments.First().Value)
        );
    }

    [Fact]
    public void AddAssessment_ScoreExceedsMax_ShouldThrowArgumentException()
    {
        var assessment = new Assessment
        {
            ProjectId = Guid.CreateVersion7(),
            CriterionId = _testCriterion.Id,
            ExpertId = Guid.CreateVersion7(),
            Value = 6.0,
            Id = Guid.CreateVersion7()
        };

        var exception = Assert.Throws<ArgumentException>(() =>
            _sheet.AddAssessment(assessment));

        Assert.Contains("превышает максимум", exception.Message);
    }

    [Fact]
    public void AddAssessment_UnknownCriterion_ShouldThrowInvalidOperationException()
    {
        var assessment = new Assessment
        {
            ProjectId = Guid.CreateVersion7(),
            CriterionId = Guid.CreateVersion7(),
            ExpertId = Guid.CreateVersion7(),
            Value = 1.0,
            Id = Guid.CreateVersion7()
        };

        Assert.Throws<InvalidOperationException>(() =>
            _sheet.AddAssessment(assessment));
    }
}

public class EntityBoundariesTests
{
    [Fact]
    public void Assessment_NegativeValue_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Assessment
        {
            ProjectId = Guid.CreateVersion7(),
            CriterionId = Guid.CreateVersion7(),
            ExpertId = Guid.CreateVersion7(),
            Value = -1.5,
            Id = Guid.CreateVersion7()
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Criterion_ZeroOrNegativeMaxScore_ShouldThrowArgumentException(double invalidScore)
    {
        Assert.Throws<ArgumentException>(() => new Criterion
        {
            CriterionName = "Тест",
            MaxScore = invalidScore,
            Id = Guid.CreateVersion7()
        });
    }
}