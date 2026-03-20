using System;
using EvaluationList.Domain.Entities.Staff;
using Xunit;

namespace Evaluation.Tests;

public class ExpertTests
{
    [Theory]
    [InlineData("Иван", "Иванов")]
    [InlineData("Анна-Мария", "Смирнова")]
    [InlineData("John", "Doe")]
    public void Expert_ValidNames_ShouldCreateSuccessfully(string firstName, string lastName)
    {
        var expert = new Expert
        {
            FirstName = firstName,
            LastName = lastName
        };

        Assert.Multiple(
            () => Assert.Equal(firstName, expert.FirstName),
            () => Assert.Equal(lastName, expert.LastName)
        );
    }
    
    
    [Theory]
    [InlineData("иван")]    
    [InlineData("Иван123")] 
    [InlineData("")]        
    public void Expert_InvalidFirstName_ShouldThrowArgumentException(string invalidName)
    {

        var exception = Assert.Throws<ArgumentException>(() => 
        {
            var expert = new Expert 
            { 
                FirstName = invalidName, 
                LastName = "Тестов" 
            };
        });

        Assert.Contains("Некорректный формат имени", exception.Message);
    }
}
