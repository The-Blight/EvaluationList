using System;
using System.IO;
using System.Linq;
using EvaluationList.Domain.Interfaces;
using EvaluationList.Infrastructure.Repositories;
using Xunit;

namespace EvaluationList.Infrastructure.Test;

public class TestEntity : IEntity
{
    public required Guid Id { get; init; }
    public required string Data { get; init; }
}

public class TestDto : IDto
{
    public Guid Id { get; init; }
    public required string Data { get; init; }
}

public class TestMapper : IMapper<TestEntity, TestDto>
{
    public TestDto ToDto(TestEntity entity) => new() { Id = entity.Id, Data = entity.Data };

    public TestEntity ToDomain(TestDto dto) => new() { Id = dto.Id, Data = dto.Data };
}

public class JsonRepositoryTests : IDisposable
{
    private readonly string _testFileName;
    private readonly string _fullFilePath;
    private readonly JsonRepository<TestEntity, TestDto> _repository;


    public JsonRepositoryTests()
    {
        _testFileName = $"test.repo_{Guid.CreateVersion7()}.json";
        _fullFilePath = Path.Combine(Path.GetTempPath(), _testFileName);
        var mapper = new TestMapper();
        
        _repository = new JsonRepository<TestEntity, TestDto>(_fullFilePath, mapper);
    }

    public void Dispose()
    {
        if (File.Exists(_fullFilePath))
        {
            File.Delete(_fullFilePath);
        }
    }


    [Fact]
    public void Create_ShouldCreateFile_And_SaveEntity()
    {
        var entity = new TestEntity { Id = Guid.CreateVersion7(), Data = "Test data" };
        _repository.Create(entity);
        var allEntities = _repository.GetAll().ToList();

        Assert.Multiple(
            () => Assert.True(File.Exists(_fullFilePath), "Файл должен был создаться на диске."),
            () => Assert.Single(allEntities),
            () => Assert.Equal(entity.Id, allEntities[0].Id),
            () => Assert.Equal(entity.Data, allEntities[0].Data)
        );
    }


    [Fact]
    public void GetById_ShouldReturnCorrectEntity()
    {
        var idToFind = Guid.CreateVersion7();
        _repository.Create(new TestEntity { Id = Guid.NewGuid(), Data = "First" });
        _repository.Create(new TestEntity { Id = idToFind, Data = "Target" });
        _repository.Create(new TestEntity { Id = Guid.NewGuid(), Data = "Last" });

        var result = _repository.GetById(idToFind);

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(idToFind, result.Id),
            () => Assert.Equal("Target", result.Data)
        );
    }

    [Fact]
    public void Update_ShouldModifyExistingDataInFile()
    {
        var entityId = Guid.CreateVersion7();
        _repository.Create(new TestEntity { Id = entityId, Data = "Old Data" });
        var updatedEntity = new TestEntity { Id = entityId, Data = "New Data" };

        _repository.Update(updatedEntity);
        var result = _repository.GetById(entityId);

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal("New Data", result.Data)
        );
    }


    [Fact]
    public void Delete_ShouldRemoveEntityFromFile()
    {
        var entityId = Guid.NewGuid();
        _repository.Create(new TestEntity { Id = entityId, Data = "To Be Deleted" });

        Assert.Single(_repository.GetAll());

        _repository.Delete(entityId);

        Assert.Multiple(
            () => Assert.Empty(_repository.GetAll()),
            () => Assert.Null(_repository.GetById(entityId))
        );
    }


    [Fact]
    public void GetAll_WhenFileDoesNotExist_ShouldReturnEmptyList()
    {
        var result = _repository.GetAll();

        Assert.Multiple(
            () => Assert.Empty(result),
            () => Assert.False(File.Exists(_fullFilePath))
        );
    }
}