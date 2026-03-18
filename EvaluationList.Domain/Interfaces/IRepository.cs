using System;
using System.Collections.Generic;

namespace EvaluationList.Domain.Interfaces;

/// <summary>
/// Базовый интерфейс для всех репозиториев.
/// </summary>
/// <typeparam name="T">Тип сущности.</typeparam>

public interface IRepository<T> where T : IEntity
{
    void Create(T entity);
    T? GetById(Guid id);
    IEnumerable<T> GetAll();
    void Update(T entity);
    void Delete(Guid id);
}