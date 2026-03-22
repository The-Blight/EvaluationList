using System;
using System.Linq;
using Xunit;
using EvaluationList.Domain;
using EvaluationList.Domain.Entities.Scoring;

namespace Evaluation.Tests;

public class EvaluationLIstTests
{
    private readonly EvaluationList.Domain.EvaluationList _sheet;
    private readonly Criterion _testCriterion;
    private readonly Guid _testExpertId = Guid.CreateVersion7();
    private readonly Guid _testProjectId = Guid.CreateVersion7();
    
    public EvaluationLIstTests()
    {
        _sheet = new EvaluationList.Domain.EvaluationList
        {
            ExhibitionTitle = "Выставка 2026",
            AgeGroup = "Юниоры",
            Id = Guid.CreateVersion7()
        };

        _testCriterion = new Criterion
        {
            CriterionName = "Техническая сложность",
            MaxScore = 5.0,
            Id = Guid.CreateVersion7()
        };

        _sheet.AddCriterion(_testCriterion);
        _sheet.AddExpert(_testExpertId);
        _sheet.AddProject(_testProjectId);
    }
    
    
   [Fact]
    public void AddAssessment_ValidScore_ShouldAddSuccessfully()
    {
       
        var assessment = new Assessment
        {
            ProjectId = _testProjectId,
            CriterionId = _testCriterion.Id,
            ExpertId = _testExpertId,
            Value = 4.5,
            Id = Guid.CreateVersion7() 
        };
        
        _sheet.AddAssessment(assessment);

        Assert.Single(_sheet.Assessments);
        Assert.Equal(4.5, _sheet.Assessments.First().Value);
    }
    
    [Fact]
    public void AddAssessment_ScoreExceedsMax_ShouldThrowArgumentException()
    {
        
        var assessment = new Assessment
        {
            ProjectId = _testProjectId,
            CriterionId = _testCriterion.Id,
            ExpertId = _testExpertId,
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
            ProjectId = _testProjectId,
            CriterionId = Guid.NewGuid(),
            ExpertId = _testExpertId,
            Value = 3.0,
            Id = Guid.CreateVersion7()
        };

        Assert.Throws<InvalidOperationException>(() => 
            _sheet.AddAssessment(assessment));
    }

    [Fact]
    public void AddAssessment_ExpertNotAttached_ShouldThrowInvalidOperationException()
    {
        var assessment = new Assessment
        {
            ProjectId = _testProjectId,
            CriterionId = _testCriterion.Id,
            ExpertId = Guid.NewGuid(),
            Value = 3.0,
            Id = Guid.CreateVersion7()
        };

        var ex = Assert.Throws<InvalidOperationException>(() => 
            _sheet.AddAssessment(assessment));
        Assert.Equal("Эксперт не прикреплен к этому листу.", ex.Message);
    }

    [Fact]
    public void AddAssessment_ProjectNotAttached_ShouldThrowInvalidOperationException()
    {
        var assessment = new Assessment
        {
            ProjectId = Guid.NewGuid(),
            CriterionId = _testCriterion.Id,
            ExpertId = _testExpertId,
            Value = 3.0,
            Id = Guid.CreateVersion7()
        };

        var ex = Assert.Throws<InvalidOperationException>(() => 
            _sheet.AddAssessment(assessment));
        Assert.Equal("Проект не прикреплен к этому листу.", ex.Message);
    }

    [Fact]
    public void AddAssessment_DuplicateAssessment_ShouldThrowInvalidOperationException()
    {
        var assessment1 = new Assessment
        {
            ProjectId = _testProjectId,
            CriterionId = _testCriterion.Id,
            ExpertId = _testExpertId,
            Value = 3.0,
            Id = Guid.CreateVersion7()
        };
        
        var assessment2 = new Assessment
        {
            ProjectId = _testProjectId,
            CriterionId = _testCriterion.Id,
            ExpertId = _testExpertId,
            Value = 4.0,
            Id = Guid.CreateVersion7()
        };

        _sheet.AddAssessment(assessment1);

        var ex = Assert.Throws<InvalidOperationException>(() => 
            _sheet.AddAssessment(assessment2));
        Assert.Equal("Оценка по данному критерию от этого эксперта для этого проекта уже выставлена.", ex.Message);
    }
}